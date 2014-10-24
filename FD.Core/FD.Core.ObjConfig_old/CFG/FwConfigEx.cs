using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using STComponse.DB;
using STComponse.Ex;


namespace STComponse.CFG
{
    public static class FwConfigEx
    {
        /// <summary>
        /// 返回是否可以删除对象
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool CanDeleteObject(this FwConfig config, string tableName,out List<EDataObject> list)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                list = new List<EDataObject>();
                return true;
            }
            var objs = config.DataObjects.ToList().Concat(config.DictObject).ToList();
            list = objs.Where(z => z.Relation.Any(w => w.RelConfig.RelTableName == tableName||w.RelConfig.DictTableName == tableName)).ToList();
            return !list.Any();
        }
        /// <summary>
        /// 对象是否可以修改名称
        /// </summary>
        /// <param name="config"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public static bool CanObjectRename(this FwConfig config, string oldName, string newName)
        {
            return _CanObjectRename(config, oldName, newName, w => w.ObjectName);
        }
        /// <summary>
        /// 对象是否可以修改对象代码
        /// </summary>
        /// <param name="config"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public static bool CanObjectRenameCode(this FwConfig config, string oldName, string newName)
        {
            return _CanObjectRename(config, oldName, newName, w => w.ObjectCode);
        }
        /// <summary>
        /// 对象是否可以修改表名
        /// </summary>
        /// <param name="config"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public static bool CanObjectRenameTableName(this FwConfig config, string oldName, string newName)
        {
            return _CanObjectRename(config, oldName, newName, w => w.KeyTableName);
        }
        /// <summary>
        /// 是否可以修改对象名称
        /// </summary>
        /// <param name="config"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        static bool _CanObjectRename(this FwConfig config, string oldName, string newName,Func<EDataObject,string> getAttr)
        {
            var re = config.DataObjects.All(w => getAttr(w)!= newName);
            if (!re) return false;
            re = config.DataObjects.All(w => getAttr(w) != newName);
            if (!re) return false;
            return true;
        }
        /// <summary>
        /// 修改对象代码
        /// </summary>
        /// <param name="config"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void ObjectRenameCode(this EDataObject config, string oldName, string newName)
        {
            if (config.ObjectCode == oldName)
            {
                config.ObjectCode = newName;
            }
//            config.Relation.ForEach(w =>
//            {
//                if (w.RelConfig.RelTableName == oldName) w.RelConfig.RelTableName = newName;
//                if (w.RelConfig.DictTableName == oldName) w.RelConfig.DictTableName = newName;
//            });
        }
        /// <summary>
        /// 修改对象名称
        /// </summary>
        /// <param name="config"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void ObjectRename(this EDataObject config, string oldName, string newName)
        {
            if (config.ObjectName == oldName)
            {
                config.ObjectName = newName;
            }
        }
        /// <summary>
        /// 修改对象名称
        /// </summary>
        /// <param name="config"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void ObjectRenameTableName(this EDataObject config, string oldName, string newName)
        {
            if (config.KeyTableName == oldName)
            {
                config.KeyTableName = newName;
            }
            config.Relation.ToList().ForEach(w =>
            {
                if (w.RelConfig.RelTableName == oldName)
                {
                    var a = w.RelConfig;
                    a.RelTableName = newName;
                    w.RelConfig = a;
                }
                if (w.RelConfig.DictTableName == oldName)
                {
                    var a = w.RelConfig;
                    a.DictTableName = newName;
                    w.RelConfig = a;
                }
            });
        }

        public static EDataObject FindObject(this FwConfig config, Func<EDataObject, bool> where)
        {
            var re = config.DataObjects.FirstOrDefault(@where);
           if(re==null)re = config.DictObject.FirstOrDefault(@where);
            return re;
        }
        public static void ObjectRename(this FwConfig config, string oldName, string newName)
        {
            config.DataObjects.ForEach(w => w.ObjectRename(oldName, newName));
            config.DictObject.ForEach(w => w.ObjectRename(oldName, newName));
        }
        public static void ObjectRenameCode(this FwConfig config,string oldName,string newName)
        {
            config.DataObjects.ForEach(w => w.ObjectRenameCode(oldName,newName));
            config.DictObject.ForEach(w => w.ObjectRenameCode(oldName,newName));
        }
        public static void ObjectRenameTableName(this FwConfig config, string oldName, string newName)
        {
            config.DataObjects.ForEach(w => w.ObjectRenameTableName(oldName, newName));
            config.DictObject.ForEach(w => w.ObjectRenameTableName(oldName, newName));
        }
        /// <summary>
        /// 修改对象属性代码
        /// </summary>
        /// <param name="config"></param>
        /// <param name="objectCode"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void AttributeRename(this EDataObject config, string objectCode, string oldName, string newName)
        {
            config.Relation.ToList().ForEach(w =>
            {
                var relConfig1 = w.RelConfig;
                if (relConfig1.RelTableName == objectCode)
                {
                    if (relConfig1.RelMasertKey == oldName)
                    {
                        relConfig1.RelMasertKey = newName;
                    }
                    if (relConfig1.RelDictKey == oldName)
                    {
                        relConfig1.RelDictKey = newName;
                    }
                }
                if (relConfig1.DictTableName == objectCode)
                {
                    if (relConfig1.DictKey == oldName) relConfig1.DictKey = newName;
                    if (relConfig1.DictShowName == oldName) relConfig1.DictShowName = newName;
                }
                w.RelConfig = relConfig1;

            });
        }
        /// <summary>
        /// 修改对象属性代码
        /// </summary>
        /// <param name="config"></param>
        /// <param name="objectCode"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void AttributeRename(this FwConfig config,string objectCode, string oldName, string newName)
        {
            config.DataObjects.ForEach(w => w.AttributeRename(objectCode,oldName, newName));
            config.DictObject.ForEach(w => w.AttributeRename(objectCode,oldName, newName));
        }
        /// <summary>
        /// 返回对象的主键属性
        /// </summary>
        /// <param name="edo"></param>
        /// <returns></returns>
        public static Property GetPrimary(this EDataObject edo)
        {
            var p = edo.Property.FirstOrDefault(w => w.IsPrimaryKey);
            return p;
        }

        /// <summary>
        /// 根据表名返回对象
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static EDataObject GetDataObjectByTableName(this FwConfig fc,string name)
        {
            var t = fc.DictObject.FirstOrDefault(w => w.KeyTableName == name);
            if (t != null) return t;
            t = fc.DataObjects.FirstOrDefault(w => w.KeyTableName == name);
            return t;
        }
        /// <summary>
        /// 根据表名返回对象
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static EDataObject GetDataObjectByCode(this FwConfig fc, string name)
        {
            var t = fc.DictObject.FirstOrDefault(w => w.ObjectCode == name);
            if (t != null) return t;
            t = fc.DataObjects.FirstOrDefault(w => w.ObjectCode == name);
            return t;
        }

        /// <summary>
        /// 根据对象,返回一个多表的数据库对象
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public static EDB ToDB(this FwConfig fc)
        {
            EDB edb = new EDB();
            edb.ETables = fc.DictObject.SelectMany(w => w.ToTable(fc)).Concat(fc.DataObjects.SelectMany(w => w.ToTable(fc)).ToList()).ToList();

            return edb;
        }

        /// <summary>
        /// 把对象转换成表
        /// </summary>
        /// <param name="edo"></param>
        /// <param name="fc"></param>
        /// <returns></returns>
        public static List<ETable> ToTable(this EDataObject edo,FwConfig fc)
        {
            if (string.IsNullOrEmpty(edo.KeyTableName))
            {
                throw new PException(string.Format("数据对象({0})的表名为空", edo.ObjectName));
            }
            List<ETable> et = new List<ETable> {
                new ETable {
                    ID = edo.ID,
                    TableName = edo.KeyTableName,
                    DisplayName = edo.ObjectName,
                    Remark = edo.Remark,
                    EFields = edo.Property.Select(w => w.ToField()).Where(w=>w!=null).ToList(),
                }
            };
            if (edo.Relation != null && edo.Relation.Count > 0)
            {
                var ls = edo.Relation.Where(w => w.RelationType == "简单关联").ToList();
                var lsTable = ls.Select(w =>
                {
                    var pid = edo.Property.FirstOrDefault(z => z.IsOutKey);
                    if (pid == null)
                    {
                        throw new Exception(string.Format("在类型({0})中,未找到对外使用的ID({1})", edo.ObjectName, w.ObjectPorertity));
                    }
                    var p = edo.Property.FirstOrDefault(z => z.Code == w.ObjectPorertity);

                    if(p==null)throw new Exception(string.Format("在类型({0})中,未找到关联的属性({1})",edo.ObjectName,w.ObjectPorertity));
                    var l = pid.ToField();//关联表中主表的关联字段
                    //                    l.Name = w.RelConfig.RelMasertKey;
                    l.Name = w.RelConfig.RelMasertKey;
                    l.DisplayName = w.RelConfig.RelMasertKey;
                    l.IsPrimary = false;
                    l.DefaultValue = "";
                    var r = fc.GetDataObjectByTableName(w.RelConfig.DictTableName);
                    if (r == null)
                    {
                        throw new Exception(string.Format("未找到数据表为({0})的数据对象.", w.RelConfig.DictTableName));
                    }
                    var rField = r.GetPrimary();
                    if (rField == null)
                    {
                        throw new Exception(string.Format("数据表({0})中未设置主键.", w.RelConfig.DictTableName));
                    }
                    var rf = rField.ToField();//关联表中字典表的关联字段
                    rf.Name = w.RelConfig.RelDictKey;
                    rf.DisplayName = w.RelConfig.RelDictKey;
                    rf.IsPrimary = false;
                    rf.DefaultValue = "";
                    return new ETable
                    {
                        DisplayName = w.RelConfig.RelTableName,
                        TableName = w.RelConfig.RelTableName,
                        EFields = new List<EField> {
                            new EField {
                                DefaultValue = "newid()",
                                FieldType = "GUID",
                                IsPrimary = true,
                                Name = "ID",
                                DisplayName = "ID",
                            },
                            l,
                            rf,
                        }
                    };
                }).ToList();
                et = et.Concat(lsTable).ToList();
            }
            return et;
        }

        /// <summary>
        /// 属性转换成表字段
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static EField ToField(this Property p)
        {
            if (p.ColumnType == Property.关联) return null;
            EField ef = new EField {};
            if (p.IsPrimaryKey) ef.IsPrimary = true;
            if (p.IsPrimaryKey && p.ColumnType == "GUID") ef.DefaultValue = "newid()";
            ef.FieldType = p.ColumnType;
            if (p.VarcharConfig != null) ef.Length = p.VarcharConfig.Length;
            if (p.DecimalConfig != null)
            {
                ef.Length = p.DecimalConfig.Length;
                ef.Precision = p.DecimalConfig.Precision;
            }
            if (p.VarcharConfig != null) ef.Length = p.VarcharConfig.Length;
            ef.ID = p.ID;
            ef.Name = p.Code;
            ef.DisplayName = p.Name;
            ef.Remark = string.Format("{0}/{1}/{2}",p.Name,p.ID,p.Remark??"");
            return ef;
        }
//
//        /// <summary>
//        /// 根据对象,返回一个多表的数据库对象
//        /// </summary>
//        /// <param name="fc"></param>
//        /// <returns></returns>
//        public static FwConfig ToExtendConfig(this FwConfig fc)
//        {
//            FwConfig edb = new FwConfig();
//            edb.DataObjects = new ObservableCollection<EDataObject>(fc.DictObject.SelectMany(w => w.ToExtendObjects(fc)).Concat(fc.DataObjects.SelectMany(w => w.ToExtendObjects(fc)).ToList()));
//            return edb;
//        }
//
//        /// <summary>
//        /// 把对象转换成表
//        /// </summary>
//        /// <param name="edo"></param>
//        /// <param name="fc"></param>
//        /// <returns></returns>
//        public static List<EDataObject> ToExtendObjects(this EDataObject edo, FwConfig fc)
//        {
//            if (string.IsNullOrEmpty(edo.KeyTableName))
//            {
//                throw new PException(string.Format("数据对象({0})的表名为空", edo.ObjectName));
//            }
//            List<EDataObject> et = new List<EDataObject> {
//                edb
//            };
//            if (edo.Relation != null && edo.Relation.Count > 0)
//            {
//                var ls = edo.Relation.Where(w => w.RelationType == "简单关联").ToList();
//                var lsTable = ls.Select(w =>
//                {
//                    var pid = edo.Property.FirstOrDefault(z => z.IsOutKey);
//                    if (pid == null)
//                    {
//                        throw new Exception(string.Format("在类型({0})中,未找到对外使用的ID({1})", edo.ObjectName, w.ObjectPorertity));
//                    }
//                    var p = edo.Property.FirstOrDefault(z => z.Code == w.ObjectPorertity);
//
//                    if (p == null) throw new Exception(string.Format("在类型({0})中,未找到关联的属性({1})", edo.ObjectName, w.ObjectPorertity));
//                    var l = pid.ToField();//关联表中主表的关联字段
//                    //                    l.Name = w.RelConfig.RelMasertKey;
//                    l.Name = w.RelConfig.RelMasertKey;
//                    l.DisplayName = w.RelConfig.RelMasertKey;
//                    l.IsPrimary = false;
//                    l.DefaultValue = "";
//                    var r = fc.GetDataObjectByTableName(w.RelConfig.DictTableName);
//                    if (r == null)
//                    {
//                        throw new Exception(string.Format("未找到数据表为({0})的数据对象.", w.RelConfig.DictTableName));
//                    }
//                    var rField = r.GetPrimary();
//                    if (rField == null)
//                    {
//                        throw new Exception(string.Format("数据表({0})中未设置主键.", w.RelConfig.DictTableName));
//                    }
//                    var rf = rField.ToField();//关联表中字典表的关联字段
//                    rf.Name = w.RelConfig.RelDictKey;
//                    rf.DisplayName = w.RelConfig.RelDictKey;
//                    rf.IsPrimary = false;
//                    rf.DefaultValue = "";
//                    return new ETable
//                    {
//                        DisplayName = w.RelConfig.RelTableName,
//                        TableName = w.RelConfig.RelTableName,
//                        EFields = new List<EField> {
//                            new EField {
//                                DefaultValue = "newid()",
//                                FieldType = "GUID",
//                                IsPrimary = true,
//                                Name = "ID",
//                                DisplayName = "ID",
//                            },
//                            l,
//                            rf,
//                        }
//                    };
//                }).ToList();
//                et = et.Concat(lsTable).ToList();
//            }
//            return et;
//        }
//
//        /// <summary>
//        /// 属性转换成表字段
//        /// </summary>
//        /// <param name="p"></param>
//        /// <returns></returns>
//        public static EField ToField(this Property p)
//        {
//            if (p.ColumnType == Property.关联) return null;
//            EField ef = new EField { };
//            if (p.IsPrimaryKey) ef.IsPrimary = true;
//            if (p.IsPrimaryKey && p.ColumnType == "GUID") ef.DefaultValue = "newid()";
//            ef.FieldType = p.ColumnType;
//            if (p.VarcharConfig != null) ef.Length = p.VarcharConfig.Length;
//            if (p.DecimalConfig != null)
//            {
//                ef.Length = p.DecimalConfig.Length;
//                ef.Precision = p.DecimalConfig.Precision;
//            }
//            if (p.VarcharConfig != null) ef.Length = p.VarcharConfig.Length;
//            ef.ID = p.ID;
//            ef.Name = p.Code;
//            ef.DisplayName = p.Name;
//            ef.Remark = string.Format("{0}/{1}/{2}", p.Name, p.ID, p.Remark ?? "");
//            return ef;
//        }
    }
}