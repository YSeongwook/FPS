using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject tpsVCam;
    [SerializeField] private CinemachineVirtualCamera fpsVCam;
    [SerializeField] private Transform weaponPos;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform shellPos;

    private Animator animator;
    private CharacterController cc;

    private Vector2 moveVector = Vector2.zero;
    private Vector2 moveVectorTarget;
    private Vector2 mouseDeltaPos = Vector2.zero;
    private float moveSpeed = 2f;
    private bool isFire = false;
    private bool isZoom = false;

    [Header("Bullet")]
    public GameObject bullet;
    public GameObject bullet_Shell;
    public int shell = 30;

    private float fireDelay = 0;
    private float delayCount = 0.1f;
    private bool isReload = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        Cursor.visible = false;

        SetCamType(false);
    }

    void Update()
    {
        MoveOrder();    // 이동  
        RotateOrder();  // 캐릭터 및 총기 회전

        fireDelay += Time.deltaTime;
        GunFire();      // 발사
    }

    private void LateUpdate()
    {
        CamRotate();    // 카메라 회전
    }

    private void MoveOrder()
    {
        moveVector = Vector2.Lerp(moveVector, moveVectorTarget * moveSpeed, Time.deltaTime * 5);

        Vector3 moveVector3 = new Vector3(moveVector.x * 0.5f, Physics.gravity.y, moveVector.y);

        cc.Move(this.transform.rotation * moveVector3 * Time.deltaTime);

        animator.SetFloat("XSpeed", moveVector.x);
        animator.SetFloat("ZSpeed", moveVector.y);
    }

    void CamRotate()
    {
        tpsVCam.transform.position = this.transform.position + new Vector3(0, 1.5f, 0);

        Vector3 camAngle = tpsVCam.transform.rotation.eulerAngles;

        if (isZoom) mouseDeltaPos *= 0.2f;
        else mouseDeltaPos *= 0.4f;

        float x = camAngle.x - mouseDeltaPos.y;

        if (x < 180f) x = Mathf.Clamp(x, -1f, 25f);
        else x = Mathf.Clamp(x, 345f, 361f);

        tpsVCam.transform.rotation = Quaternion.Euler(x, camAngle.y + mouseDeltaPos.x, camAngle.z);
        mouseDeltaPos *= 0.9f;
    }
    void RotateOrder()
    {
        Vector3 direction = (tpsVCam.transform.forward).normalized;

        Quaternion rotationWeapon = Quaternion.LookRotation(direction);
        rotationWeapon = Quaternion.Euler(rotationWeapon.eulerAngles.x, this.transform.rotation.eulerAngles.y, rotationWeapon.eulerAngles.z);
        weaponPos.rotation = Quaternion.Slerp(weaponPos.rotation, rotationWeapon, Time.deltaTime * 6);

        direction = new Vector3(direction.x, 0, direction.z);

        Quaternion rotationBody = Quaternion.LookRotation(direction);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationBody, Time.deltaTime * 6);
    }
    void SetCamType(bool isFps)
    {
        if (isFps)
        {
            fpsVCam.Priority = 11;
        }
        else
        {
            fpsVCam.Priority = 9;
        }
    }
    void GunFire()
    {
        if (fireDelay >= delayCount && isFire && shell > 0 && !isReload)
        {
            fireDelay = 0;

            GameObject bulletIst = ObjectPool.Instance.DequeueObject(bullet);
            bulletIst.transform.position = firePos.position;
            bulletIst.transform.rotation = firePos.rotation;

            bulletIst.GetComponent<Rigidbody>().velocity = bulletIst.transform.forward * 500;

            EffectManager.Instance.FireEffectGenenate(firePos.position, firePos.rotation);
            mouseDeltaPos = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 3f));

            animator.SetTrigger("Fire");
            shell--;
        }
    }
    void Reload()
    {
        animator.SetTrigger("Reload");
        StartCoroutine(ReloadEnd());
    }

    IEnumerator ReloadEnd()
    {
        yield return new WaitForSeconds(3f);
        isReload = false;

        shell += 30;
        shell = Mathf.Clamp(shell, 0, 31);
    }

    void OnMove(InputValue inputValue) // 이동(WASD)
    {
        moveVectorTarget = inputValue.Get<Vector2>();//인풋 벡터 받아옴
    }

    void OnSprint(InputValue inputValue)
    {
        float value = inputValue.Get<float>();
        moveSpeed = (value * 4f) + 1f;
    }

    void OnFire(InputValue inputValue)
    {
        float isClick = inputValue.Get<float>();

        if (isClick == 1)   // 눌렀을 때
        {
            isFire = true;
        }
        else // 뗄 때
        {
            isFire = false;
        }
    }

    void OnZoom(InputValue inputValue)
    {
        float isClick = inputValue.Get<float>();

        if (isClick == 1)
        {
            isZoom = true;
        }
        else
        {
            isZoom = false;
        }
        SetCamType(isZoom);
    }

    void OnAim(InputValue inputValue)
    {
        mouseDeltaPos = inputValue.Get<Vector2>();    
    }

    void OnReload(InputValue inputValue)
    {
        float isClick = inputValue.Get<float>();

        if (!isReload)
        {
            //Debug.Log(isClick);
            isReload = true;
            Reload();
        }
    }
}
