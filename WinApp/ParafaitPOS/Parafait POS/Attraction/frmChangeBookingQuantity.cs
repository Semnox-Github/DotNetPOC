/* Project Name - frmChangeBookingQuantity
* Description  - frmChangeBookingQuantity form
*
**************
** Version Log
 **************
 * Version     Date          Modified By     Remarks
*********************************************************************************************
*2.80.0        20-Aug-2019    Girish Kundar  Modified :  Added logger methods
********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Parafait_POS.Attraction
{
    public partial class frmChangeBookingQuantity : Form
    {
        public int _Quantity = -1;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public frmChangeBookingQuantity(int Quantity, string FormText = null)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry();//Added for logger function on 08-Mar-2016
            InitializeComponent();
            nudChangeQuantity.Maximum = Quantity;
            if (FormText == null)
                nudChangeQuantity.Value = Quantity;
            else
            {
                this.Text = FormText;
                nudChangeQuantity.Value = 1;
            }
            log.LogMethodExit();//Added for logger function on 08-Mar-2016
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();//Added for logger function on 08-Mar-2016
            _Quantity = (int)nudChangeQuantity.Value;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();//Added for logger function on 08-Mar-2016
        }
    }
}
