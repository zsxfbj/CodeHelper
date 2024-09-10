using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CodeHelper.Enum
{
    /// <summary>
    /// 枚举类型扩展方法
    /// </summary>
    public static class EnumerableExtensions
    {
        private static ConcurrentDictionary<System.Enum, string> _concurrentDictionary = new ConcurrentDictionary<System.Enum, string>();

        private static ConcurrentDictionary<Type, Dictionary<string, string>> _concurrentDicDictionary = new ConcurrentDictionary<Type, Dictionary<string, string>>();

        /// <summary>
        /// 枚举的类型个数
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static int Count(this System.Collections.IEnumerable items)
        {
            var result = 0;

            if (items != default)
            {
                var enumerator = items.GetEnumerator();
                result = enumerator.Count();
            }

            return result;
        }

        /// <summary>
        /// 计算枚举的个数
        /// </summary>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        private static int Count(this System.Collections.IEnumerator enumerator)
        {
            var result = 0;

            if (enumerator != default)
            {
                while (enumerator.MoveNext())
                {
                    result++;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source is null || !source.Any();
        }

        private static string GetDescription(FieldInfo field)
        {
            var att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }

        /// <summary>
        /// 获取枚举的描述信息(Descripion)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        public static string GetDescription(this System.Enum @this)
        {
            return _concurrentDictionary.GetOrAdd(@this, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                //如果field为null则应该是组合位域值，
                return field == null ? key.GetDescriptions() : GetDescription(field);
            });
        }

        /// <summary>
        /// 获取枚举的说明
        /// </summary>
        /// <param name="split">位枚举的分割符号（仅对位枚举有作用）</param>
        public static string GetDescriptions(this System.Enum em, string split = ",")
        {
            var names = em.ToString().Split(',');
            string[] res = new string[names.Length];
            var type = em.GetType();
            for (int i = 0; i < names.Length; i++)
            {
                var field = type.GetField(names[i].Trim());
                if (field == null) continue;
                res[i] = GetDescription(field);
            }
            return string.Join(split, res);
        }

        /// <summary>
        /// 获取特性 (DisplayAttribute) 的名称；如果未使用该特性，则返回枚举的名称。
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayName(this System.Enum enumValue)
        {
            string? displayName = enumValue.ToString();
            FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo != null)
            {
                DisplayAttribute[]? attrs = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    displayName = attrs[0].Name;
                }
            }
            return (displayName != null ? displayName : "");
        }

        /// <summary>
        /// 获取特性 (DisplayAttribute) 的说明；如果未使用该特性，则返回枚举的名称。
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayDescription(this System.Enum enumValue)
        {
            string? descption = enumValue.ToString();
            FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo != null)
            {
                DisplayAttribute[]? attrs = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    descption = attrs[0].Description;
                }
            }

            return (descption != null ? descption : "");
        }
    }
}
