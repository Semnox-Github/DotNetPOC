/********************************************************************************************
* Project Name - Semnox.Parafait.Transaction - TransactionService
* Description  - TransactionService 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.70        17-Mar-2019      Akshay Gulaganji   modified discountCouponsDTO.isActive (from string to bool),
*                                                discountCouponsUsedDTO.IsActive
*2.70.2        12-Aug-2019      Deeksha            Added logger methods.
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Transaction
{
    public class TransactionService
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private ExecutionContext executionContext;
        public TransactionService(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            this.utilities = utilities;
            this.executionContext = ExecutionContext.GetExecutionContext();
            log.LogMethodExit();
        }

        public Transaction MergeTransactions(Transaction first, Transaction second, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(first, second, sqlTransaction);
            List<Transaction.TransactionLine> allTransactionLines = new List<Transaction.TransactionLine>();
            allTransactionLines.AddRange(first.TrxLines);
            allTransactionLines.AddRange(second.TrxLines);
            CheckValidOrderTypeGroupExist(allTransactionLines);
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Dictionary<int, int> lineIdMap = new Dictionary<int, int>();
            Transaction mergedTransaction;
            using (SqlCommand cmd = new SqlCommand("", sqlTransaction.Connection))
            {
                cmd.Transaction = sqlTransaction;
                try
                {
                    int count;
                    int lineNumber = (int)utilities.executeScalar("select isnull(max(lineId), 0) from trx_lines where TrxId = @trxId", sqlTransaction, new SqlParameter("@trxId", first.Trx_id));
                    // Move transaction line and related entity
                    foreach (Transaction.TransactionLine line in second.TrxLines)
                    {
                        if (line.DBLineId == 0)
                        {
                            log.Debug("line.DBLineId == 0");
                            continue;
                        }
                        
                        lineIdMap.Add(line.DBLineId, ++lineNumber);
                        cmd.Parameters.Clear();
                        //Pass parameters to the query
                        cmd.Parameters.AddWithValue("@trxId", second.Trx_id);
                        cmd.Parameters.AddWithValue("@newTrxId", first.Trx_id);
                        cmd.Parameters.AddWithValue("@lineId", line.DBLineId);
                        cmd.Parameters.AddWithValue("@newLineId", lineIdMap[line.DBLineId]);

                        log.LogVariableState("@trxId", second.Trx_id);
                        log.LogVariableState("@newTrxId", first.Trx_id);
                        log.LogVariableState("@lineId", line.DBLineId);
                        log.LogVariableState("@newLineId", lineIdMap[line.DBLineId]);

                        //Move the second transaction lines to first transaction
                        log.Debug("Moving transaction line");
                        cmd.CommandText = @"UPDATE trx_lines 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Booking Attendees");
                        cmd.CommandText = @"UPDATE BookingAttendees 
                                            SET trxid = @newTrxId 
                                            WHERE trxid = @trxId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        //cmd.CommandText = @"UPDATE TrxSplitPayments 
                        //                    SET trxid = @newTrxId 
                        //                    WHERE trxid = @trxId";
                        //count = cmd.ExecuteNonQuery();
                        //log.Debug("count : " + count);


                        log.Debug("Moving Rental Allocation");
                        cmd.CommandText = @"UPDATE RentalAllocation 
                                            SET trxid = @newTrxId, TrxLineId = @newLineId 
                                            WHERE trxid = @trxId and TrxLineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Card Credit Plus");
                        cmd.CommandText = @"UPDATE CardCreditPlus 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Trx Discounts");
                        cmd.CommandText = @"UPDATE TrxDiscounts 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Check In Details");
                        cmd.CommandText = @"UPDATE CheckInDetails 
                                            SET CheckInTrxId = @newTrxId, CheckInTrxLineId = @newLineId 
                                            WHERE CheckInTrxId = @trxId and CheckInTrxLineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Check In Details -> CheckOutTrxId");
                        cmd.CommandText = @"UPDATE CheckInDetails 
                                            SET CheckOutTrxId = @newTrxId 
                                            WHERE CheckOutTrxId = @trxId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        cmd.CommandText = @"UPDATE KDSOrderEntry 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Trx User Verification Details");
                        cmd.CommandText = @"UPDATE TrxUserVerificationDetails 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Discount Coupons");
                        cmd.CommandText = @"UPDATE DiscountCoupons 
                                            SET TransactionId = @newTrxId, LineId = @newLineId 
                                            WHERE TransactionId = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        //cmd.CommandText = @"UPDATE TrxTaxLines 
                        //                    SET trxid = @newTrxId, LineId = @newLineId 
                        //                    WHERE trxid = @trxId and LineId = @lineId";
                        //cmd.ExecuteNonQuery();

                        log.Debug("Moving Waivers Signed");
                        cmd.CommandText = @"UPDATE WaiversSigned 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Card Games");
                        cmd.CommandText = @"UPDATE CardGames 
                                            SET trxid = @newTrxId, TrxLineId = @newLineId 
                                            WHERE trxid = @trxId and TrxLineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Trx Reservation Schedule");
                        cmd.CommandText = @"UPDATE TrxReservationSchedule 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Redemption Ticket Allocation");
                        cmd.CommandText = @"UPDATE RedemptionTicketAllocation 
                                            SET trxid = @newTrxId, TrxLineId = @newLineId 
                                            WHERE trxid = @trxId and TrxLineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Card Discounts");
                        cmd.CommandText = @"UPDATE CardDiscounts 
                                            SET TransactionId = @newTrxId, LineId = @newLineId 
                                            WHERE TransactionId = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Membership Rewards Log");
                        cmd.CommandText = @"UPDATE MembershipRewardsLog 
                                            SET trxid = @newTrxId, TrxLineId = @newLineId 
                                            WHERE trxid = @trxId and TrxLineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        //cmd.CommandText = @"UPDATE LoyaltyBatchProcess 
                        //                    SET TransactionId = @newTrxId 
                        //                    WHERE TransactionId = @trxId";
                        //count = cmd.ExecuteNonQuery();
                        //log.Debug("count : " + count);

                        //cmd.CommandText = @"UPDATE LoyaltyBatchErrorLog 
                        //                    SET TransactionId = @newTrxId 
                        //                    WHERE TransactionId = @trxId";
                        //count = cmd.ExecuteNonQuery();
                        //log.Debug("count : " + count);

                        log.Debug("Moving Trx Parent Modifier Details");
                        cmd.CommandText = @"UPDATE TrxParentModifierDetails 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Attraction Bookings");
                        cmd.CommandText = @"UPDATE AttractionBookings 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving CheckIns with line");
                        cmd.CommandText = @"UPDATE CheckIns 
                                            SET CheckInTrxId = @newTrxId 
                                            WHERE CheckInTrxId = @trxId and TrxLineId IS NULL ";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving CheckIns without line");
                        cmd.CommandText = @"UPDATE CheckIns 
                                            SET CheckInTrxId = @newTrxId, TrxLineId = @newLineId 
                                            WHERE CheckInTrxId = @trxId and TrxLineId = @lineId ";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Discount Coupons Used");
                        cmd.CommandText = @"UPDATE DiscountCouponsUsed 
                                            SET trxid = @newTrxId, LineId = @newLineId 
                                            WHERE trxid = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        //cmd.CommandText = @"UPDATE TrxUserLogs 
                        //                    SET trxid = @newTrxId, LineId = @newLineId 
                        //                    WHERE trxid = @trxId and LineId = @lineId";
                        //cmd.ExecuteNonQuery();

                        log.Debug("Moving Bookings");
                        cmd.CommandText = @"UPDATE Bookings 
                                            SET trxid = @newTrxId 
                                            WHERE trxid = @trxId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Locker Allocation");
                        cmd.CommandText = @"UPDATE LockerAllocation 
                                            SET trxid = @newTrxId, TrxLineId = @newLineId 
                                            WHERE trxid = @trxId and TrxLineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);

                        log.Debug("Moving Inventory Transaction");
                        cmd.CommandText = @"UPDATE InventoryTransaction 
                                            SET ParafaitTrxId = @newTrxId, LineId = @newLineId 
                                            WHERE ParafaitTrxId = @trxId and LineId = @lineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);
                    }

                    // Update transaction parent line
                    foreach (Transaction.TransactionLine line in second.TrxLines)
                    {
                        if (line.ParentLine == null || line.ParentLine.DBLineId == 0)
                        {
                            log.Debug("line.ParentLine == null || line.ParentLine.DBLineId == 0");
                            continue;
                        }

                        if (lineIdMap.ContainsKey(line.DBLineId) == false ||
                            lineIdMap.ContainsKey(line.ParentLine.DBLineId) == false)
                        {
                            log.Debug(@"lineIdMap.ContainsKey(line.DBLineId) == false ||
                                              lineIdMap.ContainsKey(line.ParentLine.DBLineId) == false");
                            continue;
                        }
                        cmd.Parameters.Clear();
                        //Pass parameters to the query
                        cmd.Parameters.AddWithValue("@newTrxId", first.Trx_id);
                        cmd.Parameters.AddWithValue("@newLineId", lineIdMap[line.DBLineId]);
                        cmd.Parameters.AddWithValue("@newParentLineId", lineIdMap[line.ParentLine.DBLineId]);


                        log.LogVariableState("@newTrxId", first.Trx_id);
                        log.LogVariableState("@newLineId", lineIdMap[line.DBLineId]);
                        log.LogVariableState("@newParentLineId", utilities.ParafaitEnv.LoginID);

                        log.Debug("Updating transaction parent line id");
                        // Update parent lines for the merged transaction
                        cmd.CommandText = @"UPDATE trx_lines 
                                            SET ParentLineId = @newParentLineId
                                            WHERE trxid = @newTrxId and LineId = @newLineId";
                        count = cmd.ExecuteNonQuery();
                        log.Debug("count : " + count);
                    }

                    cmd.CommandText = @"delete from trxTaxLines where TrxId = @trxId;
                                        update trxPayments set trxId = @newTrxId where trxId = @trxId;
                                        update trx_header 
                                        set status = 'CANCELLED', trxAmount = 0, TaxAmount = 0, trxNetAmount = 0
                                        , cashAmount = 0, lastUpdateTime = getdate(), LastUpdatedBy = @user 
                                        where TrxId = @trxId;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@trxId", second.Trx_id);
                    cmd.Parameters.AddWithValue("@newTrxId", first.Trx_id);
                    cmd.Parameters.AddWithValue("@user", utilities.ParafaitEnv.LoginID);
                    cmd.ExecuteNonQuery();

                    mergedTransaction =
                        transactionUtils.CreateTransactionFromDB(first.Trx_id, utilities, false, false, sqlTransaction);

                    foreach (Transaction.TransactionLine line in second.TrxLines)
                    {
                        if (line.DBLineId == 0)
                        {
                            mergedTransaction.TrxLines.Add(line);
                        }
                    }

                    mergedTransaction.updateAmounts();
                    mergedTransaction.InsertTrxLogs(first.Trx_id, -1, executionContext.GetUserId(), "MERGE", "Merging with another transaction of TrxId: " + second.Trx_id.ToString() + ". Trx No: " + second.Trx_No.ToString());
                    
                    second.InsertTrxLogs(second.Trx_id, -1, utilities.ParafaitEnv.LoginID, "DELETE LINES", "Order moved to another Trx " + first.Trx_id.ToString() + ". Trx No: " + first.Trx_No.ToString(), cmd.Transaction);
                    log.LogVariableState("@trxId", second.Trx_id);
                    log.LogVariableState("@newTrxId", first.Trx_id);
                    log.LogVariableState("@user", utilities.ParafaitEnv.LoginID);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while cancelling merged transaction", ex);
                    log.LogMethodExit("Throwing Exception - " + ex);
                    throw ;
                }
            }
            log.LogMethodExit(mergedTransaction);
            return mergedTransaction;
        }

        public void CheckValidOrderTypeGroupExist(List<Transaction.TransactionLine> TrxLines)
        {
            log.LogMethodEntry(TrxLines);
            HashSet<int> orderTypeIdSet = null;
            int orderTypeGroupId = -1;
            if (TrxLines != null || TrxLines.Count > 0)
            {
                orderTypeIdSet = new HashSet<int>();
                foreach (var line in TrxLines)
                {
                    if (line.LineValid)
                    {
                        orderTypeIdSet.Add(line.OrderTypeId);
                    }
                }
                if (orderTypeIdSet.SetEquals(new HashSet<int>() { -1 }))
                {
                    orderTypeGroupId = -1;
                }
                else
                {
                    OrderTypeGroupDTO orderTypeGroupDTO = GetOrderTypeGroup(orderTypeIdSet);
                    if (orderTypeGroupDTO != null)
                    {
                        orderTypeGroupId = orderTypeGroupDTO.Id;
                    }
                    else
                    {
                        throw new Exception("No valid order type group can be found");
                    }
                }
            }
            log.LogMethodExit();
        }

        private OrderTypeGroupDTO GetOrderTypeGroup(HashSet<int> orderTypeIdSet)
        {
            log.LogMethodEntry(orderTypeIdSet);
            List<OrderTypeGroupBL> orderTypeGroupBLList = GetOrderTypeGroupBLList();
            OrderTypeGroupDTO orderTypeGroupDTO = null;
            foreach (var orderTypeGroupBL in orderTypeGroupBLList)
            {
                if (orderTypeGroupBL.Match(orderTypeIdSet))
                {
                    if (orderTypeGroupDTO == null || orderTypeGroupDTO.Precedence < orderTypeGroupBL.OrderTypeGroupDTO.Precedence)
                    {
                        orderTypeGroupDTO = orderTypeGroupBL.OrderTypeGroupDTO;
                    }
                }
            }
            log.LogMethodExit(orderTypeGroupDTO);
            return orderTypeGroupDTO;
        }

        private List<OrderTypeGroupBL> GetOrderTypeGroupBLList()
        {
            log.LogMethodEntry();
            OrderTypeGroupListBL orderTypeGroupListBL = new OrderTypeGroupListBL(executionContext);
            List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, (utilities.ParafaitEnv.IsCorporate ? utilities.ParafaitEnv.SiteId : -1).ToString()));
            searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, 1.ToString()));
            List<OrderTypeGroupDTO> orderTypeGroupDTOList = orderTypeGroupListBL.GetOrderTypeGroupDTOList(searchParameters);
            List<OrderTypeGroupBL> orderTypeGroupBLList = new List<OrderTypeGroupBL>();
            if (orderTypeGroupDTOList != null)
            {
                foreach (var ordTypGrpDTO in orderTypeGroupDTOList)
                {
                    orderTypeGroupBLList.Add(new OrderTypeGroupBL(executionContext, ordTypGrpDTO));
                }
            }
            log.LogMethodExit(orderTypeGroupBLList);
            return orderTypeGroupBLList;
        }
    }
}
