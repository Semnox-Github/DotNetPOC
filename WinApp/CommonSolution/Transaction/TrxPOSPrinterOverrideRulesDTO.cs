/********************************************************************************************
 * Project Name - TrxPOSPrinterOverrideRules DTO
 * Description  - DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        11-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement 
 ********************************************************************************************/
using System;
using System.Linq;
using Semnox.Parafait.POS;
using System.Collections.Generic;

namespace Semnox.Parafait.Transaction
{
    public class TrxPOSPrinterOverrideRulesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by Trx POS Printer Override Rule Id
            /// </summary>
            TRX_POS_PRINTER_OVERRIDE_RULE_ID,

            /// <summary>
            /// Search by Transaction Id
            /// </summary>
            TRANSACTION_ID,

            /// <summary>
            /// Search by POS Printer Id
            /// </summary>
            POS_PRINTER_ID,

            /// <summary>
            /// Search by POS Printer Override Rule Id
            /// </summary>
            POS_PRINETR_OVERRIDE_RULE_ID,

            /// <summary>
            /// Search by POS Printer Override Rule Id
            /// </summary>
            POS_PRINETR_OVERRIDE_OPTION_ID,

            /// <summary>
            /// Search by Option Item Code
            /// </summary>
            OPTION_ITEM_CODE,

            /// <summary>
            /// Search by isactive
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by POS Printer Override Rule Id list
            /// </summary>
            TRX_POS_PRINTER_OVERRIDE_RULE_ID_LIST
        }
        private int trxPOSPrinterOverrideRuleId;
        private int transactionId;
        private int pOSPrinterId;
        private int pOSPrinterOverrideRuleId;
        private int pOSPrinterOverrideOptionId;
        private POSPrinterOverrideOptionItemCode optionItemCode;
        private string itemSourceColumnGuid;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId; 
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <returns></returns>
        public TrxPOSPrinterOverrideRulesDTO()
        {
            log.LogMethodEntry();
            trxPOSPrinterOverrideRuleId = -1;
            transactionId = -1;
            pOSPrinterId = -1;
            pOSPrinterOverrideRuleId = -1;
            pOSPrinterOverrideOptionId = -1;
            optionItemCode = POSPrinterOverrideOptionItemCode.NONE;
            itemSourceColumnGuid = string.Empty;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public TrxPOSPrinterOverrideRulesDTO(int trxPOSPrinterOverrideRuleId, int transactionId, int pOSPrinterId, int pOSPrinterOverrideRuleId, 
                                             int POSPrinterOverrideOptionId, POSPrinterOverrideOptionItemCode optionItemCode, string ItemSourceColumnGuid, bool isActive)
            : this()
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRuleId, transactionId, pOSPrinterId, pOSPrinterOverrideRuleId, POSPrinterOverrideOptionId, optionItemCode, 
                                ItemSourceColumnGuid, isActive);
            this.trxPOSPrinterOverrideRuleId = trxPOSPrinterOverrideRuleId;
            this.transactionId = transactionId;
            this.pOSPrinterId = pOSPrinterId;
            this.pOSPrinterOverrideRuleId = pOSPrinterOverrideRuleId;
            this.POSPrinterOverrideOptionId = POSPrinterOverrideOptionId;
            this.optionItemCode = optionItemCode;
            this.itemSourceColumnGuid = ItemSourceColumnGuid;
            this.isActive = isActive;
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TrxPOSPrinterOverrideRulesDTO(int trxPOSPrinterOverrideRuleId, int transactionId, int pOSPrinterId, int pOSPrinterOverrideRuleId,
                                             int POSPrinterOverrideOptionId, POSPrinterOverrideOptionItemCode optionItemCode, string ItemSourceColumnGuid, bool isActive,
                                            string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                            bool synchStatus, int siteId, int masterEntityId)
            : this(trxPOSPrinterOverrideRuleId, transactionId, pOSPrinterId, pOSPrinterOverrideRuleId, POSPrinterOverrideOptionId, optionItemCode,
                                ItemSourceColumnGuid, isActive)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRuleId, transactionId, pOSPrinterId, pOSPrinterOverrideRuleId, POSPrinterOverrideOptionId, optionItemCode,
                                ItemSourceColumnGuid, isActive, createdBy, creationDate, lastUpdatedBy,
                               lastUpdatedDate, guid, siteId, masterEntityId, synchStatus);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the TrxPOSPrinterOverrideRuleId field
        /// </summary>
        public int TrxPOSPrinterOverrideRuleId { get { return trxPOSPrinterOverrideRuleId; } set { this.IsChanged = true; trxPOSPrinterOverrideRuleId = value; } }

        /// <summary>
        /// Get/Set method of the Transaction Id field
        /// </summary>
        public int TransactionId { get { return transactionId; } set { this.IsChanged = true; transactionId = value; } }

        /// <summary>
        /// Get/Set method of the POSPrinter Id field
        /// </summary>
        public int POSPrinterId { get { return pOSPrinterId; } set { this.IsChanged = true; pOSPrinterId = value; } }

        /// <summary>
        /// Get/Set method of the POS Printer Override Rule Id field
        /// </summary>
        public int POSPrinterOverrideRuleId { get { return pOSPrinterOverrideRuleId; } set { pOSPrinterOverrideRuleId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POS Printer Override Option Id field
        /// </summary>
        public int POSPrinterOverrideOptionId { get { return pOSPrinterOverrideOptionId; } set { pOSPrinterOverrideOptionId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Option ItemCode field
        /// </summary>
        public POSPrinterOverrideOptionItemCode OptionItemCode { get { return optionItemCode; } set { optionItemCode = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Item Source Column Guid field
        /// </summary>
        public string ItemSourceColumnGuid { get { return itemSourceColumnGuid; } set { itemSourceColumnGuid = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || trxPOSPrinterOverrideRuleId < 0;
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
        ///// Returns true or false whether the POSPrinterOverrideOptionDTO changed or any of its children are changed
        ///// </summary>
        //public bool IsChangedRecursive
        //{
        //    get
        //    {
        //        if (IsChanged)
        //        {
        //            return true;
        //        }
        //        if (trxPOSPrinterOverrideRulesDTOList != null &&
        //           trxPOSPrinterOverrideRulesDTOList.Any(x => x.IsChanged))
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
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}


