/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - Redemption scan view 
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
    /// Interaction logic for RedemptionScanView.xaml
    /// </summary>
    public partial class RedemptionScanView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor
        public RedemptionScanView()
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
                RedemptionScanVM scanVM = DataContext as RedemptionScanVM;
                if (scanVM != null && scanVM.ExecutionContext != null)
                {
                    TranslateHelper.Translate(scanVM.ExecutionContext, this);
                }
            }
            log.LogMethodExit();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (MainGrid != null)
            {
                if (this.DataContext != null)
                {
                    RedemptionScanVM scanVM = this.DataContext as RedemptionScanVM;
                    if (scanVM != null && scanVM.MultiScreenMode)
                    {
                        MainGrid.MaxWidth = e.NewSize.Width - 30;
                        MainGrid.MinWidth = e.NewSize.Width - 30;
                        if (scanVM.RedemptionMainUserControlVM != null && scanVM.RedemptionMainUserControlVM.MainVM != null)
                        {
                            if (scanVM.RedemptionMainUserControlVM.MainVM.RedemptionUserControlVMs.Count == 2)
                            {
                                MainGrid.MinHeight = 400;
                            }
                            else if (scanVM.RedemptionMainUserControlVM.MainVM.RowCount == 2)
                            {
                                MainGrid.MaxHeight = scanVM.RedemptionMainUserControlVM.RedemptionMainUserControl.ActualHeight - 50;
                                MainGrid.MinHeight = scanVM.RedemptionMainUserControlVM.RedemptionMainUserControl.ActualHeight - 50;
                            }
                        }
                        if (MainGrid.MaxHeight < 500)
                        {
                            MainGrid.MinHeight = e.NewSize.Height - 40;
                        }
                    }
                    else
                    {
                        MainGrid.MinWidth = 700;
                        MainGrid.MinHeight = 400;
                    }
                }
                else
                {
                    MainGrid.MinWidth = 700;
                    MainGrid.MinHeight = 400;
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
