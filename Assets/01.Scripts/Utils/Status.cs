using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Status : NetworkBehaviour, IDamaged
{
    [SerializeField] protected float maxHp = 100f;
    [SyncVar(hook = nameof(OnHpChange))]
    public float currentHp;

    private string lastHitBodyPart;

    [SerializeField]
    private Text s_text;
    [SerializeField]
    private Image healthBar; // HP 바를 위한 Image 컴포넌트 참조

    public Animator animator;
    public CharacterController chracterController;
    public PlayerController playerController;
    public AudioListener audioListener;
    public GameObject tpsPos;

    // public float CurrentHp { get { return currentHp; } set { currentHp = value; } }

    private bool isAlive = true;
    private bool isBleeding = false;
    private float bleedingInterval = 6f; // 출혈 간격
    private float bleedingTimer = 0f;    // 출혈 타이머

    void Start()
    {
        healthBar = GameObject.Find("HP Bar").GetComponent<Image>();
        s_text = GameObject.Find("Survive Text").GetComponent<Text>();
        
        // 체력 초기화
        currentHp = maxHp;
        // 게임 시작 시 HP 바 업데이트
        UpdateHealthBar(); 
    }

    public void OnHpChange(float oldHp, float newHp)
    {
        Debug.Log($"HP changed from {oldHp} to {newHp}");
        // 로그를 추가하여 클라이언트에서 실행되는지 확인
        Debug.Log("HP Change detected on " + (isServer ? "Server" : "Client"));
        UpdateHealthBar();
        if (newHp <= 0 && oldHp > 0) // 체력이 0 이하로 떨어진 경우
        {
            CheckDeathBodyPart(lastHitBodyPart); // 마지막으로 피격된 부위에 따른 사망 애니메이션
            playerController.enabled = false;
            chracterController.enabled = false;
            // 기타 사망 관련 로직 수행
        }
    }

    [Server]
    public void TakeDamage(float damage, string hitBodyPart)
    {
        if (currentHp > 0)
        {
            currentHp -= damage;

            if (currentHp <= 0 && isAlive)
            {
                currentHp = 0;
                isAlive = false;
                RpcPlayDeathAnimation(hitBodyPart); // 모든 클라이언트에 사망 애니메이션을 실행하도록 요청
                RpcAnnounceDeath(PlayerSetting.nickname, hitBodyPart); // 모든 클라이언트에 사망 로그 전송
                playerController.enabled = false;   // 플레이어 컨트롤러 스크립트 비활성화
                chracterController.enabled = false; // 캐릭터 컨트롤러 비활성화
                // 카메라 포지션 수정, 보류
                // 서버에 사망 알리기
                audioListener.enabled = false;      // 오디오 리스너 비활성화
                // UI 전환
            }
            else if (Random.Range(0f, 1f) < 0.3f) // 30% 확률로 출혈 발생
            {
                Bleeding();
            }
        }
    }

    [ClientRpc]
    public void RpcAnnounceDeath(string playerName, string hitBodyPart)
    {
        Debug.Log($"{playerName} has died due to a hit to the {hitBodyPart}.");
        s_text.text = $"{playerName} has died due to a hit to the {hitBodyPart}.";
        StartCoroutine(ClearDeathAnnouncement());
    }

    IEnumerator ClearDeathAnnouncement()
    {
        yield return new WaitForSecondsRealtime(4f);  // 4초 기다립니다.
        s_text.text = "";  // 텍스트를 비웁니다.
    }

    public void DamagedHead()
    {
        // 즉사
        RequestDamage(maxHp, "Head");
    }

    public void DamagedThorax()
    {
        RequestDamage(50, "Thorax");
        // 뛰지 못하게
        playerController.canSprint = false; // 뛰지 못하게
    }

    public void DamagedArm()
    {
        RequestDamage(20, "Arm");
        // 공격 속도 절반
        playerController.DelayCount *= 2f;  // 공격 속도 절반
    }

    public void DamagedLeg()
    {
        RequestDamage(20, "Leg");
        // 이동 속도 절반
        playerController.MoveSpeed /= 2f;   // 이동 속도 절반
    }

    [ClientRpc]
    public void RpcPlayDeathAnimation(string hitBodyPart)
    {
        CheckDeathBodyPart(hitBodyPart);
    }

    // 출혈 효과
    public void Bleeding()
    {
        if (!isBleeding)
        {
            isBleeding = true;
            StartCoroutine(BleedingCoroutine());
        }
    }

    IEnumerator BleedingCoroutine()
    {
        while (currentHp > 0 && isBleeding)
        {
            yield return new WaitForSeconds(bleedingInterval);
            currentHp -= 3;

            // 체력이 0 이하로 떨어진 경우
            if (currentHp <= 0 && isAlive)
            {
                isAlive = false;
                currentHp = 0;
                playerController.enabled = false;   // 플레이어 컨트롤러 스크립트 비활성화
                chracterController.enabled = false; // 캐릭터 컨트롤러 비활성화
                CheckDeathBodyPart("Thorax"); // 사망 처리
                // 카메라 포지션 수정, 보류
                // 서버에 사망 알리기
                // 오디오 리스너 비활성화
                audioListener.enabled = false;
                // UI 전환
            }
        }

        isBleeding = false;
    }

    // 어느 부위에 맞아 사망했는지 판별
    public void CheckDeathBodyPart(string hitBodyPart)
    {
        switch(hitBodyPart)
        {
            case "Head":
                animator.SetTrigger("Death_Top");
                break;
            case "Thorax":
            case "Arm":
                animator.SetTrigger("Death_Middle");
                break;
            case "Leg":
                animator.SetTrigger("Death_Bottom");
                break;
            case "Zone":
                animator.SetTrigger("Death_Bottom");
                break;
            default:
                Debug.LogWarning("Unknown body part hit.");
                break;
        }
    }

    public void RequestDamage(float damage, string hitBodyPart)
    {
        if (isServer)
        {
            TakeDamage(damage, hitBodyPart);
        }
        else
        {
            CmdRequestDamage(damage, hitBodyPart);
        }
    }

    [Command]
    public void CmdRequestDamage(float damage, string hitBodyPart)
    {
        TakeDamage(damage, hitBodyPart);
    }

    // HP 바를 업데이트하는 함수
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
        }
    }

}