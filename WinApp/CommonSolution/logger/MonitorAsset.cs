/********************************************************************************************
 * Project Name - Monitor Asset
 * Description  - Bussiness logic of monitor asset
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016   Raghuveera          Created
 *2.60        03-May-2019   Jagan Mohana Rao    Created SaveUpdateMonitorAsset() and changes log method entry and exit
 *2.60.2      17-June-2019  Jagan Mohana        Created the DeleteMonitorAsset and Delete methods
  *2.90        18-Jun-2020    Mushahid Faizan    Modified : 3 tier changes for Rest API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Monitor asset will creates and modifies the assets
    /// </summary>
    public class MonitorAsset
    {
        private MonitorAssetDTO monitorAssetDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private MonitorAsset(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="monitorAssetDTO"></param>
        public MonitorAsset(ExecutionContext executionContext, MonitorAssetDTO monitorAssetDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, monitorAssetDTO);
            this.monitorAssetDTO = monitorAssetDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the MonitorAsset monitorAssetId as the parameter
        /// Would fetch the monitorAssetDTO object from the database based on the monitorAssetId passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="monitorAssetId">Id</param>
        public MonitorAsset(ExecutionContext executionContext, int monitorAssetId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, monitorAssetId, sqlTransaction);
            MonitorAssetDataHandler monitorAssetDataHandler = new MonitorAssetDataHandler(sqlTransaction);
            monitorAssetDTO = monitorAssetDataHandler.GetMonitorAsset(monitorAssetId);

            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the monitor asset
        /// asset will be inserted if assetid is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            MonitorAssetDataHandler monitorAssetDataHandler = new MonitorAssetDataHandler(sqlTransaction);
            if (monitorAssetDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (monitorAssetDTO.AssetId <= 0)
            {
                monitorAssetDTO = monitorAssetDataHandler.InsertMonitorAsset(monitorAssetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                monitorAssetDTO.AcceptChanges();
            }
            else if (monitorAssetDTO.IsChanged)
            {
                monitorAssetDTO = monitorAssetDataHandler.UpdateMonitorAsset(monitorAssetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                monitorAssetDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the monitorAsset records from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete(MonitorAssetDTO monitorAssetDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(monitorAssetDTO, sqlTransaction);
            try
            {
                MonitorAssetDataHandler monitorAssetDataHandler = new MonitorAssetDataHandler(sqlTransaction);
                if (monitorAssetDTO.AssetId >= 0)
                {
                    monitorAssetDataHandler.DeleteMonitorAsset(monitorAssetDTO.AssetId);
                }
                monitorAssetDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (monitorAssetDTO == null)
            {
                //Validation to be implemented.
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// get monitorAssetDTO Object
        /// </summary>
        public MonitorAssetDTO GetMonitorAssetDTO
        {
            get { return monitorAssetDTO; }
        }

    }
    /// <summary>
    /// Manages the list of monitor asset
    /// </summary>
    public class MonitorAssetList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MonitorAssetDTO> monitorAssetDTOList = new List<MonitorAssetDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MonitorAssetList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="monitorAssetDTOList"></param>
        /// <param name="executionContext"></param>
        public MonitorAssetList(ExecutionContext executionContext, List<MonitorAssetDTO> monitorAssetDTOList) : this(executionContext)
        {
            log.LogMethodEntry(monitorAssetDTOList);
            this.monitorAssetDTOList = monitorAssetDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the monitor asset list
        /// </summary>
        public List<MonitorAssetDTO> GetAllMonitorAssets(List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            MonitorAssetDataHandler monitorAssetDataHandler = new MonitorAssetDataHandler(sqlTransaction);
            log.LogMethodExit();
            monitorAssetDTOList = monitorAssetDataHandler.GetMonitorAssetList(searchParameters);
            return monitorAssetDTOList;
        }


        /// <summary>
        /// Saves the  list of monitorAssetDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (monitorAssetDTOList == null ||
                monitorAssetDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < monitorAssetDTOList.Count; i++)
            {
                var monitorAssetDTO = monitorAssetDTOList[i];
                if (monitorAssetDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    MonitorAsset monitorAsset = new MonitorAsset(executionContext, monitorAssetDTO);
                    monitorAsset.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving monitorAssetDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("monitorAssetDTO", monitorAssetDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the monitorAssetDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (monitorAssetDTOList != null && monitorAssetDTOList.Any())
            {
                foreach (MonitorAssetDTO monitorAssetDTO in monitorAssetDTOList)
                {
                    if (monitorAssetDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                MonitorAsset monitorAsset = new MonitorAsset(executionContext, monitorAssetDTO);
                                monitorAsset.Delete(monitorAssetDTO, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
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
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}

