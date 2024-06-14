/********************************************************************************************
*Project Name - Parafait Report                                                                          
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*2.80       18-Sep-2019             Dakshakh raj                Modified : Added logs                           
*********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Semnox.Parafait.Report.Reports
{
    /// <summary>
    /// TriggerQuery class
    /// </summary>
    public partial class TriggerQuery : Form
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// triggerQueryStr Property
        /// </summary>
        public string triggerQueryStr;
        /// <summary>
        /// TriggerQuery method
        /// </summary>
        /// <param name="query">query</param>
        public TriggerQuery(string query)
        {
            log.LogMethodEntry(query);
            InitializeComponent();
            triggerQueryStr = query;
            txtDBQuery.Text = triggerQueryStr;
            log.LogMethodExit();
        }

        /// <summary>
        /// btnCancel_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSave_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if(!txtDBQuery.Text.ToLower().StartsWith("select"))
            {
                MessageBox.Show("Query should begin with Select");
                log.LogMethodExit();
                return;
            }
            triggerQueryStr = txtDBQuery.Text;
            this.DialogResult = DialogResult.OK;
            log.LogMethodExit();
        }
    }
}
