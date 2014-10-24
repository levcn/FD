using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Fw.Entity;
using Fw.Reflection;
using STComponse.CFG;


namespace Fw.DataAccess
{
    public class MsSqlDataAccess : BaseDataAccess
    {
        #region Overrides of BaseDataAccess

        public override List<T> Select<T>(Expression<Func<T, bool>> predicate)
        {
            throw new Exception();
            //SqlFactory sf = new SqlFactory();
            //return EntityConvertor.GetEntity<T>(ExecuteDataTable(sf.GetSelect(predicate)));
        }
        public override List<T> Select<T>(string whereStr)
        {
            throw new Exception();
            //SqlFactory sf = new SqlFactory();
            //return EntityConvertor.GetEntity<T>(ExecuteDataTable(sf.GetSelect<T>(whereStr)));
        }
        //public void test<T>(Func<IOrderedQueryable<T>, IOrderedQueryable<T>> ordered)
        //{
        //    var ddd = ordered(new EnumerableQuery<T>(new List<T>()));
        //    ddd.
        //}

        public override IList Select(Type type, string storedProcedureNae, List<STParamete> parametes)
        {
            SqlCommand comm = new SqlCommand { CommandText = storedProcedureNae, CommandType = CommandType.StoredProcedure };
            if (parametes != null)
                for (int i = 0; i < parametes.Count; i++)
                {
                    comm.Parameters.Add(new SqlParameter(parametes[i].Name, parametes[i].Value));
                }

            var ds = ExecuteDataSet(comm);
            PageInfo pi = null;
            return EntityConvertor.GetEntity(type, ds, ref pi, false);
        }
        public IList Select(Type type, string storedProcedureNae, List<SqlParameter> parameters, out List<SqlParameter> parametersout)
        {
            SqlCommand comm = new SqlCommand { CommandText = storedProcedureNae, CommandType = CommandType.StoredProcedure };
            if (parameters != null)
                for (int i = 0; i < parameters.Count; i++)
                {
                    comm.Parameters.Add(parameters);
                }

            var ds = ExecuteDataSet(comm);
            PageInfo pi = null;
            var  re = EntityConvertor.GetEntity(type, ds, ref pi, false);
            parametersout = comm.Parameters.OfType<SqlParameter>().ToList();
            return re;
        }
        public IList Select(Assembly asse,EDataObject type,FwConfig fc, string storedProcedureNae, List<SqlParameter> parameters, out List<SqlParameter> parametersout)
        {
            SqlCommand comm = new SqlCommand { CommandText = storedProcedureNae, CommandType = CommandType.StoredProcedure };
            if (parameters != null)
                for (int i = 0; i < parameters.Count; i++)
                {
                    comm.Parameters.Add(parameters[i]);
                }

            var ds = ExecuteDataSet(comm);
            PageInfo pi = null;
            var re = EntityConvertor.GetEntity(asse, type, fc, ds, ref pi, false);
            parametersout = comm.Parameters.OfType<SqlParameter>().ToList();
            return re;
        }
        public int ExecuteStoredProcNonQuery(string storedProcedureNae, List<string> parames = null, List<string> paramesValue = null)
        {
            var comm = new SqlCommand { CommandText = storedProcedureNae, CommandType = CommandType.StoredProcedure };
            if (parames != null)
                for (var i = 0; i < parames.Count; i++)
                {
                    if (paramesValue != null) comm.Parameters.Add(new SqlParameter(parames[i], paramesValue[i]));
                }

            return ExecuteNonQuery(comm);
        }

        public override IList Select(Type type, string storedProcedureNae, List<STParamete> parametes, ref PageInfo pi)
        {
            SqlCommand comm = new SqlCommand { CommandText = storedProcedureNae, CommandType = CommandType.StoredProcedure };
            if (parametes != null)
                for (int i = 0; i < parametes.Count; i++)
                {
                    comm.Parameters.Add(new SqlParameter(parametes[i].Name, parametes[i].Value));
                }
            comm.Parameters.Add("@pageCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            comm.Parameters.Add("@recordCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            var ds = ExecuteDataSet(comm);
            pi.TotalPage = (int)comm.Parameters["@pageCount"].Value;
            pi.TotalRecord = (int)comm.Parameters["@recordCount"].Value;
            return EntityConvertor.GetEntity(type, ds, ref pi, false, true);
        }

        public override IList Select(Type type, string whereStr, string orderBy, string rowNumberName = "RowNumber", DbParameter[] sqlParameters = null)//
        {
            PageInfo pi = null;
            return Select(type, whereStr, orderBy, ref pi, rowNumberName, sqlParameters);
        }
        static object ooo = new object();

        public override IList Select(Type type, string whereStr, string orderBy, ref PageInfo pageInfo, string rowNumberName = "RowNumber", DbParameter[] sqlParameters = null, string[] white = null, string[] black = null)
        {
            lock (ooo)
            {
                
                SqlFactory sf = new SqlFactory();
                var command = sf.GetSelect(type, whereStr, orderBy, rowNumberName, pageInfo, sqlParameters,white,black);
            
                var ds = ExecuteDataSet(command);
                return EntityConvertor.GetEntity(type, ds, ref pageInfo, white: white, black: black);
            }
        }
        public override IList Select(Assembly asse,EDataObject type,FwConfig fc, string whereStr, string orderBy, ref PageInfo pageInfo, string rowNumberName = "RowNumber", DbParameter[] sqlParameters = null, string[] white = null, string[] black = null)
        {
            lock (ooo)
            {
                SqlFactory sf = new SqlFactory();
                var command = sf.GetSelect(type, fc,whereStr, orderBy, rowNumberName, pageInfo, sqlParameters, white, black);
            
                var ds = ExecuteDataSet(command);
                return EntityConvertor.GetEntity(asse,type,fc ,ds, ref pageInfo, white: white, black: black);
            }
        }
        public object Select(Type type, out PageInfo pageInfo, string predicate = null, int pageSize = 10, int pageIndex = 1, string @orderby = "")
        {
            pageInfo = new PageInfo();
            SqlFactory sf = new SqlFactory();
            var commands = sf.GetSelect(type, predicate, pageSize, pageIndex, orderby);
            int recountCount = ExecuteScalar<int>(commands[0]);

            if (pageSize <= 1) pageSize = 1;
            if (pageIndex <= 1) pageIndex = 1;
            int pageCount = (int)Math.Round((recountCount) / (float)pageSize + 0.5);

            var startRecord = (pageIndex - 1) * pageSize + 1;
            var endRecord = startRecord + pageSize - 1;
            commands[1].CommandText += ") as List"
                                       + " where rowId between " + startRecord + " and " + endRecord;
            //+ " order by " + orderby;
            var dataTable = ExecuteDataTable(commands[1]);
            pageInfo.EndRecord = endRecord;
            pageInfo.PageIndex = pageIndex;
            pageInfo.PageSize = pageSize;
            pageInfo.StartRecord = startRecord;
            pageInfo.TotalPage = pageCount;
            pageInfo.TotalRecord = recountCount;

            return EntityConvertor.GetEntity(type, dataTable);
        }

        public override List<T> Select<T>(out PageInfo pageInfo, string predicate = null, int pageSize = 10, int pageIndex = 1, string @orderby = "")
        {
            pageInfo = new PageInfo();
            SqlFactory sf = new SqlFactory();
            var commands = sf.GetSelect<T>(predicate, pageSize, pageIndex, orderby);
            int recountCount = ExecuteScalar<int>(commands[0]);

            int pageCount = (int)Math.Round((recountCount) / (float)pageSize + 0.5);
            var startRecord = (pageIndex - 1) * pageSize + 1;
            var endRecord = startRecord + pageSize - 1;
            commands[1].CommandText += ") as List"
                                       + " where rowId between " + startRecord + " and " + endRecord;
            //+ " order by " + orderby;
            var dataTable = ExecuteDataTable(commands[1]);
            pageInfo.EndRecord = endRecord;
            pageInfo.PageIndex = pageIndex;
            pageInfo.PageSize = pageSize;
            pageInfo.StartRecord = startRecord;
            pageInfo.TotalPage = pageCount;
            pageInfo.TotalRecord = recountCount;

            return EntityConvertor.GetEntity<T>(dataTable);
        }
        public override List<T> Select<T>(out PageInfo pageInfo, Expression<Func<T, bool>> predicate = null, int pageSize = 10, int pageIndex = 1, string @orderby = "")
        {
            pageInfo = new PageInfo();
            SqlFactory sf = new SqlFactory();
            var commands = sf.GetSelect(predicate, pageSize, pageIndex, orderby);
            int recountCount = ExecuteScalar<int>(commands[0]);

            int pageCount = (int)Math.Round((recountCount) / (float)pageSize + 0.5);
            var startRecord = (pageIndex - 1) * pageSize + 1;
            var endRecord = startRecord + pageSize - 1;
            commands[1].CommandText += ") as List"
                                       + " where rowId between " + startRecord + " and " + endRecord;
            //+ " order by " + orderby;
            var dataTable = ExecuteDataTable(commands[1]);
            pageInfo.EndRecord = endRecord;
            pageInfo.PageIndex = pageIndex;
            pageInfo.PageSize = pageSize;
            pageInfo.StartRecord = startRecord;
            pageInfo.TotalPage = pageCount;
            pageInfo.TotalRecord = recountCount;

            return EntityConvertor.GetEntity<T>(dataTable);
        }

        public int Insert<T>(FwConfig fc, T list)
        {
            SqlFactory sf = new SqlFactory();
            return ExecuteNonQuery(sf.GetInsert(list, fc));
        }
        public int Update(FwConfig fc, object item, string[] white = null, string[] black = null)
        {
            SqlFactory sf = new SqlFactory();
            return ExecuteNonQuery(sf.GetUpdate(item,fc, white, black));
        }
        public int Update(Type type, object item, string[] white = null, string[] black = null)
        {
            SqlFactory sf = new SqlFactory();
            return ExecuteNonQuery(sf.GetUpdate(item, white, black));
        }
        public override void Update<T>(object o, Expression<Func<T, bool>> predicate)
        {
            SqlFactory sf = new SqlFactory();
            ExecuteNonQuery(sf.GetUpdate(o, predicate));
        }

        //public override void Delete<T>(Expression<Func<T, bool>> predicate)
        //{
        //    SqlFactory sf = new SqlFactory();
        //    ExecuteNonQuery(sf.GetDelete(predicate));
        //}

        public override void Insert<T>(List<T> list)
        {
            list.ForEach(w => Insert(w));
        }
        public int Insert(Type type, object item)
        {
            SqlFactory sf = new SqlFactory();
            return ExecuteNonQuery(sf.GetInsert(item, type));
        }
        public override int Insert<T>(T list)
        {
            SqlFactory sf = new SqlFactory();
            return ExecuteNonQuery(sf.GetInsert(list));
        }

        public override int Update(object item)
        {
            SqlFactory sf = new SqlFactory();
            return ExecuteNonQuery(sf.GetUpdate(item));
        }

        public int Delete(EDataObject type, List<string> pKey)
        {
            SqlFactory sf = new SqlFactory();
            return ExecuteNonQuery(sf.GetDelete(type, pKey));
        }
        public override int Delete(Type type, List<string> pKey)
        {
            SqlFactory sf = new SqlFactory();
            return ExecuteNonQuery(sf.GetDelete(type, pKey));
        }

        //public void Delete(Type type,object list)
        //{
        //    SqlFactory sf = new SqlFactory();
        //    ExecuteNonQuery(sf.GetDelete(type,list));
        //}
        //public override void Delete<T>(T list)
        //{
        //    SqlFactory sf = new SqlFactory();
        //    ExecuteNonQuery(sf.GetDelete(list));
        //}

        public override DataSet ExecuteDataSet(IDbCommand command)
        {
            OutputLog(command);
            command.Connection = Connection;
            SqlDataAdapter ada = new SqlDataAdapter((SqlCommand)command);
            DataSet ds = new DataSet();
//            var s = command.Parameters[0] as SqlParameter;
//            s.Direction == ParameterDirection.ReturnValue
            Open();
            try
            {
                ada.Fill(ds, "sss");
            }
            catch (Exception e)
            {
                throw e;
            }
            Close();
            return ds;
        }

        public override DataTable ExecuteDataTable(IDbCommand command)
        {
            OutputLog(command);

            var ds = ExecuteDataSet(command);
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        public override T ExecuteScalar<T>(IDbCommand command)
        {
            OutputLog(command);
            T re = default(T);
            var table = ExecuteDataTable(command);
            if (table != null && table.Rows.Count > 0)
            {
                re = (T)Convert.ChangeType(table.Rows[0][0], typeof(T));
            }
            return re;
        }
        public override int ExecuteNonQuery(List<IDbCommand> command)
        {
            var re = 0;
            command.ForEach(OutputLog);
            Open();
            command.ForEach(
                w =>
                {
                    w.Connection = Connection;
                    re += w.ExecuteNonQuery();
                });
            Close();
            return re;
        }
        public override int ExecuteNonQuery(IDbCommand command)
        {
            var re = 0;
            OutputLog(command);
            Open();
            command.Connection = Connection;
            var tran = Connection.BeginTransaction();
            command.Transaction = tran;
            try
            {
                re = command.ExecuteNonQuery();
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
            }
            Close();
            return re;
        }

        public override string ConnectionString { get; set; }

        private SqlConnection connection;
        public override IDbConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SqlConnection(ConnectionString);
                }
                return connection;
            }
            set
            {
            }
        }

        public override void OutputLog(IDbCommand command)
        {
            if (LogStream != null && LogStream.CanWrite)
            {
                StreamWriter sw = new StreamWriter(LogStream);
                LogStream.Position = LogStream.Length;
                sw.WriteLine(command.CommandText);
                if (command.Parameters != null)
                {
                    command.Parameters.Cast<SqlParameter>()
                        .Select(w => string.Format(@"{0} = {1}", w.ParameterName, w.Value))
                        .ToList()
                        .ForEach(sw.WriteLine);
                }
                sw.WriteLine("===============================");
                sw.Flush();
            }
        }

        public override void Open()
        {
            if (Connection.State != ConnectionState.Connecting || Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public override void Close()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        #endregion

        /// <summary>
        /// 返回数据库是否可以打开
        /// </summary>
        /// <returns></returns>
        internal bool CheckDBConnection()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
