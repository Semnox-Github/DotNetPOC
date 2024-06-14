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
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        21-Dec-2015   Kiran          Created 
 *2.70        07-Jul-2019   Dakshakh raj   Modified 
 *2.70.3      02-Apr-2020   Girish Kundar  Modified : GetAllAssetGroupAsset() method return pattern
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes    
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// AssetGroupAssetMapper is used to map the asset groups to the assets
    /// </summary>
    public class AssetGroupAssetMapper : AssetType
    {
        private AssetGroupAssetDTO assetGroupAssetDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private AssetGroupAssetMapper(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.assetGroupAssetDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized Constructor
        /// </summary>
        /// <param name="assetGroupAssetDTO"></param>
        /// <param name="executionContext"></param>
        public AssetGroupAssetMapper(ExecutionContext executionContext, AssetGroupAssetDTO assetGroupAssetDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, assetGroupAssetDTO);
            this.assetGroupAssetDTO = assetGroupAssetDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the asset type id as the parameter
        /// Would fetch the asset type object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Asset Type id</param>
        public AssetGroupAssetMapper(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
             : this(executionContext)
        {
            log.LogMethodEntry(id);
            AssetGroupAssetDataHandler assetGroupAssetDataHandler = new AssetGroupAssetDataHandler(sqlTransaction);
            assetGroupAssetDTO = assetGroupAssetDataHandler.GetAssetGroupAsset(id);
            if (assetGroupAssetDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, MessageContainerList.GetMessage(executionContext,"Asset Type"), id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(assetGroupAssetDTO);
        }

        /// <summary>
        /// Saves the asset group asset
        /// Checks if the assetgroup id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (assetGroupAssetDTO.IsChanged == false
                   && assetGroupAssetDTO.AssetGroupAssetId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            AssetGroupAssetDataHandler assetGroupAssetDataHandler = new AssetGroupAssetDataHandler(sqlTransaction);
            if (assetGroupAssetDTO.AssetGroupAssetId < 0)
            {
                assetGroupAssetDTO = assetGroupAssetDataHandler.InsertAssetGroupAsset(assetGroupAssetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                assetGroupAssetDTO.AcceptChanges();
            }
            else
            {
                if (assetGroupAssetDTO.IsChanged == true)
                {
                    assetGroupAssetDTO= assetGroupAssetDataHandler.UpdateAssetGroupAsset(assetGroupAssetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    assetGroupAssetDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the generic asset DTO
        /// </summary>
        public AssetGroupAssetDTO AssetDTO { get { return assetGroupAssetDTO; } }
    }


    /// <summary>
    /// Manages the list of asset group asset mapper DTOs
    /// </summary>
    public class AssetGroupAssetMapperList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<AssetGroupAssetDTO> assetGroupAssetList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public AssetGroupAssetMapperList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.assetGroupAssetList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="assetGroupAssetList"></param>
        public AssetGroupAssetMapperList(ExecutionContext executionContext, List<AssetGroupAssetDTO> assetGroupAssetList)
        {
            log.LogMethodEntry(executionContext, assetGroupAssetList);
            this.executionContext = executionContext;
            this.assetGroupAssetList = assetGroupAssetList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the asset group - asset mapper list
        /// </summary>       
        public List<AssetGroupAssetDTO> GetAllAssetGroupAsset(List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AssetGroupAssetDataHandler assetGroupAssetDataHandler = new AssetGroupAssetDataHandler(sqlTransaction);
            List<AssetGroupAssetDTO> assetGroupAssetDTOList = assetGroupAssetDataHandler.GetAssetGroupAssetDTOList(searchParameters);
            log.LogMethodExit(assetGroupAssetDTOList);
            return assetGroupAssetDTOList;
        }

        public void SaveAssetGroupAsset()
        {
            try
            {
                log.LogMethodEntry();
                if (assetGroupAssetList != null && assetGroupAssetList.Any())
                {
                    foreach (AssetGroupAssetDTO assetGroupAssetDTO in assetGroupAssetList)
                    {
                        AssetGroupAssetMapper assetGroupAsset = new AssetGroupAssetMapper(executionContext, assetGroupAssetDTO);
                        assetGroupAsset.Save();
                    }
                }
                log.LogMethodExit();
            }

            catch (SqlException ex)
            {
                log.Error(ex);
                if (ex.Number == 2601 || ex.Number == 2627)
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
