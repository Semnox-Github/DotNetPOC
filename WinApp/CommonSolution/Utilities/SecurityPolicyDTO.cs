/********************************************************************************************
 * Project Name - Utilities
 * Description  - DTO of security policy
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        24-Mar-2019   Jagan Mohana          Created 
              09-Apr-2019   Mushahid Faizan       Added LogMethodEntry & LogMethodExit method.
              29-Jul-2019   Mushahid Faizan       added IsActive Column
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Core.Utilities
{
    public class SecurityPolicyDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by POLICY_ID field
            /// </summary>
            POLICY_ID,
            /// <summary>
            /// Search by POLICY_NAME field
            /// </summary>
            POLICY_NAME,
            /// <summary>
            /// Search by IsActive Field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by SITEID Field
            /// </summary>
            SITEID
        }

        int policyId;
        string policyName;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;
        int siteId;
        string guid;
        bool synchStatus;
        int masterEntityId;
        bool isActive;
        List<SecurityPolicyDetailsDTO> securityPolicyDetailsDTOList;

        /// <summary>
        /// Default constructor
        /// Added siteId = -1 by Mushahid Faizan on 09-Apr-2019
        /// </summary>
        public SecurityPolicyDTO()
        {
            log.LogMethodEntry();
            this.policyId = -1;
            this.masterEntityId = -1;
            this.siteId = -1;
            this.isActive = true;
            this.securityPolicyDetailsDTOList = new List<SecurityPolicyDetailsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SecurityPolicyDTO(int policyId, string policyName, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                 int siteId, string guid, bool synchStatus, int masterEntityId, bool isActive)
        {
            log.LogMethodEntry(policyId, policyName, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, guid, synchStatus, masterEntityId, isActive);
            this.policyId = policyId;
            this.policyName = policyName;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the PolicyId field
        /// </summary>
        [DisplayName("PolicyId")]
        [ReadOnly(true)]
        public int PolicyId { get { return policyId; } set { policyId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PolicyName field
        /// </summary>
        [DisplayName("PolicyName")]
        public string PolicyName { get { return policyName; } set { policyName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        [DisplayName("SyncStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        /// <summary>
        /// Get/Set methods for SecurityPolicyDTOList 
        /// </summary>
        public List<SecurityPolicyDetailsDTO> SecurityPolicyDTOList
        {
            get
            {
                return securityPolicyDetailsDTOList;
            }

            set
            {
                securityPolicyDetailsDTOList = value;
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
                    return notifyingObjectIsChanged || policyId < 0;
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