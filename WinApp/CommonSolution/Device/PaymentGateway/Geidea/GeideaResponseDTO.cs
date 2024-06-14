/******************************************************************************************************
 * Project Name - Device
 * Description  - Geidea Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.150.0     26-July-2022    Dakshakh Raj   Geidea Payment gateway integration
 ********************************************************************************************************/

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class GeideaResponseDTO
    {
        private string panNo;
        private decimal purAmount;
        private decimal addtlAmount;
        private string stanNo;
        private string trxDateTime;
        private string cardExpDate;
        private string trxRrn;
        private string authCode;
        private string responseCode;
        private string terminalId;
        private string schemeId;
        private string merchantId;
        private string ecrRefNo;
        private string versionNumber;
        private string outReciept;
        private string outRspLength;
        private string responseText;


        public GeideaResponseDTO(string panNo, decimal purAmount, decimal addtlAmount, string stanNo, string trxDateTime, string cardExpDate, string trxRrn, string authCode,
                            string responseCode, string terminalId, string schemeId, string merchantId, string ecrRefNo, string versionNumber, string outReciept,
                            string outRspLength, string responseText)
        {
            this.panNo = panNo;
            this.purAmount = purAmount;
            this.addtlAmount = addtlAmount;
            this.stanNo = stanNo;
            this.trxDateTime = trxDateTime;
            this.cardExpDate = cardExpDate;
            this.trxRrn = trxRrn;
            this.authCode = authCode;
            this.responseCode = responseCode;
            this.terminalId = terminalId;
            this.schemeId = schemeId;
            this.merchantId = merchantId;
            this.ecrRefNo = ecrRefNo;
            this.versionNumber = versionNumber;
            this.outReciept = outReciept;
            this.outRspLength = outRspLength;
            this.responseText = responseText;
        }

        public string PanNo { get { return panNo; } set { panNo = value; } }
        public decimal PurAmount { get { return purAmount; } set { purAmount = value; } }
        public decimal AddtlAmount { get { return addtlAmount; } set { addtlAmount = value; } }
        public string StanNo { get { return stanNo; } set { stanNo = value; } }
        public string TrxDateTime { get { return trxDateTime; } set { trxDateTime = value; } }
        public string CardExpDate { get { return cardExpDate; } set { cardExpDate = value; } }
        public string TrxRrn { get { return trxRrn; } set { trxRrn = value; } }
        public string AuthCode { get { return authCode; } set { authCode = value; } }
        public string ResponseCode { get { return responseCode; } set { responseCode = value; } }
        public string TerminalId { get { return terminalId; } set { terminalId = value; } }
        public string SchemeId { get { return schemeId; } set { schemeId = value; } }
        public string MerchantId { get { return merchantId; } set { merchantId = value; } }
        public string EcrRefNo { get { return ecrRefNo; } set { ecrRefNo = value; } }
        public string VersionNumber { get { return versionNumber; } set { versionNumber = value; } }
        public string OutReciept { get { return outReciept; } set { outReciept = value; } }
        public string OutRspLength { get { return outRspLength; } set { outRspLength = value; } }
        public string ResponseText { get { return responseText; } set { responseText = value; } }
    }
}
