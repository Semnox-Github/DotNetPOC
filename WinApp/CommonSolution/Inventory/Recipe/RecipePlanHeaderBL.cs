/********************************************************************************************
 * Project Name - Inventory
 * Description  - BL Logic for ReciepPlanHeader
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0        22-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using System.Globalization;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Plan Header BL
    /// </summary>
    public class RecipePlanHeaderBL
    {
        private RecipePlanHeaderDTO recipePlanHeaderDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private RecipePlanHeaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RecipePlanHeaderBL object using the recipePlanHeaderDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="recipePlanHeaderDTO">recipePlanHeaderDTO DTO object</param>
        public RecipePlanHeaderBL(ExecutionContext executionContext, RecipePlanHeaderDTO recipePlanHeaderDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipePlanHeaderDTO);
            this.recipePlanHeaderDTO = recipePlanHeaderDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RecipePlanHeader  id as the parameter
        /// Would fetch the RecipePlanHeader object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -RecipePlanHeader </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RecipePlanHeaderBL(ExecutionContext executionContext, int recipePlanHeaderId,
                                         bool loadChildRecords = false, bool activeChildRecords = false,
                                         SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipePlanHeaderId, loadChildRecords, activeChildRecords, sqlTransaction);
            RecipePlanHeaderDataHandler recipePlanHeaderDataHandler = new RecipePlanHeaderDataHandler(sqlTransaction);
            this.recipePlanHeaderDTO = recipePlanHeaderDataHandler.GetRecipePlanHeaderId(recipePlanHeaderId);
            if (loadChildRecords == false ||
                recipePlanHeaderDTO == null)
            {
                log.LogMethodExit();
                return;
            }
            RecipePlanDetailsListBL recipePlanDetailsListBL = new RecipePlanDetailsListBL(executionContext);
            recipePlanHeaderDTO.RecipePlanDetailsDTOList = recipePlanDetailsListBL.GetRecipePlanHeaderDTOListOfRecipe(new List<int> { recipePlanHeaderId }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RecipePlanHeader DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipePlanHeaderDTO.IsChanged == false && recipePlanHeaderDTO.IsChangedRecursive == false
                && recipePlanHeaderDTO.RecipePlanHeaderId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RecipePlanHeaderDataHandler recipePlanHeaderDataHandler = new RecipePlanHeaderDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (recipePlanHeaderDTO.RecipePlanHeaderId < 0)
            {
                recipePlanHeaderDTO = recipePlanHeaderDataHandler.Insert(recipePlanHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipePlanHeaderDTO.AcceptChanges();
            }
            else if (recipePlanHeaderDTO.IsChanged)
            {
                recipePlanHeaderDTO = recipePlanHeaderDataHandler.Update(recipePlanHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipePlanHeaderDTO.AcceptChanges();
            }
            if (recipePlanHeaderDTO.RecipePlanDetailsDTOList != null &&
                 recipePlanHeaderDTO.RecipePlanDetailsDTOList.Any())
            {
                foreach (RecipePlanDetailsDTO recipePlanDetailsDTO in recipePlanHeaderDTO.RecipePlanDetailsDTOList)
                {
                    if (recipePlanDetailsDTO.IsChanged)
                    {
                        recipePlanDetailsDTO.RecipePlanHeaderId = recipePlanHeaderDTO.RecipePlanHeaderId;
                        RecipePlanDetailsBL recipePlanDetailsBL = new RecipePlanDetailsBL(executionContext, recipePlanDetailsDTO);
                        recipePlanDetailsBL.Save(sqlTransaction);

                    }
                }
                recipePlanHeaderDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the RecipePlanHeaderDTO 
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
        /// Gets the RecipePlanHeaderDTO
        /// </summary>
        public RecipePlanHeaderDTO RecipePlanHeaderDTO
        {
            get
            {
                return recipePlanHeaderDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of RecipePlanHeader
    /// </summary>
    public class RecipePlanHeaderListBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = new List<RecipePlanHeaderDTO>();

        /// <summary>
        /// Parameterized constructor of RecipePlanHeaderListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public RecipePlanHeaderListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="recipePlanHeaderDTOList">RecipePlanHeader DTO List as parameter </param>
        public RecipePlanHeaderListBL(ExecutionContext executionContext,
                                               List<RecipePlanHeaderDTO> recipePlanHeaderDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipePlanHeaderDTOList);
            this.recipePlanHeaderDTOList = recipePlanHeaderDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the recipePlanHeaderDTO list
        /// </summary>
        public List<RecipePlanHeaderDTO> GetAllRecipePlanHeaderDTOList(List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            RecipePlanHeaderDataHandler recipePlanHeaderDataHandler = new RecipePlanHeaderDataHandler(sqlTransaction);
            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderDataHandler.GetRecipePlanHeaderDTOList(searchParameters);
            if (loadChildRecords == false ||
                recipePlanHeaderDTOList == null ||
                recipePlanHeaderDTOList.Any() == false)
            {
                log.LogMethodExit(recipePlanHeaderDTOList, "Child records are not loaded.");
                return recipePlanHeaderDTOList;
            }
            BuildRecipePlanHeaderDTOList(recipePlanHeaderDTOList, activeChildRecords, sqlTransaction);
            if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Any())
            {
                foreach (RecipePlanHeaderDTO recipePlanHeaderDTO in recipePlanHeaderDTOList)
                {
                    recipePlanHeaderDTO.RecipeCount = recipePlanHeaderDTO.RecipePlanDetailsDTOList.Count + " Recipe";
                }
            }
            log.LogMethodExit(recipePlanHeaderDTOList);
            return recipePlanHeaderDTOList;
        }

        private void BuildRecipePlanHeaderDTOList(List<RecipePlanHeaderDTO> recipePlanHeaderDTOList, bool activeChildRecords,
                                                    SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(recipePlanHeaderDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, RecipePlanHeaderDTO> recipePlanHeaderDTOIdMap = new Dictionary<int, RecipePlanHeaderDTO>();
            List<int> recipePlanHeaderIdList = new List<int>();
            for (int i = 0; i < recipePlanHeaderDTOList.Count; i++)
            {
                if (recipePlanHeaderDTOIdMap.ContainsKey(recipePlanHeaderDTOList[i].RecipePlanHeaderId))
                {
                    continue;
                }
                recipePlanHeaderDTOIdMap.Add(recipePlanHeaderDTOList[i].RecipePlanHeaderId, recipePlanHeaderDTOList[i]);
                recipePlanHeaderIdList.Add(recipePlanHeaderDTOList[i].RecipePlanHeaderId);
            }
            RecipePlanDetailsListBL recipePlanDetailsListBL = new RecipePlanDetailsListBL(executionContext);
            List<RecipePlanDetailsDTO> recipePlanDetailsDTOList = recipePlanDetailsListBL.GetRecipePlanHeaderDTOListOfRecipe(recipePlanHeaderIdList, activeChildRecords, sqlTransaction);
            if (recipePlanDetailsDTOList != null && recipePlanDetailsDTOList.Any())
            {
                for (int i = 0; i < recipePlanDetailsDTOList.Count; i++)
                {
                    if (recipePlanHeaderDTOIdMap.ContainsKey(recipePlanDetailsDTOList[i].RecipePlanHeaderId) == false)
                    {
                        continue;
                    }
                    RecipePlanHeaderDTO recipePlanHeaderDTO = recipePlanHeaderDTOIdMap[recipePlanDetailsDTOList[i].RecipePlanHeaderId];
                    if (recipePlanHeaderDTO.RecipePlanDetailsDTOList == null)
                    {
                        recipePlanHeaderDTO.RecipePlanDetailsDTOList = new List<RecipePlanDetailsDTO>();
                    }
                    recipePlanHeaderDTO.RecipePlanDetailsDTOList.Add(recipePlanDetailsDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the RecipePlanHeader DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of RecipePlanHeaderDTO </returns>
        public List<RecipePlanHeaderDTO> GetRecipePlanHeaderDTOList(List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RecipePlanHeaderDataHandler recipePlanHeaderDataHandler = new RecipePlanHeaderDataHandler(sqlTransaction);
            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderDataHandler.GetRecipePlanHeaderDTOList(searchParameters);
            log.LogMethodExit(recipePlanHeaderDTOList);
            return recipePlanHeaderDTOList;
        }

        /// <summary>
        /// Saves the  list of RecipePlanHeader DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipePlanHeaderDTOList == null ||
                recipePlanHeaderDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < recipePlanHeaderDTOList.Count; i++)
            {
                RecipePlanHeaderDTO recipePlanHeaderDTO = recipePlanHeaderDTOList[i];
                if (recipePlanHeaderDTO.IsChanged == false && recipePlanHeaderDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    RecipePlanHeaderBL recipePlanHeaderBL = new RecipePlanHeaderBL(executionContext, recipePlanHeaderDTO);
                    recipePlanHeaderBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving RecipePlanHeaderDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RecipePlanHeaderDTO", recipePlanHeaderDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        public void CopyRecipe(DateTime fromdate, DateTime todate, List<DateTime> sourceDateList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(fromdate, todate);
            List<RecipePlanHeaderDTO> recipePlanHeaderDTOCopyList = new List<RecipePlanHeaderDTO>();
            int loopCount = 0;
            double diff = (todate - fromdate).TotalDays;
            loopCount = Convert.ToInt32(Math.Round(diff, 2));
            loopCount++;
            for (int i = 0; i < loopCount; i++)
            {
                DateTime srcFromDate;
                if (sourceDateList.Count > 1)
                {
                    srcFromDate = sourceDateList[i];
                }
                else
                {
                    srcFromDate = sourceDateList[0];
                }
                RecipePlanHeaderDTO srcRecipePlanHeaderDTO = null;
                if (!recipePlanHeaderDTOList.Exists(x => x.PlanDateTime == srcFromDate))
                {
                    fromdate = fromdate.AddDays(1);
                    continue;
                }
                else
                {
                    srcRecipePlanHeaderDTO = recipePlanHeaderDTOList.Find(x => x.PlanDateTime.Date == srcFromDate.Date);
                    if(srcRecipePlanHeaderDTO.RecipePlanDetailsDTOList.Count <= 0)
                    {
                        fromdate = fromdate.AddDays(1);
                        continue;
                    }
                }
                RecipePlanHeaderDTO copyRecipePlanHeaderDTO = null;
                List<RecipePlanDetailsDTO> recipePlanDetailsDTOList = null;
                copyRecipePlanHeaderDTO = new RecipePlanHeaderDTO(-1, srcRecipePlanHeaderDTO.PlanDateTime, srcRecipePlanHeaderDTO.RecurFlag, srcRecipePlanHeaderDTO.RecurEndDate,
                                                                   srcRecipePlanHeaderDTO.RecurFrequency, srcRecipePlanHeaderDTO.RecurType, -1, srcRecipePlanHeaderDTO.Sunday,
                                                                   srcRecipePlanHeaderDTO.Monday, srcRecipePlanHeaderDTO.Tuesday, srcRecipePlanHeaderDTO.Wednesday, srcRecipePlanHeaderDTO.Thursday,
                                                                   srcRecipePlanHeaderDTO.Friday, srcRecipePlanHeaderDTO.Saturday, srcRecipePlanHeaderDTO.IsActive);

                recipePlanDetailsDTOList = srcRecipePlanHeaderDTO.RecipePlanDetailsDTOList;

                RecipePlanHeaderListBL recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext);
                List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchPlanParams = new List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>>();
                searchPlanParams.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.FROM_DATE, fromdate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchPlanParams.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.TO_DATE, fromdate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                List<RecipePlanHeaderDTO> recipePlanHeaderList = recipePlanHeaderListBL.GetAllRecipePlanHeaderDTOList(searchPlanParams, true);
                if (recipePlanHeaderList != null && recipePlanHeaderList.Any())
                {
                    copyRecipePlanHeaderDTO = recipePlanHeaderList[0];
                }
                else
                {
                    copyRecipePlanHeaderDTO.PlanDateTime = fromdate.Date;
                }
                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                DateTime currentDatetime = serverTimeObject.GetServerDateTime();
                foreach (RecipePlanDetailsDTO dto in recipePlanDetailsDTOList)
                {
                    if (copyRecipePlanHeaderDTO.RecipePlanDetailsDTOList.Exists(x => x.ProductId == dto.ProductId))
                    {
                        copyRecipePlanHeaderDTO.RecipePlanDetailsDTOList.Find(x => x.ProductId == dto.ProductId).FinalQty = dto.FinalQty;
                        copyRecipePlanHeaderDTO.RecipePlanDetailsDTOList.Find(x => x.ProductId == dto.ProductId).IncrementalQty = dto.FinalQty - dto.PlannedQty;
                        copyRecipePlanHeaderDTO.RecipePlanDetailsDTOList.Find(x => x.ProductId == dto.ProductId).QtyModifiedDate = currentDatetime;
                        continue;
                    }
                    copyRecipePlanHeaderDTO.RecipePlanDetailsDTOList.Add(new RecipePlanDetailsDTO(-1, -1, dto.ProductId, dto.PlannedQty, dto.IncrementalQty, dto.FinalQty, dto.UOMId, dto.RecipeEstimationDetailId,
                                                                                               dto.QtyModifiedDate, dto.IsActive));
                }
                fromdate = fromdate.AddDays(1);
                recipePlanHeaderDTOCopyList.Add(copyRecipePlanHeaderDTO);
            }
            recipePlanHeaderDTOList = new List<RecipePlanHeaderDTO>();
            recipePlanHeaderDTOList = recipePlanHeaderDTOCopyList;
            Save(sqlTransaction);
            log.LogMethodExit();
        }

        public List<RecipePlanHeaderDTO> GetRecipePlanHeaderDTOList(DateTime fromDate, DateTime toDate, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(fromDate, toDate, siteId);
            RecipePlanHeaderDataHandler recipePlanHeaderDataHandler = new RecipePlanHeaderDataHandler(sqlTransaction);
            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderDataHandler.GetRecipePlanHedeaderDTO(fromDate, toDate, siteId);
            if (recipePlanHeaderDTOList == null ||
                recipePlanHeaderDTOList.Any() == false)
            {
                log.LogMethodExit(recipePlanHeaderDTOList, "Child records are not loaded.");
                return recipePlanHeaderDTOList;
            }
            BuildRecipePlanHeaderDTOList(recipePlanHeaderDTOList, true, sqlTransaction);
            log.LogMethodExit(recipePlanHeaderDTOList);
            return recipePlanHeaderDTOList;
        }
    }
}
