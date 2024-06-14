/********************************************************************************************
 * Project Name - Organization
 * Description  - Organization logic of organization
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        01-Mar-2016   Raghuveera          Created
 *2.60        29-Mar-2019   Mushahid Faizan     Added LogMethodEntry/Exit in Save() 
 *                                              & Modified Constructor having Execution Context & organizationDTO.
 *2.70.2       23-Jul-2019   Deeksha             Modifications as per three tier standard.
 *2.110        04-Jan-2021   Laster Menezes     Added new method GetChildOrganizationListWithSiteByParentOrg
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// Organization Class
    /// </summary>
    public class Organization
    {
        private OrganizationDTO organizationDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string connectionString = string.Empty;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Default Constructor Organization 
        /// </summary>
        public Organization()
        {
            log.LogMethodEntry();
            this.organizationDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default Constructor Organization 
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public Organization(string connectionString)
        {
            log.LogMethodEntry(connectionString);
            this.organizationDTO = null;
            this.connectionString = connectionString;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the Execution Context and Organization DTO parameter
        /// </summary>
        /// <param name="organizationDTO">Parameter of the type OrganizationDTO</param>
        /// <param name="executionContext">Parameter of the type executionContext</param>
        public Organization(ExecutionContext executionContext, OrganizationDTO organizationDTO)
        {
            log.LogMethodEntry(executionContext, organizationDTO);
            this.organizationDTO = organizationDTO;
            this.machineUserContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Reports  
        /// Reports   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            OrganizationDataHandler organizationDataHandler = new OrganizationDataHandler(sqlTransaction);
            if (!string.IsNullOrEmpty(connectionString))
            {
                organizationDataHandler = new OrganizationDataHandler(connectionString);
            }
            if (organizationDTO.OrgId < 0)
            {
                organizationDTO = organizationDataHandler.InsertOrganization(organizationDTO, machineUserContext.GetUserId());
                organizationDTO.AcceptChanges();
            }
            else
            {
                if (organizationDTO.IsChanged)
                {
                    organizationDTO = organizationDataHandler.UpdateOrganization(organizationDTO, machineUserContext.GetUserId());
                    organizationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// DeleteOrganization method
        /// </summary>
        /// <param name="orgId">orgId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns int</returns>
        public int DeleteOrganization(int orgId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(orgId, sqlTransaction);
            OrganizationDataHandler organizationDataHandler = new OrganizationDataHandler(sqlTransaction);
            int id = organizationDataHandler.DeleteOrganization(orgId);
            log.LogMethodExit(id);
            return id;

        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public OrganizationDTO GetOrganizationDTO
        {
            get
            {
                return organizationDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of organization
    /// </summary>
    public class OrganizationList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Gets the root organization data 
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>        
        /// <returns>Returns OrganizationDTO</returns>
        public List<OrganizationDTO> GetRootOrganizationList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            OrganizationDataHandler organizationDataHandler = new OrganizationDataHandler(sqlTransaction);
            List<OrganizationDTO> organizationDTOs = new List<OrganizationDTO>();
            organizationDTOs = organizationDataHandler.GetRootOrganizationList();
            log.LogMethodExit(organizationDTOs);
            return organizationDTOs;
        }

        /// <summary>
        /// Returns the organization list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<OrganizationDTO> GetAllOrganizations(List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>> searchParameters, bool loadChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            OrganizationDataHandler organizationDataHandler = new OrganizationDataHandler(sqlTransaction);
            List<OrganizationDTO> organizationList = new List<OrganizationDTO>();
            organizationList = organizationDataHandler.GetOrganizationList(searchParameters);
            if (loadChildRecords && organizationList != null && organizationList.Count != 0)
            {
                SiteList siteList = new SiteList(null);
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchSiteParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                List<SiteDTO> siteDTOs = siteList.GetAllSites(searchSiteParameters);
                if (siteDTOs != null && siteDTOs.Count != 0)
                {
                    foreach (OrganizationDTO organizationDTO in organizationList)
                    {
                        organizationDTO.SiteList = siteDTOs.FindAll(m => m.OrgId == organizationDTO.OrgId).ToList();
                    }
                }
            }

            log.LogMethodExit(organizationList);
            return organizationList;
        }


        /// <summary>
        /// GetChildOrganizationListWithSiteByParentOrg
        /// </summary>
        /// <param name="parentOrganizationDTOList"></param>
        /// <param name="childOrganizationDTOs"></param>
        /// <returns>List of Child OrganizationDTOs</returns>
        public List<OrganizationDTO> GetChildOrganizationListWithSiteByParentOrg(List<OrganizationDTO> parentOrganizationDTOList, List<OrganizationDTO> childOrganizationDTOs)
        {
            log.LogMethodEntry(parentOrganizationDTOList);
            foreach (OrganizationDTO organizationDTO in parentOrganizationDTOList)
            {
                List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>> organizationSearchParams = new List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>>();
                organizationSearchParams.Add(new KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>(OrganizationDTO.SearchByOrganizationParameters.PARENT_ORG_ID, organizationDTO.OrgId.ToString()));
                List<OrganizationDTO> organizationDTOList = GetAllOrganizations(organizationSearchParams, true);
                if(organizationDTOList != null && organizationDTOList.Any())
                {
                    GetChildOrganizationListWithSiteByParentOrg(organizationDTOList, childOrganizationDTOs);
                }
                else
                {                    
                    SiteList siteList = new SiteList(executionContext);
                    List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchSiteParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                    List<SiteDTO> siteDTOs = siteList.GetAllSites(searchSiteParameters);
                    if (siteDTOs != null && siteDTOs.Count != 0)
                    {
                        organizationDTO.SiteList = siteDTOs.FindAll(m => m.OrgId == organizationDTO.OrgId).ToList();
                    }
                    childOrganizationDTOs.Add(organizationDTO);
                }
            }
            log.LogMethodExit(childOrganizationDTOs);
            return childOrganizationDTOs;
        }
    }
}
