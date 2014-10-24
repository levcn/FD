using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace StaffTrain.FwClass.Controls
{
    /// <summary>
    /// 位置
    /// </summary>
    [Flags]
    public enum Position
    {
        /// <summary>
        /// 横向居左
        /// </summary>
        HLeft = 1,

        /// <summary>
        /// 横向居中
        /// </summary>
        HCenter = 2,

        /// <summary>
        /// 横向居右
        /// </summary>
        HRight = 4,

        /// <summary>
        /// 垂直居上
        /// </summary>
        VTop = 8,

        /// <summary>
        /// 垂直居中
        /// </summary>
        VCenter = 16,

        /// <summary>
        /// 垂直居下
        /// </summary>
        VBottom = 32,
        /// <summary>
        /// 左上
        /// </summary>
        LeftTop = HLeft |VTop,

        /// <summary>
        /// 左中
        /// </summary>
        LeftCenter = HLeft | VCenter,

        /// <summary>
        /// 左下
        /// </summary>
        LeftBottom = HLeft | VBottom,

        /// <summary>
        /// 中上
        /// </summary>
        CenterTop = HCenter | VTop,

        /// <summary>
        /// 正中间
        /// </summary>
        Center = HCenter | VCenter,

        /// <summary>
        /// 下中
        /// </summary>
        BottomCenter = HCenter | VBottom,

        /// <summary>
        /// 右上
        /// </summary>
        RightTop = HRight | VTop,

        /// <summary>
        /// 右中
        /// </summary>
        RightCenter = HRight | VCenter,

        /// <summary>
        /// 右下
        /// </summary>
        RightBottom = HRight | VBottom,

    }
}
