using CodeHelper.Enum;
using CodeHelper.IBLL;
using CodeHelper.Utility;

namespace CodeHelper.BLLFactory
{
    /// <summary>
    /// 
    /// </summary>
    public class DbCodeCreatorFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static IDbCodeCreator Create(CodeTypes codeType = CodeTypes.CSharp)
        {
            string path = "CodeHelper." + codeType.ToString() + "BLL";
            string className = path + ".DbCodeCreator";
            return MemcacheClient.CreateObject<IDbCodeCreator>(path, className);
        }

    }
}
