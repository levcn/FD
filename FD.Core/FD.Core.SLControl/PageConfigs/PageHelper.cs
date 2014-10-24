using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.DataClientTools;


namespace SLControls.PageConfigs
{
    public class PageHelper
    {
        
    }

    public class DBHelper
    {
        /// <summary>
        /// 返回数据库的链接状态
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetDBConnection()
        {
            return await ActionHelper.CustomRequest<bool>("PageAction", "DBConnection", new object[] {  });
        }
    }

}
