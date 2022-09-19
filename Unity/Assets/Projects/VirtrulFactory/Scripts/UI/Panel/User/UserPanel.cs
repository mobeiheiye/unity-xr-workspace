using CUI.UI;
using CUI.WebRequest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using VirtrulFactory.Manager;

namespace VirtrulFactory.UI
{
    public class UserPanel : UIStatePanel
    {
        [Header("UserPanel")]
        [SerializeField] private Image userIcon;
        [SerializeField] private Text txt_UserName;
        [SerializeField] private Toggle toggle_Login;
        [SerializeField] private EventTrigger toggle_Logout;
        [SerializeField] private float loginCheckTime = 10;

        private Sprite defaultIcon;

        protected override void Init()
        {
            base.Init();
            defaultIcon = userIcon.sprite;
            toggle_Login.gameObject.SetActive(true);
            toggle_Logout.gameObject.SetActive(false);

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(delegate
            {
                UserManager.Logout();
            });
            toggle_Logout.triggers.Add(entry);

            UserManager.OnLogin += delegate (bool succeed, string message)
              {
                  if (succeed)
                  {
                      ResourceManager.GetImage(UserManager.UserServerUrl + '/' + UserManager.CurrentUser.IconPath, GetIconCallBack);
                      txt_UserName.text = UserManager.CurrentUser.Name;
                      toggle_Login.gameObject.SetActive(false);
                      toggle_Logout.gameObject.SetActive(true);
                      StartCoroutine("LoginCheck");
                  }
              };
            UserManager.OnLogout += delegate
              {
                  userIcon.sprite = defaultIcon;
                  txt_UserName.text = null;
                  toggle_Login.gameObject.SetActive(true);
                  toggle_Logout.gameObject.SetActive(false);
                  StopCoroutine("LoginCheck");
              };
        }

        private void GetIconCallBack(string id, Sprite icon)
        {
            if (UserManager.CurrentUser.IconPath == id)
            {
                userIcon.sprite = icon;
            }
        }
        private IEnumerator LoginCheck()
        {
            while (true)
            {
                UserManager.IsLogin();
                yield return new WaitForSeconds(loginCheckTime);
            }
        }
    }
}