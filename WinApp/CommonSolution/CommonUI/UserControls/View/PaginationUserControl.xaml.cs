using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for PaginationUserControl.xaml
    /// </summary>
    public partial class PaginationUserControl : UserControl
    {
        #region Members
        public static readonly RoutedEvent PaginationActionsClickedEvent = EventManager.RegisterRoutedEvent("PaginationActionsClicked", RoutingStrategy.Direct, 
            typeof(RoutedEventHandler), typeof(PaginationUserControl));
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public event RoutedEventHandler PaginationActionsClicked
        {
            add
            {
                AddHandler(PaginationActionsClickedEvent, value);
            }
            remove
            {
                RemoveHandler(PaginationActionsClickedEvent, value);
            }
        }
        #endregion

        #region Constructor        
        public PaginationUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        internal void RaiseSelectionChangedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(PaginationActionsClickedEvent);
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
        #endregion
    }
}
