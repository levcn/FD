using System.Collections.Generic;


namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// 命令
    /// </summary>
    public class ActionCommand
    {
        public ActionCommand()
        {
            Entity = new Entity();
            Operator = new Operator();
            Parameter = new Parameter();
        }
        /// <summary>
        /// 实体
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public Operator Operator { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public Parameter Parameter { get; set; }
    }

    /// <summary>
    /// 操作实体
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// 操作对象的序列化字符串
        /// </summary>
        public string ActionObjectEntryStr { get; set; } // 


        /// <summary>
        /// 操作对象的实体类名称
        /// </summary>
        public string ActionObjectName { get; set; } // 
    }

    public class Operator
    {
        public Operator()
        {
            Version = 2;
        }
        /// <summary>
        /// 是否是自定义命令
        /// </summary>
        public bool IsCustomCmd { get; set; }

        /// <summary>
        /// 自定义命令
        /// </summary>
        public CustomCmd CustomCmd { get; set; }

        /// <summary>
        /// 操作类型(查询、删除、修改)select,update,delete,insert
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// 数据操作的版本,1:原始版本,2:使用配置文件的版本
        /// </summary>
        public int Version { get; set; }
    }

    public class Parameter
    {
        /// <summary>
        /// 存储过程参数
        /// </summary>
        public StoredProcedureParams StoredProcedureParams { get; set; }

        /// <summary>
        /// 操作参数
        /// </summary>
        public SelectAcionParams SelectAcionParams { get; set; } //

        
        /// <summary>
        /// 字段过虑
        /// </summary>
        public FieldFilter FieldFilter { get; set; }
    }
//    /// <summary>
//    /// 接收客户端发来的命令
//    /// </summary>
//    public class ActionCmd
//    {
//        /// <summary>
//        /// 是否是自定义命令
//        /// </summary>
//        public bool IsCustomCmd { get; set; }
//
//        /// <summary>
//        /// 自定义命令
//        /// </summary>
//        public CustomCmd CustomCmd { get; set; }
//
//        /// <summary>
//        /// 操作类型(查询、删除、修改)select,update,delete,insert
//        /// </summary>
//        public string ActionType { get; set; }
//
//        /// <summary>
//        /// 存储过程参数
//        /// </summary>
//        public StoredProcedureParams StoredProcedureParams { get; set; }
//
//        /// <summary>
//        /// 操作参数
//        /// </summary>
//        public SelectAcionParams SelectAcionParams { get; set; } //
//
//        /// <summary>
//        /// 操作对象的实体类名称
//        /// </summary>
//        public string ActionObjectName { get; set; } // 
//
//        /// <summary>
//        /// 操作对象的序列化字符串
//        /// </summary>
//        public string ActionObjectEntryStr { get; set; } // 
//
//        /// <summary>
//        /// 字段过虑
//        /// </summary>
//        public FieldFilter FieldFilter { get; set; }
//    }

    public class StoredProcedureParams
    {
        public string StoredProcedureName { get; set; }
//        public List<string> ParamsName { get; set; }
//        public List<string> ParamsValue { get; set; }
        public List<STParamete> Params { get; set; }
        public List<OutputValue> OutputValues { get; set; }
    }

    /// <summary>
    /// 选择的时候指定列
    /// </summary>
    public class FieldFilter
    {
        /// <summary>
        /// 如果黑名单存在,则黑名单里的字段不搜索
        /// </summary>
        public string[] BlackNames { get; set; }

        /// <summary>
        /// 如果白名单存在,则只搜索白名单字段,如果一个字段(x)同时存在于白名单和黑名单中,则以黑名单为准不搜索该字段
        /// </summary>
        public string[] WhiteNames { get; set; }
    }
}
