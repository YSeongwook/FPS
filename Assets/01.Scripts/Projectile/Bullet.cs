using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        ProjectileDisable(collision.contacts[0].point);
        //HpChek(collision);
    }

    void ProjectileDisable(Vector3 hitPosition)
    {
        EffectManager.Instance.HitEffectGenenate(hitPosition);
        ObjectPool.Instance.EnqueueObject(gameObject);
    }

    //void HpChek(Collision collision) //충돌한 컬라이더의 hp가 있는지 체크후 연산
    //{ 
    //    GameObject colObject = collision.gameObject; //충돌한 컬라이더의 오브젝트를 불러옴

    //    Hpmanager hpManager = colObject.GetComponent<Hpmanager>(); //hp매니저를 흭득시도

    //    if (hpManager != null) //hp매니저가 있을시 처리
    //    {
    //        hpManager.Hp -=10;
    //        Debug.Log (hpManager.Hp);
    //    }
  

}
