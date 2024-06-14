/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Change password view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     20-Jul-2021   Raja Uthanda          Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for FontPickerView.xaml
    /// </summary>
    public partial class FontPickerView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        #endregion

        #region Methods
        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                FontPickerVM fontPickerVM = DataContext as FontPickerVM;
                if (fontPickerVM != null && fontPickerVM.ExecutionContext != null)
                {
                    keyboardHelper = new KeyboardHelper();
                    keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnKeyBoard }, false /*ParafaitDefaultViewContainerList.GetParafaitDefault(fontPickerVM.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false*/, new List<Control>());
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor        
        public FontPickerView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight - 20;
            MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth - 20;
            //MainGrid.Width = 500;
            //MainGrid.Height = 400;
            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }
        #endregion
    }
}
