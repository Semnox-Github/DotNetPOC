/********************************************************************************************
* Project Name - POS Redesign
* Description  - Common - Custom numeric up down
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     25-Nov-2020   Siba Maharana            Created for POS UI Redesign
*2.140.0     23-Jun-2021   Prashanth                Modified : Added StepSize Property, Modified NumericIncreaseValue and NumericDecreaseValue methods
********************************************************************************************/

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    public enum NumericButtonType
    {
        None = 0,
        Reset = 1,
        Text = 2,
        Delete = 3,
        TextBlock = 4
    }

    [TemplatePart(Name = "txtCustomNumericResultBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "btnCustomNumericDown", Type = typeof(Button))]
    [TemplatePart(Name = "btnCustomNumericUp", Type = typeof(Button))]

    public class CustomNumericUpDown : Control
    {

        #region Members
        private Button btnNumericDown;
        private Button btnNumericUp;
        private TextBox txtResult;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region DependencyProperties
        public static readonly RoutedEvent DeleteButtonClickedEvent = EventManager.RegisterRoutedEvent("DeleteButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(CustomNumericUpDown));

        public static readonly RoutedEvent AddButtonClickedEvent = EventManager.RegisterRoutedEvent("AddButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(CustomNumericUpDown));

        public static readonly DependencyProperty RemainingBalanceDependencyProperty = DependencyProperty.Register("RemainingBalance", typeof(string), typeof(CustomNumericUpDown), new PropertyMetadata("0"));

        public static readonly DependencyProperty TextBlockHeadingDependencyProperty = DependencyProperty.Register("TextBlockHeading", typeof(string), typeof(CustomNumericUpDown), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(CustomNumericUpDown), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomNumericUpDown), new PropertyMetadata(Size.Small));

        public static readonly DependencyProperty ValueDependencyProperty = DependencyProperty.Register("Value", typeof(int), typeof(CustomNumericUpDown), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public static readonly DependencyProperty StepSizeDependencyProperty = DependencyProperty.Register("StepSize", typeof(int), typeof(CustomNumericUpDown), new PropertyMetadata(1));

        public static readonly DependencyProperty MultiplyValueDependencyProperty = DependencyProperty.Register("MultiplyValue", typeof(int), typeof(CustomNumericUpDown), new PropertyMetadata(0, OnMultiplyValueChanged));

        public static readonly DependencyProperty MaxValueDependencyProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(CustomNumericUpDown), new PropertyMetadata(int.MaxValue, OnMaxValueChanged));

        public static readonly DependencyProperty MinValueDependencyProperty = DependencyProperty.Register("MinValue", typeof(int), typeof(CustomNumericUpDown), new PropertyMetadata(0, OnMinValueChanged));

        public static readonly DependencyProperty IsMaxSizeDisplayDependencyProperty = DependencyProperty.Register("IsMaxSizeDisplay", typeof(bool), typeof(CustomNumericUpDown), new PropertyMetadata(false));

        public static readonly DependencyProperty NumericDependencyButtonTypeProperty = DependencyProperty.Register("NumericButtonType", typeof(NumericButtonType), typeof(CustomNumericUpDown), new PropertyMetadata(NumericButtonType.None));

        public static readonly DependencyProperty ButtonTextDependencyProperty = DependencyProperty.Register("ButtonText", typeof(string), typeof(CustomNumericUpDown), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty NumberKeyboardTypeDependencyProperty = DependencyProperty.Register("NumberKeyboardType", typeof(NumberKeyboardType), typeof(CustomNumericUpDown), new PropertyMetadata(NumberKeyboardType.Positive, OnNumberKeyboardTypeChanged));

        private readonly RoutedUICommand numericDeleteValueCommand = new RoutedUICommand("NumericDeleteClicked", "NumericDeleteClicked", typeof(CustomNumericUpDown));

        private readonly RoutedUICommand numericAddValueCommand = new RoutedUICommand("NumericAddClicked", "NumericAddClicked", typeof(CustomNumericUpDown));

        private readonly RoutedUICommand numericDecreaseValueCommand = new RoutedUICommand("NumericDecreaseValue", "NumericDecreaseValue", typeof(CustomNumericUpDown));

        private readonly RoutedUICommand numericIncreaseValueCommand = new RoutedUICommand("NumericIncreaseValue", "NumericIncreaseValue", typeof(CustomNumericUpDown));

        private readonly RoutedUICommand updateValueCommand = new RoutedUICommand("UpdateValue", "UpdateValue", typeof(CustomNumericUpDown));

        #endregion DependencyProperties

        #region Properties
        public Button DecreaseButton
        {
            get
            {
                return (Button)this.Template.FindName("btnCustomNumericDown", this);
            }
        }

        public Button IncreaseButton
        {
            get
            {
                return (Button)this.Template.FindName("btnCustomNumericUp", this); ;
            }
        }

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

        public event RoutedEventHandler DeleteButtonClicked
        {
            add
            {
                AddHandler(DeleteButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(DeleteButtonClickedEvent, value);
            }
        }

        public TextBox TextBox
        {
            get
            {
                return txtResult;
            }
        }

        public string RemainingBalance
        {
            get
            {
                return (string)GetValue(RemainingBalanceDependencyProperty);
            }
            private set
            {
                SetValue(RemainingBalanceDependencyProperty, value);
            }
        }

        public string TextBlockHeading
        {
            get
            {
                return (string)GetValue(TextBlockHeadingDependencyProperty);
            }
            set
            {
                SetValue(TextBlockHeadingDependencyProperty, value);
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

        public NumberKeyboardType NumberKeyboardType
        {
            get
            {
                return (NumberKeyboardType)GetValue(NumberKeyboardTypeDependencyProperty);
            }
            set
            {
                SetValue(NumberKeyboardTypeDependencyProperty, value);
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

        public int Value
        {
            get
            {
                return (int)GetValue(ValueDependencyProperty);
            }
            set
            {
                SetValue(ValueDependencyProperty, value);
            }
        }

        public int StepSize
        {
            get
            {
                return (int)GetValue(StepSizeDependencyProperty);
            }
            set
            {
                SetValue(StepSizeDependencyProperty, value);
            }
        }

        public int MultiplyValue
        {
            get
            {
                return (int)GetValue(MultiplyValueDependencyProperty);
            }
            set
            {
                SetValue(MultiplyValueDependencyProperty, value);
            }
        }

        public int MaxValue
        {
            get
            {
                return (int)GetValue(MaxValueDependencyProperty);
            }
            set
            {
                SetValue(MaxValueDependencyProperty, value);
            }
        }

        public int MinValue
        {
            get
            {
                return (int)GetValue(MinValueDependencyProperty);
            }
            set
            {
                SetValue(MinValueDependencyProperty, value);
            }
        }

        public bool IsMaxSizeDisplay
        {
            get
            {
                return (bool)GetValue(IsMaxSizeDisplayDependencyProperty);
            }
            set
            {
                SetValue(IsMaxSizeDisplayDependencyProperty, value);
            }
        }

        public NumericButtonType NumericButtonType
        {
            get
            {
                return (NumericButtonType)GetValue(NumericDependencyButtonTypeProperty);
            }
            set
            {
                SetValue(NumericDependencyButtonTypeProperty, value);
            }
        }

        public string ButtonText
        {
            get
            {
                return (string)GetValue(ButtonTextDependencyProperty);
            }
            set
            {
                SetValue(ButtonTextDependencyProperty, value);
            }
        }

        #endregion Properties

        #region Methods
        private void Commands()
        {
            log.LogMethodEntry();
            CommandBindings.Add(new CommandBinding(numericIncreaseValueCommand, (a, b) => NumericIncreaseValue()));
            CommandBindings.Add(new CommandBinding(numericDecreaseValueCommand, (a, b) => NumericDecreaseValue()));
            CommandBindings.Add(new CommandBinding(updateValueCommand, (a, b) => UpdateValue()));
            CommandBindings.Add(new CommandBinding(numericDeleteValueCommand, (a, b) => NumericDeleteClicked()));
            CommandBindings.Add(new CommandBinding(numericAddValueCommand, (a, b) => NumericAddClicked()));
            CommandManager.RegisterClassInputBinding(typeof(TextBox),
                                                     new KeyBinding(updateValueCommand, new KeyGesture(Key.Enter)));
            log.LogMethodExit();
        }

        private void NumericDeleteClicked()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(DeleteButtonClickedEvent);
            log.LogMethodExit();
        }

        private void NumericDeleteButton()
        {
            log.LogMethodEntry();
            Button btnDelete = GetTemplateChild("btnDelete") as Button;
            if (btnDelete != null)
            {
                btnDelete.Command = numericDeleteValueCommand;
                btnDelete.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
            }
            log.LogMethodExit();
        }

        private void NumericAddClicked()
        {
            log.LogMethodEntry();
            RaiseCustomEvent(AddButtonClickedEvent);
            log.LogMethodExit();
        }

        private void NumericAddButton()
        {
            log.LogMethodEntry();
            Button btnAdd = GetTemplateChild("btnAdd") as Button;
            if (btnAdd != null)
            {
                btnAdd.Command = numericAddValueCommand;
                btnAdd.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
            }
            log.LogMethodExit();
        }
        private static void OnNumberKeyboardTypeChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(element, e);
            CustomNumericUpDown control = (CustomNumericUpDown)element;
            if (control != null)
            {
                NumberKeyboardType numberKeyboardType = (NumberKeyboardType)e.NewValue;
                if ((numberKeyboardType == NumberKeyboardType.Both || numberKeyboardType == NumberKeyboardType.Negative)
                    && control.MinValue == 0)
                {
                    control.MinValue = int.MinValue;
                }
            }
            log.LogMethodExit();
        }
        private static void OnValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(element, e);
            CustomNumericUpDown control = (CustomNumericUpDown)element;
            if (control != null && control.txtResult != null)
            {
                int value = control.Value;
                ValueToBounds(ref value, control.MinValue, control.MaxValue);
                control.txtResult.Text = value.ToString();
                control.Value = value;
                control.SetMutliplyValue();
            }
            log.LogMethodExit();
        }

        private static void OnMaxValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(element, e);
            CustomNumericUpDown control = (CustomNumericUpDown)element;
            int maxValue = (int)e.NewValue;

            if (maxValue < control.MinValue)
            {
                control.MinValue = maxValue;
            }
            if (maxValue <= control.Value)
            {
                control.Value = maxValue;
            }
            log.LogMethodExit();
        }

        private static void OnMultiplyValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(element, e);
            CustomNumericUpDown numericUpDown = (CustomNumericUpDown)element;
            if (numericUpDown != null)
            {
                numericUpDown.SetMutliplyValue();
            }
            log.LogMethodExit();
        }

        private static void OnMinValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(element, e);
            CustomNumericUpDown control = (CustomNumericUpDown)element;
            int minValue = (int)e.NewValue;
            if (minValue > control.MaxValue)
            {
                control.MaxValue = minValue;
            }
            if (minValue >= control.Value)
            {
                control.Value = minValue;
            }
            log.LogMethodExit();
        }

        public override void OnApplyTemplate()
        {
            log.LogMethodEntry();
            if (Template != null)
            {
                Border border = this.Template.FindName("CustomNumericTextBoxOuterBorder", this) as Border;
                if (border != null)
                {
                    border.PreviewMouseDown += OnBorderMouseDown;
                    border.PreviewMouseMove += OnBorderMouseMove;
                }
            }
            base.OnApplyTemplate();
            AttachToVisualTree();
            Commands();
            log.LogMethodExit();
        }

        private void OnBorderMouseMove(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            (sender as Border).Cursor = Cursors.IBeam;
            log.LogMethodExit();
        }

        private void OnBorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (TextBox != null)
            {
                this.TextBox.Focus();
                Keyboard.Focus(this.TextBox);
            }
            log.LogMethodExit();
        }

        private void AttachToVisualTree()
        {
            log.LogMethodEntry();
            NumericTextBox();
            NumericIncreaseButton();
            NumericDecreaseButton();
            NumericDeleteButton();
            NumericAddButton();
            log.LogMethodExit();
        }

        private void NumericTextBox()
        {
            log.LogMethodEntry();
            TextBox textBox = GetTemplateChild("txtCustomNumericResultBox") as TextBox;
            if (textBox != null)
            {
                txtResult = textBox;
                txtResult.Text = Value.ToString();
                txtResult.TextChanged += OnTextChanged;
                txtResult.PreviewTextInput += NumericOnly;
                txtResult.LostFocus += OntxtResultLostFocus;
            }
            log.LogMethodExit();
        }

        private void NumericIncreaseButton()
        {
            log.LogMethodEntry();
            Button btnCountup = GetTemplateChild("btnCustomNumericUp") as Button;
            if (btnCountup != null)
            {
                btnNumericUp = btnCountup;
                btnNumericUp.Focusable = false;
                btnNumericUp.Command = numericIncreaseValueCommand;
                btnNumericUp.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
            }
            log.LogMethodExit();
        }

        private void NumericDecreaseButton()
        {
            log.LogMethodEntry();
            Button btnCountDown = GetTemplateChild("btnCustomNumericDown") as Button;
            if (btnCountDown != null)
            {
                btnNumericDown = btnCountDown;
                btnNumericDown.Focusable = false;
                btnNumericDown.Command = numericDecreaseValueCommand;
            }
            log.LogMethodExit();
        }

        private static void ValueToBounds(ref int value, int minValue, int maxValue)
        {
            log.LogMethodEntry();
            if (value < minValue)
            {
                value = minValue;
            }
            else if (value > maxValue)
            {
                value = maxValue;
            }
            log.LogMethodEntry();
        }

        private void UpdateValue()
        {
            log.LogMethodEntry();
            Value = ParseStringToInt(txtResult.Text);
            log.LogMethodExit();
        }

        private void RemoveFocus()
        {
            log.LogMethodEntry();
            Focusable = true;
            Focus();
            Focusable = false;
            log.LogMethodExit();
        }

        private int ParseStringToInt(string source)
        {
            log.LogMethodEntry(source);
            int value;
            int.TryParse(source, out value);
            log.LogMethodExit();
            return value;
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

        private void NumericIncreaseValue()
        {
            log.LogMethodEntry();
            int value = ParseStringToInt(txtResult.Text);
            if (StepSize == 1)
            {
                if (value < MaxValue)
                {
                    value++;
                }
            }
            else if (StepSize > 1)
            {
                if (value <= (MaxValue - StepSize))
                {
                    value = value + StepSize;
                }
            }

            Value = value;
            SetMutliplyValue();
            log.LogMethodExit();
        }

        private void NumericDecreaseValue()
        {
            log.LogMethodEntry();
            int value = ParseStringToInt(txtResult.Text);
            if (StepSize == 1)
            {
                if (value > MinValue)
                {
                    value--;
                }
                if (value <= 0 && NumberKeyboardType == NumberKeyboardType.Positive)
                {
                    value = 0;
                }
            }
            else if (StepSize > 1)
            {
                if (value >= MinValue + StepSize)
                {
                    value = value - StepSize;
                }
                if (value <= 0 && NumberKeyboardType == NumberKeyboardType.Positive)
                {
                    value = 0;
                }
            }

            Value = value;
            SetMutliplyValue();
            log.LogMethodExit();
        }

        private void SetMutliplyValue()
        {
            log.LogMethodEntry();
            if (NumericButtonType == NumericButtonType.TextBlock)
            {
                RemainingBalance = (Value * MultiplyValue).ToString();
            }
            log.LogMethodExit();
        }
        #endregion Methods

        #region Events

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            TextBox textBox = sender as TextBox;
            try
            {
                int data = int.Parse(textBox.Text.ToString());
                if (data <= MaxValue)
                {
                    textBox.Text = data.ToString();
                    Value = data;
                }
                else
                {
                    textBox.Text = Value.ToString();
                }
            }
            catch
            {
                textBox.Text = "0";
                Value = 0;
            }
            if (NumericButtonType == NumericButtonType.TextBlock && MaxValue > 0 && txtResult != null
                && !string.IsNullOrEmpty(txtResult.Text))
            {
                int balance = MaxValue - int.Parse(txtResult.Text);
                if (balance >= 0)
                {
                    RemainingBalance = balance.ToString();
                }
            }
            SetMutliplyValue();
            log.LogMethodExit();
        }

        private void OntxtResultLostFocus(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Value = ParseStringToInt(txtResult.Text);
            log.LogMethodExit();
        }
        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Handled = IsTextNumeric(e.Text);
            log.LogMethodExit();
        }
        private static bool IsTextNumeric(string str)
        {
            log.LogMethodEntry(str);
            Regex reg = new Regex("[^0-9]+");
            log.LogMethodExit();
            return reg.IsMatch(str);
        }

        #endregion Events

        #region Constructors

        static CustomNumericUpDown()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomNumericUpDown), new FrameworkPropertyMetadata(typeof(CustomNumericUpDown)));
            log.LogMethodExit();
        }
        #endregion

    }
}
