using System.Collections.ObjectModel;


namespace ProjectCreater.Settings
{
    /// <summary>
    /// 生成C#代码的样式
    /// </summary>
    public class CodeStyle
    {
        public CodeStyle()
        {
            TagPix = new ObservableCollection<string>();
        }

        /// <summary>
        /// 生成属性时,需要过虑的头
        /// </summary>
        public ObservableCollection<string> TagPix { get; set; }

        /// <summary>
        /// 是否过虑头信息
        /// </summary>
        public bool FilterTagPix { get; set; }

        /// <summary>
        /// 代码保存地址
        /// </summary>
        public string SavePath { get; set; }
        
    }
}