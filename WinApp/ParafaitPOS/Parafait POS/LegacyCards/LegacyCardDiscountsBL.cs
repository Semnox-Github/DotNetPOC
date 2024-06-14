/*/********************************************************************************************
 * Project Name - LegacyCardDiscountsBL
 * Description  - BL for LegacyCardDiscountsBL
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     18-Feb-2022    Dakshakh                Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Parafait_POS
{
    /// <summary>
    /// Business logic for AccountDiscount class.
    /// </summary>
    public class LegacyCardDiscountsBL
    {
        LegacyCardDiscountsDTO legacyCardDiscountsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LegacyCardDiscountsBL class
        /// </summary>
        private LegacyCardDiscountsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the LegacyCardDiscount id as the parameter
        /// Would fetch the LegacyCardDiscount object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public LegacyCardDiscountsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LegacyCardDiscountsDataHandler legacyCardDiscountsDataHandler = new LegacyCardDiscountsDataHandler(sqlTransaction);
            legacyCardDiscountsDTO = legacyCardDiscountsDataHandler.GetLegacyCardDiscountsDTO(id);
            if (legacyCardDiscountsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "legacyCardDiscountsDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LegacyCardDiscountsBL object using the LegacyCardDiscountsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="legacyCardDiscountsDTO">LegacyCardDiscountsDTO object</param>
        public LegacyCardDiscountsBL(ExecutionContext executionContext, LegacyCardDiscountsDTO legacyCardDiscountsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, legacyCardDiscountsDTO);
            this.legacyCardDiscountsDTO = legacyCardDiscountsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the LegacyCardDiscountsDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            LegacyCardDiscountsDataHandler legacyCardDiscountsDataHandler = new LegacyCardDiscountsDataHandler(sqlTransaction);
            if (legacyCardDiscountsDTO.IsChanged)
            {
                if (legacyCardDiscountsDTO.LegacyCardDiscountId < 0)
                {
                    legacyCardDiscountsDTO = legacyCardDiscountsDataHandler.InsertLegacyCardDiscount(legacyCardDiscountsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    legacyCardDiscountsDTO.AcceptChanges();
                }
                else
                {
                    if (legacyCardDiscountsDTO.IsChanged)
                    {
                        legacyCardDiscountsDTO = legacyCardDiscountsDataHandler.UpdateAccountDiscount(legacyCardDiscountsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        legacyCardDiscountsDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LegacyCardDiscountsDTO LegacyCardDiscountsDTO
        {
            get
            {
                return legacyCardDiscountsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of AccountDiscount
    /// </summary>
    public class LegacyCardDiscountListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public LegacyCardDiscountListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the LegacyCardDiscount list
        /// </summary>
        public List<LegacyCardDiscountsDTO> GetAccountDiscountDTOList(List<KeyValuePair<LegacyCardDiscountsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            LegacyCardDiscountsDataHandler legacyCardDiscountsDataHandler = new LegacyCardDiscountsDataHandler(sqlTransaction);
            List<LegacyCardDiscountsDTO> returnValue = legacyCardDiscountsDataHandler.GetLegacyCardDiscountsDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
