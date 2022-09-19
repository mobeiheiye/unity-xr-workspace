using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 3D模型资源包
    /// </summary>
    [Serializable]
    public class ModelBundle : Asset
    {
        [Header("<ModelBundle>")]
        /// <summary>
        /// 安卓路径
        /// </summary>
        public string AndroidPath;
        /// <summary>
        /// 苹果路径
        /// </summary>
        public string IOSPath;
        /// <summary>
        /// PC路径
        /// </summary>
        public string StandalonePath;
        /// <summary>
        /// 网页路径
        /// </summary>
        public string WebPath;

        /// <summary>
        /// 状态机
        /// </summary>
        public StateMachine StateMachine;
    }
}