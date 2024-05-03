using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    private const string bulletTag = "Bullet";
    private float hp = 100.0f;

    public GameObject bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        //bloodEffect = Resources.Load<GameObject>("BloodSplat_FX");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("!");
        if(collision.collider.tag == bulletTag)
        {
            ShowBloodEffect(collision);
            //collision.gameObject.SetActive(false);
            hp -= 10;
            Debug.Log(hp);
            if(hp <= 0.0f)
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

        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        //Destroy(blood, 1.0f);
    }
}
