using CashdrawersUI.ViewModel;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.CashdrawersUI
{
    /// <summary>
    /// Interaction logic for POSUserAssignmentView.xaml
    /// </summary>
    public partial class POSUserAssignmentView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;

        public POSUserAssignmentView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth - 100;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += POSUserAssignmentView_ContentRendered;
        }

        private void POSUserAssignmentView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.DataContext != null)
            {
                POSUserAssignmentVM pOSUserAssignmentVM = this.DataContext as POSUserAssignmentVM;

                if (pOSUserAssignmentVM != null)
                {
                    ExecutionContext executioncontext = pOSUserAssignmentVM.ExecutionContext;
                    if (executioncontext != null)
                    {
                        TranslateHelper.Translate(executioncontext, this);
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                            keyboardHelper = new KeyboardHelper();
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultContainerList.GetParafaitDefault(executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

    }
}
