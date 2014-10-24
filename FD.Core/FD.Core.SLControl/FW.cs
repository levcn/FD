using SLControls.Editors;
using SLControls.Errors;
using SLControls.PageConfigs;
using STComponse.CFG;


namespace SLControls
{
    /// <summary>
    ///     框架类
    /// </summary>
    public static class FW
    {
        private static readonly TMessageBox messageBox = new TMessageBox();
        private static readonly ErrorHelper errorHelper = new ErrorHelper();

        /// <summary>
        ///     消息框操作
        /// </summary>
        public static ErrorHelper Error
        {
            get
            {
                return errorHelper;
            }
        }
        private static readonly DBHelper db = new DBHelper();

        /// <summary>
        ///     数据库
        /// </summary>
        public static DBHelper DB
        {
            get
            {
                return db;
            }
        }
        public static FwConfig FwConfig { get; set; }


        /// <summary>
        ///     消息框操作
        /// </summary>
        public static TMessageBox MessageBox
        {
            get
            {
                return messageBox;
            }
        }
    }
}