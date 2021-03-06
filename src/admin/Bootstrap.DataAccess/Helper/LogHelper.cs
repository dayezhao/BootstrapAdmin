﻿using Longbow.Tasks;
using Longbow.Web.Mvc;
using PetaPoco;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 操作日志相关操作类
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <param name="op"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="opType"></param>
        /// <returns></returns>
        public static Page<Log> RetrievePages(PaginationOption op, DateTime? startTime, DateTime? endTime, string? opType) => DbContextManager.Create<Log>()?.RetrievePages(op, startTime, endTime, opType) ?? new Page<Log>() { Items = new List<Log>() };

        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Log> RetrieveAll(DateTime? startTime, DateTime? endTime, string? opType) => DbContextManager.Create<Log>()?.RetrieveAll(startTime, endTime, opType) ?? new Log[0];

        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static bool Save(Log log)
        {
            log.LogTime = DateTime.Now;
            return DbContextManager.Create<Log>()?.Save(log) ?? false;
        }

        #region 数据库脚本执行日志相关代码
        private static readonly BlockingCollection<DBLog> _messageQueue = new BlockingCollection<DBLog>(new ConcurrentQueue<DBLog>());
        /// <summary>
        /// 添加数据库日志实体类到内部集合中
        /// </summary>
        /// <param name="log"></param>
        public static System.Threading.Tasks.Task AddDBLog(DBLog log) => System.Threading.Tasks.Task.Run(() =>
        {
            if (!_messageQueue.IsAddingCompleted && !_pause)
            {
                _messageQueue.Add(log);
            }
        });

        private static bool _pause;
        /// <summary>
        /// 暂停接收脚本执行日志
        /// </summary>
        public static void Pause() => _pause = true;

        /// <summary>
        /// 开始接收脚本执行日志
        /// </summary>
        public static void Run() => _pause = false;

        /// <summary>
        /// 查询所有SQL日志信息
        /// </summary>
        /// <param name="op"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Page<DBLog> RetrieveDBLogs(PaginationOption op, DateTime? startTime, DateTime? endTime, string? userName) => DbContextManager.Create<DBLog>()?.RetrievePages(op, startTime, endTime, userName) ?? new Page<DBLog>() { Items = new List<DBLog>() };

        /// <summary>
        /// 数据库脚本执行日志任务实体类
        /// </summary>
        public class DbLogTask : ITask
        {
            /// <summary>
            /// 任务执行方法
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public System.Threading.Tasks.Task Execute(CancellationToken cancellationToken)
            {
                var logs = new List<DBLog>();
                while (_messageQueue.TryTake(out var log))
                {
                    if (log != null) logs.Add(log);
                    if (logs.Count >= 100) break;
                }
                if (logs.Any())
                {
                    using var db = DbManager.Create(enableLog: false);
                    db.InsertBatch(logs);
                }
                return System.Threading.Tasks.Task.CompletedTask;
            }
        }
        #endregion
    }
}
