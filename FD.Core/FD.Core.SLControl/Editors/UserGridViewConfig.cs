using System.Collections.Generic;


namespace SLControls.Editors
{
    /// <summary>
    /// 表格设置
    /// </summary>
    public class UserGridViewConfig
    {
        public List<ColumnConfig> Columns { get; set; }

        /// <summary>
        /// 是否显示分页信息
        /// </summary>
        public bool? ShowPager { get; set; }

        /// <summary>
        /// 编辑的设置
        /// </summary>
        public ObjectEditorConfig EditorConfig { get; set; }

        /// <summary>
        /// 显示工具栏按钮
        /// </summary>
        public bool? ShowToolBar { get; set; }

        /// <summary>
        /// 搜索设置
        /// </summary>
        public SearchConfig SearchConfig{get;set;}
        public int? Width{get;set;}
        public int? Height{get;set;}
    }
    /// <summary>
    /// 对象搜索设置
    /// </summary>
    public class SearchConfig
    {
        /// <summary>
        /// 搜索条目
        /// </summary>
        public List<SearchItem> Items { get; set; }

        /// <summary>
        /// 对象名,用于生成表名
        /// </summary>
        public string ObjectName { get; set; }
    }

    /// <summary>
    /// 搜索条件
    /// </summary>
    public class SearchItem
    {
        /// <summary>
        /// 显示名(标签名)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName{ get; set; }

        /// <summary>
        /// 搜索类型(1文本,2下拉框,3日期[带范围],4数字[带范围])
        /// </summary>
        public int SearchType{ get; set; }

        /// <summary>
        /// 控件的宽度(可以不设置)
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// 如果是下拉框,指定数据源的名称
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayIndex { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsCancel{ get; set; }
    }

    public class ColumnConfig
    {
        /// <summary>
        /// 列头名称
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// 绑定对象的属性名
        /// </summary>
        public string DataMemberBindingPath { get; set; }

        /// <summary>
        /// 列宽
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// 显示列显示位置
        /// </summary>
        public int? DisplayIndex { get; set; }
    }
}
