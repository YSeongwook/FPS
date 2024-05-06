using UnityEngine;
using Mirror;

public class Status : NetworkBehaviour, IDamaged
{
    [SerializeField] protected float maxHp = 100f;
    [SyncVar(hook = nameof(OnHpChange))]
    public float currentHp;

    private string lastHitBodyPart;

    public Animator animator;
    public CharacterController chracterController;
    public PlayerController playerController;

    //public float CurrentHp { get { return currentHp; } set { currentHp = value; } }

    private void Awake()
    {

    }

    void Start()
    {
        // 체력 초기화
        currentHp = maxHp;
    }

    public void OnHpChange(float oldHp, float newHp)
    {
        Debug.Log($"HP changed from {oldHp} to {newHp}");
        // 로그를 추가하여 클라이언트에서 실행되는지 확인
        Debug.Log("HP Change detected on " + (isServer ? "Server" : "Client"));
        if (newHp <= 0 && oldHp > 0) // 체력이 0 이하로 떨어진 경우
        {
            CheckDeathBodyPart(lastHitBodyPart); // 마지막으로 피격된 부위에 따른 사망 애니메이션
            playerController.enabled = false;
            chracterController.enabled = false;
            // 기타 사망 관련 로직 수행
        }
    }


    [Server]
    public void TakeDamage(float damage, string hitBodyPart)
    {
        if (currentHp > 0)
        {
            currentHp -= damage;

            if (currentHp <= 0)
            {
                currentHp = 0;
                RpcPlayDeathAnimation(hitBodyPart); // 모든 클라이언트에 사망 애니메이션을 실행하도록 요청
                playerController.enabled = false;
                chracterController.enabled = false;
            }
        }
    }

    public void DamagedHead()
    {
        // 즉사
        RequestDamage(maxHp, "Head");
    }

    public void DamagedThorax()
    {
        RequestDamage(50, "Thorax");
        // 뛰지 못하게
    }

    public void DamagedArm()
    {
        RequestDamage(20, "Arm");
        // 공격 속도 절반
    }

    public void DamagedLeg()
    {
        RequestDamage(20, "Leg");
        // 이동 속도 절반
    }

    [ClientRpc]
    public void RpcPlayDeathAnimation(string hitBodyPart)
    {
        CheckDeathBodyPart(hitBodyPart);
    }

    // 어느 부위에 맞아 사망했는지 판별
    public void CheckDeathBodyPart(string hitBodyPart)
    {
        switch(hitBodyPart)
        {
            case "Head":
                animator.SetTrigger("Death_Top");
                break;
            case "Thorax":
                animator.SetTrigger("Death_Middle");
                break;
            case "Arm":
                animator.SetTrigger("Death_Middle");
                break;
            case "Leg":
                animator.SetTrigger("Death_Bottom");
                break;
            case "Zone":
                animator.SetTrigger("Death_Bottom");
                break;
            default:
                Debug.LogWarning("Unknown body part hit.");
                break;
        }
    }

    public void RequestDamage(float damage, string hitBodyPart)
    {
        if (isServer)
        {
            TakeDamage(damage, hitBodyPart);
        }
        else
        {
            CmdRequestDamage(damage, hitBodyPart);
        }
    }

    [Command]
    public void CmdRequestDamage(float damage, string hitBodyPart)
    {
        TakeDamage(damage, hitBodyPart);
    }
}