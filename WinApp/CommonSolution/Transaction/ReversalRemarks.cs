using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    public partial class ReversalRemarks : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Remarks;
        public string reason;//Added to capture the selected drop down value in ReversalRemarks  form on 07-Oct-2015
        Utilities _Utilities;
        private VirtualKeyboardController virtualKeyboardController;
        private string lookUpCode = "TRX_REVERSAL_REASONS";
        public ReversalRemarks(Utilities inUtilities, string lookUpCode = "TRX_REVERSAL_REASONS")
        {
            log.LogMethodEntry(inUtilities, lookUpCode);

            _Utilities = inUtilities;
            InitializeComponent();
            virtualKeyboardController = new VirtualKeyboardController();
            virtualKeyboardController.Initialize(this, new List<Control>(){btnShowNumPad}, ParafaitDefaultContainerList.GetParafaitDefault<bool>(inUtilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            this.lookUpCode = lookUpCode;
            log.LogMethodExit(null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (txtRemarks.Text.Trim().Length < 60)
                Remarks = txtRemarks.Text.Trim();
            else
                Remarks = txtRemarks.Text.Trim().Substring(0, 59);
            reason = cmbReversalReason.SelectedValue.ToString();// Assign the value selected in the dropdown to string reason on 07-Oct-2015

            log.LogVariableState("reason ", reason);
            log.LogMethodExit(null);
        }

        private void ReversalRemarks_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (virtualKeyboardController != null)
                virtualKeyboardController.Dispose();

            log.LogMethodExit(null);
        }

        private void ReversalRemarks_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            this.ActiveControl = txtRemarks;

            DataTable dt = _Utilities.executeDataTable(@"select lookupValue
                                                         from lookupView 
                                                        where LookupName = @lookUpCode 
                                                        union all 
                                                         select '' order by 1",
                                                        new SqlParameter("@lookUpCode", lookUpCode));
            cmbReversalReason.DataSource = dt;
            cmbReversalReason.ValueMember = "LookupValue";
            cmbReversalReason.DisplayMember = "LookupValue";

            cmbReversalReason.SelectedIndex = 0;

            //cmbReversalReason.SelectedIndexChanged += cmbReversalReason_SelectedIndexChanged;
            this.Location = new System.Drawing.Point(this.Location.X, (this.Location.Y - 125));
            log.LogMethodExit(null);
        }

        //void cmbReversalReason_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //if (String.IsNullOrEmpty(txtRemarks.Text))
        //    //{
        //    //    txtRemarks.Text = cmbReversalReason.SelectedValue.ToString();
        //    //}
        //}
    }
}
