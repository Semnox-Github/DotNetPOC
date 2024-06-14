/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  LoyaltyBonusRewardCriteria
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
    /// Business logic for LoyaltyBonusRewardCriteria class.
    /// </summary>
    public class LoyaltyBonusRewardCriteriaBL
    {
        private LoyaltyBonusRewardCriteriaDTO loyaltyBonusRewardCriteriaDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LoyaltyBonusRewardCriteriaBL class
        /// </summary>
        private LoyaltyBonusRewardCriteriaBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LoyaltyBonusRewardCriteriaBL object using the LoyaltyBonusRewardCriteriaDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="loyaltyBonusRewardCriteriaDTO">LoyaltyBonusRewardCriteria DTO object</param>
        public LoyaltyBonusRewardCriteriaBL(ExecutionContext executionContext, LoyaltyBonusRewardCriteriaDTO loyaltyBonusRewardCriteriaDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyBonusRewardCriteriaDTO);
            this.loyaltyBonusRewardCriteriaDTO = loyaltyBonusRewardCriteriaDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the LoyaltyBonusRewardCriteria id as the parameter
        /// Would fetch the LoyaltyBonusRewardCriteria object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        /// <param name="id"> LoyaltyBonusRewardCriteria Id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LoyaltyBonusRewardCriteriaBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LoyaltyBonusRewardCriteriaDataHandler loyaltyBonusRewardCriteriaDataHandler = new LoyaltyBonusRewardCriteriaDataHandler(sqlTransaction);
            loyaltyBonusRewardCriteriaDTO = loyaltyBonusRewardCriteriaDataHandler.GetLoyaltyBonusRewardCriteriaDTO(id);
            if (loyaltyBonusRewardCriteriaDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " LoyaltyBonusRewardCriteria", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is called from the Parent class  LoyaltyBonusAttribute Save() method.
        /// Saves the LoyaltyBonusRewardCriteri aDTO. This method id called from the Parent class Save() method .
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyBonusRewardCriteriaDTO.IsChanged == false &&
                loyaltyBonusRewardCriteriaDTO.Id > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            LoyaltyBonusRewardCriteriaDataHandler loyaltyBonusRewardCriteriaDataHandler = new LoyaltyBonusRewardCriteriaDataHandler(sqlTransaction);
            if (loyaltyBonusRewardCriteriaDTO.ActiveFlag == true)
            {
                if (loyaltyBonusRewardCriteriaDTO.Id < 0)
                {
                    log.LogVariableState("LoyaltyBonusRewardCriteriaDTO", loyaltyBonusRewardCriteriaDTO);
                    loyaltyBonusRewardCriteriaDTO = loyaltyBonusRewardCriteriaDataHandler.Insert(loyaltyBonusRewardCriteriaDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyBonusRewardCriteriaDTO.AcceptChanges();
                }
                else if (loyaltyBonusRewardCriteriaDTO.IsChanged)
                {
                    log.LogVariableState("LoyaltyBonusRewardCriteriaDTO", loyaltyBonusRewardCriteriaDTO);
                    loyaltyBonusRewardCriteriaDTO = loyaltyBonusRewardCriteriaDataHandler.Update(loyaltyBonusRewardCriteriaDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyBonusRewardCriteriaDTO.AcceptChanges();
                }
            }
            else  
            {
                if (loyaltyBonusRewardCriteriaDTO.Id >= 0)
                    loyaltyBonusRewardCriteriaDataHandler.Delete(loyaltyBonusRewardCriteriaDTO);
                loyaltyBonusRewardCriteriaDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the LoyaltyBonusRewardCriteriaDTO
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
        public LoyaltyBonusRewardCriteriaDTO LoyaltyBonusRewardCriteriaDTO
        {
            get
            {
                return loyaltyBonusRewardCriteriaDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of LoyaltyBonusRewardCriteria
    /// </summary>
    public class LoyaltyBonusRewardCriteriaListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<LoyaltyBonusRewardCriteriaDTO> loyaltyBonusRewardCriteriaDTOList = new List<LoyaltyBonusRewardCriteriaDTO>(); // must
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LoyaltyBonusRewardCriteriaListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="loyaltyBonusRewardCriteriaDTOList">LoyaltyBonusRewardCriteriaDTO List passed as a parameter</param>
        public LoyaltyBonusRewardCriteriaListBL(ExecutionContext executionContext,
                                                List<LoyaltyBonusRewardCriteriaDTO> loyaltyBonusRewardCriteriaDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyBonusRewardCriteriaDTOList);
            this.loyaltyBonusRewardCriteriaDTOList = loyaltyBonusRewardCriteriaDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the LoyaltyBonusRewardCriteria List based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the LoyaltyBonusRewardCriteriaDTO List</returns>
        public List<LoyaltyBonusRewardCriteriaDTO> GetLoyaltyBonusRewardCriteriaDTOList(List<KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LoyaltyBonusRewardCriteriaDataHandler loyaltyBonusRewardCriteriaDataHandler = new LoyaltyBonusRewardCriteriaDataHandler(sqlTransaction);
            List<LoyaltyBonusRewardCriteriaDTO> loyaltyBonusRewardCriteriaDTOList = loyaltyBonusRewardCriteriaDataHandler.GetLoyaltyBonusRewardCriteriaDTO(searchParameters);
            log.LogMethodExit(loyaltyBonusRewardCriteriaDTOList);
            return loyaltyBonusRewardCriteriaDTOList;
        }

        /// <summary>
        /// Saves the LoyaltyBonusRewardCriteria DTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyBonusRewardCriteriaDTOList == null ||
               loyaltyBonusRewardCriteriaDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < loyaltyBonusRewardCriteriaDTOList.Count; i++)
            {
                var loyaltyBonusRewardCriteriaDTO = loyaltyBonusRewardCriteriaDTOList[i];
                if (loyaltyBonusRewardCriteriaDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    LoyaltyBonusRewardCriteriaBL loyaltyBonusRewardCriteriaBL = new LoyaltyBonusRewardCriteriaBL(executionContext, loyaltyBonusRewardCriteriaDTO);
                    loyaltyBonusRewardCriteriaBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving LoyaltyBonusRewardCriteriaDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("LoyaltyBonusRewardCriteriaDTO", loyaltyBonusRewardCriteriaDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}