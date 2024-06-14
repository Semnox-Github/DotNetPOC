/********************************************************************************************
 * Project Name - InventoryUI
 * Description  - POReceiveRemarkView 
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
    /// Interaction logic for POReceiveRemarkView.xaml
    /// </summary>
    public partial class POReceiveRemarkView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;

        /// <summary>
        /// PerformManualEventView
        /// </summary>
        public POReceiveRemarkView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            if (MainGrid != null)
            {
                MainGrid.MaxWidth = Width -200;
                MainGrid.MaxHeight = Height-100;
            }
            this.ContentRendered += POReceiveRemarkView_ContentRendered;
        }

        private void POReceiveRemarkView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.DataContext != null)
            {
                POReceiveRemarksVM poReceiveRemarksVM = this.DataContext as POReceiveRemarksVM;

                if (poReceiveRemarksVM != null)
                {
                    ExecutionContext executioncontext = poReceiveRemarksVM.ExecutionContext;
                    if (executioncontext != null)
                    {
                        TranslateHelper.Translate(executioncontext, this);
                        keyboardHelper = new KeyboardHelper();
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnFooterKeyboard }, ParafaitDefaultContainerList.GetParafaitDefault(executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                    }
                }
            }
            log.LogMethodExit();
        }

    }
}
