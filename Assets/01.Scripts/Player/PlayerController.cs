using Cinemachine;
using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject tpsPos;
    [SerializeField] private CinemachineVirtualCamera tpsVCam;
    [SerializeField] private CinemachineVirtualCamera fpsVCam;
    [SerializeField] private Transform weaponPos;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform shellPos;

    private Animator animator;
    private CharacterController cc;

    [SyncVar] private Vector2 moveVector = Vector2.zero;
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

    [Header("Zoom")]
    public GameObject crossHair;

    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        UnityEngine.Cursor.visible = false;

        SetCamType(false);
    }

    void Update()
    {

        if (isLocalPlayer)
        {
            MoveOrder();    // 이동  
            RotateOrder();  // 캐릭터 및 총기 회전

            fireDelay += Time.deltaTime;
            GunFire();      // 발사

            SetVCamPriority();
        }
    }

    void SetVCamPriority()
    {
        // fpsVCam.Priority = 1;
        tpsVCam.Priority = 20;
    }

    private void LateUpdate()
    {
        if (isLocalPlayer) CamRotate();    // 카메라 회전
    }

    private void MoveOrder()
    {
        animator.SetFloat("XSpeed", moveVector.x);
        animator.SetFloat("ZSpeed", moveVector.y);

        moveVector = Vector2.Lerp(moveVector, moveVectorTarget * moveSpeed, Time.deltaTime * 5);

        Vector3 moveVector3 = new Vector3(moveVector.x * 0.5f, Physics.gravity.y, moveVector.y);

        cc.Move(this.transform.rotation * moveVector3 * Time.deltaTime);

    }

    void CamRotate()
    {
        tpsPos.transform.position = this.transform.position + new Vector3(0, 1.5f, 0);

        Vector3 camAngle = tpsPos.transform.rotation.eulerAngles;

        if (isZoom) mouseDeltaPos *= 0.1f;
        else mouseDeltaPos *= 0.2f;

        float x = camAngle.x - mouseDeltaPos.y;

        if (x < 180f) x = Mathf.Clamp(x, 0f, 15f);
        else x = Mathf.Clamp(x, 345f, 361f);

        // 현재 회전 상태와 목표 회전 상태를 쿼터니언으로 변환합니다.
        Quaternion currentRotation = tpsPos.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(x, camAngle.y + mouseDeltaPos.x, camAngle.z);

        // 현재 회전 상태에서 목표 회전 상태로 부드럽게 보간합니다.
        tpsPos.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 80);
        mouseDeltaPos *= 0.3f;
    }

    void RotateOrder()
    {
        Vector3 direction = (tpsPos.transform.forward).normalized;

        Quaternion rotationWeapon = Quaternion.LookRotation(direction);
        rotationWeapon = Quaternion.Euler(rotationWeapon.eulerAngles.x - 10f, this.transform.rotation.eulerAngles.y, rotationWeapon.eulerAngles.z);
        weaponPos.rotation = Quaternion.Slerp(weaponPos.rotation, rotationWeapon, Time.deltaTime * 4f);

        direction = new Vector3(direction.x, 0, direction.z);

        Quaternion rotationBody = Quaternion.LookRotation(direction);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationBody, Time.deltaTime * 8f);
    }

    void SetCamType(bool isFps)
    {
        if (isFps && isLocalPlayer)
        {
            fpsVCam.Priority = 21;
        }
        else
        {
            fpsVCam.Priority = 9;
        }
    }

    [Command]
    void CmdFireBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bulletInstance = ObjectPool.Instance.DequeueObject(bullet);
        if (bulletInstance == null)
        {
            bulletInstance = Instantiate(bullet, position, rotation); // 풀이 비어있으면 새로 생성
        }
        else
        {
            bulletInstance.transform.position = position;
            bulletInstance.transform.rotation = rotation;
        }

        bulletInstance.GetComponent<Rigidbody>().velocity = bulletInstance.transform.forward * 500;
        NetworkServer.Spawn(bulletInstance);
    }


    void GunFire()
    {
        if (fireDelay >= delayCount && isFire && shell > 0 && !isReload)
        {
            fireDelay = 0;
            Quaternion fireRotation = Quaternion.Euler(firePos.rotation.eulerAngles.x, firePos.rotation.eulerAngles.y, firePos.rotation.eulerAngles.z);
            CmdFireBullet(firePos.position, fireRotation); // 서버에 발사 요청

            EffectManager.Instance.FireEffectGenenate(firePos.position, firePos.rotation);
            mouseDeltaPos = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 3f));
            animator.SetTrigger("Fire");
            shell--;
        }
    }


    void Reload()
    {
        if (isLocalPlayer)
        {
            animator.SetTrigger("Reload");
            StartCoroutine(ReloadEnd());
        }
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
        Debug.Log($"isLocalPlayer : {isLocalPlayer}");
        if (isLocalPlayer)
            moveVectorTarget = inputValue.Get<Vector2>();//인풋 벡터 받아옴
    }

    void OnSprint(InputValue inputValue)
    {
        if (isLocalPlayer)
        {
            float value = inputValue.Get<float>();
            moveSpeed = (value * 2f) + 2f;
        }
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
            crossHair.SetActive(true);
        }
        else
        {
            isZoom = false;
            crossHair.SetActive(true);
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

        // 줌하고 있는 중에는 재장전 불가능
        if (!isReload && !isZoom)
        {
            isReload = true;
            Reload();
        }
    }
}
