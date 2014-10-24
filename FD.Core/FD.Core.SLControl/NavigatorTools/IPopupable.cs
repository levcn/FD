using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using StaffTrain.FwClass.DataClientTools;


namespace StaffTrain.FwClass.NavigatorTools
{
//    public interface IPopupable
//    {
//        void OpenFullWindowsShow(UIElement element, bool allowClose = true);
//
//        void OpenFullWindowsShow(UIElement element, List<Button> rightTopButtons, bool allowClose = true);
//        void CloseFullWindowsShow();
//        void PendantVisibility(Visibility pVisibility);//是否显示右侧挂件
//        void OpenWattingPopup();
//
//        void OpenMessagePopup(MessageType messageType, string title, string message, Action<Result> action = null, ButtonContentType btnType = ButtonContentType.OkCancel);
//
//        void ClosePopup();
//
//        Action ClosedPopup { get; set; }  
//    }
    /// <summary>
    /// 信息类型
    /// </summary>
    public enum MessageType
    {
        Infomation,
        Warning,
        Error,
    }
    /// <summary>
    /// 按钮类型
    /// </summary>
    public enum ButtonContentType
    {
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel,
    }
    public enum Result
    {
        Yes, No, None
    }
}
