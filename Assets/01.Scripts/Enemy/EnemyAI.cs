using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.PATROL;

    [Header("Transform")]
    private Transform playerTr;
    private Transform enemyTr;

    private Animator animator;
    private MoveAgent moveAgent;
    private EnemyFire enemyFire;    

    public float attackDist = 8.0f; // 공격 거리
    public float traceDis = 15.0f;  // 쫓아가는 거리
    public float staticTraceDis = 15.0f;    // 고정 거리
    public bool isDie = false;

    private WaitForSeconds ws; // 코루틴 지연시간 변수

    private SkinnedMeshRenderer skinnedMeshRenderer;
    public Material changeMaterial;
    public float changeMaterialTime = 1f;

    Collider[] childColliders;


    private float playerHp;

    // 애니메이터 컨트롤러에 정의한 파라미터의 해시 값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("IsDie");
    private readonly int hashOffeset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");

    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //if (player != null) playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();
        enemyFire = GetComponent<EnemyFire>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        ws = new WaitForSeconds(0.3f);

        // 애니메이션의 시작 프레임과 속도가 다르기 때문에 걸음걸이가 조금씩 다르게
        animator.SetFloat(hashOffeset, Random.Range(0.0f, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.2f));

        childColliders = GetComponentsInChildren<Collider>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            var player = FindObjectOfType<PlayerController>();
            //Debug.Log(player);
            if (player != null) playerTr = player.GetComponent<Transform>();        
            //Debug.Log(playerTr.position);
            if (state == State.DIE) yield break;

            playerHp = player.GetComponentInParent<Status>().currentHp;
            if (playerHp <= 0)
            {
               state =  State.PATROL;
               yield break ;
            }
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist) state = State.ATTACK;
            else if (dist <= traceDis) state = State.TRACE;
            else state = State.PATROL;

            yield return ws;
        }
    }

    IEnumerator Action()
    {
        // 적 캐릭터 사망할 때까지 무한루프
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    // 총알 발사 정지
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    traceDis = staticTraceDis;
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);

                    if (enemyFire.isFire == false) enemyFire.isFire = true;                 
                    break;
                case State.DIE:
                    isDie = true;
                    enemyFire.isFire = false;
                    moveAgent.Stop();

                    // 모든 자식 오브젝트의 콜라이더 비활성화
                    foreach (Collider collider in childColliders)
                    {
                        collider.enabled = false;
                    }
                    break;
            }
        }
    }
    

    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

}
