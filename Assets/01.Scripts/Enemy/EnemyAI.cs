using Mirror.Examples.AdditiveLevels;
using System.Collections;
using System.Collections.Generic;
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

    private Transform playerTr;
    private Transform enemyTr;

    private Animator animator;

    public float attackDist = 5.0f; // 공격거리
    public float traceDis = 10.0f;  // 쫓아가는거리

    public bool isDie = false;

    private WaitForSeconds ws;// 코루틴 지연시간 변수

    private MoveAgent moveAgent;

    private EnemyFire enemyFire;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    public Material changeMaterial;
    public float changeMaterialTime = 1f;


    // 애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("IsDie");
    private readonly int hashOffeset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");


    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) playerTr = player.GetComponent <Transform>();

        enemyTr = GetComponent<Transform>();
        moveAgent = GetComponent<MoveAgent>();  
        animator = GetComponent<Animator>();
        enemyFire = GetComponent<EnemyFire>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        ws = new WaitForSeconds(0.3f);

        // 애니메이션의 시작프레임과 속도가다르기 때문에 걸음걸이가 조금씩 다르게
        animator.SetFloat(hashOffeset, Random.Range(0.0f, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.2f));
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator Action()
    {
        // 적 캐릭터 사망할때까지 무한루프
        while(!isDie)
        {
            yield return ws;

            switch(state)
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
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);

                    if(enemyFire.isFire == false) enemyFire.isFire = true;
                    break;
                case State.DIE:
                    isDie = true;
                    enemyFire.isFire = false;
                    moveAgent.Stop();
                    GetComponent<CapsuleCollider>().enabled = false;
                    animator.SetTrigger(hashDie);                   
                    StartCoroutine("TransitionMaterialColor");
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }

        }
    }

    IEnumerator CheckState()
    {
        while(!isDie)
        {
            if (state == State.DIE) yield break;

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if(dist <= attackDist)      state = State.ATTACK;
            else if(dist <= traceDis)   state = State.TRACE;
            else                        state = State.PATROL;

            yield return ws;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

    IEnumerator TransitionMaterialColor()
    {
        Material currentMaterial = skinnedMeshRenderer.material;

        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < changeMaterialTime)
        {
            elapsedTime = Time.time - startTime;

            // 보간된 Material 계산
            Material newMaterial = BlendMaterials(currentMaterial, changeMaterial, elapsedTime / changeMaterialTime);

            // 마테리얼 설정
            skinnedMeshRenderer.material = newMaterial;

            yield return null;
        }

        // 변경이 완료된 후에 GameObject를 비활성화
        gameObject.SetActive(false);
    }

    Material BlendMaterials(Material materialA, Material materialB, float blendFactor)
    {
        // Material을 보간합니다.
        Material blendedMaterial = new Material(materialA);
        blendedMaterial.Lerp(materialA, materialB, blendFactor);
        return blendedMaterial;
    }
}
