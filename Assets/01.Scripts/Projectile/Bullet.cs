using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("Enemy"))
            ProjectileDisable(collision.contacts[0].point);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    EnemyDamage enemyDamage = collision.gameObject.transform.root.GetComponent<EnemyDamage>();

    //    if(collision.gameObject.CompareTag("Leg"))
    //    {
    //        Debug.Log("Leg");
    //        enemyDamage.TakeDamge(5, collision);
    //    }
    //    else if (collision.gameObject.CompareTag("Arm"))
    //    {
    //        Debug.Log("Arm");
    //        enemyDamage.TakeDamge(8, collision);
    //    }
    //    else if (collision.gameObject.CompareTag("Body"))
    //    {
    //        Debug.Log("Body");
    //        enemyDamage.TakeDamge(10, collision);
    //    }
    //    else if (collision.gameObject.CompareTag("Head"))
    //    {
    //        Debug.Log("Head");
    //        enemyDamage.TakeDamge(15, collision);
    //    }

    //    ProjectileDisable(collision.contacts[0].point);
    //}

    void ProjectileDisable(Vector3 hitPosition)
    {
        EffectManager.Instance.HitEffectGenenate(hitPosition);
        ObjectPool.Instance.EnqueueObject(gameObject);
    }
}