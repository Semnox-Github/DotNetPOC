/********************************************************************************************
 * Project Name - EntityExclusionDetail BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2         15-Jul-2019      Girish Kundar        Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
  *3.0          05-Nov-2020      Mushahid Faizan        Modified : executionContext changes in the constructor.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business logic for entity exclusion details
    /// </summary>
    public class EntityExclusionDetail
    {
        private EntityExclusionDetailDTO entityExclusionDetailDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        private EntityExclusionDetail(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="exclusionId"> id of the entity exclusion detail</param>
        public EntityExclusionDetail(ExecutionContext executionContext ,int exclusionId , SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(exclusionId, sqlTransaction);
            EntityExclusionDetailDataHandler entityExclusionDetailDataHandler = new EntityExclusionDetailDataHandler(sqlTransaction);
            entityExclusionDetailDTO = entityExclusionDetailDataHandler.GetEntityExclusionDetail(exclusionId);
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="entityExclusionDetailDTO">Parameter of the type EntityExclusionDetailDTO</param>
        public EntityExclusionDetail(ExecutionContext executionContext, EntityExclusionDetailDTO entityExclusionDetailDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(entityExclusionDetailDTO);
            this.entityExclusionDetailDTO = entityExclusionDetailDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the entity exclusion details
        /// entity exclusion details will be inserted if EntityExclusionDetailId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction); 
            EntityExclusionDetailDataHandler entityExclusionDetailDataHandler = new EntityExclusionDetailDataHandler(sqlTransaction);
            if (entityExclusionDetailDTO.ExclusionId <= 0)
            {
                entityExclusionDetailDTO = entityExclusionDetailDataHandler.InsertEntityExclusionDetail(entityExclusionDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                entityExclusionDetailDTO.AcceptChanges();
            }
            else
            {
                if (entityExclusionDetailDTO.IsChanged)
                {
                    entityExclusionDetailDTO = entityExclusionDetailDataHandler.UpdateEntityExclusionDetail(entityExclusionDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    entityExclusionDetailDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// get the entity exclusion details DTO
        /// </summary>
        public EntityExclusionDetailDTO EntityExclusionDetailDTO { get { return entityExclusionDetailDTO; } }
    }

    /// <summary>
    /// Manages the list of entity exclusion details
    /// </summary>
    public class EntityExclusionDetailList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the entity data
        /// </summary>
        public DataTable GetEntityData(string entityName, int entityFieldCount , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(entityName, entityFieldCount);
            EntityExclusionDetailDataHandler entityExclusionDetailDataHandler = new EntityExclusionDetailDataHandler(sqlTransaction);
            DataTable dtTable;
            if (entityFieldCount == 1)
            {
                dtTable = entityExclusionDetailDataHandler.GetEntityField(entityName);
            }
            else
            {
                dtTable = entityExclusionDetailDataHandler.GetEntityData(entityName);
            }
            log.LogMethodExit(dtTable);
            return dtTable;
        }

        /// <summary>
        /// Returns the entity exclusion detail list
        /// </summary>
        public EntityExclusionDetailDTO GetEntityExclusionDetail(int exclusionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(exclusionId, sqlTransaction);
            EntityExclusionDetailDataHandler entityExclusionDetailDataHandler = new EntityExclusionDetailDataHandler(sqlTransaction);
            EntityExclusionDetailDTO entityExclusionDetailDTO = entityExclusionDetailDataHandler.GetEntityExclusionDetail(exclusionId);
            log.LogMethodExit(entityExclusionDetailDTO);
            return entityExclusionDetailDTO;
        }
        /// <summary>
        /// Returns the entity exclusion details list
        /// </summary>
        public List<EntityExclusionDetailDTO> GetAllEntityExclusionDetail(List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            EntityExclusionDetailDataHandler entityExclusionDetailDataHandler = new EntityExclusionDetailDataHandler(sqlTransaction);
            List<EntityExclusionDetailDTO> entityExclusionDetailDTOList =   entityExclusionDetailDataHandler.GetEntityExclusionDetailList(searchParameters);
            log.LogMethodExit(entityExclusionDetailDTOList);
            return entityExclusionDetailDTOList;
        }
    }
}
