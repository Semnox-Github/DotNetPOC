/********************************************************************************************
 * Project Name - ProductKeyDTO
 * Description  - Data object of ProductKey
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        28-May-2019   Girish Kundar           Created 
 *2.70        23-sept-2019  Mushahid Faizan        Added AddOnFeatures attribute to get add-on features.
 *2.120       09-Mar-2020   Girish Kundar          Modified : Licence count feature added for DashBoard
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// This is the ProductKeyDTO data object class. This acts as data holder for the ProductKey business object
    /// </summary>
    public class ProductKeyDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  SITE KEY field
            /// </summary>
            SITE_KEY,
            /// <summary>
            /// Search by  LICENSE KEY field
            /// </summary>
            LICENSE_KEY,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int id;
        private byte[] siteKey;
        private byte[] licenseKey;
        private byte[] featureKey;
        private byte[] authKey;
        private string noOfPOSMachinesLicensed;
        private string guid;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        List<KeyValuePair<string, int>> addOnFeatures = new List<KeyValuePair<string, int>>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductKeyDTO()
        {
            log.LogMethodEntry();
            id = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public ProductKeyDTO(int id, byte[] siteKey, byte[] licenseKey, byte[] featureKey, byte[] authKey, string noOfPOSMachinesLicensed,
                             string guid, DateTime lastUpdatedDate, string lastUpdatedBy, int siteId, bool synchStatus, int masterEntityId,
                             string createdBy, DateTime creationDate)
        {
            log.LogMethodEntry(id, siteKey, licenseKey, featureKey, authKey, noOfPOSMachinesLicensed, guid, lastUpdatedDate,
                               lastUpdatedBy, siteId, synchStatus, masterEntityId, createdBy, creationDate);
            this.id = id;
            this.siteKey = siteKey;
            this.licenseKey = licenseKey;
            this.featureKey = featureKey;
            this.authKey = authKey;
            this.noOfPOSMachinesLicensed = noOfPOSMachinesLicensed;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id  field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SiteKey  field
        /// </summary>
        public byte[] SiteKey
        {
            get { return siteKey; }
            set { siteKey = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LicenseKey  field
        /// </summary>
        public byte[] LicenseKey
        {
            get { return licenseKey; }
            set { licenseKey = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FeatureKey  field
        /// </summary>
        public byte[] FeatureKey
        {
            get { return featureKey; }
            set { featureKey = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the AuthKey field
        /// </summary>
        public byte[] AuthKey
        {
            get { return authKey; }
            set { authKey = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the NoOfPOSMachinesLicensed field
        /// </summary>
        public string NoOfPOSMachinesLicensed
        {
            get { return noOfPOSMachinesLicensed; }
            set { noOfPOSMachinesLicensed = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; this.IsChanged = true; }
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
            set { synchStatus = value; this.IsChanged = true; }
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
            set { createdBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the AddOnFeatures field
        /// </summary>
        public List<KeyValuePair<string, int>> AddOnFeatures
        {
            get { return addOnFeatures; }
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