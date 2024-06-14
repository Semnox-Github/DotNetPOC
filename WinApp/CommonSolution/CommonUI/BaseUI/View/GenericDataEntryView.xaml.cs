/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - GenericDataEntryView
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda            Modified to add numeric up down
 ********************************************************************************************/

using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using System.Windows.Input;
using System.Windows.Media;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for GenericDataEntryView.xaml
    /// </summary>
    public partial class GenericDataEntryView : Window
    {
        #region Members
        public static readonly RoutedEvent NumericDeleteButtonClickedEvent = EventManager.RegisterRoutedEvent("NumericDeleteButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDataEntryView));
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        #endregion

        #region Properties
        public KeyboardHelper KeyBoardHelper
        {
            get
            {
                return keyboardHelper;
            }
        }

        public event RoutedEventHandler NumericDeleteButtonClicked
        {
            add
            {
                AddHandler(NumericDeleteButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(NumericDeleteButtonClickedEvent, value);
            }
        }
        #endregion

        #region Constructor
        public GenericDataEntryView()
        {
            log.LogMethodEntry();
            InitializeComponent();

            keyboardHelper = new KeyboardHelper();

            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight - 72;
            MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight - 20;

            this.ContentRendered += OnContentRendered;

            log.LogMethodExit();
        }
        #endregion

        #region Methods
        internal void RaiseCustomEvent()
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = NumericDeleteButtonClickedEvent;
            args.Source = this;
            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        public static Control FindVisualChild(DependencyObject depObj)
        {
            log.LogMethodEntry(depObj);
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is CustomTextBox)
                    {
                        return (Control)child;
                    }
                    Control childItem = FindVisualChild(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }
            log.LogMethodExit();
            return null;
        }
        private void OnContentRendered(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnKeyBoard }, ParafaitDefaultViewContainerList.GetParafaitDefault(TranslateHelper.executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
            if (ContentItemsControl != null && DataContext != null)
            {
                GenericDataEntryVM dataEntryVM = DataContext as GenericDataEntryVM;
                if (dataEntryVM != null && dataEntryVM.DataEntryCollections != null && dataEntryVM.DataEntryCollections.Count > 0)
                {
                    CustomTextBox textbox = FindVisualChild(ContentItemsControl) as CustomTextBox;
                    if (textbox != null && dataEntryVM.DataEntryCollections.IndexOf(textbox.DataContext as DataEntryElement) == 0)
                    {
                        FocusManager.SetFocusedElement(ContentItemsControl, textbox);
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
