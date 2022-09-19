using System.IO;
using UnityEditor;
using UnityEngine;
using CUI.OpenFileDialog;

namespace CUI.QRCode
{
    /// <summary>
    /// 4位45进制类别码 + 32位16进制GUID 转 45进制二维码
    /// 建议使用2级M纠错码
    /// </summary>
    public class QRCodeBuilderWindow : EditorWindow
    {
        private string type = string.Empty;
        private string guid = string.Empty;
        private int sizeLevel = 1;
        private QRCodeCorrectionLevel correctionLevel = QRCodeCorrectionLevel.L;

        private QRCodeBuilderWindow()
        {
            this.titleContent = new GUIContent("创建二维码");
        }
        [MenuItem("COMMON/创建二维码")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow<QRCodeBuilderWindow>();
        }

        private void OnGUI()
        {
            //创建二维码
            GUILayout.BeginVertical();
            type = EditorGUILayout.TextField("实体类型", type);
            //GUID输入框
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("GUID");
            guid = EditorGUILayout.TextArea(guid);
            GUILayout.EndVertical();
            //尺寸级别
            sizeLevel = EditorGUILayout.IntPopup("尺寸级别", sizeLevel, new[] { "1", "2", "3" }, new[] { 1, 2, 3 });
            //纠错级别
            correctionLevel = (QRCodeCorrectionLevel)EditorGUILayout.EnumPopup("纠错级别", correctionLevel);
            //创建按钮
            if (GUILayout.Button("绘制图片"))
            {
                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(guid))
                {
                    return;
                }

                string path = Application.streamingAssetsPath + "/QRCode/" + type;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string[] guids = guid.Split('\n');
                foreach (var item in guids)
                {
                    string currentID = item.Trim();
                    Debug.Log(currentID.Length);
                    string finalCode = GetFinalCodeFromGUID(type, currentID);
                    if (string.IsNullOrEmpty(finalCode))
                    {
                        Debug.LogError("创建二维码失败=>BadCode:" + type + "-" + currentID);
                        continue;
                    }
                    Texture2D texture = ZXingQRManager.BuildQRCode(finalCode, 100, sizeLevel, correctionLevel);
                    LocalFileManager.SaveJPG(texture, path + "/" + currentID);
                }
                System.Diagnostics.Process.Start(path);
            }
            GUILayout.EndVertical();
        }

        public static string GetFinalCodeFromGUID(string type, string guid)
        {
            if (type.Length != 4 || guid.Length != 36) return null;
            string finalCode = guid.Replace("-", "");
            finalCode = finalCode.ToUpper();
            finalCode = type + new NumberValue(16, finalCode).ChangeScale(45).ValueArray.PadLeft(24, '0');
            if (finalCode.Length != 28) return null;
            return finalCode;
        }
        public static string GetGUIDFromFinalCode(string finalCode, out string type)
        {
            if (finalCode.Length != 28)
            {
                type = string.Empty;
                return null;
            }
            type = finalCode.Substring(0, 4);
            string guid = finalCode.Substring(4);
            guid = new NumberValue(45, guid).ChangeScale(16).ValueArray;
            guid = guid.ToLower();
            guid = guid.Insert(8, "-");
            guid = guid.Insert(13, "-");
            guid = guid.Insert(18, "-");
            guid = guid.Insert(23, "-");
            return guid;
        }
        public static string GetGUIDFromFinalCode(string finalCode)
        {
            if (finalCode.Length != 28)
            {
                return null;
            }
            string guid = finalCode.Substring(4);
            guid = new NumberValue(45, guid).ChangeScale(16).ValueArray;
            guid = guid.ToLower();
            guid = guid.Insert(8, "-");
            guid = guid.Insert(13, "-");
            guid = guid.Insert(18, "-");
            guid = guid.Insert(23, "-");
            return guid;
        }
    }
}
