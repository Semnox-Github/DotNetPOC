/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Card Details - view model for Card details user control
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     24-Mar-2021   Raja Uthanda            Created for POS UI Redesign Tasks
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;


namespace Semnox.Parafait.AccountsUI
{
    /// <summary>
    /// Interaction logic for CardDetailsUserControl.xaml
    /// </summary>
    public partial class CardDetailsUserControl : UserControl
    {

        #region Members
        public static readonly RoutedEvent CardAddedEvent = EventManager.RegisterRoutedEvent("CardAdded", RoutingStrategy.Direct,
typeof(RoutedEventHandler), typeof(CardDetailsUserControl));
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public event RoutedEventHandler CardAdded
        {
            add
            {
                AddHandler(CardAddedEvent, value);
            }
            remove
            {
                RemoveHandler(CardAddedEvent, value);
            }
        }
        #endregion
        #region Methods
        internal void RaiseCardAddedEvent()
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = CardAddedEvent;
            args.Source = this;
            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        #endregion
        #region Constructors
        public CardDetailsUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion
    }
}
