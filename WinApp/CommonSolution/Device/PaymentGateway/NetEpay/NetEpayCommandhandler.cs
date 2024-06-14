using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class NetEpayCommandhandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DSIEMVXLib.DsiEMVX dsiEMVX;
        bool isManual;
        public NetEpayRequestDTO CreatePaymentAdjustRequest(NetEpayRequestDTO netEpayRequestDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO, double amount, double tips, bool capture = false)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(String.Format("{0:0.00}", amount), null, String.Format("{0:0.00}", tips), null, null);
            //NetEpayAccount Account = new NetEpayAccount();
            //Amount.Purchase = String.Format("{0:0.00}", amount);
            //Amount.Gratuity = String.Format("{0:0.00}", tips);
            netEpayRequestDTO.TranType = "Credit";
            if (!capture)
            {
                netEpayRequestDTO.TranCode = "PreAuthCaptureByRecordNo";
            }
            else
            {
                netEpayRequestDTO.TranCode = "AdjustByRecordNo";
            }
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.RefNo = cCTransactionsPGWDTO.RefNo;
            netEpayRequestDTO.AuthCode = cCTransactionsPGWDTO.AuthCode;
            netEpayRequestDTO.RecordNo = cCTransactionsPGWDTO.TokenID;
            netEpayRequestDTO.Frequency = "OneTime";
            netEpayRequestDTO.AcqRefData = cCTransactionsPGWDTO.AcqRefData;
            netEpayRequestDTO.ForceOffline = "N";
            netEpayRequestDTO.MaxTransactions = "0";
            netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";

            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }

        public NetEpayRequestDTO CreatePaymentRequest(NetEpayRequestDTO netEpayRequestDTO, double amount)
        {
            log.LogMethodEntry(netEpayRequestDTO);

           // dsiEMVX = new DSIEMVXLib.DsiEMVX();
            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(String.Format("{0:0.00}", amount), null, "0", null, null);
            //NetEpayAccount Account = new NetEpayAccount();
            //Amount.Purchase = String.Format("{0:0.00}", amount);
            //Amount.Gratuity = "0";
            netEpayRequestDTO.TranCode = "EMVSale";
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.PartialAuth = "Allow";
            netEpayRequestDTO.RecordNo = "RecordNumberRequested";

            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }

        public NetEpayRequestDTO CreatePreAuthRequest(NetEpayRequestDTO netEpayRequestDTO, double amount, bool zeroAuth = false)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(String.Format("{0:0.00}", amount), String.Format("{0:0.00}", amount), null, null, null);
            //NetEpayAccount Account = new NetEpayAccount();
            if (zeroAuth)
            {
                netEpayRequestDTO.TranCode = "PreAuthByRecordNo";
            }
            else
            {
                netEpayRequestDTO.TranCode = "EMVPreAuth";
            }
            //Amount.Purchase = String.Format("{0:0.00}", amount);
            //Amount.Authorize = String.Format("{0:0.00}", amount);
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.OKAmount = "Disallow";
            netEpayRequestDTO.Duplicate = "None";
            netEpayRequestDTO.PartialAuth = "Allow";
            netEpayRequestDTO.RecordNo = "RecordNumberRequested";
            netEpayRequestDTO.Frequency = "OneTime";
            netEpayRequestDTO.DisplayTextHandle = "00101484";
            netEpayRequestDTO.ReturnCustomerData = "Payment";
            netEpayRequestDTO.ForceOffline = "N";
            netEpayRequestDTO.MaxTransactions = "0";
            netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";


            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }

        public NetEpayRequestDTO CreateZeroAuthRequest(NetEpayRequestDTO netEpayRequestDTO, double amount)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            // dsiEMVX = new DSIEMVXLib.DsiEMVX();
            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(String.Format("{0:0.00}", amount), null, null, null, null);
            //NetEpayAccount Account = new NetEpayAccount();
            //Amount.Purchase = String.Format("{0:0.00}", amount);
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.TranCode = "EMVZeroAuth";
            netEpayRequestDTO.OKAmount = "Disallow";
            netEpayRequestDTO.PartialAuth = "PartialAuth";
            netEpayRequestDTO.RecordNo = "RecordNumberRequested";
            netEpayRequestDTO.Frequency = "OneTime";
            netEpayRequestDTO.DisplayTextHandle = "00101484";
            //netEpayRequestDTO.ReturnCustomerData = "NoPayment";
            
            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }

        public NetEpayRequestDTO CreateRVoidRequest(NetEpayRequestDTO netEpayRequestDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO, double amount, double tip)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            // dsiEMVX = new DSIEMVXLib.DsiEMVX();
            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(String.Format("{0:0.00}", amount), null, String.Format("{0:0.00}", tip), null, null);
            //NetEpayAccount Account = new NetEpayAccount();
            //Amount.Purchase = String.Format("{0:0.00}", amount);
            //Amount.Gratuity = String.Format("{0:0.00}", tip);
            netEpayRequestDTO.TranCode = "VoidSaleByRecordNo";
            netEpayRequestDTO.RefNo = cCTransactionsPGWDTO.RefNo;
            netEpayRequestDTO.AuthCode = cCTransactionsPGWDTO.AuthCode;
            netEpayRequestDTO.RecordNo = cCTransactionsPGWDTO.TokenID;
            netEpayRequestDTO.Frequency = "OneTime";
            netEpayRequestDTO.AcqRefData = cCTransactionsPGWDTO.AcqRefData;
            netEpayRequestDTO.ForceOffline = "N";
            netEpayRequestDTO.MaxTransactions = "0";
            netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";
            netEpayRequestDTO.Amount = Amount;

            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }
        public NetEpayRequestDTO CreateReturnRequest(NetEpayRequestDTO netEpayRequestDTO, double amount)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            // dsiEMVX = new DSIEMVXLib.DsiEMVX();
            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(String.Format("{0:0.00}", amount), null, null, null, null);
            //NetEpayAccount Account = new NetEpayAccount();
            //Amount.Purchase = String.Format("{0:0.00}", amount);
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.TranCode = "EMVPreAuth";
            netEpayRequestDTO.CollectData = "CardholderName";
            netEpayRequestDTO.Duplicate = "None";
            netEpayRequestDTO.OKAmount = "Disallow";
            netEpayRequestDTO.PartialAuth = "Allow";
            netEpayRequestDTO.RecordNo = "EMVReturn";
            netEpayRequestDTO.Frequency = "OneTime";
            netEpayRequestDTO.DisplayTextHandle = "00101484";
            netEpayRequestDTO.MaxTransactions = "0";
            netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";

            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }

        public bool isManualEntry(NetEpayRequestDTO isManualCheck, Utilities utilities)
        {
            log.LogMethodEntry();

            isManual = false;
            isManualCheck.TranCode = "GetAnswer";
            isManualCheck.Question = utilities.MessageUtils.getMessage(1474);
            isManualCheck.QuestionTimeout = "60";
            isManualCheck.AnswerYes = "Yes";
            isManualCheck.AnswerNo = "No";
            dsiEMVX = new DSIEMVXLib.DsiEMVX();
            string resultString = string.Empty;
            TStream tStream = new TStream(isManualCheck, null);
            tStream.Transaction = isManualCheck;

            XmlSerializer requestXml = new XmlSerializer(tStream.GetType());
            MemoryStream requestStream = new MemoryStream();

            requestXml.Serialize(requestStream, tStream);
            requestStream.Position = 0;
            using (StreamReader reader = new StreamReader(requestStream))
            {
                String requestString = reader.ReadToEnd();
                resultString = dsiEMVX.ProcessTransaction(requestString);
            }

            XElement printData = XElement.Parse(resultString).Element("PrintData");
            RStream rStream = new RStream();
            XmlSerializer serializer = new XmlSerializer(rStream.GetType());
            using (StringReader sr = new StringReader(resultString))
            {
                rStream = (RStream)serializer.Deserialize(sr);
            }
            if (rStream.CmdResponse.CmdStatus.Equals("Success"))
            {
                isManual = true;
            }
            log.LogMethodExit(isManual);
            return isManual;
        }

    }
}
