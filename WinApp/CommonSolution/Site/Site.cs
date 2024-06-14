/********************************************************************************************
 * Project Name - Site
 * Description  - Bussiness logic of site
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Feb-2016   Raghuveera          Created 
 *2.40        23-Oct-2018   Jagan Mohan         Created method GetAllSitesUserLogedIn
 *2.60        08-Mar-2019   Jagan Mohan         Created method InsertUpdateSites() and SaveUpdateSiteList()
 *2.60.2      24-Apr-2019   Mushahid Faizan     Renamed InsertUpdateSites() to Save() and Modified SaveUpdateSiteList().
 *2.60.2      20-May-2019   Jagan Mohana        Created new method GetMaxSiteId();
 *2.60        08-May-2019   Nitin Pai           Created method GetHQSite for Guest app
 *2.70.2      23-Jul-2019   Deeksha             Modifications as per three tier standard.
 *3.0         11-Nov-2020   Mushahid Faizan     Modified : POS UI redesign with Rest API
 *2.120.1     26-May-2021   Deeksha             Modified as part of AWS Job Scheduler enhancements
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// Business Logic class for handling Site
    /// </summary>
    public class Site
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SiteDTO siteDTO;
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="siteDTO">siteDTO</param>
        public Site(ExecutionContext executionContext, SiteDTO siteDTO)
        {
            log.LogMethodEntry(executionContext, siteDTO);
            this.executionContext = executionContext;
            this.siteDTO = siteDTO;
            log.LogMethodExit();
        }
        public Site(ExecutionContext executionContext, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            this.executionContext = executionContext;
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            siteDTO = siteDataHandler.GetSite(siteId, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor which gets the siteDTO of passed guid
        /// </summary>
        /// <param name="guid"> guid of the site</param>
        /// <param name="sqlTransaction"> sqlTransction object</param>
        public Site(string guid, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(guid, sqlTransaction);
            this.executionContext = ExecutionContext.GetExecutionContext();
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);

            List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchBySiteParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
            searchBySiteParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.GUID, guid));
            List<SiteDTO> SiteDTOList = siteDataHandler.GetSiteList(searchBySiteParameters, sqlTransaction);
            if (SiteDTOList == null || (SiteDTOList != null && SiteDTOList.Count == 0))
            {
                siteDTO = new SiteDTO();
            }
            else
            {
                siteDTO = SiteDTOList[0];
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the site DTO of the passed siteId
        /// </summary>
        /// <param name="siteId">Site Id of the site</param>
        /// <param name="sqlTransaction">SqlTransction object</param>
        public Site(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            log.LogMethodExit();
            siteDTO = siteDataHandler.GetSite(siteId, sqlTransaction);
        }

        /// <summary>
        /// Loads the site DTO of the passed siteId
        /// </summary>
        /// <param name="siteId">Site Id of the site</param>
        /// <param name="sqlTransaction">SqlTransction object</param>
        public Site(int siteId, bool loadChild, bool activeChild, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            log.LogMethodExit();
            siteDTO = siteDataHandler.GetSite(siteId, sqlTransaction);
            if(loadChild)
            {
                Build(activeChild, sqlTransaction);
            }
        }

        /// <summary>
        /// Generate PriceListProductsDTO list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null) //added Build to get childRecords.
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            SiteDetailListBL siteDetailList = new SiteDetailListBL(executionContext);

            List<KeyValuePair<SiteDetailDTO.SearchByParameters, string>> searchContentMapParameters = new List<KeyValuePair<SiteDetailDTO.SearchByParameters, string>>();
            searchContentMapParameters.Add(new KeyValuePair<SiteDetailDTO.SearchByParameters, string>(SiteDetailDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            searchContentMapParameters.Add(new KeyValuePair<SiteDetailDTO.SearchByParameters, string>(SiteDetailDTO.SearchByParameters.PARENT_SITE_ID, Convert.ToString(siteDTO.SiteId)));
            siteDTO.SiteDetailDTOList = siteDetailList.GetSiteDetails(searchContentMapParameters, sqlTransaction);
            siteDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// UserId and siteId should come from the context.
        /// Insert Method with the list of site object as the parameter
        /// Would Insert the site object in the database. 
        /// </summary>        
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                SiteDataHandler siteHandler = new SiteDataHandler(sqlTransaction);
                if (siteDTO.SiteId < 0)
                {
                    siteDTO.SiteId = siteHandler.GetMaxSiteId();
                    siteDTO = siteHandler.InsertSite(siteDTO, executionContext.GetUserId());
                    siteDTO.AcceptChanges();
                    AddManagementFormAccess(sqlTransaction);
                }
                else
                {
                    if (siteDTO.IsChanged)
                    {
                        SiteDTO existingSiteDTO = new Site(siteDTO.SiteId, sqlTransaction).getSitedTO;
                        siteHandler.UpdateSite(siteDTO, executionContext.GetUserId());
                        siteDTO.AcceptChanges();
                        if (existingSiteDTO.SiteName.ToLower().ToString() != siteDTO.SiteName.ToLower().ToString())
                        {
                            RenameManagementFormAccess(existingSiteDTO.SiteName, sqlTransaction);
                        }
                        if (existingSiteDTO.IsActive != siteDTO.IsActive)
                        {
                            UpdateManagementFormAccess(siteDTO.SiteName, siteDTO.IsActive, siteDTO.Guid, sqlTransaction);
                        }
                    }
                }
                if (siteDTO.SiteDetailDTOList != null && siteDTO.SiteDetailDTOList.Any())
                {
                    SiteDetailListBL siteDetailListBL = new SiteDetailListBL(executionContext, siteDTO.SiteDetailDTOList);
                    siteDTO.SiteDetailDTOList = siteDetailListBL.Save(sqlTransaction);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        private void AddManagementFormAccess(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (siteDTO.SiteId > -1)
            {
                SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
                siteDataHandler.AddManagementFormAccess(siteDTO.SiteName, siteDTO.Guid, executionContext.GetSiteId(), siteDTO.IsActive);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void RenameManagementFormAccess(string existingFormName, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (siteDTO.SiteId > -1)
            {
                SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
                siteDataHandler.RenameManagementFormAccess(siteDTO.SiteName, existingFormName, executionContext.GetSiteId(), siteDTO.Guid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void UpdateManagementFormAccess(string formName, bool updatedIsActive, string functionGuid, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (siteDTO.SiteId > -1)
            {
                SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
                siteDataHandler.UpdateManagementFormAccess(formName, executionContext.GetSiteId(), updatedIsActive, functionGuid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Getting site DTO object
        /// </summary>
        public SiteDTO getSitedTO { get { return siteDTO; } }

    }
    /// <summary>
    /// Manages the list of site
    /// </summary>
    public class SiteList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<SiteDTO> siteList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SiteList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public SiteList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.siteList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="siteList">siteList</param>
        /// <param name="executionContext">executionContext</param>
        public SiteList(ExecutionContext executionContext, List<SiteDTO> siteList)
        {
            log.LogMethodEntry(siteList, executionContext);
            this.siteList = siteList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the site list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<SiteDTO> GetAllSites(List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters,
                                                SqlTransaction sqlTransaction = null, bool loadChildrecords = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            List<SiteDTO> siteDTOs = siteDataHandler.GetSiteList(searchParameters, sqlTransaction);
            if(siteDTOs != null && siteDTOs.Any() && loadChildrecords)
            {
                Build(siteDTOs, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(siteDTOs);
            return siteDTOs;
        }


        /// <summary>
        /// Returns the site list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<SiteDTO> GetAllSites(List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters,
                                                string connectionString)
        {
            log.LogMethodEntry(searchParameters, connectionString);
            SiteDataHandler siteDataHandler = new SiteDataHandler(connectionString);
            List<SiteDTO> siteDTOs = siteDataHandler.GetSiteList(searchParameters,null);
            log.LogMethodExit(siteDTOs);
            return siteDTOs;
        }


        /// <summary>
        /// Returns the siteDTO
        /// </summary>
        /// <param name="siteDataRow">siteDataRow</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>siteDTO</returns>
        public SiteDTO GetSiteDTO(DataRow siteDataRow, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteDataRow, sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            SiteDTO siteDTO = new SiteDTO();
            siteDTO = siteDataHandler.GetSiteDTO(siteDataRow);
            log.LogMethodExit(siteDTO);
            return siteDTO;
        }

        /// <summary>
        /// select the site list from the site table based on the parameter value
        /// </summary>        
        /// <param name="siteId"> Site id to exclude from the list else -1</param>
        /// <param name="orgId">site organization id or -1</param>
        /// <param name="companyId"> site company id or -1</param>
        /// <param name="sqlTransaction"> sqlTransaction</param>
        /// <returns>siteDTOs</returns>
        public List<SiteDTO> GetAllSites(int siteId, int orgId, int companyId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, orgId, companyId, sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            List<SiteDTO> siteDTOs = new List<SiteDTO>();
            siteDTOs = siteDataHandler.GetAllSite(siteId, orgId, companyId); ;
            log.LogMethodExit(siteDTOs);
            return siteDTOs;
        }

        /// <summary>
        /// Returns the roaming sites based on the current site id passed
        /// </summary>
        /// <param name="currentSiteId">current site id</param>
        /// <param name="sqlTransaction"> sqlTransaction</param>
        /// <returns>siteDTOList</returns>
        public List<SiteDTO> GetRoamingSites(int currentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(currentSiteId, sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            List<SiteDTO> siteDTOList = siteDataHandler.GetRoamingSites(currentSiteId);
            log.LogMethodExit(siteDTOList);
            return siteDTOList;
        }

        /// <summary>
        /// returns all the active sites except master site
        /// </summary>
        /// <param name="currentSiteId">current site id</param>
        /// <param name="sqlTransaction"> sqlTransaction</param>
        /// <returns>siteDTOList</returns>
        public List<SiteDTO> GetAllSitesForRoaming(int currentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(currentSiteId, sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            List<SiteDTO> siteDTOList = siteDataHandler.GetAllSitesForRoaming(currentSiteId);
            log.LogMethodExit(siteDTOList);
            return siteDTOList;
        }

        /// <summary>
        /// Get the site list based on the user loginId
        /// </summary>        
        /// <param name="loginId">loginId</param>
        /// <param name="sqlTransaction"> sqlTransaction</param>
        /// <returns>commonLookupDTOs</returns>
        public List<CommonLookupDTO> GetAllSitesUserLogedIn(string loginId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loginId, sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            List<CommonLookupDTO> commonLookupDTOs = new List<CommonLookupDTO>();
            commonLookupDTOs = siteDataHandler.GetAllSitesUserLogedIn(loginId); ;
            log.LogMethodExit(commonLookupDTOs);
            return commonLookupDTOs;
        }


        /// <summary>
        /// Builds the List of Site object based on the list of site id.
        /// </summary>
        /// <param name="siteDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<SiteDTO> siteDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, SiteDTO> siteDTOIdMap = new Dictionary<int, SiteDTO>();
            List<int> siteIdList = new List<int>();
            for (int i = 0; i < siteDTOList.Count; i++)
            {
                if (siteDTOIdMap.ContainsKey(siteDTOList[i].SiteId))
                {
                    continue;
                }
                siteDTOIdMap.Add(siteDTOList[i].SiteId, siteDTOList[i]);
                siteIdList.Add(siteDTOList[i].SiteId);
            }

            SiteDetailListBL siteDetailListBL = new SiteDetailListBL(executionContext);
            List<SiteDetailDTO> siteDetailDTOList = siteDetailListBL.GetSiteDetailDTOList(siteIdList, activeChildRecords, sqlTransaction);
            if (siteDetailDTOList != null && siteDetailDTOList.Any())
            {
                for (int i = 0; i < siteDetailDTOList.Count; i++)
                {
                    if (siteDTOIdMap.ContainsKey(siteDetailDTOList[i].ParentSiteId) == false)
                    {
                        continue;
                    }
                    SiteDTO siteDTO = siteDTOIdMap[siteDetailDTOList[i].ParentSiteId];
                    if (siteDTO.SiteDetailDTOList == null)
                    {
                        siteDTO.SiteDetailDTOList = new List<SiteDetailDTO>();
                    }
                    siteDTO.SiteDetailDTOList.Add(siteDetailDTOList[i]);
                }
            }
        }
        /// <summary>
        /// Save and Updated the sites details
        /// </summary>
        public void SaveUpdateSiteList()
        {
            log.LogMethodEntry();
            if (siteList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (SiteDTO siteDto in siteList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Site site = new Site(executionContext, siteDto);
                            site.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            log.LogMethodExit(null, "Throwing Exception : " + valEx.Message);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Details of the HQ Site
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <returns>HQDTO</returns>
        public SiteDTO GetMasterSiteFromHQ(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            SiteDTO HQDTO = siteDataHandler.GetMasterSiteFromHQ();
            log.LogMethodExit(HQDTO);
            return HQDTO;

        }

        public DateTime? GetSiteLastUpdateTime(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SiteDataHandler siteDataHandler = new SiteDataHandler(sqlTransaction);
            DateTime? result = siteDataHandler.GetSiteLastUpdateTime();
            log.LogMethodExit(result);
            return result;
        }
    }
}