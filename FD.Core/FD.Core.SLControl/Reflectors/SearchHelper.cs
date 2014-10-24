using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.Extends;
using StaffTrain.FwClass.DataClientTools;


namespace StaffTrain.FwClass.NavigatorTools
{
//    public class SearchHelper
//    {
//        /// <summary>
//        /// 给定一些条件,返回条件的SQL查询
//        /// </summary>
//        /// <param name="list"></param>
//        /// <param name="tableName">主表的表名 </param>
//        /// <param name="pKey">主表的主键 </param>
//        /// <returns></returns>
//        public static string GetWhereStr(List<SearchEntry> list)
//        {
//            if (list == null || list.Count == 0) return null;
//            list = list.Select(w =>
//            {
//                //if (string.Equals(w.ColumnName, pKey, StringComparison.OrdinalIgnoreCase))
//                //{
//                //    w.ColumnName = string.Format("{1}",  w.ColumnName);
//                //}
//                return w;
//            }).ToList();
//            var group1 = list.Where(w => string.IsNullOrEmpty(w.GroupName)).ToList();
//            var otherList = list.Where(w => !group1.Any(q => q == w)).ToList();
//            var otherGroups = otherList.GroupBy(w => w.GroupName).ToList();
//            var str = "";
//            if (group1.Count != 0)
//            {
//                str = GetOneWhereStr(group1, "and");
//                if (otherGroups.Count > 0) str += " and ";
//            }
//            if (otherGroups.Count > 0)
//            {
//                str += otherGroups.Select(w => GetOneWhereStr(w.ToList(), " or ")).ToList().Serialize(" and ");
//            }
//            return str;
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="group1"></param>
//        /// <param name="andOr"> </param>
//        /// <returns></returns>
//        private static string GetOneWhereStr(IEnumerable<SearchEntry> group1, string andOr)
//        {
//            return group1.Select(w => string.Format("{0} {1} {2}", w.ColumnName, w.Flag, w.value)).Serialize(" " + andOr + " ");
//        }
//    }
}
