/************************************************************************************************************************************
* Project Name - Semnox.Parafait.Transaction - TransactionUtils
* Description  - TransactionUtils 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*************************************************************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Online Transaction in Kiosk changes 
*2.4.0       04-Oct-2018      Indhu K            Modified for Modifierchanges
*2.50.0      18-Dec-2018      Mathew Ninan       Deprecating StaticDataExchange. Transaction and 
*                                                TrxPayment moved to 3 tier framework
*2.50.0      30-Jan-2019      Lakshminarayana    Fixed discount bug
*2.60.0      05-Mar-2019      Nitin Pai          Bear Cat 86-68 Created 
*2.60.0      17-Apr-2019      Divya A            Manager Approval Changes
*2.60.2      03-Jun-2019      Mathew Ninan       Credits should be deducted if New card and recharge is done
*                                                in same transaction. Active flag check was done 
*                                                after card was inactivated
*2.60.2      17-Jun-2019      Nitin Pai          Update 86-68 stock only if the flag is turned on   
*2.70.0      02-Jul-2019      Mathew Ninan       Moved Check-in to 3 Tier. ReverseCheckIns
*                                                and CreateTransactionfromDB modified      
*2.70        26-Mar-2019      Guru S A           Booking phase 2 enhancement changes                                           
*2.70        17-Mar-2019      Akshay G           modified discountCouponsDTO.isActive (from string to bool)
*2.70.2      22-Aug-2019      Jinto Thomas       Signature changes in taskProcs.createTask method call
*2.70.2      04-Oct-2019      Jinto Thomas       Modified insert trx_header query. Added createdby,
*                                                                          creationdate fields
*2.70.2        07-Nov-2019     Akshay G          ClubSpeed enhancement changes - Added TrxLine.lastUpdateDate in the method CreateTransactionFromDB()
*2.70.2        07-Nov-2019     Guru S A          Waiver phase 2 enhancement changes
*2.70.2         26-Nov-2019    Lakshminarayana   Virtual store enhancement
*2.70.2        04-Feb-2020     Nitin Pai         Fixes for reschedule attraction and slot
*2.70.3        20-Apr-2020     Archana           RevrseTransaction() is modified to include loadTickets and LoadBonus reversal
*2.80.0       26-May-2020      Dakshakh          CardCount enhancement for product type Cardsale/GameTime/Attraction/Combo 
*2.90.0       23-Jun-2020      Raghuveera        Variable refund changes in ReverseTransactionEntity() for saving the order during reversal
*2.90.0       23-Jun-2020      Girish Kundar     Modified : BowaPegas shift open close and Viber changes
*2.100.0      06-Aug-2020      Mathew Ninan      Capture Save timing for Transaction 
*2.100.0      26-Sept-2020     Girish Kundar     Modified : CashDrawer modification for Bowapegas 
*2.100        24-Sep-2020      Nitin Pai         Attraction Reschdule: Changed to look at DAS schedule instead of ATB schedule
*             29-Oct-2020      Mathew Ninan      added LinkChildCard to GetProductDetails and GetTransactionFromDB 
*2.110.0      08-Dec-2020      Guru S A          Subscription changes
*2.110.0      14-Dec-2020      Dakshakh Raj      Modified: for Peru Invoice Enhancement   
*2.140.0      14-Sep-2021      Guru S A          Waiver mapping UI enhancements
*2.140.0      01-Jun-2021      Fiona Lishal      Modified for Delivery Order enhancements for F&B
*2.140.0      09-Sep-2021      Girish Kundar     Modified: Check In/Check out changes
*2.130.4      08-Feb-2022	   Girish Kundar     Modified: Smartro Fiscalization
*2.130.4      22-Feb-2022      Mathew Ninan      Modified DateTime to ServerDateTime
*2.150.3      30-Jun-2023      Abhishek          Modified:Hecere Locker Integration, to erase card associated with Hecere Locker.
*2.150.3      28-Jul-2023      Prashanth         Modified: Changes made to ReverseCard method to load deducted tickets back to card's credit plus level during reversal action
*2.155.0      04-Jul-2023      Muaaz Musthafa    Modified: Added CreditPlusConsumptionId in CreateTransactionDB()
****************************************************************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Printer;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.Customer.Waivers;

namespace Semnox.Parafait.Transaction
{
    public class TransactionUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;
        ParafaitEnv ParafaitEnv;
        //Added the below variable on 15-Dec-2015 to print reversal transaction receipt//
        System.Drawing.Printing.PrintDocument MyPrintDocument;
        int transactionId = -1;
        int reversalTransactionId = -1;
        double reversalAmount = 0.0;
        //End 15-Dec-2015
        /// <summary>
        /// ReversalTransactionId
        /// </summary>
        public int ReversalTransactionId { get { return reversalTransactionId; } }
        /// <summary>
        /// ReaderDevice
        /// </summary>
        public DeviceClass ReaderDevice;
        private bool openCashDrawer;
        /// <summary>
        /// OpenCashDrawer
        /// </summary>
        public bool OpenCashDrawer   // property used for Bowa changes to restrict the cash drawer for type A in ViewTransactionUI - reversal
        {
            get { return openCashDrawer; }   // get method
        }
        /// <summary>
        /// TransactionUtils
        /// </summary>
        /// <param name="ParafaitUtilities"></param>
        public TransactionUtils(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);

            Utilities = ParafaitUtilities;
            ParafaitEnv = Utilities.ParafaitEnv;
            openCashDrawer = true;
            ReaderDevice = Utilities.ReaderDevice;

            log.LogMethodExit(null);
        }

        public bool reverseTransaction(int TrxId, int TrxLineId, bool ReverseCardUpdates, string POSMachine, string loginId, int userId, string Approver, string remarks, ref string Message, bool disableAutoPrintCheck = false) //19-Oct-2015 Modification done for fiscal printer
        {
            log.LogMethodEntry(TrxId, TrxLineId, ReverseCardUpdates, POSMachine, loginId, userId, Approver, remarks, Message, disableAutoPrintCheck);
            DateTime saveStartTime = Utilities.getServerTime();
            using (SqlConnection cnn = Utilities.createConnection())
            {
                SqlCommand cmd = new SqlCommand("", cnn);

                if ((Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "DISABLE_REVERSAL_OF_CLOSED_TRANSACTION_PAST_DAYS")).Equals("Y"))
                {
                    DateTime bussStartTime = Utilities.getServerTime().Date.AddHours(Convert.ToInt32(Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME")));
                    DateTime bussEndTime = bussStartTime.AddDays(1);
                    if (Utilities.getServerTime() < bussStartTime)
                    {
                        bussStartTime = bussStartTime.AddDays(-1);
                        bussEndTime = bussStartTime.AddDays(1);
                    }
                    cmd.CommandText = @"select trxDate from trx_header where trxid = @trxid and status = 'CLOSED'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@trxid", TrxId);
                    object trxDate = cmd.ExecuteScalar();
                    if (trxDate != null && Convert.ToDateTime(trxDate) < bussStartTime)
                    {
                        Message = Utilities.MessageUtils.getMessage(2442, (int)(bussStartTime - Convert.ToDateTime(trxDate)).TotalDays, 1);

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    cmd.Parameters.Clear();
                }
                cmd.CommandText = @"select top 1 1
                                    from products pp , trx_lines l
                                    where  l.trxId = @trxid  
                                    and  pp.product_id = l.product_Id 
                                    and  pp.product_name in ('Card Consolidation Task','Consolidation Task From Card', 'Consolidation Task To Card')";
                cmd.Parameters.AddWithValue("@trxid", TrxId);
                if (cmd.ExecuteScalar() != null)
                {
                    Message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2640);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                cmd.Parameters.Clear();

                cmd.CommandText = @"SELECT 1 from TRX_HEADER where TrxId = @trxid and status = 'PENDING'";
                cmd.Parameters.AddWithValue("@trxid", TrxId);
                if (cmd.ExecuteScalar() != null)
                {
                    Message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 1413);
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                cmd.Parameters.Clear();
                cmd.CommandText = @"select top 1 'REDEEMLOYALTY'
                                    from  trx_lines l, products pp
                                    where l.trxId = @trxId
                                    and l.product_id = pp.product_id
                                    and pp.product_name = 'Loyalty Product'
                                    and NOT exists (SELECT 1
                                                    from trx_lines tlIn
                                                    where tlIn.TrxId = l.TrxId
                                                    and l.product_Id != tlIn.product_id)
                                                            union all
                                                (select top 1 'SALESRETURNEXCHANGE'
                                                from tasks t, task_type tt
                                                where tt.task_type = 'SALESRETURNEXCHANGE'  
                                                and tt.task_type_id = t.task_type_id
                                                and ISNULL(t.trxId, t.attribute2) = @trxId)";
                cmd.Parameters.AddWithValue("@trxId", TrxId);
                var returnVariable = cmd.ExecuteScalar();
                if (returnVariable != null)
                {
                    Message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2663, returnVariable.ToString());
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                cmd.Parameters.Clear();
                cmd.CommandText = @"select top 1 1 
                                from trx_lines l, cards c, trx_header h
                                where (l.card_id = c.card_id  
                                       or exists (select trxp.CardId
                                                    from trxPayments trxp   
                                                   where trxp.TrxId= l.TrxId
                                                     and trxp.trxId = @trxId 
                                                     and trxp.CardId=c.card_id 
                                                     and  c.refund_flag <> 'N')
                                       )
                                and h.trxId = l.trxId
                                and h.trxId = @trxId
                                and (h.site_id = @SiteId or @SiteId = -1)
                                and (l.LineId = @lineId or @lineId = -1)
                                and (c.refund_flag = 'Y'
                                    or exists (select 1
                                                from gameplay gp
                                                where gp.card_id = c.card_id
                                                and gp.play_date > h.trxDate
                                                and not exists (select 1 
	                                                            from trx_header th, trx_lines tlin, 
						                                            products pp, TransactionLineGamePlayMapping tlg
					                                            where th.TrxId = tlin.TrxId
					                                            and th.Status not in ('CANCELLED','SYSTEMABANDONED')
					                                            and tlin.product_id = pp.product_id
					                                            and pp.product_name = 'Load Bonus Task'
					                                            and tlin.CancelledTime is null
						                                        and tlin.TrxId = tlg.TrxId
						                                        and tlin.LineId = tlg.LineId
						                                        and tlg.IsActive = 1
						                                        and tlg.GamePlayId = gp.gameplay_id
						                                                )
                                               )
								    or exists (select 1
									             from redemption R, Redemption_cards rc
												WHERE r.redemption_id = rc.redemption_id
												  and rc.card_id = c.card_id
                                                  and ISNULL(rc.ticket_count,0) != 0
												  and r.redeemed_date > h.trxdate)
                                    or exists (select 1
                                                from tasks t, task_type tp
                                                where t.task_type_id = tp.task_type_id
												and (t.card_id = c.card_id
														OR (T.transfer_to_card_id IS NOT NULL 
														    AND T.transfer_to_card_id = C.card_id)
														OR (T.consolidate_card1 IS NOT NULL 
														    AND T.consolidate_card1 = C.card_id)
														OR (T.consolidate_card2 IS NOT NULL 
														    AND T.consolidate_card2 = C.card_id)
														OR (T.consolidate_card3 IS NOT NULL 
														   AND T.consolidate_card3 = C.card_id)
														OR (T.consolidate_card4 IS NOT NULL 
														    AND T.consolidate_card4 = C.card_id)
														OR (T.consolidate_card5 IS NOT NULL 
														    AND T.consolidate_card5 = C.card_id)
													)
                                                and not exists (select 1 
                                                            FROM transactionlinegameplaymapping tlg
                                                            WHERE tlg.trxid = t.trxid
                                                              AND tp.task_type = 'LOADBONUS')
                                                and t.task_date > h.trxdate
                                                and isnull(t.TrxId, -1) != h.trxId)
                                    or exists (select 1
                                                from trxPayments tp
                                                where c.card_id = tp.CardId
                                                and tp.PaymentDate > h.trxDate 
                                                and tp.trxId != h.trxId))";
                cmd.Parameters.AddWithValue("@trxId", TrxId);
                cmd.Parameters.AddWithValue("@lineId", TrxLineId);

                log.LogVariableState("@trxId", TrxId);
                log.LogVariableState("@lineId", TrxLineId);

                if (Utilities.ParafaitEnv.IsCorporate == false || Utilities.ParafaitEnv.SiteId <= 0)
                {
                    log.LogVariableState("@SiteId", -1);
                    cmd.Parameters.AddWithValue("@SiteId", -1);
                }
                else
                {
                    log.LogVariableState("@SiteId", Utilities.ParafaitEnv.SiteId);
                    cmd.Parameters.AddWithValue("@SiteId", Utilities.ParafaitEnv.SiteId);
                }
                cmd.CommandTimeout = 600; //10 minutes
                if (cmd.ExecuteScalar() != null)
                {
                    Message = Utilities.MessageUtils.getMessage(334);

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }

                TransactionListBL transactionListBL = new TransactionListBL(Utilities.ExecutionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, TrxId.ToString()));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));

                List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(searchParam, Utilities, null, 0, 1000, true);
                log.LogVariableState("transactionDTOList", transactionDTOList);

                string legacyProductId = "-1";
                LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "LEGACY_CARD_TRANSFER_PRODUCT_ID"));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    legacyProductId = lookupValuesDTOList[0].Description;
                }
                if (transactionDTOList != null && transactionDTOList.Any() && transactionDTOList[0].TransactionLinesDTOList != null && transactionDTOList[0].TransactionLinesDTOList.Any())
                {
                    if (transactionDTOList[0].TransactionLinesDTOList[0].ProductTypeCode == ProductTypeValues.GENERICSALE && transactionDTOList[0].TransactionLinesDTOList[0].ProductId.Equals(Convert.ToInt32(legacyProductId)))
                    {
                        log.LogMethodExit();
                        throw new Exception(Utilities.MessageUtils.getMessage(4103));
                    }
                    else if (transactionDTOList[0].TransactionLinesDTOList.Exists(t => t.ProductTypeCode == ProductTypeValues.CHECKIN
                                                                                    || t.ProductTypeCode == ProductTypeValues.CHECKOUT))
                    {
                        List<TransactionLineDTO> checkInCheckoutLines = transactionDTOList[0].TransactionLinesDTOList
                            .FindAll(t => t.ProductTypeCode == ProductTypeValues.CHECKIN || t.ProductTypeCode == ProductTypeValues.CHECKOUT).ToList();
                        if (checkInCheckoutLines != null && checkInCheckoutLines.Any())
                        {
                            if (TrxLineId != -1)
                            {
                                int producId = transactionDTOList[0].TransactionLinesDTOList[TrxLineId - 1].ProductId;
                                ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext.SiteId, producId);
                                if (productsContainerDTO.PauseType == ProductsContainerDTO.PauseUnPauseType.PAUSE
                                    || productsContainerDTO.PauseType == ProductsContainerDTO.PauseUnPauseType.UNPAUSE)
                                {
                                    log.LogMethodExit();
                                    throw new Exception(Utilities.MessageUtils.getMessage(4807)); //Transaction Reversal for Pause/UnPause Operation is not allowed
                                }
                            }
                            else
                            {//do not allow header level reversal even if it contains 1 Pause/UnPause line
                                foreach(TransactionLineDTO line in checkInCheckoutLines)
                                {
                                    ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext.SiteId, line.ProductId);
                                    if (productsContainerDTO.PauseType == ProductsContainerDTO.PauseUnPauseType.PAUSE
                                         || productsContainerDTO.PauseType == ProductsContainerDTO.PauseUnPauseType.UNPAUSE)
                                    {
                                        log.LogMethodExit();
                                        throw new Exception(Utilities.MessageUtils.getMessage(4807)); //Transaction Reversal for Pause/UnPause Operation is not allowed
                                    }
                                }
                            }
                        }

                    }
                }
                cmd.CommandText = @"select top 1 1 
                                        from trx_header h
                                        where (@lineId != -1 and PaymentReference like '%Trx Id/Line Id: ' + @trxIdStr + '/' + @lineIdStr + ':%')
                                            or (@lineId = -1 and PaymentReference like '%Trx Id% ' + @trxIdStr + '%')
                                            or (@lineId != -1 and PaymentReference like '%Trx Id:% ' + @trxIdStr + ':%')
                                            or (TrxId = @TrxId and PaymentReference like 'Reversal of Trx Id%')
	                                        or (@lineId = -1 and  OriginalTrxID = @trxid )
	                                        or (@lineId != -1 and 1 = isnull((select top 1 1 
	                                                                            from trx_lines l 
	                                                                            where h.trxId = l.trxId 
										                                            and h.OriginalTrxID = @trxid
	                                                                                and l.OriginalLineID = @lineId), -1)
                                                )";

                cmd.Parameters.AddWithValue("@trxIdStr", TrxId.ToString());
                cmd.Parameters.AddWithValue("@lineIdStr", TrxLineId.ToString());

                if (cmd.ExecuteScalar() != null)
                {
                    Message = Utilities.MessageUtils.getMessage(335);

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }

                cmd.CommandText = "select status from Trx_header where trxid = @trxId";
                string status = cmd.ExecuteScalar().ToString();
                if (status == Transaction.TrxStatus.CANCELLED.ToString() || status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
                {
                    Message = Utilities.MessageUtils.getMessage(509);

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }

                cmd.CommandText = @"SELECT 1 FROM trx_lines tl 
                           WHERE TrxId=@oldTrxId AND (LineId=@lineId OR @lineId=-1) 
                           AND EXISTS(SELECT * FROM products p 
                                               WHERE p.product_id = tl.product_id AND p.product_type_id 
                                                     IN (SELECT pt.product_type_id FROM product_type pt 
                                                                                   WHERE product_type = 'LOCKER_RETURN' or product_type = 'RENTAL_RETURN'))";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@oldTrxId", TrxId);
                cmd.Parameters.AddWithValue("@TrxId", TrxId);
                cmd.Parameters.AddWithValue("@lineId", TrxLineId);
                log.LogVariableState("@oldTrxId", TrxId);
                log.LogVariableState("@lineId", TrxLineId);
                object unreversableTrxns = cmd.ExecuteScalar();
                if (unreversableTrxns != null)
                {
                    Message = Utilities.MessageUtils.getMessage(2361);
                    return false;
                }

                //Modified the query to Include condition trx_header Original_System_Reference to avoid messagebox prompting for online Transcations Jun-30-2016//
                if (Utilities.ParafaitEnv.IsClientServer)
                {
                    cmd.CommandText = "select top 1 1 from trx_header h,trxPayments tp, PaymentModes m where h.TrxId=tp.TrxId and m.PaymentModeId = tp.PaymentModeId and tp.TrxId = @trxId and m.isCreditCard = 'Y' and  h.Original_System_Reference is null";

                    if (cmd.ExecuteScalar() != null && status != "BOOKING" && status != "RESERVED") //Change done on 18-Jan-2016
                    {
                        if (MessageBox.Show(Utilities.MessageUtils.getMessage(336), "Credit Card Payment", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            Message = Utilities.MessageUtils.getMessage(337);

                            log.LogVariableState("Message ", Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
                if (IsPostTransactionProcessIsolated("POSPlus").Equals("N"))//starts:Modification on 17-May-2016 for adding PosPlus 
                {
                    cmd.CommandText = @"select External_System_Reference from trx_header where trxid=@Trxid";
                    //cmd.Parameters.AddWithValue("@Trxid", TrxId.ToString());
                    if (cmd.ExecuteScalar() == null)
                    {
                        Message = Utilities.MessageUtils.getMessage("Please complete the transaction.");

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }

                }//Ends:Modification on 17-May-2016 for adding PosPlus

                if (TrxLineId > 0)
                {
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter sqlDataAdapter;
                    cmd.CommandText = @"select 1 
                                          from trx_lines tl,Products p,  product_type pt 
                                         where tl.product_id = p.product_id and tl.TrxId = @TrxId and tl.lineId= @LineId
		                                   and p.product_type_id = pt.product_type_id  
									       and isnull(p.site_id,-1) = isnull(pt.site_id,-1)
                                           and pt.product_type = 'LOCKER'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrxId", TrxId);
                    cmd.Parameters.AddWithValue("@LineId", TrxLineId);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {

                        cmd.CommandText = @"select tl.* from trx_lines tl,Products p , product_type pt 
                                             where tl.product_id = p.product_id 
                                               and tl.TrxId = @TrxId 
                                               and tl.cancelledTime is null
                                               and tl.lineId = @LineId
		                                       and pt.product_type_id = p.product_type_id 
									           and isnull(p.site_id,-1) = isnull(pt.site_id,-1) 
                                               and pt.product_type = 'LOCKERDEPOSIT'";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@TrxId", TrxId);
                        cmd.Parameters.AddWithValue("@LineId", TrxLineId - 1);
                        sqlDataAdapter = new SqlDataAdapter(cmd);
                        sqlDataAdapter.Fill(dataTable);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dataTable.Rows[0]["LineId"]) != TrxLineId)
                            {
                                reverseTransaction(TrxId, Convert.ToInt32(dataTable.Rows[0]["LineId"]), ReverseCardUpdates, POSMachine, loginId, userId, Approver, remarks, ref Message, disableAutoPrintCheck);
                            }
                        }
                    }
                    cmd.CommandText = @"select 1 from trx_lines tl,Products p, product_type pt 
                                            where tl.product_id = p.product_id and tl.TrxId = @TrxId and tl.lineId= @LineId
		                                       and  pt.product_type_id = p.product_type_id 
									                 and isnull(p.site_id,-1) = isnull(pt.site_id,-1) and pt.product_type = 'RENTAL'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrxId", TrxId);
                    cmd.Parameters.AddWithValue("@LineId", TrxLineId);

                    result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        cmd.CommandText = @"select tl.* from trx_lines tl,Products p, product_type pt 
                                            where tl.product_id = p.product_id 
                                                     and tl.TrxId = @TrxId and tl.lineId = @lineId
                                                     and tl.cancelledTime is null
		                                             and pt.product_type_id = p.product_type_id 
									                 and isnull(p.site_id,-1) = isnull(pt.site_id,-1) and pt.product_type = 'DEPOSIT'";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@TrxId", TrxId);
                        cmd.Parameters.AddWithValue("@lineId", TrxLineId - 1);
                        log.LogVariableState("@TrxId", TrxId);
                        dataTable = new DataTable();
                        sqlDataAdapter = new SqlDataAdapter(cmd);
                        sqlDataAdapter.Fill(dataTable);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dataTable.Rows[0]["LineId"]) != TrxLineId)
                            {
                                reverseTransaction(TrxId, Convert.ToInt32(dataTable.Rows[0]["LineId"]), ReverseCardUpdates, POSMachine, loginId, userId, Approver, remarks, ref Message, disableAutoPrintCheck);
                            }
                        }
                    }
                }
                SqlTransaction sqlTrx = cnn.BeginTransaction();
                cmd.Connection = cnn;
                cmd.Transaction = sqlTrx;
                cmd.Parameters.Clear();
                bool printNonFiscalReceipt = false;
                int reversalTrxId = 0;
                if (!ReverseTransactionEntity(TrxId, TrxLineId, loginId, userId, Approver, remarks, ref Message, ref reversalTrxId, sqlTrx, cnn))
                {
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                if (reversalTrxId > 0)
                {
                    try
                    {
                        Transaction captureTrxTime = new Transaction(Utilities);
                        captureTrxTime.UpdateTrxHeaderSavePrintTime(reversalTrxId, saveStartTime, Utilities.getServerTime(), null, null, sqlTrx);

                        updateStock(TrxId, TrxLineId, reversalTrxId, loginId, sqlTrx);

                        //Modified 02/2019 for BearCat - 86-68
                        if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ALLOW_PRODUCTS_TOBE_MARKED_UNAVAILABLE"))
                        {
                            UpdateAvailableQuantityForCancelledTransaction(TrxId, sqlTrx);
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to update stock ", ex);
                        Message = "Update Stock: " + ex.Message;
                        sqlTrx.Rollback();

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }

                    try
                    {
                        cmd.CommandText = @"select value_loaded 
                                        from tasks t, task_type tt 
                                        where tt.task_type_id = t.task_type_id 
                                        and tt.task_type = @taskType
                                        and t.attribute1 = @oldTrxId";

                        cmd.Parameters.AddWithValue("@taskType", TaskProcs.SPECIALPRICING);
                        cmd.Parameters.AddWithValue("@oldTrxId", TrxId);

                        log.LogVariableState("@taskType", TaskProcs.SPECIALPRICING);
                        log.LogVariableState("@oldTrxId", TrxId);

                        object specialPricingId = cmd.ExecuteScalar();
                        if (specialPricingId != null) //special pricing exists on trx
                        {
                            TaskProcs taskProcs = new TaskProcs(Utilities);
                            taskProcs.createTask(-1, TaskProcs.SPECIALPRICING, Convert.ToInt32(specialPricingId), -1, -1, -1, -1, reversalTrxId, -1, "reversal", sqlTrx);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while calculating Special Pricing ", ex);
                        Message = "Special Pricing: " + ex.Message;
                        sqlTrx.Rollback();

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }

                    try
                    {
                        cmd.CommandText = @"select t.value_loaded , t.card_id
                                        from tasks t, task_type tt 
                                        where tt.task_type_id = t.task_type_id 
                                        and tt.task_type = @taskType
                                        and t.attribute1 = @oldTrxId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@taskType", TaskProcs.LOADTICKETS);
                        cmd.Parameters.AddWithValue("@oldTrxId", TrxId);

                        log.LogVariableState("@taskType", TaskProcs.LOADTICKETS);
                        log.LogVariableState("@oldTrxId", TrxId);

                        // object ticketsLoaded = cmd.ExecuteScalar();
                        SqlDataAdapter daLT = new SqlDataAdapter(cmd);
                        DataTable dtLoadTicketTask = new DataTable();
                        daLT.Fill(dtLoadTicketTask);
                        if (dtLoadTicketTask.Rows.Count > 0)
                        {
                            object ticketsLoadedForTask = dtLoadTicketTask.Rows[0]["value_loaded"];
                            object cardIdForTask = dtLoadTicketTask.Rows[0]["card_id"];
                            int ticketsLoadedForTaskValue = -1;
                            int cardIdForTaskValue = -1;
                            if (ticketsLoadedForTask != null)
                                ticketsLoadedForTaskValue = Convert.ToInt32(ticketsLoadedForTask);
                            if (cardIdForTask != null)
                                cardIdForTaskValue = Convert.ToInt32(cardIdForTask);
                            TaskProcs taskProcs = new TaskProcs(Utilities);
                            taskProcs.createTask(cardIdForTaskValue, TaskProcs.LOADTICKETS, ticketsLoadedForTaskValue * -1, -1, -1, -1, -1, reversalTrxId, -1, "Reverse LoadTickets: " + remarks, sqlTrx, 0, 0, 0, 0, reversalTrxId);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while reversing load tickets ", ex);
                        Message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2647) + " " + ex.Message;
                        sqlTrx.Rollback();

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }

                    try
                    {
                        cmd.CommandText = @"select t.value_loaded, isnull(char(attribute1), 'B') bonusType, t.card_id
                                        from tasks t, task_type tt 
                                        where tt.task_type_id = t.task_type_id 
                                        and tt.task_type = @taskType
                                        and t.trxId = @oldTrxId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@taskType", TaskProcs.LOADBONUS);
                        cmd.Parameters.AddWithValue("@oldTrxId", TrxId);

                        log.LogVariableState("@taskType", TaskProcs.LOADBONUS);
                        log.LogVariableState("@oldTrxId", TrxId);

                        SqlDataAdapter daLT = new SqlDataAdapter(cmd);
                        DataTable dtLoadTicketTask = new DataTable();
                        daLT.Fill(dtLoadTicketTask);
                        if (dtLoadTicketTask.Rows.Count > 0)
                        {
                            TaskProcs taskProcs = new TaskProcs(Utilities);
                            double bonusLoadedForTask = Convert.ToInt32(dtLoadTicketTask.Rows[0]["value_loaded"]);
                            int cardIdForTaskValue = Convert.ToInt32(dtLoadTicketTask.Rows[0]["card_id"]);
                            TaskProcs.EntitlementType bonusType = taskProcs.getEntitlementType(dtLoadTicketTask.Rows[0]["bonusType"].ToString());
                            string bonusTypeValue = taskProcs.GetBonusTypeCodeValue(bonusType);
                            taskProcs.createTask(cardIdForTaskValue, TaskProcs.LOADBONUS, bonusLoadedForTask * -1, -1, -1, -1, -1, bonusTypeValue[0], -1, "Reverse LoadBonus: " + remarks, sqlTrx, -1, -1, -1, -1, reversalTrxId);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while reversing load bonus ", ex);
                        Message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2641) + " " + ex.Message;
                        sqlTrx.Rollback();

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }

                    if (!ReverseCard(TrxId, TrxLineId, loginId, ReverseCardUpdates, ref Message, reversalTrxId, sqlTrx, cnn))
                    {
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }


                    try
                    {
                        reverseCheckIns(TrxId, TrxLineId, reversalTrxId, sqlTrx, ref Message);
                    }
                    catch (ValidationException ex)
                    {
                        Message = ex.Message;
                        sqlTrx.Rollback();
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }


                    try
                    {
                        reverseAttractionBookings(TrxId, TrxLineId, sqlTrx);
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    try
                    {
                        ReverseSubscriptions(TrxId, TrxLineId, Approver, sqlTrx);
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    try
                    {
                        ReverseReservationSchedule(TrxId, TrxLineId, sqlTrx);
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    Transaction trxRefund = new Transaction(Utilities);
                    if (!trxRefund.refundCreditCardPayments(TrxId, (int)reversalTrxId, sqlTrx, ref Message))
                    {
                        sqlTrx.Rollback();

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER").ToString() == "Y" && (status != "OPEN" && status != "INITIATED" && status != "ORDERED" && status != "PREPARED"))
                    {
                        openCashDrawer = true;
                        string _FISCAL_PRINTER = Utilities.getParafaitDefaults("FISCAL_PRINTER");
                        FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(_FISCAL_PRINTER);
                        if (fiscalPrinter != null)
                        {
                            fiscalPrinter.OpenPort();
                        }
                        else
                        {
                            sqlTrx.Rollback();
                            Message = Utilities.MessageUtils.getMessage("Fiscal printer initialization is failed.");
                            log.Error(Message);
                            return false;
                        }
                        StringBuilder errormessage = new StringBuilder();
                        if (!fiscalPrinter.CheckPrinterStatus(errormessage))
                        {
                            sqlTrx.Rollback();
                            Message = errormessage.ToString();
                            log.Error(errormessage);
                            return false;
                        }
                        log.Debug("Fiscal printer status is OK");
                        try
                        {

                            if (!fiscalPrinter.PrintReceipt((int)reversalTrxId, ref Message, sqlTrx))
                            {
                                if (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString()))
                                {
                                    log.Debug("Smartro reversal already completed. ");
                                }
                                else if (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                                {
                                    printNonFiscalReceipt = true;
                                }
                                else
                                {
                                    sqlTrx.Rollback();
                                    Message = "Fiscal Printer reversal failed";

                                    log.LogVariableState("Message ", Message);
                                    log.LogMethodExit(false);
                                    return false;
                                }
                            }
                            if (printNonFiscalReceipt == false && Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                            {
                                openCashDrawer = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            //Bowa  throws exception if any error
                            sqlTrx.Rollback();
                            Message = "Fiscal Printer reversal failed";
                            log.LogVariableState("Message ", Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                        log.Debug("Fiscal print is successful.");
                    }

                    //object book = Utilities.executeScalar(@"select 1 from Bookings where Status ='CANCELLED' and TrxId=@BooktrxId 
                    //                                    and (ReservationCode is null
                    //                                    or exists (select 'x' from trx_header where trxid = @BooktrxId 
                    //                                                  and Original_System_Reference is not null
                    //                                              )
                    //                                        )", new SqlParameter("@BooktrxId", TrxId));
                    //if (book == null)
                    //{
                    //Transaction trxRefund = new Transaction(Utilities);
                    //if (!trxRefund.refundCreditCardPayments(TrxId, (int)reversalTrxId, sqlTrx, ref Message))
                    //{
                    //    sqlTrx.Rollback();

                    //    log.LogVariableState("Message ", Message);
                    //    log.LogMethodExit(false);
                    //    return false;
                    //}
                    //}//Ends: Modification on 2016-Jul-08 for booking reverse feature.

                    try
                    {
                        cmd.CommandText = "select* from RentalAllocation where TrxId = @oldTrxId and (TrxLineId = @lineId or @lineId = -1) and Refunded = 1";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@oldTrxId", TrxId);
                        cmd.Parameters.AddWithValue("@lineId", TrxLineId);
                        log.LogVariableState("@oldTrxId", TrxId);
                        log.LogVariableState("@lineId", TrxLineId);
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            cmd.CommandText = "delete from RentalAllocation " +
                                                        "where trxid = @oldTrxId " +
                                                          "and (TrxlineId = @lineId or @lineId = -1); ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@oldTrxId", TrxId);
                            cmd.Parameters.AddWithValue("@lineId", TrxLineId);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            sqlTrx.Rollback();
                            Message = Utilities.MessageUtils.getMessage(2362);
                            log.LogMethodExit();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Query Execution Unsuccessful!", ex);
                    }
                    if (IsPostTransactionProcessIsolated("POSPlus").Equals("N"))//starts:Modification on 17-May-2016 for adding PosPlus 
                    {
                        if (!CallPostTransaction(sqlTrx, cnn, ref Message, true, 0.0, reversalTrxId, TrxLineId))
                        {
                            sqlTrx.Rollback();
                            if (cnn != null)
                                cnn.Close();

                            log.LogVariableState("Message ", Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }//Ends:Modification on 17-May-2016 for adding PosPlus 

                    sqlTrx.Commit();
                    Transaction logTrx = new Transaction(Utilities);
                    logTrx.InsertTrxLogs(TrxId, TrxLineId, loginId, "REVERSE", "Transaction Reversed. Approved by: " + Approver, null,
                        String.IsNullOrEmpty(Approver) ? "-1" : Approver, Utilities.getServerTime());
                    cmd.CommandText = "select top 1 1 from PostTransactionProcesses where Active = 1";
                    if (cmd.ExecuteScalar() != null)
                    {
                        System.Threading.ThreadStart thr = delegate
                        {
                            if (IsPostTransactionProcessIsolated("POSPlus").Equals("Y"))//starts:Modification on 17-May-2016 for adding PosPlus 
                            {
                                Utilities.executeNonQuery("exec SPPosttransactionprocessing @trxId", new SqlParameter("@trxId", reversalTrxId));
                            }//Ends:Modification on 17-May-2016 for adding PosPlus 
                        };
                        new System.Threading.Thread(thr).Start();
                    }

                    Message = Utilities.MessageUtils.getMessage(339, reversalTrxId.ToString());
                    //Begin: Added to call the event to print the receipt on Dec-15-2015//
                    if ((status != "BOOKING" && status != "RESERVED") || printNonFiscalReceipt)
                    {
                        try
                        {
                            object reversalAmt = Utilities.executeScalar("Select TrxNetAmount from trx_header where TrxId = @reversalTrxId",
                                                        new SqlParameter("@reversalTrxId", reversalTrxId));

                            log.LogVariableState("@reversalTrxId", reversalTrxId);

                            if (reversalAmt != DBNull.Value)
                                reversalAmount = Convert.ToDouble(reversalAmt);


                            transactionId = TrxId;
                            reversalTransactionId = reversalTrxId;


                            if (disableAutoPrintCheck) // Do nothing
                            {
                                log.LogVariableState("Message ", Message);
                                log.LogMethodExit(true);
                                return true;
                            }

                            if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER").ToString() == "N" || printNonFiscalReceipt)
                            {
                                MyPrintDocument = new System.Drawing.Printing.PrintDocument();
                                MyPrintDocument.PrintPage += new PrintPageEventHandler(MyPrintDocument_PrintPage);
                                DateTime printStartTime = Utilities.getServerTime();
                                POSMachines posMachine = new POSMachines(Utilities.ExecutionContext, Utilities.ParafaitEnv.POSMachineId);
                                List<POSPrinterDTO> posPrintersDTOList = posMachine.PopulatePrinterDetails();
                                PrintTransaction prt = new PrintTransaction(posPrintersDTOList);
                                string message = "";
                                Transaction trx = CreateTransactionFromDB(reversalTrxId, Utilities);
                                string printOption = Utilities.getParafaitDefaults("TRX_AUTO_PRINT_AFTER_SAVE");
                                if (printOption == "Y")
                                {
                                    //printDetails();
                                    prt.Print(trx, ref message, false, true);
                                }
                                else if (printOption == "A")
                                {
                                    if (Utilities.ParafaitEnv.IsClientServer && MessageBox.Show(Utilities.MessageUtils.getMessage(484), "Transaction Reversal Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        //printDetails();
                                        prt.Print(trx, ref message, false, true);
                                    }
                                }
                                trx.UpdateTrxHeaderSavePrintTime(reversalTrxId, null, null, printStartTime, Utilities.getServerTime());
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while printing transaction reversal", ex);
                        }
                    }
                    //End: Added to call the event to print the receipt on Dec-15-2015//

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    sqlTrx.Rollback();
                    Message = Utilities.MessageUtils.getMessage(340);

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
            }
        }
        public bool ReverseCard(int TrxId, int TrxLineId, string loginId, bool ReverseCardUpdates, ref string Message, int reversalTrxId, SqlTransaction sqlTrx, SqlConnection cnn)//Starts: Modification on 2016-Jul-08 for adding reopen feature.
        {
            log.LogMethodEntry(TrxId, TrxLineId, loginId, ReverseCardUpdates, Message, reversalTrxId, sqlTrx, cnn);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.Transaction = sqlTrx;
            cmd.Parameters.Clear();
            if (ReverseCardUpdates)
            {
                try
                {
                    cmd.Parameters.AddWithValue("@login", loginId);
                    cmd.Parameters.AddWithValue("@ReversalTrxId", reversalTrxId);

                    log.LogVariableState("@login", loginId);
                    log.LogVariableState("@ReversalTrxId", reversalTrxId);

                    if (reversalTrxId > -1)
                    {
                        //code added for checking whether the payment is done through credit plus balance from the card. -05/08/2015.
                        //If so, instead of updating the credits in card, the creditplus balance in CardCreditPlus table should be updated.
                        // If the payment is done through game card and both credits and creditplus point from the card are consumed, both credits and credit plus 
                        //values should get updated. The query just below this updates the cardcreditplus table for creditplus points.

                        cmd.CommandText = @"update cp  set CreditPlusBalance = isnull(CreditPlusBalance, 0) - tp.Amount, 
                                            LastupdatedDate = getdate(), LastUpdatedBy = @login 
                                            from CardCreditPlus cp 
                                             join (select sum(amount) amount, cardId, cardCreditPlusId 
	                                                 from trxPayments 
			                                        where trxId = @ReversalTrxId 
		                                            group by cardId, CardCreditPlusId) tp  
                                                                      on tp.cardId = cp.Card_id
                                                                      and tp.CardCreditPlusId=cp.CardCreditPlusId
	                                         join cards c on c.card_id=cp.Card_id
	                                                     and c.refund_flag='N'";
                        cmd.ExecuteNonQuery();

                        //cmd.CommandText = @"INSERT INTO dbo.CardCreditPlusLog (CardCreditPlusId, CreditPlus, CreditPlusType, CreditPlusBalance, PlayStartTime, CreatedBy, CreationDate, LastUpdatedBy, LastupdatedDate, site_id )
                        //                 (SELECT cp.CardCreditPlusId ,cp.CreditPlus ,cp.CreditPlusType ,cp.CreditPlusBalance ,cp.PlayStartTime  ,@login ,GETDATE() ,@login  ,GETDATE()   ,cp.site_id 
                        //                   from CardCreditPlus cp join trxPayments tp on tp.cardId = cp.Card_id and tp.CardCreditPlusId=cp.CardCreditPlusId
                        //                        join cards c on c.card_id=cp.Card_id and c.refund_flag='N'
                        //                  where tp.trxid = @ReversalTrxId)";
                        //cmd.ExecuteNonQuery();

                        //In the following query the condition 'and CardCreditPlusId is null' was added on 05/08/2015 .This query updates the credit point in the card.
                        cmd.CommandText = @"update cards set credits = isnull(credits, 0) - tp.Amount,
                                            credits_played = case when (credits_played + tp.amount) <= 0 then 0 else (credits_played + tp.Amount) end,
                                            last_update_time = getdate(), lastUpdatedBy = @login
                                            from (select cardId, sum(amount) amount 
                                                 from trxPayments
                                                where trxid = @ReversalTrxId and CardCreditPlusId is null
                                                group by cardId) tp
                                            where tp.cardId = cards.card_id
                                              and refund_flag = 'N';
                                            select CASE WHEN CardCreditPlusId IS NULL THEN Amount * -1 ELSE 0 END amount, 
                                                   CASE WHEN CardCreditPlusId IS NOT NULL THEN Amount * -1 ELSE 0 END CreditPlus_amount, 
                                                   card_number 
                                              from trxPayments tp, cards c 
                                            where TrxId = @ReversalTrxId 
                                            and c.card_id = tp.CardId
                                            and c.refund_flag = 'N'";
                        //new code ends-05/08/2015

                        SqlDataAdapter daPayment = new SqlDataAdapter(cmd);
                        DataTable dtPayment = new DataTable();
                        daPayment.Fill(dtPayment);

                        if (ParafaitEnv.MIFARE_CARD && dtPayment.Rows.Count > 0)
                        {
                            if (ReaderDevice == null)
                            {
                                Message = "MiFare Reader not initialized";
                                sqlTrx.Rollback();

                                log.LogVariableState("Message ", Message);
                                log.LogMethodExit(false);
                                return false;
                            }

                            foreach (DataRow dr in dtPayment.Rows)
                            {
                                MessageBox.Show("Place card " + dr["card_number"].ToString() + " on reader");
                                Card mifareCard = new MifareCard(ReaderDevice, dr["card_number"].ToString(), "", Utilities);
                                if (mifareCard.CardStatus == "NEW")
                                {
                                    Message = "Invalid MiFare card";
                                    sqlTrx.Rollback();

                                    log.LogVariableState("Message ", Message);
                                    log.LogMethodExit(false);
                                    return false;
                                }

                                //Modified below call for Mifare card to pass credit plus amount in case of refund
                                if (mifareCard.updateMifareCard(false, ref Message, dr["amount"], 0, 0, dr["CreditPlus_amount"]) == false)
                                {
                                    sqlTrx.Rollback();

                                    log.LogVariableState("Message ", Message);
                                    log.LogMethodExit(false);
                                    return false;
                                }
                            }
                        }
                    }

                    cmd.CommandText = "update cards set valid_flag = 'N', notes = 'Transaction Cancelled', " +
                                                        "ExpiryDate = null, last_update_time = getdate(), lastUpdatedBy = @login " +
                                       "where card_id in (select card_id " +
                                                        "from trx_lines l, products p, product_type pt " +
                                                        "where trxid = @oldTrxId " +
                                                        "and (lineId = @lineId or @lineId = -1) " +
                                                        "and p.product_id = l.product_id " +
                                                        "and p.product_type_id = pt.product_type_id " +
                                                        "and pt.product_type in ('NEW', 'CARDDEPOSIT'))";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@oldTrxId", TrxId);
                    cmd.Parameters.AddWithValue("@lineId", TrxLineId);
                    cmd.Parameters.AddWithValue("@login", loginId);

                    log.LogVariableState("@oldTrxId", TrxId);
                    log.LogVariableState("@lineId", TrxLineId);
                    log.LogVariableState("@login", loginId);

                    cmd.ExecuteNonQuery();
                    LockerAllocationList lockerAllocationList = new LockerAllocationList();
                    List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>> lockerAllocationSearchParams = new List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>>();
                    lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.TRX_ID, TrxId.ToString()));
                    if (TrxLineId != -1)
                    {
                        lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.TRX_LINE_ID, TrxLineId.ToString()));
                    }
                    List<LockerAllocationDTO> lockerAllocationDTOs = lockerAllocationList.GetAllLockerAllocations(lockerAllocationSearchParams, sqlTrx);
                    if (lockerAllocationDTOs != null && lockerAllocationDTOs.Count > 0)
                    {
                        ParafaitLockCardHandler locker;
                        string lockerMake = Utilities.getParafaitDefaults("LOCKER_LOCK_MAKE");
                        string zoneLevelLockerMake;
                        LockerZones lockerZones = new LockerZones(Utilities.ExecutionContext);

                        foreach (LockerAllocationDTO lockerAllocationDTO in lockerAllocationDTOs)
                        {
                            if (lockerAllocationDTO.Refunded)
                            {
                                sqlTrx.Rollback();
                                Message = Utilities.MessageUtils.getMessage(2365);
                                log.LogVariableState("Message", Message);
                                log.LogMethodExit(false);
                                return false;
                            }
                            else
                            {
                                if (lockerAllocationDTO.LockerId != -1)
                                {
                                    lockerZones.LoadLockerZonebyLockerId(lockerAllocationDTO.LockerId, sqlTrx);
                                    if (lockerZones.GetLockerZonesDTO != null)
                                    {
                                        zoneLevelLockerMake = (string.IsNullOrEmpty(lockerZones.GetLockerZonesDTO.LockerMake) ? lockerMake : lockerZones.GetLockerZonesDTO.LockerMake);
                                        if (!zoneLevelLockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()) &&
                                            zoneLevelLockerMake != ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString() &&
                                            zoneLevelLockerMake != ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString() &&
                                            zoneLevelLockerMake != ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString())//The logic to be removed, if the acs reader merged with hecere
                                        {
                                            if (ReaderDevice == null)
                                            {
                                                sqlTrx.Rollback();
                                                Message = Utilities.MessageUtils.getMessage(281);
                                                log.LogVariableState("Message", Message);
                                                log.LogMethodExit(false);
                                                return false;
                                            }
                                            if (!ReaderDevice.readCardNumber().Equals(lockerAllocationDTO.CardNumber))
                                            {
                                                sqlTrx.Rollback();
                                                Message = Utilities.MessageUtils.getMessage(2363, lockerAllocationDTO.CardNumber);
                                                log.LogVariableState("Message", Message);
                                                log.LogMethodExit(false);
                                                return false;
                                            }
                                        }
                                        Locker lockerBl = new Locker(lockerAllocationDTO.LockerId);
                                        if (zoneLevelLockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.COCY.ToString()))
                                            locker = new CocyLockCardHandler(ReaderDevice, Utilities.ExecutionContext, Convert.ToByte(Utilities.ParafaitEnv.MifareCustomerKey));
                                        else if (zoneLevelLockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString()))
                                            locker = new InnovateLockCardHandler(ReaderDevice, Utilities.ExecutionContext, Convert.ToByte(Utilities.ParafaitEnv.MifareCustomerKey), lockerAllocationDTO.CardNumber);
                                        else if (zoneLevelLockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))
                                            locker = new PassTechLockCardHandler(ReaderDevice, Utilities.ExecutionContext);
                                        else if (zoneLevelLockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()) || zoneLevelLockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()))
                                            locker = new MetraLockCardHandler(ReaderDevice, Utilities.ExecutionContext, lockerAllocationDTO.CardNumber, lockerBl.getLockerDTO.Identifier.ToString(), lockerZones.GetLockerZonesDTO.ZoneCode, zoneLevelLockerMake, lockerZones.GetLockerZonesDTO.LockerMode);
                                        else if (zoneLevelLockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()))
                                            locker = new HecereLockCardHandler(ReaderDevice, Utilities.ExecutionContext, lockerAllocationDTO.CardNumber);
                                        else
                                            locker = new ParafaitLockCardHandler(ReaderDevice, Utilities.ExecutionContext);
                                        locker.SetAllocation(lockerAllocationDTO);
                                        locker.ReturnLocker(sqlTrx);
                                    }
                                    else
                                    {
                                        sqlTrx.Rollback();
                                        Message = Utilities.MessageUtils.getMessage(2364);
                                        log.LogVariableState("Message", Message);
                                        log.LogMethodExit(false);
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    if (ParafaitEnv.MIFARE_CARD)
                    {
                        cmd.CommandText = @"select distinct c.card_number
                                                from trx_lines l, cards c
                                              where trxid = @oldTrxId 
                                              and (lineId = @lineId or @lineId = -1)
                                              and c.card_id = l.card_id
                                              and c.valid_flag = 'N'";
                        SqlDataAdapter daInvCard = new SqlDataAdapter(cmd);
                        DataTable dtInvCard = new DataTable();
                        daInvCard.Fill(dtInvCard);

                        foreach (DataRow dr in dtInvCard.Rows)
                        {
                            Card mifareCard = new MifareCard(ReaderDevice, dr["card_number"].ToString(), "", Utilities);
                            if (mifareCard.CardStatus == "NEW")
                            {
                                Message = "Invalid MiFare card";
                                sqlTrx.Rollback();

                                log.LogVariableState("Message ", Message);
                                log.LogMethodExit(false);
                                return false;
                            }
                            string message = "";
                            if (!mifareCard.refund_MCard(ref message))
                            {
                                Message = message;
                                sqlTrx.Rollback();

                                log.LogVariableState("Message = ", Message);
                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                    }

                    cmd.CommandText = @"Select l.*, (select isnull(sum(CreditPlus), 0) 
                                                        from CardCreditPlus 
                                                       where CreditPlusType = 'A' 
                                                        and TrxId = @oldTrxId and LineId = l.lineId) CreditPlus " +
                                        "from trx_lines l, products p, product_type pt, cards c " +
                                        "where trxid = @oldTrxId " +
                                        "and (lineId = @lineId or @lineId = -1) " +
                                        "and l.card_id = c.card_id " +
                                        //  "and c.valid_flag = 'Y' " +
                                        "and p.product_id = l.product_id " +
                                        "and p.product_type_id = pt.product_type_id";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtLoadedCards = new DataTable();
                    da.Fill(dtLoadedCards);
             
                    cmd.CommandText = "update cards set " +
                                    "credits = case when credits - isnull(@trxCredits, 0) < 0 then 0 else credits - isnull(@trxCredits, 0) end, " +
                                    "courtesy = case when courtesy - isnull(@trxCourtesy, 0) < 0 then 0 else courtesy - isnull(@trxCourtesy, 0) end, " +
                                    "bonus = case when bonus - isnull(@trxBonus, 0) < 0 then 0 else bonus - isnull(@trxBonus, 0) end, " +
                                    "time = case when time - isnull(@trxTime, 0) < 0 then 0 else time - isnull(@trxTime, 0) end, " +
                                    "loyalty_points = case when loyalty_points - isnull(@trxLoyaltyPoints, 0) < 0 then 0 else loyalty_points - isnull(@trxLoyaltyPoints, 0) end, " +
                                    "ticket_count = case when @trxTickets > 0 then case when ticket_count - isnull(@trxTickets, 0) < 0 then 0 else ticket_count - isnull(@trxTickets, 0) end else ticket_count  end, " +
                                    "notes = 'Transaction Cancelled', " +
                                    "last_update_time = getdate(), lastUpdatedBy = @login " +
                                 "where card_id = @card_id";
                                        
                    for (int i = 0; i < dtLoadedCards.Rows.Count; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@card_id", dtLoadedCards.Rows[i]["card_id"]);
                        cmd.Parameters.AddWithValue("@trxCredits", dtLoadedCards.Rows[i]["credits"]);
                        cmd.Parameters.AddWithValue("@trxCourtesy", dtLoadedCards.Rows[i]["courtesy"]);
                        cmd.Parameters.AddWithValue("@trxBonus", dtLoadedCards.Rows[i]["bonus"]);
                        cmd.Parameters.AddWithValue("@trxTime", dtLoadedCards.Rows[i]["time"]);
                        cmd.Parameters.AddWithValue("@trxLoyaltyPoints", dtLoadedCards.Rows[i]["loyalty_points"]);
                        cmd.Parameters.AddWithValue("@trxTickets", dtLoadedCards.Rows[i]["tickets"]);
                        cmd.Parameters.AddWithValue("@login", loginId);

                        log.LogVariableState("@card_id", dtLoadedCards.Rows[i]["card_id"]);
                        log.LogVariableState("@trxCredits", dtLoadedCards.Rows[i]["credits"]);
                        log.LogVariableState("@trxCourtesy", dtLoadedCards.Rows[i]["courtesy"]);
                        log.LogVariableState("@trxBonus", dtLoadedCards.Rows[i]["bonus"]);
                        log.LogVariableState("@trxTime", dtLoadedCards.Rows[i]["time"]);
                        log.LogVariableState("@trxLoyaltyPoints", dtLoadedCards.Rows[i]["loyalty_points"]);
                        log.LogVariableState("@trxTickets", dtLoadedCards.Rows[i]["tickets"]);
                        log.LogVariableState("@login", loginId);

                        try
                        {
                            cmd.ExecuteNonQuery();

                            if (ParafaitEnv.MIFARE_CARD)
                            {
                                if ((dtLoadedCards.Rows[i]["credits"] != DBNull.Value && Convert.ToDouble(dtLoadedCards.Rows[i]["credits"]) != 0)
                                    || (dtLoadedCards.Rows[i]["bonus"] != DBNull.Value && Convert.ToDouble(dtLoadedCards.Rows[i]["bonus"]) != 0)
                                    || (dtLoadedCards.Rows[i]["courtesy"] != DBNull.Value && Convert.ToDouble(dtLoadedCards.Rows[i]["courtesy"]) != 0)
                                    || (dtLoadedCards.Rows[i]["CreditPlus"] != DBNull.Value && Convert.ToDouble(dtLoadedCards.Rows[i]["CreditPlus"]) != 0))
                                {
                                    if (ReaderDevice == null)
                                    {
                                        Message = "MiFare Reader not initialized";
                                        sqlTrx.Rollback();

                                        log.LogMethodExit(false);
                                        return false;
                                    }

                                    if (ReaderDevice.readCardNumber() != dtLoadedCards.Rows[i]["card_number"].ToString())
                                        MessageBox.Show("Place card " + dtLoadedCards.Rows[i]["card_number"].ToString() + " on reader");
                                    Card mifareCard = new MifareCard(ReaderDevice, dtLoadedCards.Rows[i]["card_number"].ToString(), "", Utilities);
                                    if (mifareCard.CardStatus == "NEW")
                                    {
                                        Message = "Invalid MiFare card";
                                        sqlTrx.Rollback();

                                        log.LogMethodExit(false);
                                        return false;
                                    }

                                    if (mifareCard.updateMifareCard(false, ref Message,
                                        -(dtLoadedCards.Rows[i]["credits"] == DBNull.Value ? 0 : Convert.ToDouble(dtLoadedCards.Rows[i]["credits"])),
                                        -(dtLoadedCards.Rows[i]["bonus"] == DBNull.Value ? 0 : Convert.ToDouble(dtLoadedCards.Rows[i]["bonus"])),
                                        -(dtLoadedCards.Rows[i]["courtesy"] == DBNull.Value ? 0 : Convert.ToDouble(dtLoadedCards.Rows[i]["courtesy"])),
                                        -(dtLoadedCards.Rows[i]["CreditPlus"] == DBNull.Value ? 0 : Convert.ToDouble(dtLoadedCards.Rows[i]["CreditPlus"]))) == false)
                                    {
                                        sqlTrx.Rollback();

                                        log.LogMethodExit(false);
                                        return false;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while updating mifare card", ex);
                            Message = "Recharges: " + ex.Message;
                            sqlTrx.Rollback();

                            log.LogVariableState("Message ", Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    cmd.CommandText = "select CreditPlus, sourceCreditPlusId from CardCreditPlus where TrxId = @oldTrxId  and LineId = @lineId and CreditPlus < 0 and sourceCreditPlusId is not null";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@oldTrxId", TrxId);
                    cmd.Parameters.AddWithValue("@lineId", TrxLineId);
                    log.LogVariableState("@oldTrxId", TrxId);
                    log.LogVariableState("@lineId", TrxLineId);
                    SqlDataAdapter cpDa = new SqlDataAdapter(cmd);
                    DataTable cpDt = new DataTable();
                    cpDa.Fill(cpDt);
                    cmd.CommandText = "update cardcreditplus set CreditPlusBalance = isnull(CreditPlusBalance, 0) - @creditsToAdd, LastupdatedDate = getdate(), LastUpdatedBy = @loginId " +
                                      "where CardCreditPlusId = @CardCreditPlusId";
                    for (int i = 0; i < cpDt.Rows.Count; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@creditsToAdd", cpDt.Rows[i]["CreditPlus"]);
                        cmd.Parameters.AddWithValue("@loginId", loginId);
                        cmd.Parameters.AddWithValue("@CardCreditPlusId", cpDt.Rows[i]["sourceCreditPlusId"]);
                        
                        log.LogVariableState("@creditsToAdd", cpDt.Rows[i]["CreditPlus"]);
                        log.LogVariableState("@loginId", loginId);
                        log.LogVariableState("@CardCreditPlusId", cpDt.Rows[i]["sourceCreditPlusId"]);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while updating card", ex);
                            Message = "Card: " + ex.Message;
                            sqlTrx.Rollback();

                            log.LogVariableState("Message ", Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }               

                    cmd.CommandText = "delete from cardCreditPlusConsumption " +
                                    "where cardCreditPlusId in (select cardCreditPlusId " +
                                                            "from CardCreditPlus cp " +
                                                            "where trxid = @oldTrxId " +
                                                            "and (lineId = @lineId or @lineId = -1)); " +
                                    "delete from CardCreditPlusPurchaseCriteria " +
                                        "where cardCreditPlusId in (select cardCreditPlusId " +
                                                            "from CardCreditPlus cp " +
                                                            "where trxid = @oldTrxId " +
                                                            "and (lineId = @lineId or @lineId = -1)); " +
                                    "update cardCreditPlus set CreditPlus = 0, CreditPlusBalance = 0, ExtendOnReload = 'N' " +
                                    "where trxid = @oldTrxId " +
                                        "and (lineId = @lineId or @lineId = -1) ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@oldTrxId", TrxId);
                    cmd.Parameters.AddWithValue("@lineId", TrxLineId);

                    log.LogVariableState("@oldTrxId", TrxId);
                    log.LogVariableState("@lineId", TrxLineId);

                    cmd.ExecuteNonQuery();

                    //cmd.CommandText = @"INSERT INTO dbo.CardCreditPlusLog (CardCreditPlusId, CreditPlus, CreditPlusType, CreditPlusBalance, PlayStartTime, CreatedBy, CreationDate, LastUpdatedBy, LastupdatedDate, site_id, SynchStatus, MasterEntityId )
                    //                   (SELECT CardCreditPlusId ,CreditPlus ,CreditPlusType ,CreditPlusBalance ,PlayStartTime  ,@login ,GETDATE() ,@login  ,GETDATE()   ,site_id ,SynchStatus ,MasterEntityId
                    //                      from CardCreditPlus cp 
                    //                     where cp.trxid = @oldTrxId and (cp.lineId = @lineId or @lineId = -1))";
                    //cmd.Parameters.AddWithValue("@login", loginId);
                    //cmd.ExecuteNonQuery();

                    cmd.CommandText = "select l.card_id, game_profile_id, game_id, pg.quantity, l.TrxId, l.LineId, l.card_number, l.product_id " +
                                      "from products p, trx_lines l, productGames pg, cards c " +
                                      "where p.product_id = l.product_id " +
                                      "and pg.product_id = l.product_id " +
                                      "and trxId = @oldTrxId " +
                                      "and (lineId = @lineId or @lineId = -1) " +
                                      "and l.card_id = c.card_id " +
                                      "--and c.valid_flag = 'Y'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@oldTrxId", TrxId);
                    cmd.Parameters.AddWithValue("@lineId", TrxLineId);

                    log.LogVariableState("@oldTrxId", TrxId);
                    log.LogVariableState("@lineId", TrxLineId);

                    SqlDataAdapter gmDa = new SqlDataAdapter(cmd);
                    DataTable gmDt = new DataTable();
                    gmDa.Fill(gmDt);
                    for (int i = 0; i < gmDt.Rows.Count; i++)
                    {
                        cmd.CommandText = "update cardGames set quantity = case when quantity - isnull(@quantity, 0) <= 0 then 0 else quantity - isnull(@quantity, 0) end, " +
                                                                "BalanceGames = case when BalanceGames - isnull(@quantity, 0) <= 0 then 0 else BalanceGames - isnull(@quantity, 0) end " +
                                            "where card_id = @card_id " +
                                            "and isnull(game_profile_id, -1) = isnull(@game_profile_id, -1) " +
                                            "and isnull(game_id, -1) = isnull(@game_id, -1) " +
                                            "and Quantity > 0 " +
                                            "and TrxId = @TrxId " +
                                           @"and TrxLineId =  @LineId; 
                                                update cardgames 
                                                set Quantity = 0 
                                                where Quantity < 0 
                                                  and TrxId = @TrxId
                                                  and TrxLineId =  @LineId;
                                                update cardgames 
                                                set BalanceGames = 0, ExpiryDate = getdate() 
                                                where BalanceGames <= 0 
                                                  and TrxId = @TrxId
                                                  and TrxLineId =  @LineId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@card_id", gmDt.Rows[i]["card_id"]);
                        cmd.Parameters.AddWithValue("@game_profile_id", gmDt.Rows[i]["game_profile_id"]);
                        cmd.Parameters.AddWithValue("@game_id", gmDt.Rows[i]["game_id"]);
                        cmd.Parameters.AddWithValue("@quantity", gmDt.Rows[i]["quantity"]);
                        cmd.Parameters.AddWithValue("@TrxId", gmDt.Rows[i]["TrxId"]);
                        cmd.Parameters.AddWithValue("@LineId", gmDt.Rows[i]["LineId"]);

                        log.LogVariableState("@card_id", gmDt.Rows[i]["card_id"]);
                        log.LogVariableState("@game_profile_id", gmDt.Rows[i]["game_profile_id"]);
                        log.LogVariableState("@game_id", gmDt.Rows[i]["game_id"]);
                        log.LogVariableState("@quantity", gmDt.Rows[i]["quantity"]);
                        log.LogVariableState("@TrxId", gmDt.Rows[i]["TrxId"]);
                        log.LogVariableState("@LineId", gmDt.Rows[i]["LineId"]);

                        cmd.ExecuteNonQuery();
                    }

                    if (ParafaitEnv.MIFARE_CARD)
                    {
                        cmd.CommandText = "select l.card_id, l.card_number, l.product_id " +
                                      "from products p, trx_lines l, cards c " +
                                      "where p.product_id = l.product_id " +
                                      "and exists (select 1 from productGames pg where pg.product_id = l.product_id) " +
                                      "and trxId = @oldTrxId " +
                                      "and (lineId = @lineId or @lineId = -1) " +
                                      "and c.card_id = l.card_id " +
                                      "and c.valid_flag = 'Y'";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@oldTrxId", TrxId);
                        cmd.Parameters.AddWithValue("@lineId", TrxLineId);

                        log.LogVariableState("@oldTrxId", TrxId);
                        log.LogVariableState("@lineId", TrxLineId);

                        SqlDataAdapter cgmDa = new SqlDataAdapter(cmd);
                        DataTable cgmDt = new DataTable();
                        cgmDa.Fill(cgmDt);

                        foreach (DataRow dr in cgmDt.Rows)
                        {
                            if (ReaderDevice.readCardNumber() != dr["card_number"].ToString())
                                MessageBox.Show("Place card " + dr["card_number"].ToString() + " on reader");
                            Card mifareCardGM = new MifareCard(ReaderDevice, dr["card_number"].ToString(), "", Utilities);

                            mifareCardGM.AddEntitlements(Convert.ToInt32(dr["product_id"]));
                            mifareCardGM.ReverseEntitlements(mifareCardGM.Entitlements);
                            foreach (Card.Entitlement ent in mifareCardGM.Entitlements)
                                ent.EntCount = 0;
                            mifareCardGM.addCredits = 0;
                            mifareCardGM.addBonus = 0;
                            mifareCardGM.addCourtesy = 0;
                            mifareCardGM.addTime = 0;
                            mifareCardGM.addTicketCount = 0;
                            mifareCardGM.addCreditPlusCardBalance = 0;

                            mifareCardGM.rechargeCard(sqlTrx);
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error("Error occured when updating the credits or credits plus balance", ex);
                    Message = "Card Updates: " + ex.Message;
                    sqlTrx.Rollback();

                    log.LogMethodExit(false);
                    return false;
                }
            }

            log.LogMethodExit(true);
            return true;
        }//Ends: Modification on 2016-Jul-08 for adding reopen feature.

        public bool ReverseTransactionEntity(int TrxId, int TrxLineId, string loginId, int userId, string Approver, string remarks, ref string Message, ref int reversalTrxId, SqlTransaction sqlTrx, SqlConnection cnn)//Starts: Modification on 2016-Jul-08 for adding reopen feature.
        {
            log.LogMethodEntry(TrxId, TrxLineId, loginId, userId, Approver, remarks, Message, reversalTrxId, sqlTrx, cnn);
            object result;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.Transaction = sqlTrx;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@trxid", TrxId);
            cmd.Parameters.AddWithValue("@lineId", TrxLineId);
            cmd.CommandText = @"select distinct 1 
                                      from trx_lines tl, LockerAllocation la
                                      where tl.TrxId = @trxid and tl.card_id = la.CardId 
                                           and tl.TrxId = la.TrxId and tl.LineId = la.TrxLineId and la.Refunded = 1 
                                           and (tl.LineId = @lineId or tl.LineId = (@lineId + 1) or @lineId = -1)";
            result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                log.Error("Locker is already returned.");
                Message = Utilities.MessageUtils.getMessage(2365);
                sqlTrx.Rollback();
                log.LogVariableState("Message ", Message);
                log.LogMethodExit(false);
                return false;
            }
            cmd.CommandText = @"select  distinct 1 from trx_lines tl, products p,product_type pt 
	                                               where  tl.TrxId = @trxid 
	                                                     and tl.product_id = p.product_id  and p.product_type_id = pt.product_type_id 
                                                         and pt.product_type in('RENTAL','DEPOSIT') ";
            result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                cmd.CommandText = @"SELECT ISNULL((select top 1 ra.Refunded 
                                                     from trx_lines tl, RentalAllocation ra
                                                    where isnull(tl.card_id, -1) = isnull(ra.cardId, -1) 
                                                      and tl.TrxId = ra.TrxId
                                                      and ra.refunded = 1 
                                                      and tl.TrxId = @trxid and tl.LineId = ra.TrxLineId 
                                                      and (ra.TrxLineId = @lineId or ra.TrxLineId = (@lineId + 1) or @lineId = -1)),0)";
                result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value && result.Equals(true))
                {
                    log.Error("Rental return is already done.");
                    Message = Utilities.MessageUtils.getMessage(2362);
                    sqlTrx.Rollback();
                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            cmd.Parameters.Clear();
            cmd.CommandText = @"select oh.TransactionOrderTypeId from OrderHeader oh,trx_header th where th.OrderId=oh.OrderId and th.TrxId=@trxid";
            cmd.Parameters.AddWithValue("@trxid", TrxId);
            object transactionOrdertypeId = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            Transaction transaction = new Transaction(Utilities);
            //Dictionary<string, int> transactionOrdertype = transaction.LoadTransactionOrderType();
            if (transactionOrdertypeId != null && transactionOrdertypeId != DBNull.Value &&
                 ((transaction.TransactionOrderTypes.ContainsKey("Item Refund") && Convert.ToInt32(transactionOrdertypeId) == transaction.TransactionOrderTypes["Item Refund"]) ||
                  (transaction.TransactionOrderTypes.ContainsKey("Refund") && Convert.ToInt32(transactionOrdertypeId) == transaction.TransactionOrderTypes["Refund"])))
            {
                log.Error("Reversal is not allowed.");
                Message = Utilities.MessageUtils.getMessage("Reversal is not allowed.");
                sqlTrx.Rollback();
                log.LogVariableState("Message ", Message);
                log.LogMethodExit(false);
                return false;
            }
            OrderHeaderBL orderHeaderBL = new OrderHeaderBL(Utilities.ExecutionContext, new OrderHeaderDTO());
            orderHeaderBL.OrderHeaderDTO.POSMachineId = Utilities.ParafaitEnv.POSMachineId;
            if (transaction.TransactionOrderTypes.ContainsKey("Void"))
                orderHeaderBL.OrderHeaderDTO.TransactionOrderTypeId = transaction.TransactionOrderTypes["Void"];
            orderHeaderBL.OrderHeaderDTO.UserId = Utilities.ParafaitEnv.User_Id;
            orderHeaderBL.OrderHeaderDTO.Status = OrderStatus.COMPLETE;
            orderHeaderBL.Save(sqlTrx);
            if (TrxLineId == -1) // full trx reversal
            {
                cmd.CommandText = "insert into trx_header " +
                                   "([TrxDate] " +
                                   ",[trx_no] " +
                                   ",[TrxAmount] " +
                                   ",[TrxDiscountPercentage] " +
                                   ",[TaxAmount] " +
                                   ",[TrxNetAmount] " +
                                   ",[pos_machine] " +
                                   ",[POSMachineId] " +
                                   ",[user_id] " +
                                   ",[payment_mode] " +
                                   ",[CashAmount] " +
                                   ",[CreditCardAmount] " +
                                   ",[GameCardAmount] " +
                                   ",[OtherPaymentModeAmount] " +
                                   ",[POSTypeId] " +
                                   ",[PaymentReference] " +
                                   ",[PrimaryCardId] " +
                                   ",[customerId] " +
                                   ",[Status], site_id, LastUpdateTime, LastUpdatedBy, " +
                                       "OriginalTrxid, CreatedBy, OrderTypeGroupId, CreationDate, External_System_Reference,OrderId )" + //Added to populate original Trx ID  29-Jun-2016
                               "(select " +
                                   "getdate() " +
                                   ",[trx_no] " +
                                   ",[TrxAmount] * -1 " +
                                   ",[TrxDiscountPercentage] " +
                                   ",[TaxAmount] * -1 " +
                                   ",[TrxNetAmount] * -1 " +
                                   ",isnull(@pos_machine, [pos_machine]) " +
                                   ",isnull(@POSMachineId, [POSMachineId]) " +
                                   ",@user_id " +
                                   ",[payment_mode] " +
                                   ",[CashAmount] * -1 " +
                                   ",[CreditCardAmount] * -1 " +
                                   ",[GameCardAmount] * -1 " +
                                   ",[OtherPaymentModeAmount] * -1 " +
                                   ",[POSTypeId] " +
                                   ",@reference " +
                                   ",[PrimaryCardId] " +
                                   ",[customerId] " +
                                   ",[Status], site_id, getdate(), @loginId, " +
                                   "@OriginalTrxid , @user_id , OrderTypeGroupId, getdate() , External_System_Reference,@orderid " + //Added to populate original Trx ID  29-Jun-2016
                               "from trx_header " +
                               "where trxid = @trxid); " +
                               "select @@identity";
            }
            else // single line cancellation
            {
                cmd.CommandText = "insert into trx_header " +
                                   "([TrxDate] " +
                                   ",[trx_no] " +
                                   ",[TrxAmount] " +
                                   ",[TrxDiscountPercentage] " +
                                   ",[TaxAmount] " +
                                   ",[TrxNetAmount] " +
                                   ",[pos_machine] " +
                                   ",[POSMachineId] " +
                                   ",[user_id] " +
                                   ",[payment_mode] " +
                                   ",[CashAmount] " +
                                   ",[CreditCardAmount] " +
                                   ",[GameCardAmount] " +
                                   ",[OtherPaymentModeAmount] " +
                                   ",[POSTypeId] " +
                                   ",[PaymentReference] " +
                                   ",[PrimaryCardId] " +
                                   ",[customerId] " +
                                   ",[Status], site_id, LastUpdateTime, LastUpdatedBy, " +
                                       "OriginalTrxid, CreatedBy, OrderTypeGroupId, CreationDate, External_System_Reference,OrderId )" + //Added to populate original Trx ID  29-Jun-2016
                               "(select " +
                                   "getdate() " +
                                   ",trx_no " +
                                   ",sum([Amount] * -1) " +
                                   ",sum(Amount * isnull(d.discountPercentage, 0)) / case when sum([Amount]) = 0 then null else sum(amount) end * 100.0 " +
                                   ",sum(isnull(Tax_Percentage/100.0, 0) * Price * Quantity * -1) " +
                                   ",sum(Amount * (1 - isnull(d.discountPercentage, 0)) * -1) " +
                                   ",isnull(@pos_machine, [pos_machine]) " +
                                   ",isnull(@POSMachineId, [POSMachineId]) " +
                                   ",@user_id " +
                                   ",[payment_mode] " +
                                   ",sum(isnull((case when [CashAmount] = 0 then null else [CashAmount] end * Amount * (1 - isnull(d.discountPercentage, 0)) * -1) / TrxNetAmount, 0)) " +
                                   ",sum(isnull((case when [CreditCardAmount] = 0 then null else [CreditCardAmount] end * Amount * (1 - isnull(d.discountPercentage, 0)) * -1) / TrxNetAmount, 0)) " +
                                   ",sum(isnull((case when [GameCardAmount] = 0 then null else [GameCardAmount] end * Amount * (1 - isnull(d.discountPercentage, 0)) * -1) / TrxNetAmount, 0)) " +
                                   ",sum(isnull((case when [OtherPaymentModeAmount] = 0 then null else [OtherPaymentModeAmount] end * Amount * (1 - isnull(d.discountPercentage, 0)) * -1) / TrxNetAmount, 0)) " +
                                   ",POSTypeId " +
                                   ",@reference " +
                                   ",[PrimaryCardId] " +
                                   ",[customerId] " +
                                   ",Status, h.site_id, getdate(), @loginId, " +
                                       "@OriginalTrxid ,@user_id , OrderTypeGroupId, getdate(), h.External_System_Reference, @orderId " + //Added to populate original Trx ID  29-Jun-2016
                                   @"from trx_header h, trx_lines l left outer join
                                        (select lineid, isnull(sum(discountPercentage), 0)/100.0 discountPercentage
                                            from trxDiscounts
                                            where trxId = @trxid group by lineid) d on d.lineId = l.lineId " +
                               "where h.trxid = @trxid " +
                               "and h.trxid = l.trxid " +
                               "and (l.lineId = @lineId or card_id in (select card_id " + //selected line is NEW card issue, so cancel all other lines on the same card
                                                                    "from trx_lines l1, products p, product_type pt " +
                                                                    "where l1.trxid = @trxid " +
                                                                    "and l1.lineId = @lineId " +
                                                                    "and p.product_id = l1.product_id " +
                                                                    "and p.product_type_id = pt.product_type_id " +
                                                                    "and pt.product_type in ('NEW', 'CARDDEPOSIT'))) " +
                                "group by " +
                                   "[trx_no] " +
                                   //        ",d.discountPercentage " + Commented on 03-Feb-2016 as deposit was coming as separate row
                                   ",[pos_machine] " +
                                   ",[POSMachineId] " +
                                   ",[payment_mode] " +
                                   ",[POSTypeId] " +
                                   ",[Status] " +
                                   ",[PrimaryCardId] " +
                                   ",[customerId] " +
                                   ",h.site_id " +
                                ", OrderTypeGroupId, h.External_System_Reference); " +
                               "select @@identity";
                cmd.Parameters.AddWithValue("@lineId", TrxLineId);

                log.LogVariableState("@lineId", TrxLineId);
            }

            cmd.Parameters.AddWithValue("@trxid", TrxId);
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@loginId", loginId);

            log.LogVariableState("@trxid", TrxId);
            log.LogVariableState("@user_id", userId);
            log.LogVariableState("@loginId", loginId);

            if (string.IsNullOrEmpty(ParafaitEnv.POSMachine))
            {
                cmd.Parameters.AddWithValue("@pos_machine", DBNull.Value);
                log.LogVariableState("@pos_machine", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@pos_machine", ParafaitEnv.POSMachine);
                log.LogVariableState("@pos_machine", ParafaitEnv.POSMachine);
            }

            cmd.Parameters.AddWithValue("@OriginalTrxid", TrxId); //Added to populate original Trx ID  29-Jun-2016

            log.LogVariableState("@OriginalTrxid", TrxId);


            if (ParafaitEnv.POSMachineId <= 0)
            {
                cmd.Parameters.AddWithValue("@POSMachineId", DBNull.Value);
                log.LogVariableState("@POSMachineId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@POSMachineId", ParafaitEnv.POSMachineId);
                log.LogVariableState("@POSMachineId", ParafaitEnv.POSMachineId);
            }

            if (TrxLineId == -1)
            {
                cmd.Parameters.AddWithValue("@reference", "Reversal of Trx Id: " + TrxId.ToString() + ": " + remarks + " [Auth:" + Approver + "]");
                log.LogVariableState("@reference", "Reversal of Trx Id: " + TrxId.ToString() + ": " + remarks + " [Auth:" + Approver + "]");
            }
            else
            {
                cmd.Parameters.AddWithValue("@reference", "Reversal of Trx Id/Line Id: " + TrxId.ToString() + "/" + TrxLineId.ToString() + ": " + remarks + " [Auth:" + Approver + "]");
                log.LogVariableState("@reference", "Reversal of Trx Id/Line Id: " + TrxId.ToString() + "/" + TrxLineId.ToString() + ": " + remarks + " [Auth:" + Approver + "]");
            }
            cmd.Parameters.AddWithValue("@orderid", orderHeaderBL.OrderHeaderDTO.OrderId);
            log.LogVariableState("@orderid", orderHeaderBL.OrderHeaderDTO.OrderId);

            object o = null;
            try
            {
                o = cmd.ExecuteScalar();
            }

            catch (Exception ex)
            {
                log.Error("Error occured when inserting values into trx_header ", ex);
                Message = "Trx Header: " + ex.Message;
                sqlTrx.Rollback();

                log.LogVariableState("Message ", Message);
                log.LogMethodExit(false);
                return false;
            }
            //Added on 12-may-2016 for deleting the couponUsed Details
            try
            {
                DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(Utilities.ExecutionContext);
                List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchDiscountCouponsUsedParams = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                searchDiscountCouponsUsedParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.TRANSACTION_ID, TrxId.ToString()));
                searchDiscountCouponsUsedParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                if (TrxLineId != -1)
                {
                    searchDiscountCouponsUsedParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.LINE_ID, TrxLineId.ToString()));
                }
                List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchDiscountCouponsUsedParams, sqlTrx);
                if (discountCouponsUsedDTOList != null)
                {
                    foreach (var discountCouponsUsedDTO in discountCouponsUsedDTOList)
                    {
                        discountCouponsUsedDTO.IsActive = false;
                        DiscountCouponsUsedBL discountCouponsUsedBL = new DiscountCouponsUsedBL(Utilities.ExecutionContext, discountCouponsUsedDTO);
                        discountCouponsUsedBL.Save(sqlTrx);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while saving used coupons", ex);
                Message = "DiscountCouponsUsed: " + ex.Message;
                sqlTrx.Rollback();

                log.LogVariableState("Message ", Message);
                log.LogMethodExit(false);
                return false;
            }
            //end 

            try
            {
                DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(Utilities.ExecutionContext);
                List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchDiscountCouponsParams = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.TRANSACTION_ID, TrxId.ToString()));
                searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                if (TrxLineId != -1)
                {
                    searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.LINE_ID, TrxLineId.ToString()));
                }
                List<DiscountCouponsDTO> discountCouponsDTOList = discountCouponsListBL.GetDiscountCouponsDTOList(searchDiscountCouponsParams, sqlTrx);
                if (discountCouponsDTOList != null)
                {
                    foreach (var discountCouponsDTO in discountCouponsDTOList)
                    {
                        discountCouponsDTO.IsActive = false;
                        DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(Utilities.ExecutionContext, discountCouponsDTO);
                        discountCouponsBL.Save(sqlTrx);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while saving issued coupons", ex);
                Message = "Coupons Issued: " + ex.Message;
                sqlTrx.Rollback();

                log.LogVariableState("Message ", Message);
                log.LogMethodExit(false);
                return false;
            }
            try
            {
                List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> tagIssuedSearchParameters = new List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>>();
                tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.TRANSACTIONID, TrxId.ToString()));
                if (TrxLineId != -1)
                {
                    tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.LINEID, TrxLineId.ToString()));
                }
                NotificationTagIssuedListBL notificationTagIssuedListBL = new NotificationTagIssuedListBL(Utilities.ExecutionContext);
                List<NotificationTagIssuedDTO> NotificationTagIssuedListDTO = notificationTagIssuedListBL.GetAllNotificationTagIssuedDTOList(tagIssuedSearchParameters, sqlTrx);
                foreach (NotificationTagIssuedDTO notificationTagIssuedDTO in NotificationTagIssuedListDTO)
                {
                    notificationTagIssuedDTO.IsReturned = true;
                    notificationTagIssuedDTO.ReturnDate = Utilities.getServerTime();
                    log.LogVariableState("NotificationTagIssued:", notificationTagIssuedDTO);
                    NotificationTagIssuedBL notificationTagIssuedBL
                        = new NotificationTagIssuedBL(Utilities.ExecutionContext, notificationTagIssuedDTO);
                    notificationTagIssuedBL.Save(sqlTrx);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while saving Notification Tag Issued", ex);
                Message = "Notification Tag Issued: " + ex.Message;
                sqlTrx.Rollback();

                log.LogVariableState("Message ", Message);
                log.LogMethodExit(false);
                return false;
            }
            try
            {
                CardDiscountsListBL cardDiscountsListBL = new CardDiscountsListBL();
                List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>> searchCardDiscountsParams = new List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>>();
                searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.TRANSACTION_ID, TrxId.ToString()));
                searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                if (TrxLineId != -1)
                {
                    searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.LINE_ID, TrxLineId.ToString()));
                }
                List<CardDiscountsDTO> cardDiscountsDTOList = cardDiscountsListBL.GetCardDiscountsDTOList(searchCardDiscountsParams, sqlTrx);
                if (cardDiscountsDTOList != null)
                {
                    foreach (var cardDiscountsDTO in cardDiscountsDTOList)
                    {
                        cardDiscountsDTO.IsActive = "N";
                        CardDiscountsBL cardDiscountsBL = new CardDiscountsBL(cardDiscountsDTO);
                        cardDiscountsBL.Save(sqlTrx);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while saving card discounts", ex);
                Message = "Card Discounts: " + ex.Message;
                sqlTrx.Rollback();

                log.LogVariableState("Message ", Message);
                log.LogMethodExit(false);
                return false;
            }

            if (o != DBNull.Value)
            {
                reversalTrxId = Convert.ToInt32(o);

                cmd.CommandText = "update trx_header set status = 'CANCELLED', LastUpdateTime = getdate(), LastUpdatedBy = @loginId where trxId = @trxId and status in ('OPEN','INITIATED','ORDERED','PREPARED', 'CANCELLED','RESERVED','BOOKING')";//Added 'RESERVED' Status on 12 Sep-2016
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@trxId", TrxId);
                cmd.Parameters.AddWithValue("@loginId", loginId);
                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@trxId", reversalTrxId);
                cmd.Parameters.AddWithValue("@loginId", loginId);
                cmd.ExecuteNonQuery();

                List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
                cmd.Parameters.Clear();
                cmd.CommandText = "update trx_header set trx_no = @trx_no where trxId = @trxId";
                cmd.Parameters.AddWithValue("@trxId", reversalTrxId);
                string Trx_No = "";
                List<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOList = new List<InvoiceSequenceMappingDTO>();
                InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(Utilities.ExecutionContext);
                searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.EFFECTIVE_DATE_LESSER_THAN, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_TYPE, "CREDIT"));
                invoiceSequenceMappingDTOList = invoiceSequenceMappingListBL.GetAllInvoiceSequenceMappingList(searchParameters);
                if (invoiceSequenceMappingDTOList != null)
                {
                    var newinvoiceSequenceMappingDTOList = invoiceSequenceMappingDTOList.OrderByDescending(x => x.EffectiveDate).ToList();
                    InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(Utilities.ExecutionContext, newinvoiceSequenceMappingDTOList[0].InvoiceSequenceSetupId, sqlTrx);

                    try
                    {
                        Trx_No = invoiceSequenceSetupBL.GetSequenceNumber(sqlTrx);
                        if (Trx_No != null)  //If mapping exists
                        {
                            cmd.Parameters.AddWithValue("@trx_no", Trx_No);
                            log.LogVariableState("@trx_no", Trx_No);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (SeriesExpiredException ex)
                    {
                        log.Error("SeriesExpiredException - Execution of query for Trx_No Unsuccessful ", ex);
                        Message = Utilities.MessageUtils.getMessage(1333);
                        sqlTrx.Rollback();

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    catch (SeriesEndNumberExceededException ex)
                    {
                        log.Error("SeriesEndNumberExceededException - Execution of query for Trx_No Unsuccessful ", ex);
                        Message = Utilities.MessageUtils.getMessage(1334);
                        sqlTrx.Rollback();

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                else if (Utilities.getParafaitDefaults("USE_ORIGINAL_TRXNO_FOR_REFUND").Equals("N"))
                {
                    int OrderTypeGroupId = -1;
                    SqlCommand cmdOrderTypeGroup = Utilities.getCommand(sqlTrx);
                    cmdOrderTypeGroup.CommandText = @"select OrderTypeGroupId 
                                                    from trx_header 
                                                    where trxid = @trxid";
                    cmdOrderTypeGroup.Parameters.AddWithValue("@trxid", TrxId);

                    object OrderTypeGroupIdObj = cmdOrderTypeGroup.ExecuteScalar();

                    try
                    {
                        if (OrderTypeGroupIdObj != null)
                            OrderTypeGroupId = Convert.ToInt32(OrderTypeGroupIdObj);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Conversion of OrderTypeGroupIdObj to Integer is Unsuccessful! ", ex);
                        OrderTypeGroupId = -1;
                    }

                    Semnox.Core.GenericUtilities.CommonFuncs commonFuncs = new Semnox.Core.GenericUtilities.CommonFuncs(Utilities);
                    TransactionUtils transactionUtils = new TransactionUtils(Utilities);
                    Transaction curTrx = transactionUtils.CreateTransactionFromDB(TrxId, Utilities);
                    TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO = null;
                    if (curTrx != null && curTrx.TrxPOSPrinterOverrideRulesDTOList != null && curTrx.TrxPOSPrinterOverrideRulesDTOList.Any())
                    {
                        trxPOSPrinterOverrideRulesDTO = curTrx.TrxPOSPrinterOverrideRulesDTOList.Find(ruleDTO => ruleDTO.OptionItemCode == POSPrinterOverrideOptionItemCode.REVERSALSEQUENCE);
                    }
                    if (trxPOSPrinterOverrideRulesDTO != null)
                    {
                        TrxPOSPrinterOverrideRulesListBL trxPOSPrinterOverrideRulesListBL = new TrxPOSPrinterOverrideRulesListBL(Utilities.ExecutionContext);
                        string seqId = (trxPOSPrinterOverrideRulesListBL.GetSequenceId(trxPOSPrinterOverrideRulesDTO, sqlTrx)).ToString();
                        SequencesDTO sequencesDTO = new SequencesBL(Utilities.ExecutionContext, Convert.ToInt32(seqId)).GetSequencesDTO;
                        SequencesBL sequencesBL = new SequencesBL(Utilities.ExecutionContext, sequencesDTO);
                        if (Utilities.ExecutionContext != null)
                        {
                            string nextSequenceNumber = sequencesBL.GetNextSequenceNo(sqlTrx);
                            Trx_No = nextSequenceNumber;
                        }
                    }
                    else
                    {
                        Trx_No = commonFuncs.getNextReverseTrxNo(Utilities.ParafaitEnv.POSMachineId, OrderTypeGroupId, (TrxLineId == -1 ? true : false), sqlTrx);
                    }
                    if (string.IsNullOrEmpty(Trx_No))
                        Trx_No = commonFuncs.getNextTrxNo(Utilities.ParafaitEnv.POSMachineId, OrderTypeGroupId, sqlTrx);
                    cmd.Parameters.AddWithValue("@trx_no", Trx_No);

                    log.LogVariableState("@trx_no", Trx_No);

                    cmd.ExecuteNonQuery();
                }

                //Begin: Modification to Cancel Booking if transaction is reversed added on 12-Sep-2016
                if (TrxLineId == -1)
                {
                    object oBookingId = null;
                    cmd.CommandText = "Select bookingId from bookings where trxId = @TrxId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrxId", TrxId);
                    oBookingId = cmd.ExecuteScalar();
                    if (oBookingId != DBNull.Value && oBookingId != null)
                    {
                        cmd.CommandText = "UPDATE bookings SET status = 'CANCELLED' where BookingId = @BookingId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@BookingId", oBookingId);

                        log.LogVariableState("@BookingId", oBookingId);

                        cmd.ExecuteNonQuery();
                    }
                }
                //End: Modification to Cancel Booking if transaction is reversed added on 12-Sep-2016

                cmd.CommandText = "insert into trx_lines " +
                                   "([LineId] " +
                                   ",[TrxId] " +
                                   ",[product_id] " +
                                   ",[price] " +
                                   ",[quantity] " +
                                   ",[amount] " +
                                   ",[card_id] " +
                                   ",[card_number] " +
                                   ",[credits] " +
                                   ",[courtesy] " +
                                   ",[tax_percentage] " +
                                   ",[tax_id] " +
                                   ",[time] " +
                                   ",[bonus] " +
                                   ",[loyalty_points] " +
                                   ",[tickets] " +
                                   ",[site_id] " +
                                   ",[UserPrice] " +
                                   ", [IsWaiverSignRequired] " +
                                    ",[CancelledBy]" +
                                    ",[CancelledTime]" +
                                    ",[ProductDescription]" +
                                    ",[OriginalLineID]" +
                                    ",[trxProfileId]" +
                                    ", [AllocatedProductPrice]) " + //Added to populate original line ID 29-Jun-2016
                               "(select " +
                                   "[LineId] " +
                                   ",@ReversalTrxId " +
                                   ",[product_id] " +
                                   ",[price] " +
                                   ",[quantity] * -1 " +
                                   ",[amount] * -1" +
                                   ",[card_id] " +
                                   ",[card_number] " +
                                   ",[credits] * -1 " +
                                   ",[courtesy] * -1 " +
                                   ",[tax_percentage] " +
                                   ",[tax_id] " +
                                   ",[time] * -1 " +
                                   ",[bonus] * -1 " +
                                   ",[loyalty_points] * -1 " +
                                   ",[tickets] * -1 " +
                                   ",[site_id] " +
                                   ",[UserPrice] " +
                                   ", [IsWaiverSignRequired] " +
                                    ",[CancelledBy]" +
                                    ",[CancelledTime]" +
                                    ",[ProductDescription] " + //Added to populate original line ID  29-Jun-2016
                                        ", [LineId] " +
                                          ",[trxProfileId]" +
                                        ", [AllocatedProductPrice] * -1 " +
                               "from trx_lines " +
                               "where trxid = @oldTrxId " +
                               "and (lineId = @lineId or @lineId = -1 " +
                                    "or card_id in (select card_id " + //selected line is NEW card issue, so include all other lines on the same card
                                                     "from trx_lines l1, products p, product_type pt " +
                                                    "where l1.trxid = @oldTrxId " +
                                                      "and l1.lineId = @lineId " +
                                                      "and p.product_id = l1.product_id " +
                                                      "and p.product_type_id = pt.product_type_id " +
                                                      "and pt.product_type in ('NEW', 'CARDDEPOSIT'))))";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReversalTrxId", reversalTrxId);
                cmd.Parameters.AddWithValue("@oldTrxId", TrxId);
                cmd.Parameters.AddWithValue("@lineId", TrxLineId);

                log.LogVariableState("@ReversalTrxId", reversalTrxId);
                log.LogVariableState("@oldTrxId", TrxId);
                log.LogVariableState("@lineId", TrxLineId);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured when Inserting values into trx_lines ", ex);
                    Message = "Trx Line: " + ex.Message;
                    sqlTrx.Rollback();

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                //Consumption balance Reversal process
                DataTable dtConsumpBalance = Utilities.executeDataTable(@"select l.CreditPlusConsumptionId, ccp.card_id, count(1) reverseBalanceCount
                                                                            from trx_lines l, CardCreditPlusConsumption ccpc, CardCreditPlus ccp
                                                                            where l.trxid = @TrxId
                                                                            and (l.LineId = @lineId or @lineId = -1)
	                                                                        and l.CreditPlusConsumptionId = ccpc.PKId
	                                                                        and ccpc.CardCreditPlusId = ccp.CardCreditPlusId
                                                                            and cancelledBy IS NULL
                                                                            and CreditPlusConsumptionId IS NOT NULL
                                                                        group by l.CreditPlusConsumptionId, ccp.card_id",
                                                                        sqlTrx,
                                                                        new SqlParameter("@TrxId", TrxId),
                                                                        new SqlParameter("@lineId", TrxLineId));
                if (dtConsumpBalance.Rows.Count > 0)
                {
                    CreditPlus CPconsumpBal = new CreditPlus(Utilities);
                    for (int i = 0; i < dtConsumpBalance.Rows.Count; i++)
                    {
                        log.LogVariableState("Transaction Id: ", TrxId);
                        log.LogVariableState("Trx Line Id: ", TrxLineId);
                        log.LogVariableState("CreditPlus Consumption Id: ", dtConsumpBalance.Rows[i]["CreditPlusConsumptionId"]);
                        log.LogVariableState("CreditPlus Consumption card Id: ", dtConsumpBalance.Rows[i]["card_id"]);
                        CPconsumpBal.refundCreditPlusConsumption(Convert.ToInt32(dtConsumpBalance.Rows[i]["CreditPlusConsumptionId"]), Convert.ToInt32(dtConsumpBalance.Rows[i]["card_id"]), Convert.ToInt32(dtConsumpBalance.Rows[i]["reverseBalanceCount"]), sqlTrx);
                    }
                }
                //Consumption balance reversal end
                // Start Modification On 15-4-2016
                try
                {
                    cmd.Parameters.AddWithValue("@user_id", loginId);
                    log.LogVariableState("@user_id", loginId);
                    cmd.CommandText = @"insert into TrxParentModifierDetails (TrxId, LineId, ParentModifierId, ParentProductId, ParentProductName, ParentPrice, LastUpdatedBy,LastUpdatedTime) 
                                        select @ReversalTrxId, LineId, ParentModifierId, ParentProductId, ParentProductName, ParentPrice * -1, @user_id, getdate()
                                        from TrxParentModifierDetails 
                                        where TrxId = @oldTrxId
                                        and (LineId = @lineId or @lineId = -1)";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error("Unsuccessful execution of Transaact SQL statement!  ", ex);

                    Message = "Parent Modifier Line: " + ex.Message;
                    sqlTrx.Rollback();

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                //End Modification On 15-4-2016

                try
                {
                    cmd.CommandText = @"insert into TrxTaxLines (TrxId, LineId, TaxId, TaxStructureId, Percentage, amount, site_id,ProductSplitAmount) 
                                        select @ReversalTrxId, LineId, TaxId, TaxStructureId, Percentage, amount * -1, site_id, ProductSplitAmount*-1
                                        from TrxTaxLines 
                                        where TrxId = @oldTrxId
                                        and (lineId = @lineId or @lineId = -1)";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error("Unsuccessful execution of Transaact SQL statement " + cmd.CommandText, ex);
                    Message = "Trx Tax Line: " + ex.Message;
                    sqlTrx.Rollback();

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }

                try
                {
                    TransactionDiscountsListBL transactionDiscountsListBL = new TransactionDiscountsListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>(TransactionDiscountsDTO.SearchByParameters.TRANSACTION_ID, TrxId.ToString()));
                    if (TrxLineId != -1)
                    {
                        searchParams.Add(new KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>(TransactionDiscountsDTO.SearchByParameters.LINE_ID, TrxLineId.ToString()));
                    }
                    List<TransactionDiscountsDTO> transactionDiscountsDTOList = transactionDiscountsListBL.GetTransactionDiscountsDTOList(searchParams, sqlTrx);
                    HashSet<int> couponSetIdHashSet = new HashSet<int>();
                    if (transactionDiscountsDTOList != null)
                    {
                        foreach (var transactionDiscountsDTO in transactionDiscountsDTOList)
                        {
                            TransactionDiscountsDTO newTransactionDiscountsDTO = new TransactionDiscountsDTO();
                            newTransactionDiscountsDTO.LineId = transactionDiscountsDTO.LineId;
                            newTransactionDiscountsDTO.TransactionId = reversalTrxId;
                            newTransactionDiscountsDTO.DiscountId = transactionDiscountsDTO.DiscountId;
                            newTransactionDiscountsDTO.DiscountPercentage = transactionDiscountsDTO.DiscountPercentage;
                            newTransactionDiscountsDTO.DiscountAmount = -transactionDiscountsDTO.DiscountAmount;
                            if (transactionDiscountsDTO.DiscountCouponsUsedDTO != null)
                            {
                                couponSetIdHashSet.Add(transactionDiscountsDTO.DiscountCouponsUsedDTO.CouponSetId);
                            }
                            TransactionDiscountsBL transactionDiscountsBL = new TransactionDiscountsBL(Utilities.ExecutionContext, newTransactionDiscountsDTO);
                            transactionDiscountsBL.Save(sqlTrx);
                        }
                        foreach (int couponSetId in couponSetIdHashSet)
                        {
                            DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(Utilities.ExecutionContext, couponSetId, sqlTrx);
                            discountCouponsBL.Unuse(sqlTrx);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving transaction discounts", ex);
                    Message = "Trx Discounts: " + ex.Message;
                    sqlTrx.Rollback();

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                //object book = Utilities.executeScalar(@"select 1 from Bookings where Status in('SYSTEMABANDONED','CANCELLED') and TrxId=@BooktrxId", sqlTrx, new SqlParameter("@BooktrxId", TrxId));
                //if (book == null)
                //{
                //log.LogVariableState("book ", book);
                try
                {
                    Transaction paymentTransaction = new Transaction(Utilities);
                    paymentTransaction.CreateReversePayment(TrxId, reversalTrxId, sqlTrx);
                }
                catch (Exception ex)
                {
                    log.Error("Failed to Create Payment Information", ex);
                    Message = "Create Payment Info: " + ex.Message;
                    sqlTrx.Rollback();

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                //}
            }

            log.LogMethodExit(true);
            return true;
        }//Ends: Modification on 2016-Jul-08 for adding reopen feature.
        public bool CallPostTransaction(SqlTransaction TrxTransacation, SqlConnection trxConn, ref string message, bool IsReverseTransaction, double amount, int TrxId = -1, int TrxLineId = -1)//starts:Modification on 17-May-2016 for adding PosPlus 
        {
            log.LogMethodEntry(TrxTransacation, trxConn, message, IsReverseTransaction, amount, TrxId, TrxLineId);
            try
            {
                DataTable dTable;
                string controlCode = "";
                POSPlus pOSPlus = new POSPlus(Utilities);
                POSPlusRequest pOSPlusRequest = new POSPlusRequest();
                pOSPlusRequest.OrganizationNumber = Utilities.ParafaitEnv.SiteId;
                pOSPlusRequest.SerialNo = Utilities.getParafaitDefaults("FISCAL_DEVICE_SERIAL_NUMBER");
                pOSPlusRequest.TransactionDate = ServerDateTime.Now;
                pOSPlusRequest.Type = "normal(normal)";
                if (IsReverseTransaction)
                {
                    dTable = Utilities.executeDataTable(@"select TrxAmount from trx_header where TrxId=@TrxId",
                                                        TrxTransacation, new SqlParameter("@TrxId", TrxId));
                    if (dTable != null && dTable.Rows.Count > 0)
                    {
                        pOSPlusRequest.ReturnAmount = Convert.ToDouble((dTable.Rows[0]["TrxAmount"] == DBNull.Value) ? "0.0" : dTable.Rows[0]["TrxAmount"].ToString());
                    }
                    else
                    {
                        message = "Failed to fetch the transaction details.";

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                else
                {
                    pOSPlusRequest.ReturnAmount = amount;
                }

                pOSPlusRequest.CashRegisterId = Utilities.getParafaitDefaults("FISCAL_CASH_REGISTER_ID");
                dTable = Utilities.executeDataTable(@"Select Percentage,sum(Amount) as Amount from TrxTaxLines 
                                                    where TrxId = @TrxId and (LineId = @LineId or @LineId = -1) group by Trxid, Percentage",
                                                    TrxTransacation, new SqlParameter("@TrxId", TrxId),
                                                    new SqlParameter("@LineId", TrxLineId));

                log.LogVariableState("@LineId", TrxLineId);

                if (dTable != null && dTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dTable.Rows.Count - 1 && i <= 4; i++)
                    {
                        pOSPlusRequest.VATPercentage[i] = (dTable.Rows[i]["Percentage"] == DBNull.Value) ? 0.0 : Convert.ToDouble(dTable.Rows[i]["Percentage"].ToString());
                        pOSPlusRequest.VATAmount[i] = (dTable.Rows[i]["Amount"] == DBNull.Value) ? 0.0 : Convert.ToDouble(dTable.Rows[i]["Amount"].ToString());
                    }
                }
                controlCode = pOSPlus.PostTransaction(pOSPlusRequest, ref message);
                if (string.IsNullOrEmpty(controlCode))
                {
                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    message = message.Replace("OK", "");
                    SqlCommand Trxcmd;
                    if (trxConn != null)
                    {
                        Trxcmd = new SqlCommand();
                        Trxcmd.Connection = trxConn;
                        Trxcmd.Transaction = TrxTransacation;

                        log.LogVariableState("Trxcmd ", Trxcmd);
                    }
                    else
                    {
                        Trxcmd = Utilities.getCommand(TrxTransacation);
                    }
                    Trxcmd.CommandText = "update trx_header set External_System_Reference = @externalSystemReference where trxId = @trxId";
                    Trxcmd.Parameters.AddWithValue("@externalSystemReference", controlCode);
                    Trxcmd.Parameters.AddWithValue("@trxId", TrxId);
                    Trxcmd.ExecuteNonQuery();

                    log.LogVariableState("Trxcmd ", Trxcmd);
                    log.LogVariableState("message ", message);
                    log.LogMethodExit(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while completing post transaction process", ex);
                message = "POSPlus " + ex.Message;

                log.LogMethodExit(false);
                return false;
            }
        }//Ends:Modification on 17-May-2016 for adding PosPlus 
        public string IsPostTransactionProcessIsolated(string process)//starts:Modification on 17-May-2016 for adding PosPlus 
        {
            log.LogMethodEntry(process);

            DataTable dTable;
            dTable = Utilities.executeDataTable(@"select * from PostTransactionProcesses where Active=1");
            if (dTable != null && dTable.Rows.Count > 0)
            {
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    if (dTable.Rows[i]["Process"].ToString().ToLower().Equals(process.ToLower()))
                    {
                        string returnValueNew = (string.IsNullOrEmpty(dTable.Rows[i]["IsIsolated"].ToString())) ? "Y" : dTable.Rows[i]["IsIsolated"].ToString();
                        log.LogMethodExit(returnValueNew);
                        return returnValueNew;
                    }
                }
            }

            log.LogMethodExit("Y");
            return "Y";//If it returns Y then the pos plus operations will be takes place after commit
        }//Ends:Modification on 17-May-2016 for adding PosPlus 

        //Begin: Added to print the receipt when transaction is reversed on Dec-15-2015//
        public bool printDetails()
        {
            log.LogMethodEntry();

            if (SetupThePrinting())
            {
                try
                {
                    MyPrintDocument.Print();
                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error("Document's printing process Unsuccessful!", ex);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        //end//

        //Begin: Added to print initiate printing parameters on Dec-15-2015
        public bool SetupThePrinting()
        {
            log.LogMethodEntry();

            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            MyPrintDialog.UseEXDialog = true;

            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
            {
                log.LogMethodExit(false);
                return false;
            }

            MyPrintDocument.DocumentName = Utilities.MessageUtils.getMessage("Void Transaction Receipt");//Modified name to VOID on 28-Jan-2016
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins =
                             new Margins(10, 10, 20, 20);

            log.LogMethodExit(true);
            return true;
        }
        //End: Added to print initiate printing parameters on Dec-15-2015

        //Begin: Added to print the receipt when transaction is reversed on Dec-15-2015//
        public void MyPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            Utilities utils = new Utilities();
            int col1x = 0;
            int yLocation = 40;
            int yIncrement = 20;
            Font defaultFont = new System.Drawing.Font("courier narrow", 10f);
            e.Graphics.DrawString(ParafaitEnv.SiteName, new Font(defaultFont.FontFamily, 12F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 30;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Transaction Reversal Receipt"), new Font(defaultFont.FontFamily, 12F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 30;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("POS Name") + ": " + ParafaitEnv.POSMachine, new Font(defaultFont.FontFamily, 12F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 30;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("User Name") + ": " + ParafaitEnv.Username, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Original Transaction Id") + ": " + transactionId, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Reversal Transaction Id") + ": " + reversalTransactionId, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Transaction reversed on") + ": " + ServerDateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(Utilities.MessageUtils.getMessage("Reversal Amount") + ": " + (-1 * reversalAmount), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;

            log.LogMethodExit(null);
        }
        //End//

        void reverseCheckIns(int origTrxId, int TrxLineId, long reversalTrxId, SqlTransaction sqlTrx, ref string Message)
        {
            log.LogMethodEntry(origTrxId, TrxLineId, reversalTrxId, sqlTrx);

            CheckInListBL checkInListBL = new CheckInListBL(Utilities.ExecutionContext);
            List<KeyValuePair<CheckInDTO.SearchByParameters, string>> checkInListSearchParams = new List<KeyValuePair<CheckInDTO.SearchByParameters, string>>();
            checkInListSearchParams.Add(new KeyValuePair<CheckInDTO.SearchByParameters, string>(CheckInDTO.SearchByParameters.CHECK_IN_TRX_ID, origTrxId.ToString()));
            List<CheckInDTO> checkInDTOList = checkInListBL.GetCheckInDTOList(checkInListSearchParams, true, true, sqlTrx);
            if (checkInDTOList != null)
            {
                foreach (CheckInDTO checkInDTO in checkInDTOList)
                {
                    checkInDTO.CheckInDetailDTOList = checkInDTO.CheckInDetailDTOList.Where(x => x.CheckOutTime == null || x.CheckOutTime > Utilities.getServerTime()).ToList();
                    if (TrxLineId != -1)
                    {
                        checkInDTO.CheckInDetailDTOList = checkInDTO.CheckInDetailDTOList.Where(x => x.CheckInTrxLineId == TrxLineId).ToList();
                    }

                    //If check in is already checked out or any checkout record exists for check in details then do not allow to refund
                    // It is a Process change. As existing application allows to refund even if the customer used the checkin facility and completed it
                    if (checkInDTO.CheckInDetailDTOList.Exists(x => x.Status == CheckInStatus.CHECKEDOUT))
                    {
                        log.Error("Transaction has check in details with checkout status. Cannot refund the transaction");
                        Message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4081); //Transaction has check in details with status CheckedOut. Cannot refund the transaction
                        throw new ValidationException(Message);
                    }

                    foreach (CheckInDetailDTO checkInDetailDTO in checkInDTO.CheckInDetailDTOList)
                    {
                        checkInDetailDTO.CheckOutTrxId = Convert.ToInt32(reversalTrxId);
                        if (TrxLineId != -1)
                        {
                            checkInDetailDTO.TrxLineId = 1;
                        }
                        else
                        {
                            checkInDetailDTO.TrxLineId = checkInDetailDTO.CheckInTrxLineId;
                        }
                        List<CheckInDetailDTO> checkInDetailDTOList = new List<CheckInDetailDTO>();
                        checkInDetailDTOList.Add(checkInDetailDTO);
                        CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, checkInDTO);
                        try
                        {
                            checkInBL.PerformCheckOut(checkInDetailDTOList, sqlTrx);
                        }
                        catch (ValidationException validEx)
                        {
                            log.Error(validEx.GetAllValidationErrorMessages());
                        }
                    }
                }
            }

            log.LogMethodExit(null);
        }

        public void reverseAttractionBookings(int origTrxId, int TrxLineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(origTrxId, TrxLineId, sqlTrx);
            AttractionBookingList attractionBookingList = new AttractionBookingList(Utilities.ExecutionContext);
            List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> atbSearchParams = new List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>>();
            atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.TRX_ID, origTrxId.ToString()));
            atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.IS_EXPIRY_DATE_EXPIRED, "Y"));
            if (TrxLineId > -1)
                atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.LINE_ID, TrxLineId.ToString()));
            List<AttractionBookingDTO> attractionBookingDTOList = attractionBookingList.GetAttractionBookingDTOList(atbSearchParams, true);

            if (attractionBookingDTOList != null && attractionBookingDTOList.Count > 0)
            {
                foreach (AttractionBookingDTO attractionBookingDTO in attractionBookingDTOList)
                {
                    attractionBookingDTO.ExpiryDate = Utilities.getServerTime();
                    AttractionBooking attractionBooking = new AttractionBooking(Utilities.ExecutionContext, attractionBookingDTO);
                    attractionBooking.Save(-1, sqlTrx);

                    if (attractionBookingDTO.AttractionBookingSeatsDTOList != null)
                    {
                        foreach (AttractionBookingSeatsDTO seatsDTO in attractionBookingDTO.AttractionBookingSeatsDTOList)
                        {
                            AttractionBookingSeatsBL attractionBookingSeatsBL = new AttractionBookingSeatsBL(Utilities.ExecutionContext, seatsDTO);
                            attractionBookingSeatsBL.Save(sqlTrx);
                        }
                    }
                    // check and reverse the day attraction booking
                    attractionBooking.ReverseDayAttraction(origTrxId, TrxLineId, sqlTrx, false);
                }

            }
            log.LogMethodExit(null);
        }

        void updateStock(int origTrxId, int lineId, long reversalTrxId, string loginId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(origTrxId, lineId, reversalTrxId, loginId, sqlTrx);

            SqlCommand InvCmd = Utilities.getCommand(sqlTrx);
            InvCmd.CommandText = "Insert into InventoryTransaction " +
                                "([TrxDate] " +
                               ",[ParafaitTrxId] " +
                               ",[LineId] " +
                               ",[Username] " +
                               ",[POSMachine] " +
                               ",[POSMachineId] " +
                               ",[ProductId] " +
                               ",[LocationId] " +
                               ",[Quantity] " +
                               ",[SalePrice] " +
                               ",[TaxPercentage] " +
                               ",[InventoryTransactionTypeId] " +
                               ",[LotId] " +
                                 ",[TaxInclusivePrice] " +
                               ",[ProductCost] " +
                               ",[CreatedBy] " +
                               ",[CreationDate] " +
                                ",[LastUpdatedBy] " +
                               ",[LastUpdateDate] " +
                               ",[UOMId] ) " +
                               "(select " +
                               "getdate() " +
                               ",@reversalTrxId " +
                               ",[LineId] " +
                               ",[Username] " +
                               ",[POSMachine] " +
                               ",[POSMachineId] " +
                               ",[ProductId] " +
                               ",[LocationId] " +
                               ",[Quantity] * -1 " +
                               ",[SalePrice] " +
                               ",[TaxPercentage] " +
                               ",[InventoryTransactionTypeId] " +
                               ",[LotId] " +
                               ",[TaxInclusivePrice] " +
                                ",[ProductCost] " +
                               ",@LoginId " +
                               ",getdate() " +
                                ",@LoginId " +
                                ",getdate() " +
                                ",[UOMId] " +
                               "from InventoryTransaction " +
                               "where ParafaitTrxId = @origTrxId " +
                               "and (LineId = @lineId or @lineId = -1))";
            InvCmd.Parameters.Clear();
            InvCmd.Parameters.AddWithValue("@origTrxId", origTrxId);
            InvCmd.Parameters.AddWithValue("@reversalTrxId", reversalTrxId);
            InvCmd.Parameters.AddWithValue("@lineId", lineId);
            InvCmd.Parameters.AddWithValue("@LoginId", loginId);
            InvCmd.ExecuteNonQuery();

            log.LogVariableState("@origTrxId", origTrxId);
            log.LogVariableState("@reversalTrxId", reversalTrxId);
            log.LogVariableState("@lineId", lineId);

            SqlCommand cmd = Utilities.getCommand(sqlTrx);

            cmd.CommandText = "select ProductId, POSMachineId, Quantity, isnull(LotId, -1) LotId " +
                                "from InventoryTransaction IT " +
                                "where ParafaitTrxId = @origTrxId " +
                                "and (LineId = @lineId or @lineId = -1)";

            cmd.Parameters.AddWithValue("@origTrxId", origTrxId);
            cmd.Parameters.AddWithValue("@lineId", lineId);

            log.LogVariableState("@origTrxId", origTrxId);
            log.LogVariableState("@lineId", lineId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            InvCmd.CommandText = @"update Inventory 
		                            set quantity = quantity + @Quantity, 
                                       Lastupdated_userid = @LoginId, timestamp = getdate()
		                            where exists 
			                            (select 1 
			                                from Product P 
			                                    left outer join posMachines pos 
			                                    on POSMachineId = @POSMachine
                                            where P.ProductId = @ProductId
                                            and P.ProductId = Inventory.ProductId
                                            and isnull(pos.InventoryLocationId, P.outboundLocationId) = Inventory.LocationId)
                                      and (LotId = @LotId or @LotId = -1)";
            SqlCommand InvLotCmd = Utilities.getCommand(sqlTrx);
            InvLotCmd.CommandText = @"UPDATE InventoryLot SET BalanceQuantity = BalanceQuantity+@Quantity 
					,Lastupdatedby = @LoginId, lastupdateddate = GETDATE() WHERE LotId = @lotId";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!((dt.Rows[i]["LotId"]) == DBNull.Value))
                {
                    InvLotCmd.Parameters.Clear();
                    InvLotCmd.Parameters.AddWithValue("@quantity", dt.Rows[i]["Quantity"]);
                    InvLotCmd.Parameters.AddWithValue("@LoginId", loginId);
                    InvLotCmd.Parameters.AddWithValue("@LotId", dt.Rows[i]["LotId"]);
                    InvLotCmd.ExecuteNonQuery();
                }
                InvCmd.Parameters.Clear();
                InvCmd.Parameters.AddWithValue("@ProductId", dt.Rows[i]["ProductId"]);
                InvCmd.Parameters.AddWithValue("@quantity", dt.Rows[i]["Quantity"]);
                InvCmd.Parameters.AddWithValue("@POSMachine", dt.Rows[i]["POSMachineId"]);
                InvCmd.Parameters.AddWithValue("@LoginId", loginId);
                InvCmd.Parameters.AddWithValue("@LotId", dt.Rows[i]["LotId"]);
                InvCmd.ExecuteNonQuery();
            }

            log.LogMethodExit(null);
        }

        public void VIP_CardTypeUpgrade(int CardId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(CardId, SQLTrx);

            SqlCommand cardCmd = Utilities.getCommand(SQLTrx);

            cardCmd.CommandText = @"select --isnull(CardTypeId, -1) cardTypeId, 
                                        isnull(c.customer_id, -1) customer_id, 
                                        isnull(credits_played, 0) credits_played, 
                                        isnull(c.loyalty_points, 0) + isnull(c.CreditPlusLoyaltyPoints, 0) LoyaltyPoints, 
                                        vip_customer,
                                        CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,p.UniqueId))  unique_id,
                                        (select isnull(sum(l.amount), 0) from trx_lines l where l.card_id = c.card_id) TotalrechargeAmount
                                    from cardView c left outer join customers cu on cu.customer_id = c.customer_id left outer join Profile p on cu.profileId = p.id
                                    where c.card_id = @card_id";

            cardCmd.Parameters.AddWithValue("@card_id", CardId);
            cardCmd.Parameters.AddWithValue("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE"));
            log.LogVariableState("@card_id", CardId);

            SqlDataAdapter da = new SqlDataAdapter(cardCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                log.LogMethodExit(null);
                return;
            }

            //int cardTypeId = Convert.ToInt32(dt.Rows[0]["cardTypeId"]);
            //int localCardTypeId = cardTypeId;
            int customerId = Convert.ToInt32(dt.Rows[0]["customer_id"]);
            double TotalSpendAmount = Convert.ToDouble(dt.Rows[0]["credits_played"]);
            double TotalRechargeAmount = Convert.ToDouble(dt.Rows[0]["TotalrechargeAmount"]);
            double TotalLoyaltyPoints = Convert.ToDouble(dt.Rows[0]["LoyaltyPoints"]);
            string vip_customer = dt.Rows[0]["vip_customer"].ToString();
            string uniqueId = dt.Rows[0]["unique_id"].ToString().Trim();
            bool VIPChange = false;

            if (vip_customer == "N" && (ParafaitEnv.REGISTRATION_MANDATORY_FOR_VIP == "N" || (customerId != -1 && (ParafaitEnv.UNIQUE_ID_MANDATORY_FOR_VIP == "N" || string.IsNullOrEmpty(uniqueId) == false))))
            {
                if ((TotalSpendAmount >= ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS && ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                 || (TotalRechargeAmount >= ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS && ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS > 0))
                {
                    VIPChange = true;
                    vip_customer = "Y";
                }
            }

            if (VIPChange)
            {
                //cardCmd.CommandText = "update cards set vip_customer = @vip_customer, cardTypeId = @cardTypeId where card_id = @card_id";
                cardCmd.CommandText = "update cards set vip_customer = @vip_customer where card_id = @card_id";
                cardCmd.Parameters.AddWithValue("vip_customer", vip_customer);
                //if (localCardTypeId != -1)
                //{
                //    cardCmd.Parameters.AddWithValue("cardTypeId", localCardTypeId);
                //    upgradeCardMembership(CardId, localCardTypeId, ParafaitEnv.LoginID, SQLTrx);
                //}
                //else
                //    cardCmd.Parameters.AddWithValue("cardTypeId", DBNull.Value);
                cardCmd.ExecuteNonQuery();
            }

            log.LogMethodExit(null);
        }

        public void VIPUpgrade(Card card, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(card, sqlTrx);

            if (ParafaitEnv.REGISTRATION_MANDATORY_FOR_VIP == "N" || (card.customer_id != -1 && (ParafaitEnv.UNIQUE_ID_MANDATORY_FOR_VIP == "N" || (card.customerDTO != null && string.IsNullOrEmpty(card.customerDTO.UniqueIdentifier) == false))))
            {
                if ((card.credits_played >= ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS && ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                 || (card.TotalRechargeAmount >= ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS && ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS > 0))
                    card.vip_customer = 'Y';
            }

            log.LogMethodExit(card.vip_customer);
        }

        public DataTable getProductDetails(int ProductId, Card PrimaryCard)
        {
            log.LogMethodEntry(ProductId, PrimaryCard);

            using (SqlConnection cnn = Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //Modified the query to add a CardValidFor to get the validity on Dec-01-2015//
                    //Modified the query to add a Include user role pricelist for agent online booking Jun-30-2016//
                    //Customer Feed Back-Query Changed InvokeCustomerRegistration
                    cmd.Connection = cnn;
                    cmd.CommandText = @"select p.product_id,IsNull(p.OrderTypeId, pt.OrderTypeId) as OrderTypeId, p.product_name, p.CardExpiryDate, isnull(isnull(plp.price, p.price), 0) Price, p.tickets, InventoryProductCode, 
                                p.credits, p.courtesy, p.bonus, p.time, p.tax_id, p.ticket_allowed, p.vip_card, 
                                pt.description product_type_desc, pt.product_type, p.face_value, p.TaxInclusivePrice, 
                                availableUnits, AutoCheckOut,
                                p.checkinfacilityId as checkinfacilityId,
                                t.tax_name,
                                null CreditPlusConsumptionId, MembershipID, TrxRemarksMandatory, CategoryId, 
                                AutoGenerateCardNumber, QuantityPrompt, OnlyForVIP, isnull(t.tax_percentage, 0) tax_percentage, 
                                AllowPriceOverride, ManagerApprovalRequired, RegisteredCustomerOnly, MinimumUserPrice, VerifiedCustomerOnly,
                                isnull(MinimumQuantity, 0) MinimumQuantity, isnull(TrxHeaderRemarksMandatory, 0) TrxHeaderRemarksMandatory, pt.cardSale,
                                p.CategoryId, isnull(p.isGroupMeal, 'N') isGroupMeal,
                                CASE WHEN ISNULL(p.WaiverRequired,'N') = 'N' THEN
                                          CASE WHEN ISNULL(p.waiversetId,-1) > -1 THEN 
                                               'Y'
                                               ELSE 
                                                  ISNULL((SELECT top 1 'Y' 
                                                             FROM FacilityWaiver fw, ProductsAllowedInFacility paf, FacilityMapDetails fmd
					                                            where paf.ProductsId =  p.Product_id
					                                              and paf.IsActive = 1
					                                              and paf.FacilityMapId = fmd.FacilityMapId
					                                              and fmd.IsActive = 1
					                                              and fw.FacilityId = fmd.FacilityId
					                                              and fw.IsActive = 1
					                                              and ISNULL(fw.EffectiveFrom, getdate()) <= getdate()
					                                              and ISNULL(fw.EffectiveTo, getdate()) >= getdate()),'N')
                                               END
                                     ELSE p.WaiverRequired END as WaiverRequired, 
                                isnull(p.InvokeCustomerRegistration,0) InvokeCustomerRegistration, lz.ZoneCode, lz.LockerMode,
                                Isnull(p.ZoneId,-1) ZoneId,lz.LockerMake, p.LockerExpiryInHours, isnull(LockerExpiryDate,case when LockerExpiryInHours is null or LockerExpiryInHours = 0 then NULL else DATEADD(minute,LockerExpiryInHours*60, getdate())End) as LockerExpiryDate, p.WaiverSetId, 
                                (select null) UsedInDiscounts, 
                                isnull(p.MaxQtyPerDay, 0) MaxQtyPerDay,
                                isnull(p.HsnSacCode,'') HsnSacCode,
                                isnull(p.CardValidFor, 0) CardValidFor,
                                isnull(p.CardCount, 0) CardCount,
                                isnull(p.LoadToSingleCard, 0) LoadToSingleCard,
                                isnull(p.EnableVariableLockerHours, 0) EnableVariableLockerHours,
                                isnull(p.LinkChildCard, 0) LinkAsChildCard,
                                isnull(p.LicenseType, '') LicenseType,
                                ISNULL(ps.ProductSubscriptionId,-1) as ProductSubscriptionId,
                                ISNULL(ps.AutoRenew,0) AutoRenewSubscription,
                                ps.PaymentCollectionMode,
                                ISNULL(p.IssueNotificationDevice, 0) IssueNotificationDevice,
                                ISNULL(p.NotificationTagProfileId, -1) NotificationTagProfileId
                               from products p 
                               left outer join (select top 1 ProductId, Price
                                                    from PriceListProducts
                                                   where ProductId = @product_id
                                                   and PriceListId = (select top 1 *
                                                                       from (select PriceListId
                                                                               from Membership
                                                                              where MembershipId = @MembershipId
                                                                               --from CardType
                                                                              --where CardTypeId = @CardTypeId
                                                                              union all
                                                                            select PriceListId
                                                                                from UserRolePriceList
                                                                                 where Role_id = @RoleId
                                                                              union all
                                                                             select PriceListId
                                                                               from PriceList
                                                                              where PriceListName = 'Default'
                                                                            ) v
                                                                      )
                                                    and isnull(EffectiveDate, getdate()-10000) <= getdate()
                                                    order by EffectiveDate desc) plp
                                    on plp.ProductId = p.product_id
                                Left Outer join LockerZones lz on lz.ZoneId = p.ZoneId  
                                left outer join tax t on t.tax_id = p.tax_id
                                left outer join ProductSubscription ps on ps.ProductsId = p.product_id and ISNULL(ps.IsActive,0) = 1,
                                product_type pt where p.product_id = @product_id and p.product_type_id = pt.product_type_id";

                    cmd.Parameters.AddWithValue("@product_id", ProductId);
                    log.LogVariableState("@product_id", ProductId);

                    if (PrimaryCard != null)
                    {
                        cmd.Parameters.AddWithValue("@MembershipId", PrimaryCard.MembershipId);
                        log.LogVariableState("@MembershipId", PrimaryCard.MembershipId);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MembershipId", -1);
                        log.LogVariableState("@MembershipId", -1);
                    }

                    //Begin Modification on Jun-30-2016 :Agent online booking role based pricelist feature
                    if (Utilities.ParafaitEnv.RoleId != -1)
                    {
                        cmd.Parameters.AddWithValue("@RoleId", Utilities.ParafaitEnv.RoleId);
                        log.LogVariableState("@RoleId", Utilities.ParafaitEnv.RoleId);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@RoleId", -1);
                        log.LogVariableState("@RoleId", -1);
                    }
                    //End Modification Jun-30-2016 //

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable DT = new DataTable();
                    da.Fill(DT);

                    log.LogMethodExit(DT);
                    return DT;
                }
            }
        }


        //Modified on 29-Sep-2016 for implementing multicurrency
        //public void CreateCashTrxPayment(int TrxId, double Amount, double TenderedAmount, SqlTransaction SQLTrx, double TipAmount = 0, int SplitId = -1)
        //{
        //    log.LogMethodEntry(TrxId, Amount, TenderedAmount, SQLTrx, TipAmount, SplitId);

        //    CreateCashTrxPayment(TrxId, Amount, TenderedAmount, SQLTrx, string.Empty, 0, TipAmount, SplitId);

        //    log.LogMethodExit(null);
        //}

        //public void CreateCashTrxPayment(int TrxId, double Amount, double TenderedAmount, SqlTransaction SQLTrx, string CurrencyCode, double CurrencyRate, double TipAmount = 0, int SplitId = -1)
        //{
        //    log.LogMethodEntry(TrxId, Amount, TenderedAmount, SQLTrx, CurrencyCode, CurrencyRate, TipAmount, SplitId);

        //    SqlCommand creditPluscmd = Utilities.getCommand(SQLTrx);
        //    creditPluscmd.CommandText = @"insert into TrxPayments 
        //                                    (TrxId, PaymentModeId, Amount, PaymentDate, LastUpdatedUser, 
        //                                        site_id,
        //                                        TenderedAmount, TipAmount, splitId, PosMachine, CurrencyCode, CurrencyRate) 
        //                                  select top 1 @TrxId, PaymentModeId, @Amount, getdate(), @user, 
        //                                            (select site_id from trx_header where trxId = @TrxId), 
        //                                            @TenderedAmount, @Tipamt, @splitId, @PosMachine, @currencyCode, @currencyRate 
        //                                  from PaymentModes where isCash = 'Y'";

        //    creditPluscmd.Parameters.AddWithValue("@TrxId", TrxId);
        //    creditPluscmd.Parameters.AddWithValue("@Amount", Amount);
        //    creditPluscmd.Parameters.AddWithValue("@user", ParafaitEnv.Username);
        //    creditPluscmd.Parameters.AddWithValue("@TenderedAmount", TenderedAmount);
        //    creditPluscmd.Parameters.AddWithValue("@Tipamt", TipAmount);
        //    creditPluscmd.Parameters.AddWithValue("@splitId", SplitId == -1 ? DBNull.Value : (object)SplitId);
        //    creditPluscmd.Parameters.AddWithValue("@PosMachine", ParafaitEnv.POSMachine); //Added POS Machine at Payments 23-Mar-2016



        //    //added on 29-Sep-2016 for implementing multicurrency
        //    if (!string.IsNullOrEmpty(CurrencyCode))
        //    {
        //        creditPluscmd.Parameters.AddWithValue("@currencyCode", CurrencyCode);
        //        log.LogVariableState("@currencyCode", CurrencyCode);
        //    }
        //    else
        //    {
        //        creditPluscmd.Parameters.AddWithValue("@currencyCode", DBNull.Value);
        //        log.LogVariableState("@currencyCode", DBNull.Value);
        //    }

        //    if (CurrencyRate != 0)
        //    {
        //        creditPluscmd.Parameters.AddWithValue("@currencyRate", CurrencyRate);
        //        log.LogVariableState("@currencyRate", CurrencyRate);
        //    }
        //    else
        //    {
        //        creditPluscmd.Parameters.AddWithValue("@currencyRate", DBNull.Value);
        //        log.LogVariableState("@currencyRate", DBNull.Value);
        //    }
        //    //end

        //    creditPluscmd.ExecuteNonQuery();

        //    log.LogVariableState("@TrxId", TrxId);
        //    log.LogVariableState("@Amount", Amount);
        //    log.LogVariableState("@user", ParafaitEnv.Username);
        //    log.LogVariableState("@TenderedAmount", TenderedAmount);
        //    log.LogVariableState("@Tipamt", TipAmount);
        //    log.LogVariableState("@splitId", SplitId == -1 ? DBNull.Value : (object)SplitId);
        //    log.LogVariableState("@PosMachine", ParafaitEnv.POSMachine);
        //    log.LogMethodExit(null);
        //}//End Modification on 09-Nov-2015:Tip feature

        //public void CreateCreditCardTrxPayment(int TrxId,
        //                                        double Amount,
        //                                        int PaymentModeId,
        //                                        string CreditCardNumber,
        //                                        string NameOnCreditCard,
        //                                        string CreditCardName,
        //                                        string CreditCardExpiry,
        //                                        string CreditCardAuthorization,
        //                                        string Reference,
        //                                        object CCResponseId,
        //                                        string Memo,
        //                                        SqlTransaction SQLTrx,
        //                                        double TipAmount = 0,//Begin Modification on 09-Nov-2015:Tip feature
        //                                        int SplitId = -1,
        //                                        double? CouponValue = null,
        //                                        bool? isTaxable = null)//Begin Modification on 13-Mar-2016: Split feature
        //{
        //    log.LogMethodEntry(TrxId, Amount, PaymentModeId, CreditCardNumber, NameOnCreditCard, CreditCardName,
        //                                                                            CreditCardExpiry,
        //                                                                            CreditCardAuthorization,
        //                                                                            Reference,
        //                                                                            CCResponseId,
        //                                                                            Memo, SQLTrx, TipAmount, SplitId, CouponValue, isTaxable);

        //    SqlCommand creditCardcmd = Utilities.getCommand(SQLTrx);
        //    creditCardcmd.CommandText = @"insert into TrxPayments (TrxId, PaymentModeId, Amount, CreditCardNumber, NameOnCreditCard,
        //                                                           CreditCardName, CreditCardExpiry, CreditCardAuthorization, 
        //                                                           Reference, CCResponseId, Memo, PaymentDate, LastUpdatedUser, site_id,
        //                                                           TipAmount, PosMachine, SplitId, CouponValue, IsTaxable) 
        //                                        values (@TrxId, @PaymentModeId, @Amount, @CreditCardNumber, @NameOnCreditCard,
        //                                                @CreditCardName, @CreditCardExpiry, @CreditCardAuthorization, 
        //                                                @Reference, @CCResponseId, @Memo, getdate(), @user, 
        //                                                (select site_id from trx_header where trxId = @TrxId), @TipAmount,
        //                                                @PosMachine, @splitId, @CouponValue, @IsTaxable)";

        //    creditCardcmd.Parameters.AddWithValue("@TrxId", TrxId);
        //    creditCardcmd.Parameters.AddWithValue("@PaymentModeId", PaymentModeId);
        //    creditCardcmd.Parameters.AddWithValue("@Amount", Amount);
        //    creditCardcmd.Parameters.AddWithValue("@CCResponseId", CCResponseId);

        //    if (string.IsNullOrEmpty(Memo))
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@Memo", DBNull.Value);
        //        log.LogVariableState("@Memo", DBNull.Value);
        //    }
        //    else
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@Memo", Memo);
        //        log.LogVariableState("@Memo", Memo);
        //    }

        //    creditCardcmd.Parameters.AddWithValue("@user", ParafaitEnv.Username);
        //    creditCardcmd.Parameters.AddWithValue("@PosMachine", ParafaitEnv.POSMachine);
        //    creditCardcmd.Parameters.AddWithValue("@splitId", SplitId == -1 ? DBNull.Value : (object)SplitId);

        //    if (string.IsNullOrEmpty(CreditCardNumber))
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@CreditCardNumber", DBNull.Value);
        //        log.LogVariableState("@CreditCardNumber", DBNull.Value);

        //    }
        //    else
        //    {
        //        if (CreditCardNumber.Length > 4)
        //            CreditCardNumber = CreditCardNumber.Substring(CreditCardNumber.Length - 4);
        //        CreditCardNumber = new String('X', 12) + CreditCardNumber;
        //        creditCardcmd.Parameters.AddWithValue("@CreditCardNumber", CreditCardNumber);
        //        log.LogVariableState("@CreditCardNumber", CreditCardNumber);
        //    }

        //    if (string.IsNullOrEmpty(NameOnCreditCard))
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@NameOnCreditCard", DBNull.Value);
        //        log.LogVariableState("@NameOnCreditCard", DBNull.Value);
        //    }
        //    else
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@NameOnCreditCard", NameOnCreditCard);
        //        log.LogVariableState("@NameOnCreditCard", NameOnCreditCard);
        //    }

        //    if (string.IsNullOrEmpty(CreditCardName))
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@CreditCardName", DBNull.Value);
        //        log.LogVariableState("@CreditCardName", DBNull.Value);
        //    }
        //    else
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@CreditCardName", CreditCardName);
        //        log.LogVariableState("@CreditCardName", CreditCardName);
        //    }

        //    if (string.IsNullOrEmpty(CreditCardExpiry))
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@CreditCardExpiry", DBNull.Value);
        //        log.LogVariableState("@CreditCardExpiry", DBNull.Value);
        //    }
        //    else
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@CreditCardExpiry", CreditCardExpiry);
        //        log.LogVariableState("@CreditCardExpiry", CreditCardExpiry);
        //    }

        //    if (string.IsNullOrEmpty(CreditCardAuthorization))
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@CreditCardAuthorization", DBNull.Value);
        //        log.LogVariableState("@CreditCardAuthorization", DBNull.Value);
        //    }
        //    else
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@CreditCardAuthorization", CreditCardAuthorization);
        //        log.LogVariableState("@CreditCardAuthorization", CreditCardAuthorization);
        //    }

        //    if (string.IsNullOrEmpty(Reference))
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@Reference", DBNull.Value);
        //        log.LogVariableState("@Reference", DBNull.Value);
        //    }
        //    else
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@Reference", Reference);
        //        log.LogVariableState("@Reference", Reference);
        //    }

        //    creditCardcmd.Parameters.AddWithValue("@TipAmount", TipAmount);
        //    if (CouponValue != null && CouponValue.HasValue)
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@CouponValue", CouponValue.Value);
        //    }
        //    else
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@CouponValue", DBNull.Value);
        //    }
        //    if(isTaxable != null && isTaxable.HasValue)
        //    {
        //        if(isTaxable.Value)
        //        {
        //            creditCardcmd.Parameters.AddWithValue("@IsTaxable", "Y");
        //        }
        //        else
        //        {
        //            creditCardcmd.Parameters.AddWithValue("@IsTaxable", "N");
        //        }
        //    }
        //    else
        //    {
        //        creditCardcmd.Parameters.AddWithValue("@IsTaxable", DBNull.Value);
        //    }
        //    creditCardcmd.ExecuteNonQuery();

        //    Utilities.logSQLCommand("POSCCPayment", creditCardcmd);

        //    log.LogVariableState("@TrxId", TrxId);
        //    log.LogVariableState("@PaymentModeId", PaymentModeId);
        //    log.LogVariableState("@Amount", Amount);
        //    log.LogVariableState("@CCResponseId", CCResponseId);
        //    log.LogVariableState("@user", ParafaitEnv.Username);
        //    log.LogVariableState("@PosMachine", ParafaitEnv.POSMachine);
        //    log.LogVariableState("@splitId", SplitId == -1 ? DBNull.Value : (object)SplitId);
        //    log.LogVariableState("@TipAmount", TipAmount);

        //    log.LogMethodExit(null);
        //}

        //Added on 12-may-2016 for Updating the Coupon Details
        public void UpdateCouponUsedDetails(int couponId, string couponNumber, int trxId, SqlTransaction sqlTrx, double amount)
        {
            log.LogMethodEntry(couponId, couponNumber, trxId, sqlTrx, amount);

            SqlCommand couponUsed = Utilities.getCommand(sqlTrx);
            if (amount > 0)
            {
                couponUsed.CommandText = @"if not exists (select 1 from DiscountCouponsUsed 
                                                           where CouponSetId = @CouponSetId 
                                                             and isnull(isactive, 'Y') = 'Y'
                                                             and CouponNumber = @CouponNumber)
                                                             insert into DiscountCouponsUsed (CouponSetId, CouponNumber, TrxId, site_id, isActive)
                                                             values (@CouponSetId, @CouponNumber, @TrxId, @siteId, 'Y')";
            }
            else
            {
                couponUsed.CommandText = @"update DiscountCouponsUsed set isActive = 'N' where CouponSetId = @CouponSetId and CouponNumber = @CouponNumber";
            }

            couponUsed.Parameters.AddWithValue("@CouponSetId", couponId);
            couponUsed.Parameters.AddWithValue("@CouponNumber", couponNumber);
            couponUsed.Parameters.AddWithValue("@TrxId", trxId);
            couponUsed.Parameters.AddWithValue("@siteId", ParafaitEnv.SiteId);
            couponUsed.ExecuteNonQuery();

            log.LogVariableState("@CouponSetId", couponId);
            log.LogVariableState("@CouponNumber", couponNumber);
            log.LogVariableState("@TrxId", trxId);
            log.LogVariableState("@siteId", ParafaitEnv.SiteId);

            log.LogMethodExit(null);
        }
        //end

        //public void UpdateCashTipPayment(int PayementID, double TipAmount, SqlTransaction SQLTrx)//Begin Modification on 09-Nov-2015:Tip feature
        //{
        //    log.LogMethodEntry(PayementID, TipAmount, SQLTrx);

        //    SqlCommand creditPluscmd = Utilities.getCommand(SQLTrx);
        //    creditPluscmd.CommandText = @"update TrxPayments set TipAmount=@Tipamt where PaymentId=@PaymentId";
        //    creditPluscmd.Parameters.AddWithValue("@PaymentId", PayementID);
        //    creditPluscmd.Parameters.AddWithValue("@Tipamt", TipAmount);
        //    creditPluscmd.ExecuteNonQuery();

        //    log.LogVariableState("@PaymentId", PayementID);
        //    log.LogVariableState("@Tipamt", TipAmount);
        //    log.LogMethodExit(null);
        //}//End Modification on 09-Nov-2015:Tip feature

        //public void UpdateCreditTipPayment(int PayementID, double TipAmount, string authCode, string refNo, object CCResponseId, SqlTransaction SQLTrx)//Begin Modification on 09-Nov-2015:Tip feature
        //{
        //    log.LogMethodEntry(PayementID, TipAmount, authCode, refNo, CCResponseId, SQLTrx);

        //    SqlCommand creditPluscmd = Utilities.getCommand(SQLTrx);
        //    creditPluscmd.CommandText = @"update TrxPayments set TipAmount=@Tipamt, Reference=@refNo,CreditCardAuthorization=@AuthCode,CCResponseId=@responseId where PaymentId=@PaymentId";
        //    creditPluscmd.Parameters.AddWithValue("@PaymentId", PayementID);
        //    creditPluscmd.Parameters.AddWithValue("@refNo", refNo);
        //    creditPluscmd.Parameters.AddWithValue("@AuthCode", authCode);
        //    creditPluscmd.Parameters.AddWithValue("@responseId", CCResponseId);
        //    creditPluscmd.Parameters.AddWithValue("@Tipamt", TipAmount);
        //    creditPluscmd.ExecuteNonQuery();

        //    log.LogVariableState("@PaymentId", PayementID);
        //    log.LogVariableState("@refNo", refNo);
        //    log.LogVariableState("@AuthCode", authCode);
        //    log.LogVariableState("@responseId", CCResponseId);
        //    log.LogVariableState("@Tipamt", TipAmount);

        //    log.LogMethodExit(null);
        //}//End Modification on 09-Nov-2015:Tip feature

        //public void CreateCardTrxPayment(int TrxId, int CardId, double Amount, SqlTransaction SQLTrx, int SplitId = -1)
        //{
        //    log.LogMethodEntry(TrxId, CardId, Amount, SQLTrx, SplitId);

        //    SqlCommand creditPluscmd = Utilities.getCommand(SQLTrx);
        //    creditPluscmd.CommandText = @"insert into TrxPayments 
        //                                        (TrxId, PaymentModeId, PaymentDate, site_id,
        //                                         Amount, CardId, CardEntitlementType, LastUpdatedUser, SplitId, PosMachine) 
        //                                 select top 1 @TrxId, PaymentModeId, getdate(), (select site_id from trx_header where trxId = @TrxId),
        //                                        @Amount, @CardId, 'C', @user, @splitId, @PosMachine
        //                                   from PaymentModes where isDebitCard = 'Y'";
        //    creditPluscmd.Parameters.AddWithValue("@TrxId", TrxId);
        //    creditPluscmd.Parameters.AddWithValue("@Amount", Amount);
        //    creditPluscmd.Parameters.AddWithValue("@CardId", CardId);
        //    creditPluscmd.Parameters.AddWithValue("@user", ParafaitEnv.Username);
        //    creditPluscmd.Parameters.AddWithValue("@splitId", SplitId == -1 ? DBNull.Value : (object)SplitId);
        //    creditPluscmd.Parameters.AddWithValue("@PosMachine", ParafaitEnv.POSMachine); //23-Mar-2016
        //    creditPluscmd.ExecuteNonQuery();

        //    log.LogVariableState("@TrxId", TrxId);
        //    log.LogVariableState("@Amount", Amount);
        //    log.LogVariableState("@CardId", CardId);
        //    log.LogVariableState("@user", ParafaitEnv.Username);
        //    log.LogVariableState("@splitId", SplitId == -1 ? DBNull.Value : (object)SplitId);
        //    log.LogVariableState("@PosMachine", ParafaitEnv.POSMachine);

        //    log.LogMethodExit(null);
        //}
        //public void InsertTrxPayment(int TrxId, int OldTrxId, SqlTransaction SQLTrx) //Starts: Modification on 2016-Jul-08 for adding reopen feature.
        //{
        //    log.LogMethodEntry(TrxId, OldTrxId, SQLTrx);

        //    SqlCommand creditCardcmd = Utilities.getCommand(SQLTrx);

        //    creditCardcmd.CommandText = @"insert into TrxPayments (TrxId, PaymentModeId, Amount, CreditCardNumber, NameOnCreditCard,
        //                                                           CreditCardName, CreditCardExpiry, CreditCardAuthorization, 
        //                                                           Reference, CCResponseId, Memo, PaymentDate, LastUpdatedUser, site_id,
        //                                                           TipAmount, PosMachine, SplitId) 
        //                                           (select @TrxId, PaymentModeId, Amount, CreditCardNumber, NameOnCreditCard,
        //                                                           CreditCardName, CreditCardExpiry, CreditCardAuthorization, 
        //                                                           Reference, CCResponseId, Memo, PaymentDate, @User, site_id,
        //                                                           TipAmount, @PosMachine, SplitId from TrxPayments where TrxId=@OldTrxId)";
        //    creditCardcmd.Parameters.AddWithValue("@TrxId", TrxId);
        //    creditCardcmd.Parameters.AddWithValue("@OldTrxId", OldTrxId);
        //    creditCardcmd.Parameters.AddWithValue("@User", ParafaitEnv.Username);
        //    creditCardcmd.Parameters.AddWithValue("@PosMachine", ParafaitEnv.POSMachine);
        //    creditCardcmd.ExecuteNonQuery();
        //    Utilities.logSQLCommand("POSCCPayment", creditCardcmd);

        //    log.LogVariableState("@TrxId", TrxId);
        //    log.LogVariableState("@OldTrxId", OldTrxId);
        //    log.LogVariableState("@User", ParafaitEnv.Username);
        //    log.LogVariableState("@PosMachine", ParafaitEnv.POSMachine);

        //    log.LogMethodExit(null);
        //} //Ends: Modification on 2016-Jul-08 for adding reopen feature.

        //public void CreateCreditPlusTrxPayment(int TrxId, int CardCreditPlusId, double Amount, SqlTransaction SQLTrx, int SplitId = -1)
        //{
        //    log.LogMethodEntry(TrxId, CardCreditPlusId, Amount, SQLTrx, SplitId);

        //    SqlCommand creditPluscmd = Utilities.getCommand(SQLTrx);
        //    creditPluscmd.CommandText = @"insert into TrxPayments 
        //                                        (TrxId, PaymentModeId, PaymentDate, site_id,
        //                                         Amount, CardId, CardCreditPlusId, CardEntitlementType, LastUpdatedUser, 
        //                                         SplitId, PosMachine) 
        //                                 select @TrxId, (select top 1 PaymentModeId from PaymentModes where isDebitCard = 'Y'), getdate(), (select site_id from trx_header where trxId = @TrxId),
        //                                        @Amount, Card_Id, CardCreditPlusId, CreditPlusType, @user, 
        //                                        @splitId, @PosMachine
        //                                   from CardCreditPlus
        //                                  where CardCreditPlusId = @CardCreditPlusId";
        //    creditPluscmd.Parameters.AddWithValue("@TrxId", TrxId);
        //    creditPluscmd.Parameters.AddWithValue("@Amount", Amount);
        //    creditPluscmd.Parameters.AddWithValue("@CardCreditPlusId", CardCreditPlusId);
        //    creditPluscmd.Parameters.AddWithValue("@user", ParafaitEnv.Username);
        //    creditPluscmd.Parameters.AddWithValue("@splitId", SplitId == -1 ? DBNull.Value : (object)SplitId);
        //    creditPluscmd.Parameters.AddWithValue("@PosMachine", ParafaitEnv.POSMachine); //23-Mar-2016
        //    creditPluscmd.ExecuteNonQuery();

        //    log.LogVariableState("@TrxId", TrxId);
        //    log.LogVariableState("@Amount", Amount);
        //    log.LogVariableState("@CardCreditPlusId", CardCreditPlusId);
        //    log.LogVariableState("@user", ParafaitEnv.Username);
        //    log.LogVariableState("@splitId", SplitId == -1 ? DBNull.Value : (object)SplitId);
        //    log.LogVariableState("@PosMachine", ParafaitEnv.POSMachine);

        //    log.LogMethodExit(null);
        //}

        //public bool CardUpdatedByOthers(int CardId, DateTime CurrentUpdateTime)
        //{
        //    log.LogMethodEntry(CardId, CurrentUpdateTime);

        //    log.LogMethodExit();
        //    return CardUpdatedByOthers(CardId, CurrentUpdateTime, null);
        //}

        //public bool CardUpdatedByOthers(int CardId, DateTime CurrentUpdateTime, SqlTransaction SQLTrx)
        //{
        //    log.LogMethodEntry(CardId, CurrentUpdateTime, SQLTrx);

        //    object o = Utilities.executeScalar("select last_update_time from cards WHERE card_id = @cardid", SQLTrx,
        //                                        new SqlParameter("@cardid", CardId));

        //    log.LogVariableState("@cardid", CardId);

        //    if (o != null && o != DBNull.Value)
        //    {
        //        if (CurrentUpdateTime < Convert.ToDateTime(o))
        //        {
        //            log.LogMethodExit(true);
        //            return true;
        //        }
        //        else
        //        {
        //            log.LogMethodExit(false);
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        log.LogMethodExit(false);
        //        return false;
        //    }
        //}

        public Transaction CreateTransactionFromDB(int TrxId, Utilities pUtilities, bool OrderByProductname = false, bool IncludeCancelledLines = false, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(TrxId, pUtilities, OrderByProductname, IncludeCancelledLines);

            Transaction Trx = new Transaction(pUtilities);
            Trx.Trx_id = TrxId;
            DataTable productdataTable = new DataTable();
            //Modified the following query On 03-OCT-2016 for getting reprint count 
            string CommandText = @"select h.*,pmd.TrxId as ParentTrxId, pmd.LineId as ParentModifierLineId,pmd.ParentModifierId, 
                                    pmd.ParentProductId, pmd.ParentProductName, pmd.ParentPrice, p.CategoryId, l.*, l.remarks lineRemarks,l.ApprovedBy ApprovedBy,product_name, product_type, pt.description, t.tax_name,
                                    inventoryProductCode, AllowPriceOverRide, u.username, getdate() sysDate, p.TaxInclusivePrice, l.OrderTypeId, l.ModifierSetId,
                                                            bkg.AttractionScheduleId, bkg.ScheduleTime, bkg.ScheduleToTime, bkg.AttractionPlayId, bkg.BookingId, bkg.ScheduleFromTime,
                                    locks.LockerName, locks.Identifier, la.Id LockerAllocationId, locks.ZoneCode,locks.LockerMake, p.ZoneId, la.ValidToTime, 
                                    (select null) UsedInDiscounts,
                                    (Select top 1 pm.ModifierSetId
                                        from ProductModifiers pm
                                        where pm.ProductId = p.product_Id) TrxLineModifierSetId, ReprintCount,ISNULL(p.HsnSacCode, '') HsnSacCode, AllocatedProductPrice, isNull(h.TrxProfileId,l.TrxProfileId) as TrxLineProfileId,
                                    tuv.VerificationId, tuv.VerificationName, tuv.VerificationRemarks, isnull(p.isGroupMeal, 'N') isGroupMeal,
                                    l.Guid as trxLineGuid, checkIn.CheckInId, checkIn.CheckInTime, checkIn.checkInDetailId, 
                                    checkout.CheckOutId, checkOut.checkOutDetailId, checkOut.CheckOutTime,
                                    l.comboProductId, p.AutoGenerateCardNumber, cl.guid CardGuid, h.guid as TrxGuid, h.External_System_Reference,
                                    ISNULL(p.LinkChildCard, 0) LinkAsChildCard,
                                    ISNULL(p.LicenseType, '') LicenseType,
                                    ISNULL(NotificationTagIssued.NotificationTagIssuedId, -1) NotificationTagIssuedId
                                    from trx_header h, trx_lines l
                                    left outer join tax t on t.tax_id = l.tax_id
                                    left outer join cards cl on l.card_id = cl.card_id
                                    left outer join TrxUserVerificationDetails tuv 
                                    on l.trxid = tuv.Trxid and l.lineid = tuv.lineId and (tuv.IsActive = 'Y' or @includeCancelledLines = 1)
                                    left outer join TrxParentModifierDetails pmd 
                                    on l.TrxId = pmd.TrxId 
                                    and l.LineId = pmd.LineId 
                                        			left outer join (SELECT da.AttractionScheduleId, 
									                        bkg.TrxId, 
															bkg.LineId, 
															bkg.BookedUnits,
															da.AttractionPlayId,
                                                            bkg.bookingId,
									                        da.ScheduleDateTime ScheduleTime,
									                        ats.ScheduleTime ScheduleFromTime, 
															ats.ScheduleToTime
									                   FROM AttractionBookings Bkg, DayAttractionSchedule da,
													        AttractionSchedules ats
													  WHERE ats.AttractionScheduleId = da.AttractionScheduleId
                                                        AND bkg.DayAttractionScheduleId = da.DayAttractionScheduleId
                                                        AND bkg.trxid = @trxId
                                                        AND bkg.BookedUnits > 0
                                                        and bkg.ExpiryDate is null
													) bkg													  
									on l.TrxId = bkg.TrxId
									and l.lineId = bkg.LineId	
		                            LEFT OUTER JOIN (SELECT cd.CHECKINTIME, ci.CheckInId, cd.CheckInDetailId checkInDetailId, ci.CheckInTrxId, CheckinTrxLineId 
		                                               from CheckIns ci, CheckInDetails cd
                                                      where ci.CheckInId = cd.CheckInId 
						                                and ci.CheckInTrxId = @trxId
						                             ) checkIn	
                                    on 	checkIn.CheckInTrxId = l.trxId 
                                    and checkIn.CheckInTrxLineId = l.LineId
		                            LEFT OUTER JOIN (SELECT cd.CheckOutTime, ci.CheckInId checkOutId, cd.CheckInDetailId checkOutDetailId, cd.CheckInDetailId, cd.CheckOutTrxId, cd.TrxLineId 
		                                               from CheckIns ci, CheckInDetails cd
                                                      where ci.CheckInId = cd.CheckInId 
						                                and cd.CheckOutTrxId = @trxId 
						                             ) checkOut	
                                    on 	checkOut.CheckOutTrxId = l.trxId 
                                    and checkOut.TrxLineId = l.LineId	
		                            LEFT OUTER JOIN (SELECT nti.Trxid, nti.LineId, nti.NotificationTagIssuedId 
		                                               from NotificationTagIssued nti
                                                      where nti.trxId = @trxId 
						                             ) NotificationTagIssued	
                                    on 	NotificationTagIssued.Trxid = l.trxId 
                                    and NotificationTagIssued.LineId = l.LineId	
                                    left outer join LockerAllocation la
                                    on la.trxId = l.trxId
                                    and la.trxLineId = l.LineId
                                    left outer join (select locks.*, lzn.LockerMode, lzn.ZoneCode, lzn.ZoneId, lzn.ZoneName,lzn.LockerMake 
                                                                 from Lockers locks                                    
									                                  Left outer join LockerPanels lpnl on locks.PanelId = lpnl.PanelId
									                                  Left outer join LockerZones lzn on lzn.ZoneId = lpnl.ZoneId)locks 
                                    on locks.LockerId = la.LockerId,
                                    products p, product_type pt, users u
                                where h.trxid = l.trxid 
                                and h.trxid = @trxId
                                and (l.CancelledTime is null or @includeCancelledLines = 1)
                                and h.user_id = u.user_id
                                and l.product_id = p.product_id
                                and p.product_type_id = pt.product_type_id
                                order by " + (OrderByProductname ? "l.ParentLineId, product_name" : "lineId");
            DataTable dt = Utilities.executeDataTable(CommandText, sqlTrx, new SqlParameter("@trxId", TrxId), new SqlParameter("includeCancelledLines", IncludeCancelledLines));

            log.LogVariableState("@trxId", TrxId);
            log.LogVariableState("includeCancelledLines", IncludeCancelledLines);

            //Begin Modification Dec-21-2015 - Define the customer object here//
            CustomerDTO customerDTO = null;//new CustomerDTO();
            //End Modification Dec-21-2015 //

            bool CANCEL_PRINTED_TRX_LINE = Utilities.getParafaitDefaults("CANCEL_PRINTED_TRX_LINE").Equals("Y");
            Dictionary<int, Transaction.TransactionLine> transactionLineDictionary = new Dictionary<int, Transaction.TransactionLine>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (i == 0)
                {
                    //Begin Modification Dec-21-2015 Assign the cutomer Object to Transaction,needed to get the customer id when the order is suspended 
                    if (dt.Rows[i]["CustomerId"] != DBNull.Value)
                    {
                        customerDTO = (new CustomerBL(Utilities.ExecutionContext, Convert.ToInt32(dt.Rows[i]["CustomerId"]), true, true, sqlTrx)).CustomerDTO;
                    }
                    Trx.customerDTO = customerDTO;
                    //End Modification Dec-21-2015//
                    Trx.Trx_No = dt.Rows[i]["Trx_No"].ToString();
                    Trx.TrxGuid = dt.Rows[i]["TrxGuid"].ToString();
                    Trx.transactionOTP = dt.Rows[i]["TransactionOTP"].ToString();
                    Trx.TransactionDate = Convert.ToDateTime(dt.Rows[i]["TrxDate"]);
                    Trx.TrxDate = Trx.TransactionDate;
                    Trx.EntitlementReferenceDate = Trx.TransactionDate;
                    Trx.Transaction_Amount = Convert.ToDouble(dt.Rows[i]["TrxAmount"]);
                    Trx.Tax_Amount = (dt.Rows[i]["TaxAmount"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["TaxAmount"]));
                    Trx.Net_Transaction_Amount = Math.Round(Convert.ToDouble(dt.Rows[i]["TrxNetAmount"]), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                    Trx.POSMachine = dt.Rows[i]["pos_machine"].ToString();
                    if (dt.Rows[i]["payment_mode"] != DBNull.Value)
                        Trx.PaymentMode = Convert.ToInt32(dt.Rows[i]["payment_mode"]);
                    Trx.CashAmount = Convert.ToDouble(dt.Rows[i]["CashAmount"]);
                    Trx.CreditCardAmount = Convert.ToDouble(dt.Rows[i]["CreditCardAmount"]);
                    Trx.GameCardAmount = Convert.ToDouble(dt.Rows[i]["GameCardAmount"]);
                    Trx.OtherModeAmount = Convert.ToDouble(dt.Rows[i]["OtherPaymentModeAmount"]);
                    Trx.PaymentReference = dt.Rows[i]["PaymentReference"].ToString();
                    Trx.PrimaryCard = (dt.Rows[i]["PrimaryCardId"] == DBNull.Value ? null : new Card(Convert.ToInt32(dt.Rows[i]["PrimaryCardId"]), pUtilities.ParafaitEnv.LoginID, Utilities, sqlTrx));
                    Trx.Status = (Transaction.TrxStatus)Enum.Parse(typeof(Transaction.TrxStatus), dt.Rows[i]["Status"].ToString(), true);
                    Trx.TrxProfileId = (dt.Rows[i]["TrxProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["TrxProfileId"]));
                    Trx.Username = dt.Rows[i]["Username"].ToString();
                    Trx.Remarks = dt.Rows[i]["Remarks"].ToString();
                    Trx.DBReadTime = Convert.ToDateTime(dt.Rows[i]["sysDate"]);
                    Trx.TokenNumber = dt.Rows[i]["TokenNumber"].ToString();
                    Trx.site_id = dt.Rows[i]["site_id"];
                    Trx.ReprintCount = (dt.Rows[i]["ReprintCount"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dt.Rows[i]["ReprintCount"])); //Modification On 03-OCT-2016 for getting reprint count
                    Trx.OriginalTrxId = (dt.Rows[i]["OriginalTrxID"].Equals(DBNull.Value) ? -1 : Convert.ToInt32(dt.Rows[i]["OriginalTrxID"]));
                    Trx.OrderTypeGroupId = (dt.Rows[i]["OrderTypeGroupId"].Equals(DBNull.Value) ? -1 : Convert.ToInt32(dt.Rows[i]["OrderTypeGroupId"]));

                    Trx.externalSystemReference = dt.Rows[i]["External_System_Reference"].ToString();

                    Trx.customerIdentifier = dt.Rows[i]["CustomerIdentifier"].ToString();
                    if (!String.IsNullOrEmpty(Trx.customerIdentifier))
                    {
                        string decryptedCustomerReference = Encryption.Decrypt(Trx.customerIdentifier);
                        // Do not remove empty entries as format is email|phone, so empty is valid entry
                        string[] customerIdentifierStringArray = decryptedCustomerReference.Split(new[] { '|' });
                        log.LogVariableState("customerIdentifierStringArray", customerIdentifierStringArray);
                        Trx.customerIdentifiersList = new List<string>();
                        foreach (string identifier in customerIdentifierStringArray)
                        {
                            Trx.customerIdentifiersList.Add(identifier);
                        }
                        log.LogVariableState("customerIdentifiersList", Trx.customerIdentifiersList);
                    }

                    //Begin:Added to update originalSystemReference with trxId on Nov-19-2015//
                    Trx.originalSystemReference = dt.Rows[i]["Original_System_Reference"].Equals(DBNull.Value) ? string.Empty : dt.Rows[i]["Original_System_Reference"].ToString();
                    //end:Added to update originalSystemReference with trxId on Nov-19-2015//
                    Trx.POSTypeId = (dt.Rows[i]["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["POSTypeId"]));
                    Trx.TransactionInfo.PrimaryCard = Trx.PrimaryCard;
                    Trx.IsGroupMeal = (dt.Rows[i]["isGroupMeal"].ToString() == "Y"); //Mark transaction as Group Meal transaction.

                    if (dt.Rows[i]["OrderId"] != DBNull.Value)
                    {
                        Trx.Order = new OrderHeaderBL(Utilities.ExecutionContext, (int)dt.Rows[i]["OrderId"], Trx, false, sqlTrx);
                        if (Trx.Order.OrderHeaderDTO != null)
                        {
                            Trx.TransactionInfo.OrderId = Trx.Order.OrderHeaderDTO.OrderId;
                            Trx.TransactionInfo.TableNumber = Trx.Order.OrderHeaderDTO.TableNumber;
                            Trx.TransactionInfo.WaiterName = Trx.Order.OrderHeaderDTO.WaiterName;
                        }
                    }

                    if (Trx.TrxProfileId != -1)
                        Trx.TransactionInfo.TrxProfile = Utilities.executeScalar("select profileName from trxProfiles where trxProfileId = @profileId", sqlTrx, new SqlParameter("@profileId", Trx.TrxProfileId)).ToString();

                    object spPricingId = Utilities.executeScalar(@"select top 1 t.value_loaded 
                                                                    from tasks t, task_type ty 
                                                                    where t.task_type_id = ty.task_type_id 
                                                                    and t.Attribute1 = @trxId
                                                                    and ty.task_type = 'SPECIALPRICING'", sqlTrx,
                            new SqlParameter("@trxId", TrxId));
                    log.LogVariableState("@trxId", TrxId);
                    //Add payments for transaction ver 2.50.0
                    TransactionPaymentsListBL trxPaymentsListBL = new TransactionPaymentsListBL();
                    List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> trxPaymentSearchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                    trxPaymentSearchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, Trx.Trx_id.ToString()));
                    List<TransactionPaymentsDTO> transactionPaymentsDTOList = trxPaymentsListBL.GetTransactionPaymentsDTOList(trxPaymentSearchParameters, sqlTrx);
                    if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count > 0)
                        Trx.TransactionPaymentsDTOList = transactionPaymentsDTOList;
                    //Add payments for transaction ver 2.50.0
                    if (spPricingId != null)
                        pUtilities.ParafaitEnv.specialPricingId = Convert.ToInt32(spPricingId);

                }

                Transaction.TransactionLine TrxLine = new Transaction.TransactionLine();
                TrxLine.LineValid = true;
                TrxLine.CancelledLine = !(dt.Rows[i]["CancelledTime"] == DBNull.Value);
                if (dt.Rows[i]["comboProductId"] != DBNull.Value)
                {
                    TrxLine.ComboproductId = Convert.ToInt32(dt.Rows[i]["comboProductId"]);
                }
                else
                    TrxLine.ComboproductId = -1;

                TrxLine.ProductID = Convert.ToInt32(dt.Rows[i]["product_id"]);
                if (dt.Rows[i]["CategoryId"] != DBNull.Value)
                    TrxLine.CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"]);
                if (!String.IsNullOrEmpty(dt.Rows[i]["productDescription"].ToString().Trim()))
                {
                    TrxLine.ProductName = dt.Rows[i]["productDescription"].ToString().Trim();
                    if (dt.Rows[i]["ParentProductId"] != DBNull.Value)
                    {
                        TrxLine.ParentModifierProductId = Convert.ToInt32(dt.Rows[i]["ParentProductId"]);
                    }
                    if (dt.Rows[i]["ParentPrice"] != DBNull.Value)
                    {
                        TrxLine.ParentModifierPrice = Convert.ToDouble(dt.Rows[i]["ParentPrice"]);
                    }
                    if (dt.Rows[i]["ParentModifierId"] != DBNull.Value)
                    {
                        TrxLine.ParentModifierSetId = Convert.ToInt32(dt.Rows[i]["ParentModifierId"]);
                    }
                    if (dt.Rows[i]["ParentProductName"] != DBNull.Value)
                    {
                        TrxLine.ParentModifierName = dt.Rows[i]["ParentProductName"].ToString().Trim();
                    }
                }
                else
                    TrxLine.ProductName = dt.Rows[i]["product_name"].ToString();
                TrxLine.ProductTypeCode = dt.Rows[i]["product_type"].ToString();
                TrxLine.ProductType = dt.Rows[i]["description"].ToString();
                TrxLine.InventoryProductCode = dt.Rows[i]["inventoryProductCode"].ToString();
                TrxLine.AllowPriceOverride = (dt.Rows[i]["AllowPriceOverride"].ToString() == "Y" ? true : false);
                TrxLine.TaxInclusivePrice = dt.Rows[i]["TaxInclusivePrice"].ToString();
                TrxLine.OrderTypeId = dt.Rows[i]["OrderTypeId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["OrderTypeId"]) : -1;
                TrxLine.taxName = dt.Rows[i]["tax_name"].ToString();
                TrxLine.OriginalLineID = dt.Rows[i]["OriginalLineID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["OriginalLineID"]) : -1;
                TrxLine.productHSNCode = dt.Rows[i]["HsnSacCode"].ToString();
                TrxLine.TrxProfileId = (dt.Rows[i]["TrxLineProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["TrxLineProfileId"]));
                TrxLine.guid = dt.Rows[i]["trxLineGuid"].ToString(); // Added to fix saved trx line discount display issue in handheld
                TrxLine.AllocatedProductPrice = dt.Rows[i]["AllocatedProductPrice"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["AllocatedProductPrice"]);
                //Modified on - 19-May-2016 - for waiver implementation
                TrxLine.IsWaiverRequired = (dt.Rows[i]["IsWaiverSignRequired"] == DBNull.Value ? string.Empty : dt.Rows[i]["IsWaiverSignRequired"].ToString());
                //End  Modified on - 19-May-2016 - for waiver implementation
                //Starts Modification on - 04-Oct-2018 - for Modifier changes for 2.40
                TrxLine.ModifierSetId = dt.Rows[i]["ModifierSetId"];
                //Ends Modification on - 04-Oct-2018 - for Modifier changes for 2.40
                // Starts Modification on -07-11-2019 for ClubSpeed enhancement changes - Akshay G
                TrxLine.lastUpdateDate = dt.Rows[i]["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[i]["LastUpdateDate"]);
                // Ends Modification on -07-11-2019 for ClubSpeed enhancement changes - Akshay G
                CommandText = @"select p.*, CASE WHEN ISNULL(p.WaiverRequired,'N') = 'N' THEN
                                                CASE WHEN ISNULL(p.waiversetId,-1) > -1 THEN 
                                                     'Y'
                                                ELSE 
                                                  ISNULL((SELECT top 1 'Y' 
                                                             FROM FacilityWaiver fw, ProductsAllowedInFacility paf, FacilityMapDetails fmd
					                                            where paf.ProductsId =  p.Product_id
					                                              and paf.IsActive = 1
					                                              and paf.FacilityMapId = fmd.FacilityMapId
					                                              and fmd.IsActive = 1
					                                              and fw.FacilityId = fmd.FacilityId
					                                              and fw.IsActive = 1
					                                              and ISNULL(fw.EffectiveFrom, getdate()) >= getdate()
					                                              and ISNULL(fw.EffectiveTo, getdate()) <= getdate()),'N')
                                                END
                                               ELSE p.WaiverRequired END as WaiversRequired 
                                from products p where product_id = @productId ";

                TrxLine.CardGuid = dt.Rows[i]["CardGuid"] != DBNull.Value ? Convert.ToString(dt.Rows[i]["CardGuid"]) : string.Empty;

                productdataTable = Utilities.executeDataTable(CommandText, sqlTrx, new SqlParameter("@productId", Convert.ToInt32(dt.Rows[i]["product_id"])));

                if (productdataTable.Rows.Count > 0 && productdataTable.Rows[0]["WaiverSetId"] != DBNull.Value)
                    TrxLine.WaiverSetId = Convert.ToInt32(productdataTable.Rows[0]["WaiverSetId"]);

                switch (TrxLine.ProductTypeCode)
                {
                    case "CHECK-IN":
                    case "CHECK-OUT": TrxLine.AllowCancel = false; break;
                }

                switch (TrxLine.ProductTypeCode)
                {
                    case "CHECK-IN":
                    case "CHECK-OUT":
                    case "ATTRACTION":
                    case "NEW":
                    case "RECHARGE":
                    case "CARDSALE":
                    case "CARDDEPOSIT":
                    case "EXCESSVOUCHERVALUE":
                    case "GAMETIME":
                    case "VARIABLECARD": TrxLine.AllowEdit = false; break;
                    default: TrxLine.AllowEdit = true; break;
                }

                TrxLine.DBLineId = Convert.ToInt32(dt.Rows[i]["lineId"]);
                transactionLineDictionary.Add(TrxLine.DBLineId, TrxLine);
                if (dt.Rows[i]["ParentLineId"] != DBNull.Value)
                {
                    int parentLineId = Convert.ToInt32(dt.Rows[i]["ParentLineId"]);
                    TrxLine.TransactionLineDTO.ParentLineId = parentLineId;
                    foreach (Transaction.TransactionLine parentLine in Trx.TrxLines)
                    {
                        if (parentLine.DBLineId == parentLineId)
                        {
                            TrxLine.ParentLine = parentLine;
                            if (CanStopProductPriceOverRide(TrxLine))
                            {
                                TrxLine.AllowPriceOverride = false;
                            }
                            TrxLine.UserPrice = false;
                            break;
                        }
                    }
                }
                if (dt.Rows[i]["ParentModifierId"] != DBNull.Value)
                {
                    TrxLine.AllowPriceOverride = false;
                    TrxLine.UserPrice = false;
                }

                if (TrxLine.ProductTypeCode.Equals("EXCESSVOUCHERVALUE"))
                {
                    TrxLine.LineValid = false;
                    TrxLine.AllowCancel = false;
                }

                if (TrxLine.ProductTypeCode.Equals("VOUCHER"))
                {
                    DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchDiscountCouponsParams = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                    searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.TRANSACTION_ID, TrxId.ToString()));
                    searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.LINE_ID, TrxLine.DBLineId.ToString()));
                    searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                    TrxLine.IssuedDiscountCouponsDTOList = discountCouponsListBL.GetDiscountCouponsDTOList(searchDiscountCouponsParams, sqlTrx);
                }

                TrxLine.OriginalPrice = TrxLine.Price = Convert.ToDouble(dt.Rows[i]["price"]);
                if (dt.Rows[i]["quantity"] != DBNull.Value)
                    TrxLine.quantity = Convert.ToDecimal(dt.Rows[i]["quantity"]);
                else
                    TrxLine.quantity = 1;
                TrxLine.LineAmount = Convert.ToDouble(dt.Rows[i]["amount"]);
                if (dt.Rows[i]["card_number"] != DBNull.Value)
                {
                    TrxLine.CardNumber = dt.Rows[i]["card_number"].ToString();
                    bool found = false;
                    foreach (Transaction.TransactionLine line in Trx.TrxLines)
                    {
                        if (TrxLine.CardNumber.Equals(line.CardNumber))
                        {
                            TrxLine.card = line.card;
                            found = true;
                        }
                    }
                    if (!found)
                        TrxLine.card = new Card((int)dt.Rows[i]["card_id"], "", Utilities, sqlTrx);
                }
                TrxLine.Credits = (dt.Rows[i]["credits"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["credits"]));
                TrxLine.LoyaltyPoints = (dt.Rows[i]["loyalty_points"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["loyalty_points"]));
                TrxLine.Bonus = (dt.Rows[i]["bonus"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["bonus"]));
                TrxLine.Courtesy = (dt.Rows[i]["courtesy"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["courtesy"]));
                TrxLine.Time = (dt.Rows[i]["time"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["time"]));
                TrxLine.Tickets = (dt.Rows[i]["tickets"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["tickets"]));
                TrxLine.tax_id = (dt.Rows[i]["tax_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["tax_id"]));
                TrxLine.tax_percentage = (dt.Rows[i]["tax_percentage"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["tax_percentage"]));
                TrxLine.Remarks = dt.Rows[i]["lineRemarks"].ToString();
                TrxLine.ApprovedBy = dt.Rows[i]["ApprovedBy"].ToString();
                TrxLine.MembershipId = (dt.Rows[i]["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["MembershipId"]));
                TrxLine.MembershipRewardsId = (dt.Rows[i]["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["MembershipRewardsId"]));
                if (Convert.ToInt32(dt.Rows[i]["NotificationTagIssuedId"]) > -1)
                {
                    TrxLine.NotificationTagIssuedDTO = new NotificationTagIssuedBL(Utilities.ExecutionContext, Convert.ToInt32(dt.Rows[i]["NotificationTagIssuedId"]), sqlTrx).GetNotificationTagIssuedDTO;
                }
                log.LogVariableState("@trxId", TrxId);
                log.LogVariableState("@lineId", TrxLine.DBLineId);

                if (TrxLine.ProductTypeCode.Equals("ATTRACTION"))
                {
                    //DataTable dtATB = Utilities.executeDataTable(@"select top 1 ScheduleTime, PlayName, BookedUnits, BookingId
                    //                                                from AttractionBookings atb, AttractionPlays atp
                    //                                                where trxId = @trxId 
                    //                                                and atb.AttractionPlayId = atp.AttractionPlayId
                    //                                                and lineId = @lineId order by BookingId desc",
                    //                                            new SqlParameter("@trxId", TrxId),
                    //                                            new SqlParameter("@lineId", TrxLine.DBLineId));
                    AttractionBookingList attractionBookingList = new AttractionBookingList(Utilities.ExecutionContext);
                    List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> atbSearchParams = new List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>>();
                    atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.TRX_ID, TrxId.ToString()));
                    atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.LINE_ID, TrxLine.DBLineId.ToString()));
                    atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.IS_EXPIRY_DATE_EXPIRED, "Y"));
                    List<AttractionBookingDTO> attractionBookingDTOList = attractionBookingList.GetAttractionBookingDTOList(atbSearchParams, true, sqlTrx);

                    if (attractionBookingDTOList != null && attractionBookingDTOList.Count > 0)
                    {
                        //TrxLine.AttractionDetails = dtATB.Rows[0]["PlayName"].ToString() + ":" +
                        //                            Convert.ToDateTime(dtATB.Rows[0]["ScheduleTime"]).ToString("d-MMM-yyyy h:mm tt");
                        TrxLine.AttractionDetails = attractionBookingDTOList[0].AttractionPlayName + ":" + attractionBookingDTOList[0].ScheduleFromDate.ToString("d-MMM-yyyy h:mm tt");

                        //DataTable dtSeats = Utilities.executeDataTable(@"select fs.SeatName 
                        //                                                from AttractionBookingSeats abs, FacilitySeats fs 
                        //                                                where fs.SeatId = abs.SeatId 
                        //                                                and BookingId = @bookingId",
                        //                                                new SqlParameter("@bookingId", dtATB.Rows[0]["BookingId"]));

                        log.LogVariableState("@bookingId", attractionBookingDTOList[0].BookingId);

                        string seats = "";
                        //foreach (DataRow dr in dtSeats.Rows)
                        //{
                        //    seats += dr["SeatName"].ToString() + ",";
                        //}
                        if (attractionBookingDTOList[0].AttractionBookingSeatsDTOList != null && attractionBookingDTOList[0].AttractionBookingSeatsDTOList.Count > 0)
                        {
                            foreach (AttractionBookingSeatsDTO attractionBookingSeatsDTO in attractionBookingDTOList[0].AttractionBookingSeatsDTOList)
                            {
                                seats += attractionBookingSeatsDTO.SeatName + ",";
                            }
                        }
                        if (string.IsNullOrEmpty(seats) == false)
                            TrxLine.AttractionDetails += ":" + seats.TrimEnd(',');

                        TrxLine.LineAtb = new AttractionBooking(Utilities.ExecutionContext, attractionBookingDTOList[0]);
                    }
                    //TrxLine.LineAtb = new AttractionBooking(Utilities.ExecutionContext);
                    //TrxLine.LineAtb.AttractionBookingDTO.AttractionPlayId = (dt.Rows[i]["AttractionPlayId"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["AttractionPlayId"]));
                    //TrxLine.LineAtb.AttractionBookingDTO.AttractionScheduleId = (dt.Rows[i]["AttractionScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["AttractionScheduleId"]));
                    //TrxLine.LineAtb.AttractionBookingDTO.BookingId = (dt.Rows[i]["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["BookingId"]));
                    //TrxLine.LineAtb.AttractionBookingDTO.ScheduleTime = (dt.Rows[i]["ScheduleTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[i]["ScheduleTime"]));
                    //TrxLine.LineAtb.AttractionBookingDTO.ScheduleFromTime = Decimal.Round(dt.Rows[i]["ScheduleFromTime"] == DBNull.Value ? -1 : Convert.ToDecimal(dt.Rows[i]["ScheduleFromTime"]), 2, MidpointRounding.AwayFromZero);
                    //TrxLine.LineAtb.AttractionBookingDTO.ScheduleToTime = Decimal.Round(dt.Rows[i]["ScheduleToTime"] == DBNull.Value ? -1 : Convert.ToDecimal(dt.Rows[i]["ScheduleToTime"]), 2, MidpointRounding.AwayFromZero);
                }
                else if (TrxLine.ProductTypeCode.Equals("RENTAL"))
                {
                    TransactionReservationScheduleListBL transactionReservationScheduleListBL = new TransactionReservationScheduleListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>> trsSearchParams = new List<KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>>();
                    trsSearchParams.Add(new KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>(TransactionReservationScheduleDTO.SearchByParameters.TRX_ID, TrxId.ToString()));
                    trsSearchParams.Add(new KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>(TransactionReservationScheduleDTO.SearchByParameters.LINE_ID, TrxLine.DBLineId.ToString()));
                    trsSearchParams.Add(new KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>(TransactionReservationScheduleDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                    List<TransactionReservationScheduleDTO> transactionReservationScheduleDTOList = transactionReservationScheduleListBL.GetTransactionReservationScheduleDTOList(trsSearchParams, sqlTrx);

                    if (transactionReservationScheduleDTOList != null && transactionReservationScheduleDTOList.Any())
                    {
                        transactionReservationScheduleDTOList = transactionReservationScheduleDTOList.OrderBy(trs => trs.Cancelled).ThenByDescending(trs => trs.TrxReservationScheduleId).ToList();
                        TrxLine.TransactionReservationScheduleDTOList = transactionReservationScheduleDTOList;
                    }
                }
                else if (TrxLine.ProductTypeCode.Equals("CHECK-IN"))
                {
                    if (dt.Rows[i]["CheckInId"] == DBNull.Value)
                    {
                        object date = Utilities.executeScalar("select top 1 CheckInTime from CheckInDetails where CheckInTrxId = @trxId and CheckInTrxLineId = @LineId", sqlTrx,
                                                                new SqlParameter("@trxId", TrxId),
                                                                new SqlParameter("@LineId", TrxLine.DBLineId));
                        if (date != null)
                        {
                            TrxLine.ProductName += date;
                        }
                        log.LogVariableState("@trxId", TrxId);
                        log.LogVariableState("@LineId", TrxLine.DBLineId);
                    }
                    else
                    {
                        if (dt.Rows[i]["CheckInDetailId"] != DBNull.Value)
                        {
                            TrxLine.LineCheckInDetailDTO = new CheckInDetailBL(Utilities.ExecutionContext, Convert.ToInt32(dt.Rows[i]["CheckInDetailId"]), false, false, sqlTrx).CheckInDetailDTO;
                            if (TrxLine.LineCheckInDetailDTO != null)
                            {
                                CheckInDTO lineCheckInDTO = new CheckInBL(Utilities.ExecutionContext, TrxLine.LineCheckInDetailDTO.CheckInId, false, false, sqlTrx).CheckInDTO;
                                if (lineCheckInDTO != null && TrxLine.ParentLine == null)
                                    TrxLine.LineCheckInDTO = lineCheckInDTO;
                                if (lineCheckInDTO != null && TrxLine.ParentLine != null && TrxLine.ParentLine.ProductTypeCode == "COMBO")
                                    if (Trx.TrxLines.Exists(x => x.LineCheckInDTO != null) == false)
                                    {
                                        TrxLine.LineCheckInDTO = lineCheckInDTO;
                                    }
                                if (lineCheckInDTO != null && TrxLine.LineCheckInDetailDTO.CheckInTime != null)
                                    TrxLine.ProductName += Convert.ToDateTime(TrxLine.LineCheckInDetailDTO.CheckInTime).ToString(" - h:mm tt");
                            }
                        }
                    }
                }
                else if (TrxLine.ProductTypeCode.Equals("CHECK-OUT"))
                {
                    if (dt.Rows[i]["CheckOutDetailId"] == DBNull.Value)
                    {
                        DateTime checkInTime = Convert.ToDateTime(Utilities.executeScalar(@"select top 1 cd.CheckInTime from CheckIns ci, CheckInDetails cd
                                                                                        where ci.CheckInId = cd.CheckInId and cd.CheckOutTrxId = @trxId and cd.TrxLineId = @LineId", sqlTrx,
                                                                                           new SqlParameter("@trxId", TrxId),
                                                                                           new SqlParameter("@LineId", TrxLine.DBLineId)));

                        log.LogVariableState("@trxId", TrxId);
                        log.LogVariableState("@LineId", TrxLine.DBLineId);

                        DateTime checkOutTime = Convert.ToDateTime(Utilities.executeScalar("select top 1 CheckOutTime from CheckInDetails where CheckOutTrxId = @trxId and TrxLineId = @LineId", sqlTrx,
                                                                                           new SqlParameter("@trxId", TrxId),
                                                                                           new SqlParameter("@LineId", TrxLine.DBLineId)));

                        log.LogVariableState("@trxId", TrxId);
                        log.LogVariableState("@LineId", TrxLine.DBLineId);

                        TrxLine.ProductName += checkOutTime.ToString(" - h:mm tt [") + (checkOutTime - checkInTime).TotalMinutes.ToString("N0") + " Mins]";
                    }
                    else
                    {
                        TrxLine.LineCheckOutDetailDTO = new CheckInDetailBL(Utilities.ExecutionContext, Convert.ToInt32(dt.Rows[i]["CheckOutDetailId"]), false, false, sqlTrx).CheckInDetailDTO;
                        if (TrxLine.LineCheckOutDetailDTO != null)
                        {
                            CheckInDTO lineCheckInDTO = new CheckInBL(Utilities.ExecutionContext, TrxLine.LineCheckOutDetailDTO.CheckInId, false, false, sqlTrx).CheckInDTO;
                            if (lineCheckInDTO != null)
                            {
                                TrxLine.LineCheckInDTO = lineCheckInDTO;
                                TrxLine.ProductName += Convert.ToDateTime(TrxLine.LineCheckOutDetailDTO.CheckInTime).ToString(" - h:mm tt [") + (Convert.ToDateTime(TrxLine.LineCheckOutDetailDTO.CheckOutTime) - Convert.ToDateTime(TrxLine.LineCheckOutDetailDTO.CheckInTime)).TotalMinutes.ToString("N0") + " Mins]";
                            }
                        }
                    }
                }
                else if (TrxLine.ProductTypeCode.Equals("LOCKER"))
                {
                    if (dt.Rows[i]["LockerAllocationId"] != DBNull.Value)
                    {
                        LockerAllocation lockerAllocation = null;
                        TrxLine.LockerName = (dt.Rows[i]["LockerName"] == DBNull.Value ? "Free Mode" : dt.Rows[i]["LockerName"].ToString());
                        TrxLine.LockerNumber = (dt.Rows[i]["Identifier"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["Identifier"]));
                        if (Convert.ToInt32(dt.Rows[i]["LockerAllocationId"]) > -1 && TrxLine.lockerAllocationDTO == null)
                        {
                            lockerAllocation = new LockerAllocation(Convert.ToInt32(dt.Rows[i]["LockerAllocationId"]), sqlTrx);
                            TrxLine.lockerAllocationDTO = lockerAllocation.GetLockerAllocationDTO;
                        }
                        else
                        {
                            TrxLine.lockerAllocationDTO = new LockerAllocationDTO();
                        }
                        //if(TrxLine.lockerAllocationDTO.ZoneCode
                        //TrxLine.ZoneCode = (dt.Rows[i]["ZoneCode"] == DBNull.Value ? "" : dt.Rows[i]["ZoneCode"].ToString());
                        if (TrxLine.lockerAllocationDTO.ValidToTime.Equals(DateTime.MinValue))
                        {
                            TrxLine.lockerAllocationDTO.ValidToTime = (dt.Rows[i]["ValidToTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[i]["ValidToTime"]));
                        }
                    }
                }

                TrxLine.tax_amount = (double)TrxLine.quantity * TrxLine.tax_percentage * TrxLine.Price / 100.0;
                if (dt.Rows[i]["ReceiptPrinted"] != DBNull.Value)
                    TrxLine.ReceiptPrinted = Convert.ToBoolean(dt.Rows[i]["ReceiptPrinted"]);
                if (dt.Rows[i]["UserPrice"] != DBNull.Value)
                    TrxLine.UserPrice = Convert.ToBoolean(dt.Rows[i]["UserPrice"]);
                TrxLine.KOTPrintCount = dt.Rows[i]["KOTPrintCount"];
                if (TrxLine.KOTPrintCount != DBNull.Value)
                    TrxLine.PrintKOT = false;

                if (dt.Rows[i]["KDSSent"] != DBNull.Value)
                    TrxLine.KDSSent = Convert.ToBoolean(dt.Rows[i]["KDSSent"]);

                if (dt.Rows[i]["UsedInDiscounts"] != DBNull.Value)
                    TrxLine.UsedInDiscounts = true;

                if (dt.Rows[i]["TrxLineModifierSetId"] != DBNull.Value)
                    TrxLine.HasModifier = true;

                if ((TrxLine.ReceiptPrinted || TrxLine.KDSSent) && CANCEL_PRINTED_TRX_LINE == false)
                    TrxLine.AllowEdit = false;

                TrxLine.LinkAsChildCard = Convert.ToBoolean(Convert.ToInt32(dt.Rows[i]["LinkAsChildCard"]));
                TrxLine.LicenseType = dt.Rows[i]["LicenseType"].ToString();
                if (Trx.Status == Transaction.TrxStatus.PENDING)
                {
                    TrxLine.AllowCancel = false;
                    TrxLine.AllowEdit = false;
                    TrxLine.AllowPriceOverride = false;
                }

                if (dt.Rows[i]["VerificationId"] != DBNull.Value)
                {
                    TrxLine.userVerificationId = dt.Rows[i]["VerificationId"].ToString();
                    if (dt.Rows[i]["VerificationName"] != DBNull.Value)
                        TrxLine.userVerificationName = dt.Rows[i]["VerificationName"].ToString();
                    if (dt.Rows[i]["VerificationRemarks"] != DBNull.Value)
                        TrxLine.userVerificationName = dt.Rows[i]["VerificationRemarks"].ToString();
                }

                if (Trx.Status == Transaction.TrxStatus.PENDING)
                {
                    TrxLine.AllowCancel = false;
                    TrxLine.AllowEdit = false;
                    TrxLine.AllowPriceOverride = false;
                }

                //Add CreditPlusConsumptionId - Muaaz added for CCP consumption details
                if (dt.Rows[i]["CreditPlusConsumptionId"] != DBNull.Value)
                {
                    int.TryParse(Convert.ToString(dt.Rows[i]["CreditPlusConsumptionId"]), out TrxLine.CreditPlusConsumptionId);
                }

                Trx.TrxLines.Add(TrxLine);
            }

            //Booking migration transactions will have parent lines having line id greater than their child lines
            if (Trx.TrxLines != null && Trx.TrxLines.Count > 1)
            {
                List<Transaction.TransactionLine> trxChildLines = Trx.TrxLines.Where(tl => tl.LineValid == true && tl.ParentLine == null && tl.TransactionLineDTO != null && tl.TransactionLineDTO.ParentLineId != -1).ToList();
                if (trxChildLines != null && trxChildLines.Count > 0)
                {
                    foreach (Transaction.TransactionLine childTrxLine in trxChildLines)
                    {
                        Transaction.TransactionLine parentTrxLine = Trx.TrxLines.Find(tl => tl.LineValid == true && tl.DBLineId == childTrxLine.TransactionLineDTO.ParentLineId);
                        if (parentTrxLine != null)
                        {
                            childTrxLine.ParentLine = parentTrxLine;
                            childTrxLine.AllowPriceOverride = false;
                            childTrxLine.UserPrice = false;
                        }
                    }
                }
            }

            TransactionDiscountsListBL transactionDiscountsListBL = new TransactionDiscountsListBL(Utilities.ExecutionContext);
            List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>(TransactionDiscountsDTO.SearchByParameters.TRANSACTION_ID, TrxId.ToString()));
            searchParams.Add(new KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>(TransactionDiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
            List<TransactionDiscountsDTO> transactionDiscountsDTOList = transactionDiscountsListBL.GetTransactionDiscountsDTOList(searchParams, sqlTrx);
            if (transactionDiscountsDTOList != null && transactionDiscountsDTOList.Count > 0)
            {
                foreach (var transactionDiscountsDTO in transactionDiscountsDTOList)
                {
                    if (transactionLineDictionary.ContainsKey(transactionDiscountsDTO.LineId))
                    {
                        if (transactionLineDictionary[transactionDiscountsDTO.LineId].TransactionDiscountsDTOList == null)
                        {
                            transactionLineDictionary[transactionDiscountsDTO.LineId].TransactionDiscountsDTOList = new List<TransactionDiscountsDTO>();
                        }
                        transactionLineDictionary[transactionDiscountsDTO.LineId].TransactionDiscountsDTOList.Add(transactionDiscountsDTO);
                    }
                }
            }

            Trx.getTotalPaidAmount(sqlTrx);
            Trx.updateAmounts(false, sqlTrx);
            Trx.UpdateDiscountsSummary();
            List<DiscountApplicationHistoryDTO> discountApplicationHistoryDTOList = new List<DiscountApplicationHistoryDTO>();
            if (Trx.DiscountsSummaryDTOList != null)
            {
                discountApplicationHistoryDTOList = new List<DiscountApplicationHistoryDTO>();
                Dictionary<int, List<DiscountApplicationHistoryDTO>> discountIdDiscountApplicationHistoryDTOListDictionary = new Dictionary<int, List<DiscountApplicationHistoryDTO>>();
                Dictionary<string, DiscountApplicationHistoryDTO> discountCouponDiscountApplicationHistoryDTODictionary = new Dictionary<string, DiscountApplicationHistoryDTO>();
                //Create application history for line discounts
                foreach (var line in Trx.TrxLines)
                {
                    if (line.TransactionDiscountsDTOList == null)
                    {
                        continue;
                    }

                    foreach (TransactionDiscountsDTO transactionDiscountsDTO in line.TransactionDiscountsDTOList.Where(x => x.Applicability == DiscountApplicability.LINE))
                    {
                        DiscountApplicationHistoryDTO discountApplicationHistoryDTO = new DiscountApplicationHistoryDTO();
                        discountApplicationHistoryDTO.DiscountId = transactionDiscountsDTO.DiscountId;
                        discountApplicationHistoryDTO.Remarks = transactionDiscountsDTO.Remarks;
                        discountApplicationHistoryDTO.ApprovedBy = transactionDiscountsDTO.ApprovedBy;
                        discountApplicationHistoryDTO.TransactionLineBL = line;
                        DiscountContainerDTO discountContainerDTO = DiscountContainerList.GetDiscountContainerDTOOrDefault(Utilities.ExecutionContext, transactionDiscountsDTO.DiscountId);
                        if (discountContainerDTO  == null)
                        {
                            continue;
                        }
                        if (discountContainerDTO.VariableDiscounts == "Y")
                        {
                            discountApplicationHistoryDTO.VariableDiscountAmount = transactionDiscountsDTO.DiscountAmount;
                        }
                        discountApplicationHistoryDTOList.Add(discountApplicationHistoryDTO);
                    }
                    Dictionary<int, int> discountApplicationCountDictionary = new Dictionary<int, int>();
                    foreach (TransactionDiscountsDTO transactionDiscountsDTO in line.TransactionDiscountsDTOList.Where(x => x.Applicability == DiscountApplicability.TRANSACTION))
                    {
                        DiscountContainerDTO discountContainerDTO = DiscountContainerList.GetDiscountContainerDTOOrDefault(Utilities.ExecutionContext, transactionDiscountsDTO.DiscountId);
                        if(discountContainerDTO == null)
                        {
                        	continue;
                        }
                        if (discountContainerDTO.CouponMandatory == "Y")
                        {
                            if (transactionDiscountsDTO.DiscountCouponsUsedDTO == null)
                            {
                                continue;
                            }

                            if (discountCouponDiscountApplicationHistoryDTODictionary.ContainsKey(
                                    transactionDiscountsDTO.DiscountCouponsUsedDTO.CouponNumber) == false)
                            {
                                DiscountApplicationHistoryDTO discountApplicationHistoryDTO = new DiscountApplicationHistoryDTO();
                                discountApplicationHistoryDTO.DiscountId = transactionDiscountsDTO.DiscountId;
                                discountApplicationHistoryDTO.Remarks = transactionDiscountsDTO.Remarks;
                                discountApplicationHistoryDTO.ApprovedBy = transactionDiscountsDTO.ApprovedBy;
                                discountApplicationHistoryDTO.CouponNumber =
                                    transactionDiscountsDTO.DiscountCouponsUsedDTO.CouponNumber;
                                if (discountContainerDTO.VariableDiscounts == "Y")
                                {
                                    discountApplicationHistoryDTO.VariableDiscountAmount = transactionDiscountsDTO.DiscountAmount;
                                }
                                discountCouponDiscountApplicationHistoryDTODictionary.Add(discountApplicationHistoryDTO.CouponNumber, discountApplicationHistoryDTO);
                                discountApplicationHistoryDTOList.Add(discountApplicationHistoryDTO);
                            }
                            else if (discountContainerDTO.VariableDiscounts == "Y")
                            {
                                discountCouponDiscountApplicationHistoryDTODictionary[
                                        transactionDiscountsDTO.DiscountCouponsUsedDTO.CouponNumber]
                                    .VariableDiscountAmount += transactionDiscountsDTO.DiscountAmount;
                            }
                            continue;
                        }

                        if (discountApplicationCountDictionary.ContainsKey(transactionDiscountsDTO.DiscountId) == false)
                        {
                            discountApplicationCountDictionary.Add(transactionDiscountsDTO.DiscountId, 0);
                        }

                        discountApplicationCountDictionary[transactionDiscountsDTO.DiscountId]++;

                        if (discountIdDiscountApplicationHistoryDTOListDictionary.ContainsKey(transactionDiscountsDTO
                                .DiscountId) == false)
                        {
                            discountIdDiscountApplicationHistoryDTOListDictionary.Add(transactionDiscountsDTO
                                .DiscountId, new List<DiscountApplicationHistoryDTO>());
                        }

                        if (discountIdDiscountApplicationHistoryDTOListDictionary[transactionDiscountsDTO
                                .DiscountId].Count <
                            discountApplicationCountDictionary[transactionDiscountsDTO.DiscountId])
                        {
                            DiscountApplicationHistoryDTO discountApplicationHistoryDTO = new DiscountApplicationHistoryDTO();
                            discountApplicationHistoryDTO.DiscountId = transactionDiscountsDTO.DiscountId;
                            discountApplicationHistoryDTO.Remarks = transactionDiscountsDTO.Remarks;
                            discountApplicationHistoryDTO.ApprovedBy = transactionDiscountsDTO.ApprovedBy;
                            if (discountContainerDTO.VariableDiscounts == "Y")
                            {
                                discountApplicationHistoryDTO.VariableDiscountAmount = transactionDiscountsDTO.DiscountAmount;
                            }
                            discountIdDiscountApplicationHistoryDTOListDictionary[transactionDiscountsDTO
                                .DiscountId].Add(discountApplicationHistoryDTO);
                            discountApplicationHistoryDTOList.Add(discountApplicationHistoryDTO);
                        }
                        else if (discountContainerDTO.VariableDiscounts == "Y")
                        {
                            discountIdDiscountApplicationHistoryDTOListDictionary[transactionDiscountsDTO
                                    .DiscountId][
                                    discountApplicationCountDictionary[transactionDiscountsDTO.DiscountId] - 1]
                                .VariableDiscountAmount += transactionDiscountsDTO.DiscountAmount;
                        }
                    }
                }
            }

            Trx.DiscountApplicationHistoryDTOList = discountApplicationHistoryDTOList;


            //Get Waivers Signed From Database
            List<KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>> searchWaverSignedParams = new List<KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>>();
            searchWaverSignedParams.Add(new KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>(WaiverSignatureDTO.SearchByWaiverSignatureParameters.TRX_ID, Trx.Trx_id.ToString()));
            //searchWaverSignedParams.Add(new KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>(WaiverSignatureDTO.SearchByWaiverSignatureParameters.IS_ACTIVE, "1"));
            List<WaiverSignatureDTO> waiverSignedHistDTOList = new WaiverSignatureListBL(Utilities.ExecutionContext).GetWaiverSignatureDTOList(searchWaverSignedParams, sqlTrx);

            if (waiverSignedHistDTOList != null && waiverSignedHistDTOList.Count() > 0)
            {
                foreach (var line in Trx.TrxLines)
                {
                    if (line.LineValid)
                    {
                        line.WaiverSignedDTOList = new List<WaiverSignatureDTO>();
                        foreach (var waiverSignedDTO in waiverSignedHistDTOList)
                        {
                            if (waiverSignedDTO.LineId == line.DBLineId)
                            {
                                waiverSignedDTO.ProductId = line.ProductID;
                                waiverSignedDTO.ProductName = line.ProductName;
                                if (waiverSignedDTO.CustomerSignedWaiverId > -1)
                                {
                                    CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(Utilities.ExecutionContext, waiverSignedDTO.CustomerSignedWaiverId);
                                    waiverSignedDTO.CustomerId = customerSignedWaiverBL.GetCustomerSignedWaiverDTO.SignedFor;
                                }
                                //waiverSignedDTO.WaiverOriginalFileName = waiverSignedDTO.WaiverSignedFileName;
                                line.WaiverSignedDTOList.Add(waiverSignedDTO);
                                //Trx.AddWaiverSignedToHistoryList(waiverSignedDTO);
                            }
                        }
                    }
                }
            }
            else if (productdataTable.Rows.Count > 0
                     && (productdataTable.Rows[0]["WaiverSetId"] != DBNull.Value || productdataTable.Rows[0]["WaiversRequired"] == "Y"))
            {
                foreach (var line in Trx.TrxLines)
                {
                    Trx.CreateWaiverSetLineData(line, Convert.ToInt32(productdataTable.Rows[0]["WaiverSetId"]), sqlTrx);
                }
            }
            LoadAttendeeDetails(Trx, sqlTrx);
            LoadSubscriptionDetails(Trx, sqlTrx);
            LoadTrxPOSPrinterOverriderRules(Trx, sqlTrx);
            LoadTrxOrderDispensingDTO(Trx, sqlTrx);
            LoadKDSKOTChildDTOs(Trx, sqlTrx);
            log.LogMethodExit(Trx);
            return (Trx);
        }
        /// <summary>
        /// CanStopProductPriceOverRide
        /// </summary>
        /// <param name="trxLine"></param>
        /// <returns></returns>
        public static bool CanStopProductPriceOverRide(Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry(trxLine);
            bool canStopProductPriceOverRide = false;
            if (trxLine.AllowPriceOverride)
            {
                if (trxLine.ParentLine != null)
                {
                    bool parentIsCharged = ComboParentHasPrice(trxLine.ParentLine);
                    if (parentIsCharged)
                    {
                        canStopProductPriceOverRide = true;
                    }
                }
            }
            log.LogMethodExit(canStopProductPriceOverRide);
            return canStopProductPriceOverRide;
        }

        private static bool ComboParentHasPrice(Transaction.TransactionLine parentLine)
        {
            log.LogMethodEntry(parentLine);
            bool parentIsCharged = false;
            if (parentLine.ProductTypeCode == ProductTypeValues.COMBO && parentLine.Price != 0)
            {
                parentIsCharged = true;
            }
            else if (parentLine.ParentLine != null)
            {
                parentIsCharged = ComboParentHasPrice(parentLine.ParentLine);
            }
            log.LogMethodExit(parentIsCharged);
            return parentIsCharged;
        }

        //public void UpdateTrxPayments(int trxid, int paymentID, int paymentModeId, double TrxAmount, double TipAmount, int ccResponseId, string reference, SqlTransaction SQLTrx)//Starts:Modification on 20-Oct-2016 for adding firstdata tip feature
        //{
        //    log.LogMethodEntry(trxid, paymentID, paymentModeId, TrxAmount, TipAmount, ccResponseId, reference, SQLTrx);

        //    SqlCommand creditCardPaymentcmd;
        //    if (SQLTrx == null)
        //    {
        //        creditCardPaymentcmd = Utilities.getCommand();
        //    }
        //    else
        //    {
        //        creditCardPaymentcmd = Utilities.getCommand(SQLTrx);
        //    }
        //    creditCardPaymentcmd.CommandText = @"update TrxPayments set PaymentModeId = @paymentModeId, Amount = @amount,TipAmount = @tipAmount, CCResponseId = (Case when @responseId<=0 Then CCResponseId else @responseId end), Reference = @reference,LastUpdatedUser = @user where PaymentId = @PaymentId and TrxId = @trxId";
        //    creditCardPaymentcmd.Parameters.AddWithValue("@amount", TrxAmount);
        //    creditCardPaymentcmd.Parameters.AddWithValue("@paymentModeId", paymentModeId);
        //    creditCardPaymentcmd.Parameters.AddWithValue("@PaymentId", paymentID);
        //    creditCardPaymentcmd.Parameters.AddWithValue("@trxId", trxid);
        //    creditCardPaymentcmd.Parameters.AddWithValue("@tipAmount", TipAmount);
        //    creditCardPaymentcmd.Parameters.AddWithValue("@responseId", ccResponseId);
        //    creditCardPaymentcmd.Parameters.AddWithValue("@reference", reference);
        //    creditCardPaymentcmd.Parameters.AddWithValue("@user", Utilities.ParafaitEnv.LoginID);

        //    log.LogVariableState("@amount", TrxAmount);
        //    log.LogVariableState("@paymentModeId", paymentModeId);
        //    log.LogVariableState("@PaymentId", paymentID);
        //    log.LogVariableState("@trxId", trxid);
        //    log.LogVariableState("@tipAmount", TipAmount);
        //    log.LogVariableState("@responseId", ccResponseId);
        //    log.LogVariableState("@reference", reference);
        //    log.LogVariableState("@user", Utilities.ParafaitEnv.LoginID);

        //    log.LogMethodExit(null);
        //}//Ends:Modification on 20-Oct-2016 for adding firstdata tip feature
        ////Begin: Added for add Tip Payment in Payment Details on 04-Feb-2016//
        //public bool CreateTipPayment(int paymentId, List<int> userIdList, SqlTransaction SQLTrx)//, ref string Message)
        //{
        //    log.LogMethodEntry(paymentId, userIdList, SQLTrx);
        //    try
        //    {
        //        SqlCommand tipPaycmd = Utilities.getCommand(SQLTrx);

        //        foreach (int tipUserId in userIdList)
        //        {
        //            tipPaycmd.CommandText = @"INSERT INTO TipPayment(PaymentId, user_id, CreatedBy, CreationDate) VALUES (@paymentId, @userIdList, @user, getdate()) ";
        //            tipPaycmd.Parameters.Clear();
        //            tipPaycmd.Parameters.AddWithValue("@paymentId", paymentId);
        //            tipPaycmd.Parameters.AddWithValue("@userIdList", tipUserId);
        //            tipPaycmd.Parameters.AddWithValue("@user", ParafaitEnv.Username);
        //            tipPaycmd.ExecuteNonQuery();

        //            log.LogVariableState("@paymentId", paymentId);
        //            log.LogVariableState("@userIdList", tipUserId);
        //            log.LogVariableState("@user", ParafaitEnv.Username);
        //        }


        //        log.LogMethodExit(true);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("", ex);
        //        log.LogMethodExit(false);
        //        return false;
        //    }
        //}
        //End: Added for add Tip Payment in Payment Details on 04-Feb-2016//
        //SYSTEMABANDONED
        public void SetStatusAsSystemAbandoned(int transactionId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(transactionId, sqlTrx);
            if (transactionId > -1)
            {
                Utilities.executeNonQuery("Update trx_header set status = 'SYSTEMABANDONED' where status = 'CANCELLED' and trxId = @trxId", sqlTrx, new SqlParameter[] { new SqlParameter("@trxId", transactionId) });
            }
            log.LogMethodExit();
        }

        //Modified 02/2019 for BearCat - 86-68
        public void UpdateAvailableQuantityForCancelledTransaction(int TrxId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(TrxId, sqlTrx);
            ProductsAvailabilityBL productsAvailability = new ProductsAvailabilityBL(Utilities.ExecutionContext, sqlTrx);
            int rowsUpdated = productsAvailability.UpdateAvailableQuantityForCancelledTransaction(TrxId);
            log.LogMethodExit();
        }

        internal void LoadAttendeeDetails(Transaction trx, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(trx);
            if (trx.Trx_id > 0)
            {
                BookingAttendeeList bookingAttendeeList = new BookingAttendeeList(Utilities.ExecutionContext);
                List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>(BookingAttendeeDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                searchParam.Add(new KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>(BookingAttendeeDTO.SearchByParameters.TRX_ID, trx.Trx_id.ToString()));
                trx.BookingAttendeeList = bookingAttendeeList.GetAllBookingAttendeeList(searchParam, sqlTrx);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get Event Code WaiverSet DTO List
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        public List<WaiverSetDTO> GetEventCodeWaiverSetDTOList(string eventCode)
        {
            log.LogMethodEntry(eventCode);
            List<WaiverSetDTO> waiverSetDTOList = null;
            if (string.IsNullOrEmpty(eventCode))
            {
                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2339));//2339,'Enter a valid event code'
            }
            ReservationListBL reservationListBL = new ReservationListBL(Utilities.ExecutionContext);
            Transaction trx = null;
            if (reservationListBL.ValidEventCode(eventCode))
            {
                trx = CreateTransactionFromReservationCode(eventCode);
            }
            else
            {
                trx = CreateTransactionFromTransactionOTP(eventCode);
            }
            if (trx != null)
            {
                waiverSetDTOList = trx.GetWaiverSetDTOList();
            }
            log.LogMethodExit(waiverSetDTOList);
            return waiverSetDTOList;
        }
        /// <summary>
        /// Create Transaction From ReservationCode
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        public Transaction CreateTransactionFromReservationCode(string eventCode)
        {
            log.LogMethodEntry(eventCode);
            if (string.IsNullOrEmpty(eventCode))
            {
                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2603));
            }
            Transaction trx = null;
            ReservationBL reservationBL = new ReservationBL(Utilities.ExecutionContext, Utilities, eventCode);
            trx = reservationBL.BookingTransaction;
            log.LogMethodExit(trx);
            return trx;
        }
        /// <summary>
        /// Valid Transaction EventCode
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        public int ValidTransactionEventCode(string eventCode)
        {
            log.LogMethodEntry(eventCode);
            int trxId = -1;
            if (string.IsNullOrEmpty(eventCode))
            {
                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2604));
            }
            DataTable dtTrx = Utilities.executeDataTable(@"Select trxId 
                                                             from trx_header th
                                                            where th.TransactionOTP = @trxOTP
                                                              and (th.site_id = @siteId OR @siteId = -1)
                                                              and th.Status not in ('SYSTEMABANDONED','CANCELLED')
                                                              and th.TrxDate >= getdate()
                                                            ", new SqlParameter[] { new SqlParameter("@trxOTP", eventCode), new SqlParameter("@siteId", Utilities.ExecutionContext.GetSiteId()) });
            log.LogVariableState("dtTrx", dtTrx);
            if (dtTrx != null && dtTrx.Rows.Count > 0)
            {
                trxId = Convert.ToInt32(dtTrx.Rows[0]["trxId"]);
            }
            log.LogMethodExit(trxId);
            return trxId;
        }
        /// <summary>
        /// Get Event Code Transaction
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        public Transaction GetEventCodeTransaction(string eventCode)
        {
            log.LogMethodEntry(eventCode);
            Transaction trx = null;
            trx = CreateTransactionFromReservationCode(eventCode);
            if (trx == null)
            {
                trx = CreateTransactionFromTransactionOTP(eventCode);
            }
            log.LogMethodExit(trx);
            return trx;
        }
        /// <summary>
        /// Create Transaction From Transaction OTP
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        public Transaction CreateTransactionFromTransactionOTP(string eventCode)
        {
            log.LogMethodEntry(eventCode);
            Transaction trx = null;
            if (string.IsNullOrEmpty(eventCode))
            {
                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2604));
            }
            int trxId = ValidTransactionEventCode(eventCode);
            if (trxId > -1)
            {
                trx = CreateTransactionFromDB(trxId, Utilities);
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2604));
            }
            log.LogMethodExit(trx);
            return trx;
        }
        /// <summary>
        /// Can Add To Event Code
        /// </summary>
        /// <param name="eventCode"></param>
        /// <param name="customerDTOList"></param>
        /// <returns></returns>
        public bool CanAddToEventCode(string eventCode, List<CustomerDTO> customerDTOList)
        {
            log.LogMethodEntry(eventCode, customerDTOList);
            bool canAddToEventCode = true;
            bool validRCode = false;
            bool validTCode = false;
            int trxId = -1;
            ReservationListBL reservationListBL = new ReservationListBL(Utilities.ExecutionContext);
            if (reservationListBL.ValidEventCode(eventCode))
            {
                validRCode = true;
            }
            else
            {
                trxId = ValidTransactionEventCode(eventCode);
                if (trxId > -1)
                {
                    validTCode = true;
                }
            }
            if (validRCode == false && validTCode == false)
            {
                canAddToEventCode = false;
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "CHECK_WAIVER_REGISTRATION_COUNT_FOR_TRANSACTION", false))
            {
                if (customerDTOList != null && customerDTOList.Any())
                {
                    Transaction trx = null;
                    if (validTCode)
                    {
                        trx = CreateTransactionFromDB(trxId, Utilities);
                    }
                    else
                    {
                        trx = CreateTransactionFromReservationCode(eventCode);
                    }
                    if (trx != null)
                    {
                        List<CustomerDTO> custList = new List<CustomerDTO>(customerDTOList);
                        List<int> mappedCustomerIdList = trx.GetMappedCustomerIdListForWaiver();
                        if (mappedCustomerIdList != null && mappedCustomerIdList.Any())
                        {
                            for (int i = 0; i < custList.Count; i++)
                            {
                                if (mappedCustomerIdList.Exists(custId => custId == custList[i].Id))
                                {
                                    custList.RemoveAt(i);
                                }
                            }
                        }
                        log.LogVariableState("custList", custList);
                        if (custList != null && custList.Any())
                        {
                            List<Transaction.TransactionLine> trxLineList = trx.GetTransactionLinesPendingWaiverSign();
                            log.LogVariableState("trxLineList.Count", trxLineList.Count);
                            if (trxLineList != null && trxLineList.Count < custList.Count)
                            {
                                canAddToEventCode = false;
                            }
                        }
                    }
                    else
                    {
                        canAddToEventCode = false;
                    }
                }
            }
            log.LogMethodExit(canAddToEventCode);
            return canAddToEventCode;
        }
        /// <summary>
        /// Re open Transaction
        /// </summary>
        /// <param name="trx"></param>
        /// <param name="sqlTrx"></param>
        /// <returns>reopenedTrxId</returns>
        public int ReopenTransaction(Transaction trx, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            int reopenedTrxId = -1;
            SqlTransaction newSqlTrx = null;
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                log.LogVariableState("Orginal trx", trx);
                if (trx == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Transaction details are missing"));
                }
                if (sqlTrx != null)
                {
                    newSqlTrx = sqlTrx;
                }
                else
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    newSqlTrx = dBTransaction.SQLTrx;
                }
                SqlConnection cnn = newSqlTrx.Connection;
                //Get payment details from original transactions before reversal
                TransactionPaymentsListBL trxPaymentsListBL = new TransactionPaymentsListBL();
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> trxPaymentSearchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                trxPaymentSearchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, trx.Trx_id.ToString()));
                trxPaymentSearchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));

                List<TransactionPaymentsDTO> transactionPaymentsDTOList = trxPaymentsListBL.GetNonReversedTransactionPaymentsDTOList(trxPaymentSearchParameters, null, newSqlTrx);

                DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(Utilities.ExecutionContext);
                List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchDiscountCouponsUsedParams = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                searchDiscountCouponsUsedParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.TRANSACTION_ID, trx.Trx_id.ToString()));
                searchDiscountCouponsUsedParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));

                List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchDiscountCouponsUsedParams, newSqlTrx);
                if (discountCouponsUsedDTOList != null && discountCouponsUsedDTOList.Any())
                {
                    foreach (var discountCouponsUsedDTO in discountCouponsUsedDTOList)
                    {
                        discountCouponsUsedDTO.IsActive = false;
                        DiscountCouponsUsedBL discountCouponsUsedBL = new DiscountCouponsUsedBL(Utilities.ExecutionContext, discountCouponsUsedDTO);
                        discountCouponsUsedBL.Save(newSqlTrx);
                    }
                }

                DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(Utilities.ExecutionContext);
                List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchDiscountCouponsParams = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.TRANSACTION_ID, trx.Trx_id.ToString()));
                searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                List<DiscountCouponsDTO> discountCouponsDTOList = discountCouponsListBL.GetDiscountCouponsDTOList(searchDiscountCouponsParams, newSqlTrx);
                if (discountCouponsDTOList != null && discountCouponsDTOList.Any())
                {
                    foreach (var discountCouponsDTO in discountCouponsDTOList)
                    {
                        discountCouponsDTO.IsActive = false;
                        DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(Utilities.ExecutionContext, discountCouponsDTO);
                        discountCouponsBL.Save(newSqlTrx);
                    }
                }

                CardDiscountsListBL cardDiscountsListBL = new CardDiscountsListBL();
                List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>> searchCardDiscountsParams = new List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>>();
                searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.TRANSACTION_ID, trx.Trx_id.ToString()));
                searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                List<CardDiscountsDTO> cardDiscountsDTOList = cardDiscountsListBL.GetCardDiscountsDTOList(searchCardDiscountsParams, newSqlTrx);
                if (cardDiscountsDTOList != null && cardDiscountsDTOList.Any())
                {
                    foreach (var cardDiscountsDTO in cardDiscountsDTOList)
                    {
                        cardDiscountsDTO.IsActive = "N";
                        CardDiscountsBL cardDiscountsBL = new CardDiscountsBL(cardDiscountsDTO);
                        cardDiscountsBL.Save(newSqlTrx);
                    }
                }

                string cmdText = @"update TrxParentModifierDetails 
                                    set ParentPrice = 0, LastUpdatedBy = @user_id,LastUpdatedTime = getdate()
                                   where TrxId = @oldTrxId";
                Utilities.executeNonQuery(cmdText, newSqlTrx, new SqlParameter[] { new SqlParameter("@user_id", Utilities.ExecutionContext.GetUserId()),
                                                                        new SqlParameter("@oldTrxId", trx.Trx_id)});
                string message = string.Empty;
                if (ReverseCard(trx.Trx_id, -1, Utilities.ParafaitEnv.LoginID, true, ref message, -1, newSqlTrx, cnn) == false)
                {
                    log.Error("ReverseCard has error: " + message);
                    throw new ValidationException(message);
                }

                for (int i = 0; i < trx.TrxLines.Count; i++)
                {
                    trx.deleteLineFromDB(trx.TrxLines[i], newSqlTrx);
                }
                int orginalTrxId = trx.Trx_id;
                log.LogVariableState("orginalTrxId", orginalTrxId);
                trx.Trx_id = 0;
                for (int i = 0; i < trx.TrxLines.Count; i++)
                {
                    trx.TrxLines[i].DBLineId = 0;
                    trx.TrxLines[i].LineValid = true;
                }
                trx.TrxDate = trx.EntitlementReferenceDate = trx.GameCardReadTime = Utilities.getServerTime();
                trx.Status = Transaction.TrxStatus.OPEN;
                trx.Utilities.ParafaitEnv.POSTypeId = trx.POSTypeId;
                int returnCode = trx.SaveOrder(ref message, newSqlTrx);
                if (returnCode != 0)
                {
                    log.Error("SaveOrder as returnCode != 0 error: " + message);
                    throw new ValidationException(message);
                }
                else
                {
                    if (transactionPaymentsDTOList != null)
                    {
                        foreach (TransactionPaymentsDTO trxPaymentDTO in transactionPaymentsDTOList)
                        {
                            trxPaymentDTO.TransactionId = trx.Trx_id;
                            TransactionPaymentsBL trxPaymentBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                            trxPaymentBL.Save(newSqlTrx);
                        }
                    }
                    string updateNewTrxHeader = @"
                                                 declare @OriginalSystemReference nvarchar(100);
                                                 declare @TransactionOTP nvarchar(100);
                                                 declare @POSMachine nvarchar(50);
                                                 declare @ExternalSystemReference nvarchar(200);
                                                 declare @POSMachineId int;
                                                 declare @UserId int; 
                                                 declare @SiteId int; 

                                                 SELECT top 1 @OriginalSystemReference = Original_System_Reference,
                                                        @TransactionOTP = transactionOTP, 
			                                            @POSMachine = Pos_Machine,
			                                            @POSMachineId = PosMachineId,
			                                            @UserId = user_id,
			                                            @ExternalSystemReference = External_System_reference,
                                                        @SiteId = site_id 
	                                               from trx_header where TrxId = @OldTrxId;

                                                 update trx_header
	                                                set Original_System_Reference= @OriginalSystemReference,
		                                                transactionOTP = @TransactionOTP,
		                                                Pos_Machine = @POSMachine,
		                                                PosMachineId = @POSMachineId,
		                                                user_id = @UserId,
		                                                External_System_reference  = @ExternalSystemReference,
		                                                site_id = @SiteId, 
		                                                LastUpdateTime = getdate(), LastUpdatedBy = @LastUpdatedBy
	                                                where trxId = @TrxId ";
                    Utilities.executeNonQuery(updateNewTrxHeader, newSqlTrx, new SqlParameter[] { new SqlParameter("@OldTrxId", orginalTrxId),
                                                                                               new SqlParameter("@TrxId", trx.Trx_id),
                                                                                               new SqlParameter("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID)
                                                                                             });

                    string updateOriginalTrxHeader = @" 
                                                 update trx_header
	                                                set Status = 'SYSTEMABANDONED', Original_System_Reference= null, transactionOTP = null, External_System_reference  = null, 
                                                        TrxAmount = 0, TrxDiscountPercentage = 0, TaxAmount=0, TrxNetAmount = 0, cashAmount = 0, CreditCardAmount = 0, GameCardAmount =0, Remarks ='Reversing the transaction for reopening.', 
		                                                LastUpdateTime = getdate(), LastUpdatedBy = @LastUpdatedBy
	                                                where trxId = @OldTrxId ";
                    Utilities.executeNonQuery(updateOriginalTrxHeader, newSqlTrx, new SqlParameter[] { new SqlParameter("@OldTrxId", orginalTrxId),
                                                                                               new SqlParameter("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID)
                                                                                             });

                    string updateBookings = @" update bookings
                                                 set TrxId = @TrxId, LastUpdatedDate = getdate(), LastUpdatedBy = @LastUpdatedBy
                                               where trxId = @OldTrxId";
                    Utilities.executeNonQuery(updateBookings, newSqlTrx, new SqlParameter[] { new SqlParameter("@OldTrxId", orginalTrxId),
                                                                                               new SqlParameter("@TrxId", trx.Trx_id),
                                                                                               new SqlParameter("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID)
                                                                                             });

                }
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                }
                reopenedTrxId = trx.Trx_id;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                }
                throw;
            }
            finally
            {
                if (dBTransaction != null)
                {
                    dBTransaction.Dispose();
                }
            }
            log.LogMethodExit(reopenedTrxId);
            return reopenedTrxId;
        }

        private void LoadSubscriptionDetails(Transaction trx, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            //Get subscriptiion details for the transaction
            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(Utilities.ExecutionContext);
            List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchSubscriptionParams = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
            searchSubscriptionParams.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.TRANSACTION_ID, trx.Trx_id.ToString()));
            searchSubscriptionParams.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchSubscriptionParams, Utilities, true, sqlTrx);

            if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Count() > 0)
            {
                foreach (var line in trx.TrxLines)
                {
                    if (line.LineValid)
                    {
                        line.SubscriptionHeaderDTO = null;
                        foreach (SubscriptionHeaderDTO subscriptionHeaderDTO in subscriptionHeaderDTOList)
                        {
                            if (subscriptionHeaderDTO.TransactionLineId == line.DBLineId && subscriptionHeaderDTO.TransactionId == trx.Trx_id)
                            {
                                line.SubscriptionHeaderDTO = subscriptionHeaderDTO;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Load Trx POS Printer Overrider Rules
        /// </summary>
        /// <param name="Trx"></param>
        /// <param name="sqlTrx"></param>
        private void LoadTrxPOSPrinterOverriderRules(Transaction Trx, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(Trx, sqlTrx);
            TrxPOSPrinterOverrideRulesListBL trxPOSPrinterOverrideRulesListBL = new TrxPOSPrinterOverrideRulesListBL(Utilities.ExecutionContext);
            List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRANSACTION_ID, Trx.Trx_id.ToString()));
            Trx.TrxPOSPrinterOverrideRulesDTOList = trxPOSPrinterOverrideRulesListBL.GetTrxPOSPrinterOverrideRulesDTOList(searchByParameters, sqlTrx);
            log.LogVariableState("TrxPOSPrinterOverrideRulesDTOList", Trx.TrxPOSPrinterOverrideRulesDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Original Trx Data
        /// </summary>
        /// <param name="trx"></param>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        private Transaction GetOriginalTrxData(Transaction trx, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(trx, sqlTrx);
            try
            {
                Transaction GetOriginalTrx = null;
                if (trx != null && trx.OriginalTrxId != null)
                {
                    List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
                    TransactionListBL transactionListBL = new TransactionListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, (trx.OriginalTrxId).ToString()));
                    transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, null, sqlTrx, 0, 5000);
                    if (transactionDTOList != null && transactionDTOList.Any())
                    {
                        TransactionUtils transactionUtils = new TransactionUtils(Utilities);
                        GetOriginalTrx = transactionUtils.CreateTransactionFromDB(transactionDTOList[0].TransactionId, Utilities);
                    }
                }
                return GetOriginalTrx;
            }
            catch (Exception ex)
            {
                log.Error("Get Original Trx Data Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Getting Original Trx Data" + ex.Message);
                throw;
            }
        }

        private void ReverseSubscriptions(int origTrxId, int TrxLineId, string approver, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(origTrxId, TrxLineId, approver, sqlTrx);

            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(Utilities.ExecutionContext);
            List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.TRANSACTION_ID, origTrxId.ToString()));
            if (TrxLineId > -1)
            {
                searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.TRANSACTION_LINE_ID, TrxLineId.ToString()));
            }
            List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParam, Utilities, true, sqlTrx);
            if (subscriptionHeaderDTOList != null)
            {
                for (int i = 0; i < subscriptionHeaderDTOList.Count; i++)
                {
                    SubscriptionHeaderBL subscriptionHeaderBL = new SubscriptionHeaderBL(Utilities.ExecutionContext, subscriptionHeaderDTOList[i]);
                    string approverLoginId = approver;
                    subscriptionHeaderBL.ClearSubscriptionEntity(approverLoginId, sqlTrx);
                }
            }
            log.LogMethodExit();
        }

        private void LoadTrxOrderDispensingDTO(Transaction trx, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            TransactionOrderDispensingListBL transctionOrderDispensingListBL = new TransactionOrderDispensingListBL(Utilities.ExecutionContext);
            List<KeyValuePair<TransactionOrderDispensingDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<TransactionOrderDispensingDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<TransactionOrderDispensingDTO.SearchByParameters, string>(TransactionOrderDispensingDTO.SearchByParameters.TRANSACTION_ID, trx.Trx_id.ToString()));
            List<TransactionOrderDispensingDTO> transctionOrderDispensingDTOList = transctionOrderDispensingListBL.GetTransctionOrderDispensing(searchByParameters, true, false, sqlTrx);
            if (transctionOrderDispensingDTOList != null && transctionOrderDispensingDTOList.Any())
            {
                trx.TransctionOrderDispensingDTO = transctionOrderDispensingDTOList[0];
                if (trx != null && trx.TransactionDTO != null)
                {
                    trx.TransactionDTO.TransctionOrderDispensingDTO = transctionOrderDispensingDTOList[0];
                }
            }
            log.LogVariableState("TransctionOrderDispensingDTO", trx.TransctionOrderDispensingDTO);
            log.LogMethodExit();
        }

        private void LoadKDSKOTChildDTOs(Transaction trx, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            KDSOrderLineListBL kdsOrderLineListBL = new KDSOrderLineListBL(Utilities.ExecutionContext);
            List<KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>(KDSOrderLineDTO.SearchByParameters.TRX_ID, trx.Trx_id.ToString()));
            List<KDSOrderLineDTO> kdsOrderLineDTOList = kdsOrderLineListBL.GetKDSOrderLineDTOList(searchParameters, sqlTrx);
            if (kdsOrderLineDTOList != null && kdsOrderLineDTOList.Any())
            {
                for (int i = 0; i < trx.TrxLines.Count; i++)
                {
                    List<KDSOrderLineDTO> lineChildDTOList = kdsOrderLineDTOList.Where(kot => kot.TrxId == trx.Trx_id && kot.LineId == trx.TrxLines[i].DBLineId).ToList();
                    if (lineChildDTOList != null && lineChildDTOList.Any())
                    {
                        trx.TrxLines[i].KDSOrderLineDTOList = lineChildDTOList;
                        if (trx.TrxLines[i].TransactionLineDTO != null)
                        {
                            trx.TrxLines[i].TransactionLineDTO.KDSOrderLineDTOList = lineChildDTOList;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void ReverseReservationSchedule(int origTrxId, int TrxLineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(origTrxId, TrxLineId, sqlTrx);
            TransactionReservationScheduleListBL transactionReservationScheduleListBL = new TransactionReservationScheduleListBL(Utilities.ExecutionContext);
            List<KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>(TransactionReservationScheduleDTO.SearchByParameters.TRX_ID, origTrxId.ToString()));
            if (TrxLineId > -1)
            {
                searchParam.Add(new KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>(TransactionReservationScheduleDTO.SearchByParameters.LINE_ID, TrxLineId.ToString()));
            }
            List<TransactionReservationScheduleDTO> reservationScheduleDTOList = transactionReservationScheduleListBL.GetTransactionReservationScheduleDTOList(searchParam, sqlTrx);
            if (reservationScheduleDTOList != null)
            {
                for (int i = 0; i < reservationScheduleDTOList.Count; i++)
                {
                    if (reservationScheduleDTOList[i].Cancelled == false)
                    {
                        TransactionReservationScheduleBL subscriptionHeaderBL = new TransactionReservationScheduleBL(Utilities.ExecutionContext, reservationScheduleDTOList[i]);
                        subscriptionHeaderBL.ExpireSchedule(sqlTrx);
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
