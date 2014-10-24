using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Fw.Entity;
using STComponse.CFG;


namespace Fw.DataAccess
{
    abstract public class BaseDataAccess
    {
        public abstract List<T> Select<T>(Expression<Func<T, bool>> predicate) where T : new();
        public abstract List<T> Select<T>(string whereStr) where T : new();
        public abstract List<T> Select<T>(out PageInfo pageInfo, Expression<Func<T, bool>> predicate = null, int pageSize = 10, int pageIndex = 1, string orderby = "") where T : new();

        public abstract List<T> Select<T>(out PageInfo pageInfo, string predicate = null, int pageSize = 10, int pageIndex = 1, string @orderby = "") where T : new();

        public abstract IList Select(Type type, string storedProcedureNae, List<STParamete> parametes);

        public abstract IList Select(Type type, string storedProcedureNae, List<STParamete> parametes, ref PageInfo pageInfo);

        //public abstract object Select(Type type,out PageInfo pageInfo, string predicate = null, int pageSize = 10, int pageIndex = 1, string @orderby = "") ;
        public abstract IList Select(Type type, string whereStr, string orderBy, string rowNumberName = "RowNumber", DbParameter[] sqlParameters = null);

        public abstract IList Select(Type type, string whereStr, string orderBy, ref PageInfo pageInfo, string rowNumberName = "RowNumber", DbParameter[] sqlParameters = null, string[] white = null, string[] black = null);
        public abstract IList Select(Assembly asse, EDataObject type, FwConfig fc, string whereStr, string orderBy, ref PageInfo pageInfo, string rowNumberName = "RowNumber", DbParameter[] sqlParameters = null, string[] white = null, string[] black = null);

        public abstract void Update<T>(object o, Expression<Func<T, bool>> predicate) where T : new();
        //public abstract void Delete<T>(Expression<Func<T, bool>> predicate) where T : new();

        public abstract void Insert<T>(List<T> list) where T : new();
        public abstract int Insert<T>(T list) where T : new();
        public abstract int Update(object o);
        //public abstract void Delete<T>(T list) where T : new();
        public abstract int Delete(Type type, List<string> pKey);

        public abstract DataSet ExecuteDataSet(IDbCommand command);

        public abstract DataTable ExecuteDataTable(IDbCommand command);

        public abstract T ExecuteScalar<T>(IDbCommand command);

        public abstract int ExecuteNonQuery(IDbCommand command);
        public abstract int ExecuteNonQuery(List<IDbCommand> command);

        public abstract string ConnectionString { get; set; }

        public abstract IDbConnection Connection { get; set; }
        public Stream LogStream { get; set; }

        public abstract void OutputLog(IDbCommand command);

        public abstract void Open();

        public abstract void Close();
    }
}
