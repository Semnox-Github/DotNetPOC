/********************************************************************************************
 * Project Name - TagSerialMappingBL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *2.60        4-May-2019        Nagesh Badiger     Modified -- SaveAndUpdate() and SaveBulkUploadCards() & TagSerialMappingListBL() parameterized Constructor.
 *2.70.2      10-Jul-2019       Lakshminarayana     Modified for adding ultralight c card support.
 *2.90        21-May-2020      Girish Kundar       Modified : Made default constructor as Private   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for TagSerialMapping class.
    /// </summary>
    public class TagSerialMappingBL
    {
        private TagSerialMappingDTO tagSerialMappingDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Parameterized constructor of TagSerialMappingBL class
        /// </summary>
        private TagSerialMappingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the tagSerialMapping id as the parameter
        /// Would fetch the tagSerialMapping object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public TagSerialMappingBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TagSerialMappingDataHandler tagSerialMappingDataHandler = new TagSerialMappingDataHandler(sqlTransaction);
            tagSerialMappingDTO = tagSerialMappingDataHandler.GetTagSerialMappingDTO(id);
            if (tagSerialMappingDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TagSerialMapping", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates TagSerialMappingBL object using the TagSerialMappingDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="tagSerialMappingDTO">TagSerialMappingDTO object</param>
        public TagSerialMappingBL(ExecutionContext executionContext, TagSerialMappingDTO tagSerialMappingDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, tagSerialMappingDTO);
            this.tagSerialMappingDTO = tagSerialMappingDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TagSerialMapping
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            SaveWithoutValidation(sqlTransaction);
            log.LogMethodExit();
        }

        internal void SaveWithoutValidation(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            TagSerialMappingDataHandler tagSerialMappingDataHandler = new TagSerialMappingDataHandler(sqlTransaction);
            if (tagSerialMappingDTO.TagSerialMappingId < 0)
            {
                tagSerialMappingDTO = tagSerialMappingDataHandler.InsertTagSerialMapping(tagSerialMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                tagSerialMappingDTO.AcceptChanges();
            }
            else
            {
                if (tagSerialMappingDTO.IsChanged)
                {
                    tagSerialMappingDTO = tagSerialMappingDataHandler.UpdateTagSerialMapping(tagSerialMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    tagSerialMappingDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the TagSerialMapping
        /// If validation fails/success then Adding the Error messagae and status for each object and return to the UI for WebManagement Studio.
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public TagSerialMappingDTO SaveAndUpdate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                tagSerialMappingDTO.Message = validationErrorList.Select(m => m.Message).SingleOrDefault();
                tagSerialMappingDTO.Status = "Failed";
            }
            else
            {
                try
                {
                    SaveWithoutValidation(sqlTransaction);
                    tagSerialMappingDTO.Status = "Success";
                }
                catch (Exception ex)
                {
                    tagSerialMappingDTO.Message = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                    tagSerialMappingDTO.Status = "Failed";
                }
            }
            log.LogMethodExit(tagSerialMappingDTO);
            return tagSerialMappingDTO;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TagSerialMappingDTO TagSerialMappingDTO
        {
            get
            {
                return tagSerialMappingDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(tagSerialMappingDTO.TagNumber))
            {
                validationErrorList.Add(new ValidationError("TagSerialMapping", "TagNumber", MessageContainerList.GetMessage(executionContext, "Invalid card number")));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }

            TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(executionContext);
            if (tagNumberLengthList.Contains(tagSerialMappingDTO.TagNumber.Trim().Length) == false)
            {
                validationErrorList.Add(new ValidationError("TagSerialMapping", "TagNumber", MessageContainerList.GetMessage(executionContext, "Invalid card number length")));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria(AccountDTO.SearchByParameters.TAG_NUMBER, Operator.EQUAL_TO, tagSerialMappingDTO.TagNumber);
            accountSearchCriteria.OrderBy(AccountDTO.SearchByParameters.ACCOUNT_ID);
            accountSearchCriteria.Paginate(0, 10);
            AccountListBL accountListBL = new AccountListBL(executionContext);
            List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria, false, false, sqlTransaction:sqlTransaction);
            if (accountDTOList != null && accountDTOList.Count > 0)
            {
                validationErrorList.Add(new ValidationError("TagSerialMapping", "TagNumber", MessageContainerList.GetMessage(executionContext, "Card exists in system")));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            TagSerialMappingListBL tagSerialMappingListBL = new TagSerialMappingListBL(executionContext);
            List<KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.TAG_NUMBER, tagSerialMappingDTO.TagNumber.Trim()));
            List<TagSerialMappingDTO> tagSerialMappingDTOList = tagSerialMappingListBL.GetTagSerialMappingDTOList(searchByParameters);
            if (tagSerialMappingDTOList != null && tagSerialMappingDTOList.Count > 0)
            {
                validationErrorList.Add(new ValidationError("TagSerialMapping", "TagNumber", MessageContainerList.GetMessage(executionContext, "Card already loaded")));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            tagSerialMappingListBL = new TagSerialMappingListBL(executionContext);
            searchByParameters = new List<KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER, tagSerialMappingDTO.SerialNumber.Trim()));
            tagSerialMappingDTOList = tagSerialMappingListBL.GetTagSerialMappingDTOList(searchByParameters);
            if (tagSerialMappingDTOList != null && tagSerialMappingDTOList.Count > 0)
            {
                validationErrorList.Add(new ValidationError("TagSerialMapping", "SerialNumber", MessageContainerList.GetMessage(executionContext, "Serial Number already loaded")));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of TagSerialMapping
    /// </summary>
    public class TagSerialMappingListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        List<TagSerialMappingDTO> tagSerialMappingDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public TagSerialMappingListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.tagSerialMappingDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the TagSerialMapping list
        /// </summary>
        public List<TagSerialMappingDTO> GetTagSerialMappingDTOList(List<KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            TagSerialMappingDataHandler tagSerialMappingDataHandler = new TagSerialMappingDataHandler(sqlTransaction);
            List<TagSerialMappingDTO> returnValue = tagSerialMappingDataHandler.GetTagSerialMappingDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="tagSerialMappingDTOList"></param>
        /// <param name="executionContext"></param>
        public TagSerialMappingListBL(List<TagSerialMappingDTO> tagSerialMappingDTOList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(tagSerialMappingDTOList, executionContext);
            this.executionContext = executionContext;
            this.tagSerialMappingDTOList = tagSerialMappingDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or updated the bulk uploading cards
        /// If validation fails/success then adding the TagSerialMappingDTO object into to the List and return to the UI for WebManagement Studio.
        /// </summary>
        /// <returns>resultTagSerialMappingDTOList</returns>

        public List<TagSerialMappingDTO> SaveBulkUploadCards()
        {
            log.LogMethodEntry();
            List<TagSerialMappingDTO> resultTagSerialMappingDTOList = new List<TagSerialMappingDTO>();
            if (tagSerialMappingDTOList != null)
            {
                foreach (TagSerialMappingDTO tagSerialMappingDTO in tagSerialMappingDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TagSerialMappingBL tagSerialMappingBL = new TagSerialMappingBL(executionContext, tagSerialMappingDTO);
                            TagSerialMappingDTO resultTagSerialMappingDTO = tagSerialMappingBL.SaveAndUpdate(parafaitDBTrx.SQLTrx);
                            resultTagSerialMappingDTOList.Add(resultTagSerialMappingDTO);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
                log.LogMethodExit(resultTagSerialMappingDTOList);
            }
            return resultTagSerialMappingDTOList;
        }
    }
}
