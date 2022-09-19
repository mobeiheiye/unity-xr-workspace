using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtrulFactory.Trainning
{
    /// <summary>
    /// 实训
    /// </summary>
    [Serializable]
    public class Trainning : Entity
    {
        [Header("<TrainningTask>")]
        /// <summary>
        /// 任务书
        /// </summary>
        public TaskPaper TaskPaper;
        /// <summary>
        /// 实训发起人
        /// </summary>
        public User Originator;
        /// <summary>
        /// 实训执行人
        /// </summary>
        public List<User> Executors;
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime;
        /// <summary>
        /// 实训时间（0为无限时间）
        /// </summary>
        public int Duration;
        /// <summary>
        /// 实训报告
        /// </summary>
        public List<PersonalTrainningReport> TrainningReports;

    }
}