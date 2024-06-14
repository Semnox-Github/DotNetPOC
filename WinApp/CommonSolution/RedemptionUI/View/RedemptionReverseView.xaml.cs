/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - Redemption reverse view 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;

using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.RedemptionUI
{
    /// <summary>
    /// Interaction logic for RedemptionReverseView.xaml
    /// </summary>
    public partial class RedemptionReverseView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor
        public RedemptionReverseView()
        {
            log.LogMethodEntry();
            InitializeComponent();

            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.SizeChanged += OnSizeChanged;
            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void OnContentRendered(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                RedemptionReverseVM reverseVM = DataContext as RedemptionReverseVM;
                if (reverseVM != null)
                {
                    TranslateHelper.Translate(reverseVM.ExecutionContext, this);
                }
            }
            log.LogMethodExit();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (MainGrid != null)
            {
                MainGrid.MaxWidth = e.NewSize.Width - 100;
                MainGrid.MaxHeight = e.NewSize.Height - 50;
                MainGrid.MinWidth = e.NewSize.Width - 100;
                MainGrid.MinHeight = e.NewSize.Height - 50;
                if (DataContext != null)
                {
                    RedemptionReverseVM redemptionReverseVM = DataContext as RedemptionReverseVM;
                    if (redemptionReverseVM != null && redemptionReverseVM.MultiScreenMode)
                    {
                        MainGrid.MaxWidth = e.NewSize.Width - 10;
                        MainGrid.MaxHeight = e.NewSize.Height - 10;
                        MainGrid.MinWidth = e.NewSize.Width - 10;
                        MainGrid.MinHeight = e.NewSize.Height - 5;
                    }
                }
            }
            log.LogMethodExit();
        }

        #endregion
    }
}
