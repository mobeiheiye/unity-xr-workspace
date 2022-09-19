using UnityEngine;

namespace CUI.UI
{
    /// <summary>
    /// 摄像机跟随鼠标晃动
    /// </summary>
    public class PanWithMouse : MonoBehaviour
    {
        [SerializeField] private Vector2 degrees = new Vector2(5f, 3f);
        [SerializeField] private float range = 1f;

        private Transform mTrans;
        private Quaternion mStart;
        private Vector2 mRot = Vector2.zero;

        private void Start()
        {
            mTrans = transform;
            mStart = mTrans.localRotation;
        }

        private void FixedUpdate()
        {
            float delta = Time.deltaTime;
            Vector3 pos = Input.mousePosition;

            float halfWidth = Screen.width * 0.5f;
            float halfHeight = Screen.height * 0.5f;
            if (range < 0.1f)
            {
                range = 0.1f;
            }

            float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth / range, -1f, 1f);
            float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight / range, -1f, 1f);
            mRot = Vector2.Lerp(mRot, new Vector2(x, y), delta * 5f);

            mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * degrees.y, mRot.x * degrees.x, 0f);
        }
    }
}