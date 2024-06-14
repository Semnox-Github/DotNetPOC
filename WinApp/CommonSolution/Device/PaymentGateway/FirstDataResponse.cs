using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// First Data Respose Class
    /// </summary>
    public class FirstDataResponse
    {
        /// <summary>
        /// string ReferenceNo
        /// </summary>
        public string ReferenceNo = "";
        /// <summary>
        /// string responseCode
        /// </summary>
        public string responseCode = "";
        /// <summary>
        /// string responseMessage
        /// </summary>
        public string responseMessage = "";
        /// <summary>
        /// string responseAuthId 
        /// </summary>
        public string responseAuthId = "";
        /// <summary>
        /// string CardNo
        /// </summary>
        public string CardNo = "";
        /// <summary>
        /// string ccResponseId
        /// </summary>
        public string ccResponseId = "";
        /// <summary>
        /// string CardName
        /// </summary>
        public string CardName = "";
        /// <summary>
        /// double TransAmount
        /// </summary>
        public double TransAmount;
        /// <summary>
        /// string CardExpiryDate
        /// </summary>
        public string CardExpiryDate;
        /// <summary>
        /// Receipt Text
        /// </summary>
        public string ReceiptText;
    }
}
