using System.Collections.Generic;


namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// 自定义命令
    /// </summary>
    public class CustomCmd
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public List<string> Params { get; set; }
        public List<string> ParamTypes { get; set; }

        /// <summary>
        /// 类名是否使用相对命名空间
        /// </summary>
        public bool IsRelativePath { get; set; }
    }
}