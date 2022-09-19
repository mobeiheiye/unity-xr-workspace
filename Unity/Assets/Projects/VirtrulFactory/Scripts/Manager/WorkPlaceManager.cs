using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 工作区管理器
    /// </summary>
    public class WorkPlaceManager
    {



        private static Dictionary<string, StateParamater> stateParamaterDic = new Dictionary<string, StateParamater>();
        public static void AddStateParamater(StateParamater stateParamater)
        {
            stateParamaterDic.Add(stateParamater.StateParamaterName, stateParamater);
            //stateParamater.OnValueChange += OnStateParamaterValueChange;
        }
        private static void OnStateParamaterValueChange(string stateName, string stateValue)
        {
            Debug.Log("StateParamaterValueChange ## StateName : " + stateName + " ## StateValue : " + stateValue);
        }
        public static void SetParamaterValue(string stateName, string stateValue)
        {

        }

    }
}