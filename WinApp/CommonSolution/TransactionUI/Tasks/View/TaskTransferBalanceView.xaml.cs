/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Transfer Balance
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     30-Mar-2021    Raja Uthanda           Created for POS UI Redesign 
 *2.150.0     17-Dec-2021   Uthanda Raja            Modified for remove the TranslateHelper.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    /// <summary>
    /// Interaction logic for TaskTransferBalanceView.xaml
    /// </summary>
    public partial class TaskTransferBalanceView : Window
    {
        #region Members
        private KeyboardHelper keyboardHelper;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructors
        public TaskTransferBalanceView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            keyboardHelper = new KeyboardHelper();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        internal void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                TaskTransferBalanceVM transferBalanceVM = DataContext as TaskTransferBalanceVM;
                if (transferBalanceVM != null && transferBalanceVM.ExecutionContext != null)
                {
                    FooterUserControl footerControl = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                    if (footerControl != null)
                    {
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerControl.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(
                            transferBalanceVM.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                    }
                    TranslateHelper.Translate(transferBalanceVM.ExecutionContext, this);
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
