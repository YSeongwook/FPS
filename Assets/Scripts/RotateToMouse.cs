using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField] private float rotCamXAxisSpeed = 5f;   // 카메라 x축 회전 속도
    [SerializeField] private float rotCamYAxisSpeed = 2f;   // 카메라 y축 회전 속도
    [SerializeField] private float smoothness = 5f;         // 부드러운 이동을 위한 변수

    private float limitMinX = -10f;
    private float limitMaxX = 10f;
    private float eulerAngleX;
    private float eulerAngleY;

    private Quaternion targetRotation;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleX -= mouseY * rotCamXAxisSpeed * Time.deltaTime;
        eulerAngleY += mouseX * rotCamYAxisSpeed * Time.deltaTime * 2;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        targetRotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothness);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
