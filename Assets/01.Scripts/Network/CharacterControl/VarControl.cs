using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VarControl : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextMeshProUGUI;

    void Start()
    {
        if(isLocalPlayer)
            TextMeshProUGUI.text = PlayerSetting.nickname;
    }
}
