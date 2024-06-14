/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - custom color picker
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     27-Jul-2021   Raja Uthanda             Parafait Redesign UI
 ********************************************************************************************/
using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    public class CustomColorPicker : TextBox
    {
        #region Members        
        private string defaultErrorText = "Enter color in the format of";
        private int passingIndex = 2;

        private Window parentWindow;
        private ColorPickerView colorPickerView;        
        private ExecutionContext executionContext;
        private Regex opacityRegEx = new Regex("^#(?:[0-9a-fA-F]{3,4}){1,2}$");

        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly DependencyProperty SolidColorBrushDependencyProperty = DependencyProperty.Register("SolidColorBrush", typeof(SolidColorBrush), typeof(CustomColorPicker), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty ShowOpacityDependencyProperty = DependencyProperty.Register("ShowOpacity", typeof(bool), typeof(CustomColorPicker), new PropertyMetadata(true, OnShowOpacityChanged));

        public static readonly DependencyProperty ErrorTextVisibleDependencyProperty = DependencyProperty.Register("ErrorTextVisible", typeof(bool), typeof(CustomColorPicker), new PropertyMetadata(true));

        public static readonly DependencyProperty DefaultValueDependencyProperty = DependencyProperty.Register("DefaultValue", typeof(string), typeof(CustomColorPicker), new PropertyMetadata(TranslateHelper.TranslateMessage("Pick Color")));

        public static readonly DependencyProperty ErrorStateDependencyProperty = DependencyProperty.Register("ErrorState", typeof(bool), typeof(CustomColorPicker), new PropertyMetadata(false));

        public static readonly DependencyProperty ErrorMessageDependencyProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(CustomColorPicker), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(CustomColorPicker), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsMandatoryDependencyProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(CustomColorPicker), new PropertyMetadata(false));

        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomColorPicker), new PropertyMetadata(Size.Small));
        #endregion

        #region Properties        
        private ExecutionContext ExecutionContext
        {
            get
            {
                if(executionContext == null)
                {
                    FindParent(this);
                }
                return executionContext;
            }
        }
        public SolidColorBrush SolidColorBrush
        {
            get
            {
                return (SolidColorBrush)GetValue(SolidColorBrushDependencyProperty);
            }
            private set
            {
                SetValue(SolidColorBrushDependencyProperty, value);
            }
        }
        public bool ShowOpacity
        {
            get
            {
                return (bool)GetValue(ShowOpacityDependencyProperty);
            }
            set
            {
                SetValue(ShowOpacityDependencyProperty, value);
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
                if (value != String.Empty)
                {
                    ErrorState = true;
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
        
        #region Constructor
        static CustomColorPicker()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomColorPicker), new FrameworkPropertyMetadata(typeof(CustomColorPicker)));
            log.LogMethodExit();
        }
        public CustomColorPicker()
        {
            log.LogMethodEntry();
            this.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(OpenColorPickerWindow));
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private static void OnShowOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(d, e);
            CustomColorPicker customColorPicker = d as CustomColorPicker;
            if(customColorPicker != null)
            {
                customColorPicker.ShowOpacityChanged();
            }
            log.LogMethodExit();
        }
        private void ShowOpacityChanged()
        {
            log.LogMethodEntry();
            if(!string.IsNullOrEmpty(Text) && !CheckFormat(Text) && SolidColorBrush != null)
            {
                SetText(SolidColorBrush.Color);
            }
            log.LogMethodExit();
        }
        private string GetTranslatedMessage(string message)
        {
            log.LogMethodEntry(message);
            if (ExecutionContext != null)
            {
                message = MessageViewContainerList.GetMessage(ExecutionContext, message);
            }
            log.LogMethodExit();
            return message;
        }
        private bool ConvertStandardColor()
        {
            log.LogMethodEntry();
            bool isStandardColor = false;
            List<PropertyInfo> colorsTypes = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
            if(colorsTypes != null && colorsTypes.Count > 0)
            { 
                PropertyInfo colorsType = colorsTypes.FirstOrDefault(c => c.Name.ToLower() == Text.ToLower());
                if(colorsType != null)
                {
                    SetText((Color)colorsType.GetValue(colorsType));
                    isStandardColor = true;
                }
            }
            log.LogMethodExit();
            return isStandardColor;
        }
        private void SetText(Color color)
        {
            log.LogMethodEntry(color);
            string text = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
            if (ShowOpacity)
            {
                text = text.Insert(0, color.A.ToString() + ","); 
            }
            Text = text;
            if (!string.IsNullOrEmpty(text) && IsFocused)
            {
                CaretIndex = Text.Length;
            }
            log.LogMethodExit();
        }
        private void SetMaxLength()
        {
            log.LogMethodEntry();
            if (MaxLength <= 0)
            {
                MaxLength = ShowOpacity ? 9 : 7;
            }
            log.LogMethodExit();
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {            
            log.LogMethodEntry(e);
            SetMaxLength();
            string text = Text;
            if (!string.IsNullOrEmpty(text) && !CheckFormat(text) && !ConvertStandardColor())
            {
                byte[] byteList = GetByteValues(text);
                if (byteList.Length > 0)
                {
                    SetText(new Color() { A = byteList[0], R = byteList[1], G = byteList[2], B = byteList[3] });
                }                
            }
            Validate(Text);            
            base.OnTextChanged(e);
            log.LogMethodExit();
        }
        private bool ConvertFromHexaToInt(ref int convertValue, string hexString)
        {
            log.LogMethodEntry(convertValue, hexString);
            bool isByte = false;
            convertValue = -1;
            try
            {
                convertValue = Convert.ToInt32(hexString, 16);
            }
            catch (Exception ex)
            {
                log.Log("Converting exception", ex);
            }
            finally
            {
                if (convertValue >= 0 && convertValue <= byte.MaxValue)
                {
                    isByte = true;
                }
            }
            log.LogMethodExit();
            return isByte;
        }
        private string[] GetStringByteList(string text)
        {
            log.LogMethodEntry(text);
            log.LogMethodExit();
            return text.Replace("argb(", string.Empty).Replace("rgb(", string.Empty).Replace(")", string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private bool CheckFormat(string text)
        {
            log.LogMethodEntry(false);
            int count = GetStringByteList(text).Count();
            log.LogMethodExit();
            return (ShowOpacity && count == 4) || (!ShowOpacity && count == 3) ? true : false;            
        }
        private bool Validate(string text)
        {
            log.LogMethodEntry();
            SetMaxLength();
            bool validateString = !string.IsNullOrEmpty(text) && CheckFormat(text);
            if(validateString)
            {
                SetSolidBrush(text);   
            }
            SetErrorValues(!validateString);
            log.LogMethodExit();
            return validateString;
        }
        private void SetSolidBrush(string text)
        {
            log.LogMethodEntry(text);
            string[] stringList = GetStringByteList(text);
            if(stringList != null)
            {
                if(ShowOpacity)
                {
                    SolidColorBrush = new SolidColorBrush(new Color() { A = Convert.ToByte(stringList[0]), R = Convert.ToByte(stringList[1]), G = Convert.ToByte(stringList[2]), B = Convert.ToByte(stringList[3]) });
                }
                else
                {
                    SolidColorBrush = new SolidColorBrush(new Color() { A = 255, R = Convert.ToByte(stringList[0]), G = Convert.ToByte(stringList[1]), B = Convert.ToByte(stringList[2]) });
                }
                
            }
            log.LogMethodExit();
        }
        private void SetErrorValues(bool errorState)
        {
            log.LogMethodEntry(errorState);
            if (!IsReadOnly && IsEnabled)
            {
                ErrorMessage = errorState ? GetTranslatedMessage(defaultErrorText) + (ShowOpacity ? " 255,255,255,255" : " 255,255,255") : string.Empty;
                ErrorState = errorState;
            }
            else
            {
                ErrorMessage = string.Empty;
                ErrorState = false;
            }
            log.LogMethodExit();
        }
        private byte[] GetByteValues(string text)
        {
            log.LogMethodEntry(text);
            byte[] values = new byte[4];
            if ((ShowOpacity && text.Length == 9) || (!ShowOpacity && text.Length == 7))
            {   
                int converterdValue = 0;
                if(!ShowOpacity)
                {
                    values[0] = byte.MaxValue;                
                }
                else if(ConvertFromHexaToInt(ref converterdValue, text.Substring(1, passingIndex)))
                {
                    values[0] = (byte)converterdValue;
                }
                if (ConvertFromHexaToInt(ref converterdValue, text.Substring(ShowOpacity ? 3 : 1, passingIndex)))
                {
                    values[1] = (byte)converterdValue;
                }
                if (ConvertFromHexaToInt(ref converterdValue, text.Substring(ShowOpacity ? 5 : 3, passingIndex)))
                {
                    values[2] = (byte)converterdValue;
                }
                if (ConvertFromHexaToInt(ref converterdValue, text.Substring(ShowOpacity ? 7 : 5, passingIndex)))
                {
                    values[3] = (byte)converterdValue;
                }
            }
            log.LogMethodExit();
            return values;
        }
        private void FindParent(Visual myVisual)
        {
            log.LogMethodEntry(myVisual);
            if (executionContext == null)
            {
                if (myVisual == null)
                {
                    return;
                }
                object visual = VisualTreeHelper.GetParent(myVisual);
                if (visual is Window)
                {
                    parentWindow = visual as Window;
                    if (parentWindow.DataContext != null && parentWindow.DataContext is ViewModelBase)
                    {
                        executionContext = (parentWindow.DataContext as ViewModelBase).ExecutionContext;
                        SetErrorValues(ErrorState);
                    }
                }
                else
                {
                    FindParent(visual as Visual);
                }
            }
            log.LogMethodExit();
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            log.LogMethodEntry(e);
            e.Handled = true;
            if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                || e.Key == Key.Enter || e.Key == Key.Space)
            {
                OpenColorPickerWindow(null, null);
            }   
            base.OnPreviewKeyDown(e);
            log.LogMethodExit();
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {            
            log.LogMethodEntry(e);
            OpenColorPickerWindow(null, null);
            base.OnMouseUp(e);
            log.LogMethodExit();
        }
        private void OpenColorPickerWindow(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!IsReadOnly && IsEnabled && colorPickerView == null)
            { 
                colorPickerView = new ColorPickerView();
                if(parentWindow != null)
                {
                    colorPickerView.Owner = parentWindow;
                }
                string text = Text;
                ColorPickerVM colorPickerVM = new ColorPickerVM(ExecutionContext) { ShowOpacity = ShowOpacity };
                byte[] byteValues = this.GetByteValues(text);
                if (!string.IsNullOrEmpty(text) && SolidColorBrush != null)
                {
                    colorPickerVM.Opacity = ShowOpacity ? SolidColorBrush.Color.A : byte.MaxValue;
                    colorPickerVM.Red = SolidColorBrush.Color.R;
                    colorPickerVM.Green = SolidColorBrush.Color.G;
                    colorPickerVM.Blue = SolidColorBrush.Color.B;
                }
                colorPickerView.DataContext = colorPickerVM;
                colorPickerView.ShowDialog();
                if (colorPickerVM.ButtonClickType == ButtonClickType.Ok)
                {
                    SetText(colorPickerVM.SelectedBrush.Color);
                }
                colorPickerView = null;
            }
            log.LogMethodExit();
        }
        public override void OnApplyTemplate()
        {
            log.LogMethodEntry();
            FindParent(this);
            base.OnApplyTemplate();
            log.LogMethodExit();
        }
        #endregion

    }
}
