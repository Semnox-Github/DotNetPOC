/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  PromotionExclusionDate
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      24-Jun-2019     Girish Kundar           Created 
 *2.80      08-Apr-2020     Mushahid Faizan         Modified : 3 tier changes for Rest API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Business logic for PromotionExclusionDate class.
    /// </summary>
    public class PromotionExclusionDateBL
    {
        private PromotionExclusionDateDTO promotionExclusionDateDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PromotionExclusionDateBL class
        /// </summary>
        private PromotionExclusionDateBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates PromotionExclusionDateBL object using the PromotionExclusionDateDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="promotionExclusionDateDTO">PromotionExclusionDate DTO object</param>
        public PromotionExclusionDateBL(ExecutionContext executionContext, PromotionExclusionDateDTO promotionExclusionDateDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, promotionExclusionDateDTO);
            this.promotionExclusionDateDTO = promotionExclusionDateDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the PromotionExclusionDate  id as the parameter
        /// Would fetch the PromotionExclusionDate object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -PromotionExclusionDate </param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PromotionExclusionDateBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            PromotionExclusionDateDataHandler promotionExclusionDateDataHandler = new PromotionExclusionDateDataHandler(sqlTransaction);
            promotionExclusionDateDTO = promotionExclusionDateDataHandler.GetPromotionExclusionDateDTO(id);
            if (promotionExclusionDateDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PromotionExclusionDateDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PromotionExclusionDate DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (promotionExclusionDateDTO.IsChanged == false
                && promotionExclusionDateDTO.PromotionExclusionId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            PromotionExclusionDateDataHandler promotionExclusionDateDataHandler = new PromotionExclusionDateDataHandler(sqlTransaction);
            if (promotionExclusionDateDTO.IsActive == true)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (promotionExclusionDateDTO.PromotionExclusionId < 0)
                {
                    log.LogVariableState("PromotionExclusionDateDTO", promotionExclusionDateDTO);
                    promotionExclusionDateDTO = promotionExclusionDateDataHandler.Insert(promotionExclusionDateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    promotionExclusionDateDTO.AcceptChanges();
                }
                else if (promotionExclusionDateDTO.IsChanged)
                {
                    log.LogVariableState("PromotionExclusionDateDTO", promotionExclusionDateDTO);
                    promotionExclusionDateDTO = promotionExclusionDateDataHandler.Update(promotionExclusionDateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    promotionExclusionDateDTO.AcceptChanges();
                }

            }
            else  
            {
                if (promotionExclusionDateDTO.PromotionExclusionId >= 0)
                {
                    promotionExclusionDateDataHandler.Delete(promotionExclusionDateDTO);
                }
                promotionExclusionDateDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the PromotionExclusionDate DTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PromotionExclusionDateDTO PromotionExclusionDateDTO
        {
            get
            {
                return promotionExclusionDateDTO;
            }
        }
    }

    // <summary>
    /// Manages the list of PromotionExclusionDate
    /// </summary>
    public class PromotionExclusionDateListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PromotionExclusionDateDTO> promotionExclusionDateDTOList = new List<PromotionExclusionDateDTO>(); // To be initialized
        /// <summary>
        /// Parameterized constructor of PromotionExclusionDateListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public PromotionExclusionDateListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="promotionExclusionDateDTOList">PromotionExclusionDate DTO List as parameter </param>
        public PromotionExclusionDateListBL(ExecutionContext executionContext,
                                               List<PromotionExclusionDateDTO> promotionExclusionDateDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, promotionExclusionDateDTOList);
            this.promotionExclusionDateDTOList = promotionExclusionDateDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the PromotionExclusionDate DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of PromotionExclusionDateDTO </returns>
        public List<PromotionExclusionDateDTO> GetPromotionExclusionDateDTOList(List<KeyValuePair<PromotionExclusionDateDTO.SearchByParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PromotionExclusionDateDataHandler promotionExclusionDateDataHandler = new PromotionExclusionDateDataHandler(sqlTransaction);
            List<PromotionExclusionDateDTO> promotionExclusionDateDTOList = promotionExclusionDateDataHandler.GetPromotionExclusionDateDTOList(searchParameters);
            log.LogMethodExit(promotionExclusionDateDTOList);
            return promotionExclusionDateDTOList;
        }

        /// <summary>
        /// Saves the  list of PromotionExclusionDate DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (promotionExclusionDateDTOList == null ||
                promotionExclusionDateDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < promotionExclusionDateDTOList.Count; i++)
            {
                var promotionExclusionDateDTO = promotionExclusionDateDTOList[i];
                if (promotionExclusionDateDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    PromotionExclusionDateBL promotionExclusionDateBL = new PromotionExclusionDateBL(executionContext, promotionExclusionDateDTO);
                    promotionExclusionDateBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving PromotionExclusionDateDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("PromotionExclusionDateDTO", promotionExclusionDateDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PromotionExclusionDateDTO List for product Id List
        /// </summary>
        /// <param name="promotionIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<PromotionExclusionDateDTO> GetPromotionExclusionDateDTOListForProducts(List<int> promotionIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(promotionIdList, activeRecords, sqlTransaction);
            PromotionExclusionDateDataHandler promotionExclusionDateDataHandler = new PromotionExclusionDateDataHandler(sqlTransaction);
            List<PromotionExclusionDateDTO> promotionExclusionDateDTOList = promotionExclusionDateDataHandler.GetPromotionExclusionDateDTOList(promotionIdList, activeRecords);
            log.LogMethodExit(promotionExclusionDateDTOList);
            return promotionExclusionDateDTOList;
        }
    }
}
