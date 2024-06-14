using System.Windows;

namespace Semnox.Parafait.TransactionUI.Sales.View
{
    /// <summary>
    /// Interaction logic for ProductQuantityPrompt.xaml
    /// </summary>
    public partial class ProductQuantityPromptView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// VCATPrintOptionView
        /// </summary>
        public ProductQuantityPromptView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            if (MainGrid != null)
            {
                MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth - 300;
                MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight - 100;
            }
        }
    }
}
