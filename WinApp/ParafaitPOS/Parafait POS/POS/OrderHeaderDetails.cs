/******************************************************************************************************************
 * Project Name - Order header details
 * Description  - Class to show Order screen and manage Order object
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ******************************************************************************************************************
 *1.00.0      22-May-2008   Iqbal Mohammad          Created 
 *2.100.0     17-Aug-2020   Mathew Ninan            Captured Transaction Order Type Id
  *****************************************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class OrderHeaderDetails : Form
    {
        public OrderHeaderBL Order;
        Transaction Trx;
        TextBox CurrentTextBox;
        int TableId;

        public OrderHeaderDetails(Transaction transaction, int pTableId)
        {
            InitializeComponent();
            Trx = transaction;
            if (transaction.Order != null)
                Order = transaction.Order;
            TableId = pTableId;

            if (Trx.PrimaryCard != null && Trx.PrimaryCard.customerDTO != null)
                txtCustomername.Text = Trx.PrimaryCard.customerDTO.FirstName + (Trx.PrimaryCard.customerDTO.LastName.Trim() == "" ? "" : " " + Trx.PrimaryCard.customerDTO.LastName.Trim());
            else if (Trx.customerDTO != null)
                txtCustomername.Text = Trx.customerDTO.ProfileDTO.FirstName + (Trx.customerDTO.ProfileDTO.LastName.Trim() == "" ? "" : " " + Trx.customerDTO.ProfileDTO.LastName.Trim());
            CurrentTextBox = txtTableNumber;
            txtWaiterName.Text = POSStatic.Utilities.ParafaitEnv.Username;
            object o = POSStatic.Utilities.executeScalar(@"SELECT Top 1 FacilityId FROM FacilityPOSAssignment WHERE POSMachineId=@posMachineId", new SqlParameter("@posMachineId", POSStatic.ParafaitEnv.POSMachineId));

            if (o != null && o != DBNull.Value)
            {
                txtTableNumber.Enabled = false;
            }
            else
            {
                txtTableNumber.Enabled = true;
            }//end

            if (TableId != -1)
            {
                
                txtTableNumber.Text = POSStatic.Utilities.executeScalar("select TableName from FacilityTables where TableId = @id",
                                                                             new SqlParameter("@id", TableId)).ToString();
            }
            POSStatic.Utilities.setLanguage(this);
        }
        private void OrderHeaderDetails_Load(object sender, EventArgs e)
        {
            if (Trx.Order != null && Trx.Order.OrderHeaderDTO != null && Trx.Order.OrderHeaderDTO.OrderId > -1)
            {
                Order = new OrderHeaderBL(POSStatic.Utilities.ExecutionContext, Trx.Order.OrderHeaderDTO.OrderId);
                if (!string.IsNullOrEmpty(Order.OrderHeaderDTO.CustomerName))
                    txtCustomername.Text = Order.OrderHeaderDTO.CustomerName;
                if (!string.IsNullOrEmpty(Order.OrderHeaderDTO.Remarks))
                    txtRemarks.Text = Order.OrderHeaderDTO.Remarks;
                if (!string.IsNullOrEmpty(Order.OrderHeaderDTO.TableNumber))
                    txtTableNumber.Text = Order.OrderHeaderDTO.TableNumber;
                if (!string.IsNullOrEmpty(Order.OrderHeaderDTO.WaiterName))
                    txtWaiterName.Text = Order.OrderHeaderDTO.WaiterName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Order == null)
            {
                Order = new OrderHeaderBL(POSStatic.Utilities.ExecutionContext, Trx);
                Order.OrderHeaderDTO.UserId = Trx.Utilities.ParafaitEnv.User_Id;
            }
                
            Order.OrderHeaderDTO.CustomerName = txtCustomername.Text;
            Order.OrderHeaderDTO.Remarks = txtRemarks.Text;
            Order.OrderHeaderDTO.TableNumber = txtTableNumber.Text;
            Order.OrderHeaderDTO.WaiterName = txtWaiterName.Text;
            Order.OrderHeaderDTO.TableId = TableId;
            if (Trx.Order != null && Trx.Order.OrderHeaderDTO != null && Trx.Order.OrderHeaderDTO.TransactionOrderTypeId != -1)
            {
                Order.OrderHeaderDTO.TransactionOrderTypeId = Trx.Order.OrderHeaderDTO.TransactionOrderTypeId;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            this.ActiveControl = CurrentTextBox;
            showNumberPadForm('-');
        }

        AlphaNumericKeyPad keypad;
        void showNumberPadForm(char firstKey)
        {
            TextBox txtBox = CurrentTextBox;
            if (txtBox != null && !txtBox.ReadOnly)
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, txtBox);
                    keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                    keypad.Show();
                    this.Location = new Point(this.Location.X, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height - this.Height);
                }
                else if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.Show();
                }
            }
        }

        private void txtTableNumber_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = txtTableNumber;
            SetKeyPadTextBox();
        }

        private void txtWaiterName_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = txtWaiterName;
            SetKeyPadTextBox();
        }

        private void txtCustomername_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = txtCustomername;
            SetKeyPadTextBox();
        }

        private void txtRemarks_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = txtRemarks;
            SetKeyPadTextBox();
        }

        private void OrderHeaderDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (keypad != null)
                keypad.Close();
        }

        private void SetKeyPadTextBox()
        {
            if (keypad != null && keypad.Visible)
                keypad.currentTextBox = CurrentTextBox;
        }
    }
}
