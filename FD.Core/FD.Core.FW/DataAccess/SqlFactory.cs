using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Fw.Entity;
using Fw.Extends;
using Fw.Extends.Table;
using Fw.Reflection;
using Fw.Serializer;
using Fw.UserAttributes;
using STComponse.CFG;
using STComponse.GCode;


namespace Fw.DataAccess
{
    /// <summary>
    /// SQL命令生成器
    /// </summary>
    public class SqlFactory : SqlFactoryBase
    {
        protected override IDbCommand GetCommand(string sql = null, params DbParameter[] paras)
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
        public IDbCommand GetCommand(List<string> sqlList)
        {
            IDbCommand comm = new SqlCommand();
            if (sqlList != null)
            {
                var strSql = string.Join(";", sqlList.ToArray());
                comm.CommandText = strSql;
            }
            return comm;
        }
        public IDbCommand GetCommand(string strSql)
        {
            IDbCommand comm = new SqlCommand();
            if (!String.IsNullOrEmpty(strSql))
            {
                var sqlList = new List<string> { strSql };
                comm = GetCommand(sqlList);
            }
            return comm;
        }
        protected override DbParameter GetDbParameter(string name, object value)
        {
            return new SqlParameter(name, value);
        }
        public List<IDbCommand> GetSelect<T>(string predicate, int pageSize, int pageIndex = 1, string orderby = "")//
        {
            return GetSelect(typeof(T), predicate, pageSize, pageIndex, orderby);//
        }

        /// <summary>
        /// 返回一个表的查询SQL(Select)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<IDbCommand> GetSelect<T>(Expression<Func<T, bool>> predicate = null, int pageSize = 10, int pageIndex = 1, string orderby = "")//
        {
            List<IDbCommand> re = new List<IDbCommand>(2);
            var baseProps = ReflectionHelper.GetBaseTypeProperty<T>(PropertyType.BaseType);
            var tableName = ReflectionHelper.GetTableName<T>();
            string fields = ExtensionUtil.Serialize(baseProps.Select(w => GetColumnsAsString(tableName, w)), ",");
            string sql = "select count(*) from " + tableName;
            string sql1 = "Select * FROM (select ROW_NUMBER() Over(order by " + orderby + ") as rowId," + fields + " from " + tableName;
            List<SqlParameter> sqlParameters1 = new List<SqlParameter>();
            List<SqlParameter> sqlParameters2 = new List<SqlParameter>();
            if (predicate != null)
            {
                ConditionBuilder builder = new ConditionBuilder();
                builder.Build(predicate.Body);
                string sqlCondition = builder.Condition;
                //var dwww = builder.Arguments;
                sqlParameters1 = builder.GetParameter<SqlParameter>();
                sqlParameters2 = builder.GetParameter<SqlParameter>();
                sql += " Where " + sqlCondition;
                sql1 = "Select * FROM (select ROW_NUMBER() Over(order by " + orderby + ") as rowId," + fields + " from " +
                tableName + " where " + sqlCondition;
            }
            re.Add(GetCommand(sql, sqlParameters1.ToArray()));
            re.Add(GetCommand(sql1, sqlParameters2.ToArray()));
            return re;
        }
        /// <summary>
        /// 返回指定类型的查询
        /// </summary>
        /// <param name="type">类型名</param>
        /// <param name="whereStr">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="rowNumberName">RowNumber列头</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public IDbCommand GetSelect(Type type, string whereStr, string orderBy, string rowNumberName = "RowNumber", PageInfo pageInfo = null, DbParameter[] sqlParameters = null, string[] white = null, string[] black = null)
        {
            var countSql = "";
            if (pageInfo != null)
            {
                pageInfo = CheckPageInfo(pageInfo);
            }
            var primaryPropertyField = ReflectionHelper.GetPrimaryPropertyField(type);
            if (orderBy == null || orderBy.Trim().Length == 0)//如果没有设置排序,则按主键排序
            {
                orderBy = primaryPropertyField;
            }
            var trs = new List<TableRelation>();
            var props = ReflectionHelper.GetTableAndColumnsList(type, ref trs, white: white, black: black);
            if (sqlParameters == null) sqlParameters = new DbParameter[0];
            string sql = "select {0} From {1}";
            var tableName = ReflectionHelper.GetTableName(type);
            var mainTableName = tableName;
            if (!string.IsNullOrEmpty(whereStr))
            {
                sql += " Where " + whereStr;
            }
            if (props.Count == 1 && pageInfo == null)
            {
                //Select a as t_a,b as t_b,c as t_c From t
                string fields = props[0].PropertyInfos.Select(ReflectionHelper.GetPropertyField).Serialize(",");
                sql = string.Format(sql, fields, tableName);
                return GetCommand(sql, sqlParameters.ToArray());
            }
            if (props.Count == 1 && pageInfo != null)//单表
            {
                string sqlWhere = "";
                if (!string.IsNullOrEmpty(whereStr))
                {
                    sqlWhere += " Where " + whereStr;
                }
                //Select a as t_a,b as t_b,c as t_c From t
                string fields = props[0].PropertyInfos.Select(ReflectionHelper.GetPropertyField).Serialize(",");
                sql = string.Format(sql, fields, tableName);

                countSql = string.Format(@"select Count(*) From ({0}) as ft
", sql);
                sql = string.Format(@"Select * into #{4} 
from (
	select row_number() over(Order By {0}) AS {1} ,* 
	From (
			SELECT {6} FROM {4} {5}
		) as dd
	) as d
where {1} >= {2} and {1} <= {3} ;

select * from #{4};;

", orderBy, rowNumberName, pageInfo.StartRecord, pageInfo.EndRecord, props[0].TableName, sqlWhere, fields);


                if (countSql != string.Empty) sql += countSql;
                return GetCommand(sql, sqlParameters.ToArray());
            }

            if (pageInfo == null)
            {
                sql = "select {0}.{1} From {2}";
                // TEDIT THT 2012-7-4 修改SQL查询
                //				sql = "select top 10000000  {0} From {1}";
                //Select a as t_a,b as t_b,c as t_c From t

                var fields = props[0].PropertyInfos.Select(w => mainTableName + "." + ReflectionHelper.GetPropertyField(w)).Serialize(",");
                tableName = GetTableNameJoin(trs);
                sql = string.Format(sql, mainTableName, primaryPropertyField, tableName);
                if (!string.IsNullOrEmpty(whereStr))
                {
                    sql += " Where " + whereStr;
                }
                sql = string.Format(@"Select * into #{3} 
from (
	select row_number() over(Order By {0}) AS {1} ,* 
	From (
			SELECT {5} FROM {3} Where {4} in({2})
		) as dd
	) as d

select * from #{3} as ddddddw;

", orderBy, rowNumberName, sql, props[0].TableName, primaryPropertyField, fields);

            }
            else
            {
                sql = "select DISTINCT {0}.{1} From {2}";
                //Select a as t_a,b as t_b,c as t_c From t
                //props.ForEach(w => pis.AddRange(w.PropertyInfos.Select(q => GetColumnsAsString(w.TableName, q)).ToList()));
                //string fields = pis.Serialize(",");
                string fields = props[0].PropertyInfos.Select(w => mainTableName + "." + ReflectionHelper.GetPropertyField(w)).Serialize(",");
                tableName = GetTableNameJoin(trs);
                sql = string.Format(sql, mainTableName, primaryPropertyField, tableName);
                if (!string.IsNullOrEmpty(whereStr))
                {
                    sql += " Where " + whereStr;
                }
                countSql = string.Format(@"select Count(*) From ({0}) as ft
", sql);
                sql = string.Format(@"Select * into #{5} 
from (
	select row_number() over(Order By {0}) AS {1} ,* 
	From (
			SELECT {7} FROM {5} Where {6} in({2})
		) as dd
	) as d
where {1} >= {3} and {1} <= {4};

select * from #{5};;

", orderBy, rowNumberName, sql, pageInfo.StartRecord, pageInfo.EndRecord, props[0].TableName, primaryPropertyField, fields);
            }
            var associationType = ReflectionHelper.GetBaseTypeProperty(type, PropertyType.CustomType);

            associationType = ReflectionHelper.GetBlack(associationType, black);
            associationType = ReflectionHelper.GetWhite(associationType, white);
            var tempTableName = "#" + props[0].TableName;
            foreach (var propertyInfo in associationType)
            {


                List<TableRelation> trs1 = new List<TableRelation>();
                var tableColns = ReflectionHelper.GetTableAndColumnsList(type, ref trs1, propertyInfo, true);
                var join = GetTableNameJoin(trs1, false);
                List<string> fieldNames = new List<string>();
                tableColns.ForEach(w => fieldNames.AddRange(w.PropertyInfos.Select(q => GetColumnsAsString(w.TableName, q))));
                if (tableColns.Count >= 2)
                {
                    string sql1 = @"Select {0} From #{2} {1};
";
                    sql1 = string.Format(sql1, fieldNames.Serialize(","), @join, props[0].TableName);
                    sql1 = sql1.Replace(props[0].TableName + ".", tempTableName + ".");

                    sql += sql1;
                }
                if (tableColns.Count == 1)
                {
                    var pFieldName = ReflectionHelper.GetPrimaryPropertyField(tableColns[0].Type);
                    var relTableName = tableColns[0].TableName;
                    string sql1 = @"SELECT 
	{0}

	FROM {1} Where {2} IN(
	SELECT DISTINCT {1}.{2}

FROM {4} {3});
";
                    sql1 = string.Format(sql1, fieldNames.Serialize(","), relTableName, pFieldName, @join, tempTableName);
                    sql1 = sql1.Replace(props[0].TableName + ".", tempTableName + ".");
                    sql += sql1;
                }
            }
            if (countSql != string.Empty) sql += countSql;
            return GetCommand(sql, sqlParameters.ToArray());
        }
        /// <summary>
        /// 返回指定类型的查询
        /// </summary>
        /// <param name="type">类型名</param>
        /// <param name="whereStr">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="rowNumberName">RowNumber列头</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public IDbCommand GetSelect(EDataObject type,FwConfig fc, string whereStr, string orderBy, string rowNumberName = "RowNumber", PageInfo pageInfo = null, DbParameter[] sqlParameters = null, string[] white = null, string[] black = null)
        {
            var countSql = "";
            if (pageInfo != null)
            {
                pageInfo = CheckPageInfo(pageInfo);
            }
//            var obj = Config.GetDataObjectByCode(type);
            var primaryPropertyField = type.GetPrimary();
//            var primaryPropertyField = ReflectionHelper.GetPrimaryPropertyField(type);
            if (orderBy == null || orderBy.Trim().Length == 0)//如果没有设置排序,则按主键排序
            {
                orderBy = primaryPropertyField.Code;
            }
            var trs = new List<TableRelation>();
            var props = ReflectionHelper.GetTableAndColumnsList(type,fc, ref trs, white: white, black: black);
            if (sqlParameters == null) sqlParameters = new DbParameter[0];
            string sql = "select {0} From {1}";
            var tableName = type.KeyTableName;
            var mainTableName = tableName;
            if (!string.IsNullOrEmpty(whereStr))
            {
                sql += " Where " + whereStr;
            }
            if (props.Count == 1 && pageInfo == null)
            {
                //Select a as t_a,b as t_b,c as t_c From t
                string fields = ExtensionUtil.Serialize(props[0].PropertyInfos.Select(w => w.Code), ",");
                sql = string.Format(sql, fields, tableName);
                return GetCommand(sql, sqlParameters.ToArray());
            }
            if (props.Count == 1 && pageInfo != null)//单表
            {
                string sqlWhere = "";
                if (!string.IsNullOrEmpty(whereStr))
                {
                    sqlWhere += " Where " + whereStr;
                }
                //Select a as t_a,b as t_b,c as t_c From t
                string fields = ExtensionUtil.Serialize(props[0].PropertyInfos.Select(w => w.Code), ",");
                sql = string.Format(sql, fields, tableName);

                countSql = string.Format(@"select Count(*) From ({0}) as ft
", sql);
                sql = string.Format(@"Select * into #{4} 
from (
	select row_number() over(Order By {0}) AS {1} ,* 
	From (
			SELECT {6} FROM {4} {5}
		) as dd
	) as d
where {1} >= {2} and {1} <= {3} ;

select * from #{4};;

", orderBy, rowNumberName, pageInfo.StartRecord, pageInfo.EndRecord, props[0].TableName, sqlWhere, fields);


                if (countSql != string.Empty) sql += countSql;
                return GetCommand(sql, sqlParameters.ToArray());
            }

            if (pageInfo == null)
            {
                sql = "select {0}.{1} From {2}";
                // TEDIT THT 2012-7-4 修改SQL查询
                //				sql = "select top 10000000  {0} From {1}";
                //Select a as t_a,b as t_b,c as t_c From t

                var fields = ExtensionUtil.Serialize(props[0].PropertyInfos.Select(w => mainTableName + "." + w.Code), ",");
                tableName = GetTableNameJoin(trs);
                sql = string.Format(sql, mainTableName, primaryPropertyField.Code, tableName);
                if (!string.IsNullOrEmpty(whereStr))
                {
                    sql += " Where " + whereStr;
                }
                sql = string.Format(@"Select * into #{3} 
from (
	select row_number() over(Order By {0}) AS {1} ,* 
	From (
			SELECT {5} FROM {3} Where {4} in({2})
		) as dd
	) as d

select * from #{3} as ddddddw;

", orderBy, rowNumberName, sql, props[0].TableName, primaryPropertyField.Code, fields);

            }
            else
            {
                sql = "select DISTINCT {0}.{1} From {2}";
                //Select a as t_a,b as t_b,c as t_c From t
                //props.ForEach(w => pis.AddRange(w.PropertyInfos.Select(q => GetColumnsAsString(w.TableName, q)).ToList()));
                //string fields = pis.Serialize(",");
                string fields = ExtensionUtil.Serialize(props[0].PropertyInfos.Select(w => mainTableName + "." + w.Code), ",");
                tableName = GetTableNameJoin(trs);
                sql = string.Format(sql, mainTableName, primaryPropertyField.Code, tableName);
                if (!string.IsNullOrEmpty(whereStr))
                {
                    sql += " Where " + whereStr;
                }
                countSql = string.Format(@"select Count(*) From ({0}) as ft
", sql);
                sql = string.Format(@"Select * into #{5} 
from (
	select row_number() over(Order By {0}) AS {1} ,* 
	From (
			SELECT {7} FROM {5} Where {6} in({2})
		) as dd
	) as d
where {1} >= {3} and {1} <= {4};

select * from #{5};;

", orderBy, rowNumberName, sql, pageInfo.StartRecord, pageInfo.EndRecord, props[0].TableName, primaryPropertyField.Code, fields);
            }
            var associationType = type.Relation.ToList();//Where(w => w.RelationType != Relation.字典).

            associationType = ReflectionHelper.GetBlack(associationType, black);
            associationType = ReflectionHelper.GetWhite(associationType, white);
            var tempTableName = "#" + props[0].TableName;
            foreach (var propertyInfo in associationType)
            {
                List<TableRelation> trs1 = new List<TableRelation>();
                var tableColns = ReflectionHelper.GetTableAndColumnsList(type,fc, ref trs1, propertyInfo, true);
                var tableAndColumnsFcs = tableColns.ToArray();
                Array.Reverse(tableAndColumnsFcs);
                tableColns = tableAndColumnsFcs.ToList();
//trs1.ForEach(z =>
//{
//    if (z.TableName1 == props[0].TableName) z.TableName1 = tempTableName;
//    if (z.TableName2 == props[0].TableName) z.TableName2 = tempTableName;
//});
                var join = GetTableNameJoin(trs1, false);

                List<string> fieldNames = new List<string>();
                tableColns.ForEach(w => fieldNames.AddRange(w.PropertyInfos.Select(q => GetColumnsAsString(w.TableName, q))));
                if (tableColns.Count >= 2)
                {
                    string sql1 = @"Select {0} From #{2} {1};
";
                    sql1 = string.Format(sql1, ExtensionUtil.Serialize(fieldNames, ","), @join, props[0].TableName);
                    sql1 = sql1.Replace(props[0].TableName + ".", tempTableName + ".");

                    sql += sql1;
                }
                if (tableColns.Count == 1)
                {
                    var pFieldName = tableColns[0].Type.GetPrimary().Code;
                    var relTableName = tableColns[0].TableName;
                    string sql1 = @"SELECT 
	{0}

	FROM {1} Where {2} IN(
	SELECT DISTINCT {1}.{2}

FROM {4} {3});
";
                    sql1 = string.Format(sql1, ExtensionUtil.Serialize(fieldNames, ","), relTableName, pFieldName, @join, tempTableName);
                    sql1 = sql1.Replace(props[0].TableName + ".", tempTableName + ".");
                    sql += sql1;
                }
            }
            if (countSql != string.Empty) sql += countSql;
            return GetCommand(sql, sqlParameters.ToArray());
        }

        /// <summary>
        /// 验证分页信息有有效性
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        private static PageInfo CheckPageInfo(PageInfo pageInfo)
        {
            if (pageInfo.PageIndex < 1) pageInfo.PageIndex = 1;
            pageInfo.StartRecord = (pageInfo.PageIndex - 1) * pageInfo.PageSize + 1;
            pageInfo.EndRecord = pageInfo.StartRecord + pageInfo.PageSize - 1;
            return pageInfo;
        }

        public List<IDbCommand> GetSelect(Type type, string predicate, int pageSize, int pageIndex = 1, string orderby = "")//
        {
            List<TableRelation> trs = new List<TableRelation>();
            var props = ReflectionHelper.GetTableAndColumnsList(type, ref trs);
            List<IDbCommand> re = new List<IDbCommand>(2);
            var baseProps = ReflectionHelper.GetBaseTypeProperty(type, PropertyType.BaseType);
            var tableName = ReflectionHelper.GetTableName(type);
            string fields = ExtensionUtil.Serialize(baseProps.Select(w => GetColumnsAsString(tableName, w)), ",");
            string sql = "";
            string sql1 = "";
            if (props.Count == 1)//单表
            {
                sql = "select count(*) from " + tableName;
                sql1 = "Select * FROM (select ROW_NUMBER() Over(order by " + orderby + ") as rowId," + fields + " from " + tableName;
                if (!string.IsNullOrEmpty(predicate))
                {
                    sql += " Where " + predicate;
                    sql1 = "Select * FROM (select ROW_NUMBER() Over(order by " + orderby + ") as rowId," + fields + " from " +
                    tableName + " where " + predicate;
                }
            }
            else
            {
                tableName = GetTableNameJoin(trs);
                List<string> pis = new List<string>();
                props.ForEach(w => pis.AddRange(w.PropertyInfos.Select(q => GetColumnsAsString(w.TableName, q)).ToList()));
                fields = ExtensionUtil.Serialize(pis, ",");
                sql = "select count(*) from " + tableName;
                sql1 = "Select * FROM (select ROW_NUMBER() Over(order by " + orderby + ") as rowId," + fields + " from " +
                       tableName;
                if (!string.IsNullOrEmpty(predicate))
                {
                    sql += " Where " + predicate;
                    sql1 = "Select * FROM (select ROW_NUMBER() Over(order by " + orderby + ") as rowId," + fields + " from " +
                    tableName + " where " + predicate;
                }
            }
            re.Add(GetCommand(sql));
            re.Add(GetCommand(sql1));
            return re;
        }
        //public IDbCommand GetSelect<T>(Expression<Func<T, bool>> predicate = null)//
        //{
        //    SqlParameter[] sqlParameters = null;
        //    string sqlCondition = null;
        //    if (predicate != null)
        //    {
        //        ConditionBuilder builder = new ConditionBuilder();
        //        builder.Build(predicate.Body);
        //        sqlCondition = builder.Condition;
        //        //var dwww = builder.Arguments;
        //        sqlParameters = builder.GetParameter<SqlParameter>().ToArray();
        //    }
        //    return GetSelect(typeof(T),sqlCondition,sqlParameters.ToArray(),);
        //}

        /// <summary>
        /// Left Join {1} On {0}.{2} = {1}.{3}
        /// </summary>
        /// <param name="trs"></param>
        /// <param name="haveMasterTable">是否包含主表名称</param>
        /// <returns></returns>
        private string GetTableNameJoin(List<TableRelation> trs, bool haveMasterTable = true)
        {
//            trs.Reverse();
            //a Left Join b On a.p = b.p
            var re = "";
            var list = new List<String>();
            if (haveMasterTable)
            {
                re = trs[0].TableName1;
                list.Add(trs[0].TableName1);
                for (var i = 0; i < trs.Count; i++)
                {
                    re += string.Format(" Left Join {1} {4} On {0}.{2} = {1}.{3}", trs[i].TableName1, trs[i].TableName2, trs[i].Column1, trs[i].Column2, list.Contains(trs[i].TableName2) ? String.Format("tmp{0}", i) : string.Empty);
                    list.Add(trs[i].TableName2);
                }
            }
            else
            {
                for (var i = 0; i < trs.Count; i++)
                {
                    var join = trs[i].LeftJoin ? "left" : "inner";
                    //re += string.Format(" Left Join {1} On {0}.{2} = {1}.{3}", trs[i].TableName1, trs[i].TableName2, trs[i].Column1, trs[i].Column2);
                    re += string.Format(" {5} Join {1} {4} On {0}.{2} = {1}.{3}", trs[i].TableName1, trs[i].TableName2, trs[i].Column1, trs[i].Column2, list.Contains(trs[i].TableName2) ? String.Format("tmp{0}", i) : string.Empty, join);
                    list.Add(trs[i].TableName2);
                }
            }

            return re;
        }

        public List<IDbCommand> GetUpdate(object t,FwConfig fc, string[] white = null, string[] black = null)
        {
            var objs = fc.DataObjects.ToList();
            objs = objs.Concat(fc.DictObject).ToList();
            Type type = t.GetType();
            var obj = objs.FirstOrDefault(w => w.ObjectCode == type.Name);
            var baseProps = obj.GetBaseProperties();
            baseProps = ReflectionHelper.GetBlack(baseProps, black);
            baseProps = ReflectionHelper.GetWhite(baseProps, white);
            var p = obj.GetPrimary();
            if (!baseProps.Contains(p)) baseProps.Add(p);
            var associationType = obj.Relation.ToList();
            associationType = ReflectionHelper.GetBlack(associationType, black);
            associationType = ReflectionHelper.GetWhite(associationType, white);
            List<IDbCommand> refInsert = new List<IDbCommand>();//添加关联表的命令
            List<IDbCommand> refDelete = new List<IDbCommand>();//删除关联表的命令

            foreach (var prop in associationType)
            {
//                if (ReflectionHelper.IsRelationTableAndSaveRelation(propertyInfo))//有关联表;要先删除关联表

                if (prop.RelationType == Relation.复杂关联 || prop.RelationType == Relation.简单关联)
                {
                    var sql1 = @"Delete From {0} Where {1} = '{2}' 
";
                    var relConfig = prop.RelConfig;

                    var relTableName = relConfig.RelTableName;
                    var relTableMasterKey = relConfig.RelMasertKey;
//                    var relTableMasterKey = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(prop).OtherKey;
                    var thisKeyName = obj.GetPrimary().Code;
//                    var thisKeyName = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(prop).ThisKey;
                    var propertyInfos = type.GetProperties();
                    var relProp = propertyInfos.FirstOrDefault(w => w.Name == prop.ObjectPorertity);
                    var prim = propertyInfos.FirstOrDefault(w => w.Name == thisKeyName);
                    var thisKeyValue = ReflectionHelper.GetPropertyValue(prim, t);
//                    var thisKeyValue = ReflectionHelper.GetPropertyValue(baseProps.First(z => z.Name == thisKeyName), t);
                    //var masterKeyValue = ReflectionHelper.GetPrimaryKey(t);
                    //var masterKeyValue = ReflectionHelper.GetPrimaryValue(t);
                    sql1 = string.Format(sql1, relTableName, relTableMasterKey, thisKeyValue);
                    refDelete.Add(GetCommand(sql1));

                    var pv = ReflectionHelper.GetPropertyValue(relProp, t) as IList;
                    if (pv != null)
                    {
                        var dictObj = objs.FirstOrDefault(z => z.ObjectCode == relConfig.DictTableName);
                        var relDictObj = objs.FirstOrDefault(z => z.ObjectCode == relConfig.RelTableName);
                        var arr = new ArrayList();
                        for (int i = 0; i < pv.Count; i++)
                        {
                            var item = pv[i];
                            var id = ReflectionHelper.GetPropertyValue("ID", item);
                            if (!arr.Contains(id))
                            {
                                if (prop.RelationType == Relation.简单关联)
                                {
                                    IDbCommand command = GetInsertSql(t, item, obj, dictObj, prop);
                                    refInsert.Add(command);
                                }
                                else
                                {
                                    IDbCommand command = GetInsertSql(relDictObj,item);
                                    refInsert.Add(command);
                                }
                            }

                            arr.Add(id);
                        }
                    }
                }
                else//只用字典表
                {

                }
            }
            var tableName = ReflectionHelper.GetTableName(type);
            //Update tableName Set a='t_a',b as t_b,c as t_c From t
            string sql2 = "Update {0} Set {1} Where {2}";
            string fields = ExtensionUtil.Serialize(ReflectionHelper.GetNonPrimaryFields(obj, baseProps).Select(GetColumnsAsUpdate), ",");

            sql2 = string.Format(sql2, tableName, fields, GetPrimaryKeyWhereString(obj));
            var paras = GetFieldParameates(t, baseProps.ToArray());
            var sqlCommand = GetCommand(sql2, paras.ToArray());
            refDelete.AddRange(refInsert);//先删除关联,再添加关联
            refDelete.Add(sqlCommand);//再更新
            return refDelete;
        }
        /// <summary>
        /// 返回一个表的查询SQL(Update)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<IDbCommand> GetUpdate(object t, string[] white = null, string[] black = null)
        {
            Type type = t.GetType();
            var baseProps = ReflectionHelper.GetBaseTypeProperty(type, PropertyType.BaseType);
            baseProps = ReflectionHelper.GetBlack(baseProps, black);
            baseProps = ReflectionHelper.GetWhite(baseProps, white);
            var p = ReflectionHelper.GetPrimaryKey(type);
            if (!baseProps.Contains(p)) baseProps.Add(p);
            var associationType = ReflectionHelper.GetBaseTypeProperty(type, PropertyType.CustomType);
            associationType = ReflectionHelper.GetBlack(associationType, black);
            associationType = ReflectionHelper.GetWhite(associationType, white);
            List<IDbCommand> refInsert = new List<IDbCommand>();//添加关联表的命令
            List<IDbCommand> refDelete = new List<IDbCommand>();//删除关联表的命令

            foreach (var propertyInfo in associationType)
            {
                if (ReflectionHelper.IsRelationTableAndSaveRelation(propertyInfo))//有关联表;要先删除关联表
                {
                    var sql1 = @"Delete From {0} Where {1} = '{2}' 
";
                    var relTableName = ReflectionHelper.GetTableName(propertyInfo);
                    var relTableMasterKey = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(propertyInfo).OtherKey;
                    var thisKeyName = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(propertyInfo).ThisKey;
                    var thisKeyValue = ReflectionHelper.GetPropertyValue(baseProps.First(z => z.Name == thisKeyName), t);
                    //var masterKeyValue = ReflectionHelper.GetPrimaryKey(t);
                    //var masterKeyValue = ReflectionHelper.GetPrimaryValue(t);
                    sql1 = string.Format(sql1, relTableName, relTableMasterKey, thisKeyValue);
                    refDelete.Add(GetCommand(sql1));
                    var pv = ReflectionHelper.GetPropertyValue(propertyInfo, t) as IList;
                    if (pv != null)
                    {
                        var arr = new ArrayList();
                        for (int i = 0; i < pv.Count; i++)
                        {
                            var item = pv[i];
                            var id = ReflectionHelper.GetPropertyValue("ID", item);
                            if (!arr.Contains(id))
                            {
                                IDbCommand command = GetInsertSql(item);
                                refInsert.Add(command);
                            }

                            arr.Add(id);
                        }
                    }
                }
                else//只用字典表
                {

                }
            }
            var tableName = ReflectionHelper.GetTableName(type);
            //Update tableName Set a='t_a',b as t_b,c as t_c From t
            string sql2 = "Update {0} Set {1} Where {2}";
            string fields = ExtensionUtil.Serialize(ReflectionHelper.GetNonPrimaryFields(type, baseProps).Select(GetColumnsAsUpdate), ",");

            sql2 = string.Format(sql2, tableName, fields, GetPrimaryKeyWhereString(type));
            var paras = GetFieldParameates(t, baseProps.ToArray());
            var sqlCommand = GetCommand(sql2, paras.ToArray());
            refDelete.AddRange(refInsert);//先删除关联,再添加关联
            refDelete.Add(sqlCommand);//再更新
            return refDelete;
        }
        /// <summary>
        /// 返回一个表的查询SQL(Update)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public IDbCommand GetUpdate<T>(object o, Expression<Func<T, bool>> predicate)
        {
            var baseProps = ReflectionHelper.GetBaseTypeProperty(o.GetType(), PropertyType.BaseType);
            var tableName = ReflectionHelper.GetTableName<T>();
            //Update tableName Set a='t_a',b as t_b,c as t_c From t
            string sql = "Update {0} Set {1} Where {2}";
            string fields = ExtensionUtil.Serialize(ReflectionHelper.GetNonPrimaryFields<T>(baseProps).Select(GetColumnsAsUpdate), ",");
            string sqlCondition = "";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (predicate != null)
            {
                ConditionBuilder builder = new ConditionBuilder();
                builder.Build(predicate.Body);
                sqlCondition = builder.Condition;
                parameters = builder.GetParameter<SqlParameter>();
            }
            var paras = GetFieldParameates(o, baseProps.ToArray());
            sql = string.Format(sql, tableName, fields, sqlCondition);
            return GetCommand(sql, parameters.Concat(paras.Cast<SqlParameter>()).ToArray());
        }
        public IDbCommand GetDelete<T>(T t)
        {
            return GetDelete(typeof(T), t);
        }
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDbCommand GetDelete(Type type, List<string> ids)
        {
            var strIDs = ExtensionUtil.Serialize(ids, ",");
            var tableName = ReflectionHelper.GetTableName(type);
            string sql = "";
            var associationType = ReflectionHelper.GetBaseTypeProperty(type, PropertyType.CustomType);
            foreach (var propertyInfo in associationType)
            {
                if (ReflectionHelper.IsRelationTableAndSaveRelation(propertyInfo))//有关联表;要先删除关联表
                {
                    //var sql1 = @"Delete From {0} Where {1} In ({2}) ";
                    var sql1 = @"Delete From {0} Where {1} In (SELECT {2} FROM {3} WHERE {3}.{5} IN({4}))";
                    var relTableName = ReflectionHelper.GetTableName(propertyInfo);
                    var relTableMasterKey = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(propertyInfo).OtherKey;
                    var thisKey = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(propertyInfo).ThisKey;
                    //var masterKeyValue = ReflectionHelper.GetPrimaryValue(type);
                    //sql1 = string.Format(sql1, relTableName, relTableMasterKey, strIDs);
                    sql1 = string.Format(sql1, relTableName, relTableMasterKey, thisKey, tableName, strIDs, ReflectionHelper.GetPrimaryKey(type).Name);
                    sql += sql1;
                }
            }


            var sql2 = @"Delete {0} Where {1} In({2})";
            sql2 = string.Format(sql2, tableName, ReflectionHelper.GetPropertyField(ReflectionHelper.GetPrimaryKey(type)), strIDs);
            sql += sql2;
            return GetCommand(sql);
        }
        public IDbCommand GetDelete(Type type, object t)
        {
            var baseProps = ReflectionHelper.GetPrimaryKey(type);
            var tableName = ReflectionHelper.GetTableName(type);
            //Delete tableName Where a = @a
            string sql = "Delete {0} Where {1}";
            sql = string.Format(sql, tableName, GetPrimaryKeyWhereString(type));
            var paras = GetFieldParameates(t, baseProps);
            return GetCommand(sql, paras.ToArray());
        }
        public IDbCommand GetDelete(EDataObject type, object t)
        {
            var baseProps = type.GetPrimary();
            var tableName = type.KeyTableName;
            //Delete tableName Where a = @a
            string sql = "Delete {0} Where {1}";
            sql = string.Format(sql, tableName, GetPrimaryKeyWhereString(type));
            var paras = GetFieldParameates(t, baseProps);
            return GetCommand(sql, paras.ToArray());
        }
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDbCommand GetDelete(EDataObject type, List<string> ids)
        {
            var strIDs = ExtensionUtil.Serialize(ids, ",");
            var tableName = type.KeyTableName;
            string sql = "";
//            var associationType = ReflectionHelper.GetBaseTypeProperty(type, PropertyType.CustomType);
            var associationType = type.Relation.ToList();
            var thisKey = type.GetPrimary().Code;
            foreach (var propertyInfo in associationType)
            {
//                if (ReflectionHelper.IsRelationTableAndSaveRelation(propertyInfo))//有关联表;要先删除关联表
                {
                    //var sql1 = @"Delete From {0} Where {1} In ({2}) ";
                    var sql1 = @"Delete From {0} Where {1} In (SELECT {2} FROM {3} WHERE {3}.{5} IN({4}))";
                    var relConfig = propertyInfo.RelConfig;
                    if (!relConfig.IsDict)
                    {
                        var relTableName = relConfig.RelTableName;
                        var relTableMasterKey = relConfig.RelMasertKey;
                        //var masterKeyValue = ReflectionHelper.GetPrimaryValue(type);
                        //sql1 = string.Format(sql1, relTableName, relTableMasterKey, strIDs);
                        sql1 = string.Format(sql1, relTableName, relTableMasterKey, thisKey, tableName, strIDs, thisKey);
                        sql += sql1;
                    }
                }
            }


            var sql2 = @"Delete {0} Where {1} In({2})";
            sql2 = string.Format(sql2, tableName, thisKey, strIDs);
            sql += sql2;
            return GetCommand(sql);
        }
        public IDbCommand GetDelete<T>(Expression<Func<T, bool>> predicate)
        {
            var baseProps = ReflectionHelper.GetPrimaryKey<T>();
            var tableName = ReflectionHelper.GetTableName<T>();
            //Delete tableName Where a = @a
            string sql = "Delete {0} Where {1}";
            string sqlCondition = "";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (predicate != null)
            {
                ConditionBuilder builder = new ConditionBuilder();
                builder.Build(predicate.Body);
                sqlCondition = builder.Condition;
                parameters = builder.GetParameter<SqlParameter>();

            }
            sql = string.Format(sql, tableName, sqlCondition);
            return GetCommand(sql, parameters.ToArray());
        }
        public List<IDbCommand> GetInsert(object t, FwConfig fc)
        {
            var objs = fc.DataObjects.ToList();
            objs = objs.Concat(fc.DictObject).ToList();
            var type = t.GetType();
            var obj = objs.FirstOrDefault(w => w.ObjectCode == type.Name);
            var associationType = obj.Relation.ToList();
            List<IDbCommand> refInsert = new List<IDbCommand>();//添加关联表的命令
            List<IDbCommand> refDelete = new List<IDbCommand>();//添加关联表的命令
            var tableName = obj.KeyTableName;
            refInsert.Add(GetInsertSql(obj,t));

            foreach (var prop in associationType)
            {
//                if (ReflectionHelper.IsRelationTableAndSaveRelation(propertyInfo))//有关联表;要先删除关联表
//                var relConfig = rel.RelConfig;
                if (prop.RelationType == Relation.复杂关联 || prop.RelationType == Relation.简单关联)
                {
                    var sql1 = @"Delete From {0} Where {1} = '{2}' 
";
                    var relConfig = prop.RelConfig;

                    var relTableName = relConfig.RelTableName;
                    var relTableMasterKey = relConfig.RelMasertKey;
                    //                    var relTableMasterKey = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(prop).OtherKey;
                    var thisKeyName = obj.GetPrimary().Code;
                    //                    var thisKeyName = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(prop).ThisKey;
                    var propertyInfos = type.GetProperties();
                    var relProp = propertyInfos.FirstOrDefault(w => w.Name == prop.ObjectPorertity);
                    var prim = propertyInfos.FirstOrDefault(w => w.Name == thisKeyName);
                    var thisKeyValue = ReflectionHelper.GetPropertyValue(prim, t);
                    //                    var thisKeyValue = ReflectionHelper.GetPropertyValue(baseProps.First(z => z.Name == thisKeyName), t);
                    //var masterKeyValue = ReflectionHelper.GetPrimaryKey(t);
                    //var masterKeyValue = ReflectionHelper.GetPrimaryValue(t);
                    sql1 = string.Format(sql1, relTableName, relTableMasterKey, thisKeyValue);
                    refDelete.Add(GetCommand(sql1));

                    var pv = ReflectionHelper.GetPropertyValue(relProp, t) as IList;
                    if (pv != null)
                    {
                        var dictObj = objs.FirstOrDefault(z => z.ObjectCode == relConfig.DictTableName);
                        var arr = new ArrayList();
                        for (int i = 0; i < pv.Count; i++)
                        {
                            var item = pv[i];
                            var id = ReflectionHelper.GetPropertyValue("ID", item);
                            if (!arr.Contains(id))
                            {
                                IDbCommand command = GetInsertSql(t, item, obj, dictObj, prop);
                                refInsert.Add(command);
                            }

                            arr.Add(id);
                        }
                    }
                }
                else//只用字典表
                {

                }
            }
            refDelete.AddRange(refInsert);//先删除关联,再添加
            return refDelete;
        }
        public List<IDbCommand> GetInsert(object t, Type type)
        {
            var associationType = ReflectionHelper.GetBaseTypeProperty(type, PropertyType.CustomType);
            List<IDbCommand> refInsert = new List<IDbCommand>();//添加关联表的命令
            List<IDbCommand> refDelete = new List<IDbCommand>();//添加关联表的命令
            var tableName = ReflectionHelper.GetTableName(type);
            refInsert.Add(GetInsertSql(t));

            foreach (var propertyInfo in associationType)
            {
                if (ReflectionHelper.IsRelationTableAndSaveRelation(propertyInfo))//有关联表;要先删除关联表
                {
                    var sql1 = @"Delete From {0} Where {1} In (SELECT {2} FROM {3} WHERE {3}.{5} IN('{4}')) ";
                    var relTableName = ReflectionHelper.GetTableName(propertyInfo);
                    var relTableMasterKey = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(propertyInfo).OtherKey;
                    var masterKeyValue = ReflectionHelper.GetPrimaryValue(t);
                    //sql1 = string.Format(sql1, relTableName, relTableMasterKey, masterKeyValue);

                    var thisKey = ReflectionHelper.GetAttribute<LevcnAssociationAttribute>(propertyInfo).ThisKey;
                    sql1 = string.Format(sql1, relTableName, relTableMasterKey, thisKey, tableName, masterKeyValue, ReflectionHelper.GetPrimaryKey(type).Name);

                    refDelete.Add(GetCommand(sql1));
                    var pv = ReflectionHelper.GetPropertyValue(propertyInfo, t) as IList;
                    if (pv != null)
                    {
                        for (int i = 0; i < pv.Count; i++)
                        {
                            var item = pv[i];
                            IDbCommand command = GetInsertSql(item);
                            refInsert.Add(command);
                        }
                    }
                }
                else//只用字典表
                {

                }
            }
            refDelete.AddRange(refInsert);//先删除关联,再添加
            return refDelete;
        }
        /// <summary>
        /// 返回一个类型的
        /// </summary>
        /// <param name="o"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDbCommand GetInsertSql(object o)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            var type = o.GetType();
            var baseProps = ReflectionHelper.GetBaseTypeProperty(type, PropertyType.BaseType);
            var tableName = ReflectionHelper.GetTableName(type);
            //Update tableName Set a='t_a',b as t_b,c as t_c From t
            string sql = "Insert Into {0} ({1}) Values({2})";
            baseProps = baseProps.Where(w =>
            {
                var attributes = w.GetCustomAttributes(typeof(LevcnColumnAttribute), false);
                if (attributes.Any())
                {
                    var studentAttr = (LevcnColumnAttribute)attributes[0];
                    if (studentAttr.IsAutoInt) return false;
                }
                return true;
            }).ToList();
            var fields = baseProps.Select(ReflectionHelper.GetPropertyField).ToList();
            sql = string.Format(sql, tableName, ExtensionUtil.Serialize(fields, ","), ExtensionUtil.Serialize(fields.Select(w => "@" + w), ","));

            var paras = GetFieldParameates(o, baseProps.ToArray());
            parameters.AddRange(paras);
            return GetCommand(sql, parameters.ToArray());
        }
        /// <summary>
        /// 返回一个类型的
        /// </summary>
        /// <param name="edo"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDbCommand GetInsertSql(EDataObject edo,object o)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            var baseProps = edo.GetBaseProperties();
            var tableName = edo.KeyTableName;
            //Update tableName Set a='t_a',b as t_b,c as t_c From t
            string sql = "Insert Into {0} ({1}) Values({2})";
//            baseProps = baseProps.Where(w =>
//            {
//                var attributes = w.GetCustomAttributes(typeof(LevcnColumnAttribute), false);
//                if (attributes.Any())
//                {
//                    var studentAttr = (LevcnColumnAttribute)attributes[0];
//                    if (studentAttr.IsAutoInt) return false;
//                }
//                return true;
//            }).ToList();
            var fields = baseProps.Select(w=>w.Code).ToList();
            sql = string.Format(sql, tableName, ExtensionUtil.Serialize(fields, ","), ExtensionUtil.Serialize(fields.Select(w => "@" + w), ","));

            var paras = GetFieldParameates(o, baseProps.ToArray());
            parameters.AddRange(paras);
            return GetCommand(sql, parameters.ToArray());
        }
        public IDbCommand GetInsertSql(object o, object dict, EDataObject edo, EDataObject dictEdo, Relation rel)
        {
            string sql = "Insert Into {0} ({1}) Values({2})";
            var relConfig = rel.RelConfig;
            var tableName = relConfig.RelTableName;
            string[]fields = {"ID",relConfig.RelMasertKey,relConfig.RelDictKey};
            var mastValue = ReflectionHelper.GetPropertyValue(o.GetType().GetProperty(edo.GetPrimary().Code), o);
            var dictValue = ReflectionHelper.GetPropertyValue(dict.GetType().GetProperty(dictEdo.GetPrimary().Code), dict);
            var props = new []{
                new SqlParameter(fields[0], Guid.NewGuid()),
                new SqlParameter(fields[1], mastValue),
                new SqlParameter(fields[2], dictValue),
            };
            SqlCommand comm = new SqlCommand();
            sql = string.Format(sql, tableName, fields.Serialize(","), fields.Select(w => "@" + w).Serialize(","));
            comm.CommandText = sql;
            props.ToList().ForEach(w => comm.Parameters.Add(w));
            return comm;
        }

        /// <summary>
        /// 返回一个类型的
        /// </summary>
        /// <param name="o"></param>
        /// <param name="sqlOperType"></param>
        /// <returns></returns>
        public String GetSql(object o, SqlOperType sqlOperType, List<SearchEntry> lWhere = null)
        {
            var strSql = new StringBuilder();
            var tableName = ReflectionHelper.GetTableName(o.GetType());
            switch (sqlOperType)
            {
                case SqlOperType.Insert:
                    strSql.Append("Insert Into {0} ({1}) Values({2})");
                    GetInsertUpdateSql(o, strSql.ToString(), tableName, ref strSql);
                    break;
                case SqlOperType.Update:
                    strSql.Append("Update {0} Set {1} Where {2}");
                    GetInsertUpdateSql(o, strSql.ToString(), tableName, ref strSql);
                    break;
                case SqlOperType.Delete:
                    if (lWhere == null || lWhere.Count == 0)
                        strSql.Append(string.Format("Delete {0} Where 1=1", tableName));
                    else
                    {
                        strSql.Append(string.Format("DELETE {0} WHERE 1=1 ", ReflectionHelper.GetTableName(o.GetType())));
                        lWhere.ForEach(w => strSql.Append(string.Format(" and {0}{1}{2}", w.ColumnName, w.Flag, w.GetSearchValue())));
                    }
                    break;
                case SqlOperType.Sql:
                    strSql.Append(o);
                    break;
            }

            return strSql.ToString();
        }

        protected void GetInsertUpdateSql(object o, string strSql, string tableName, ref StringBuilder stringBuilder)
        {
            var baseProps = ReflectionHelper.GetBaseTypeProperty(o.GetType(), PropertyType.BaseType);
            var fields = baseProps.Select(ReflectionHelper.GetPropertyField).ToList();
            var paras = GetFieldParameates(o, baseProps.ToArray());
            var result = string.Format(strSql, tableName, ExtensionUtil.Serialize(fields, ","), ExtensionUtil.Serialize(paras.Select(w => w.ParameterName), ","));
            paras.ForEach(p =>
                {
                    result = result.Replace(p.ParameterName, String.Format("'{0}'", p.Value));
                });
            stringBuilder.Remove(0, stringBuilder.Length);
            stringBuilder.Append(result);
        }
        /// <summary>
        /// 返回一个表的查询SQL(Update)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<IDbCommand> GetInsert<T>(T t)
        {
            return GetInsert(t, typeof(T));
        }
        private string GetPrimaryKeyWhereString<T>()
        {
            return GetPrimaryKeyWhereString(typeof(T));
        }
        /// <summary>
        /// 返回一个类的主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private string GetPrimaryKeyWhereString(Type type)
        {
            var primaryKeyPar = ReflectionHelper.GetPrimaryKey(type);
            if (primaryKeyPar == null) throw new Exception("类别未设置主键" + type);
            //var val = ReflectionHelper.GetPropertyValue(primaryKeyPar, t);
            return string.Format("{0} = @{0}", ReflectionHelper.GetPropertyField(primaryKeyPar));
        }
        /// <summary>
        /// 返回一个类的主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private string GetPrimaryKeyWhereString(EDataObject type)
        {
            var primaryKeyPar = type.GetPrimary();
            if (primaryKeyPar == null) throw new Exception("类别未设置主键" + type);
            //var val = ReflectionHelper.GetPropertyValue(primaryKeyPar, t);
            return string.Format("{0} = @{0}", primaryKeyPar.Code);
        }
        /// <summary>
        /// 返回一个类型的所有参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseProps"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private List<DbParameter> GetFieldParameates<T>(T t, params PropertyInfo[] baseProps)
        {
            var type = typeof(BigFieldValue);
            return baseProps.Select(w =>
            {
                var propertyValue = ReflectionHelper.GetPropertyValue(w, t);
                var p = ReflectionHelper.GetAttribute<LevcnColumnAttribute>(w);
                if (p != null && (p.IsBigField || w.PropertyType == type))
                {
                    if (propertyValue != DBNull.Value)
                    {
                        propertyValue = JsonHelper.FastJsonSerializer(propertyValue);
                    }
                }
                
                return GetDbParameter("@" + ReflectionHelper.GetPropertyField(w), propertyValue);
            }).ToList();
        }
        /// <summary>
        /// 返回一个类型的所有参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseProps"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private List<DbParameter> GetFieldParameates<T>(T t, params Property[] baseProps)
        {
            var props = t.GetType().GetProperties();
            return baseProps.Select(w =>
            {

                var propertyValue = ReflectionHelper.GetPropertyValue(props.FirstOrDefault(z=>z.Name == w.Code), t);
                if (w.ColumnType == "大字段")
                {
                    if (propertyValue != null)
                    {
                        propertyValue = JsonHelper.FastJsonSerializer(propertyValue);
                    }
                }
//                var p = ReflectionHelper.GetAttribute<LevcnColumnAttribute>(w);

//                if (p != null && p.IsBigField)
//                {
//                    if (propertyValue != DBNull.Value)
//                    {
//                        propertyValue = JsonHelper.FastJsonSerializer(propertyValue);
//                    }
//                }
                return GetDbParameter("@" + w.Code, propertyValue);
            }).ToList();
        }
        /// <summary>
        /// 返回重写的字段名(field as tableName_Field)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static string GetColumnsAsString(string tableName, Property pi)
        {
            var f = pi.Code;
            return string.Format("{2}.{0} as {1}", f, GetColumnsRename(tableName, f), tableName);
        }
        /// <summary>
        /// 返回重写的字段名(field as tableName_Field)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static string GetColumnsAsString(string tableName, PropertyInfo pi)
        {
            var f = ReflectionHelper.GetPropertyField(pi);
            return string.Format("{2}.{0} as {1}", f, GetColumnsRename(tableName, f), tableName);
        }
        /// <summary>
        /// 返回重写的字段名(field as tableName_Field)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        string GetColumnsAsUpdate(PropertyInfo pi)
        {
            var f = ReflectionHelper.GetPropertyField(pi);
            var sql = "{0} = {1}";
            return string.Format(sql, f, "@" + f);
        }
        /// <summary>
        /// 返回重写的字段名(field as tableName_Field)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        string GetColumnsAsUpdate(Property pi)
        {
            var f = pi.Code;
            var sql = "{0} = {1}";
            return string.Format(sql, f, "@" + f);
        }
        /// <summary>
        /// 返回列重命名后的列名(tableName_Field)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetColumnsRename(string tableName, string fieldName)
        {
            return tableName + "_" + fieldName;
        }
    }
}
