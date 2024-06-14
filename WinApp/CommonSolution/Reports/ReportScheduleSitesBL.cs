/********************************************************************************************
 * Project Name - Reports
 * Description  - BL Logic for ReportScheduleSites
 * 
 **************
 **Version Log
 **************
 *Version        Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110        04-Jan-2021   Laster Menezes      Created 
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportScheduleSites BL
    /// </summary>
    public class ReportScheduleSitesBL
    {
        private ReportScheduleSitesDTO reportScheduleSitesDTO;
        private readonly ExecutionContext executionContext;
        string connectionString = string.Empty;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set method of the ConnectionString field
        /// </summary>
        public string ConnectionString { get { return connectionString; } set { connectionString = value; } }

        /// <summary>
        /// ReportScheduleSitesBL object using the reportScheduleSitesDTO
        /// </summary>
        /// <param name="executionContext"></param>
        private ReportScheduleSitesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ReportScheduleSitesBL object using the reportScheduleSitesDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="reportScheduleSitesDTO"></param>
        public ReportScheduleSitesBL(ExecutionContext executionContext, ReportScheduleSitesDTO reportScheduleSitesDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, reportScheduleSitesDTO);
            this.reportScheduleSitesDTO = reportScheduleSitesDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (reportScheduleSitesDTO.IsChanged == false
                && reportScheduleSitesDTO.ReportScheduleSitesId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ReportScheduleSitesDataHandler reportScheduleSitesDataHandler = new ReportScheduleSitesDataHandler(sqlTransaction);
            if (reportScheduleSitesDTO.ReportScheduleSitesId < 0)
            {
                reportScheduleSitesDTO = reportScheduleSitesDataHandler.Insert(reportScheduleSitesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                reportScheduleSitesDTO.AcceptChanges();
            }
            else if (reportScheduleSitesDTO.IsChanged)
            {
                reportScheduleSitesDTO = reportScheduleSitesDataHandler.Update(reportScheduleSitesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                reportScheduleSitesDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            ReportScheduleSitesDataHandler reportScheduleSitesDataHandler = new ReportScheduleSitesDataHandler(sqlTransaction);
            if (reportScheduleSitesDTO.ReportScheduleSitesId > 0)
            {
                reportScheduleSitesDataHandler.Delete(reportScheduleSitesDTO);
                reportScheduleSitesDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        public ReportScheduleSitesDTO ReportScheduleSitesDTO
        {
            get
            {
                return ReportScheduleSitesDTO;
            }
        }

    }


    public class ReportScheduleSitesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ReportScheduleSitesDTO> reportScheduleSitesDTOList = new List<ReportScheduleSitesDTO>();

        /// <summary>
        /// Parameterized constructor of ReportScheduleSitesListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public ReportScheduleSitesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="promotionRuleDTOList">ReportScheduleSitesDTO List as parameter </param>
        public ReportScheduleSitesListBL(ExecutionContext executionContext, List<ReportScheduleSitesDTO> reportScheduleSitesDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, reportScheduleSitesDTOList);
            this.reportScheduleSitesDTOList = reportScheduleSitesDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        ///  Returns the Get the ReportScheduleSites DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of ReportScheduleSitesDTO </returns>
        public List<ReportScheduleSitesDTO> GetReportScheduleSitesDTOList(List<KeyValuePair<ReportScheduleSitesDTO.SearchByParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReportScheduleSitesDataHandler reportScheduleSitesDataHandler = new ReportScheduleSitesDataHandler(sqlTransaction);
            List<ReportScheduleSitesDTO> reportScheduleSitesDTOList = reportScheduleSitesDataHandler.GetReportScheduleSitesDTOList(searchParameters);
            log.LogMethodExit(reportScheduleSitesDTOList);
            return reportScheduleSitesDTOList;
        }



        /// <summary>
        /// Saves the  list of ReportScheduleSitesDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (reportScheduleSitesDTOList == null ||
                reportScheduleSitesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < reportScheduleSitesDTOList.Count; i++)
            {
                ReportScheduleSitesDTO reportScheduleSitesDTO = reportScheduleSitesDTOList[i];
                if (reportScheduleSitesDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ReportScheduleSitesBL reportScheduleSitesBL = new ReportScheduleSitesBL(executionContext, reportScheduleSitesDTO);
                    reportScheduleSitesBL.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving ReportScheduleSitesDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ReportScheduleSitesDTO", reportScheduleSitesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (reportScheduleSitesDTOList == null ||
                reportScheduleSitesDTOList.Count > 0 == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < reportScheduleSitesDTOList.Count; i++)
            {
                ReportScheduleSitesDTO reportScheduleSitesDTO = reportScheduleSitesDTOList[i];
                try
                {
                    ReportScheduleSitesBL reportScheduleSitesBL = new ReportScheduleSitesBL(executionContext, reportScheduleSitesDTO);
                    reportScheduleSitesBL.Delete(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
            }
        }



        /// <summary>
        /// GetScheduleSiteDTOListByScheduleID
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns>SiteDTO List based on scheduleId</returns>
        public List<SiteDTO> GetScheduleSiteDTOListByScheduleID(int scheduleId)
        {
            log.LogMethodEntry(scheduleId);
            List<SiteDTO> scheduleSiteDTOs = new List<SiteDTO>();
            ReportScheduleSitesDataHandler reportScheduleSitesDataHandler = new ReportScheduleSitesDataHandler();
            List<KeyValuePair<ReportScheduleSitesDTO.SearchByParameters, string>> reportScheduleSitesSearchParams = new List<KeyValuePair<ReportScheduleSitesDTO.SearchByParameters, string>>();
            reportScheduleSitesSearchParams.Add(new KeyValuePair<ReportScheduleSitesDTO.SearchByParameters, string>(ReportScheduleSitesDTO.SearchByParameters.SCHEDULE_ID, scheduleId.ToString()));
            reportScheduleSitesSearchParams.Add(new KeyValuePair<ReportScheduleSitesDTO.SearchByParameters, string>(ReportScheduleSitesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<ReportScheduleSitesDTO> reportScheduleSitesDTOList = reportScheduleSitesDataHandler.GetReportScheduleSitesDTOList(reportScheduleSitesSearchParams);
            if(reportScheduleSitesDTOList != null && reportScheduleSitesDTOList.Any())
            {
                foreach(ReportScheduleSitesDTO reportScheduleSitesDTO in reportScheduleSitesDTOList)
                {
                    if (reportScheduleSitesDTO.ReportScheduleSitesOrgId != -1)
                    {
                        List<OrganizationDTO> childOrganizationDTOs = new List<OrganizationDTO>();
                        OrganizationList organizationList = new OrganizationList();
                        List<OrganizationDTO> organizationDTOList = new List<OrganizationDTO>();
                        List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>> organizationSearchParams = new List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>>();
                        organizationSearchParams.Add(new KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>(OrganizationDTO.SearchByOrganizationParameters.ORG_ID, reportScheduleSitesDTO.ReportScheduleSitesOrgId.ToString()));
                        organizationDTOList = organizationList.GetAllOrganizations(organizationSearchParams,true);

                        if (organizationDTOList != null && organizationDTOList.Any())
                        {
                            childOrganizationDTOs = organizationList.GetChildOrganizationListWithSiteByParentOrg(organizationDTOList, childOrganizationDTOs);
                        }

                        if (childOrganizationDTOs != null && childOrganizationDTOs.Any())
                        {
                            foreach (OrganizationDTO organizationDTO in childOrganizationDTOs)
                            {
                                if (organizationDTO.SiteList != null && organizationDTO.SiteList.Any())
                                {
                                    foreach (SiteDTO siteDTO in organizationDTO.SiteList)
                                    {
                                        scheduleSiteDTOs.Add(siteDTO);
                                    }
                                }
                            }
                        }
                    }
                    else if (reportScheduleSitesDTO.ReportScheduleSitesSiteId != -1)
                    {
                        SiteList siteList = new SiteList(executionContext);
                        List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchSiteParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                        searchSiteParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, reportScheduleSitesDTO.ReportScheduleSitesSiteId.ToString()));
                        List<SiteDTO> siteDTOList = siteList.GetAllSites(searchSiteParameters);
                        foreach(SiteDTO siteDTO in siteDTOList)
                        {
                            scheduleSiteDTOs.Add(siteDTO);
                        }
                    }
                }
            }
            log.LogMethodExit(scheduleSiteDTOs);
            return scheduleSiteDTOs;
        }       
    }
}
