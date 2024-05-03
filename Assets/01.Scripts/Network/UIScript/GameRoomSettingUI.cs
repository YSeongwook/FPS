using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoomSettingUI : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Close()
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        animator.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("close");
    }

    public void ExitGameRoom()
    {
        var manager = FPSRoomManager.singleton;
        if(manager.mode == Mirror.NetworkManagerMode.Host)
        {
            manager.StopHost();
        }
        else if(manager.mode == Mirror.NetworkManagerMode.ClientOnly)
        {
            manager.StopClient();
        }
    }
}
