/********************************************************************************************
 * Project Name -CardCore / Account
 * Description  - Business logic file for  DailyCardBalance
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks          
 *********************************************************************************************
 *2.70        16-June-2019   Girish Kundar           Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CardCore
{
    public class DailyCardBalanceBL
    {
        DailyCardBalanceDTO dailyCardBalanceDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of DailyCardBalanceBL class
        /// </summary>
        public DailyCardBalanceBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            dailyCardBalanceDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the dailyCardBalance id as the parameter
        /// Would fetch the dailyCardBalance object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public DailyCardBalanceBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            DailyCardBalanceDataHandler dailyCardBalanceDataHandler = new DailyCardBalanceDataHandler(sqlTransaction);
            dailyCardBalanceDTO = dailyCardBalanceDataHandler.GetDailyCardBalanceDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DailyCardBalanceBL object using the DailyCardBalanceDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="dailyCardBalanceDTO">DailyCardBalanceDTO object</param>
        public DailyCardBalanceBL(ExecutionContext executionContext, DailyCardBalanceDTO dailyCardBalanceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, dailyCardBalanceDTO);
            this.dailyCardBalanceDTO = dailyCardBalanceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the DailyCardBalance
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null) // Modified for the new Structure of Data Handler.
        {
            log.LogMethodEntry(sqlTransaction);
            DailyCardBalanceDataHandler dailyCardBalanceDataHandler = new DailyCardBalanceDataHandler(sqlTransaction);
            if (dailyCardBalanceDTO.DailyCardBalanceId < 0)
            {
                dailyCardBalanceDTO = dailyCardBalanceDataHandler.InsertDailyCardBalance(dailyCardBalanceDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                dailyCardBalanceDTO.AcceptChanges();
            }
            else
            {
                if (dailyCardBalanceDTO.IsChanged)
                {
                    dailyCardBalanceDTO = dailyCardBalanceDataHandler.UpdateDailyCardBalance(dailyCardBalanceDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    dailyCardBalanceDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DailyCardBalanceDTO DailyCardBalanceDTO
        {
            get
            {
                return dailyCardBalanceDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of DailyCardBalance
    /// </summary>
    public class DailyCardBalanceListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public DailyCardBalanceListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the DailyCardBalance list
        /// </summary>
        public List<DailyCardBalanceDTO> GetDailyCardBalanceDTOList(List<KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DailyCardBalanceDataHandler dailyCardBalanceDataHandler = new DailyCardBalanceDataHandler(sqlTransaction);
            List<DailyCardBalanceDTO> returnValue = dailyCardBalanceDataHandler.GetDailyCardBalanceDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
