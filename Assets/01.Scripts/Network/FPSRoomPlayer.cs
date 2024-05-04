using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FPSRoomPlayer : NetworkRoomPlayer
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        // 모든 클라이언트에서 실행되는 로직
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // 이 객체가 로컬 플레이어인 경우에만 실행되는 로직
        // Ready 버튼을 활성화하거나 관련 UI 변경
    }

    public override void OnClientEnterRoom()
    {
        // 클라이언트가 방에 들어왔을 때 호출
    }

    //public override void OnClientReady(bool readyState)
    //{
    //    // 클라이언트의 준비 상태 변경시 호출
    //}
}
