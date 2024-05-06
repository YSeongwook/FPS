using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject fin;
    public GameObject clip;
    public float BoomRaius = 2.5f;
    public Collider[] cols;
    public int Dmg = 110;
    public bool inHand = false;
    public GameObject Hand;
    public float speed = 10f;

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
        StartCoroutine("Boom");

        gameObject.GetComponent<Collider>().isTrigger = false;
        clip.GetComponent<Rigidbody>().isKinematic = false;
        inHand = false;
        GreandeRigid = GetComponent<Rigidbody>();
        GreandeRigid.isKinematic = false;
        // GreandeRigid.AddForce(transform.up *40f*time); 
        // GreandeRigid.AddForce(transform.right * 40f * time); 

        Vector3 throwDirection = transform.up + transform.forward;

        GreandeRigid.AddForce(throwDirection.normalized * speed * time, ForceMode.Impulse);
        //StartCoroutine(Collideron()); 
    }

    IEnumerator Boom()
    {
        yield return new WaitForSecondsRealtime(3f);
        cols = Physics.OverlapSphere(transform.position, BoomRaius);
        audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        ProjectileDisable(transform.position);
        foreach (Collider col in cols)
        {
            Status status = col.GetComponent<Status>(); //hp매니저를 흭득시도 
            if (status != null) //hp매니저가 있을시 처리 
            {
                float distance = Vector3.Distance(gameObject.transform.position, col.transform.position);
                float currentHp = Mathf.Max(0, Dmg - (int)distance * 5);//거리에따라 데미지감소를 추가하여 데미지감소 
                //status.TakeDamge(currentHp); 
                Debug.Log("hpmanager있음");
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
