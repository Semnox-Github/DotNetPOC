using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI.Sales.View
{
    /// <summary>
    /// Interaction logic for ProductQuantityPrompt.xaml
    /// </summary>
    public partial class ProductQuantityPromptView : Window
    {
        #region Members
        private KeyboardHelper keyboardHelper;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public KeyboardHelper KeyBoardHelper { get { return keyboardHelper; } }
        #endregion
        /// <summary>
        /// VCATPrintOptionView
        /// </summary>
        public ProductQuantityPromptView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            keyboardHelper = new KeyboardHelper();
            if (MainGrid != null)
            {
                MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth;
                MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight;
            }
            this.ContentRendered += OnContentRendered;
        }
        #region Methods
        internal void OnContentRendered(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                ProductQuantityPromptVM productQuantityPromptVM = DataContext as ProductQuantityPromptVM;
                if (productQuantityPromptVM != null && productQuantityPromptVM.ExecutionContext != null)
                {
                    FooterUserControl footerUserControl = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                    if (footerUserControl != null)
                    {
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerUserControl.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(productQuantityPromptVM.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                    }
                    TranslateHelper.Translate(productQuantityPromptVM.ExecutionContext, this);
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
