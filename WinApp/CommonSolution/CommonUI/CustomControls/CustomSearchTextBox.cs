/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom search textbox
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    public class CustomSearchTextBox : TextBox
    {
        #region Members
        public static readonly DependencyProperty NonEmptyDependencyProperty = DependencyProperty.Register("NonEmpty", typeof(bool), typeof(CustomSearchTextBox), new PropertyMetadata(false));

        public static readonly DependencyProperty IsDefaultValueDependencyProperty = DependencyProperty.Register("IsDefaultValue", typeof(bool), typeof(CustomSearchTextBox), new PropertyMetadata(false));

        public static readonly DependencyProperty DefaultValueDependencyProperty = DependencyProperty.Register("DefaultValue", typeof(string), typeof(CustomSearchTextBox), new PropertyMetadata(TranslateHelper.TranslateMessage("Search here..."), new PropertyChangedCallback(OnDefaultTextChanged)));

        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(CustomSearchTextBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomSearchTextBox), new PropertyMetadata(Size.Small));

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool NonEmpty
        {
            get { return (bool)GetValue(NonEmptyDependencyProperty); }
            private set { SetValue(NonEmptyDependencyProperty, value); }
        }

        public bool IsDefaultValue
        {
            get { return (bool)GetValue(IsDefaultValueDependencyProperty); }
            private set { SetValue(IsDefaultValueDependencyProperty, value); }
        }

        public string DefaultValue
        {
            get { return (string)GetValue(DefaultValueDependencyProperty); }
            set { SetValue(DefaultValueDependencyProperty, value); }
        }

        public string Heading
        {
            get { return (string)GetValue(HeadingDependencyProperty); }
            set { SetValue(HeadingDependencyProperty, value); }
        }

        public Size Size
        {
            get { return (Size)GetValue(SizeDependencyProperty); }
            set { SetValue(SizeDependencyProperty, value); }
        }
        #endregion

        #region Methods        
        protected override void OnInitialized(EventArgs e)
        {
            log.LogMethodEntry();
            if (this.Text == string.Empty)
            {
                //this.Text = DefaultValue;
            }
            base.OnInitialized(e);
            log.LogMethodExit();
        }

        private static void OnDefaultTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry();
            CustomSearchTextBox textBox = d as CustomSearchTextBox;
            if (textBox != null)
            {
                textBox.ChangeDefaultText();
            }
            log.LogMethodExit();
        }

        private void ChangeDefaultText()
        {
            log.LogMethodEntry();
            if ((this.Text == string.Empty || this.Text == TranslateHelper.TranslateMessage("Search here...")) && this.Text != DefaultValue)
            {
                this.Text = DefaultValue;
            }
            log.LogMethodExit();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            log.LogMethodEntry();
            IsDefaultValue = this.Text == DefaultValue ? true : false;

            if (!IsDefaultValue && this.Text != string.Empty)
            {
                NonEmpty = true;
            }
            else
            {
                NonEmpty = false;
            }
            base.OnTextChanged(e);
            log.LogMethodExit();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            log.LogMethodEntry();
            if (this.Text == DefaultValue)
            {
                this.Text = string.Empty;
            }
            if (this.Text != string.Empty)
            {
                this.CaretIndex = this.Text.Length;
            }
            base.OnGotFocus(e);
            log.LogMethodExit();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            log.LogMethodEntry();
            if (this.Text == string.Empty)
            {
                this.Text = DefaultValue;
            }
            base.OnLostFocus(e);
            log.LogMethodExit();
        }

        private void ClearButtonClicked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry();
            this.Text = string.Empty;
            this.Focus();
            log.LogMethodExit();
        }
        #endregion

        #region Constuctor
        //static CustomSearchTextBox()
        //{
        //    log.LogMethodEntry();
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomSearchTextBox), new
        //   FrameworkPropertyMetadata(typeof(CustomSearchTextBox)));
        //    log.LogMethodExit();
        //}

        public CustomSearchTextBox()
        {
            log.LogMethodEntry();
            this.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent,
                      new RoutedEventHandler(ClearButtonClicked));
            log.LogMethodExit();
        }
        #endregion
    }
}
