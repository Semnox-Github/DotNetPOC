/********************************************************************************************
 * Project Name - TrxPOSPrinterOverrideRules BL
 * Description  - BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
  *2.110.0     11-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement
 ********************************************************************************************/

using System;
using System.Linq;
using Semnox.Parafait.POS;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using System.Collections.Generic;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    public class TrxPOSPrinterOverrideRulesBL
    {
        private TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO;
        private readonly ExecutionContext executionContext;
        private readonly SqlTransaction sqlTransaction;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TrxPOSPrinterOverrideRulesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TrxPOSPrinterOverrideRulesBL object using the trxPOSPrinterOverrideRulesDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="trxPOSPrinterOverrideRulesDTO">trxPOSPrinterOverrideRulesDTO object</param>
        public TrxPOSPrinterOverrideRulesBL(ExecutionContext executionContext, TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxPOSPrinterOverrideRulesDTO);
            this.trxPOSPrinterOverrideRulesDTO = trxPOSPrinterOverrideRulesDTO;
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the TrxPOSPrinterOverrideRules id as the parameter
        /// To fetch the TrxPOSPrinterOverrideRules object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id - trxPOSPrinterOverrideRules </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public TrxPOSPrinterOverrideRulesBL(ExecutionContext executionContext, int trxPOSPrinterOverrideRulesId,
                                           SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxPOSPrinterOverrideRulesId, sqlTransaction);
            TrxPOSPrinterOverrideRulesDataHandler trxPOSPrinterOverrideRulesDataHandler = new TrxPOSPrinterOverrideRulesDataHandler(sqlTransaction);
            trxPOSPrinterOverrideRulesDTO = trxPOSPrinterOverrideRulesDataHandler.GetTrxPOSPrinterOverrideRulesDTO(trxPOSPrinterOverrideRulesId);
            if (trxPOSPrinterOverrideRulesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TrxPOSPrinterOverrideRules", trxPOSPrinterOverrideRulesId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TrxPOSPrinterOverrideRules
        /// TrxPOSPrinterOverrideRules will be inserted if TrxPOSPrinterOverrideRulesId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            TrxPOSPrinterOverrideRulesDataHandler trxPOSPrinterOverrideRulesDataHandler = new TrxPOSPrinterOverrideRulesDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                log.LogMethodExit(null, "Thowing Validation Exception" + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation Failed.", validationErrors);
            }
            if (trxPOSPrinterOverrideRulesDTO.TrxPOSPrinterOverrideRuleId < 0)
            {
                trxPOSPrinterOverrideRulesDTO = trxPOSPrinterOverrideRulesDataHandler.Insert(trxPOSPrinterOverrideRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxPOSPrinterOverrideRulesDTO.AcceptChanges();
            }
            else
            {
                if (trxPOSPrinterOverrideRulesDTO.IsChanged)
                {
                    trxPOSPrinterOverrideRulesDTO = trxPOSPrinterOverrideRulesDataHandler.Update(trxPOSPrinterOverrideRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    trxPOSPrinterOverrideRulesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the trxPOSPrinterOverrideRulesDTO 
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
        /// Gets the trxPOSPrinterOverrideRulesDTO
        /// </summary>
        public TrxPOSPrinterOverrideRulesDTO TrxPOSPrinterOverrideRulesDTO
        {
            get
            {
                return trxPOSPrinterOverrideRulesDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of POSPrinterOverrideRules
    /// </summary>
    public class TrxPOSPrinterOverrideRulesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly SqlTransaction sqlTransaction;
        private List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList = new List<TrxPOSPrinterOverrideRulesDTO>();

        /// <summary>
        /// Parameterized constructor of trxPOSPrinterOverrideRulesListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public TrxPOSPrinterOverrideRulesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="trxPOSPrinterOverrideRulesDTOList">trxPOSPrinterOverrideRules DTO List as parameter </param>
        public TrxPOSPrinterOverrideRulesListBL(ExecutionContext executionContext, List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxPOSPrinterOverrideRulesDTOList);
            this.trxPOSPrinterOverrideRulesDTOList = trxPOSPrinterOverrideRulesDTOList;
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the TrxPOSPrinterOverrideRules DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of trxPOSPrinterOverrideRulesDTO </returns>
        public List<TrxPOSPrinterOverrideRulesDTO> GetTrxPOSPrinterOverrideRulesDTOList(List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TrxPOSPrinterOverrideRulesDataHandler trxPOSPrinterOverrideRulesDataHandler = new TrxPOSPrinterOverrideRulesDataHandler(sqlTransaction);
            List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList = trxPOSPrinterOverrideRulesDataHandler.GetTrxPOSPrinterOverrideRulesDTOList(searchParameters);
            log.LogMethodExit(trxPOSPrinterOverrideRulesDTOList);
            return trxPOSPrinterOverrideRulesDTOList;
        }

        /// <summary>
        /// Gets trxPOSPrinterOverrideRulesDTO List based on the Id list of TrxPOSPrinterOverrideRules
        /// </summary>
        /// <param name="TrxPOSPrinterOverrideRulesIdList">TrxPOSPrinterOverrideRulesIdList has list of Ids</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the trxPOSPrinterOverrideRulesDTO List</returns>
        public List<TrxPOSPrinterOverrideRulesDTO> GetTrxPOSPrinterOverrideRulesDTOList(List<int> trxPOSPrinterOverrideRulesIdList, SqlTransaction sqlTransaction, bool activeRecords = true)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRulesIdList, sqlTransaction);
            TrxPOSPrinterOverrideRulesDataHandler trxPOSPrinterOverrideRulesDataHandler = new TrxPOSPrinterOverrideRulesDataHandler(sqlTransaction);
            List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList = trxPOSPrinterOverrideRulesDataHandler.GetTrxPOSPrinterOverrideRulesDTOList(trxPOSPrinterOverrideRulesIdList, activeRecords);
            log.LogMethodExit(trxPOSPrinterOverrideRulesDTOList);
            return trxPOSPrinterOverrideRulesDTOList;
        }

        /// <summary>
        /// Gets the Template ID
        /// </summary>
        /// <returns></returns>
        public int GetTemplateId(List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOParamList, POSPrinterDTO posPrinterDTO)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRulesDTOParamList, posPrinterDTO);
            int templateId = -1;
            if (trxPOSPrinterOverrideRulesDTOParamList != null && trxPOSPrinterOverrideRulesDTOParamList.Any() && posPrinterDTO != null && posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter)
            {
                TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO = trxPOSPrinterOverrideRulesDTOParamList.Find(ruleDTO => ruleDTO.OptionItemCode == POSPrinterOverrideOptionItemCode.RECEIPT
                                                                                                                                     && ruleDTO.IsActive);
                if (trxPOSPrinterOverrideRulesDTO != null)
                {
                    ReceiptPrintTemplateHeaderListBL receiptPrintTemplateHeaderBLList = new ReceiptPrintTemplateHeaderListBL(executionContext);
                    List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>(ReceiptPrintTemplateHeaderDTO.SearchByParameters.GUID, trxPOSPrinterOverrideRulesDTO.ItemSourceColumnGuid));
                    List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOList = receiptPrintTemplateHeaderBLList.GetReceiptPrintTemplateHeaderDTOList(searchByParameters, false);
                    if (receiptPrintTemplateHeaderDTOList != null && receiptPrintTemplateHeaderDTOList.Any())
                    {
                        templateId = receiptPrintTemplateHeaderDTOList[0].TemplateId;
                    }
                }
            }
            log.LogMethodExit(templateId);
            return templateId;
        }
        /// <summary>
        /// GetSequenceId
        /// </summary>
        /// <returns></returns>
        public string GetSequenceId(TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            string sequenceId = "-1";
            if (trxPOSPrinterOverrideRulesDTO != null)
            {
                //POSPrinterOverrideRulesListBL pOSPrinterOverrideRulesBLList = new POSPrinterOverrideRulesListBL(executionContext);
                //List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>>();
                //searchByParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID, trxPOSPrinterOverrideRulesDTO.POSPrinterId.ToString()));
                //searchByParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID, trxPOSPrinterOverrideRulesDTO.POSPrinterOverrideOptionId.ToString()));
                //searchByParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.OPTION_ITEM_CODE, Convert.ToString(POSPrinterOverrideOptionItemCode.SEQUENCE)));
                //List<POSPrinterOverrideRulesDTO> POSPrinterOverrideRulesDTOList = pOSPrinterOverrideRulesBLList.GetPOSPrinterOverrideRulesDTOList(searchByParameters);
                //if (POSPrinterOverrideRulesDTOList != null && POSPrinterOverrideRulesDTOList.Any())
                if (trxPOSPrinterOverrideRulesDTO != null)
                {
                    SequencesListBL sequencesListBL = new SequencesListBL(executionContext);
                    List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchBySeqParameters = new List<KeyValuePair<SequencesDTO.SearchByParameters, string>>();
                    searchBySeqParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.GUID, trxPOSPrinterOverrideRulesDTO.ItemSourceColumnGuid));
                    List<SequencesDTO> sequencesDTOList = sequencesListBL.GetAllSequencesList(searchBySeqParameters);
                    if (sequencesDTOList != null && sequencesDTOList.Any())
                    {
                        sequenceId = Convert.ToString(sequencesDTOList[0].SequenceId);
                    }
                }
            }
            log.LogMethodExit(sequenceId);
            return sequenceId;
        }
        /// <summary>
        /// Saves the  list of trxPOSPrinterOverrideRules DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (trxPOSPrinterOverrideRulesDTOList == null ||
                trxPOSPrinterOverrideRulesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < trxPOSPrinterOverrideRulesDTOList.Count; i++)
            {
                TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO = trxPOSPrinterOverrideRulesDTOList[i];
                if (trxPOSPrinterOverrideRulesDTO.IsChanged || trxPOSPrinterOverrideRulesDTO.TrxPOSPrinterOverrideRuleId < 0)
                {
                    try
                    {
                        TrxPOSPrinterOverrideRulesBL TrxPOSPrinterOverrideRulesBL = new TrxPOSPrinterOverrideRulesBL(executionContext, trxPOSPrinterOverrideRulesDTO, sqlTransaction);
                        TrxPOSPrinterOverrideRulesBL.Save(sqlTransaction);
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
                        log.Error("Error occurred while saving trxPOSPrinterOverrideRulesDTO.", ex);
                        log.LogVariableState("Record Index ", i);
                        log.LogVariableState("trxPOSPrinterOverrideRulesDTO", trxPOSPrinterOverrideRulesDTO);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
        }
    }

}
