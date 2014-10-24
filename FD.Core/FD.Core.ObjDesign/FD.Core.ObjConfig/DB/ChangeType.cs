using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STComponse.DB
{
    /// <summary>
    /// 数据库的修改类型
    /// </summary>
    [Flags]
    public enum ChangeType
    {
        /// <summary>
        /// 未改变
        /// </summary>
        None = 0,
        /// <summary>
        /// 改名
        /// <![CDATA[命令:EXEC sp_rename 'SYS_User.OldName','NewName','COLUMN']]>
        /// </summary>
        Rename = 1,

        /// <summary>
        /// 修改字段类型,字段属性等
        /// </summary>
        ChangeAttr = 2,

        /// <summary>
        /// 备注改变
        /// </summary>
        ChangeRemark = 4,

        /// <summary>
        /// 删除
        /// <![CDATA[
        /// if exists(select * from syscolumns where id=object_id('SYS_User') and name='name') 
        /// begin
        ///	alter table SYS_User
        ///	drop column name 
        /// end
        /// ]]>
        /// </summary>
        Delete = 8,

        /// <summary>
        /// 添加
        /// </summary>
        Add = 16,
        /// <summary>
        /// 表中使用,表的字段发生变化
        /// </summary>
        ChangeColumn = 32,
    }
}
