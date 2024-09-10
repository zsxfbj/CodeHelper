using System;
using CodeHelper.Enum;
using CodeHelper.IBLL;
using CodeHelper.Model.VO;

namespace CodeHelper.CSharpBLL
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeCreator : ICodeCreator
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="serverConfig"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetCreateReqDTOClass(TableVO table, DbTypes dbType)
        {
            
            
            throw new NotImplementedException();
        }

        public string GetDALClass(TableVO table, DbTypes dbType)
        {
            throw new NotImplementedException();
        }

        public string GetEntityClass(TableVO table, DbTypes dbType)
        {
            throw new NotImplementedException();
        }

        public string GetVOClass(TableVO table, DbTypes dbType)
        {
            throw new NotImplementedException();
        }
    }
}
