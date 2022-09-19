using System;
using System.Collections.Generic;

namespace VirtrulFactory.Trainning
{
    /// <summary>
    /// 个人实训报告
    /// </summary>
    [Serializable]
    public class PersonalTrainningReport : Entity
    {
        /// <summary>
        /// 实训状态枚举
        /// </summary>
        [Serializable]
        public enum TrainningStatusEnum
        {
            Wait,
            Doing,
            Done
        }

        /// <summary>
        ///  对应的实训
        /// </summary>
        public Trainning RefrenceTask;
        /// <summary>
        /// 用户Token
        /// </summary>
        public string UserToken;
        /// <summary>
        /// 实训状态
        /// </summary>
        public TrainningStatusEnum TrainningStatus;
        /// <summary>
        /// 任务轨迹
        /// </summary>
        public List<TaskScore> WorkTrace;
    }

    /// <summary>
    /// 任务成绩
    /// </summary>
    [Serializable]
    public struct TaskScore
    {
        public Task Task;
        public List<StateValue> WorkTrace;
        public float Score;
    }
}