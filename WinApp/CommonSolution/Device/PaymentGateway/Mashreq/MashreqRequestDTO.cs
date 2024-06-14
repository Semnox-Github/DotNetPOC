/******************************************************************************************************
 * Project Name - Device
 * Description  - Mashreq Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By                 Remarks          
 ******************************************************************************************************
 *2.150.0     26-July-2022    Prasad, Dakshakh Raj       Mashreq Payment gateway integration
 ********************************************************************************************************/

 namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class MashreqRequestDTO
    {
        public string CommandType { get; set; }
        public string transactionAmount { get; set; }
        public string mrefValue { get; set; }
        public string authCode { get; set; }
        public string invoiceNumber { get; set; }
        public string deviceId { get; set; }
        public string posId { get; set; }
        public string tipAmount { get; set; }
        public string paymentId { get; set; }
        public int trnxType { get; set; }
    }
}
