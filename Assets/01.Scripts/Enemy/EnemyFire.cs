using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public AudioSource gunShot;
    public AudioSource reload;

    private Animator animator;
    private Transform playerTr;
    private Transform enemyTr;

    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    private float nextFire = 1.5f;
    private readonly float fireRate = 0.1f;
    private readonly float damping = 10.0f;

    [SerializeField] private readonly float realoadTime = 3.0f;
    [SerializeField] private readonly int maxBullet = 10;
    public float MinFireTime { get; set; }
    public float MaxFireTime { get; set; }

    private int currBullet = 10;
    private bool isReload = false;

    private WaitForSeconds wsReload;

    public bool isFire = false;

    [Header("Bullet")]
    public GameObject bullet;
    public GameObject bullet_Shell;

    [SerializeField] private Transform firePos;
    [SerializeField] private Transform shellPos;

    private Vector3 randomFirePos;
    private float randomX;
    private float randomY;

    private Quaternion rot;

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        wsReload = new WaitForSeconds(realoadTime);

        MinFireTime = 0f;
        MaxFireTime = 1f;

        StartCoroutine(changeLotation());
    }

    void Update()
    {

    }

    IEnumerator changeLotation()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Transform playerTr = player.GetComponent<Transform>();

            Vector3 directionToPlayer = playerTr.position - enemyTr.position;
            // 매우 작은 벡터를 체크하기 위해 epsilon 사용
            if (directionToPlayer.sqrMagnitude > Mathf.Epsilon)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                targetRotation.Normalize();  // Quaternion 정규화
                enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, targetRotation, Time.deltaTime * damping);
            }
        }

        if (!isReload && isFire)
        {
            if (Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(MinFireTime, MaxFireTime);
            }
        }
        yield return new WaitForSeconds(3f);
    }

    private void Fire()
    {
        randomX = (Random.Range(0, 0.8f));
        randomY = (Random.Range(0, 1.2f));
        randomFirePos = new Vector3(randomX, randomY, 0);
        animator.SetTrigger(hashFire);

        GameObject bulletIst = ObjectPool.Instance.DequeueObject(bullet);
        bulletIst.transform.position = firePos.position + randomFirePos;
        bulletIst.transform.rotation = firePos.rotation;

        bulletIst.GetComponent<Rigidbody>().velocity = bulletIst.transform.forward * 500;

        EffectManager.Instance.FireEffectGenenate(firePos.position, firePos.rotation);
        gunShot.PlayOneShot(gunShot.clip);
        isReload = (--currBullet % maxBullet == 0);

        if (isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        reload.PlayOneShot(reload.clip);

        yield return wsReload;

        currBullet = maxBullet;
        isReload = false;
    }
}
