using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using SLControls.Extends;
using StaffTrain.FwClass.DataClientTools;


namespace StaffTrain.FwClass.Reflectors
{
    /// <summary>
    /// 客户端使用的反射
    /// </summary>
    public static partial class ReflectionHelper
    {
        /// <summary>
        /// 从界面上返回搜索条件
        /// </summary>
        /// <param name="uiElement"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<SearchEntry> GetSearchEntry(Panel uiElement, Type type)
        {
            List<SearchEntry> re = new List<SearchEntry>();
            var tableName = GetTableName(type);
            re.AddRange(GetSearchEntryByBaseProperty(GetBaseTypeProperty(PropertyType.BaseType, type), uiElement, tableName));
            var associationType = GetBaseTypeProperty(type, PropertyType.CustomType);
            foreach (var propertyInfo in associationType)
            {
                List<TableRelation> trs1 = new List<TableRelation>();
                var tableColns = GetTableAndColumnsList(type, ref trs1, propertyInfo, true, propertyInfo.PropertyType.IsGenericType?1:0);
                tableColns.ForEach(w=> re.AddRange(GetSearchEntryByBaseProperty(GetBaseTypeProperty(PropertyType.BaseType, w.Type), uiElement, w.TableName)));
                
            }
            if (re.Count == 0) re = null;
            return re;
        }

        /// <summary>
        /// 返回基本属性列表的搜索条件
        /// </summary>
        /// <param name="getBaseTypeProperty"></param>
        /// <param name="uiElement"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IEnumerable<SearchEntry> GetSearchEntryByBaseProperty(List<PropertyInfo> getBaseTypeProperty, Panel uiElement, string tableName)
        {
            return getBaseTypeProperty.Select(w => GetGetSearchEntryItem(w, uiElement, tableName)).Where(w => w != null).ToList();
        }
        /// <summary>
        /// 根据属性返回搜索条件
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="uiElement"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static SearchEntry GetGetSearchEntryItem(PropertyInfo propertyInfo, Panel uiElement, string tableName)
        {
            SearchEntry re = null;
            var val = GetUIElementValue(uiElement, propertyInfo.Name)??"";
            val = val.Replace("'", "").Replace(";", "");
            if(!string.IsNullOrEmpty(val))
            {
                re = new SearchEntry
                             {
                                 ColumnName = tableName + "." + GetPropertyField(propertyInfo),
                                     Flag = "=",
                                     value = val
                             };
                if (propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType == typeof(Guid)||
                    propertyInfo.PropertyType == typeof(Guid?))
                {
                    re.value = string.Format("'{0}'", re.value);
                }
            }
            val = GetUIElementValue(uiElement, propertyInfo.Name + "_Like");
            if (!string.IsNullOrEmpty(val))
            {
                re = new SearchEntry
                {
                    ColumnName = tableName + "." + GetPropertyField(propertyInfo),
                    Flag = "=",
                    value = val
                };
                if (propertyInfo.PropertyType == typeof(string))
                {
                    re.Flag = "like";
                    re.value = string.Format("'%{0}%'", re.value);
                }
            }
            val = GetUIElementValue(uiElement, propertyInfo.Name + "_Start");
            if (!string.IsNullOrEmpty(val))
            {
                 re = new SearchEntry
                {
                    ColumnName = GetPropertyField(propertyInfo),
                    Flag = ">=",
                    value = val
                };
                
            }
            val = GetUIElementValue(uiElement, propertyInfo.Name + "_End");
            if (!string.IsNullOrEmpty(val))
            {
                 re = new SearchEntry
                {
                    ColumnName = GetPropertyField(propertyInfo),
                    Flag = "<=",
                    value = val
                };
            }
            if (re != null && propertyInfo.PropertyType == typeof(DateTime))
            {
                re.value = string.Format("'{0}'", re.value);
            }
            val = GetUIElementValue(uiElement, propertyInfo.Name + "_In");
            if (!string.IsNullOrEmpty(val))
            {
                var values = val.Split(',');
                re = new SearchEntry
                {
                    ColumnName = GetPropertyField(propertyInfo),
                    Flag = "in",
                    //value = val
                };
                if(propertyInfo.PropertyType == typeof(string)||propertyInfo.PropertyType == typeof(DateTime))
                {
                    re.value = string.Format("({0})",values.Select(w => string.Format("'{0}'", w)).ToList().Serialize(","));
                }
            }
            
            return re;
        }

        private static string GetUIElementValue(Panel uiElement,string name)
        {
            var o = uiElement.FindName(name) as UIElement;
            if (o != null)
            {
                if (o is TextBox) return (o as TextBox).Text;
                if (o is ComboBox)
                {
                    var obj = (o as ComboBox).SelectedValue;
                    if(obj !=null) return obj.ToString();
                }
                
            }
            return string.Empty;
        }
    }
}
