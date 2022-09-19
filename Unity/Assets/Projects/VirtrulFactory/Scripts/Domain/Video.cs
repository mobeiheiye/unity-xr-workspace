using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 视频
    /// </summary>
    [Serializable]
    public class Video : Asset
    {
        [Header("<Video>")]
        /// <summary>
        /// 普清路径
        /// </summary>
        public string LDPath;
        /// <summary>
        /// 标清路径
        /// </summary>
        public string SDPath;
        /// <summary>
        /// 高清路径
        /// </summary>
        public string HDPath;
    }
}