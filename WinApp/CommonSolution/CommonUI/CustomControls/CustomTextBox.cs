/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom textbox 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Semnox.Parafait.CommonUI
{
    public enum Size
    {
        XSmall = 0,
        Small = 1,
        Medium = 2,
        Large = 3,
        DataGridSize = 4
    }

    public enum ValidationType
    {
        None = 0,
        AlphabetsOnly = 1,
        Alphanumeric = 2,
        NumberOnly = 3,
        ZipCode = 4,
        Login = 5,
        DecimalOnly = 6
    }
    public class CustomTextBox : TextBox
    {
        #region Members        
        public static readonly DependencyProperty ErrorTextVisibleDependencyProperty = DependencyProperty.Register("ErrorTextVisible", typeof(bool), typeof(CustomTextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty NonEmptyDependencyProperty = DependencyProperty.Register("NonEmpty", typeof(bool), typeof(CustomTextBox), new PropertyMetadata(false));
        public static readonly DependencyProperty IsMandatoryDependencyProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(CustomTextBox), new PropertyMetadata(false));
        public static readonly DependencyProperty DefaultValueDependencyProperty = DependencyProperty.Register("DefaultValue", typeof(string), typeof(CustomTextBox), new PropertyMetadata(TranslateHelper.TranslateMessage("Enter")));
        public static readonly DependencyProperty ErrorStateDependencyProperty = DependencyProperty.Register("ErrorState", typeof(bool), typeof(CustomTextBox), new FrameworkPropertyMetadata() { DefaultValue = false, BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        public static readonly DependencyProperty ErrorMessageDependencyProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(CustomTextBox), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(CustomTextBox), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomTextBox), new PropertyMetadata(Size.Medium));
        public static readonly DependencyProperty ValidationTypeDependencyProperty = DependencyProperty.Register("ValidationType", typeof(ValidationType), typeof(CustomTextBox), new PropertyMetadata(ValidationType.None));
        public static readonly DependencyProperty IsMaskedDependencyProperty = DependencyProperty.Register("IsMasked", typeof(bool), typeof(CustomTextBox), new PropertyMetadata(false, OnIsMaskedChanged));
        public static readonly DependencyProperty NumberKeyboardTypeDependencyProperty = DependencyProperty.Register("NumberKeyboardType", typeof(NumberKeyboardType), typeof(CustomTextBox), new PropertyMetadata(NumberKeyboardType.Positive));
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties 
        public bool IsIntDataType
        {
            get
            {
                Type type = GetDataType(CustomTextBox.TextProperty);
                if (type != null && (type == typeof(int) || type == typeof(int?)))
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsByteDataType
        {
            get
            {
                Type type = GetDataType(CustomTextBox.TextProperty);
                if (type != null && type == typeof(byte) || type == typeof(byte?))
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsDecimalDataType
        {
            get
            {
                Type type = GetDataType(CustomTextBox.TextProperty);
                if (type != null && (type == typeof(float) || type == typeof(float?) || type == typeof(decimal) || type == typeof(decimal?)
                    || type == typeof(double) || type == typeof(double?)))
                {
                    return true;
                }
                return false;
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
        public bool ErrorState
        {
            get
            {
                return (bool)GetValue(ErrorStateDependencyProperty);
            }
            set
            {
                SetValue(ErrorStateDependencyProperty, value);
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
        public bool IsMasked
        {
            get
            {
                return (bool)GetValue(IsMaskedDependencyProperty);
            }
            set
            {
                SetValue(IsMaskedDependencyProperty, value);
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
        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageDependencyProperty); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    SetErrorState(true);
                }
                SetValue(ErrorMessageDependencyProperty, value);
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
        public ValidationType ValidationType
        {
            get
            {
                return (ValidationType)GetValue(ValidationTypeDependencyProperty);
            }
            set
            {
                string error = String.Empty;
                bool errorState = !CustomTextBoxValidationHelper.IsValid(this.Text, value, out error);
                SetErrorState(errorState);
                SetValue(ValidationTypeDependencyProperty, value);
            }
        }
        #endregion

        #region Methods   
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            log.LogMethodEntry(e);
            ApplyTemplate();
            if (IsByteDataType && !((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Back
                || e.Key == Key.Delete || e.Key == Key.NumLock || e.Key == Key.Left || e.Key == Key.Right)))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
            base.OnPreviewKeyDown(e);
        }        
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            log.LogMethodEntry(e);
            BindingExpression bindingExpression = this.GetBindingExpression(CustomTextBox.TextProperty);
            if (bindingExpression != null && string.IsNullOrEmpty(Text) && (IsIntDataType || IsDecimalDataType || IsByteDataType))
            {
                Text = "0";
                if (Text.Length > 0)
                {
                    CaretIndex = Text.Length;
                }
            }
            SetNonEmpty();
            if (!string.IsNullOrEmpty(Text))
            {
                bool errorState = false;
                string errorMessage = string.Empty;
                if(ValidationType != ValidationType.None)
                {
                    errorState = !CustomTextBoxValidationHelper.IsValid(Text, ValidationType, out errorMessage);                    
                }
                SetErrorValues(errorState, errorMessage);
            }
            base.OnTextChanged(e);
            log.LogMethodExit();
        }        
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            log.LogMethodEntry(e);
            if (!string.IsNullOrEmpty(Text))
            {
                CaretIndex = Text.Length;
            }
            base.OnGotFocus(e);
            log.LogMethodExit();
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            log.LogMethodEntry(e);
            if (ValidationType == ValidationType.None && ErrorState && !string.IsNullOrEmpty(Text))
            {
                SetErrorValues(false, string.Empty);
            }
            else if (ErrorState)
            {
                SetErrorValues(false, string.Empty);
                Text = string.Empty;
            }
            SetNonEmpty();
            base.OnLostFocus(e);
            log.LogMethodExit();
        }
        private static void OnIsMaskedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(d,e);
            CustomTextBox textBox = d as CustomTextBox;
            if (textBox != null)
            {
                textBox.IsMaskedChanged();
            }
            log.LogMethodExit();
        }
        private void IsMaskedChanged()
        {
            log.LogMethodEntry();
            FontFamily = IsMasked ? new FontFamily(AppDomain.CurrentDomain.BaseDirectory + @"Styles\Resources\#password") : Application.Current.Resources["FontFamily"] as FontFamily;
            log.LogMethodExit();
        }
        private void SetNonEmpty()
        {
            log.LogMethodEntry();
            NonEmpty = !string.IsNullOrEmpty(Text) ? true : false;
            log.LogMethodExit();
        }
        private Type GetDataType(DependencyProperty dependencyProperty)
        {
            log.LogMethodEntry(dependencyProperty);
            Type type = null;
            PropertyInfo propertyInfo = GetPropertyInfo(dependencyProperty);
            if (propertyInfo != null)
            {
                type = propertyInfo.PropertyType;
            }
            log.LogMethodExit();
            return type;
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
                PropertyInfo propertyInfo = GetPropertyInfo(CustomTextBox.ErrorStateDependencyProperty);
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    propertyInfo.SetValue(this.GetBindingExpression(CustomTextBox.ErrorStateDependencyProperty).DataItem, errorState);
                }
                else
                {
                    ErrorState = errorState;
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constuctor
        static CustomTextBox()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomTextBox), new FrameworkPropertyMetadata(typeof(CustomTextBox)));
            log.LogMethodExit();
        }
        #endregion

    }
}
