/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - TokenCardInventory UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-May-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.TransactionUI
{
    /// <summary>
    /// Interaction logic for TokenInventoryView.xaml
    /// </summary>
    public partial class TokenInventoryView : Window
    {
        private KeyboardHelper keyboardHelper;
        public TokenInventoryView()
        {
            InitializeComponent();
            keyboardHelper = new KeyboardHelper();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += TokenInventoryView_ContentRendered;
        }

        private void TokenInventoryView_ContentRendered(object sender, EventArgs e)
        {
            if (this.DataContext != null)
            {
                TokenInventoryVM tokenInventoryVM = this.DataContext as TokenInventoryVM;

                if (tokenInventoryVM != null)
                {
                    ExecutionContext executionContext = tokenInventoryVM.ExecutionContext;

                    if (executionContext != null)
                    {
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        TranslateHelper.Translate(executionContext, this);
                        if (footerView != null)
                        {
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
        }
    }
}
