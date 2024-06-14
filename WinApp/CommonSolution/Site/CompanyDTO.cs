/********************************************************************************************
 * Project Name - Site
 * Description  - DTO of Company
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        08-Mar-2019   Jagan Mohana          Created 
 *            29-Mar-2019   Mushahid Faizan       Added LogMethodEntry & LogMethodExit,Removed unused namespaces & enum numbering.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Site
{
    public class CompanyDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchBySiteParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            COMPANY_ID,
            /// <summary>
            /// Search by SITE_NAME field
            /// </summary>
            COMPANY_NAME,
            /// <summary>
            /// Search by MASTER_SITE_ID field
            /// </summary>
            MASTER_SITE_ID
        }

        int companyId;
        string companyName;
        string loginKey;
        int masterSiteId;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;
        List<OrganizationStructureDTO> organizationStructureDTOList;
        List<OrganizationDTO> organizationDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CompanyDTO()
        {
            log.LogMethodEntry();
            companyId = -1;
            masterSiteId = -1;
            organizationStructureDTOList = new List<OrganizationStructureDTO>();
            organizationDTOList = new List<OrganizationDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CompanyDTO(int companyId, string companyName, string loginKey, int masterSiteId, string createdBy, DateTime creationDate,
                        string lastUpdatedBy, DateTime lastUpdateDate)
        {
            log.LogMethodEntry(companyId, companyName, loginKey, masterSiteId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.companyId = companyId;
            this.companyName = companyName;
            this.loginKey = loginKey;
            this.masterSiteId = masterSiteId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the CompanyId field
        /// </summary>
        [DisplayName("Company Id")]
        [ReadOnly(true)]
        public int CompanyId { get { return companyId; } set { companyId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CompanyName field
        /// </summary>
        [DisplayName("Company Name")]
        public string CompanyName { get { return companyName; } set { companyName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LoginKey field
        /// </summary>
        [DisplayName("Login Key")]
        public string LoginKey { get { return loginKey; } set { loginKey = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterSiteId field
        /// </summary>
        [DisplayName("MasterSiteId")]
        public int MasterSiteId { get { return masterSiteId; } set { masterSiteId = value; this.IsChanged = true; } }
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
        /// Get/Set method of the OrganizationDTOList field
        /// </summary>
        [Browsable(false)]
        public List<OrganizationDTO> OrganizationDTOList
        {
            get
            {
                return organizationDTOList;
            }

            set
            {
                organizationDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OrganizationStructureDTO field
        /// </summary>
        [Browsable(false)]
        public List<OrganizationStructureDTO> OrganizationStructureDTOList
        {
            get
            {
                return organizationStructureDTOList;
            }

            set
            {
                organizationStructureDTOList = value;
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
                    return notifyingObjectIsChanged || companyId < 0;
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
