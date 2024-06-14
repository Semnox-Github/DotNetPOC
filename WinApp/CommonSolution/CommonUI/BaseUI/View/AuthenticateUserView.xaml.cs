/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Authorize User view
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
    /// Interaction logic for AuthenticateUserView.xaml
    /// </summary>
    public partial class AuthenticateUserView : Window
    {
        #region Members
        public static readonly RoutedEvent CloseButtonClickedEvent = EventManager.RegisterRoutedEvent("CloseButtonClicked", RoutingStrategy.Direct,
         typeof(RoutedEventHandler), typeof(AuthenticateUserView));

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

        public event RoutedEventHandler CloseButtonClicked
        {
            add
            {
                AddHandler(CloseButtonClickedEvent, value);
            }
            remove
            {
                RemoveHandler(CloseButtonClickedEvent, value);
            }
        }
        #endregion

        #region Constructors
        public AuthenticateUserView(bool isPopup = true)
        {
            log.LogMethodEntry(isPopup);
            if (!isPopup)
            {
                this.Style = Application.Current.Resources["BaseWindowDashboardStyle"] as Style;
            }
            else
            {
                this.Style = Application.Current.Resources["PopupStylewithFooter"] as Style;
            }
            InitializeComponent();
            keyboardHelper = new KeyboardHelper();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            if(isPopup)
            {
                MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight - 20;
                this.Top = 0;
                this.Left = 0;
            }
            this.SizeChanged += OnSizeChanged;
            this.KeyUp += OnKeyUp;
            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Key == System.Windows.Input.Key.Enter && DataContext != null)
            {
                AuthenticateUserVM authenticateUserVM = this.DataContext as AuthenticateUserVM;
                if (authenticateUserVM != null)
                {
                    if (txtPassword.IsFocused)
                    {
                        authenticateUserVM.LoginCredentials(this);
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
            if (MainGrid != null && DataContext != null)
            {   
                AuthenticateUserVM authenticateUserVM = DataContext as AuthenticateUserVM;
                if (authenticateUserVM != null)
                {   
                    switch (authenticateUserVM.LoginStyle)
                    {
                        case loginStyle.PopUp:
                            {
                                MainGrid.MinWidth = e.NewSize.Width / 3;
                                MainGrid.MinHeight = e.NewSize.Height / 3;
                                if (authenticateUserVM.MultiScreenMode && authenticateUserVM.IsMultiScreenRowTwo)
                                {
                                    MainGrid.MinWidth = e.NewSize.Width - 50;
                                    MainGrid.MinHeight = e.NewSize.Height - 50;
                                }
                                else if (authenticateUserVM.MultiScreenMode && authenticateUserVM.IsFingerPrintEnabled == Visibility.Collapsed)
                                {
                                    MainGrid.MinWidth = e.NewSize.Width - 150;
                                    MainGrid.MinHeight = e.NewSize.Height - 50;
                                }
                            }
                            break;
                        case loginStyle.FullScreen:
                            {
                                MainGrid.MaxWidth = e.NewSize.Width / 3;
                            }
                            break;
                    }
                }
            }
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
                AuthenticateUserVM authenticateUserVM = this.DataContext as AuthenticateUserVM;
                if (authenticateUserVM != null)
                {
                    authenticateUserVM.AuthenticateUserView = this;
                    ExecutionContext executionContext = authenticateUserVM.ExecutionContext;
                    if (authenticateUserVM.ExecutionContext != null)
                    {
                        TranslateHelper.Translate(executionContext, this);
                        
                        switch(authenticateUserVM.LoginStyle)
                        {
                            case loginStyle.FullScreen:
                                {
                                    FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                                    if (footerView != null)
                                    {
                                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>(), showStandardKeyboard: true);
                                    }
                                }
                                break;
                            case loginStyle.PopUp:
                                {
                                    keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>(), showStandardKeyboard: true);
                                }
                                break;
                        }
                        
                    }
                }
            }
            log.LogMethodExit();
        }

        internal void RaiseCloseButtonClickedEvent()
        {
            log.LogMethodEntry();
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = CloseButtonClickedEvent;
            args.Source = this;
            this.RaiseEvent(args);
            log.LogMethodExit();
        }
        #endregion
    }
}
