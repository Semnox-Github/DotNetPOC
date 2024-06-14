/********************************************************************************************
 * Project Name - TagsUI
 * Description  - PerformManualEventView 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.120       04-Mar-2021   Girish Kundar          Created - Is Radian change
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.TagsUI
{
    /// <summary>
    /// Interaction logic for PerformManualEventView.xaml
    /// </summary>
    public partial class PerformManualEventView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;

        /// <summary>
        /// PerformManualEventView
        /// </summary>
        public PerformManualEventView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += TagView_ContentRendered;
        }

        private void TagView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);


            if (this.DataContext != null)
            {
                PerformManualEventVM performManualEventVM = this.DataContext as PerformManualEventVM;

                if (performManualEventVM != null)
                {
                    ExecutionContext executioncontext = performManualEventVM.GetExecutionContext();

                    if (executioncontext != null)
                    {
                        TranslateHelper.Translate(executioncontext, this);
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                            keyboardHelper = new KeyboardHelper();
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultContainerList.GetParafaitDefault(executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

    }
}
