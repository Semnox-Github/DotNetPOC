using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ThirdParty;
using Semnox.Parafait.Product;
using System.Data.SqlClient;
using Parafait_POS.Redemption;
using Semnox.Parafait.JobUtils;
using Parafait_POS.Subscription;
using Semnox.Parafait.Waiver;
using Parafait_POS.Waivers;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;

namespace Parafait_POS
{
    public partial class CustomerRegistrationUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerDetailUI customerDetailUI;
        private Utilities utilities;
        private CustomerDTO customerDTO;
        private Action<string, string> displayMessageLine;
        Action SetLastTrxActivityTime;

        private frmStatus frmstatus;
        private CustomerFeedbackQuestionnairUI customerFeedbackQuestionnairUI;

        LoyaltyRedemptionBL objLoyaltyRedemptionBL = new LoyaltyRedemptionBL();
        LoyaltyRedemptionDTO objLoyaltyRedemptionDTO = null;
        frmCapillaryRedemption frmRedemption = new frmCapillaryRedemption();

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        public bool IncorrectCustomerSetupForWaiver { get; set; }
        public string WaiverSetupErrorMsg { get; set; }
        public Transaction NewTrx { get; set; }
        public Card CurrentCard{ get; set;}
        public bool IsCalledFromProductClick { get; set; }
        public bool BypassRegisterCustomer { get; set; }
        public bool IsValid { get; set; }

        public CustomerDTO CustomerDTO
        {
            get
            {
                return customerDTO;
            }
            set
            {
                customerDTO = value;
                displayCustomerDetails(value);
            }
        }

        public CustomerRegistrationUI(Utilities utilities, 
                                      Action<string, string> displayMessageLine,
                                      Action SetLastTrxActivityTime)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            this.displayMessageLine = displayMessageLine;
            this.SetLastTrxActivityTime = SetLastTrxActivityTime;
            customerDetailUI = new CustomerDetailUI(utilities, POSUtils.ParafaitMessageBox, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            customerDetailUI.CustomerContactInfoEntered += customerDetailUI_CustomerContactInfoEntered;
            customerDetailUI.UniqueIdentifierValidating += txtUniqueId_Validating;
            panelCustomer.Controls.Add(customerDetailUI);
            //Customer Verification button
            if (utilities.getParafaitDefaults("ENABLE_KIOSK_CUSTOMER_VERIFICATION").Equals("N"))
            {
                btnVerifyCustomer.Visible = false;
            }
            //Customer Relation button
            if (utilities.getParafaitDefaults("SHOW_ADD_RELATION_IN_CUSTOMER_SCREEN").Equals("N"))
            {
                btnCustomerRelationship.Visible = false;
            }
            if(POSStatic.AUTO_DEBITCARD_PAYMENT_POS)
            {
                btnLookupCustomer.Enabled = false;
            }

            //Customer Waiver button
            WaiverSetContainer waiverSetContainer = null;
            List<WaiverSetDTO> waiverSetDTOList = null;
            try
            {
                waiverSetContainer = WaiverSetContainer.GetInstance;

            }
            catch
            {
                //Assume waiver is not setup
            }
            if (waiverSetContainer != null)
                waiverSetDTOList = waiverSetContainer.GetWaiverSetDTOList(utilities.ExecutionContext.GetSiteId());
            if (waiverSetDTOList == null
                || (waiverSetDTOList != null && waiverSetDTOList.Count <= 0))
            {
                btnWaivers.Visible = false;
            }
            //Customer Membership button
            if (MembershipViewContainerList.GetMembershipContainerDTOCollection(utilities.ExecutionContext.GetSiteId(), null).MembershipContainerDTOList.Any() == false)
            {
                btnMembershipDetails.Visible = false;
            }
            //Customer Inactivate button
            if (utilities.getParafaitDefaults("CUSTOMER_INACTIVATION_METHOD").Equals("None"))
            {
                btnInActivate.Visible = false;
            }
            //Customer Subscription button
            if (!ProductsContainerList.GetActiveProductsContainerDTOList(utilities.ExecutionContext.GetSiteId()).Exists(x => x.ProductSubscriptionContainerDTO != null))
            {
                btnCustomerSubscription.Visible = false;
            }
            log.LogMethodExit();
        }

        private void buttonCustomerSave_Click(object sender, EventArgs e)
        {
            try
            {
                log.LogMethodEntry(sender, e);
                IsValid = false;
                SetLastTrxActivityTime();
                if (CurrentCard == null && POSStatic.REGISTER_CUSTOMER_WITHOUT_CARD == false)
                {
                    displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 247), ERROR);
                    return;
                }

                List<ValidationError> validationErrorList;
                customerDetailUI.ClearValdationErrors();
                validationErrorList = customerDetailUI.UpdateCustomerDTO();
                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDetailUI.CustomerDTO);
                try
                {
                    validationErrorList.AddRange(customerBL.Validate());
                    if (validationErrorList.Count > 0)
                    {
                        customerDetailUI.ShowValidationError(validationErrorList);
                        displayMessageLine(validationErrorList[0].Message, ERROR);
                        return;
                    }
                }
                catch (ValidationException ex)
                {
                    customerBL = new CustomerBL(utilities.ExecutionContext, customerDetailUI.CustomerDTO.Id);
                    customerDTO = customerBL.CustomerDTO;
                    customerDetailUI.CustomerDTO = customerBL.CustomerDTO;
                    customerDetailUI.UpdateCustomerDTO();
                    //SetCustomerTextBoxInfo();
                    validationErrorList = new List<ValidationError>();
                    validationErrorList.Add(new ValidationError("Customer", "Active waiver found", ex.Message));
                    customerDetailUI.ShowValidationError(validationErrorList);
                    displayMessageLine(validationErrorList[0].Message, ERROR);
                    return;
                }
                IsValid = true;


                customerDTO = customerDetailUI.CustomerDTO;
                if (CurrentCard != null)
                    CurrentCard.customerDTO = customerDTO;
                else if (CurrentCard == null
                    && customerDTO != null
                    && customerDTO.Id <= -1
                    && ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N"))
                {
                    string strProdId = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOAD_PRODUCT_ON_REGISTRATION");
                    int productId = -1;
                    if (int.TryParse(strProdId, out productId) == true && productId != -1)
                    {
                        Products regProductBL = new Products(productId);
                        ProductsDTO regProductDTO = regProductBL.GetProductsDTO;
                        if (regProductDTO.AutoGenerateCardNumber.Equals("Y"))
                        {
                            RandomTagNumber randomCardNumber = new RandomTagNumber(utilities.ExecutionContext);
                            if (CurrentCard == null)
                            {
                                CurrentCard = new Semnox.Parafait.Transaction.Card(randomCardNumber.Value, utilities.ParafaitEnv.LoginID, utilities);
                                CurrentCard.primaryCard = "Y"; //Assign auto gen card as primary card
                            }
                            SqlConnection connection = utilities.getConnection();
                            SqlTransaction sqlTransaction = connection.BeginTransaction();
                            try
                            {
                                CurrentCard.createCard(sqlTransaction);
                                sqlTransaction.Commit();
                                CurrentCard.customerDTO = customerDTO;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 253), WARNING);
                                sqlTransaction.Rollback();
                                return;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }

                if (CurrentCard != null && CurrentCard.card_id == -1)
                {
                    displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 253), WARNING);
                    return;
                }

                try
                {
                    if (customerDTO.Id == -1 || (CurrentCard != null && CurrentCard.customer_id != customerDTO.Id))
                    {
                        SqlConnection connection = utilities.getConnection();
                        SqlTransaction transaction = connection.BeginTransaction();
                        try
                        {

                            customerBL.Save(transaction);
                            transaction.Commit();
                        }
                        catch (ValidationException ex)
                        {
                            customerDetailUI.ShowValidationError(ex.ValidationErrorList);
                            displayMessageLine(ex.Message, ERROR);
                            transaction.Rollback();
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            displayMessageLine(ex.Message, ERROR);
                            transaction.Rollback();
                            return;
                        }
                        finally
                        {
                            connection.Close();
                        }
                        if (CurrentCard != null)
                        {
                            CurrentCard.updateCustomer();
                        }

                        //added for capillary API Integration
                        if (utilities.getParafaitDefaults("ENABLE_CAPILLARY_INTEGRATION").Equals("Y") && !string.IsNullOrEmpty(customerDTO.PhoneNumber))
                        {
                            //Added on 26-sep-2016 for showing capillary response awaited message
                            displayMessageLine("Please wait..Response awaited from Capillary...", WARNING);
                            //end
                            string msg = "";
                            objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();
                            objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.IsCustomerExist(customerDTO.PhoneNumber, ref msg);

                            //Check customer Exist in Capillary
                            if (objLoyaltyRedemptionDTO != null)
                            {
                                if (!objLoyaltyRedemptionDTO.success && !objLoyaltyRedemptionDTO.item_status)
                                {
                                    utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "N", customerDTO.Id.ToString(), utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                    AddCustomersToCapillary();
                                }
                                else
                                {
                                    utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "Y", customerDTO.Id.ToString(), utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                }
                            }
                            else
                            {
                                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, "API call failed: " + msg), "Capillary API Response");
                                utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", msg, "CustomerExist", 0, "N", customerDTO.Id.ToString(), utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                displayMessageLine(msg, MESSAGE);
                            }

                            AddPendingCustomersToCapillary(); //Add pending customers to Capillary
                        }
                        //end
                    }
                    else // customer already exists. no need to update card
                    {
                        SqlConnection connection = utilities.getConnection();
                        SqlTransaction transaction = connection.BeginTransaction();
                        try
                        {

                            customerBL.Save(transaction);
                            transaction.Commit();
                        }
                        catch (ValidationException ex)
                        {
                            customerDetailUI.ShowValidationError(ex.ValidationErrorList);
                            displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 253), WARNING);
                            transaction.Rollback();
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 253), WARNING);
                            transaction.Rollback();
                            return;
                        }
                        finally
                        {
                            connection.Close();
                        }
                        if (CurrentCard != null)
                            CurrentCard.updateCustomer();

                        //Added on 1-june-2016 for Updating customerDetails to Capillary
                        if (utilities.getParafaitDefaults("ENABLE_CAPILLARY_INTEGRATION").Equals("Y") && !string.IsNullOrEmpty(customerDTO.PhoneNumber))
                        {
                            //Added on 26-sep-2016 for showing capillary response awaited message 
                            displayMessageLine("Please wait..Response awaited from Capillary...", WARNING);
                            //end

                            string msg = "";
                            objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();
                            objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.IsCustomerExist(customerDTO.PhoneNumber, ref msg);

                            if (objLoyaltyRedemptionDTO != null) //Time-Out Exception occured 
                            {
                                //Check customer Exist in Capillary
                                if (!objLoyaltyRedemptionDTO.success && !objLoyaltyRedemptionDTO.item_status)
                                {
                                    utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "N", customerDTO.Id.ToString(), utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                    AddCustomersToCapillary();
                                }
                                else //Customer Exist Update Details in Capillary
                                {
                                    utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "Y", customerDTO.Id.ToString(), utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                                    //API Call to Update customer in capillary
                                    objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.UpdateCustomerToCapillary(customerDTO);

                                    if (objLoyaltyRedemptionDTO.success && objLoyaltyRedemptionDTO.item_status)
                                    {
                                        //Added on 26-sep-2016 for clearing capillary response awaited message 
                                        displayMessageLine("", MESSAGE);
                                        //end
                                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer details successfully interfaced to Capillary"), "Capillary API Response");
                                        utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called UpdateCustomerToCapillary() method", objLoyaltyRedemptionDTO.message, "CustomerUpdate", 0, "Y", customerDTO.Id.ToString(), utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                    }
                                    else
                                    {
                                        //Added on 26-sep-2016 for clearing capillary response awaited message 
                                        displayMessageLine("", MESSAGE);
                                        //end
                                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, "Failed to interface customer details to Capillary: " + objLoyaltyRedemptionDTO.message), "Capillary API Response");
                                        utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called UpdateCustomerToCapillary() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "N", customerDTO.Id.ToString(), utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                    }
                                }
                            }
                            else
                            {
                                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, "API call failed: " + msg), "Capillary API Response");
                                utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", msg, "CustomerExist", 0, "N", customerDTO.Id.ToString(), utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                displayMessageLine(msg, MESSAGE);
                            }
                        }
                        //end
                    }
                    //Begin : 19-Jan-2016 to display customerDTO info below card details in POS
                    //SetCustomerTextBoxInfo();
                    //End : 19-Jan-2016 to display customer info below card details in POS

                    if (CurrentCard != null)
                    {
                        CurrentCard.getCardDetails(CurrentCard.CardNumber);
                        //displayCardDetails();
                    }

                    displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 254), MESSAGE);
                    Close();
                    if (IsCalledFromProductClick)
                    {
                        if (utilities.getParafaitDefaults("ENABLE_CUSTOMER_FEEDBACK_PROCESS").Equals("Y"))
                        {
                            PerformCustomerFeedback("Visit Count");
                        }
                    }

                    //Added for merkle Integration
                    #region Merkle API Integration
                    if (utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION").Equals("Y") && customerDTO != null)
                    {
                        StartMerkleCall(customerDTO.CustomerCuponsDT, true, 0);
                    }
                    #endregion
                    //end
                }
                catch (Exception ex)
                {
                    displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 255, ex.Message), ERROR);
                }
            }
            finally
            {
                buttonCustomerSave.Enabled = true;
            }
        }

        private void customerDetailUI_CustomerContactInfoEntered(object sender, CustomerContactInfoEnteredEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, e.ContactType.ToString());
            customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, e.ContactValue)
                                  .OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                bool showCustomerLookup = false;
                bool duplicateCustomerFound = false;
                foreach (var customerDTOItem in customerDTOList)
                {
                    if (customerDTOItem.Id != customerDetailUI.CustomerDTO.Id)
                    {
                        duplicateCustomerFound = true;
                        break;
                    }
                }
                if (duplicateCustomerFound)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ENABLE_CAPILLARY_INTEGRATION"))
                    {
                        DialogResult result = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1452), MessageContainerList.GetMessage(utilities.ExecutionContext, "Validate Phone / Email"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        showCustomerLookup = true;
                    }
                    else
                    {
                        DialogResult result = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 524), MessageContainerList.GetMessage(utilities.ExecutionContext, "Validate Phone / Email"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK || result == DialogResult.Yes)
                        {
                            showCustomerLookup = true;
                        }
                    }
                    if (showCustomerLookup)
                    {
                        if (customerDTOList.Count > 1)
                        {
                            CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, "", "", "",
                                                                                     e.ContactType == ContactType.EMAIL ? e.ContactValue : "",
                                                                                     e.ContactType == ContactType.PHONE ? e.ContactValue : "",
                                                                                     "");
                            if (customerLookupUI.ShowDialog() == DialogResult.OK)
                            {
                                customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                            }
                            else
                            {
                                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ENABLE_CAPILLARY_INTEGRATION"))
                                {
                                    if (customerDetailUI.CustomerDTO.Id != customerDTOList[0].Id)
                                    {
                                        customerDetailUI.CustomerDTO = customerDTOList[0];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (customerDetailUI.CustomerDTO.Id != customerDTOList[0].Id)
                            {
                                customerDetailUI.CustomerDTO = customerDTOList[0];
                            }
                        }

                        if (CurrentCard == null)
                        {
                            customerDTO = customerDetailUI.CustomerDTO;
                            customerDetailUI.CustomerDTO = customerDetailUI.CustomerDTO;
                        }
                        else if (customerDetailUI.CustomerDTO.Id != CurrentCard.customer_id)
                        {
                            CurrentCard.customerDTO = customerDetailUI.CustomerDTO;
                            customerDTO = customerDetailUI.CustomerDTO;
                        }
                    }
                }

            }
            log.LogMethodExit();
        }

        private void txtUniqueId_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (CurrentCard == null)
                return;

            if ((sender as TextBox).Text.Trim() == "" && utilities.ParafaitEnv.UNIQUE_ID_MANDATORY_FOR_VIP == "Y" && CurrentCard.vip_customer == 'Y')
            {
                e.Cancel = true;
                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,289), WARNING);
                return;
            }
            displayMessageLine("", MESSAGE);

            if ((sender as TextBox).Text.Trim() == "")
                return;

            List<CustomerDTO> customerDTOList = null;
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.EQUAL_TO, (sender as TextBox).Text.Trim());
            if (customerDetailUI.CustomerDTO != null && customerDetailUI.CustomerDTO.Id >= 0)
            {
                customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_ID, Operator.NOT_EQUAL_TO, customerDetailUI.CustomerDTO.Id);
            }
            customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, false, false);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {

                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 290), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Registration"), MessageBoxButtons.OK) == System.Windows.Forms.DialogResult.OK)
                {
                    if (utilities.ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
                        e.Cancel = true;
                }
                else
                {
                    displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 290), WARNING);
                    if (utilities.ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
                    {
                        e.Cancel = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        public LoyaltyRedemptionDTO AddCustomersToCapillary()
        {
            objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();
            objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.AddCustomerToCapillary(customerDTO);

            if (!objLoyaltyRedemptionDTO.success && !objLoyaltyRedemptionDTO.item_status)
            {
                utilities.executeScalar(@"IF EXISTS (SELECT 1 FROM EventLog 
                                                    WHERE Source ='Loyalty Redemption' AND Category ='CustomerInsert' AND Value =@custId AND Name = 'N' )        
	                                                BEGIN 
	                                                    UPDATE Eventlog SET Source ='Loyalty Redemption', Timestamp = getdate(), Description=@description, Category ='CustomerInsert', Value =@custId, Name = 'N'
                                                        WHERE Source ='Loyalty Redemption' AND Category ='CustomerInsert' AND Value =@custId AND Name = 'N'
	                                                END                             
                                                    ELSE
	                                                BEGIN
                                                        INSERT INTO EventLog (Source, Timestamp,Type, Username, Computer, Data, Category, Description, Value, Name) 
                                                        Values ('Loyalty Redemption',Getdate(), 'D',@userName, @posMachine,'called AddCustomerDetailsToCapillary method','CustomerInsert',@description,@custId, 'N')
	                                                END", new SqlParameter("@description", objLoyaltyRedemptionDTO.message),
                                                        new SqlParameter("@custId", customerDTO.Id),
                                                        new SqlParameter("@userName", utilities.ParafaitEnv.LoginID),
                                                        new SqlParameter("@posMachine", utilities.ParafaitEnv.POSMachine));
                //Added on 26-sep-2016 for clearing capillary Response message
                displayMessageLine("", MESSAGE);
                //end
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext,"Failed to interface customer details into Capillary: " + objLoyaltyRedemptionDTO.message), "Capillary API Response");
            }
            else
            {
                //Added on 26-sep-2016 for clearing capillary Response message
                displayMessageLine("", MESSAGE);
                //end
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext,"Customer details successfully interfaced to Capillary"), "Capillary API Response");
                utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called AddCustomersToCapillary method ", objLoyaltyRedemptionDTO.message, "CustomerInsert", 0, "Y", customerDTO.Id.ToString(), utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
            }
            return objLoyaltyRedemptionDTO;
        }

        public string StartMerkleCall(DataTable couponsDT, bool showCoupon, int elementCount)
        {
            log.Info("Start-StartMerkleCall()");

            //QR code testing code should be added here
            string couponNumber = string.Empty;
            string phoneNum = string.Empty;
            string accessTokenId = string.Empty;
            try
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    CustomerDetails getCustomerDetails = new CustomerDetails(utilities);
                    if (customerDTO != null)
                    {
                        if (!string.IsNullOrEmpty(customerDTO.PhoneNumber) || !string.IsNullOrEmpty(customerDTO.WeChatAccessToken))
                        {
                            bool isExceptionMsg = false;

                            if (!string.IsNullOrEmpty(customerDTO.WeChatAccessToken))
                                accessTokenId = customerDTO.WeChatAccessToken.ToString();

                            if (!string.IsNullOrEmpty(customerDTO.PhoneNumber))
                                phoneNum = customerDTO.PhoneNumber.ToString();

                            DataTable custDT = new DataTable();
                            if (couponsDT != null && couponsDT.Rows.Count > 0)
                            {
                                customerDTO.CustomerCuponsDT = couponsDT;
                            }
                            else
                            {
                                frmstatus = new frmStatus("Merkle Integration");
                                DialogResult statusResult = DialogResult.None;
                                frmstatus.WindowState = FormWindowState.Normal;
                                displayMessageLine("Please wait..Response awaited from Merkle...", WARNING);
                                bool isComplete = false;

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    //get customer details
                                    try
                                    {
                                        if (statusResult != DialogResult.Cancel)
                                        {
                                            custDT = getCustomerDetails.QueryResult(phoneNum, accessTokenId);// API call to get customer details

                                            if (custDT != null)
                                            {
                                                //Update the customer
                                                UpdateCustomerDetails(custDT);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        isExceptionMsg = true;
                                        displayMessageLine("Merkle Integration Failed: " + ex.Message, WARNING);
                                        utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while getting customer details ,Customer WeChatb Token: " + accessTokenId + "Errro details: " + ex.ToString(), "", "MerkleAPIIntegration", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                    }
                                    //Check cancelled by user
                                    if (statusResult != DialogResult.Cancel)
                                    {
                                        if (customerDTO != null && custDT != null && custDT.Rows.Count > 0 && string.IsNullOrEmpty(custDT.Rows[0]["code"].ToString()))
                                        {
                                            customerDTO.CustomerCuponsDT = new DataTable();
                                            CouponDetails getCouponsDetails = new CouponDetails(utilities);

                                            //get customer coupons list
                                            try
                                            {
                                                if (statusResult != DialogResult.Cancel)
                                                {
                                                    customerDTO.CustomerCuponsDT = getCouponsDetails.QueryResult(phoneNum, accessTokenId);//API call to get customers coupons
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                isExceptionMsg = true;
                                                displayMessageLine("Merkle Integration Failed: " + ex.Message, WARNING);
                                                utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while getting Coupon details ,Customer WeChatb Token: " + accessTokenId + "Errro details: " + ex.ToString(), "", "MerkleAPIIntegration", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                                            }
                                            finally
                                            {
                                                isComplete = true;
                                                if (frmstatus.InvokeRequired)
                                                {
                                                    frmstatus.Invoke(new Action(() => frmstatus.Close()));
                                                }
                                                else
                                                {
                                                    frmstatus.Close();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            isComplete = true;
                                            if (frmstatus.InvokeRequired)
                                            {
                                                frmstatus.Invoke(new Action(() => frmstatus.Close()));
                                            }
                                            else
                                            {
                                                frmstatus.Close();
                                            }
                                        }
                                    }
                                });
                                thread.Name = "Get Coupons";
                                thread.Start();
                                if (!isComplete)
                                {
                                    statusResult = frmstatus.ShowDialog();
                                }
                            }

                            if (customerDTO != null && customerDTO.CustomerCuponsDT != null && customerDTO.CustomerCuponsDT.Rows.Count > 0)
                            {
                                DataTable validCoupnsDT = GetValidCouponDetails(customerDTO.CustomerCuponsDT);

                                if (validCoupnsDT != null && validCoupnsDT.Rows.Count > 0)
                                {
                                    customerDTO.CustomerCuponsDT = validCoupnsDT;
                                    GenericDataEntry customerCoupon = new GenericDataEntry(customerDTO, showCoupon, elementCount);
                                    displayMessageLine("", MESSAGE);
                                    customerCoupon.StartPosition = FormStartPosition.CenterScreen;
                                    customerCoupon.BringToFront();
                                    customerCoupon.Text = "Customer Coupons";
                                    if (elementCount > 0)
                                    {
                                        customerCoupon.DataEntryObjects[0].mandatory = true;
                                        customerCoupon.DataEntryObjects[0].label = MessageContainerList.GetMessage(utilities.ExecutionContext,"Coupon Number");
                                    }
                                    if (customerCoupon.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        if (customerCoupon.DataEntryObjects[0].data != null)
                                            couponNumber = customerCoupon.DataEntryObjects[0].data;
                                    }
                                    else
                                    {
                                        couponNumber = string.Empty;
                                    }
                                }
                            }
                            if (frmstatus != null)
                            {
                                if (frmstatus.Visible == true)
                                {
                                    this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                                }
                            }
                            //clear the message
                            if (!isExceptionMsg)
                            {
                                displayMessageLine("", MESSAGE);
                            }
                        }
                    }
                }
                else
                {
                    displayMessageLine("Internet Connection Failed to Enable Merkle Integration..", MESSAGE);
                }
            }
            catch (Exception ex)
            {
                //displayMessageLine("", MESSAGE);
                if (frmstatus != null)
                {
                    if (frmstatus.Visible == true)
                    {
                        this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                    }
                }
                log.Error("Error while calling Merkle API Integrtaion :" + ex.ToString());
            }
            log.Info("Ends-StartMerkleCall()");
            return couponNumber;
        }

        public void UpdateCustomerDetails(DataTable customerDT)
        {
            if (customerDT != null && customerDT.Rows.Count > 0)
            {
                bool isDetailsModified = false;
                //phone number fecth from Merkle to Pparafait
                if (!string.IsNullOrEmpty(customerDT.Rows[0]["external_customer_id"].ToString()))
                {
                    if (customerDT.Rows[0]["external_customer_id"].ToString() != customerDTO.PhoneNumber)
                    {
                        ContactDTO contactDTO = null;
                        if (customerDTO.ContactDTOList != null)
                        {
                            foreach (var contactDTOItem in customerDTO.ContactDTOList)
                            {
                                if (contactDTOItem.ContactType == ContactType.PHONE)
                                {
                                    contactDTO = contactDTOItem;
                                    break;
                                }
                            }
                            if (contactDTO == null)
                            {
                                contactDTO = new ContactDTO();
                                contactDTO.ContactType = ContactType.PHONE;
                                customerDTO.ContactDTOList.Add(contactDTO);
                            }
                        }
                        else
                        {
                            contactDTO = new ContactDTO();
                            contactDTO.ContactType = ContactType.PHONE;
                            customerDTO.ContactDTOList = new List<ContactDTO>();
                            customerDTO.ContactDTOList.Add(contactDTO);
                        }
                        contactDTO.Attribute1 = customerDT.Rows[0]["external_customer_id"].ToString();
                        isDetailsModified = true;
                    }
                }

                //Email Id replace
                if (!string.IsNullOrEmpty(customerDT.Rows[0]["email"].ToString()))
                {
                    if (customerDT.Rows[0]["email"].ToString() != customerDTO.Email)
                    {
                        ContactDTO contactDTO = null;
                        if (customerDTO.ContactDTOList != null)
                        {
                            foreach (var contactDTOItem in customerDTO.ContactDTOList)
                            {
                                if (contactDTOItem.ContactType == ContactType.EMAIL)
                                {
                                    contactDTO = contactDTOItem;
                                    break;
                                }
                            }
                            if (contactDTO == null)
                            {
                                contactDTO = new ContactDTO();
                                contactDTO.ContactType = ContactType.EMAIL;
                                customerDTO.ContactDTOList.Add(contactDTO);
                            }
                        }
                        else
                        {
                            contactDTO = new ContactDTO();
                            contactDTO.ContactType = ContactType.EMAIL;
                            customerDTO.ContactDTOList = new List<ContactDTO>();
                            customerDTO.ContactDTOList.Add(contactDTO);
                        }
                        contactDTO.Attribute1 = customerDT.Rows[0]["email"].ToString();
                        isDetailsModified = true;
                    }
                }

                //Name
                if (!string.IsNullOrEmpty(customerDT.Rows[0]["name"].ToString()))
                {
                    if (customerDT.Rows[0]["name"].ToString() != customerDTO.FirstName)
                    {
                        customerDTO.FirstName = customerDT.Rows[0]["name"].ToString();
                        isDetailsModified = true;
                    }
                }

                if (isDetailsModified)
                {
                    SqlConnection sqlConnection = utilities.createConnection();
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                    try
                    {
                        CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDTO);
                        customerBL.Save(sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while saving customer details", ex);
                        sqlTransaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }

                }

            }
        }

        public DataTable GetValidCouponDetails(DataTable dt)
        {
            DateTime trxDate;
            if (NewTrx == null || NewTrx.TrxDate == DateTime.MinValue)
            {
                trxDate = DateTime.Now;
            }
            else
            {
                trxDate = NewTrx.TrxDate;
            }

            DataTable validCouponsDT = new DataTable();

            validCouponsDT.Columns.AddRange(new DataColumn[7] { new DataColumn("Sl No", typeof(int)),
                                                                new DataColumn("Select", typeof(bool)),
                                                                new DataColumn("Discount Name", typeof(string)),
                                                                new DataColumn("code", typeof(string)),
                                                                new DataColumn("Value", typeof(string)),
                                                                new DataColumn("description", typeof(string)),
                                                                new DataColumn("expires_at", typeof(string))});
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow rw in dt.Rows)
                {
                    DataTable discountDT = POSStatic.Utilities.executeDataTable(@"select top 1 dc.Discount_id, (select discount_name from discounts where discount_id = dc.Discount_id) as DiscountName, 
                                                              ISNULL(CAST(dc.CouponValue as varchar), 
                                                               ISNULL( CAST((Select DiscountAmount from discounts where discount_id = dc.Discount_id) as varchar), 
                                                                    CAST((Select discount_percentage from discounts where discount_id = dc.Discount_id) as varchar)+ '%'
                                                               )) as Value,
                                                                      dc.ExpiryDate
                                                                       FROM DiscountCoupons dc
                                                                       WHERE ((Tonumber is null and FromNumber= @couponNumber)
                                                                       or Tonumber is not null
                                                                       and len(@couponNumber)=len(FromNumber)
                                                                       and @couponNumber between isnull(FromNumber,'') and isnull(ToNumber,'zzzzzzzzzzzzzzzzzzzz')
                                                                       or(FromNumber is null and ToNumber is null and dc.Count is not null))
                                                                       and(isnull(Count, 0)= 0 or count>(select count(*)
                                                                       from DiscountCouponsUsed u
                                                                       where u.CouponSetId=dc.CouponSetId))
                                                                       and((dc.FromNumber is null and dc.ToNumber is null and dc.Count is not null)or
                                                                       (dc.FromNumber is not null and dc.ToNumber is null and dc.Count is not null)or
                                                                       not exists(select 1
                                                                       from DiscountCouponsUsed u
                                                                       where u.CouponSetId=dc.CouponSetId
                                                                       and u.CouponNumber= @couponNumber))
                                                                       and (CONVERT(date,@trxdate) > = CONVERT(date, ISNULL(StartDate, GETDATE())))
                                                                   and (CONVERT(date,@trxdate) <= CONVERT(date, ISNULL(ExpiryDate, GETDATE())))",
                                                                        new SqlParameter("@trxdate", trxDate), new SqlParameter("@couponNumber", rw["code"]));
                    if (discountDT != null && discountDT.Rows.Count > 0)
                    {
                        validCouponsDT.Rows.Add(validCouponsDT.Rows.Count + 1, 0, discountDT.Rows[0]["DiscountName"], rw["code"], discountDT.Rows[0]["Value"], rw["description"], rw["expires_at"]);
                    }
                }
            }
            return validCouponsDT;
        }

        public void AddPendingCustomersToCapillary()
        {
            string msg = "";
            DataTable dt = utilities.executeDataTable(@"SELECT isnull(Value,-1) as CustomerId 
                                                       FROM EventLog 
                                                       WHERE Source ='Loyalty Redemption' AND Category = 'CustomerInsert' AND Name = 'N'");

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow rw in dt.Rows)
                {
                    DataTable customerData = utilities.executeDataTable(@"SELECT customer_name, last_name, email, gender, contact_phone1 
                                                                           FROM customers where customer_id = @custId",
                                                                           new SqlParameter("@custId", rw["CustomerId"].ToString()));

                    if (customerData != null && customerData.Rows.Count > 0)
                    {
                        objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();

                        CustomerDTO CapillaryCustomer = (new CustomerBL(utilities.ExecutionContext, Convert.ToInt32(rw["CustomerId"]))).CustomerDTO;

                        objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.IsCustomerExist(CapillaryCustomer.PhoneNumber, ref msg);
                        if (objLoyaltyRedemptionDTO.success && objLoyaltyRedemptionDTO.item_status)
                            continue;

                        objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.AddCustomerToCapillary(CapillaryCustomer);
                        if (objLoyaltyRedemptionDTO.success && objLoyaltyRedemptionDTO.item_status)
                        {
                            utilities.executeDataTable(@"UPDATE EventLog 
                                                         SET Name = 'Y', Username = @userName, Computer =@posMachine
                                                         WHERE Source ='Loyalty Redemption' AND Category = 'CustomerInsert' AND Value = @custId",
                                                         new SqlParameter("@custId", rw["CustomerId"].ToString()),
                                                         new SqlParameter("@userName", utilities.ParafaitEnv.LoginID),
                                                         new SqlParameter("@posMachine", utilities.ParafaitEnv.POSMachine));
                        }
                    }
                }
            }
        }

        private bool PerformCustomerFeedback(string SurveyType)//Starts:Modification on 02-Jan-2017 for cutomer feed back
        {
            log.Debug("Starts-PerformCustomerFeedback()");
            int customerId = -1;
            string customer_name = "";
            string phoneNumber = "";
            CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails(utilities.ExecutionContext);
            CustomerFeedbackSurveyList customerFeedbackSurveyList = new CustomerFeedbackSurveyList(utilities.ExecutionContext);
            List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList = new List<CustomerFeedbackSurveyDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchBycustomerFeedbackSurveyParameters = new List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>>();

            CustomerFeedbackSurveyPOSMappingList customerFeedbackSurveyPOSMappingList = new CustomerFeedbackSurveyPOSMappingList(utilities.ExecutionContext);
            List<CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyPOSMappingDTOList = new List<CustomerFeedbackSurveyPOSMappingDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>> searchByCustomerFeedbackSurveyPOSMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();

            customerFeedbackQuestionnairUI = null;
            if (CurrentCard != null && CurrentCard.customerDTO != null)
            {
                customerId = CurrentCard.customerDTO.Id;
                customer_name = CurrentCard.customerDTO.FirstName;
                phoneNumber = CurrentCard.customerDTO.PhoneNumber;
            }
            else if (customerDTO != null)
            {
                customerId = customerDTO.Id;
                customer_name = customerDTO.FirstName;
                phoneNumber = customerDTO.PhoneNumber;
            }

            if (customerId == -1 && SurveyType != "Transaction")
            {
                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,"Customer is not registered"), WARNING);
                return false;
            }

            searchBycustomerFeedbackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.IS_ACTIVE, "1"));
            searchBycustomerFeedbackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            customerFeedbackSurveyDTOList = customerFeedbackSurveyList.GetAllCustomerFeedbackSurvey(searchBycustomerFeedbackSurveyParameters);
            if (customerFeedbackSurveyDTOList != null && customerFeedbackSurveyDTOList.Count > 0)
            {
                foreach (CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO in customerFeedbackSurveyDTOList)
                {
                    searchByCustomerFeedbackSurveyPOSMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();
                    searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.IS_ACTIVE, "1"));
                    searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                    searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_ID, customerFeedbackSurveyDTO.CustFbSurveyId.ToString()));
                    searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.POS_MACHINE_ID, utilities.ParafaitEnv.POSMachineId.ToString()));
                    customerFeedbackSurveyPOSMappingDTOList = customerFeedbackSurveyPOSMappingList.GetAllCustomerFeedbackSurveyPOSMapping(searchByCustomerFeedbackSurveyPOSMappingParameters);
                    if (customerFeedbackSurveyPOSMappingDTOList != null && customerFeedbackSurveyPOSMappingDTOList.Count > 0)
                    {
                        if ((!customerFeedbackSurveyDTO.FromDate.Equals(DateTime.MinValue) && customerFeedbackSurveyDTO.FromDate.CompareTo(DateTime.Now) <= 0) && (customerFeedbackSurveyDTO.ToDate.Equals(DateTime.MinValue) || customerFeedbackSurveyDTO.ToDate.CompareTo(DateTime.Now) >= 0))
                        {
                            if (SurveyType == "Transaction" || (SurveyType != "Transaction" && !customerFeedbackSurveyDetails.IsQuestionAsked(customerFeedbackSurveyDTO.CustFbSurveyId, "CUSTOMER", customerId, DateTime.Today, "Visit Count")))
                            {
                                if (SurveyType == "Visit Count")
                                {
                                    if (customerFeedbackQuestionnairUI == null)
                                    {
                                        customerFeedbackQuestionnairUI = new CustomerFeedbackQuestionnairUI(utilities, "CUSTOMER", customerId, customer_name, phoneNumber, SurveyType);
                                    }
                                    customerFeedbackQuestionnairUI.SetSurveyId(customerFeedbackSurveyDTO.CustFbSurveyId);
                                    customerFeedbackQuestionnairUI.Text = customerFeedbackSurveyDTO.SurveyName;
                                    if (utilities.getParafaitDefaults("SHOW_IN_WAIVER").Equals("Y"))
                                    {
                                        if (Screen.AllScreens.Length == 1)
                                        {
                                            displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,1006), WARNING);
                                            return false;
                                        }
                                        DisplayFormToWaiver(customerFeedbackQuestionnairUI, "CustomerFeedback");
                                    }
                                    else
                                    {
                                        customerFeedbackQuestionnairUI.ShowDialog();
                                    }
                                }
                                else if (SurveyType == "Transaction")
                                {
                                    if (!customerFeedbackSurveyDetails.IsRetailQuestionAsked(customerFeedbackSurveyDTO.CustFbSurveyId, "Transaction"))
                                    {
                                        if (customerFeedbackQuestionnairUI == null)
                                        {
                                            customerFeedbackQuestionnairUI = new CustomerFeedbackQuestionnairUI(utilities, "TRX_HEADER", -1, customer_name, phoneNumber, SurveyType);
                                        }
                                        customerFeedbackQuestionnairUI.SetSurveyId(customerFeedbackSurveyDTO.CustFbSurveyId);
                                        customerFeedbackQuestionnairUI.Text = customerFeedbackSurveyDTO.SurveyName;
                                        if (utilities.getParafaitDefaults("SHOW_IN_WAIVER").Equals("Y"))
                                        {
                                            if (Screen.AllScreens.Length == 1)
                                            {
                                                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,1006), WARNING);
                                                return false;
                                            }
                                            DisplayFormToWaiver(customerFeedbackQuestionnairUI, "CustomerFeedback");
                                        }
                                        else
                                        {
                                            customerFeedbackQuestionnairUI.ShowDialog();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            IsCalledFromProductClick = false; //resetting flag
            log.Debug("Ends-PerformCustomerFeedback()");
            return true;
        }//Ends:Modification on 02-Jan-2017 for customer feed back

        void DisplayFormToWaiver(Form WavierForm, string Applicability)//Starts:Modification on 02-Jan-2017 for cutomer feed back
        {
            log.Debug("starts-DisplayFormToWaiver(wavierForm, CashierForm, Applicability, TitleOfStatusForm)");
            Screen[] sc;
            sc = Screen.AllScreens;
            if (sc.Length > 1)
            {
                WavierForm.FormBorderStyle = FormBorderStyle.None;
                WavierForm.Left = sc[1].Bounds.Width;
                WavierForm.Top = sc[1].Bounds.Height;
                WavierForm.StartPosition = FormStartPosition.Manual;
                WavierForm.Location = sc[1].Bounds.Location;
                Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
                WavierForm.Location = p;
                WavierForm.TopMost = true;
                WavierForm.WindowState = FormWindowState.Maximized;
            }
            frmstatus = new frmStatus(WavierForm, "CustomerFeedback");
            //frmstatus.TopMost = true;
            frmstatus.StartPosition = FormStartPosition.CenterScreen;
            frmstatus.Location = sc[0].Bounds.Location;
            Point p1 = new Point(sc[0].Bounds.Location.X, sc[0].Bounds.Location.Y);
            frmstatus.Location = p1;
            frmstatus.BringToFront();
            frmstatus.WindowState = FormWindowState.Normal;

            DialogResult statusResult = new System.Windows.Forms.DialogResult();
            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                statusResult = frmstatus.ShowDialog();
            });
            thread.Start();

            WavierForm.ShowDialog();
            this.Invoke(new MethodInvoker(() => frmstatus.Close()));
            if (sc.Length > 1)
            {
                Cursor.Position = new Point(sc[0].Bounds.Width / 2, sc[0].Bounds.Height / 2);
            }
            log.Debug("Ends-DisplayFormToWaiver(wavierForm, CashierForm, Applicability, TitleOfStatusForm)");

        }

        private void btnCloseCustomerView_Click(object sender, EventArgs e)
        {
            log.Info("btnCloseCustomerView_Click - begin");
            int managerId = -1;
            if (IsCalledFromProductClick && customerDTO == null)
            {
                if (!Authenticate.Manager(ref managerId))
                {
                    Close();
                    displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,1664), WARNING);
                    //throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext,1664));
                    //return;
                }
                else
                {
                    BypassRegisterCustomer = true;
                    Users approveUser = new Users(utilities.ExecutionContext, managerId);
                    if (approveUser.UserDTO != null)
                    {
                        log.Info("Manager Approval received to bypass customer registration. Manager Id: " + managerId);
                        utilities.EventLog.logEvent("Customer Registration", 'D', "Bypass Customer Registration", "Bypass Customer Registration approved by Manager Login: " + approveUser.UserDTO.LoginId, "RegisterCustomer", 3, "Manager User Guid", approveUser.UserDTO.Guid, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    }
                }
            }
            IsValid = false;
            List<ValidationError> validationErrorList;
            validationErrorList = customerDetailUI.UpdateCustomerDTO();
            CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDetailUI.CustomerDTO);
            try
            {
                validationErrorList.AddRange(customerBL.Validate());
                if (validationErrorList.Count > 0)
                {
                    IsValid = false;
                }
                else
                {
                    IsValid = true;
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                IsValid = false;
            }
            Close();
            if (IsCalledFromProductClick)//Starts:Modification 02-Jan-2017 for Customer feedback
            {
                if (utilities.getParafaitDefaults("ENABLE_CUSTOMER_FEEDBACK_PROCESS").Equals("Y"))
                {
                    PerformCustomerFeedback("Visit Count");
                }
            }//Ends:Modification 02-Jan-2017 for Customer feedback

            #region Merkle API Integration
            if (utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION").Equals("Y") && customerDTO != null)
            {
                StartMerkleCall(customerDTO.CustomerCuponsDT, true, 0);
            }
            #endregion
            log.Info("btnCloseCustomerView_Click - end");
        }

        private void btnLookupCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities,
                                                                         customerDetailUI.FirstName,
                                                                         customerDetailUI.MiddleName,
                                                                         customerDetailUI.LastName,
                                                                         customerDetailUI.Email,
                                                                         customerDetailUI.PhoneNumber,
                                                                         customerDetailUI.UniqueIdentifier);
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    if (customerLookupUI.SelectedCustomerDTO != null)
                    {
                        if (CurrentCard == null)
                        {
                            if (POSStatic.REGISTER_CUSTOMER_WITHOUT_CARD == false)
                                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,247), WARNING);
                            else
                            {
                                customerDTO = customerLookupUI.SelectedCustomerDTO;
                                customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                                displayCustomerDetails(customerDTO);
                            }
                        }
                        else if (customerLookupUI.SelectedCustomerDTO.Id != CurrentCard.customer_id)
                        {
                            CurrentCard.customerDTO = customerLookupUI.SelectedCustomerDTO;
                            customerDTO = customerLookupUI.SelectedCustomerDTO; //assign customerDTO details to trx customerDTO object
                            customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                            displayCustomerDetails(customerDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnVerifyCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastTrxActivityTime();
            if (CurrentCard == null)
            {
                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,257), WARNING);
                return;
            }
            customerDetailUI.UpdateCustomerDTO();
            if (customerDetailUI.CustomerDTO.CustomerType == CustomerType.UNREGISTERED)
            {
                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,2491), WARNING);
                return;
            }
            if (CurrentCard.customerDTO == null)
            {
                CurrentCard.customerDTO = customerDetailUI.CustomerDTO;
            }

            if (CurrentCard.customerDTO.Verified)
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1453));
                return;
            }

            try
            {
                CustomerVerificationUI customerVerificationUI = new CustomerVerificationUI(utilities, customerDetailUI.CustomerDTO, POSUtils.ParafaitMessageBox);
                if (customerVerificationUI.ShowDialog() == DialogResult.OK)
                {
                    customerDetailUI.RefreshBindings();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while verifying the customer", ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnInActivate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetLastTrxActivityTime();
            int managerId = -1;
            using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
            {
                try
                {
                    if (Authenticate.Manager(ref managerId))
                    {
                        // use using
                        log.Debug("Deleteting customer " + customerDetailUI.CustomerDTO.Id);
                        // Show a confirmation message
                        frmParafaitMessageBox frmParafaitMessageBox = new frmParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 4661), "Customer inactivation", MessageBoxButtons.YesNo);
                        DialogResult dr = frmParafaitMessageBox.ShowDialog();
                        if (dr == DialogResult.Yes)
                        {
                            parafaitDBTransaction.BeginTransaction();
                            DeleteCustomerBL deleteCustomerBL = new DeleteCustomerBL(utilities.ExecutionContext, customerDetailUI.CustomerDTO.Id);
                            deleteCustomerBL.DeleteCustomer(parafaitDBTransaction.SQLTrx);
                            parafaitDBTransaction.EndTransaction();
                            log.Debug("Successfully deleted customer " + customerDetailUI.CustomerDTO.Id);
                            Close();
                            displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer record has been inactivated"), WARNING);
                        }
                    }
                }
                catch (Exception ex)
                {
                    parafaitDBTransaction.RollBack();
                    frmParafaitMessageBox frmParafaitMessageBox = new frmParafaitMessageBox("Error occurred during inactivation of customer.Error:" + ex.Message, "Customer inactivation", MessageBoxButtons.OK);
                    frmParafaitMessageBox.ShowDialog();
                    log.Error(ex.Message, ex);
                }
            }
            log.LogMethodExit();
        }

        private void btnMembershipDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetLastTrxActivityTime();
            if ((CurrentCard != null && CurrentCard.customerDTO != null && CurrentCard.customerDTO.Id != -1) ||
               (CurrentCard == null && this.customerDTO != null && this.customerDTO.Id != -1))
            {
                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, (CurrentCard == null ? customerDTO : CurrentCard.customerDTO), true);
                if (customerBL.IsMember())
                {
                    using (frmMembershipDetails frmMembershipDetails = new frmMembershipDetails(customerBL))
                    {
                        frmMembershipDetails.ShowDialog();
                    }
                }
                else
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext,2289));
                }
            }
            else
            {
                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 2352, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer")), WARNING);
                //,'Sorry, can proceed only after &1 record is saved'
            }
            log.LogMethodExit();
        }


        private void btnWaivers_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetLastTrxActivityTime();
            try
            {
                if (this.customerDTO != null && this.customerDTO.Id != -1)
                {
                    WaiverSetContainer waiverSetContainer = null;
                    try
                    {
                        waiverSetContainer = WaiverSetContainer.GetInstance;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2435));//Unexpected error while getting waiver file details. Please check the setup
                    }
                    List<WaiverSetDTO> waiverSetDTOList = waiverSetContainer.GetWaiverSetDTOList(utilities.ExecutionContext.GetSiteId());
                    if (waiverSetDTOList != null && waiverSetDTOList.Any())
                    {
                        if (IncorrectCustomerSetupForWaiver)
                        {
                            if (string.IsNullOrEmpty(WaiverSetupErrorMsg))
                            {
                                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext,2316));
                            }
                            else
                            {
                                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext,WaiverSetupErrorMsg));
                            }
                            log.LogMethodExit(IncorrectCustomerSetupForWaiver, "incorrectCustomerSetupForWaiver");
                            return;
                        }
                        using (frmCustomerWaiverUI frm = new frmCustomerWaiverUI(utilities, this.customerDTO, null, POSUtils.ParafaitMessageBox))
                        { 
                            frm.ShowDialog();
                            CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, this.customerDTO.Id, true, true);
                            this.customerDTO = customerBL.CustomerDTO;
                            customerDetailUI.CustomerDTO = customerBL.CustomerDTO;
                            displayCustomerDetails(this.customerDTO);
                        }
                    }
                }
                else
                {
                    displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 2352, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer")), WARNING);
                    //,'Sorry, can proceed only after &1 record is saved'
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }


        private void btnCustomerSubscription_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetLastTrxActivityTime();
            try
            {
                if (this.customerDTO != null && this.customerDTO.Id != -1)
                {
                    SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(utilities.ExecutionContext);

                    List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_ID, this.customerDTO.Id.ToString()));
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                    List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, utilities, true);
                    
                    //if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
                    {
                        using (frmCustomerSubscription frm = new frmCustomerSubscription(utilities, customerDTO.Id, subscriptionHeaderDTOList))
                        { 
                            frm.ShowDialog();
                            displayCustomerDetails(this.customerDTO);
                        }
                    }
                }
                else
                {
                    displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 2352, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer")), WARNING);
                    //,'Sorry, can proceed only after &1 record is saved'
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void displayCustomerDetails(CustomerDTO customerDTO)
        {
            try
            {
                if (customerDetailUI != null)
                {
                    if (customerDTO == null)
                    {
                        customerDTO = new CustomerDTO();
                    }
                    customerDetailUI.CustomerDTO = customerDTO;
                    Control CurrentActiveTextBox = customerDetailUI.Controls.Find("txtFirstName", true)[0];
                    CurrentActiveTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }
        }

        private void btnCustomerRelationship_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastTrxActivityTime();
            if (customerDetailUI.CustomerDTO != null && customerDetailUI.CustomerDTO.Id > -1)
            {
                CustomerRelationshipListUI customerRelationshipListUI = new CustomerRelationshipListUI(utilities, customerDetailUI.CustomerDTO, POSUtils.ParafaitMessageBox);
                customerRelationshipListUI.ShowDialog();
            }
            log.LogMethodExit();
        }
    }
}
