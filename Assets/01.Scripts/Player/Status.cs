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
        EventManager.StartListening(GlobalEvents.HitHead, DamagedLeg);
        EventManager.StartListening(GlobalEvents.HitThorax, DamagedLeg);
        EventManager.StartListening(GlobalEvents.HitArm, DamagedLeg);
        EventManager.StartListening(GlobalEvents.HitLeg, DamagedLeg);
    }

    void Start()
    {
        // 체력 초기화
        currentHp = maxHp;
    }

    public void DamagedHead()
    {
        // 즉사
        currentHp -= maxHp;
    }

    public void DamagedThorax()
    {
        currentHp -= 50;
        // 뛰지 못하게
    }

    public void DamagedArm()
    {
        currentHp -= 20;
        // 공격 속도 절반
    }

    public void DamagedLeg()
    {
        currentHp -= 20;
        // 이동 속도 절반
    }
}
