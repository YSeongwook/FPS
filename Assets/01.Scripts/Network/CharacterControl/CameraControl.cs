using UnityEngine;
using Mirror;
using Cinemachine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraControl : NetworkBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] private GameObject tpsPos;
    [SerializeField] private CinemachineVirtualCamera tpsVCam;
    [SerializeField] private CinemachineVirtualCamera fpsVCam;

    [Header("Weapon Position")]
    [SerializeField] private Transform weaponPos;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform shellPos;
    
    private Vector2 mouseDeltaPos = Vector2.zero;

    private Animator animator;

    private bool isFire = false;
    private bool isZoom = false;

    [Header("Bullet")]
    public GameObject bullet;
    public GameObject bullet_Shell;
    public int shell = 30;

    private float fireDelay = 0;
    private float delayCount = 0.1f;
    private bool isReload = false;

    [Header("crosshair")]
    public GameObject crosshair;

    private void Start()
    {
        animator = GetComponent<Animator>();

        //if (isLocalPlayer)
        //{
        //    //Camera cam = Camera.main;
        //    //Debug.Log($"cam : {cam == null}");
        //    //cam.transform.SetParent(transform);
        //    //cam.transform.localPosition = Vector3.zero;
        //    //cam.orthographicSize = 2.5f;
        //    //tpsVCam
        //    fpsVCam.Priority = 20;
        //}

        //tpsVCam = tpsPos.GetComponent<CinemachineVirtualCamera>();

        // Cursor.visible = false;

        SetCamType(false);
    }

    private void Update()
    {
        // Object가 Client 소유인지 여부 확인
        if (isLocalPlayer)
        {
            RotateOrder();              // 캐릭터 및 총기 회전
            fireDelay += Time.deltaTime;
            GunFire();                  // 발사

            SetVCamPriority();
        }
    }

    private void LateUpdate()
    {
        // Object가 Client 소유인지 여부 확인
        if(isLocalPlayer) CamRotate();    // 카메라 회전
    }

    void CamRotate()
    {
        tpsPos.transform.position = this.transform.position + new Vector3(0, 1.5f, 0);

        Vector3 camAngle = tpsPos.transform.rotation.eulerAngles;

        if (isZoom) mouseDeltaPos *= 0.1f;
        else mouseDeltaPos *= 0.2f;

        float x = camAngle.x - mouseDeltaPos.y;

        if (x < 180f) x = Mathf.Clamp(x, -1f, 15f);
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

    // 수정이 필요
    void SetCamType(bool isFps)
    {
        if (isFps)
        {
            fpsVCam.Priority = 21;
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

            GameObject bulletInstance = ObjectPool.Instance.DequeueObject(bullet);
            bulletInstance.transform.position = firePos.position;

            // 발사 각도를 조정하여 목표 회전 각도를 계산
            Quaternion fireRotation = Quaternion.Euler(firePos.rotation.eulerAngles.x, firePos.rotation.eulerAngles.y, firePos.rotation.eulerAngles.z);
            bulletInstance.transform.rotation = fireRotation;

            bulletInstance.GetComponent<Rigidbody>().velocity = bulletInstance.transform.forward * 500;

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
            crosshair.SetActive(true);
        }
        else
        {
            isZoom = false;
            crosshair.SetActive(true);
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

    void SetVCamPriority()
    {
        // fpsVCam.Priority = 1;
        tpsVCam.Priority = 20;
    }
}
