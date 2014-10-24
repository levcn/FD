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

namespace SLControls.Controls
{
    /// <summary>
    /// 标识属性是可以编辑的
    /// </summary>
    public class EditableAttribute : Attribute
    {
        /// <summary>
        /// 属性分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 属性的显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 保存的属性名,默认使用对象的属性名
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 属性的说明
        /// </summary>
        public string Description { get; set; }
    }
}
