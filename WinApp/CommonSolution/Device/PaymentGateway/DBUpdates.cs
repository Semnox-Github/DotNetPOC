using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;
using System.Collections;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class DBUpdates
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void UpdateRequestMessageDetails(ClsRequestMessageAttributes myreqmsgattributes)
        {
            log.LogMethodEntry(myreqmsgattributes);

            DataAccessCore dac = new DataAccessCore();
            List<SqlParameter> ParameterList = new List<SqlParameter>();
            try
            {
                ParameterList.Add(new SqlParameter("@InvoiceNo", myreqmsgattributes.InvoiceNo));
                ParameterList.Add(new SqlParameter("@RequestDatetime", ServerDateTime.Now));
                ParameterList.Add(new SqlParameter("@POSAmount", myreqmsgattributes.Purchase));
                ParameterList.Add(new SqlParameter("@TransactionType", myreqmsgattributes.TranType.ToString()));
                ParameterList.Add(new SqlParameter("@ReferenceNo", myreqmsgattributes.RefNo));
                ParameterList.Add(new SqlParameter("@Status", WSCore.CmdStatus.ToString()));
                //ParameterList.Add(new SqlParameter("@Result", SqlDbType.Int));
                //return dac.sqlExecuteNonQueryReturnVal(structStoredProcs.SPInsertRequestMessageDetails, ParameterList, true);
                dac.ExecuteSP(structStoredProcs.SPInsertRequestMessageDetails, ParameterList);

                log.LogVariableState("@InvoiceNo", myreqmsgattributes.InvoiceNo);
                log.LogVariableState("@RequestDatetime", ServerDateTime.Now);
                log.LogVariableState("@POSAmount", myreqmsgattributes.Purchase);
                log.LogVariableState("@TransactionType", myreqmsgattributes.TranType.ToString());
                log.LogVariableState("@ReferenceNo", myreqmsgattributes.RefNo);
                log.LogVariableState("@Status", WSCore.CmdStatus.ToString());

            }
            catch (Exception ex)
            {
                log.Error("Error occured while Updating Request Message Details", ex);
                log.LogMethodExit(null, "Throwing Exception" + ex);
                throw ex;
            }

            log.LogMethodExit(null);
        }

        public DataSet GetInvoiceAmounts(string InvoiceNo)
        {
            log.LogMethodEntry(InvoiceNo);

            DataSet ds = new DataSet();
            DataAccessCore dac = new DataAccessCore();
            List<SqlParameter> ParameterList = new List<SqlParameter>();
            try
            {
                ParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
                ds = dac.ExecuteSP(structStoredProcs.SPGetInvoiceAppovedAmounts, ParameterList);
                log.LogVariableState("@InvoiceNo", InvoiceNo);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Getting Invoice Amounts", ex);
                log.LogMethodExit(null, "Throwing Exception");
                throw ex;
            }

            log.LogMethodExit(ds);
            return ds;
        }

        public DataSet GetInvoiceAmounts(ClsResponseMessageAttributes myreqmsgattributes)
        {
            log.LogMethodEntry(myreqmsgattributes);

            DataSet returnValueNew = GetInvoiceAmounts(myreqmsgattributes.InvoiceNo);

            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        //class changed
        public List<ClsRequestMessageAttributes> GetInvDetforReturnVoidSale(object ResponseId)
        {
            log.LogMethodEntry(ResponseId);

            //class changed
            List<ClsRequestMessageAttributes> list = new List<ClsRequestMessageAttributes>();
            DataSet myDS = new DataSet();
            DataAccessCore dac = new DataAccessCore();
            List<SqlParameter> ParameterList = new List<SqlParameter>();
            try
            {
                ParameterList.Add(new SqlParameter("@ResponseId", ResponseId));

                log.LogVariableState("@ResponseId", ResponseId);

                myDS = dac.ExecuteSP(structStoredProcs.SPGetVoidSaleReturnInvoiceDetails, ParameterList);
                DataTable dtInv = myDS.Tables[0];
                foreach (DataRow row in dtInv.Rows)
                {
                    //class changed
                    ClsRequestMessageAttributes myvoidsalemsgattributes = new ClsRequestMessageAttributes();
                    //myvoidsalemsgattributes.TranType = (CardType)Enum.Parse(typeof(CardType), row["TransactionType"].ToString());
                    myvoidsalemsgattributes.InvoiceNo = row["InvoiceNo"].ToString();
                    myvoidsalemsgattributes.RefNo = row["RefNo"].ToString();
                    myvoidsalemsgattributes.RecordNo = row["RecordNo"].ToString();
                    myvoidsalemsgattributes.Purchase = row["Authorize"].ToString();
                    myvoidsalemsgattributes.AuthCode = row["AuthCode"].ToString();
                    myvoidsalemsgattributes.CardType = row["CardType"].ToString();
                    myvoidsalemsgattributes.TranCode = row["TranCode"].ToString();
                    myvoidsalemsgattributes.AcctNo = row["AcctNo"].ToString();
                    myvoidsalemsgattributes.AcqRefData = row["AcqRefData"].ToString();
                    myvoidsalemsgattributes.ProcessData = row["ProcessData"].ToString();
                    list.Add(myvoidsalemsgattributes);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Getting Invoice Det for Return Void Sale", ex);
                log.LogMethodExit(null, "Throwing Exception");
                throw  ex;
            }

            log.LogMethodExit(list);
            return list;
        }

        public void UpdateResponseMessageDetails(ClsResponseMessageAttributes objResponseMessage)
        {
            log.LogMethodEntry(objResponseMessage);

            DataAccessCore dac = new DataAccessCore();
            List<SqlParameter> ParameterList = new List<SqlParameter>();
            //ParameterList.Add(new SqlParameter("@TokenID", objResponseMessage.DSIXReturnCode));
            ParameterList.Add(new SqlParameter("@MerchantID", objResponseMessage.MerchantID));
            ParameterList.Add(new SqlParameter("@RecordNo", objResponseMessage.RecordNo));
            ParameterList.Add(new SqlParameter("@DSIXReturnCode", objResponseMessage.DSIXReturnCode));
            ParameterList.Add(new SqlParameter("@CmdStatus", objResponseMessage.CmdStatus));
            ParameterList.Add(new SqlParameter("@TextResponse", objResponseMessage.TextResponse));
            ParameterList.Add(new SqlParameter("@AcctNo", objResponseMessage.AcctNo));
            ParameterList.Add(new SqlParameter("@CardType", objResponseMessage.CardType));
            ParameterList.Add(new SqlParameter("@TranCode", objResponseMessage.TranCode));
            ParameterList.Add(new SqlParameter("@RefNo", objResponseMessage.RefNo));
            ParameterList.Add(new SqlParameter("@InvoiceNo", objResponseMessage.InvoiceNo));
            ParameterList.Add(new SqlParameter("@Purchase", objResponseMessage.Purchase));
            ParameterList.Add(new SqlParameter("@Authorize", objResponseMessage.Authorize));
            ParameterList.Add(new SqlParameter("@TransactionDatetime", ServerDateTime.Now));
            ParameterList.Add(new SqlParameter("@AuthCode", objResponseMessage.AuthCode));
            ParameterList.Add(new SqlParameter("@ProcessData", objResponseMessage.ProcessData));
            ParameterList.Add(new SqlParameter("@ResponseOrigin", objResponseMessage.ResponseOrigin));
            ParameterList.Add(new SqlParameter("@UserTraceData", objResponseMessage.UserTraceData));
            ParameterList.Add(new SqlParameter("@CaptureStatus", objResponseMessage.CaptureStatus));
            ParameterList.Add(new SqlParameter("@AcqRefData", objResponseMessage.AcqRefData));
            ParameterList.Add(new SqlParameter("@Result", -1));
            ParameterList.Add(new SqlParameter("@TipAmount", objResponseMessage.Gratuity));//Modification on 09-Nov-2015:Tip Feature

            log.LogVariableState("@MerchantID", objResponseMessage.MerchantID);
            log.LogVariableState("@RecordNo", objResponseMessage.RecordNo);
            log.LogVariableState("@DSIXReturnCode", objResponseMessage.DSIXReturnCode);
            log.LogVariableState("@CmdStatus", objResponseMessage.CmdStatus);
            log.LogVariableState("@TextResponse", objResponseMessage.TextResponse);
            log.LogVariableState("@AcctNo", objResponseMessage.AcctNo);
            log.LogVariableState("@CardType", objResponseMessage.CardType);
            log.LogVariableState("@TranCode", objResponseMessage.TranCode);
            log.LogVariableState("@RefNo", objResponseMessage.RefNo);
            log.LogVariableState("@InvoiceNo", objResponseMessage.InvoiceNo);
            log.LogVariableState("@Purchase", objResponseMessage.Purchase);
            log.LogVariableState("@Authorize", objResponseMessage.Authorize);
            log.LogVariableState("@TransactionDatetime", ServerDateTime.Now);
            log.LogVariableState("@AuthCode", objResponseMessage.AuthCode);
            log.LogVariableState("@ProcessData", objResponseMessage.ProcessData);
            log.LogVariableState("@ResponseOrigin", objResponseMessage.ResponseOrigin);
            log.LogVariableState("@UserTraceData", objResponseMessage.UserTraceData);
            log.LogVariableState("@CaptureStatus", objResponseMessage.CaptureStatus);
            log.LogVariableState("@AcqRefData", objResponseMessage.AcqRefData);
            log.LogVariableState("@Result", -1);
            log.LogVariableState("@TipAmount", objResponseMessage.Gratuity);

            try
            {
                objResponseMessage.ResponseId = dac.sqlExecuteNonQueryReturnVal(structStoredProcs.SPInsertResponseMessageDetails, ParameterList, true);
            }
            catch (Exception ex)
            {
                log.Error("Error when getting a valid value for ResponseId", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw ex;
            }

            log.LogMethodExit(null);
        }
    }
}
