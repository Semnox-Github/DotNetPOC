/********************************************************************************************
* Project Name - User
* Description  - Business Logic for a Leave. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70        20-Nov-2019   Indrajeet Kumar                Created
*2.90        20-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
*2.90        02-Sep-2020   Girish Kundar          Modified : Added missing validation for Leave start and end date
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business Logic for LeaveBL
    /// </summary>
    public class LeaveBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LeaveDTO leaveDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor of LeaveBL
        /// </summary>
        /// <param name="executionContext"></param>
        private LeaveBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LeaveBL object using the LeaveDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="leaveDTO">leaveDTO</param>
        public LeaveBL(ExecutionContext executionContext, LeaveDTO leaveDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveDTO);
            this.leaveDTO = leaveDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the leave id as the parameter
        /// Would fetch the leave object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="leaveId">id of Leave Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LeaveBL(ExecutionContext executionContext, int leaveId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveId, sqlTransaction);
            LeaveDataHandler leaveDataHandler = new LeaveDataHandler(sqlTransaction);
            this.leaveDTO = leaveDataHandler.GetLeaveDTO(leaveId);
            if (leaveDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "LeaveDTO", leaveId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the LeaveBL
        /// Checks if the LeaveBL id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LeaveDataHandler leaveDataHandler = new LeaveDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (leaveDTO.LeaveId < 0)
            {
                leaveDTO = leaveDataHandler.Insert(leaveDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                leaveDTO.AcceptChanges();
            }
            else
            {
                if (leaveDTO.IsChanged)
                {
                    if (leaveDTO.Type == "Leave")
                    {
                        LeaveDTO tempmleaveDTO = leaveDataHandler.GetLeaveDTO(leaveDTO.LeaveId);
                        leaveDTO.Type = tempmleaveDTO.Type;
                    }
                    leaveDataHandler.Update(leaveDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    leaveDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            LeaveDataHandler leaveDataHandler = new LeaveDataHandler(sqlTransaction);
            if (leaveDTO.LeaveId < 0)
            {
                bool result = leaveDataHandler.LeaveStatusCheck(leaveDTO.UserId, leaveDTO.StartDate, leaveDTO.EndDate);
                if (result == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 715));
                }
                decimal Leavebalance = leaveDataHandler.GetBalanceLeaves(leaveDTO.UserId, leaveDTO.LeaveTypeId);
                if(Leavebalance <= 0 && leaveDTO.LeaveStatus != "APPROVED")
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 740));
                }
                double previousLeaves = leaveDataHandler.prvAppliedLeaves(leaveDTO.UserId);
                if (Convert.ToDouble(Leavebalance) < Convert.ToDouble(leaveDTO.LeaveDays) + Convert.ToDouble(previousLeaves) && leaveDTO.LeaveStatus != "APPROVED")
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 740));
                }
                if (leaveDTO.StartDate != null && leaveDTO.EndDate != null)
                {
                    DateTime startDate = leaveDTO.StartDate;
                    DateTime endDate = Convert.ToDateTime(leaveDTO.EndDate);
                    if (startDate > endDate)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 724));
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the LeaveDTO based on LeaveId
        /// </summary>
        internal void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                LeaveDataHandler leaveDataHandler = new LeaveDataHandler(sqlTransaction);
                leaveDataHandler.Delete(leaveDTO.LeaveId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Deleting LeaveDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Gets LeaveDTO Object
        /// </summary>
        public LeaveDTO GetLeaveDTO
        {
            get { return leaveDTO; }
        }
    }

    /// <summary>
    /// Manages the list of LeaveBL
    /// </summary>
    public class LeaveListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LeaveDTO> leaveDTOList = new List<LeaveDTO>();
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LeaveListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="leaveDTOList">leaveDTOList</param>
        public LeaveListBL(ExecutionContext executionContext, List<LeaveDTO> leaveDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveDTOList);
            this.leaveDTOList = leaveDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Leave list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LeaveDTO> GetLeaveDTOList(List<KeyValuePair<LeaveDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LeaveDataHandler leaveDataHandler = new LeaveDataHandler(sqlTransaction);
            List<LeaveDTO> leaveDTOList = leaveDataHandler.GetLeaveDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(leaveDTOList);
            return leaveDTOList;
        }

        /// <summary>
        /// Saves the Leave list
        /// </summary>
        public List<LeaveDTO> SaveUpdateLeave(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<LeaveDTO> leaveDTOLists = new List<LeaveDTO>();
            if (leaveDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (LeaveDTO leaveDTO in leaveDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            LeaveBL leaveBL = new LeaveBL(executionContext, leaveDTO);
                            leaveBL.Save(sqlTransaction);
                            leaveDTOLists.Add(leaveBL.GetLeaveDTO);
                            parafaitDBTrx.EndTransaction();

                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();
                }
            }
            return leaveDTOLists;
        }

        /// <summary>
        /// The Below method will populate the History based on the UserId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<LeaveDTO> PopulateHistoryGrid(int userId)
        {
            log.LogMethodEntry(userId);
            LeaveDataHandler leaveDataHandler = new LeaveDataHandler();
            List<LeaveDTO> leaveDTOList = leaveDataHandler.PopulateHistoryGrid(userId);
            log.LogMethodExit();
            return leaveDTOList;
        }

        /// <summary>
        /// The Below method will populate the Inbox based on the mgrId.
        /// </summary>
        /// <param name="mgrId"></param>
        /// <returns></returns>
        public List<LeaveDTO> PopulateInbox(int mgrId)
        {
            log.LogMethodEntry(mgrId);
            LeaveDataHandler leaveDataHandler = new LeaveDataHandler();
            List<LeaveDTO> leaveDTOList = leaveDataHandler.PopulateInbox(mgrId);
            log.LogMethodExit();
            return leaveDTOList;
        }

        /// <summary>
        /// Will return the List of Leaves based on userId 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<LeaveTypeBalanceDTO> LoadLeaveBalances(int userId)
        {
            log.LogMethodEntry(userId);
            LeaveDataHandler leaveDataHandler = new LeaveDataHandler();
            List<LeaveTypeBalanceDTO> leaveTypeBalanceDTO = leaveDataHandler.LoadLeaveBalances(userId);
            log.LogMethodExit(leaveTypeBalanceDTO);
            return leaveTypeBalanceDTO;
        }

        /// <summary>
        /// Will generate the Leave for the Employee
        /// </summary>
        /// <param name="cycleId"></param>
        /// <returns></returns>
        public List<LeaveDTO> GenerateLeave(int cycleId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cycleId);
            LeaveDataHandler leaveDataHandler = new LeaveDataHandler(sqlTransaction);
            List<LeaveDTO> leaveDTOList = leaveDataHandler.Generate(cycleId, executionContext.GetUserId());
            log.LogMethodExit();
            return leaveDTOList;
        }

        /// <summary>
        /// Delete the Leave
        /// <summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (leaveDTOList != null && leaveDTOList.Any())
            {
                foreach (LeaveDTO leaveDTO in leaveDTOList)
                {
                    if (leaveDTO.IsChanged && leaveDTO.LeaveId > -1)
                    {
                        try
                        {
                            LeaveBL leaveBL = new LeaveBL(executionContext, leaveDTO);
                            leaveBL.Delete(sqlTransaction);
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
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
