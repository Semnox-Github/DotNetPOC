﻿#pragma checksum "..\..\..\..\UserControls\View\GenericDataGridUserControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "31150FEA6D25A3D94B45483CC169F9EB9A73DC5F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Semnox.Parafait.CommonUI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Semnox.Parafait.CommonUI {
    
    
    /// <summary>
    /// GenericDataGridUserControl
    /// </summary>
    public partial class GenericDataGridUserControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 191 "..\..\..\..\UserControls\View\GenericDataGridUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Semnox.Parafait.CommonUI.ComboGroupUserControl GMComboBoxGroupUserControl;
        
        #line default
        #line hidden
        
        
        #line 203 "..\..\..\..\UserControls\View\GenericDataGridUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 235 "..\..\..\..\UserControls\View\GenericDataGridUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Semnox.Parafait.CommonUI.CustomScrollViewer scvParent;
        
        #line default
        #line hidden
        
        
        #line 242 "..\..\..\..\UserControls\View\GenericDataGridUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl HeaderUniformGrid;
        
        #line default
        #line hidden
        
        
        #line 261 "..\..\..\..\UserControls\View\GenericDataGridUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Semnox.Parafait.CommonUI.CustomScrollViewer scvList;
        
        #line default
        #line hidden
        
        
        #line 273 "..\..\..\..\UserControls\View\GenericDataGridUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbContent;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CommonUI;component/usercontrols/view/genericdatagridusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UserControls\View\GenericDataGridUserControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.GMComboBoxGroupUserControl = ((Semnox.Parafait.CommonUI.ComboGroupUserControl)(target));
            return;
            case 2:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.scvParent = ((Semnox.Parafait.CommonUI.CustomScrollViewer)(target));
            return;
            case 4:
            this.HeaderUniformGrid = ((System.Windows.Controls.ItemsControl)(target));
            return;
            case 5:
            this.scvList = ((Semnox.Parafait.CommonUI.CustomScrollViewer)(target));
            return;
            case 6:
            this.lbContent = ((System.Windows.Controls.ListBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
