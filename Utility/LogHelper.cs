using System;
using System.IO;
using NLog;

namespace CodeHelper.Utility
{
    /// <summary>
    /// 日志工具类
    /// </summary>
    public static class LogHelper
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 追踪，追踪生成的文件名称
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="info"></param>
        public static void Trace(string logName, string info)
        {
            LogEventInfo theEventInfo = new LogEventInfo();
            theEventInfo.Level = LogLevel.Trace;
            theEventInfo.Properties["LogName"] = logName;
            theEventInfo.Message = info;
            logger.Trace(theEventInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            logger.Info(info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public static void Error(string error)
        {
            logger.Error(error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warn"></param>
        public static void Warn(string warn)
        {
            logger.Warn(warn);
        }

        /// <summary>
        /// 清理没用的日志文件及目录
        /// </summary>
        /// <param name="logDirectory"></param>
        public static void ClearUselessLogs(string logDirectory)
        {
            if (!string.IsNullOrEmpty(logDirectory) && Directory.Exists(logDirectory))
            {
                string directoryPath = Path.Combine(logDirectory, "Info");
                ClearLogDirectory(directoryPath);
                string directoryPath2 = Path.Combine(logDirectory, "Error");
                ClearLogDirectory(directoryPath2);
                string directoryPath3 = Path.Combine(logDirectory, "Warn");
                ClearLogDirectory(directoryPath3);
                string directoryPath4 = Path.Combine(logDirectory, "Trace");
                ClearLogDirectory(directoryPath4);
            }
        }

        /// <summary>
        /// 清理日志目录
        /// </summary>
        /// <param name="directoryPath"></param>
        private static void ClearLogDirectory(string directoryPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(directoryPath) && Directory.Exists(directoryPath))
                {
                    string[] files = Directory.GetFiles(directoryPath);
                    if (files != null && files.Length > 0)
                    {
                        foreach (string filePath in files)
                        {
                            ClearLogFile(filePath);
                        }
                    }
                    Directory.Delete(directoryPath);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 清理日志文件
        /// </summary>
        /// <param name="filePath"></param>
        private static void ClearLogFile(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    var date = fileNameWithoutExtension.Substring(fileNameWithoutExtension.Length - 10);
                    DateTime now = DateTime.Now;
                    if (DateTime.TryParse(date, out now) && (DateTime.Now - now).Days > 31)
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
