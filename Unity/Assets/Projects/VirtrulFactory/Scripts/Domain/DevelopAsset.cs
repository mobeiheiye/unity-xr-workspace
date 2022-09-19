using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 拓展资产
    /// </summary>
    [Serializable]
    public class DevelopAsset : Asset
    {
        [Header("<DevelopAsset>")]
        /// <summary>
        /// 作者姓名
        /// </summary>
        public string AuthorName;
    }
}