using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private CharacterController cc;

    private Vector2 inputMovement = Vector2.zero;
    private Vector3 moveDir = Vector3.zero;
    private bool isMoved = false;
    private float xSpeed = 0f;
    private float zSpeed = 0f;
    private float xTargetSpeed = 0f;
    private float zTargetSpeed = 0f;

    private float yRotation;
    private float xRotation;
    public Camera cam;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float mouseSpeed = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서를 화면 안에서 고정
        Cursor.visible = false;                     // 마우스 커서를 보이지 않도록 설정
    }

    private void Update()
    {
        xSpeed = UpdateParameterSpeed(xSpeed, xTargetSpeed);
        zSpeed = UpdateParameterSpeed(zSpeed, zTargetSpeed);

        anim.SetFloat("XSpeed", xSpeed);
        anim.SetFloat("ZSpeed", zSpeed);

        LookAt();
    }

    void FixedUpdate()
    {
        if (isMoved) 
        {
            cc.Move(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();

        if (context.performed)
        {
            isMoved = true;


            UpdateMoveDirection();
            UpdateTargetSpeed();
        }
        else if (context.canceled)
        {
            isMoved = false;
            xTargetSpeed = 0f;
            zTargetSpeed = 0f;
            moveDir = Vector3.zero;
        }
    }
    
    // 현재 이동방향 이상함
    void UpdateMoveDirection()
    {
        moveDir = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;
    }

    void UpdateTargetSpeed()
    {
        // WASD 각 키의 입력에 따라서 속도 증감
        if (inputMovement.x > 0f)
        {
            xTargetSpeed = 3f; // D 키가 눌렸을 때 X 방향 속도 증가
        }
        else if (inputMovement.x < 0f)
        {
            xTargetSpeed = -3f; // A 키가 눌렸을 때 X 방향 속도 감소
        }
        else
        {
            xTargetSpeed = 0f; // A와 D 키가 모두 눌리지 않은 경우 X 방향 속도 초기화
        }

        if (inputMovement.y > 0f)
        {
            zTargetSpeed = 3f; // W 키가 눌렸을 때 Z 방향 속도 증가
        }
        else if (inputMovement.y < 0f)
        {
            zTargetSpeed = -3f; // S 키가 눌렸을 때 Z 방향 속도 감소
        }
        else
        {
            zTargetSpeed = 0f; // W와 S 키가 모두 눌리지 않은 경우 Z 방향 속도 초기화
        }
    }

    float UpdateParameterSpeed(float speed, float targetSpeed)
    {
        float speedChange = 3f * Time.deltaTime;
        if (targetSpeed > speed)
        {
            speed += speedChange;
            if (speed > targetSpeed)
                speed = targetSpeed;
        }
        else if (targetSpeed < speed)
        {
            speed -= speedChange;
            if (speed < targetSpeed)
                speed = targetSpeed;
        }

        return speed;
    }

    public void Fire(InputAction.CallbackContext context)
    {

    }

    void LookAt()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

        // 마우스 입력에 따라 회전값 업데이트
        yRotation += mouseX;
        xRotation -= mouseY;

        // 수직 회전 값을 -90도에서 90도 사이로 제한
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 새로운 회전값 계산
        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // 부드러운 회전을 위해 Slerp 사용
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Euler(0, yRotation, 0); // 플레이어 캐릭터의 수평 회전값은 부드럽게 변하지 않도록 직접 설정
    }

}
