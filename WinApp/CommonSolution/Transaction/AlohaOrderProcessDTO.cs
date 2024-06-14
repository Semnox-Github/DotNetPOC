/********************************************************************************************
* Project Name - ParafaitExSysServer
* Description  - FindOrderResponseDTO
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
 *2.140.6     29-May-2023    Deeksha              Created as part of Aloha BSP Enhancements
*********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// AlohaOrderProcessDTO
    /// </summary>
    public class AlohaOrderProcessDTO
    {
        private int siteId;
        private int trxId;
        private TransactionDTO transactionDTO;
        private DateTime trxDate;
        private int requestId;
        private int programId;
        private DateTime requestEndTime =  DateTime.MinValue;
        private bool isSubscribed = false;
        private ExecutionContext executionContext;
        private List<KeyValuePair<string, string>> bspConfigValues = new List<KeyValuePair<string, string>>();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="trxId"></param>
        /// <param name="trxDate"></param>
        /// <param name="transactionDTO"></param>
        public AlohaOrderProcessDTO(int siteId, int trxId, DateTime trxDate, TransactionDTO transactionDTO)
        {
            log.LogMethodEntry(siteId, trxId, trxDate, transactionDTO);
            this.siteId = siteId;
            this.trxId = trxId;
            this.trxDate = trxDate;
            this.transactionDTO = transactionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// SiteId
        /// </summary>
        public int SiteId { get { return siteId; }  set { siteId = value; } }

        /// <summary>
        /// TrxId
        /// </summary>
        public List<KeyValuePair<string,string>> BSPConfigValues { get { return bspConfigValues; } set { bspConfigValues = value; } }

        /// <summary>
        /// RequestEndTime
        /// </summary>
        public DateTime RequestEndTime { get { return requestEndTime; } set { requestEndTime = value; } }

        /// <summary>
        /// IsSubscribed
        /// </summary>
        public bool IsSubscribed { get { return isSubscribed; } set { isSubscribed = value; } }

        /// <summary>
        /// RequestId
        /// </summary>
        public int RequestId { get { return requestId; } set { requestId = value; } }

        /// <summary>
        /// RequestId
        /// </summary>
        public ExecutionContext SiteExecutionContext { get { return executionContext; } set { executionContext = value; } }

        /// <summary>
        /// TransactionDTO
        /// </summary>
        public TransactionDTO TransactionDTO { get { return transactionDTO; } set { transactionDTO = value; } }

        /// <summary>
        /// ProgramId
        /// </summary>
        public int ProgramId { get { return programId; } set { programId = value; } }


        /// <summary>
        /// TrxDate
        /// </summary>
        public DateTime TrxDate { get { return trxDate; } set { trxDate = value; } }
    }

}