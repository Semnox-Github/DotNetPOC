/********************************************************************************************
 * Project Name - POSPrinterOverrideOptions BL
 * Description  - BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        09-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement
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
    public class POSPrinterOverrideOptionsBL
    {
        private POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private POSPrinterOverrideOptionsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates POSPrinterOverrideOptionsBL object using the POSPrinterOverrideOptionsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="pOSPrinterOverrideOptionsDTO">pOSPrinterOverrideOptionsDTO DTOobject</param>
        public POSPrinterOverrideOptionsBL(ExecutionContext executionContext, POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, pOSPrinterOverrideOptionsDTO);
            this.pOSPrinterOverrideOptionsDTO = pOSPrinterOverrideOptionsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the POSPrinterOverrideOptions  id as the parameter
        /// To fetch the POSPrinterOverrideOptions object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id - POSPrinterOverrideOptions </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public POSPrinterOverrideOptionsBL(ExecutionContext executionContext, int pOSPrinterOverrideOptionsId,
                                           SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, pOSPrinterOverrideOptionsId, sqlTransaction);
            POSPrinterOverrideOptionsDataHandler pOSPrinterOverrideOptionsDatahandler = new POSPrinterOverrideOptionsDataHandler(sqlTransaction);
            pOSPrinterOverrideOptionsDTO = pOSPrinterOverrideOptionsDatahandler.GetPOSPrinterOverrideOptionsDTO(pOSPrinterOverrideOptionsId);
            if (pOSPrinterOverrideOptionsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "POSPrinterOverrideOptions", pOSPrinterOverrideOptionsId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the POSPrinterOverrideOptions
        /// POSPrinterOverrideOptions will be inserted if POSPrinterOverrideOptionId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            POSPrinterOverrideOptionsDataHandler pOSPrinterOverrideOptionsDatahandler = new POSPrinterOverrideOptionsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                log.LogMethodExit(null, "Thowing Validation Exception" + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation Failed.", validationErrors);
            }
            if (pOSPrinterOverrideOptionsDTO.POSPrinterOverrideOptionId < 0)
            {
                pOSPrinterOverrideOptionsDTO = pOSPrinterOverrideOptionsDatahandler.Insert(pOSPrinterOverrideOptionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                pOSPrinterOverrideOptionsDTO.AcceptChanges();
            }
            else
            {
                if (pOSPrinterOverrideOptionsDTO.IsChanged)
                {
                    pOSPrinterOverrideOptionsDTO = pOSPrinterOverrideOptionsDatahandler.Update(pOSPrinterOverrideOptionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    pOSPrinterOverrideOptionsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the POSPrinterOverrideOptionsDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;

            List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>(POSPrinterOverrideOptionsDTO.SearchByParameters.OPTION_NAME, pOSPrinterOverrideOptionsDTO.OptionName));
            searchParameters.Add(new KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>(POSPrinterOverrideOptionsDTO.SearchByParameters.IS_ACTIVE, "1"));
            POSPrinterOverrideOptionsListBL pOSPrinterOverrideOptionsListBL = new POSPrinterOverrideOptionsListBL(executionContext);
            List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList = pOSPrinterOverrideOptionsListBL.GetPOSPrinterOverrideOptionsDTOList(searchParameters, sqlTransaction);
            if (pOSPrinterOverrideOptionsDTO != null)
            {
                if (string.IsNullOrEmpty(pOSPrinterOverrideOptionsDTO.OptionName))
                {
                    validationErrorList.Add(new ValidationError("POSPrinterOverrideOptions", "OptionName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Option Name"))));
                }
                if (string.IsNullOrEmpty(pOSPrinterOverrideOptionsDTO.OptionDescription))
                {
                    validationErrorList.Add(new ValidationError("POSPrinterOverrideOptions", "OptionDescription", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Option Description"))));
                }
                if (!string.IsNullOrEmpty(pOSPrinterOverrideOptionsDTO.OptionName) &&
                    pOSPrinterOverrideOptionsDTOList!=null &&
                    pOSPrinterOverrideOptionsDTOList.Any() &&
                    pOSPrinterOverrideOptionsDTOList.Exists(ppoo => ppoo.POSPrinterOverrideOptionId != this.pOSPrinterOverrideOptionsDTO.POSPrinterOverrideOptionId
                                                                       && ppoo.OptionName == pOSPrinterOverrideOptionsDTO.OptionName))
                {
                    validationErrorList.Add(new ValidationError("POSPrinterOverrideOptions", "OptionName", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, pOSPrinterOverrideOptionsDTO.OptionName))));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the POSPrinterOverrideOptionsDTO
        /// </summary>
        public POSPrinterOverrideOptionsDTO POSPrinterOverrideOptionsDTO
        {
            get
            {
                return pOSPrinterOverrideOptionsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of POSPrinterOverrideOptions
    /// </summary>
    public class POSPrinterOverrideOptionsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList = new List<POSPrinterOverrideOptionsDTO>();

        /// <summary>
        /// Parameterized constructor of POSPrinterOverrideOptionsListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public POSPrinterOverrideOptionsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="pOSPrinterOverrideOptionsDTOList">POSPrinterOverrideOptions DTO List as parameter </param>
        public POSPrinterOverrideOptionsListBL(ExecutionContext executionContext,
                                               List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, pOSPrinterOverrideOptionsDTOList);
            this.pOSPrinterOverrideOptionsDTOList = pOSPrinterOverrideOptionsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the POSPrinterOverrideOptions DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of POSPrinterOverrideOptionsDTO </returns>
        public List<POSPrinterOverrideOptionsDTO> GetPOSPrinterOverrideOptionsDTOList(List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            POSPrinterOverrideOptionsDataHandler pOSPrinterOverrideOptionsDataHandler = new POSPrinterOverrideOptionsDataHandler(sqlTransaction);
            List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList = pOSPrinterOverrideOptionsDataHandler.GetPOSPrinterOverrideOptionsDTOList(searchParameters);
            log.LogMethodExit(pOSPrinterOverrideOptionsDTOList);
            return pOSPrinterOverrideOptionsDTOList;
        }

        /// <summary>
        /// Gets POSPrinterOverrideOptionsDTO List based on the Id list of POSPrinterOverrideOptions
        /// </summary>
        /// <param name="POSPrinterOverrideOptionsIdList">POSPrinterOverrideOptionsIdList has list of Ids</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the POSPrinterOverrideOptionsDTO List</returns>
        public List<POSPrinterOverrideOptionsDTO> GetPOSPrinterOverrideOptionsDTOList(List<int> POSPrinterOverrideOptionsIdList, SqlTransaction sqlTransaction = null, bool activeRecords = true)
        {
            log.LogMethodEntry(POSPrinterOverrideOptionsIdList, sqlTransaction);
            POSPrinterOverrideOptionsDataHandler pOSPrinterOverrideOptionsDatahandler = new POSPrinterOverrideOptionsDataHandler(sqlTransaction);
            List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList = pOSPrinterOverrideOptionsDatahandler.GetPOSPrinterOverrideOptionsDTOList(POSPrinterOverrideOptionsIdList, activeRecords);
            log.LogMethodExit(pOSPrinterOverrideOptionsDTOList);
            return pOSPrinterOverrideOptionsDTOList;
        }

        /// <summary>
        /// Saves the  list of POSPrinterOverrideOptions DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (pOSPrinterOverrideOptionsDTOList == null ||
                pOSPrinterOverrideOptionsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < pOSPrinterOverrideOptionsDTOList.Count; i++)
            {
                POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO = pOSPrinterOverrideOptionsDTOList[i];
                if (pOSPrinterOverrideOptionsDTO.IsChanged || pOSPrinterOverrideOptionsDTO.POSPrinterOverrideOptionId < 0)
                {
                    try
                    {
                        POSPrinterOverrideOptionsBL pOSPrinterOverrideOptionsBL = new POSPrinterOverrideOptionsBL(executionContext, pOSPrinterOverrideOptionsDTO);
                        pOSPrinterOverrideOptionsBL.Save(sqlTransaction);
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
                        log.Error("Error occurred while saving POSPrinterOverrideOptionsDTO.", ex);
                        log.LogVariableState("Record Index ", i);
                        log.LogVariableState("POSPrinterOverrideOptionsDTO", pOSPrinterOverrideOptionsDTO);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
        }
    }

}
