using UnityEngine;
using System.Collections.Generic;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace CUI.QRCode
{
    /// <summary>
    /// 二维码管理器
    /// 容量：
    ///1级L纠错        41位10进制、25位45进制
    ///1级M纠错      34位10进制、20位45进制
    ///1级Q纠错       27位10进制、16位45进制
    ///1级H纠错       17位10进制、10位45进制
    ///2级L纠错        77位10进制、47位45进制
    ///2级M纠错      63位10进制、38位45进制
    ///2级Q纠错      48位10进制、29位45进制
    ///2级H纠错      34位10进制、20位45进制
    /// </summary>
    public class ZXingQRManager
    {
        private static MultiFormatWriter writer = new MultiFormatWriter();
        private static QRCodeReader reader = new QRCodeReader();
        private static QRCodeWriter QRWriter = new QRCodeWriter();

        /// <summary>
        /// string转二维码
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_size"></param>
        /// <returns></returns>
        public static Texture2D GetQRTextureFromString(string _value, int _size)
        {
            if (string.IsNullOrEmpty(_value) || _size < 32)
            {
                return null;
            }
            Texture2D _texture = new Texture2D(_size, _size);

            Dictionary<EncodeHintType, object> _hints = new Dictionary<EncodeHintType, object>();
            _hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

            BitMatrix _bit = writer.encode(_value, BarcodeFormat.QR_CODE, _size, _size, _hints);

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_bit[j, i])
                    {
                        _texture.SetPixel(j, i, Color.black);
                    }
                    else
                    {
                        _texture.SetPixel(j, i, Color.white);
                    }
                }
            }
            _texture.Apply();
            return _texture;
        }

        /// <summary>
        /// 二维码转string
        /// </summary>
        /// <param name="_texture"></param>
        /// <returns></returns>
        public static string GetStringFromTexture(Texture2D _texture)
        {
            if (!_texture)
            {
                return null;
            }
            Color32LuminanceSource _source = new Color32LuminanceSource(_texture.GetPixels32(), _texture.width, _texture.height);
            BinaryBitmap _bitmap = new BinaryBitmap(new HybridBinarizer(_source));

            Dictionary<DecodeHintType, object> _hints = new Dictionary<DecodeHintType, object>();
            _hints.Add(DecodeHintType.CHARACTER_SET, "UTF-8");

            Result _result = reader.decode(_bitmap, _hints);
            if (_result == null)
            {
                return null;
            }
            return _result.Text;
        }

        public static Texture2D BuildQRCode(string _value, int _textrueSize, int _qrLevel, QRCodeCorrectionLevel _correctionLevel)
        {
            if (string.IsNullOrEmpty(_value) || _textrueSize < 1)
            {
                return null;
            }
            Texture2D _texture = new Texture2D(_textrueSize, _textrueSize);

            Dictionary<EncodeHintType, object> _hints = new Dictionary<EncodeHintType, object>();
            _hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            _hints.Add(EncodeHintType.MARGIN, 0);
            _hints.Add(EncodeHintType.QR_VERSION, _qrLevel);
            _hints.Add(EncodeHintType.ERROR_CORRECTION, _correctionLevel.ToString());

            Debug.Log(_value + "=>" + _value.Length);
            BitMatrix _bit = QRWriter.encode(_value, BarcodeFormat.QR_CODE, _textrueSize, _textrueSize, _hints);

            for (int i = 0; i < _textrueSize; i++)
            {
                for (int j = 0; j < _textrueSize; j++)
                {
                    if (_bit[j, i])
                    {
                        _texture.SetPixel(j, i, Color.black);
                    }
                    else
                    {
                        _texture.SetPixel(j, i, Color.white);
                    }
                }
            }
            _texture.Apply();
            return _texture;
        }
    }

    public enum QRCodeCorrectionLevel
    {
        L,
        M,
        Q,
        H
    }
}