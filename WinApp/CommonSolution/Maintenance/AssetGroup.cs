/********************************************************************************************
 * Project Name - Asset Group
 * Description  - Logical grouping of assets so that tasks can be assigned to the group
 * instead of assigning to each asset
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        21-Dec-2015   Kiran                   Created 
 *2.60        24-Apr-2019   Mehraj                  Modified Added AssetGroup()
 *2.70        07-Jul-2019   Dakshakh raj            Modified
 *2.70        24-Apr-2019   Mehraj                  Modified Added AssetGroup() and AssetGroupList() constructor,
                                                    Added SaveAssetGroups() method
 *2.70.3      02-Apr-2020   Girish Kundar           Modified : GetAllAssetGroups() method return pattern
 *2.80        10-May-2020   Girish Kundar           Modified: REST API Changes    
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
    /// Asset group is a construct created to allow grouping of assets
    /// This might be based on things like "proximity", "being serviced by one group" etc    
    /// </summary>
    public class AssetGroup
    {
        private AssetGroupDTO assetGroupDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        private AssetGroup(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            assetGroupDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="assetGroupDTO"></param>
        public AssetGroup(ExecutionContext executionContext, AssetGroupDTO assetGroupDTO)
        {
            log.LogMethodEntry(executionContext, assetGroupDTO);
            this.executionContext = executionContext;
            this.assetGroupDTO = assetGroupDTO;
            log.LogMethodExit();
        }

        public AssetGroup(ExecutionContext executionContext, int id,SqlTransaction sqlTransaction = null)
          : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AssetGroupDataHandler assetGroupDataHandler = new AssetGroupDataHandler(sqlTransaction);
            assetGroupDTO = assetGroupDataHandler.GetAssetGroup(id);
            if (assetGroupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Asset Group", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="assetGroupDTO">Asset Group DTO</param>
        public AssetGroup(AssetGroupDTO assetGroupDTO)
        {
            log.LogMethodEntry(assetGroupDTO);
            this.assetGroupDTO = assetGroupDTO;
            this.executionContext = ExecutionContext.GetExecutionContext();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the asset group
        /// Checks if the assetgroup id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (assetGroupDTO.IsChanged == false
                    && assetGroupDTO.AssetGroupId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            AssetGroupDataHandler assetGroupDataHandler = new AssetGroupDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (assetGroupDTO.AssetGroupId < 0)
            {
                assetGroupDTO = assetGroupDataHandler.InsertAssetGroup(assetGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                assetGroupDTO.AcceptChanges();
            }
            else
            {
                if (assetGroupDTO.IsChanged == true)
                {
                    assetGroupDTO = assetGroupDataHandler.UpdateAssetGroup(assetGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    assetGroupDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            AssetGroupDataHandler assetGroupDataHandler = new AssetGroupDataHandler(sqlTransaction);
            List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> searchParams = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
            searchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<AssetGroupDTO> assetGroupDTOList = assetGroupDataHandler.GetAssetGroupList(searchParams);
            if (assetGroupDTOList != null && assetGroupDTOList.Any())
            {
                if (assetGroupDTOList.Exists(x => x.AssetGroupName == assetGroupDTO.AssetGroupName ) && assetGroupDTO.AssetGroupId == -1)
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " asset group"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (assetGroupDTOList.Exists(x => x.AssetGroupName == assetGroupDTO.AssetGroupName && x.AssetGroupId != assetGroupDTO.AssetGroupId))
                {
                    log.Debug("Duplicate update entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " asset group"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AssetGroupDTO GetAssetGroupDTO { get { return assetGroupDTO; } }
    }

    /// <summary>
    /// Manages the list of asset groups
    /// </summary>
    public class AssetGroupList
    {
         private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<AssetGroupDTO> assetGroupList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public AssetGroupList(ExecutionContext executionContext)
        {
            log.LogMethodExit(executionContext);
            this.executionContext = executionContext;
            this.assetGroupList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized  Constructor
        /// </summary>
        /// <param name="assetGroupList"></param>
        /// <param name="executionContext"></param>
        public AssetGroupList(List<AssetGroupDTO> assetGroupList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(assetGroupList, executionContext);
            this.assetGroupList = assetGroupList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the asset group list
        /// </summary>
        public List<AssetGroupDTO> GetAllAssetGroups(List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> searchParameters,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters);
            AssetGroupDataHandler assetGroupDataHandler = new AssetGroupDataHandler(sqlTransaction);
            List<AssetGroupDTO> assetGroupDTOList = assetGroupDataHandler.GetAssetGroupList(searchParameters);
            log.LogMethodExit(assetGroupDTOList);
            return assetGroupDTOList;
        }

        /// <summary>
        /// Saves the AssetGroups collection
        /// </summary>
        public void SaveAssetGroups()
        {
            try
            {
                log.LogMethodEntry();
                if (assetGroupList != null && assetGroupList.Any())
                {
                    foreach (AssetGroupDTO assetGroupDTO in assetGroupList)
                    {
                        AssetGroup assetType = new AssetGroup(executionContext, assetGroupDTO);
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
                throw ;
            }
        }
    }
}
