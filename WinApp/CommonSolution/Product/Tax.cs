/********************************************************************************************
 * Project Name - Tax
 * Description  - Bussiness logic of Tax
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Jan-2016   Raghuveera          Created 
 *2.70        01-Feb-2019   Mushahid Faizan     Modified - Added Save(), SaveUpdateTaxList(), GetTaxDTOList() for CRUD Operations.
 *2.70        02-Apr-2019   Akshay Gulaganji    Added isActive search parameter to child i.e., TaxStructureDTO in GetTaxDTOList() method
 *2.70        15-Jul-2019   Mehraj              Implemented Delete() and DeleteTaxList()
 *2.110.0     07-Oct-2020   Mushahid Faizan     Modified as per 3 tier standards, Added methods for Pagination and Excel Sheet functionalities,
 *                                              Renamed SaveUpdateTaxList method to Save.
 *2.150.0     13-Dec-2022   Abhishek            Modified:Validate() as a part of Web Inventory Redesign.                                              
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Tax creates and modifies the tax details 
    /// </summary>
    public class Tax
    {
        private TaxDTO taxDTO;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// parameterized constructor of Tax class
        /// </summary>
        private Tax(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            taxDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public Tax(ExecutionContext executionContext, int taxId) : this(executionContext)
        {
            log.LogMethodEntry(taxId);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchByTaxParameters
                = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            searchByTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_ID, taxId.ToString()));
            TaxDataHandler assetTaxDataHandler = new TaxDataHandler();
            List<TaxDTO> taxDTOList = assetTaxDataHandler.GetTaxList(searchByTaxParameters);
            if (taxDTOList != null && taxDTOList.Count > 0)
            {
                taxDTO = taxDTOList[0];
            }
            else
            {
                taxDTO = null;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor of Tax class
        /// </summary>
        /// <param name="taxDTO">TaxDTO object</param>
        /// <param name="executionContext"></param>
        public Tax(ExecutionContext executionContext, TaxDTO taxDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, taxDTO);
            this.taxDTO = taxDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Tax id as the parameter
        /// Would fetch the Tax object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of Tax Object</param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false.</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false.</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Tax(ExecutionContext executionContext, int id, bool loadChildRecords = false,
                                       bool activeChildRecords = false, SqlTransaction sqlTransaction = null)       //added constructor
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TaxDataHandler taxDataHandler = new TaxDataHandler(sqlTransaction);
            taxDTO = taxDataHandler.GetTaxDTO(id, sqlTransaction);
            if (taxDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Tax", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for Tax object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)    //added build
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            TaxStructureList taxStructureList = new TaxStructureList(executionContext);
            List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>> searchByTaxParams = new List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>>();
            searchByTaxParams.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.TAX_ID, taxDTO.TaxId.ToString()));
            searchByTaxParams.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchByTaxParams.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.IS_ACTIVE, "1"));
            }
            List<TaxStructureDTO> taxStructureDTOList = taxStructureList.GetTaxStructureList(searchByTaxParams, sqlTransaction);
            taxDTO.TaxStructureDTOList = taxStructureDTOList;
            log.LogMethodExit();
        }
        public double GetTaxAmount(double amount, bool IsInclusive)
        {
            log.LogMethodEntry(amount, IsInclusive);
            double taxAmount = 0.0;
            if (taxDTO != null)
            {
                if (IsInclusive)
                {
                    taxAmount = (taxDTO.TaxPercentage / 100 * amount / (1 + taxDTO.TaxPercentage / 100));
                }
                else
                {
                    taxAmount = amount * taxDTO.TaxPercentage / 100;
                }
            }
            log.LogMethodExit(taxAmount);
            return taxAmount;
        }

        public TaxDTO GetTaxDTO() { return this.taxDTO; }

        /// <summary>
        /// Saves the Tax  
        /// Tax will be inserted if id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry();
            TaxDataHandler taxDataHandler = new TaxDataHandler(SQLTrx);
            if (taxDTO == null || taxDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            Validate(SQLTrx);
            if (taxDTO.TaxId < 0)
            {
                taxDTO = taxDataHandler.InsertTax(taxDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                taxDTO.AcceptChanges();
            }
            else
            {
                if (taxDTO.IsChanged)
                {
                    taxDTO = taxDataHandler.UpdateTax(taxDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    taxDTO.AcceptChanges();
                }
            }
            SaveChild(SQLTrx);
            UpdatePercentage(SQLTrx);
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the child records : TaxStructureDTOList 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveChild(SqlTransaction sqlTransaction)
        {
            if (taxDTO.TaxStructureDTOList != null &&
                taxDTO.TaxStructureDTOList.Any())
            {
                List<TaxStructureDTO> updatedTaxStructureDTOList = new List<TaxStructureDTO>();
                foreach (var taxStructureDTO in taxDTO.TaxStructureDTOList)
                {
                    if (taxStructureDTO.TaxId != taxDTO.TaxId)
                    {
                        taxStructureDTO.TaxId = taxDTO.TaxId;
                    }
                    if (taxStructureDTO.IsChanged)
                    {
                        updatedTaxStructureDTOList.Add(taxStructureDTO);
                    }
                }
                if (updatedTaxStructureDTOList.Any())
                {
                    TaxStructureList taxStructureBL = new TaxStructureList(executionContext, updatedTaxStructureDTOList);
                    taxStructureBL.Save(sqlTransaction);
                }
                taxDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Saves the child records : TaxStructureDTOList
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void UpdatePercentage(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            Tax tax = new Tax(executionContext, taxDTO.TaxId, true, true, sqlTransaction);
            TaxDTO updatedTaxDTO = tax.GetTaxDTO();
            double structureTax = 0;
            if (updatedTaxDTO.TaxStructureDTOList == null || updatedTaxDTO.TaxStructureDTOList.Any() == false)
            {
                structureTax = updatedTaxDTO.TaxPercentage;
            }
            if (updatedTaxDTO.TaxStructureDTOList != null && updatedTaxDTO.TaxStructureDTOList.Any())
            {
                foreach (TaxStructureDTO taxStructureDTO in updatedTaxDTO.TaxStructureDTOList)
                {
                    if (taxStructureDTO.IsActive)
                    {
                        if (taxStructureDTO.ParentStructureId > -1)
                        {
                            structureTax = structureTax + CalulatePercentage(taxStructureDTO.Percentage, taxStructureDTO.ParentStructureId, updatedTaxDTO.TaxStructureDTOList);
                        }
                        else
                        {
                            structureTax += taxStructureDTO.Percentage;
                        }
                    }
                }
            }
            taxDTO.TaxPercentage = structureTax;
            TaxDataHandler taxDataHandler = new TaxDataHandler(sqlTransaction);
            taxDTO = taxDataHandler.UpdateTax(taxDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            taxDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : TaxStructureDTOList
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private double CalulatePercentage(double taxPercentage, int taxStructureId, List<TaxStructureDTO> taxStructureDTOList)
        {
            log.LogMethodEntry(taxPercentage, taxStructureId, taxStructureDTOList);
            TaxStructureDTO taxStructureDTO = null;
            taxStructureDTO = taxStructureDTOList.Where(x => x.TaxStructureId == taxStructureId).FirstOrDefault();
            if (taxStructureDTO.ParentStructureId < 0)
            {
                taxPercentage = (taxPercentage * taxStructureDTO.Percentage) / 100;
            }
            else
            {
                taxPercentage = (taxPercentage * CalulatePercentage(taxStructureDTO.Percentage, taxStructureDTO.ParentStructureId, taxStructureDTOList)) / 100;
            }
            log.LogMethodExit(taxPercentage);
            return taxPercentage;
        }

        /// <summary>
        /// Validates the TaxDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (string.IsNullOrWhiteSpace(taxDTO.TaxName))
            {
                log.Error("Enter Tax Name ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2607, MessageContainerList.GetMessage(executionContext, "Tax"));
                throw new ValidationException(errorMessage);
            }
            if (string.IsNullOrWhiteSpace(taxDTO.TaxPercentage.ToString()) || (System.Text.RegularExpressions.Regex.IsMatch(taxDTO.TaxPercentage.ToString(), @"^[a-zA-Z]+$")))
            {
                log.Error("Enter Tax Percentage ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "TaxPercentage"));
                throw new ValidationException(errorMessage);
            }
            TaxDataHandler TaxDataHandler = new TaxDataHandler(sqlTransaction);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<TaxDTO> TaxDTOList = TaxDataHandler.GetTaxList(searchParameters);
            if (taxDTO.TaxId > -1 && taxDTO.ActiveFlag == false)
            {
                ProductsDataHandler productsDataHandler = new ProductsDataHandler(sqlTransaction);
                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.TAX_ID, taxDTO.TaxId.ToString()));
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "1"));
                List<ProductsDTO> productsDTOList = productsDataHandler.GetAllProductList(searchParams);
                if (productsDTOList != null && productsDTOList.Any())
                {
                    log.Error("Unable to delete Tax.An active product with this tax exists.");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4751);
                    throw new ValidationException(errorMessage);
                }
            }
            if (TaxDTOList != null && TaxDTOList.Any())
            {
                if (TaxDTOList.Exists(x => x.TaxName.ToLower() == taxDTO.TaxName.ToLower()) && taxDTO.TaxId == -1)
                {
                    log.Error("Duplicate entries detail ");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Tax"));
                    throw new ValidationException(errorMessage);
                }
                if (TaxDTOList.Exists(x => x.TaxName.ToLower() == taxDTO.TaxName.ToLower() && x.TaxId != taxDTO.TaxId))
                {
                    log.Error("Duplicate entries detail ");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Tax"));
                    throw new ValidationException(errorMessage);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete a Tax record 
        /// </summary>
        /// <param name="SQLTrx"></param>
        public void Delete(SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry();
            try
            {
                TaxDataHandler taxDataHandler = new TaxDataHandler(SQLTrx);
                taxDataHandler.DeleteTax(taxDTO.TaxId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }
    /// <summary>
    /// Manages the list of asset tax DTOs
    /// </summary>
    public class TaxList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TaxDTO> taxList = new List<TaxDTO>();
        private List<TaxStructureDTO> taxStructureList = new List<TaxStructureDTO>();
        private ExecutionContext executionContext;
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// No Parameter constructor
        /// </summary>
        public TaxList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public TaxList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="taxList"></param>
        /// <param name="executionContext"></param>
        public TaxList(ExecutionContext executionContext, List<TaxDTO> taxList) : this(executionContext)
        {
            log.LogMethodEntry(taxList, executionContext);
            this.taxList = taxList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the asset tax list
        /// </summary>
        public List<TaxDTO> GetAllTaxes(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchByTaxParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                        SqlTransaction sqlTransaction = null, int currentPage = 0, int pageSize = 0)//starts:Modification on 18-Jul-2016 for publish feature
        {
            log.LogMethodEntry(searchByTaxParameters, loadChildRecords, loadActiveRecords);
            TaxDataHandler assetTaxDataHandler = new TaxDataHandler(sqlTransaction);
            List<TaxDTO> taxDTOList = assetTaxDataHandler.GetTaxDTOList(searchByTaxParameters, currentPage, pageSize);
            if (taxDTOList != null && taxDTOList.Count != 0 && loadChildRecords)
            {
                Build(taxDTOList, loadActiveRecords, currentPage, pageSize, sqlTransaction);
            }
            log.LogMethodExit(taxDTOList);
            return taxDTOList;
        }//Ends:Modification on 18-Jul-2016 for publish feature

        private void Build(List<TaxDTO> taxDTOList, bool activeChildRecords, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            Dictionary<int, TaxDTO> taxDTODictionary = new Dictionary<int, TaxDTO>();
            List<int> taxIdList = new List<int>();
            for (int i = 0; i < taxDTOList.Count; i++)
            {
                if (taxDTODictionary.ContainsKey(taxDTOList[i].TaxId))
                {
                    continue;
                }
                taxDTODictionary.Add(taxDTOList[i].TaxId, taxDTOList[i]);
                taxIdList.Add(taxDTOList[i].TaxId);
            }
            TaxStructureList taxStructureListBL = new TaxStructureList(executionContext);
            List<TaxStructureDTO> taxStructureDTOList = taxStructureListBL.GetTaxStructureDTOList(taxIdList, activeChildRecords, currentPage, pageSize, sqlTransaction);

            if (taxStructureDTOList != null && taxStructureDTOList.Any())
            {
                for (int i = 0; i < taxStructureDTOList.Count; i++)
                {
                    if (taxDTODictionary.ContainsKey(taxStructureDTOList[i].TaxId) == false)
                    {
                        continue;
                    }
                    TaxDTO taxDTO = taxDTODictionary[taxStructureDTOList[i].TaxId];
                    if (taxDTO.TaxStructureDTOList == null)
                    {
                        taxDTO.TaxStructureDTOList = new List<TaxStructureDTO>();
                    }
                    taxDTO.TaxStructureDTOList.Add(taxStructureDTOList[i]);
                }
            }
        }

        /// <summary>
        /// Returns the no of Taxes matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetTaxCount(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TaxDataHandler taxDataHandler = new TaxDataHandler(sqlTransaction);
            int count = taxDataHandler.GetTaxCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// This method is will return Sheet object for Tax.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            TaxDataHandler uomDataHandler = new TaxDataHandler(sqlTransaction);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            taxList = uomDataHandler.GetTaxList(searchParameters);

            TaxExcelDTODefinition taxExcelDTODefinition = new TaxExcelDTODefinition(executionContext, "");
            ///Building headers from TaxExcelDTODefinition
            taxExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (taxList != null && taxList.Any())
            {
                foreach (TaxDTO taxDTO in taxList)
                {
                    taxExcelDTODefinition.Configure(taxDTO);

                    Row row = new Row();
                    taxExcelDTODefinition.Serialize(row, taxDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }


        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            TaxExcelDTODefinition taxExcelDTODefinition = new TaxExcelDTODefinition(executionContext, "");
            List<TaxDTO> rowTaxDTOList = new List<TaxDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    TaxDTO rowTaxDTO = (TaxDTO)taxExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowTaxDTOList.Add(rowTaxDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowTaxDTOList != null && rowTaxDTOList.Any())
                    {
                        TaxList taxListBL = new TaxList(executionContext, rowTaxDTOList);
                        taxListBL.Save();
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
        /// Save and Updated the Tax details
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();

            if (taxList == null ||
               taxList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < taxList.Count; i++)
            {
                var taxDTO = taxList[i];
                if (taxDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    Tax taxBL = new Tax(executionContext, taxDTO);
                    taxBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving taxDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("taxDTO", taxDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete taxlist and taxstructure
        /// </summary>
        public void DeleteTaxList()
        {
            log.LogMethodEntry();

            if (taxList != null)
            {
                foreach (TaxDTO taxDto in taxList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        try
                        {
                            if (taxDto.TaxStructureDTOList != null && taxDto.TaxStructureDTOList.Count > 0)
                            {
                                foreach (TaxStructureDTO taxStructureDTO in taxDto.TaxStructureDTOList)
                                {
                                    if (taxStructureDTO.IsChanged && taxStructureDTO.IsActive == false)
                                    {
                                        TaxStructureBL taxStructureBL = new TaxStructureBL(executionContext, taxStructureDTO);
                                        taxStructureBL.Delete(parafaitDBTrx.SQLTrx);
                                    }
                                }
                            }
                            if (taxDto.IsChanged)
                            {
                                Tax tax = new Tax(executionContext, taxDto);
                                tax.Delete(parafaitDBTrx.SQLTrx);

                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
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
                            log.Error(ex);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the Last Updated time of The Tax.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public DateTime? GetTaxModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            TaxDataHandler taxDataHandler = new TaxDataHandler(null);
            DateTime? result = taxDataHandler.GetTaxModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
