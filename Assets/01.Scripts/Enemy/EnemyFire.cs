using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    private AudioSource audio;
    private Animator animator;
    private Transform playerTr;
    private Transform enemyTr;

    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    private float nextFire = 0.0f;
    private readonly float fireRate = 0.1f;
    private readonly float damping = 10.0f;

    [SerializeField]private readonly float realoadTime = 2.0f;
    [SerializeField]private readonly int maxBullet = 10;
    private int currBullet = 10;
    private bool isReload = false;

    private WaitForSeconds wsReload;

    public bool isFire = false;
    public AudioClip fireSfx;
    public AudioClip reloadSfx;

    [Header("Bullet")]
    public GameObject bullet;
    public GameObject bullet_Shell;

    [SerializeField] private Transform firePos;
    [SerializeField] private Transform shellPos;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(realoadTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isReload && isFire)
        {
            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 3f);
            }
            // 플레이어 바라보게 
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    private void Fire()
    {
        animator.SetTrigger(hashFire);
        //audio.PlayOneShot(fireSfx, 1.0f);

        GameObject bulletIst = ObjectPool.Instance.DequeueObject(bullet);
        bulletIst.transform.position = firePos.position;
        bulletIst.transform.rotation = firePos.rotation;

        bulletIst.GetComponent<Rigidbody>().velocity = bulletIst.transform.forward * 500;

        EffectManager.Instance.FireEffectGenenate(firePos.position, firePos.rotation);

        isReload = (--currBullet % maxBullet == 0);

        if (isReload)
        {
            StartCoroutine(Reloading());
        }

        

    }

    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        audio.PlayOneShot(reloadSfx, 1.0f);

        yield return wsReload;

        currBullet = maxBullet;
        isReload = false;
    }
}
