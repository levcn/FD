using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STComponse.CFG;


namespace STComponse.DB
{
    /// <summary>
    /// 数据库
    /// </summary>
    public class EDB : ICloneable
    {
        public List<ETable> ETables { get; set; }

        public static implicit operator string(EDB f)
        {
            var createTableRenameFieldStr = f.ETables.Select(w => (string)w).Where(w => !string.IsNullOrEmpty(w)).Serialize1("\r\nGO\r\n\r\n");
            var tableRemark = f.ETables.Select(w => w.GetFieldsRemarkSql()).Where(w => !string.IsNullOrEmpty(w)).Serialize1("\r\nGO\r\n\r\n");
            var defaultValue = f.ETables.Select(w => w.GetDefaultValue()).Where(w => !string.IsNullOrEmpty(w)).Serialize1("\r\nGO\r\n\r\n");
            //            var renameField = f.ETables.Select(w => w.GetRenameColumnSql()).Where(w => !string.IsNullOrEmpty(w)).Serialize("\r\nGO\r\n\r\n");
            var renameTable = f.ETables.Select(w => w.GetRenameSql()).Where(w => !string.IsNullOrEmpty(w)).Serialize1("\r\nGO\r\n\r\n");

            return string.Format(@"SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

{0}

{1}
SET ANSI_PADDING OFF
GO

{2}

{3}", renameTable, createTableRenameFieldStr, tableRemark, defaultValue);
        }

        public object Clone()
        {
            return this.ToJson().ToObject<EDB>();
        }
    }

    public static class EDBEx
    {
        public static string GetChangeText(this EDB db)
        {
            StringBuilder sb = new StringBuilder();
            db.ETables.ForEach(e =>
            {
                if (e.ChangeType == ChangeType.Delete) sb.AppendLine("删除表:" + e.TableName);
                else if (e.ChangeType == ChangeType.None || e.ChangeType == ChangeType.Add) sb.AppendLine("添加表:" + e.TableName);
                else
                {
                    if (e.ChangeType == ChangeType.Rename) sb.AppendLine(string.Format("修改表名:{0}->,{1};", e.OldTableName, e.TableName));
                    if (e.ChangeType == ChangeType.ChangeRemark) sb.AppendLine("修改备注:" + e.TableName);
                    if (e.ChangeType == ChangeType.ChangeColumn)
                    {
                        StringBuilder sb1 = new StringBuilder();
                        sb.AppendLine("表:" + e.TableName);
                        e.EFields.ForEach(w =>
                        {
                            if (w.ChangeType.HasFlag(ChangeType.Delete)) sb1.AppendLine("    删除字段:" + w.Name);
                            if (w.ChangeType.HasFlag(ChangeType.Rename)) sb1.AppendLine(string.Format("    修改字段名:{0}->,{1};", w.OldName, w.Name));
                            if (w.ChangeType ==ChangeType.None || w.ChangeType .HasFlag(ChangeType.Add)) sb1.AppendLine("    添加字段名:" + w.Name);
                            if (w.ChangeType .HasFlag( ChangeType.ChangeRemark)) sb1.AppendLine("    修改备注:" + w.Name);
                            if (w.ChangeType .HasFlag( ChangeType.ChangeAttr)) sb1.AppendLine("    修改字段属性:" + w.Name);

                        });
                        sb1.AppendLine("---");
                        sb.AppendLine(sb1.ToString());
                    }
                }
            });
            return sb.ToString();
        }
    }

}