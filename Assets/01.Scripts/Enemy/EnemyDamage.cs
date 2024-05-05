
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private const string bulletTag = "Bullet";
    [SerializeField] private const string enemyTag = "Enemy";
    [SerializeField] private float maxHp = 100.0f;
    [SerializeField] private float hp;

    // 현재 hp 프로퍼티
    public float CurrentHP { get; private set; }

    public GameObject bloodEffect;

    private void Awake()
    {
        hp = maxHp;
    }


    public void TakeDamge(float damage, Collision collision)
    {
        GetComponent<EnemyAI>().traceDis = 100.0f;
        GetComponent<EnemyAI>().state = EnemyAI.State.TRACE;
        ShowBloodEffect(collision);
        hp -= damage;
        Debug.Log(hp);
        if (hp <= 0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
        }
    }




    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == bulletTag)
    //    {
    //        GetComponent<EnemyAI>().traceDis = 100.0f;
    //        GetComponent<EnemyAI>().state = EnemyAI.State.TRACE;
    //        ShowBloodEffect(collision);
    //        //collision.gameObject.SetActive(false);
    //        hp -= 10;

    //        Debug.Log(hp);

    //        if (hp <= 0f)
    //        {
    //            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
    //            //PlayerDie();

    //        }
    //    }
    //}

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

    private void ShowBloodEffect(Collision collision)
    {
        Vector3 pos = collision.contacts[0].point;      // 최초 충돌 지점의 
        Vector3 _normal = collision.contacts[0].normal; // 법선 벡터를 구하여
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal); // 회전값 계산
        // -Vector3.forward를 충돌점의 법선벡터가 바라보는 방향과 일치 시키기

        GameObject blood = ObjectPool.Instance.DequeueObject(bloodEffect);

        blood.transform.position = pos;
        blood.transform.rotation = rot;
    }
}
