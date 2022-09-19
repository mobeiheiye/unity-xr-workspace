using UnityEngine;
using System.Collections;

namespace CUI.Shape
{
    [ExecuteInEditMode]
    public class ThreeDCurveLine : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] private Transform startAnchor;
        [SerializeField] private Transform endAnchor;
        [SerializeField] [Range(10, 30)] int clipCount = 20;
        [SerializeField] float width = 1;

        void Start()
        {
            if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
            Init(clipCount, width, startAnchor, endAnchor);
        }
        public void Init(int _clipCount, float _width, Transform _startAnchor, Transform _endAnchor)
        {
            width = _width;
            clipCount = _clipCount;
            startAnchor = _startAnchor;
            endAnchor = _endAnchor;
        }
        void OnEnable()
        {
            lineRenderer.enabled = true;
        }
        void OnDisable()
        {
            lineRenderer.enabled = false;
        }
        void Update()
        {
            if (startAnchor && endAnchor)
            {
                Curve.DrawSimpleCurve(startAnchor.position, startAnchor.up, endAnchor.position, endAnchor.up, lineRenderer, clipCount, width);
            }
        }
    }
}