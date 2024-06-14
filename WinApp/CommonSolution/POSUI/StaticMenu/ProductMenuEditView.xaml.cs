/********************************************************************************************
 * Project Name - POS UI
 * Description  - Product Menu Edit View
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     2-Jul-2021   Lakshminarayana          Created for product menu enhancement 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace Semnox.Parafait.POSUI.StaticMenu
{
    /// <summary>
    /// Interaction logic for ProductMenuEditView.xaml
    /// </summary>
    public partial class ProductMenuEditView : Window
    {
        #region Members
        private KeyboardHelper keyboardHelper;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public ProductMenuEditView()
        {
            log.LogMethodEntry();

            InitializeComponent();

            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            //if(MainGrid != null)
            {
                MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth - 100;
                MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight - 50;
            }

            this.ContentRendered += ProductMenuEditView_ContentRendered;

            log.LogMethodExit();
        }

        private void ProductMenuEditView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            if (DataContext != null)
            {
                ProductMenuEditViewModel productMenuEditViewModel = this.DataContext as ProductMenuEditViewModel;
                if (productMenuEditViewModel != null)
                {
                    ExecutionContext executionContext = productMenuEditViewModel.ExecutionContext;
                    if (executionContext != null)
                    {
                        keyboardHelper = new KeyboardHelper();
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                    }
                }
            }
            log.LogMethodExit();
        }

    }
}
