using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private const string bulletTag = "Bullet";
    [SerializeField] private float maxHp = 100.0f;
    [SerializeField] private float hp;

    // 현재 hp 프로퍼티
    public float CurrentHP { get; private set; }

    public GameObject bloodEffect;

    private void Awake()
    {
        hp = maxHp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == bulletTag)
        {
            ShowBloodEffect(collision);
            //collision.gameObject.SetActive(false);
            hp -= 10;

            Debug.Log(hp);

            if (hp <= 0f)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
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
