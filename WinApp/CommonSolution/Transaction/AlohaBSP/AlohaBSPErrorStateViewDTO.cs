/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of AlohaBSPErrorStateViewDTO
 *
 **************
 ** Version Log
  **************
  * Version     Date            Modified By            Remarks
 *********************************************************************************************
 *2.160.0       07-Feb-2023     Deeksha                Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the ExSysSynchLog data object class. This acts as data holder for the ExSysSynchLog business object
    /// </summary>
    public class AlohaBSPErrorStateViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private int logId;
        private int concurrentRequestId;
        private DateTime timestamp;
        private string parafaitObjectGuid;
        private string status;
        private int siteId;
        private int storeCode;
        private int trxId;
        private decimal trxAmount;
        private decimal bspTrxAmount;
        private decimal paidAmount;
        private string bspID;
        private DateTime trxDate;

        public AlohaBSPErrorStateViewDTO(int logId, int concurrentRequestId, DateTime timestamp,
                                        string parafaitObjectGuid, string status, int siteId, int storeCode,
                                        int trxId, decimal trxAmount, decimal bspTrxAmount, decimal paidAmount,
                                        string bspID, DateTime trxDate)
        {
            log.LogMethodEntry();
            this.logId = logId;
            this.concurrentRequestId = concurrentRequestId;
            this.timestamp = timestamp;
            this.parafaitObjectGuid = parafaitObjectGuid;
            this.status = status;
            this.siteId = siteId;
            this.storeCode = storeCode;
            this.trxId = trxId;
            this.trxAmount = trxAmount;
            this.bspTrxAmount = bspTrxAmount;
            this.paidAmount = paidAmount;
            this.bspID = bspID;
            this.trxDate = trxDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the storeCode  field
        /// </summary>
        public string BSPId
        {
            get { return bspID; }
            set { bspID = value; }
        }

        /// <summary>
        /// Get/Set method of the storeCode  field
        /// </summary>
        public int SiteCode
        {
            get { return storeCode; }
            set { storeCode = value; }
        }

        /// <summary>
        /// Get/Set method of the TrxId  field
        /// </summary>
        public int TrxId
        {
            get { return trxId; }
            set { trxId = value; }
        }

        /// <summary>
        /// Get/Set method of the TrxAmount  field
        /// </summary>
        public decimal TrxAmount
        {
            get { return trxAmount; }
            set { trxAmount = value; }
        }

        /// <summary>
        /// Get/Set method of the BSPTrxAmount  field
        /// </summary>
        public decimal BSPTrxAmount
        {
            get { return bspTrxAmount; }
            set { bspTrxAmount = value; }
        }

        /// <summary>
        /// Get/Set method of the PaidAmount  field
        /// </summary>
        public decimal PaidAmount
        {
            get { return paidAmount; }
            set { paidAmount = value; }
        }

        /// <summary>
        /// Get/Set method of the TrxDate  field
        /// </summary>
        public DateTime TrxDate
        {
            get { return trxDate; }
            set { trxDate = value; }
        }

        /// <summary>
        /// Get/Set method of the LogId  field
        /// </summary>
        public int LogId
        {
            get { return logId; }
            set { logId = value;  }
        }

        /// <summary>
        /// Get/Set method of the concurrentRequestId  field
        /// </summary>
        public int ConcurrentRequestId
        {
            get { return concurrentRequestId; }
            set { concurrentRequestId = value;  }
        }

        /// <summary>
        /// Get/Set method of the Timestamp  field
        /// </summary>
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value;  }
        }
        /// <summary>
        /// Get/Set method of the ParafaitObjectGuid  field
        /// </summary>
        public string ParafaitObjectGuid
        {
            get { return parafaitObjectGuid; }
            set { parafaitObjectGuid = value; }
        }


        /// <summary>
        /// Get/Set method of the Status  field
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
      
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

    }
}
