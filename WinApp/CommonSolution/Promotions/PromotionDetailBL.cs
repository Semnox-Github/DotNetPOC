/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  PromotionDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      24-Jun-2019     Girish Kundar           Created 
 *2.80      08-Apr-2020     Mushahid Faizan         Modified : 3 tier changes for Rest API.
 *2.130     08-Oct-2021     Girish Kundar           Modified : Added DB Audit log for promotion detail as Redeem changes.
 *2.130.8   14-Apr-2022     Abhishek                Modified : Added GameAttributes methods as a part of Promotion Game Attribute Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;


namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Business logic for PromotionDetail class.
    /// </summary>
    public class PromotionDetailBL
    {
        private PromotionDetailDTO promotionDetailDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PromotionDetailBL class
        /// </summary>
        private PromotionDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates PromotionDetailBL object using the PromotionDetail DTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="promotionDetailDTO">PromotionDetail DTO object</param>
        public PromotionDetailBL(ExecutionContext executionContext, PromotionDetailDTO promotionDetailDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, promotionDetailDTO);
            this.promotionDetailDTO = promotionDetailDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the PromotionDetail  id as the parameter
        /// Would fetch the PromotionDetail object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -PromotionDetail </param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PromotionDetailBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            PromotionDetailDataHandler promotionDetailDataHandler = new PromotionDetailDataHandler(sqlTransaction);
            promotionDetailDTO = promotionDetailDataHandler.GetPromotionDetailDTO(id);
            if (promotionDetailDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PromotionDetailDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PromotionDetail DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (promotionDetailDTO.IsChanged == false
                && promotionDetailDTO.PromotionDetailId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            PromotionDetailDataHandler promotionDetailDataHandler = new PromotionDetailDataHandler(sqlTransaction);
            if (promotionDetailDTO.IsActive == true)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (promotionDetailDTO.PromotionDetailId < 0)
                {
                    log.LogVariableState("PromotionDetailDTO", promotionDetailDTO);
                    promotionDetailDTO = promotionDetailDataHandler.Insert(promotionDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(promotionDetailDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("promotion_detail", promotionDetailDTO.Guid, sqlTransaction);
                        
                    }
                    promotionDetailDTO.AcceptChanges();
                }
                else if (promotionDetailDTO.IsChanged)
                {
                    log.LogVariableState("PromotionDetailDTO", promotionDetailDTO);
                    promotionDetailDTO = promotionDetailDataHandler.Update(promotionDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(promotionDetailDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("promotion_detail", promotionDetailDTO.Guid, sqlTransaction);
                       
                    }
                    promotionDetailDTO.AcceptChanges();
                }

            }
            else  
            {
                if (promotionDetailDTO.PromotionDetailId >= 0)
                {
                    PromotionDetailListBL promotionDetailListBL = new PromotionDetailListBL(executionContext);
                    List<MachineAttributeDTO> machineAttributeDTOList = promotionDetailListBL.GetPromotionDetailGameAttributes(promotionDetailDTO.PromotionDetailId)
                        .Where(x => x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL).ToList();
                    if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
                    {
                        foreach (MachineAttributeDTO machineAttributeDTO in machineAttributeDTOList)
                        {
                            promotionDetailListBL.DeleteMachineAttribute(machineAttributeDTO.AttributeId, promotionDetailDTO.PromotionDetailId, sqlTransaction);
                        }
                    }
                    promotionDetailDataHandler.Delete(promotionDetailDTO);
                }
                promotionDetailDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the PromotionDetail DTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // Validation do here
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PromotionDetailDTO PromotionDetailDTO
        {
            get
            {
                return promotionDetailDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of PromotionDetail
    /// </summary>
    public class PromotionDetailListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<PromotionDetailDTO> promotionDetailDTOList = new List<PromotionDetailDTO>(); // must
        /// <summary>
        /// Parameterized constructor for PromotionDetailDTOListBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public PromotionDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for PromotionDetailDTOListBL
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="promotionDetailDTOList">PromotionDetailDTO List passed as a parameter</param>
        public PromotionDetailListBL(ExecutionContext executionContext,
                                                 List<PromotionDetailDTO> promotionDetailDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, promotionDetailDTOList);
            this.promotionDetailDTOList = promotionDetailDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PromotionDetailDTO List based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the PromotionDetailDTO List</returns>
        public List<PromotionDetailDTO> GetPromotionDetailDTOList(List<KeyValuePair<PromotionDetailDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PromotionDetailDataHandler promotionDetailDataHandler = new PromotionDetailDataHandler(sqlTransaction);
            List<PromotionDetailDTO> promotionDetailDTOList = promotionDetailDataHandler.GetPromotionDetailDTOList(searchParameters);
            log.LogMethodExit(promotionDetailDTOList);
            return promotionDetailDTOList;
        }

        /// <summary>
        /// Saves the PromotionDetailDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (promotionDetailDTOList == null ||
               promotionDetailDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < promotionDetailDTOList.Count; i++)
            {
                var promotionDetailDTO = promotionDetailDTOList[i];
                if (promotionDetailDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    PromotionDetailBL promotionDetailBL = new PromotionDetailBL(executionContext, promotionDetailDTO);
                    promotionDetailBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving PromotionDetailDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("PromotionDetailDTO", promotionDetailDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the PromotionDetailDTO List for product Id List
        /// </summary>
        /// <param name="promotionIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<PromotionDetailDTO> GetPromotionDetailDTOListForProducts(List<int> promotionIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(promotionIdList, activeRecords, sqlTransaction);
            PromotionDetailDataHandler promotionDetailDataHandler = new PromotionDetailDataHandler(sqlTransaction);
            List<PromotionDetailDTO> promotionDetailDTOList = promotionDetailDataHandler.GetPromotionDetailDTOList(promotionIdList, activeRecords);
            log.LogMethodExit(promotionDetailDTOList);
            return promotionDetailDTOList;
        }

        /// <summary>
        /// Get the Promotion Detail Game Attributes
        /// </summary>
        /// <param name="promotionDetailId">promotionDetailId</param>
        public List<MachineAttributeDTO> GetPromotionDetailGameAttributes(int promotionDetailId)
        {
            log.LogMethodEntry(promotionDetailId);
            PromotionDetailBL promotionDetailBL = new PromotionDetailBL(executionContext, promotionDetailId, false, false, null);
            PromotionDetailDTO promotionDetailDTO = new PromotionDetailDTO();
            promotionDetailDTO = promotionDetailBL.PromotionDetailDTO;
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler();
            List<MachineAttributeDTO> machineAttributeDTOList = new List<MachineAttributeDTO>();
            int gameProfileId = -1;
            if (promotionDetailDTO.GameId != -1)
            {
                GameContainerList.Rebuild(executionContext.GetSiteId());
                List<GameContainerDTO> gameContainerDTOList = GameContainerList.GetGameContainerDTOList(executionContext.GetSiteId(), executionContext);
                gameProfileId = gameContainerDTOList.Where(game => game.GameId == promotionDetailDTO.GameId).Select(x => x.GameProfileId).FirstOrDefault();
            }
            else
            {
                gameProfileId = promotionDetailDTO.GameprofileId;
            }
            machineAttributeDTOList = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL, gameProfileId, promotionDetailDTO.GameId, -1, promotionDetailDTO.PromotionId, promotionDetailDTO.PromotionDetailId, executionContext.GetSiteId());
            log.LogMethodExit(machineAttributeDTOList);
            return machineAttributeDTOList;
        }

        /// <summary>
        /// Save the Promotion Detail Game Attributes
        /// </summary>
        /// <param name="machineAttributeDTOList">machineAttributeDTOList</param>
        /// <param name="promotionDetailId">promotionDetailId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void SaveAndUpdateGameAttribute(List<MachineAttributeDTO> machineAttributeDTOList, int promotionDetailId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(machineAttributeDTOList);
            if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler();
                MachineAttributeDTO machineAttributeDTO = new MachineAttributeDTO();
                foreach (MachineAttributeDTO currAttribute in machineAttributeDTOList)
                {
                    int attributeId = machineAttributeDataHandler.GetAttributeId(currAttribute.AttributeName.ToString(), executionContext.GetSiteId());
                    List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.ATTRIBUTE_ID, Convert.ToString(attributeId)));
                    searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.PROMOTION_DETAIL_ID, Convert.ToString(promotionDetailId)));
                    attributeId = machineAttributeDataHandler.GetEntityIDs(searchByParameters);
                    machineAttributeDTO = currAttribute;
                    if (attributeId == 0 && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL && machineAttributeDTO.EnableForPromotion)
                    {
                        machineAttributeDTO = machineAttributeDataHandler.InsertMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL, promotionDetailId, executionContext.GetUserId(), executionContext.GetSiteId());
                        if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                        {
                            AuditLog auditLog = new AuditLog(executionContext);
                            auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
                        }
                        machineAttributeDTO.AcceptChanges();
                    }
                    else
                    {
                        if (machineAttributeDTO.IsChanged == true && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL && machineAttributeDTO.EnableForPromotion) //11-Jan-2016 - Checking for context
                        {
                            machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL, promotionDetailId, executionContext.GetUserId(), executionContext.GetSiteId());
                            if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                            {
                                AuditLog auditLog = new AuditLog(executionContext);
                                auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
                            }
                            machineAttributeDTO.AcceptChanges();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Promotion Detail Game Attributes
        /// </summary>
        /// <param name="attributeId">attributeId</param>
        /// <param name="promotionDetailId">promotionDetailId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void DeleteMachineAttribute(int attributeId, int promotionDetailId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(attributeId, promotionDetailId);
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            machineAttributeDataHandler.DeleteMachineAttribute(attributeId, promotionDetailId, executionContext.GetSiteId(), MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL);
            log.LogMethodExit();
        }
    }
}
