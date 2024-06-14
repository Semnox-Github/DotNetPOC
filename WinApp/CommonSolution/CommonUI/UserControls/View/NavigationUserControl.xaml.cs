/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common -navigation user control
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
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

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for NavigationUserControl.xaml
    /// </summary>
    public partial class NavigationUserControl : UserControl
    {

        #region Members
        public static readonly RoutedEvent NavigationTagClickedEvent = EventManager.RegisterRoutedEvent("NavigationTagClicked", RoutingStrategy.Direct,
                 typeof(RoutedEventHandler), typeof(NavigationUserControl));

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public event RoutedEventHandler NavigationTagClicked
        {
            add
            {
                AddHandler(NavigationTagClickedEvent, value);
            }
            remove
            {
                RemoveHandler(NavigationTagClickedEvent, value);
            }
        }
        #endregion

        #region Methods
        private void RaiseCustomEvent(RoutedEvent routedEvent)
        {
            log.LogMethodEntry(routedEvent);
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = routedEvent;
            args.Source = this;
            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        internal void RaiseNavigationTagClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(NavigationTagClickedEvent);
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public NavigationUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion
    }
}
