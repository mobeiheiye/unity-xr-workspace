using System;
using System.Collections.Generic;
using UnityEngine;
using CUI.Collections;

namespace VirtrulFactory
{
    /// <summary>
    /// 任务书
    /// </summary>
    [Serializable]
    public class TaskPaper : DevelopAsset
    {
        [Header("<TaskPaper>")]
        public Scene DependFactory;
        /// <summary>
        /// 初始状态
        /// </summary>
        public List<StateValue> StartState;
        /// <summary>
        /// 任务树
        /// </summary>
        public Task TaskTree;
    }

    /// <summary>
    /// 任务
    /// </summary>
    [Serializable]
    public class Task
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Infor;
        /// <summary>
        /// 任务描述配音
        /// </summary>
        public string InforAudioPath;
        /// <summary>
        /// 目标状态
        /// </summary>
        public List<TargetStateValue> TargetState;
        /// <summary>
        /// 分数
        /// </summary>
        public float FullMark;
        /// <summary>
        /// 子节点
        /// </summary>
        public Task Children;
    }

    /// <summary>
    /// 目标状态值
    /// </summary>
    [Serializable]
    public struct TargetStateValue
    {
        /// <summary>
        /// 状态值
        /// </summary>
        public StateValue StateValue;
        /// <summary>
        /// 区间误差加权
        /// </summary>
        public RangeWeighting RangeWeighting;
        /// <summary>
        /// 枚举选项加权
        /// </summary>
        public EnumWeighting EnumWeighting;
    }

    /// <summary>
    /// 区间误差加权
    /// </summary>
    [Serializable]
    public struct RangeWeighting
    {
        public float Allowance;
    }

    /// <summary>
    /// 枚举选项加权
    /// </summary>
    [Serializable]
    public struct EnumWeighting
    {
        public List<int> EnumScore;
    }
}