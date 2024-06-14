using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Semnox.Parafait.AccountsUI
{
    /// <summary>
    /// Interaction logic for TransactionCardUserControl.xaml
    /// </summary>
    public partial class TransactionCardDetailsUserControl : UserControl
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly RoutedEvent DeleteClickedEvent = EventManager.RegisterRoutedEvent("DeleteClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(TransactionCardDetailsUserControl));
        public static readonly RoutedEvent HeaderClickedEvent = EventManager.RegisterRoutedEvent("HeaderClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(TransactionCardDetailsUserControl));
        #endregion

        #region Properties
        public event RoutedEventHandler HeaderClicked
        {
            add
            {
                AddHandler(HeaderClickedEvent, value);
            }
            remove
            {
                RemoveHandler(HeaderClickedEvent, value);
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
        #endregion

        #region Methods
        internal void RaiseHeaderClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(HeaderClickedEvent);
            log.LogMethodExit();
        }
        internal void RaiseDeleteClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(DeleteClickedEvent);
            log.LogMethodExit();
        }
        private void RaiseCustomEvent(RoutedEvent routedEvent)
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = routedEvent;
            args.Source = this;
            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public TransactionCardDetailsUserControl()
        {
            InitializeComponent();
        }
        #endregion

    }
}
