using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace CUI.OpenFileDialog
{
    /// <summary>
    /// 本地文件管理器
    /// </summary>
    public class LocalFileManager
    {
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="_path">文件路径</param>
        /// <returns>是否成功</returns>
        public static bool OpenFileDialog(out string _path)
        {
            OpenFileName _ofn = new OpenFileName();
            _ofn.structSize = Marshal.SizeOf(_ofn);
            _ofn.filter = "All Files\0*.*\0\0";
            _ofn.file = new string(new char[256]);
            _ofn.maxFile = _ofn.file.Length;
            _ofn.fileTitle = new string(new char[64]);
            _ofn.maxFileTitle = _ofn.fileTitle.Length;
            _ofn.initialDir = Application.dataPath;
            _ofn.title = "Open Project";
            _ofn.defExt = "MagicPPT";
            _ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
            bool _result = GetOpenFileName(_ofn);
            _path = _ofn.file;
            return _result;
        }

        public static void WriteDataToLocalFile(string path, string data)
        {
            FileInfo _t = new FileInfo(path);
            if (_t.Exists)
            {
                _t.Delete();
            }

            StreamWriter _sw = _t.CreateText();
            _sw.WriteLine(data);
            _sw.Close();
            _sw.Dispose();
        }
        public static string ReadDataFromLocalFile(string path)
        {
            StreamReader _sr = null;
            try
            {
                _sr = System.IO.File.OpenText(path);
            }
            catch
            {
                return null;
            }
            string _data = string.Empty;
            string _line;
            while ((_line = _sr.ReadLine()) != null)
            {
                _data += _line;
            }
            _sr.Close();
            _sr.Dispose();
            return _data;
        }


        /// <summary>
        /// 按行读数据
        /// </summary>
        /// <param name="path">表格路径</param>
        /// <returns>数据序列</returns>
        public static List<string> GetDataLines(string path)
        {
            List<string> _data = new List<string>();
            StreamReader reader = new StreamReader(path);
            //读取表头
            string line = reader.ReadLine();
            //读取数据
            while ((line = reader.ReadLine()) != null)
            {
                _data.Add(line);
            }
            return _data;
        }

        /// <summary>
        /// 按行读数据
        /// </summary>
        /// <param name="textAsset">表格</param>
        /// <returns>数据序列</returns>
        public static List<string> GetDataLines(TextAsset textAsset)
        {
            string[] data = textAsset.text.Split("\n"[0]);
            List<string> _data = new List<string>();
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] != string.Empty)
                {
                    _data.Add(data[i].Trim());
                }
            }
            return _data;
        }

        /// <summary>
        /// 保存JPG到本地
        /// </summary>
        /// <param name="_texture"></param>
        /// <param name="_path"></param>
        public static void SaveJPG(Texture2D _texture, string _path)
        {
            byte[] _byte = _texture.EncodeToJPG();
            _path += ".jpg";
            Debug.Log(_path);
            FileStream f = new FileStream(_path, FileMode.Create);
            f.Write(_byte, 0, _byte.Length);
            f.Flush();
            f.Close();
            Debug.Log("JPG保存成功:" + _path);
        }

        /// <summary>
        /// 保存PNG到本地
        /// </summary>
        /// <param name="_texture"></param>
        /// <param name="_path"></param>
        public static void SavePNG(Texture2D _texture, string _path)
        {
            byte[] _byte = _texture.EncodeToPNG();
            _path += ".png";
            FileStream f = new FileStream(_path, FileMode.Create);
            f.Write(_byte, 0, _byte.Length);
            f.Flush();
            f.Close();
            Debug.Log("PNG保存成功:" + _path);
        }

        /// <summary>
        /// 获取相机图像
        /// </summary>
        /// <param name="_target">相机</param>
        /// <returns></returns>
        public static Texture2D GetTextureFromCamera(Camera _target)
        {
            if (!_target)
            {
                return null;
            }
            //渲染到RenderTexture
            RenderTexture _renderTexture = new RenderTexture((int)_target.pixelRect.width, (int)_target.pixelRect.height, 0);
            _target.targetTexture = _renderTexture;
            _target.Render();
            RenderTexture.active = _renderTexture;
            //生成Texture2D
            Texture2D _result = new Texture2D((int)_target.pixelRect.width, (int)_target.pixelRect.height);
            _result.ReadPixels(new Rect(0, 0, (int)_target.pixelRect.width, (int)_target.pixelRect.height), 0, 0);
            _result.Apply();
            //恢复Camera
            _target.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(_renderTexture);
            //返回
            return _result;
        }

        /// <summary>
        /// 获取相机图像
        /// </summary>
        /// <param name="_targets">相机集合</param>
        /// <returns></returns>
        public static Texture2D GetTextureFromCamera(Camera[] _targets)
        {
            if (_targets == null)
            {
                return null;
            }
            //渲染到RenderTexture
            RenderTexture _renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            foreach (var _camera in _targets)
            {
                _camera.targetTexture = _renderTexture;
                _camera.Render();
            }
            RenderTexture.active = _renderTexture;
            //生成Texture2D
            Texture2D _result = new Texture2D(Screen.width, Screen.height);
            _result.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            _result.Apply();
            //恢复Camera
            foreach (var _camera in _targets)
            {
                _camera.targetTexture = null;
            }
            RenderTexture.active = null;
            GameObject.Destroy(_renderTexture);
            //返回
            return _result;
        }
    }
}