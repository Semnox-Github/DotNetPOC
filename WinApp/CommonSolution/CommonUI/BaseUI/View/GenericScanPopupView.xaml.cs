/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Generic Scan pop up view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     17-Nov-2020   Amitha Joy              Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for GenericActionPopup.xaml
    /// </summary>
    public partial class GenericScanPopupView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constuctors
        public GenericScanPopupView()
        {
            log.LogMethodEntry();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            InitializeComponent();
            if (MainGrid != null)
            {
                MainGrid.MinWidth = (SystemParameters.PrimaryScreenWidth - 100) / 3;
                MainGrid.MinHeight = (SystemParameters.PrimaryScreenWidth - 100) / 3;
                MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth - 100;
                MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight - 50;
            }
            this.ContentRendered += OnContentRendered;
            this.SizeChanged += OnSizeChanged;
            log.LogMethodExit();
        }
        #endregion

        #region Methods

        private void OnContentRendered(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                GenericScanPopupVM scanPopupVM = DataContext as GenericScanPopupVM;
                if (scanPopupVM != null && scanPopupVM.ExecutionContext != null)
                {
                    TranslateHelper.Translate(scanPopupVM.ExecutionContext, this);
                }
            }
            log.LogMethodExit();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                GenericScanPopupVM scanPopupVM = DataContext as GenericScanPopupVM;
                if (scanPopupVM != null && scanPopupVM.IsMultiScreenRowTwo && MainGrid != null)
                {
                    MainGrid.MaxWidth = e.NewSize.Width - 230;
                    MainGrid.MaxHeight = e.NewSize.Height - 230;
                    MainGrid.MinWidth = e.NewSize.Width - 50;
                    MainGrid.MinHeight = e.NewSize.Height - 50;
                    if (MainGrid.MinWidth > 400)
                    {
                        MainGrid.MinWidth = 400;
                    }
                    if (MainGrid.MinHeight > 400)
                    {
                        MainGrid.MinHeight = 300;
                    }
                    if (imageGrid != null)
                    {
                        imageGrid.Height = 100;
                    }
                }
                else
                {
                    MainGrid.MinWidth = (SystemParameters.PrimaryScreenWidth - 100) / 3;
                    MainGrid.MinHeight = (SystemParameters.PrimaryScreenWidth - 100) / 3;
                    MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth - 100;
                    MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight - 50;
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
