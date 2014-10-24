using System.Collections.Generic;
using SLControls.DataClientTools;
using SLControls.Editors;
using StaffTrain.FwClass.Reflectors;
using STComponse.CFG;
using STComponse.ObjectConfig;


namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// 返回的结果
    /// </summary>
    public class ResultData
    {
        public string ErrorMsg; //错误信息
        public int AllRecord; //所有记录数
        public int Record; //受影响记录数
        public string ObjectEntryStr; //实体对象数据
        public string DetailErrorMsg;
        public List<OutputValue> OutputValues { get; set; }

        public T GetEntity<T>()
        {
            return ObjectEntryStr.ToObject<T>();
        }
        public string FriendlyErrorMsg;//友好错误信息
        public PageInfo PageInfo { get; set; }
        public bool HaveError = false;
    }
    /// <summary>
    /// 返回的结果
    /// </summary>
    public class ResultData<T> : ResultData
    {
        public T GetEntity()
        {
            return ObjectEntryStr.ToObject<T>();
        }
    }

    public class ResultDataItem
    {
        public List<OutputValue> OutputValues { get; set; }
        public bool HaveError = false;
        public string ErrorMsg; //错误信息
        public string DetailErrorMsg;
        public object Entity { get; set; }
    }

    /// <summary>
    /// 返回的结果
    /// </summary>
    public class ResultDataItem<T> : ResultDataItem
    {
        public new T Entity
        {
            get
            {
                return (T)base.Entity;
            }
            set
            {
                base.Entity = value;
            }
        }

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

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
    public static class ResultDataEx
    {
        /// <summary>
        /// 把通用结果转成分页数据结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="re"></param>
        /// <returns></returns>
        public static PagerListResult<T> ToPagerList<T>(this ResultData re)
        {
            return new PagerListResult<T>(re.GetEntity<List<T>>(), re.PageInfo);
        }

        public static ResultData<T> ToEntity<T>(this ResultData owner)
        {
            var re  = new ResultData<T>();
            ReflectionHelper.SetAttributeValues(re,owner);
            return re;
        }
    }
}


