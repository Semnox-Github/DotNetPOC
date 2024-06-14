/********************************************************************************************
 * Project Name - Asset Type
 * Description  - A high level structure created to classify the assets 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00       21-Dec-2015     Kiran           Created 
 *2.60        24-Mar-2019   Mehraj         Modified a default constructor AssetType() with Base()
                                           Added a constructor AssetType() with ExecutionContext and SaveAssetTypes()
 *2.70       04-Jul-2019     Dakshakh raj    Modified 
 *2.80       04-Apr-2020     Girish Kundar   Modified : Merge code from WMS for Cobra 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset type defines the various classification of assets
    /// Like building, machinery etc, to create a high level grouping of the asset
    /// </summary>
    public class AssetType
    {
       private AssetTypeDTO assetType;
       private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of AssetType class
        /// </summary>
        /// called in Asset mapper class
        public AssetType(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="assetType"></param>
        public AssetType(ExecutionContext executionContext, AssetTypeDTO assetType)
            : this(executionContext)
        {
            log.LogMethodEntry(assetType);
            this.assetType = assetType;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the asset type id as the parameter
        /// Would fetch the asset type object from the database based on the id passed. 
        /// </summary>
        /// <param name="assetTypeId">Asset Type id</param>
        public AssetType(ExecutionContext executionContext, int assetTypeId,SqlTransaction sqlTransaction = null)
             : this(executionContext)
        {
            log.LogMethodEntry(assetTypeId);
            AssetTypeDataHandler assetTypeDataHandler = new AssetTypeDataHandler(sqlTransaction);
            assetType = assetTypeDataHandler.GetAssetType(assetTypeId);
            if (assetType == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Asset Type", assetTypeId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(assetType);
        }

        /// <summary>
        /// Saves the asset type
        /// Checks if the assettype id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (assetType.IsChanged == false
                   && assetType.AssetTypeId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            AssetTypeDataHandler assetTypeDataHandler = new AssetTypeDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (assetType.AssetTypeId < 0)
            {
                assetType = assetTypeDataHandler.InsertAssetType(assetType, executionContext.GetUserId(), executionContext.GetSiteId());
                assetType.AcceptChanges();
            }
            else
            {
                if (assetType.IsChanged)
                {
                    assetType = assetTypeDataHandler.UpdateAssetType(assetType, executionContext.GetUserId(), executionContext.GetSiteId());
                    assetType.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            AssetTypeDataHandler assetTypeDataHandler = new AssetTypeDataHandler(sqlTransaction);
            List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> searchParams = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
            searchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<AssetTypeDTO> assetTypeDTOList = assetTypeDataHandler.GetAssetTypeList(searchParams);
            if (assetTypeDTOList != null && assetTypeDTOList.Any())
            {
                if (assetTypeDTOList.Exists(x => x.Name == assetType.Name) && assetType.AssetTypeId == -1)
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " asset type"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (assetTypeDTOList.Exists(x => x.Name == assetType.Name && x.AssetTypeId != assetType.AssetTypeId))
                {
                    log.Debug("Duplicate update entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " asset type"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AssetTypeDTO GetAssetType { get { return assetType; } }
    }

    /// <summary>
    /// Manages the list of asset types
    /// </summary>
    public class AssetTypeList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<AssetTypeDTO> assetTypesList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public AssetTypeList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.assetTypesList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="assetTypesList"></param>
        public AssetTypeList(ExecutionContext executionContext, List<AssetTypeDTO> assetTypesList)
        {
            log.LogMethodEntry(executionContext, assetTypesList);
            this.executionContext = executionContext;
            this.assetTypesList = assetTypesList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the asset type list
        /// </summary>
        public List<AssetTypeDTO> GetAllAssetTypes(List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> searchParameters,SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry(searchParameters);
            AssetTypeDataHandler assetTypeDataHandler = new AssetTypeDataHandler(sqlTransaction);
            log.LogMethodExit(searchParameters);
            return assetTypeDataHandler.GetAssetTypeList(searchParameters);
        }

        /// <summary>
        /// Returns the asset type list in a batch. Specifically for External systems
        /// </summary>
        public List<AssetTypeDTO> GetAllAssetTypesBatch(List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> searchParameters, int maxRows = int.MaxValue, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AssetTypeDataHandler assetTypeDataHandler = new AssetTypeDataHandler(sqlTransaction);
            List<AssetTypeDTO>  assetTypeDTOList = assetTypeDataHandler.GetAssetTypeListBatch(searchParameters, maxRows);
            log.LogMethodExit(assetTypeDTOList);
            return assetTypeDTOList;
        }

        /// <summary>
        /// Save AssetTypes
        /// </summary>
        public void SaveAssetTypes()
        {
            try
            {
                log.LogMethodEntry();
                if (assetTypesList != null && assetTypesList.Any())
                {
                    foreach (AssetTypeDTO assetTypeDTO in assetTypesList)
                    {
                        AssetType assetType = new AssetType(executionContext, assetTypeDTO);
                        assetType.Save();
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
    }
}
