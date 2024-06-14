/********************************************************************************************
 * Project Name - InventoryUI
 * Description  - ReceiptView 
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
using System.Windows;

namespace Semnox.Parafait.InventoryUI
{
    /// <summary>
    /// Interaction logic for PurchaseOrderView.xaml
    /// </summary>
    public partial class ReceiptView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;

        /// <summary>
        /// PerformManualEventView
        /// </summary>
        
        public ReceiptView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            if (MainGrid != null)
            {
                MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth -100 ;
                MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight -50 ;
            }
            this.ContentRendered += ReceiptView_ContentRendered;
        }

        private void ReceiptView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.DataContext != null)
            {
                PurchaseOrderVM purchaseOrderVM = this.DataContext as PurchaseOrderVM;

                if (purchaseOrderVM != null)
                {
                    ExecutionContext executioncontext = purchaseOrderVM.ExecutionContext;
                }
            }
            log.LogMethodExit();
        }

    }
}
