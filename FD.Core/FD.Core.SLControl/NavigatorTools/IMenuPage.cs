using System.Collections.Generic;


namespace FD.Core.SLControl.NavigatorTools
{
    /// <summary>
    /// 支持菜单的页面
    /// </summary>
    public interface IMenuPage
    {
        void GotoMenuItem(List<IMenuItem> item, object o);
    }
}