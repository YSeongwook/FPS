using EventLibrary;
using UnityEngine;
using EnumTypes;

public class Status : MonoBehaviour, IDamaged
{
    [SerializeField] protected float maxHp = 100f;
    [SerializeField] protected float currentHp;

    public float CurrentHp { get { return currentHp; } set { currentHp = value; } }

    private void Awake()
    {
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitHead, DamagedHead);
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitThorax, DamagedThorax);
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitArm, DamagedArm);
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitLeg, DamagedLeg);
    }

    void Start()
    {
        // 체력 초기화
        currentHp = maxHp;
    }

    public void TakeDamge(float damage)
    {
        currentHp -= damage;

        Debug.Log(currentHp);
    }

    // 사망 처리 추가

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