using UnityEngine;
using EnumTypes;
using EventLibrary;

public class EnemyDamage : MonoBehaviour, IDamaged
{
    [SerializeField] protected float maxHp = 100f;
    [SerializeField] protected float currentHp;

    // 현재 hp 프로퍼티
    public float CurrentHP { get; private set; }

    private void Awake()
    {
        currentHp = maxHp;

        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitHead, DamagedHead);
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitThorax, DamagedThorax);
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitArm, DamagedArm);
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitLeg, DamagedLeg);
    }

    public void TakeDamge(float damage)
    {
        currentHp -= damage;

        Debug.Log(this.currentHp);

        // 조건 추가(현재 추적 중인 플레이어가 없다면..)
        GetComponent<EnemyAI>().traceDis = 100.0f;
        GetComponent<EnemyAI>().state = EnemyAI.State.TRACE;


        if (currentHp <= 0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
        }
    }

    public void DamagedHead()
    {
        // 즉사
        TakeDamge(maxHp);
    }

    public void DamagedThorax()
    {
        TakeDamge(50);
        // 뛰지 못하게
    }

    public void DamagedArm()
    {
        TakeDamge(20);
        // 공격 속도 절반
    }

    public void DamagedLeg()
    {
        TakeDamge(20);
        // 이동 속도 절반
    }
}
