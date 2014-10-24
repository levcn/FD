using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using STComponse.CFG;


namespace Fw.DataAccess
{
    public class SqlFactoryBase
    {
        private static FwConfig config= new FwConfig();

        /// <summary>
        /// 当前正在使用的设置
        /// </summary>
        public static FwConfig Config
        {
            get
            {
                return config;
            }
            set
            {
                config = value;
            }
        }

        
        protected virtual IDbCommand GetCommand(string sql = null, params DbParameter[] paras)
        {
            SqlCommand comm = new SqlCommand();
            if (sql != null)
            {
                comm.CommandText = sql;
            }
            if (paras != null)
            {
                paras.ToList().ForEach(w => comm.Parameters.Add(w));
            }
            return comm;
        }
        protected virtual DbParameter GetDbParameter(string name, object value)
        {
            return new SqlParameter(name, value);
        }
    }
}
