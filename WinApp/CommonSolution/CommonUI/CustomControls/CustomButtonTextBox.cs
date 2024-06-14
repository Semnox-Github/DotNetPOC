using System.Windows;
using System.Reflection;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    public enum ButtonTextBoxType
    {
        Resend = 0,
        Search = 1
    }
    public class CustomButtonTextBox : TextBox
    {
        #region Members
        private Button actionButton;
        public static readonly RoutedEvent ButtonClickedEvent = EventManager.RegisterRoutedEvent("ButtonClicked", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(CustomButtonTextBox));
        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomButtonTextBox), new PropertyMetadata(Size.Medium));
        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(CustomButtonTextBox), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty DefaultValueDependencyProperty = DependencyProperty.Register("DefaultValue", typeof(string), typeof(CustomButtonTextBox), new PropertyMetadata(TranslateHelper.TranslateMessage("Enter")));
        public static readonly DependencyProperty ButtonTypeDependencyProperty = DependencyProperty.Register("ButtonType", typeof(ButtonTextBoxType), typeof(CustomButtonTextBox), new PropertyMetadata(ButtonTextBoxType.Resend));
        public static readonly DependencyProperty NonEmptyDependencyProperty = DependencyProperty.Register("NonEmpty", typeof(bool), typeof(CustomButtonTextBox), new PropertyMetadata(false));
        public static readonly DependencyProperty IsMandatoryDependencyProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(CustomButtonTextBox), new PropertyMetadata(false));
        public static readonly DependencyProperty ErrorTextVisibleDependencyProperty = DependencyProperty.Register("ErrorTextVisible", typeof(bool), typeof(CustomButtonTextBox), new PropertyMetadata(true));

        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public Button ActionButton
        {
            get
            {
                log.LogMethodEntry();
                if(actionButton == null && Template != null)
                {
                    actionButton = Template.FindName("btnAction", this) as Button;
                }
                log.LogMethodExit(actionButton);
                return actionButton;
            }
        }

        public event RoutedEventHandler ButtonClicked
        {
            add
            {
                AddHandler(ButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(ButtonClickedEvent, value);
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
        public string DefaultValue
        {
            get
            {
                return (string)GetValue(DefaultValueDependencyProperty);
            }
            set
            {
                SetValue(DefaultValueDependencyProperty, value);
            }
        }
        public ButtonTextBoxType ButtonType
        {
            get
            {
                return (ButtonTextBoxType)GetValue(SizeDependencyProperty);
            }
            set
            {
                SetValue(SizeDependencyProperty, value);
            }
        }
        public Size Size
        {
            get
            {
                return (Size)GetValue(SizeDependencyProperty);
            }
            set
            {
                SetValue(SizeDependencyProperty, value);
            }
        }
        public bool ErrorTextVisible
        {
            get
            {
                return (bool)GetValue(ErrorTextVisibleDependencyProperty);
            }
            set
            {
                SetValue(ErrorTextVisibleDependencyProperty, value);
            }
        }
        public bool NonEmpty
        {
            get
            {
                return (bool)GetValue(NonEmptyDependencyProperty);
            }
            private set
            {
                SetValue(NonEmptyDependencyProperty, value);
            }
        }
        public bool IsMandatory
        {
            get
            {
                return (bool)GetValue(IsMandatoryDependencyProperty);
            }
            set
            {
                SetValue(IsMandatoryDependencyProperty, value);
            }
        }
        #endregion

        #region Methods
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            log.LogMethodEntry(e);
            NonEmpty = !string.IsNullOrEmpty(Text) ? true : false;
            base.OnTextChanged(e);
            log.LogMethodExit();
        }
        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RaiseCustomEvent(ButtonClickedEvent);
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
        static CustomButtonTextBox()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomButtonTextBox), new FrameworkPropertyMetadata(typeof(CustomButtonTextBox)));            
            log.LogMethodExit();
        }
        public CustomButtonTextBox()
        {
            log.LogMethodEntry();
            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnButtonClicked));
            log.LogMethodExit();
        }        
        #endregion
    }
}
