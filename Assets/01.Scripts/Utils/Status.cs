using System.Collections;
using UnityEngine;

public class Status : MonoBehaviour, IDamaged
{
    [SerializeField] protected float maxHp = 100f;
    [SerializeField] protected float currentHp;
    public Animator animator;
    public CharacterController chracterController;
    public PlayerController playerController;
    public AudioListener audioListener;
    public GameObject tpsPos;

    public float CurrentHp { get { return currentHp; } set { currentHp = value; } }

    private bool isAlive = true;
    private bool isBleeding = false;
    private float bleedingInterval = 6f; // 출혈 간격
    private float bleedingTimer = 0f;    // 출혈 타이머

    void Start()
    {
        // 체력 초기화
        currentHp = maxHp;
    }

    public void TakeDamge(float damage, string hitBodyPart)
    {
        currentHp -= damage;
        Debug.Log($"Player HP: {currentHp}");

        if(currentHp <= 0 && isAlive)
        {
            isAlive = false;
            currentHp = 0;
            playerController.enabled = false;   // 플레이어 컨트롤러 스크립트 비활성화
            chracterController.enabled = false; // 캐릭터 컨트롤러 비활성화
            CheckDeathBodyPart(hitBodyPart);
            // 카메라 포지션 수정, 보류
            // 서버에 사망 알리기
            // 오디오 리스너 비활성화
            audioListener.enabled = false;
            // UI 전환
        }
        else if (Random.Range(0f, 1f) < 0.3f) // 30% 확률로 출혈 발생
        {
            Bleeding();
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
        playerController.canSprint = false; // 뛰지 못하게
    }

    public void DamagedArm()
    {
        TakeDamge(20, "Arm");
        playerController.DelayCount *= 2f;  // 공격 속도 절반
    }

    public void DamagedLeg()
    {
        TakeDamge(20, "Leg");
        playerController.MoveSpeed /= 2f;   // 이동 속도 절반
    }

    // 출혈 효과
    public void Bleeding()
    {
        if (!isBleeding)
        {
            isBleeding = true;
            StartCoroutine(BleedingCoroutine());
        }
    }

    IEnumerator BleedingCoroutine()
    {
        while (currentHp > 0 && isBleeding)
        {
            yield return new WaitForSeconds(bleedingInterval);
            currentHp -= 3;

            // 체력이 0 이하로 떨어진 경우
            if (currentHp <= 0 && isAlive)
            {
                isAlive = false;
                currentHp = 0;
                playerController.enabled = false;   // 플레이어 컨트롤러 스크립트 비활성화
                chracterController.enabled = false; // 캐릭터 컨트롤러 비활성화
                CheckDeathBodyPart("Thorax"); // 사망 처리
                // 카메라 포지션 수정, 보류
                // 서버에 사망 알리기
                // 오디오 리스너 비활성화
                audioListener.enabled = false;
                // UI 전환
            }
        }

        isBleeding = false;
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