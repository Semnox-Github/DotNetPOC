/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of TrxTaxLine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      22-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the TransactionTaxLineDTO data object class. This acts as data holder for the TrxTaxLine business object
    /// </summary>
    public class TransactionTaxLineDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   TRXID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by   TRX_TAX_LINE_ID field
            /// </summary>
            TRX_TAX_LINE_ID,
            /// <summary>
            /// Search by   TRX_TAX_LINE_ID field
            /// </summary>
            TRX_TAX_LINE_ID_LIST,
            /// <summary>
            /// Search by  TAX_ID field
            /// </summary>
            TAX_ID,
            /// <summary>
            /// Search by  TAX_STRUCTURE_ID field
            /// </summary>
            TAX_STRUCTURE_ID,
            /// <summary>
            /// Search by  LINE_ID field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int trxTaxLineId; 
        private int trxId;
        private int lineId;
        private int taxId;
        private int taxStructureId;
        private decimal percentage;
        private decimal amount;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private decimal productSplitAmount;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransactionTaxLineDTO()
        {
            log.LogMethodEntry();
            trxId = -1;
            taxId = -1;
            taxStructureId = -1;
            lineId = -1;
            trxTaxLineId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Parameterized constructor with required fields
        /// </summary>
        public TransactionTaxLineDTO(int trxTaxLineId, int trxId, int lineId, int taxId, int taxStructureId, decimal percentage, decimal amount,
                             decimal productSplitAmount)
            :this()
        {

            log.LogMethodEntry(trxTaxLineId, trxId, lineId, taxId, taxStructureId, percentage, amount,productSplitAmount);
            this.trxTaxLineId = trxTaxLineId;
            this.trxId = trxId;
            this.lineId = lineId;
            this.taxId = taxId;
            this.taxStructureId = taxStructureId;
            this.percentage = percentage;
            this.amount = amount;
            this.productSplitAmount = productSplitAmount;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Parameterized constructor with all the fields
        /// </summary>
        public TransactionTaxLineDTO(int trxTaxLineId ,int trxId, int lineId, int taxId,  int taxStructureId,  decimal percentage,   decimal amount,
                             string guid,bool synchStatus,int siteId, decimal productSplitAmount,int masterEntityId,
                            string createdBy, DateTime creationDate,string lastUpdatedBy, DateTime lastUpdatedDate )
            :this(trxTaxLineId, trxId, lineId, taxId, taxStructureId, percentage, amount, productSplitAmount)
        {

            log.LogMethodEntry(trxTaxLineId ,trxId, lineId,taxId, taxStructureId, percentage, amount, guid, synchStatus, siteId, 
                                productSplitAmount, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the TrxTaxLineId field
        /// </summary>
        public int TrxTaxLineId
        {
            get { return trxTaxLineId; }
            set { trxTaxLineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        public int TrxId
        {
            get { return trxId; }
            set { trxId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TaxId field
        /// </summary>
        public int TaxId
        {
            get { return taxId; }
            set { taxId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        public int LineId
        {
            get { return lineId; }
            set { lineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TaxStructureId field
        /// </summary>
        public int TaxStructureId
        {
            get { return taxStructureId; }
            set { taxStructureId = value; this.IsChanged = true; }
        }
        
        /// <summary>
        /// Get/Set method of the Percentage field
        /// </summary>
        public decimal Percentage
        {
            get { return percentage; }
            set { percentage = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Amount field
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductSplitAmount field
        /// </summary>
        public decimal ProductSplitAmount
        {
            get { return productSplitAmount; }
            set { productSplitAmount = value; this.IsChanged = true; }
        }
        
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;}
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value;  }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value;  }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
       
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || trxTaxLineId < 0;
                }
            }
            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        ///// <summary>
        ///// Returns whether the DTO changed or any of its children are changed
        ///// </summary>
        //public bool IsChangedRecursive
        //{
        //    get
        //    {
        //        if (IsChanged)
        //        {
        //            return true;
        //        }
        //        if (DTOList != null &&
        //           DTOList.Any(x => x.IsChangedRecursive))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
