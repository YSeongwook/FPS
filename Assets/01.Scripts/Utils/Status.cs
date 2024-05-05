using EventLibrary;
using UnityEngine;
using EnumTypes;

public class Status : MonoBehaviour
{
    [SerializeField] protected float maxHp = 100f;
    [SerializeField] protected float currentHp;

    public float CurrentHp { get { return currentHp; } set {  currentHp = value; } }

    private void Awake()
    {
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitHead, DamagedLeg);
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitThorax, DamagedLeg);
        EventManager<HitBodyPart>.StartListening(HitBodyPart.HitArm, DamagedLeg);
        EventManager<HitBodyPart >.StartListening(HitBodyPart.HitLeg, DamagedLeg);
    }

    void Start()
    {
        // 체력 초기화
        currentHp = maxHp;
    }

    public void DamagedHead()
    {
        Debug.Log("Head");
        // 즉사
        TakeDamge(maxHp);
    }

    public void DamagedThorax()
    {
        Debug.Log("Thorax");
        TakeDamge(50);
        // 뛰지 못하게
    }

    public void DamagedArm()
    {
        Debug.Log("Arm");
        TakeDamge(20);
        // 공격 속도 절반
    }

    public void DamagedLeg()
    {
        Debug.Log("Leg");
        TakeDamge(20);
        // 이동 속도 절반
    }

    public void TakeDamge(float damage)
    {
        currentHp -= damage;
    }
}
