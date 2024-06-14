/********************************************************************************************
* Project Name - Recipe Management 
* Description  - UI to choose date range for export feature
* 
**************
**Version Log
**************
*Version     Date          Modified By       Remarks          
*********************************************************************************************
*2.100.0     26-Nov-2020    Deeksha          Created as part of Recipe Management enhancement.
********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory.Recipe
{
    public partial class frmExportDateRangeUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public frmExportDateRangeUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            dtpFromDate.Value = utilities.getServerTime();
            dtpToDate.Value = utilities.getServerTime();
            utilities.setLanguage(this);
            ThemeUtils.SetupVisuals(this);
            log.LogMethodExit();
        }

        public DateTime FromDate
        {
            get { return dtpFromDate.Value.Date; } 
        }

        public DateTime ToDate
        {
            get { return dtpToDate.Value.Date; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool result = ValidateDateRange();
            if(!result)
            {
                this.DialogResult = DialogResult.None;
            }
            log.LogMethodExit();
        }

        private bool ValidateDateRange()
        {
            log.LogMethodEntry();
            if(dtpToDate.Value.Date < dtpFromDate.Value.Date)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 724)); //To Date should be greater than From Date)
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }
    }
}
