/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Generic display items UserControl 
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
    /// Interaction logic for GenericDisplayItemsUserControl.xaml
    /// </summary>
    public partial class GenericDisplayItemsUserControl : UserControl
    {
        #region Members
        public static readonly RoutedEvent ScrollChangedEvent = EventManager.RegisterRoutedEvent("ScrollChanged", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDisplayItemsUserControl));

        public static readonly RoutedEvent ContentRenderedEvent = EventManager.RegisterRoutedEvent("ContentRendered", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDisplayItemsUserControl));

        public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDisplayItemsUserControl));

        public static readonly RoutedEvent OfferOrInfoClickedEvent = EventManager.RegisterRoutedEvent("OfferOrInfoClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDisplayItemsUserControl));

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties

        public event RoutedEventHandler ScrollChanged
        {
            add
            {
                AddHandler(ScrollChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ScrollChangedEvent, value);
            }
        }
        public event RoutedEventHandler ContentRendered
        {
            add
            {
                AddHandler(ContentRenderedEvent, value);
            }
            remove
            {
                RemoveHandler(ContentRenderedEvent, value);
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

        public event RoutedEventHandler OfferOrInfoClicked
        {
            add
            {
                AddHandler(OfferOrInfoClickedEvent, value);
            }
            remove
            {
                RemoveHandler(OfferOrInfoClickedEvent, value);
            }
        }
        #endregion

        #region Methods
        internal void RaiseItemClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(ItemClickedEvent);
            log.LogMethodExit();
        }

        internal void RaiseScrollChangedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(ScrollChangedEvent);
            log.LogMethodExit();
        }
        internal void RaiseContentRenderedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(ContentRenderedEvent);
            log.LogMethodExit();
        }
        internal void RaiseOfferOrInfoClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(OfferOrInfoClickedEvent);
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

        #region Constructors
        public GenericDisplayItemsUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion
    }
}
