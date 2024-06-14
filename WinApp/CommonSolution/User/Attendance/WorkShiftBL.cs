/********************************************************************************************
 * Project Name - User
 * Description  - Business logic of WorkShiftBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      27-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class WorkShiftBL
    {
        private WorkShiftDTO workShiftDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private WorkShiftBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="workShiftDTO"></param>
        public WorkShiftBL(ExecutionContext executionContext, WorkShiftDTO workShiftDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, workShiftDTO);
            this.workShiftDTO = workShiftDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the WorkShiftBL id as the parameter
        /// Would fetch the workShiftDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public WorkShiftBL(ExecutionContext executionContext, int id, bool loadChildRecords = false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            WorkShiftDataHandler workShiftDataHandler = new WorkShiftDataHandler(sqlTransaction);
            workShiftDTO = workShiftDataHandler.GetWorkShift(id, sqlTransaction);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Generate adBroadcast list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            WorkShiftScheduleListBL workShiftScheduleListBL = new WorkShiftScheduleListBL(executionContext);
            List<KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>> searchParameters = new List<KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>>();
            searchParameters.Add(new KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>(WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.WORK_SHIFT_ID, workShiftDTO.WorkShiftId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>(WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.IS_ACTIVE, "1"));
            }
            workShiftDTO.WorkShiftScheduleDTOList = workShiftScheduleListBL.GetWorkShiftScheduleDTOList(searchParameters, sqlTransaction);
            WorkShiftUserListBL workShiftUserListBL = new WorkShiftUserListBL(executionContext);
            List<KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>> psearchParameters = new List<KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>>();
            psearchParameters.Add(new KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>(WorkShiftUserDTO.SearchByWorkShiftUserParameters.WORK_SHIFT_ID, workShiftDTO.WorkShiftId.ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>(WorkShiftUserDTO.SearchByWorkShiftUserParameters.IS_ACTIVE, "1"));
            }
            workShiftDTO.WorkShiftUsersDTOList = workShiftUserListBL.GetWorkShiftUserDTOList(psearchParameters, sqlTransaction);
            log.LogMethodExit(workShiftDTO);
        }

        /// <summary>
        /// Saves the WorkShiftBL
        /// WorkShiftBL will be inserted if WorkShiftBL is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            WorkShiftDataHandler workShiftDataHandler = new WorkShiftDataHandler(sqlTransaction);

            if (workShiftDTO.WorkShiftId < 0)
            {
                workShiftDTO = workShiftDataHandler.Insert(workShiftDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                workShiftDTO.AcceptChanges();
            }
            else
            {
                if (workShiftDTO.IsChanged == true)
                {
                    workShiftDTO = workShiftDataHandler.Update(workShiftDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    workShiftDTO.AcceptChanges();
                }
            }
            if (workShiftDTO.WorkShiftScheduleDTOList != null && workShiftDTO.WorkShiftScheduleDTOList.Any())
            {
                foreach (WorkShiftScheduleDTO workShiftScheduleDTO in workShiftDTO.WorkShiftScheduleDTOList)
                {
                    if (workShiftScheduleDTO.IsChanged)
                    {
                        WorkShiftScheduleBL workShiftScheduleBL = new WorkShiftScheduleBL(executionContext, workShiftScheduleDTO);
                        workShiftScheduleBL.Save(sqlTransaction);
                    }
                }
            }
            if (workShiftDTO.WorkShiftUsersDTOList != null && workShiftDTO.WorkShiftUsersDTOList.Any())
            {
                foreach (WorkShiftUserDTO workShiftUserDTO in workShiftDTO.WorkShiftUsersDTOList)
                {
                    if (workShiftUserDTO.IsChanged)
                    {
                        WorkShiftUserBL workShiftUserBL = new WorkShiftUserBL(executionContext, workShiftUserDTO);
                        workShiftUserBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get workShiftDTO Object
        /// </summary>
        public WorkShiftDTO GetWorkShiftDTO
        {
            get { return workShiftDTO; }
        }

    }
    /// <summary>
    /// Manages the list of WorkShiftListBL
    /// </summary>
    public class WorkShiftListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<WorkShiftDTO> workShiftDTOList;
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public WorkShiftListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="workShiftDTOList"></param>
        public WorkShiftListBL(ExecutionContext executionContext, List<WorkShiftDTO> workShiftDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, workShiftDTOList);
            this.workShiftDTOList = workShiftDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the WorkShiftBL list
        /// </summary>
        public List<WorkShiftDTO> GetWorkShiftDTOList(List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>> searchParameters,
                                          bool loadChildRecords = false, bool loadActiveRecords = false,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            WorkShiftDataHandler workShiftDataHandler = new WorkShiftDataHandler(sqlTransaction);
            List<WorkShiftDTO> workShiftDTOList = workShiftDataHandler.GetWorkShiftDTOList(searchParameters, sqlTransaction);
            if (workShiftDTOList != null && workShiftDTOList.Any() && loadChildRecords)
            {
                Build(workShiftDTOList, loadActiveRecords, sqlTransaction);
            }
            log.LogMethodExit(workShiftDTOList);
            return workShiftDTOList;
        }


        private void Build(List<WorkShiftDTO> workShiftDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(workShiftDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, WorkShiftDTO> workShiftIdDictionary = new Dictionary<int, WorkShiftDTO>();
            StringBuilder sb = new StringBuilder(string.Empty);
            string workShiftIdSet;
            for (int i = 0; i < workShiftDTOList.Count; i++)
            {
                if (workShiftDTOList[i].WorkShiftId == -1 ||
                    workShiftIdDictionary.ContainsKey(workShiftDTOList[i].WorkShiftId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(workShiftDTOList[i].WorkShiftId);
                workShiftIdDictionary.Add(workShiftDTOList[i].WorkShiftId, workShiftDTOList[i]);
            }

            workShiftIdSet = sb.ToString();

            WorkShiftScheduleListBL workShiftScheduleListBL = new WorkShiftScheduleListBL(executionContext);
            List<KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>> searchParameters = new List<KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>>();
            searchParameters.Add(new KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>(WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.WORK_SHIFT_ID_LIST, workShiftIdSet.ToString()));
            searchParameters.Add(new KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>(WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>(WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters.IS_ACTIVE, "1"));
            }
            List<WorkShiftScheduleDTO> workShiftScheduleDTOList = workShiftScheduleListBL.GetWorkShiftScheduleDTOList(searchParameters, sqlTransaction);

            if (workShiftScheduleDTOList.Any())
            {
                log.LogVariableState("workShiftScheduleDTOList", workShiftScheduleDTOList);
                foreach (var workShiftScheduleDTO in workShiftScheduleDTOList)
                {
                    if (workShiftIdDictionary.ContainsKey(workShiftScheduleDTO.WorkShiftId))
                    {
                        if (workShiftIdDictionary[workShiftScheduleDTO.WorkShiftId].WorkShiftScheduleDTOList == null)
                        {
                            workShiftIdDictionary[workShiftScheduleDTO.WorkShiftId].WorkShiftScheduleDTOList = new List<WorkShiftScheduleDTO>();
                        }
                        workShiftIdDictionary[workShiftScheduleDTO.WorkShiftId].WorkShiftScheduleDTOList.Add(workShiftScheduleDTO);
                    }
                }
            }

            WorkShiftUserListBL workShiftUserListBL = new WorkShiftUserListBL(executionContext);
            List<KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>> psearchParameters = new List<KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>>();
            psearchParameters.Add(new KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>(WorkShiftUserDTO.SearchByWorkShiftUserParameters.WORK_SHIFT_ID_LIST, workShiftIdSet.ToString()));
            psearchParameters.Add(new KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>(WorkShiftUserDTO.SearchByWorkShiftUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>(WorkShiftUserDTO.SearchByWorkShiftUserParameters.IS_ACTIVE, "1"));
            }
            List<WorkShiftUserDTO> workShiftUsersDTOList = workShiftUserListBL.GetWorkShiftUserDTOList(psearchParameters, sqlTransaction);

            if (workShiftUsersDTOList.Any())
            {
                log.LogVariableState("workShiftUsersDTOList", workShiftUsersDTOList);
                foreach (var workShiftUsersDTO in workShiftUsersDTOList)
                {
                    if (workShiftIdDictionary.ContainsKey(workShiftUsersDTO.WorkShiftId))
                    {
                        if (workShiftIdDictionary[workShiftUsersDTO.WorkShiftId].WorkShiftUsersDTOList == null)
                        {
                            workShiftIdDictionary[workShiftUsersDTO.WorkShiftId].WorkShiftUsersDTOList = new List<WorkShiftUserDTO>();
                        }
                        workShiftIdDictionary[workShiftUsersDTO.WorkShiftId].WorkShiftUsersDTOList.Add(workShiftUsersDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update WorkShiftBL for Web Management Studio
        /// </summary>
        public List<WorkShiftDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<WorkShiftDTO> workShiftDTOLists = new List<WorkShiftDTO>();
            if (workShiftDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (WorkShiftDTO workShiftDTO in workShiftDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            WorkShiftBL workShiftBL = new WorkShiftBL(executionContext, workShiftDTO);
                            workShiftBL.Save(sqlTransaction);
                            workShiftDTOLists.Add(workShiftBL.GetWorkShiftDTO);
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
            return workShiftDTOLists;
        }
    }
}
