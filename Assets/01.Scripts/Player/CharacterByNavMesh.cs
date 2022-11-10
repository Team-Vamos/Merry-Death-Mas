using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterByNavMesh : MonoBehaviour, IDmgAble
{

    public int maxHp = 100;

    public int Hp
    {
        get;
        private set;
    }
    public bool getFlagLive => Hp > 0;

    [SerializeField]
    private UIHP_NPC uiHP_NPC;

    public Transform weaponHitTransform;

    private CharacterController characterController;
    private NavMeshAgent agent;

    private Vector3 calcVelocity = Vector3.zero;

    public LayerMask layerGround;
    private bool flagOnGrounded = true;
    private float defaultGroundDistance = 0.2f;

    [SerializeField]
    private Animator animator;
    readonly int moveHash = Animator.StringToHash("Move");


    void Start()
    {
        characterController = GetComponent<CharacterController>();

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool(moveHash, true);

        Hp = maxHp;

        if (uiHP_NPC)
        {
            uiHP_NPC.MinHp = 0;
            uiHP_NPC.MaxHp = maxHp;
            uiHP_NPC.Value = Hp;
        }
    }

    void Update()
    {
        flagOnGrounded = characterController.isGrounded;

        if (flagOnGrounded && calcVelocity.y < 0)
        {
            calcVelocity.y = 0.0f;
        }

        Transform cameraTransform = Camera.main.transform;

        //메인 카메라가 바라보는 방향이 월드상에서 어떤 방향인가
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //벡터 내적
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //방향 벡터이자 목표점
        Vector3 targetDirection = vertical * forward + horizontal * right;

        agent.SetDestination(targetDirection);

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            characterController.Move(agent.velocity * Time.deltaTime);
            animator.SetBool(moveHash, true);
        }
        else
        {
            characterController.Move(Vector3.zero);
            animator.SetBool(moveHash, false);
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }

    public void setDmg(int dmg, GameObject atkEffectPrefab)
    {
        if (!getFlagLive) { return; }

        Hp -= dmg;

        if (uiHP_NPC) { uiHP_NPC.Value = Hp; }

        if (atkEffectPrefab)
        {
            Instantiate(atkEffectPrefab, weaponHitTransform);
        }

        if (getFlagLive)
        {
            animator?.SetTrigger("hitTriggerHash");
        }
        else
        {
            if (uiHP_NPC != null) uiHP_NPC.enabled = false;
        }
    }
}
