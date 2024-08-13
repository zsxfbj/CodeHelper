using System;
using System.Reflection;
using System.Runtime.Caching;
using System.Text.Json;

namespace CodeHelper.Utility
{
    /// <summary>
    /// 框架自带缓存业务类
    /// </summary>
    public class MemcacheClient
    {
        /// <summary>
        /// 使用默认的数据缓存库
        /// </summary>
        private readonly static MemoryCache DefaultCache = MemoryCache.Default;

        /// <summary>
        /// 工厂实例化缓存
        /// </summary>
        private readonly static MemoryCache FactoryCache = new MemoryCache("FactoryCache");

        /// <summary>
        /// 类反射，自动创建，有则直接返回
        /// </summary>
        public static T CreateObject<T>(string path, string typeName)
        {
            if (IsExist(path))
            {                
                return (T)FactoryCache.Get(typeName);
            }
            T objType = (T)Assembly.Load(path).CreateInstance(typeName);
            if (objType == null)
            {
                throw new NotImplementedException("请完成" + typeName + "类的编写");
            }
            FactoryCache.Set(typeName, objType, DateTimeOffset.MaxValue);
            return objType;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetValue<T>(string key, T value)
        {
            DefaultCache.Set(key, JsonSerializer.Serialize(value), new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.MaxValue, SlidingExpiration = TimeSpan.FromDays(1) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        public static void SetValue<T>(string key, T value, DateTimeOffset expireTime)
        {
            DefaultCache.Set(key, JsonSerializer.Serialize(value), new CacheItemPolicy { AbsoluteExpiration = expireTime });
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(string key)
        {
            if (!IsExist(key))
            {
                return default!;
            }
            object obj = DefaultCache.Get(key);
            if(obj == null)
            {
                return default!;
            }
            return JsonSerializer.Deserialize<T>(obj.ToString()!)!;
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            DefaultCache.Remove(key);
        }

        /// <summary>
        /// 判断是否存在key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExist(string key) { 
            return DefaultCache.Contains(key);
        }
    }
}
