/********************************************************************************************
 * Project Name - User
 * Description  - Business logic of WorkShiftUserBL
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
    public class WorkShiftUserBL
    {
        private WorkShiftUserDTO workShiftUserDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of WorkShiftUserBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private WorkShiftUserBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the WorkShiftUserBL id as the parameter
        /// Would fetch the WorkShiftUser object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public WorkShiftUserBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            WorkShiftUserDataHandler workShiftUserDataHandler = new WorkShiftUserDataHandler(sqlTransaction);
            workShiftUserDTO = workShiftUserDataHandler.GetWorkShiftUser(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates WorkShiftUserBL object using the WorkShiftUserDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="WorkShiftUserDTO">workShiftUserDTO object</param>
        public WorkShiftUserBL(ExecutionContext executionContext, WorkShiftUserDTO workShiftUserDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, workShiftUserDTO);
            this.workShiftUserDTO = workShiftUserDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get WorkShiftUserDTO Object
        /// </summary>
        public WorkShiftUserDTO GetWorkShiftUserDTO
        {
            get { return workShiftUserDTO; }
        }

        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (workShiftUserDTO == null)
            {
                //Validation to be implemented.
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the WorkShiftUserDTO
        /// Checks if the WorkShiftSchedule id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            WorkShiftUserDataHandler workShiftUserDataHandler = new WorkShiftUserDataHandler(sqlTransaction);
            Validate();
            if (workShiftUserDTO.Id < 0)
            {
                workShiftUserDTO = workShiftUserDataHandler.Insert(workShiftUserDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                workShiftUserDTO.AcceptChanges();
            }
            else
            {
                if (workShiftUserDTO.IsChanged)
                {
                    workShiftUserDTO = workShiftUserDataHandler.Update(workShiftUserDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    workShiftUserDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of workShiftUserList BL
    /// </summary>
    public class WorkShiftUserListBL
    {
        private List<WorkShiftUserDTO> workShiftUserDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public WorkShiftUserListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.workShiftUserDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="workShiftUserDTO"></param>
        /// <param name="executionContext"></param>
        public WorkShiftUserListBL(ExecutionContext executionContext, List<WorkShiftUserDTO> workShiftUserDTOList)
        {
            log.LogMethodEntry(workShiftUserDTOList, executionContext);
            this.executionContext = executionContext;
            this.workShiftUserDTOList = workShiftUserDTOList;
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
                if (workShiftUserDTOList != null)
                {
                    foreach (WorkShiftUserDTO workShiftUserDTO in workShiftUserDTOList)
                    {
                        WorkShiftUserBL workShiftUserBL = new WorkShiftUserBL(executionContext, workShiftUserDTO);
                        workShiftUserBL.Save(sqlTransaction);
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
        /// Returns the WorkShiftUserDTO  List
        /// </summary>
        public List<WorkShiftUserDTO> GetWorkShiftUserDTOList(List<KeyValuePair<WorkShiftUserDTO.SearchByWorkShiftUserParameters, string>> searchParameters,
                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            WorkShiftUserDataHandler workShiftUserDataHandler = new WorkShiftUserDataHandler(sqlTransaction);
            List<WorkShiftUserDTO> workShiftUserDTOList = workShiftUserDataHandler.GetWorkShiftUserDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(workShiftUserDTOList);
            return workShiftUserDTOList;
        }
    }
}
