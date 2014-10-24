using System;


namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// Post设置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionConfig
    {
        public ActionConfig()
        {
//            Waitting = null;
            Exception = null;
        }
//
//        /// <summary>
//        /// 等待方法
//        /// </summary>
//        public Action<WattingConfig> Waitting { get; set; }
//
//        /// <summary>
//        /// 等待设置
//        /// </summary>
//        public WattingConfig WaittingConfig { get; set; }

        /// <summary>
        /// 出错的回调
        /// </summary>
        public Action<ResultData> Exception { get; set; }

//        /// <summary>
//        /// 执行查询完成之后的回调
//        /// </summary>
//        public Action<PostResult<T>> PostBackMethod { get; set; }
    }
}
