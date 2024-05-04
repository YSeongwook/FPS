using UnityEngine;
using Mirror;

public class CameraControl : NetworkBehaviour
{
    private Camera playerCamera;

    private void Awake()
    {
        // 동일 레벨의 모든 카메라 컴포넌트 검색
        Camera[] cameras = FindObjectsOfType<Camera>();

        foreach (Camera cam in cameras)
        {
            if (cam.transform.parent == transform.parent) // 같은 부모를 가진 카메라 찾기
            {
                playerCamera = cam;
                break; // 첫 번째로 찾은 카메라를 할당하고 반복 종료
            }
        }

        allocateCamera();
    }

    private void FixedUpdate()
    {
        Debug.Log($"isLocalPlayer : {isLocalPlayer}");
        Debug.Log($"isOwned : {isOwned}");

        allocateCamera();
    }

    private void allocateCamera()
    {
        // 카메라를 현재 로컬 플레이어에만 할당
        if (isLocalPlayer || isOwned)
        {
            playerCamera.enabled = true;
            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localPosition = new Vector3(0, 1, -10); // 적절한 위치 조정
        }
        else
        {
            playerCamera.enabled = false;
        }
    }
}
