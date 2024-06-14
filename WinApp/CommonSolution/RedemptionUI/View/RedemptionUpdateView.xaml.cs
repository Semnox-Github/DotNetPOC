/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - Redemption update view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Windows;

using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.RedemptionUI
{
    /// <summary>
    /// Interaction logic for RedemptionUpdateView.xaml
    /// </summary>
    public partial class RedemptionUpdateView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        #endregion

        #region Properties
        public KeyboardHelper KeyboardHelper
        {
            get
            {
                return keyboardHelper;
            }
        }
        #endregion

        #region Constructor
        public RedemptionUpdateView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            keyboardHelper = new KeyboardHelper();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.SizeChanged += OnSizeChanged;
            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                RedemptionUpdateVM redemptionUpdateVM = DataContext as RedemptionUpdateVM;
                if (redemptionUpdateVM != null && redemptionUpdateVM.ExecutionContext != null)
                {
                    TranslateHelper.Translate(redemptionUpdateVM.ExecutionContext, this);
                    keyboardHelper.ShowKeyBoard(this, new System.Collections.Generic.List<System.Windows.Controls.Control>() { btnKeyBoard },
                ParafaitDefaultViewContainerList.GetParafaitDefault(redemptionUpdateVM.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new System.Collections.Generic.List<System.Windows.Controls.Control>());
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
                    RedemptionUpdateVM redemptionUpdateVM = DataContext as RedemptionUpdateVM;
                    if (redemptionUpdateVM != null && redemptionUpdateVM.MultiScreenMode)
                    {
                        MainGrid.MaxWidth = e.NewSize.Width - 10;
                        MainGrid.MaxHeight = e.NewSize.Height - 10;
                        MainGrid.MinWidth = e.NewSize.Width - 10;
                        MainGrid.MinHeight = e.NewSize.Height - 10;
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
