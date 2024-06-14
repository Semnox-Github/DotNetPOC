/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redeem Loyalty UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0      23-July-2021  Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    /// <summary>
    /// Interaction logic for TaskRedeemLoyaltyView.xaml
    /// </summary>
    public partial class TaskRedeemLoyaltyView : Window
    {
        public TaskRedeemLoyaltyView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += TaskRedeemLoyaltyView_ContentRendered;
        }

        private void TaskRedeemLoyaltyView_ContentRendered(object sender, EventArgs e)
        {
            if (this.DataContext != null)
            {
                TaskRedeemLoyaltyVM taskRedeemLoyaltyVM = this.DataContext as TaskRedeemLoyaltyVM;
                if (taskRedeemLoyaltyVM != null)
                {
                    ExecutionContext executionContext = taskRedeemLoyaltyVM.ExecutionContext;
                    if (executionContext != null)
                    {
                        TranslateHelper.Translate(executionContext, this);
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                            KeyboardHelper keyboardHelper = new KeyboardHelper();
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
        }
    }
}
