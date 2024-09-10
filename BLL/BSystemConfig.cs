using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using CodeHelper.Model.DTO;
using CodeHelper.Model.Exception;
using CodeHelper.Utility;

namespace CodeHelper.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class BSystemConfig : Singleton<BSystemConfig>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetDbPath()
        {
            string doc = AppDomain.CurrentDomain.BaseDirectory + "/database/";
            if (!Directory.Exists(doc))
            {
                Directory.CreateDirectory(doc);
            }
            return doc + "config.json";
        } 


        /// <summary>
        /// 获取服务配置
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ServerConfigDTO>? GetServerConfigs()
        {            
            if (File.Exists(GetDbPath())) 
            {
                using(FileStream fs = new FileStream(GetDbPath(), FileMode.Open, FileAccess.Read))
                {
                    using (TextReader tr = new StreamReader(fs, Encoding.UTF8))
                    {
                        string json = tr.ReadToEnd();
                        tr.Close();
                        return JsonSerializer.Deserialize<Dictionary<string, ServerConfigDTO>>(json);
                    }
                }
            }
            return new Dictionary<string, ServerConfigDTO>();
        }

        /// <summary>
        /// 保存服务配置
        /// </summary>
        /// <param name="config"></param>
        public void Save(ServerConfigDTO config)
        {
            if (config == null)
            {
                throw new ServiceException { ErrorCode = Enum.ExceptionTypes.ParameterError, ErrorMessage = "数据库配置信息为null" };
            }

            if (string.IsNullOrEmpty(config.Id))
            {
                config.Id = Guid.NewGuid().ToString().Replace("-", "");
            }
            Dictionary<string, ServerConfigDTO>? servers = GetServerConfigs();
            if (servers == null) {
                servers = new Dictionary<string, ServerConfigDTO>();
            }

            if (servers.ContainsKey(config.Id))
            {
                servers[config.Id] = config;
            }
            else
            {
                servers.Add(config.Id, config);
            }

            using (FileStream fs = new FileStream(GetDbPath(), FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (TextWriter tw = new StreamWriter(fs, Encoding.UTF8))
                {
                    tw.Write(JsonSerializer.Serialize(servers));
                    tw.Flush();
                    tw.Close();
                }
            }
        }

        /// <summary>
        /// 根据配置名称删除配置信息
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ServiceException { ErrorCode = Enum.ExceptionTypes.ParameterError, ErrorMessage = "请选择一个数据服务器配置信息" };
            }
            Dictionary<string, ServerConfigDTO>? servers = GetServerConfigs();
            if (servers != null)
            {
                servers.Remove(id);

                using (FileStream fs = new FileStream(GetDbPath(), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (TextWriter tw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        tw.Write(JsonSerializer.Serialize(servers));
                        tw.Flush();
                        tw.Close();
                    }
                }
            }
        }
    }
}
