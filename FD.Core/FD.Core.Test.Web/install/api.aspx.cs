using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fw.DataAccess;
using Fw.Extends;
using Fw.Reflection;
using StaffTrain.SVFw.SQLScript;


namespace FD.Core.Test.Web.install
{
    public partial class api : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var action = Request["Action"];
            Result r = new Result();
            try
            {
                if (action == "GetDBs")
                {
                    r.Data = GetDBs();
                } 
                if (action == "Create")
                {
                    r.Data = Create();
                }
                r.Success = 1;
            }
            catch (Exception ee)
            {
                r.Success = 0;
                r.ErrorMessage = ee.Message;
            }
            Response.Write(r.ToJson());
            Response.End();
        }
        /// <summary>
        /// 建表和其它
        /// </summary>
        /// <returns></returns>
        private string Create()
        {
            var ServerName = Request["ServerName"];
            var ServerUID = Request["ServerUID"];
            var ServerPWD = Request["ServerPWD"];
            var DBName = Request["DBName"];
            var scripts = SQLScriptManage.Current.GetScripts();
            var access = GetMsSqlDataAccess(ServerName, ServerUID, ServerPWD, DBName);
            scripts.ForEach(w =>
            {
                access.ExecuteNonQuery(new SqlCommand(w));
            });
            return "";
        }
        private string GetDBs()
        {
            var ServerName = Request["ServerName"];
            var ServerUID = Request["ServerUID"];
            var ServerPWD = Request["ServerPWD"];
            //
            var access = GetMsSqlDataAccess(ServerName, ServerUID, ServerPWD, "master");
            var t = access.ExecuteDataTable(new SqlCommand("select name as T1_name from master..sysdatabases"));
            var list = EntityConvertor.GetEntity<T1>(t);
            return list.Select(w => w.name).ToList().ToJson();
        }

        private static MsSqlDataAccess GetMsSqlDataAccess(string ServerName, string ServerUID, string ServerPWD,string dbName)
        {
            return DataAccessFactory.GetMsSqlDataAccess(
                    string.Format("Data Source={0};Initial Catalog={3};User ID={1};PWD={2}", ServerName, ServerUID, ServerPWD, dbName));
        }
    }

    public class Result
    {
        public int Success { get; set; }
        public string ErrorMessage { get; set; }
        public string Data { get; set; }
    }
    public class T1
    {
        public string name { get; set; }
    }
}