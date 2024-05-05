
using UnityEngine;
using EnumTypes;
using EventLibrary;

public class EnemyDamage : Status
{
    [SerializeField] private const string bulletTag = "Bullet";
    [SerializeField] private const string enemyTag = "Enemy";

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
        base.TakeDamge(damage);

        GetComponent<EnemyAI>().traceDis = 100.0f;
        GetComponent<EnemyAI>().state = EnemyAI.State.TRACE;
        if (currentHp <= 0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
        }
    }

    private void PlayerDie()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.state = EnemyAI.State.PATROL;
            }
        }
    }
}
