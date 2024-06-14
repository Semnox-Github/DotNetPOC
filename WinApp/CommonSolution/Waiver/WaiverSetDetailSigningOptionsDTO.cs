using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Waiver
{

    /// <summary>
    ///  WaiverSetDetailSigningOptionsDTO Class
    /// </summary>
    public class WaiverSetDetailSigningOptionsDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int waiverSetDetailId;
        private int lookupValueId;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        public enum SearchByParameters
        {

            /// <summary>
            /// Search by ID
            /// </summary>
            ID = 0,
            /// <summary>
            /// Search by WAIVER_SET_DETAIL_ID
            /// </summary>
            WAIVER_SET_DETAIL_ID = 1,
            /// <summary>
            /// Search by LOOKUP_VALUE_ID
            /// </summary>
            LOOKUP_VALUE_ID = 2,
            /// <summary>
            /// Search by SITE_ID
            /// </summary>
            SITE_ID = 3

        }

        /// <summary>
        /// Default constructor
        /// </summary> 
        public WaiverSetDetailSigningOptionsDTO()
        {
            this.id = -1;
            this.waiverSetDetailId = -1;
            this.lookupValueId = -1;
            this.createdBy = "";
            this.creationDate = DateTime.MinValue;
            this.lastUpdatedDate = DateTime.MinValue;
            this.lastUpdatedBy = "";
            this.site_id = -1;
            this.guid = "";
            this.synchStatus = false;
            this.masterEntityId = -1;

        }

        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public WaiverSetDetailSigningOptionsDTO(int id, int waiverSetDetailId, int lookupValueId, string createdBy, DateTime creationDate, DateTime lastUpdatedDate,
                                    string lastUpdatedBy, int site_id, string guid, bool synchStatus, int masterEntityId)
        {
            this.id = id;
            this.waiverSetDetailId = waiverSetDetailId;
            this.lookupValueId = lookupValueId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
        }

        /// <summary>
        /// Get/Set method of the WaiverSigningOptionId field
        /// </summary>
        [DisplayName("WaiverSigningOptionId")]
        [DefaultValue(-1)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverSetDetailId field
        /// </summary>
        [DisplayName("WaiverSetDetailId")]
        [DefaultValue(-1)]
        public int WaiverSetDetailId { get { return waiverSetDetailId; } set { waiverSetDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LookupValueId field
        /// </summary>
        [DisplayName("LookupValueId")]
        [DefaultValue(-1)]
        public int LookupValueId { get { return lookupValueId; } set { lookupValueId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [DefaultValue("")]
        public string CreatedBy { get { return createdBy; } }


        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [DefaultValue("")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } }


        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return site_id; } set { site_id = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [DefaultValue("")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [DefaultValue(-1)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }



    }



}