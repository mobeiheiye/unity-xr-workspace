using CUI.Shape;
using UnityEngine;
using UnityEngine.UI;

namespace CUI.UI
{
    /// <summary>
    /// 文字标签
    /// </summary>
    public class TextTag : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rect_BG;
        [SerializeField]
        private BoxCollider collider_BG;
        [SerializeField]
        private InputField input_Tag;
        [SerializeField]
        private int minWidth;
        [SerializeField]
        private int maxWidth;
        [SerializeField]
        private int minHeight;
        [SerializeField]
        private int maxHeight;
        [SerializeField]
        private int frameWidth;

        [SerializeField]
        private Transform[] anchors;

        [SerializeField]
        private float curveWidth;
        [SerializeField]
        private int curveClipCount;
        [SerializeField]
        private ThreeDCurveLine curve;
        private Transform currentAnchor = null;

        public string Text
        {
            get { return input_Tag.text; }
            set
            {
                input_Tag.text = value;

            }
        }

        [SerializeField]
        private Transform target = null;
        public Transform Target
        {
            get { return target; }
            set
            {
                target = value;
                if (target)
                {
                    currentAnchor = GetNearestAnchors();
                    curve.Init(curveClipCount, curveWidth, currentAnchor, target);
                    curve.enabled = true;
                }
                else
                {
                    curve.enabled = false;
                }
            }
        }

        private void Start()
        {
            //Target = GameObject.Find("TestTagAnchor").transform;
        }

        private void Update()
        {
            if (target)
            {
                if (GetNearestAnchors() != currentAnchor)
                {
                    currentAnchor = GetNearestAnchors();
                    curve.Init(curveClipCount, curveWidth, currentAnchor, target);
                }
            }
            UpdateInputFieldSize();
            UpdateForward();
        }

        private Transform GetNearestAnchors()
        {
            Transform _anchor = null;
            float _minDistance = Mathf.Infinity;
            foreach (var item in anchors)
            {
                float _distance = Vector3.Distance(target.position, item.position);
                if (_distance < _minDistance)
                {
                    _anchor = item;
                    _minDistance = _distance;
                }
            }
            return _anchor;
        }

        private void UpdateInputFieldSize()
        {
            float _width = frameWidth * 2 + input_Tag.textComponent.preferredWidth;
            float _height = frameWidth * 2 + input_Tag.textComponent.preferredHeight;
            _width = Mathf.Clamp(_width, minWidth, maxWidth);
            _height = Mathf.Clamp(_height, minHeight, maxHeight);
            rect_BG.sizeDelta = new Vector2(_width, _height);
            collider_BG.size = new Vector3(_width, _height, 1);
            collider_BG.center = new Vector3(_width / 2, -_height / 2, 0);
        }

        private void UpdateForward()
        {
            if (Camera.main)
            {
                transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            }
        }
    }
}