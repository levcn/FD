using System;
using SLControls.DataClientTools;
using StaffTrain.FwClass.NavigatorTools;


namespace StaffTrain.FwClass.DataClientTools.Configs
{
    public class ActionSetting
    {
        /// <summary>
        /// 数据访问接口的地址
        /// </summary>
        public static Uri DataAccessUri
        {
            get
            {
                return new Uri(DataAccess.BaseUri+ "Action/ActionHandler.ashx");
            }
        }

//        /// <summary>
//        /// 实体类在服务器端的命名空间
//        /// </summary>
//        public static string ServerEntityNamespace = "StaffTrain.Web.Entity.";

        /// <summary>
        /// 上传文件的地址
        /// </summary>
        public static Uri UploadUri
        {
            get
            {
                return new Uri(DataAccess.BaseUri+ "Action/FileUpload.ashx");
            }
        }

//        static IPopupable popupPage = null;
//        public static IPopupable PopupPage{get
//        {
//            return popupPage;
//        }
//        set
//        {
//            popupPage = value;
//        }}
    }
}
