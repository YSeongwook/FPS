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
    private GameRoomPlayerCounter gameRoomPlayerCounter;
    public GameRoomPlayerCounter GameRoomPlayerCounter { get { return gameRoomPlayerCounter; } }

    private void Awake()
    {
        Instance = this;
    }
}
