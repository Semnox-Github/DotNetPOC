/********************************************************************************************
 * Project Name - Fiscalization
 * Description  - Data object of FiscalizationReprocessDTO  
 *  
 **************
 **Version Log
 **************
 *Version          Date          Modified By         Remarks          
 *********************************************************************************************
 *2.155.1.0        14-Aug-2023   Guru S A            Created for chile fiscalization
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// FiscalizationReprocessDTO
    /// </summary>
    public class FiscalizationReprocessDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         
        private string fiscalization;
        private int transactionId; 
        private int concurrentRequestId; 

        /// <summary>
        /// FiscalizationReprocessDTO
        /// </summary>
        public FiscalizationReprocessDTO()
        {
            log.LogMethodEntry();
            transactionId = -1;
            concurrentRequestId = -1;
            log.LogMethodExit();
        }
        /// <summary> 
        /// FiscalizationReprocessDTO
        /// </summary> 
        public FiscalizationReprocessDTO(string fiscalization, int transactionId, int concurrentRequestId)
        {
            log.LogMethodEntry(fiscalization, transactionId, concurrentRequestId);
            this.fiscalization = fiscalization;
            this.transactionId = transactionId; 
            this.concurrentRequestId = concurrentRequestId; 
            log.LogMethodExit();
        }
        /// <summary> 
        /// FiscalizationReprocessDTO
        /// </summary> 
        public FiscalizationReprocessDTO(FiscalizationReprocessDTO fiscDTO)
        {
            log.LogMethodEntry(fiscDTO);
            this.fiscalization = fiscDTO.Fiscalization;
            this.transactionId = fiscDTO.TransactionId; 
            this.concurrentRequestId = fiscDTO.ConcurrentRequestId; 
            log.LogMethodExit();
        }
        /// <summary>
        /// Fiscalization
        /// </summary>
        public string Fiscalization
        {
            set { fiscalization = value; }
            get { return fiscalization; }
        }
        /// <summary>
        /// TransactionId
        /// </summary>
        public int TransactionId
        {
            set { transactionId = value; }
            get { return transactionId; }
        }
        /// <summary>
        /// ConcurrentRequestId
        /// </summary>
        public int ConcurrentRequestId
        {
            set { concurrentRequestId = value; }
            get { return concurrentRequestId; }
        } 
    }
}
