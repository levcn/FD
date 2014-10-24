using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFControls.Ex
{
    public static class ObservableCollectionEx
    {
        public static void ForEach<T>(this ObservableCollection<T> list,Action<T> action)
        {
            list.ToList().ForEach(action);
        }
    }
}
