/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - GenericGMDataGrid UserControl  
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
    /// Interaction logic for GenericGMDataGridUserControl.xaml
    /// </summary>
    public partial class GenericDataGridUserControl : UserControl
    {

        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly RoutedEvent SearchClickedEvent =
         EventManager.RegisterRoutedEvent("SearchClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDataGridUserControl));
        public static readonly RoutedEvent IsSelectedEvent =
         EventManager.RegisterRoutedEvent("IsSelected", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDataGridUserControl));
        #endregion

        #region Properties
        public event RoutedEventHandler SearchClicked
        {
            add { AddHandler(SearchClickedEvent, value); }
            remove { RemoveHandler(SearchClickedEvent, value); }
        }

        public event RoutedEventHandler IsSelected
        {
            add { AddHandler(IsSelectedEvent, value); }
            remove { RemoveHandler(IsSelectedEvent, value); }
        }
        #endregion

        #region Methods
        internal void RaiseSearchClickedEvent()
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = SearchClickedEvent;

            args.Source = this;

            this.RaiseEvent(args);
            log.LogMethodExit();
        }

        internal void RaiseIsSelectionChangedEvent()
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = IsSelectedEvent;

            args.Source = this;

            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public GenericDataGridUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion
    }
}
