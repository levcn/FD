using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLControls
{
    /// <summary>
    /// 通用事件代理
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <param name="args"></param>
    public delegate void TEventHandler<in TEventArgs>(TEventArgs args);
    public delegate void TEventHandler<in TSender, in TEventArgs1>(TSender sender, TEventArgs1 args);
    public delegate void TEventHandler<in TSender, in TEventArgs1, in TEventArgs2>(TSender sender, TEventArgs1 args, TEventArgs2 args2);
    public delegate void TEventHandler<in TSender, in TEventArgs1, in TEventArgs2, in TEventArgs3>(TSender sender, TEventArgs1 args1, TEventArgs2 args2, TEventArgs3 args3);
    public delegate void TEventHandler<in TSender, in TEventArgs1, in TEventArgs2, in TEventArgs3, in TEventArgs4>(TSender sender, TEventArgs1 args1, TEventArgs2 args2, TEventArgs3 args3, TEventArgs4 args4);
    public delegate void TEventHandler<in TSender, in TEventArgs1, in TEventArgs2, in TEventArgs3, in TEventArgs4, in TEventArgs5>(TSender sender, TEventArgs1 args1, TEventArgs2 args2, TEventArgs3 args3, TEventArgs4 args4, TEventArgs5 args5);
    public delegate void TEventHandler<in TSender, in TEventArgs1, in TEventArgs2, in TEventArgs3, in TEventArgs4, in TEventArgs5, in TEventArgs6>(TSender sender, TEventArgs1 args1, TEventArgs2 args2, TEventArgs3 args3, TEventArgs4 args4, TEventArgs5 args5, TEventArgs6 args6);
    public delegate void TEventHandler<in TSender, in TEventArgs1, in TEventArgs2, in TEventArgs3, in TEventArgs4, in TEventArgs5, in TEventArgs6, in TEventArgs7>(TSender sender, TEventArgs1 args1, TEventArgs2 args2, TEventArgs3 args3, TEventArgs4 args4, TEventArgs5 args5, TEventArgs6 args6, TEventArgs7 args7);
    public delegate void TEventHandler<in TSender, in TEventArgs1, in TEventArgs2, in TEventArgs3, in TEventArgs4, in TEventArgs5, in TEventArgs6, in TEventArgs7, in TEventArgs8>(TSender sender, TEventArgs1 args1, TEventArgs2 args2, TEventArgs3 args3, TEventArgs4 args4, TEventArgs5 args5, TEventArgs6 args6, TEventArgs7 args7, TEventArgs8 args8);
    public delegate void TEventHandler<in TSender, in TEventArgs1, in TEventArgs2, in TEventArgs3, in TEventArgs4, in TEventArgs5, in TEventArgs6, in TEventArgs7, in TEventArgs8, in TEventArgs9>(TSender sender, TEventArgs1 args1, TEventArgs2 args2, TEventArgs3 args3, TEventArgs4 args4, TEventArgs5 args5, TEventArgs6 args6, TEventArgs7 args7, TEventArgs8 args8, TEventArgs9 args9);
}
