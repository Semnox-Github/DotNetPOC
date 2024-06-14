
/********************************************************************************************
 * Project Name - CMSGroupDisplayGroupDTO
 * Description  - Data object of the CMSGroupDisplayGroupDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        23-Sep-2016    Rakshith          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class CMSGroupDisplayGroupDTO : IChangeTracking
    {
       Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    /// <summary>
    /// SearchByDisplayGroupsParameters enum controls the search fields, this can be expanded to include additional fields
    /// </summary>
    public enum SearchByDisplayGroupsParameters
    {
        /// <summary>
        /// Search by GROUP_DISPLAY_GROUP_ID field
        /// </summary>
        GROUP_DISPLAY_GROUP_ID = 0,

        /// <summary>
        /// Search by GROUP_ID field
        /// </summary>
        GROUP_ID = 1,
        
        /// <summary>
        /// Search by PRODUCT_DISPLAY_GROUP_ID field
        /// </summary>
        PRODUCT_DISPLAY_GROUP_ID = 2
    }

        int groupDisplayGroupId;
        int groupId;
        int productDisplayGroupId;
        string createdBy;
        DateTime creationDate;
        DateTime lastUpdatedDate;
        string lastUpdatedUser;
        int siteId;
        string guid;
        bool synchStatus;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSGroupDisplayGroupDTO()
        {
            log.Debug("Starts-CmsDisplayGroup() default constructor.");
            siteId = -1;
            productDisplayGroupId = -1;
            groupId = -1;
            groupDisplayGroupId = -1;
            log.Debug("Ends-CmsDisplayGroup() default constructor.");
        }

         /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CMSGroupDisplayGroupDTO(int groupDisplayGroupId, int groupId, int productDisplayGroupId,  string createdBy, DateTime creationDate, DateTime lastUpdatedDate,
                                            string lastUpdatedUser, int siteId, string guid, bool synchStatus)
                                     
        {
            log.Debug("Starts-CMSGroupDisplayGroupDTO(with all the data fields) Parameterized constructor.");
            this.groupDisplayGroupId = groupDisplayGroupId;
            this.groupId = groupId;
            this.productDisplayGroupId = productDisplayGroupId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            log.Debug("Ends-CmsDisplayGroup(with all the data fields) Parameterized constructor.");
        }

        /// <summary>
        /// Get/Set method of the CMSGroupDisplayGroupId field
        /// </summary>
        [DisplayName("GroupDisplayGroupId")]
        public int GroupDisplayGroupId { get { return groupDisplayGroupId; } set { groupDisplayGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CMSGroupId field
        /// </summary>
        [DisplayName("GroupId")]
        public int GroupId { get { return groupId; } set { groupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductDisplay GroupId field
        /// </summary>
        [DisplayName("ProductDisplay GroupId")]
        public int ProductDisplayGroupId { get { return productDisplayGroupId; } set { productDisplayGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        [ReadOnly(true)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedUser ")]
        [ReadOnly(true)]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value;  } }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [ReadOnly(true)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [ReadOnly(true)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        [Browsable(false)]
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value;  } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
  
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
                    return notifyingObjectIsChanged || groupDisplayGroupId < 0;
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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }
    }
}
