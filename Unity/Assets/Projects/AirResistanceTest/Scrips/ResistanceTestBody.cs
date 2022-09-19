using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResistanceTest
{
    public class ResistanceTestBody : MonoBehaviour
    {
        [SerializeField] private ResistanceTestModel m_ResistanceTest;
        [SerializeField] private float m_Time = 0;
        [SerializeField] private float m_Velocity = 0;
        [SerializeField] private float m_FinalVelocity = 0;

        [SerializeField] private Transform m_Model;
        [SerializeField] private Transform m_Parachute;
        [SerializeField] private Renderer m_Renderer;

        [SerializeField] private InputField input_Mass;
        [SerializeField] private InputField input_Coefficient;
        [SerializeField] private Text txt_Name;
        [SerializeField] private Text txt_Time;
        [SerializeField] private Text txt_Height;
        [SerializeField] private Text txt_Velocity;
        [SerializeField] private Text txt_FinalVelocity;

        private void Start()
        {
            if (input_Mass)
            {
                input_Mass.text = m_ResistanceTest.Mass.ToString();
                input_Mass.onValueChanged.AddListener(delegate (string value)
                {
                    float fValue = float.Parse(value);
                    fValue = Mathf.Clamp(fValue, 1, 100);
                    m_ResistanceTest.Mass = fValue;
                    input_Mass.text = fValue.ToString();
                    m_Renderer.material.color = new Color(1 - fValue / 100, 1 - fValue / 100, 1 - fValue / 100);
                    Init();
                });
            }
            if (input_Coefficient)
            {
                input_Coefficient.text = m_ResistanceTest.Coefficient.ToString();
                input_Coefficient.onValueChanged.AddListener(delegate (string value)
                {
                    float fValue = float.Parse(value);
                    fValue = Mathf.Clamp(fValue, 1, 10);
                    m_ResistanceTest.Coefficient = fValue;
                    input_Coefficient.text = fValue.ToString();
                    m_Parachute.localScale = Vector3.one * 50f * (1 + fValue / 10);
                    Init();
                });
            }
        }

        public void Init()
        {
            Init(m_Model.position);
        }
        public void Init(Vector3 startPos)
        {
            if (input_Mass) input_Mass.readOnly = false;
            if (input_Coefficient) input_Coefficient.readOnly = false;
            if (txt_Name)
            {
                txt_Name.text = name;
            }
            m_Time = 0;
            m_Model.position = startPos;
            m_Velocity = 0;
            UpdateUI();

            m_FinalVelocity = m_ResistanceTest.GetFinalVelocity();
            if (txt_FinalVelocity) txt_FinalVelocity.text = m_FinalVelocity.ToString();

            if (input_Mass) input_Mass.text = m_ResistanceTest.Mass.ToString();
            m_Renderer.material.color = new Color(1 - m_ResistanceTest.Mass / 10,
              1 - m_ResistanceTest.Mass / 10,
              1 - m_ResistanceTest.Mass / 10);

            if (input_Coefficient) input_Coefficient.text = m_ResistanceTest.Coefficient.ToString();
            m_Parachute.localScale = Vector3.one * 50f * (1 + m_ResistanceTest.Coefficient / 10);
        }

        public void StartTest()
        {
            StartCoroutine("Test");
        }

        public void StopTest()
        {
            StopAllCoroutines();
        }

        private void UpdateUI()
        {
            if (txt_Time) txt_Time.text = m_Time.ToString();
            if (txt_Height) txt_Height.text = m_Model.position.y.ToString();
            if (txt_Velocity) txt_Velocity.text = m_Velocity.ToString();
        }

        IEnumerator Test()
        {
            if (input_Mass) input_Mass.readOnly = true;
            if (input_Coefficient) input_Coefficient.readOnly = true;
            while (m_Model.position.y > 0)
            {
                yield return null;
                m_Time += Time.deltaTime;
                m_Velocity = m_ResistanceTest.GetTestVelocity(m_Time);
                m_Model.position += new Vector3(0, -m_Velocity * Time.deltaTime, 0);
                UpdateUI();
            }
            Vector3 finalPos = m_Model.position;
            finalPos.y = 0;
            m_Model.position = finalPos;
            UpdateUI();
            if (input_Mass) input_Mass.readOnly = false;
            if (input_Coefficient) input_Coefficient.readOnly = false;
        }

        private void Update()
        {
        }
    }

    [Serializable]
    public class ResistanceTestModel
    {
        public static float Gravity = 9.8f;

        public float Mass = 1;
        public float Coefficient = 1;

        public float GetTestVelocity(float time)
        {
            return Mass * Gravity / Coefficient * (1 - Mathf.Pow((float)Math.E, -Coefficient * time / Mass));
        }
        public float GetFinalVelocity()
        {
            return GetTestVelocity(Mathf.Infinity);
        }
    }
}