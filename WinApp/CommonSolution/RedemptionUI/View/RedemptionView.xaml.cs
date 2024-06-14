/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - Redemption main view
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
    /// Interaction logic for RedemptionView.xaml
    /// </summary>
    public partial class RedemptionView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor
        public RedemptionView()
        {
            log.LogMethodEntry();
            InitializeComponent();
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
                RedemptionMainVM mainVM = DataContext as RedemptionMainVM;
                if (mainVM != null && mainVM.ExecutionContext != null)
                {
                    TranslateHelper.Translate(mainVM.ExecutionContext, this);
                }
            }
            log.LogMethodExit();
        }

        private void RedemptionMainUserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RedemptionMainUserControl mainUserControl = sender as RedemptionMainUserControl;
            if (mainUserControl != null && mainUserControl.DataContext != null &&
                mainUserControl.DataContext is RedemptionMainUserControlVM)
            {
                RedemptionMainUserControlVM mainUserControlVM = mainUserControl.DataContext as RedemptionMainUserControlVM;
                if (mainUserControlVM != null)
                {
                    mainUserControlVM.RedempMainUserControlVM = mainUserControlVM;
                    mainUserControlVM.RedemptionMainUserControl = mainUserControl;
                    mainUserControlVM.RedemptionMainView = this;
                    mainUserControlVM.MainVM = this.DataContext as RedemptionMainVM;
                    mainUserControlVM.UpdateDefaultValues();
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
