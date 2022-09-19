using UnityEngine;

namespace CUI.Device
{
    /// <summary>
    /// 移动设备陀螺仪控制器
    /// </summary>
    public class MoveWithGyroscope : MonoBehaviour
    {
        private void Awake()
        {
            if (SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true;
            }
        }
        
        private void Update()
        {
            if (SystemInfo.supportsGyroscope)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, ConvertRotation(Input.gyro.attitude), 0.5f);
            }
            if (SystemInfo.supportsAccelerometer)
            {
                for (int i = 0; i < Input.accelerationEventCount; i++)
                {
                    AccelerationEvent accEvent = Input.GetAccelerationEvent(i);
                    transform.position += accEvent.acceleration * accEvent.deltaTime;
                }
            }
        }
        private Quaternion ConvertRotation(Quaternion q)
        {
            return Quaternion.Euler(90, 0, 0) * (new Quaternion(-q.x, -q.y, q.z, q.w));
        }
    }
}