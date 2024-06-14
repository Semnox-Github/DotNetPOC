using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Parafait_POS.Payment
{
    public class SplitPayments
    {
        internal class clsSplit
        {
            internal int splitId = -1;
            int splitCount = -1;
            internal string reference;
            internal Transaction Trx;
            internal decimal balanceAmount = 0;
            internal bool isEqualSplit = false;
            //Begin: Modified Added for logger function on 08-Mar-2016
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //End: Modified Added for logger function on 08-Mar-2016
            private const string SPLIT_PAYMENTS = "Split Payments";

            public clsSplit(int splitcount, Transaction trx)
            {
                //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
                //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
                //Logger.setRootLogLevel(log);
                //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

                log.Debug("Starts-clsSplit(" + splitcount + ",trx)");//Added for logger function on 08-Mar-2016
                Trx = trx;
                splitCount = splitcount;
                log.Debug("Ends-clsSplit(" + splitcount + ",trx)");//Added for logger function on 08-Mar-2016
            }

            public void refreshBalance()
            {
                log.Debug("Starts-refreshBalance()");//Added for logger function on 08-Mar-2016
                Trx.updateAmounts(false);
                Trx.RemoveDuplicateChargeLines(SPLIT_PAYMENTS, null, null, null);
                Trx.SetAutoGratuityAmount(null);
                Trx.SetServiceCharges(null);
                Trx.updateAmounts(false);
                if (splitId != -1)
                {
                    object o = POSStatic.Utilities.executeScalar(@"select isnull(sum(amount), 0) from trxPayments where splitId = @splitId", new SqlParameter("@splitId", splitId));
                    Trx.TotalPaidAmount = Convert.ToDouble(o);
                    balanceAmount = (decimal)(Trx.Net_Transaction_Amount - Trx.TotalPaidAmount);
                    if (balanceAmount != 0 && balanceAmount < 1 && balanceAmount > -1)
                    {
                        decimal roundAmt = Convert.ToDecimal(POSStatic.Utilities.executeScalar(@"select isnull(sum(amount), 0) 
                                                                                from trxPayments 
                                                                                where splitId = @splitId
                                                                                  and paymentModeId = (select top 1 paymentModeId from PaymentModes where isRoundOff = 'Y')", 
                                                                            new SqlParameter("@splitId", splitId))
                                                            );
                        balanceAmount = decimal.Round(balanceAmount, POSStatic.Utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                        roundAmt = decimal.Round(roundAmt, POSStatic.Utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                        if ((Math.Abs(balanceAmount)) == (Math.Abs(roundAmt)))
                        {//Ignore difference if round off is taking care of rounding
                            balanceAmount = 0;
                        }
                    }
                }
                else
                    balanceAmount = (decimal)Trx.Net_Transaction_Amount;

                log.Debug("Ends-refreshBalance()");//Added for logger function on 08-Mar-2016
            }

            public void addLine(int LineId)
            {
                log.Debug("Starts-refreshBalance("+LineId+")");//Added for logger function on 08-Mar-2016
                foreach (Transaction.TransactionLine tl in Trx.TrxLines)
                {
                    if (tl.DBLineId == LineId)
                    {
                        tl.LineValid = true;
                        MoveChildLines(tl, true);
                        break;
                    }
                }

                refreshBalance();

                log.Debug("Ends-refreshBalance(" + LineId + ")");//Added for logger function on 08-Mar-2016
            }

            private void MoveChildLines(Transaction.TransactionLine parentLine, bool lineValid)
            {
                log.LogMethodEntry(parentLine, lineValid);
                foreach (Transaction.TransactionLine tlChild in Trx.TrxLines)
                {
                    if (parentLine.Equals(tlChild.ParentLine))
                    {
                        tlChild.LineValid = lineValid;
                        MoveChildLines(tlChild, lineValid);
                    }
                }
                log.LogMethodExit();
            }

            public void removeLine(int LineId)
            {
                log.Debug("Starts-removeLine(" + LineId + ")");//Added for logger function on 08-Mar-2016
                foreach (Transaction.TransactionLine tl in Trx.TrxLines)
                {
                    if (tl.DBLineId == LineId)
                    {
                        tl.LineValid = false;
                        MoveChildLines(tl, false);
                        break;
                    }
                }

                refreshBalance();

                log.Debug("Ends-removeLine(" + LineId + ")");//Added for logger function on 08-Mar-2016
            }

            public void save()
            {
                log.Debug("Starts-save()");//Added for logger function on 08-Mar-2016
                if (splitId == -1)
                {
                    object oSplitId = POSStatic.Utilities.executeScalar(@"insert into trxSplitPayments (TrxId, UserReference, NoOfSplits, LastUpdatedDate, LastUpdatedBy)
                                                                        values (@trxId, @ref, @noOfSplits, getdate(), @user); 
                                                                        select @@identity",
                                                                       new SqlParameter("@trxId", Trx.Trx_id),
                                                                       new SqlParameter("@ref", (string.IsNullOrEmpty(reference)? DBNull.Value : (object)reference)),
                                                                       new SqlParameter("@noOfSplits", (splitCount == -1 ? DBNull.Value : (object)splitCount)),
                                                                       new SqlParameter("@user", POSStatic.ParafaitEnv.LoginID));
                    splitId = Convert.ToInt32(oSplitId);
                }
                else
                {
                    POSStatic.Utilities.executeNonQuery(@"update trxSplitPayments set UserReference = @ref, LastUpdatedDate = getdate(), LastUpdatedBy = @user
                                                        where splitId = @splitId",
                                                        new SqlParameter("@splitId", splitId),
                                                        new SqlParameter("@ref", (string.IsNullOrEmpty(reference) ? DBNull.Value : (object)reference)),
                                                        new SqlParameter("@user", POSStatic.ParafaitEnv.LoginID));
                }

                POSStatic.Utilities.executeNonQuery("delete from trxSplitLines where splitId = @splitId",
                                                    new SqlParameter("@splitId", splitId));

                foreach (Transaction.TransactionLine tl in Trx.TrxLines)
                {
                    if (tl.LineValid)
                    {
                        POSStatic.Utilities.executeNonQuery("insert trxSplitLines (SplitId, LineId) values (@splitId, @LineId)",
                                                             new SqlParameter("@splitId", splitId),
                                                             new SqlParameter("@LineId", tl.DBLineId));
                    }
                }
                log.Debug("Ends-save()");//Added for logger function on 08-Mar-2016
            }            
        }

        internal List<clsSplit> splits = new List<clsSplit>();
        internal bool isEqualSplit = true;
        internal Transaction _trx;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public SplitPayments(Transaction inTrx)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-SplitPayments(inTrx)");//Added for logger function on 08-Mar-2016
            _trx = inTrx;
            if (inTrx.Trx_id != -1)
            {
                DataTable dt = POSStatic.Utilities.executeDataTable(@"Select * from trxSplitPayments where TrxId = @trxId order by splitId",
                                                        new SqlParameter("@trxId", inTrx.Trx_id));
                if (dt.Rows.Count > 0)
                {
                    int splitCount = -1;
                    if (!Int32.TryParse(dt.Rows[0]["NoOfSplits"].ToString(), out splitCount))
                        splitCount = -1;
                    if (splitCount != -1)
                    {
                        isEqualSplit = true;

                        createEqualSplits(splitCount);

                        int i = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            splits[i].splitId = Convert.ToInt32(dr["SplitId"]);
                            splits[i].reference = dr["UserReference"].ToString();
                            i++;
                        }

                        foreach (clsSplit s in splits)
                            s.refreshBalance();
                    }
                    else
                    {
                        isEqualSplit = false;

                        // if a trx line is not in any split, add it to the first split

                        POSStatic.Utilities.executeNonQuery(@"insert into trxSplitLines (SplitId, LineId) 
                                                                select isnull((select min(SplitId) 
                                                                                from trxSplitPayments tsp
                                                                                where trxId = @trxId
                                                                                 and not exists (select 'x' from trxpayments p
                                                                                                   where p.splitid = tsp.SplitId
				                                                                                    and p.trxid = tsp.trxid))
                                                                               , (select min(SplitId) 
                                                                                    from trxSplitPayments tsp
                                                                                   where trxId = @trxId))
                                                                       , l.LineId 
                                                                from trx_lines l
                                                                where l.trxId = @trxId
                                                                and l.CancelledTime is null
                                                                and l.lineId not in (select lineId 
                                                                                      from trxSplitLines 
                                                                                     where splitId in (select splitId 
                                                                                                        from trxSplitPayments 
                                                                                                        where trxId = @trxId))",
                                                            new SqlParameter("@trxId", inTrx.Trx_id));

                        PrintTransaction pt = new PrintTransaction(POSStatic.POSPrintersDTOList);
                        foreach (DataRow dr in dt.Rows)
                        {
                            DataTable dtLines = POSStatic.Utilities.executeDataTable(@"select * from trxSplitLines where splitId = @splitId",
                                                                    new SqlParameter("@splitId", dr["splitId"]));
                            Transaction splitTrx = pt.cloneTrx(inTrx);
                            foreach (Transaction.TransactionLine tl in splitTrx.TrxLines)
                            {
                                tl.LineValid = false;
                            }

                            foreach (DataRow drl in dtLines.Rows)
                            {
                                foreach (Transaction.TransactionLine tl in splitTrx.TrxLines)
                                {
                                    if (tl.DBLineId == Convert.ToInt32(drl["LineId"]))
                                    {
                                        tl.LineValid = true;
                                        break;
                                    }
                                }
                            }

                            clsSplit split = new clsSplit(-1, splitTrx);
                            split.reference = dr["UserReference"].ToString();
                            split.splitId = Convert.ToInt32(dr["SplitId"]);
                            split.refreshBalance();
                            splits.Add(split);
                        }
                    }
                }
            }
            log.Debug("Ends-SplitPayments(inTrx)");//Added for logger function on 08-Mar-2016
        }

        

        public SplitPayments(Transaction inTrx, bool inIsEqualSplit, int inCount)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-SplitPayments(inTrx,"+inIsEqualSplit+","+inCount+")");//Added for logger function on 08-Mar-2016
            _trx = inTrx;
            isEqualSplit = inIsEqualSplit;
            if (isEqualSplit)
            {
                createEqualSplits(inCount);
            }
            else
            {
                createSplitsByItem(inCount);
            }

            foreach (clsSplit s in splits)
                s.refreshBalance();

            log.Debug("Ends-SplitPayments(inTrx," + inIsEqualSplit + "," + inCount + ")");//Added for logger function on 08-Mar-2016
        }        

        private void UpdateDiscountAmount(Transaction.TransactionLine line, int noOfSplits)
        {
            if(line.TransactionDiscountsDTOList != null)
            {
                foreach (var transactionDiscountsDTO in line.TransactionDiscountsDTOList)
                {
                    decimal lineAmount = (decimal)line.Price * (decimal)line.quantity;
                    lineAmount = lineAmount + (decimal)(lineAmount * (decimal)line.tax_percentage / 100);
                    if(transactionDiscountsDTO.DiscountAmount == null)
                    {
                        transactionDiscountsDTO.DiscountAmount = 0;
                    }
                    transactionDiscountsDTO.DiscountAmount = transactionDiscountsDTO.DiscountAmount / noOfSplits;
                    if(lineAmount > 0)
                    {
                        transactionDiscountsDTO.DiscountPercentage = transactionDiscountsDTO.DiscountAmount / lineAmount;
                    }
                    else
                    {
                        transactionDiscountsDTO.DiscountPercentage = 0;
                    }
                }
            }
        }

        void createEqualSplits(int count)
        {
            log.Debug("Starts-createEqualSplits(" + count + ")");//Added for logger function on 08-Mar-2016
            splits.Clear();
            PrintTransaction pt = new PrintTransaction(POSStatic.POSPrintersDTOList);
            Transaction split = pt.cloneTrx(_trx);
            foreach (Transaction.TransactionLine tl in split.TrxLines)
            {
                tl.Price = tl.Price / count;
                UpdateDiscountAmount(tl, count);
            }
            
            splits.Add(new clsSplit(count, split));
            int i = count;
            while (i-- > 1)
                splits.Add(new clsSplit(count, pt.cloneTrx(split)));

            log.Debug("Ends-createEqualSplits(" + count + ")");//Added for logger function on 08-Mar-2016
        }

        void createSplitsByItem(int count)
        {
            log.Debug("Starts-createSplitsByItem(" + count + ")");//Added for logger function on 08-Mar-2016
            splits.Clear();
            int noOfSplits = count;
            PrintTransaction pt = new PrintTransaction(POSStatic.POSPrintersDTOList);
            Transaction split = pt.cloneTrx(_trx);
            splits.Add(new clsSplit(-1, split));
            while (count-- > 1)
            {
                split = pt.cloneTrx(_trx);
                foreach (Transaction.TransactionLine tl in split.TrxLines)
                {
                    if (tl.ProductTypeCode != ProductTypeValues.GRATUITY && tl.ProductTypeCode != ProductTypeValues.SERVICECHARGE)
                    {
                        tl.LineValid = false;
                    }
                }
                splits.Add(new clsSplit(-1, split));
            }
            log.Debug("Starts-createSplitsByItem(" + count + ")");//Added for logger function on 08-Mar-2016
        }

        public bool paymentExists
        {
            get
            {
                object sum = POSStatic.Utilities.executeScalar(@"select isnull(sum(amount), 0) from trxPayments where trxId = @trxId",
                                          new SqlParameter("@trxId", _trx.Trx_id));
                if (Convert.ToDecimal(sum) != 0)
                    return true;
                else
                    return false;  
            }
        }

        public void ClearSplits()
        {
            log.Debug("Starts-ClearSplits()");//Added for logger function on 08-Mar-2016
            if (paymentExists)
                throw new ApplicationException("Payment exists for transaction. Cannot clear splits.");

            POSStatic.Utilities.executeNonQuery(@"delete from trxSplitLines 
                                                    where splitId in (select splitId 
                                                                        from trxSplitPayments 
                                                                        where trxId = @trxID);
                                                    update trxPayments set splitId = null
                                                        where trxId = @trxId;
                                                    delete from trxSplitPayments 
                                                        where trxId = @trxId",
                                                    new SqlParameter("@trxId", _trx.Trx_id));
            splits.Clear();
            log.Debug("Ends-ClearSplits()");//Added for logger function on 08-Mar-2016
        }
    }
}
