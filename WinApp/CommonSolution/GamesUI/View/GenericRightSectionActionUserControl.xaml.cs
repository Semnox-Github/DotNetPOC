/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Game Management - right section of main screen
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy              Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.GamesUI
{
    /// <summary>
    /// Interaction logic for GenericRightSectionActionUserControl.xaml
    /// </summary>
    public partial class GenericRightSectionActionUserControl : UserControl
    {
        #region Members
        public static readonly RoutedEvent EditClickedEvent =
         EventManager.RegisterRoutedEvent("EditButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericRightSectionActionUserControl));
        public static readonly RoutedEvent LastClickedEvent =
         EventManager.RegisterRoutedEvent("LastButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericRightSectionActionUserControl));
        public static readonly RoutedEvent ButtonGroupClickedEvent =
         EventManager.RegisterRoutedEvent("ButtonGroupClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericRightSectionActionUserControl));
        #endregion

        #region Properties
        public event RoutedEventHandler EditButtonClicked
        {
            add { AddHandler(EditClickedEvent, value); }
            remove { RemoveHandler(EditClickedEvent, value); }
        }

        public event RoutedEventHandler LastButtonClicked
        {
            add { AddHandler(LastClickedEvent, value); }
            remove { RemoveHandler(LastClickedEvent, value); }
        }

        public event RoutedEventHandler ButtonGroupClicked
        {
            add { AddHandler(ButtonGroupClickedEvent, value); }
            remove { RemoveHandler(ButtonGroupClickedEvent, value); }
        }
        #endregion

        #region Methods
        internal void RaiseEditClickedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = EditClickedEvent;

            args.Source = this;

            this.RaiseEvent(args);
        }

        internal void RaiseLastClickedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = LastClickedEvent;

            args.Source = this;

            this.RaiseEvent(args);
        }

        internal void RaiseButtonGroupClickedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs();

            args.RoutedEvent = ButtonGroupClickedEvent;

            args.Source = this;

            this.RaiseEvent(args);
        }
        #endregion

        #region Constructors
        public GenericRightSectionActionUserControl()
        {
            InitializeComponent();
        }
        #endregion
    }
}
