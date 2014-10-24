using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.Extends;
using StaffTrain.FwClass.DataClientTools.Configs;
using StaffTrain.FwClass.Serializer;


namespace StaffTrain.FwClass.DataClientTools
{
    public class PostResult
    {
        public PostResult(string e)
        {
//            if (e.Error == null)
            {
//                EventArgs = e;
                ResultString = e;
                if (ResultString[0] == '[')
                {
                    ResultData = ResultString.FromJson<List<ResultData>>();
                }
                else if (ResultString[0] == '{')
                {
                    ResultData = new List<ResultData>{ResultString.FromJson<ResultData>()};
                    if (ResultData[0] != null && ResultData[0].HaveError)
                    {
                        throw new Exception(ResultData[0].ErrorMsg + ResultData[0].DetailErrorMsg);
                    }
                }
//                ResultData = JsonHelper.JsonDeserialize<List<ResultData>>(e);
//                if (ResultData != null) ObjectEntry = JsonHelper.JsonDeserialize<T>(ResultData.ObjectEntryStr);
            }
//            else
//            {
//                ActionSetting.PopupPage.OpenMessagePopup(MessageType.Error, "错误提示", e.Error.Message + e.Error.GetBaseException().ToString(), btnType: ButtonContentType.Ok);
//            }
        }
//        public UploadStringCompletedEventArgs EventArgs { get; set; }
        public string ResultString { get; set; }
        public List<ResultData> ResultData { get; set; }
//        public List<string> ObjectEntry { get; set; }
    }
}
