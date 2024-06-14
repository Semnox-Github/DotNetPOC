/********************************************************************************************
 * Project Name - OrderHeaderBL
 * Description  - OrderHeaderBL  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     01-Feb-2021    Girish Kundar       Modified : Urban Piper changes
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class OrderHeaderBL
    {
        OrderHeaderDTO orderHeaderDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<Transaction> transactionList;
        private string passPhrase;
        /// <summary>
        /// Default constructor of OrderHeader class
        /// </summary>
        private OrderHeaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            orderHeaderDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the OrderHeader DTO based on the orderHeader id passed 
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        public OrderHeaderBL(ExecutionContext executionContext, int orderHeaderId, bool openTransactionsOnly = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, orderHeaderId, sqlTransaction);
            OrderHeaderDataHandler orderHeaderDataHandler = new OrderHeaderDataHandler(passPhrase, sqlTransaction);
            orderHeaderDTO = orderHeaderDataHandler.GetOrderHeaderDTO(orderHeaderId, openTransactionsOnly);
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the OrderHeader DTO based on the orderHeader id passed 
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        public OrderHeaderBL(ExecutionContext executionContext, Transaction transaction, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transaction, sqlTransaction);
            transactionList = new List<Transaction>();
            transactionList.Add(transaction);
            transaction.Order = this;
            orderHeaderDTO = new OrderHeaderDTO(); 
            if (transaction.PrimaryCard != null && transaction.PrimaryCard.CardStatus.Equals("ISSUED"))
                orderHeaderDTO.CardId = transaction.PrimaryCard.card_id;
            orderHeaderDTO.POSMachineId = transaction.Utilities.ParafaitEnv.POSMachineId;
            orderHeaderDTO.UserId = transaction.Utilities.ParafaitEnv.User_Id;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the OrderHeader DTO based on the orderHeader id passed 
        /// </summary>
        /// <param name="orderHeaderId">OrderHeader id</param>
        public OrderHeaderBL(ExecutionContext executionContext, int orderId, Transaction transaction, bool openTransactionsOnly = false, SqlTransaction sqlTransaction = null)
            : this(executionContext, orderId, openTransactionsOnly, sqlTransaction)
        {
            log.LogMethodEntry(executionContext, orderId, transaction, sqlTransaction);
            transactionList = new List<Transaction>();
            transactionList.Add(transaction);
            transaction.Order = this;
            log.LogMethodExit();
        }
        public Transaction MergeTransaction(Transaction transaction, Utilities utilities, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(transaction);
            List<Transaction> transactionList = GetTransactionList(utilities);
            if(transactionList.Count == 0)
            {
                transactionList.Add(transaction);
                transaction.Order = this;
            }
            else if(transactionList.Count == 1)
            {
                TransactionService transactionService = new TransactionService(utilities);
                Transaction mergedTransaction = transactionService.MergeTransactions(transactionList[0], transaction, sqlTransaction);
                mergedTransaction.Order = this;
                transactionList[0] = mergedTransaction;
                log.LogMethodExit(null, "Returning the merged transaction");
                string message = string.Empty;
                if (mergedTransaction.SaveOrder(ref message, sqlTransaction) != 0)
                {
                    log.LogMethodExit(null, "Error occured while saving transaction : " + message);
                    throw new Exception(message);
                }
                return mergedTransaction;
            }
            else
            {
                transactionList.Add(transaction);
                transaction.Order = this;
            }
            log.LogMethodExit();
            return transaction;
        }

        public void AddTransactions(List<Transaction> transactions, Utilities utilities)
        {
            List<Transaction> transactionList = GetTransactionList(utilities);
            transactionList.AddRange(transactions);
            foreach (var transaction in transactions)
            {
                transaction.Order = this;
            }
        }

        public List<Transaction> GetTransactionList(Utilities utilities)
        {
            log.LogMethodEntry();
            if(transactionList == null)
            {
                transactionList = new List<Transaction>();
                foreach (var transactionId in orderHeaderDTO.TransactionIdList)
                {
                    TransactionUtils transactionUtils = new TransactionUtils(utilities);
                    Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
                    transactionList.Add(transaction);
                }
            }
            log.LogMethodExit(transactionList);
            return transactionList;
        }
        

        /// <summary>
        /// Creates orderHeader object using the OrderHeader
        /// </summary>
        /// <param name="orderHeader">OrderHeader object</param>
        public OrderHeaderBL(ExecutionContext executionContext, OrderHeaderDTO orderHeader)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.orderHeaderDTO = orderHeader;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the orderHeader record
        /// Checks if the OrderHeaderId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if(orderHeaderDTO.UserId == -1 && executionContext != null)
            {
                orderHeaderDTO.UserId = executionContext.UserPKId;
            }
            if(orderHeaderDTO.IsChanged)
            {
                if(orderHeaderDTO.IsActive)
                {
                    List<ValidationError> validationErrorList = Validate(sqlTransaction);
                    if (validationErrorList.Count > 0)
                    {
                        throw new ValidationException("Validation Failed", validationErrorList);
                    }
                }
                OrderHeaderDataHandler orderHeaderDataHandler = new OrderHeaderDataHandler(passPhrase, sqlTransaction);
                if (orderHeaderDTO.OrderId < 0)
                {
                    orderHeaderDTO = orderHeaderDataHandler.InsertOrderHeader(orderHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    orderHeaderDTO.AcceptChanges();
                }
                else
                {
                    if (orderHeaderDTO.IsChanged)
                    {
                        orderHeaderDTO = orderHeaderDataHandler.UpdateOrderHeader(orderHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        orderHeaderDTO.AcceptChanges();
                    }
                }
                if(orderHeaderDTO.OrderDetailsDTOList != null && orderHeaderDTO.OrderDetailsDTOList.Any())
                {
                    OrderDetailListBL orderDetailListBL = new OrderDetailListBL(executionContext, orderHeaderDTO.OrderDetailsDTOList);
                    orderDetailListBL.Save();
                }
            }
            log.LogMethodExit();
        }

        public void CancelOrder(Utilities utilities)
        {
            log.LogMethodExit();
            List<Transaction> transactionList = GetTransactionList(utilities);
            foreach (var transaction in transactionList)
            {
                string message = string.Empty;
                if (transaction.cancelTransaction(ref message) == false)
                {
                    log.LogMethodExit(null, "Error occured while cancelling transaction : " + message);
                    throw new Exception(message);
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "CANCEL_PRINTED_TRX_LINE"))
                {
                    foreach (Transaction.TransactionLine tl in transaction.TrxLines)
                    {
                        if (tl.ProductTypeCode == "MANUAL")
                        {
                            POSMachines pOSMachines = new POSMachines(executionContext, executionContext.GetMachineId());
                            transaction.cancelOrderedKOT(null, pOSMachines.PopulatePrinterDetails());
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private List<ValidationError> Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public OrderHeaderDTO OrderHeaderDTO { get { return orderHeaderDTO; } }
    }

    /// <summary>
    /// Manages the list of orderHeader
    /// </summary>
    public class OrderHeaderList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private string passPhrase;
        public OrderHeaderList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the orderHeader list
        /// </summary>
        public List<OrderHeaderDTO> GetOrderHeaderDTOList(List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            OrderHeaderDataHandler orderHeaderDataHandler = new OrderHeaderDataHandler(passPhrase, sqlTransaction);
            List<OrderHeaderDTO> orderHeaderDTOList = orderHeaderDataHandler.GetOrderHeaderDTOList(searchParameters);
            if (orderHeaderDTOList != null && orderHeaderDTOList.Any() && loadChildRecords)
            {
                Build(orderHeaderDTOList, loadActiveChild, sqlTransaction);
            }
            log.LogMethodExit(orderHeaderDTOList);
            return orderHeaderDTOList;
        }

        /// <summary>
        /// Builds the List of OrderHeader object based on the list of orderHeader id.
        /// </summary>
        /// <param name="orderHeaderDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<OrderHeaderDTO> orderHeaderDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(orderHeaderDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, OrderHeaderDTO> orderHeaderDTOIdMap = new Dictionary<int, OrderHeaderDTO>();
            List<int> orderHeaderIdList = new List<int>();
            for (int i = 0; i < orderHeaderDTOList.Count; i++)
            {
                if (orderHeaderDTOIdMap.ContainsKey(orderHeaderDTOList[i].OrderId))
                {
                    continue;
                }
                orderHeaderDTOIdMap.Add(orderHeaderDTOList[i].OrderId, orderHeaderDTOList[i]);
                orderHeaderIdList.Add(orderHeaderDTOList[i].OrderId);
            }

            OrderDetailListBL orderDetailListBL = new OrderDetailListBL(executionContext);
            List<OrderDetailDTO> orderDetailDTOList = orderDetailListBL.GetOrderDetailDTOList(orderHeaderIdList, activeChildRecords, sqlTransaction);
            if (orderDetailDTOList != null && orderDetailDTOList.Any())
            {
                for (int i = 0; i < orderDetailDTOList.Count; i++)
                {
                    if (orderHeaderDTOIdMap.ContainsKey(orderDetailDTOList[i].OrderId) == false)
                    {
                        continue;
                    }
                    OrderHeaderDTO orderHeaderDTO = orderHeaderDTOIdMap[orderDetailDTOList[i].OrderId];
                    if (orderHeaderDTO.OrderDetailsDTOList == null)
                    {
                        orderHeaderDTO.OrderDetailsDTOList = new List<OrderDetailDTO>();
                    }
                    orderHeaderDTO.OrderDetailsDTOList.Add(orderDetailDTOList[i]);
                }
            }
        }

        public List<OrderHeaderDTO> GetOpenOrderHeaderDTOList(List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> searchParameters,
                                                              int POSTypeId,
                                                              string POSMachineName,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, POSTypeId, POSMachineName, sqlTransaction);
            bool enableOrderShareAcrossPOSCounters = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS");
            bool enableOrderShareAcrossUsers = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ORDER_SHARE_ACROSS_USERS");
            bool enableOrderShareAcrossPOS = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS");
            OrderHeaderDataHandler orderHeaderDataHandler = new OrderHeaderDataHandler(passPhrase, sqlTransaction);
            List<OrderHeaderDTO> orderHeaderDTOList = orderHeaderDataHandler.GetOpenOrderHeaderDTOList(searchParameters, 
                                                                                                       POSTypeId, 
                                                                                                       executionContext.GetUserPKId(), 
                                                                                                       executionContext.GetMachineId(), 
                                                                                                       POSMachineName, 
                                                                                                       enableOrderShareAcrossPOSCounters, 
                                                                                                       enableOrderShareAcrossUsers, 
                                                                                                       enableOrderShareAcrossPOS);
            log.LogMethodExit(orderHeaderDTOList);
            return orderHeaderDTOList;
        }
    }
}
