/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - header tags UserControl 
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


namespace Semnox.Parafait.RedemptionUI
{
    /// <summary>
    /// Interaction logic for RedemptionHeaderTagsUserControl.xaml
    /// </summary>
    public partial class RedemptionHeaderTagsUserControl : UserControl
    {

        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly RoutedEvent HeaderTagClickedEvent = EventManager.RegisterRoutedEvent("HeaderTagClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(RedemptionHeaderTagsUserControl));
        #endregion

        #region Properties
        public event RoutedEventHandler HeaderTagClicked
        {
            add
            {
                AddHandler(HeaderTagClickedEvent, value);
            }
            remove
            {
                RemoveHandler(HeaderTagClickedEvent, value);
            }
        }
        #endregion

        #region Methods
        internal void RaiseHeaderTagClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(HeaderTagClickedEvent);
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

        #region Constructor
        public RedemptionHeaderTagsUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion

    }
}
