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
 *2.130.0     13-Jun-2021         Mushahid Faizan       Modified : Web Inventory UI Changes.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities.Excel;
using System.Globalization;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Plan Details BL
    /// </summary>
    public class RecipePlanDetailsBL
    {
        private RecipePlanDetailsDTO recipePlanDetailsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private RecipePlanDetailsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RecipePlanDetailsBL object using the recipePlanDetailsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="recipePlanDetailsDTO">recipePlanDetailsDTO DTO object</param>
        public RecipePlanDetailsBL(ExecutionContext executionContext, RecipePlanDetailsDTO recipePlanDetailsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipePlanDetailsDTO);
            this.recipePlanDetailsDTO = recipePlanDetailsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RecipePlanHeader  id as the parameter
        /// Would fetch the RecipePlanHeader object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -RecipePlanHeader </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RecipePlanDetailsBL(ExecutionContext executionContext, int recipePlanDetailsId, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipePlanDetailsId, sqlTransaction);
            RecipePlanDetailsDataHandler recipePlanDetailsDataHandler = new RecipePlanDetailsDataHandler(sqlTransaction);
            recipePlanDetailsDTO = recipePlanDetailsDataHandler.GetRecipePlanDetailsDTO(recipePlanDetailsId);
            if (recipePlanDetailsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "recipePlanDetailsDTO", recipePlanDetailsId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RecipePlanDetails DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipePlanDetailsDTO.IsChanged == false
                && recipePlanDetailsDTO.RecipePlanDetailId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RecipePlanDetailsDataHandler recipePlanDetailsDataHandler = new RecipePlanDetailsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (recipePlanDetailsDTO.RecipePlanDetailId < 0)
            {
                recipePlanDetailsDTO = recipePlanDetailsDataHandler.Insert(recipePlanDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipePlanDetailsDTO.AcceptChanges();
            }
            else if (recipePlanDetailsDTO.IsChanged)
            {
                recipePlanDetailsDTO = recipePlanDetailsDataHandler.Update(recipePlanDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipePlanDetailsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the RecipePlanDetailsDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (recipePlanDetailsDTO == null)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2248, MessageContainerList.GetMessage(executionContext, "recipePlanDetailsDTO"));//"Recipe Manufacturing Details are missing"
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Recipe Manufacturing Details"), MessageContainerList.GetMessage(executionContext, "DTO"), errorMessage));
            }
            if (recipePlanDetailsDTO.RecipePlanDetailId < 0 || recipePlanDetailsDTO.IsChanged)
            {
                if (recipePlanDetailsDTO.FinalQty <= 0)
                {
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2760, recipePlanDetailsDTO.RecipeName);//"Please Enter valid Actual Quantity for the Recipe"
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Recipe Plan Details"), MessageContainerList.GetMessage(executionContext, "Actual Quantity"), errorMessage));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the RecipePlanHeaderDTO
        /// </summary>
        public RecipePlanDetailsDTO RecipePlanDetailsDTO
        {
            get
            {
                return recipePlanDetailsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of RecipePlanDetails
    /// </summary>
    public class RecipePlanDetailsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RecipePlanDetailsDTO> recipePlanDetailsDTOList = new List<RecipePlanDetailsDTO>();
        private List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = new List<RecipePlanHeaderDTO>();

        /// <summary>
        /// Parameterized constructor of RecipePlanDetailsBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public RecipePlanDetailsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="promotionRuleDTOList">RecipePlanDetails DTO List as parameter </param>
        public RecipePlanDetailsListBL(ExecutionContext executionContext, List<RecipePlanDetailsDTO> recipePlanDetailsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipePlanDetailsDTOList);
            this.recipePlanDetailsDTOList = recipePlanDetailsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the RecipePlanDetails DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of PromotionRuleDTO </returns>
        public List<RecipePlanDetailsDTO> GetRecipePlanDetailsDTOList(List<KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RecipePlanDetailsDataHandler recipePlanDetailsDataHandler = new RecipePlanDetailsDataHandler(sqlTransaction);
            List<RecipePlanDetailsDTO> recipePlanDetailsDTOList = recipePlanDetailsDataHandler.GetRecipePlanDetailsDTOList(searchParameters);
            log.LogMethodExit(recipePlanDetailsDTOList);
            return recipePlanDetailsDTOList;
        }


        /// <summary>
        /// Gets the RecipePlanDetailsDTO List for recipePlanHeaderHeaderIdList
        /// </summary>
        /// <param name="recipePlanHeaderHeaderIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of RecipePlanDetailsDTO</returns>
        public List<RecipePlanDetailsDTO> GetRecipePlanHeaderDTOListOfRecipe(List<int> recipePlanHeaderHeaderIdList,
                                                                bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(recipePlanHeaderHeaderIdList, activeRecords, sqlTransaction);
            RecipePlanDetailsDataHandler recipePlanDetailsDataHandler = new RecipePlanDetailsDataHandler(sqlTransaction);
            List<RecipePlanDetailsDTO> recipePlanDetailsDTOlist = recipePlanDetailsDataHandler.GetRecipePlanDetailsDTOListOfRecipe(recipePlanHeaderHeaderIdList, activeRecords);
            log.LogMethodExit(recipePlanDetailsDTOlist);
            return recipePlanDetailsDTOlist;
        }

        /// <summary>
        /// Saves the  list of RecipePlanDetailsDTOList DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipePlanDetailsDTOList == null ||
                recipePlanDetailsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < recipePlanDetailsDTOList.Count; i++)
            {
                RecipePlanDetailsDTO recipePlanDetailsDTO = recipePlanDetailsDTOList[i];
                if (recipePlanDetailsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RecipePlanDetailsBL RecipePlanDetailsBL = new RecipePlanDetailsBL(executionContext, recipePlanDetailsDTO);
                    RecipePlanDetailsBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving RecipePlanDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RecipePlanDetailsDTO", recipePlanDetailsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is will return Sheet object for RecipePlanDetailsDTO.
        /// <returns></returns>
        public Sheet BuildTemplate(DateTime? fromDate = null, DateTime? toDate = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<RecipePlanDetailsExcel> recipePlanDetailsDTOList = new List<RecipePlanDetailsExcel>();
            ProductContainer productContainer = new ProductContainer(executionContext);
            UOMContainer uOMContainer = new UOMContainer(executionContext);
            if (ProductContainer.productDTOList == null || ProductContainer.productDTOList.Count <= 0 ||
                UOMContainer.uomDTOList == null || UOMContainer.uomDTOList.Count <= 0)
            {
                log.Debug("Container is empty");
                return null;
            }

            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            RecipePlanDetailsExcelDTODefination recipePlanDetailsExcelDTODefinition = new RecipePlanDetailsExcelDTODefination(executionContext, "");
            ///Building headers from recipePlanDetailsExcelDTODefinition
            recipePlanDetailsExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);


            RecipePlanHeaderListBL recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext);
            List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (fromDate != null && toDate != null)
            {
                searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.FROM_DATE, Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.TO_DATE, Convert.ToDateTime(toDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

            }
            recipePlanHeaderDTOList = recipePlanHeaderListBL.GetAllRecipePlanHeaderDTOList(searchParameters, true, true);

            if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Any())
            {
                foreach (RecipePlanHeaderDTO recipePlanHeaderDTO in recipePlanHeaderDTOList)
                {
                    RecipePlanDetailsExcel recipePlanDetailsDTO = new RecipePlanDetailsExcel();
                    recipePlanDetailsDTO.PlanDate = recipePlanHeaderDTO.PlanDateTime;
                    recipePlanDetailsDTOList.Add(recipePlanDetailsDTO);

                    foreach (RecipePlanDetailsDTO detailsDTO in recipePlanHeaderDTO.RecipePlanDetailsDTOList)
                    {
                        RecipePlanDetailsExcel recipePlanDetailsExcel = new RecipePlanDetailsExcel();

                        recipePlanDetailsExcel.PlanDate = null;
                        recipePlanDetailsExcel.PlannedQty = detailsDTO.PlannedQty;
                        recipePlanDetailsExcel.FinalQty = detailsDTO.FinalQty;
                        recipePlanDetailsExcel.RecipeName = ProductContainer.productDTOList.Find(x => x.ProductId == detailsDTO.ProductId).Description;
                        log.Debug("detailsDTO.UOMId:" + detailsDTO.UOMId);
                        recipePlanDetailsExcel.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == detailsDTO.UOMId).UOM;
                        recipePlanDetailsExcel.IncrementedQty = detailsDTO.IncrementalQty;
                        recipePlanDetailsDTOList.Add(recipePlanDetailsExcel);

                    }

                    if (recipePlanHeaderDTO.RecipePlanDetailsDTOList != null && recipePlanHeaderDTO.RecipePlanDetailsDTOList.Any())
                    {
                        foreach (RecipePlanDetailsExcel recipePlanDetailsExcel in recipePlanDetailsDTOList)
                        {
                            recipePlanDetailsExcelDTODefinition.Configure(recipePlanDetailsExcel);
                            Row row = new Row();
                            recipePlanDetailsExcelDTODefinition.Serialize(row, recipePlanDetailsExcel);
                            sheet.AddRow(row);
                        }
                    }
                }
            }
            log.LogMethodExit(sheet);
            return sheet;
        }

    }
}

