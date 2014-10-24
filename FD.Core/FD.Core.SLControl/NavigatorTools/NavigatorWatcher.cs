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
using FD.Core.SLControl.NavigatorTools;
using StaffTrain.FwClass.Controls;
using StaffTrain.FwClass.Reflectors;


namespace StaffTrain.FwClass.NavigatorTools
{
    /// <summary>
    /// 页面导航类,当前显示的页面不在堆栈中,堆栈中只显示已经关闭的历史页面
    /// </summary>
    public class NavigatorWatcher : IDisposable
    {
        public static NavigatorWatcher Current { get; set; }
        public NavigatorWatcher(ContentControl container)
        {
            Container = container;
        }
        public ContentControl Container { get; set; }
        public Stack<NavigatorItem> Items = new Stack<NavigatorItem>();
        private NavigatorItem currentItem;

        /// <summary>
        /// 打开新的页面
        /// </summary>
        /// <param name="container"></param>
        /// <param name="pagePath"></param>
        /// <param name="pageData"></param>
        /// <param name="isMenuClicked"></param>
        /// <param name="isFullScreen"></param>
        /// <param name="parentControls"></param>
        /// <param name="menus"></param>
        /// <param name="o"></param>
        public void GoToNewPage(ContentControl container, string pagePath, object pageData, bool isMenuClicked, bool isFullScreen, IControlContainer parentControls = null, List<IMenuItem> menus = null, object o = null)//ISwitchable page
        {
            
            var objectByName = ReflectionHelper.GetObjectByName(pagePath);
            var obj = objectByName as ISwitchable;
            if (obj != null)
            {
                obj.ParentControl = parentControls;
                obj.StateData = pageData;
                obj.Args = o;
                var item = new NavigatorItem {StateData = pageData, SwitchableItem = obj};
                GatherCurrentItemData();
                //                if (currentItem != null) AddItem(currentItem);
                Switch(container ?? Container, item, menus, o);
            }
            else
            {
                if (objectByName == null)
                {
                    throw new Exception("未找到指定的页面:" + pagePath);
                }
                else
                {
                    throw new Exception("页面未实现ISwitchable接口:" + pagePath);
                    
                }
            }
        }
        /// <summary>
        /// 收集当前页面的数据,保存是数据属性中
        /// </summary>
        private void GatherCurrentItemData()
        {
            if (null != currentItem)
            {
                if (currentItem.SwitchableItem != null)
                {
                    currentItem.StateData = currentItem.SwitchableItem.StateData;
                }
            }
        }
        /// <summary>
        /// 恢复一个页面的数据
        /// </summary>
        /// <param name="ni"></param>
        void RestoreItemData(NavigatorItem ni)
        {
            ni.SwitchableItem.StateData = ni.StateData;
        }
        /// <summary>
        /// 关闭当前页面,打开上一个页面
        /// </summary>
        public void CloseCurrentPage()
        {
            var item = GetItem();
            item = GetItem();
            if (item != null)
            {
                RestoreItemData(item);
                Switch(Container,item);
            }
        }

        /// <summary>
        /// 关闭当前页面,打开上一个页面
        /// </summary>
        public void CloseCurrentPage(NavigatorItem item)
        {
            if (item != null)
            {
                RestoreItemData(item);
                Switch(Container, item);
            }
        }

        /// <summary>
        /// 把容器中的控件清空,放入新的控件
        /// </summary>
        /// <param name="ni"></param>
        public void Switch(NavigatorItem ni)
        {
            Switch(Container, ni);
        }
        /// <summary>
        /// 把容器中的控件清空,放入新的控件
        /// </summary>
        /// <param name="ni"></param>
        public void Switch(ISwitchable ni)
        {
            Switch(Container, new NavigatorItem{StateData = ni.StateData,SwitchableItem = ni});
        }
        /// <summary>
        /// 是否添加到导航队列里
        /// </summary>
        public bool AddToItem = true;
        /// <summary>
        /// 把容器中的控件清空,放入新的控件
        /// </summary>
        /// <param name="ni"></param>
        public void Switch(ContentControl container, NavigatorItem ni, List<IMenuItem> menus = null, object o = null)
        {
            if (ni != null)
            {
                AddItem(ni);
//                container.Children.Clear();
//                container.Children.Add(ni.SwitchableItem as UIElement);
                container.Content = ni.SwitchableItem as UIElement;
//                container.Content = new Button{Content = "123123123123123111111111111"};
                var mp = ni.SwitchableItem as IMenuPage;
                if (menus != null && mp!=null)
                {
                    mp.GotoMenuItem(menus, o);
                }
//                ni.SwitchableItem = null;
                currentItem = ni;
            }
        }
        /// <summary>
        /// 把一个导航页面放入队列
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(NavigatorItem item)
        {
            if (AddToItem) Items.Push(item);
        }
        ///// <summary>
        ///// 把一个导航页面放入队列
        ///// </summary>
        ///// <param name="item"></param>
        //public void AddItem(ISwitchable ni)
        //{
        //    var ni1 = new NavigatorItem {StateData = ni.StateData, SwitchableItem = ni};
        //    AddItem(ni1);
        //}

        /// <summary>
        /// 从队列中取出一个条目
        /// </summary>
        /// <returns></returns>
        public NavigatorItem GetItem()
        {
            if (Items.Count > 0) return Items.Pop();
            return null;
        }
        public void Clear()
        {
            Items.Clear();
        }

        public void Dispose()
        {
            Clear();
            Container = null;
            currentItem = null;
//            Current = null;
        }
    }
}
