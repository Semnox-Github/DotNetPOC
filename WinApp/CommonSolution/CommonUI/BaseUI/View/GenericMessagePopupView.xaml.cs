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
using System.Windows;
using System.Reflection;


namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for GenericMessagePopupView.xaml
    /// </summary>
    public partial class GenericMessagePopupView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructors
        public GenericMessagePopupView()
        {
            log.LogMethodEntry();
            InitializeComponent();

            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            if (MainGrid != null)
            {
                MainGrid.MaxHeight = Height - 20;
            }
            log.LogMethodExit();
        }
        #endregion

    }
}
