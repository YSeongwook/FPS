using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("총알 트리거");
        ProjectileDisable(col.transform.position);
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("총알 트리거");
        ProjectileDisable(col.transform.position);
    }

    void ProjectileDisable(Vector3 hitPosition)
    {
        EffectManager.Instance.HitEffectGenenate(hitPosition);
        ObjectPool.Instance.EnqueueObject(gameObject);
    }
}
