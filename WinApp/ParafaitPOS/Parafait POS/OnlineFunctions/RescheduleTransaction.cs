/********************************************************************************************
* Project Name - Parafait_POS - ItemPanel
* Description  - ItemPanel 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
********************************************************************************************* 
*2.60.0      21-Mar-2019      Iqbal              Created 
*2.70.0      20-Apr-2019      Guru S A           Booking phase 2 changes
*2.70.0      24-Jul-2019      Nitin Pai          Attraction enhancement for combo products
*2.80.0      22-Apr-2020      Guru S A            Ability to reschedule attraction schedule selection as per reservation change
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Transaction;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;
using Parafait_POS.Attraction;
using Semnox.Parafait.Customer;

namespace Parafait_POS.OnlineFunctions
{
    class RescheduleTransaction
    {
        int trxId;
        DateTime newTrxDate;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RescheduleTransaction(int TrxId, DateTime NewTrxDate)
        {
            log.LogMethodEntry(TrxId, NewTrxDate);
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            trxId = TrxId;
            newTrxDate = NewTrxDate;
            log.LogMethodExit();
        }

        public bool Perform()
        {
            log.LogMethodEntry();
            TransactionUtils TransactionUtils = new TransactionUtils(POSStatic.Utilities);
            Transaction transaction = TransactionUtils.CreateTransactionFromDB(trxId, POSStatic.Utilities);
            if (newTrxDate.Date < POSStatic.Utilities.getServerTime().Date)
            {
                throw new ApplicationException(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 2068));// "New date cannot be earlier than today");
            }
            List<List<Transaction.TransactionLine>> attractionLines = new List<List<Transaction.TransactionLine>>(); // group of trx lines with same attraction product
            Dictionary<int, int> quantityMap = new Dictionary<int, int>();

            foreach (var tl in transaction.TrxLines)
            {
                if (tl.ProductTypeCode == "ATTRACTION" && tl.LineAtb != null && !tl.LineProcessed)
                {
                    List<Transaction.TransactionLine> lines = new List<Transaction.TransactionLine>();
                    foreach (var attractionLine in transaction.TrxLines)
                    {
                        if (tl.ProductID == attractionLine.ProductID && !attractionLine.LineProcessed)
                        {
                            lines.Add(attractionLine);
                            attractionLine.LineProcessed = true;
                            if (quantityMap.ContainsKey(tl.ProductID))
                            {
                                quantityMap[tl.ProductID] = quantityMap[tl.ProductID] + Decimal.ToInt32(tl.quantity);
                            }
                            else
                            {
                                quantityMap.Add(tl.ProductID, Decimal.ToInt32(tl.quantity));
                            }
                        }
                    }
                    
                    attractionLines.Add(lines);
                }
            }

            if (attractionLines.Count == 0
                 && newTrxDate.Date.Equals(transaction.TransactionDate.Date))
            {
                throw new ApplicationException(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 2069));//"New date and current transaction date are same. Nothing to reshedule.");
            }


            //List<AttractionBooking> atbList = new List<AttractionBooking>();
            //foreach (var attrLines in attractionLines)
            //{
            //    int quantity = attrLines.Count;
            //    int productId = attrLines[0].ProductID;

                
            //    using (AttractionSchedules ats = new AttractionSchedules(null, productId, quantity, newTrxDate, "-1"))
            //    {
            //        List<AttractionBookingDTO> scheduleList = new List<AttractionBookingDTO>();
            //        if (ats.ShowSchedules(scheduleList) == true)
            //        {
            //            foreach (AttractionBookingDTO schedule in scheduleList)
            //            {
            //                int count = schedule.BookedUnits;
            //                while (count-- > 0)
            //                {
            //                    AttractionBooking atb = new AttractionBooking(POSStatic.Utilities.ExecutionContext, schedule);
            //                    atbList.Add(atb);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            log.LogMethodExit(false);
            //            return false;
            //        }
            //    }
            //}

            String message = "";
            List<AttractionBooking> attractionBookings = null;
            CustomerDTO customerDTO = (transaction != null
                                       && transaction.PrimaryCard != null
                                       && transaction.PrimaryCard.customerDTO != null ? transaction.PrimaryCard.customerDTO :
                                       (transaction != null && transaction.customerDTO != null ? transaction.customerDTO : null));

            if (attractionLines != null && attractionLines.Any())
            {
                using (AttractionSchedule attractionSchedules = new AttractionSchedule(POSStatic.Utilities, POSStatic.Utilities.ExecutionContext))
                {
                    attractionBookings = attractionSchedules.ShowSchedulesForNonPOSScreens(quantityMap, null, newTrxDate, null,
                                                -1, false, customerDTO, -1, ref message);

                    log.LogVariableState("attractionBookings", attractionBookings);
                    if (attractionBookings == null)
                    {
                        //No schedules available for the day now.Please select another date
                        throw new ApplicationException(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 5158));
                    }
                    if (!attractionBookings.Any())
                    {                        
                        //"No attractions selected. Nothing to reshedule."
                        throw new ApplicationException(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 2097));
                    }
                }
            }

            using (SqlTransaction sqlTrx = POSStatic.Utilities.getConnection().BeginTransaction())
            {
                try
                {
                    InvalidateCurrentEntitlements(transaction, sqlTrx);
                    
                    int atbIndex = 0;
                    List<AttractionBooking> oldAtbList = new List<AttractionBooking>();
                    foreach (var attrLines in attractionLines)
                    {
                        List<AttractionBooking> atbList = attractionBookings.Where(x => x.AttractionBookingDTO.AttractionProductId == attrLines[0].ProductID).ToList();
                        if (atbList.Any())
                        {
                            foreach (var trxLine in attrLines)
                            {
                                try
                                {
                                    AttractionBooking atb = atbList[0];
                                    AttractionBooking clone = new AttractionBooking(POSStatic.Utilities.ExecutionContext);
                                    clone.CloneObject(atb, 1);
                                    clone.AttractionBookingDTO.AttractionProductId = atb.AttractionBookingDTO.AttractionProductId;
                                    clone.AttractionBookingDTO.TrxId = trxId;
                                    clone.AttractionBookingDTO.LineId = trxLine.DBLineId;
                                    clone.AttractionBookingDTO.ExpiryDate = DateTime.MinValue;
                                    clone.Save(trxLine.card == null ? -1 : trxLine.card.card_id, sqlTrx);
                                    oldAtbList.Add(trxLine.LineAtb);
                                    trxLine.LineAtb = clone;
                                    if (atb.AttractionBookingDTO.BookedUnits > 1)
                                        atb.AttractionBookingDTO.BookedUnits -= 1;
                                    else
                                        attractionBookings.Remove(atb);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    log.Info("Unable to Save AttractionBooking. Error: " + ex.Message);
                                    throw new ApplicationException(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 2070, ex.Message));// "Attraction Save - " + message);
                                }
                            }
                        }
                    }

                    CreateNewEntitlements(transaction, sqlTrx);

                    foreach (var atb in oldAtbList)
                    {
                        atb.Expire(sqlTrx);
                    }

                    sqlTrx.Commit();

                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    sqlTrx.Rollback();
                    log.LogMethodExit();
                    throw;
                }
            }
        }

        void InvalidateCurrentEntitlements(Transaction transaction, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            POSStatic.Utilities.executeNonQuery(@"update cardGames 
                                                    set ExpiryDate = getdate(),
                                                        LastUpdatedBy = @userId,
                                                        Last_update_date = getdate(),
                                                        IsActive = 0
                                                  where TrxId = @trxId",
                                            sqlTrx,
                                            new SqlParameter("@trxId", trxId),
                                            new SqlParameter("@userId", POSStatic.Utilities.ParafaitEnv.LoginID));

            POSStatic.Utilities.executeNonQuery(@"update cardCreditPlus 
                                                    set PeriodTo = getdate(), 
                                                        LastUpdatedBy = @userId,
                                                        LastUpdatedDate = getdate(),
                                                        IsActive = 0
                                                  where TrxId = @trxId",
                                            sqlTrx,
                                            new SqlParameter("@trxId", trxId),
                                            new SqlParameter("@userId", POSStatic.Utilities.ParafaitEnv.LoginID));

            POSStatic.Utilities.executeNonQuery(@"update cardDiscounts 
                                                    set Expiry_Date = getdate(), 
                                                        last_updated_user = @userId,
                                                        last_updated_date = getdate(),
                                                        IsActive = 'N'
                                                  where TransactionId = @trxId",
                                            sqlTrx,
                                            new SqlParameter("@trxId", trxId),
                                            new SqlParameter("@userId", POSStatic.Utilities.ParafaitEnv.LoginID));
            log.LogMethodExit();
        }

        void CreateNewEntitlements(Transaction transaction, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            double businessDayStartTime = 6;
            double.TryParse(POSStatic.Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);

            transaction.EntitlementReferenceDate = newTrxDate.Date.AddHours(businessDayStartTime);

            POSStatic.Utilities.executeNonQuery(@"update trx_header 
                                                    set TrxDate = @trxDate, LastUpdateTime = getdate(), LastUpdatedBy = @userId
                                                  where TrxId = @trxId",
                                                sqlTrx,
                                                new SqlParameter("@trxId", trxId),
                                                new SqlParameter("@trxDate", transaction.EntitlementReferenceDate),
                                                new SqlParameter("@userId", POSStatic.Utilities.ParafaitEnv.User_Id));

            int timeDiff = (int)(newTrxDate.Date - transaction.TransactionDate.Date).TotalMinutes;
            bool reservationTransaction = transaction.IsReservationTransaction(sqlTrx);
            foreach (var tl in transaction.TrxLines)
            {
                if (tl.card != null)
                {
                    int subscriptionLineCount = (tl.SubscriptionHeaderDTO != null && tl.SubscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null
                                                     && tl.SubscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any() ? tl.SubscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count : 1);
                    for (int j = 0; j < subscriptionLineCount; j++)
                    {
                        SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = (tl.SubscriptionHeaderDTO != null && tl.SubscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null
                                                                                             && tl.SubscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any()
                                                                                           ? tl.SubscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j] : null);

                        transaction.CreateCardGames(sqlTrx, tl.card.card_id, tl.ProductID, trxId, tl.DBLineId, tl, subscriptionBillingScheduleDTO, reservationTransaction);
                        transaction.CreateCardCreditPlus(sqlTrx, tl.card.card_id, tl.ProductID, trxId, tl.DBLineId, tl, subscriptionBillingScheduleDTO, reservationTransaction);
                        transaction.CreateCardDiscounts(sqlTrx, trxId, tl.DBLineId, tl.card.card_id, tl.ProductID, tl, subscriptionBillingScheduleDTO, reservationTransaction);
                    }
                    POSStatic.Utilities.executeNonQuery(@"update cards
                                                                    set ExpiryDate = DateAdd(MINUTE, @diff, cards.ExpiryDate), 
                                                                        LastUpdatedBy = @userId,
                                                                        Last_update_time = getdate()
                                                                    from trx_lines tl, products p
                                                                    where tl.product_id = p.product_id
                                                                    and tl.card_id = cards.card_id
                                                                    and p.CardValidFor > 0
                                                                    and tl.TrxId = @trxId
                                                                    and tl.LineId = @lineId",
                                                        sqlTrx,
                                                        new SqlParameter("@trxId", trxId),
                                                        new SqlParameter("@lineId", tl.DBLineId),
                                                        new SqlParameter("@diff", timeDiff),
                                                        new SqlParameter("@userId", POSStatic.Utilities.ParafaitEnv.LoginID));
                }
            }

            // Call the CompleteTransaction method to mark the credit plus lines as completed 
            String message = "";
            if (!transaction.CompleteTransaction(sqlTrx, ref message))
            {
                throw new Exception(message);
            }

            log.LogMethodExit();
        }
    }
}
