/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data transfer object of Refund Card request
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.150.2     22-Feb-2023      Abhishek            Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Data transfer object of Refund Card Object
    /// </summary>
    public class RefundCardDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public RefundCardDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public RefundCardDTO(List<int> accountIdList, double cardDeposit, double credits, double creditPlus, string remarks, string message, bool makeNewOnFullRefund)
        {
            log.LogMethodEntry(accountIdList, cardDeposit, credits, creditPlus, remarks, message, makeNewOnFullRefund);
            AccountIdList = accountIdList;
            CardDeposit = cardDeposit;
            MakeNewOnFullRefund = makeNewOnFullRefund;
            Credits = credits;
            CreditPlus = creditPlus;
            Remarks = remarks;
            Message = message;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of AccountIdList field
        /// </summary>
        public List<int> AccountIdList { get; set; }

        /// <summary>
        /// Get/Set method of CardDeposit field
        /// </summary>
        public double CardDeposit { get; set; }

        /// <summary>
        /// Get/Set method of Credits field
        /// </summary>
        public double Credits { get; set; }

        /// <summary>
        /// Get/Set method of CreditPlus field
        /// </summary>
        public double CreditPlus { get; set; }

        /// <summary>
        /// Get/Set method of RefundValue field
        /// </summary>
        public decimal RefundValue { get; set; }

        /// <summary>
        /// Get/Set method of MakeNewOnFullRefund field
        /// </summary>
        public bool MakeNewOnFullRefund { get; set; }

        /// <summary>
        /// Get/Set method of Remarks field
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Get/Set method of Message field
        /// </summary>
        public string Message { get; set; }
    }
}
