using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager Instance;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private GameRoomPlayerCounter gameRoomPlayerCounter;
    public GameRoomPlayerCounter GameRoomPlayerCounter { get { return gameRoomPlayerCounter; } }

    private void Awake()
    {
        Instance = this;
    }

    public void ActiveStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    public void SetInteractableStartButton(bool interactable)
    {
        startButton.interactable = interactable;
    }

    public void OnClickStartButton()
    {
        var manager = NetworkManager.singleton as FPSRoomManager;
        manager.ServerChangeScene(manager.GameplayScene);
    }
}
