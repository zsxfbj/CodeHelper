using System;
using System.Security.Cryptography;
using System.Text;

namespace CodeHelper.Utility
{
    /// <summary>
    /// 字符串扩展类型
    /// </summary>
    public static class StringExtensions
    {
        #region public static string LowerCamelCase(this string input)
        /// <summary>
        /// 获取小驼峰式命名法
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>小驼峰式命名法</returns>
        public static string LowerCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            return input.Substring(0,1).ToLower() + input.Substring(1);
        }
        #endregion public static string LowerCamelCase(this string input)

        #region public static string UpperCamelCase(this string input)
        /// <summary>
        /// 获取大驼峰式命名法
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>大驼峰式命名法</returns>
        public static string UpperCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            return input.Substring(0, 1).ToUpper() + input.Substring(1);
        }
        #endregion public static string UpperCamelCase(this string input)

        #region public static string FromBase64String(this string outstr)
        /// <summary>
        /// FromBase64String
        /// </summary>
        /// <param name="outstr"></param>
        /// <returns></returns>
        public static string FromBase64String(this string outstr)
        {
            if (string.IsNullOrEmpty(outstr)) return string.Empty;
            return Encoding.UTF8.GetString(Convert.FromBase64String(outstr));
        }
        #endregion public static string FromBase64String(this string outstr)

        #region public static string ToBase64String(this string input)
        /// <summary>
        /// ToBase64String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToBase64String(this string input)
        {

            if (string.IsNullOrEmpty(input)) return string.Empty;
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }
        #endregion public static string ToBase64String(this string input)

        #region public static string Md5(this string input)
        /// <summary>
        /// Creates a MD5 hash of the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>hash string</returns>
        public static string Md5(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(bytes);
            return ToHexStrFromByte(hash);
        }
        #endregion public static string Md5(this string input)

        #region public static string Sha256(this string input)
        /// <summary>
        /// Creates a SHA256 hash of the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>hash string</returns>
        public static string Sha256(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return ToHexStrFromByte(hash);
        }
        #endregion public static string Sha256(this string input)

        #region public static string Sha512(this string input)
        /// <summary>
        /// Creates a SHA512 hash of the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>hash string</returns>
        public static string Sha512(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var sha = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return ToHexStrFromByte(hash);
        }
        #endregion public static string Sha512(this string input)

        #region public static string ToHexStrFromByte(this byte[] byteDatas)
        /// <summary>
        /// 字节数组转16进制字符串：空格分隔
        /// </summary>
        /// <param name="byteDatas"></param>
        /// <returns></returns>
        public static string ToHexStrFromByte(this byte[] byteDatas)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < byteDatas.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", byteDatas[i]));
            }
            return builder.ToString().Trim();
        }
        #endregion public static string ToHexStrFromByte(this byte[] byteDatas)

        #region public static string ToHexString(this string plainString, Encoding encode)
        /// <summary>
        /// 用指定编码将给定的字符串转16进制格式字符串
        /// </summary>
        /// <param name="plainString">待转换的字符串</param>
        /// <param name="encode">编码规则</param>
        /// <returns></returns>
        public static string ToHexString(this string plainString, Encoding encode)
        {
            byte[] byteDatas = encode.GetBytes(plainString);
            return ToHexStrFromByte(byteDatas);
        }
        #endregion public static string ToHexString(this string plainString, Encoding encode)

        #region public static byte[] ToBytesFromHexString(this string hexString)
        /// <summary>
        /// 16进制格式字符串转字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] ToBytesFromHexString(this string hexString)
        {
            //以 ' ' 分割字符串，并去掉空字符

            byte[] returnBytes = new byte[hexString.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < hexString.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString[i]);
            }
            return returnBytes;
        }
        #endregion public static byte[] ToBytesFromHexString(this string hexString)

        #region public static string ToStringFromHexString(this string hexString, Encoding encode)
        /// <summary>
        /// 16进制格式字符串转普通文本
        /// </summary>
        /// <param name="hexString">16进制格式字符串</param>
        /// <param name="encode">编码规则</param>
        /// <returns></returns>
        public static string ToStringFromHexString(this string hexString, Encoding encode)
        {
            byte[] _bytes = ToBytesFromHexString(hexString);
            return encode.GetString(_bytes);
        }
        #endregion public static string ToStringFromHexString(this string hexString, Encoding encode)
    }
}
