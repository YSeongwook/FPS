using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private float maxHp = 100f;    // 최대 체력
    public float CurrentHp { get; private set; }    // 현재 체력

    [SerializeField] private const string bulletTag = "Bullet";
    public GameObject bloodEffect;

    void Start()
    {
        CurrentHp = maxHp;
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("플레이어 피격");

        // 총알에 맞았을 시
        if(col.collider.CompareTag(bulletTag))
        {
            ShowBloodEffect(col);
            CurrentHp -= 10;

            Debug.Log(CurrentHp);

            if (CurrentHp <= 0f)
            {
                // 사망 처리, 사망 이벤트 트리거, 사망 애니메이션 재생, 콜라이더 비활성화
                Debug.Log("플레이어 사망");
            }
        }
    }

    // 피격 시 출혈 효과
    private void ShowBloodEffect(Collision col)
    {
        Vector3 pos = col.contacts[0].point;      // 최초 충돌 지점의 
        Vector3 normal = col.contacts[0].normal;  // 법선 벡터를 구하여
        
        // 회전값 계산, -Vector3.forward를 충돌점의 법선 벡터가 바라보는 방향과 일치 시키기
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, normal);

        GameObject blood = ObjectPool.Instance.DequeueObject(bloodEffect);

        blood.transform.position = pos;
        blood.transform.rotation = rot;
    }

    // Status를 가진 오브젝트가 살아있는지 확인
    public bool IsAlive()
    {
        return CurrentHp > 0;
    }
}
