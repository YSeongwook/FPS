using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerMoveController : NetworkBehaviour
{
    private Animator animator;
    private CharacterController cc;

    [SyncVar] private Vector2 moveVector = Vector2.zero;
    private Vector2 moveVectorTarget;
    private float moveSpeed = 2f;

    private void Awake()
    {
        //GameObject tpsVCamInstance = Instantiate(tpsVCam);
        //tpsVCam = tpsVCamInstance;
        //CinemachineVirtualCamera fpsVCamInstance = Instantiate(fpsVCam).GetComponent<CinemachineVirtualCamera>();
        //fpsVCam = fpsVCamInstance;
        //Transform weaponPosInstance = Instantiate(weaponPos).GetComponent<Transform>();
        //weaponPos = weaponPosInstance;
        //Transform firePosInstance = Instantiate(firePos).GetComponent<Transform>();
        //firePos = firePosInstance;
        //Transform shellPosInstance = Instantiate(shellPos).GetComponent<Transform>();
        //shellPos = shellPosInstance;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveOrder();    // 이동  
    }

    private void MoveOrder()
    {
        animator.SetFloat("XSpeed", moveVector.x);
        animator.SetFloat("ZSpeed", moveVector.y);

        moveVector = Vector2.Lerp(moveVector, moveVectorTarget * moveSpeed, Time.deltaTime * 5);

        Vector3 moveVector3 = new Vector3(moveVector.x * 0.5f, Physics.gravity.y, moveVector.y);

        cc.Move(this.transform.rotation * moveVector3 * Time.deltaTime);
    }

    void OnMove(InputValue inputValue) // 이동(WASD)
    {
        // Object가 Client 소유인지 여부 확인
        if (isOwned)
            moveVectorTarget = inputValue.Get<Vector2>();//인풋 벡터 받아옴
    }

    void OnSprint(InputValue inputValue)
    {
        // Object가 Client 소유인지 여부 확인
        if (isOwned)
        {
            float value = inputValue.Get<float>();
            moveSpeed = (value * 4f) + 1f;
        }
    }
}
