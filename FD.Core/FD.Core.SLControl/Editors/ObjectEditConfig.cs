using System.Collections.Generic;


namespace SLControls.Editors
{
    /// <summary>
    /// 对象编辑设置
    /// </summary>
    public class ObjectEditorConfig
    {
        public List<ObjectPropertyEditConfig> PropertyEditConfigs { get; set; }
    }

    /// <summary>
    /// 属性编辑设置
    /// </summary>
    public class ObjectPropertyEditConfig
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否可以编辑
        /// </summary>
        public bool? Editable { get; set; }

        /// <summary>
        /// 是否显示出来
        /// </summary>
        public bool? IsDisplay { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderIndex { get; set; }
    }
}
