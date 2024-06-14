/********************************************************************************************
 * Project Name - TransactionProfile BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        1-Dec-2017      Lakshminarayana     Created 
 *2.60        17-Mar-2019   Jagan Mohana Rao      Modified - Added SaveUpdateTransactionProfilesList(),GetTransactionProfileAndTaxRulesDTOList & constructor.
              05-Apr-2019   Mushahid Faizan       Modified- Save(),SaveUpdateTransactionProfilesList(),GetTransactionProfileAndTaxRulesDTOList()
 *                                                Added LogMethodEntry & LogMethodExit,Removed unused namespaces.
 *2.110.0    07-Dec-2020   Prajwal S       Updated Three Tier                                                
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for TransactionProfile class.
    /// </summary>
    public class TransactionProfileBL
    {
        TransactionProfileDTO transactionProfileDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public TransactionProfileBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the transactionProfile id as the parameter
        /// Would fetch the transactionProfile object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public TransactionProfileBL(ExecutionContext executionContext, int id, bool loadChildRecords= false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, executionContext,sqlTransaction);
            TransactionProfileDataHandler transactionProfileDataHandler = new TransactionProfileDataHandler(sqlTransaction);
            transactionProfileDTO = transactionProfileDataHandler.GetTransactionProfileDTO(id);
            if (transactionProfileDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TransactionProfileDTO", id);   //added to thow exception
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if(loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(transactionProfileDTO);
        }


        /// <summary>
        /// Builds the child records for Tax object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)    //added build
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            TransactionProfileTaxRulesList transactionProfileTaxRulesList = new TransactionProfileTaxRulesList(executionContext);
            List<KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>> searchByTransitionsProfileParams = new List<KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>>();
            searchByTransitionsProfileParams.Add(new KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>(TransactionProfileTaxRulesDTO.SearchByParameters.TRX_PROFILE_ID, transactionProfileDTO.TransactionProfileId.ToString()));
            searchByTransitionsProfileParams.Add(new KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>(TransactionProfileTaxRulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchByTransitionsProfileParams.Add(new KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>(TransactionProfileTaxRulesDTO.SearchByParameters.ISACTIVE, "1"));
            }
            List<TransactionProfileTaxRulesDTO> transactionProfileTaxRulesDTOList = transactionProfileTaxRulesList.GetAllTransactionProfileTaxRules(searchByTransitionsProfileParams, sqlTransaction);
           transactionProfileDTO.TransactionProfileTaxRulesDTOList = new List<TransactionProfileTaxRulesDTO>(transactionProfileTaxRulesDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates TransactionProfileBL object using the TransactionProfileDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transactionProfileDTO"></param>
        public TransactionProfileBL(ExecutionContext executionContext, TransactionProfileDTO transactionProfileDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionProfileDTO);
            this.transactionProfileDTO = transactionProfileDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TransactionProfile
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            TransactionProfileDataHandler transactionProfileDataHandler = new TransactionProfileDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (transactionProfileDTO.TransactionProfileId < 0)
            {
                transactionProfileDTO = transactionProfileDataHandler.Insert(transactionProfileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                transactionProfileDTO.AcceptChanges();
            }
            else
            {
                if (transactionProfileDTO.IsChanged)
                {
                    transactionProfileDataHandler.Update(transactionProfileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    transactionProfileDTO.AcceptChanges();
                }
            }
            if (transactionProfileDTO.TransactionProfileTaxRulesDTOList != null || transactionProfileDTO.TransactionProfileTaxRulesDTOList.Count != 0)
            {
                foreach (TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO in transactionProfileDTO.TransactionProfileTaxRulesDTOList)
                {
                    if (transactionProfileTaxRulesDTO.IsChanged)
                    {
                        transactionProfileTaxRulesDTO.TrxProfileId = transactionProfileDTO.TransactionProfileId;
                        TransactionProfileTaxRules transactionProfileTaxRules = new TransactionProfileTaxRules(executionContext, transactionProfileTaxRulesDTO);
                        transactionProfileTaxRules.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete TransactionProfile
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            TransactionProfileDataHandler transactionProfileDataHandler = new TransactionProfileDataHandler(sqlTransaction);
            if (transactionProfileDTO.TransactionProfileId >= 0)
            {
                if (transactionProfileDTO.TransactionProfileTaxRulesDTOList != null && transactionProfileDTO.TransactionProfileTaxRulesDTOList.Count > 0)
                {
                    foreach (TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO in transactionProfileDTO.TransactionProfileTaxRulesDTOList)
                    {
                        transactionProfileTaxRulesDTO.TrxProfileId = transactionProfileDTO.TransactionProfileId;
                        TransactionProfileTaxRules transactionProfileTaxRules = new TransactionProfileTaxRules(executionContext, transactionProfileTaxRulesDTO);
                        transactionProfileTaxRules.Delete(transactionProfileTaxRulesDTO.Id);
                    }
                    transactionProfileDataHandler.Delete(transactionProfileDTO.TransactionProfileId);
                }
                else
                {

                    transactionProfileDataHandler.Delete(transactionProfileDTO.TransactionProfileId);
                }
                
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the RecipeEstimationDetailsDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (transactionProfileDTO == null)
            {
                //Validation to be implemented.
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

    

    /// <summary>
    /// Gets the DTO
    /// </summary>
    public TransactionProfileDTO TransactionProfileDTO
        {
            get
            {
                return transactionProfileDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of TransactionProfile
    /// </summary>
    /// 

    public class TransactionProfileListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TransactionProfileDTO> transactionProfileDTOsList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransactionProfileListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterzed Constructor
        /// </summary>
        public TransactionProfileListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="transactionProfileDTOsList"></param>
        /// <param name="executionContext"></param>
        public TransactionProfileListBL(ExecutionContext executionContext, List<TransactionProfileDTO> transactionProfileDTOsList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionProfileDTOsList);
            this.transactionProfileDTOsList = transactionProfileDTOsList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the TransactionProfile list
        /// </summary>
        public List<TransactionProfileDTO> GetTransactionProfileDTOList(List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            TransactionProfileDataHandler transactionProfileDataHandler = new TransactionProfileDataHandler(sqlTransaction);
            List<TransactionProfileDTO> transactionProfileDTOsList = transactionProfileDataHandler.GetTransactionProfileDTOList(searchParameters);
            if (transactionProfileDTOsList != null && transactionProfileDTOsList.Count != 0 && loadChildRecords)
            {
                foreach (TransactionProfileDTO transactionProfileDTO in transactionProfileDTOsList)
                {
                    List<KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>> searchTaxRulesParameters = new List<KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>>();
                    searchTaxRulesParameters.Add(new KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>(TransactionProfileTaxRulesDTO.SearchByParameters.TRX_PROFILE_ID, transactionProfileDTO.TransactionProfileId.ToString()));
                    // Modification to get Child records on  05-Apr-2019 by Mushahid Faizan.
                    TransactionProfileTaxRulesList transactionProfileTaxRulesList = new TransactionProfileTaxRulesList(executionContext);
                    transactionProfileDTO.TransactionProfileTaxRulesDTOList = transactionProfileTaxRulesList.GetAllTransactionProfileTaxRules(searchTaxRulesParameters);
                }
            }
            log.LogMethodExit(transactionProfileDTOsList);
            return transactionProfileDTOsList;
        }

        public DateTime? GetTransactionProfileModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            TransactionProfileDataHandler transactionProfileDataHandler = new TransactionProfileDataHandler();
            DateTime? result = transactionProfileDataHandler.GetTransactionProfileModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// This method should be used to Save and Update the Transaction Profiles details for Web Management Studio.
        /// </summary>
        public void SaveUpdateTransactionProfilesList()
        {
            log.LogMethodEntry();

            if (transactionProfileDTOsList != null && transactionProfileDTOsList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (TransactionProfileDTO transactionProfileDTO in transactionProfileDTOsList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TransactionProfileBL transactionProfileBL = new TransactionProfileBL(executionContext, transactionProfileDTO);
                            transactionProfileBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new ForeignKeyException(MessageContainerList.GetMessage(executionContext, 1869));
                        }
                    }
                }
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// Delete the TransactionProfiles records
        /// </summary>
        public void DeleteTransactionProfilesList()
        {
            log.LogMethodEntry();
            try
            {
                log.LogMethodEntry();
                if (transactionProfileDTOsList != null && transactionProfileDTOsList.Count > 0)
                {
                    foreach (TransactionProfileDTO transactionProfileDTO in transactionProfileDTOsList)
                    {
                        TransactionProfileBL transactionProfileBL = new TransactionProfileBL(executionContext, transactionProfileDTO);
                        transactionProfileBL.Delete();
                    }
                }
                log.LogMethodExit();
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
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
