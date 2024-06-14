/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - TaskLoadBonus View
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     10-May-2021    Fiona           Created for POS UI Redesign 
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
    /// Interaction logic for TaskLoadBonusView.xaml
    /// </summary>
    public partial class TaskLoadBonusView : Window
    {
        private KeyboardHelper keyboardHelper;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public TaskLoadBonusView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            this.ContentRendered += TaskLoadBonusView_ContentRendered;
            log.LogMethodExit();
        }
        private void TaskLoadBonusView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.DataContext != null)
            {
                TaskLoadBonusVM taskLoadBonusVM = this.DataContext as TaskLoadBonusVM;
                if (taskLoadBonusVM != null)
                {
                    ExecutionContext executionContext = taskLoadBonusVM.ExecutionContext;
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
            log.LogMethodEntry();
        }
    }
}
