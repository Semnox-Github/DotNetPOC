using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class OrderService
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        public OrderService(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public void MergerOrders(List<OrderHeaderDTO> orderHeaderDTOList, Utilities utilities)
        {
            log.LogMethodEntry(orderHeaderDTOList);
            if(orderHeaderDTOList.Count < 2)
            {
                throw new ArgumentException("Only multiple orders can be merged");
            }
            Transaction orderTypeTransaction = new Transaction(utilities);
            if (orderHeaderDTOList.Exists(x => x.TransactionOrderTypeId == orderTypeTransaction.TransactionOrderTypes["Item Refund"]))
            {
                throw new ArgumentException("Item Refund orders cannot be part of Order Merge.");
            }
            OrderHeaderDTO orderHeaderDTO = null;
            if (ContainsSplitOrder(orderHeaderDTOList))
            {
                MergeSplitOrders(orderHeaderDTOList, utilities);
                log.LogMethodExit(null, "Merged split orders");
                return;
            }
            orderHeaderDTO = orderHeaderDTOList[0];
            OrderHeaderBL orderHeaderBL = new OrderHeaderBL(executionContext, orderHeaderDTO);
            SqlConnection sqlConnection = utilities.getConnection();
            using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
            {
                for (int i = 1; i < orderHeaderDTOList.Count; i++)
                {
                    OrderHeaderBL otherOrderHeaderBL = new OrderHeaderBL(executionContext, orderHeaderDTOList[i]);
                    Transaction transaction = otherOrderHeaderBL.GetTransactionList(utilities)[0];
                    orderHeaderBL.MergeTransaction(transaction, utilities, sqlTransaction);
                    orderHeaderBL.OrderHeaderDTO.TableNumber += ", " + otherOrderHeaderBL.OrderHeaderDTO.TableNumber;
                }
                orderHeaderBL.Save(sqlTransaction);
                Transaction mergedTransaction = orderHeaderBL.GetTransactionList(utilities)[0];
                string message = string.Empty;
                if (mergedTransaction.SaveOrder(ref message, sqlTransaction) != 0)
                {
                    log.LogMethodExit(null, "Error occured while saving transaction : " + message);
                    throw new Exception(message);
                }
                sqlTransaction.Commit();
            }
            log.LogMethodExit();
        }

        private void MergeSplitOrders(List<OrderHeaderDTO> orderHeaderDTOList, Utilities utilities)
        {
            OrderHeaderBL orderHeaderBL = new OrderHeaderBL(executionContext, orderHeaderDTOList[0]);
            for (int i = 1; i < orderHeaderDTOList.Count; i++)
            {
                OrderHeaderBL otherOrderHeaderBL = new OrderHeaderBL(executionContext, orderHeaderDTOList[i]);
                foreach (var transaction in otherOrderHeaderBL.GetTransactionList(utilities))
                {
                    transaction.Order = orderHeaderBL;
                    if(orderHeaderBL.OrderHeaderDTO.TableNumber.Contains(orderHeaderBL.OrderHeaderDTO.TableNumber) == false)
                    {
                        orderHeaderBL.OrderHeaderDTO.TableNumber += ", " + otherOrderHeaderBL.OrderHeaderDTO.TableNumber;
                    }
                    string message = string.Empty;
                    if(transaction.SaveOrder(ref message) != 0)
                    {
                        log.LogMethodExit(null, "Error occured while saving transaction : " + message);
                        throw new Exception(message);
                    }
                }
            }
            orderHeaderBL.Save();
        }

        private bool ContainsSplitOrder(List<OrderHeaderDTO> orderHeaderDTOList)
        {
            log.LogMethodEntry(orderHeaderDTOList);
            bool result = false;
            foreach (var orderHeaderDTO in orderHeaderDTOList)
            {
                if(orderHeaderDTO.TransactionIdList.Count > 1)
                {
                    result = true;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
