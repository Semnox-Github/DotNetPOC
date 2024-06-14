/********************************************************************************************
 * Project Name - POS
 * Description  - user control for customer object in frmMapWaiversToTransaction form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.80.0      25-Sep-2019   Guru S A                Created for Waivers phase 2 enhancement changes 
 *2.120.1     31-May-2021   Nitin Pai               Show Customer age on screen   
 *2.140.0     14-Sep-2021   Guru S A                Waiver mapping UI enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver; 

namespace Parafait_POS.Waivers
{
    
    public partial class usrCtlCustomer : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool isSelected;
        private CustomerDTO customerDTO;
        private List<WaiversDTO> waiversDTOList;
        private int guestCustomerId;
        private Transaction transaction;
        private ExecutionContext executionContext; 
        internal delegate  void SignWaiverDelegate(CustomerDTO customerDTO);
        internal SignWaiverDelegate signWaiverDelegate;
        private LookupValuesList serverTimeObject;
        private ToolTip customerNameToolTip;
        private ToolTip customerEmailToolTip;

        public bool IsSelected { get { return isSelected; } set { isSelected = value; SetColor(); } }
        public CustomerDTO CustomerDTO { get { return customerDTO; } set { customerDTO = value; } }
        public usrCtlCustomer(ExecutionContext executionContext, CustomerDTO selectedCustDTO, System.Collections.Generic.List<Semnox.Parafait.Waiver.WaiversDTO> trxWaiversDTOList, Transaction trx , int guestCustomerId)
        {
            log.LogMethodEntry(selectedCustDTO, trxWaiversDTOList, trx, guestCustomerId);
            InitializeComponent();
            this.customerDTO = selectedCustDTO;
            this.waiversDTOList = trxWaiversDTOList;
            this.transaction = trx;
            this.executionContext = executionContext; 
            serverTimeObject = new LookupValuesList(executionContext);
            this.guestCustomerId = guestCustomerId;
            SetDisplayElements();
            log.LogMethodExit();
        }

        public void SetDisplayElements()
        {
            log.LogMethodEntry();
            string customerAge = string.Empty;
            if (CustomerDTO.DateOfBirth != null)
            {                                
                //Config check to display customer Age
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SHOW_CUSTOMER_AGE_ONSCREEN").Equals("Y"))
                {
                    CustomerBL customerBL = new CustomerBL(executionContext, CustomerDTO);
                    customerAge = customerBL.GetAge().ToString();
                }
            }

            this.lblCustomerName.Text = this.CustomerDTO.FirstName + " " +
                    (string.IsNullOrEmpty(this.CustomerDTO.LastName) ? string.Empty : this.CustomerDTO.LastName)  + " " + (string.IsNullOrEmpty(customerAge)
                            ? "" : "[" + MessageContainerList.GetMessage(executionContext, "AGE") + ": " + customerAge + " "+MessageContainerList.GetMessage(executionContext, "YEARS") + "]");
            customerNameToolTip = new ToolTip();
            customerNameToolTip.SetToolTip(lblCustomerName, lblCustomerName.Text);

            this.lblEmailId.Text = this.CustomerDTO.Email;
            customerEmailToolTip = new ToolTip();
            customerEmailToolTip.SetToolTip(lblEmailId, lblCustomerName.Text);

            bool hasSigned = false;
            if (guestCustomerId > -1 && this.CustomerDTO.Id == guestCustomerId)
            {
               if(transaction.IsWaiverSignaturePending() == false)
                {
                    hasSigned = true;
                }
            }
            else
            {
                CustomerBL customerBL = new CustomerBL(executionContext, CustomerDTO);
                DateTime trxDatevalue = GetTrxDate();
                if (customerBL.HasSigned(waiversDTOList, trxDatevalue))
                {
                    hasSigned = true;
                }
            }
            if (hasSigned)
            {
                this.pbxSigned.BackgroundImage = Properties.Resources.GreenTick;
                this.pbxSigned.BackgroundImageLayout = ImageLayout.Stretch;
                //this.pbxSigned.Location = new System.Drawing.Point(this.lblEmailId.Location.X + this.lblEmailId.Width + 5, this.lblCustomerName.Location.Y);
                this.btnSignWaiver.Visible = false;
                this.pbxSigned.Visible = true; 
                //if (showRemoveButton)
                //{  
                //    //this.pbxRemove.Location = new Point(this.pbxSigned.Location.X + this.pbxSigned.Width + 5, this.lblCustomerName.Location.Y); 
                //} 
                //else
                //{
                //    this.pbxRemove.Visible = false;
                //}
                
            }
            else
            {
                this.pbxSigned.Visible = false;
                //this.btnSignWaiver.Location = new System.Drawing.Point(this.lblEmailId.Location.X + this.lblEmailId.Width + 5, this.lblCustomerName.Location.Y);
                this.btnSignWaiver.Visible = true; 
                //if (showRemoveButton)
                //{
                //    //this.pbxRemove.Location = new Point(this.btnSignWaiver.Location.X + this.btnSignWaiver.Width + 5, this.lblCustomerName.Location.Y); 
                //}
                //else
                //{
                //    this.pbxRemove.Visible = false;
                //}
            }
            //this.Width = 340;
            this.Width = 432;
            log.LogMethodExit();
        }

        private DateTime GetTrxDate()
        {
            log.LogMethodEntry();
            DateTime trxDatevalue = (this.transaction.TrxDate == DateTime.MinValue ?
                                     (this.transaction.TransactionDate == DateTime.MinValue ? serverTimeObject.GetServerDateTime() : this.transaction.TransactionDate)
                                     : this.transaction.TrxDate);
            log.LogMethodExit(trxDatevalue);
            return trxDatevalue;
        }
        private void lblCustomerName_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.IsSelected)
                this.IsSelected = false;
            else
                this.IsSelected = true;

            customerNameToolTip.Show(lblCustomerName.Text, this, new Point(lblCustomerName.Location.X - lblCustomerName.Width, lblCustomerName.Location.Y),1000);
            //SetColor();
            log.LogMethodExit();
        }

        private void SetColor()
        {
            log.LogMethodEntry();
            if (isSelected)
            {
                this.BackColor = Color.PaleTurquoise;
            }
            else
            {
                BackColor = System.Drawing.SystemColors.InactiveCaption;
            }
            log.LogMethodExit();
        }

        private void btnSignWaiver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.customerDTO != null)
            {
                signWaiverDelegate(customerDTO);
            }
            log.LogMethodExit();
        }

        private void usrCtlCustomer_MouseEnter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //customerNameToolTip.Show(lblCustomerName.Text, this, new Point(lblCustomerName.Location.X - lblCustomerName.Width, lblCustomerName.Location.Y), 1000);
            this.Cursor = Cursors.Hand;
            log.LogMethodExit();
        }

        private void usrCtlCustomer_MouseHover(object sender, EventArgs e)
        {            
            log.LogMethodEntry();
            //customerNameToolTip.Show(lblCustomerName.Text, this, new Point(lblCustomerName.Location.X - lblCustomerName.Width, lblCustomerName.Location.Y), 1000);
            this.Cursor = Cursors.Hand;
            log.LogMethodExit();
        }

        private void usrCtlCustomer_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //customerNameToolTip.Hide(this);
            log.LogMethodExit();
        }

        private void usrCtlCustomer_MouseLeave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //customerNameToolTip.Hide(this);
            log.LogMethodExit();
             
        }
    }
}
