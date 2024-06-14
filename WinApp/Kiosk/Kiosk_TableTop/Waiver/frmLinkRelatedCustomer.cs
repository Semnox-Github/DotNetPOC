/********************************************************************************************
 * Project Name - Portait Kiosk
 * Description  - frmLinkRelatedCustomer UI form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Parafait_Kiosk
{
    public partial class frmLinkRelatedCustomer : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private CustomerDTO customerDTO = null;
        private CustomerDTO relatedCustomerDTO = null;
        private string defaultMsg;
        public frmLinkRelatedCustomer(CustomerDTO signatoryCustomerDTO, CustomerDTO newRelatedCustomerDTO)
        {
            log.LogMethodEntry(signatoryCustomerDTO, newRelatedCustomerDTO);
            this.utilities = KioskStatic.Utilities;
            this.customerDTO = signatoryCustomerDTO;
            this.relatedCustomerDTO = newRelatedCustomerDTO;
            utilities.setLanguage();
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            cmbRelation.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            LoadRelationShipTypes();
            if (customerDTO != null)
            {
                this.lblCustomerValue.Text = (string.IsNullOrEmpty(customerDTO.FirstName) ? string.Empty : customerDTO.FirstName)
                                            + " "
                                            + (string.IsNullOrEmpty(customerDTO.LastName) ? string.Empty : customerDTO.LastName);
            }
            if (relatedCustomerDTO != null)
            {
                this.lblRelatedCustomerValue.Text = relatedCustomerDTO.FirstName + " " + (string.IsNullOrEmpty(relatedCustomerDTO.LastName) ? string.Empty : relatedCustomerDTO.LastName);
            }
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2427);//Please select relationship type
            DisplayMessageLine(defaultMsg);
            //SetTextBoxFontColors();
            SetCustomizedFontColors();
            utilities.setLanguage(this);
            KioskStatic.logToFile("Loading link related customer form");
            log.LogMethodExit();
        }

        private void LoadRelationShipTypes()
        {
            log.LogMethodEntry();
            List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = null;
            CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(utilities.ExecutionContext);
            List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchCustomerRelationshipTypeParams = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
            searchCustomerRelationshipTypeParams.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchCustomerRelationshipTypeParams.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.SiteId.ToString()));
            customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchCustomerRelationshipTypeParams);

            if (customerRelationshipTypeDTOList == null)
            {
                customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();
            }
            customerRelationshipTypeDTOList.Insert(0, new CustomerRelationshipTypeDTO());
            customerRelationshipTypeDTOList[0].Name = "SELECT";
            customerRelationshipTypeDTOList[0].Id = -1;
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = customerRelationshipTypeDTOList;
            cmbRelation.DataSource = bindingSource;
            cmbRelation.DisplayMember = "Name";
            cmbRelation.ValueMember = "Id";
            log.LogMethodExit();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                KioskStatic.logToFile("Save button is clicked");
                DisplayMessageLine(defaultMsg);
                if (cmbRelation.SelectedIndex > 0 && cmbRelation.SelectedValue != null)
                {
                    CustomerRelationshipDTO customerRelationshipDTO = new CustomerRelationshipDTO();
                    customerRelationshipDTO.CustomerRelationshipTypeId = Convert.ToInt32(cmbRelation.SelectedValue);
                    customerRelationshipDTO.CustomerId = customerDTO.Id;
                    customerRelationshipDTO.RelatedCustomerId = relatedCustomerDTO.Id;
                    CustomerRelationshipBL customerRelationshipBL = new CustomerRelationshipBL(utilities.ExecutionContext, customerRelationshipDTO);
                    customerRelationshipBL.Save();
                    this.Close();
                }
                else
                {
                    throw new ValidationException(defaultMsg);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
                DisplayMessageLine(ex.Message);
            }
            log.LogMethodExit();
        }
        void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Cancel button is clicked");
            //2436,'Newly created customer &1 will not be linked with &2. Do you want to proceed?
            using (frmYesNo frm = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2436, this.lblRelatedCustomerValue.Text, this.lblCustomerValue.Text)))
            {
                if (frm.ShowDialog() == DialogResult.Yes)
                {
                    KioskStatic.logToFile("user wants to proceed without linking customers");
                    log.Info("user wants to proceed without linking customers");
                    this.Close();
                }
            }
            log.LogMethodExit();
        }


        private void frmLinkRelatedCustomer_Closing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Closing link related customer form");
            log.LogMethodExit();
        }
        private void SetTextBoxFontColors()
        {
            log.LogMethodEntry();
            if (KioskStatic.CurrentTheme == null ||
               (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
            {
                //lblCustomerValue.ForeColor = Color.Black;
                //lblRelatedCustomerValue.ForeColor = Color.Black;
                cmbRelation.ForeColor = Color.Black;
            }
            else
            {
                //lblCustomerValue.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                //lblRelatedCustomerValue.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                cmbRelation.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmLinkRelatedCustomer");
            try
            {
                this.lblCustomer.ForeColor = KioskStatic.CurrentTheme.FrmLinkCustLblCustomerTextForeColor;
                this.lblCustomerValue.ForeColor = KioskStatic.CurrentTheme.FrmLinkCustLblCustomerValueTextForeColor;
                this.lblRelatedCustomer.ForeColor = KioskStatic.CurrentTheme.FrmLinkCustLblRelatedCustomerTextForeColor;
                this.lblRelatedCustomerValue.ForeColor = KioskStatic.CurrentTheme.FrmLinkCustLblRelatedCustomerValueTextForeColor;
                this.label2.ForeColor = KioskStatic.CurrentTheme.FrmLinkCustLabel2TextForeColor;
                this.cmbRelation.ForeColor = KioskStatic.CurrentTheme.FrmLinkCustCmbRelationTextForeColor;
                this.btnSave.ForeColor = KioskStatic.CurrentTheme.FrmLinkCustBtnSaveTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmLinkCustBtnCancelTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmLinkCustTxtMessageTextForeColor;
                this.BackgroundImage = ThemeManager.GetBackgroundImageTwo(ThemeManager.CurrentThemeImages.LinkRelatedCustomerBackgroundImage);
                btnSave.BackgroundImage =
                    btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmLinkRelatedCustomer: " + ex.Message);
            }
            log.LogMethodExit();
        }

    }
}
