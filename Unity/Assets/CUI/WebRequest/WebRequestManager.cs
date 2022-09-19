using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections;
using System.Net;

namespace CUI.WebRequest
{
    /// <summary>
    /// Unity Get/Post/Put/Delete
    /// </summary>
    public class WebRequestManager : MonoBehaviour
    {
        private static WebRequestManager instence;
        public static WebRequestManager Instence
        {
            get
            {
                if (instence == null)
                {
                    instence = GameObject.FindObjectOfType<WebRequestManager>();
                    if (instence == null)
                    {
                        instence = new GameObject("WebRequestManager").AddComponent<WebRequestManager>();
                    }
                }
                return instence;
            }
        }

        private string texturesCacheRoot;

        public void Get(string requestID, string url, UnityAction<string, string, string> callback)
        {
            StartCoroutine(GetRequest(requestID, url, callback));
        }
        public void Get(string requestID, string url, UnityAction<string, Sprite> callback)
        {
            StartCoroutine(GetTextureRequest(requestID, url, callback));
        }
        public void Get(string requestID, string url, UnityAction<string, string, AssetBundle> callback)
        {
            StartCoroutine(GetAssetBundleRequest(requestID, url, callback));
        }
        public void Post(string requestID, string url, WWWForm form, UnityAction<string, string, string> callback)
        {
            StartCoroutine(PostRequest(requestID, url, form, callback));
        }
        public void Post(string requestID, string url, byte[] data, UnityAction<string, string, string> callback)
        {
            StartCoroutine(PostRequest(requestID, url, data, callback));
        }
        public void Put(string requestID, string url, UnityAction<string, string, string> callback)
        {
            StartCoroutine(PutRequest(requestID, url, null, callback));
        }
        public void Put(string requestID, string url, byte[] data, UnityAction<string, string, string> callback)
        {
            StartCoroutine(PutRequest(requestID, url, data, callback));
        }
        public void Delete(string requestID, string url, UnityAction<string, string, string> callback)
        {
            StartCoroutine(DeleteRequest(requestID, url, callback));
        }

        private IEnumerator GetRequest(string requestID, string url, UnityAction<string, string, string> callback)
        {
            using (UnityWebRequest _request = UnityWebRequest.Get(url))
            {
                yield return _request.SendWebRequest();
                if (_request.result != UnityWebRequest.Result.Success)
                {

                    if (callback != null)
                    {
                        callback(requestID, string.Empty, _request.error);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(requestID, _request.downloadHandler.text, string.Empty);
                    }
                }
            }
        }

        private IEnumerator GetTextureRequest(string requestID, string url, UnityAction<string, Sprite> callback)
        {
            using (UnityWebRequest _request = new UnityWebRequest(url))
            {
                DownloadHandlerTexture _downloadHandler = new DownloadHandlerTexture(true);
                _request.downloadHandler = _downloadHandler;
                yield return _request.SendWebRequest();
                if (_request.result != UnityWebRequest.Result.Success)
                {
                    if (callback != null)
                    {
                        callback(requestID, null);
                    }
                }
                else
                {
                    Texture2D _texture2D = _downloadHandler.texture;
                    if (_texture2D)
                    {
                        if (callback != null)
                        {
                            callback(requestID, Sprite.Create(_texture2D, new Rect(0, 0, _texture2D.width, _texture2D.height), new Vector2(0.5f, 0.5f)));
                        }
                    }
                    else
                    {
                        if (callback != null)
                        {
                            callback(requestID, null);
                        }
                    }
                }
            }
            Resources.UnloadUnusedAssets();
        }

        private IEnumerator GetAssetBundleRequest(string requestID, string url, UnityAction<string, string, AssetBundle> callback)
        {
            using (UnityWebRequest _request = UnityWebRequestAssetBundle.GetAssetBundle(url))
            {
                DownloadHandlerAssetBundle _downloadHandler = new DownloadHandlerAssetBundle(url, 0);
                _request.downloadHandler = _downloadHandler;
                yield return _request.SendWebRequest();
                if (_request.result != UnityWebRequest.Result.Success)
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(_request);
                    callback?.Invoke(requestID, string.Empty, bundle);
                }
                else
                {
                    callback?.Invoke(requestID, _request.error, null);
                }
            }
        }

        private IEnumerator PostRequest(string requestID, string url, WWWForm form, UnityAction<string, string, string> callback)
        {
            using (UnityWebRequest _request = UnityWebRequest.Post(url, form))
            {
                yield return _request.SendWebRequest();
                if (_request.result != UnityWebRequest.Result.Success)
                {
                    if (callback != null)
                    {
                        callback(requestID, string.Empty, _request.error);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(requestID, _request.downloadHandler.text, string.Empty);
                    }
                }
            }
        }

        private IEnumerator PostRequest(string requestID, string url, byte[] data, UnityAction<string, string, string> callback)
        {
            using (UnityWebRequest _request = new UnityWebRequest(url, "POST"))
            {
                DownloadHandler downloadHandler = new DownloadHandlerBuffer();
                _request.downloadHandler = downloadHandler;
                UploadHandler _uploadHandler = new UploadHandlerRaw(data);
                _request.uploadHandler = _uploadHandler;
                _request.SetRequestHeader("Content-Type", "application/json");
                yield return _request.SendWebRequest();
                if (_request.result != UnityWebRequest.Result.Success)
                {
                    if (callback != null)
                    {
                        callback(requestID, string.Empty, _request.error);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(requestID, _request.downloadHandler.text, string.Empty);
                    }
                }
            }
        }

        private IEnumerator PutRequest(string requestID, string url, byte[] data, UnityAction<string, string, string> callback)
        {
            using (UnityWebRequest _request = UnityWebRequest.Put(url, data))
            {
                DownloadHandler downloadHandler = new DownloadHandlerBuffer();
                _request.downloadHandler = downloadHandler;
                _request.SetRequestHeader("Content-Type", "application/json;charset=UTF-8 ");
                yield return _request.SendWebRequest();
                if (_request.result != UnityWebRequest.Result.Success)
                {
                    if (callback != null)
                    {
                        callback(requestID, string.Empty, _request.error);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(requestID, _request.downloadHandler.text, string.Empty);
                    }
                }
            }
        }

        private IEnumerator DeleteRequest(string requestID, string url, UnityAction<string, string, string> callback)
        {
            using (UnityWebRequest _request = new UnityWebRequest(url, "DELETE"))
            {
                DownloadHandler downloadHandler = new DownloadHandlerBuffer();
                _request.downloadHandler = downloadHandler;
                _request.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");
                yield return _request.SendWebRequest();
                if (_request.result != UnityWebRequest.Result.Success)
                {
                    if (callback != null)
                    {
                        callback(requestID, string.Empty, _request.error);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(requestID, _request.downloadHandler.text, string.Empty);
                    }
                }
            }
        }

        private void Awake()
        {
            instence = this;
            texturesCacheRoot =
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Application.dataPath + "/StreamingAssets/Cache/Textures";
#elif UNITY_IPHONE || UNITY_ANDROID
        Application.persistentDataPath+ "/Textures";
#else
        string.Empty;
#endif
        }
    }
}