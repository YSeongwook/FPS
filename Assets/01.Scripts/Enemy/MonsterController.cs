using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public enum State
    {
        IDLE, TRACE, ATTACK, DIE
    }

    public State state = State.IDLE;

    public float traceDistance = 10.0f;
    public float attackDistance = 2.0f;

    public bool isDie = false;

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;
    private StateMachine stateMachine;

    // 애니메이터 Hash값 캐싱, 속도 측면에서 이점이 있다.
    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");

    void Awake()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(State.IDLE, new IdleState(this));
        stateMachine.AddState(State.TRACE, new TraceState(this));
        stateMachine.AddState(State.ATTACK, new AttackState(this));
        stateMachine.InitState(State.IDLE);

        agent.destination = playerTr.position;
    }

    void Start()
    {
        StartCoroutine(CheckMonsterState());
    }

    private IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            if (state == State.DIE)
            {
                stateMachine.ChangeState(State.DIE);
                yield break;
            }

            float distance = Vector3.Distance(playerTr.position, monsterTr.position);
            if (distance <= attackDistance)
            {
                stateMachine.ChangeState(State.ATTACK);
            }
            else if(distance <= traceDistance)
            {
                stateMachine.ChangeState(State.TRACE);
            } 
            else
            {
                stateMachine.ChangeState(State.IDLE);
            }
        }
    }

    private class BaseMonsterState : BaseState
    {
        protected MonsterController owner;

        public BaseMonsterState(MonsterController owner)
        {
            this.owner = owner;
        }
    }

    private class IdleState : BaseMonsterState
    {
        public IdleState(MonsterController owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.anim.SetBool(owner.hashTrace, false);
        }
    }

    private class TraceState : BaseMonsterState
    {
        public TraceState(MonsterController owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.SetDestination(owner.playerTr.position);
            owner.agent.isStopped = false;
            owner.anim.SetBool(owner.hashTrace, true);
            owner.anim.SetBool(owner.hashAttack, false);
        }
    }

    private class AttackState : BaseMonsterState
    {
        public AttackState(MonsterController owner) : base(owner) { }

        public override void Enter()
        {
            owner.anim.SetBool(owner.hashAttack, true);
        }
    }
}
