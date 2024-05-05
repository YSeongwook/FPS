using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private const string bulletTag = "Bullet";
    [SerializeField] private const string enemyTag = "Enemy";
    [SerializeField] private float maxHp = 100.0f;
    [SerializeField] private float hp;
    public float CurrentHP { get; private set; }

    public GameObject bloodEffect;

    private void Awake()
    {
        hp = maxHp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("DAMAGE");
        if (collision.collider.tag == bulletTag)
        {
            ShowBloodEffect(collision);
            hp -= 10;

            Debug.Log(hp);

            if (hp <= 0f)
            {
                Debug.Log("playerDie");
                PlayerDie();

            }
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
