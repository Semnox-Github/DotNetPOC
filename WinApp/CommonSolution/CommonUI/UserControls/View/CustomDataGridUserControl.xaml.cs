/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - View Model for editable data grid
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     24-Mar-2021   Raja Uthanda            Created for POS UI Redesign 
 *2.150.0     29-Mar-2022   Uthanda Raja            Include the SelectedItemChanged event
 ********************************************************************************************/
using System.Windows;
using System.Reflection;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for GenericCustomDataGrid.xaml
    /// </summary>
    public partial class CustomDataGridUserControl : UserControl
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly RoutedEvent DataGridRowMouseUpEvent = EventManager.RegisterRoutedEvent("DataGridRowMouseUp", RoutingStrategy.Direct,
                 typeof(RoutedEventHandler), typeof(CustomDataGridUserControl));

        public static readonly RoutedEvent DataGridSortingEvent = EventManager.RegisterRoutedEvent("DataGridSorting", RoutingStrategy.Direct,
                 typeof(RoutedEventHandler), typeof(CustomDataGridUserControl));

        public static readonly RoutedEvent ComboBoxSelectionChangedEvent = EventManager.RegisterRoutedEvent("ComboBoxSelectionChanged", RoutingStrategy.Direct,
                 typeof(RoutedEventHandler), typeof(CustomDataGridUserControl));

        public static readonly RoutedEvent ButtonClickedEvent = EventManager.RegisterRoutedEvent("ButtonClicked", RoutingStrategy.Direct,
                 typeof(RoutedEventHandler), typeof(CustomDataGridUserControl));

        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(CustomDataGridUserControl));

        public static readonly RoutedEvent SelectedItemsChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemsChanged", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(CustomDataGridUserControl));

        public static readonly RoutedEvent SearchClickedEvent = EventManager.RegisterRoutedEvent("SearchClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(CustomDataGridUserControl));

        public static readonly RoutedEvent DeleteClickedEvent = EventManager.RegisterRoutedEvent("DeleteClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(CustomDataGridUserControl));

        public static readonly RoutedEvent DeleteAllClickedEvent = EventManager.RegisterRoutedEvent("DeleteAllClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(CustomDataGridUserControl));
        #endregion

        #region Properties        
        public event RoutedEventHandler SelectedItemsChanged
        {
            add
            {
                AddHandler(SelectedItemsChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectedItemsChangedEvent, value);
            }
        }
        public event RoutedEventHandler DataGridRowMouseUp
        {
            add
            {
                AddHandler(DataGridRowMouseUpEvent, value);
            }
            remove
            {
                RemoveHandler(DataGridRowMouseUpEvent, value);
            }
        }
        public event RoutedEventHandler DataGridSorting
        {
            add
            {
                AddHandler(DataGridSortingEvent, value);
            }
            remove
            {
                RemoveHandler(DataGridSortingEvent, value);
            }
        }
        public event RoutedEventHandler ComboBoxSelectionChanged
        {
            add
            {
                AddHandler(ComboBoxSelectionChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ComboBoxSelectionChangedEvent, value);
            }
        }
        public event RoutedEventHandler ButtonClicked
        {
            add
            {
                AddHandler(ButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(ButtonClickedEvent, value);
            }
        }
        public event RoutedEventHandler DeleteClicked
        {
            add
            {
                AddHandler(DeleteClickedEvent, value);
            }
            remove
            {
                RemoveHandler(DeleteClickedEvent, value);
            }
        }

        public event RoutedEventHandler DeleteAllClicked
        {
            add
            {
                AddHandler(DeleteAllClickedEvent, value);
            }
            remove
            {
                RemoveHandler(DeleteAllClickedEvent, value);
            }
        }
        public event RoutedEventHandler SearchClicked
        {
            add
            {
                AddHandler(SearchClickedEvent, value);
            }
            remove
            {
                RemoveHandler(SearchClickedEvent, value);
            }
        }

        public event RoutedEventHandler SelectionChanged
        {
            add
            {
                AddHandler(SelectionChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectionChangedEvent, value);
            }
        }
        #endregion

        #region Methods 
        internal void RaiseSelectedItemsChangedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(SelectedItemsChangedEvent);
            log.LogMethodExit();
        }
        internal void RaiseDataGridRowMouseUpEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(DataGridRowMouseUpEvent);
            log.LogMethodExit();
        }
        internal void RaiseDataGridSortingEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(DataGridSortingEvent);
            log.LogMethodExit();
        }
        internal void RaiseComboBoxSelectionChangedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(ComboBoxSelectionChangedEvent);
            log.LogMethodExit();
        }
        internal void RaiseButtonClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(ButtonClickedEvent);
            log.LogMethodExit();
        }
        internal void RaiseDeleteClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(DeleteClickedEvent);
            log.LogMethodExit();
        }
        internal void RaiseDeleteAllClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(DeleteAllClickedEvent);
            log.LogMethodExit();
        }
        internal void RaiseSearchClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(SearchClickedEvent);
            log.LogMethodExit();
        }

        internal void RaiseSelectionChangedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(SelectionChangedEvent);
            log.LogMethodExit();
        }
        private void RaiseCustomEvent(RoutedEvent routedEvent)
        {
            log.LogMethodEntry(routedEvent);
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = routedEvent;
            args.Source = this;
            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.OriginalSource is CustomComboBox)
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public CustomDataGridUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion
    }
}
