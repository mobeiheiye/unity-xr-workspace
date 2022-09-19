using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class User : Entity
    {
        /// <summary>
        /// 性别
        /// </summary>
        public enum SexEnum
        {
            男 = 0,
            女 = 1
        }

        [Header("<User>")]
        /// <summary>
        /// 账户名
        /// </summary>
        public string Account;
        /// <summary>
        /// 性别
        /// </summary>
        public SexEnum Sex;
        /// <summary>
        /// 最近一次登陆Token
        /// </summary>
        public string LastToken;
        /// <summary>
        /// 角色权限值
        /// </summary>
        public Role UserRole;
        /// <summary>
        /// 组织
        /// </summary>
        public List<Organization> Organizations;
        /// <summary>
        /// 创造资产
        /// </summary>
        public List<DevelopAsset> DevelopAssets;
        /// <summary>
        /// 订阅资产
        /// </summary>
        public List<Subscription> Subscriptions;
    }

    /// <summary>
    /// 订阅
    /// </summary>
    [Serializable]
    public struct Subscription
    {
        public string ColumnID;
        public string ExpirationDate;   //yyyy-MM-dd HH:mm
    }
}