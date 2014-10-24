using System.Collections.Generic;


namespace Fw.Entity
{
    public class SelectAcionParams
    {
        public List<SearchEntry> Search;//查询条件
        public PageInfo PageInfo;//分页
        //public string GroupBy;
        public string OrderBy;
        public string WhereStr;
        //public List<ColumnEntry> Columns;//操作列
    }
}