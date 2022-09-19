using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CUI.UI
{
    /// <summary>
    /// ugui state panel
    /// </summary>
    public class UIStatePanel : MonoBehaviour
    {
        [Header("UIPanel")]
        [SerializeField] protected Button btn_Open;
        [SerializeField] protected Button btn_Back;
        [SerializeField] protected KeyCode key_Back;
        [SerializeField] protected Toggle toggle_Switch;
        [SerializeField] protected KeyCode key_Switch;
        [SerializeField] protected bool startEnable = false;

        private Animator mStateAnimator = null;
        private void Start()
        {
            Init();
        }
        private void Update()
        {
            PanelUpdate();
        }

        private PanelState CurrentState
        {
            get
            {
                return (PanelState)Enum.ToObject(typeof(PanelState), mStateAnimator.GetInteger("PanelState"));
            }
            set
            {
                mStateAnimator.SetInteger("PanelState", (int)value);
            }
        }

        protected virtual void Init()
        {
            mStateAnimator = GetComponent<Animator>();

            if (btn_Open)
            {
                btn_Open.onClick.AddListener(Show);
            }

            if (btn_Back)
            {
                btn_Back.onClick.AddListener(Hide);
            }

            if (toggle_Switch)
            {
                toggle_Switch.onValueChanged.AddListener(
                    delegate (bool _isOn)
                    {
                        if (_isOn) Show();
                        else Hide();
                    });
            }

            CurrentState = startEnable ? PanelState.Default : PanelState.Hide;
        }
        protected virtual void PanelUpdate()
        {
            if (key_Back != KeyCode.None && Input.GetKeyUp(key_Back))
            {
                CurrentState = PanelState.Hide;
            }
            if (key_Switch != KeyCode.None && Input.GetKeyUp(key_Switch))
            {
                CurrentState = CurrentState == PanelState.Hide ? PanelState.Default : PanelState.Hide;
            }
        }
        protected virtual void Log(string message)
        {
            Debug.Log(this.GetType().Name + "=>" + message);
        }

        public virtual void Show()
        {
            foreach (var item in GetComponentsInChildren<Collider>())
            {
                item.enabled = true;
            }
            CurrentState = PanelState.Default;
        }
        public virtual void Hide()
        {
            foreach (var item in GetComponentsInChildren<Collider>())
            {
                item.enabled = false;
            }
            CurrentState = PanelState.Hide;
        }
        public virtual void Shake()
        {
            CurrentState = PanelState.Shaking;
        }
        public virtual void Loading(string msg)
        {
            CurrentState = PanelState.Loading;
        }
        public virtual void Error()
        {
            StartCoroutine("SetError");
        }
        public virtual void Flash()
        {
            StartCoroutine("SetFlash");
        }

        public void ShowAt(Vector3 pos)
        {
            transform.position = pos;
            Show();
        }
        public void ShowAt(Vector3 pos, Vector3 euler)
        {
            transform.position = pos;
            transform.eulerAngles = euler;
            Show();
        }

        private IEnumerator SetError()
        {
            mStateAnimator.SetBool("Error", true);
            yield return new WaitForSeconds(0.5f);
            mStateAnimator.SetBool("Error", false);
        }
        private IEnumerator SetFlash()
        {
            mStateAnimator.SetBool("Flash", true);
            yield return new WaitForSeconds(0.5f);
            mStateAnimator.SetBool("Flash", false);
        }
    }

    /// <summary>
    /// panel states
    /// </summary>
    [Serializable]
    public enum PanelState
    {
        Default,
        Hide,
        Loading,
        Shaking
    }
}