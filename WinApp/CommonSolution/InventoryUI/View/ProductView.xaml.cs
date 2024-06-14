/********************************************************************************************
 * Project Name - InventoryUI
 * Description  - ProductView 
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
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;

        /// <summary>
        /// ProductView
        /// </summary>
        public ProductView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            if (MainGrid != null)
            {
                MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth - 250 ;
                MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight- 100 ;
            }
            this.ContentRendered += ProductView_ContentRendered;
        }

        private void ProductView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.DataContext != null)
            {
                ReceiveStockVM receiveStockVM = this.DataContext as ReceiveStockVM;

                if (receiveStockVM != null)
                {
                    ExecutionContext executioncontext = receiveStockVM.ExecutionContext;
                }
            }
            log.LogMethodExit();
        }

    }
}
