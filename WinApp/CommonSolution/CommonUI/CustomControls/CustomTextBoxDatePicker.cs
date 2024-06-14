/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom textbox date picker
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.130.0     07-Jul-2021   Raja Uthanda            Adding time picker  
 ********************************************************************************************/
using System;
using System.Windows;
using System.Reflection;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Controls;

using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    public enum DatePickerValidationType
    {
        None = 0,
        DateWithYear = 1,
        DateWithoutYear = 2
    }

    public class CustomTextBoxDatePicker : TextBox
    {

        #region Members
        private string errorString = "Enter date in the format";
        private string dateTimeFormat;
        private DatePickerView datePickerView;
        private ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly RoutedEvent DatePickerLoadedEvent = EventManager.RegisterRoutedEvent("DatePickerLoaded", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(CustomTextBoxDatePicker));
        public static readonly DependencyProperty EditableMonthYearDependencyProperty = DependencyProperty.Register("EditableMonthYear", typeof(bool), typeof(CustomTextBoxDatePicker), new PropertyMetadata(false));
        public static readonly DependencyProperty ShowTimePickerDependencyProperty = DependencyProperty.Register("ShowTimePicker", typeof(bool), typeof(CustomTextBoxDatePicker), new PropertyMetadata(false, OnValidationTypeChanged));
        public static readonly DependencyProperty ErrorTextVisibleDependencyProperty = DependencyProperty.Register("ErrorTextVisible", typeof(bool), typeof(CustomTextBoxDatePicker), new PropertyMetadata(true));
        public static readonly DependencyProperty NonEmptyDependencyProperty = DependencyProperty.Register("NonEmpty", typeof(bool), typeof(CustomTextBoxDatePicker), new PropertyMetadata(false));
        public static readonly DependencyProperty IsMandatoryDependencyProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(CustomTextBoxDatePicker), new PropertyMetadata(false));
        public static readonly DependencyProperty DefaultValueDependencyProperty = DependencyProperty.Register("DefaultValue", typeof(string), typeof(CustomTextBoxDatePicker), new PropertyMetadata(TranslateHelper.TranslateMessage("Select")));
        public static readonly DependencyProperty ErrorStateDependencyProperty = DependencyProperty.Register("ErrorState", typeof(bool), typeof(CustomTextBoxDatePicker), new FrameworkPropertyMetadata() { DefaultValue = false, BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        public static readonly DependencyProperty ErrorMessageDependencyProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(CustomTextBoxDatePicker), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(CustomTextBoxDatePicker), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomTextBoxDatePicker), new PropertyMetadata(Size.Small));
        public static readonly DependencyProperty ValidationTypeDependencyProperty = DependencyProperty.Register("ValidationType", typeof(DatePickerValidationType), typeof(CustomTextBoxDatePicker), new PropertyMetadata(DatePickerValidationType.None, OnValidationTypeChanged));
        public static readonly DependencyProperty DisablePastDatesDependencyProperty = DependencyProperty.Register("DisablePastDates", typeof(bool), typeof(CustomTextBoxDatePicker), new PropertyMetadata(false));
        public static readonly DependencyProperty EnableMonthSelectionDependencyProperty = DependencyProperty.Register("EnableMonthSelection", typeof(bool), typeof(CustomTextBoxDatePicker), new PropertyMetadata(true));
        public static readonly DependencyProperty EnableDaySelectionDependencyProperty = DependencyProperty.Register("EnableDaySelection", typeof(bool), typeof(CustomTextBoxDatePicker), new PropertyMetadata(true));
        public static readonly DependencyProperty DisableTillDependencyProperty = DependencyProperty.Register("DisableTill", typeof(DateTime), typeof(CustomTextBoxDatePicker), new PropertyMetadata(DateTime.MinValue));
        #endregion

        #region Properties
        public DateTime DisableTill
        {
            get { return (DateTime)GetValue(DisableTillDependencyProperty); }
            set { SetValue(DisableTillDependencyProperty, value); }
        }
        public bool EnableMonthSelection
        {
            get { return (bool)GetValue(EnableMonthSelectionDependencyProperty); }
            set { SetValue(EnableMonthSelectionDependencyProperty, value); }
        }
        public bool EnableDaySelection
        {
            get { return (bool)GetValue(EnableDaySelectionDependencyProperty); }
            set { SetValue(EnableDaySelectionDependencyProperty, value); }
        }
        public bool EditableMonthYear
        {
            get
            {
                return (bool)GetValue(EditableMonthYearDependencyProperty);
            }
            set
            {
                SetValue(EditableMonthYearDependencyProperty, value);
            }
        }
        public bool ShowTimePicker
        {
            get
            {
                return (bool)GetValue(ShowTimePickerDependencyProperty);
            }
            set
            {
                SetValue(ShowTimePickerDependencyProperty, value);
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

        public event RoutedEventHandler DatePickerLoaded
        {
            add
            {
                AddHandler(DatePickerLoadedEvent, value);
            }
            remove
            {
                RemoveHandler(DatePickerLoadedEvent, value);
            }
        }

        public DatePickerView DatePickerView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(datePickerView);
                return datePickerView;
            }
        }

        public bool DisablePastDates
        {
            get
            {
                log.LogMethodEntry();
                return (bool)GetValue(DisablePastDatesDependencyProperty);
            }
            set
            {
                log.LogMethodEntry(value);
                SetValue(DisablePastDatesDependencyProperty, value);
            }
        }

        internal bool NonEmpty
        {
            get
            {
                log.LogMethodEntry();
                return (bool)GetValue(NonEmptyDependencyProperty);
            }
            private set
            {
                log.LogMethodEntry(value);
                SetValue(NonEmptyDependencyProperty, value);
            }
        }

        public string DefaultValue
        {
            get
            {
                log.LogMethodEntry();
                return (string)GetValue(DefaultValueDependencyProperty);
            }
            set
            {
                log.LogMethodEntry(value);
                SetValue(DefaultValueDependencyProperty, value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                log.LogMethodEntry();
                return (string)GetValue(ErrorMessageDependencyProperty);
            }
            set
            {
                log.LogMethodEntry(value);
                if (!string.IsNullOrEmpty(String.Empty))
                {
                    SetErrorState(true);
                }
                SetValue(ErrorMessageDependencyProperty, value);
            }
        }

        public bool ErrorState
        {
            get
            {
                log.LogMethodEntry();
                return (bool)GetValue(ErrorStateDependencyProperty);
            }
            set
            {
                log.LogMethodEntry(value);
                SetValue(ErrorStateDependencyProperty, value);
            }
        }

        public DatePickerValidationType ValidationType
        {
            get
            {
                log.LogMethodEntry();
                return (DatePickerValidationType)GetValue(ValidationTypeDependencyProperty);
            }
            set
            {
                string error = String.Empty;
                if (this.Text != DefaultValue)
                {
                    IsValid();
                }
                log.LogMethodEntry(value);
                SetValue(ValidationTypeDependencyProperty, value);
            }
        }

        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                return (string)GetValue(HeadingDependencyProperty);
            }
            set
            {
                log.LogMethodEntry(value);
                SetValue(HeadingDependencyProperty, value);
            }
        }

        public Size Size
        {
            get
            {
                log.LogMethodEntry();
                return (Size)GetValue(SizeDependencyProperty);
            }
            set
            {
                log.LogMethodEntry(value);
                SetValue(SizeDependencyProperty, value);
            }
        }

        public bool IsMandatory
        {
            get
            {
                log.LogMethodEntry();
                return (bool)GetValue(IsMandatoryDependencyProperty);
            }
            set
            {
                log.LogMethodEntry(value);
                SetValue(IsMandatoryDependencyProperty, value);
            }
        }
        #endregion

        #region Methods
        private static void OnValidationTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(d, e);
            CustomTextBoxDatePicker customTextBoxDatePicker = d as CustomTextBoxDatePicker;
            if (customTextBoxDatePicker != null)
            {
                customTextBoxDatePicker.OnTextChanged(null);
            }
            log.LogMethodExit();
        }
        private void FindParent(Visual myVisual)
        {
            log.LogMethodEntry(myVisual);
            if (executionContext == null && string.IsNullOrEmpty(dateTimeFormat))
            {
                if (myVisual == null)
                {
                    return;
                }
                object visual = VisualTreeHelper.GetParent(myVisual);
                if (visual is Window)
                {
                    Window parent = visual as Window;
                    if (parent.DataContext != null && parent.DataContext is ViewModelBase)
                    {
                        executionContext = (parent.DataContext as ViewModelBase).ExecutionContext;
                    }
                }
                else
                {
                    FindParent(visual as Visual);
                }
            }
            log.LogMethodExit();
        }
        private void SetDateTimeFormat()
        {
            log.LogMethodEntry();
            if (executionContext != null)
            {
                dateTimeFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, ShowTimePicker ? "DATETIME_FORMAT" : "DATE_FORMAT");
                CheckDateWithoutYear();
            }
            log.LogMethodExit();
        }
        private void FormatText()
        {
            log.LogMethodEntry();
            FindParent(this);
            SetDateTimeFormat();
            if (!string.IsNullOrEmpty(dateTimeFormat) && !string.IsNullOrEmpty(this.Text) && this.Text != DefaultValue)
            {
                DateTime result;
                if (DateTime.TryParse(Text, out result))
                {
                    string formattedText = result.ToString(dateTimeFormat);
                    if (!string.IsNullOrEmpty(formattedText) && Text != formattedText)
                    {
                        if (ShowTimePicker)
                        {
                            if (dateTimeFormat.Length == Text.Length - 1 || dateTimeFormat.Length == Text.Length || dateTimeFormat.Length < Text.Length)
                            {
                                Text = formattedText;
                            }
                        }
                        else if (dateTimeFormat.Length == Text.Length - 1 || dateTimeFormat.Length < Text.Length)
                        {
                            Text = formattedText;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void SetTextInFormat()
        {
            log.LogMethodEntry();
            if (IsLoaded)
            {
                FormatText();
                NonEmpty = !string.IsNullOrEmpty(this.Text);
                if (NonEmpty)
                {
                    IsValid();
                }
            }
            log.LogMethodExit();
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender);
            SetTextInFormat();
            log.LogMethodExit();
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            log.LogMethodEntry();
            SetTextInFormat();
            if (e != null)
            {
                base.OnTextChanged(e);
            }
            log.LogMethodExit();
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            log.LogMethodEntry(e);
            if (this.Text != string.Empty)
            {
                this.CaretIndex = this.Text.Length;
            }
            base.OnGotFocus(e);
            log.LogMethodExit();
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            log.LogMethodEntry(e);
            if (ErrorState && string.IsNullOrEmpty(Text))
            {
                SetErrorValues(false, string.Empty);
            }
            base.OnLostFocus(e);
            log.LogMethodExit();
        }
        private void SetDateFormatWithoutYear()
        {
            log.LogMethodEntry();
            int index = dateTimeFormat.ToLower().IndexOf('y') - 1;
            int lastIndex = dateTimeFormat.ToLower().LastIndexOf('y') + 1;
            if (index > 0 && lastIndex >= index)
            {
                dateTimeFormat = dateTimeFormat.Remove(index, lastIndex - index);
            }
            log.LogMethodExit();
        }
        private void OnDatePickerClicked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(e);
            if (!IsReadOnly && IsEnabled)
            {
                datePickerView = new DatePickerView();
                DatePickerVM datePickerVM = new DatePickerVM();
                datePickerVM.ExecutionContext = executionContext;
                datePickerVM.DisableTill = DisableTill;
                datePickerVM.EnableDaySelection = EnableDaySelection;
                datePickerVM.EnableMonthSelection = EnableMonthSelection;
                datePickerVM.EnableYearSelection = ValidationType == DatePickerValidationType.DateWithoutYear ? false : true;
                datePickerVM.ShowTimePicker = ShowTimePicker;
                datePickerVM.EditableMonthYear = EditableMonthYear;
                if (!string.IsNullOrEmpty(Text))
                {
                    string decimalValue = "D2";
                    DateTime result;
                    if (DateTime.TryParse(Text, out result) && result != DateTime.MinValue)
                    {
                        datePickerVM.SelectedYear = result.Year;
                        datePickerVM.SelectedMonth = datePickerVM.Months[result.Month - 1];
                        datePickerVM.SelectedDate = result.Day;
                        if (ShowTimePicker)
                        {
                            datePickerVM.SelectedHour = result.Hour > 12 ? (result.Hour - 12).ToString(decimalValue) : result.Hour.ToString(decimalValue);
                            datePickerVM.SelectedMinute = result.Minute.ToString(decimalValue);
                            datePickerVM.AMorPM = result.ToString("tt", CultureInfo.InvariantCulture).ToLower() == "am" ? AMorPM.AM : AMorPM.PM;
                        }
                    }
                }
                datePickerView.DataContext = datePickerVM;
                datePickerView.Closed += DatePickerViewClosed;
                datePickerView.Show();

                RoutedEventArgs args = new RoutedEventArgs();
                args.RoutedEvent = DatePickerLoadedEvent;
                args.Source = this;
                this.RaiseEvent(args);
            }
            log.LogMethodExit();
        }
        private void DatePickerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (datePickerView != null && !string.IsNullOrEmpty(datePickerView.SelectedDate))
            {
                DateTime dDate;
                if (DateTime.TryParse(datePickerView.SelectedDate, out dDate))
                {
                    FindParent(this);
                    if (!string.IsNullOrEmpty(dateTimeFormat))
                    {
                        CheckDateWithoutYear();
                        Text = dDate.ToString(dateTimeFormat);
                    }
                    else
                    {
                        string normalFormat = string.Empty;
                        switch (ValidationType)
                        {
                            case DatePickerValidationType.DateWithoutYear:
                                {
                                    normalFormat = ShowTimePicker ? "{0:dd-MMM hh:mm tt}" : "{0:dd-MMM}";
                                }
                                break;
                            case DatePickerValidationType.DateWithYear:
                                {
                                    normalFormat = ShowTimePicker ? "{0:dd-MMM-yyyy hh:mm tt}" : "{0:dd-MMM-yyyy}";
                                }
                                break;
                        }
                        Text = string.IsNullOrEmpty(normalFormat) ? datePickerView.SelectedDate : String.Format(normalFormat, dDate);
                    }
                }
                datePickerView = null;
            }
            log.LogMethodExit();
        }
        private void CheckDateWithoutYear()
        {
            log.LogMethodEntry();
            switch (this.ValidationType)
            {
                case DatePickerValidationType.DateWithoutYear:
                    {
                        SetDateFormatWithoutYear();
                    }
                    break;
            }
            log.LogMethodExit();
        }
        private void IsValid()
        {
            log.LogMethodEntry();
            DateTime dDate;
            bool errorState = false;
            string errorMessage = string.Empty;
            string message = executionContext != null ? MessageViewContainerList.GetMessage(executionContext, errorString, null) + " " : errorString + " ";
            if (!string.IsNullOrEmpty(dateTimeFormat))
            {
                CheckDateWithoutYear();
                if (!DateTime.TryParseExact(this.Text, new[] { dateTimeFormat }, null, DateTimeStyles.None, out dDate))
                {
                    errorState = true;
                    errorMessage = message + " " + dateTimeFormat;
                }
            }
            SetErrorValues(errorState, errorMessage);
            log.LogMethodExit();
        }
        private void SetErrorValues(bool errorState, string errorMessage)
        {
            log.LogMethodEntry(errorState, errorMessage);
            SetErrorState(errorState);
            if (ErrorMessage != errorMessage)
            {
                ErrorMessage = errorMessage;
            }
            log.LogMethodExit();
        }
        private void SetErrorState(bool errorState)
        {
            log.LogMethodEntry(errorState);
            if (ErrorState != errorState)
            {
                PropertyInfo propertyInfo = GetPropertyInfo(CustomTextBoxDatePicker.ErrorStateDependencyProperty);
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    propertyInfo.SetValue(this.GetBindingExpression(CustomTextBoxDatePicker.ErrorStateDependencyProperty).DataItem, errorState);
                }
                else
                {
                    ErrorState = errorState;
                }
            }
            log.LogMethodExit();
        }
        private PropertyInfo GetPropertyInfo(DependencyProperty dependencyProperty)
        {
            log.LogMethodEntry();
            PropertyInfo propertyInfo = null;
            BindingExpression bindingExpression = this.GetBindingExpression(dependencyProperty);
            if (bindingExpression != null)
            {
                string[] splits = bindingExpression.ParentBinding.Path.Path.Split('.');
                Type type = bindingExpression.DataItem.GetType();
                foreach (string split in splits)
                {
                    propertyInfo = type.GetProperty(split);
                    if (propertyInfo != null)
                    {
                        type = propertyInfo.PropertyType;
                    }
                }
            }
            log.LogMethodExit();
            return propertyInfo;
        }
        #endregion

        #region Constructor
        static CustomTextBoxDatePicker()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomTextBoxDatePicker), new FrameworkPropertyMetadata(typeof(CustomTextBoxDatePicker)));
            log.LogMethodExit();
        }

        public CustomTextBoxDatePicker()
        {
            log.LogMethodEntry();
            this.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(OnDatePickerClicked));
            this.Loaded += OnLoaded;
            log.LogMethodExit();
        }
        #endregion

    }
}
