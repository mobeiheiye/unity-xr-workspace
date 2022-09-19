using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CUI.WebRequest
{
    public class ResourceManager
    {
        private static Dictionary<string, Sprite> dic_Image = new Dictionary<string, Sprite>();
        private static Dictionary<string, List<UnityAction<string, Sprite>>> dic_ImageCallBack = new Dictionary<string, List<UnityAction<string, Sprite>>>();
        public static void GetImage(string path, UnityAction<string, Sprite> callBack)
        {
            if (!string.IsNullOrEmpty(path) && dic_Image.ContainsKey(path))
            {
                callBack(path, dic_Image[path]);
            }
            else
            {
                if (dic_ImageCallBack.ContainsKey(path))
                {
                    dic_ImageCallBack[path].Add(callBack);
                }
                else
                {
                    dic_ImageCallBack.Add(path, new List<UnityAction<string, Sprite>>() { callBack });
                }

                WebRequestManager.Instence.Get(path, path, GetImageWebCallBack);
            }
        }
        private static void GetImageWebCallBack(string path, Sprite image)
        {
            if (image)
            {
                if (!dic_Image.ContainsKey(path))
                {
                    dic_Image.Add(path, image);
                }

                if (dic_ImageCallBack.ContainsKey(path))
                {
                    foreach (var item in dic_ImageCallBack[path])
                    {
                        item(path, image);
                    }
                    dic_ImageCallBack.Remove(path);
                }
            }
        }

        private static Dictionary<string, GameObject> dic_model = new Dictionary<string, GameObject>();
        private static Dictionary<string, List<UnityAction<string, GameObject>>> dic_ModelCallBack = new Dictionary<string, List<UnityAction<string, GameObject>>>();
        public static void GetModel(string path, UnityAction<string, GameObject> callBack)
        {
            if (!string.IsNullOrEmpty(path) && dic_model.ContainsKey(path))
            {
                callBack(path, dic_model[path]);
            }
            else
            {
                if (dic_ModelCallBack.ContainsKey(path))
                {
                    dic_ModelCallBack[path].Add(callBack);
                }
                else
                {
                    dic_ModelCallBack.Add(path, new List<UnityAction<string, GameObject>>() { callBack });
                }

                WebRequestManager.Instence.Get(path, path, GetModelWebCallBack);
            }
        }
        private static void GetModelWebCallBack(string path, string error, AssetBundle assetBundle)
        {
            if (assetBundle)
            {
                GameObject model = assetBundle.LoadAsset(assetBundle.GetAllAssetNames()[0]) as GameObject;
                if (model)
                {
                    if (!dic_model.ContainsKey(path))
                    {
                        dic_model.Add(path, model);
                    }

                    if (dic_ModelCallBack.ContainsKey(path))
                    {
                        foreach (var item in dic_ModelCallBack[path])
                        {
                            item(path, model);
                        }
                        dic_ModelCallBack.Remove(path);
                    }
                }
            }
        }
    }
}