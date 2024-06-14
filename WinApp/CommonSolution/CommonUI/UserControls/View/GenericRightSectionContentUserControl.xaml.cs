/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - GMGenericRightSection UserControl 
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
    /// Interaction logic for GMGenericRightSectionUserControl.xaml
    /// </summary>
    public partial class GenericRightSectionContentUserControl : UserControl
    {
        #region Members
        public static readonly RoutedEvent SearchButtonClickedEvent =
         EventManager.RegisterRoutedEvent("SearchButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDataGridUserControl));

        public static readonly RoutedEvent PreviousNavigationClickedEvent =
         EventManager.RegisterRoutedEvent("PreviousNavigationClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDataGridUserControl));
        public static readonly RoutedEvent NextNavigationClickedEvent =
         EventManager.RegisterRoutedEvent("NextNavigationClickedEvent", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDataGridUserControl));
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties

        public event RoutedEventHandler SearchButtonClicked
        {
            add { AddHandler(SearchButtonClickedEvent, value); }
            remove { RemoveHandler(SearchButtonClickedEvent, value); }
        }

        public event RoutedEventHandler PreviousNavigationClicked
        {
            add { AddHandler(PreviousNavigationClickedEvent, value); }
            remove { RemoveHandler(PreviousNavigationClickedEvent, value); }
        }

        public event RoutedEventHandler NextNavigationClicked
        {
            add { AddHandler(NextNavigationClickedEvent, value); }
            remove { RemoveHandler(NextNavigationClickedEvent, value); }
        }
        #endregion

        #region Methods
        internal void RaiseSearchButtonClickedEvent()
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = SearchButtonClickedEvent;
            args.Source = this;
            this.RaiseEvent(args);
            log.LogMethodExit();
        }

        internal void RaisePreviousNavigationClickedEvent()
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = PreviousNavigationClickedEvent;

            args.Source = this;

            this.RaiseEvent(args);
            log.LogMethodExit();
        }

        internal void RaiseNextNavigationClickedEvent()
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = NextNavigationClickedEvent;

            args.Source = this;

            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public GenericRightSectionContentUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion

    }
}
