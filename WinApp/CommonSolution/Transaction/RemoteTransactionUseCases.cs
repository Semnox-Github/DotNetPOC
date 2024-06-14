/********************************************************************************************
 * Project Name - Transaction
 * Description  - RemoteTransactionUseCases
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110       13-Nov-2020       Girish Kundar      Created :Waiver Link chnages
 2.130       21-Nov-2021       Girish Kundar      Modifies :Check In check out changes
 2.130.9     16-Jun-2022       Guru S A           Execute online transaction changes in Kiosk 
 *2.140.0    01-Jun-2021       Fiona Lishal       Modified for Delivery Order enhancements for F&B
 *2.150.0    12-Dec-2022       Abhishek           Modified : Added usecases for waiver
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Waiver;
using Semnox.Parafait.Customer;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// RemoteTransactionUseCases
    /// </summary>
    public class RemoteTransactionUseCases : RemoteUseCases, ITransactionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string WAIVER_LINK_URL = "api/Customer/Waiver/Links";
        private const string UPDATE_STATUS_URL = "api/Transaction/Transactions/Status";
        private const string CUSTOMER_RECONFIRMERD_ORDER_URL = "api/Transaction/Transactions/CustomerReconfirmedOrder";
        private const string PREPARATION_RECONFIRMERD_ORDER_URL = "api/Transaction/Transactions/PreparationReconfirmedOrder";
        private  string ASSIGN_RIDER_URL = "api/Transaction/{transactionId}/AssignRider";
        private  string UNASSIGN_RIDER_URL = "api/Transaction/{transactionId}/UnAssignRider";
        private  string UPDATE_RIDER_DELIVERY_STATUS_URL = "api/Transaction/{transactionId}/UpdateRiderDeliveryStatus";
        private  string UPDATE_RIDER_ASSIGNMENT_REMARKS_URL = "api/Transaction/{transactionId}/UpdateRiderAssignmentRemarks";
        private  string AMEND_KOT_SCHEDULE_URL = "api/Transaction/{transactionId}/AmendKOTScheduleTime";
        private const string GET_TRANSACTIONS_URL = "api/Transaction/Transactions";
        private const string GET_TRANSACTIONS_COUNT = "api/Transaction/GetTransactionsCount";
        private const string GET_TRANSACTIONORDERDISPENSING = "api/Transaction/TransactionOrderDispensing";
        private const string STAFF_CARD_URL = "api/Transaction/StaffCards";
        //private string CHECKIN_URL = "api/Transaction/{transactionId}/Checkin/{checkInId}/CheckIn";
        //private string PAUSE_URL = "api/Transaction/{transactionId}/Checkin/{checkInId}/Pause";
        //private string CHECKOUT_URL = "api/Transaction/{transactionId}/Checkin/{checkInId}/CheckOut";
        private string UPDATESTATUS_URL = "api/Transaction/Checkin/{checkInId}/Status";
        private const string SUBMIT_URBAN_PIPER_ORDER_CANCELLATION_REQUEST = "api/Transaction/SubmitUrbanPiperOrderCancellationRequest";
        private const string GET_PRINTABLE_LINES_URL = "api/Transaction/PrintableTransactionLines";
        private string POST_PRINT_URL = "api/Transaction/{TransactionId}/TransactionServices";
        private const string SELECTED_TRANSACTION_PAYMENT_URL = "api/Transaction/SettleTransactionPayments";
        private const string UPDATE_TRANSACTION_PAYMENT_MODE_URL = "api/Transaction/UpdateTransactionPaymentMode";
        private const string GET_UNSETTLED_TRANSACTION_PAYMENTS = "api/Transaction/GetUnsettledTransactionPayments";

        /// <summary>
        /// RemoteTransactionUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteTransactionUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetWaiverLinks
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<string> GetWaiverLinks(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("transactionId".ToString(), transactionId.ToString()));
            try
            {
                string result = await Get<string>(WAIVER_LINK_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionDTO, string>>> UpdateTransactionStatus(List<TransactionDTO> inputList)
        {
            log.LogMethodEntry(inputList);

            try
            {
                List<KeyValuePair<TransactionDTO, string>> result = await Post<List<KeyValuePair<TransactionDTO, string>>>(UPDATE_STATUS_URL, inputList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionDTO, string>>> SetAsCustomerReconfirmedOrder(List<TransactionDTO> inputList)
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<TransactionDTO, string>> result = await Post<List<KeyValuePair<TransactionDTO, string>>>(CUSTOMER_RECONFIRMERD_ORDER_URL, inputList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// SetAsPreparationReconfirmedOrder
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionDTO, string>>> SetAsPreparationReconfirmedOrder(List<TransactionDTO> inputList)
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<TransactionDTO, string>> result = await Post<List<KeyValuePair<TransactionDTO, string>>>(PREPARATION_RECONFIRMERD_ORDER_URL, inputList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// AssignRider
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTO"></param>
        /// <returns></returns>
        public async Task<TransactionOrderDispensingDTO> AssignRider(int transactionId, TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO)
        {
            log.LogMethodEntry();
            try
            {
                ASSIGN_RIDER_URL = "api/Transaction/" + transactionId + "/AssignRider";
                TransactionOrderDispensingDTO result = await Post<TransactionOrderDispensingDTO>(ASSIGN_RIDER_URL, transactionDeliveryDetailsDTO);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTO"></param>
        /// <returns></returns>
        public async Task<TransactionOrderDispensingDTO> UnAssignRider(int transactionId, TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO)
        {
            log.LogMethodEntry();
            try
            {
                UNASSIGN_RIDER_URL = "api/Transaction/" + transactionId + "/UnAssignRider";
                TransactionOrderDispensingDTO result = await Post<TransactionOrderDispensingDTO>(UNASSIGN_RIDER_URL, transactionDeliveryDetailsDTO);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// SaveRiderDeliveryStatus
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTOList"></param>
        /// <returns></returns>
        public async Task<TransactionOrderDispensingDTO> SaveRiderDeliveryStatus(int transactionId, List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList)
        {
            log.LogMethodEntry();
            try
            {
                UPDATE_RIDER_DELIVERY_STATUS_URL = "api/Transaction/" + transactionId + "/UpdateRiderDeliveryStatus";
                TransactionOrderDispensingDTO result = await Post<TransactionOrderDispensingDTO>(UPDATE_RIDER_DELIVERY_STATUS_URL, transactionDeliveryDetailsDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTOList"></param>
        /// <returns></returns>
        public async Task<TransactionOrderDispensingDTO> SaveRiderAssignmentRemarks(int transactionId, List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList)
        {
            log.LogMethodEntry(transactionId, transactionDeliveryDetailsDTOList);
            try
            {
                UPDATE_RIDER_ASSIGNMENT_REMARKS_URL = "api/Transaction/" + transactionId + "/UpdateRiderAssignmentRemarks";
                TransactionOrderDispensingDTO result = await Post<TransactionOrderDispensingDTO>(UPDATE_RIDER_ASSIGNMENT_REMARKS_URL, transactionDeliveryDetailsDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="timeToAmend"></param>
        /// <returns></returns>
        public async Task<List<TransactionLineDTO>> AmendKOTScheduleTime(int transactionId, double timeToAmend)
        {
            log.LogMethodEntry(transactionId, timeToAmend);
            try
            {
                AMEND_KOT_SCHEDULE_URL = "api/Transaction/" + transactionId + "/AmendKOTScheduleTime";
                List<TransactionLineDTO> result = await Post<List<TransactionLineDTO>>(AMEND_KOT_SCHEDULE_URL, timeToAmend);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// GetTransactionDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="utilities"></param>
        /// <param name="sqlTransaction"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="buildChildRecords"></param>
        /// <param name="buildTickets"></param>
        /// <param name="buildReceipt"></param>
        /// <param name="hasOrderDispensingEntry"></param>
        /// <returns></returns>
        public async Task<List<TransactionDTO>> GetTransactionDTOList(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters, Utilities utilities, SqlTransaction sqlTransaction = null, int pageNumber = 0, int pageSize = 10, bool buildChildRecords = false, bool buildTickets = false, bool buildReceipt = false)
        {
            log.LogMethodEntry(searchParameters, utilities, sqlTransaction, pageNumber, pageSize, buildChildRecords, buildTickets, buildReceipt);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), buildChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), buildTickets.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("buildReceipt".ToString(), buildReceipt.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<TransactionDTO> result = await Get<List<TransactionDTO>>(GET_TRANSACTIONS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private IEnumerable<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<TransactionDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                switch (searchParameter.Key)
                {
                    case TransactionDTO.SearchByParameters.TRANSACTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("transactionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("transactionStatus".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.TRANSACTION_OTP:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("transactionOTP".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("referenceId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.CUSTOMER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.CUSTOMER_GUID_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerGuid".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), searchParameter.Value));
                        }
                        break;

                    case TransactionDTO.SearchByParameters.CUSTOMER_SIGNED_WAIVER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerSignedWaiverId".ToString(), searchParameter.Value));
                        }
                        break;

                    case TransactionDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("originalSystemReference".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.ONLINE_ONLY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("onlineTransactionOnly".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.GUID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("transactionGuid".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.CUSTOMER_IDENTIFIER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerIdentifier".ToString(), searchParameter.Value));
                        }
                        break;
                    case TransactionDTO.SearchByParameters.NEEDS_ORDER_DISPENSING:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("needsOrderDispensingEntry".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        /// <summary>
        /// GetTransactionCount
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<int> GetTransactionCount(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                int result = await Get<int>(GET_TRANSACTIONS_COUNT, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

        }
        /// <summary>
        /// CreateTransactionOrderDispensingDTO
        /// </summary>
        /// <param name="transactionOrderDispensingDTOList"></param>
        /// <returns></returns>
        public async Task<List<TransactionOrderDispensingDTO>> CreateTransactionOrderDispensingDTO(List<TransactionOrderDispensingDTO> transactionOrderDispensingDTOList)
        {
            log.LogMethodEntry(transactionOrderDispensingDTOList);
            try
            {

                List<TransactionOrderDispensingDTO> result = await Post<List<TransactionOrderDispensingDTO>>(GET_TRANSACTIONORDERDISPENSING, transactionOrderDispensingDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        ///// <summary>
        ///// CheckIn
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <param name="checkInId"></param>
        ///// <param name="checkInDetailDTOList"></param>
        ///// <returns></returns>
        //public async Task<CheckInDTO> CheckIn(int transactionId, int checkInId, List<CheckInDetailDTO> checkInDetailDTOList)
        //{
        //    log.LogMethodEntry(transactionId, checkInId, checkInDetailDTOList);
        //    try
        //    {
        //        CHECKIN_URL = "api/Transaction/" + transactionId + "/Checkin/{" + checkInId + "}/CheckIn";
        //        CheckInDTO result = await Post<CheckInDTO>(CHECKIN_URL, checkInDetailDTOList);
        //        log.LogMethodExit(result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Checkout
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <param name="checkInId"></param>
        ///// <param name="checkInDetailDTOList"></param>
        ///// <returns></returns>
        //public async Task<CheckInDTO> CheckOut(int transactionId, int checkInId, List<CheckInDetailDTO> checkInDetailDTOList)
        //{
        //    log.LogMethodEntry(transactionId, checkInId, checkInDetailDTOList);
        //    try
        //    {
        //        CHECKOUT_URL = "api/Transaction/" + transactionId + "/Checkin/{" + checkInId + "}/CheckOut";
        //        CheckInDTO result = await Post<CheckInDTO>(CHECKIN_URL, checkInDetailDTOList);
        //        log.LogMethodExit(result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw;
        //    }
        //}
        /// <summary>
        /// Settle Transaction Payments
        /// </summary>
        /// <param name="transactionPaymentsDTOlist"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionPaymentsDTO, string>>> SettleTransactionPayments(List<TransactionPaymentsDTO> transactionPaymentsDTOlist)
        {
            log.LogMethodEntry(transactionPaymentsDTOlist);
            List<KeyValuePair<TransactionPaymentsDTO, string>> resultList = new List<KeyValuePair<TransactionPaymentsDTO, string>>();
            try
            {
                List<KeyValuePair<TransactionPaymentsDTO, string>> result = await Post<List<KeyValuePair<TransactionPaymentsDTO, string>>>(SELECTED_TRANSACTION_PAYMENT_URL, transactionPaymentsDTOlist);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        /// <summary>
        /// UpdateTransactionPaymentModeDetails
        /// </summary>
        /// <param name="transactionPaymentsDTOList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionPaymentsDTO, string>>> UpdateTransactionPaymentModeDetails(List<TransactionPaymentsDTO> transactionPaymentsDTOList)
        {
            log.LogMethodEntry(transactionPaymentsDTOList);
            try
            {
                List<KeyValuePair<TransactionPaymentsDTO, string>> result = await Post<List<KeyValuePair<TransactionPaymentsDTO, string>>>(UPDATE_TRANSACTION_PAYMENT_MODE_URL, transactionPaymentsDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public Task<List<TransactionDTO>> GetTransactionDTOList(TransactionSearchCriteria searchCriteria, bool buildChildRecords = false, bool buildTickets = false, bool buildReceipt = false)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetUnsettledTransactionPayments
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="deliveryChannelId"></param>
        /// <param name="trxFromDate"></param>
        /// <param name="trxToDate"></param>
        /// <returns></returns>
        public async Task<List<TransactionPaymentsDTO>> GetUnsettledTransactionPayments(int transactionId = -1, int paymentModeId = -1, int deliveryChannelId = -1, DateTime? trxFromDate = null, DateTime? trxToDate = null)
        {
            log.LogMethodEntry(transactionId, paymentModeId, deliveryChannelId, trxFromDate, trxToDate);
            List<TransactionPaymentsDTO> result = new List<TransactionPaymentsDTO>();
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("transactionId".ToString(), transactionId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("paymentModeId".ToString(), paymentModeId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("deliveryChannelId".ToString(), deliveryChannelId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("trxFromDate".ToString(), (trxFromDate == null ? string.Empty: ((DateTime)trxFromDate).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
            searchParameterList.Add(new KeyValuePair<string, string>("trxToDate".ToString(), (trxToDate == null ? string.Empty : ((DateTime)trxToDate).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
            try
            {
                result = await Get< List<TransactionPaymentsDTO>>(GET_UNSETTLED_TRANSACTION_PAYMENTS, searchParameterList);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(result);
            return result;
        }

        
      

        ///// <summary>
        ///// Pause
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <param name="checkInId"></param>
        ///// <param name="checkInDetailDTOList"></param>
        ///// <returns></returns>
        //public async Task<CheckInDTO> Pause(int transactionId, int checkInId, List<CheckInDetailDTO> checkInDetailDTOList)
        //{
        //    log.LogMethodEntry(transactionId, checkInId, checkInDetailDTOList);
        //    try
        //    {
        //        PAUSE_URL = "api/Transaction/" + transactionId + "/Checkin/{" + checkInId + "}/Pause";
        //        CheckInDTO result = await Post<CheckInDTO>(CHECKIN_URL, checkInDetailDTOList);
        //        log.LogMethodExit(result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw;
        //    }
        //}
        ///// <summary>
        /// UpdateCheckInStatus
        /// </summary>
        /// <param name="checkInId"></param>
        /// <param name="checkInDetailDTOList"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<CheckInDTO> UpdateCheckInStatus(int checkInId, List<CheckInDetailDTO> checkInDetailDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(checkInId, checkInDetailDTOList);
            try
            {
                UPDATESTATUS_URL = "api/Transaction/Checkin/{" + checkInId + "}/Status";
                CheckInDTO result = await Post<CheckInDTO>(UPDATESTATUS_URL, checkInDetailDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// GetPrintableTransactionLines
        /// </summary> 
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, List<TransactionLineDTO>>>> GetPrintableTransactionLines(int transactionId, string printerTypeList, bool forVirtualStore)
        {
            log.LogMethodEntry(transactionId, printerTypeList, forVirtualStore);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("transactionId".ToString(), transactionId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("printerTypeList".ToString(), printerTypeList));
            searchParameterList.Add(new KeyValuePair<string, string>("forVirtualStore".ToString(), (forVirtualStore ? "true" : "false")));

            try
            {
                List<KeyValuePair<string, List<TransactionLineDTO>>> result = await Get<List<KeyValuePair<string, List<TransactionLineDTO>>>>(GET_PRINTABLE_LINES_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// SaveStaffCards
        /// </summary>
        /// <param name="staffCardDTOList"></param>
        /// <returns></returns>
        public async Task<List<StaffCardDTO>> CreateStaffCard(List<StaffCardDTO> staffCardDTOList)
        {
            log.LogMethodEntry(staffCardDTOList);
            try
            {
                List<StaffCardDTO> response = await Post<List<StaffCardDTO>>(STAFF_CARD_URL, staffCardDTOList);
                log.LogMethodExit(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<StaffCardDTO>> DeactivateStaffCard(List<StaffCardDTO> staffCardDTOs)
        {
            log.LogMethodEntry(staffCardDTOs);
            try
            {
                await Delete<List<StaffCardDTO>>(STAFF_CARD_URL, staffCardDTOs);
                log.LogMethodExit(staffCardDTOs);
                return staffCardDTOs;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// PrintVirtualStoreTransaction
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionListDTOList"></param>
        /// <returns></returns>
        public async Task<TransactionDTO> PrintVirtualStoreTransaction(int transactionId, List<TransactionLineDTO> transactionListDTOList)
        {
            log.LogMethodEntry(transactionId, transactionListDTOList);
            TransactionDTO result = null;
            try
            {
                string POST_PRINT_URL = "api/Transaction/{" + transactionId.ToString() + "}/TransactionServices";
                string responseString = await Post(POST_PRINT_URL, JsonConvert.SerializeObject(transactionListDTOList));
                dynamic response = JsonConvert.DeserializeObject(responseString);
                if (response != null)
                {
                    object data = response["data"];
                    result = JsonConvert.DeserializeObject<TransactionDTO>(data.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// PrintExecuteOnlinetaskReceipt
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="taskInfoList"></param>
        /// <returns></returns>
        public async Task<TransactionDTO> PrintExecuteOnlinetaskReceipt(int transactionId, List<int> taskInfoList)
        {
            log.LogMethodEntry(transactionId, taskInfoList);
            TransactionDTO result = null;
            try
            {
                string POST_PRINT_URL = "api/Transaction/{" + transactionId.ToString() + "}/TransactionServices/ExecuteOnlineReceipt";
                string responseString = await Post(POST_PRINT_URL, JsonConvert.SerializeObject(taskInfoList));
                dynamic response = JsonConvert.DeserializeObject(responseString);
                if (response != null)
                {
                    object data = response["data"];
                    result = JsonConvert.DeserializeObject<TransactionDTO>(data.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// ValidateReversedTransaction
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<TransactionDTO, string>>> SubmitUrbanPiperOrderCancellationRequest(Dictionary<TransactionDTO, string> inputList)
        {
            log.LogMethodEntry(inputList);

            try
            {
                List<KeyValuePair<TransactionDTO, string>> result = await Post<List<KeyValuePair<TransactionDTO, string>>>(SUBMIT_URBAN_PIPER_ORDER_CANCELLATION_REQUEST, inputList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// SaveWaiverSignatures
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="waiverSignatureDTOList"></param>
        /// <returns></returns>
        public async Task<List<WaiverSignatureDTO>> SaveWaiverSignatures(int transactionId, List<WaiverSignatureDTO> waiverSignatureDTOList)
        {
            log.LogMethodEntry(transactionId, waiverSignatureDTOList);
            try
            {
                string WAIVER_SIGNATURE_URL = "api/Transaction/{" + transactionId + "}/MapCustomerSignedWaiverToTransaction";
                List<WaiverSignatureDTO> responseData = await Post<List<WaiverSignatureDTO>>(WAIVER_SIGNATURE_URL, waiverSignatureDTOList);
                log.LogMethodExit(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// GetLatestSignedCustomers
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public async Task<List<CustomerDTO>> GetLatestSignedCustomers(int transactionId, int totalCount = 10)
        {
            log.LogMethodEntry(transactionId, totalCount);
            try
            {
                string LATEST_SIGNATURE_CUSTOMER_URL = "api/Transaction/{" + transactionId + "}/LatestSignedCustomers";
                List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                searchParameterList.Add(new KeyValuePair<string, string>("totalCount".ToString(), totalCount.ToString()));
                List<CustomerDTO> result = await Post<List<CustomerDTO>>(LATEST_SIGNATURE_CUSTOMER_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
    }
}
