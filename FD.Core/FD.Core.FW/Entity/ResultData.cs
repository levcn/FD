using System.Collections.Generic;
using Aspose.Words.Lists;


namespace Fw.Entity
{
    /// <summary>
    /// 返回的结果
    /// </summary>
    public class ResultData
    {
        public ResultData()
        {
            OutputValues = new List<OutputValue>();
        }
        public bool HaveError = false;
        public string ErrorMsg; //错误信息
        public string DetailErrorMsg;
        public string FriendlyErrorMsg;//友好错误信息
        public int AllRecord; //所有记录数
        public int Record; //受影响记录数
        public string ObjectEntryStr; //实体对象数据
        public List<OutputValue> OutputValues { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public class OutputValue
    {
        /// <summary>
        /// 值从哪来
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }

        public string Value { get; set; }
    }
}