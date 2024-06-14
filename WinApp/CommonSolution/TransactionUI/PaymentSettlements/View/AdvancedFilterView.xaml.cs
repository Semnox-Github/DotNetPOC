﻿/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - AdvancedFilterView
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     28-Sep-2021    Fiona                  Created 
 ********************************************************************************************/
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
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    /// <summary>
    /// Interaction logic for AdvancedFilterView.xaml
    /// </summary>
    public partial class AdvancedFilterView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        public AdvancedFilterView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += OnContentRendered; 
            log.LogMethodExit();
        }
        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.DataContext != null)
            {
                AdvancedFilterVM advancedFilterVM = this.DataContext as AdvancedFilterVM;

                if (advancedFilterVM != null)
                {
                    ExecutionContext executioncontext = advancedFilterVM.ExecutionContext;

                    if (executioncontext != null)
                    {
                        TranslateHelper.Translate(executioncontext, this);

                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                            keyboardHelper = new KeyboardHelper();
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
