using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using CUI.UI;
using VirtrulFactory;
using VirtrulFactory.UI;

namespace CUI.Device
{
    /// <summary>
    /// 角色视角控制器
    /// </summary>
    public class PlayerViewController : MonoBehaviour
    {
        public static PlayerViewController Current;

        [SerializeField] private PlayerViewOption viewConfigParameter;
        [SerializeField] private Transform focusPoint;
        [SerializeField] private Camera viewCamera;
        [SerializeField] private PlayerViewValue defaultValue;

        private float focusDistance = 0;

        private Transform focusTestPoint;
        private Transform cameraTestPoint;

        private DepthOfField depthOfField;

        private void Awake()
        {
            Current = this;
        }
        private void Start()
        {
            //init test point
            focusTestPoint = new GameObject().transform;
            focusTestPoint.name = "FocusTestPoint";
            focusTestPoint.position = defaultValue.FocusPointPosition;
            focusTestPoint.eulerAngles = defaultValue.FocusPointRotation;
            cameraTestPoint = new GameObject().transform;
            cameraTestPoint.name = "CameraTestPoint";
            cameraTestPoint.parent = viewCamera.transform.parent;
            cameraTestPoint.localEulerAngles = Vector3.zero;
            cameraTestPoint.localPosition = new Vector3(0, 0, viewCamera.transform.localPosition.z);
            //InteractManager events
            InteractManager.OnScreenDrag += ScreenDrag;
            InteractManager.OnZoom += Zoom;
            InteractManager.OnDoubleClick += ScreenClick;
            //init values
            ViewValue = defaultValue;

            PostProcessVolume processVolume = viewCamera.GetComponent<PostProcessVolume>();
            if (processVolume) processVolume.profile.TryGetSettings(out depthOfField);
        }
        private void Update()
        {
            SmoothDampFollow();
        }

        public PlayerViewValue ViewValue
        {
            get
            {
                PlayerViewValue view;
                view.FocusPointPosition = focusTestPoint.position;
                view.FocusPointRotation = new Vector3(focusTestPoint.eulerAngles.x, focusTestPoint.eulerAngles.y, 0);
                view.FocusDistance = focusDistance;
                return view;
            }
            set
            {
                viewCamera.transform.localPosition = new Vector3(0, 0, -value.FocusDistance);
                cameraTestPoint.localPosition = new Vector3(0, 0, -value.FocusDistance);
                focusTestPoint.Translate(0, 0, value.FocusDistance - focusDistance, Space.Self);
                focusPoint.Translate(0, 0, value.FocusDistance - focusDistance, Space.Self);
                focusDistance = value.FocusDistance;
                focusTestPoint.position = value.FocusPointPosition;
                focusTestPoint.eulerAngles = new Vector3(value.FocusPointRotation.x, value.FocusPointRotation.y, 0);
            }
        }

        private void ScreenDrag(Vector2 _deltaPos, Vector2 _deltaRot)
        {
            //旋转
            focusTestPoint.Rotate(Vector3.up, _deltaRot.x * viewConfigParameter.RotatePower, Space.World);
            focusTestPoint.Rotate(Vector3.right, -_deltaRot.y * viewConfigParameter.RotatePower, Space.Self);
            //移动
            focusTestPoint.Translate(-_deltaPos.x * viewConfigParameter.MovePower, -_deltaPos.y * viewConfigParameter.MovePower, 0, Space.Self);
        }
        private void Zoom(float _deltaSize)
        {
            if (focusDistance == 0)
            {
                focusTestPoint.Translate(0, 0, _deltaSize * viewConfigParameter.MovePower * 20, Space.Self);
                depthOfField.focusDistance.value = defaultValue.FocusDistance;
            }
            else if (focusDistance > _deltaSize * viewConfigParameter.MovePower * 20)
            {
                focusDistance -= _deltaSize * viewConfigParameter.MovePower * 20;
                cameraTestPoint.localPosition = new Vector3(0, 0, -focusDistance);
                depthOfField.focusDistance.value = focusDistance;
            }
            else
            {
                cameraTestPoint.localPosition = Vector3.zero;
                focusTestPoint.Translate(0, 0, focusDistance - _deltaSize * viewConfigParameter.MovePower * 20, Space.Self);
                focusDistance = 0;
                depthOfField.focusDistance.value = defaultValue.FocusDistance;
            }
        }
        private void ScreenClick(GameObject _clickObject, Vector3 _clickPoint)
        {
            if (_clickObject)
            {
                viewCamera.transform.localPosition = Vector3.zero;
                cameraTestPoint.localPosition = Vector3.zero;
                focusTestPoint.Translate(0, 0, -focusDistance, Space.Self);
                focusPoint.Translate(0, 0, -focusDistance, Space.Self);
                focusTestPoint.LookAt(_clickPoint);
                focusDistance = Vector3.Distance(_clickPoint, cameraTestPoint.position);
                viewCamera.transform.localPosition = new Vector3(0, 0, -focusDistance);
                cameraTestPoint.localPosition = new Vector3(0, 0, -focusDistance);
                focusPoint.Translate(0, 0, focusDistance, Space.Self);
                focusTestPoint.Translate(0, 0, focusDistance, Space.Self);
                depthOfField.focusDistance.value = focusDistance;
            }
            else
            {
                ViewValue = defaultValue;
            }
        }

        private void LandCheck()
        {
            //RaycastHit hit;
            //Physics.Raycast();
        }
        private void SmoothDampFollow()
        {
            focusPoint.rotation = Quaternion.Slerp(focusPoint.rotation, focusTestPoint.rotation, Time.deltaTime * viewConfigParameter.SmoothDampPower);
            focusPoint.position = Vector3.Lerp(focusPoint.position, focusTestPoint.position, Time.deltaTime * viewConfigParameter.SmoothDampPower);
            viewCamera.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, viewCamera.transform.localPosition.z), new Vector3(0, 0, cameraTestPoint.localPosition.z), Time.deltaTime * viewConfigParameter.SmoothDampPower);

        }
    }

    /// <summary>
    ///  角色视角配置参数
    /// </summary>
    [Serializable]
    public struct PlayerViewOption
    {
        public float RotatePower;
        public float MovePower;
        public float SmoothDampPower;
        public bool LandCheck;
        public LayerMask LandLayer;
    }

    /// <summary>
    ///  角色视角值
    /// </summary>
    [Serializable]
    public struct PlayerViewValue
    {
        public Vector3 FocusPointPosition;
        public Vector3 FocusPointRotation;
        public float FocusDistance;
    }
}