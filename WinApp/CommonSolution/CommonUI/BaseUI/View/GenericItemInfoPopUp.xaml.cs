/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Generic Item info popup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     17-Nov-2020   Siba Maharana              Created for POS UI Redesign 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for GenericItemInfoPopUp.xaml
    /// </summary>
    public partial class GenericItemInfoPopUp : Window
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

        public GenericItemInfoPopUp()
        {
            log.LogMethodEntry();
            InitializeComponent();

            keyboardHelper = new KeyboardHelper();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            if (MainContainerGrid != null)
            {
                this.MainContainerGrid.MaxHeight = SystemParameters.PrimaryScreenHeight - 40;
            }
            this.SizeChanged += OnSizeChanged;
            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void OnContentRendered(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.DataContext != null)
            {
                GenericItemInfoPopUpVM itemInfoPopUpVM = DataContext as GenericItemInfoPopUpVM;
                if (itemInfoPopUpVM != null)
                {
                    TranslateHelper.Translate(itemInfoPopUpVM.ExecutionContext, this);
                }
            }
            keyboardHelper.ShowKeyBoard(this, new List<Control>(), ParafaitDefaultViewContainerList.GetParafaitDefault(TranslateHelper.executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
            log.LogMethodExit();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (DataContext != null)
            {
                MainContainerGrid.MinWidth = 500;
                MainContainerGrid.MaxHeight = e.NewSize.Height - 20;
                GenericItemInfoPopUpVM itemInfoPopUpVM = DataContext as GenericItemInfoPopUpVM;
                if (itemInfoPopUpVM != null && itemInfoPopUpVM.MultiScreenMode)
                {
                    MainContainerGrid.MaxWidth = e.NewSize.Width - 20;
                    if (itemInfoPopUpVM.MultiScreenMode && e.NewSize.Width < 520)
                    {
                        MainContainerGrid.MaxWidth = e.NewSize.Width - 50;
                        MainContainerGrid.MinWidth = e.NewSize.Width - 50;
                    }
                    if (itemInfoPopUpVM.IsMultiScreenRowTwo)
                    {
                        MainContainerGrid.MaxWidth = e.NewSize.Width - 50;
                        MainContainerGrid.MinWidth = e.NewSize.Width - 50;
                        MainContainerGrid.MinHeight = e.NewSize.Height - 20;
                        MainContainerGrid.MaxHeight = e.NewSize.Height - 15;
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
