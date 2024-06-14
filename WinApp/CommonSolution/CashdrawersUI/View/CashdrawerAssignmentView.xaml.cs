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
    /// Interaction logic for CashdrawerAssignmentView.xaml
    /// </summary>
    public partial class CashdrawerAssignmentView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;

        public CashdrawerAssignmentView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += CashdrawerAssignmentView_ContentRendered;
        }

        private void CashdrawerAssignmentView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.DataContext != null)
            {
                CashdrawerAssignmentVM cashdrawerAssignmentVM = this.DataContext as CashdrawerAssignmentVM;

                if (cashdrawerAssignmentVM != null)
                {
                    ExecutionContext executioncontext = cashdrawerAssignmentVM.ExecutionContext;
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
