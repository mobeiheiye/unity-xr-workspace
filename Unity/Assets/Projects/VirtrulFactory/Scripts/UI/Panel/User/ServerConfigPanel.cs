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
    public class ServerConfigPanel : UIStatePanel
    {
        [Header("ServerConfigPanel")]
        [SerializeField] private InputField input_ServerAddress;
        [SerializeField] private InputField input_ServerPort;
        [SerializeField] private Button btn_ConfigMakeSure;

        protected override void Init()
        {
            base.Init();
            input_ServerAddress.text = UserManager.UserServerAddress;
            input_ServerPort.text = UserManager.UserServerPort;
            btn_ConfigMakeSure.onClick.AddListener(delegate
            {
                if (string.IsNullOrEmpty(input_ServerAddress.text)) input_ServerAddress.text = UserManager.UserServerAddress;
                if (string.IsNullOrEmpty(input_ServerPort.text)) input_ServerPort.text = UserManager.UserServerPort;
                UserManager.ConfigUserServer(input_ServerAddress.text, input_ServerPort.text);
                Loading(string.Empty);
            });
            UserManager.OnUserServerConfig += delegate (bool succeed, string message)
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
        }
    }
}