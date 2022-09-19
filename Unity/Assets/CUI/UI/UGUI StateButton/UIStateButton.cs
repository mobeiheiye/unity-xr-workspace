using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CUI.UI
{
    /// <summary>
    /// ugui state button
    /// </summary>
    public class UIStateButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private bool m_HasCheckState = false;

        public UnityEvent OnClick;
        public UnityEvent<bool> OnSelect;

        private Animator m_Animator;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
        }
        private void Update()
        {

        }
        private void OnEnable()
        {
        }
        private void OnDestroy()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClick != null) OnClick.Invoke();
            if (m_HasCheckState)
            {
                bool isSelect = m_Animator.GetBool("IsSelect");
                m_Animator.SetBool("IsSelect", !isSelect);
                if (OnSelect != null) OnSelect.Invoke(isSelect);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_Animator.SetBool("IsHover", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_Animator.SetBool("IsHover", false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_Animator.SetBool("IsPress", true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_Animator.SetBool("IsPress", false);
        }
    }

}