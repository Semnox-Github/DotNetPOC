/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - numeric keyboard view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     17-Nov-2020   Raja Uthanda              Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for NumberKeyboardView.xaml
    /// </summary>
    public partial class NumberKeyboardView : Window
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        #region Constructors
        public NumberKeyboardView()
        {
            log.LogMethodEntry();
            InitializeComponent();

            this.DataContext = new NumberKeyboardVM()
            {
                NumberKeyboardView = this
            };
            this.ContentRendered += OnContentRendered;

            log.LogMethodExit();
        }

        private void OnContentRendered(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.txtNum.Focus();
            this.txtNum.CaretIndex = this.txtNum.Text.Length;
            log.LogMethodExit();
        }
        #endregion
    }
}
