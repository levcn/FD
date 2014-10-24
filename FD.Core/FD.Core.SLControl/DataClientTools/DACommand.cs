namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// dataAccessCommand
    /// </summary>
    public class DACommand<T>
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public T Item { get; set; }
        //        /// <summary>
        //        /// 实体类型
        //        /// </summary>
        //        public object Item { get; set; }
        //        /// <summary>
        //        /// 类型
        //        /// </summary>
        //        public Type EntityType { get; set; }

        public ActionType ActionType { get; set; }
    }
}