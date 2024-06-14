/********************************************************************************************
 * Project Name - SubscriptionUIHelper 
 * Description  - helper class for Subscription UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     21-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A             For Subscription phase 2 changes
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities; 
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System.Collections.Generic; 
using System.Windows.Forms;

namespace Parafait_POS.Subscription
{
    /// <summary>
    /// SubscriptionUIHelper
    /// </summary>
    public static class SubscriptionUIHelper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// LoadSelectedPaymentCollectionMode
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cmbSelectedPaymentCollectionMode"></param>
        /// <returns></returns>
        public static AutoCompleteComboBox LoadSelectedPaymentCollectionMode(ExecutionContext executionContext, AutoCompleteComboBox cmbSelectedPaymentCollectionMode)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> paymentModeList = new List<KeyValuePair<string, string>>();
            paymentModeList.Add(new KeyValuePair<string, string>(SubscriptionPaymentCollectionMode.CUSTOMER_CHOICE, SubscriptionPaymentCollectionMode.GetPaymentDescription(SubscriptionPaymentCollectionMode.CUSTOMER_CHOICE)));
            paymentModeList.Add(new KeyValuePair<string, string>(SubscriptionPaymentCollectionMode.SUBSCRIPTION_CYCLE, SubscriptionPaymentCollectionMode.GetPaymentDescription(SubscriptionPaymentCollectionMode.SUBSCRIPTION_CYCLE)));
            paymentModeList.Add(new KeyValuePair<string, string>(SubscriptionPaymentCollectionMode.FULL, SubscriptionPaymentCollectionMode.GetPaymentDescription(SubscriptionPaymentCollectionMode.FULL)));

            cmbSelectedPaymentCollectionMode.DataSource = paymentModeList;
            cmbSelectedPaymentCollectionMode.ValueMember = "Key";
            cmbSelectedPaymentCollectionMode.DisplayMember = "Value";
            log.LogMethodExit();
            return cmbSelectedPaymentCollectionMode;
        }
        /// <summary>
        /// LoadSubscriptionStatus
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cmbSubscriptionStatus"></param>
        /// <returns></returns>
        public static AutoCompleteComboBox LoadSubscriptionStatus(ExecutionContext executionContext, AutoCompleteComboBox cmbSubscriptionStatus)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> subscriptionStatusList = new List<KeyValuePair<string, string>>();
            subscriptionStatusList.Add(new KeyValuePair<string, string>("ALL", "ALL"));
            subscriptionStatusList.Add(new KeyValuePair<string, string>(SubscriptionStatus.ACTIVE, SubscriptionStatus.ACTIVE));
            subscriptionStatusList.Add(new KeyValuePair<string, string>(SubscriptionStatus.CANCELLED, SubscriptionStatus.CANCELLED));
            subscriptionStatusList.Add(new KeyValuePair<string, string>(SubscriptionStatus.EXPIRED, SubscriptionStatus.EXPIRED));
            subscriptionStatusList.Add(new KeyValuePair<string, string>(SubscriptionStatus.PAUSED, SubscriptionStatus.PAUSED));

            cmbSubscriptionStatus.DataSource = subscriptionStatusList;
            cmbSubscriptionStatus.ValueMember = "Key";
            cmbSubscriptionStatus.DisplayMember = "Value";
            log.LogMethodExit();
            return cmbSubscriptionStatus;
        }
        /// <summary>
        /// LoadCreditCards
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="executionContext"></param>
        /// <param name="customerId"></param>
        /// <param name="cmbCreditCardId"></param>
        /// <returns></returns>
        internal static AutoCompleteComboBox LoadCreditCards(Utilities utilities, ExecutionContext executionContext, int customerId, AutoCompleteComboBox cmbCreditCardId)
        {
            log.LogMethodEntry(customerId);
            List<CustomerCreditCardsDTO> customerCreditCardsDTOList = GetCustomerCreditCardsDTO(utilities, executionContext, customerId);
            cmbCreditCardId.DataSource = customerCreditCardsDTOList;
            cmbCreditCardId.ValueMember = "CustomerCreditCardsId";
            cmbCreditCardId.DisplayMember = "CreditCardNumber";
            log.LogMethodExit();
            return cmbCreditCardId;
        } 
        internal static DataGridViewComboBoxColumn LoadCreditCards(Utilities utilities, ExecutionContext executionContext, int customerId, DataGridViewComboBoxColumn dgvColumnField)
        {
            log.LogMethodEntry(customerId);
            List<CustomerCreditCardsDTO> customerCreditCardsDTOList = GetCustomerCreditCardsDTO(utilities, executionContext, customerId);
            dgvColumnField.DataSource = customerCreditCardsDTOList;
            dgvColumnField.ValueMember = "CustomerCreditCardsId";
            dgvColumnField.DisplayMember = "CreditCardNumber";
            log.LogMethodExit();
            return dgvColumnField;
        }
        private static List<CustomerCreditCardsDTO> GetCustomerCreditCardsDTO(Utilities utilities, ExecutionContext executionContext, int customerId)
        {
            log.LogMethodEntry(customerId);
            CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext);
            List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
            searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<CustomerCreditCardsDTO> customerCreditCardsDTOList = customerCreditCardsListBL.GetCustomerCreditCardsDTOList(searchParameters, utilities);
            if (customerCreditCardsDTOList == null)
            {
                customerCreditCardsDTOList = new List<CustomerCreditCardsDTO>();
            }
            customerCreditCardsDTOList.Insert(0, new CustomerCreditCardsDTO());
            log.LogMethodExit();
            return customerCreditCardsDTOList;
        }

        internal static AutoCompleteComboBox LoadCustomerContacts(ExecutionContext executionContext, int customerId, AutoCompleteComboBox cmbCustomerContact)
        {
            log.LogMethodEntry(customerId);
            List<ContactDTO> contactDTOList = GetContactDTOList(executionContext, customerId);
            cmbCustomerContact.DataSource = contactDTOList;
            cmbCustomerContact.ValueMember = "Id";
            cmbCustomerContact.DisplayMember = "Attribute1";
            log.LogMethodExit();
            return cmbCustomerContact;
        }

        internal static DataGridViewComboBoxColumn LoadCustomerContacts(ExecutionContext executionContext, int customerId, DataGridViewComboBoxColumn dgvHeaderCustomerContactId)
        {
            log.LogMethodEntry(customerId);
            List<ContactDTO> contactDTOList = GetContactDTOList(executionContext, customerId);
            dgvHeaderCustomerContactId.DataSource = contactDTOList;
            dgvHeaderCustomerContactId.ValueMember = "Id";
            dgvHeaderCustomerContactId.DisplayMember = "Attribute1";
            log.LogMethodExit();
            return dgvHeaderCustomerContactId;
        }

        private static List<ContactDTO> GetContactDTOList(ExecutionContext executionContext, int customerId)
        {
            log.LogMethodEntry(customerId);
            ContactListBL contactListBL = new ContactListBL(executionContext); List<KeyValuePair<ContactDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ContactDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
            searchParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

            List<ContactDTO> contactDTOList = contactListBL.GetContactDTOList(searchParameters);
            if (contactDTOList == null)
            {
                contactDTOList = new List<ContactDTO>();
            }
            contactDTOList.Insert(0, new ContactDTO());
            log.LogMethodExit();
            return contactDTOList;
        }

        internal static DataGridViewComboBoxColumn LoadTransactionId(Utilities utilities, int transactionId, DataGridViewComboBoxColumn dgvColumnField)
        {
            log.LogMethodEntry(transactionId);
            List<TransactionDTO> transactionDTOList = null;
            if (transactionId > -1)
            {
                TransactionListBL transactionListBL = new TransactionListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                transactionDTOList = transactionListBL.GetTransactionDTOList(searchParameters, utilities, null, 0, 1000);
            }
            if (transactionDTOList == null)
            {
                transactionDTOList = new List<TransactionDTO>();
            }
            transactionDTOList.Insert(0, new TransactionDTO());
            dgvColumnField.DataSource = transactionDTOList;
            dgvColumnField.ValueMember = "TransactionId";
            dgvColumnField.DisplayMember = "TransactionNumber";
            log.LogMethodExit();
            return dgvColumnField;
        }

        internal static DataGridViewComboBoxColumn LoadCustomerId(ExecutionContext executionContext, int customerId, DataGridViewComboBoxColumn dgvColumnField)
        {
            log.LogMethodEntry(customerId);
            List<CustomerDTO> customerDTOList = null;
            if (customerId > -1)
            {
                CustomerBL customerBL = new CustomerBL(executionContext, customerId);
                customerDTOList = new List<CustomerDTO>();
                customerDTOList.Add(customerBL.CustomerDTO);
            }
            if (customerDTOList == null)
            {
                customerDTOList = new List<CustomerDTO>();
            }
            customerDTOList.Insert(0, new CustomerDTO());
            dgvColumnField.DataSource = customerDTOList;
            dgvColumnField.ValueMember = "Id";
            dgvColumnField.DisplayMember = "FirstName";
            log.LogMethodExit();
            return dgvColumnField;
        }  

        internal static DataGridViewComboBoxColumn LoadLinkedTransactionId(Utilities utilities, int subscriptionHeaderId, DataGridViewComboBoxColumn dgvColumnField)
        {
            log.LogMethodEntry(subscriptionHeaderId);
            List<TransactionDTO> transactionDTOList = null;
            if (subscriptionHeaderId > -1)
            {
                TransactionListBL transactionListBL = new TransactionListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.LINKED_BILL_CYCLE_TRX_FOR_SUBSCRIPTION_HEADER_ID, subscriptionHeaderId.ToString()));
                transactionDTOList = transactionListBL.GetTransactionDTOList(searchParameters, utilities, null, 0, 1000);
            }
            if (transactionDTOList == null)
            {
                transactionDTOList = new List<TransactionDTO>();
            }
            transactionDTOList.Insert(0, new TransactionDTO());
            dgvColumnField.DataSource = transactionDTOList;
            dgvColumnField.ValueMember = "TransactionId";
            dgvColumnField.DisplayMember = "TransactionNumber";
            log.LogMethodExit();
            return dgvColumnField;
        }

        internal static List<PaymentModeDTO> GetCreditCardPaymentModeDTO(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetAllPaymentModeList(searchParameters);
            log.LogMethodExit(paymentModeDTOList);
            return paymentModeDTOList;
        }

        internal static DataGridView SetDGVCellFont(ExecutionContext executionContext, DataGridView dgvInput)
        {
            log.LogMethodEntry();
            //System.Drawing.Font font;
            //try
            //{
            //    font = new Font(ParafaitDefaultContainer.GetParafaitDefault(executionContext,"ParafaitEnv.DEFAULT_GRID_FONT"), 15, FontStyle.Regular);
            //}
            //catch (Exception ex)
            //{
            //    log.Error("Error occured while applying new font", ex);
            //    font = new Font("Tahoma", 15, FontStyle.Regular);
            //}
            //foreach (DataGridViewColumn c in dgvInput.Columns)
            //{
            //    c.DefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            //}
            log.LogMethodExit();
            return dgvInput;
        }
        internal static string GetManagerApproval(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            string mgrLoginId = string.Empty;
            int mgrId = -1;
            if (Authenticate.Manager(ref mgrId) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required"));
            }
            else
            {
                UserContainerDTO mgrDTO = UserContainerList.GetUserContainerDTO(executionContext.GetSiteId(), mgrId); 
                mgrLoginId = mgrDTO.LoginId;
            }
            log.LogMethodExit(mgrLoginId);
            return mgrLoginId;
        }
        /// <summary>
        /// LoadCancellationOptions
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cmbCancellationOption"></param>
        /// <returns></returns>
        public static AutoCompleteComboBox LoadCancellationOptions(ExecutionContext executionContext, AutoCompleteComboBox cmbCancellationOption)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> cancellationOptions = new List<KeyValuePair<string, string>>();
            cancellationOptions.Add(new KeyValuePair<string, string>(SubscriptionCancellationOption.CANCELL_UNBILLED_CYCLES, SubscriptionCancellationOption.GetCancellationOptionDescription(SubscriptionCancellationOption.CANCELL_UNBILLED_CYCLES)));
            cancellationOptions.Add(new KeyValuePair<string, string>(SubscriptionCancellationOption.CANCEL_AUTO_RENEWAL_ONLY, SubscriptionCancellationOption.GetCancellationOptionDescription(SubscriptionCancellationOption.CANCEL_AUTO_RENEWAL_ONLY))); 

            cmbCancellationOption.DataSource = cancellationOptions;
            cmbCancellationOption.ValueMember = "Key";
            cmbCancellationOption.DisplayMember = "Value";
            log.LogMethodExit();
            return cmbCancellationOption;
        }
    }
}
