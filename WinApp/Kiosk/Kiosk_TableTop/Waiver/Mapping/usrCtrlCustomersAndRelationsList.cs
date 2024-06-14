/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - Waiver Mapping
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.151.2     29-Dec-2023      Sathyavathi        Created for Waiver Mapping Enhancement
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Waiver;

namespace Parafait_Kiosk
{
    public partial class UsrCtrlCustomersAndRelationsList : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Semnox.Parafait.Transaction.Transaction.TransactionLine trxLineinProgress;
        private List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList;
        private CustomerDTO attendeeSelectedForMapping;
        private CustomerDTO customerDTO;
        private int relationshipTypeId;
        private DateTime trxDate;
        private const string PENDING = "PENDING";
        private const string SIGNED = "SIGNED";
        private string DATEFORMAT;
        private bool isSelected = false;
        private int groupOwner = -1;
        internal delegate void SelectedDelegate(UsrCtrlCustomersAndRelationsList usrCtrlCustomerRelations);
        internal SelectedDelegate selctedAttendee;

        internal bool IsSelected { get { return isSelected; } set { isSelected = value; UpdateCheckboxImage(); } }
        internal int GroupOwner { get { return groupOwner; } }

        public UsrCtrlCustomersAndRelationsList(CustomerDTO customerDTO, List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList,
            int relationshipTypeId, Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine, DateTime trxDate, int groupOwner)
        {
            log.LogMethodEntry("kioskTransaction", customerDTO, customerRelationshipTypeDTOList, relationshipTypeId, trxLine, trxDate);
            KioskStatic.logToFile("In UsrCtrlCustomersAndRelationsList class");
            InitializeComponent();
            this.customerDTO = customerDTO;
            this.customerRelationshipTypeDTOList = customerRelationshipTypeDTOList;
            this.relationshipTypeId = relationshipTypeId;
            this.trxLineinProgress = trxLine;
            this.trxDate = trxDate;
            this.groupOwner = groupOwner;
            //this.relatedCustomerId = customerRelationshipDTO.RelatedCustomerId;
            DATEFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "DATE_FORMAT");
            //LoadRelationShipTypes();
            SetDisplayElements();
            isSelected = false;
            log.LogMethodExit();
        }

        public void usrControl_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (isSelected == false)
                {
                    if (selctedAttendee != null)
                    {
                        selctedAttendee(this);
                        isSelected = true;
                        UpdateCheckboxImage();
                    }
                }
                else
                {
                    if (selctedAttendee != null)
                    {
                        selctedAttendee(this);
                        isSelected = false;
                        UpdateCheckboxImage();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in usrControl_Click() of UsrCtrlCustomersAndRelationsList : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void UpdateCheckboxImage()
        {
            this.pbxSelectd.BackgroundImage = (isSelected == true) ? Parafait_Kiosk.Properties.Resources.NewTickedCheckBox
                : Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
        }

        private void SetOnScreenMessages()
        {
            log.LogMethodEntry();
            try
            {
                CustomerBL relatedCustomerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, customerDTO);
                this.attendeeSelectedForMapping = relatedCustomerBL.CustomerDTO;
                lblRelatedCustomerName.Text = relatedCustomerBL.CustomerDTO.FirstName;
                if (!string.IsNullOrWhiteSpace(relatedCustomerBL.CustomerDTO.LastName))
                {
                    lblRelatedCustomerName.Text += " " + relatedCustomerBL.CustomerDTO.LastName;
                }

                string signingStatus = PENDING;
                if (trxLineinProgress.WaiverSignedDTOList != null && trxLineinProgress.WaiverSignedDTOList.Any())
                {
                    foreach (WaiverSignatureDTO waiverSignedDTO in trxLineinProgress.WaiverSignedDTOList)
                    {
                        if (waiverSignedDTO != null)
                        {
                            int waiverSetDetailId = waiverSignedDTO.WaiverSetDetailId;
                            if (relatedCustomerBL.HasSigned(waiverSetDetailId, trxDate))
                            {
                                signingStatus = SIGNED;
                                log.LogVariableState("Customer Name", relatedCustomerBL.CustomerDTO.FirstName);
                                log.LogVariableState("Signed Status", signingStatus);
                                List<CustomerSignedWaiverDTO> waiversSignedByCustomer = relatedCustomerBL.GetWaiversSignedByCustomer();
                                if (waiversSignedByCustomer != null && waiversSignedByCustomer.Any())
                                {
                                    CustomerSignedWaiverDTO waiver = waiversSignedByCustomer.Where(w => w.WaiverSetId == waiverSignedDTO.WaiverSetId).FirstOrDefault();
                                    if (waiver != null)
                                    {
                                        DateTime validityDate = (waiver.ExpiryDate == null) ? DateTime.MinValue : (DateTime)waiver.ExpiryDate;
                                        lblValidity.Text = (validityDate == DateTime.MinValue) ? string.Empty : validityDate.ToString(DATEFORMAT);
                                        log.LogVariableState("Waiver Name", waiver.WaiverName);
                                        log.LogVariableState("Waiver Name", waiver.WaiverName);
                                        log.LogVariableState("Validity Date", validityDate);
                                        KioskStatic.logToFile("Customer Name: " + relatedCustomerBL.CustomerDTO.FirstName
                                            + ", Waiver Name: " + waiver.WaiverName
                                            + ", Signed Status: " + signingStatus
                                            + ", Validity Date: " + validityDate);
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                lblSignStatus.Text = signingStatus;

                string relationShipName = string.Empty;
                if (relationshipTypeId == -1)
                {
                    relationShipName = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "You"); //Literal
                }
                else
                {
                    if (customerRelationshipTypeDTOList != null && customerRelationshipTypeDTOList.Any())
                    {
                        relationShipName = (customerRelationshipTypeDTOList.Find(crt => crt.Id == relationshipTypeId) != null ?
                            customerRelationshipTypeDTOList.Find(crt => crt.Id == relationshipTypeId).Description : string.Empty);
                    }
                }

                lblRelationship.Text = relationShipName;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in usrControl_Click() of UsrCtrlCustomersAndRelationsList : " + ex.Message);
            }
            log.LogMethodExit();
        }

        //private void LoadRelationShipTypes()
        //{
        //    log.LogMethodEntry();
        //    CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(KioskStatic.Utilities.ExecutionContext);
        //    List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
        //    searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, KioskStatic.Utilities.ExecutionContext.SiteId.ToString()));
        //    searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
        //    customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchParameters);
        //    if (customerRelationshipTypeDTOList == null || customerRelationshipTypeDTOList.Count == 0)
        //    {
        //        customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();
        //    }
        //    customerRelationshipTypeDTOList.Insert(0, new CustomerRelationshipTypeDTO(-1, string.Empty, string.Empty, true));
        //    log.LogMethodExit(customerRelationshipTypeDTOList);
        //}

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            try
            {
                SetOnScreenMessages();
                SetCustomImages();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetDisplayElements of usrCtrlCustomer" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
            this.pbxSelectd.BackgroundImage = Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
            pBInfo.BackgroundImage = ThemeManager.CurrentThemeImages.ComboInformationIcon;
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            lblRelatedCustomerName.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsRelatedCustomerNameTextForeColor;
            lblRelationship.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsRelationshipTextForeColor;
            lblSignStatus.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsSignStatusTextForeColor;
            lblValidity.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsValidityTextForeColor;
            log.LogMethodExit();
        }
    }
}
