/*/********************************************************************************************
 * Project Name - LegacyCardCreditPlusConsumption BL
 * Description  - BL for Legacy Card Credit Plus Consumption
 *
 **************
 ** Version Log
 **************
 *Version     Date Modified      By          Remarks
 *********************************************************************************************
 *2.130.4     21-Feb-2022        Dakshakh    Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Parafait_POS
{
    public class LegacyCardCreditPlusConsumptionBL
    {
        private readonly ExecutionContext executionContext;
        private LegacyCardCreditPlusConsumptionDTO legacyCardCreditPlusConsumptionDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LegacyCardCreditPlusConsumptionBL class
        /// </summary>
        private LegacyCardCreditPlusConsumptionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the accountCreditPlusConsumption id as the parameter
        /// Would fetch the LegacyCardCreditPlusConsumption object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public LegacyCardCreditPlusConsumptionBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LegacyCardCreditPlusConsumptionDataHandler legacyCardCreditPlusConsumptionDataHandler = new LegacyCardCreditPlusConsumptionDataHandler(sqlTransaction);
            legacyCardCreditPlusConsumptionDTO = legacyCardCreditPlusConsumptionDataHandler.GetLegacyCardCreditPlusConsumptionDTO(id);
            if (legacyCardCreditPlusConsumptionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "LegacyCardCreditPlusConsumption", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LegacyCardCreditPlusConsumptionBL object using the LegacyCardCreditPlusConsumptionDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="LegacyCardCreditPlusConsumptionDTO">LegacyCardCreditPlusConsumptionDTO object</param>
        public LegacyCardCreditPlusConsumptionBL(ExecutionContext executionContext, LegacyCardCreditPlusConsumptionDTO legacyCardCreditPlusConsumptionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, legacyCardCreditPlusConsumptionDTO);
            this.legacyCardCreditPlusConsumptionDTO = legacyCardCreditPlusConsumptionDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the LegacyCardCreditPlusConsumption
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            LegacyCardCreditPlusConsumptionDataHandler legacyCardCreditPlusConsumptionDataHandler = new LegacyCardCreditPlusConsumptionDataHandler(sqlTransaction);
            if (legacyCardCreditPlusConsumptionDTO.IsChanged)
            {
                if (legacyCardCreditPlusConsumptionDTO.IsActive)
                {
                    if (legacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusConsumptionId < 0)
                    {
                        legacyCardCreditPlusConsumptionDTO = legacyCardCreditPlusConsumptionDataHandler.InsertLegacyCardCreditPlusConsumption(legacyCardCreditPlusConsumptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        legacyCardCreditPlusConsumptionDTO.AcceptChanges();
                    }
                    else
                    {
                        if (legacyCardCreditPlusConsumptionDTO.IsChanged)
                        {
                            legacyCardCreditPlusConsumptionDTO = legacyCardCreditPlusConsumptionDataHandler.UpdateLegacyCardCreditPlusConsumption(legacyCardCreditPlusConsumptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                            legacyCardCreditPlusConsumptionDTO.AcceptChanges();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LegacyCardCreditPlusConsumptionDTO LegacyCardCreditPlusConsumptionDTO
        {
            get
            {
                return legacyCardCreditPlusConsumptionDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of LegacyCardCreditPlusConsumption
    /// </summary>
    public class LegacyCreditPlusConsumptionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public LegacyCreditPlusConsumptionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountCreditPlusConsumption list
        /// </summary>
        public List<LegacyCardCreditPlusConsumptionDTO> GetLegacyCardCreditPlusConsumptionDTOList(List<KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            LegacyCardCreditPlusConsumptionDataHandler legacyCardCreditPlusConsumptionDataHandler = new LegacyCardCreditPlusConsumptionDataHandler(sqlTransaction);
            List<LegacyCardCreditPlusConsumptionDTO> returnValue = legacyCardCreditPlusConsumptionDataHandler.GetLegacyCardCreditPlusConsumptionDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
