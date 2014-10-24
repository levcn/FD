using System;
using System.Collections.ObjectModel;
using System.Linq;


namespace STComponse.Ex
{
    public static class ObservableCollectionEx
    {
        public static void ForEach<T>(this ObservableCollection<T> list,Action<T> action)
        {
            list.ToList().ForEach(action);
        }
    }
}
