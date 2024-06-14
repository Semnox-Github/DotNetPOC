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

using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.RedemptionUI
{
    /// <summary>
    /// Interaction logic for RedemptionMainUserControl.xaml
    /// </summary>
    public partial class RedemptionMainUserControl : UserControl
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly RoutedEvent AddButtonClickedEvent = EventManager.RegisterRoutedEvent("AddButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(RedemptionMainUserControl));

        public static readonly RoutedEvent RemoveButtonClickedEvent = EventManager.RegisterRoutedEvent("RemoveButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(RedemptionMainUserControl));

        private KeyboardHelper keyboardHelper;
        #endregion

        #region Properties
        public event RoutedEventHandler AddButtonClicked
        {
            add
            {
                AddHandler(AddButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(AddButtonClickedEvent, value);
            }
        }

        public event RoutedEventHandler RemoveButtonClicked
        {
            add
            {
                AddHandler(RemoveButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(RemoveButtonClickedEvent, value);
            }
        }

        public KeyboardHelper KeyboardHelper
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(keyboardHelper);
                return keyboardHelper;
            }
        }
        #endregion

        #region Methods
        internal void RaiseRemoveButtonClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(RemoveButtonClickedEvent);
            log.LogMethodExit();
        }

        internal void RaiseAddButtonClickedEvent()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(AddButtonClickedEvent);
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
        public RedemptionMainUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            keyboardHelper = new KeyboardHelper();
            log.LogMethodExit();
        }
        #endregion
    }
}
