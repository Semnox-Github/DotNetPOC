/********************************************************************************************
 * Project Name - TagsUI
 * Description  - NotificationTagSearchView 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.120       04-Mar-2021   Girish Kundar          Created - POS Stock UI change
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.InventoryUI
{
    /// <summary>
    /// Interaction logic for ReceiveStockView.xaml
    /// </summary>
    public partial class ReceiveStockView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;

        /// <summary>
        /// PerformManualEventView
        /// </summary>
        public ReceiveStockView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            keyboardHelper = new KeyboardHelper();
            this.ContentRendered += ReceiveStockView_ContentRendered;
        }

        internal void ReceiveStockView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);


            if (this.DataContext != null)
            {
                ReceiveStockVM receiveStockVM = this.DataContext as ReceiveStockVM;

                if (receiveStockVM != null)
                {
                    ExecutionContext executioncontext = receiveStockVM.ExecutionContext;

                    if (executioncontext != null)
                    {
                        TranslateHelper.Translate(executioncontext, this);
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {   
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultContainerList.GetParafaitDefault(executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

    }
}
