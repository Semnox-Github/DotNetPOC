/********************************************************************************************
 * Project Name - ScoringEngine
 * Description  - Bussiness logic of ScoringEventCalendar
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.120.0        02-02-2021   Prajwal S      Created 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.GenericUtilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Achievements.ScoringEngine
{
    /// <summary>
    /// Represents a ScoringEventCalendarBL.
    /// </summary>
    internal class ScoringEventCalendarBL
    {
        private ScoringEventCalendarDTO scoringEventCalendarDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private ScoringEventCalendarBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scoringEventCalendarDTO">ScoringEventCalendarDTO</param>
        internal ScoringEventCalendarBL(ExecutionContext executionContext, ScoringEventCalendarDTO scoringEventCalendarDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scoringEventCalendarDTO);
            if (scoringEventCalendarDTO.Id < 0)
            {
                //validate();
            }
            this.scoringEventCalendarDTO = scoringEventCalendarDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScoringEventCalendarBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ScoringEventCalendarDataHandler scoringEventCalendarDataHandler = new ScoringEventCalendarDataHandler(sqlTransaction);
            scoringEventCalendarDTO = scoringEventCalendarDataHandler.GetScoringEventCalendar(id);
            if (scoringEventCalendarDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ScoringEventCalendar", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get ScoringEventCalendarDTO Object
        /// </summary>
        internal ScoringEventCalendarDTO ScoringEventCalendarDTO
        {
            get { return scoringEventCalendarDTO; }
        }


        /// <summary>
        /// Saves the ScoringEventCalendars
        /// Checks if the id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScoringEventCalendarDataHandler scoringEventCalendarDataHandler = new ScoringEventCalendarDataHandler(sqlTransaction);

            if (scoringEventCalendarDTO.Id < 0)
            {
                scoringEventCalendarDTO = scoringEventCalendarDataHandler.Insert(scoringEventCalendarDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scoringEventCalendarDTO.AcceptChanges();
            }
            else
            {
                if (scoringEventCalendarDTO.IsChanged)
                {
                    scoringEventCalendarDTO = scoringEventCalendarDataHandler.Update(scoringEventCalendarDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    scoringEventCalendarDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        internal void Update(ScoringEventCalendarDTO parameterScoringEventCalendarDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterScoringEventCalendarDTO);
            ChangeScoringEventId(parameterScoringEventCalendarDTO.ScoringEventId);
            ChangeActiveFlag(parameterScoringEventCalendarDTO.ActiveFlag);
            ChangeDay(parameterScoringEventCalendarDTO.Day);
            ChangeDate(parameterScoringEventCalendarDTO.Date);
            ChangeFromTime(parameterScoringEventCalendarDTO.FromTime);
            ChangeToTime(parameterScoringEventCalendarDTO.ToTime);
            ChangeFromDatel(parameterScoringEventCalendarDTO.FromDate);
            ChangeEndDate(parameterScoringEventCalendarDTO.EndDate);
            log.LogMethodExit();
        }

        private void ChangeEndDate(DateTime? endDate)
        {
            log.LogMethodEntry(endDate);
            if (scoringEventCalendarDTO.EndDate == endDate)
            {
                log.LogMethodExit(null, "No changes to EndDate");
                return;
            }
            scoringEventCalendarDTO.EndDate = endDate;
            log.LogMethodExit();
        }

        private void ChangeFromDatel(DateTime? fromDate)
        {
            log.LogMethodEntry(fromDate);
            if (scoringEventCalendarDTO.FromDate == fromDate)
            {
                log.LogMethodExit(null, "No changes to FromDate");
                return;
            }
            scoringEventCalendarDTO.FromDate = fromDate;
            log.LogMethodExit();
        }
        private void ChangeToTime(string toTime)
        {
            log.LogMethodEntry(toTime);
            if (scoringEventCalendarDTO.ToTime == toTime)
            {
                log.LogMethodExit(null, "No changes to ToTime");
                return;
            }
            scoringEventCalendarDTO.ToTime = toTime;
            log.LogMethodExit();
        }

        private void ChangeFromTime(string fromTime)
        {
            log.LogMethodEntry(fromTime);
            if (scoringEventCalendarDTO.FromTime == fromTime)
            {
                log.LogMethodExit(null, "No changes to FromTime");
                return;
            }
            scoringEventCalendarDTO.FromTime = fromTime;
            log.LogMethodExit();
        }

        private void ChangeDay(int? day)
        {
            log.LogMethodEntry(day);
            if (scoringEventCalendarDTO.Day == day)
            {
                log.LogMethodExit(null, "No changes to Day");
                return;
            }
            scoringEventCalendarDTO.Day = day;
            log.LogMethodExit();
        }

        private void ChangeDate(DateTime? date)
        {
            log.LogMethodEntry(date);
            if (scoringEventCalendarDTO.Date == date)
            {
                log.LogMethodExit(null, "No changes to Date");
                return;
            }
            scoringEventCalendarDTO.Date = date;
            log.LogMethodExit();
        }

        private void ChangeActiveFlag(bool activeFlag)
        {
            log.LogMethodEntry(activeFlag);
            if (scoringEventCalendarDTO.ActiveFlag == activeFlag)
            {
                log.LogMethodExit(null, "No changes to ScoringEventCalendar activeFlag");
                return;
            }
            scoringEventCalendarDTO.ActiveFlag = activeFlag;
            log.LogMethodExit();
        }

        private void ChangeScoringEventId(int scoringEventId)
        {
            log.LogMethodEntry(scoringEventId);
            // ValidateScoringEventId(scoringEventId);
            if (scoringEventCalendarDTO.ScoringEventId == scoringEventId)
            {
                log.LogMethodExit(null, "No changes to ScoringEventId");
                return;
            }
            scoringEventCalendarDTO.ScoringEventId = scoringEventId;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ScoringEventCalendar
    /// </summary>
    public class ScoringEventCalendarListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ScoringEventCalendarDTO> scoringEventCalendarList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScoringEventCalendarListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="ScoringEventCalendarList"></param>
        public ScoringEventCalendarListBL(ExecutionContext executionContext, List<ScoringEventCalendarDTO> ScoringEventCalendarList)
             : this()
        {
            log.LogMethodEntry(executionContext, ScoringEventCalendarList);
            this.executionContext = executionContext;
            this.scoringEventCalendarList = ScoringEventCalendarList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ScoringEventCalendarDTO List for scoringEventId Id List
        /// </summary>
        /// <param name="scoringEventIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ScoringEventCalendarDTO> GetScoringEventCalendarDTOListOfEvents(List<int> scoringEventIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scoringEventIdList, activeRecords, sqlTransaction);
            ScoringEventCalendarDataHandler scoringEventCalendarDataHandler = new ScoringEventCalendarDataHandler(sqlTransaction);
            List<ScoringEventCalendarDTO> scoringEventCalendarDTOList = scoringEventCalendarDataHandler.GetScoringEventCalendarDTOList(scoringEventIdList, activeRecords);
            log.LogMethodExit(scoringEventCalendarDTOList);
            return scoringEventCalendarDTOList;
        }

        /// <summary>
        /// Returns the ScoringEventCalendar list
        /// </summary>
        public List<ScoringEventCalendarDTO> GetScoringEventCalendarDTOList(List<KeyValuePair<ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScoringEventCalendarDataHandler scoringEventCalendarDataHandler = new ScoringEventCalendarDataHandler(sqlTransaction);
            List<ScoringEventCalendarDTO> scoringEventCalendarDTOList = scoringEventCalendarDataHandler.GetScoringEventCalendarList(searchParameters);
            log.LogMethodExit(scoringEventCalendarDTOList);
            return scoringEventCalendarDTOList;
        }

        /// <summary>
        /// This method should be used to Save ScoringEventCalendar
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (scoringEventCalendarList != null && scoringEventCalendarList.Count != 0)
                {
                    foreach (ScoringEventCalendarDTO scoringEventCalendarDTO in scoringEventCalendarList)
                    {
                        ScoringEventCalendarBL scoringEventCalendarBL = new ScoringEventCalendarBL(executionContext, scoringEventCalendarDTO);
                        scoringEventCalendarBL.Save(sqlTransaction);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(sqlEx.Message);
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
            log.LogMethodExit();
        }
    }
}
