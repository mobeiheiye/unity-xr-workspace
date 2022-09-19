using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 工厂
    /// </summary>
    [Serializable]
    public class Scene : DevelopAsset
    {
        [Header("<Factory>")]
        /// <summary>
        /// 厂房
        /// </summary>
        public List<ModelBundle> Models;
        /// <summary>
        /// 厂房布置信息
        /// </summary>
        public List<TransformData> ModelsTransform;
        /// <summary>
        /// 初始状态
        /// </summary>
        public List<StateValue> StartState;
    }

    /// <summary>
    /// 布置信息
    /// </summary>
    [Serializable]
    public struct TransformData
    {
        public string ModelName;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Size;
    }
}