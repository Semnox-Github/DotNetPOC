/********************************************************************************************
* Project Name - POS
* Description  - Parafait POS Main UI
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.70.3       14-Feb-2020     Lakshminarayana      Modified: Creating unregistered customer during check-in process
*2.110.0     24-Dec-2020      GUru S A             Subscription changes
*2.120.1     31-May-2021      Nitin Pai            Show Customer age on screen, Don't allow customer edit for active waivers
*2.130.10    08-Sep-2022      Nitin Pai            Modified as part of customer delete enhancement.
**********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Semnox.Core.Utilities; 
using Semnox.Parafait.Customer;
using Semnox.Parafait.ThirdParty;
using Semnox.Parafait.Communication;
using Semnox.Core.GenericUtilities;
using System.Globalization;
using Parafait_POS.Waivers;
using Semnox.Parafait.Product;
using Semnox.Parafait.Waiver;
using System.Linq;
using Semnox.Parafait.Transaction;
using Parafait_POS.Subscription;
using Semnox.Parafait.Languages;

namespace Parafait_POS
{
    public partial class POS
    {
        bool IsCalledFromProductClick = false;//Modification on 02-Jan-2017 for customerDTO feedback survey
        bool BypassRegisterCustomer = false;
        private void General_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Control tt = (Control)sender;
                Control nextCtl = tt.Parent.GetNextControl(tt, true);
                this.ActiveControl = nextCtl;
            }
        }


        private void btnRegisterCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (CurrentCard == null && POSStatic.REGISTER_CUSTOMER_WITHOUT_CARD == false)
            {
                displayMessageLine(MessageUtils.getMessage(247), ERROR);
                return;
            }
            
            customerRegistrationUI.CurrentCard = CurrentCard;
            customerRegistrationUI.IsCalledFromProductClick = IsCalledFromProductClick;
            customerRegistrationUI.CustomerDTO = CurrentCard == null ? ((customerDTO == null) ? null : customerDTO) : CurrentCard.customerDTO;
            customerRegistrationUI.NewTrx = NewTrx;
            customerRegistrationUI.IncorrectCustomerSetupForWaiver = incorrectCustomerSetupForWaiver;
            customerRegistrationUI.WaiverSetupErrorMsg = waiverSetupErrorMsg;
            customerRegistrationUI.BypassRegisterCustomer = BypassRegisterCustomer;
            customerRegistrationUI.ShowDialog();
            if(customerRegistrationUI.IsValid)
            {
                customerDTO = customerRegistrationUI.CustomerDTO;
                CurrentCard = customerRegistrationUI.CurrentCard;
            }
            else
            {
                if(customerRegistrationUI.CustomerDTO != null && customerRegistrationUI.CustomerDTO.Id > -1)
                {
                    CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerRegistrationUI.CustomerDTO.Id);
                    customerDTO = customerBL.CustomerDTO;
                }
                else
                {
                    customerDTO = null;
                }
            }
            IsCalledFromProductClick = customerRegistrationUI.IsCalledFromProductClick;
            BypassRegisterCustomer = customerRegistrationUI.BypassRegisterCustomer;
            SetCustomerTextBoxInfo();
            if (CurrentCard != null)
            {
                CurrentCard.getCardDetails(CurrentCard.CardNumber);
                displayCardDetails();
            }
            //Panel CustomerDetailsView = this.Controls["CustomerDetailsView"] as Panel;
            //if (CustomerDetailsView != null)
            //{
            //    CustomerDetailsView.Visible = true;
            //    CustomerDetailsView.BringToFront();
            //    customerDetailUI.CustomerDTO = CurrentCard == null ? ((customerDTO == null) ? new CustomerDTO() : customerDTO) : CurrentCard.customerDTO;
            //    CurrentActiveTextBox = customerDetailUI.Controls.Find("txtFirstName", true)[0];
            //    CurrentActiveTextBox.Focus();
            //}

            log.LogMethodExit();
        }

        //private void btnCloseCustomerView_Click(object sender, EventArgs e)
        //{
        //    log.Info("btnCloseCustomerView_Click - begin");
        //    int managerId = -1;
        //    if (IsCalledFromProductClick && customerDTO == null)
        //    {
        //        if (!Authenticate.Manager(ref managerId))
        //        {
        //            if (this.Controls["CustomerDetailsView"] != null)
        //                this.Controls["CustomerDetailsView"].Visible = false;
        //            displayMessageLine(MessageUtils.getMessage(1664), WARNING);
        //            throw new Exception(MessageUtils.getMessage(1664));
        //            //return;
        //        }
        //        else
        //        {
        //            BypassRegisterCustomer = true;
        //            log.Info("Manager Approval received to bypass customer registration. Manager Id: " + managerId);
        //        }
        //    }

        //    if (this.Controls["CustomerDetailsView"] != null)
        //        this.Controls["CustomerDetailsView"].Visible = false;
        //    if (IsCalledFromProductClick)//Starts:Modification 02-Jan-2017 for Customer feedback
        //    {
        //        if (Utilities.getParafaitDefaults("ENABLE_CUSTOMER_FEEDBACK_PROCESS").Equals("Y"))
        //        {
        //            PerformCustomerFeedback("Visit Count");
        //        }
        //    }//Ends:Modification 02-Jan-2017 for Customer feedback

        //    #region Merkle API Integration
        //    if (Utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION").Equals("Y") && customerDTO != null)
        //    {
        //        StartMerkleCall(customerDTO.CustomerCuponsDT, true, 0);
        //    }
        //    #endregion
        //    SetCustomerTextBoxInfo();
        //    log.Info("btnCloseCustomerView_Click - end");
        //}

        private void POS_KeyPress(object sender, KeyPressEventArgs e)
        {
            lastTrxActivityTime = DateTime.Now;

            if (tabControlCardAction.TabPages.Count == 0)
                return;

            if (tabControlCardAction.SelectedTab.Name == "tabPageCardCustomer"
                || tabControlSelection.SelectedTab.Name == "tabPageSystem"
                || txtProductSearch.Focused
                || nudQuantity.Focused
                || (customerRegistrationUI != null && customerRegistrationUI.Visible)
               )
            {
                if (this.customerRegistrationUI != null && customerRegistrationUI.Visible)
                {
                    //if (tabControlCardAction.SelectedTab.Name == "tabPageCardCustomer")
                    if (CurrentCard == null && POSStatic.REGISTER_CUSTOMER_WITHOUT_CARD == false)
                    {
                        displayMessageLine(MessageUtils.getMessage(247), ERROR);
                        e.Handled = true;
                    }
                }
                return;
            }
            //Start Modification : For adding card textbox in activities tab on 14-Feb-2017
            if (tabControlCardAction.SelectedTab.Name == "tabPageActivities")
            {
                return;
            }
            if (tcOrderView.Visible && tcOrderView.SelectedTab.Name == "tpOrderOrderView")
            {
                return;
            }
            if (char.IsNumber(e.KeyChar) || e.KeyChar == POSStatic.decimalChar)
                showNumberPadForm(e.KeyChar);
        }

        //private void buttonCustomerSave_Click(object sender, EventArgs e)
        //{
        //    lastTrxActivityTime = DateTime.Now;
        //    if (CurrentCard == null && POSStatic.REGISTER_CUSTOMER_WITHOUT_CARD == false)
        //    {
        //        displayMessageLine(MessageUtils.getMessage(247), ERROR);
        //        return;
        //    }

        //    List<ValidationError> validationErrorList;
        //    customerDetailUI.ClearValdationErrors();
        //    validationErrorList = customerDetailUI.UpdateCustomerDTO();
        //    CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDetailUI.CustomerDTO);
        //    try
        //    {
        //        validationErrorList.AddRange(customerBL.Validate());
        //        if (validationErrorList.Count > 0)
        //        {
        //            customerDetailUI.ShowValidationError(validationErrorList);
        //            displayMessageLine(validationErrorList[0].Message, ERROR);
        //            return;
        //        }
        //    }
        //    catch (ValidationException ex)
        //    {
        //        customerBL = new CustomerBL(Utilities.ExecutionContext, customerDetailUI.CustomerDTO.Id);
        //        customerDTO = customerBL.CustomerDTO;
        //        customerDetailUI.CustomerDTO = customerBL.CustomerDTO;
        //        customerDetailUI.UpdateCustomerDTO();
        //        SetCustomerTextBoxInfo();
        //        validationErrorList = new List<ValidationError>();
        //        validationErrorList.Add(new ValidationError("Customer", "Active waiver found", ex.Message));
        //        customerDetailUI.ShowValidationError(validationErrorList);
        //        displayMessageLine(validationErrorList[0].Message, ERROR);
        //        return;
        //    }



        //    customerDTO = customerDetailUI.CustomerDTO;
        //    if (CurrentCard != null)
        //        CurrentCard.customerDTO = customerDTO;
        //    else if (CurrentCard == null
        //        && customerDTO != null
        //        && customerDTO.Id <= -1
        //        && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N"))
        //    {
        //        string strProdId = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOAD_PRODUCT_ON_REGISTRATION");
        //        int productId = -1;
        //        if (int.TryParse(strProdId, out productId) == true && productId != -1)
        //        {
        //            Products regProductBL = new Products(productId);
        //            ProductsDTO regProductDTO = regProductBL.GetProductsDTO;
        //            if (regProductDTO.AutoGenerateCardNumber.Equals("Y"))
        //            {
        //                RandomTagNumber randomCardNumber = new RandomTagNumber(Utilities.ExecutionContext);
        //                if (CurrentCard == null)
        //                {
        //                    CurrentCard = new Semnox.Parafait.Transaction.Card(randomCardNumber.Value, Utilities.ParafaitEnv.LoginID, Utilities);
        //                    CurrentCard.primaryCard = "Y"; //Assign auto gen card as primary card
        //                }
        //                SqlConnection connection = Utilities.getConnection();
        //                SqlTransaction sqlTransaction = connection.BeginTransaction();
        //                try
        //                {
        //                    CurrentCard.createCard(sqlTransaction);
        //                    sqlTransaction.Commit();
        //                    CurrentCard.customerDTO = customerDTO;
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message);
        //                    displayMessageLine(MessageUtils.getMessage(253), WARNING);
        //                    sqlTransaction.Rollback();
        //                    return;
        //                }
        //                finally
        //                {
        //                    connection.Close();
        //                }
        //            }
        //        }
        //    }

        //    if (CurrentCard != null && CurrentCard.card_id == -1)
        //    {
        //        displayMessageLine(MessageUtils.getMessage(253), WARNING);
        //        return;
        //    }

        //    try
        //    {
        //        if (customerDTO.Id == -1 || (CurrentCard != null && CurrentCard.customer_id != customerDTO.Id))
        //        {
        //            SqlConnection connection = Utilities.getConnection();
        //            SqlTransaction transaction = connection.BeginTransaction();
        //            try
        //            {

        //                customerBL.Save(transaction);
        //                transaction.Commit();
        //            }
        //            catch (ValidationException ex)
        //            {
        //                customerDetailUI.ShowValidationError(ex.ValidationErrorList);
        //                displayMessageLine(ex.Message, ERROR);
        //                transaction.Rollback();
        //                return;
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //                displayMessageLine(ex.Message, ERROR);
        //                transaction.Rollback();
        //                return;
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //            if (CurrentCard != null)
        //            {
        //                CurrentCard.updateCustomer();
        //            }

        //            added for capillary API Integration
        //            if (Utilities.getParafaitDefaults("ENABLE_CAPILLARY_INTEGRATION").Equals("Y") && !string.IsNullOrEmpty(customerDTO.PhoneNumber))
        //                {
        //                    Added on 26 - sep - 2016 for showing capillary response awaited message
        
        //                        displayMessageLine("Please wait..Response awaited from Capillary...", WARNING);
        //                    end
        //                string msg = "";
        //                    objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();
        //                    objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.IsCustomerExist(customerDTO.PhoneNumber, ref msg);

        //                    Check customer Exist in Capillary
        //                if (objLoyaltyRedemptionDTO != null)
        //                    {
        //                        if (!objLoyaltyRedemptionDTO.success && !objLoyaltyRedemptionDTO.item_status)
        //                        {
        //                            Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "N", customerDTO.Id.ToString(), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
        //                            AddCustomersToCapillary();
        //                        }
        //                        else
        //                        {
        //                            Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "Y", customerDTO.Id.ToString(), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage("API call failed: " + msg), "Capillary API Response");
        //                        Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", msg, "CustomerExist", 0, "N", customerDTO.Id.ToString(), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
        //                        textBoxMessageLine.Text = msg;
        //                    }

        //                    AddPendingCustomersToCapillary(); //Add pending customers to Capillary
        //                }
        //            end
        //        }
        //        else // customer already exists. no need to update card
        //        {
        //            SqlConnection connection = Utilities.getConnection();
        //            SqlTransaction transaction = connection.BeginTransaction();
        //            try
        //            {

        //                customerBL.Save(transaction);
        //                transaction.Commit();
        //            }
        //            catch (ValidationException ex)
        //            {
        //                customerDetailUI.ShowValidationError(ex.ValidationErrorList);
        //                displayMessageLine(MessageUtils.getMessage(253), WARNING);
        //                transaction.Rollback();
        //                return;
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //                displayMessageLine(MessageUtils.getMessage(253), WARNING);
        //                transaction.Rollback();
        //                return;
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //            if (CurrentCard != null)
        //                CurrentCard.updateCustomer();

        //            Added on 1 - june - 2016 for Updating customerDetails to Capillary
        //                if (Utilities.getParafaitDefaults("ENABLE_CAPILLARY_INTEGRATION").Equals("Y") && !string.IsNullOrEmpty(customerDTO.PhoneNumber))
        //                {
        //                    Added on 26 - sep - 2016 for showing capillary response awaited message
        
        //                        displayMessageLine("Please wait..Response awaited from Capillary...", WARNING);
        //                    end

        //                string msg = "";
        //                    objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();
        //                    objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.IsCustomerExist(customerDTO.PhoneNumber, ref msg);

        //                    if (objLoyaltyRedemptionDTO != null) //Time-Out Exception occured 
        //                    {
        //                        Check customer Exist in Capillary
        //                        if (!objLoyaltyRedemptionDTO.success && !objLoyaltyRedemptionDTO.item_status)
        //                        {
        //                            Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "N", customerDTO.Id.ToString(), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
        //                            AddCustomersToCapillary();
        //                        }
        //                        else //Customer Exist Update Details in Capillary
        //                        {
        //                            Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "Y", customerDTO.Id.ToString(), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);

        //                            API Call to Update customer in capillary
        //                            objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.UpdateCustomerToCapillary(customerDTO);

        //                            if (objLoyaltyRedemptionDTO.success && objLoyaltyRedemptionDTO.item_status)
        //                            {
        //                                Added on 26 - sep - 2016 for clearing capillary response awaited message
        
        //                                    displayMessageLine("", MESSAGE);
        //                                end
        //                                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage("Customer details successfully interfaced to Capillary"), "Capillary API Response");
        //                                Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called UpdateCustomerToCapillary() method", objLoyaltyRedemptionDTO.message, "CustomerUpdate", 0, "Y", customerDTO.Id.ToString(), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
        //                            }
        //                            else
        //                            {
        //                                Added on 26 - sep - 2016 for clearing capillary response awaited message
        
        //                                    displayMessageLine("", MESSAGE);
        //                                end
        //                                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage("Failed to interface customer details to Capillary: " + objLoyaltyRedemptionDTO.message), "Capillary API Response");
        //                                Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called UpdateCustomerToCapillary() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "N", customerDTO.Id.ToString(), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage("API call failed: " + msg), "Capillary API Response");
        //                        Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", msg, "CustomerExist", 0, "N", customerDTO.Id.ToString(), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
        //                        textBoxMessageLine.Text = msg;
        //                    }
        //                }
        //            end
        //        }
        //    Begin: 19 - Jan - 2016 to display customerDTO info below card details in POS
        //        SetCustomerTextBoxInfo();
        //    End: 19 - Jan - 2016 to display customer info below card details in POS

        //        if (CurrentCard != null)
        //        {
        //            CurrentCard.getCardDetails(CurrentCard.CardNumber);
        //            displayCardDetails();
        //        }

        //        displayMessageLine(MessageUtils.getMessage(254), MESSAGE);
        //        Panel CustomerDetailsView = this.Controls["CustomerDetailsView"] as Panel;//Starts:Modification on 02-Jan-2017 for customer feedback 
        //        if (CustomerDetailsView != null)
        //        {
        //            CustomerDetailsView.Visible = false;
        //            CustomerDetailsView.SendToBack();
        //            if (IsCalledFromProductClick)
        //            {
        //                if (Utilities.getParafaitDefaults("ENABLE_CUSTOMER_FEEDBACK_PROCESS").Equals("Y"))
        //                {
        //                    PerformCustomerFeedback("Visit Count");
        //                }
        //            }
        //        }//Ends:Modification on 02-Jan-2017 for customer feedback 

        //        Added for merkle Integration
        //        #region Merkle API Integration
        //            if (Utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION").Equals("Y") && customerDTO != null)
        //            {
        //                StartMerkleCall(customerDTO.CustomerCuponsDT, true, 0);
        //            }
        //        #endregion
        //        end
        //    }
        //    catch (Exception ex)
        //    {
        //        displayMessageLine(MessageUtils.getMessage(255, ex.Message), ERROR);
        //    }
        //}

        private void SetCustomerTextBoxInfo()
        {
            log.LogMethodEntry();
            if (customerDTO != null)
            {
                if (customerDTO.Id == -1)
                {
                    textBoxCustomerInfo.Text = "";
                }
                else
                {
                    textBoxCustomerInfo.Text = customerDTO.FirstName + (string.IsNullOrEmpty(customerDTO.LastName) ? "" : " " + customerDTO.LastName);
                    if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0 && !string.IsNullOrEmpty(customerDTO.LatestAddressDTO.City))
                        textBoxCustomerInfo.AppendText(", " + customerDTO.LatestAddressDTO.City);
                    textBoxCustomerInfo.AppendText(Environment.NewLine);

                    if (customerDTO.DateOfBirth != null)
                    {
                        textBoxCustomerInfo.AppendText("DOB: " + customerDTO.DateOfBirth.Value.ToString("M"));
                        if (customerDTO.DateOfBirth.Value.DayOfYear >= DateTime.Now.DayOfYear - 1 &&
                            customerDTO.DateOfBirth.Value.DayOfYear <= DateTime.Now.DayOfYear + 1)
                        {
                            textBoxCustomerInfo.BackColor = Color.Red;
                            textBoxCustomerInfo.ForeColor = Color.Black;
                        }
                    }

                    // Modification by Indrajeet - To Display AGE
                    textBoxCustomerInfo.AppendText(Environment.NewLine);
                    if (customerDTO.DateOfBirth != null)
                    {
                        CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                        int customerAge = customerBL.GetAge();
                        // Config check to display customer Age
                        if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "SHOW_CUSTOMER_AGE_ONSCREEN").Equals("Y"))
                        {
                            textBoxCustomerInfo.AppendText(MessageContainerList.GetMessage(Utilities.ExecutionContext,"AGE") + ": " + + customerAge);
                        }                                               
                    }
                    SetTxtVIPStatusTextBox();
                }
            }
            log.LogMethodExit();
        }

        //private void btnCustomerRelationship_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    if (customerDetailUI.CustomerDTO != null && customerDetailUI.CustomerDTO.Id > -1)
        //    {
        //        CustomerRelationshipListUI customerRelationshipListUI = new CustomerRelationshipListUI(Utilities, customerDetailUI.CustomerDTO, POSUtils.ParafaitMessageBox);
        //        customerRelationshipListUI.ShowDialog();
        //    }
        //    log.LogMethodExit();
        //}

        //#region Loyalty Redemption Methods
        //public void AddPendingCustomersToCapillary()
        //{
        //    string msg = "";
        //    DataTable dt = Utilities.executeDataTable(@"SELECT isnull(Value,-1) as CustomerId 
        //                                               FROM EventLog 
        //                                               WHERE Source ='Loyalty Redemption' AND Category = 'CustomerInsert' AND Name = 'N'");

        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow rw in dt.Rows)
        //        {
        //            DataTable customerData = Utilities.executeDataTable(@"SELECT customer_name, last_name, email, gender, contact_phone1 
        //                                                                   FROM customers where customer_id = @custId",
        //                                                                   new SqlParameter("@custId", rw["CustomerId"].ToString()));

        //            if (customerData != null && customerData.Rows.Count > 0)
        //            {
        //                objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();

        //                CustomerDTO CapillaryCustomer = (new CustomerBL(Utilities.ExecutionContext, Convert.ToInt32(rw["CustomerId"]))).CustomerDTO;

        //                objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.IsCustomerExist(CapillaryCustomer.PhoneNumber, ref msg);
        //                if (objLoyaltyRedemptionDTO.success && objLoyaltyRedemptionDTO.item_status)
        //                    continue;

        //                objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.AddCustomerToCapillary(CapillaryCustomer);
        //                if (objLoyaltyRedemptionDTO.success && objLoyaltyRedemptionDTO.item_status)
        //                {
        //                    Utilities.executeDataTable(@"UPDATE EventLog 
        //                                                 SET Name = 'Y', Username = @userName, Computer =@posMachine
        //                                                 WHERE Source ='Loyalty Redemption' AND Category = 'CustomerInsert' AND Value = @custId",
        //                                                 new SqlParameter("@custId", rw["CustomerId"].ToString()),
        //                                                 new SqlParameter("@userName", Utilities.ParafaitEnv.LoginID),
        //                                                 new SqlParameter("@posMachine", Utilities.ParafaitEnv.POSMachine));
        //                }
        //            }
        //        }
        //    }
        //}

        public LoyaltyRedemptionDTO AddCustomersToCapillary()
        {
            objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();
            objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.AddCustomerToCapillary(customerDTO);

            if (!objLoyaltyRedemptionDTO.success && !objLoyaltyRedemptionDTO.item_status)
            {
                Utilities.executeScalar(@"IF EXISTS (SELECT 1 FROM EventLog 
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
                                                        new SqlParameter("@userName", Utilities.ParafaitEnv.LoginID),
                                                        new SqlParameter("@posMachine", Utilities.ParafaitEnv.POSMachine));
                //Added on 26-sep-2016 for clearing capillary Response message
                displayMessageLine("", MESSAGE);
                //end
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage("Failed to interface customer details into Capillary: " + objLoyaltyRedemptionDTO.message), "Capillary API Response");
            }
            else
            {
                //Added on 26-sep-2016 for clearing capillary Response message
                displayMessageLine("", MESSAGE);
                //end
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage("Customer details successfully interfaced to Capillary"), "Capillary API Response");
                Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called AddCustomersToCapillary method ", objLoyaltyRedemptionDTO.message, "CustomerInsert", 0, "Y", customerDTO.Id.ToString(), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
            }
            return objLoyaltyRedemptionDTO;
        }
        //#endregion

        //private void btnVerifyCustomer_Click(object sender, EventArgs e)
        //{
        //    lastTrxActivityTime = DateTime.Now;
        //    if (CurrentCard == null)
        //    {
        //        displayMessageLine(MessageUtils.getMessage(257), WARNING);
        //        return;
        //    }
        //    customerDetailUI.UpdateCustomerDTO();
        //    if (customerDetailUI.CustomerDTO.CustomerType == CustomerType.UNREGISTERED)
        //    {
        //        displayMessageLine(MessageUtils.getMessage(2491), WARNING);
        //        return;
        //    }
        //    if (CurrentCard.customerDTO == null)
        //    {
        //        CurrentCard.customerDTO = customerDetailUI.CustomerDTO;
        //    }

        //    if (CurrentCard.customerDTO.Verified)
        //    {
        //        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1453));
        //        return;
        //    }

        //    try
        //    {
        //        CustomerVerificationUI customerVerificationUI = new CustomerVerificationUI(Utilities, customerDetailUI.CustomerDTO, POSUtils.ParafaitMessageBox);
        //        if (customerVerificationUI.ShowDialog() == DialogResult.OK)
        //        {
        //            customerDetailUI.RefreshBindings();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured while verifying the customer", ex);
        //        POSUtils.ParafaitMessageBox(ex.Message);
        //    }
        //}

        //private void txtUniqueId_Validating(object sender, CancelEventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    if (CurrentCard == null)
        //        return;

        //    if ((sender as TextBox).Text.Trim() == "" && ParafaitEnv.UNIQUE_ID_MANDATORY_FOR_VIP == "Y" && CurrentCard.vip_customer == 'Y')
        //    {
        //        e.Cancel = true;
        //        displayMessageLine(MessageUtils.getMessage(289), WARNING);
        //        return;
        //    }
        //    displayMessageLine("", MESSAGE);

        //    if ((sender as TextBox).Text.Trim() == "")
        //        return;

        //    List<CustomerDTO> customerDTOList = null;
        //    CustomerListBL customerListBL = new CustomerListBL(Utilities.ExecutionContext);
        //    CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.EQUAL_TO, (sender as TextBox).Text.Trim());
        //    if (customerDetailUI.CustomerDTO != null && customerDetailUI.CustomerDTO.Id >= 0)
        //    {
        //        customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_ID, Operator.NOT_EQUAL_TO, customerDetailUI.CustomerDTO.Id);
        //    }
        //    customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
        //                          .Paginate(0, 20);
        //    customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, false, false);
        //    if (customerDTOList != null && customerDTOList.Count > 0)
        //    {

        //        if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(290), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Customer Registration"), MessageBoxButtons.OK) == System.Windows.Forms.DialogResult.OK)
        //        {
        //            if (ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
        //                e.Cancel = true;
        //        }
        //        else
        //        {
        //            displayMessageLine(MessageUtils.getMessage(290), WARNING);
        //            if (ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
        //            {
        //                e.Cancel = true;
        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        //private void btnLookupCustomer_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    try
        //    {
        //        CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities,
        //                                                                 customerDetailUI.FirstName,
        //                                                                 customerDetailUI.MiddleName,
        //                                                                 customerDetailUI.LastName,
        //                                                                 customerDetailUI.Email,
        //                                                                 customerDetailUI.PhoneNumber,
        //                                                                 customerDetailUI.UniqueIdentifier);
        //        if (customerLookupUI.ShowDialog() == DialogResult.OK)
        //        {
        //            if (customerLookupUI.SelectedCustomerDTO != null)
        //            {
        //                if (CurrentCard == null)
        //                {
        //                    if (POSStatic.REGISTER_CUSTOMER_WITHOUT_CARD == false)
        //                        displayMessageLine(MessageUtils.getMessage(247), WARNING);
        //                    else
        //                    {
        //                        customerDTO = customerLookupUI.SelectedCustomerDTO;
        //                        customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
        //                        displayCustomerDetails(customerDTO);
        //                    }
        //                }
        //                else if (customerLookupUI.SelectedCustomerDTO.Id != CurrentCard.customer_id)
        //                {
        //                    CurrentCard.customerDTO = customerLookupUI.SelectedCustomerDTO;
        //                    customerDTO = customerLookupUI.SelectedCustomerDTO; //assign customerDTO details to trx customerDTO object
        //                    customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
        //                    displayCustomerDetails(customerDTO);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        POSUtils.ParafaitMessageBox(ex.Message);
        //    }
        //    log.LogMethodExit();
        //}

        private void customerDetailUI_CustomerContactInfoEntered(object sender, CustomerContactInfoEnteredEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerListBL customerListBL = new CustomerListBL(Utilities.ExecutionContext);
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
                    if (customerDTOItem.Id != customerRegistrationUI.CustomerDTO.Id)
                    {
                        duplicateCustomerFound = true;
                        break;
                    }
                }
                if (duplicateCustomerFound)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ENABLE_CAPILLARY_INTEGRATION"))
                    {
                        DialogResult result = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1452), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Validate Phone / Email"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        showCustomerLookup = true;
                    }
                    else
                    {
                        DialogResult result = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 524), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Validate Phone / Email"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK || result == DialogResult.Yes)
                        {
                            showCustomerLookup = true;
                        }
                    }
                    if (showCustomerLookup)
                    {
                        if (customerDTOList.Count > 1)
                        {
                            CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities, "", "", "",
                                                                                     e.ContactType == ContactType.EMAIL ? e.ContactValue : "",
                                                                                     e.ContactType == ContactType.PHONE ? e.ContactValue : "",
                                                                                     "");
                            if (customerLookupUI.ShowDialog() == DialogResult.OK)
                            {
                                customerRegistrationUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                            }
                            else
                            {
                                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ENABLE_CAPILLARY_INTEGRATION"))
                                {
                                    if (customerRegistrationUI.CustomerDTO.Id != customerDTOList[0].Id)
                                    {
                                        customerRegistrationUI.CustomerDTO = customerDTOList[0];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (customerRegistrationUI.CustomerDTO.Id != customerDTOList[0].Id)
                            {
                                customerRegistrationUI.CustomerDTO = customerDTOList[0];
                            }
                        }

                        if (CurrentCard == null)
                        {
                            customerDTO = customerRegistrationUI.CustomerDTO;
                            customerRegistrationUI.CustomerDTO = customerRegistrationUI.CustomerDTO;
                        }
                        else if (customerRegistrationUI.CustomerDTO.Id != CurrentCard.customer_id)
                        {
                            CurrentCard.customerDTO = customerRegistrationUI.CustomerDTO;
                            customerDTO = customerRegistrationUI.CustomerDTO;
                        }
                    }
                }

            }
            log.LogMethodExit();
        }
        //private void btnInActivate_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();
        //    int managerId = -1;
        //    using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
        //    {
        //        try
        //        {
        //            if (Authenticate.Manager(ref managerId))
        //            {
        //                // use using
        //                log.Debug("Deleteting customer " + customerDetailUI.CustomerDTO.Id);
        //                // Show a confirmation message
        //                frmParafaitMessageBox frmParafaitMessageBox = new frmParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4661), "Customer inactivation", MessageBoxButtons.YesNo);
        //                DialogResult dr = frmParafaitMessageBox.ShowDialog();
        //                if (dr == DialogResult.Yes)
        //                {
        //                    parafaitDBTransaction.BeginTransaction();
        //                    DeleteCustomerBL deleteCustomerBL = new DeleteCustomerBL(Utilities.ExecutionContext, customerDetailUI.CustomerDTO.Id);
        //                    deleteCustomerBL.DeleteCustomer(parafaitDBTransaction.SQLTrx);
        //                    parafaitDBTransaction.EndTransaction();
        //                    log.Debug("Successfully deleted customer " + customerDetailUI.CustomerDTO.Id);
        //                    if (this.Controls["CustomerDetailsView"] != null)
        //                        this.Controls["CustomerDetailsView"].Visible = false;
        //                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Customer record has been inactivated"), WARNING);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            parafaitDBTransaction.RollBack();
        //            frmParafaitMessageBox frmParafaitMessageBox = new frmParafaitMessageBox("Error occurred during inactivation of customer.Error:" + ex.Message, "Customer inactivation", MessageBoxButtons.OK);
        //            frmParafaitMessageBox.ShowDialog();
        //            log.Error(ex.Message, ex);
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        //private void btnMembershipDetails_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();

        //    if ((CurrentCard != null && CurrentCard.customerDTO != null && CurrentCard.customerDTO.Id != -1) ||
        //       (CurrentCard == null && this.customerDTO != null && this.customerDTO.Id != -1))
        //    {
        //        CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, (CurrentCard == null ? customerDTO : CurrentCard.customerDTO), true);
        //        if (customerBL.IsMember())
        //        {
        //            using (frmMembershipDetails frmMembershipDetails = new frmMembershipDetails(customerBL))
        //            {
        //                frmMembershipDetails.ShowDialog();
        //            }
        //        }
        //        else
        //        {
        //            POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2289));
        //        }
        //    }
        //    else
        //    {
        //        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2352, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Customer")), WARNING);
        //        //,'Sorry, can proceed only after &1 record is saved'
        //    }
        //    log.LogMethodExit();
        //}


        //private void btnWaivers_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();
        //    try
        //    {
        //        if (this.customerDTO != null && this.customerDTO.Id != -1)
        //        {
        //            WaiverSetContainer waiverSetContainer = null;
        //            try
        //            {
        //                waiverSetContainer = WaiverSetContainer.GetInstance;
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error(ex);
        //                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2435));//Unexpected error while getting waiver file details. Please check the setup
        //            }
        //            List<WaiverSetDTO> waiverSetDTOList = waiverSetContainer.GetWaiverSetDTOList(Utilities.ExecutionContext.GetSiteId());
        //            if (waiverSetDTOList != null && waiverSetDTOList.Any())
        //            {
        //                if (incorrectCustomerSetupForWaiver)
        //                {
        //                    if (string.IsNullOrEmpty(waiverSetupErrorMsg))
        //                    {
        //                        POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2316));
        //                    }
        //                    else
        //                    {
        //                        POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(waiverSetupErrorMsg));
        //                    }
        //                    log.LogMethodExit(incorrectCustomerSetupForWaiver, "incorrectCustomerSetupForWaiver");
        //                    return;
        //                }
        //                using (frmCustomerWaiverUI frm = new frmCustomerWaiverUI(Utilities, this.customerDTO, null, POSUtils.ParafaitMessageBox))
        //                {
        //                    if (frm.Width > this.Width + 28)
        //                    {
        //                        frm.Width = this.Width - 30;
        //                    }
        //                    frm.ShowDialog();
        //                    CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, this.customerDTO.Id, true, true);
        //                    this.customerDTO = customerBL.CustomerDTO;
        //                    customerDetailUI.CustomerDTO = customerBL.CustomerDTO;
        //                    displayCustomerDetails(this.customerDTO);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2352, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Customer")), WARNING);
        //            //,'Sorry, can proceed only after &1 record is saved'
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1824, ex.Message));
        //    }
        //    log.LogMethodExit();
        //}


        //private void btnCustomerSubscription_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();
        //    try
        //    {
        //        if (this.customerDTO != null && this.customerDTO.Id != -1)
        //        {
        //            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(Utilities.ExecutionContext);

        //            List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
        //            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_ID, this.customerDTO.Id.ToString()));
        //            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
        //            List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, Utilities, true);

        //            //if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
        //            {
        //                using (frmCustomerSubscription frm = new frmCustomerSubscription(Utilities, customerDTO.Id, subscriptionHeaderDTOList))
        //                {
        //                    if (frm.Width > this.Width + 28)
        //                    {
        //                        frm.Width = this.Width - 30;
        //                    }
        //                    frm.ShowDialog();
        //                    displayCustomerDetails(this.customerDTO);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2352, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Customer")), WARNING);
        //            //,'Sorry, can proceed only after &1 record is saved'
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1824, ex.Message));
        //    }
        //    log.LogMethodExit();
        //}
    }
}
