using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 模块权限
    /// </summary>
    public class Permission
    {
        private static List<string> permissionList;

        /// <summary>
        /// 初始化权限列表
        /// </summary>
        /// <param name="list">权限列表</param>
        public static void InitList(List<string> list)
        {
            permissionList = list;

            //    permissionList = new List<string> {
            // "客户端_客观业务_视频_使用",
            // "客户端_客观业务_全景_使用",
            // "客户端_客观业务_模型_使用",
            // "客户端_客观业务_实训场景_使用",
            // "客户端_客观业务_实训设备_使用",
            //" 客户端_客观业务_实训工具_使用",
            // "客户端_主观业务_课件_使用",
            // "客户端_主观业务_课件_编辑",
            // "客户端_主观业务_实训任务_使用",
            // "客户端_主观业务_实训任务_编辑",
            // "客户端_主观业务_试卷_使用",
            // "客户端_主观业务_试卷_编辑",
            // "客户端_顶层业务_专栏_使用"
            // };
        }

        /// <summary>
        /// 获取单个权限值
        /// </summary>
        /// <param name="name">权限名称</param>
        /// <returns></returns>
        public static int NameToValue(string name)
        {
            return permissionList.IndexOf(name);
        }
        /// <summary>
        /// 获取单个权限名称
        /// </summary>
        /// <param name="value">权限值</param>
        /// <returns></returns>
        public static string ValueToName(int value)
        {
            return permissionList.Count > value ? permissionList[value] : null;
        }
        /// <summary>
        /// 获取二进制权限值
        /// </summary>
        /// <param name="names">权限名称数组</param>
        /// <returns>二进制权限值</returns>
        public static int GetPermissionValue(string[] names)
        {
            if (names == null) throw new ArgumentNullException(nameof(names));
            int value = 0;
            foreach (var name in names)
            {
                int num = NameToValue(name);
                if (num != -1) value |= 1 << num;
            }
            return value;
        }
        /// <summary>
        /// 获取权限名称
        /// </summary>
        /// <param name="value">二进制权限值</param>
        /// <returns>权限名称</returns>
        public static string[] GetPermissionNames(int value)
        {
            if (value < 0) throw new ArgumentNullException(nameof(value));
            List<string> names = new List<string>();

            int time = 0;
            while (value > 0)
            {
                if (value % 2 == 1) names.Add(ValueToName(time));
                value /= 2;
                time++;
            }
            return names.ToArray();
        }
    }
}