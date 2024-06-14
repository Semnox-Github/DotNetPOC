/********************************************************************************************
 * Project Name - Inventory
 * Description  - BL Logic for ReciepPlanHeader
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       23-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *2.110.0       20-Feb-2021   Dakshakh Raj        Modified: Get Sequence method changes
  *********************************************************************************************/
using System;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Manufacturing Header BL
    /// </summary>
    public class RecipeManufacturingHeaderBL
    {
        private RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private RecipeManufacturingHeaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates RecipeManufacturingHeaderBL object using the RecipeManufacturingHeaderDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="RecipeManufacturingHeaderDTO">RecipeManufacturingHeaderDTO DTO object</param>
        public RecipeManufacturingHeaderBL(ExecutionContext executionContext,
                                         RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeManufacturingHeaderDTO);
            this.recipeManufacturingHeaderDTO = recipeManufacturingHeaderDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RecipeManufacturingHeader  id as the parameter
        /// Would fetch the RecipeManufacturingHeader object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -RecipeManufacturingHeader </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RecipeManufacturingHeaderBL(ExecutionContext executionContext, int recipeManufacturingHeaderId,
                                         bool loadChildRecords = false, bool activeChildRecords = false,
                                         SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeManufacturingHeaderId, loadChildRecords, activeChildRecords, sqlTransaction);
            RecipeManufacturingHeaderDataHandler recipeManufacturingHeaderDataHandler = new RecipeManufacturingHeaderDataHandler(sqlTransaction);
            this.recipeManufacturingHeaderDTO = recipeManufacturingHeaderDataHandler.GetRecipeManufacturingHeaderId(recipeManufacturingHeaderId);
            if (loadChildRecords == false ||
                recipeManufacturingHeaderDTO == null)
            {
                log.LogMethodExit();
                return;
            }
            RecipeManufacturingDetailsListBL recipeManufacturingDetailsListBL = new RecipeManufacturingDetailsListBL(executionContext);
            recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList = recipeManufacturingDetailsListBL.GetRecipeManufacturingHeaderDTOListOfRecipe(new List<int> { recipeManufacturingHeaderId }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RecipeManufacturingHeader DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipeManufacturingHeaderDTO.IsChanged == false
                && recipeManufacturingHeaderDTO.RecipeManufacturingHeaderId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RecipeManufacturingHeaderDataHandler recipeManufacturingHeaderDataHandler = new RecipeManufacturingHeaderDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (recipeManufacturingHeaderDTO.RecipeManufacturingHeaderId < 0)
            {
                GetSequenceNumber(sqlTransaction, executionContext);
                recipeManufacturingHeaderDTO = recipeManufacturingHeaderDataHandler.Insert(recipeManufacturingHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipeManufacturingHeaderDTO.AcceptChanges();
            }
            else if (recipeManufacturingHeaderDTO.IsChanged)
            {
                log.LogVariableState("RecipeManufacturingHeaderDTO", recipeManufacturingHeaderDTO);
                recipeManufacturingHeaderDTO = recipeManufacturingHeaderDataHandler.Update(recipeManufacturingHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipeManufacturingHeaderDTO.AcceptChanges();
            }
            if (recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList != null &&
                  recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Count != 0)
            {
                foreach (RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO in recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList)
                {
                    if (recipeManufacturingDetailsDTO.IsChanged)
                    {
                        recipeManufacturingDetailsDTO.RecipeManufacturingHeaderId = recipeManufacturingHeaderDTO.RecipeManufacturingHeaderId;
                    }
                }
                RecipeManufacturingDetailsListBL recipeManufacturingDetailsListBL = new RecipeManufacturingDetailsListBL(executionContext, recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList);
                recipeManufacturingDetailsListBL.Save(sqlTransaction);
            }
            if (recipeManufacturingHeaderDTO.IsComplete)
            {
                RecipeManufacturingDetailsListBL recipeManufacturingDetailsListBL = new RecipeManufacturingDetailsListBL(executionContext, recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList);
                recipeManufacturingDetailsListBL.UpdateInventoryStock(recipeManufacturingHeaderDTO.MFGDateTime , sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Sequence Number
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="executionContext"></param>
        private void GetSequenceNumber(SqlTransaction sqlTransaction, ExecutionContext executionContext)
        {
            log.LogMethodEntry(sqlTransaction, executionContext);
            SequencesListBL sequencesListBL = new SequencesListBL(executionContext);
            SequencesDTO sequencesDTO = null;
            List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchBySeqParameters = new List<KeyValuePair<SequencesDTO.SearchByParameters, string>>();
            searchBySeqParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SEQUENCE_NAME, "RecipeManufacturingHeader"));
            List<SequencesDTO> sequencesDTOList = sequencesListBL.GetAllSequencesList(searchBySeqParameters);
            if (sequencesDTOList != null && sequencesDTOList.Any())
            {
                if (sequencesDTOList.Count == 1)
                {
                    sequencesDTO = sequencesDTOList[0];
                }
                else
                {
                    sequencesDTO = sequencesDTOList.FirstOrDefault(seq => seq.POSMachineId == executionContext.GetMachineId());
                    if (sequencesDTO == null)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2956, executionContext.GetMachineId()));
                    }
                }
                SequencesBL sequenceBL = new SequencesBL(executionContext, sequencesDTO);
                recipeManufacturingHeaderDTO.RecipeMFGNumber = sequenceBL.GetNextSequenceNo(sqlTransaction);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Validates the RecipeManufacturingHeaderDTO 
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
        /// Gets the RecipeManufacturingHeaderDTO
        /// </summary>
        public RecipeManufacturingHeaderDTO RecipeManufacturingHeaderDTO
        {
            get
            {
                return recipeManufacturingHeaderDTO;
            }
        }
    }

    public class RecipeManufacturingHeaderListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = new List<RecipeManufacturingHeaderDTO>();

        /// <summary>
        /// Parameterized constructor of RecipeManufacturingHeaderListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public RecipeManufacturingHeaderListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="RecipeManufacturingHeaderDTOList">RecipeManufacturingHeader DTO List as parameter </param>
        public RecipeManufacturingHeaderListBL(ExecutionContext executionContext,
                                               List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeManufacturingHeaderDTOList);
            this.recipeManufacturingHeaderDTOList = recipeManufacturingHeaderDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the RecipeManufacturingHeaderDTO list
        /// </summary>
        public List<RecipeManufacturingHeaderDTO> GetAllRecipeManufacturingHeaderDTOList(List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            RecipeManufacturingHeaderDataHandler recipeManufacturingHeaderDataHandler = new RecipeManufacturingHeaderDataHandler(sqlTransaction);
            List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = recipeManufacturingHeaderDataHandler.GetRecipeManufacturingHeaderDTOList(searchParameters);
            if (loadChildRecords == false ||
                recipeManufacturingHeaderDTOList == null ||
                recipeManufacturingHeaderDTOList.Any() == false)
            {
                log.LogMethodExit(recipeManufacturingHeaderDTOList, "Child records are not loaded.");
                return recipeManufacturingHeaderDTOList;
            }
            BuildRecipeManufacturingHeaderDTOList(recipeManufacturingHeaderDTOList, activeChildRecords, sqlTransaction);
            log.LogMethodExit(recipeManufacturingHeaderDTOList);
            return recipeManufacturingHeaderDTOList;
        }

        private void BuildRecipeManufacturingHeaderDTOList(List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList, bool activeChildRecords,
                                                    SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(recipeManufacturingHeaderDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOIdMap = new Dictionary<int, RecipeManufacturingHeaderDTO>();
            List<int> recipeManufacturingHeaderIdList = new List<int>();
            for (int i = 0; i < recipeManufacturingHeaderDTOList.Count; i++)
            {
                if (recipeManufacturingHeaderDTOIdMap.ContainsKey(recipeManufacturingHeaderDTOList[i].RecipeManufacturingHeaderId))
                {
                    continue;
                }
                recipeManufacturingHeaderDTOIdMap.Add(recipeManufacturingHeaderDTOList[i].RecipeManufacturingHeaderId, recipeManufacturingHeaderDTOList[i]);
                recipeManufacturingHeaderIdList.Add(recipeManufacturingHeaderDTOList[i].RecipeManufacturingHeaderId);
            }
            RecipeManufacturingDetailsListBL recipeManufacturingDetailsListBL = new RecipeManufacturingDetailsListBL(executionContext);
            List<RecipeManufacturingDetailsDTO> recipeManufacturingDetailsDTOList = recipeManufacturingDetailsListBL.GetRecipeManufacturingHeaderDTOListOfRecipe(recipeManufacturingHeaderIdList, activeChildRecords, sqlTransaction);
            if (recipeManufacturingDetailsDTOList != null && recipeManufacturingDetailsDTOList.Any())
            {
                for (int i = 0; i < recipeManufacturingDetailsDTOList.Count; i++)
                {
                    if (recipeManufacturingHeaderDTOIdMap.ContainsKey(recipeManufacturingDetailsDTOList[i].RecipeManufacturingHeaderId) == false)
                    {
                        continue;
                    }
                    RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO = recipeManufacturingHeaderDTOIdMap[recipeManufacturingDetailsDTOList[i].RecipeManufacturingHeaderId];
                    if (recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList == null)
                    {
                        recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList = new List<RecipeManufacturingDetailsDTO>();
                    }
                    recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Add(recipeManufacturingDetailsDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the RecipeManufacturingHeader DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of RecipeManufacturingHeaderDTO </returns>
        public List<RecipeManufacturingHeaderDTO> GetRecipeManufacturingHeaderDTOList(List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> searchParameters,
                                                                                        SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RecipeManufacturingHeaderDataHandler recipeManufacturingHeaderDataHandler = new RecipeManufacturingHeaderDataHandler(sqlTransaction);
            List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = recipeManufacturingHeaderDataHandler.GetRecipeManufacturingHeaderDTOList(searchParameters);
            log.LogMethodExit(recipeManufacturingHeaderDTOList);
            return recipeManufacturingHeaderDTOList;
        }
        /// <summary>
        /// Saves the  list of RecipeManufacturingHeader DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipeManufacturingHeaderDTOList == null ||
                recipeManufacturingHeaderDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < recipeManufacturingHeaderDTOList.Count; i++)
            {
                RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO = recipeManufacturingHeaderDTOList[i];
                if (recipeManufacturingHeaderDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RecipeManufacturingHeaderBL recipeManufacturingHeaderBL = new RecipeManufacturingHeaderBL(executionContext, recipeManufacturingHeaderDTO);
                    recipeManufacturingHeaderBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving RecipeManufacturingHeaderDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RecipeManufacturingHeaderDTO", recipeManufacturingHeaderDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}

