using System.ComponentModel;
using System.Windows.Controls;


namespace SLControls.Editors
{
    public class BaseControl : ContentControl
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DesignerProperties.IsInDesignTool)
            {
                OnLoadDesignData();
            }
        }

        protected bool IsDesign()
        {
            return DesignerProperties.IsInDesignTool;
        }

        public virtual void OnLoadDesignData()
        {}
    }

    public class BaseContentControl : ContentControl
    {
        protected bool IsDesign()
        {
            return DesignerProperties.IsInDesignTool;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DesignerProperties.IsInDesignTool)
            {
                OnLoadDesignData();
            }
        }

        public virtual void OnLoadDesignData()
        {}
    }

    public class BaseItemsControl : ItemsControl
    {
        protected bool IsDesign()
        {
            return DesignerProperties.IsInDesignTool;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DesignerProperties.IsInDesignTool)
            {
                OnLoadDesignData();
            }
        }

        public virtual void OnLoadDesignData()
        {}
    }

    /// <summary>
    ///     复合控件的状态
    /// </summary>
    public enum EditState
    {
        /// <summary>
        ///     运行模式
        /// </summary>
        Run,

        /// <summary>
        ///     一般编辑模式
        /// </summary>
        Normal,

        /// <summary>
        ///     鼠标移上
        /// </summary>
        Hover,

        /// <summary>
        ///     鼠标点击获得焦点
        /// </summary>
        Focus,
    }
}