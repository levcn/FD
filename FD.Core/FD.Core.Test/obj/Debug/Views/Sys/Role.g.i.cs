﻿#pragma checksum "E:\Levcn\FD\FD.Core\FD.Core.Test\Views\Sys\Role.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E07EA0481BED43AD7FB651E049748C7D"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34014
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using FD.Core.Test.ViewModel;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace FD.Core.Test.Views.Sys {
    
    
    public partial class Role : SLControls.Editors.BasePage {
        
        internal SLControls.Editors.BasePage aaa;
        
        internal FD.Core.Test.ViewModel.RoleTypeModel entity;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SLControls.Editor1.ListPanel c;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/FD.Core.Test;component/Views/Sys/Role.xaml", System.UriKind.Relative));
            this.aaa = ((SLControls.Editors.BasePage)(this.FindName("aaa")));
            this.entity = ((FD.Core.Test.ViewModel.RoleTypeModel)(this.FindName("entity")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.c = ((SLControls.Editor1.ListPanel)(this.FindName("c")));
        }
    }
}

