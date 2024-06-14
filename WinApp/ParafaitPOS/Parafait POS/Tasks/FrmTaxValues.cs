/********************************************************************************************
 * Project Name - Parafait POS
 * Description  - Input the new Tax Values
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
*2.6.3       18-Jun-2019      Indhu K        Ability to let the customer enter the new tax values
*2.7.0       22-July-2019     Mithesh        Venezuela Fiscal Printer related changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.Printer.FiscalPrint;

namespace Parafait_POS
{
    public partial class FrmTaxValues : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<string> taxValuesList = new List<string>();
        Utilities utilities;
        FiscalPrinter FiscalPrinter;

        public FrmTaxValues(Utilities Utilities, FiscalPrinter FiscalPrinter)
        {
            log.LogMethodEntry(Utilities, FiscalPrinter);
            InitializeComponent();

            List<string> taxList = FiscalPrinter.InitializeTax();

            txtTaxValue1.Text = taxList[0];
            txtTaxValue2.Text = taxList[1];
            txtTaxValue3.Text = taxList[2];

            txtExsistingTax1.Text = taxList[0];
            txtExsistingTax2.Text = taxList[1];
            txtExsistingTax3.Text = taxList[2];

            utilities = Utilities;
            this.FiscalPrinter = FiscalPrinter;
            log.LogMethodExit();
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if(txtTaxValue1.Text == string.Empty ||
                   txtTaxValue2.Text == string.Empty ||
                   txtTaxValue3.Text == string.Empty)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, utilities.MessageUtils.getMessage("Invalid Tax Percentage")));
                }
                else
                {
                    taxValuesList.Clear();
                    GetTaxValue(txtTaxValue1.Text);
                    GetTaxValue(txtTaxValue2.Text);
                    GetTaxValue(txtTaxValue3.Text);

                    string Message = string.Empty;
                    bool status = FiscalPrinter.ChangeTaxValues(ref Message, taxValuesList);
                    if (!status)
                    {
                        POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(2202), "Fiscal Printer Report Error");
                        btnZReport.Visible = true;
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(122), "Fiscal Printer");
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
        }

        private void txtTaxValue1_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            double taxValue;
            if (double.TryParse(txtTaxValue1.Text, out taxValue))
            {
                double val = Math.Round(double.Parse(txtTaxValue1.Text), 2);
                txtTaxValue1.Text = val.ToString();
            }
            else
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, utilities.MessageUtils.getMessage("Invalid Tax Percentage")));
                txtTaxValue1.Text = string.Empty;
            }
            log.LogMethodExit();
        }

        private void txtTaxValue2_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            double taxValue;
            if (double.TryParse(txtTaxValue2.Text, out taxValue))
            {
                double val = Math.Round(double.Parse(txtTaxValue2.Text), 2);
                txtTaxValue2.Text = val.ToString();
            }
            else
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, utilities.MessageUtils.getMessage("Invalid Tax Percentage")));
                txtTaxValue1.Text = string.Empty;
            }
            log.LogMethodExit();
        }

        private void txtTaxValue3_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            double taxValue;
            if (double.TryParse(txtTaxValue3.Text, out taxValue))
            {
                double val = Math.Round(double.Parse(txtTaxValue3.Text), 2);
                txtTaxValue3.Text = val.ToString();
            }
            else
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, utilities.MessageUtils.getMessage("Invalid Tax Percentage")));
                txtTaxValue1.Text = string.Empty;
            }
            log.LogMethodExit();
        }

        private void GetTaxValue(string taxValue)
        {
            log.LogMethodEntry(taxValue);
            var s = string.Format("{0:00.00}", Convert.ToDouble(taxValue));
            taxValuesList.Add(s.ToString());
            log.LogMethodExit();
        }

        private void btnZReport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (txtTaxValue1.Text == string.Empty ||
                txtTaxValue2.Text == string.Empty ||
                txtTaxValue3.Text == string.Empty)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, utilities.MessageUtils.getMessage("Invalid Tax Percentage")));
            }
            else
            {
                string message = string.Empty;
                FiscalPrinter.PrintReport("RunZReport", ref message);

                taxValuesList = new List<string>();
                GetTaxValue(txtTaxValue1.Text);
                GetTaxValue(txtTaxValue2.Text);
                GetTaxValue(txtTaxValue3.Text);

                bool status = FiscalPrinter.ChangeTaxValues(ref message, taxValuesList);
                if (!status)
                    POSUtils.ParafaitMessageBox(message, "Fiscal Printer Report Error");
                else
                    POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(122), "Fiscal Printer");
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }
    }
}
