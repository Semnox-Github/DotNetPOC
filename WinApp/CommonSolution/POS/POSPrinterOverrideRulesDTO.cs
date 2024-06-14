/********************************************************************************************
 * Project Name - POSPrinterOverrideOptions DTO
 * Description  - DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        09-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class POSPrinterOverrideRulesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by POS Printer Override Rule Id
            /// </summary>
            POS_PRINTER_OVERRIDE_RULE_ID,

            /// <summary>
            /// Search by POS Printer Id
            /// </summary>
            POS_PRINTER_ID,

            /// <summary>
            /// Search by POS Printer Override Option ID
            /// </summary>
            POS_PRINTER_OVERRIDE_OPTION_ID,

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
            POS_PRINTER_OVERRIDE_RULE_ID_LIST
        }
        private int posPrinterOverrideRuleId;
        private int posPrinterId;
        private int posPrinterOverrideOptionId;
        private string optionItemCode;
        private string itemSourceColumnGuid;
        private bool defaultOption;
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
        public POSPrinterOverrideRulesDTO()
        {
            log.LogMethodEntry();
            posPrinterOverrideRuleId = -1;
            posPrinterId = -1;
            posPrinterOverrideOptionId = -1;
            optionItemCode = string.Empty;
            defaultOption = true;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public POSPrinterOverrideRulesDTO(int posPrinterOverrideRuleId, int posPrinterId, int posPrinterOverrideOptionId, string optionItemCode, string itemSourceColumnGuid,
                                          bool defaultOption, bool isActive)
            : this()
        {
            log.LogMethodEntry(posPrinterOverrideRuleId, posPrinterId, posPrinterOverrideOptionId, optionItemCode, itemSourceColumnGuid, defaultOption, isActive);
            this.posPrinterOverrideRuleId = posPrinterOverrideRuleId;
            this.posPrinterId = posPrinterId;
            this.posPrinterOverrideOptionId = posPrinterOverrideOptionId;
            this.optionItemCode = optionItemCode;
            this.itemSourceColumnGuid = itemSourceColumnGuid;
            this.defaultOption = defaultOption;
            this.isActive = isActive;
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public POSPrinterOverrideRulesDTO(int posPrinterOverrideRuleId, int posPrinterId, int posPrinterOverrideOptionId, string optionItemCode, string itemSourceColumnGuid,
                                          bool defaultOption, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                            bool synchStatus, int siteId, int masterEntityId)
            : this(posPrinterOverrideRuleId, posPrinterId, posPrinterOverrideOptionId, optionItemCode, itemSourceColumnGuid, defaultOption, isActive)
        {
            log.LogMethodEntry(posPrinterOverrideRuleId, posPrinterId, posPrinterOverrideOptionId, optionItemCode, itemSourceColumnGuid, defaultOption, isActive, createdBy, creationDate, lastUpdatedBy,
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
        /// Copy constructor
        /// </summary>
        public POSPrinterOverrideRulesDTO(POSPrinterOverrideRulesDTO posPrinterOverrideRulesDTO)
           : this(posPrinterOverrideRulesDTO.posPrinterOverrideRuleId, posPrinterOverrideRulesDTO.posPrinterId, posPrinterOverrideRulesDTO.posPrinterOverrideOptionId, 
                 posPrinterOverrideRulesDTO.optionItemCode, posPrinterOverrideRulesDTO.itemSourceColumnGuid, posPrinterOverrideRulesDTO.defaultOption, posPrinterOverrideRulesDTO.isActive,
                 posPrinterOverrideRulesDTO.createdBy, posPrinterOverrideRulesDTO.creationDate, posPrinterOverrideRulesDTO.lastUpdatedBy, posPrinterOverrideRulesDTO.lastUpdatedDate,
                 posPrinterOverrideRulesDTO.guid, posPrinterOverrideRulesDTO.synchStatus, posPrinterOverrideRulesDTO.siteId, posPrinterOverrideRulesDTO.masterEntityId)
        {
            log.LogMethodEntry(posPrinterOverrideRulesDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the POSPrinterOverrideRuleId field
        /// </summary>
        public int POSPrinterOverrideRuleId { get { return posPrinterOverrideRuleId; } set { this.IsChanged = true; posPrinterOverrideRuleId = value; } }

        /// <summary>
        /// Get/Set method of the POSPrinterId field
        /// </summary>
        public int POSPrinterId { get { return posPrinterId; } set { this.IsChanged = true; posPrinterId = value; } }

        /// <summary>
        /// Get/Set method of the POSPaPrinterOverrideOptionId field
        /// </summary>
        public int POSPrinterOverrideOptionId { get { return posPrinterOverrideOptionId; } set { this.IsChanged = true; posPrinterOverrideOptionId = value; } }

        /// <summary>
        /// Get/Set method of the OptionItemCode field
        /// </summary>
        public string OptionItemCode { get { return optionItemCode; } set { optionItemCode = value; } }

        /// <summary>
        /// Get/Set method of the ItemSourceColumnGuid field
        /// </summary>
        public string ItemSourceColumnGuid { get { return itemSourceColumnGuid; } set { itemSourceColumnGuid = value; } }

        /// <summary>
        /// Get/Set method of the DefaultOption field
        /// </summary>
        public bool DefaultOption { get { return defaultOption; } set { this.IsChanged = true; defaultOption = value; } }

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
                    return notifyingObjectIsChanged || POSPrinterOverrideRuleId < 0;
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
        //        if (pOSPrinterOverrideRulesDTOList != null &&
        //           pOSPrinterOverrideRulesDTOList.Any(x => x.IsChanged))
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

