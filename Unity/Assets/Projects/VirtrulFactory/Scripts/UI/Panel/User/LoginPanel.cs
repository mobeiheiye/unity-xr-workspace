using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CUI.UI;
using CUI.WebRequest;
using VirtrulFactory.Manager;

namespace VirtrulFactory.UI
{
    public class LoginPanel : UIStatePanel
    {
        [Header("LoginPanel")]
        [SerializeField] private InputField input_Account;
        [SerializeField] private InputField input_Password;
        [SerializeField] private Button btn_Login;

        protected override void Init()
        {
            base.Init();
            input_Account.text = string.Empty;
            input_Password.text = string.Empty;
            if (!string.IsNullOrEmpty(UserManager.Token)) UserManager.Login();
            btn_Login.onClick.AddListener(delegate
            {
                UserManager.Login(input_Account.text, input_Password.text);
                Loading(string.Empty);
            });
            UserManager.OnLogin += delegate (bool succeed, string message)
            {
                if (succeed)
                {
                    Show();
                }
                else
                {
                    //ErrorLog(message);
                }
            };
            UserManager.OnLogout += delegate
            {
                input_Account.text = string.Empty;
                input_Password.text = string.Empty;
                Show();
            };
        }
    }
}