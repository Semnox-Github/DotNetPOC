/********************************************************************************************
* Project Name - Parafait_Kiosk -frmAgeGate.cs
* Description  - frmAgeGate 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using Semnox.Core.Utilities;
using System;

namespace Parafait_FnB_Kiosk
{
    public partial class frmAgeGate : BaseForm
    {
        int alcocholAgeLimit = 21;
        public frmAgeGate()
        {
            log.LogMethodEntry();
            InitializeComponent();

            Int32.TryParse(Common.utils.getParafaitDefaults("ALCOHOL_SALE_AGE_LIMIT"), out alcocholAgeLimit);
            log.LogMethodExit();
        }

        private void frmAgeGate_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblYear.BringToFront();
                lblMonth.BringToFront();
                lblDay.BringToFront();

                for (int i = 1; i <= 12; i++)
                    cmbMonth.Items.Add(i.ToString().PadLeft(2, '0'));
                cmbMonth.SelectedIndex = -1;

                for (int i = ServerDateTime.Now.Year - 100; i <= ServerDateTime.Now.AddYears(-17).Year; i++)
                    cmbYear.Items.Add(i);
                cmbYear.SelectedItem = ServerDateTime.Now.AddYears(-(alcocholAgeLimit - 1)).Year;

                for (int i = 1; i <= 31; i++)
                    cmbDay.Items.Add(i.ToString().PadLeft(2, '0'));

                cmbMonth.SelectedIndexChanged += delegate
                {
                    lblMonth.Text = cmbMonth.SelectedItem.ToString();
                    fillDayCombo();
                };

                cmbYear.SelectedIndexChanged += delegate
                {
                    lblYear.Text = cmbYear.SelectedItem.ToString();
                    fillDayCombo();
                };

                cmbDay.SelectedIndex = -1;

                cmbDay.SelectedIndexChanged += delegate
                {
                    lblDay.Text = cmbDay.SelectedItem.ToString();
                };

                lblDay.Click += delegate
                {
                    base.ResetTimeOut();
                    cmbDay.DroppedDown = true;
                };

                lblMonth.Click += delegate
                {
                    base.ResetTimeOut();
                    cmbMonth.DroppedDown = true;
                };

                lblYear.Click += delegate
                {
                    base.ResetTimeOut();
                    cmbYear.DroppedDown = true;
                };
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }

        void fillDayCombo()
        {
            log.LogMethodEntry();
            if (cmbMonth.SelectedIndex == -1 || cmbYear.SelectedIndex == -1)
            {
                log.LogMethodExit();
                return;
            }

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DateTime refDate = DateTime.ParseExact(cmbYear.SelectedItem.ToString() + "-" + cmbMonth.SelectedItem.ToString() + "-01", "yyyy-MM-dd", provider);

            int lastDayOfMonth = refDate.AddMonths(1).AddDays(-1).Day;

            int savDay = cmbDay.SelectedIndex;

            cmbDay.Items.Clear();

            for (int i = 1; i <= lastDayOfMonth; i++)
                cmbDay.Items.Add(i.ToString().PadLeft(2, '0'));

            try
            {
                cmbDay.SelectedIndex = savDay;
            }
            catch
            {
                lblDay.Text = "Day";
                cmbDay.SelectedIndex = -1;
            }
            log.LogMethodExit();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();

            if (cmbDay.SelectedIndex == -1 || cmbMonth.SelectedIndex == -1 || cmbYear.SelectedIndex == -1)
            {
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1112));
                log.LogMethodExit();
                return;
            }

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DateTime refDate = DateTime.ParseExact(cmbYear.SelectedItem.ToString() + "-" + cmbMonth.SelectedItem.ToString() + "-" + cmbDay.SelectedItem.ToString(), "yyyy-MM-dd", provider);

            if (refDate <= ServerDateTime.Now.AddYears(-alcocholAgeLimit))
            {
                Common.ShowAlert(Common.utils.MessageUtils.getMessage(1111));
                UserTransaction.OrderDetails.AgeGateDate = refDate;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1112));
            log.LogMethodExit();
        }
    }
}
