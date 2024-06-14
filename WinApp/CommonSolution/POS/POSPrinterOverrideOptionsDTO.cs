/********************************************************************************************
 * Project Name - POSPrinterOverrideOptions DTO
 * Description  - DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        08-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;

namespace Semnox.Parafait.POS
{
    public class POSPrinterOverrideOptionsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by POS Printer Override Option Id
            /// </summary>
            POS_PRINTER_OVERRIDE_OPTION_ID,

            /// <summary>
            /// Search by Option Name
            /// </summary>
            OPTION_NAME,

            /// <summary>
            /// Search by Option Description
            /// </summary>
            OPTION_DESCRIPTION,

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
            /// Search by POS Printer Override Option Id list
            /// </summary>
            POS_PRINTER_OVERRIDE_OPTION_ID_LIST
        }

        private int posPrinterOverrideOptionId;
        private string optionName;
        private string optionDescription;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        //private List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <returns></returns>
        public POSPrinterOverrideOptionsDTO()
        {
            log.LogMethodEntry();
            posPrinterOverrideOptionId = -1;
            optionName = string.Empty;
            optionDescription = string.Empty;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public POSPrinterOverrideOptionsDTO(int posPrinterOverrideOptionId, string optionName, string optionDescription, bool isActive)
            : this()
        {
            log.LogMethodEntry(posPrinterOverrideOptionId, optionName, optionDescription, isActive);
            this.posPrinterOverrideOptionId = posPrinterOverrideOptionId;
            this.optionName = optionName;
            this.optionDescription = optionDescription;
            this.isActive = isActive;
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public POSPrinterOverrideOptionsDTO(int posPrinterOverrideOptionId, string optionName, string optionDescription, bool isActive,
                                            string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                            bool synchStatus, int siteId, int masterEntityId)
            : this(posPrinterOverrideOptionId, optionName, optionDescription, isActive)
        {
            log.LogMethodEntry(posPrinterOverrideOptionId, optionName, optionDescription, isActive, createdBy, creationDate, lastUpdatedBy,
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
        /// Get/Set method of the POSPaPrinterOverrideOptionId field
        /// </summary>
        public int POSPrinterOverrideOptionId { get { return posPrinterOverrideOptionId; } set { this.IsChanged = true; posPrinterOverrideOptionId = value; }        }
        
        /// <summary>
        /// Get/Set method of the OptionName field
        /// </summary>
        public string OptionName { get { return optionName; } set { optionName = value; IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the OptionDescription field
        /// </summary>
        public string OptionDescription { get { return optionDescription; } set { optionDescription = value; IsChanged = true; } }
      
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
                    return notifyingObjectIsChanged || posPrinterOverrideOptionId < 0;
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

        /// <summary>
        /// Returns true or false whether the POSPrinterOverrideOptionDTO changed or any of its children are changed
        /// </summary>
        //public bool IsChangedRecursive
        //{
        //    get
        //    {
        //        if (IsChanged)
        //        {
        //            return true;
        //        }
        //        if (pOSPrinterOverrideOptionsDTOList != null &&
        //           pOSPrinterOverrideOptionsDTOList.Any(x => x.IsChanged))
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
                                           
                                            
                                            