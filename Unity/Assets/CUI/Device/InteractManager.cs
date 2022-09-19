using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

namespace CUI.Device
{
    public class InteractManager : MonoBehaviour
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool IsMobile();
#endif

        public static GameObject CurrentHover { get; private set; }
        public static UnityAction<GameObject, bool> OnHoverChange;

        public static GameObject CurrentDrag { get; private set; }
        public static UnityAction<GameObject, Vector3, Quaternion> OnDragStart;
        public static UnityAction<GameObject, Vector3, Quaternion> OnDrag;
        public static UnityAction<GameObject, Vector3, Quaternion> OnDragEnd;

        public static UnityAction<Vector2, Vector2> OnScreenDragStart;
        public static UnityAction<Vector2, Vector2> OnScreenDrag;
        public static UnityAction<Vector2, Vector2> OnScreenDragEnd;

        public static UnityAction<GameObject, Vector3> OnClick;
        public static UnityAction<GameObject, Vector3> OnDoubleClick;

        public static UnityAction<float> OnZoomStart;
        public static UnityAction<float> OnZoom;
        public static UnityAction<float> OnZoomEnd;

        [SerializeField] private Camera viewCamera;
        [SerializeField] private LayerMask dragLayer;
        [SerializeField] private bool isLog = false;

        //当前交互是否在拖拽物体
        private bool isDragObject = false;
        //DragStart时指针射线长度
        private float dragCursorDeepth = 0;
        //DragStart时物体和指针的相对位置
        private Vector3 pickPosOffset = Vector3.zero;
        //DragStart时物体和指针的相对旋转
        //private Quaternion pickRotOffset = new Quaternion();

        //当前交互是否在拖拽镜头
        private bool isDragScreen = false;

        //当前交互是否在点击
        //private bool isClick = false;
        //两次抬起时间小于doubleClickDuration则判断为DoubleClick
        private readonly float doubleClickDelay = 0.3f;
        //抬起的时间标记
        private float upTime = 0;

        //当前交互是否在缩放
        private bool isZoom = false;

        private void Start()
        {
            if (isLog)
            {
                OnDragStart += delegate (GameObject target, Vector3 pos, Quaternion rot) { Debug.Log("InteractManager :: [OnDragStart] :<Target>" + target + "<Pos>" + pos.ToString() + "<Rot>" + rot.ToString()); };
                OnDrag += delegate (GameObject target, Vector3 pos, Quaternion rot) { Debug.Log("InteractManager :: [OnDrag] :<Target>" + target + "<Pos>" + pos.ToString() + "<Rot>" + rot.ToString()); };
                OnDragEnd += delegate (GameObject target, Vector3 pos, Quaternion rot) { Debug.Log("InteractManager :: [OnDragEnd] :<Target>" + target + "<Pos>" + pos.ToString() + "<Rot>" + rot.ToString()); };
                OnScreenDragStart += delegate (Vector2 pos, Vector2 rot) { Debug.Log("InteractManager :: [OnScreenDragStart] :<Pos>" + pos.ToString() + "<Rot>" + rot.ToString()); };
                OnScreenDrag += delegate (Vector2 pos, Vector2 rot) { Debug.Log("InteractManager :: [OnScreenDrag] :<Pos>" + pos.ToString() + "<Rot>" + rot.ToString()); };
                OnScreenDragEnd += delegate (Vector2 pos, Vector2 rot) { Debug.Log("InteractManager :: [OnScreenDragEnd] :<Pos>" + pos.ToString() + "<Rot>" + rot.ToString()); };
                OnClick += delegate (GameObject target, Vector3 pos) { Debug.Log("InteractManager :: [OnClick] :<Target>" + target + "<Pos>" + pos.ToString()); };
                OnDoubleClick += delegate (GameObject target, Vector3 pos) { Debug.Log("InteractManager :: [OnDoubleClick] :<Target>" + target + "<Pos>" + pos.ToString()); };
                OnZoomStart += delegate (float scrollWheel) { Debug.Log("InteractManager :: [OnZoomStart] :<ScrollWheel>" + scrollWheel.ToString()); };
                OnZoom += delegate (float scrollWheel) { Debug.Log("InteractManager :: [OnZoom] :<ScrollWheel>" + scrollWheel.ToString()); };
                OnZoomEnd += delegate (float scrollWheel) { Debug.Log("InteractManager :: [OnZoomEnd] :<ScrollWheel>" + scrollWheel.ToString()); };
            }
        }
        private void LateUpdate()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            UpdateStandaloneInput();
#elif UNITY_WEBGL
            if (IsMobile())
            {
                UpdateTouchInput();
            }
            else
            {
                UpdateStandaloneInput();
            }
#else
            UpdateTouchInput();
#endif
        }

        private void UpdateStandaloneInput()
        {
            Ray _ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Input.GetMouseButtonDown(0))
            {
                //物体拖动起始（排除UI层）。发射线获取拖动的物体，并获取光标和物体的相对位置
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, dragLayer))
                    {
                        CurrentDrag = _hit.transform.gameObject;
                        pickPosOffset = _hit.point - _hit.transform.position;
                        Plane _plane = new Plane(viewCamera.transform.forward, viewCamera.transform.position);
                        dragCursorDeepth = _plane.GetDistanceToPoint(_hit.point);
                        if (OnDragStart != null) OnDragStart(CurrentDrag, Vector3.zero, new Quaternion(0, 0, 0, 0));
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                //物体拖动
                if (CurrentDrag)
                {
                    Vector3 _dragPos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragCursorDeepth)) - pickPosOffset - CurrentDrag.transform.position;
                    if (OnDrag != null) OnDrag(CurrentDrag, _dragPos, new Quaternion(0, 0, 0, 0));
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //物体拖动结束。置空拖动物体
                //点击触发（排除UI层）。每次Up触发单击，并记录单击时间；两次Up时间间隔少于双击触发间隔，则触发双击
                if (CurrentDrag)
                {
                    Vector3 _dragPos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragCursorDeepth)) - pickPosOffset - CurrentDrag.transform.position;
                    if (OnDragEnd != null) OnDragEnd(CurrentDrag, _dragPos, new Quaternion(0, 0, 0, 0));
                    CurrentDrag = null;
                }
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (Time.time - upTime > doubleClickDelay)
                    {
                        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
                        {
                            if (OnClick != null) OnClick(_hit.transform.gameObject, _hit.point);
                        }
                        else
                        {
                            if (OnClick != null) OnClick(null, Input.mousePosition);
                        }
                        upTime = Time.time;
                    }
                    else
                    {
                        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
                        {
                            if (OnDoubleClick != null) OnDoubleClick(_hit.transform.gameObject, _hit.point);
                        }
                        else
                        {
                            if (OnDoubleClick != null) OnDoubleClick(null, Input.mousePosition);
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //屏幕旋转拖动起始（排除UI层）
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (OnScreenDragStart != null) OnScreenDragStart(Vector2.zero, Vector2.zero);
                    isDragScreen = true;
                }
            }
            else if (Input.GetMouseButton(1))
            {
                //屏幕旋转拖动
                if (isDragScreen)
                {
                    if (OnScreenDrag != null) OnScreenDrag(Vector2.zero, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                //屏幕旋转拖动结束
                if (isDragScreen)
                {
                    if (OnScreenDragEnd != null) OnScreenDragEnd(Vector2.zero, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
                    isDragScreen = false;
                }
            }
            else if (Input.GetMouseButtonDown(2))
            {
                //屏幕拖动起始（排除UI层）
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (OnScreenDragStart != null) OnScreenDragStart(Vector2.zero, Vector2.zero);
                    isDragScreen = true;
                }
            }
            else if (Input.GetMouseButton(2))
            {
                //屏幕拖动
                if (isDragScreen)
                {
                    if (OnScreenDrag != null) OnScreenDrag(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), Vector2.zero);
                }
            }
            else if (Input.GetMouseButtonUp(2))
            {
                //屏幕拖动结束
                if (isDragScreen)
                {
                    if (OnScreenDragEnd != null) OnScreenDragEnd(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), Vector2.zero);
                    isDragScreen = false;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                //屏幕缩放起始（排除UI层）
                //屏幕缩放（排除UI层）由isZoom判断当前是缩放开始还是缩放中
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (isZoom)
                    {
                        if (OnZoom != null) OnZoom(Input.GetAxis("Mouse ScrollWheel"));
                    }
                    else
                    {
                        if (OnZoomStart != null) OnZoomStart(Input.GetAxis("Mouse ScrollWheel"));
                        if (OnZoom != null) OnZoom(Input.GetAxis("Mouse ScrollWheel"));
                        isZoom = true;
                    }
                }
            }
            else
            {
                //缩放停止
                //鼠标悬浮
                if (isZoom)
                {
                    if (OnZoomEnd != null) OnZoomEnd(0);
                    isZoom = false;
                }
                if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
                {
                    if (!_hit.transform.gameObject.Equals(CurrentHover))
                    {
                        if (CurrentHover)
                        {
                            if (OnHoverChange != null) OnHoverChange(CurrentHover, false);
                        }
                        CurrentHover = _hit.transform.gameObject;
                        if (CurrentHover)
                        {
                            if (OnHoverChange != null) OnHoverChange(CurrentHover, true);
                        }
                    }
                }
                else
                {
                    if (CurrentHover)
                    {
                        if (OnHoverChange != null) OnHoverChange(CurrentHover, false);
                        CurrentHover = null;
                    }
                }
            }
        }

        float twoTouchDistance = 0;
        Vector3 twoTouchCenter = Vector3.zero;
        private void UpdateTouchInput()
        {
            Ray _ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Input.touchCount == 1)
            {
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        //物体拖动起始（排除UI层）。发射线获取拖动的物体，并获取光标和物体的相对位置
                        if (!EventSystem.current.IsPointerOverGameObject())
                        {
                            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, dragLayer))
                            {
                                CurrentDrag = _hit.transform.gameObject;
                                pickPosOffset = _hit.point - _hit.transform.position;
                                Plane _plane = new Plane(viewCamera.transform.forward, viewCamera.transform.position);
                                dragCursorDeepth = _plane.GetDistanceToPoint(_hit.point);
                                if (OnDragStart != null) OnDragStart(CurrentDrag, Vector3.zero, new Quaternion(0, 0, 0, 0));
                                isDragObject = true;
                            }
                            else
                            {
                                CurrentDrag = null;
                                if (OnScreenDragStart != null) OnScreenDragStart(Vector2.zero, Vector2.zero);
                                isDragScreen = true;
                            }
                        }
                        break;
                    case TouchPhase.Ended:
                        //物体拖动结束。置空拖动物体
                        //点击触发（排除UI层）。每次Up触发单击，并记录单击时间；两次Up时间间隔少于双击触发间隔，则触发双击
                        if (isDragObject)
                        {
                            Vector3 _dragPos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragCursorDeepth)) - pickPosOffset - CurrentDrag.transform.position;
                            if (OnDragEnd != null) OnDragEnd(CurrentDrag, _dragPos, new Quaternion(0, 0, 0, 0));
                            CurrentDrag = null;
                            isDragObject = false;
                        }
                        else if (isDragScreen)
                        {
                            if (OnScreenDragEnd != null) OnScreenDragEnd(Vector2.zero, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
                            isDragScreen = false;
                        }
                        if (!EventSystem.current.IsPointerOverGameObject())
                        {
                            if (Time.time - upTime > doubleClickDelay)
                            {
                                if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
                                {
                                    if (OnClick != null) OnClick(_hit.transform.gameObject, _hit.point);
                                }
                                else
                                {
                                    if (OnClick != null) OnClick(null, Input.mousePosition);
                                }
                                upTime = Time.time;
                            }
                            else
                            {
                                if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
                                {
                                    if (OnDoubleClick != null) OnDoubleClick(_hit.transform.gameObject, _hit.point);
                                }
                                else
                                {
                                    if (OnDoubleClick != null) OnDoubleClick(null, Input.mousePosition);
                                }
                            }
                        }
                        break;
                    default:
                        //物体拖动
                        if (isDragObject)
                        {
                            if (CurrentDrag)
                            {
                                Vector3 _dragPos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragCursorDeepth)) - pickPosOffset - CurrentDrag.transform.position;
                                if (OnDrag != null) OnDrag(CurrentDrag, _dragPos, new Quaternion(0, 0, 0, 0));
                            }
                        }
                        else if (isDragScreen)
                        {
                            if (OnScreenDrag != null) OnScreenDrag(Vector2.zero, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) / 2);
                        }
                        break;
                }
            }
            else if (Input.touchCount == 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        //开始平移视角
                        twoTouchCenter = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2;
                        if (OnScreenDragStart != null) OnScreenDragStart(Vector2.zero, Vector2.zero);
                        isDragScreen = true;
                        //开始缩放
                        twoTouchDistance = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                        if (OnZoomStart != null) OnZoomStart(0);
                        isZoom = true;
                    }
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended ||
                    Input.GetTouch(1).phase == TouchPhase.Ended ||
                    Input.GetTouch(0).phase == TouchPhase.Canceled ||
                    Input.GetTouch(1).phase == TouchPhase.Canceled)
                {
                    if (isDragScreen)
                    {
                        if (OnScreenDragEnd != null) OnScreenDragEnd(Vector2.zero, Vector2.zero);
                        isDragScreen = false;
                    }
                    if (isZoom)
                    {
                        if (OnZoomEnd != null) OnZoomEnd(0);
                        isZoom = false;
                    }
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved ||
                    Input.GetTouch(1).phase == TouchPhase.Moved ||
                    Input.GetTouch(0).phase == TouchPhase.Stationary ||
                    Input.GetTouch(1).phase == TouchPhase.Stationary)
                {
                    if (isDragScreen)
                    {
                        Vector3 _twoTouchCenter = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2;
                        Vector3 _touchOffset = _twoTouchCenter - twoTouchCenter;
                        if (OnScreenDrag != null) OnScreenDrag(_touchOffset / 50, Vector2.zero);
                        twoTouchCenter = _twoTouchCenter;
                    }
                    if (isZoom)
                    {
                        float _twoTouchDistance = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                        if (OnZoom != null) OnZoom((_twoTouchDistance - twoTouchDistance) / 200);
                        twoTouchDistance = _twoTouchDistance;
                    }
                }
            }
        }
    }
}