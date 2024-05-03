using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        ProjectileDisable(collision.contacts[0].point);
    }

    void ProjectileDisable(Vector3 hitPosition)
    {
        EffectManager.Instance.HitEffectGenenate(hitPosition);
        ObjectPool.Instance.EnqueueObject(gameObject);
    }
}
