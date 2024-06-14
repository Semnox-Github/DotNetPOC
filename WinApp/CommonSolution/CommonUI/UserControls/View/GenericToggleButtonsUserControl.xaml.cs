/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Generic toggle buttons UserControl 
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
    /// Interaction logic for GenericToggleButtonsUserControl.xaml
    /// </summary>
    public partial class GenericToggleButtonsUserControl : UserControl
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly RoutedEvent ToggleCheckedEvent = EventManager.RegisterRoutedEvent("ToggleChecked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericToggleButtonsUserControl));

        public static readonly RoutedEvent ToggleUncheckedEvent = EventManager.RegisterRoutedEvent("ToggleUnchecked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericToggleButtonsUserControl));
        #endregion

        #region Properties
        public event RoutedEventHandler ToggleChecked
        {
            add
            {
                AddHandler(ToggleCheckedEvent, value);
            }
            remove
            {
                RemoveHandler(ToggleCheckedEvent, value);
            }
        }

        public event RoutedEventHandler ToggleUnchecked
        {
            add
            {
                AddHandler(ToggleUncheckedEvent, value);
            }
            remove
            {
                RemoveHandler(ToggleUncheckedEvent, value);
            }
        }
        #endregion

        #region Methods
        internal void RaiseToggleCheckedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(ToggleCheckedEvent);
            log.LogMethodExit();
        }

        internal void RaiseToggleUncheckedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvents(ToggleUncheckedEvent);
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
        public GenericToggleButtonsUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion
    }
}
