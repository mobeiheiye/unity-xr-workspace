using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 资产
    /// </summary>
    [Serializable]
    public class Asset : Entity
    {
        [Header("<Asset>")]
        /// <summary>
        /// 资产拥有者名称
        /// </summary>
        public string OwnerName;
        /// <summary>
        /// 版本
        /// </summary>
        public string Version;
        /// <summary>
        /// 资产标签
        /// </summary>
        public List<string> Tags;
        /// <summary>
        /// 访问数
        /// </summary>
        public int VisitCount;
    }
}