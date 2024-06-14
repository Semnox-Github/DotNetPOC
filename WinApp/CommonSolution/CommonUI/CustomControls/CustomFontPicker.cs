/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - custom font picker
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     27-Jul-2021   Raja Uthanda             Parafait Redesign UI
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Semnox.Parafait.CommonUI
{
    public class CustomFontPicker : TextBox
    {
        #region Members        
        private string defaultErrorText = "Please enter value in correct format.";

        private ExecutionContext executionContext;
        private FontPickerView fontPickerView;
        private Window parentWindow;

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly DependencyProperty FontValueObjectDependencyProperty = DependencyProperty.Register("FontValueObject", typeof(FontValueObject), typeof(CustomFontPicker), new PropertyMetadata(null));

        public static readonly DependencyProperty ErrorTextVisibleDependencyProperty = DependencyProperty.Register("ErrorTextVisible", typeof(bool), typeof(CustomFontPicker), new PropertyMetadata(true));

        public static readonly DependencyProperty DefaultValueDependencyProperty = DependencyProperty.Register("DefaultValue", typeof(string), typeof(CustomFontPicker), new PropertyMetadata(TranslateHelper.TranslateMessage("Pick Font")/*, new PropertyChangedCallback(OnDefaultTextChanged)*/));

        public static readonly DependencyProperty ErrorStateDependencyProperty = DependencyProperty.Register("ErrorState", typeof(bool), typeof(CustomFontPicker), new PropertyMetadata(false));

        public static readonly DependencyProperty ErrorMessageDependencyProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(CustomFontPicker), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(CustomFontPicker), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsMandatoryDependencyProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(CustomFontPicker), new PropertyMetadata(false));

        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomFontPicker), new PropertyMetadata(Size.Small));
        #endregion

        #region Properties        
        private ExecutionContext ExecutionContext
        {
            get
            {
                if (executionContext == null)
                {
                    FindParent(this);
                }
                return executionContext;
            }
        }
        public FontValueObject FontValueObject
        {
            get
            {
                return (FontValueObject)GetValue(FontValueObjectDependencyProperty);
            }
            private set
            {
                SetValue(FontValueObjectDependencyProperty, value);
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
        static CustomFontPicker()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomFontPicker), new FrameworkPropertyMetadata(typeof(CustomFontPicker)));
            log.LogMethodExit();
        }

        public CustomFontPicker()
        {
            log.LogMethodEntry();
            this.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(OnFontPickerClicked));
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private string GetTranslatedMessage(string message)
        {
            log.LogMethodEntry();
            if (ExecutionContext != null)
            {
                message = MessageViewContainerList.GetMessage(ExecutionContext, defaultErrorText);
            }
            log.LogMethodExit();
            return message;
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            log.LogMethodEntry(e);
            if(!string.IsNullOrEmpty(Text))
            {
                bool validText = false;
                string message = string.Empty;
                try
                {
                    FontValueObject = new FontValueObject(Text);
                    if(FontValueObject != null)
                    {
                        validText = true;//FontValueObject.ValidateFont(Text);
                    }
                }
                catch(Exception ex)
                {
                    log.Info(ex.Message);
                }
                finally
                {
                    SetErrorValues(!validText);
                }
            }
            base.OnTextChanged(e);
            log.LogMethodExit();
        }
        
        private void SetErrorValues(bool errorState)
        {
            log.LogMethodEntry();
            if (!IsReadOnly && IsEnabled)
            {
                ErrorState = errorState;
                ErrorMessage = errorState ? GetTranslatedMessage(defaultErrorText) : string.Empty;
            }
            else
            {
                ErrorState = false;
                ErrorMessage = string.Empty;
            }
            log.LogMethodExit();
        }
        private bool Validate()
        {
            log.LogMethodEntry();
          
            log.LogMethodExit();
            return true;
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
            if((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                OnFontPickerClicked(null, null);
            }
            base.OnPreviewKeyDown(e);
            log.LogMethodExit();
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            log.LogMethodEntry(e);
            OnFontPickerClicked(null, null);
            base.OnMouseUp(e);
            log.LogMethodExit();
        }
        private void OnFontPickerClicked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(!IsReadOnly && IsEnabled && fontPickerView == null)
            {                 
                fontPickerView = new FontPickerView();
                if(parentWindow != null)
                { 
                    fontPickerView.Owner = parentWindow;
                }
                string text = Text;
                FontPickerVM fontPickerVM = null;
                if (!string.IsNullOrEmpty(text) && Validate() && FontValueObject != null)
                {
                    FontPickerVM fontPicker = new FontPickerVM(ExecutionContext);
                    int size = Convert.ToInt32(fontPicker.SizeCollection.OrderBy(v => Math.Abs((long)Convert.ToInt32(v) - FontValueObject.FontSize)).First());
                    FamilyTypeface fontStyle = FontValueObject.FontFamily.FamilyTypefaces.ToList().Where(f => f.Weight == FontValueObject.FontWeight && f.Style == FontValueObject.FontStyle).FirstOrDefault();
                    if(fontStyle == null)
                    {
                        fontStyle = FontValueObject.FontFamily.FamilyTypefaces[0];
                    }
                    if(size <= 0)
                    {
                        size = 12;
                    }
                    fontPickerVM = new FontPickerVM(ExecutionContext, null, null, FontValueObject.FontFamily, fontStyle, size);
                }
                if(fontPickerVM == null)
                {
                    fontPickerVM = new FontPickerVM(ExecutionContext);
                }
                fontPickerView.DataContext = fontPickerVM;
                fontPickerView.ShowDialog();
                if (fontPickerVM.ButtonClickType == ButtonClickType.Ok)
                {
                    try
                    {
                        FontValueObject = new FontValueObject(fontPickerVM.FontFamily, (decimal)fontPickerVM.FontSize, fontPickerVM.FontStyle.Style, fontPickerVM.FontStyle.Weight);
                    }
                    catch(Exception ex)
                    {
                        log.Info(ex);
                    }
                    finally
                    {
                        if(FontValueObject != null)
                        {
                            Text = FontValueObject.ToString();
                            if (!string.IsNullOrEmpty(Text))
                            {
                                CaretIndex = Text.Length;
                            }
                        }
                    }
                }
                fontPickerView = null;
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
