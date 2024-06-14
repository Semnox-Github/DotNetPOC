/********************************************************************************************
 * Project Name - Vendor
 * Description  - Vendor BL
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2       25-Jul-2019       Deeksha            Modifications as per three tier standard.
 *2.110.0     14-Oct-2020   Mushahid Faizan   Modified as per 3 tier standards, Added methods for Pagination, Validations and Excel Sheet functionalities,
 *                                            Removed SearchVendorList, Added Catch block in Save.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Vendor
{
    /// <summary>
    /// class Vendor
    /// </summary>
    public class Vendor
    {
        private VendorDTO vendorDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private Vendor(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="vendorDTO">Parameter of the type PurchaseTaxDTO</param>
        public Vendor(ExecutionContext executionContext, VendorDTO vendorDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, vendorDTO);
            this.vendorDTO = vendorDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Vendor id as the parameter
        /// Would fetch the Vendor object based on the ID passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="VendorId">Location id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Vendor(ExecutionContext executionContext, int VendorId, SqlTransaction sqlTransaction = null) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, VendorId, sqlTransaction);
            VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
            vendorDTO = vendorDataHandler.GetVendor(VendorId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the Vendor
        /// </summary>
        /// <returns>validationErrors</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();

            VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
            searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, executionContext.GetSiteId().ToString()));
            List<VendorDTO> vendorDTOList = vendorDataHandler.GetVendorList(searchParameters);


            if (vendorDTOList != null && vendorDTOList.Any())
            {
                if (vendorDTOList.Exists(x => x.Name.ToLower() == vendorDTO.Name.ToLower()) && vendorDTO.VendorId == -1)
                {
                    log.Debug("Duplicate entries detail");
                    validationErrorList.Add(new ValidationError("Vendor", "Name", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Vendor"))));
                }
                if (vendorDTOList.Exists(x => x.Name.ToLower() == vendorDTO.Name.ToLower() && x.VendorId != vendorDTO.VendorId))
                {
                    log.Debug("Duplicate update entries detail");
                    validationErrorList.Add(new ValidationError("Vendor", "Name", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Vendor"))));
                }
            }

            if ((!string.IsNullOrWhiteSpace(vendorDTO.Email) ) && !Regex.IsMatch(vendorDTO.Email, @"^((([\w]+\.[\w]+)+)|([\w]+))@(([\w]+\.)+)([A-Za-z]{1,9})$"))
            {
                log.Debug("Please enter valid email");
                validationErrorList.Add(new ValidationError("Vendor", "Email", MessageContainerList.GetMessage(executionContext, "Please enter valid email", MessageContainerList.GetMessage(executionContext, "Email"))));
            }

            if (string.IsNullOrWhiteSpace(vendorDTO.Name))
            {
                ValidationError validationError = new ValidationError("Vendor", "Name", MessageContainerList.GetMessage(executionContext, 2607, MessageContainerList.GetMessage(executionContext, "Vendor")));
                validationErrorList.Add(validationError);
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the Vendor
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public virtual void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (vendorDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Vendor not changed.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Count > 0)
            {
                log.LogMethodExit(null, "Validation failed : " + string.Join(", ", validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation failed.", validationErrors);
            }
            VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
            vendorDataHandler.Save(vendorDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }
        /// <summary>
        /// vendor DTO
        /// </summary>
        public VendorDTO getVendorDTO
        {
            get
            {
                return vendorDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of vendor
    /// </summary>
    public class VendorList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<VendorDTO> vendorDTOList = new List<VendorDTO>();
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public VendorList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="vendorDTOList">vendorDTOList</param>
        public VendorList(ExecutionContext executionContext, List<VendorDTO> vendorDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, vendorDTOList);
            this.vendorDTOList = vendorDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the vendor list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<VendorDTO> GetAllVendors(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
            var result = vendorDataHandler.GetVendorList(searchParameters, currentPage, pageSize);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Retriving vendor by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the vendor</param>
        ///  <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of VendorDTO </returns>
        public List<VendorDTO> GetVendorList(string sqlQuery, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlQuery, sqlTransaction);
            VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
            var result = vendorDataHandler.GetVendorList(sqlQuery);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the coulumns name list of vendor table.
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        /// <returns>result</returns>
        public DataTable GetVendorColumnsName(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
            var result = vendorDataHandler.GetVendorColumns();
            log.LogMethodExit(result);
            return result;
        }

        ///// <summary>
        ///// Returns the vendor list for the Search Parameter
        ///// </summary>
        ///// <param name="searchParameters">searchParameters</param>
        ///// <param name="sqlTransaction">sqlTransaction</param>
        ///// <returns>result</returns>
        //public List<VendorDTO> SearchVendorList(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(searchParameters, sqlTransaction);
        //    VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
        //    var result = vendorDataHandler.SearchVendorList(searchParameters);
        //    log.LogMethodExit(result);
        //    return result;
        //}

        /// <summary>
        /// Returns the no of Vendors matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetVendorCount(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
            int count = vendorDataHandler.GetVendorCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }


        /// <summary>
        /// This method is will return Sheet object for vendorDTO.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
            searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, executionContext.GetSiteId().ToString()));
            vendorDTOList = vendorDataHandler.GetVendorList(searchParameters);

            VendorExcelDTODefinition vendorExcelDTODefinition = new VendorExcelDTODefinition(executionContext, "");
            ///Building headers from VendorExcelDTODefinition
            vendorExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (vendorDTOList != null && vendorDTOList.Any())
            {
                foreach (VendorDTO vendorDTO in vendorDTOList)
                {
                    vendorExcelDTODefinition.Configure(vendorDTO);

                    Row row = new Row();
                    vendorExcelDTODefinition.Serialize(row, vendorDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }


        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            VendorExcelDTODefinition vendorExcelDTODefinition = new VendorExcelDTODefinition(executionContext, "");
            List<VendorDTO> rowVendorDTOList = new List<VendorDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    VendorDTO rowVendorDTO = (VendorDTO)vendorExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowVendorDTOList.Add(rowVendorDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowVendorDTOList != null && rowVendorDTOList.Any())
                    {
                        VendorList vendorsListBL = new VendorList(executionContext, rowVendorDTOList);
                        vendorsListBL.Save(sqlTransaction);
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
        /// Validates and saves the vendorDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (vendorDTOList == null ||
                vendorDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            List<VendorDTO> updatedVendorDTOList = new List<VendorDTO>(vendorDTOList.Count);
            for (int i = 0; i < vendorDTOList.Count; i++)
            {
                if (vendorDTOList[i].IsChanged == false)
                {
                    continue;
                }
                Vendor vendor = new Vendor(executionContext, vendorDTOList[i]);
                List<ValidationError> validationErrors = vendor.Validate();
                if (validationErrors.Any())
                {
                    validationErrors.ToList().ForEach(c => c.RecordIndex = i + 1);
                    log.LogMethodExit(null, "Validation failed. " + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException("Validation failed for Category.", validationErrors, i);
                }
                updatedVendorDTOList.Add(vendorDTOList[i]);
            }
            if (updatedVendorDTOList.Any() == false)
            {
                log.LogMethodExit(null, "Nothing changed.");
                return;
            }
            try
            {
                VendorDataHandler vendorDataHandler = new VendorDataHandler(sqlTransaction);
                vendorDataHandler.Save(updatedVendorDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
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
                log.Error("Error occurred while saving updatedVendorDTOList.", ex);
                log.LogVariableState("updatedVendorDTOList", updatedVendorDTOList);
                throw;
            }
            log.LogMethodExit();
        }

        public List<int> GetInactiveVendorsToBePublished(int masterSiteId)
        {
            log.LogMethodEntry(masterSiteId);
            VendorDataHandler vendorDataHandler = new VendorDataHandler();
            List<int> result = vendorDataHandler.GetInactiveVendorToBePublished(masterSiteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
