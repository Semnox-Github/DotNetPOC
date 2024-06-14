using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.logging;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.DBSynch
{
    /// <summary>
    /// Helper class to create roaming data
    /// </summary>
    public class DBSynchLogService
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private string tableName;
        private string entityGuid;
        private int entitySiteId;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="tableName">table name</param>
        /// <param name="entityGuid">guid of the entity</param>
        /// <param name="entitySiteId">siteid of the entity</param>
        public DBSynchLogService(ExecutionContext executionContext, string tableName, string entityGuid, int entitySiteId)
        {
            log.LogMethodEntry(executionContext, tableName, entityGuid);
            this.executionContext = executionContext;
            this.tableName = tableName;
            this.entityGuid = entityGuid;
            this.entitySiteId = entitySiteId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates roaming data
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void CreateRoamingDataForCustomer(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (executionContext.GetIsCorporate())
            {
                if (entitySiteId >= 0)
                {
                    SiteList siteList = new SiteList(executionContext);
                    List<SiteDTO> roamingSiteDTOList = null;
                    if(ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_ROAM_CUSTOMERS_ACROSS_ZONES"))
                    {
                        roamingSiteDTOList = siteList.GetAllSitesForRoaming(entitySiteId);
                    }
                    else
                    {
                        roamingSiteDTOList = siteList.GetRoamingSites(entitySiteId);
                    }
                    if (roamingSiteDTOList.Count > 0)
                    {
                        foreach (var siteDTO in roamingSiteDTOList)
                        {
                            if(siteDTO.SiteId != entitySiteId)
                            {
                                DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", entityGuid, tableName, DateTime.Now, siteDTO.SiteId);
                                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
                                dBSynchLogBL.Save(sqlTransaction);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates roaming data
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void CreateRoamingData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (executionContext.GetIsCorporate())
            {
                if (entitySiteId >= 0)
                {
                    SiteList siteList = new SiteList(executionContext);
                    List<SiteDTO> roamingSiteDTOList = null;
                    roamingSiteDTOList = siteList.GetRoamingSites(entitySiteId);
                    if (roamingSiteDTOList.Count > 0)
                    {
                        foreach (var siteDTO in roamingSiteDTOList)
                        {
                            if (siteDTO.SiteId != entitySiteId)
                            {
                                DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", entityGuid, tableName, DateTime.Now, siteDTO.SiteId);
                                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
                                dBSynchLogBL.Save(sqlTransaction);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates roaming data
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void CreateRoamingDataOnAllSites(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (executionContext.GetIsCorporate())
            {
                log.LogVariableState("GetIsCorporate ", true);
                log.LogVariableState("(entitySiteId ", entitySiteId);
                if (entitySiteId >= 0)
                {
                    SiteList siteList = new SiteList(executionContext);
                    List<SiteDTO> roamingSiteDTOList = null;
                    
                    roamingSiteDTOList = siteList.GetAllSitesForRoaming(entitySiteId);
                    log.LogVariableState("(roamingSiteDTOList.Count ", roamingSiteDTOList.Count);
                    log.LogVariableState("(tableName ", tableName);
                    if (roamingSiteDTOList.Count > 0)
                    {
                        foreach (var siteDTO in roamingSiteDTOList)
                        {
                            log.LogVariableState("(siteDTO.SiteId ", siteDTO.SiteId);
                            if (siteDTO.SiteId != entitySiteId)
                            {
                                DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", entityGuid, tableName, DateTime.Now, siteDTO.SiteId);
                                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
                                dBSynchLogBL.Save(sqlTransaction);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
