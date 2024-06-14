/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Pause Time UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     18-May-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    /// <summary>
    /// Interaction logic for TaskPauseTimeView.xaml
    /// </summary>
    public partial class TaskPauseTimeView : Window
    {
        KeyboardHelper keyBoardHelper;
        public TaskPauseTimeView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += TaskPauseTimeView_ContentRendered;
        }

        private void TaskPauseTimeView_ContentRendered(object sender, EventArgs e)
        {
            if (this.DataContext != null)
            {
                TaskPauseTimeVM taskPauseTimeVM = this.DataContext as TaskPauseTimeVM;
                if (taskPauseTimeVM != null)
                {
                    ExecutionContext executionContext = taskPauseTimeVM.ExecutionContext;
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
