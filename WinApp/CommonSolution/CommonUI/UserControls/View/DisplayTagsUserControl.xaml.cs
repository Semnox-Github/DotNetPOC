/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - header section
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda           Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda           to add interactive button style
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for HeaderUserControl.xaml
    /// </summary>
    public partial class DisplayTagsUserControl : UserControl
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(DisplayTagsUserControl));
        #endregion

        #region Properties
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
        internal void RaiseItemClickedEvent()
        {
            log.LogMethodEntry();

            RaiseCustomEvent(ItemClickedEvent);

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

        #region Constructors
        public DisplayTagsUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion

    }
}
