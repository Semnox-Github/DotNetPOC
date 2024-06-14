/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Link cards View 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-June-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.TransactionUI
{
    /// <summary>
    /// Interaction logic for LinkCardsView.xaml
    /// </summary>
    public partial class LinkCardsView : Window
    {
        private KeyboardHelper KeyboardHelper;
        public LinkCardsView()
        {
            InitializeComponent();
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            KeyboardHelper = new KeyboardHelper();
            this.ContentRendered += LinkCardsView_ContentRendered;
        }

        private void LinkCardsView_ContentRendered(object sender, EventArgs e)
        {
            if (this.DataContext != null)
            {
                LinkCardsVM linkCardsVM = this.DataContext as LinkCardsVM;

                if (linkCardsVM != null)
                {
                    ExecutionContext executionContext = linkCardsVM.ExecutionContext;
                    if (executionContext != null)
                    {
                        TranslateHelper.Translate(executionContext, this);
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                            KeyboardHelper = new KeyboardHelper();
                            KeyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
        }
    }
}
