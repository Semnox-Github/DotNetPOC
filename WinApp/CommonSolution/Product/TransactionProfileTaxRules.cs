/********************************************************************************************
 * Project Name - TransactionProfileTaxRules
 * Description  - Bussiness logic of Transaction Profile Tax Rules
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        14-Mar-2016   Jagan Mohana    Created 
 *2.70        05-Apr-2019   Mushahid Faizan Added LogMethodEntry & LogMethodExit
 *            25-Jul-2019   Mushahid Faizan Added Delete Method.
 *2.110.00    08-Dec-2020   Prajwal S       Updated Three Tier           
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class TransactionProfileTaxRules
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TransactionProfileTaxRules(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="TransactionProfileTaxRulesDTO"></param>
        public TransactionProfileTaxRules(ExecutionContext executionContext, TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionProfileTaxRulesDTO);
            this.transactionProfileTaxRulesDTO = transactionProfileTaxRulesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TransactionProfileTaxRules  id as the parameter
        /// Would fetch the TransactionProfileTaxRules object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="Id">id -PromotionRule </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public TransactionProfileTaxRules(ExecutionContext executionContext, int id,
                                            SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TransactionProfileTaxRulesDataHandler TransactionProfileTaxRulesDataHandler = new TransactionProfileTaxRulesDataHandler(sqlTransaction);
            transactionProfileTaxRulesDTO = TransactionProfileTaxRulesDataHandler.GetTransactionProfileTaxRulesId(id);
            if (transactionProfileTaxRulesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TransactionProfileTaxRulesDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(transactionProfileTaxRulesDTO);
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (transactionProfileTaxRulesDTO == null)
            {
                //Validation to be implemented.
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionProfileTaxRulesDTO TransactionProfileTaxRulesDTO
        {
            get
            {
                return transactionProfileTaxRulesDTO;
            }
        }
    

    /// <summary>
    /// Saves the Tax Rules
    /// Checks if the id is not less than or equal to 0
    /// If it is less than or equal to 0, then inserts
    /// else updates
    /// </summary>
    public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            TransactionProfileTaxRulesDataHandler transactionProfileTaxRulesDataHandler = new TransactionProfileTaxRulesDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (transactionProfileTaxRulesDTO.IsActive)
            {
                if (transactionProfileTaxRulesDTO.Id < 0)
                {
                    transactionProfileTaxRulesDTO = transactionProfileTaxRulesDataHandler.Insert(transactionProfileTaxRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    transactionProfileTaxRulesDTO.AcceptChanges();
                }
                else
                {
                    if (transactionProfileTaxRulesDTO.IsChanged)
                    {
                        transactionProfileTaxRulesDTO = transactionProfileTaxRulesDataHandler.Update(transactionProfileTaxRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        transactionProfileTaxRulesDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if (transactionProfileTaxRulesDTO.Id >= 0)
                {
                    transactionProfileTaxRulesDataHandler.Delete(transactionProfileTaxRulesDTO.Id);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Delete the TransactionProfileTaxRules based on id
        /// </summary>
        /// <param name="id"></param>        
        public void Delete(int id)
        {
            log.LogMethodEntry(id);
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrx.BeginTransaction();
                    TransactionProfileTaxRulesDataHandler transactionProfileTaxRulesDataHandler = new TransactionProfileTaxRulesDataHandler(parafaitDBTrx.SQLTrx);
                    transactionProfileTaxRulesDataHandler.Delete(id);
                    parafaitDBTrx.EndTransaction();
                    log.LogMethodExit();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    throw;
                }
            }
        }
    }
    public class TransactionProfileTaxRulesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TransactionProfileTaxRulesDTO> transactionProfileTaxRulesDTOList = new List<TransactionProfileTaxRulesDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public TransactionProfileTaxRulesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="TransactionProfileTaxRulesDTOList">TransactionProfileTaxRules DTO List as parameter </param>
        public TransactionProfileTaxRulesList(ExecutionContext executionContext, List<TransactionProfileTaxRulesDTO> transactionProfileTaxRulesDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionProfileTaxRulesDTOList);
            this.transactionProfileTaxRulesDTOList = transactionProfileTaxRulesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Transaction Profile Tax Rules list
        /// </summary>
        public List<TransactionProfileTaxRulesDTO> GetAllTransactionProfileTaxRules(List<KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionProfileTaxRulesDataHandler transactionProfileTaxRulesDataHandler = new TransactionProfileTaxRulesDataHandler(sqlTransaction);
            List<TransactionProfileTaxRulesDTO> transactionProfileTaxRulesDTOList = transactionProfileTaxRulesDataHandler.GetTransactionProfileTaxRules(searchParameters, sqlTransaction);
            log.LogMethodExit(transactionProfileTaxRulesDTOList);
            return transactionProfileTaxRulesDTOList;
        }


        /// <summary>
        /// Saves the  list of TransactionProfileTaxRulesDTOList DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (transactionProfileTaxRulesDTOList == null ||
                transactionProfileTaxRulesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < transactionProfileTaxRulesDTOList.Count; i++)
            {
                TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO = transactionProfileTaxRulesDTOList[i];
                if (transactionProfileTaxRulesDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TransactionProfileTaxRules transactionProfileTaxRules = new TransactionProfileTaxRules(executionContext, transactionProfileTaxRulesDTO);
                    transactionProfileTaxRules.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving TransactionProfileTaxRulesDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TransactionProfileTaxRulesDTO", transactionProfileTaxRulesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}