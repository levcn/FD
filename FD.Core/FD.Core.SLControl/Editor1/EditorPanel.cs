using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FD.Core.SLControl.Data;
using SLControls.DataClientTools;
using SLControls.Extends;
using StaffTrain.FwClass.DataClientTools;


namespace SLControls.Editor1
{
    public class EditorPanel : ContentControl
    {
        public EditorPanel()
        {
            this.DefaultStyleKey = typeof(EditorPanel);
            DataContextChanged += EditorPanel_DataContextChanged;
            Loaded += EditorPanel_Loaded;
            Unloaded += EditorPanel_Unloaded;
        }

        void EditorPanel_Loaded(object sender, RoutedEventArgs e)
        {
            BaseListEntity baseListEntity = DataContext as BaseListEntity;
            if (baseListEntity!=null) InitValidEvent(baseListEntity);
        }

        void EditorPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            BaseListEntity baseListEntity = DataContext as BaseListEntity;
            if (baseListEntity != null) UnRegisterValidEvent(baseListEntity);
            
        }

        /// <summary>
        /// 绑定数据源事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EditorPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BaseListEntity entity = e.NewValue as BaseListEntity;
            if (entity != null)
            {
                InitValidEvent(entity);
            }
            BaseListEntity baseListEntity = e.OldValue as BaseListEntity;
            if (baseListEntity != null) UnRegisterValidEvent(baseListEntity);
        }

        private void UnRegisterValidEvent(BaseListEntity entity)
        {
            entity.OnValidate -= entity_OnValidate;
        }
        private void InitValidEvent(BaseListEntity entity)
        {
            UnRegisterValidEvent(entity);
            entity.OnValidate += entity_OnValidate;
        }

        async void entity_OnValidate(object sender, CustomValidArgs args)
        {
            if (args.ValidType == 7)
            {
                var list = await ActionHelper.Select(args.Obj.GetType(), new List<SearchEntry> {
                    new SearchEntry {
                        ColumnName = args.PropName,
                        Flag = " = ",
                        value = args.PropValue.ToString().Include("'"),
                    },
                    new SearchEntry {
                        ColumnName = "ID",
                        Flag = " <> ",
                        value = args.Obj.GetType().GetProperty("ID").GetValue(args.Obj,null).ToString().Include("'"),
                    },
                }, null, 1, 2, null, null, null);
                var plr = list as PagerListResult;
                if (plr != null && plr.ListObjectData.Count == 0)
                {
                    args.Obj.ClearError(args.PropName);
                    //                    args.Obj.OnPropertySelectedChanged(args.PropName);
                    args.Obj.OnErrorsChanged(new DataErrorsChangedEventArgs(args.PropName));
                }
                else
                {
                    args.Obj.SetErrors(args.PropName,"已经存在");
                    args.Obj.OnErrorsChanged(new DataErrorsChangedEventArgs(args.PropName));
                }
            }
        }
    }
}
