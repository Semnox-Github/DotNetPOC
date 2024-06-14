/********************************************************************************************
 * Project Name - Organization DTO
 * Description  - Data object of organization  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        01-Mar-2016   Raghuveera          Created
 *2.70.2        23-Jul-2019   Deeksha             Modifications as per three tier standard.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// This is the Organization data object class. This acts as data holder for the Organization business object
    /// </summary>
    public class OrganizationDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByOrganizationParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByOrganizationParameters
        {
            /// <summary>
            /// Search by ORG ID field
            /// </summary>
            ORG_ID,
            /// <summary>
            /// Search by ORG NAME field
            /// </summary>
            ORG_NAME,
            /// <summary>
            /// Search by PARENT ORG ID field
            /// </summary>
            PARENT_ORG_ID,
            /// <summary>
            /// Search by COMPANY_ID field
            /// </summary>
            COMPANY_ID
        }

        private int orgId;
        private string orgName;
        private int structureId;
        private int parentOrgId;
        private int companyId;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private List<SiteDTO> siteList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrganizationDTO()
        {
            log.LogMethodEntry();
            orgId = -1;
            structureId = -1;
            parentOrgId = -1;
            companyId = -1;
            siteList = new List<SiteDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public OrganizationDTO(int orgId, string orgName, int structureId, int parentOrgId, int companyId)
            :this()
        {
            log.LogMethodEntry(orgId, orgName, structureId, parentOrgId, companyId);
            this.orgId = orgId;
            this.orgName = orgName;
            this.structureId = structureId;
            this.parentOrgId = parentOrgId;
            this.companyId = companyId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public OrganizationDTO(int orgId, string orgName, int structureId, int parentOrgId, int companyId, string guid,
                               string createdBy,DateTime creationDate,string lastUpdatedBy,DateTime lastUpdateDate)
            :this(orgId, orgName, structureId, parentOrgId, companyId)
        {
            log.LogMethodEntry(orgId, orgName, structureId, parentOrgId, companyId, guid, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

    

        /// <summary>
        /// Get/Set method of the OrgId field
        /// </summary>
        [DisplayName("Org Id")]
        [ReadOnly(true)]
        public int OrgId { get { return orgId; } set { orgId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the OrgName field
        /// </summary>
        [DisplayName("Org Name")]
        public string OrgName { get { return orgName; } set { orgName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the StructureId field
        /// </summary>
        [DisplayName("StructureId")]
        public int StructureId { get { return structureId; } set { structureId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ParentOrgId field
        /// </summary>
        [DisplayName("ParentOrgId")]
        public int ParentOrgId { get { return parentOrgId; } set { parentOrgId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CompanyId field
        /// </summary>
        [DisplayName("CompanyId")]
        public int CompanyId { get { return companyId; } set { companyId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public String CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public String LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }


        /// <summary>
        /// Get/Set method of the SiteList field
        /// </summary>
        [Browsable(false)]
        public List<SiteDTO> SiteList
        {
            get
            {
                return siteList;
            }

            set
            {
                siteList = value;
            }
        }

        /// <summary>
        /// Returns whether the OrganizationDTO changed or any of its siteDTOLists  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (siteList != null &&
                   siteList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
                    return notifyingObjectIsChanged || orgId < 0;
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
