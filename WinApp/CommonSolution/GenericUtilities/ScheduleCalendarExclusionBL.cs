/********************************************************************************************
 * Project Name - Schedule Exclusion
 * Description  - Schedule Exclusion business logics 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        13-Jan-2016   Raghuveera          Created
 *2.40        29-Sep-2018   Jagan Mohan         Added new constructor ScheduleExclusionList and 
 *                                                 methods SaveUpdateScheduleExclusionsList
 *2.70        08-Mar-2019   Guru S A            Renamed ScheduleExclusion as ScheduleCalendarExclusionBL
 *2.70.2        26-Jul-2019   Dakshakh raj        Modified : Log method entries/exits, Save method
  *2.90        11-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate,
 *                                                 List class changes as per 3 tier standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Schedule Exclusion inserts and modifies the schedule inclusion/exclusion records
    /// </summary>
    public class ScheduleCalendarExclusionBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ScheduleCalendarExclusionDTO scheduleExclusionDTO;
        private ExecutionContext executionContext;

        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ScheduleCalendarExclusionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.scheduleExclusionDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="scheduleExclusionId">scheduleExclusionId</param>
        public ScheduleCalendarExclusionBL(ExecutionContext executionContext, int scheduleExclusionId, SqlTransaction sqlTransaction = null) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scheduleExclusionDTO);

            ScheduleCalendarExclusionDataHandler scheduleExclusionDataHandler = new ScheduleCalendarExclusionDataHandler(executionContext,sqlTransaction);
            this.scheduleExclusionDTO = scheduleExclusionDataHandler.GetScheduleCalendarExclusion(scheduleExclusionId);
            this.scheduleExclusionDTO = SiteContainerList.FromSiteDateTime(executionContext, this.scheduleExclusionDTO,"", "Siteid");
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="scheduleExclusionDTO">scheduleExclusionDTO</param>
        public ScheduleCalendarExclusionBL(ExecutionContext executionContext, ScheduleCalendarExclusionDTO scheduleExclusionDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scheduleExclusionDTO);
            this.scheduleExclusionDTO = scheduleExclusionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the schedule exclusion
        /// Checks if the tasks id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScheduleCalendarExclusionDataHandler scheduleExclusionDataHandler = new ScheduleCalendarExclusionDataHandler(executionContext,sqlTransaction);
            if (scheduleExclusionDTO.IsChanged == false && scheduleExclusionDTO.ScheduleExclusionId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }


            if (scheduleExclusionDTO.ScheduleExclusionId < 0)
            {
                SiteContainerList.ToSiteDateTime(executionContext, scheduleExclusionDTO);
                scheduleExclusionDTO = scheduleExclusionDataHandler.InsertScheduleCalendarExclusion(scheduleExclusionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                SiteContainerList.FromSiteDateTime(executionContext, scheduleExclusionDTO, "", "Siteid");
                scheduleExclusionDTO.AcceptChanges();
            }
            else
            {
                if (scheduleExclusionDTO.IsChanged)
                {
                    SiteContainerList.ToSiteDateTime(executionContext, scheduleExclusionDTO);
                    scheduleExclusionDTO = scheduleExclusionDataHandler.UpdateScheduleCalendarExclusion(scheduleExclusionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    SiteContainerList.FromSiteDateTime(executionContext, scheduleExclusionDTO, "", "Siteid");
                    scheduleExclusionDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validate the scheduleExclusionDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Returns the exclusion days
        /// </summary>
        /// <param name="exclusionId">Exclusion id is integer type parameter</param>
        /// <param name="dtdate">DateTime type parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>bool type value</returns>
        public bool GetExclusionDays(int exclusionId, DateTime dtdate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(exclusionId, dtdate, sqlTransaction);
            ScheduleCalendarExclusionDataHandler scheduleExclusionDataHandler = new ScheduleCalendarExclusionDataHandler(executionContext,sqlTransaction);
            bool exclusionDays = scheduleExclusionDataHandler.GetExclusionDays(exclusionId, dtdate);
            log.LogMethodExit(exclusionDays);
            return exclusionDays;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ScheduleCalendarExclusionDTO ScheduleCalendarExclusionDTO
        {
            get
            {
                return scheduleExclusionDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of schedule exclusion
    /// </summary>
    public class ScheduleCalendarExclusionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ScheduleCalendarExclusionDTO> scheduleExclusionDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// create the scheduleExclusionDTOs list object
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ScheduleCalendarExclusionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// create the scheduleExclusionDTOs list object
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="scheduleExclusionDTOs">scheduleExclusionDTOs</param>
        public ScheduleCalendarExclusionListBL(ExecutionContext executionContext, List<ScheduleCalendarExclusionDTO> scheduleExclusionDTOList)
        {
            log.LogMethodEntry(scheduleExclusionDTOList, executionContext);
            this.scheduleExclusionDTOList = scheduleExclusionDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Schedule exclusion list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ScheduleCalendarExclusionDTO> GetAllScheduleExclusions(List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScheduleCalendarExclusionDataHandler scheduleExclusionDataHandler = new ScheduleCalendarExclusionDataHandler(executionContext,sqlTransaction);
            List<ScheduleCalendarExclusionDTO> scheduleExclusionDTOs = scheduleExclusionDataHandler.GetScheduleCalendarExclusionList(searchParameters);
            scheduleExclusionDTOs = SiteContainerList.FromSiteDateTime(executionContext, scheduleExclusionDTOs, "", "Siteid");
            log.LogMethodExit(scheduleExclusionDTOs);
            return scheduleExclusionDTOs;
        }

        /// <summary>
        /// Gets the ScheduleCalendarExclusionDTO List for scheduleIdList
        /// </summary>
        /// <param name="scheduleIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ScheduleCalendarExclusionDTO</returns>
        public List<ScheduleCalendarExclusionDTO> GetScheduleCalendarExclusionDTOList(List<int> scheduleIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scheduleIdList, activeRecords);
            ScheduleCalendarExclusionDataHandler scheduleExclusionDataHandler = new ScheduleCalendarExclusionDataHandler(executionContext,sqlTransaction);
            this.scheduleExclusionDTOList = scheduleExclusionDataHandler.GetScheduleCalendarExclusionDTOList(scheduleIdList, activeRecords);
            this.scheduleExclusionDTOList = SiteContainerList.FromSiteDateTime(executionContext, this.scheduleExclusionDTOList, "", "Siteid");
            log.LogMethodExit(scheduleExclusionDTOList);
            return scheduleExclusionDTOList;
        }

   
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (scheduleExclusionDTOList == null ||
                scheduleExclusionDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < scheduleExclusionDTOList.Count; i++)
            {
                var scheduleExclusionDTO = scheduleExclusionDTOList[i];
                if (scheduleExclusionDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ScheduleCalendarExclusionBL scheduleCalendarExclusionBL = new ScheduleCalendarExclusionBL(executionContext, scheduleExclusionDTO);
                    scheduleCalendarExclusionBL.Save();
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
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving scheduleExclusionDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("scheduleExclusionDTO", scheduleExclusionDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
