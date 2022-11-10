using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. 카메라로부터 위치 오브셋 피봇 오프셋을 설정
/// 
/// 2. 충동 체크 : 이중 체크 (앞, 리버스)
/// 캐릭터로 부터 카메라 사이
/// 카메라로 부터 캐릭터 사이
/// 
/// 3. Recoil ( 반동 )
/// 
/// 4. FOV
/// </summary>

[RequireComponent(typeof(Camera))]
public class ObitCamera : MonoBehaviour
{
    public Transform charactorPlayer;

    public Vector3 pivotOffset = new Vector3(0f, 1f, 0f);
    public Vector3 camOffset = new Vector3(0.4f, 0.5f, -2.0f);

    public float smooth = 10;
    public float aimingMouseSpeedH = 6.0f;
    public float aimingMouseSpeedV = 6.0f;
    public float angleMaxV = 30.0f;
    public float angleMinV = -60.0f;

    public float angleBounceRecoil = 5.0f;

    private float angleHorizontal = 0.0f;
    private float angleVertical = 0.0f;

    private Transform transformCamera;

    private Camera fovCamera;

    //플레이어로 부터 카메라까지 벡터
    private Vector3 posRealCamera;

    //카메라와 플레이어 사이 거리
    private float posDistanceRealCamera;

    private Vector3 lerpPivotOffset;
    private Vector3 lerpCamOffset;
    private Vector3 targetPivotOffset;
    private Vector3 targetCamOffset;

    private float lerpDefaultFOV;
    private float lerpTargetFOV;

    private float maxVerticalAngleTarget;
    private float angleRecoil = 0f;

    public float getHorizontal
    {
        get
        {
            return angleHorizontal;
        }
    }

    private void Awake()
    {
        transformCamera = transform;
        fovCamera = transformCamera.GetComponent<Camera>();

        transformCamera.position = charactorPlayer.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        transformCamera.rotation = Quaternion.identity;

        posRealCamera = transformCamera.position - charactorPlayer.position;
        posDistanceRealCamera = posRealCamera.magnitude - 0.5f;

        lerpPivotOffset = pivotOffset;
        lerpCamOffset = camOffset;

        lerpDefaultFOV = fovCamera.fieldOfView;
        angleHorizontal = charactorPlayer.eulerAngles.y;

        //리셋 3종
        //aim
        //fov
        //angle
        resetAimOffset();
        resetFOV();
        resetMaxVAngle();
    }

    public void resetAimOffset()
    {
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
    }

    public void resetFOV()
    {
        this.lerpTargetFOV = lerpDefaultFOV;
    }

    public void resetMaxVAngle()
    {
        maxVerticalAngleTarget = angleMaxV;
    }

    public void recoilBounceAngleV(float val)
    {
        angleRecoil = val;
    }

    public void setPosTargetOffset(Vector3 newPivotOffset, Vector3 newCamOffset)
    {
        targetPivotOffset = newPivotOffset;
        targetCamOffset = newCamOffset;
    }

    public void setFOV(float _val)
    {
        this.lerpTargetFOV = _val;
    }

    bool ckViewingPos(Vector3 ckPos, float playerHeight)
    {
        Vector3 target = charactorPlayer.position + (Vector3.up * playerHeight);

        if (Physics.SphereCast(ckPos, 0.2f, target - ckPos, out RaycastHit hit, posDistanceRealCamera))
        {
            if (hit.transform != charactorPlayer && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    bool ckViewingPosR(Vector3 ckPos, float playerHeight, float maxDistance)
    {
        Vector3 origin = charactorPlayer.position + (Vector3.up * playerHeight);

        if (Physics.SphereCast(origin, 0.2f, ckPos - origin, out RaycastHit hit, maxDistance))
        {
            if (hit.transform != charactorPlayer && hit.transform != transformCamera && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    bool ckDoubleViewingPos(Vector3 ckPos, float offset)
    {
        float playerFoucusHeight = charactorPlayer.GetComponent<CapsuleCollider>().height * 0.75f;
        return ckViewingPos(ckPos, playerFoucusHeight) && ckViewingPosR(ckPos, playerFoucusHeight, offset);
    }

    private void Update()
    {
        angleHorizontal += Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f) * aimingMouseSpeedH;
        angleVertical += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1f, 1f) * aimingMouseSpeedV;

        angleVertical = Mathf.Clamp(angleVertical, angleMinV, maxVerticalAngleTarget);

        angleVertical = Mathf.LerpAngle(angleVertical, angleVertical + angleRecoil, 10 * Time.deltaTime);

        Quaternion camRotationY = Quaternion.Euler(0f, angleHorizontal, 0f);
        Quaternion aimRotation = Quaternion.Euler(-angleVertical, angleHorizontal, 0f);
        transformCamera.rotation = aimRotation;

        fovCamera.fieldOfView = Mathf.Lerp(fovCamera.fieldOfView, lerpTargetFOV, Time.deltaTime);
        Vector3 posBaseTemp = charactorPlayer.position + camRotationY * targetPivotOffset;
        Vector3 noCollisionOffset = targetCamOffset;

        for (float offsetZ = targetCamOffset.z; offsetZ <= 0f; offsetZ += 0.5f)
        {
            noCollisionOffset.z = offsetZ;

            if (ckDoubleViewingPos(posBaseTemp + aimRotation * noCollisionOffset, Mathf.Abs(offsetZ)) || offsetZ == 0f)
            {
                break;
            }
        }

        lerpCamOffset = Vector3.Lerp(lerpCamOffset, noCollisionOffset, smooth * Time.deltaTime);
        lerpPivotOffset = Vector3.Lerp(lerpPivotOffset, targetPivotOffset, smooth * Time.deltaTime);

        transformCamera.position = charactorPlayer.position + camRotationY * lerpPivotOffset + aimRotation * lerpCamOffset;

        if (angleRecoil > 0.0f)
        {
            angleRecoil -= angleBounceRecoil * Time.deltaTime;
        }
        else if (angleRecoil < 0.0f)
        {
            angleRecoil += angleBounceRecoil * Time.deltaTime;
        }
    }

    public float getCurrentPivotMagnitude(Vector3 finalPivotOffset)
    {
        return (Mathf.Abs((finalPivotOffset - lerpPivotOffset).magnitude));
    }
}
