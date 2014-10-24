using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace STComponse.DB
{
    /// <summary>
    /// 库比较返回差异
    /// </summary>
    public class EDBCompare
    {
        public static EDB Compare(EDB old, EDB _new)
        {
            EDB re = null;
            if (_new != null) //如果新的不为空
            {
                re = _new.Clone() as EDB;
                Debug.Assert(re != null);
                if (old == null)
                {
//                    re.ChangeType |= ChangeType.Add;
                }
                else
                {
                    re.ETables.Clear();
                    _new.ETables.ForEach(newField =>
                    {
                        var eTable = old.ETables.SearchItem(newField);
//                        var eTable = old.ETables.FirstOrDefault(z => z.ID == newField.ID);
//                        if (eTable == null)//如果根据ID没有找到
//                        {
//                            eTable = old.ETables.FirstOrDefault(z => z.TableName == newField.TableName);
//                            if (eTable == null) //如果根据表名没有找到
//                            {
//                                eTable = old.ETables.FirstOrDefault(z => z.DisplayName == newField.DisplayName);//使用表的名称
//                            }
//                        }
                        var r = ETableCompare.Compare(eTable, newField);
                        if (r != null) //比较表差异,如果有不同之处,添加到列表中
                        {
                            re.ETables.Add(r);
                        }
                    });
                    //在新版本表中删除了的表
                    var deletedFields = old.ETables.Where(w => _new.ETables.SearchItem(w) ==null).Select(w => ETableCompare.Compare(w, null)).ToList();
                    re.ETables.AddRange(deletedFields);
                }
            }
            return re;
        }

    }

    /// <summary>
    /// 表比较,返回差异
    /// </summary>
    public class ETableCompare
    {
        public static ETable Compare(ETable old, ETable _new)
        {
            ETable re = null;
            if (_new != null) //如果新的不为空
            {
                re = _new.Clone() as ETable;
                Debug.Assert(re != null);
                if (old == null)
                {
                    re.ChangeType |= ChangeType.Add;
                }
                else
                {
                    if (old.TableName != _new.TableName)
                    {
                        re.ChangeType |= ChangeType.Rename;
                        re.OldTableName = old.TableName;
                    }
                    re.EFields.Clear();
                    _new.EFields.ForEach(newField =>
                    {
                        var oldField = old.EFields.SearchItem(newField);
//                        var oldField = old.EFields.FirstOrDefault(z => z.ID == newField.ID);
//                        if (oldField == null)//如果根据ID没有找到
//                        {
//                            oldField = old.EFields.FirstOrDefault(z => z.Name == newField.Name);
//                            if (oldField == null) //如果根据表名没有找到
//                            {
//                                oldField = old.EFields.FirstOrDefault(z => z.DisplayName == newField.DisplayName);//使用表的名称
//                            }
//                        }
                        var r = EFieldCompare.Compare(oldField, newField);
                        if (r != null)//比较字段差异,如果有不同之处,添加到列表中
                        {
                            re.EFields.Add(r);
                        }
                    });
                    //在新版本表中删除了的字段
                    var deletedFields = old.EFields.Where(w => _new.EFields.SearchItem(w)==null).Select(w => EFieldCompare.Compare(w, null)).ToList();
                    re.EFields.AddRange(deletedFields);
                    if (re.EFields.Count > 0)
                    {
                        re.ChangeType |= ChangeType.ChangeColumn;
                    }
                }
            }
            else
            {
                if (old != null)
                {
                    re = old.Clone() as ETable;
                    Debug.Assert(re != null);
                    re.ChangeType |= ChangeType.Delete;
                    re.EFields = new List<EField>();
                }
            }
            if (re != null && re.ChangeType == ChangeType.None && re.EFields.Count == 0) re = null;
           
            return re;
        }
    }

    /// <summary>
    /// 字段比较返回差异
    /// </summary>
    public class EFieldCompare
    {
        /// <summary>
        /// 对比两个字段,并返回差异,如果一样,返回null
        /// </summary>
        /// <param name="old"></param>
        /// <param name="_new"></param>
        /// <returns></returns>
        public static EField Compare(EField old, EField _new)
        {

            EField re = null;
            if (_new != null) //如果新的不为空
            {
                re = _new.Clone() as EField;
                Debug.Assert(re!=null);
                if (old == null)
                {
                    re.ChangeType |= ChangeType.Add;
                }
                else
                {
                    if (old.Name != _new.Name)
                    {
                        re.ChangeType |= ChangeType.Rename;
                        re.OldName = old.Name;
                    }
                    if (!old.AttrEquals(_new))
                    {
                        re.ChangeType |= ChangeType.ChangeAttr;
                    }
                    if (!(old.Remark == null && _new.Remark != null ||
                          (old.Remark != null && _new.Remark == null)))
                    {
                        if (!string.Equals(old.Remark, _new.Remark, StringComparison.OrdinalIgnoreCase))
                        {
                            re.ChangeType |= ChangeType.ChangeRemark;
                        }
                    }
                }
            }
            else
            {
                if (old != null)
                {
                    re = old.Clone() as EField;
                    Debug.Assert(re != null);
                    re.ChangeType |= ChangeType.Delete;
                }
            }
            if (re != null && re.ChangeType == ChangeType.None) re = null;
            if (re != null && old!=null) re.DefaultValueConstraint = old.DefaultValueConstraint;
            return re;
        }

    }
}
