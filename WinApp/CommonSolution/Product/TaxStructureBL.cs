/********************************************************************************************
 * Project Name - TaxStructure BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 **********************************************************************************************
*2.50        30-Jan-2019   Mushahid Faizan     Created
*2.50        17-Mar-2019   Manoj Durgam        Added ExecutionContext to the constructor of TaxStructureBL
*2.60        11-Apr-2019   Girish Kundar       Copied this file to Inventory Module 
*2.70        15-Jul-2019   Mehraj              Added Delete() method
*2.110.0     08-Oct-2020   Mushahid Faizan     Added GetTaxStructureDTOList for pagination.
*2.150.0     13-Dec-2022   Abhishek            Modified:Validate() as a part of Web Inventory Redesign.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for TaxStructure class.
    /// </summary>
    public class TaxStructureBL
    {
        private TaxStructureDTO taxStructureDTO = new TaxStructureDTO();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TaxStructureBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            taxStructureDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates TaxStructureBL object using the TaxStructureDTO 
        /// </summary>
        /// <param name="executionContex"></param>
        /// <param name="taxStructureDTO"></param>
        public TaxStructureBL(ExecutionContext executionContext, TaxStructureDTO taxStructureDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, taxStructureDTO);
            this.taxStructureDTO = taxStructureDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the TaxStructureBL id as the parameter
        /// Would fetch the TaxStructureBL object from the database based on the id passed. 
        /// </summary>
        public TaxStructureBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)       //added constructor
       : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TaxStructureDataHandler taxStructureDataHandler = new TaxStructureDataHandler(sqlTransaction);
            taxStructureDTO = taxStructureDataHandler.GetTaxStructureDTO(id);
            if (taxStructureDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TaxStructure", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TaxStructure  
        /// TaxStructure will be inserted if id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry();
            Validate(SQLTrx);
            TaxStructureDataHandler taxStructureHandler = new TaxStructureDataHandler(SQLTrx);
            if (taxStructureDTO.TaxStructureId < 0)
            {
                taxStructureDTO = taxStructureHandler.InsertTaxStructure(taxStructureDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                taxStructureDTO.AcceptChanges();
            }
            else if (taxStructureDTO.IsChanged && taxStructureDTO.DeleteTaxStructure)
            {
                taxStructureDTO = taxStructureHandler.DeleteTaxStructure(taxStructureDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                taxStructureDTO.AcceptChanges();
            }
            else if (taxStructureDTO.IsChanged)
            {
                taxStructureDTO = taxStructureHandler.UpdateTaxStructure(taxStructureDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                taxStructureDTO.AcceptChanges();
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Delete Taxstructure records
        /// </summary>
        /// <param name="SQLTrx"></param>
        public void Delete(SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry();
            try
            {
                TaxStructureDataHandler taxStructureDataHandler = new TaxStructureDataHandler(SQLTrx);
                taxStructureDataHandler.DeleteTaxStructure(taxStructureDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TaxStructureDTO TaxStructureDTO
        {
            get
            {
                return taxStructureDTO;
            }
        }

        /// <summary>
        /// Validates the TaxStructureDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (string.IsNullOrWhiteSpace(taxStructureDTO.StructureName))
            {
                log.Error("Enter Tax Structure Name ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2607, MessageContainerList.GetMessage(executionContext, "Tax Structure"));
                throw new ValidationException(errorMessage);
            }
            if (string.IsNullOrWhiteSpace(taxStructureDTO.Percentage.ToString()) || (System.Text.RegularExpressions.Regex.IsMatch(taxStructureDTO.Percentage.ToString(), @"^[a-zA-Z]+$")))
            {
                log.Error("Enter Tax Structure Percentage ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Tax Structure Percentage"));
                throw new ValidationException(errorMessage);
            }
            TaxStructureDataHandler taxStructureDataHandler = new TaxStructureDataHandler(sqlTransaction);
            List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>> searchParameters = new List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>>();
            searchParameters.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.IS_ACTIVE, "1"));
            List<TaxStructureDTO> taxStructureDTOList = taxStructureDataHandler.GetTaxStructureList(searchParameters);
            if (taxStructureDTO != null && taxStructureDTO.TaxStructureId > -1 && taxStructureDTO.IsActive == false)
            {
                if (taxStructureDTOList != null && taxStructureDTOList.Any(x=>x.ParentStructureId == taxStructureDTO.TaxStructureId))
                {
                    log.Error("An active tax structure exists with this as parent tax structure. ");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4752);
                    throw new ValidationException(errorMessage);
                }
            }
            if(!CheckCyclicReference(taxStructureDTO.ParentStructureId, taxStructureDTO.TaxStructureId, taxStructureDTOList))
            {
                log.Debug("Cyclic tax structure hierarchy is not allowed.");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4826);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        public bool CheckCyclicReference(int parentId, int childId, List<TaxStructureDTO> taxStructureDTOList)
        {
            log.LogMethodEntry();
            if (taxStructureDTOList != null && taxStructureDTOList.Any())
            {
                if(parentId == -1 || childId == -1)
                {
                    return true;
                }
                TaxStructureDTO taxStructureDTO = taxStructureDTOList.Where(x => x.TaxStructureId == parentId).FirstOrDefault();
                if (taxStructureDTO.ParentStructureId == childId)
                {
                    return false;
                }
                else if (taxStructureDTO.ParentStructureId > -1)
                {
                    return CheckCyclicReference(taxStructureDTO.ParentStructureId, childId, taxStructureDTOList);
                }
                else
                {
                    return true;
                }
            }
            log.LogMethodExit();
            return true;
        }
    }

    /// <summary>
    /// Manages the list of TaxStructure
    /// </summary>
    public class TaxStructureList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TaxStructureDTO> taxStructureList = new List<TaxStructureDTO>();
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of TaxStructureList class
        /// </summary>
        /// <param name="executionContext"></param>
        public TaxStructureList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of TaxStructureList class
        /// </summary>
        /// <param name="executionContext"></param>
        public TaxStructureList(ExecutionContext executionContext,
                                    List<TaxStructureDTO> taxStructureList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.taxStructureList = taxStructureList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TaxStructure list
        /// </summary>
        public List<TaxStructureDTO> GetTaxStructureList(List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            TaxStructureDataHandler taxStructureHandler = new TaxStructureDataHandler(sqlTransaction);
            List<TaxStructureDTO> taxStructureDTOList = taxStructureHandler.GetTaxStructureList(searchParameters);
            log.LogMethodExit(taxStructureDTOList);
            return taxStructureDTOList;
        }

        /// <summary>
        /// Gets the TaxStructureDTO List for taxIdList
        /// </summary>
        /// <param name="taxIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of DSignageLookupValuesDTO</returns>
        public List<TaxStructureDTO> GetTaxStructureDTOList(List<int> taxIdList, bool activeRecords = true, int currentPage = 0, int pageSize = 10,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(taxIdList, activeRecords);
            TaxStructureDataHandler taxStructureHandler = new TaxStructureDataHandler(sqlTransaction);
            this.taxStructureList = taxStructureHandler.GetTaxStructureDTOList(taxIdList, activeRecords, currentPage, pageSize);
            log.LogMethodExit(taxStructureList);
            return taxStructureList;
        }

        /// <summary>
        /// Saves Tax Structure List
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            if (taxStructureList == null ||
               taxStructureList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < taxStructureList.Count; i++)
            {
                var taxStructureDTO = taxStructureList[i];
                if (taxStructureDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TaxStructureBL taxStructureBL = new TaxStructureBL(executionContext, taxStructureDTO);
                    taxStructureBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving taxStructureDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("taxStructureDTO", taxStructureDTO);
                    throw;
                }
            }
        }
    }
}
