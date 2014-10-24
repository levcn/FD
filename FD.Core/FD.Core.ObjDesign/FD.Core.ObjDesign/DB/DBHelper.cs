using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Threading.Tasks;
using ProjectCreater.Settings;
using STComponse.CFG;
using STComponse.DB;
using WPFControls.Ex;


namespace ProjectCreater.DB
{
    public static class DBHelper
    {
        /// <summary>
        /// 返回指定的数据库名是否存在
        /// </summary>
        /// <param name="config"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static bool CheckDBExist(DBConfig config, string dbName)
        {
            return ExecuteScalar(config.GetConnStrNoDB(), string.Format("SELECT NAME FROM master.dbo.sysdatabases WHERE NAME = N'{0}'", dbName.Filter())) !=null ;
        }

        public static Task<bool> CheckDBExistAndCreateAsyn(DBConfig config, string dbName)
        {
            return Task.Run(() =>
            {
                return CheckDBExistAndCreate(config, dbName);
            });
        }
        /// <summary>
        /// 新建数据库
        /// </summary>
        /// <param name="config"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static bool CheckDBExistAndCreate(DBConfig config, string dbName)
        {
            return ExecuteScalar(config.GetConnStrNoDB(), string.Format(@"IF not EXISTS (SELECT NAME FROM master.dbo.sysdatabases WHERE NAME = N'{0}')
   Create database {0}", dbName.Filter())) != null;
        }

        /// <summary>
        /// 异步返回所有的表信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Task<List<ETable>> GetTablesAsyn(DBConfig config)
        {
            return Task.Run(() => GetTables(config));
        }

        /// <summary>
        /// 异步返回所有的表信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Task<List<string>> GetDBsAsyn(DBConfig config)
        {
            return Task.Run(() => GetDBs(config));
        }

        private static List<string> GetDBs(DBConfig config)
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(config.GetConnStr());

            var dt = ExecuteDataTable(config.GetConnStrNoDB(), "Select Name FROM Master..SysDatabases order by Name ");
            List<string> re = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                re.Add(row[0].ToString());
            }
            return re;
        }

        /// <summary>
        /// 返回所有的表信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<ETable> GetTables(DBConfig config)
        {
            //throw new Exception("dfdf");
            List<ETable> result = new List<ETable>();

            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(config.GetConnStr());

            using (SqlConnection conn = new SqlConnection(scsb.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL2000_GetTables, conn);
                //throw new Exception(cmd.CommandText);
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    var str = (reader[6] ?? "").ToString();
                    result.Add(
                        new ETable()
                        {
                            TableName = reader.GetString(0),
                            DisplayName = reader.GetString(0),
                            Remark = str
                        });
                }
                reader.Close();
            }
            result.ForEach(w =>
            {
                w.EFields = GetTableColumns(config, w.TableName);
            });
            return result;
        }
        /// <summary>
        /// 返回一个表的所有列表
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<EField> GetTableColumns(DBConfig config,string tableName)
        {
            List<EField> result = new List<EField>();

            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(config.GetConnStr());

            using (SqlConnection conn = new SqlConnection(scsb.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL2008_GetTableColumns, conn);
                //throw new Exception(SQL2000_GetTableColumns);
                cmd.Parameters.Add(new SqlParameter("@DatabaseName", scsb.InitialCatalog));
                cmd.Parameters.Add(new SqlParameter("@SchemaName", "dbo"));
                cmd.Parameters.Add(new SqlParameter("@TableName", tableName));
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    var dfd = reader["Length"];
                    int len = 0;
                    if (dfd != DBNull.Value)
                    {
                        len = Convert.ToInt32(dfd);
                    }
                    var eField = new EField()
                    {
                        Name = (reader["Name"] ?? "").ToString().Trim(),
//                        DisplayName = (reader["Name"] ?? "").ToString(),
                        FieldType = GetCSharpType((reader["DataType"] ?? "").ToString()),
                        Length = len,
                        Nullable = reader["Nullable"].ToString().Equals("1"),
                        DefaultValue = DBNull.Value == reader["defaultval"] ? "" : reader["defaultval"].ToString(),
                        IsPrimary = Int32.Parse(reader["IsPK"].ToString()) == 1,
                        Precision = Int32.Parse(reader["scale"].ToString()),
                        DefaultValueConstraint = reader["DefaultValueConstraint"].ToString(),
                        //Identity = reader.GetInt32(8),
                        //IdentitySeed = Convert.ToInt32(reader.GetString(12)),
                        //IdentityIncrement = Convert.ToInt32(reader.GetString(13)),
                        Remark = reader["notes"].ToString().Trim(),
                    };
                    if (!string.IsNullOrEmpty(eField.Remark))
                    {
                        var r = eField.Remark.Split('/');
                        if (r.Length > 1)eField.DisplayName = r[0];
                        if (r.Length > 2)eField.ID = r[1].ToGuid();
//                        eField.DisplayName = eField.Remark.Trim();
                    }
                    if (string.IsNullOrEmpty(eField.DisplayName))
                    {
                        eField.DisplayName = eField.Name.Trim();
                    }
                    result.Add(eField);
                }
                reader.Close();
            }

            return result;
        }
        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="config"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
public static bool CheckDBExistAndDrop(DBConfig config, string dbName)
        {
            return ExecuteScalar(config.GetConnStrNoDB(), string.Format(@"IF EXISTS (SELECT NAME FROM master.dbo.sysdatabases WHERE NAME = N'{0}')
   Drop database {0}", dbName.Filter())) != null;
        }
        public static string Filter(this string str)
        {
            return str.Replace("'", "''");
        }
        private static string GetCSharpType(string type)
        {
            if (string.IsNullOrEmpty(type))
                return "string";

            string reval = string.Empty;
            switch (type.ToLower())
            {
                case "ntext":
                case "text":
                    reval = Property.备注;
                    break;
                case "varchar":
                case "nchar":
                case "char":
                case "nvarchar":
                    reval = Property.字符串;
                    break;
                case "int":
                case "smallint":
                case "bit":
                case "bigint":
                    reval = Property.整数;
                    break;
                case "float":
                case "decimal":
                case "real":
                case "tinyint":
                case "smallmoney":
                case "money":
                case "numeric":
                    reval = Property.小数;
                    break;
                case "binary":
                    reval = "System.Byte[]";
                    break;
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    reval = Property.日期;
                    break;
                case "uniqueidentifier":
                    reval = Property.GUID;
                    break;
                case "image":
                case "varbinary":
                    reval = "System.Byte[]";
                    break;
                case "Variant":
                    reval = "Object";
                    break;
                default:
                    reval = Property.字符串;
                    break;
            }
            return reval;
        }
        public static object ExecuteScalar(string connectionStr, string command = null)
        {
            //            using (SqlConnection conn = new SqlConnection(connectionStr))
//            {
//                conn.Open();
//                SqlCommand comm = new SqlCommand(command, conn);
            var dt=  ExecuteDataTable(connectionStr, command);
            if (dt.Rows.Count > 0)return  dt.Rows[0][0];
            return null;
            //                return comm.execute();
            //            }
        }

        public static DataTable ExecuteDataTable(string connectionStr, string command)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(command, connectionStr);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public static DataSet ExecuteDataSet(string connectionStr, string command)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(command, connectionStr);
            DataSet dt = new DataSet();
            adapter.Fill(dt);
            return dt;
        }
//        public static object ExecuteScalar(string connectionStr, string command = null)
//        {
//            using (SqlConnection conn = new SqlConnection(connectionStr))
//            {
//                conn.Open();
//                SqlCommand comm = new SqlCommand(command, conn);
//                return comm.ExecuteScalar();
//            }
//        }

        #region NAME

        private const string SQL2008_GetTableColumns =
            @"
              select
 c.name as Name,c.prec AS Length,c.isnullable AS Nullable,
 [IsPk]=case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=c.id and name in (
        SELECT name FROM sysindexes WHERE indid in(SELECT indid FROM sysindexkeys WHERE id = c.id AND colid=c.colid))) then '1' else '0' end,
[defaultval]=isnull(e.text,''),
t.name as DataType ,(select value from sys.extended_properties as ex where ex.major_id = c.id and ex.minor_id = c.colid) as notes ,
isnull(c.scale,0) as scale,
isnull((select Name from sys.objects where object_id = (select default_object_id from sys.columns where name = c.name and object_id = ta.object_id) and type_desc='DEFAULT_CONSTRAINT'),'') as DefaultValueCONSTRAINT
from
 syscolumns as c inner join sys.tables as ta on c.id=ta.object_id 
 inner join  (select name,system_type_id from sys.types where name<>'sysname') as t on c.xtype=t.system_type_id 
 left join syscomments e on c.cdefault=e.id
where
 ta.name=@TableName order by c.colid
";
        private const string SQL2000_GetTables =
            @"
          SELECT
              object_name(so.id) AS OBJECT_NAME,
              user_name(so.uid)  AS USER_NAME,
              so.type            AS TYPE,
              so.crdate          AS DATE_CREATED,
              fg.file_group      AS FILE_GROUP,
              so.id              AS OBJECT_ID,
              comm = (select top 1 [value] from fn_listextendedproperty (NULL, 'user', 'dbo', 'table', object_name(so.id),NULL, NULL))
          FROM 
              dbo.sysobjects so
          LEFT JOIN (
                SELECT 
                    s.groupname AS file_group,
                    i.id        AS id
                FROM dbo.sysfilegroups s
                INNER JOIN dbo.sysindexes i
                    ON i.groupid = s.groupid 
                WHERE i.indid < 2                           
              ) AS fg
              ON so.id = fg.id
          WHERE
              so.type = N'U'
              AND permissions(so.id) & 4096 <> 0
              AND ObjectProperty(so.id, N'IsMSShipped') = 0
          ORDER BY user_name(so.uid), object_name(so.id)";
        #endregion
    }
}
