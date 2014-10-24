using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STComponse.CFG;


namespace STComponse.GCode
{
    public static class FwConfigCodeEx
    {
        public static void SaveCodeFile(this FwConfig fc,string template, string path)
        {
            var a = fc.DataObjects.SelectMany(w =>
            {
                try
                {
                    return w.ToClass(fc);
                }
                catch (PException eee)
                {
                    throw new PException(string.Format("(Name:{0};Code:{1})生成类文件出现异常",w.ObjectName,w.ObjectCode)+eee.Message);
                }

            }).ToList();
            var b = fc.DictObject.SelectMany(w =>
            {
                try
                {
                    return w.ToClass(fc);
                }
                catch (PException eee)
                {
                    throw new PException(string.Format("(Name:{0};Code:{1})生成类文件出现异常", w.ObjectName, w.ObjectCode) + eee.Message);
                }

            }).ToList();
            var c = a.Concat(b).ToList();
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (Directory.Exists(path))
            {
                c.ForEach(w =>
                {
                    var fullFilePath = Path.Combine(path, w.ClassName+".cs");
                    File.WriteAllText(fullFilePath, w.ToCodeStr(template),Encoding.UTF8);
                });
            }
        }

        public static List<GClass> ToClass(this EDataObject edo, FwConfig fc)
        {
            if(string.IsNullOrEmpty(edo.ObjectCode))throw new PException(string.Format("对象({0})的Code为空",edo.ObjectName));
            if (string.IsNullOrEmpty(edo.ObjectName)) throw new PException(string.Format("对象({0})的Code为空", edo.ObjectCode));
            if (edo.Property == null) throw new PException(string.Format("对象({0})的属性列表为空", edo.ObjectCode));
            List<GClass> re = new List<GClass> {
                new GClass {
                    ClassName = edo.ObjectCode,
                    Properties = edo.Property.Select(w => w.ToGProperty(edo)).ToList(),
                }
            };
            //添加字典属性
            re[0].Properties.AddRange(edo.Relation.Where(w => w.RelationType == "字典").Select(w => w.ToGProperty(edo)).ToList());
//            if (edo.Relation != null && edo.Relation.Count > 0)
//            {
//                var relations = edo.Relation.Where(w => w.RelationType == "简单关联").ToList();
//                var classes = relations.Select(w =>
//                {
//                    var p = edo.Property.FirstOrDefault(z => z.Code == w.ObjectPorertity);
//                    if (p == null) throw new PException(string.Format("在类型({0})中,未找到关联的属性({1})", edo.ObjectName, w.ObjectPorertity));
//                    var l = p.ToGProperty(edo);//关联表中主表的关联字段
//                    l.PropertyName = w.RelConfig.RelMasertKey;
//                    var dataObject = fc.GetDataObjectByTableName(w.RelConfig.DictTableName);
//                    if (dataObject == null)
//                    {
//                        throw new Exception(string.Format("未找到数据表为({0})的数据对象.", w.RelConfig.DictTableName));
//                    }
//                    var primary = dataObject.GetPrimary();
//                    if (primary == null)
//                    {
//                        throw new Exception(string.Format("数据表({0})中未设置主键.", w.RelConfig.DictTableName));
//                    }
//                    var rf = primary.ToGProperty(edo);//关联表中字典表的关联字段
//                    rf.PropertyName = w.RelConfig.RelDictKey;
//                    return new GClass
//                    {
//                        ClassName = w.RelConfig.RelTableName,
//                        Properties = new List<GProperty> {
//                            new GProperty {
//                                TypeName = "Guid",
//                                PropertyName = "ID",
//                            },
//                            l,
//                            rf,
//                        }
//                    };
//                }).ToList();
//                re = re.Concat(classes).ToList();
//            }
            return re;
        }
        /// <summary>
        /// 返回属性在C#代码中类型字符串
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string ToCType(this Property p)
        {
            string re = "";
            switch (p.ColumnType)
            {
                case "字符串":
                case "备注":
                    re= "string";
                    break;
                case "GUID":
                    re = "Guid?";
                    break;
                case "整数":
                    re = "int";
                    break;
                case "逻辑":
                    re = "int?";
                    break;
                    
                case "小数":
                    re = "decimal?";
                    break;
                case "日期":
                    re = "DateTime?";
                    break;
                case "大字段":
                    re = "string";
                    break;
//                case "关联":
//                    re = string.Format("List<{0}>",p.);
//                    break;
                default:
                    throw new PException(string.Format("属性(Name:{1},Code:{2}),未知的类型({0})",p.ColumnType,p.Name,p.Code));
            }
            if (p.IsPrimaryKey) re = re.Replace("?", "");
            return re;
        }

        public static GProperty ToGProperty(this Relation p, EDataObject edb)
        {
            var type = p.RelConfig.IsDict?p.RelConfig.DictTableName:p.RelConfig.RelTableName;
//            var rel = edb.Property.FirstOrDefault(w => w.Name == p.ObjectPorertity);
//            if (rel == null)
//            {
//                throw new PException(string.Format("字典属性中未找对应的主键属性({0},{1})", edb.ObjectName, p.ObjectPorertity));
//            }
//            type = GetGenPropertyName(type);
            return new GProperty
            {
                PropertyName = type,
                TypeName = type,
            };
        }
        public static GProperty ToGProperty(this Property p,EDataObject edb = null)
        {
            if (p.ColumnType == "关联")
            {
                var rel = edb.Relation.FirstOrDefault(w => w.ObjectPorertity == p.Code);
                if (rel == null)
                {
                    throw new PException(string.Format("关联属性中未找到关联信息({0},{1})", edb.ObjectName, p.Name));
                }
                if (rel.RelationType == Relation.简单关联)
                {
                    return new GProperty {
                        PropertyName = GetGenPropertyName(p.Code),
//                        DefaultValue = p.InitValue,
                        TypeName = string.Format("List<{0}>", rel.RelConfig.DictTableName),
                    };
                }
                else
                {
                    return new GProperty
                    {
                        PropertyName = GetGenPropertyName(p.Code),
//                        DefaultValue = p.InitValue,
                        TypeName = string.Format("List<{0}>", rel.RelConfig.RelTableName),
                    };
                }
            }
            else
            {
                var defaultValue = "";
                if (p.ColumnType == Property.大字段)
                {
                    var bigfield = edb.GetBigFieldByPropertyID(p.ID);
                    if (bigfield != null)
                    {
                        defaultValue = bigfield.ToJson();
                    }
                }
                return new GProperty {
                    PropertyName = GetGenPropertyName(p.Code),
                    DefaultValue =  defaultValue,
                    TypeName = p.ToCType(),
                };
            }
        }
        public static List<string> TagPix = new List<string>();
        public static string GetGenPropertyName(string code)
        {
            foreach (string s in TagPix)
            {
                if (code.StartsWith(s))
                {
                    return code.Substring(s.Length);
                }
            }
            return code;
        }
    }
}
