/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - LeftPane User control
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda            for multi screen mode
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for LeftPaneUserControl.xaml
    /// </summary>
    public partial class LeftPaneUserControl : UserControl
    {
        #region Members
        private string selectedItem;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly RoutedEvent NaivigationClickEvent = EventManager.RegisterRoutedEvent("NavigationClick", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(LeftPaneUserControl));

        public static readonly RoutedEvent MenuSelectedEvent = EventManager.RegisterRoutedEvent("MenuSelected", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(LeftPaneUserControl));

        public static readonly RoutedEvent AddButtonClickedEvent = EventManager.RegisterRoutedEvent("AddButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(LeftPaneUserControl));

        public static readonly RoutedEvent RemoveButtonClickedEvent = EventManager.RegisterRoutedEvent("RemoveButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(LeftPaneUserControl));
        #endregion

        #region Properties
        public string SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
            }
        }

        public event RoutedEventHandler NavigationClick
        {
            add { AddHandler(NaivigationClickEvent, value); }
            remove { RemoveHandler(NaivigationClickEvent, value); }
        }

        public event RoutedEventHandler MenuSelected
        {
            add { AddHandler(MenuSelectedEvent, value); }
            remove { RemoveHandler(MenuSelectedEvent, value); }
        }

        public event RoutedEventHandler AddButtonClicked
        {
            add
            {
                AddHandler(AddButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(AddButtonClickedEvent, value);
            }
        }

        public event RoutedEventHandler RemoveButtonClicked
        {
            add
            {
                AddHandler(RemoveButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(RemoveButtonClickedEvent, value);
            }
        }
        #endregion

        #region Methods
        internal void RaiseRemoveButtonClickedEvent()
        {
            log.LogMethodEntry();

            RaiseCustomEvent(RemoveButtonClickedEvent);

            log.LogMethodExit();
        }

        internal void RaiseAddButtonClickedEvent()
        {
            log.LogMethodEntry();

            RaiseCustomEvent(AddButtonClickedEvent);

            log.LogMethodExit();
        }

        internal void RaiseNavigationClickEvent()
        {
            log.LogMethodEntry();

            RaiseCustomEvent(NaivigationClickEvent);

            log.LogMethodExit();
        }

        internal void RaiseMenuSelectedEvent()
        {
            log.LogMethodEntry();

            RaiseCustomEvent(MenuSelectedEvent);

            log.LogMethodExit();
        }

        private void RaiseCustomEvent(RoutedEvent routedEvent)
        {
            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = routedEvent;

            args.Source = this;

            this.RaiseEvent(args);
        }
        #endregion

        #region Constructors
        public LeftPaneUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            selectedItem = string.Empty;
            log.LogMethodExit();
        }
        #endregion

    }
}
