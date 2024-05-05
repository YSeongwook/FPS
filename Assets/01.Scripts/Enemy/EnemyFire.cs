using System.Collections;
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

    [SerializeField] private readonly float realoadTime = 2.0f;
    [SerializeField] private readonly int maxBullet = 10;
    [SerializeField] private float randomFireTime = 1f;
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

    private Vector3 randomFirePos;
    private float randomX;
    private float randomY;

    void Start()
    {      
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(realoadTime);
    }

    

    void Update()
    {
        var player = FindObjectOfType<PlayerController>();
        playerTr = player.GetComponent<Transform>();
        Debug.Log(playerTr.position);
        if (!isReload && isFire)
        {
            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, randomFireTime);
            }

            // 플레이어 바라보게 
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    private void Fire()
    {
        randomX = (Random.Range(0, 0.3f));
        randomY = (Random.Range(0, 0.3f));
        randomFirePos = new Vector3(randomX, 0f, randomY);
        animator.SetTrigger(hashFire);
        //audio.PlayOneShot(fireSfx, 1.0f);

        GameObject bulletIst = ObjectPool.Instance.DequeueObject(bullet);
        bulletIst.transform.position = firePos.position + randomFirePos;
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
