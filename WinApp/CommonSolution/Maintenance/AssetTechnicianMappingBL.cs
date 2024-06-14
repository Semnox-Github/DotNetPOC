/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Business Logic of the MaintenanceJobStatus class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0        23-Sept-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Maintenance
{
    public class AssetTechnicianMappingBL
    {
        private AssetTechnicianMappingDTO assetTechnicianMappingDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private AssetTechnicianMappingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        ///<summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="assetTechnicianMappingDTO"></param>
        public AssetTechnicianMappingBL(ExecutionContext executionContext, AssetTechnicianMappingDTO assetTechnicianMappingDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, assetTechnicianMappingDTO);
            this.executionContext = executionContext;
            this.assetTechnicianMappingDTO = assetTechnicianMappingDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the assetTechnicianMappingId as the parameter
        /// Would fetch the assetTechnicianMappingDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="assetTechnicianMappingId">Id</param>
        public AssetTechnicianMappingBL(ExecutionContext executionContext, int assetTechnicianMappingId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, assetTechnicianMappingId, sqlTransaction);
            AssetTechnicianMappingDataHandler assetTechnicianMappingDataHandler = new AssetTechnicianMappingDataHandler(sqlTransaction);
            assetTechnicianMappingDTO = assetTechnicianMappingDataHandler.GetAssetTechnicianMappingDTO(assetTechnicianMappingId);
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the assetTechnicianMappingDTO
        /// asset will be inserted if assetTechnicianMappingId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AssetTechnicianMappingDataHandler assetTechnicianMappingDataHandler = new AssetTechnicianMappingDataHandler(sqlTransaction);
            if (assetTechnicianMappingDTO.IsChanged == false &&
                assetTechnicianMappingDTO.MapId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
            if (assetTechnicianMappingDTO.MapId < 0)
            {
                assetTechnicianMappingDTO = assetTechnicianMappingDataHandler.InsertAssetTechnicianMapping(assetTechnicianMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                assetTechnicianMappingDTO.AcceptChanges();
            }
            else if (assetTechnicianMappingDTO.IsChanged)
            {
                assetTechnicianMappingDTO = assetTechnicianMappingDataHandler.UpdateAssetTechnicianMapping(assetTechnicianMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                assetTechnicianMappingDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the assetTechnicianMappingDTO
        /// </summary>
        /// <returns></returns>

        private List<ValidationError> Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            AssetTechnicianMappingDataHandler assetTechnicianMappingDataHandler = new AssetTechnicianMappingDataHandler(sqlTransaction);
            List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            //searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.IS_ACTIVE, "1"));

            bool isSiteLevel = false;

            if (assetTechnicianMappingDTO.AssetId > -1)
            {
                searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.ASSET_ID, assetTechnicianMappingDTO.AssetId.ToString()));
            }
            else if (assetTechnicianMappingDTO.AssetTypeId > -1)
            {
                searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.ASSET_TYPE_ID, assetTechnicianMappingDTO.AssetTypeId.ToString()));
            } else
            {
                isSiteLevel = true;
            }

            List<AssetTechnicianMappingDTO> assetTechnicianMappingDTOList = null; 
           
            if(isSiteLevel)
            {
                assetTechnicianMappingDTOList = assetTechnicianMappingDataHandler.GetSiteTechnicianMappingDTOList(searchParams);
            } else
            {
                assetTechnicianMappingDTOList = assetTechnicianMappingDataHandler.GetAllTechnicianMappingDTOList(searchParams);
            }


            if (assetTechnicianMappingDTOList != null && assetTechnicianMappingDTOList.Any())
            {
                if (assetTechnicianMappingDTO.MapId > 0)
                {
                    if (assetTechnicianMappingDTOList.FindAll(x => x.UserId == assetTechnicianMappingDTO.UserId).Count > 1)
                    {
                        log.Debug("Error: Duplicate records.  Selected user is already present in Technician Mapping.");
                        validationErrorList.Add(new ValidationError("TechnicianMapping", "User", MessageContainerList.GetMessage(executionContext, 2874, MessageContainerList.GetMessage(executionContext, "TechnicianMapping"))));
                    }
                } else
                {
                    if (assetTechnicianMappingDTOList.FindAll(x => x.UserId == assetTechnicianMappingDTO.UserId).Count > 0)
                    {
                        log.Debug("Error: Duplicate records.  Selected user is already present in Technician Mapping.");
                        validationErrorList.Add(new ValidationError("TechnicianMapping", "User", MessageContainerList.GetMessage(executionContext, 2874, MessageContainerList.GetMessage(executionContext, "TechnicianMapping"))));
                    }
                }

                if (assetTechnicianMappingDTO.IsPrimary)
                {
                    AssetTechnicianMappingDTO primaryMappingDTO = assetTechnicianMappingDTOList.Find(x => x.IsPrimary == true);
                    if (primaryMappingDTO != null && assetTechnicianMappingDTO.MapId != primaryMappingDTO.MapId)
                    {
                        log.Debug("Error: Duplicate records.  There can be only one technician marked as parimary for an Asset/Asset Aype or Site.");
                        validationErrorList.Add(new ValidationError("TechnicianMapping", "User", MessageContainerList.GetMessage(executionContext, 2873, MessageContainerList.GetMessage(executionContext, "TechnicianMapping"))));
                    }
                }

            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


        /// <summary>
        /// get AssetTechnicianMappingDTO Object
        /// </summary>
        public AssetTechnicianMappingDTO GetAssetTechnicianMappingDTO
        {
            get { return assetTechnicianMappingDTO; }
        }

        /// <summary>
        /// set AssetTechnicianMappingDTO Object        
        /// </summary>
        public AssetTechnicianMappingDTO SetAssetTechnicianMappingDTO
        {
            set { assetTechnicianMappingDTO = value; }
        }
    }

    /// <summary>
    /// Manages the list of AssetTechnicianMappingBL
    /// </summary>
    public class AssetTechnicianMappingListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AssetTechnicianMappingDTO> assetTechnicianMappingDTOList = new List<AssetTechnicianMappingDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AssetTechnicianMappingListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="assetTechnicianMappingDTO"></param>
        public AssetTechnicianMappingListBL(ExecutionContext executionContext, List<AssetTechnicianMappingDTO> assetTechnicianMappingDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, assetTechnicianMappingDTOList);
            this.assetTechnicianMappingDTOList = assetTechnicianMappingDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AssetTechnicianMappingDTO  list
        /// </summary>
        public List<AssetTechnicianMappingDTO> GetAllAssetTechnicianMappingList(List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AssetTechnicianMappingDataHandler assetTechnicianMappingDataHandler = new AssetTechnicianMappingDataHandler(sqlTransaction);
            assetTechnicianMappingDTOList = assetTechnicianMappingDataHandler.GetAssetTechnicianMappingDTOList(searchParameters);
            log.LogMethodExit();
            return assetTechnicianMappingDTOList;
        }

        public List<AssetTechnicianMappingDTO> GetAllAssetTypeTechnicianMappingList(List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AssetTechnicianMappingDataHandler assetTechnicianMappingDataHandler = new AssetTechnicianMappingDataHandler(sqlTransaction);
            assetTechnicianMappingDTOList = assetTechnicianMappingDataHandler.GetAssetTypeTechnicianMappingDTOList(searchParameters);
            log.LogMethodExit();
            return assetTechnicianMappingDTOList;
        }

        public List<AssetTechnicianMappingDTO> GetAllSiteTechnicianMappingList(List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AssetTechnicianMappingDataHandler assetTechnicianMappingDataHandler = new AssetTechnicianMappingDataHandler(sqlTransaction);
            assetTechnicianMappingDTOList = assetTechnicianMappingDataHandler.GetSiteTechnicianMappingDTOList(searchParameters);
            log.LogMethodExit();
            return assetTechnicianMappingDTOList;
        }

        public DateTime? GetAssetTechnicianMappingTime(int siteId, SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(siteId);
            AssetTechnicianMappingDataHandler assetTechnicianMappingDataHandler = new AssetTechnicianMappingDataHandler(sqlTransaction);
            DateTime? result = assetTechnicianMappingDataHandler.GetAssetTechnicianMappingTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Saves the AssetTechnicianMappingDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (assetTechnicianMappingDTOList == null ||
                assetTechnicianMappingDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < assetTechnicianMappingDTOList.Count; i++)
            {
                var assetTechnicianMappingDTO = assetTechnicianMappingDTOList[i];
                if (assetTechnicianMappingDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AssetTechnicianMappingBL assetTechnicianMappingBL = new AssetTechnicianMappingBL(executionContext, assetTechnicianMappingDTO);
                    assetTechnicianMappingBL.Save(sqlTransaction);
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
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving assetTechnicianMappingDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("assetTechnicianMappingDTO", assetTechnicianMappingDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
