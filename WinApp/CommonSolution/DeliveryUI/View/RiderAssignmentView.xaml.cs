/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - RiderAssignment UI
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Jun-2021   Fiona                  Created 
 ********************************************************************************************/
using System;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.Generic;

using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.DeliveryUI
{
    /// <summary>
    /// Interaction logic for RiderAssignmentView.xaml
    /// </summary>
    public partial class RiderAssignmentView : Window
    {
        #region Members
        private KeyboardHelper keyboardHelper;
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public KeyboardHelper KeyboardHelper { get { return keyboardHelper; } }
        #endregion

        #region Constructor
        public RiderAssignmentView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            keyboardHelper = new KeyboardHelper();
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
                RiderAssignmentVM riderAssignmentVM = DataContext as RiderAssignmentVM;
                if (riderAssignmentVM != null && riderAssignmentVM.ExecutionContext != null)
                {
                    TranslateHelper.Translate(riderAssignmentVM.ExecutionContext, this);
                    FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                    if (footerView != null)
                    {
                        keyboardHelper.ShowKeyBoard( this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(riderAssignmentVM.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false,
                             new List<Control>());
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
