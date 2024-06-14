/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Change Ticket Mode UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     28-Mar-2021   Prashanth V            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Semnox.Parafait.TransactionUI
{
    /// <summary>
    /// Interaction logic for TaskChangeTicketModeView.xaml
    /// </summary>
    public partial class TaskChangeTicketModeView : Window
    {
        KeyboardHelper keyboardHelper;
        public TaskChangeTicketModeView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            this.ContentRendered += TaskChangeTicketModeView_ContentRendered;
        }

        private void TaskChangeTicketModeView_ContentRendered(object sender, EventArgs e)
        {
            if (this.DataContext != null)
            {
                TaskChangeTicketModeVM taskChangeTicketModeVM = this.DataContext as TaskChangeTicketModeVM;
                if (taskChangeTicketModeVM != null)
                {
                    ExecutionContext executionContext = taskChangeTicketModeVM.ExecutionContext;
                    if (executionContext != null)
                    {
                        TranslateHelper.Translate(executionContext, this);
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                            keyboardHelper = new KeyboardHelper();
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
        }
    }
}
