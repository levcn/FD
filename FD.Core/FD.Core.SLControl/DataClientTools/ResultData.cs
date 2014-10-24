using System.Collections.Generic;
using SLControls.DataClientTools;
using SLControls.Editors;
using StaffTrain.FwClass.Reflectors;
using STComponse.CFG;
using STComponse.ObjectConfig;


namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// ���صĽ��
    /// </summary>
    public class ResultData
    {
        public string ErrorMsg; //������Ϣ
        public int AllRecord; //���м�¼��
        public int Record; //��Ӱ���¼��
        public string ObjectEntryStr; //ʵ���������
        public string DetailErrorMsg;
        public List<OutputValue> OutputValues { get; set; }

        public T GetEntity<T>()
        {
            return ObjectEntryStr.ToObject<T>();
        }
        public string FriendlyErrorMsg;//�Ѻô�����Ϣ
        public PageInfo PageInfo { get; set; }
        public bool HaveError = false;
    }
    /// <summary>
    /// ���صĽ��
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
        public string ErrorMsg; //������Ϣ
        public string DetailErrorMsg;
        public object Entity { get; set; }
    }

    /// <summary>
    /// ���صĽ��
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
        /// ֵ������
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ֵ
        /// </summary>
        public string Value { get; set; }
    }
    public static class ResultDataEx
    {
        /// <summary>
        /// ��ͨ�ý��ת�ɷ�ҳ���ݽ��
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


