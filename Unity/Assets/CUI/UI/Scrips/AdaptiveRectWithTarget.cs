using UnityEngine;

namespace CUI.UI
{
    /// <summary>
    /// 根据目标Rect自适应调整自身Rect
    /// </summary>
    public class AdaptiveRectWithTarget : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

        private Vector2 offset = Vector2.zero;
        private RectTransform self;

        private void Start()
        {
            self = transform as RectTransform;
            offset = self.sizeDelta - target.sizeDelta;
        }

        private void Update()
        {
            if (target && self) { self.sizeDelta = target.sizeDelta + offset; }
        }
    }
}