using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STComponse.CFG;


namespace STComponse.DB
{
    public class ETable : ICloneable
    {
        public ETable()
        {
//            ChangeType = ChangeType.Add;
        }
        public Guid ID { get; set; }
        public ChangeType ChangeType { get; set; }
        public List<EField> EFields { get; set; }

        /// <summary>
        /// 表名(SYS_User)
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 显示名(人员表)
        /// </summary>
        public string DisplayName { get; set; }
        public string OldTableName { get; set; }

        /// <summary>
        /// 返回字段改名的查询
        /// </summary>
        /// <returns></returns>
        
        public string GetRenameColumnSql()
        {
            var renameColumnSql = EFields.Where(w => w.ChangeType == ChangeType.Rename)
                    .Select(w => string.Format("EXEC sp_rename '{0}.[{1}]','[{2}]','COLUMN'"
                            ,TableName,w.OldName,w.Name)).ToList();
            if (renameColumnSql.Count() == 0)
            {
                return "";
            }
            else
            {
                return string.Format(@"
--修改字段名(表:{1})
{0}", 
    renameColumnSql.Serialize1("\r\n\r\n"),
    TableName);
            }
        }

        public string GetRenameSql()
        {
            if (ChangeType == ChangeType.Rename)return string.Format(@"
--修改表名(原:{0},新:{1})
EXEC sp_rename '[{0}]', '[{1}]'", OldTableName,TableName);
            return "";
        }
        public static implicit operator string(ETable f)
        {
            if (f.ChangeType == ChangeType.Add || f.ChangeType == ChangeType.None)//添加表
            {
                return string.Format(@"
--创建表({1})
if object_id(N'{1}',N'U') is null
CREATE TABLE [dbo].[{1}](
{0}
)",
                        f.EFields.Select(w => (string) w).Where(w=>w!=null).Serialize1(",\r\n"),
                        f.TableName);
            }
            else if (f.ChangeType == ChangeType.ChangeColumn)//修改表
            {
                var str = f.EFields.Select(w =>
                {
                    var alter = w.GetAlter();
                    if (w.ChangeType.HasFlag(ChangeType.Add))
                    {
                        return string.Format(@"
--添加字段(表:{0},字段:{1})
if not exists(select * from syscolumns where id=object_id('{0}') and name='{1}')Alter TABLE [dbo].[{0}] {2}", f.TableName, w.Name, alter);
                    }
                    else if (w.ChangeType.HasFlag(ChangeType.ChangeAttr))
                    {
                        return string.Format(@"
--修改字段属性(表:{0},字段:{1})
if exists(select * from syscolumns where id=object_id('{0}') and name='{1}')Alter TABLE [dbo].[{0}] {2}", f.TableName, w.Name, alter);
                    }
                    else if (w.ChangeType.HasFlag(ChangeType.Rename))
                    {
                        return string.Format(@"
--修改字段名(表:{0},字段:{1})
if exists(select * from syscolumns where id=object_id('{0}') and name='{1}')EXECUTE sp_rename N'dbo.{0}.{1}','{2}'", f.TableName, w.OldName, w.Name);
                    }
                    else if (w.ChangeType.HasFlag(ChangeType.Delete))
                    {
                        return string.Format(@"
--删除字段(表:{0},字段:{1})
{3}
if exists(select * from syscolumns where id=object_id('{0}') and name='{1}')Alter TABLE [dbo].[{0}] {2}", f.TableName, w.Name, alter,w.GetDeleteDefaultValue(f.TableName));
                    }
                    else if (w.ChangeType.HasFlag(ChangeType.ChangeRemark))
                    {
                        return w.GetRemark(f.TableName);
                    }
                    else
                    {
                        throw new PException("未处理的字段修改类型:"+w.ChangeType);
                    }
                });
                return string.Format(@"
--修改字段开始(表:{1})
if object_id(N'{1}',N'U') is not null
Begin
{0}
End
--修改字段结束(表:{1})
",
 str.Where(w=>w!=null).Serialize1("\r\n"),
//                        f.EFields.Select(w => w.GetAlter()).Serialize(",\r\n"),
                        f.TableName);
            }
            else if (f.ChangeType == ChangeType.Delete) //修改表
            {
                return string.Format(@"
--删除表({0})
if not(object_id(N'{0}',N'U') is null)
Drop TABLE [dbo].[{0}]",
                        f.TableName);
            }
                throw new PException(string.Format("未知的表变化类型:({0})", f.ChangeType));
        }

        /// <summary>
        /// 返回备注
        /// </summary>
        /// <returns></returns>
        public string GetFieldsRemarkSql()
        {
            var enumerable = EFields.Where(w => w.ChangeType == ChangeType.ChangeAttr || w.ChangeType == ChangeType.None || w.ChangeType == ChangeType.Add).Select(w => w.GetRemark(TableName)).Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
            return enumerable.Count == 0 ? "" : enumerable.Serialize1("\r\nGO\r\n\r\n");
        }

        public string GetDefaultValue()
        {
            var enumerable = EFields.Where(w => w.ChangeType == ChangeType.ChangeAttr || w.ChangeType == ChangeType.None || w.ChangeType == ChangeType.Add).Select(w => w.GetDefaultValue(TableName)).Where(w => w != null).ToList();
            return enumerable.Count == 0 ? "" : enumerable.Serialize1("\r\nGO\r\n\r\n");
        }

        public object Clone()
        {
            return this.ToJson().ToObject<ETable>();
        }

        public string Remark { get; set; }
    }

    public static class ETableEx
    {
        /// <summary>
        /// 在列表中找到指定的条目
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static EField SearchItem(this List<EField> list, EField item)
        {
            var field = list.FirstOrDefault(z => z.ID == item.ID && item.ID!=Guid.Empty);
            if (field == null)//如果根据ID没有找到
            {
                field = list.FirstOrDefault(z => z.Name == item.Name);
                if (field == null) //如果根据表名没有找到
                {
                    field = list.FirstOrDefault(z => z.DisplayName == item.DisplayName);//使用表的名称
                }
            }
            return field;
        }/// <summary>
        /// 在列表中找到指定的条目
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ETable SearchItem(this List<ETable> list, ETable item)
        {
            var eTable = list.FirstOrDefault(z => z.ID == item.ID && item.ID != Guid.Empty);
            if (eTable == null)//如果根据ID没有找到
            {
                eTable = list.FirstOrDefault(z => z.TableName == item.TableName);
                if (eTable == null) //如果根据表名没有找到
                {
                    eTable = list.FirstOrDefault(z => z.DisplayName == item.DisplayName);//使用表的名称
                }
            }
            return eTable;
        }
    }
}
