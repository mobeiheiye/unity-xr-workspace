using CUI.WebRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace VirtrulFactory
{
    public class UserManager
    {
        #region Server
        private const string defaultUserServerAddress = "10.10.22.126";
        private const string defaultUserServerPort = "9955";
        public static string UserServerAddress
        {
            get
            {
                return string.IsNullOrEmpty(PlayerPrefs.GetString("User/ServerAddress")) ?
                    defaultUserServerAddress :
                    PlayerPrefs.GetString("User/ServerAddress");
            }
            private set
            {
                if (string.IsNullOrEmpty(value)) PlayerPrefs.DeleteKey("User/ServerAddress");
                else PlayerPrefs.SetString("User/ServerAddress", value);
            }
        }
        public static string UserServerPort
        {
            get
            {
                return string.IsNullOrEmpty(PlayerPrefs.GetString("User/ServerPort")) ?
                    defaultUserServerPort :
                    PlayerPrefs.GetString("User/ServerPort");
            }
            private set
            {
                if (string.IsNullOrEmpty(value)) { PlayerPrefs.DeleteKey("User/ServerPort"); }
                else PlayerPrefs.SetString("User/ServerPort", value);
            }
        }
        public static string UserServerUrl
        {
            get
            {
                return UserServerAddress + ':' + UserServerPort;
            }
        }

        public static UnityAction<bool, string> OnUserServerConfig;
        public static void ConfigUserServer(string address, string port)
        {
            string url = "http://" + address + ':' + port + "/user/testLink?";
            UserServerAddress = address;
            UserServerPort = port;
            WebRequestManager.Instence.Get(url, url, ConfigUserServerRequestCallBack);
        }
        private static void ConfigUserServerRequestCallBack(string requestID, string data, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                OnUserServerConfig?.Invoke(true, data);
            }
            else
            {
                UserServerAddress = string.Empty;
                UserServerPort = string.Empty;
                OnUserServerConfig?.Invoke(false, "设置失败:" + error);
            }
        }
        #endregion

        #region User
        public static User CurrentUser { get; private set; }
        public static string Token
        {
            get { return PlayerPrefs.GetString("User/Token"); }
            private set
            {
                if (string.IsNullOrEmpty(value)) PlayerPrefs.DeleteKey("User/Token");
                else PlayerPrefs.SetString("User/Token", value);
            }
        }
        public static bool HasPermission(string permissionName)
        {
            if (CurrentUser == null) return false;
            if (CurrentUser.UserRole == null) return false;
            return Permission.GetPermissionNames(CurrentUser.UserRole.PermissionValue).Contains(permissionName);
        }

        public static UnityAction<bool, string> OnLogin;
        public static void Login()
        {
            string url = "http://" + UserServerUrl + "/user/autologin?";
            WWWForm form = new WWWForm();
            form.AddField("token", Token);
            form.AddField("equipment", SystemInfo.deviceUniqueIdentifier);
            WebRequestManager.Instence.Post("Login", url, form, LoginRequestCallBack);
        }
        public static void Login(string acc, string psd)
        {
            string url = "http://" + UserServerUrl + "/server/zhijiaomofang/api/login?";
            WWWForm form = new WWWForm();
            form.AddField("account", acc);
            form.AddField("password", psd);
            form.AddField("equipment", SystemInfo.deviceUniqueIdentifier);
            WebRequestManager.Instence.Post("Login", url, form, LoginRequestCallBack);
        }
        private static void LoginRequestCallBack(string requestID, string data, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                CurrentUser = JsonUtility.FromJson<User>(data);
                OnLogin?.Invoke(true, "");
            }
            else
            {
                CurrentUser = null;
                OnLogin?.Invoke(false, error);
            }
        }

        public static UnityAction OnLogout;
        public static void Logout()
        {
            Debug.Log("OnLogout");
            CurrentUser = null;
            Token = string.Empty;
            OnLogout?.Invoke();
        }

        public static void IsLogin()
        {
            string url = "http://" + UserServerUrl + "/user/isLogin?";
            WWWForm form = new WWWForm();
            form.AddField("token", Token);
            WebRequestManager.Instence.Post("IsLogin", url, form, IsLoginRequestCallBack);
        }
        public static void IsLoginRequestCallBack(string requestID, string data, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                if (!bool.Parse(data))
                {
                    Logout();
                }
            }
            else
            {
                Logout();
            }
        }
        #endregion

        #region UserMessage
        #endregion
    }
}