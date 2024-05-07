using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSystem : NetworkBehaviour
{
    public static GameSystem Instance;

    public List<IngamePlayerMoveController> players = new List<IngamePlayerMoveController>();

    public void AddPlayer(IngamePlayerMoveController player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    //private IEnumerator GameReady()
    //{
    //    var manager = NetworkManager.singleton as FPSRoomManager;
    //    while(manager.roomSlots.Count != players.Count) 
    //    {
    //        yield return null;
    //    }
    //}

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
