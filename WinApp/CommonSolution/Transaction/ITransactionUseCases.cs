/********************************************************************************************
* Project Name - Transaction
* Description  - ITransactionUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
  2.110       13-Nov-2020       Girish Kundar      Created :Waiver Link changes
  2.130.1     21-Nov-2021       Girish Kundar      Modified :Check in Check Out changes
  2.130.9     16-Jun-2022       Guru S A           Execute online transaction changes in Kiosk
 *2.140.0     01-Jun-2021       Fiona Lishal       Modified for Delivery Order enhancements for F&B
 *2.150.0     12-Dec-2022       Abhishek           Modified : Added usecases for waiver
********************************************************************************************/
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Waiver;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Specification of the user use cases
    /// </summary>
    public interface ITransactionUseCases
    {
        /// <summary>
        /// GetWaiverLinks
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<string> GetWaiverLinks(int transactionId);
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
        /// <returns></returns>
        Task<List<TransactionDTO>> GetTransactionDTOList(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters, Utilities utilities, SqlTransaction sqlTransaction = null,
            int pageNumber = 0, int pageSize = 10, bool buildChildRecords = false, bool buildTickets = false, bool buildReceipt = false);

        /// <summary>
        /// GetTransactionDTOList
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="buildChildRecords"></param>
        /// <param name="buildTickets"></param>
        /// <param name="buildReceipt"></param>
        /// <returns></returns>
        Task<List<TransactionDTO>> GetTransactionDTOList(TransactionSearchCriteria searchCriteria, bool buildChildRecords = false, bool buildTickets = false,
            bool buildReceipt = false);
        /// <summary>
        /// GetTransactionCount
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        Task<int> GetTransactionCount(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters);
        /// <summary>
        /// UpdateTransactionStatus
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        Task<List<KeyValuePair<TransactionDTO, string>>> UpdateTransactionStatus(List<TransactionDTO> inputList);
        /// <summary>
        /// CreateTransactionOrderDispensingDTO
        /// </summary>
        /// <param name="transactionOrderDispensingDTOList"></param>
        /// <returns></returns>
        Task<List<TransactionOrderDispensingDTO>> CreateTransactionOrderDispensingDTO(List<TransactionOrderDispensingDTO> transactionOrderDispensingDTOList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>

        Task<List<KeyValuePair<TransactionDTO, string>>> SetAsCustomerReconfirmedOrder(List<TransactionDTO> inputList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        Task<List<KeyValuePair<TransactionDTO, string>>> SetAsPreparationReconfirmedOrder(List<TransactionDTO> inputList);
        /// <summary>
        /// AssignRider
        /// </summary>
        /// <returns></returns>
        Task<TransactionOrderDispensingDTO> AssignRider(int transactionId, TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTO"></param>
        /// <returns></returns>
        Task<TransactionOrderDispensingDTO> UnAssignRider(int transactionId, TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTOList"></param>
        /// <returns></returns>
        Task<TransactionOrderDispensingDTO> SaveRiderDeliveryStatus(int transactionId, List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionDeliveryDetailsDTOList"></param>
        /// <returns></returns>
        Task<TransactionOrderDispensingDTO> SaveRiderAssignmentRemarks(int transactionId, List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList);
        /// <summary>
        /// AmendKOTScheduleTime
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="timeToAmend"></param>
        /// <returns></returns>
        Task<List<TransactionLineDTO>> AmendKOTScheduleTime(int transactionId, double timeToAmend);

        ///// <summary>
        ///// Checkin
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <param name="checkInId"></param>
        ///// <param name="checkInDetailDTOList"></param>
        ///// <returns>CheckInDTO</returns>
        //Task<CheckInDTO> CheckIn(int transactionId, int checkInId, List<CheckInDetailDTO> checkInDetailDTOList);
        ///// <summary>
        ///// Checkin
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <param name="checkInId"></param>
        ///// <param name="checkInDetailDTOList"></param>
        ///// <returns>CheckInDTO</returns>
        //Task<CheckInDTO> Pause(int transactionId, int checkInId, List<CheckInDetailDTO> checkInDetailDTOList);
        ///// <summary>
        ///// Checkin
        ///// </summary>
        ///// <param name="transactionId"></param>
        ///// <param name="checkInId"></param>
        ///// <param name="checkInDetailDTOList"></param>
        ///// <returns>CheckInDTO</returns>
        //Task<CheckInDTO> CheckOut(int transactionId, int checkInId, List<CheckInDetailDTO> checkInDetailDTOList);

        /// <summary>
        /// UpdateCheckInStatus
        /// </summary>
        /// <param name="checkInId"></param>
        /// <param name="checkInDetailDTOList"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        Task<CheckInDTO> UpdateCheckInStatus(int checkInId, List<CheckInDetailDTO> checkInDetailDTOList, SqlTransaction sqlTransaction = null);

        /// <summary>
        /// SaveStaffCards
        /// </summary>
        /// <param name="staffCardDTOList"></param>
        /// <returns></returns>
        Task<List<StaffCardDTO>> CreateStaffCard(List<StaffCardDTO> staffCardDTOList);
        /// <summary>
        /// SubmitUrbanPiperOrderCancellationRequest
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        Task<List<KeyValuePair<TransactionDTO, string>>> SubmitUrbanPiperOrderCancellationRequest(Dictionary<TransactionDTO, string> inputList);
        /// <summary>
        /// Settle Transaction Payments
        /// </summary>
        /// <param name="TransactionPaymentsDTOlist"></param>
        /// <returns></returns>
        Task<List<KeyValuePair<TransactionPaymentsDTO, string>>> SettleTransactionPayments(List<TransactionPaymentsDTO> TransactionPaymentsDTOlist);

        /// <summary>
        /// UpdateTransactionPaymentModeDetails
        /// </summary>
        /// <param name="transactionPaymentsDTOList"></param>
        /// <returns></returns>
        Task<List<KeyValuePair<TransactionPaymentsDTO, string>>> UpdateTransactionPaymentModeDetails(List<TransactionPaymentsDTO> transactionPaymentsDTOList);
        /// <summary>
        /// GetUnsettledTransactionPayments
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="printerTypeList"></param>
        /// <param name="forVirtualStore"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="deliveryChannelId"></param>
        /// <param name="trxFromDate"></param>
        /// <param name="trxToDate"></param>
        /// <returns></returns>
        Task<List<TransactionPaymentsDTO>> GetUnsettledTransactionPayments(int transactionId=-1, int paymentModeId =-1, int deliveryChannelId =-1, DateTime? trxFromDate = null, DateTime? trxToDate = null);
        /// <summary>
        /// GetPrintableTransactionLines
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="printerTypeList"></param>
        /// <param name="forVirtualStore"></param>
        /// <returns></returns>
        Task<List<KeyValuePair<string, List<TransactionLineDTO>>>> GetPrintableTransactionLines(int transactionId, string printerTypeList, bool forVirtualStore);
        /// <summary>
        /// PrintVirtualStoreTransaction
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionListDTOList"></param>
        /// <returns></returns>
        Task<TransactionDTO> PrintVirtualStoreTransaction(int transactionId, List<TransactionLineDTO> transactionListDTOList);
        /// <summary>
        /// PrintExecuteOnlinetaskReceipt
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="taskInfoList"></param>
        /// <returns></returns>
        Task<TransactionDTO> PrintExecuteOnlinetaskReceipt(int transactionId, List<int> taskInfoList);
        /// <summary>
        /// deactivate staff card
        /// </summary>
        /// <param name="staffCardDTOs"></param>
        /// <returns></returns>
        Task<List<StaffCardDTO>> DeactivateStaffCard(List<StaffCardDTO> staffCardDTOs);

        /// <summary>
        /// SaveWaiverSignatures
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="waiverSignatureDTOList"></param>
        /// <returns></returns>
        Task<List<WaiverSignatureDTO>> SaveWaiverSignatures(int transactionId, List<WaiverSignatureDTO> waiverSignatureDTOList);
        /// <summary>
        /// GetLatestSignedCustomers
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        Task<List<CustomerDTO>> GetLatestSignedCustomers(int transactionId, int totalCount = 10);
    }
}
