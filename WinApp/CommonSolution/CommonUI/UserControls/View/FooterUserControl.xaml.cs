/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Footer UserControl   
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda            Handle multi screen mode
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;


namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for FooterUserControl.xaml
    /// </summary>
    public partial class FooterUserControl : UserControl
    {
        #region Members
        public static readonly RoutedEvent SidebarClickedEvent = EventManager.RegisterRoutedEvent("SidebarClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(FooterUserControl));

        public static readonly RoutedEvent MessageClickedEvent = EventManager.RegisterRoutedEvent("MessageClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(FooterUserControl));

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public event RoutedEventHandler SidebarClicked
        {
            add { AddHandler(SidebarClickedEvent, value); }
            remove { RemoveHandler(SidebarClickedEvent, value); }
        }

        public event RoutedEventHandler MessageClicked
        {
            add { AddHandler(MessageClickedEvent, value); }
            remove { RemoveHandler(MessageClickedEvent, value); }
        }
        #endregion

        #region Methods
        internal void RaiseSidebarClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(SidebarClickedEvent);
            log.LogMethodExit();
        }

        internal void RaiseMessageClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(MessageClickedEvent);
            log.LogMethodExit();
        }

        internal void RaiseCustomEvent(RoutedEvent routedEvent)
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = routedEvent;
            args.Source = this;
            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public FooterUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion


    }
}
