using UnityEngine;
using Mirror;

public class ShrinkZone : NetworkBehaviour
{
    public Vector3 shrinkRate = new Vector3(0.1f, 0.1f, 0.1f);
    public float minSize = 0.03f;
    public float damagePerSecond = 100f;

    private bool isOutsideZone = false;
    private Status playerStatus;  // Status 클래스의 인스턴스를 저장

    void Update()
    {
        //if (!isServer) return;

        if (transform.localScale.x > minSize && transform.localScale.y > minSize && transform.localScale.z > minSize)
        {
            transform.localScale -= shrinkRate * Time.deltaTime;
        }

        if (isOutsideZone && playerStatus != null)
        {
            playerStatus.TakeDamage(damagePerSecond * Time.deltaTime, "Zone");  // Status 클래스의 TakeDamage 메서드 호출
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!isServer) return;

        if (other.CompareTag("Player"))
        {
            isOutsideZone = false;
            playerStatus = other.GetComponent<Status>();  // 플레이어의 Status 컴포넌트 참조 저장
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (!isServer) return;

        if (other.CompareTag("Player"))
        {
            isOutsideZone = true;
        }
    }
}
