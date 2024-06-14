/********************************************************************************************
 * Project Name - PriceList
 * Description  - Data object of the DisplayGroup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-May-2016    Amaresh          Created
 *2.50        05-Feb-2019    Indrajeet Kumar  Add Child DTOList and IsActive
 *2.70.2        17-Jul-2019    Deeksha          Modifications as per three tier standard.
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.PriceList
{
    public class PriceListDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByPriceListParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPriceListParameters
        {
            /// <summary>
            /// Search by Price List field
            /// </summary>
            PRICE_LIST_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE
        }

        private int priceListId;
        private string priceListName;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;

        private List<PriceListProductsDTO> priceListProductsList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
      
         /// <summary>
        /// Default constructor
        /// </summary>
        public PriceListDTO()
        {
            log.LogMethodEntry();
            priceListId = -1;
            siteId = -1;
            isActive = true;
            priceListProductsList = new List<PriceListProductsDTO>();
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// PriceList Constructor with required fields
        /// </summary>
        /// <param name="priceListId"></param>
        /// <param name="priceListName"></param>
        public PriceListDTO(int priceListId, string priceListName, bool isActive)
            :this()
        {
            log.LogMethodEntry(priceListId, priceListName, isActive);
            this.priceListId = priceListId;
            this.priceListName = priceListName;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// PriceList constructor with all the  fields
        /// </summary>
        public PriceListDTO(int priceListId, string priceListName, string guid, bool synchStatus, int siteId, DateTime lastUpdatedDate,
                                    string lastUpdatedBy,int masterEntityId,string createdBy,DateTime creationDate,bool isActive)
            :this(priceListId, priceListName, isActive)
        {
            log.LogMethodEntry( priceListId,  priceListName,  guid,  synchStatus,  siteId,  lastUpdatedDate,lastUpdatedBy, 
                                masterEntityId,  createdBy,  creationDate, isActive);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PriceListId field
        /// </summary>
        [DisplayName("PriceList Id")]
        public int PriceListId { get { return priceListId; } set { priceListId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PriceListName
        /// </summary>
        [DisplayName("PriceList Name")]
        public string PriceListName { get { return priceListName; } set { priceListName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid
        /// </summary>
        [Browsable(false)]
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        [DisplayName("Last Updated Date")]
        [ReadOnly(true)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [Browsable(false)]
        [DisplayName("LastUpdatedBy ")]
        [ReadOnly(true)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }


        /// <summary>
        /// Get/Set method of the CreationDate fields
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set for PriceListProductsList Field
        /// </summary>
        public List<PriceListProductsDTO> PriceListProductsList { get { return priceListProductsList; } set { priceListProductsList = value; this.IsChanged = true; } }

        /// <summary>
        /// Returns whether the PriceListDTO changed or any of its List  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (priceListProductsList != null &&
                   priceListProductsList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        // <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || priceListId < 0;
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
