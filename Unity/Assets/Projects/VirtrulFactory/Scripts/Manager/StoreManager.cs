using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CUI.WebRequest;

namespace VirtrulFactory.Manager
{
    public class StoreManager
    {
        #region Column
        private static Dictionary<string, Column> dicColumns = new Dictionary<string, Column>();

        public static void AddColumn(Column column)
        {
            if (dicColumns.ContainsKey(column.ID))
            {
                dicColumns[column.ID] = column;
            }
            else
            {
                dicColumns.Add(column.ID, column);
            }
        }
        public static Column GetColumn(string id)
        {
            return dicColumns.ContainsKey(id) ? dicColumns[id] : null;
        }
        #endregion

        #region BasicAsset
        private static Dictionary<string, Asset> dicResources = new Dictionary<string, Asset>();
        public static void AddResource(Asset resource)
        {
            if (dicResources.ContainsKey(resource.ID))
            {
                dicResources[resource.ID] = resource;
            }
            else
            {
                dicResources.Add(resource.ID, resource);
            }
        }
        public static Asset GetResource(string id)
        {
            return dicResources.ContainsKey(id) ? dicResources[id] : null;
        }
        #endregion

        #region DevelopAsset
        private static Dictionary<string, TaskPaper> dicWorkTasks = new Dictionary<string, TaskPaper>();
        public static void AddWorkTask(TaskPaper workTask)
        {
            if (dicWorkTasks.ContainsKey(workTask.ID))
            {
                dicWorkTasks[workTask.ID] = workTask;
            }
            else
            {
                dicWorkTasks.Add(workTask.ID, workTask);
            }
        }
        public static TaskPaper GetWorkTask(string id)
        {
            return dicWorkTasks.ContainsKey(id) ? dicWorkTasks[id] : null;
        }
        #endregion
    }
}