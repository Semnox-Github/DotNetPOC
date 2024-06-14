/********************************************************************************************
* Project Name - POS Redesign
* Description  - Common - user control for generic display items
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
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Semnox.Parafait.CommonUI
{
    public enum ButtonType
    {
        Offer = 0,
        Info = 1,
        None
    }

    public class GenericDisplayItemControl : Control
    {
        #region Members
        public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDisplayItemControl));

        public static readonly RoutedEvent OfferOrInfoClickedEvent = EventManager.RegisterRoutedEvent("OfferOrInfoClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(GenericDisplayItemControl));

        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(GenericDisplayItemControl), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemsSourceDependencyProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<DiscountItem>), typeof(GenericDisplayItemControl), new PropertyMetadata(new ObservableCollection<DiscountItem>()));

        public static readonly DependencyProperty ButtonTypeDependencyProperty = DependencyProperty.Register("ButtonType", typeof(ButtonType), typeof(GenericDisplayItemControl), new PropertyMetadata(ButtonType.None));

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        public event RoutedEventHandler OfferOrInfoClicked
        {
            add
            {
                AddHandler(OfferOrInfoClickedEvent, value);
            }
            remove
            {
                RemoveHandler(OfferOrInfoClickedEvent, value);
            }
        }

        public string Heading
        {
            get
            {
                return (string)GetValue(HeadingDependencyProperty);
            }
            set
            {
                SetValue(HeadingDependencyProperty, value);
            }
        }

        public ObservableCollection<DiscountItem> ItemsSource
        {
            get
            {
                return (ObservableCollection<DiscountItem>)GetValue(ItemsSourceDependencyProperty);
            }
            set
            {
                SetValue(ItemsSourceDependencyProperty, value);
            }
        }

        public ButtonType ButtonType
        {
            get
            {
                return (ButtonType)GetValue(ButtonTypeDependencyProperty);
            }
            set
            {
                SetValue(ButtonTypeDependencyProperty, value);
            }
        }
        #endregion

        #region Methods       
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
        public GenericDisplayItemControl()
        {
            log.LogMethodEntry();

            this.AddHandler(MouseDownEvent,
                new RoutedEventHandler(OnItemClicked));

            this.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent,
            new RoutedEventHandler(OnButtonClicked));

            log.LogMethodExit();
        }

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RaiseCustomEvents(OfferOrInfoClickedEvent);
            log.LogMethodExit();
        }

        private void OnItemClicked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RaiseCustomEvents(ItemClickedEvent);
            log.LogMethodExit();
        }
        #endregion
    }
}
