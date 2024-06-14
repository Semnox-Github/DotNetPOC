/********************************************************************************************
 * Project Name - Asset 
 * Description  - Class managing the assets. This has been created as part of the maintenance 
 * module. First the assets and the tasks are defined. Then the tasks are assigned to the assets
 * and user is assigned to the task. 
 * A schedule is defined that causes the tasks to recur and these tasks are then presented on 
 * various front end environments like the Parafait POS, tablet etc. 
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By    Remarks          
 *********************************************************************************************
 *1.00        21-Dec-2015   Kiran           Created 
 *2.70        07-Jul-2019   Dakshakh raj    Modified 
 *2.70        24-Apr-2019   Mehraj          Modified added Parameterized Constructor and ImportMachines()
 *2.80        14-May-2020   Mushahid Faizan Modified: ImportMachines()
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// GenericAsset is used to define the assets
    /// The amusement center/park could be defined as collection of assets
    /// This is then used for two purposes
    /// a. Asset register - To calculate the depreciation
    /// b. Task management - To define the tasks and assign it to the team members
    /// </summary>
    public class GenericAsset : AssetType
    {
        private GenericAssetDTO genericAssetDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private GenericAsset(ExecutionContext executionContext)
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public GenericAsset(ExecutionContext executionContext, GenericAssetDTO genericAssetDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, genericAssetDTO);
            this.genericAssetDTO = genericAssetDTO;
            log.LogMethodExit();
        }

        public GenericAsset(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AssetDataHandler assetDataHandler = new AssetDataHandler(sqlTransaction);
            genericAssetDTO = assetDataHandler.GetAsset(id);
            if (genericAssetDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Asset", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(genericAssetDTO);
        }
        /// <summary>
        /// Saves the asset 
        /// Checks if the asset id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (genericAssetDTO.IsChanged == false
                  && genericAssetDTO.AssetId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            AssetDataHandler assetDataHandler = new AssetDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (genericAssetDTO.AssetId < 0)
            {
                genericAssetDTO = assetDataHandler.InsertAsset(genericAssetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                genericAssetDTO.AcceptChanges();
            }
            else
            {
                if (genericAssetDTO.IsChanged)
                {
                    genericAssetDTO = assetDataHandler.UpdateAsset(genericAssetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    genericAssetDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            AssetDataHandler assetDataHandler = new AssetDataHandler(sqlTransaction);
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            searchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<GenericAssetDTO> genericAssetDTOList = assetDataHandler.GetGenericAssetsList(searchParams);
            if (genericAssetDTOList != null && genericAssetDTOList.Any())
            {
                if (genericAssetDTOList.Exists(x => x.Name == genericAssetDTO.Name && x.AssetId != genericAssetDTO.AssetId))
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " asset type/asset"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }

            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the generic asset DTO
        /// </summary>
        public GenericAssetDTO AssetDTO { get { return genericAssetDTO; } }
    }

    /// <summary>
    /// Manages the list of asset DTOs
    /// </summary>
    public class AssetList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<GenericAssetDTO> genericAssetDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AssetList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.genericAssetDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="genericAssetDTOList"></param>
        public AssetList(ExecutionContext executionContext, List<GenericAssetDTO> genericAssetDTOList)
        {
            log.LogMethodEntry(executionContext, executionContext);
            this.genericAssetDTOList = genericAssetDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// Returns the asset list
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public GenericAssetDTO GetAsset(int assetId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(assetId);
            AssetDataHandler assetDataHandler = new AssetDataHandler(sqlTransaction);
            log.LogMethodExit(assetDataHandler.GetAsset(assetId));
            return assetDataHandler.GetAsset(assetId);
        }
        /// <summary>
        /// Returns the asset list
        /// </summary>
        public List<GenericAssetDTO> GetAllAssets(List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AssetDataHandler assetDataHandler = new AssetDataHandler(sqlTransaction);
            List<GenericAssetDTO> genericAssetDTOList = assetDataHandler.GetGenericAssetsList(searchParameters);
            log.LogMethodExit(genericAssetDTOList);
            return genericAssetDTOList;
        }

        /// <summary>
        /// Returns the asset list in batch. Used for external systems.
        /// <param name="searchParameters">Parameter list for the select query</param>
        /// <param name="maxRows">Maximum Rows to be returned</param>
        /// </summary>
        public List<GenericAssetDTO> GetAllAssetsBatch(List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchParameters, int maxRows = int.MaxValue, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters , maxRows);
            AssetDataHandler assetDataHandler = new AssetDataHandler(sqlTransaction);
            List<GenericAssetDTO> genericAssetDTOList = assetDataHandler.GetGenericAssetListBatch(searchParameters, maxRows);
            log.LogMethodExit(genericAssetDTOList);
            return genericAssetDTOList;
        }
        /// <summary>
        /// Imports All Machines
        /// </summary>
        /// <param name="searchByMachineParameters"></param>
        /// <returns></returns>
        public string ImportMachines( List<MachineDTO> machineDTOList)
        {
            try
            {
                log.LogMethodEntry(/*searchByMachineParameters*/);
                int counter;
                string message = string.Empty;
                bool searchStatus = false;
                //MachineList machineList = new MachineList();
                //List<Semnox.Parafait.Game.MachineDTO> machineDTOList;
                //machineDTOList = machineList.GetMachineList(searchByMachineParameters);
                AssetTypeDTO assetTypeDTO = new AssetTypeDTO();
                List<AssetTypeDTO> assetTypeDTOList = new List<AssetTypeDTO>();
                AssetTypeList assetTypeList = new AssetTypeList(executionContext);
                List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> assetTypeSearchParams = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
                assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME, "Machine"));
                assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                assetTypeDTOList = assetTypeList.GetAllAssetTypes(assetTypeSearchParams);
                if (assetTypeDTOList == null)
                {
                    assetTypeDTO.Name = "Machine";
                    assetTypeDTO.IsActive = true;
                    AssetType assetType = new AssetType(executionContext, assetTypeDTO);
                    assetType.Save();
                }
                else
                {
                    assetTypeDTO = assetTypeDTOList[0];
                }
                List<GenericAssetDTO> genericAssetDTOList;
                GenericAsset genericAsset;
                AssetList assetList = new AssetList(executionContext);
                List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                genericAssetDTOList = assetList.GetAllAssets(assetSearchParams);
                counter = 0;
                if (machineDTOList != null)
                {
                    foreach (MachineDTO machineDTO in machineDTOList)
                    {
                        searchStatus = false;
                        if (genericAssetDTOList != null)
                        {
                            foreach (GenericAssetDTO assetDTO in genericAssetDTOList)
                            {
                                if (assetDTO.Machineid == machineDTO.MachineId)
                                {
                                    searchStatus = true;
                                    assetDTO.Name = machineDTO.MachineName;
                                    assetDTO.AssetTypeId = assetTypeDTO.AssetTypeId;
                                    assetDTO.IsActive = machineDTO.IsActive == "Y" ? true : false;
                                    genericAsset = new GenericAsset(executionContext, assetDTO);
                                    genericAsset.Save();
                                    counter++;
                                    continue;
                                }
                            }
                        }
                        if (!searchStatus)
                        {
                            GenericAssetDTO assetDTO = new GenericAssetDTO();
                            assetDTO.Name = machineDTO.MachineName;
                            assetDTO.AssetTypeId = assetTypeDTO.AssetTypeId;
                            assetDTO.IsActive = machineDTO.IsActive == "Y" ? true : false; 
                            assetDTO.Machineid = machineDTO.MachineId;
                            genericAsset = new GenericAsset(executionContext, assetDTO);
                            genericAsset.Save();
                            counter++;
                        }
                    }
                    if (counter > 0)
                    {
                        message =  MessageContainerList.GetMessage(executionContext, 964);
                        log.LogMethodExit(message);
                        return message;
                    }
                }
                message = MessageContainerList.GetMessage(executionContext, 965);
                log.LogMethodExit(message);
                return message;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Save AssetBatch
        /// </summary>
        public void SaveAssetList()
        {
            try
            {
                log.LogMethodEntry();
                if (genericAssetDTOList != null && genericAssetDTOList.Any())
                {
                    foreach (GenericAssetDTO genericAssetDTO in genericAssetDTOList)
                    {
                        GenericAsset genericAsset = new GenericAsset(executionContext, genericAssetDTO);
                        genericAsset.Save();
                    }
                }
                log.LogMethodExit();
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
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, ex.Message);
                throw;
            }
        }
            public Sheet BuildTemplate(bool isAssetLimited = false)
            {
                try
                {
                    log.LogMethodEntry();
                    Sheet sheet = new Sheet();
                    ///All column Headings are in a headerRow object
                    Row headerRow = new Row();
                    ///Mapper class thats map sheet object
                    GenericAssetDTODefination genericAssetDTODefination = new GenericAssetDTODefination(executionContext, "", isAssetLimited);
                    foreach (GenericAssetDTO genericAssetDTO in genericAssetDTOList)
                    {
                        genericAssetDTODefination.Configure(genericAssetDTO);
                    }
                    genericAssetDTODefination.BuildHeaderRow(headerRow);
                    sheet.AddRow(headerRow);
                    foreach (GenericAssetDTO genericAssetDTO in genericAssetDTOList)
                    {
                        Row row = new Row();
                        genericAssetDTODefination.Serialize(row, genericAssetDTO);
                        sheet.AddRow(row);
                    }
                    log.LogMethodExit();
                    return sheet;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
        }
    }
}
