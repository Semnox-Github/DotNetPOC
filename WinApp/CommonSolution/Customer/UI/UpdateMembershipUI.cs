/********************************************************************************************
 * Project Name - Customer
 * Description  - UpdateMembershipUI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019      Girish kundar      Modified :Removed Unused namespace's.
 *2.90        03-July-2020     Girish Kundar   Modified : Change as part of CardCodeDTOList replaced with AccountDTOList in CustomerDTO 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Membership;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Update Membership UI class
    /// </summary>
    public partial class UpdateMembershipUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private CustomerDTO customerDTO;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities">parafait utilities</param>
        /// <param name="customerDTO">Customer DTO</param>
        public UpdateMembershipUI(Utilities utilities, CustomerDTO customerDTO)
        {
            log.LogMethodEntry(utilities, customerDTO);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setLanguage(this);
            this.customerDTO = customerDTO;
            lblCustomerName.Text = customerDTO.FirstName + " " + customerDTO.LastName;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            log.LogMethodExit();
        }

        private async void UpdateMembershipUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            cmbCurrentMembership.DisplayMember = "Name";
            cmbCurrentMembership.ValueMember = "Id";
            cmbNewMembeship.DisplayMember = "Name";
            cmbNewMembeship.ValueMember = "Id";
            List<MembershipDTO> membershipActiveDTOList = await Task<List<MembershipDTO>>.Factory.StartNew(() => { return GetMembershipDTOList(true); });
            List<MembershipDTO> membershipCurrentDTOList = await Task<List<MembershipDTO>>.Factory.StartNew(() => { return GetMembershipDTOList(false); });
            // BindingSource bindingSource = new BindingSource();
            // bindingSource.DataSource = membershipCurrentDTOList;
            cmbCurrentMembership.DataSource = membershipCurrentDTOList;
            cmbCurrentMembership.DisplayMember = "MembershipName";
            cmbCurrentMembership.ValueMember = "MembershipId";
            cmbCurrentMembership.SelectedValue = this.customerDTO.MembershipId;
            // BindingSource bindingSourceActive = new BindingSource();
            //bindingSourceActive.DataSource = membershipActiveDTOList;
            cmbNewMembeship.DataSource = membershipActiveDTOList;
            cmbNewMembeship.DisplayMember = "MembershipName";
            cmbNewMembeship.ValueMember = "MembershipId";
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            log.LogMethodExit();
        }


        private List<MembershipDTO> GetMembershipDTOList(bool activeOnly)
        {
            log.LogMethodEntry(activeOnly);
            List<MembershipDTO> membershipDTOList = null;
            try
            {
                MembershipsList membershipsList = new MembershipsList(this.utilities.ExecutionContext);
                List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchMemberParams = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
                if(activeOnly)
                searchMemberParams.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, "1"));

                searchMemberParams.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                membershipDTOList = membershipsList.GetAllMembership(searchMemberParams, utilities.ExecutionContext.GetSiteId());
                if (membershipDTOList == null)
                {
                    membershipDTOList = new List<MembershipDTO>();
                }
                membershipDTOList.Insert(0, new MembershipDTO());
                membershipDTOList[0].MembershipName = "SELECT";
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading membership list", ex);
            }
            log.LogMethodExit(membershipDTOList);
            return membershipDTOList;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int newMembershipId = Convert.ToInt32(cmbNewMembeship.SelectedValue); 
            if (this.customerDTO.MembershipId != newMembershipId)
            {
               if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext,1528), "Update Membership", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    try
                    {
                        CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, this.customerDTO, true);
                        CustomerDTO customerDTOValue = customerBL.CustomerDTO;
                        if (customerDTOValue.AccountDTOList != null && customerDTOValue.AccountDTOList.Count > 0)
                        {
                            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
                            try
                            {
                                parafaitDBTrx.BeginTransaction(); 
                                customerBL.OverRideMembership(newMembershipId, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                                DialogResult = DialogResult.OK;
                                Close();
                            }
                            catch(ValidationException ex)
                            {
                                log.Error("validation failed", ex);
                                StringBuilder errorMessageBuilder = new StringBuilder("");
                                foreach (var validationError in ex.ValidationErrorList)
                                {
                                    errorMessageBuilder.Append(validationError.Message);
                                    errorMessageBuilder.Append(Environment.NewLine);
                                }
                                MessageBox.Show(errorMessageBuilder.ToString(), MessageContainerList.GetMessage(utilities.ExecutionContext, "Update Membership"));
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                parafaitDBTrx.RollBack();
                                this.customerDTO = customerDTOValue;
                                throw new Exception(ex.Message);                                
                            }
                            finally
                            {
                                parafaitDBTrx.Dispose();
                            }
                        }
                        else
                        {
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1529));
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1492), "Update Membership"); 
                    }
                }
            }
            else
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 371));
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }
    }
}
