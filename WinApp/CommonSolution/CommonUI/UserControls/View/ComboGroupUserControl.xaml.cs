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
    /// Interaction logic for ComboBoxGroupUserControl.xaml
    /// </summary>
    public partial class ComboGroupUserControl : UserControl
    {
        #region Members
        private ComboBoxField selectedComboBoxField;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly RoutedEvent IsSelectionChangedEvent =
         EventManager.RegisterRoutedEvent("IsSelectionChanged", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(ComboGroupUserControl));
        #endregion

        #region Properties
        internal ComboBoxField SelectedComboBoxField
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedComboBoxField);
                return selectedComboBoxField;
            }
            set
            {
                log.LogMethodEntry(value);
                selectedComboBoxField = value;
            }
        }

        public event RoutedEventHandler IsSelectionChanged
        {
            add { AddHandler(IsSelectionChangedEvent, value); }
            remove { RemoveHandler(IsSelectionChangedEvent, value); }
        }
        #endregion

        #region Methods
        internal void RaiseSelectionChangedEvent(ComboBoxField comboBoxField)
        {
            log.LogMethodEntry();
            this.SelectedComboBoxField = comboBoxField;

            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = IsSelectionChangedEvent;

            args.Source = this;

            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public ComboGroupUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion
    }
}
