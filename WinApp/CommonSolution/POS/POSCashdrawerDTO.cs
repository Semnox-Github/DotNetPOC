/********************************************************************************************
 * Project Name - POS
 * Description  - POSCashdrawerDTO - data object for the POS cashdrawer 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130.0     11-Aug-2021      Girish Kundar     Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Semnox.Parafait.Printer;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// DTO class for printers
    /// </summary>
    ///Serializable
    public class POSCashdrawerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by pos cashdrawer id
            /// </summary>
            POS_CASHDRAWER_ID,
            /// <summary>
            /// Search by POSMachineId
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by POS_MACHINE_ID_LIST
            /// </summary>
            POS_MACHINE_ID_LIST,
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
            CASHDRAWER_ID
        }

        private int posCashdrawerId;
        private int cashdrawerId;
        private int posMachineId;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public POSCashdrawerDTO()
        {
            log.LogMethodEntry();
            posCashdrawerId = -1;
            cashdrawerId = -1;
            posMachineId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public POSCashdrawerDTO(int posCashdrawerId,int cashdrawerId, int posMachineId,bool isActive)
            :this()
        {
            log.LogMethodEntry(posCashdrawerId, cashdrawerId,posMachineId, isActive);
            this.posCashdrawerId = posCashdrawerId;
            this.cashdrawerId = cashdrawerId;
            this.posMachineId = posMachineId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public POSCashdrawerDTO(POSCashdrawerDTO posCashdrawerDTO)
        {
            log.LogMethodEntry(posCashdrawerDTO);
            this.posCashdrawerId = posCashdrawerDTO.POSCashdrawerId;
            this.cashdrawerId = posCashdrawerDTO.CashdrawerId;
            this.posMachineId = posCashdrawerDTO.POSMachineId;
            this.isActive = posCashdrawerDTO.IsActive;
            this.createdBy = posCashdrawerDTO.CreatedBy;
            this.creationDate = posCashdrawerDTO.CreationDate;
            this.lastUpdatedBy = posCashdrawerDTO.LastUpdatedBy;
            this.lastUpdateDate = posCashdrawerDTO.LastUpdatedDate;
            this.siteId = posCashdrawerDTO.SiteId;
            this.masterEntityId = posCashdrawerDTO.MasterEntityId;
            this.synchStatus = posCashdrawerDTO.SynchStatus;
            this.guid = posCashdrawerDTO.Guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// constructor with all the parameter
        /// </summary>
        public POSCashdrawerDTO(int posCashdrawerId,int cashdrawerId, int posMachineId, bool isActive, DateTime creationDate, 
                                string createdBy, DateTime lastUpdateDate, string lastUpdatedBy, int siteId, string guid,
                                bool synchStatus, int masterEntityId)
            :this(posCashdrawerId, cashdrawerId ,posMachineId, isActive)
        {
            log.LogMethodEntry(posCashdrawerId, cashdrawerId, posMachineId, isActive,creationDate,
                            createdBy,  lastUpdateDate,lastUpdatedBy, siteId,  guid,synchStatus,  
                            masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the POSCashdrawerId field
        /// </summary>
        public int POSCashdrawerId
        {
            get
            {
                return posCashdrawerId;
            }
            set
            {
                posCashdrawerId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        public int POSMachineId
        {
            get
            {
                return posMachineId;
            }
            set
            {
                posMachineId = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the CashdrawerId field
        /// </summary>
        public int CashdrawerId
        {
            get
            {
                return cashdrawerId;
            }
            set
            {
                cashdrawerId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
               
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
               
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }
        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {               
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || posCashdrawerId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
