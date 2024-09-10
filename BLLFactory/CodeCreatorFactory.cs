using CodeHelper.Enum;
using CodeHelper.IBLL;
using CodeHelper.Utility;

namespace CodeHelper.BLLFactory
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeCreatorFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static ICodeCreator Create(CodeTypes codeType = CodeTypes.CSharp)
        {
            string path = "CodeHelper." + codeType.ToString() + "BLL";
            string className = path + ".CodeCreator";
            return MemcacheClient.CreateObject<ICodeCreator>(path, className);
        }

    }
}
