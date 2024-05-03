using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class OnlineUI : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField nicknameInputField;
    [SerializeField]
    private GameObject createRoomUI;

    public void OnClickCreateRoomButton()
    {
        if (nicknameInputField.text != "")
        {
            PlayerSetting.nickname = nicknameInputField.text;
            createRoomUI.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
    }

    public void OnClickEnterGameRoomButton()
    {
        if (nicknameInputField.text != "")
        {
            PlayerSetting.nickname = nicknameInputField.text;
            var manager = FPSRoomManager.singleton;
            manager.StartClient();
        }
        else
        {
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }

        
    }
}
