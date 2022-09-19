using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 专栏
    /// </summary>
    [Serializable]
    public class Column : Entity
    {
        [Header("<Column>")]
        /// <summary>
        /// 专栏作家
        /// </summary>
        public List<User> Writers;
        /// <summary>
        /// 专栏资源
        /// </summary>
        public List<Asset> Assets;
        /// <summary>
        /// 是否免费
        /// </summary>
        public bool IsFree;
        /// <summary>
        /// 订阅数
        /// </summary>
        public int SubscriptionCount;
        /// <summary>
        /// 访问数
        /// </summary>
        public int VisitsCount;
    }
}