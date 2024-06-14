/********************************************************************************************
 * Project Name - Portait Kiosk
 * Description  - frmCustomerDetailsForWaiver UI form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.2      29-Nov-2019      Guru S A               Created for Waiver phase 2 enhancement changes  
 *2.100       19-Oct-2020      Guru S A               Enabling minor signature option for waiver
 *2.120       18-May-2021      Dakshakh Raj           Handling text box fore color changes.
*2.130.0     09-Jul-2021      Dakshak                 Theme changes to support customized Font ForeColor
*2.150.0.0   21-Jun-2022      Vignesh Bhat            Back and Cancel button changes
*2.150.1     22-Feb-2023      Vignesh Bhat            Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace Parafait_Kiosk
{
    public partial class frmCustomerDetailsForWaiver : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private CustomerDTO signatoryCustomerDTO;
        private CustomerBL signatoryCustomerBL;
        private WaiverSetDTO waiverSetDTO;
        private List<CustomerDTO> signForcustomerDTOList;
        private List<CustomerRelationshipDTO> relatedCustomerDTOList;
        private List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList;
        private string defaultMsg;
        private List<int> userSelectedCustomerIdList;
        private const string RELATIONSHIPTYPEISCHILD = "CHILD";
        private bool ignoreEventCode = false;
        private string eventCode;
        private List<WaiverCustomerAndSignatureDTO> selectedWaiverAndCustomerSignatureList;
        private List<WaiverCustomerAndSignatureDTO> deviceSignatureList;
        private List<WaiverCustomerAndSignatureDTO> manualSignatureList;
        private int guestCustomerId;
        private LookupValuesList serverTimeObj;
        private string SIGNATURECHANNEL = WaiverSignatureDTO.WaiverSignatureChannel.KIOSK.ToString();
        private ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();
        public CustomerDTO GetSignatoryCustomerDTO { get { return signatoryCustomerDTO; } }


        //public List<CustomerDTO> GetSignForCustomerDTOList { get { return signForcustomerDTOList; } }
        public frmCustomerDetailsForWaiver(CustomerDTO customerDTO, WaiverSetDTO waiverSetDTO, string eventCode)
        {
            log.LogMethodEntry(customerDTO, eventCode);
            this.utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CustomerDetailsForWaiverBackgroundImage);
            //DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            KioskStatic.setDefaultFont(this);
            foreach (Control c in panel1.Controls)
            {
                c.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            this.waiverSetDTO = waiverSetDTO;
            this.signatoryCustomerDTO = customerDTO;
            this.signatoryCustomerBL = new CustomerBL(utilities.ExecutionContext, signatoryCustomerDTO);
            this.eventCode = eventCode;
            guestCustomerId = CustomerListBL.GetGuestCustomerId(utilities.ExecutionContext);
            serverTimeObj = new LookupValuesList(utilities.ExecutionContext);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.logToFile("Loading Customer details for waiver form");
            SetTextBoxFontColors();
            SetFont();
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmSelectWaiverOption_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadRelationShipTypes();
            DisplayWaiverSetDescriptiion();
            DisplayCustomerDetails();
            defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2426);
            //Please select customer record(s) for signing waiver
            DisplayMessageLine(defaultMsg);
            ResetKioskTimer();
            SetCustomizedFontColors();

            log.LogMethodExit();
        }
        private void LoadRelationShipTypes()
        {
            log.LogMethodEntry();
            CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(utilities.ExecutionContext);
            List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.SiteId.ToString()));
            searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchParameters);
            if (customerRelationshipTypeDTOList == null || customerRelationshipTypeDTOList.Count == 0)
            {
                customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();
            }
            customerRelationshipTypeDTOList.Insert(0, new CustomerRelationshipTypeDTO(-1, string.Empty, string.Empty, true));
            log.LogMethodExit(customerRelationshipTypeDTOList);
        }
        private void DisplayWaiverSetDescriptiion()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Display Waiver set description");
            ResetKioskTimer();
            if (this.waiverSetDTO != null)
            {
                lblWaiverSet.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Waiver Set") + ": "
                                           + this.waiverSetDTO.Description;
            }
            else
            {
                lblWaiverSet.Text = string.Empty;
            }
            log.LogMethodExit();
        }
        private void DisplayCustomerDetails()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Display Customer details");
            ResetKioskTimer();
            if (this.signatoryCustomerDTO != null && this.signatoryCustomerDTO.Id > -1)
            {
                lblSignatoryCustomerName.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer") + ": "
                                           + (string.IsNullOrEmpty(this.signatoryCustomerDTO.FirstName) ? "" : this.signatoryCustomerDTO.FirstName)
                                           + " " + (string.IsNullOrEmpty(this.signatoryCustomerDTO.LastName) ? "" : this.signatoryCustomerDTO.LastName);
            }
            else
            {
                lblSignatoryCustomerName.Text = string.Empty;
            }
            GetRelatedCustomers();
            RefreshDGVCustomers();
            log.LogMethodExit();
        }


        private void GetRelatedCustomers()
        {
            log.LogMethodEntry();
            CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(utilities.ExecutionContext);
            List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
            //searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, signatoryCustomerDTO.Id.ToString()));
            searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
            relatedCustomerDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchParameters);
            //if (relatedCustomerDTOList != null)
            //{
            //    for (int i = 0; i < relatedCustomerDTOList.Count; i++)
            //    {
            //        if (relatedCustomerDTOList[i].RelatedCustomerId == signatoryCustomerDTO.Id)
            //        {
            //            relatedCustomerDTOList.RemoveAt(i);
            //            i = i - 1;
            //        }
            //    }
            //}
            log.LogMethodExit();
        }


        private void RefreshDGVCustomers()
        {
            log.LogMethodEntry();
            try
            {
                List<CustomerDTO> customerDTOList = PrepareCustomerDTOList();
                if (customerDTOList != null && customerDTOList.Any() && customerDTOList[0].Id != signatoryCustomerDTO.Id)
                {
                    for (int i = 0; i < customerDTOList.Count; i++)
                    {
                        if (customerDTOList[i].Id == signatoryCustomerDTO.Id)
                        {
                            List<CustomerDTO> customerSignedByDTOList = new List<CustomerDTO>();
                            customerSignedByDTOList.Add(customerDTOList[i]);
                            customerDTOList.RemoveAt(i);
                            customerDTOList.Insert(0, customerSignedByDTOList[0]);
                            break;
                        }
                    }

                }
                LookupValuesList serverTimeObj = new LookupValuesList(utilities.ExecutionContext);
                customerName.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                this.customerRelationType.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                DateTime serverTime = serverTimeObj.GetServerDateTime();
                dgvCustomer.Rows.Clear();
                for (int i = 0; i < customerDTOList.Count; i++)
                {
                    string customerNameDisplay = (customerDTOList[i].FirstName + " " + (string.IsNullOrEmpty(customerDTOList[i].LastName) ? "" : customerDTOList[i].LastName));
                    int relationshipTypeId = (relatedCustomerDTOList != null ? ((relatedCustomerDTOList.Find(rCust => rCust.RelatedCustomerId == customerDTOList[i].Id) != null ? relatedCustomerDTOList.Find(rCust => rCust.RelatedCustomerId == customerDTOList[i].Id).CustomerRelationshipTypeId : -1)) : -1);
                    string relationShipName = string.Empty;
                    if (customerRelationshipTypeDTOList != null && customerRelationshipTypeDTOList.Any())
                    {
                        relationShipName = (customerRelationshipTypeDTOList.Find(crt => crt.Id == relationshipTypeId) != null ? customerRelationshipTypeDTOList.Find(crt => crt.Id == relationshipTypeId).Description : string.Empty);
                    }
                    dgvCustomer.Rows.Add();
                    CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDTOList[i]);
                    dgvCustomer.Rows[i].Cells["customerName"].Value = customerNameDisplay;
                    dgvCustomer.Rows[i].Cells["customerName"].ToolTipText = customerNameDisplay;
                    dgvCustomer.Rows[i].Cells["customerRelationType"].Value = relationShipName;
                    dgvCustomer.Rows[i].Cells["signFor"].Tag = false;
                    dgvCustomer.Rows[i].Cells["signFor"].Value = Properties.Resources.NewUnTickedCheckBox;
                    dgvCustomer.Rows[i].Cells["hasSignedWaiverSet"].Value = Properties.Resources.NewRedCross;
                    if (waiverSetDTO != null && waiverSetDTO.WaiverSetDetailDTOList != null && waiverSetDTO.WaiverSetDetailDTOList.Any())
                    {
                        if (customerBL.HasSigned(waiverSetDTO.WaiverSetDetailDTOList, serverTime))
                        {
                            dgvCustomer.Rows[i].Cells["hasSignedWaiverSet"].Value = Properties.Resources.NewGreenTick;
                        }
                    }
                    dgvCustomer.Rows[i].Cells["RelationshipTypeId"].Value = relationshipTypeId;
                    dgvCustomer.Rows[i].Tag = customerBL;
                }
                this.signFor.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.signFor.ImageLayout = DataGridViewImageCellLayout.Normal;
                this.hasSignedWaiverSet.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.hasSignedWaiverSet.ImageLayout = DataGridViewImageCellLayout.Normal;
                dgvCustomer.EndEdit();
                if (dgvCustomer.Rows.Count > 0)
                {
                    dgvCustomer.Rows[0].Cells["customerName"].Selected = true;
                }
                bigVerticalScrollCustomer.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }


        private List<CustomerDTO> PrepareCustomerDTOList()
        {
            log.LogMethodEntry();
            List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
            List<CustomerDTO> relatedCustDTOList = null;
            List<int> customerIdList = new List<int>();
            //customerDTOList.Add(this.signedBycustomerDTO);
            customerIdList.Add(this.signatoryCustomerDTO.Id);
            if (relatedCustomerDTOList != null && relatedCustomerDTOList.Any())
            {
                List<int> relCustomerIdList = relatedCustomerDTOList.Select(rcust => rcust.RelatedCustomerId).ToList();
                if (relatedCustomerDTOList[0].CustomerId != signatoryCustomerDTO.Id && customerIdList.Exists(custId => custId == relatedCustomerDTOList[0].CustomerId) == false)
                {
                    relCustomerIdList.Add(relatedCustomerDTOList[0].CustomerId);
                }
                customerIdList.AddRange(relCustomerIdList);
            }
            customerIdList = customerIdList.Distinct().ToList();
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            relatedCustDTOList = customerListBL.GetCustomerDTOList(customerIdList, true, true, true);
            if (relatedCustDTOList != null && relatedCustDTOList.Any())
            {
                customerDTOList.AddRange(relatedCustDTOList);
            }
            log.LogMethodExit();
            return customerDTOList;
        }


        void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            ResetKioskTimer();
            log.LogMethodExit();
        } 

        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void vScrollBarGp_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvCustomer.Rows.Count > 0)
            {
                dgvCustomer.FirstDisplayedScrollingRowIndex = e.NewValue;
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void dgvWaiverSet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                DisplayMessageLine(defaultMsg);
                DataGridViewRow dgvRow = dgvCustomer.CurrentRow;
                if (dgvRow != null && dgvRow.Cells["signFor"].Selected)
                {
                    if (this.dgvCustomer.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag == null)
                    {
                        this.dgvCustomer.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = false;
                        this.dgvCustomer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.NewUnTickedCheckBox;
                    }
                    if (this.dgvCustomer.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.Equals(false))
                    {
                        this.signatoryCustomerBL = new CustomerBL(utilities.ExecutionContext, signatoryCustomerDTO);
                        ValidateAndSetSignForFlag(e.RowIndex);
                        this.dgvCustomer.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = true;
                        this.dgvCustomer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.NewTickedCheckBox;
                    }
                    else
                    {
                        this.dgvCustomer.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = false;
                        this.dgvCustomer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.NewUnTickedCheckBox;
                    }
                }
                ResetKioskTimer();
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                KioskStatic.logToFile("Timeout occured");
                log.Error(ex); 
                this.DialogResult = DialogResult.Cancel;
                log.LogMethodExit();
                return;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("dgvWaiverSet_CellContentClick: " + ex.Message);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        private void ValidateAndSetSignForFlag(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            ResetKioskTimer();
            int relationShipTypeId = Convert.ToInt32(dgvCustomer["RelationshipTypeId", rowIndex].Value);
            log.LogVariableState("relationShipTypeId", relationShipTypeId);
            CustomerBL customerBL = (CustomerBL)dgvCustomer.Rows[rowIndex].Tag;

            if ((customerRelationshipTypeDTOList.Exists(rel => rel.Id == relationShipTypeId && rel.Name == RELATIONSHIPTYPEISCHILD)
                && customerBL.CustomerDTO.Id != this.signatoryCustomerDTO.Id)
                || customerBL.CustomerDTO.Id == this.signatoryCustomerDTO.Id)
            {
                if (customerBL.HasValidDateOfBirth() == false && customerBL.CustomerDTO.DateOfBirth != null && customerBL.CustomerDTO.DateOfBirth != DateTime.MinValue)
                {
                    customerBL = VerifyAndUpdateDOB(customerBL);
                    dgvCustomer.Rows[rowIndex].Tag = customerBL;
                    dgvCustomer.Rows[rowIndex].Cells["customerName"].Value = customerBL.CustomerDTO.FirstName + " " + (String.IsNullOrEmpty(customerBL.CustomerDTO.LastName) ? "" : customerBL.CustomerDTO.LastName);
                    if (customerBL.CustomerDTO.Id == this.signatoryCustomerDTO.Id)
                    {
                        this.signatoryCustomerDTO = customerBL.CustomerDTO;
                    }
                }

                if (customerBL.HasValidDateOfBirth() == true || (customerBL.CustomerDTO.DateOfBirth == null) || (customerBL.CustomerDTO.DateOfBirth == DateTime.MinValue))
                {
                    if (customerBL.IsAdult())
                    {
                        if (customerBL.CustomerDTO.Id != this.signatoryCustomerDTO.Id)
                        {

                            //Parent/Guardians cannot sign for adult dependent
                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2472, customerBL.CustomerDTO.FirstName + " " + customerBL.CustomerDTO.LastName));

                        }
                    }
                    else
                    {
                        if (customerBL.CustomerDTO.Id == this.signatoryCustomerDTO.Id)
                        {
                            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "WHO_CAN_SIGN_FOR_MINOR")
                                   != WaiverCustomerAndSignatureDTO.WhoCanSignForMinor.MINOR.ToString())
                            {
                                //Customer is minor, parent / guardian needs to sign
                                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2471, customerBL.CustomerDTO.FirstName + " " + customerBL.CustomerDTO.LastName));
                            }
                        }
                        else
                        {
                            if (this.signatoryCustomerBL.IsAdult() == false)
                            {
                                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2849));
                                //Sorry, minor customer cannot sign for others
                            }
                            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "WHO_CAN_SIGN_FOR_MINOR")
                               == WaiverCustomerAndSignatureDTO.WhoCanSignForMinor.MINOR.ToString())
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2850));
                                //'Sorry, minors are allowed to sign of themselves's
                            }
                        }
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2300));
                }
            }
            else
            {
                if (this.signatoryCustomerBL.IsAdult() == false)
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2849));
                    //Sorry, minor customer cannot sign for others
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2440));
                    //You can sign for minor dependent child only
                }
            }
            log.LogMethodExit();
        }

        private CustomerBL VerifyAndUpdateDOB(CustomerBL customerBL)
        {
            log.LogMethodEntry(customerBL);
            if (customerBL.HasValidDateOfBirth() == false && customerBL.CustomerDTO.DateOfBirth != null && customerBL.CustomerDTO.DateOfBirth != DateTime.MinValue)
            {
                using (frmYesNo frmYes = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 5335)))
                {
                    if (frmYes.ShowDialog() == DialogResult.Yes)
                    {
                        //Invalid date of birth. Do you want to update the same now?
                        using (Customer fcustomer = new Customer(string.Empty, null, true, customerBL.CustomerDTO))
                        {
                            DialogResult dr = fcustomer.ShowDialog();
                            if (dr == DialogResult.Cancel)
                            {
                                string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, "Timeout");
                                throw new CustomerStatic.TimeoutOccurred(msg);
                            }
                            CustomerDTO customerDTO = fcustomer.customerDTO;
                            customerBL = new CustomerBL(utilities.ExecutionContext, customerBL.CustomerDTO.Id);
                        }
                    }
                }
            }
            log.LogMethodExit();
            return customerBL;
        }



        private void frmSelectWaiverOption_Closing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Closing select waiver option form");
            log.LogMethodExit();
        }

        private void btnAddNewRelations_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                ResetKioskTimer();
                KioskStatic.logToFile("Add new relations option selected");
                txtMessage.Text = defaultMsg;
                CustomerDTO newRelatedCustomerDTO = RegisterCustomer();
                if (newRelatedCustomerDTO != null && newRelatedCustomerDTO.Id > -1)
                {
                    KioskStatic.logToFile("Received new related Customer details");
                    log.Info("Received new related Customer details");
                    SetRelationType(newRelatedCustomerDTO);
                }
                else
                {
                    KioskStatic.logToFile("New related Customer details is not received");
                    log.Info("New related Customer details is not received");
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
                txtMessage.Text = ex.Message;
            }
            finally
            {
                ResetKioskTimer();
                EnableButtons();
            }
            log.LogMethodExit();
        }


        private CustomerDTO RegisterCustomer()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            CustomerDTO newRelatedCustomerDTO = null;
            try
            {
                string tappedCardNumber = "";
                if (KioskStatic.AllowRegisterWithoutCard)
                {
                    KioskStatic.logToFile("Allow Register Without Card");
                    log.Info("Allow Registe Without Card");
                    using (frmTapCard ftc = new frmTapCard(utilities.MessageUtils.getMessage(496), utilities.MessageUtils.getMessage("Yes")))
                    {
                        DialogResult dr = ftc.ShowDialog();
                        if (dr == System.Windows.Forms.DialogResult.OK)
                        {
                            tappedCardNumber = null;
                        }
                        else if (ftc.Card == null)
                        {
                            ftc.Dispose();
                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 748));//Card details are not available. Please tap the Card

                        }
                        else
                        {
                            tappedCardNumber = ftc.cardNumber;
                        }
                        ftc.Dispose();
                    }
                }
                else
                {
                    KioskStatic.logToFile("Needs card for Registration");
                    log.Info("Needs card for Registration");
                    using (frmTapCard ftc = new frmTapCard(utilities.MessageUtils.getMessage(500)))
                    {
                        ftc.ShowDialog();
                        if (ftc.Card == null)
                        {
                            ftc.Dispose();
                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 748));
                            //Card details are not available. Please tap the Card

                        }

                        tappedCardNumber = ftc.cardNumber;
                        log.LogVariableState("CardNumber", tappedCardNumber);
                        ftc.Dispose();
                    }
                }
                if (!String.IsNullOrEmpty(tappedCardNumber))
                {
                    Card custRegisterCard = new Card(tappedCardNumber, "", KioskStatic.Utilities);
                    if (custRegisterCard.technician_card.Equals('Y'))
                    {
                        txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(197, custRegisterCard.CardNumber);
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, txtMessage.Text));

                    }
                    if (custRegisterCard.customer_id > -1)
                    {
                        txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage("&1 is already linked to a customer", custRegisterCard.CardNumber);
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, txtMessage.Text));

                    }
                }
                newRelatedCustomerDTO = CustomerStatic.ShowCustomerScreen(tappedCardNumber);

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                this.Activate();
                log.LogVariableState("newRelatedCustomerDTO", newRelatedCustomerDTO);
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Timeout occured");
                this.DialogResult = DialogResult.Cancel; 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                ResetKioskTimer();
                KioskStatic.logToFile("exit RegisterCustomer()");
            }
            log.LogMethodExit(newRelatedCustomerDTO);
            return newRelatedCustomerDTO;
        }
        private void SetRelationType(CustomerDTO newRelatedCustomerDTO)
        {
            log.LogMethodEntry(newRelatedCustomerDTO);
            ResetKioskTimer();
            if (newRelatedCustomerDTO != null)
            {
                using (frmLinkRelatedCustomer frmLinkCust = new frmLinkRelatedCustomer(signatoryCustomerDTO, newRelatedCustomerDTO))
                {
                    frmLinkCust.ShowDialog();
                    GetUserSelectionInfo();
                    GetRelatedCustomers();
                    RefreshDGVCustomers();
                    RestoreUserSelectionInfo();
                }
            }
            log.LogMethodExit();
        }

        private void GetUserSelectionInfo()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            userSelectedCustomerIdList = new List<int>();
            for (int i = 0; i < dgvCustomer.Rows.Count; i++)
            {
                if (this.dgvCustomer.Rows[i].Cells["signFor"].Tag.Equals(true) && dgvCustomer.Rows[i].Tag != null)
                {
                    CustomerBL customerBL = (CustomerBL)dgvCustomer.Rows[i].Tag;
                    if (customerBL != null && customerBL.CustomerDTO != null)
                    {
                        userSelectedCustomerIdList.Add(customerBL.CustomerDTO.Id);
                    }
                }
            }
            log.LogVariableState("userSelectedCustomerIdList", userSelectedCustomerIdList);
            log.LogMethodExit();
        }


        private void GetSignForCustomerList()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            signForcustomerDTOList = new List<CustomerDTO>();
            for (int i = 0; i < dgvCustomer.Rows.Count; i++)
            {
                if (this.dgvCustomer.Rows[i].Cells["signFor"].Tag.Equals(true) && dgvCustomer.Rows[i].Tag != null)
                {
                    CustomerBL customerBL = (CustomerBL)dgvCustomer.Rows[i].Tag;
                    if (customerBL != null && customerBL.CustomerDTO != null)
                    {
                        signForcustomerDTOList.Add(customerBL.CustomerDTO);
                    }
                }
            }
            log.LogVariableState("signForcustomerDTOList", signForcustomerDTOList);
            log.LogMethodExit();
        }
        private void RestoreUserSelectionInfo()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (userSelectedCustomerIdList != null && userSelectedCustomerIdList.Any())
            {
                for (int i = 0; i < dgvCustomer.Rows.Count; i++)
                {
                    if (this.dgvCustomer.Rows[i].Cells["signFor"].Tag.Equals(false) && dgvCustomer.Rows[i].Tag != null)
                    {
                        CustomerBL customerBL = (CustomerBL)dgvCustomer.Rows[i].Tag;
                        if (customerBL != null && customerBL.CustomerDTO != null
                            && userSelectedCustomerIdList.Exists(custId => custId == customerBL.CustomerDTO.Id))
                        {
                            this.dgvCustomer.Rows[i].Cells["signFor"].Tag = true;
                            this.dgvCustomer.Rows[i].Cells["signFor"].Value = Properties.Resources.NewTickedCheckBox;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            KioskStatic.logToFile("user clicked proceed button");
            try
            {
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
                DisableButtons();
                GetSignForCustomerList();
                if (signForcustomerDTOList == null || signForcustomerDTOList.Any() == false)
                {
                    throw new ValidationException(defaultMsg);
                }
                else
                {
                    KioskStatic.logToFile("User has selected customers for waiver signing");
                    log.Info("User has selected customers for waiver signing");
                    SigneWaivers();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile(ex.Message);
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message, true))
                {
                    frmOK.ShowDialog();
                }
            }
            finally
            {
                DisplayMessageLine(defaultMsg);
                EnableButtons();
                this.Cursor = Cursors.Default;
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void SigneWaivers()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ResetKioskTimer();
                if (signForcustomerDTOList == null || signForcustomerDTOList.Any() == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2426));
                    //'Please select customer record(s) for signing waiver'
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.ignoreEventCode = false;
                    if (string.IsNullOrEmpty(this.eventCode) == false
                        && ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "CHECK_WAIVER_REGISTRATION_COUNT_FOR_TRANSACTION", false))
                    {
                        TransactionUtils trxUtils = new TransactionUtils(utilities);
                        if (trxUtils.CanAddToEventCode(this.eventCode, signForcustomerDTOList) == false)
                        {
                            string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2428, this.eventCode);
                            //Sorry, cannot link any new signed waivers to the event code &1. Do you want to sign waivers without the event code?
                            using (frmYesNo frmYes = new frmYesNo(msg))
                            {
                                this.Cursor = Cursors.WaitCursor;
                                if (frmYes.ShowDialog() == DialogResult.Yes)
                                {
                                    this.Cursor = Cursors.WaitCursor;
                                    this.ignoreEventCode = true;
                                }
                                else
                                {
                                    this.Cursor = Cursors.WaitCursor;
                                    KioskStatic.logToFile("Customer decided not to ignore event code for waiver signning");
                                    log.Info("Customer decided not to ignore event code for waiver signning");
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                        }
                        this.Cursor = Cursors.WaitCursor;
                    }
                    CreateWaiverCustomerMapList(waiverSetDTO);
                    GetSignatureAndSave(waiverSetDTO);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        private void CreateWaiverCustomerMapList(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            this.Cursor = Cursors.WaitCursor;
            ResetKioskTimer();
            //managerApprovalList = new List<KeyValuePair<int, int>>();
            CreateWaiverCustomerAndSignatureDTOList(waiverSetDTO);
            UpdateListBasedOnCustomerSignatureStatus(waiverSetDTO);
            log.LogMethodExit();
        }

        private void CreateWaiverCustomerAndSignatureDTOList(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            selectedWaiverAndCustomerSignatureList = new List<WaiverCustomerAndSignatureDTO>();
            foreach (WaiversDTO waiversDTO in waiverSetDTO.WaiverSetDetailDTOList)
            {
                WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO = new WaiverCustomerAndSignatureDTO();
                waiverCustomerAndSignatureDTO.WaiversDTO = waiversDTO;
                waiverCustomerAndSignatureDTO.SignatoryCustomerDTO = signatoryCustomerDTO;
                waiverCustomerAndSignatureDTO.SignForCustomerDTOList = new List<CustomerDTO>(signForcustomerDTOList);
                waiverCustomerAndSignatureDTO.CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                waiverCustomerAndSignatureDTO.CustomerContentDTOList = null;
                selectedWaiverAndCustomerSignatureList.Add(waiverCustomerAndSignatureDTO);
            }
            log.LogMethodExit();
        }

        private void UpdateListBasedOnCustomerSignatureStatus(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            foreach (CustomerDTO selectedCustomer in signForcustomerDTOList)
            {
                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, selectedCustomer);
                if (customerBL.CustomerDTO != null && customerBL.CustomerDTO.Id != guestCustomerId)
                {

                    List<WaiversDTO> pendingWaiversList = customerBL.GetSignaturePendingWaivers(waiverSetDTO.WaiverSetDetailDTOList, serverTimeObj.GetServerDateTime());
                    if (pendingWaiversList == null || pendingWaiversList.Count != waiverSetDTO.WaiverSetDetailDTOList.Count)
                    {
                        List<WaiversDTO> signedWaivers = new List<WaiversDTO>();
                        if (pendingWaiversList == null || pendingWaiversList.Any() == false)
                        {
                            signedWaivers = new List<WaiversDTO>(waiverSetDTO.WaiverSetDetailDTOList);
                        }
                        else
                        {
                            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
                            {
                                if (pendingWaiversList.Exists(waiver => waiver.WaiverSetDetailId == selectedWaiverAndCustomerSignatureList[i].WaiversDTO.WaiverSetDetailId) == false)
                                {
                                    signedWaivers.Add(selectedWaiverAndCustomerSignatureList[i].WaiversDTO);
                                }
                            }
                        }
                        if (signedWaivers != null && signedWaivers.Any())
                        {
                            CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(utilities.ExecutionContext);
                            List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParameters = new List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>>();
                            searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_FOR, selectedCustomer.Id.ToString()));
                            //searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                            searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IS_ACTIVE, "1"));
                            searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IGNORE_EXPIRED, serverTimeObj.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(searchParameters);
                            if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                            {
                                for (int i = 0; i < signedWaivers.Count; i++)
                                {
                                    List<CustomerSignedWaiverDTO> signedWaiverCopyList = customerSignedWaiverDTOList.Where(cw => cw.WaiverSetDetailId == signedWaivers[i].WaiverSetDetailId).ToList();
                                    if (signedWaiverCopyList != null && signedWaiverCopyList.Any())
                                    {
                                        foreach (CustomerSignedWaiverDTO item in signedWaiverCopyList)
                                        {
                                            //CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(utilities.ExecutionContext, item);
                                            if (CanDeactivate(item) == false)
                                            {
                                                log.Info("remove entry from signedWaivers: " + signedWaivers[i].WaiverSetDetailId);
                                                //messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 2351, MessageContainerList.GetMessage(utilities.ExecutionContext, "resign"), signedWaivers[i].Name), MessageContainerList.GetMessage(utilities.ExecutionContext, "Sign"));
                                                this.Cursor = Cursors.WaitCursor;
                                                using (frmOKMsg frmOk = new frmOKMsg(MessageContainerList.GetMessage(utilities.ExecutionContext, 2351, MessageContainerList.GetMessage(utilities.ExecutionContext, "re-sign"), signedWaivers[i].Name)))
                                                {
                                                    frmOk.ShowDialog();
                                                }
                                                this.Cursor = Cursors.WaitCursor;
                                                signedWaivers.RemoveAt(i);
                                                i = i - 1;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        this.Cursor = Cursors.WaitCursor;
                        using (frmYesNo frmYes = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2485, customerBL.CustomerDTO.FirstName + " " + (string.IsNullOrEmpty(customerBL.CustomerDTO.LastName) ? string.Empty : customerBL.CustomerDTO.LastName))))
                        {
                            this.Cursor = Cursors.WaitCursor;
                            //"&1 has signed one or more selected waivers. Do you want to re-sign them?"
                            if ((signedWaivers != null && signedWaivers.Any())
                                && frmYes.ShowDialog() == DialogResult.Yes)
                            {
                                this.Cursor = Cursors.WaitCursor;
                                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "WAIVER_DEACTIVATION_NEEDS_MANAGER_APPROVAL", true))
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "Manager Approval Required.")
                                                                  + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, 441));
                                }
                            }
                            else
                            {
                                this.Cursor = Cursors.WaitCursor;
                                for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
                                {
                                    if (pendingWaiversList.Exists(waiver => waiver.WaiverSetDetailId == selectedWaiverAndCustomerSignatureList[i].WaiversDTO.WaiverSetDetailId) == false)
                                    {
                                        selectedWaiverAndCustomerSignatureList[i].SignForCustomerDTOList.Remove(selectedCustomer);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.Cursor = Cursors.WaitCursor;
            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
            {
                if (selectedWaiverAndCustomerSignatureList[i].SignForCustomerDTOList == null || selectedWaiverAndCustomerSignatureList[i].SignForCustomerDTOList.Any() == false)
                {
                    selectedWaiverAndCustomerSignatureList.RemoveAt(i);
                    i = i - 1;
                }
            }
            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
            {
                selectedWaiverAndCustomerSignatureList[i] = WaiverCustomerAndSignatureBL.CreateCustomerContentForWaiver(utilities.ExecutionContext, selectedWaiverAndCustomerSignatureList[i]);
            }
            log.LogMethodExit();
        }


        private void GetSignatureAndSave(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            SplitBySigningOption(waiverSetDTO);
            if (manualSignatureList != null && manualSignatureList.Any())
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2345, waiverSetDTO.Description, MessageContainerList.GetMessage(utilities.ExecutionContext, "Sign")));
            }
            DeviceWaivers();
            SaveCustomerSignedWaivers();
            log.LogMethodExit();
        }


        private void SplitBySigningOption(WaiverSetDTO waiverSetDTO)
        {
            log.LogMethodEntry(waiverSetDTO);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            deviceSignatureList = null;
            manualSignatureList = null;
            bool device = false;
            bool manual = false;
            if (waiverSetDTO.WaiverSetSigningOptionDTOList != null)
            {
                foreach (WaiverSetSigningOptionsDTO item in waiverSetDTO.WaiverSetSigningOptionDTOList)
                {
                    if (item.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.DEVICE.ToString()
                        || item.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.ONLINE.ToString())
                    {
                        device = true;
                    }
                    if (item.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.MANUAL.ToString())
                    {
                        manual = true;
                    }
                }
            }
            if (device && manual || device)
            {
                deviceSignatureList = new List<WaiverCustomerAndSignatureDTO>();
                deviceSignatureList = selectedWaiverAndCustomerSignatureList;
            }
            else if (manual)
            {
                manualSignatureList = new List<WaiverCustomerAndSignatureDTO>();
                manualSignatureList = selectedWaiverAndCustomerSignatureList;
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2308, waiverSetDTO.Name));
                //Please check the waiver signing option for &1
            }
            log.LogMethodExit();
        }

        private void DeviceWaivers()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            if (deviceSignatureList != null && deviceSignatureList.Any())
            {
                this.Cursor = Cursors.WaitCursor;
                using (frmSignWaiverFiles frmSignWaiverFiles = new frmSignWaiverFiles(deviceSignatureList))
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (frmSignWaiverFiles.ShowDialog() == DialogResult.OK)
                    {
                        deviceSignatureList = frmSignWaiverFiles.GetSignedDTOList;
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1007));
                    }
                    this.Cursor = Cursors.WaitCursor;
                }
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }




        private void SaveCustomerSignedWaivers()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            StopKioskTimer();
            //ApplicationContext ap = new ApplicationContext();
            try
            {

                List<CustomerDTO> signForCustDTOList = new List<CustomerDTO>();
                List<WaiverCustomerAndSignatureDTO> receivedSignatureDetailsList = new List<WaiverCustomerAndSignatureDTO>();
                if (deviceSignatureList != null && deviceSignatureList.Any())
                {
                    receivedSignatureDetailsList.AddRange(deviceSignatureList);
                }
                if (receivedSignatureDetailsList != null && receivedSignatureDetailsList.Any())
                {
                    //KioskHelper.ShowPreloader(ap, utilities, Properties.Resources.Back_button_box, Properties.Resources.PreLoader, statusProgressMsgQueue);

                    CustomerBL signatoryCustomerBL = new CustomerBL(utilities.ExecutionContext, receivedSignatureDetailsList[0].SignatoryCustomerDTO);
                    ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    int custSignedWaiverHeaderId = -1;
                    try
                    {
                        custSignedWaiverHeaderId = signatoryCustomerBL.CreateCustomerSignedWaiverHeader(SIGNATURECHANNEL, dBTransaction.SQLTrx);
                        bool customerIsAnAdult = signatoryCustomerBL.IsAdult();
                        int guardianId = (customerIsAnAdult ? signatoryCustomerBL.CustomerDTO.Id : -1);
                        foreach (WaiverCustomerAndSignatureDTO custSignatureDetails in receivedSignatureDetailsList)
                        {
                            foreach (CustomerDTO signForCustomerDTO in custSignatureDetails.SignForCustomerDTOList)
                            {
                                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, signForCustomerDTO);
                                int mgrId = -1;
                                //if (managerApprovalList != null && managerApprovalList.Any())
                                //{
                                //    KeyValuePair<int, int> mgrIdDetails = managerApprovalList.Find(mAprv => mAprv.Key == signForCustomerDTO.Id);
                                //    mgrId = (mgrIdDetails.Value > -1 ? mgrIdDetails.Value : -1);
                                //}
                                customerBL.CreateCustomerSignedWaiver(custSignatureDetails.WaiversDTO, custSignedWaiverHeaderId, custSignatureDetails.CustomerContentDTOList, custSignatureDetails.CustIdNameSignatureImageList, mgrId, utilities, guardianId);
                                customerBL.Save(dBTransaction.SQLTrx);
                                if (signForCustDTOList.Exists(cust => cust.Id == customerBL.CustomerDTO.Id) == false)
                                {
                                    signForCustDTOList.Add(customerBL.CustomerDTO);
                                }
                            }
                        }
                        dBTransaction.EndTransaction();
                        //KioskHelper.ClosePreLoaderForm(statusProgressMsgQueue);
                        string waiverCode = signatoryCustomerBL.GetWaiverCode(custSignedWaiverHeaderId);
                        string successMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 1197,
                                                      MessageContainerList.GetMessage(utilities.ExecutionContext, "Waivers"))
                                                      + ". " + (string.IsNullOrEmpty(waiverCode) ? string.Empty : MessageContainerList.GetMessage(utilities.ExecutionContext, 2432, waiverCode));
                        using (frmOKMsg frmMsg = new frmOKMsg(successMsg))
                        {
                            frmMsg.ShowDialog();
                        }
                        this.Cursor = Cursors.WaitCursor;

                        txtMessage.Text = successMsg;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        dBTransaction.RollBack();
                        throw;
                    }
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        AddSignForCustomersAsTransactionAttendees(signForCustDTOList);
                    }
                    catch (Exception ex)
                    {

                        log.Error(ex);
                        KioskStatic.logToFile("Map waivers to event Transaction : " + ex.Message);
                        txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message);
                        using (frmOKMsg frmMsg = new frmOKMsg(ex.Message))
                        {
                            frmMsg.ShowDialog();
                        }
                        this.Cursor = Cursors.WaitCursor;
                    }
                    try
                    {

                        SendWaiverEmail(signatoryCustomerBL.CustomerDTO, custSignedWaiverHeaderId);

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        KioskStatic.logToFile("Send Waiver Email: " + ex.Message);
                        txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message);
                        using (frmOKMsg frmMsg = new frmOKMsg(ex.Message))
                        {
                            frmMsg.ShowDialog();
                        }
                        this.Cursor = Cursors.WaitCursor;
                    }
                }
            }
            finally
            {
                //int errorRecoveryCounter = 0;
                //KioskHelper.CloseAPObject(ap, errorRecoveryCounter);
                this.Cursor = Cursors.Default;
                StartKioskTimer();
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }



        private void AddSignForCustomersAsTransactionAttendees(List<CustomerDTO> signForCustDTOList)
        {
            log.LogMethodEntry(signForCustDTOList);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(this.eventCode) == false && this.ignoreEventCode == false
                && signForCustDTOList != null && signForCustDTOList.Any())
            {
                TransactionUtils trxUtils = new TransactionUtils(utilities);
                Semnox.Parafait.Transaction.Transaction eventTrx = trxUtils.GetEventCodeTransaction(eventCode);
                if (eventTrx != null)
                {
                    foreach (CustomerDTO item in signForCustDTOList)
                    {
                        eventTrx.AddUpdateCustomerAsTransactionAttendees(item);
                    }
                    eventTrx.SaveTransactionAttendees(null);
                    KioskStatic.logToFile("Adding sign for customer as transaction attendees for event: " + eventCode);
                    log.Info("Adding sign for customer as transaction attendees for event: " + eventCode);
                }
                else
                {
                    KioskStatic.logToFile("Unable to fetch transaction for event code : " + eventCode);
                    log.Info("Unable to fetch transaction for event code : " + eventCode);
                }
            }
            log.LogMethodExit();
        }

        private void SendWaiverEmail(CustomerDTO customerDTO, int custSignedWaiverHeaderId)
        {
            log.LogMethodEntry(customerDTO, custSignedWaiverHeaderId);
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            if (customerDTO != null && customerDTO.Id > -1 && customerDTO.Id != guestCustomerId)
            {
                CustomerSignedWaiverHeaderListBL customerSignedWaiverHeaderListBL = new CustomerSignedWaiverHeaderListBL(utilities.ExecutionContext);
                customerSignedWaiverHeaderListBL.SendWaiverEmail(customerDTO, custSignedWaiverHeaderId, utilities, null);
            }
            log.LogMethodExit();
        }


        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                if (signatoryCustomerDTO != null)
                {
                    //This action will clear current customer session. Do you want to proceed?
                    using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2459)))//"This action will clear current customer session. Do you want to proceed?")))
                    {
                        if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            base.btnHome_Click(sender, e);
                        }
                    }
                }
                else
                {
                    base.btnHome_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private bool CanDeactivate(CustomerSignedWaiverDTO customerSignedWaiverDTO)
        {
            log.LogMethodEntry(customerSignedWaiverDTO);
            bool canDeactivate = true;
            if (customerSignedWaiverDTO != null && customerSignedWaiverDTO.CustomerSignedWaiverId > -1)
            {
                TransactionListBL transactionListBL = new TransactionListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_SIGNED_WAIVER_ID, customerSignedWaiverDTO.CustomerSignedWaiverId.ToString()));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS_NOT_IN, Semnox.Parafait.Transaction.Transaction.TrxStatus.CANCELLED.ToString() + "," + Semnox.Parafait.Transaction.Transaction.TrxStatus.SYSTEMABANDONED.ToString()));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, serverTimeObj.GetServerDateTime().Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.SiteId.ToString()));

                List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(searchParam, utilities);
                log.LogVariableState("transactionDTOList", transactionDTOList);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    canDeactivate = false;
                }
            }
            log.LogMethodExit(canDeactivate);
            return canDeactivate;
        }
        private void SetTextBoxFontColors()
        {
            if (KioskStatic.CurrentTheme == null ||
                  (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
            {
                foreach (Control c in dgvCustomer.Controls)
                {
                    c.ForeColor = Color.Black;
                }
            }
            else
            {
                foreach (Control c in dgvCustomer.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                }
            }
        }
        private void SetFont()
        {
            try
            {
                string fontFamilyName = string.Empty;
                //LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                //List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParms = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                //searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "KIOSK_SCREEN_IMAGE"));
                //searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "KioskFontFamilyName"));
                //List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParms);
                fontFamilyName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
                Font font = new Font(fontFamilyName, 21F);
                //if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    if (string.IsNullOrWhiteSpace(fontFamilyName) == false)
                    {
                        //fontFamilyName = lookupValuesDTOList[0].Description;
                        this.dgvCustomer.Font = new System.Drawing.Font(fontFamilyName, font.Size, this.dgvCustomer.Font.Style, this.dgvCustomer.Font.Unit, this.dgvCustomer.Font.GdiCharSet);
                        this.dgvCustomer.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverDgvCustomerTextForeColor;
                        this.btnProceed.Font = new System.Drawing.Font(fontFamilyName, this.btnProceed.Font.Size, this.btnProceed.Font.Style, this.btnProceed.Font.Unit, this.btnProceed.Font.GdiCharSet);
                        this.btnAddNewRelations.Font = new System.Drawing.Font(fontFamilyName, this.btnAddNewRelations.Font.Size, this.btnAddNewRelations.Font.Style, this.btnAddNewRelations.Font.Unit, this.btnAddNewRelations.Font.GdiCharSet);
                        this.btnCancel.Font = new System.Drawing.Font(fontFamilyName, this.btnCancel.Font.Size, this.btnCancel.Font.Style, this.btnCancel.Font.Unit, this.btnCancel.Font.GdiCharSet);
                        this.txtMessage.Font = new System.Drawing.Font(fontFamilyName, this.txtMessage.Font.Size, this.txtMessage.Font.Style, this.txtMessage.Font.Unit, this.txtMessage.Font.GdiCharSet);
                        this.lblSignatoryCustomerName.Font = new System.Drawing.Font(fontFamilyName, this.lblSignatoryCustomerName.Font.Size, this.lblSignatoryCustomerName.Font.Style, this.lblSignatoryCustomerName.Font.Unit, this.lblSignatoryCustomerName.Font.GdiCharSet);
                        this.lblWaiverSet.Font = new System.Drawing.Font(fontFamilyName, this.lblWaiverSet.Font.Size, this.lblWaiverSet.Font.Style, this.lblWaiverSet.Font.Unit, this.lblWaiverSet.Font.GdiCharSet);
                        this.label2.Font = new System.Drawing.Font(fontFamilyName, this.label2.Font.Size, this.label2.Font.Style, this.label2.Font.Unit, this.label2.Font.GdiCharSet);
                    }
                }
            }
            catch(Exception ex)
            {

                log.Error("Errow while Setting Font", ex);
                KioskStatic.logToFile("Error while setting font for the UI elements in frmCustomerDetailsForWaiver: " + ex.Message);
            }
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCustomerDetailsForWaiver");
            try
            {
                foreach (Control c in panel1.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverHeadersTextForeColor;
                    }
                }
                foreach (Control c in dgvCustomer.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverDgvCustomerTextForeColor;
                    string type = c.GetType().ToString().ToLower(); 
                }
                dgvCustomer.DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverDgvCustomerTextForeColor;
                dgvCustomer.DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.FrmCustWaiverDgvCustomerTextForeColor;
                dgvCustomer.RowsDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverDgvCustomerTextForeColor;
                dgvCustomer.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.FrmCustWaiverDgvCustomerTextForeColor;

                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverBtnHomeTextForeColor;
                this.lblSignatoryCustomerName.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverLblSignatoryCustomerNameTextForeColor;
                this.lblWaiverSet.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverLblWaiverSetTextForeColor;
                this.label2.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverLabel2TextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverBtnCancelTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverBtnCancelTextForeColor;
                this.btnAddNewRelations.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverBtnAddNewRelationsTextForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverBtnProceedTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmCustWaiverTxtMessageTextForeColor;
                this.bigVerticalScrollCustomer.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnAddNewRelations.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                panel1.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCustomerDetailsForWaiver: " + ex.Message);
            }
            log.LogMethodExit();
        }


        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                btnCancel.Enabled = true;
                btnAddNewRelations.Enabled = true;
                btnPrev.Enabled = true;
                btnProceed.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void DisableButtons()
        {
            log.LogMethodEntry();
            try
            {
                btnCancel.Enabled = false;
                btnAddNewRelations.Enabled = false;
                btnPrev.Enabled = false;
                btnProceed.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}

