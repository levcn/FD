using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fw.Extends;
using Fw.Reflection;
using Fw.UserAttributes;
using STComponse.CFG;


namespace StaffTrain.SVFw.DataAccess
{
    public class FwConfigHelper
    {/// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Relation> GetBlack(IList<Relation> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;

            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.ObjectPorertity.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) input.Remove(p);
            }
            return input;
        }
        /// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Relation> GetWhite(IList<Relation> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;
            List<Relation> re = new List<Relation>();
            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.ObjectPorertity.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) re.Add(p);
            }
            return re;
        }
        /// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Property> GetBlack(IList<Property> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;

            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) input.Remove(p);
            }
            return input;
        }
        /// <summary>
        /// 从input中删除filter列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Property> GetWhite(IList<Property> input, string[] filter)
        {
            if (filter == null || filter.Length == 0) return input;
            List<Property> re = new List<Property>();
            foreach (string s in filter)
            {
                var p = input.FirstOrDefault(w => w.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (p != null) re.Add(p);
            }
            return re;
        }

        public static List<TableAndColumnsFC> GetTableAndColumnsList(EDataObject type, ref List<TableRelation> tableNames, string _propertyInfo = null, bool onlyPi = false, int maxDeep = 2, LevcnAssociationAttribute association = null, Type parentType = null, string[] white = null, string[] black = null)
        {
            List<TableAndColumnsFC> re = new List<TableAndColumnsFC>();
            var baseType = type.Property;
            if (parentType == null)
            {
                baseType = GetBlack(baseType, black).ToObservableCollection();
                baseType = GetWhite(baseType, white).ToObservableCollection();
                var p = type.GetPrimary();
                if (!baseType.Contains(p)) baseType.Add(p);
            }

            if (maxDeep == -1) return re;
            if (!onlyPi)
            {
                re.Add(new TableAndColumnsFC
                {
                    Type = type,
                    TableName = type.KeyTableName,
                    PropertyInfos = baseType
                });
            }
            var customTypeProperty = type.Relation;
            if (parentType == null)
            {
                customTypeProperty = GetBlack(customTypeProperty, black).ToObservableCollection();
                customTypeProperty = GetWhite(customTypeProperty, white).ToObservableCollection();
            }
//            var associationType = customTypeProperty
//                .Where(w => GetAttribute<LevcnAssociationAttribute>(w) != null && GetTypeByProperty(w) != parentType);
            foreach (var propertyInfo in customTypeProperty)
            {
//                var currentAssociation = GetAttribute<LevcnAssociationAttribute>(propertyInfo);
                if (_propertyInfo == null || _propertyInfo.PropertyType == propertyInfo.PropertyType)
                {
                    Type tt = null;
                    bool isList = false;
                    if (propertyInfo.PropertyType.IsGenericType)
                    {
                        isList = true;
                        tt = propertyInfo.PropertyType.GetGenericArguments()[0];
                    }
                    else
                    {
                        tt = propertyInfo.PropertyType;
                    }
                    var deep = maxDeep - 1;
                    if (deep >= 0)
                    {
                        if (!isList || currentAssociation.Relation != RelationType.Multi) deep = 0;
                        var pRe = GetTableAndColumnsList(tt, ref tableNames, maxDeep: deep, association: currentAssociation, parentType: parentType);
                        re = re.Concat(pRe).ToList();
                        TableRelation tr = new TableRelation();
                        tr.TableName1 = GetTableName(type);
                        tr.TableName2 = GetTableName(tt);
                        var assoc = GetAttribute<LevcnAssociationAttribute>(propertyInfo);
                        tr.Column1 = assoc.ThisKey;
                        tr.Column2 = assoc.OtherKey;
                        tr.LeftJoin = assoc.LeftJoin;
                        tableNames.Add(tr);
                    }
                }
            }
            return re;
        }
    }
}
