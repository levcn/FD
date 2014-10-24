using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FD.Core.SLControl.Controls;
using SLControls.Cache;
using SLControls.Collection;
using SLControls.DataClientTools;
using SLControls.PageConfigs;
using SLControls.ThemeManages;
using StaffTrain.FwClass.DataClientTools;
using StaffTrain.FwClass.Reflectors;
using Telerik.Windows.Controls;


namespace SLControls.Editors
{
    public abstract class BasePage : LevcnUserControl
    {

        public static readonly DependencyProperty Object1Property =
                DependencyProperty.Register("Object1", typeof (Object), typeof (BasePage), new PropertyMetadata(default(Object)));

        public Object Object1
        {
            get
            {
                return (Object) GetValue(Object1Property);
            }
            set
            {
                SetValue(Object1Property, value);
            }
        }

        public static readonly DependencyProperty Object2Property =
                DependencyProperty.Register("Object2", typeof (Object), typeof (BasePage), new PropertyMetadata(default(Object)));

        public Object Object2
        {
            get
            {
                return (Object) GetValue(Object2Property);
            }
            set
            {
                SetValue(Object2Property, value);
            }
        }

        public static readonly DependencyProperty Object3Property =
                DependencyProperty.Register("Object3", typeof (Object), typeof (BasePage), new PropertyMetadata(default(Object)));

        public Object Object3
        {
            get
            {
                return (Object) GetValue(Object3Property);
            }
            set
            {
                SetValue(Object3Property, value);
            }
        }

        public static readonly DependencyProperty Object4Property =
                DependencyProperty.Register("Object4", typeof (Object), typeof (BasePage), new PropertyMetadata(default(Object)));

        public Object Object4
        {
            get
            {
                return (Object) GetValue(Object4Property);
            }
            set
            {
                SetValue(Object4Property, value);
            }
        }

        public static readonly DependencyProperty Object5Property =
                DependencyProperty.Register("Object5", typeof (object), typeof (BasePage), new PropertyMetadata(default(object)));

        public object Object5
        {
            get
            {
                return (object) GetValue(Object5Property);
            }
            set
            {
                SetValue(Object5Property, value);
            }
        }

        public static readonly DependencyProperty Object6Property =
                DependencyProperty.Register("Object6", typeof (object), typeof (BasePage), new PropertyMetadata(default(object)));

        public object Object6
        {
            get
            {
                return (object) GetValue(Object6Property);
            }
            set
            {
                SetValue(Object6Property, value);
            }
        }


        public BasePage()
        {
            PageObjectManage = new PageObjectManage(this);
            GetPageCacheName();
            Loaded += BasePage_Loaded;
        }
        /// <summary>
        /// 添加当前页面的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddPageCache(string key, object value)
        {
            value = ConvertPageCacheValue(key,value);
            CacheHelper.AddPageCache(GetPageCacheName(),key,value);
        }
        /// <summary>
        /// 提供转换值的方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object ConvertPageCacheValue(string key, object value)
        {
            return value;
        }
        /// <summary>
        /// 移除当前页面的一个缓存
        /// </summary>
        /// <param name="key"></param>
        public void RemovePageCache(string key)
        {
            CacheHelper.RemovePageCache(GetPageCacheName(), key);
        }
        ~BasePage()
        {
            CacheHelper.ClearPageCache(GetPageCacheName());//页面加载时清除页面缓存
        }

        public string cacheName;
        /// <summary>
        /// 返回当前页面的缓存Key(默认取Name,如果为空则取当前页面类名)
        /// </summary>
        /// <returns></returns>
        public string GetPageCacheName()
        {
            if (cacheName != null) return cacheName;
            if (!string.IsNullOrEmpty(this.Name))
            {
                cacheName = Name;
            }
            else
            {
                cacheName = GetType().Name;
            }
            return cacheName;
        }
        void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsDesign())
            {
                
                InitMultiControl();
                ApplyReadServerPageConfig();
            }
        }

        protected async void ApplyReadServerPageConfig()
        {
            await ApplyReadServerPageConfig(this);
        }
        /// <summary>
        /// 读取服务器上的设置,并应用到控件
        /// </summary>
        protected async Task<int> ApplyReadServerPageConfig(DependencyObject page)
        {
            PageConfig = await PageConfigManage.GetPageConfig(ThemeFileManage.CurrentTheme.Code ?? "Default", PageCode);
            ApplyPageConfig(page,PageConfig);
            return 1;
        }

        /// <summary>
        /// 页面对象管理器
        /// </summary>
        public PageObjectManage PageObjectManage { get; private set; }

        /// <summary>
        /// 页面对象列表
        /// </summary>
        public TList<PageObject> PageObjects
        {
            get
            {
                return PageObjectManage.PageObjects;
            }
        }

        /// <summary>
        ///     页面配置
        /// </summary>
        public PageConfig PageConfig { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DesignerProperties.IsInDesignTool)
            {
                OnLoadDesignData();
            }
        }

        /// <summary>
        /// 发送数据到服务器
        /// </summary>
        public async void Post(BaseMultiControl control)
        {
//            typeof(ActionHelper).GetMethod("Select",)
//            var list = await ActionHelper.Select();
//            var objectName = control.ControlConfig.PageObject;
//            if(string.IsNullOrEmpty(objectName))throw new Exception("控件指定的页面对象名为空");
//            SetObjectValue(objectName,list);
            //TODO //tht修改,时间2014-01-04 10:58,原因:

        }

        
        /// <summary>
        /// 把指定的对象保存到页面对象列表中
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="obj"></param>
        private void SetObjectValue(string propName, object obj)
        {
            var p = GetType().GetProperty(propName);
            if (p == null) throw new Exception(string.Format("在页面中未找到指定名称的对象({0})",propName));
            p.SetValue(this,obj,null);
        }
        /// <summary>
        /// 初始化页面复杂控件的绑定对象
        /// </summary>
        protected void InitControlObject()
        {
            var types = PageObjectManage.DefaultAssembly.GetTypes().ToList();
            var controls = GetMultiControls(this);
            PageConfig.ControlConfigs.ForEach(w =>
            {
                var obj = PageObjectManage.FirstOrDefault(z => z.Code == w.ObjectCode);
                if (obj != null)
                {
                    var type = types.FirstOrDefault(z => z.Name == obj.Code);
                    if (type != null)
                    {
                        var nObj = ReflectionHelper.GetObject(type);
                        if (nObj != null)
                        {
                            obj.Object = nObj;
                            var c = controls.FirstOrDefault(z => z.Name == obj.Code);
                            if (c != null)
                            {
                                c.DataContext = nObj;
                                c.Bind();
                            }
                        }
                    }
                }
                //w.ObjectCode
            });
        }

        /// <summary>
        /// 页面代码
        /// </summary>
        public virtual string PageCode
        {
            get
            {
                return GetType().Name;
            }
        }

        /// <summary>
        /// 根据Name返回指定的控件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected BaseMultiControl GetControlByName(string name)
        {
            return GetMultiControls(this).FirstOrDefault(w => w.GetName() == name);
        }
        protected List<BaseMultiControl> GetMultiControls(DependencyObject item)
        {
            var childs = item.ChildrenOfType<BaseMultiControl>().ToList();
            return childs;
        }
        protected void InitMultiControl(DependencyObject item)
        {
            GetMultiControls(item).ForEach(w =>
            {
                w.BasePage = this;
                w.EditEnd += w_EditEnd;
            });
        }

        /// <summary>
        /// 初始化页面中的复杂控件
        /// </summary>
        protected void InitMultiControl()
        {
            InitMultiControl(this);
        }

        void w_EditEnd(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
//            var control = (sender as BaseMultiControl);
            BaseMultiControl control = (sender as BaseMultiControl);
            if (control!=null && !control.IsSub)
            {
                control.LoadPropConfig();
                UploadPageConfig();
            }
        }

        protected bool IsDesign()
        {
            return DesignerProperties.IsInDesignTool;
        }

        public virtual void OnLoadDesignData()
        {}

        /// <summary>
        /// 保存当前页面的设置
        /// </summary>
        public void SaveCurrentPageConfig()
        {
            PageConfig = PageConfigManage.SavePageConfig(this);
        }

        /// <summary>
        /// 保存当前页面的设置到服务器上
        /// </summary>
        public async void UploadPageConfig()
        {
            SaveCurrentPageConfig();
            await PageConfigManage.SavePageConfigToServer(PageConfig);
        }

        /// <summary>
        /// 把配置应用到控件上
        /// </summary>
        public void ApplyPageConfig()
        {
            ApplyPageConfig(PageConfig);
        }

        public void ApplyPageConfig(PageConfig pc)
        {
            ApplyPageConfig(this, pc);
        }
        /// <summary>
        /// 把配置应用到控件上
        /// </summary>
        /// <param name="pc"></param>
        
        public void ApplyPageConfig(DependencyObject page ,PageConfig pc)
        {
            PageConfigManage.SetPageConfig(page, pc);
        }

        /// <summary>
        /// 读取主题控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetThemeControl<T>() where T : class
        {
            return await GetThemeControl<T>(ThemeFileManage.CurrentTheme.Code);
        }

        /// <summary>
        /// 读取主题控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="themeName"></param>
        /// <returns></returns>
        public async Task<T> GetThemeControl<T>(string themeName) where T : class
        {
            return await GetThemeControl<T>(themeName, GetType().Name);
        }

        /// <summary>
        /// 读取主题控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="themeName">主题名称</param>
        /// <param name="pageName">页面名称</param>
        /// <returns></returns>
        public async Task<T> GetThemeControl<T>(string themeName, string pageName) where T : class
        {
            return await ThemeFileManage.GetThemeControl<T>(themeName, pageName);
        }
    }
}