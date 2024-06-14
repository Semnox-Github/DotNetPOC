/********************************************************************************************
 * Project Name - CustomerStatic
 * Description  - user interface
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00                                        Created 
 *2.4.0       25-Nov-2018      Raghuveera     terms and condition is passing to the customer form
 *2.80        06-Sep-2019      Deeksha        Added logger methods.
 *2.150.0.0   23-Sep-2022      Sathyavathi    Check-In feature Phase-2
 *2.150.1     22-Feb-2023      Guru S A       Kiosk Cart Enhancements
 *2.150.7     10-Nov-2023      Sathyavathi    Customer Lookup Enhancement
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public static class CustomerStatic
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// ShowCustomerScreen
        /// </summary> 
        /// <returns></returns>
        public static CustomerDTO ShowCustomerScreen(string CardNumber = "")
        {
            log.LogMethodEntry(CardNumber);
            try
            {
                if (KioskStatic.ShowRegistrationTAndC)
                {
                    bool show = true;
                    if (!string.IsNullOrEmpty(CardNumber))
                    {
                        Card card = new Card(CardNumber, "", KioskStatic.Utilities);
                        if (card.customer_id > -1)
                            show = false; // customer already registered, dont show age gate and T&C
                    }

                    if (show)
                    {
                        if (KioskStatic.Utilities.getParafaitDefaults("SHOW_REGISTRATION_AGE_GATE").Equals("Y"))
                        {
                            using (frmAgeGate fa = new frmAgeGate(CardNumber))
                            {
                                DialogResult dr = fa.ShowDialog();
                                if (dr != System.Windows.Forms.DialogResult.Yes)
                                {
                                    if (dr == DialogResult.Cancel)
                                    {
                                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Timeout");
                                        throw new TimeoutOccurred(msg);
                                    }
                                    else
                                    {
                                        log.LogMethodExit();
                                        return null;
                                    }
                                }
                                else
                                {
                                    CheckForTermsAndConditions(fa.customerDTO);
                                    log.LogMethodExit(fa.customerDTO);
                                    return fa.customerDTO;
                                }
                            }
                        }
                        using (frmTermsAndConditions frt = new frmTermsAndConditions(KioskStatic.ApplicationContentModule.REGISTRATION))
                        {
                            DialogResult dr = frt.ShowDialog();
                            if (dr != System.Windows.Forms.DialogResult.Yes)
                            {
                                if (dr == DialogResult.Cancel)
                                {
                                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Timeout");
                                    throw new TimeoutOccurred(msg);
                                }
                                else
                                {
                                    log.LogMethodExit();
                                    return null;
                                }
                            }
                            else
                            {
                                using (Customer fcustomer = new Customer(CardNumber, null, true))
                                {
                                    DialogResult drr = fcustomer.ShowDialog();
                                    if (drr == DialogResult.Cancel)
                                    {
                                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Timeout");
                                        throw new TimeoutOccurred(msg);
                                    }
                                    else
                                    {
                                        CheckForTermsAndConditions(fcustomer.customerDTO);
                                        log.LogMethodExit(fcustomer.customerDTO);
                                        return fcustomer.customerDTO;
                                    }
                                }
                            }
                        }
                    }
                }

                using (Customer customer = new Customer(CardNumber))
                { 
                    if (customer.ShowDialog() == DialogResult.Cancel)
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Timeout");
                        throw new TimeoutOccurred(msg);
                    }
                    else
                    {
                        CheckForTermsAndConditions(customer.customerDTO);
                        log.LogMethodExit(customer.customerDTO);
                        return customer.customerDTO;
                    }
                }
            }
            catch (TimeoutOccurred ex)
            {
                log.Error(ex); 
                KioskStatic.logToFile("Error in ShowCustomerScreen() of CustomerStatic: " + ex.Message);
                log.LogMethodExit("timeout");
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in ShowCustomerScreen() of CustomerStatic: " + ex.Message);
                log.LogMethodExit(null);
                return null;
            }
        }
        /// <summary>
        /// ShowCustomerScreen
        /// </summary> 
        /// <returns></returns>
        public static CustomerDTO ShowCustomerScreen(CustomerDTO selectedCustomerDTO)
        {
            log.LogMethodEntry((selectedCustomerDTO != null ? selectedCustomerDTO.Id : -1));
            try
            {
                if (selectedCustomerDTO == null)
                {
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1144,
                                               MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Customer DTO"));
                    throw new ValidationException(msg);
                }
                using (Customer customer = new Customer(selectedCustomerDTO.CardNumber, null, false, selectedCustomerDTO))
                {
                    if (customer.ShowDialog() == DialogResult.Cancel)
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Timeout");
                        throw new TimeoutOccurred(msg);
                    }
                    else
                    {
                        CheckForTermsAndConditions(customer.customerDTO);
                        log.LogMethodExit(customer.customerDTO);
                        return customer.customerDTO;
                    }
                }
            }
            catch (TimeoutOccurred ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in ShowCustomerScreen() of CustomerStatic: " + ex.Message);
                log.LogMethodExit("timeout");
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in ShowCustomerScreen() of CustomerStatic: " + ex.Message);
                log.LogMethodExit(null);
                return null;
            }
            log.LogMethodExit();
        }

        public static void CheckForTermsAndConditions(CustomerDTO customerDTO)
        {
            log.LogMethodEntry((customerDTO != null) ? customerDTO.Id : -1);
            if (customerDTO != null
                && (ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "TERMS_AND_CONDITIONS").Equals("M")
                || ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "TERMS_AND_CONDITIONS").Equals("MU"))
                && !customerDTO.PolicyTermsAccepted)
            {
                bool isPolicyAccepted = AlerUserToAcceptTermsAndConditions(customerDTO);
                if (isPolicyAccepted == false)
                {
                    customerDTO = null;
                    log.LogVariableState("customerDTO.Id : ", (customerDTO != null) ? customerDTO.Id : -1);
                    //1206 - Please accept our Terms and Conditions to proceed
                    throw new ValidationException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1206));
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// LinkCustomerToTheTransaction
        /// </summary> 
        /// <returns></returns>
        public static bool LinkCustomerToTheTransaction(KioskTransaction kioskTransaction,
            ExecutionContext executionContext, bool isRegisteredCustomerOnly, bool showMsgRecommendCustomerToRegister)
        {
            log.LogMethodEntry("kioskTransaction", "executionContext", isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
            CustomerDTO customerDTO = kioskTransaction.GetTransactionCustomer();
            bool isLinked = true;
            try
            {
                if (customerDTO == null)
                {
                    if (KioskStatic.RegisterBeforePurchase || isRegisteredCustomerOnly == true)
                    {
                        using (frmSearchCustomer frmSearchCustomer = new frmSearchCustomer(isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister))
                        {
                            DialogResult dr = frmSearchCustomer.ShowDialog();
                            if (dr == DialogResult.OK)
                            {
                                customerDTO = frmSearchCustomer.GetCustomerDTO;
                            }
                            else if (dr == DialogResult.Cancel)
                            {
                                string msg = MessageContainerList.GetMessage(executionContext, "Timeout");
                                throw new TimeoutOccurred(msg);
                            }
                        }
                    }
                }
                CheckForTermsAndConditions(customerDTO);
                if (kioskTransaction.GetTransactionCustomer() == null && customerDTO != null)
                {
                    kioskTransaction.SetTransactionCustomer(customerDTO);
                }

            }
            catch (TimeoutOccurred ex)
            {
                log.Error(ex);
                isLinked = false;
                KioskStatic.logToFile("Error in LinkCustomerToTrxIfMandatory() of CustomerStatic: " + ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                isLinked = false;
                KioskStatic.logToFile("Error in LinkCustomerToTrxIfMandatory() of CustomerStatic: " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                if (kioskTransaction.GetTransactionCustomer() == null)
                {
                    isLinked = false;
                }
            }
            log.LogMethodExit(isLinked);
            return isLinked;
        }

        private static bool AlerUserToAcceptTermsAndConditions(CustomerDTO selectedCustomerDTO)
        {
            log.LogMethodEntry((selectedCustomerDTO != null ? selectedCustomerDTO.Id : -1));
            bool status = false;
            try
            {
                //1206 - Please accept our Terms and Conditions to proceed
                frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1206));
                CustomerDTO customerDTO = null;
                if (selectedCustomerDTO != null && (string.IsNullOrWhiteSpace(selectedCustomerDTO.CardNumber) == false))
                {
                    customerDTO = ShowCustomerScreen(selectedCustomerDTO.CardNumber);
                }
                else if (selectedCustomerDTO != null)
                {
                    customerDTO = ShowCustomerScreen(selectedCustomerDTO);
                }
                if (customerDTO != null && customerDTO.PolicyTermsAccepted == true)
                {
                    status = true;
                }
            }
            catch (TimeoutOccurred ex)
            {
                log.Error(ex); 
                KioskStatic.logToFile("Error in AlerUserToAcceptTermsAndConditions() of CustomerStatic: " + ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
            }
            log.LogMethodExit(status);
            return status;
        }

        /// <summary>
        /// GetCustomerForWaiver
        /// </summary>
        /// <param name="existingCustomerDTO"></param>
        /// <returns></returns>
        public static CustomerDTO GetCustomer(CustomerDTO existingCustomerDTO, bool isRegisteredCustomerOnly)
        {
            log.LogMethodEntry(existingCustomerDTO, isRegisteredCustomerOnly);
            CustomerDTO customerDTO = existingCustomerDTO;
            try
            {
                if (customerDTO == null)
                {
                    //bool isRegisteredCustomerOnly = true;
                    bool showMsgRecommendCustomerToRegister = false;
                    using (frmSearchCustomer frmSearch = new frmSearchCustomer(isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister))
                    {
                        DialogResult dr = frmSearch.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            customerDTO = frmSearch.GetCustomerDTO;
                        }
                        else if (dr == DialogResult.Cancel)
                        {
                            string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Timeout");
                            throw new TimeoutOccurred(msg);
                        }
                    }
                }
            }
            catch (TimeoutOccurred ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in GetCustomerForWaiver() of CustomerStatic: " + ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in GetCustomerForWaiver() of CustomerStatic: " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }

        public static bool LinkCustomerToTheCard(KioskTransaction kioskTransaction, bool isRegisteredCustomerOnly, Card Card)
        {
            log.LogMethodEntry("kioskTransaction", isRegisteredCustomerOnly, Card);
            bool isLinked = false;

            if (Card == null)
            {
                string errMsg = "ERROR: Link Customer To The Card failed. Card object passed as parameter is null";
                log.Error(errMsg);
                KioskStatic.logToFile(errMsg);
                throw new ValidationException(errMsg);
            }
            else
            {
                if (Card.customer_id > 0)
                {
                    if (kioskTransaction.HasCustomerRecord() == false && Card.customerDTO != null)
                    {
                        kioskTransaction.SetTransactionCustomer(Card.customerDTO);
                    }
                    isLinked = true;
                }
                else
                {
                    //Audio.PlayAudio(Audio.RegisterCardPrompt);
                    string messageParam = string.Empty;
                    if (kioskTransaction.HasCustomerRecord() == true)
                    {
                        string customerName = kioskTransaction.GetTransactionCustomer().FirstName;
                        if (!string.IsNullOrWhiteSpace(kioskTransaction.GetTransactionCustomer().LastName))
                        {
                            customerName += " " + kioskTransaction.GetTransactionCustomer().LastName;
                        }
                        //5316 - Do you want to link &1 to the card?
                        messageParam = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5316, customerName);
                        using (frmYesNo frmYN = new frmYesNo(messageParam))
                        {
                            DialogResult dr = frmYN.ShowDialog();
                            if (dr == System.Windows.Forms.DialogResult.Yes)
                            {
                                Card.customerDTO = kioskTransaction.GetTransactionCustomer();
                                Card.updateCustomer();
                                Card.getCardDetails(Card.card_id); //refresh card object
                            }
                            else if (dr == System.Windows.Forms.DialogResult.No)
                            {
                                //5317 - Do you want to associate another customer with the card?
                                messageParam = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5317);
                                using (frmYesNo frmYesNo = new frmYesNo(messageParam))
                                {
                                    DialogResult dialogueResult = frmYesNo.ShowDialog();
                                    if (dialogueResult == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        CustomerDTO customer = GetCustomer(null, isRegisteredCustomerOnly);
                                        if (customer != null)
                                        {
                                            Card.customerDTO = customer;
                                            Card.updateCustomer();
                                            isLinked = true;
                                        }
                                        else
                                        {
                                            string errMsg = "Failed to link the customer to the card";
                                            log.Error(errMsg);
                                            KioskStatic.logToFile("ERROR: " + errMsg);
                                        }
                                        Card.getCardDetails(Card.card_id); //refresh card object
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bool showMsgRecommendCustomerToRegister = true;
                        if (Card.customerDTO != null)
                        {
                            kioskTransaction.SetTransactionCustomer(Card.customerDTO);
                            isLinked = true;
                        }
                        else
                        {
                            isLinked = LinkCustomerToTheTransaction(kioskTransaction, KioskStatic.Utilities.ExecutionContext,
                                isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
                            if (isLinked == true)
                            {
                                Card.customerDTO = kioskTransaction.GetTransactionCustomer();
                                Card.updateCustomer();
                            }
                        }
                        Card.getCardDetails(Card.card_id); //refresh card object
                    }
                }
            }
            log.LogMethodExit(isLinked);
            return isLinked;
        }

        public static bool AlertUserForCustomerRegistrationRequirement(bool isKioskTrxHasCustomer, bool isCustomerMandatory, bool isLinked)
        {
            log.LogMethodEntry(isKioskTrxHasCustomer, isCustomerMandatory, isLinked);
            bool status = true;
            if (isLinked == false && isCustomerMandatory == true && isKioskTrxHasCustomer == false)
            {
                status = false;
                //220 - This Product is for Registered Customers only
                string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 220);
                frmOKMsg.ShowUserMessage(errMsg);
                KioskStatic.logToFile("ERROR: " + errMsg + "Either Customer is Not found or terms and conditions are not accepted.");
            }
            log.LogMethodExit(status);
            return status;
        }

        public class TimeoutOccurred : Exception
        {
            public TimeoutOccurred(string message) : base(message)
            {

            }
        }
    }
}
