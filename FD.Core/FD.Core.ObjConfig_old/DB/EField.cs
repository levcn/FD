using System;
using STComponse.CFG;


namespace STComponse.DB
{

    /// <summary>
    /// 数据库的字段
    /// </summary>
    public class EField:ICloneable
    {
        public EField()
        {
//            ChangeType = ChangeType.Add;
        }
        public Guid ID { get; set; }
        public ChangeType ChangeType { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public string OldName { get; set; }
        /// <summary>
        /// GUID,字符串,整数,逻辑,小数,日期,备注
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否可以为空
        /// </summary>
        public bool Nullable { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
        //        public bool IsGuid { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimary { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public int Precision { get; set; }
        /// <summary>
        /// 默认值约束名称
        /// </summary>
        public string DefaultValueConstraint { get; set; }

        public string GetDefaultValue(string tableName)
        {
            if (!string.IsNullOrEmpty(DefaultValue))
            {
                var name = DefaultValueConstraint;

                var oldDesName = name;
                var newDesName = string.Format("DF_{0}_{1}", tableName, OldName ?? Name);
                if (string.IsNullOrEmpty(oldDesName)) oldDesName = newDesName;
                return string.Format(@"
--添加默认值(表:{0},字段:{1})
if exists( select * from sys.objects where name='{4}') ALTER TABLE [dbo].[{0}] drop  CONSTRAINT [{4}]
ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [{5}]  DEFAULT ({2}) FOR [{3}]
", tableName, OldName ?? Name, DefaultValue, Name, oldDesName, newDesName);
            }
            return null;
        }

        public string GetDeleteDefaultValue(string tableName)
        {
            if (!string.IsNullOrEmpty(DefaultValue))
            {
                var name = DefaultValueConstraint;

                var oldDesName = name;
                var newDesName = string.Format("DF_{0}_{1}", tableName, OldName ?? Name);
                if (string.IsNullOrEmpty(oldDesName)) oldDesName = newDesName;
                return string.Format(@"
--删除默认值(表:{0},字段:{1})
if exists( select * from sys.objects where name='{4}') ALTER TABLE [dbo].[{0}] drop  CONSTRAINT [{4}]
", tableName, OldName ?? Name, DefaultValue, Name, oldDesName, newDesName);
            }
            return null;
        }
        public string GetAlter()
        {
            if (ChangeType.HasFlag(ChangeType.Delete))
            {
                return string.Format("drop column [{0}]", Name);
            }
            if (ChangeType.HasFlag(ChangeType.Add)
                || ChangeType == ChangeType.None
                    )
            {
                return "ADD "+(string) this;
            }
            else if (ChangeType.HasFlag(ChangeType.ChangeAttr))
            {
                return "ALTER COLUMN " + (string) this;
            }
            else if (ChangeType.HasFlag(ChangeType.Rename) || ChangeType.HasFlag(ChangeType.ChangeRemark))
            {
                return null;
            }
            else
            {
                throw new PException("未处理的字段修改类型:" + ChangeType);
            }
        }
        /// <summary>
        /// 返回备注
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetRemark(string tableName)
        {
            if (FieldType == "关联") return null;
            var r = Remark ?? "";

            return string.Format(@"
--添加字段注释值(表:{1},字段:{2})
if exists (SELECT c.name,p.value FROM sys.extended_properties p ,sys.columns c WHERE p.major_id = OBJECT_ID('{1}') and c.name='{2}' and p.major_id = c.object_id and p.minor_id=c.column_id)
EXEC sys.sp_dropextendedproperty @name=N'MS_Description',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{1}', @level2type=N'COLUMN',@level2name=N'{2}'

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{0}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{1}', @level2type=N'COLUMN',@level2name=N'{2}'

", r.Replace("'", "''"), tableName,Name);
        }

        public static implicit operator string(EField f)
        {
            string str = "";


            if (f.ChangeType.HasFlag(ChangeType.Add)
                || f.ChangeType.HasFlag(ChangeType.None)
                || f.ChangeType.HasFlag(ChangeType.ChangeAttr)
                    )
            {
                str += string.Format("[{0}]", f.Name);
                if (f.FieldType == "GUID")
                {
                    str += " [uniqueidentifier]";
                    if (f.IsPrimary)
                    {
                        str += "   NOT NULL"; //ROWGUIDCOL
                    }
                    else
                    {
                        str += " NULL";
                    }
                }
                else if (f.FieldType == "字符串")
                {
                    str += string.Format(" [varchar]({0}) NULL", f.Length);
                }
                else if (f.FieldType == "整数")
                {
                    str += string.Format(" [int] NULL");
                }
                else if (f.FieldType == "逻辑")
                {
                    str += string.Format(" [int] NULL");
                }
                else if (f.FieldType == "小数")
                {
                    str += string.Format(" [decimal]({0}, {1}) NULL,", f.Length, f.Precision);
                }
                else if (f.FieldType == "日期")
                {
                    str += string.Format(" [datetime] NULL");
                }
                else if (f.FieldType == "备注" || f.FieldType == "大字段")
                {
                    str += string.Format(" [text] NULL");
                }
                else if (f.FieldType == "大字段")
                {
                    str += string.Format(" [text] NULL");
                }
                else if (f.FieldType == "关联")
                {
                    return null;
                }
                else
                {
                    throw new PException("未知的类型:" + f.FieldType);
                }
                return str;
            }
            else
            {
                if (f.ChangeType.HasFlag(ChangeType.ChangeRemark)
                    || f.ChangeType.HasFlag(ChangeType.Rename))
                {
                    return null;
                }
            }
            throw new PException("未处理的字段修改类型:"+f.ChangeType);
        }

        public object Clone()
        {
            return this.ToJson().ToObject<EField>();
        }

        /// <summary>
        /// 返回两个对象的属性是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool AttrEquals(EField other)
        {
            if (FieldType != other.FieldType) return false;
//            if (Remark != other.Remark) return false;
            var defaultValue = DefaultValue??"";
            var value = other.DefaultValue??"";
            if (defaultValue.Trim('(', ')', '\'') != value.Trim('(', ')', '\'')) return false;
            if (FieldType!="整数"&&Length != other.Length) return false;//整数不比较长度
            if (Precision != other.Precision) return false;
//            if (FieldType != other.FieldType) return false;
            return true;
        }

    }
}