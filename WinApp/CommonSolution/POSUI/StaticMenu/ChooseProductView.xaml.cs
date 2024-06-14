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
    /// Interaction logic for ChooseProductView.xaml
    /// </summary>
    public partial class ChooseProductView : Window
    {

        private KeyboardHelper keyboardHelper;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ChooseProductView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            MainGrid.Height = SystemParameters.PrimaryScreenHeight - 20;
            MainGrid.Width = SystemParameters.PrimaryScreenWidth - 20;
            ContentRendered += OnContentRendered;

            log.LogMethodExit();
        }


        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            if (DataContext != null)
            {
                ChooseProductVM chooseProductVM = this.DataContext as ChooseProductVM;
                if (chooseProductVM != null)
                {
                    ExecutionContext executionContext = chooseProductVM.ExecutionContext;
                    if (executionContext != null)
                    {
                        keyboardHelper = new KeyboardHelper();
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnKeyBoard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
