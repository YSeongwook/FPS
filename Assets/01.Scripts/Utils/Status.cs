using UnityEngine;
using Mirror;

public class Status : NetworkBehaviour, IDamaged
{
    [SerializeField] protected float maxHp = 100f;
    [SyncVar]
    public float currentHp;
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

    public void TakeDamge(float damage, string hitBodyPart)
    {
        currentHp -= damage;
        Debug.Log($"playerHP: {currentHp}");

        if(currentHp <= 0)
        {
            currentHp = 0;
            CheckDeathBodyPart(hitBodyPart);
            playerController.enabled = false;   // 플레이어 컨트롤러 스크립트 비활성화
            chracterController.enabled = false; // 캐릭터 컨트롤러 비활성화
            // 카메라 포지션 수정
            // 서버에 사망 알리기
            // 오디오 리스너 비활성화
            // UI 전환
        }

    }

    public void DamagedHead()
    {
        // 즉사
        TakeDamge(maxHp, "Head");
    }

    public void DamagedThorax()
    {
        TakeDamge(50, "Thorax");
        // 뛰지 못하게
    }

    public void DamagedArm()
    {
        TakeDamge(20, "Arm");
        // 공격 속도 절반
    }

    public void DamagedLeg()
    {
        TakeDamge(20, "Leg");
        // 이동 속도 절반
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
            default:
                Debug.LogWarning("Unknown body part hit.");
                break;
        }
    }
}