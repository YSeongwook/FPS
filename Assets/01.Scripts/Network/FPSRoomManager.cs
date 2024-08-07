using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FPSRoomManager : NetworkManager
{
    public int maxPlayerCount;
    public int minPlayerCount = 1;
    

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    
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

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        foreach (NetworkConnection conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
            {
                Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();
                conn.identity.transform.position = spawnPos;
            }
        }
    }

    //public override void OnServerAddPlayer(NetworkConnection conn, AddPlayerMessage extraMessage)
    //{
    //    base.OnServerAddPlayer(conn, extraMessage); // 기본 플레이어 생성 로직 실행
    //
    //    // 스폰 위치 동적으로 가져오기
    //    Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();
    //    GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    //    NetworkServer.AddPlayerForConnection(conn, player);
    //}
}
