using CodeHelper.Model.VO;

namespace CodeHelper.IBLL
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClassCodeCreator
    {
        /// <summary>
        /// 获取EntityFramework对应的类
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetEFClass(TableVO table);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetVOClass(TableVO table);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetCreateReqDTOClass(TableVO table);
    }
}
