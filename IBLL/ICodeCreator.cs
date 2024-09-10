using CodeHelper.Enum;
using CodeHelper.Model.VO;

namespace CodeHelper.IBLL
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeCreator
    {
        /// <summary>
        /// 获取EntityFramework对应的类
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetEntityClass(TableVO table, DbTypes dbType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="serverConfig"></param>
        /// <returns></returns>
        string GetDALClass(TableVO table, DbTypes dbType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetVOClass(TableVO table, DbTypes dbType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetCreateReqDTOClass (TableVO table, DbTypes dbType);
    }
}
