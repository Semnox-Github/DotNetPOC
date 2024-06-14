/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Change password view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     15-Jul-2021   Raja Uthanda          Created for POS UI Redesign 
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
    /// Interaction logic for ColorPickerView.xaml
    /// </summary>
    public partial class ColorPickerView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        #endregion
        
        #region Methods
        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(DataContext != null)
            {
                ColorPickerVM colorPickerVM = DataContext as ColorPickerVM;
                if(colorPickerVM != null)
                {
                    ExecutionContext executioncontext = colorPickerVM.ExecutionContext;
                    if (executioncontext != null)
                    {
                        keyboardHelper = new KeyboardHelper();
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { this.btnKeyBoard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor        
        public ColorPickerView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            MainGrid.Height = SystemParameters.PrimaryScreenHeight - 20;
            MainGrid.Width = SystemParameters.PrimaryScreenWidth - 20;
            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }
        
        #endregion
    }
}
