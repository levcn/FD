using System.Collections.Generic;


namespace Fw.Entity
{
    public class SelectAcionParams
    {
        public List<SearchEntry> Search;//��ѯ����
        public PageInfo PageInfo;//��ҳ
        //public string GroupBy;
        public string OrderBy;
        public string WhereStr;
        //public List<ColumnEntry> Columns;//������
    }
}