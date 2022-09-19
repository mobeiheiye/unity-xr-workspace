using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 组织
    /// </summary>
    [Serializable]
    public class Organization : Entity
    {
        [Header("<Organization>")]
        /// <summary>
        /// 部门成员
        /// </summary>
        public List<User> Users;
        /// <summary>
        /// 版权资产
        /// </summary>
        public List<Asset> Copyrights;
        /// <summary>
        /// 订阅资产
        /// </summary>
        public List<Subscription> Subscriptions;
    }
}