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
using SLControls.Editors;
using StaffTrain.FwClass.DataClientTools;


namespace StaffTrain.FwClass.NavigatorTools
{
    public class PagedList<T>
    {
        private List<T> DataList;
        public int PageSize { get; private set; }
        public int PageIndex { get; private set; }
        public int PageCount { get; private set; }
        public PagedList(List<T> dataList,int pageSize,int pageIndex)
        {
            if (dataList==null)dataList = new List<T>();
            DataList = dataList;
            PageSize = pageSize;
            PageIndex = pageIndex;
            PageCount = (int)Math.Ceiling((DataList.Count)/(double)PageSize);
        }
        public List<T> GetData(Func<List<T>,List<T>> order)
        {
            var list = order(DataList);
            return list.Skip((PageIndex - 1)*PageSize).Take(PageSize).ToList();
        }
        public PageInfo GetPageInfo()
        {
            return new PageInfo { PageIndex = PageIndex, PageSize = PageSize, TotalPage = PageCount, TotalRecord = DataList.Count};
        }
        public static List<T>GetData<T>(List<T> dataList,int pageSize,int pageIndex,Func<List<T>,List<T>> order,out PageInfo pi)
        {
            var p = new PagedList<T>(dataList, pageSize, pageIndex);
            pi = p.GetPageInfo();
            return p.GetData(order);
        }
    }
}
