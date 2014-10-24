using System;
using System.Windows.Data;
//using StaffTrain.FwClass.Controls.EditItems;


namespace StaffTrain.FwClass.UserAttributes
{
    /// <summary>
    /// 字段信息定义
    /// </summary>
    public class LevcnColumnAttribute : Attribute
    {
        /// <summary>
        /// 添加时可以改变,确定以后不能修改
        /// </summary>
        public bool InitialEdit { get; set; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 当前字段属性,默认取属性名为字段名 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名,如果页面不需要编辑则可以不写
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否忽略,不从数据库中查询
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 在编辑页面上的显示顺序
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 是否可以编辑
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// 下拉框是否手工输入
        /// </summary>
        public bool CustomInput { get; set; }

        /// <summary>
        /// 在编辑页面是否隐藏
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// 该属性是否用于关联其它实体的外键
        /// </summary>
        public bool IsFlag { get; set; }

        /// <summary>
        /// 当前属性是否是显示的名称属性
        /// </summary>
        public bool IsDisplayName { get; set; }

      
        /// <summary>
        /// 使用编辑控件的类型
        /// </summary>
        public Type ControlType { get; set; }
        /// <summary>
        /// 文件上传之后存放的处理类型
        /// 决定文件上传之后的处理及存放机制
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 允许上传的文件类型
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 如果使用下拉框,设置下拉框的数据源
        /// </summary>
        public string ListSourceName { get; set; }

        /// <summary>
        /// 设置数据源的数据类型
        /// </summary>
        public Type ListSourceType { get; set; }

        /// <summary>
        /// 要打开窗口时,窗口的高
        /// </summary>
        public int WindowsHeight { get; set; }

        /// <summary>
        /// 要打开窗口时,窗口的宽
        /// </summary>
        public int WindowsWidth { get; set; }

        /// <summary>
        /// 要打开窗口时,窗口是否包含ScrollViewer
        /// </summary>
        public bool WindowNoScroll { get; set; }

        /// <summary>
        /// 使用选择控件时,选择控件的名称
        /// </summary>
        public string SelectControlName { get; set; }

        public string FilePath { get; set; }

        /// <summary>
        /// 是否是加密字符串
        /// </summary>
        public bool IsPassword { get; set; }

        /// <summary>
        /// 文字框是否可以换行
        /// </summary>
        public bool AcceptsReturn { get; set; }

        /// <summary>
        /// 文本框的高度
        /// </summary>
        public int TextBoxHeight { get; set; }

        /// <summary>
        /// 文本框的高度
        /// </summary>
        public int TextBoxWidth { get; set; }

        /// <summary>
        /// 当前ID对应另外一个属性的类型
        /// </summary>
        public Type OtherPropType { get; set; }

        /// <summary>
        /// 当前ID对应另外一个属性的名称
        /// 列表选择时传递的参数的属性名称 
        /// 具体应用参照 人员角色选择及试卷课程选择
        /// </summary>
        public string OtherPropName { get; set; }

        /// <summary>
        /// 当前字段是否表示,是 否
        /// </summary>
        public bool YesNo { get; set; }

        /// <summary>
        /// 原始文件名
        /// </summary>
        public string OriginalFileName { get; set; }

        public string[] ColumnBinding { get; set; }

        public string[] HeaderText { get; set; }

        public Type Converter { get; set; }

        /// <summary>
        /// 列表是否是叠加选择的数据
        /// </summary>
        public bool Addition { get; set; }

        /// <summary>
        /// 是否显示日期的时间
        /// </summary>
        public bool ShowTime { get; set; }

        /// <summary>
        /// 选择页面需要的参数所对应的实体类的属性
        /// EG: CourseID,PassScore
        /// </summary>
        public string ParamPropNames { get; set; }

        #region RichTextBox

        /// <summary>
        /// 保存文件
        /// </summary>
        public Type SaveFileAction { get; set; }

//        /// <summary>
//        /// 返回文件的相对URL的属性
//        /// </summary>
//        public IValueConverter FilePathConverter { get; set; }

        #endregion

    }
}