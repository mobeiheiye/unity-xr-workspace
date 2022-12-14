using CUI.WebRequest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VirtrulFactory
{
    public class AssetManager
    {
        #region AssetServer
        private const string defaultAssetServerAddress = "10.10.22.126";
        private const string defaultAssetServerPort = "9955";
        public static string AssetServerAddress
        {
            get
            {
                return string.IsNullOrEmpty(PlayerPrefs.GetString("User/AssetAddress")) ?
                    defaultAssetServerAddress : PlayerPrefs.GetString("User/AssetAddress");
            }
            private set
            {
                if (string.IsNullOrEmpty(value)) PlayerPrefs.DeleteKey("User/AssetAddress");
                else PlayerPrefs.SetString("User/AssetAddress", value);
            }
        }
        public static string AssetServerPort
        {
            get
            {
                return string.IsNullOrEmpty(PlayerPrefs.GetString("User/AssetPort")) ?
                    defaultAssetServerPort : PlayerPrefs.GetString("User/AssetPort");
            }
            private set
            {
                if (string.IsNullOrEmpty(value)) { PlayerPrefs.DeleteKey("User/AssetPort"); }
                else PlayerPrefs.SetString("User/AssetPort", value);
            }
        }
        public static string AssetServerUrl
        {
            get
            {
                return AssetServerAddress + ':' + AssetServerPort;
            }
        }

        public static UnityAction<bool, string> OnAssetServerConfig;
        public static void ConfigAssetServer(string address, string port)
        {
            string url = "http://" + address + ':' + port + "/user/testLink?";
            AssetServerAddress = address;
            AssetServerPort = port;
            WebRequestManager.Instence.Get(url, url, ConfigAssetServerRequestCallBack);
        }
        private static void ConfigAssetServerRequestCallBack(string requestID, string data, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                AssetServerAddress = string.Empty;
                AssetServerPort = string.Empty;
                OnAssetServerConfig?.Invoke(false, "设置失败:" + error);
            }
            else
            {
                OnAssetServerConfig?.Invoke(true, data);
            }
        }
        #endregion

        private static Dictionary<string, Scene> dic_Factorys = new Dictionary<string, Scene>();


        private static Dictionary<string, ModelBundle> dic_Models = new Dictionary<string, ModelBundle>();


    }

}