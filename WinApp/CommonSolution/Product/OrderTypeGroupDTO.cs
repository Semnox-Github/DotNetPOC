/********************************************************************************************
 * Project Name - OrderTypeGroup DTO
 * Description  - Data object of OrderTypeGroup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        19-Dec-2017   Lakshminarayana          Created 
 *2.110.00    27-Nov-2020   Abhishek                Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the OrderTypeGroup data object class. This acts as data holder for the OrderTypeGroup business object
    /// </summary>
    public class OrderTypeGroupDTO
    {
        private static readonly  Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Id field
            /// </summary>
            ID = 0,
            /// <summary>
            /// Search by Name field
            /// </summary>
            NAME = 2,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ACTIVE_FLAG = 3,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID = 4,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID = 5
        }

        private string name;
        private string description;
        private int precedence;
        private int id;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrderTypeGroupDTO()
        {
            log.LogMethodEntry();
            id = -1;
            masterEntityId = -1;
            isActive = true;
            precedence = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public OrderTypeGroupDTO(int id, string name, string description, int precedence, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, name, description, precedence, isActive);
            this.id = id;
            this.name = name;
            this.description = description;
            this.precedence = precedence;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public OrderTypeGroupDTO(int id, string name, string description, int precedence, bool isActive, string createdBy,
                                 DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
                                 int masterEntityId, bool synchStatus, string guid)
            : this(id, name, description, precedence, isActive)
        {
            log.LogMethodEntry(id, name, description, precedence, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, siteId, masterEntityId, synchStatus, guid);           
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }
       
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { this.IsChanged = true; name = value; } }      

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { this.IsChanged = true; description = value; } }
        
        /// <summary>
        /// Get/Set method of the precedence field
        /// </summary>
        public int Precedence { get { return precedence; } set { this.IsChanged = true; precedence = value; } }
       
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
                    return notifyingObjectIsChanged || id < 0;
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
