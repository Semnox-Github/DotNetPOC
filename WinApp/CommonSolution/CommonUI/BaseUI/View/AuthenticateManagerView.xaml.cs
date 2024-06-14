/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Authorize Manager view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     17-Nov-2020   Amitha Joy              Created for POS UI Redesign 
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
    /// Interaction logic for AuthenticateManagerView.xaml
    /// </summary>
    public partial class AuthenticateManagerView : Window
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

        #region Constructors
        public AuthenticateManagerView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            keyboardHelper = new KeyboardHelper();
            this.SizeChanged += OnSizeChanged;
            this.ContentRendered += OnContentRendered;
            this.KeyUp += OnKeyUp;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        public void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Key == System.Windows.Input.Key.Enter && DataContext != null)
            {
                AuthenticateManagerVM authenticateManagerVM = this.DataContext as AuthenticateManagerVM;
                if (authenticateManagerVM != null)
                {
                    if (txtPassword.IsFocused)
                    {
                        authenticateManagerVM.LoginManagerCredentials(this);
                    }
                    else if (txtUserName.IsFocused)
                    {
                        txtPassword.Focus();
                    }
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

        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (DataContext != null)
            {
                AuthenticateManagerVM authenticateManagerVM = this.DataContext as AuthenticateManagerVM;
                if (authenticateManagerVM != null)
                {
                    authenticateManagerVM.AuthenticateManagerView = sender as AuthenticateManagerView;
                    ExecutionContext executionContext = authenticateManagerVM.ExecutionContext;
                    if (executionContext != null && executionContext != null)
                    {
                        TranslateHelper.Translate(executionContext, this);
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnKeyBoard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>(), showStandardKeyboard: true);
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion

    }
}
