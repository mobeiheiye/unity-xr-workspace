using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CUI.QRCode;
using System.Linq;

namespace VirtrulFactory
{
    public class SceneManager
    {
        public static string ServerUrl;

        public static User CurrentUser;

        public static Dictionary<string, User> Users = new Dictionary<string, User>();
        public static Dictionary<string, Asset> Resources = new Dictionary<string, Asset>();

        public static UnityAction OnLogin;
        public static void Login()
        {
            if (CurrentUser != null && OnLogin != null) OnLogin();
        }

        public static UnityAction OnLogout;
        public static void Logout()
        {
            if (OnLogout != null) OnLogout();
        }

        public static void SelectResource(string resourceID)
        {
            PlayResource("BookResourceSelect", resourceID);
        }


        public static UnityAction<string, string> OnPlayVideo;
        public static UnityAction<string, string> OnPlayModel;
        public static void PlayResource(string playID, string resourceID)
        {
           
        }

        public static UnityAction<string, float, string> OnFinishResource;
        public static void FinishResource(string playID, float duration, string resourceID)
        {
            OnFinishResource?.Invoke(playID, duration, resourceID);
        }

        public static UnityAction<string, string> OnPlayVideoExit;
        public static void PlayVideoExit(string playID, string resourceID)
        {
            OnPlayVideoExit?.Invoke(playID, resourceID);
        }

    }
}