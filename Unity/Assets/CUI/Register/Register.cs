using UnityEngine;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Collections;
using System;
using System.Linq;

namespace CUI.Register
{
    /// <summary>
    /// 注册机
    /// </summary>
    public class RegisterController : MonoBehaviour
    {
        [SerializeField] private InputField input_InitCode;
        [SerializeField] private InputField input_FinalCode;
        [SerializeField] private GameObject registerPanel;
        [SerializeField] private Button btn_Regist;
        [SerializeField] private GameObject errorPanel;
        [SerializeField] private string nextSceneName;

        [SerializeField] private string registerPath = "SOFTWARE\\IDEALWORKSHOPS\\";
        [SerializeField] private int probationYear = 0;
        [SerializeField] private int probationMonth = 0;
        [SerializeField] private int probationDay = 0;

        private bool probationEnable = false;
        private string probationPath = string.Empty;

        private string macCode;
        private string registerCode;

        private void Start()
        {
            Init();

            registerCode = PlayerPrefs.GetString(registerPath);
            if (CodeCheck(registerCode))
            {
                if (probationEnable)
                {
                    if (PlayerPrefs.GetString(probationPath) == "True")
                    {
                        Application.Quit();
                    }

                    if (OverTimeCheck(probationYear, probationMonth, probationDay))
                    {
                        PlayerPrefs.SetString(probationPath, "True");
                        Application.Quit();
                    }
                }
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                registerPanel.SetActive(true);
                input_InitCode.text = macCode;
            }
        }
        private void Regist()
        {
            string _sourceCode = Application.companyName + macCode + Application.productName;
            if (input_FinalCode.text == GetMD5(_sourceCode))
            {
                PlayerPrefs.SetString(registerPath, GetMD5(_sourceCode));
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                StartCoroutine("Message");
            }
        }
        private IEnumerator Message()
        {
            errorPanel.SetActive(true);
            errorPanel.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(3f);
            errorPanel.SetActive(false);
        }

        private void Init()
        {
            btn_Regist.onClick.AddListener(delegate { Regist(); });
            probationEnable = !(probationYear == 0 && probationMonth == 0 && probationDay == 0);
            if (probationEnable)
            {
                probationPath = registerPath + "\\" + probationYear.ToString() + probationMonth.ToString() + probationDay.ToString();
            }
            macCode = GetMacAddress();
        }
        private bool CodeCheck(string _code)
        {
            string _sourceCode = Application.companyName + macCode + Application.productName;
            return _code == GetMD5(_sourceCode);
        }
        private bool OverTimeCheck(int _year, int _month, int _day)
        {
            if (_year < DateTime.Now.Year)
            {
                return true;
            }
            else if (_year > DateTime.Now.Year)
            {
                return false;
            }

            if (_month < DateTime.Now.Month)
            {
                return true;
            }
            else if (_month > DateTime.Now.Month)
            {
                return false;
            }

            if (_day < DateTime.Now.Day)
            {
                return true;
            }

            return false;
        }
        private string GetMacAddress()
        {
            return (from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()).FirstOrDefault();

            //string physicalAddress = "";
            //NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();
            //foreach (NetworkInterface adaper in nice)
            //{
            //    if (adaper.Description == "en0")
            //    {
            //        physicalAddress = adaper.GetPhysicalAddress().ToString();
            //        break;
            //    }
            //    else
            //    {
            //        physicalAddress = adaper.GetPhysicalAddress().ToString();
            //        if (physicalAddress != "")
            //        {
            //            break;
            //        };
            //    }
            //}
            //return physicalAddress;
        }
        private string GetMD5(string _source)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(_source);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }
            return byte2String;
        }
    }
}