﻿/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Game Management Main Page
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
    /// Interaction logic for GMEditMachineView.xaml
    /// </summary>
    public partial class GMEditMachineView : Window
    {
        #region Members
        private KeyboardHelper keyboardHelper;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public GMEditMachineView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            keyboardHelper = new KeyboardHelper();
            if (MainGrid != null)
            {
                MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth;
                MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight;
            }
            this.ContentRendered += GMEditMachineView_ContentRendered;

            log.LogMethodExit();
        }

        private void GMEditMachineView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                GMEditMachineVM gmEditMachineVM = this.DataContext as GMEditMachineVM;
                if (gmEditMachineVM != null)
                {
                    ExecutionContext executionContext = gmEditMachineVM.GetExecutionContext();
                    if (executionContext != null)
                    {
                        FooterUserControl footerUserControl = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerUserControl != null)
                        {
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerUserControl.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                        TranslateHelper.Translate(executionContext, this);
                    }
                }
            }
            log.LogMethodExit();
        }

    }
}
