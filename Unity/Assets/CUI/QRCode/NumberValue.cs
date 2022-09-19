using System;
using UnityEngine;

namespace CUI.QRCode
{
    /// <summary>
    /// 拓展数字类型，2~45进制转换
    /// </summary>
    [Serializable]
    public class NumberValue
    {
        public uint Scale;
        public string ValueArray;

        //2~45进制字符集  
        private const string characterTable = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";

        public NumberValue(uint scale, string value)
        {
            Scale = scale;
            ValueArray = value;
        }
        public NumberValue ChangeScale(uint targetScale)
        {
            string intValue = GetIntFromStr(ValueArray, Scale);
            string strValue = GetStrFromInt(intValue, targetScale);
            return new NumberValue(targetScale, strValue);
        }

        /// <summary>
        /// N进制转10进制
        /// </summary>
        /// <param name="value">N进制串</param>
        /// <param name="scale">进位制值</param>
        /// <returns></returns>
        private string GetIntFromStr(string value, uint scale)
        {
            if (scale > 45 || scale < 2) return null;
            string result = string.Empty;
            for (int i = 0; i < value.Length; i++)
            {
                int current = characterTable.IndexOf(value[value.Length - 1 - i]);
                if (current > scale - 1 || current < 0) return null;
                string currentPlace = current.ToString();
                for (int j = 0; j < i; j++)
                {
                    currentPlace = Multiply(currentPlace, scale.ToString());
                }
                result = Plus(result, currentPlace);
            }
            return result;
        }

        /// <summary>
        /// 10进制转N进制
        /// </summary>
        /// <param name="intValue">10进制值</param>
        /// <param name="scale">进位制值</param>
        /// <returns></returns>
        private string GetStrFromInt(string intValue, uint scale)
        {
            if (scale > 45 || scale < 2) return null;
            string result = string.Empty;
            while (!(intValue.Length == 1 && intValue[0] == '0'))
            {
                int remainder = 0;
                intValue = Divide(intValue, (int)scale, out remainder);
                result = characterTable[remainder] + result;
            }
            return result;
        }

        #region 高精度四则运算       
        private string Plus(string a, string b)
        {
            a = RemoveZero(a);
            b = RemoveZero(b);
            int length = a.Length > b.Length ? a.Length : b.Length;
            int[] arrayA = GetIntArrayFromString(a, 128);
            int[] arrayB = GetIntArrayFromString(b, 128);

            for (int i = 0; i < length; i++)
            {
                arrayA[i] += arrayB[i];
                arrayA[i + 1] += arrayA[i] / 10;
                arrayA[i] %= 10;
            }
            return RemoveZero(GetStringFromIntArray(arrayA));
        }
        private string Multiply(string a, string b)
        {
            a = RemoveZero(a);
            b = RemoveZero(b);
            int[] arrayA = GetIntArrayFromString(a, a.Length);
            int[] arrayB = GetIntArrayFromString(b, b.Length);

            int[] result = new int[128];
            for (int i = 0; i < arrayA.Length; i++)
            {
                for (int j = 0; j < arrayB.Length; j++)
                {
                    result[i + j] += arrayA[i] * arrayB[j];
                    result[i + j + 1] += result[i + j] / 10;
                    result[i + j] %= 10;
                }
            }

            return RemoveZero(GetStringFromIntArray(result));
        }
        private string Divide(string a, int b, out int remainder)
        {
            a = RemoveZero(a);
            int[] arrayA = GetIntArrayFromString(a, a.Length);

            int[] result = new int[a.Length];
            remainder = 0;
            for (int i = arrayA.Length - 1; i > -1; i--)
            {
                result[i] = (arrayA[i] + remainder * 10) / b;
                remainder = (arrayA[i] + remainder * 10) % b;
                //Debug.Log(result[i].ToString() + "::::" + remainder.ToString());
            }
            return RemoveZero(GetStringFromIntArray(result));
        }
        #endregion

        #region 十进制整形数组和字符串转换
        private int[] GetIntArrayFromString(string source, int length)
        {
            int[] result = new int[length];
            for (int i = source.Length - 1; i > -1; i--) result[source.Length - 1 - i] = int.Parse((source[i]).ToString());
            return result;
        }
        private string GetStringFromIntArray(int[] source)
        {
            string result = string.Empty;
            for (int i = source.Length - 1; i > -1; i--) result += source[i];
            return result;
        }
        #endregion

        private string RemoveZero(string source)
        {
            string result = source;
            while (result.Length > 1 && result[0] == '0')
            {
                result = result.Substring(1);
            }
            return result;
        }

        public override string ToString()
        {
            return ValueArray.Length + "位" + Scale + "进制" + '\n' + ValueArray;
        }
    }
}