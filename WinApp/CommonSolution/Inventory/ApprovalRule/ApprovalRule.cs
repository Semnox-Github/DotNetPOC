/********************************************************************************************
 * Project Name - Approval Rule BL
 * Description  - BL of ApprovalRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019   Deeksha         modifications as per 3 tier standards
 *2.110.0     14-Oct-2020   Mushahid Faizan   Modified as per 3 tier standards, Added methods for Pagination, Validations and Excel Sheet functionalities,
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    public class ApprovalRule
    {
        private ApprovalRuleDTO approvalRuleDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ApprovalRule class
        /// </summary>
        private ApprovalRule(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the ApprovalRule DTO based on the approvalRule id passed 
        /// </summary>
        /// <param name="approvalRuleId">ApprovalRule id</param>
        public ApprovalRule(ExecutionContext executionContext, int approvalRuleId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(approvalRuleId, sqlTransaction);
            ApprovalRuleDataHandler approvalRuleDataHandler = new ApprovalRuleDataHandler(sqlTransaction);
            approvalRuleDTO = approvalRuleDataHandler.GetApprovalRule(approvalRuleId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates approvalRule object using the ApprovalRuleDTO
        /// </summary>
        /// <param name="approvalRule">ApprovalRuleDTO object</param>
        public ApprovalRule(ExecutionContext executionContext, ApprovalRuleDTO approvalRule)
            : this(executionContext)
        {
            log.LogMethodEntry(approvalRule);
            this.approvalRuleDTO = approvalRule;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the approvalRule details on identifier
        /// </summary>
        /// <param name="identifire">integer type parameter</param>
        /// <returns>Returns ApprovalRuleDTO</returns>
        public ApprovalRuleDTO GetUserMessage(int identifire, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(identifire, sqlTransaction);
            ApprovalRuleDataHandler approvalRuleDataHandler = new ApprovalRuleDataHandler(sqlTransaction);
            ApprovalRuleDTO approvalRuleDTO = new ApprovalRuleDTO();
            approvalRuleDTO = approvalRuleDataHandler.GetApprovalRule(identifire);
            log.LogMethodExit(approvalRuleDTO);
            return approvalRuleDTO;
        }
        /// <summary>
        /// Saves the approval rule record
        /// Checks if the message id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ApprovalRuleDataHandler approvalRuleDataHandler = new ApprovalRuleDataHandler(sqlTransaction);
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            if (approvalRuleDTO == null || approvalRuleDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (approvalRuleDTO.ApprovalRuleID < 0)
            {
                approvalRuleDTO = approvalRuleDataHandler.InsertApprovalRule(approvalRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                approvalRuleDTO.AcceptChanges();
            }
            else
            {
                if (approvalRuleDTO.IsChanged)
                {
                    approvalRuleDTO = approvalRuleDataHandler.UpdateApprovalRule(approvalRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    approvalRuleDTO.AcceptChanges();
                }
            }
            if (!string.IsNullOrEmpty(approvalRuleDTO.Guid))
            {
                InventoryActivityLogDTO InventoryActivityLogDTO = new InventoryActivityLogDTO(serverTimeObject.GetServerDateTime(), "ApprovalRule Inserted",
                                                         approvalRuleDTO.Guid, false, executionContext.GetSiteId(), "ApprovalRule", -1, approvalRuleDTO.ApprovalRuleID.ToString(), -1, executionContext.GetUserId(),
                                                         serverTimeObject.GetServerDateTime(), executionContext.GetUserId(), serverTimeObject.GetServerDateTime());


                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, InventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (approvalRuleDTO.RoleId < 0)
            {
                log.Debug("Please select user role");
                validationErrorList.Add(new ValidationError("ApprovalRule", "RoleId", MessageContainerList.GetMessage(executionContext, "Please select user role", MessageContainerList.GetMessage(executionContext, "RoleId"))));
            }
            if (approvalRuleDTO.DocumentTypeID < 0)
            {
                log.Debug("Please select document type");
                validationErrorList.Add(new ValidationError("ApprovalRule", "DocumentTypeID", MessageContainerList.GetMessage(executionContext, "Please select document type", MessageContainerList.GetMessage(executionContext, "DocumentTypeID"))));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Returns the approvalRule list
        /// </summary>
        public ApprovalRuleDTO GetApprovalRule(int roleId, int documentTypeId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(roleId, documentTypeId, siteId, sqlTransaction);
            List<ApprovalRuleDTO> approvalRuleDTOList = new List<ApprovalRuleDTO>();
            ApprovalRuleDataHandler approvalRuleDataHandler = new ApprovalRuleDataHandler(sqlTransaction);
            List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchByApprovalRuleParameters = new List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>>();
            searchByApprovalRuleParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.ACTIVE_FLAG, "1"));
            searchByApprovalRuleParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.DOCUMENT_TYPE_ID, documentTypeId.ToString()));
            searchByApprovalRuleParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.ROLE_ID, roleId.ToString()));
            searchByApprovalRuleParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.SITE_ID, siteId.ToString()));
            approvalRuleDTOList = approvalRuleDataHandler.GetApprovalRuleList(searchByApprovalRuleParameters);
            if (approvalRuleDTOList != null && approvalRuleDTOList.Count > 0)
            {
                approvalRuleDTO = approvalRuleDTOList[0];
                log.LogMethodExit(approvalRuleDTO);
                return approvalRuleDTO;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ApprovalRuleDTO getApprovalRuleDTO { get { return approvalRuleDTO; } }
    }


    /// <summary>
    /// Manages the list of approvalRules
    /// </summary>
    public class ApprovalRulesList
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ApprovalRuleDTO> approvalRuleDTOList = new List<ApprovalRuleDTO>();
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ApprovalRulesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="ApprovalRuleDTOList">ApprovalRuleDTOList</param>
        public ApprovalRulesList(ExecutionContext executionContext, List<ApprovalRuleDTO> approvalRuleDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, approvalRuleDTOList);
            this.approvalRuleDTOList = approvalRuleDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the approvalRule list
        /// </summary>
        public List<ApprovalRuleDTO> GetAllApprovalRule(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ApprovalRuleDataHandler approvalRuleDataHandler = new ApprovalRuleDataHandler(sqlTransaction);
            this.approvalRuleDTOList = approvalRuleDataHandler.GetApprovalRuleList(searchParameters, currentPage, pageSize);
            log.LogMethodExit(approvalRuleDTOList);
            return approvalRuleDTOList;
        }

        /// <summary>
        /// Returns the no of Approval Rules matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetApprovalRuleCount(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ApprovalRuleDataHandler approvalRuleDataHandler = new ApprovalRuleDataHandler(sqlTransaction);
            int count = approvalRuleDataHandler.GetApprovalRuleCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }


        /// <summary>
        /// This method is will return Sheet object for approvalRuleDTO.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            ApprovalRuleDataHandler approvalRuleDataHandler = new ApprovalRuleDataHandler(sqlTransaction);
            List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters = new List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>>();
            searchParameters.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            approvalRuleDTOList = approvalRuleDataHandler.GetApprovalRuleList(searchParameters);

            ApprovalRuleExcelDTODefinition approvalRuleExcelDTODefinition = new ApprovalRuleExcelDTODefinition(executionContext, "");
            ///Building headers from ApprovalRuleExcelDTODefinition
            approvalRuleExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (approvalRuleDTOList != null && approvalRuleDTOList.Any())
            {
                foreach (ApprovalRuleDTO approvalRuleDTO in approvalRuleDTOList)
                {
                    approvalRuleExcelDTODefinition.Configure(approvalRuleDTO);

                    Row row = new Row();
                    approvalRuleExcelDTODefinition.Serialize(row, approvalRuleDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }

        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            ApprovalRuleExcelDTODefinition approvalRuleExcelDTODefinition = new ApprovalRuleExcelDTODefinition(executionContext, "");
            List<ApprovalRuleDTO> rowApprovalRuleDTOList = new List<ApprovalRuleDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    ApprovalRuleDTO rowApprovalRuleDTO = (ApprovalRuleDTO)approvalRuleExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowApprovalRuleDTOList.Add(rowApprovalRuleDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowApprovalRuleDTOList != null && rowApprovalRuleDTOList.Any())
                    {
                        ApprovalRulesList approvalRulesListBL = new ApprovalRulesList(executionContext, rowApprovalRuleDTOList);
                        approvalRulesListBL.Save(sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            log.LogMethodExit(keyValuePairs);
            return keyValuePairs;

        }

        /// <summary>
        /// Saves UOM List
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            if (approvalRuleDTOList == null ||
               approvalRuleDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < approvalRuleDTOList.Count; i++)
            {
                var approvalRuleDTO = approvalRuleDTOList[i];
                if (approvalRuleDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ApprovalRule approvalRuleBL = new ApprovalRule(executionContext, approvalRuleDTO);
                    List<ValidationError> validationErrors = approvalRuleBL.Validate(sqlTransaction);
                    if (validationErrors.Any())
                    {
                        validationErrors.ToList().ForEach(c => c.RecordIndex = i + 1);
                        log.LogMethodExit(null, "Validation failed. " + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                        throw new ValidationException("Validation failed for Category.", validationErrors, i);
                    }
                    approvalRuleBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving approvalRuleDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("approvalRuleDTO", approvalRuleDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
