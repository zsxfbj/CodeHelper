using CodeHelper.Enum;
using CodeHelper.IDAL;
using CodeHelper.Utility;

namespace CodeHelper.DALFactory
{
    /// <summary>
    /// 
    /// </summary>
    public class DbAccessFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static IDbAccess Create(DbTypes dbType = DbTypes.SQLServer)
        {
            string path = "CodeHelper." + dbType.ToString() + "DAL";
            string className = path + ".DbAccess";
            return MemcacheClient.CreateObject<IDbAccess>(path, className);
        }
    }
}
