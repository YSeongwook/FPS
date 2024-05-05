
using UnityEngine;

public class Bullet : MonoBehaviour
{

    //private void OnCollisionEnter(Collision collision)
    //{
    //    ProjectileDisable(collision.contacts[0].point);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.CompareTag("Leg"))
        {
            Debug.Log("Leg");
            collision.gameObject.transform.root.GetComponent<EnemyDamage>().TakeDamge(5, collision);
        }
        else if (collision.gameObject.CompareTag("Arm"))
        {
            Debug.Log("Arm");
            collision.gameObject.transform.root.GetComponent<EnemyDamage>().TakeDamge(8, collision);
        }
        else if (collision.gameObject.CompareTag("Body"))
        {
            Debug.Log("Body");
            collision.gameObject.transform.root.GetComponent<EnemyDamage>().TakeDamge(10, collision);
        }
        else if (collision.gameObject.CompareTag("Head"))
        {
            Debug.Log("Head");
            collision.gameObject.transform.root.GetComponent<EnemyDamage>().TakeDamge(15, collision);
        }


        ProjectileDisable(collision.contacts[0].point);
    }



    void ProjectileDisable(Vector3 hitPosition)
    {
        EffectManager.Instance.HitEffectGenenate(hitPosition);
        ObjectPool.Instance.EnqueueObject(gameObject);
    }
}