
/********************************************************************************************
 * Project Name - PriceList
 * Description  - Data object of the UserRolePriceList
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-May-2016    Amaresh          Created 
 *2.70.2        17-Jul-2019    Deeksha          Modifications as per three tier standard.
 *            07-Aug-2019    Mushahid Faizan  Added IsActive
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.PriceList
{
    public class UserRolePriceListDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByUserRolePriceListParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserRolePriceListParameters
        {
            /// <summary>
            /// Search by RolePriceListId field
            /// </summary>
            ROLE_PRICE_LISTID,

            /// <summary>
            /// Search by RoleId field
            /// </summary>
            ROLE_ID,
            /// <summary>
            /// Search by RoleId field
            /// </summary>
            ROLE_ID_LIST,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ISACTIVE,

            /// <summary>
            /// Search by SiteId field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
        }

        private int rolePriceListId;
        private int roleid;
        private int priceListId;
        private string guid;
        private int siteId;
        private string lastUpdatedBy;
        private string createdBy;
        private DateTime lastUpdatedDate;
        private DateTime creationDate;
        private bool synchStatus;
        private bool isActive;
        private int masterEntityId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

           /// <summary>
        /// Default constructor
        /// </summary>
        public UserRolePriceListDTO()
        {
            log.LogMethodEntry();
            rolePriceListId = -1;
            siteId = -1;
            masterEntityId = -1;
            roleid = -1;
            priceListId = -1;
            isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// UserRolePriceList Constructor with required fields
        /// </summary>
        /// <param name="rolePriceListId"></param>
        /// <param name="roleid"></param>
        /// <param name="priceListId"></param>
        public UserRolePriceListDTO(int rolePriceListId, int roleid, int priceListId)
            :this()
        {
            log.LogMethodEntry(rolePriceListId, roleid, priceListId);
            this.rolePriceListId = rolePriceListId;
            this.roleid = roleid;
            this.priceListId = priceListId;         
            log.LogMethodExit();
        }


        /// <summary>
        /// UserRolePriceList Constructor with all the fields
        /// </summary>
        public UserRolePriceListDTO(int rolePriceListId, int roleid, int priceListId, string guid, int siteId, DateTime lastUpdatedDate,
                                     string lastUpdatedBy, string createdBy, DateTime creationDate, bool synchStatus,int masterEntityId, bool isActive)
            :this(rolePriceListId, roleid, priceListId)
        {
            log.LogMethodEntry(rolePriceListId, roleid, priceListId,  guid,  siteId,  lastUpdatedDate,lastUpdatedBy,  createdBy, 
                                creationDate,  synchStatus,  masterEntityId, isActive);
            this.guid = guid;
            this.siteId = siteId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RolePriceListId field
        /// </summary>
        [DisplayName("RolePriceList Id")]
        public int RolePriceListId { get { return rolePriceListId; } set { rolePriceListId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Roleid
        /// </summary>
        [DisplayName("Roleid")]
        public int Roleid { get { return roleid; } set { roleid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PriceList Id field
        /// </summary>
        [DisplayName("PriceList Id")]
        public int PriceListId { get { return priceListId; } set { priceListId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid
        /// </summary>
        [Browsable(false)]
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        [ReadOnly(true)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedBy ")]
        [ReadOnly(true)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [ReadOnly(true)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [ReadOnly(true)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("masterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || rolePriceListId < 0;
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
