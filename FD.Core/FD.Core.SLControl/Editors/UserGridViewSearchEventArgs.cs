using System.Collections.Generic;
using SLFW.DB;


namespace SLControls.Editors
{
    public class UserGridViewSearchEventArgs
    {
        /// <summary>
        /// 搜索条件列表
        /// </summary>
        public List<SearchEntity> SearchEntities { get; set; }
    }
}
