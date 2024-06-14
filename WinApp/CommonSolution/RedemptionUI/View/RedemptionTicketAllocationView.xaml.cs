/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - Redemption ticket allocation view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.RedemptionUI
{
    /// <summary>
    /// Interaction logic for RedemptionTicketAllocationView.xaml
    /// </summary>
    public partial class RedemptionTicketAllocationView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        #endregion

        #region Properties
        public KeyboardHelper KeyBoardHelper
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(keyboardHelper);
                return keyboardHelper;
            }
        }
        #endregion

        #region Constructor       

        public RedemptionTicketAllocationView()
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
        private void OnContentRendered(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                RedemptionTicketAllocationVM ticketAllocationVM = DataContext as RedemptionTicketAllocationVM;
                if (ticketAllocationVM != null)
                {
                    if (ticketAllocationVM.ExecutionContext != null)
                    {
                        TranslateHelper.Translate(ticketAllocationVM.ExecutionContext, this);
                        ticketAllocationVM.OpenManualTicketNumberpad();
                    }
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
                    RedemptionTicketAllocationVM redemptionReverseVM = DataContext as RedemptionTicketAllocationVM;
                    if (redemptionReverseVM != null)
                    {
                        if (redemptionReverseVM.IsMultiScreenRowTwo)
                        {
                            MainGrid.MaxWidth = e.NewSize.Width - 25;
                            MainGrid.MaxHeight = e.NewSize.Height - 40;
                            MainGrid.MinWidth = e.NewSize.Width - 25;
                            MainGrid.MinHeight = e.NewSize.Height - 40;
                        }
                        else if (redemptionReverseVM.MultiScreenMode)
                        {
                            MainGrid.MaxWidth = e.NewSize.Width - 40;
                            MainGrid.MaxHeight = e.NewSize.Height - 40;
                            MainGrid.MinWidth = e.NewSize.Width - 40;
                            MainGrid.MinHeight = e.NewSize.Height - 40;
                        }
                        if (keyboardHelper != null)
                        {
                            keyboardHelper.NumberpadClosedEvent -= redemptionReverseVM.OnNumberpadClosedEvent;
                            keyboardHelper.NumberpadClosedEvent += redemptionReverseVM.OnNumberpadClosedEvent;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
