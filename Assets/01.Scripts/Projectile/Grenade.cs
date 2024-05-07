using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject fin;
    public GameObject clip;
    public float BoomRaius = 4f;
    public Collider[] cols;
    public int Dmg = 110;
    public bool inHand = false;
    public GameObject Hand;
    public float speed = 1f;

    Rigidbody GreandeRigid;
    new AudioSource audio;

    public void OnEnable()
    {
        audio = GetComponent<AudioSource>();
        //StartCoroutine("Boom"); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        //StartCoroutine("Boom"); 
        Debug.Log(collision.gameObject.tag);
    }

    public void Update()
    {
        if (inHand)
        {
            transform.position = Hand.transform.position;
            transform.rotation = Hand.transform.rotation;
        }
    }

    public void Fin()
    {
        fin.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Trhow(float time)
    {

        gameObject.GetComponent<Collider>().isTrigger = true;
        clip.GetComponent<Rigidbody>().isKinematic = false;
        inHand = false;
        GreandeRigid = GetComponent<Rigidbody>();
        GreandeRigid.isKinematic = false;
        //GreandeRigid.AddForce(transform.up 40ftime);
        //GreandeRigid.AddForce(transform.right * 40f * time);

        Vector3 throwDirection = transform.up + transform.forward;

        GreandeRigid.AddForce(throwDirection.normalized * speed * time, ForceMode.Impulse);
        StartCoroutine("Boom");
        //StartCoroutine(Collideron());
        Debug.Log("Trhow");
    }

    IEnumerator Boom()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        gameObject.GetComponent<Collider>().isTrigger = false;
        Debug.Log("Boom");
        yield return new WaitForSecondsRealtime(3f);
        cols = Physics.OverlapSphere(transform.position, BoomRaius);
        audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        ProjectileDisable(transform.position);
        foreach (Collider col in cols)
        {
            GameObject obj = col.gameObject;
            GameObject rootObject = col.transform.root.gameObject;

            Status status = rootObject.GetComponent<Status>();
            if (status != null) //hp매니저가 있을시 처리
            {
                float distance = Vector3.Distance(gameObject.transform.position, col.transform.position);
                float currentHp = Mathf.Max(0, Dmg - (int)distance * 5); // 거리에따라 데미지감소를 추가하여 데미지감소
                status.RequestDamage(currentHp, "Thorax");
            }

            EnemyDamage enemyDamage = rootObject.GetComponent<EnemyDamage>();
            if(enemyDamage != null)
            {
                float distance = Vector3.Distance(gameObject.transform.position, col.transform.position);
                float currentHp = Mathf.Max(0, Dmg - (int)distance * 5); // 거리에따라 데미지감소를 추가하여 데미지감소
                enemyDamage.TakeDamge(currentHp, "Thorax");
                Debug.Log("EnemyinGrenade");
            }
        }
    }

    void ProjectileDisable(Vector3 boomPosition)
    {
        EffectManager.Instance.GrenadeEffectGenenate(boomPosition);

        ObjectPool.Instance.EnqueueObject(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, BoomRaius);
    }
}
