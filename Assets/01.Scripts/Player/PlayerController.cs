using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

enum AtkMode
{
    Shovel, Melee, Gun
}

public class PlayerController : MonoBehaviour
{

    [Header("Player Setting")]
    public float turnSpeed = 10f;
    public float walkSpeed = 3f;
    private float spd = 3f;
    public float runSpeed = 5f;
    public bool stopMovement = false;

    public bool moving { get; set; }

    float m_Horizontal, m_Vertical;
    private Vector3 m_MoveVector;
    private Rigidbody m_Rigidbody;

    [HideInInspector]
    public Animator m_Animator;
    private Quaternion m_Rotation = Quaternion.identity;
    private Transform camTrans;
    private Vector3 camForward;

    private Vector3 offset;
    private float inputSpeed = 0f;

    private bool isSnow = false;
    private GameObject snowObj = null;
    private bool isEnemyClose = false;

    private AtkMode atkMode = AtkMode.Melee;
    private bool isAtk = false;

    [SerializeField]
    private GameObject[] tools;

    [SerializeField]
    private Collider shovelCollider;

    [SerializeField]
    private GameObject snowBall;

    [SerializeField]
    private Transform shootPos;

    private int Hp { get => GameManager.Instance.playerHp; }

    [SerializeField]
    private Transform respawnPos;

    [SerializeField]
    private Material[] playerMaterial;

    private Renderer[] renderers;

    [SerializeField]
    private VisualEffect atkFx;

    [SerializeField]
    private float range;

    private void OnEnable()
    {
        stopMovement = false;
        isAtk = false;
        isEnemyClose = false;
        isSnow = false;
        snowObj = null;
    }

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        camTrans = Camera.main.transform;
        playerMaterial[0].color = Color.white;
        playerMaterial[1].color = Color.white;
        renderers = GetComponentsInChildren<Renderer>();
    }


    void FixedUpdate()
    {
            //input 
            m_Horizontal = Input.GetAxis(Const.Horizontal);
            m_Vertical = Input.GetAxis(Const.Vetical);

            // move vector 
            if (camTrans != null)
            {
                camForward = Vector3.Scale(camTrans.forward, new Vector3(1, 0, 1).normalized);
                m_MoveVector = m_Vertical * camForward + m_Horizontal * camTrans.right;
                m_MoveVector.Normalize();
            }
            //animation    


            bool has_H_Input = !Mathf.Approximately(m_Horizontal, 0);
            bool has_V_Input = !Mathf.Approximately(m_Vertical, 0);

            if (!stopMovement) moving = has_H_Input || has_V_Input;
            else moving = false;

            m_Animator.SetBool(Const.Moving, moving);
            m_Animator.SetFloat(Const.Speed, inputSpeed);

            //move and rotate
            if (moving)
            {
                Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_MoveVector, turnSpeed * Time.deltaTime, 0f);
                m_Rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredForward), turnSpeed);
                m_Rigidbody.MoveRotation(m_Rotation);
                m_Rigidbody.MovePosition(m_Rigidbody.position + inputSpeed * m_MoveVector * spd * Time.deltaTime);

            }
    }

    private void Update()
    {
        if (!GameManager.Instance.Enabled)
        {
            InputMove();
            InputAtk();
            ChangeTool();
        }
    }


    private void InputMove()
    {
        if (Input.GetKey(KeyCode.LeftShift) && moving)
        {
            spd = runSpeed;
            inputSpeed = Mathf.Lerp(inputSpeed, 1, Time.deltaTime);
        }
        else
        {
            spd = walkSpeed;
            inputSpeed = (moving) ? 0.5f * Mathf.Clamp01(Mathf.Abs(m_Horizontal) + Mathf.Abs(m_Vertical)) : Mathf.Lerp(inputSpeed, 0, Time.deltaTime);
        }

        float dist = Vector3.Distance(transform.position, Vector3.zero); // the distance from player current position to the circleCenter

        if (dist > range)
        {
            Vector3 fromOrigintoObject = transform.position - Vector3.zero;
            fromOrigintoObject *= range / dist;
            transform.position = Vector3.zero + fromOrigintoObject;
        }
    }

    private void InputAtk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAtk) Atk();
        }
    }

    private void ChangeTool()
    {
        if (!isAtk)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (wheelInput > 0)
            {
                atkMode = isEnemyClose ? AtkMode.Melee : AtkMode.Shovel;
                tools[1].SetActive(false);
                tools[0].SetActive(true);
            }
            else if (wheelInput < 0)
            {
                atkMode = AtkMode.Gun;

                tools[0].SetActive(false);
                tools[1].SetActive(true);
            }
        }
    }

    private void Atk()
    {
        isAtk = true;
        stopMovement = true;
        switch (atkMode)
        {
            case AtkMode.Shovel:
                m_Animator.CrossFade(Const.Dig, 0.05f);
                break;

            case AtkMode.Melee:
                m_Animator.SetFloat(Const.AtkSpeed, GameManager.Instance.playerAtkSpd);
                m_Animator.CrossFade(Const.Melee, 0.05f);
                break;

            case AtkMode.Gun:
                if (GameManager.Instance.getSnow > 0)
                {
                    m_Animator.CrossFade(Const.Shoot, 0.5f);
                    GameManager.Instance.UseSnow();
                }
                else
                {
                    isAtk = false;
                    stopMovement = false;
                    GameManager.Instance.OutOfSnow();
                }
                break;
        }
    }

    /// <summary>
    /// After Dig
    /// </summary>
    public void afterDig()
    {
        snowObj.GetComponent<SnowPile>().whenDestroy();
        GameManager.Instance.AddSnow(Mathf.RoundToInt(snowObj.GetComponent<MeshRenderer>().material.GetFloat("_Height")));
        snowObj = null;
        isSnow = false;
    }
    public void ableMove()
    {
        isAtk = false;
        stopMovement = false;
    }

    public void BladeOn()
    {
        shovelCollider.enabled = true;
        atkFx.Play();
    }

    public void BladeOff()
    {
        shovelCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Snow"))
        {
            isSnow = true;
            snowObj = collision.gameObject;

            if (atkMode != AtkMode.Gun)
            {
                atkMode = AtkMode.Shovel;
            }
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            isSnow = false;
            isEnemyClose = true;
            if (atkMode != AtkMode.Gun)
            {
                atkMode = AtkMode.Melee;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Snow"))
        {
            isSnow = false;
            snowObj = null;
            if (atkMode != AtkMode.Gun)
            {
                atkMode = AtkMode.Melee;
            }
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (snowObj != null)
            {
                isSnow = true;
                if (atkMode != AtkMode.Gun) atkMode = AtkMode.Shovel;
            }
            isEnemyClose = false;
        }
    }

    public void Shoot()
    {
        Instantiate(snowBall, shootPos);
    }

    public void GetDmg()
    {
        GameManager.Instance.playerHp--;
        GameManager.Instance.Hert.SetActive(true);
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        playerMaterial[0].color = Color.red;
        playerMaterial[1].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        playerMaterial[0].color = Color.white;
        playerMaterial[1].color = Color.white;
        GameManager.Instance.Hert.SetActive(false);

        if (Hp < 0)
        {
            transform.position = respawnPos.position;
            BladeOff();
            GameManager.Instance.RespawnPlayer();
            gameObject.SetActive(false);
        }
    }

}
