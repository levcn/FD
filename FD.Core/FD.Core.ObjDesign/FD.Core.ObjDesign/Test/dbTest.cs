using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fw.DataAccess;
using Fw.Entity;
using ProjectCreater.Settings;
using StaffTrain.Entity;
using STComponse.CFG;


namespace ProjectCreater.Test
{
    public class dbTest
    {
        public static void MMM()
        {
            var current = new MsSqlDataAccess
            {
                ConnectionString = string.Format("Data Source = {0};User Id = {1};Password = {2};Initial Catalog={3}"
                    , "192.168.32.1"
                    , "sa"
                    , "zdfd"
                    ,"tht2")
                    
            };
            var a = VersionManage.Current.CurrentVersion.DataObjects.FirstOrDefault(w => w.ObjectCode == "SYS_User");
            PageInfo pi = new PageInfo {
                PageIndex = 1,PageSize = 10
            };
            var list = current.Select(typeof(dbTest).Assembly, a, VersionManage.Current.CurrentVersion, null, null, ref pi) as List<SYS_User>;
//            list[0].ID = Guid.NewGuid();
            list[0].Rolus = null;
            current.Update(VersionManage.Current.CurrentVersion, list[0]);
        }
    }
}
