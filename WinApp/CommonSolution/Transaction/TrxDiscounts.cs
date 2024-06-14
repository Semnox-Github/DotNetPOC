using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    public partial class TrxDiscounts : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int TrxId;
        Utilities Utilities;
        public TrxDiscounts(int pTrxId, Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(pTrxId, ParafaitUtilities);

            InitializeComponent();
            TrxId = pTrxId;
            Utilities = ParafaitUtilities;

            log.LogMethodExit(null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            Close();

            log.LogMethodExit(null);
        }

        private void TrxDiscounts_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            DataTable dt = Utilities.executeDataTable(@"select Discount_Name, LineId, DiscountPercentage ""%"", td.DiscountAmount
                                                          from TrxDiscounts td, Discounts d
                                                          where td.discountId = d.discount_id
                                                          and td.TrxId = @TrxId",
                                                        new SqlParameter[] { new SqlParameter("@TrxId", TrxId) });
            log.LogVariableState("@TrxId", TrxId);

            dgvDiscounts.DataSource = dt;
            Utilities.setupDataGridProperties(ref dgvDiscounts);
            dgvDiscounts.Columns["DiscountAmount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvDiscounts.Columns["%"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();

            Utilities.setLanguage(this);

            log.LogMethodExit(null);
        }
    }
}
