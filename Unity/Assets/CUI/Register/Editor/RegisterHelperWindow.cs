using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace CUI.Register
{
    /// <summary>
    /// 注册机编辑器
    /// </summary>
    public class RegisterHelperWindow : EditorWindow
    {
        private enum CompanyNameEnum
        {
            DsIdeal
        }
        private enum ProductNameEnum
        {
            VirtualFactory_机械加工工厂,
            VirtualFactory_汽修工厂,
            VirtualFactory_畜禽实验中心
        }

        private string[] companyNames = new[] { "DsIdeal" };
        private string[] productNames = new[] { "VirtualFactory_机械加工工厂", "VirtualFactory_汽修工厂", "VirtualFactory_畜禽实验中心" };
        private CompanyNameEnum companyName;
        private ProductNameEnum productName;
        private string macCode = string.Empty;
        private string registCode = string.Empty;

        private RegisterHelperWindow()
        {
            this.titleContent = new GUIContent("单机版注册");
        }
        [MenuItem("COMMON/单机版注册")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow<RegisterHelperWindow>();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginVertical(GUI.skin.box);
            companyName = (CompanyNameEnum)EditorGUILayout.EnumPopup("公司名称", companyName);
            productName = (ProductNameEnum)EditorGUILayout.EnumPopup("产品名称", productName);
            GUILayout.EndVertical();
            GUILayout.Label("Mac地址");
            macCode = EditorGUILayout.TextField(macCode);
            if (GUILayout.Button("获取注册码"))
            {
                GUILayout.Label("注册码");
                registCode = GetMD5(companyName.ToString() + macCode + productName.ToString());
            }
            EditorGUILayout.TextField(registCode);
            GUILayout.EndVertical();
        }

        private static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
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

