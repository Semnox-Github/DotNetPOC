/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the add payment details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   M S Shreyas             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Semnox.Parafait.ThirdParty.External 
{
    public class ExternalAddPaymentDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Get/Set for PaymentId
        /// </summary>
        public int PaymentId { get; set; }
        /// <summary>
        /// Get/Set for PaymentModeId
        /// </summary>
        public int PaymentModeId { get; set; }
        /// <summary>
        /// Get/Set for Amount
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Get/Set for PaymentDate
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// Get/Set for Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Get/Set for CreditCard
        /// </summary>
        public CreditCard CreditCard { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExternalAddPaymentDTO()
        {
            log.LogMethodEntry();
            Type = String.Empty; ;
            PaymentId = -1;
            PaymentModeId = -1;
            PaymentDate = null ; //date
            Remarks = String.Empty; ;
            CreditCard = new CreditCard() ;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameter
        /// </summary>
        public ExternalAddPaymentDTO(string type, int paymentId, int paymentModeId, decimal? amount, DateTime? paymentDate, string remarks)
        {
            log.LogMethodEntry(type, paymentId, paymentModeId, amount, paymentDate, remarks);
            this.Type = type;
            this.PaymentId = paymentId;
            this.PaymentModeId = PaymentModeId;
            this.Amount = amount;
            this.PaymentDate = paymentDate;
            this.Remarks = remarks;
            log.LogMethodExit();
        }
    }
}
