using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyDamage : MonoBehaviour, IDamaged
{
    public Animator animator;
    public NavMeshAgent agent;

    [SerializeField] protected float maxHp = 100f;
    [SerializeField] protected float currentHp;

    private MoveAgent moveAgent;
    private EnemyFire enemyFire;
    private bool isAlive;

    // 현재 hp 프로퍼티
    public float CurrentHP { get; private set; }

    private bool isBleeding = false;
    private float bleedingInterval = 6f; // 출혈 간격
    private float bleedingTimer = 0f;    // 출혈 타이머

    private void Awake()
    {
        currentHp = maxHp;
        isAlive = true;
    }

    public void TakeDamge(float damage, string hitBodyPart)
    {
        currentHp -= damage;
        Debug.Log($"Enemy HP: {currentHp}");

        // 조건 추가(현재 추적 중인 플레이어가 없다면..)
        if(currentHp > 0f)
        {
            GetComponent<EnemyAI>().traceDis = 100.0f;
            GetComponent<EnemyAI>().state = EnemyAI.State.TRACE;
        } 
        else
        {
            currentHp = 0;
            isAlive = false;
            CheckDeathBodyPart(hitBodyPart);

            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
        }

        if (currentHp > 0f && Random.Range(0f, 1f) < 0.3f) // 30% 확률로 출혈 발생
        {
            Bleeding();
        }
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
                currentHp = 0;

                CheckDeathBodyPart("Thorax"); // 사망 처리
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;

                // 카메라 포지션 수정, 보류
                // 서버에 사망 알리기
                // UI 전환
            }
        }

        isBleeding = false;
    }

    public void DamagedHead()
    {
        // 즉사
        TakeDamge(maxHp, "Head");
    }

    public void DamagedThorax()
    {
        TakeDamge(50, "Thorax");
    }

    public void DamagedArm()
    {
        TakeDamge(20, "Arm");
        // 공격 속도 1초 딜레이 추가
        enemyFire.MinFireTime += 1f;
        enemyFire.MaxFireTime += 1f;
    }

    public void DamagedLeg()
    {
        TakeDamge(20, "Leg");
        moveAgent.MoveSpeed /= 2f;  // 이동 속도 절반
    }

    // 어느 부위에 맞아 사망했는지 판별
    public void CheckDeathBodyPart(string hitBodyPart)
    {
        switch (hitBodyPart)
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
