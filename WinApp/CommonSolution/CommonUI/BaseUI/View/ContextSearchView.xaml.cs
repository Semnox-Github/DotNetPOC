/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - GenericMessagePopupView
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for ContextSearchView.xaml
    /// </summary>
    public partial class ContextSearchView : Window
    {

        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int selectedId = -1;
        private KeyboardHelper keyboardHelper;
        #endregion

        #region Properties
        public int SelectedId
        {
            get
            {
                log.LogMethodEntry();
                return selectedId;
            }
            set
            {
                log.LogMethodEntry(value);
                selectedId = value;
            }
        }
        #endregion

        #region Constuctors
        public ContextSearchView()
        {
            log.LogMethodEntry();
            InitializeComponent();

            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;

            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            this.MainGrid.MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 20;
            this.ContentRendered += ContextSearchView_ContentRendered;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void ContextSearchView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            keyboardHelper = new KeyboardHelper();
            keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnKeyBoard }, ParafaitDefaultViewContainerList.GetParafaitDefault(TranslateHelper.executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
            this.txtSearch.Focus();
            log.LogMethodExit();
        }
        #endregion

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
