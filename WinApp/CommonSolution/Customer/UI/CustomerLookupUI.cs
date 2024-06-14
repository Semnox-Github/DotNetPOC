/********************************************************************************************
 * Project Name - Customer.UI
 * Description  - User interface for CustomerLookupUI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar       Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Customer lookup UI
    /// </summary>
    public partial class CustomerLookupUI : CustomerListUI
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //string cardNumber = string.Empty;

        //public string CardNumber
        //{
        //    get
        //    {
        //        return cardNumber;
        //    }
        //}

        private CustomerDTO selectedCustomerDTO;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="uid"></param>
        public CustomerLookupUI(Utilities utilities, string firstName = "", string middleName = "", string lastName = "", string email ="", string phone ="", string uid="") :base(utilities)
        {
            log.LogMethodEntry(utilities, firstName, middleName, lastName, email, phone, uid);
            InitializeComponent();
            this.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Lookup");
            StartPosition = FormStartPosition.CenterScreen;
            txtFirstName.Text = firstName;
            txtMiddleName.Text = middleName;
            txtLastName.Text = lastName;
            txtPhone.Text = phone;
            txtEmail.Text = email;
            txtUniqueIdentifier.Text = uid;
            //
           // tlpMain.RowStyles[2] = new RowStyle(SizeType.Absolute, 0f);
           // grpCard.Visible = false;
            
            //dgvCustomerDTOList.CellContentClick += dgvCustomerDTOList_CellContentClick;
            dgvCustomerDTOList.Columns["selectDataGridViewButtonColumn"].Visible = true;
            dgvCustomerDTOList.Columns["lastNameDataGridViewTextBoxColumn"].Frozen = true;
            btnSave.Visible = false;
            btnRefresh.Visible = false;
            btnRelationship.Visible = false;
            btnExport.Visible = false;
            btnImport.Visible = false;
            btnUpdateMembership.Visible = false;
            btnClose.Margin = new Padding(0);
            flpButtonControls.Anchor = AnchorStyles.Bottom;
            this.SizeChanged += CustomerLookupUI_SizeChanged;
            flpButtonControls.Left = (this.ClientSize.Width - btnClose.Width) / 2;
            for (int i = 0; i < dgvCustomerDTOList.Columns.Count; i++)
            {
                if(dgvCustomerDTOList.Columns["selectDataGridViewButtonColumn"] != dgvCustomerDTOList.Columns[i])
                {
                    dgvCustomerDTOList.Columns[i].ReadOnly = true;
                }
            }
            dgvAddressDTOList.AllowUserToAddRows = false;
            for (int i = 0; i < dgvAddressDTOList.Columns.Count; i++)
            {
                dgvAddressDTOList.Columns[i].ReadOnly = true;
            }
            dgvContactDTOList.AllowUserToAddRows = false;
            for (int i = 0; i < dgvContactDTOList.Columns.Count; i++)
            {
                dgvContactDTOList.Columns[i].ReadOnly = true;
            }
            log.LogMethodExit();
        }

        private void CustomerLookupUI_SizeChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnClose.Margin = new Padding(0);
            flpButtonControls.Left = (this.ClientSize.Width - btnClose.Width) / 2;
            log.LogMethodExit();
        }

        /// <summary>
        /// on CustomerDTOList content clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void dgvCustomerDTOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.RowIndex >= 0 && 
                    e.ColumnIndex == dgvCustomerDTOList.Columns["selectDataGridViewButtonColumn"].Index &&
                    customerDTOListBS.Current != null &&
                    customerDTOListBS.Current is CustomerDTO)
                {
                    if (string.IsNullOrWhiteSpace(cardNumber))
                    {
                        cardNumber = dgvCustomerDTOList.Rows[e.RowIndex].Cells["descriptionDataGridViewTextBoxColumn"].Value.ToString();
                    }
                    selectedCustomerDTO = customerDTOListBS.Current as CustomerDTO;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
                else
                {
                    base.dgvCustomerDTOList_CellContentClick(sender, e);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while selecting the customer", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the selected customer
        /// </summary>
        public CustomerDTO SelectedCustomerDTO
        {
            get
            {
                return selectedCustomerDTO;
            }
        }
        
        protected override void dgvCardDTOList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogMethodExit();
        }
    }
}
