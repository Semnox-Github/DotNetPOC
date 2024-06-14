using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using IPADLib;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Threading;
//using Semnox.Webservices.PaymentGateway;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    internal static class WSCore
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, string> CardTrackDetails = new Dictionary<string, string>();
        public static enumCommandStatus CmdStatus;
        public delegate void MyWScoreHandler(ErrorResponseEventArgs responseeventargs);
        public static event MyWScoreHandler OnWScoreEventReturn;
        public static bool _isFinished = false;
        //public static string DBConnectionString;
        public static Utilities _utilities;

        public static ClsResponseMessageAttributes PrepareforInvokingWS(CardType Trantype, ClsRequestMessageAttributes myreqmsgattributes)
        {
            log.LogMethodEntry(Trantype, myreqmsgattributes);

            IRequestCreateMessage reqMessage = null;

            if (Trantype == CardType.Credit)
            {
                reqMessage = new ClsRequestCreditMessageCreation();
            }
            else if (Trantype == CardType.Debit)
            {
                reqMessage = new ClsRequestDebitMessageCreation();
            }

            ClsResponseMessageAttributes objResponseMessage = new ClsResponseMessageAttributes();

            try
            {
                string requeststring = reqMessage.CreaterequestMessage(myreqmsgattributes);
                string responsexmlmsg = null;
                DBUpdates dbupdate = new DBUpdates();
                try
                {
                    dbupdate.UpdateRequestMessageDetails(myreqmsgattributes);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred when updating Request Message Details", ex);
                    CmdStatus = enumCommandStatus.DBResponseError;
                    objResponseMessage.ErrorMessage = ex.Message;

                    log.LogMethodExit(objResponseMessage);
                    return objResponseMessage;
                }

                OnWScoreEventReturn(new ErrorResponseEventArgs(PGSEMessages.PGWSInvoke, null));
                responsexmlmsg = InvokeWS(requeststring);

                if (Environment.MachineName.StartsWith("IQBAL"))
                {
                    DataAccessCore dac = new DataAccessCore();
                    List<System.Data.SqlClient.SqlParameter> lst = new List<System.Data.SqlClient.SqlParameter>();
                    lst.Add(new System.Data.SqlClient.SqlParameter("@request", requeststring));
                    lst.Add(new System.Data.SqlClient.SqlParameter("@response", responsexmlmsg));
                    dac.sqlExecuteNonQuery("insert into CCRequestResponseXML (timestamp, request, response) values (getdate(), @request, @response)", lst);

                    log.LogVariableState("@request", requeststring);
                    log.LogVariableState("@response", responsexmlmsg);
                }

                if (string.IsNullOrEmpty(responsexmlmsg) == true)
                {
                    CmdStatus = enumCommandStatus.WSError;
                    WSUnsuccesfulUpdateDB(myreqmsgattributes);
                    objResponseMessage.CmdStatus = enumCommandStatus.WSError.ToString();
                    objResponseMessage.ErrorMessage = PGSEMessages.PGWSError;
                }
                else
                {
                    OnWScoreEventReturn(new ErrorResponseEventArgs(PGSEMessages.PGSuccess, null));
                    ClasResponseValidator objresponsevalidator = new ClasResponseValidator();
                    objResponseMessage = objresponsevalidator.ValidateIncomingResponseMessage(myreqmsgattributes, responsexmlmsg);
                    CmdStatus = (enumCommandStatus)Enum.Parse(typeof(enumCommandStatus), objResponseMessage.CmdStatus);
                    OnWScoreEventReturn(new ErrorResponseEventArgs(PGSEMessages.PGDBResMsgInsert, null));
                    try
                    {
                        dbupdate.UpdateResponseMessageDetails(objResponseMessage);
                    }
                    catch(Exception ex)
                    {
                        log.Error("Error occurred when updating Request Message Details", ex);
                        CmdStatus = enumCommandStatus.DBResponseError;
                        objResponseMessage.CmdStatus = enumCommandStatus.DBResponseError.ToString();
                        objResponseMessage.ErrorMessage = PGSEMessages.PGDBResponseError;
                    }
                    OnWScoreEventReturn(new ErrorResponseEventArgs(objResponseMessage.CmdStatus + " : " + objResponseMessage.TextResponse, objResponseMessage));
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while object Response Message", ex);
                CmdStatus = enumCommandStatus.WSError;
                WSUnsuccesfulUpdateDB(myreqmsgattributes);
                objResponseMessage.CmdStatus = enumCommandStatus.WSError.ToString();
                objResponseMessage.ErrorMessage = PGSEMessages.PGWSError + ":" + ex.Message;
            }

            log.LogMethodExit(objResponseMessage);
            return objResponseMessage;
        }

        public static List<ClsRequestMessageAttributes> GetReturnAndVoidSale(object ResponseId)
        {
            log.LogMethodEntry(ResponseId);

            DBUpdates objDb = new DBUpdates();
            List<ClsRequestMessageAttributes> list = new List<ClsRequestMessageAttributes>();
            list = objDb.GetInvDetforReturnVoidSale(ResponseId);

            log.LogMethodExit(list);
            return list;
        }

        public static List<ClsRequestMessageAttributes> GetReturnAndVoidSale(object ResponseId, string RefNo = "", string AccountNo = "", string RecordNo="")
        {
            log.LogMethodEntry(ResponseId, RefNo, AccountNo, RecordNo);

            DBUpdates objDb = new DBUpdates();
            List<ClsRequestMessageAttributes> list = new List<ClsRequestMessageAttributes>();
            list = objDb.GetInvDetforReturnVoidSale(ResponseId);

            log.LogMethodExit(list);
            return list;
        }

        public static ClsResponseMessageAttributes VoidSaleInvoice(ClsRequestMessageAttributes voidsalemsgatt, bool reversal)
        {
            log.LogMethodEntry(voidsalemsgatt, reversal);

            ClsResponseMessageAttributes objResponseMessage = new ClsResponseMessageAttributes();
            try
            {
                DBUpdates objDb = new DBUpdates();
                string strvoidsale = "";
                ClsVoidSaleMessageCreation clsvoidsalecreate = new ClsVoidSaleMessageCreation();
                strvoidsale = clsvoidsalecreate.CreateVoidSale(voidsalemsgatt, reversal);
                string responsexmlmsg = WSCore.InvokeWS(strvoidsale);

                if (Environment.MachineName.StartsWith("IQBAL"))
                {
                    DataAccessCore dac = new DataAccessCore();
                    List<System.Data.SqlClient.SqlParameter> lst = new List<System.Data.SqlClient.SqlParameter>();
                    lst.Add(new System.Data.SqlClient.SqlParameter("@request", strvoidsale));
                    lst.Add(new System.Data.SqlClient.SqlParameter("@response", responsexmlmsg));
                    dac.sqlExecuteNonQuery("insert into CCRequestResponseXML (timestamp, request, response) values (getdate(), @request, @response)", lst);

                    log.LogVariableState("@request", strvoidsale);
                    log.LogVariableState("@response", responsexmlmsg);
                }

                if (string.IsNullOrEmpty(responsexmlmsg) == true)
                {
                    CmdStatus = enumCommandStatus.WSError;
                }
                else
                {
                    ClasResponseValidator objresponsevalidator = new ClasResponseValidator();
                    OnWScoreEventReturn(new ErrorResponseEventArgs(PGSEMessages.PGSuccess, null));
                    objResponseMessage = objresponsevalidator.ValidateIncomingvoidsaleMessage(voidsalemsgatt, responsexmlmsg);
                    CmdStatus = (enumCommandStatus)Enum.Parse(typeof(enumCommandStatus), objResponseMessage.CmdStatus);
                    OnWScoreEventReturn(new ErrorResponseEventArgs(PGSEMessages.PGDBResMsgInsert, null));
                    try
                    {
                        DBUpdates dbupdate = new DBUpdates();
                        dbupdate.UpdateResponseMessageDetails(objResponseMessage);
                    }
                    catch(Exception ex)
                    {
                        log.Error("Error occurred during Void Sale Invoice", ex);
                        CmdStatus = enumCommandStatus.DBResponseError;
                        objResponseMessage.CmdStatus = enumCommandStatus.DBResponseError.ToString();
                        objResponseMessage.ErrorMessage = PGSEMessages.PGDBResponseError;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occurred during Void Sale Invoice", ex);
                CmdStatus = enumCommandStatus.WSError;
                WSCore.WSUnsuccesfulUpdateDB(voidsalemsgatt);
                objResponseMessage.CmdStatus = enumCommandStatus.WSError.ToString();
                objResponseMessage.ErrorMessage = PGSEMessages.PGWSError;
            }

            log.LogMethodExit(objResponseMessage);
            return objResponseMessage;
        }

        private static void WSUnsuccesfulUpdateDB(ClsRequestMessageAttributes myreqmsgattributes)
        {
            log.LogMethodEntry(myreqmsgattributes);

            // After unsuccesful invocation of WS, update DB for the same Invoice
            try
            {
                DBUpdates dbupdate = new DBUpdates();
                WSCore.CmdStatus = enumCommandStatus.WSError;
                dbupdate.UpdateRequestMessageDetails(myreqmsgattributes);
            }
            catch(Exception ex)
            {
                log.Error("Error occurred when performing DB update",ex);
                CmdStatus = enumCommandStatus.DBRequestError;
            }

            log.LogMethodExit(null);

        }

        public static string InvokeWS_Invoice(string InvoiceNo)
        {
            log.LogMethodEntry(InvoiceNo);

            string responsemesssage = "";
            string[] args = { ConfigurationManager.AppSettings["MerchantID"], ConfigurationManager.AppSettings["WSpassword"], InvoiceNo };
            responsemesssage = WebServiceInvoker.InvokeMethod<string>(ConfigurationManager.AppSettings["ServiceName"], ConfigurationManager.AppSettings["MethodNameInvoiceNo"], args);

            log.LogMethodExit(responsemesssage);
            return responsemesssage;
        }

        public static string InvokeWS(string requeststring)
        {
            log.LogMethodEntry(requeststring);

            string responsemesssage = "";
            int numTries = 1;
            bool failed = true;
            //wsSoapClient x = new wsSoapClient();

            for (int i = 0; i < numTries && failed; i++)
            {
                try
                {
                    string[] args = { requeststring, ConfigurationManager.AppSettings["WSpassword"] };
                    responsemesssage = WebServiceInvoker.InvokeMethod<string>(ConfigurationManager.AppSettings["ServiceName"], ConfigurationManager.AppSettings["MethodName"], args);
                    failed = false;
                }
                catch(Exception ex)
                {
                    log.Error("Unable to get a valid value for responsemesssage" , ex);

                    log.LogMethodExit(null, "Throwing Exception");
                    throw;
                }
            }

            log.LogMethodExit(responsemesssage);
            return responsemesssage;
        }

        public static void DisplayCardData(CardRequestCompleteEventArgs e)
        {
            log.LogMethodEntry(e);

            CardTrackDetails.Clear();
            if ((e.card.Track1Status == 0) && (e.card.Track1 != null))
                CardTrackDetails.Add("Track1", Encoding.ASCII.GetString(e.card.Track1));
            if ((e.card.Track2Status == 0) && (e.card.Track2 != null))
                CardTrackDetails.Add("Track2", Encoding.ASCII.GetString(e.card.Track2));
            if ((e.card.Track3Status == 0) && (e.card.Track3 != null))
                CardTrackDetails.Add("Track3", Encoding.ASCII.GetString(e.card.Track3));
            if ((e.card.EncTrack1Status == 0) && (e.card.EncTrack1 != null))
                CardTrackDetails.Add("EncTrack1", MakeHex(e.card.EncTrack1));
            if ((e.card.EncTrack2Status == 0) && (e.card.EncTrack2 != null))
                CardTrackDetails.Add("EncTrack2", MakeHex(e.card.EncTrack2));
            if ((e.card.EncTrack3Status == 0) && (e.card.EncTrack3 != null))
                CardTrackDetails.Add("EncTrack3", MakeHex(e.card.EncTrack3));
            if ((e.card.EncMPStatus == 0) && (e.card.EncMP != null))
                CardTrackDetails.Add("EncMP", MakeHex(e.card.EncMP));
            if (e.card.KSNStatus == 0)
            {
                CardTrackDetails.Add("KSN", MakeHex(e.card.KSN));
                CardTrackDetails.Add("MPSts", MakeHex(e.card.MPSts));
            }

            log.LogMethodExit(null);
        }

        private static string MakeHex(byte[] b)
        {
            log.LogMethodEntry(b);

            int len = b.Length;
            StringBuilder s = new StringBuilder(len * 2);
            for (int i = 0; i < len; i++)
            {
                s.Append(b[i].ToString("X2"));
            }

            log.LogMethodExit(s.ToString());
            return s.ToString();
        }

        public static string MakeHex(byte[] b, int start, int len)
        {
            log.LogMethodEntry(b, start, len);

            StringBuilder s = new StringBuilder(len * 2);
            for (int i = 0; i < len; i++)
            {
                s.Append(b[i + start].ToString("X2"));
            }

            log.LogMethodExit(s.ToString());
            return s.ToString();
        }
    }
}
