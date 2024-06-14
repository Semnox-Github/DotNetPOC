
/********************************************************************************************
 * Project Name - RedemptionCurrency
 * Description  - Bussiness logic of RedemptionCurrency
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Dec-2016   Amaresh      Created 
 *2.70.2        20-Jul-2019   Deeksha      Modifications as per three tier standard.
 *2.110.0    05-Oct-2020   Mushahid Faizan  3 tier changes for Rest API.
 *2.150.0     13-Dec-2022   Abhishek       Modified:Validate() as a part of Web Inventory Redesign.
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Inventory;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// RedemptionCurrency allowes to access the RedemptionCurrency details based on the bussiness logic.
    /// </summary>
    public class RedemptionCurrency
    {
        private RedemptionCurrencyDTO redemptionCurrencyDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private RedemptionCurrency(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            redemptionCurrencyDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the currencyId parameter
        /// </summary>
        /// <param name="currencyId">currencyId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RedemptionCurrency(ExecutionContext executionContext, int currencyId, SqlTransaction sqlTransaction = null) : this(executionContext)
        {
            log.LogMethodEntry(currencyId, sqlTransaction);
            RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler(sqlTransaction);
            this.redemptionCurrencyDTO = redemptionCurrencyDataHandler.GetRedemptionCurrency(currencyId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="redemptionCurrencyDTO">RedemptionCurrencyDTO</param>
        public RedemptionCurrency(ExecutionContext executionContext, RedemptionCurrencyDTO redemptionCurrencyDTO) : this(executionContext)
        {
            log.LogMethodEntry(redemptionCurrencyDTO);
            this.redemptionCurrencyDTO = redemptionCurrencyDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get RedemptionCurrencyDTO Object
        /// </summary>
        public RedemptionCurrencyDTO GetRedemptionCurrencyDTO
        {
            get { return redemptionCurrencyDTO; }
        }

        /// <summary>
        /// Saves the RedemptionCurrency
        /// Checks if the currencyId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// <param name="SqlTransaction">SqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler(sqlTrx);
            if (redemptionCurrencyDTO == null || redemptionCurrencyDTO.IsChanged == false && redemptionCurrencyDTO.CurrencyId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            Validate(sqlTrx);
            if (redemptionCurrencyDTO.CurrencyId < 0)
            {
                redemptionCurrencyDTO = redemptionCurrencyDataHandler.InsertRedemptionCurrency(redemptionCurrencyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                redemptionCurrencyDTO.AcceptChanges();
                AddManagementFormAccess(sqlTrx);
            }
            else
            {
                if (redemptionCurrencyDTO.IsChanged)
                {
                    RedemptionCurrencyDTO existingRedemptionCurrencyDTO = new RedemptionCurrency(executionContext, redemptionCurrencyDTO.CurrencyId, sqlTrx).GetRedemptionCurrencyDTO;
                    redemptionCurrencyDTO = redemptionCurrencyDataHandler.UpdateRedemptionCurrency(redemptionCurrencyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    redemptionCurrencyDTO.AcceptChanges();
                    if (existingRedemptionCurrencyDTO.CurrencyName.ToLower().ToString() != redemptionCurrencyDTO.CurrencyName.ToLower().ToString())
                    {
                        RenameManagementFormAccess(existingRedemptionCurrencyDTO.CurrencyName, sqlTrx);
                    }
                    if (existingRedemptionCurrencyDTO.IsActive != redemptionCurrencyDTO.IsActive)
                    {
                        UpdateManagementFormAccess(redemptionCurrencyDTO.CurrencyName, redemptionCurrencyDTO.IsActive, redemptionCurrencyDTO.Guid, sqlTrx);
                    }
                }
            }
            if (!string.IsNullOrEmpty(redemptionCurrencyDTO.Guid))
            {
                InventoryActivityLogDTO InventoryActivityLogDTO = new InventoryActivityLogDTO(serverTimeObject.GetServerDateTime(), "RedemptionCurrency Inserted",
                                                         redemptionCurrencyDTO.Guid, false, executionContext.GetSiteId(), "RedemptionCurrency", -1, redemptionCurrencyDTO.CurrencyId + ":" + redemptionCurrencyDTO.CurrencyName.ToString(), -1, executionContext.GetUserId(),
                                                         serverTimeObject.GetServerDateTime(), executionContext.GetUserId(), serverTimeObject.GetServerDateTime());


                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, InventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTrx);
            }
            log.LogMethodExit();
        }
        private void AddManagementFormAccess(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionCurrencyDTO.CurrencyId > -1)
            {
                RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler(sqlTransaction);
                redemptionCurrencyDataHandler.AddManagementFormAccess(redemptionCurrencyDTO.CurrencyName, redemptionCurrencyDTO.Guid, executionContext.GetSiteId(), redemptionCurrencyDTO.IsActive);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void RenameManagementFormAccess(string existingFormName, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionCurrencyDTO.CurrencyId > -1)
            {
                RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler(sqlTransaction);
                redemptionCurrencyDataHandler.RenameManagementFormAccess(redemptionCurrencyDTO.CurrencyName, existingFormName, executionContext.GetSiteId(), redemptionCurrencyDTO.Guid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void UpdateManagementFormAccess(string formName, bool updatedIsActive, string functionGuid, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionCurrencyDTO.CurrencyId > -1)
            {
                RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler(sqlTransaction);
                redemptionCurrencyDataHandler.UpdateManagementFormAccess(formName, executionContext.GetSiteId(), updatedIsActive, functionGuid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validates the redemptionCurrencyDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (string.IsNullOrWhiteSpace(redemptionCurrencyDTO.CurrencyName))
            {
                log.Error("Enter currency name ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1586);
                throw new ValidationException(errorMessage);
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(redemptionCurrencyDTO.ValueInTickets.ToString(), @"^[a-zA-Z]+$"))
            {
                log.Error("Please enter an Integer value (>0) for ValueInTickets");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 34, MessageContainerList.GetMessage(executionContext, "ValueInTickets"));
                throw new ValidationException(errorMessage);
            }
            RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler(sqlTransaction);
            List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
            searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = redemptionCurrencyDataHandler.GetRedemptionCurrencyList(searchParameters);
            if (redemptionCurrencyDTOList != null && redemptionCurrencyDTOList.Any())
            {
                if (redemptionCurrencyDTOList.Exists(x => x.CurrencyName.ToLower() == redemptionCurrencyDTO.CurrencyName.ToLower() && redemptionCurrencyDTO.CurrencyId == -1))
                {
                    log.Error("Duplicate entries detail");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Currency Name"));
                    throw new ValidationException(errorMessage);
                }
                if (redemptionCurrencyDTOList.Exists(x => x.CurrencyName.ToLower() == redemptionCurrencyDTO.CurrencyName.ToLower() && x.CurrencyId != redemptionCurrencyDTO.CurrencyId))
                {
                    log.Error("Duplicate Update entries detail");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Currency Name"));
                    throw new ValidationException(errorMessage);
                }
            }
            if (!String.IsNullOrEmpty(redemptionCurrencyDTO.ShortCutKeys))
            {
                if (redemptionCurrencyDTO.ShortCutKeys.Length > 5)
                {
                    log.Error("Shortcut Key length should be less than or equal to 5 characters for currency: ShortCutKeys ");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1587, MessageContainerList.GetMessage(executionContext, "ShortCutKeys"));
                    throw new ValidationException(errorMessage);
                }
                else
                {
                    char[] shortCutKeyArray = redemptionCurrencyDTO.ShortCutKeys.ToCharArray();
                    foreach (char charValue in shortCutKeyArray)
                    {
                        if (!Char.IsLetterOrDigit(charValue))
                        {
                            log.Error("Please enter valid (Alphanumeric) Shortcut Key value for Currency: ShortCutKeys ");
                            string errorMessage = MessageContainerList.GetMessage(executionContext, 1588, MessageContainerList.GetMessage(executionContext, "ShortCutKeys"));
                            throw new ValidationException(errorMessage);
                        }
                    }
                    List<RedemptionCurrencyDTO> duplicateShortCutKey = redemptionCurrencyDTOList.Where(rcDTO => (rcDTO.ShortCutKeys == redemptionCurrencyDTO.ShortCutKeys && rcDTO.CurrencyId != redemptionCurrencyDTO.CurrencyId && rcDTO.IsActive == true)).ToList();
                    if (duplicateShortCutKey.Count > 0)
                    {
                        log.Error("More than one currency record has same Shortcut Key value : ShortCutKeys. Please enter unique value ");
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1589, MessageContainerList.GetMessage(executionContext, "ShortCutKeys"));
                        throw new ValidationException(errorMessage);
                    }
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of RedemptionCurrency
    /// </summary>
    public class RedemptionCurrencyList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = new List<RedemptionCurrencyDTO>();
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public RedemptionCurrencyList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public RedemptionCurrencyList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public RedemptionCurrencyList(ExecutionContext executionContext, List<RedemptionCurrencyDTO> redemptionCurrencyDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.redemptionCurrencyDTOList = redemptionCurrencyDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of Currencies matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetRedemptionCurrenciesCount(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler(sqlTransaction);
            int count = redemptionCurrencyDataHandler.GetRedemptionCurrenciesCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Returns the RedemptionCurrency list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>redemptionCurrencyList</returns>
        public List<RedemptionCurrencyDTO> GetAllRedemptionCurrency(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler(sqlTransaction);
            List<RedemptionCurrencyDTO> redemptionCurrencyList = new List<RedemptionCurrencyDTO>();
            redemptionCurrencyList = redemptionCurrencyDataHandler.GetRedemptionCurrencyList(searchParameters, currentPage, pageSize);
            log.LogMethodExit(redemptionCurrencyList);
            return redemptionCurrencyList;
        }

        /// <summary>
        /// This method is will return Sheet object for RedemptionCurrency.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler(sqlTransaction);
            List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
            searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            redemptionCurrencyDTOList = redemptionCurrencyDataHandler.GetRedemptionCurrencyList(searchParameters);

            RedemptionCurrencyExcelDTODefinition redemptionCurrencyExcelDTODefinition = new RedemptionCurrencyExcelDTODefinition(executionContext, "");
            ///Building headers from RedemptionCurrencyExcelDTODefinition
            redemptionCurrencyExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (redemptionCurrencyDTOList != null && redemptionCurrencyDTOList.Any())
            {
                foreach (RedemptionCurrencyDTO redemptionCurrencyDTO in redemptionCurrencyDTOList)
                {
                    redemptionCurrencyExcelDTODefinition.Configure(redemptionCurrencyDTO);

                    Row row = new Row();
                    redemptionCurrencyExcelDTODefinition.Serialize(row, redemptionCurrencyDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }

        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            RedemptionCurrencyExcelDTODefinition redemptionCurrencyExcelDTODefinition = new RedemptionCurrencyExcelDTODefinition(executionContext, "");
            List<RedemptionCurrencyDTO> rowRedemptionCurrencyDTOList = new List<RedemptionCurrencyDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    RedemptionCurrencyDTO rowRedemptionCurrencyDTO = (RedemptionCurrencyDTO)redemptionCurrencyExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowRedemptionCurrencyDTOList.Add(rowRedemptionCurrencyDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowRedemptionCurrencyDTOList != null && rowRedemptionCurrencyDTOList.Any())
                    {
                        RedemptionCurrencyList redemptionCurrencyListBL = new RedemptionCurrencyList(executionContext, rowRedemptionCurrencyDTOList);
                        redemptionCurrencyListBL.Save(sqlTransaction);
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
        /// Save and Update the redemptionCurrencyDTOList details
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionCurrencyDTOList == null ||
                redemptionCurrencyDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < redemptionCurrencyDTOList.Count; i++)
            {
                var redemptionCurrencyDTO = redemptionCurrencyDTOList[i];
                if (redemptionCurrencyDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RedemptionCurrency redemptionCurrency = new RedemptionCurrency(executionContext, redemptionCurrencyDTO);
                    redemptionCurrency.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving redemptionCurrencyDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("redemptionCurrencyDTO", redemptionCurrencyDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        public DateTime? GetRedemptionCurrencyLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyDataHandler redemptionCurrencyDataHandler = new RedemptionCurrencyDataHandler();
            DateTime? result = redemptionCurrencyDataHandler.GetRedemptionCurrencyModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
