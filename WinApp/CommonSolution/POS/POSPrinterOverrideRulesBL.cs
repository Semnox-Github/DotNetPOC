/********************************************************************************************
 * Project Name - POSPrinterOverrideRules BL
 * Description  - BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        11-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement
 ********************************************************************************************/

using System;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.POS
{
    public class POSPrinterOverrideRulesBL
    {
        private POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private POSPrinterOverrideRulesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates POSPrinterOverrideRulesBL object using the POSPrinterOverrideRulesDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="pOSPrinterOverrideRulesDTO">pOSPrinterOverrideRulesDTO object</param>
        public POSPrinterOverrideRulesBL(ExecutionContext executionContext, POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, pOSPrinterOverrideRulesDTO);
            this.pOSPrinterOverrideRulesDTO = pOSPrinterOverrideRulesDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the POSPrinterOverrideRules  id as the parameter
        /// To fetch the POSPrinterOverrideRules object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="POSPrinterOverrideRulesId">id - POSPrinterOverrideRules </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public POSPrinterOverrideRulesBL(ExecutionContext executionContext, int POSPrinterOverrideRulesId,
                                           SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, POSPrinterOverrideRulesId, sqlTransaction);
            POSPrinterOverrideRulesDataHandler pOSPrinterOverrideRulesDataHandler = new POSPrinterOverrideRulesDataHandler(sqlTransaction);
            pOSPrinterOverrideRulesDTO = pOSPrinterOverrideRulesDataHandler.GetPOSPrinterOverrideRulesDTO(POSPrinterOverrideRulesId);
            if (pOSPrinterOverrideRulesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "POSPrinterOverrideRules", POSPrinterOverrideRulesId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the POSPrinterOverrideRules
        /// POSPrinterOverrideRules will be inserted if POSPrinterOverrideRulesId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            POSPrinterOverrideRulesDataHandler POSPrinterOverrideRulesDataHandler = new POSPrinterOverrideRulesDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                log.LogMethodExit(null, "Thowing Validation Exception" + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation Failed.", validationErrors);
            }
            if (pOSPrinterOverrideRulesDTO.POSPrinterOverrideRuleId < 0)
            {
                pOSPrinterOverrideRulesDTO = POSPrinterOverrideRulesDataHandler.Insert(pOSPrinterOverrideRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                pOSPrinterOverrideRulesDTO.AcceptChanges();
            }
            else
            {
                if (pOSPrinterOverrideRulesDTO.IsChanged)
                {
                    pOSPrinterOverrideRulesDTO = POSPrinterOverrideRulesDataHandler.Update(pOSPrinterOverrideRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    pOSPrinterOverrideRulesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the pOSPrinterOverrideRulesDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            //Need to be added
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the pOSPrinterOverrideRulesDTO
        /// </summary>
        public POSPrinterOverrideRulesDTO POSPrinterOverrideRulesDTO
        {
            get
            {
                return pOSPrinterOverrideRulesDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of POSPrinterOverrideRules
    /// </summary>
    public class POSPrinterOverrideRulesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList = new List<POSPrinterOverrideRulesDTO>();

        /// <summary>
        /// Parameterized constructor of POSPrinterOverrideRulesListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public POSPrinterOverrideRulesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="pOSPrinterOverrideRulesDTOList">POSPrinterOverrideRules DTO List as parameter </param>
        public POSPrinterOverrideRulesListBL(ExecutionContext executionContext,
                                               List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, pOSPrinterOverrideRulesDTOList);
            this.executionContext = executionContext;
            this.pOSPrinterOverrideRulesDTOList = pOSPrinterOverrideRulesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the POSPrinterOverrideRules DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of pOSPrinterOverrideRulesDTO </returns>
        public List<POSPrinterOverrideRulesDTO> GetPOSPrinterOverrideRulesDTOList(List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            POSPrinterOverrideRulesDataHandler POSPrinterOverrideRulesDataHandler = new POSPrinterOverrideRulesDataHandler(sqlTransaction);
            List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList = POSPrinterOverrideRulesDataHandler.GetPOSPrinterOverrideRulesDTOList(searchParameters);
            log.LogMethodExit(pOSPrinterOverrideRulesDTOList);
            return pOSPrinterOverrideRulesDTOList;
        }

        /// <summary>
        /// Gets pOSPrinterOverrideRulesDTO List based on the Id list of POSPrinterOverrideRules
        /// </summary>
        /// <param name="POSPrinterOverrideRulesIdList">POSPrinterOverrideRulesIdList has list of Ids</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the pOSPrinterOverrideRulesDTO List</returns>
        public List<POSPrinterOverrideRulesDTO> GetPOSPrinterOverrideRulesDTOList(List<int> POSPrinterOverrideRulesIdList, SqlTransaction sqlTransaction, bool activeRecords = true)
        {
            log.LogMethodEntry(POSPrinterOverrideRulesIdList, sqlTransaction);
            POSPrinterOverrideRulesDataHandler POSPrinterOverrideRulesDataHandler = new POSPrinterOverrideRulesDataHandler(sqlTransaction);
            List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList = POSPrinterOverrideRulesDataHandler.GetPOSPrinterOverrideRulesDTOList(POSPrinterOverrideRulesIdList, activeRecords);
            log.LogMethodExit(pOSPrinterOverrideRulesDTOList);
            return pOSPrinterOverrideRulesDTOList;
        }

        /// <summary>
        /// Saves the  list of POSPrinterOverrideRules DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (pOSPrinterOverrideRulesDTOList == null ||
                pOSPrinterOverrideRulesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < pOSPrinterOverrideRulesDTOList.Count; i++)
            {
                POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO = pOSPrinterOverrideRulesDTOList[i];
                if (pOSPrinterOverrideRulesDTO.IsChanged || pOSPrinterOverrideRulesDTO.POSPrinterOverrideRuleId < 0)
                {
                    try
                    {
                        POSPrinterOverrideRulesBL POSPrinterOverrideRulesBL = new POSPrinterOverrideRulesBL(executionContext, pOSPrinterOverrideRulesDTO);
                        POSPrinterOverrideRulesBL.Save(sqlTransaction);
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
                        log.Error("Error occurred while saving pOSPrinterOverrideRulesDTO.", ex);
                        log.LogVariableState("Record Index ", i);
                        log.LogVariableState("pOSPrinterOverrideRulesDTO", pOSPrinterOverrideRulesDTO);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
        }
    }

}
