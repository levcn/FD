using System;
using System.Reflection;
using SLControls.Collection;


namespace SLControls.Editors
{
    /// <summary>
    /// 页面中使用的对象实体类
    /// </summary>
    public class PageObject
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 对象名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对象
        /// </summary>
        public object Object { get; set; }
    }

    /// <summary>
    /// 对象管理器
    /// </summary>
    public class PageObjectManage
    {
        public static Assembly DefaultAssembly;
        public PageObjectManage(BasePage page)
        {
            Page = page;
        }
        public BasePage Page { get; private set; }
        public TList<PageObject> PageObjects { get; set; }

        public PageObject FirstOrDefault(Func<PageObject, bool> predicate)
        {
            return PageObjects.FirstOrDefault(predicate);
        }
        public void Contains(PageObject obj)
        {
            
            PageObjects.Contains(obj);
        }
        public void RemoveObject(PageObject obj)
        {
            PageObjects.Remove(obj);
        }
        public void AddObject(PageObject obj)
        {
            PageObjects.Add(obj);
        }
    }
}
