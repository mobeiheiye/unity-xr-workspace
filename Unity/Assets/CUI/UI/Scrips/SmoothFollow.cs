using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUI.UI
{
    public class SmoothFollow : MonoBehaviour
    {
        [SerializeField] private Transform m_FollowTarget;
        [SerializeField] private float m_Power;
        [SerializeField] private bool followPosition = true;
        [SerializeField] private bool followRotation = false;

        private bool isUI = false;
        private void Start()
        {
            isUI = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (isUI)
            {
                if (followPosition) transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(m_FollowTarget.position), Time.deltaTime * m_Power);
                if (followRotation) transform.rotation = Quaternion.Slerp(transform.rotation, m_FollowTarget.rotation, Time.deltaTime * m_Power);
            }
            else
            {
                if (followPosition) transform.position = Vector3.Lerp(transform.position, m_FollowTarget.position, Time.deltaTime * m_Power);
                if (followRotation) transform.rotation = Quaternion.Slerp(transform.rotation, m_FollowTarget.rotation, Time.deltaTime * m_Power);
            }
        }
    }

}
