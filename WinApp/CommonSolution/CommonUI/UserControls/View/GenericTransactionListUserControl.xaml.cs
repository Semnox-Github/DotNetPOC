/********************************************************************************************
* Project Name - POS Redesign
* Description  - Common - Generic transaction list  UserControl 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
********************************************************************************************/

using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for GenericTransactionListUserControl.xaml
    /// </summary>
    public partial class GenericTransactionListUserControl : UserControl
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent("SearchClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericTransactionListUserControl));

        public static readonly RoutedEvent ResetEvent = EventManager.RegisterRoutedEvent("ResetClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericTransactionListUserControl));

        public static readonly RoutedEvent DeleteEvent = EventManager.RegisterRoutedEvent("DeleteClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericTransactionListUserControl));

        public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClickedEvent", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericTransactionListUserControl));
        #endregion

        #region Propeties
        public event RoutedEventHandler SearchClicked
        {
            add
            {
                AddHandler(SearchEvent, value);
            }
            remove
            {
                RemoveHandler(SearchEvent, value);
            }
        }

        public event RoutedEventHandler ResetClicked
        {
            add
            {
                AddHandler(ResetEvent, value);
            }
            remove
            {
                RemoveHandler(ResetEvent, value);
            }
        }

        public event RoutedEventHandler DeleteClicked
        {
            add
            {
                AddHandler(DeleteEvent, value);
            }
            remove
            {
                RemoveHandler(DeleteEvent, value);
            }
        }

        public event RoutedEventHandler ItemClicked
        {
            add
            {
                AddHandler(ItemClickedEvent, value);
            }
            remove
            {
                RemoveHandler(ItemClickedEvent, value);
            }
        }
        #endregion

        #region Methods
        internal void RaiseSearchEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(SearchEvent);
            log.LogMethodExit();
        }

        internal void RaiseDeleteEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(DeleteEvent);
            log.LogMethodExit();
        }

        internal void RaiseResetEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(ResetEvent);
            log.LogMethodExit();
        }

        internal void RaiseItemClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(ItemClickedEvent);
            log.LogMethodExit();
        }

        private void RaiseCustomEvents(RoutedEvent routedEvent)
        {
            log.LogMethodEntry(routedEvent);
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = routedEvent;
            args.Source = this;
            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public GenericTransactionListUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion

    }
}
