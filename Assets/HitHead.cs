using EnumTypes;
using EventLibrary;
using UnityEngine;

public class HitHead : MonoBehaviour
{
    [SerializeField] private const string bulletTag = "Bullet";

    public GameObject bloodEffect;
    private void Start()
    {
        bloodEffect = Resources.Load<GameObject>("BloodSplat_FX");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag == bulletTag)
        {
            ShowBloodEffect(collision);
            EventManager<HitBodyPart>.TriggerEvent(HitBodyPart.HitHead);
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
