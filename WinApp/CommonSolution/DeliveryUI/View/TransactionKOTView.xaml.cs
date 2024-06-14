/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - TransactionKOT UI
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Jun-2021   Fiona                  Created 
 ********************************************************************************************/
using System;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.Generic;

using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.DeliveryUI
{
    /// <summary>
    /// Interaction logic for TransactionKOTView.xaml
    /// </summary>
    public partial class TransactionKOTView : Window
    {
        #region Members
        private KeyboardHelper keyboardHelper;
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public KeyboardHelper KeyboardHelper { get { return keyboardHelper; } }
        #endregion

        #region Constructors
        public TransactionKOTView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            keyboardHelper = new KeyboardHelper();
            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.DataContext != null)
            {
                TransactionKOTVM transactionKOTVM = this.DataContext as TransactionKOTVM;
                if (transactionKOTVM != null && transactionKOTVM.ExecutionContext != null)
                {
                    TranslateHelper.Translate(transactionKOTVM.ExecutionContext, this);
                    FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                    if (footerView != null)
                    {
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(transactionKOTVM.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false,
                            new List<Control>());
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion

    }
}
