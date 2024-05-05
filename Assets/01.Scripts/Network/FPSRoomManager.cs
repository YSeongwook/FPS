using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FPSRoomManager : NetworkRoomManager
{
    public int maxPlayerCount;
    public int minPlayerCount = 1;
    

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
    
        // 연결에 이미 플레이어가 할당되어 있지 않은 경우에만 실행
        if (conn.identity == null)
        {
            Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();
            GameObject player = Instantiate(spawnPrefabs[0], spawnPos, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player);
            //NetworkServer.Spawn(player, conn);
        }
        else
        {
            Debug.Log("Player already assigned to this connection.");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        // 씬 전환 전에 모든 로비 플레이어를 비활성화 또는 제거
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
            {
                var roomPlayer = conn.identity.GetComponent<NetworkRoomPlayer>();
                if (roomPlayer != null)
                {
                    NetworkServer.Destroy(roomPlayer.gameObject); // 로비 플레이어 객체 제거
                }
            }
        }

        base.ServerChangeScene(newSceneName);
    }

}


//public override void OnRoomServerConnect(NetworkConnectionToClient conn)
//{
//    base.OnRoomServerConnect(conn);
//
//    Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();
//
//    var player = Instantiate(spawnPrefabs[0], spawnPos, Quaternion.identity);
//    NetworkServer.AddPlayerForConnection(conn, player);
//    //NetworkServer.Spawn(player, conn);
//}