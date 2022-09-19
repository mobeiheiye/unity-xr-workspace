using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory
{
    /// <summary>
    /// 基础实体
    /// </summary>
    [Serializable]
    public class Entity
    {
        /// <summary>
        /// 实体字典
        /// </summary>
        public static Dictionary<string, Entity> Dic = new Dictionary<string, Entity>();
        /// <summary>
        /// 根据类型获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetEntitiesOfType<T>() where T : Entity
        {
            List<T> list = new List<T>();
            foreach (var item in Dic)
            {
                if (item.Value.GetType().Equals(typeof(T)))
                {
                    list.Add(item.Value as T);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public static T Create<T>(string json = "") where T : Entity
        {
            T model;
            if (string.IsNullOrEmpty(json))
            {
                model = Activator.CreateInstance<T>();
                model.ID = Guid.NewGuid().ToString();
            }
            else
            {
                model = JsonUtility.FromJson<T>(json);
                if (model == null || string.IsNullOrEmpty(model.ID))
                {
                    return null;
                }
            }
            Dic.Add(model.ID, model);
            return model;
        }

        /// <summary>
        /// 实体状态
        /// </summary>
        [Serializable]
        public enum StatusEnum
        {
            Enable = 0,
            Disable = 1,
            Deleted = 2
        }

        [Header("<Entity>")]
        /// <summary>
        /// 实体ID(GUID，32位16进制字母，不包含横杠)
        /// </summary>
        public string ID = string.Empty;
        /// <summary>
        /// 实体类型ID(4位45进制字母大写)
        /// </summary>
        public string TypeID = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name = string.Empty;
        /// <summary>
        /// 图标文件路径（相对）
        /// </summary>
        public string IconPath = string.Empty;
        /// <summary>
        /// 缩略图路径（相对）
        /// </summary>
        public string ImagePath = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description = string.Empty;

        /// <summary>
        /// 实体状态
        /// </summary>
        public StatusEnum Status = StatusEnum.Enable;
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime;
        /// <summary>
        /// 创建账号
        /// </summary>
        public string CreateBy;
        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime;
        /// <summary>
        /// 更新账号
        /// </summary>
        public string UpdateBy;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}