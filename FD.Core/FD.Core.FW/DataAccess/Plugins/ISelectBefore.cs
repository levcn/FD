using System;
using Fw.Entity;


namespace Fw.DataAccess.Plugins
{
    /// <summary>
    /// 查询之前的插件
    /// </summary>
    public interface ISelectBefore: IDataAccessPlugin
    {
        void Execute(Type type, ActionCommand cmd, BaseDataAccess access);
    }
}