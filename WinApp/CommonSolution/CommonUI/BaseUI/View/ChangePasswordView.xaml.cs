/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Change password view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy          Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;


namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for ChangePasswordView.xaml
    /// </summary>
    public partial class ChangePasswordView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        #endregion

        #region Properties
        public KeyboardHelper KeyboardHelper
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(keyboardHelper);
                return keyboardHelper;
            }
        }
        #endregion

        #region Constructor
        public ChangePasswordView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            keyboardHelper = new KeyboardHelper();
            this.KeyUp += OnKeyUp;
            this.ContentRendered += OnContentRendered;
            this.SizeChanged += OnSizeChanged;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Key == System.Windows.Input.Key.Enter && DataContext != null)
            {
                ChangePasswordVM changePasswordVM = this.DataContext as ChangePasswordVM;
                if (changePasswordVM != null)
                {
                    changePasswordVM.ChangePassword(this);
                }
            }
            else if (e.Key == System.Windows.Input.Key.Tab)
            {
                if (txtPassword.IsFocused)
                {
                    txtPassword.SelectAll();
                }
                else if (txtUserName.IsFocused)
                {
                    txtUserName.SelectAll();
                }
                else if (txtNewPassword.IsFocused)
                {
                    txtNewPassword.SelectAll();
                }
                else if (txtReEnterPassword.IsFocused)
                {
                    txtReEnterPassword.SelectAll();
                }
            }
            log.LogMethodExit();
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (DataContext != null)
            {
                ChangePasswordVM changePasswordVM = this.DataContext as ChangePasswordVM;
                if (changePasswordVM != null)
                {
                    ExecutionContext executionContext = changePasswordVM.GetExecutionContext();
                    if (executionContext != null)
                    {
                        TranslateHelper.Translate(executionContext, this);
                        FooterUserControl footerView = this.MainGrid.FindName("FooterUserControl") as FooterUserControl;
                        if (footerView != null)
                        {
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>(), showStandardKeyboard: true);
                        }
                        else if (this.btnKeyBoard != null)
                        {
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnKeyBoard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>(), showStandardKeyboard: true);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.OwnedWindows != null)
            {
                foreach (Window window in OwnedWindows)
                {
                    window.Width = e.NewSize.Width;
                    window.Height = e.NewSize.Height;
                    window.Top = this.Top;
                    window.Left = this.Left;
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
