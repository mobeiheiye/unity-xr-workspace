using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VirtrulFactory
{
    /// <summary>
    /// 状态机
    /// </summary>
    [Serializable]
    public class StateMachine
    {
        public string StateMachineName;
        public List<StateParamater> StateParamaters;
    }

    /// <summary>
    /// 状态参数
    /// </summary>
    [Serializable]
    public struct StateParamater
    {
        public string StateParamaterName;

        public StateParamaterValueRangeLimit RangeLimit;
        public StateParamaterValueEnumLimit EnumLimit;

        public UnityAction<string, string> OnValueChange;

        private string stateValue;
        public string Value
        {
            get
            {
                return stateValue;
            }
            set
            {
                //switch (Type)
                //{
                //    case StateParamaterType.FloatRange:
                //        float floatValue;
                //        if (float.TryParse(value, out floatValue))
                //        {
                //            floatValue = Mathf.Min(floatValue, floatRangeMax);
                //            floatValue = Mathf.Max(floatValue, floatRangeMin);
                //            stateValue = floatValue.ToString();
                //            OnValueChange?.Invoke(ID, stateValue);
                //        }
                //        break;
                //    case StateParamaterType.Enum:
                //        if (valueEnum == null || valueEnum.Count == 0) return;
                //        if (valueEnum.Contains(value))
                //        {
                //            stateValue = value;
                //        }
                //        else
                //        {
                //            stateValue = valueEnum[0];
                //        }
                //        OnValueChange?.Invoke(ID, stateValue);
                //        break;
                //    default:
                //        stateValue = value;
                //        OnValueChange?.Invoke(ID, stateValue);
                //        break;
                //}

                if (RangeLimit.Enable)
                {

                }
                if (true)
                {

                }
            }
        }
    }

    /// <summary>
    /// 状态值
    /// </summary>
    [Serializable]
    public struct StateValue
    {
        /// <summary>
        /// 状态机名称
        /// </summary>
        public string MachineName;
        /// <summary>
        /// 状态名
        /// </summary>
        public string ParamaterName;
        /// <summary>
        /// 值
        /// </summary>
        public string Value;
    }

    /// <summary>
    /// 状态参数区间取值限定
    /// </summary>
    [Serializable]
    public struct StateParamaterValueRangeLimit
    {
        public bool Enable;
        public float Min;
        public float Max;
    }

    /// <summary>
    /// 状态参数枚举取值限定
    /// </summary>
    [Serializable]
    public struct StateParamaterValueEnumLimit
    {
        public bool Enable;
        public List<string> ValueEnum;
    }

}
