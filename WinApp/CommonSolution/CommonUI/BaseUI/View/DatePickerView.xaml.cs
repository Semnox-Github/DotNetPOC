/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - DatePickerView
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/

using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.Generic;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for DatePickerView.xaml
    /// </summary>
    public partial class DatePickerView : Window
    {
        #region Members
        private string selectedDate;
        private KeyboardHelper keyboardHelper;
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string SelectedDate
        {
            get
            {
                log.LogMethodEntry();
                return selectedDate;
            }
            set
            {
                log.LogMethodEntry(value);
                selectedDate = value;
            }
        }
        #endregion

        #region Constructors and Finalizers
        public DatePickerView()
        {
            log.LogMethodEntry();
            InitializeComponent();

            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight - 72;
            keyboardHelper = new KeyboardHelper();
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
                DatePickerVM datePickerVM = DataContext as DatePickerVM;
                if (datePickerVM != null && datePickerVM.ExecutionContext != null)
                {
                    keyboardHelper.ShowKeyBoard(this, new List<Control>() { this.btnkeyBoard }, ParafaitDefaultViewContainerList.GetParafaitDefault(datePickerVM.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
