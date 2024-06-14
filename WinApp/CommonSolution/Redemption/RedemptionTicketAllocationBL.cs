/********************************************************************************************
 * Project Name - RedemptionTicketAllocation BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 ********************************************************************************************* 
 *2.30        07-Aug-2018      Guru S A            Created
 *2.70        19-Jul-2019      Deeksha             Modifications as per three tier standard.
 *2.70.3      30-Jan-2020      Archana             Modified as part of E-Ticket max enhancement
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Business logic for RedemptionTicketAllocation class.
    /// </summary>
    public class RedemptionTicketAllocationBL
    {
        private RedemptionTicketAllocationDTO redemptionTicketAllocationDTO;
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal ExecutionContext machineUserContext;

        /// <summary>
        /// Default constructor of RedemptionTicketAllocationBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public RedemptionTicketAllocationBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            machineUserContext = executionContext;
            redemptionTicketAllocationDTO = new RedemptionTicketAllocationDTO();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the redemptionTicketAllocation id as the parameter
        /// Would fetch the redemptionTicketAllocation object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RedemptionTicketAllocationBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            RedemptionTicketAllocationDataHandler redemptionTicketAllocationDataHandler = new RedemptionTicketAllocationDataHandler(sqlTransaction);
            redemptionTicketAllocationDTO = redemptionTicketAllocationDataHandler.GetRedemptionTicketAllocationDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RedemptionTicketAllocationBL object using RedemptionTicketAllocationDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="redemptionTicketAllocationDTO">RedemptionTicketAllocationDTO object</param>
        public RedemptionTicketAllocationBL(ExecutionContext executionContext, RedemptionTicketAllocationDTO redemptionTicketAllocationDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, redemptionTicketAllocationDTO);
            this.redemptionTicketAllocationDTO = redemptionTicketAllocationDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RedemptionTicketAllocation
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// <param name="SqlTransaction">SqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            RedemptionTicketAllocationDataHandler redemptionTicketAllocationDataHandler = new RedemptionTicketAllocationDataHandler(sqlTrx);
            if (redemptionTicketAllocationDTO.Id < 0)
            {
                redemptionTicketAllocationDTO = redemptionTicketAllocationDataHandler.InsertRedemptionTicketAllocation(redemptionTicketAllocationDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                redemptionTicketAllocationDTO.AcceptChanges();
            }
            else
            {
                if (redemptionTicketAllocationDTO.IsChanged)
                {
                    redemptionTicketAllocationDTO = redemptionTicketAllocationDataHandler.UpdateRedemptionTicketAallocation(redemptionTicketAllocationDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    redemptionTicketAllocationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RedemptionTicketAllocationDTO RedemptionTicketAllocationDTO
        {
            get
            {
                return redemptionTicketAllocationDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of RedemptionTicketAllocation
    /// </summary>
    public class RedemptionTicketAllocationListBL
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal ExecutionContext machineUserContext;
        private List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public RedemptionTicketAllocationListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.machineUserContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="redemptionTicketAllocationDTOList"></param>
        public RedemptionTicketAllocationListBL(ExecutionContext executionContext, List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.redemptionTicketAllocationDTOList = redemptionTicketAllocationDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the RedemptionTicketAllocation list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>redemptionTicketAllocationDTOList</returns>
        public List<RedemptionTicketAllocationDTO> GetRedemptionTicketAllocationDTOList(List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            RedemptionTicketAllocationDataHandler redemptionTicketAllocationDataHandler = new RedemptionTicketAllocationDataHandler(sqlTransaction);
            List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList = redemptionTicketAllocationDataHandler.GetRedemptionTicketAllocationDTOList(searchParameters, machineUserContext.GetSiteId());
            log.LogMethodExit(redemptionTicketAllocationDTOList);
            return redemptionTicketAllocationDTOList;
        }

        public bool CanAddManualTicketForTheDay(string loginId, int manualTickets, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loginId, manualTickets, sqlTransaction);
            bool canAdd = false;
            int remainingManualTicketCount = 0;
            int manualTicketPerDayLimitValue = 0;
            try
            {
                manualTicketPerDayLimitValue = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault<int>(machineUserContext, "PER_USER_DAILY_LIMIT_FOR_ADDING_MANUAL_TICKETS", 0));
            }
            catch (Exception ex)
            {
                log.Error("PER_USER_DAILY_LIMIT_FOR_ADDING_MANUAL_TICKETS value is not setup", ex);
                manualTicketPerDayLimitValue = 0;
            }
            if (manualTicketPerDayLimitValue != 0)
            {
                remainingManualTicketCount = GetRemainingAddManualTicketLimitForTheDay(loginId, sqlTransaction);
                if (remainingManualTicketCount - manualTickets >= 0)
                {
                    canAdd = true;
                }
            }
            else
            {
                canAdd = true;
            }
            log.LogMethodExit(canAdd);
            return canAdd;
        }

        public int GetRemainingAddManualTicketLimitForTheDay(string loginId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loginId, sqlTransaction);
            int remainingManualTicketCount = 0;
            int usedManualTicketCount = 0;
            int manualTicketPerDayLimitValue =0;
            try
            {
                manualTicketPerDayLimitValue = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault<int>(machineUserContext, "PER_USER_DAILY_LIMIT_FOR_ADDING_MANUAL_TICKETS", 0));
            }
            catch (Exception ex)
            {
                log.Error("PER_USER_DAILY_LIMIT_FOR_ADDING_MANUAL_TICKETS value is not setup", ex);
                manualTicketPerDayLimitValue = 0;
            }
            if (manualTicketPerDayLimitValue != 0)
            {
                List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.MANUAL_TICKETS_PER_DAY_BY_LOGIN_ID, loginId),
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                };
                RedemptionTicketAllocationDataHandler redemptionTicketAllocationDataHandler = new RedemptionTicketAllocationDataHandler(sqlTransaction);
                List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList = redemptionTicketAllocationDataHandler.GetRedemptionTicketAllocationDTOList(searchParameters, machineUserContext.GetSiteId());
                if (redemptionTicketAllocationDTOList != null && redemptionTicketAllocationDTOList.Count > 0)
                {
                    redemptionTicketAllocationDTOList = redemptionTicketAllocationDTOList.Where(t => t.ManualTickets > 0).ToList();
                    usedManualTicketCount = Convert.ToInt32(redemptionTicketAllocationDTOList.Sum(x => x.ManualTickets));
                }
                log.LogVariableState("usedManualTicketCount", usedManualTicketCount);

                remainingManualTicketCount = manualTicketPerDayLimitValue - usedManualTicketCount;
            }
            else
            {
                remainingManualTicketCount = 999999999; //Setting max value as manualTicketPerDayLimitValue is zero (not applicable)
            }
            log.LogMethodExit(remainingManualTicketCount);
            return remainingManualTicketCount;
        }

        public bool CanReduceManualTicketForTheDay(string loginId, int manualTickets, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loginId, manualTickets, sqlTransaction);
            bool canReduce = false;
            int remainingManualTicketCount = 0;
            int manualTicketPerDayLimitValue = 0;
            try
            {
                manualTicketPerDayLimitValue = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault<int>(machineUserContext, "PER_USER_DAILY_LIMIT_FOR_DEDUCTING_MANUAL_TICKETS", 0));
            }
            catch (Exception ex)
            {
                log.Error("PER_USER_DAILY_LIMIT_FOR_DEDUCTING_MANUAL_TICKETS value is not setup", ex);
                manualTicketPerDayLimitValue = 0;
            }
            if (manualTicketPerDayLimitValue != 0)
            {
                remainingManualTicketCount = GetRemainingReduceManualTicketLimitForTheDay(loginId, sqlTransaction);
                if (remainingManualTicketCount + manualTickets >= 0)
                {
                    canReduce = true;
                }
            }
            else
            {
                canReduce = true;
            }
            log.LogMethodExit(canReduce);
            return canReduce;
        }

        public int GetRemainingReduceManualTicketLimitForTheDay(string loginId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loginId, sqlTransaction);
            int remainingManualTicketCount = 0;
            int usedManualTicketCount = 0;
            int manualTicketPerDayLimitValue = 0;
            try
            {
                manualTicketPerDayLimitValue = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault<int>(machineUserContext, "PER_USER_DAILY_LIMIT_FOR_DEDUCTING_MANUAL_TICKETS", 0));
            }
            catch (Exception ex)
            {
                log.Error("PER_USER_DAILY_LIMIT_FOR_DEDUCTING_MANUAL_TICKETS value is not setup", ex);
                manualTicketPerDayLimitValue = 0;
            }
            if (manualTicketPerDayLimitValue != 0)
            {
                List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.MANUAL_TICKETS_PER_DAY_BY_LOGIN_ID, loginId),
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                };
                RedemptionTicketAllocationDataHandler redemptionTicketAllocationDataHandler = new RedemptionTicketAllocationDataHandler(sqlTransaction);
                List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList = redemptionTicketAllocationDataHandler.GetRedemptionTicketAllocationDTOList(searchParameters, machineUserContext.GetSiteId());
                if (redemptionTicketAllocationDTOList != null && redemptionTicketAllocationDTOList.Count > 0)
                {
                    redemptionTicketAllocationDTOList = redemptionTicketAllocationDTOList.Where(t => t.ManualTickets < 0).ToList();
                    usedManualTicketCount = Convert.ToInt32(redemptionTicketAllocationDTOList.Sum(x => x.ManualTickets));
                }
                log.LogVariableState("usedManualTicketCount", usedManualTicketCount);

                remainingManualTicketCount = manualTicketPerDayLimitValue + usedManualTicketCount;
            }
            else
            {
                remainingManualTicketCount = 999999999; //Setting max value as manualTicketPerDayLimitValue is zero (not applicable)
            }
            log.LogMethodExit(remainingManualTicketCount);
            return remainingManualTicketCount;
        }

        /// <summary>
        /// Validates and saves the redemptionTicketAllocationDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionTicketAllocationDTOList == null ||
                !redemptionTicketAllocationDTOList.Any())
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }

            RedemptionTicketAllocationDataHandler redemptionTicketAllocationDataHandler = new RedemptionTicketAllocationDataHandler(sqlTransaction);
            redemptionTicketAllocationDataHandler.Save(redemptionTicketAllocationDTOList, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
            log.LogMethodExit();
        }
        public void SaveTrxDetails(int redemptionId, List<Tuple<int, int, int>> trxdetails, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(trxdetails, sqlTrx);
            if (trxdetails == null ||
                !trxdetails.Any())
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_ID, redemptionId.ToString()),
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                };
            redemptionTicketAllocationDTOList = GetRedemptionTicketAllocationDTOList(searchParams, sqlTrx);
            if (redemptionTicketAllocationDTOList == null && !redemptionTicketAllocationDTOList.Any())
            {
                log.LogMethodExit(null, "No ticket allocation to save");
                throw new Exception("Can't fetch ticket allocation details.");
            }
            foreach (Tuple<int, int, int> trx in trxdetails)
            {
                if (redemptionTicketAllocationDTOList != null && redemptionTicketAllocationDTOList.Any(x => x.Id == trx.Item1))
                {
                    redemptionTicketAllocationDTOList.Where(x => x.Id == trx.Item1).FirstOrDefault().TrxId = trx.Item2;
                    redemptionTicketAllocationDTOList.Where(x => x.Id == trx.Item1).FirstOrDefault().TrxLineId = trx.Item3;
                }
            }
            Save(sqlTrx);
            log.LogMethodExit();
        }
    }
}
