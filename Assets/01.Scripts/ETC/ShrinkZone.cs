using UnityEngine;

public class ShrinkZone : MonoBehaviour
{
    public Vector3 shrinkRate = new Vector3(0.1f, 0.1f, 0.1f);  // 매 초마다 줄어드는 비율
    public float minSize = 0.1f;  // 최소 크기 제한

    private bool isOutsideZone = false;  // 플레이어가 자기장 밖에 있는지 상태 표시

    void Update()
    {
        if (transform.localScale.x > minSize && transform.localScale.y > minSize && transform.localScale.z > minSize)
        {
            transform.localScale -= shrinkRate * Time.deltaTime;  // 시간에 따라 Scale 감소
        }

        if (isOutsideZone)
        {
            Debug.Log("Player is outside the zone!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 'Player' 태그를 가진 오브젝트가 자기장 안으로 들어올 때
        if (other.CompareTag("Player"))
        {
            isOutsideZone = false;  // 상태를 '안'으로 변경
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 'Player' 태그를 가진 오브젝트가 자기장 밖으로 나갈 때
        if (other.CompareTag("Player"))
        {
            isOutsideZone = true;  // 상태를 '밖'으로 변경
        }
    }
}