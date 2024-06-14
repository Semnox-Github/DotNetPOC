/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - ComboBoxGroup UserControl  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;


namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for ButtonGroupUserControl.xaml
    /// </summary>
    public partial class ButtonGroupUserControl : UserControl
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly RoutedEvent ButtonGroupClickedEvent =
         EventManager.RegisterRoutedEvent("ButtonGroupClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(ButtonGroupUserControl));
        #endregion

        #region Properties
        public event RoutedEventHandler ButtonGroupClicked
        {
            add { AddHandler(ButtonGroupClickedEvent, value); }
            remove { RemoveHandler(ButtonGroupClickedEvent, value); }
        }
        #endregion

        #region Methods
        internal void RaiseButtonGroupClickedEvent()
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = ButtonGroupClickedEvent;

            args.Source = this;

            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public ButtonGroupUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion
    }
}
