using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fw.Entity;


namespace Fw.DataAccess.Plugins
{
    /// <summary>
    /// 查询之前后的插件
    /// </summary>
    public interface ISelectAfter : IDataAccessPlugin
    {
        void Execute(IList data, Type type, ActionCommand cmd, BaseDataAccess access);
        void Execute(IList data, ActionCommand cmd, BaseDataAccess access);
    }

}
