using System;
using System.Collections;
using System.Collections.Generic;
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


namespace SLControls.DataClientTools
{
    public class PagerListResult
    {
        public PagerListResult( IList listObjectData,PageInfo pageInfo)
        {
            PageInfo = pageInfo;
            ListObjectData = listObjectData;
        }

        public PageInfo PageInfo { get; set; }
        public IList ListObjectData { get; set; }
    }
    public class PagerListResult<T> : PagerListResult
    {
        public PagerListResult(List<T> listData, PageInfo pageInfo)
            : base(listData,pageInfo)
        {
            ListData = listData;
            PageInfo = pageInfo;
        }

        public List<T> ListData { get; set; }
        
    }
}
