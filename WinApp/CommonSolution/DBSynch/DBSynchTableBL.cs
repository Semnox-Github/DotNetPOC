/********************************************************************************************
 * Project Name - Utilities
 * Description  - Bussiness logic of Db synch
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        24-Mar-2016   Jagan Mohana   Created 
              09-Apr-2019   Mushahid Faizan Modified : SaveUpdateDbSynchList() method.
 *2.70        29-Jul-2019   Mushahid Faizan  Added Delete in Save Method for hard deletion.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.DBSynch
{
    public class DBSynchTableBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DBSynchTableDTO dBSynchDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public DBSynchTableBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="securityPolicyDTO"></param>
        public DBSynchTableBL(ExecutionContext executionContext, DBSynchTableDTO dBSynchDTO)
        {
            log.LogMethodEntry(executionContext, dBSynchDTO);
            this.executionContext = executionContext;
            this.dBSynchDTO = dBSynchDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the db synch
        /// Checks if the DbSynchId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            DBSynchTableDataHandler dBSynchDataHandler = new DBSynchTableDataHandler();
            if (dBSynchDTO.DbSynchId < 0)
            {
                int dbSynchId = dBSynchDataHandler.InsertDbSynch(dBSynchDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                dBSynchDTO.DbSynchId = dbSynchId;
            }
            else
            {
                if (dBSynchDTO.IsChanged)
                {
                    dBSynchDataHandler.UpdateDbSynch(dBSynchDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    dBSynchDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        public void CreateMasterData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                SiteList siteList = new SiteList(executionContext);
                int masterSiteId = siteList.GetMasterSiteFromHQ().SiteId;
                DBSynchTableDataHandler dBSynchDataHandler = new DBSynchTableDataHandler(sqlTransaction);
                dBSynchDataHandler.CreateMasterDataFromMasterSite(executionContext.GetSiteId(), masterSiteId, executionContext.GetUserId());
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete The Db Sync Table
        /// </summary>
        public void DeleteDbSyncTable()
        {
            try
            {
                DBSynchTableDataHandler dBSynchDataHandler = new DBSynchTableDataHandler();
                if (dBSynchDTO != null && dBSynchDTO.DbSynchId > 0)
                {
                    dBSynchDataHandler.Delete(dBSynchDTO.DbSynchId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Get the DTO
        /// </summary>
        public DBSynchTableDTO GetDBSynchDTO
        {
            get { return dBSynchDTO; }
        }
    }
    /// <summary>
    /// Manages the list of DBSynch
    /// </summary>
    public class DBSynchList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<DBSynchTableDTO> dbSynchDTOList;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public DBSynchList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.dbSynchDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="dbSynchDTODTOList"></param>
        /// <param name="executionContext"></param>
        public DBSynchList(ExecutionContext executionContext, List<DBSynchTableDTO> dbSynchDTOList)
        {
            log.LogMethodEntry(executionContext, dbSynchDTOList);
            this.dbSynchDTOList = dbSynchDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the db synch list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<DBSynchTableDTO> GetAllDBSynchList(List<KeyValuePair<DBSynchTableDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            DBSynchTableDataHandler dBSynchDataHandler = new DBSynchTableDataHandler();
            List<DBSynchTableDTO> dbSynchDTOList = dBSynchDataHandler.GetAllDBSynchList(searchParameters);
            log.LogMethodExit(dbSynchDTOList);
            return dbSynchDTOList;
        }
        /// <summary>
        /// This method should be used to Save and Update the DBSynch details for Web Management Studio.
        /// </summary>
        public void SaveUpdateDbSynchList()
        {
            log.LogMethodEntry();
            if (dbSynchDTOList != null && dbSynchDTOList.Count > 0)
            {
                foreach (DBSynchTableDTO dbSynchDto in dbSynchDTOList)
                {
                    try
                    {
                        DBSynchTableBL dBSynchBL = new DBSynchTableBL(executionContext, dbSynchDto);
                        dBSynchBL.Save();
                    }
                    catch (SqlException sqlEx)
                    {
                        log.Error(sqlEx);
                        log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                        if (sqlEx.Number == 547)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be used to Delete the DBSynch details for Web Management Studio.
        /// </summary>
        public void DeleteDbSynchList()
        {
            log.LogMethodEntry();
            try
            {
                if (dbSynchDTOList != null && dbSynchDTOList.Count > 0)
                {
                    foreach (DBSynchTableDTO dbSynchDto in dbSynchDTOList)
                    {
                        DBSynchTableBL dBSynchBL = new DBSynchTableBL(executionContext, dbSynchDto);
                        dBSynchBL.DeleteDbSyncTable();
                    }
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
    }
}