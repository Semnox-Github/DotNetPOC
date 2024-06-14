/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Load Ticket View
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-July-2021   Prashanth            Created for POS UI Redesign 
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
    /// Interaction logic for TaskLoadTicketView.xaml
    /// </summary>
    public partial class TaskLoadTicketView : Window
    {
        KeyboardHelper keyBoardHelper;
        public TaskLoadTicketView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += TaskLoadTicketView_ContentRendered;
        }

        private void TaskLoadTicketView_ContentRendered(object sender, EventArgs e)
        {
            if (this.DataContext != null)
            {
                TaskLoadTicketVM taskLoadTicketVM = this.DataContext as TaskLoadTicketVM;
                if (taskLoadTicketVM != null)
                {
                    ExecutionContext executionContext = taskLoadTicketVM.ExecutionContext;
                    if (executionContext != null)
                    {
                        TranslateHelper.Translate(executionContext, this);
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                            keyBoardHelper = new KeyboardHelper();
                            keyBoardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
        }
    }
}
