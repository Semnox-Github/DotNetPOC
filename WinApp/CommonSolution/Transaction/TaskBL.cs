/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  Task
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        16-June-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic for Task class.
    /// </summary>
    public class TaskBL
    {
        private TaskDTO taskDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TaskBL class
        /// </summary>
        private TaskBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates TaskBL object using the TaskDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="taskDTO">TaskDTO object</param>
        public TaskBL(ExecutionContext executionContext, TaskDTO taskDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, taskDTO);
            this.taskDTO = taskDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Task id as the parameter
        /// Would fetch the Task object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of Task passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public TaskBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TaskDataHandler taskDataHandler = new TaskDataHandler(sqlTransaction);
            taskDTO = taskDataHandler.GetTaskDTO(id);
            if (taskDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Task", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TaskDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (taskDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TaskDataHandler taskDataHandler = new TaskDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (taskDTO.TaskId < 0)
            {
                log.LogVariableState("TaskDTO", taskDTO);
                taskDTO = taskDataHandler.Insert(taskDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                taskDTO.AcceptChanges();
            }
            else if (taskDTO.IsChanged)
            {
                log.LogVariableState("TaskDTO", taskDTO);
                taskDTO = taskDataHandler.Update(taskDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                taskDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TaskDTO  values 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (true) // Fields to be validated here.
            {
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TaskDTO TaskDTO
        {
            get
            {
                return taskDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of TaskDTO
    /// </summary>
    public class TaskListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TaskDTO> taskDTOList = new List<TaskDTO>();
        /// <summary>
        /// Parameterized constructor for TaskListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public TaskListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TaskListBL with taskDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="taskDTOList">Task DTO List object is passed as parameter</param>
        public TaskListBL(ExecutionContext executionContext,
                                                List<TaskDTO> taskDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, taskDTOList);
            this.taskDTOList = taskDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the TaskDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of TaskDTO</returns>
        public List<TaskDTO> GetTaskDTOList(List<KeyValuePair<TaskDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TaskDataHandler taskDataHandler = new TaskDataHandler(sqlTransaction);
            List<TaskDTO> taskDTOList = taskDataHandler.GetTaskDTOList(searchParameters);
            log.LogMethodExit(taskDTOList);
            return taskDTOList;
        }

        /// <summary>
        /// Saves the  List of TaskDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (taskDTOList == null ||
                taskDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < taskDTOList.Count; i++)
            {
                var taskDTO = taskDTOList[i];
                if (taskDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                    taskBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TaskDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TaskDTO", taskDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonusLoadLimit"></param>
        /// <param name="accountId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public bool checkBonusLoadLimit(int bonusLoadLimit, int accountId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(bonusLoadLimit, sqlTransaction);
            TaskDataHandler taskDataHandler = new TaskDataHandler(sqlTransaction);
            int businessStart = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME");
            bool result = taskDataHandler.checkBonusLoadLimit(bonusLoadLimit, accountId, businessStart);
            log.LogMethodExit(result);
            return result;
        }

    }
}
