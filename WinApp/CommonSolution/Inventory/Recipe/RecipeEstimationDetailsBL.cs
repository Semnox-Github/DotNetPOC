/********************************************************************************************
 * Project Name - Inventory
 * Description  - BL object of RecipeEstimationDetails
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       21-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory.Recipe
{

    /// <summary>
    /// Recipe Estimation Details BL
    /// </summary>
    public class RecipeEstimationDetailsBL
    {
        private RecipeEstimationDetailsDTO recipeEstimationDetailsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private RecipeEstimationDetailsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RecipeEstimationDetailsBL object using the RecipeEstimationDetailsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="RecipeEstimationDetailsDTO">RecipeEstimationDetails DTO object</param>
        public RecipeEstimationDetailsBL(ExecutionContext executionContext, RecipeEstimationDetailsDTO recipeEstimationDetailsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeEstimationDetailsDTO);
            this.recipeEstimationDetailsDTO = recipeEstimationDetailsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RecipeEstimationDetails  id as the parameter
        /// Would fetch the RecipeEstimationDetails object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="recipeEstimationDetailId">id -PromotionRule </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RecipeEstimationDetailsBL(ExecutionContext executionContext, int recipeEstimationDetailId,
                                            SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeEstimationDetailId, sqlTransaction);
            RecipeEstimationDetailsDataHandler recipeEstimationDetailsDataHandler = new RecipeEstimationDetailsDataHandler(sqlTransaction);
            recipeEstimationDetailsDTO = recipeEstimationDetailsDataHandler.GetRecipeEstimationDetailsId(recipeEstimationDetailId);
            if (recipeEstimationDetailsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "RecipeEstimationDetailsDTO", recipeEstimationDetailId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RecipeEstimationDetails DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipeEstimationDetailsDTO.IsChanged == false
                && recipeEstimationDetailsDTO.RecipeEstimationDetailId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RecipeEstimationDetailsDataHandler recipeEstimationDetailsDataHandler = new RecipeEstimationDetailsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (recipeEstimationDetailsDTO.RecipeEstimationDetailId < 0)
            {
                recipeEstimationDetailsDTO = recipeEstimationDetailsDataHandler.Insert(recipeEstimationDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipeEstimationDetailsDTO.AcceptChanges();
            }
            else if (recipeEstimationDetailsDTO.IsChanged)
            {
                recipeEstimationDetailsDTO = recipeEstimationDetailsDataHandler.Update(recipeEstimationDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipeEstimationDetailsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the RecipeEstimationDetails DTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            
            RecipeEstimationDetailsDataHandler recipeEstimationDetailsDataHandler = new RecipeEstimationDetailsDataHandler(sqlTransaction);
            if (recipeEstimationDetailsDTO.RecipeEstimationDetailId > 0)
            {
                recipeEstimationDetailsDataHandler.Delete(recipeEstimationDetailsDTO);
                recipeEstimationDetailsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        
        

        /// <summary>
        /// Validates the RecipeEstimationDetailsDTO 
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
        public RecipeEstimationDetailsDTO RecipeEstimationDetailsDTO
        {
            get
            {
                return recipeEstimationDetailsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of RecipeEstimationDetails
    /// </summary>
    public class RecipeEstimationDetailsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList = new List<RecipeEstimationDetailsDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public RecipeEstimationDetailsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="RecipeEstimationDetailsDTOList">RecipeEstimationDetails DTO List as parameter </param>
        public RecipeEstimationDetailsListBL(ExecutionContext executionContext, List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeEstimationDetailsDTOList);
            this.recipeEstimationDetailsDTOList = recipeEstimationDetailsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the RecipeEstimationDetails DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of RecipeEstimationDetailsDTO </returns>
        public List<RecipeEstimationDetailsDTO> GetRecipeEstimationDetailsDTOList(List<KeyValuePair<RecipeEstimationDetailsDTO.SearchByParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RecipeEstimationDetailsDataHandler recipeEstimationDetailsDataHandler = new RecipeEstimationDetailsDataHandler(sqlTransaction);
            List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList = recipeEstimationDetailsDataHandler.GetRecipeEstimationHeaderDTOList(searchParameters);
            log.LogMethodExit(recipeEstimationDetailsDTOList);
            return recipeEstimationDetailsDTOList;
        }

        /// <summary>
        /// Gets the RecipeEstimationDetailsDTO List for recipeEstimationHeaderIdList 
        /// </summary>
        /// <param name="recipeEstimationHeaderIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of RecipeEstimationDetailsDTO</returns>
        public List<RecipeEstimationDetailsDTO> GetRecipeEstimationHeaderDTOListOfRecipe(List<int> recipeEstimationHeaderIdList,
                                                bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(recipeEstimationHeaderIdList, activeRecords, sqlTransaction);
            RecipeEstimationDetailsDataHandler recipeEstimationDetailsDataHandler = new RecipeEstimationDetailsDataHandler(sqlTransaction);
            List<RecipeEstimationDetailsDTO> recipeEstimationDetailsList = recipeEstimationDetailsDataHandler.GetRecipeEstimationHeaderDTOListOfRecipe(recipeEstimationHeaderIdList, activeRecords);
            log.LogMethodExit(recipeEstimationDetailsList);
            return recipeEstimationDetailsList;
        }

        /// <summary>
        /// Saves the  list of RecipeEstimationDetailsDTOList DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipeEstimationDetailsDTOList == null ||
                recipeEstimationDetailsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < recipeEstimationDetailsDTOList.Count; i++)
            {
                RecipeEstimationDetailsDTO recipeEstimationDetailsDTO = recipeEstimationDetailsDTOList[i];
                if (recipeEstimationDetailsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RecipeEstimationDetailsBL recipeEstimationDetailsBL = new RecipeEstimationDetailsBL(executionContext, recipeEstimationDetailsDTO);
                    recipeEstimationDetailsBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving RecipeEstimationDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RecipeEstimationDetailsDTO", recipeEstimationDetailsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the  list of RecipeEstimationDetailsDTOList DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipeEstimationDetailsDTOList == null ||
                recipeEstimationDetailsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < recipeEstimationDetailsDTOList.Count; i++)
            {
                RecipeEstimationDetailsDTO recipeEstimationDetailsDTO = recipeEstimationDetailsDTOList[i];
                try
                {
                    RecipeEstimationDetailsBL recipeEstimationDetailsBL = new RecipeEstimationDetailsBL(executionContext, recipeEstimationDetailsDTO);
                    recipeEstimationDetailsBL.Delete(sqlTransaction);
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
                    log.Error("Error occurred while saving RecipeEstimationDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RecipeEstimationDetailsDTO", recipeEstimationDetailsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to Purge Forecasted Data
        /// </summary>
        /// <param name="purgeDataInDays"></param>
        /// <param name="sqlTransaction"></param>
        public void PurgeOldData(int purgeDataInDays, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            RecipeEstimationDetailsDataHandler recipeEstimationDetailsDataHandler = new RecipeEstimationDetailsDataHandler(sqlTransaction);
            recipeEstimationDetailsDataHandler.PurgeOldData(purgeDataInDays);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to fetch the future events(Reservations) to consider the event in the forecast
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<RecipeEstimationDetailsDTO> GetEventProducts(DateTime fromDate, DateTime toDate, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(fromDate, toDate);
            RecipeEstimationDetailsDataHandler recipeEstimationDetailsDataHandler = new RecipeEstimationDetailsDataHandler(sqlTransaction);
            List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList = recipeEstimationDetailsDataHandler.GetEventDetails(fromDate, toDate);
            log.LogMethodExit(recipeEstimationDetailsDTOList);
            return recipeEstimationDetailsDTOList;
        }
    }
}
