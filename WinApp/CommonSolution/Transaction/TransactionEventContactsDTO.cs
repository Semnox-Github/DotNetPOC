/********************************************************************************************
 * Project Name - Transaction
 * Description  - API for the Payment Link
 * 
 **************
 **Version Log
 **************
 *Version     Date         Modified By           Remarks          
 *********************************************************************************************
 *2.100       19-Sep-2020      Girish Kundar        Created 
 *2.130.7     13-Apr-2022      Guru S A             Payment mode OTP validation changes
 ********************************************************************************************/

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  TransactionEventContactsDTO holds the external contact info that is applicable for the transaction
    /// </summary>
    public class TransactionEventContactsDTO
    { 
        /// <summary>
        /// MessageChannel EMAIL or SMS
        /// </summary>
        public TransactionPaymentLink.MessageChannel MessageChannel { get; set; }
        /// <summary>
        /// Transaction Id  
        /// </summary>
        public int TransactionId { get; set; }
        /// <summary>
        /// Email Id 
        /// </summary>
        public string EmailId { get; set; }
        /// <summary>
        /// PhoneNumber  
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// OTPValue
        /// </summary>
        public string OTPValue { get; set; }
        /// <summary>
        /// OTPValue
        /// </summary>
        public string OTPGameCard { get; set; }
    }
}
