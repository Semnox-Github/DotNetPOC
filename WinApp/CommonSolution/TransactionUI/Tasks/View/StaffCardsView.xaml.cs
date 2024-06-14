
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    /// <summary>
    /// Interaction logic for StaffCardsView.xaml
    /// </summary>
    public partial class StaffCardsView : Window
    {
        private KeyboardHelper KeyboardHelper;
        public StaffCardsView()
        {
            InitializeComponent();
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            KeyboardHelper = new KeyboardHelper();
            this.ContentRendered += StaffCardsView_ContentRendered;
        }

        private void StaffCardsView_ContentRendered(object sender, EventArgs e)
        {
            if (this.DataContext != null)
            {
                StaffCardsVM staffCardVM = this.DataContext as StaffCardsVM;

                if (staffCardVM != null)
                {

                    ExecutionContext executionContext = staffCardVM.ExecutionContext;

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
