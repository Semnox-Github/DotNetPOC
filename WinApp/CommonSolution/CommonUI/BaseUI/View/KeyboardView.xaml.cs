/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - keyboard view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Siba Maharan            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;


namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for KeyboardView.xaml
    /// </summary>
    public partial class KeyboardView : Window
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion Members

        #region Constructor

        public KeyboardView()
        {
            log.LogMethodEntry();
            InitializeComponent();

            if (SystemParameters.PrimaryScreenWidth > 1000)
            {
                Width = 930;
                Height = 300;
            }
            else
            {
                Width = 750;
                Height = 250;
            }
            log.LogMethodExit();
        }

        #endregion Constructor

    }
}
