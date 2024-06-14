/********************************************************************************************
 * Project Name - User
 * Description  - Business logic of WorkShiftScheduleBL
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

namespace Semnox.Parafait.User
{
    public class WorkShiftScheduleBL
    {
        private WorkShiftScheduleDTO workShiftScheduleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of WorkShiftScheduleBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private WorkShiftScheduleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the WorkShiftScheduleBL id as the parameter
        /// Would fetch the WorkShiftSchedule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public WorkShiftScheduleBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            WorkShiftScheduleDataHandler workShiftScheduleDataHandler = new WorkShiftScheduleDataHandler(sqlTransaction);
            workShiftScheduleDTO = workShiftScheduleDataHandler.GetWorkShiftSchedule(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates WorkShiftScheduleBL object using the WorkShiftScheduleDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="WorkShiftScheduleDTO">workShiftScheduleDTO object</param>
        public WorkShiftScheduleBL(ExecutionContext executionContext, WorkShiftScheduleDTO workShiftScheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, workShiftScheduleDTO);
            this.workShiftScheduleDTO = workShiftScheduleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get WorkShiftScheduleDTO Object
        /// </summary>
        public WorkShiftScheduleDTO GetWorkShiftScheduleDTO
        {
            get { return workShiftScheduleDTO; }
        }

        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (workShiftScheduleDTO == null)
            {
                //Validation to be implemented.
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the WorkShiftScheduleDTO
        /// Checks if the WorkShiftSchedule id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            WorkShiftScheduleDataHandler workShiftScheduleDataHandler = new WorkShiftScheduleDataHandler(sqlTransaction);
            Validate();
            if (workShiftScheduleDTO.WorkShiftScheduleId < 0)
            {
                workShiftScheduleDTO = workShiftScheduleDataHandler.Insert(workShiftScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                workShiftScheduleDTO.AcceptChanges();
            }
            else
            {
                if (workShiftScheduleDTO.IsChanged)
                {
                    workShiftScheduleDTO = workShiftScheduleDataHandler.Update(workShiftScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    workShiftScheduleDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of workShiftScheduleList BL
    /// </summary>
    public class WorkShiftScheduleListBL
    {
        private List<WorkShiftScheduleDTO> workShiftScheduleDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public WorkShiftScheduleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.workShiftScheduleDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="workShiftScheduleDTO"></param>
        /// <param name="executionContext"></param>
        public WorkShiftScheduleListBL(ExecutionContext executionContext, List<WorkShiftScheduleDTO> workShiftScheduleDTOList)
        {
            log.LogMethodEntry(workShiftScheduleDTOList, executionContext);
            this.executionContext = executionContext;
            this.workShiftScheduleDTOList = workShiftScheduleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                if (workShiftScheduleDTOList != null)
                {
                    foreach (WorkShiftScheduleDTO workShiftScheduleDTO in workShiftScheduleDTOList)
                    {
                        WorkShiftScheduleBL workShiftScheduleBL = new WorkShiftScheduleBL(executionContext, workShiftScheduleDTO);
                        workShiftScheduleBL.Save(sqlTransaction);
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the WorkShiftScheduleDTO  List
        /// </summary>
        public List<WorkShiftScheduleDTO> GetWorkShiftScheduleDTOList(List<KeyValuePair<WorkShiftScheduleDTO.SearchByWorkShiftScheduleParameters, string>> searchParameters,
                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            WorkShiftScheduleDataHandler workShiftScheduleDataHandler = new WorkShiftScheduleDataHandler(sqlTransaction);
            List<WorkShiftScheduleDTO> workShiftScheduleDTOList = workShiftScheduleDataHandler.GetWorkShiftScheduleDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(workShiftScheduleDTOList);
            return workShiftScheduleDTOList;
        }
    }
}
