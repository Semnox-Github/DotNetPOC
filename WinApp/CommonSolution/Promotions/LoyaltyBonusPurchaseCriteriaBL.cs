/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  LoyaltyBonusPurchaseCriteria
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80      24-Jun-2019     Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;


namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Business logic for LoyaltyBonusPurchaseCriteria class.
    /// </summary>
    public class LoyaltyBonusPurchaseCriteriaBL
    {
        private LoyaltyBonusPurchaseCriteriaDTO loyaltyBonusPurchaseCriteriaDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LoyaltyBonusPurchaseCriteriaBL class
        /// </summary>
        private LoyaltyBonusPurchaseCriteriaBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LoyaltyBonusPurchaseCriteriaBL object using the LoyaltyBonusPurchaseCriteriaDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="loyaltyBonusPurchaseCriteriaDTO">LoyaltyBonusPurchaseCriteria DTO object</param>
        public LoyaltyBonusPurchaseCriteriaBL(ExecutionContext executionContext, LoyaltyBonusPurchaseCriteriaDTO loyaltyBonusPurchaseCriteriaDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyBonusPurchaseCriteriaDTO);
            this.loyaltyBonusPurchaseCriteriaDTO = loyaltyBonusPurchaseCriteriaDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the LoyaltyBonusPurchaseCriteria id as the parameter
        /// Would fetch the LoyaltyBonusPurchaseCriteria object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        /// <param name="id"> LoyaltyBonusPurchaseCriteria Id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LoyaltyBonusPurchaseCriteriaBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LoyaltyBonusPurchaseCriteriaDataHandler loyaltyBonusPurchaseCriteriaDataHandler = new LoyaltyBonusPurchaseCriteriaDataHandler(sqlTransaction);
            loyaltyBonusPurchaseCriteriaDTO = loyaltyBonusPurchaseCriteriaDataHandler.GetLoyaltyBonusPurchaseCriteriaDTO(id);
            if (loyaltyBonusPurchaseCriteriaDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " LoyaltyBonusPurchaseCriteria", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is called from the Parent class  LoyaltyBonusAttribute Save() method.
        /// Saves the loyaltyBonusPurchaseCriteria DTO. This method id called from the Parent class Save() method .
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyBonusPurchaseCriteriaDTO.IsChanged == false &&
                loyaltyBonusPurchaseCriteriaDTO.Id > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            LoyaltyBonusPurchaseCriteriaDataHandler loyaltyBonusPurchaseCriteriaDataHandler = new LoyaltyBonusPurchaseCriteriaDataHandler(sqlTransaction);
            if (loyaltyBonusPurchaseCriteriaDTO.ActiveFlag == true)
            {
                if (loyaltyBonusPurchaseCriteriaDTO.Id < 0)
                {
                    log.LogVariableState("LoyaltyBonusPurchaseCriteriaDTO", loyaltyBonusPurchaseCriteriaDTO);
                    loyaltyBonusPurchaseCriteriaDTO = loyaltyBonusPurchaseCriteriaDataHandler.Insert(loyaltyBonusPurchaseCriteriaDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyBonusPurchaseCriteriaDTO.AcceptChanges();
                }
                else if (loyaltyBonusPurchaseCriteriaDTO.IsChanged)
                {
                    log.LogVariableState("LoyaltyBonusPurchaseCriteriaDTO", loyaltyBonusPurchaseCriteriaDTO);
                    loyaltyBonusPurchaseCriteriaDTO = loyaltyBonusPurchaseCriteriaDataHandler.Update(loyaltyBonusPurchaseCriteriaDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyBonusPurchaseCriteriaDTO.AcceptChanges();
                }
            }
            else  // Hard Delete only for the existing files. For new Files only the Soft Delete to be used. 
            {
                if (loyaltyBonusPurchaseCriteriaDTO.Id >= 0)
                    loyaltyBonusPurchaseCriteriaDataHandler.Delete(loyaltyBonusPurchaseCriteriaDTO);
                loyaltyBonusPurchaseCriteriaDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the LoyaltyBonusPurchaseCriteria DTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // Implement validation 
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LoyaltyBonusPurchaseCriteriaDTO LoyaltyBonusPurchaseCriteriaDTO
        {
            get
            {
                return loyaltyBonusPurchaseCriteriaDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of LoyaltyBonusPurchaseCriteria
    /// </summary>
    public class LoyaltyBonusPurchaseCriteriaListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<LoyaltyBonusPurchaseCriteriaDTO> loyaltyBonusPurchaseCriteriaDTOList = new List<LoyaltyBonusPurchaseCriteriaDTO>(); // must
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LoyaltyBonusPurchaseCriteriaListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="loyaltyBonusPurchaseCriteriaDTOList">loyaltyBonusPurchaseCriteriaDTO List passed as a parameter</param>
        public LoyaltyBonusPurchaseCriteriaListBL(ExecutionContext executionContext,
                                                 List<LoyaltyBonusPurchaseCriteriaDTO> loyaltyBonusPurchaseCriteriaDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyBonusPurchaseCriteriaDTOList);
            this.loyaltyBonusPurchaseCriteriaDTOList = loyaltyBonusPurchaseCriteriaDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the LoyaltyBonusPurchaseCriteriaDTO List based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the LoyaltyBonusPurchaseCriteriaDTO List</returns>
        public List<LoyaltyBonusPurchaseCriteriaDTO> GetLoyaltyBonusPurchaseCriteriaDTOList(List<KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LoyaltyBonusPurchaseCriteriaDataHandler loyaltyBonusPurchaseCriteriaDataHandler = new LoyaltyBonusPurchaseCriteriaDataHandler(sqlTransaction);
            List<LoyaltyBonusPurchaseCriteriaDTO> loyaltyBonusPurchaseCriteriaDTOList = loyaltyBonusPurchaseCriteriaDataHandler.GetLoyaltyBonusPurchaseCriteriaDTO(searchParameters);
            log.LogMethodExit(loyaltyBonusPurchaseCriteriaDTOList);
            return loyaltyBonusPurchaseCriteriaDTOList;
        }

        /// <summary>
        /// Saves the LoyaltyBonusPurchaseCriteria DTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyBonusPurchaseCriteriaDTOList == null ||
               loyaltyBonusPurchaseCriteriaDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < loyaltyBonusPurchaseCriteriaDTOList.Count; i++)
            {
                var loyaltyBonusPurchaseCriteriaDTO = loyaltyBonusPurchaseCriteriaDTOList[i];
                if (loyaltyBonusPurchaseCriteriaDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    LoyaltyBonusPurchaseCriteriaBL loyaltyBonusPurchaseCriteriaBL = new LoyaltyBonusPurchaseCriteriaBL(executionContext, loyaltyBonusPurchaseCriteriaDTO);
                    loyaltyBonusPurchaseCriteriaBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving LoyaltyBonusPurchaseCriteriaDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("LoyaltyBonusPurchaseCriteriaDTO", loyaltyBonusPurchaseCriteriaDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
