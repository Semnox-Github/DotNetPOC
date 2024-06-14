/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Game Management Form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy              Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.GamesUI
{
    /// <summary>
    /// Interaction logic for GMMainView.xaml
    /// </summary>
    /// 


    public partial class GMMainView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;

        #region Constructors
        public GMMainView()
        {
            log.LogMethodEntry();
            //       ParafaitPOS.App.EnsureApplicationResources();
            // this.DataContext = new GMMainVM(App.machineUserContext);
            InitializeComponent();
            this.ContentRendered += GMMainView_ContentRendered;
            log.LogMethodExit();
        }

        private void GMMainView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);


            if (this.DataContext != null)
            {
                GMMainVM gmMainVM = this.DataContext as GMMainVM;

                if (gmMainVM != null)
                {
                    ExecutionContext executioncontext = gmMainVM.GetExecutionContext();

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

        #endregion

    }
}
