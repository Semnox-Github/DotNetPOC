/********************************************************************************************
 * Project Name - DataAccessDetail BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
*2.70.2        15-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *3.0          05-Nov-2020      Mushahid Faizan        Modified : executionContext changes Save().
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class DataAccessDetail
    {
        private DataAccessDetailDTO dataAccessDetailDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="ruleDetailId"> id of the data access detail</param>
        public DataAccessDetail(ExecutionContext executionContext, int ruleDetailId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, ruleDetailId, sqlTransaction);
            this.executionContext = executionContext;
            DataAccessDetailDataHandler dataAccessDetailDataHandler = new DataAccessDetailDataHandler(sqlTransaction);
            dataAccessDetailDTO = dataAccessDetailDataHandler.GetDataAccessDetail(ruleDetailId);
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="dataAccessDetailDTO">Parameter of the type DataAccessDetailDTO</param>
        public DataAccessDetail(ExecutionContext executionContext, DataAccessDetailDTO dataAccessDetailDTO)
        {
            log.LogMethodEntry(dataAccessDetailDTO, executionContext);
            this.dataAccessDetailDTO = dataAccessDetailDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the data access details
        /// data access details will be inserted if ruleDetailId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            DataAccessDetailDataHandler dataAccessDetailDataHandler = new DataAccessDetailDataHandler(sqlTransaction);
          //EntityExclusionDetail entityExclusionDetail;
            if (dataAccessDetailDTO.RuleDetailId <= 0)
            {
                dataAccessDetailDTO = dataAccessDetailDataHandler.InsertDataAccessDetail(dataAccessDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                dataAccessDetailDTO.AcceptChanges();
            }
            else
            {
                if (dataAccessDetailDTO.IsChanged)
                {
                    dataAccessDetailDTO = dataAccessDetailDataHandler.UpdateDataAccessDetail(dataAccessDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    dataAccessDetailDTO.AcceptChanges();
                }
            }
            if (dataAccessDetailDTO.EntityExclusionDetailDTOList != null && dataAccessDetailDTO.EntityExclusionDetailDTOList.Count > 0)
            {
                foreach (EntityExclusionDetailDTO entityExclusionDetailDTO in dataAccessDetailDTO.EntityExclusionDetailDTOList)
                {
                    entityExclusionDetailDTO.RuleDetailId = dataAccessDetailDTO.RuleDetailId;
                    EntityExclusionDetail entityExclusionDetail = new EntityExclusionDetail(executionContext, entityExclusionDetailDTO);
                    entityExclusionDetail.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get the data access details DTO
        /// </summary>
        public DataAccessDetailDTO DataAccessDetailDTO { get { return dataAccessDetailDTO; } }
    }

    /// <summary>
    /// Manages the list of data access details
    /// </summary>
    public class DataAccessDetailList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Returns the data access detail list
        /// </summary>
        public DataAccessDetailDTO GetDataAccessDetail(int ruleDetailId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ruleDetailId, sqlTransaction);
            DataAccessDetailDataHandler dataAccessDetailDataHandler = new DataAccessDetailDataHandler(sqlTransaction);
            DataAccessDetailDTO dataAccessDetailDTO = dataAccessDetailDataHandler.GetDataAccessDetail(ruleDetailId);
            log.LogMethodExit(dataAccessDetailDTO);
            return dataAccessDetailDTO;
        }
        /// <summary>
        /// Returns the data access details list
        /// </summary>
        public List<DataAccessDetailDTO> GetAllDataAccessDetail(List<KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>> searchParameters, bool loadChildRecord = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DataAccessDetailDataHandler dataAccessDetailDataHandler = new DataAccessDetailDataHandler(sqlTransaction);
            List<DataAccessDetailDTO> dataAccessDetailDTOList = dataAccessDetailDataHandler.GetDataAccessDetailList(searchParameters);

            if (loadChildRecord && dataAccessDetailDTOList != null && dataAccessDetailDTOList.Any())
            {
                EntityExclusionDetailList entityExclusionDetailList = new EntityExclusionDetailList();
                List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>> searchEntityExclusionDetailParameters = new List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>>();
                searchEntityExclusionDetailParameters.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (loadActiveChildRecords)
                {
                    searchEntityExclusionDetailParameters.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.ACTIVE_FLAG, "1"));
                }
                List<EntityExclusionDetailDTO> entityExclusionDetailDTOList = entityExclusionDetailList.GetAllEntityExclusionDetail(searchEntityExclusionDetailParameters);

                foreach (DataAccessDetailDTO dataAccessDetailDTO in dataAccessDetailDTOList)
                {
                    if (entityExclusionDetailDTOList != null && entityExclusionDetailDTOList.Any())
                    {
                        dataAccessDetailDTO.EntityExclusionDetailDTOList = entityExclusionDetailDTOList.FindAll(m => m.RuleDetailId == dataAccessDetailDTO.RuleDetailId).ToList();
                    }
                }
            }
            log.LogMethodExit(dataAccessDetailDTOList);
            return dataAccessDetailDTOList;

        }
    }
}
