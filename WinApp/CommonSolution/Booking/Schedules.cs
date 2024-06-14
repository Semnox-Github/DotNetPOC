/* Project Name - Semnox.Parafait.Booking.Schedules 
* Description  - Business call object of the Schedules
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.50        26-Nov-2018    Guru S A             Booking enhancement changes 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Booking
{

    /// <summary>
    ///  Schedules
    /// </summary>
    public class SchedulesBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SchedulesDTO scheduleDTO; 
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of AttractionSchedulesBL class
        /// </summary>
        public SchedulesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            scheduleDTO = null;
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the attractionSchedule id as the parameter
        /// Would fetch the attractionSchedule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public SchedulesBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction); 
            SchedulesDataHandler scheduleDataHandler = new SchedulesDataHandler(sqlTransaction);
           scheduleDTO = scheduleDataHandler.GetScheduleDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates SchedulesBL object using the ScheduleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="scheduleDTO">scheduleDTO object</param>
        public SchedulesBL(ExecutionContext executionContext, SchedulesDTO scheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scheduleDTO);
            this.scheduleDTO = scheduleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Schedule
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SchedulesDataHandler scheduleDataHandler = new SchedulesDataHandler(sqlTransaction);
            if (scheduleDTO.ScheduleId < 0)
            {
                int id = scheduleDataHandler.InsertSchedule(scheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scheduleDTO.ScheduleId = id; 
                scheduleDTO.AcceptChanges(); 
            }
            else
            {
                if (scheduleDTO.IsChanged)
                {
                    scheduleDataHandler.UpdateSchedule(scheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId()); 
                    scheduleDTO.AcceptChanges(); 
                }
            }
            log.LogMethodExit();
        }
         
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SchedulesDTO ScheduleDTO
        { 
            get
            {
                return scheduleDTO;
            }
        }

        public void GetResourceAvailability()
        {
            log.LogMethodEntry();

            log.LogMethodExit();
        }
    }


    /// <summary>
    /// Manages the list of Schedules
    /// </summary>
    public class SchedulesListBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public SchedulesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// GetScheduleDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<ScheduleDTO></returns>
        public List<SchedulesDTO> GetScheduleDTOList(List<KeyValuePair<SchedulesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SchedulesDataHandler scheduleDataHandler = new SchedulesDataHandler(sqlTransaction);
            List<SchedulesDTO> returnValue = scheduleDataHandler.GetScheduleDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// GetAttractionSchedule
        /// </summary>
        /// <param name="bookingproductId"></param>
        /// <param name="reservationDate"></param>
        /// <param name="facilityId"></param>
        /// <returns>List<scheduleDTO></returns>
        public List<SchedulesDTO> GetScheduleDTOList(int bookingproductId, DateTime reservationDate, int facilityId)
        {
            log.LogMethodEntry(bookingproductId, reservationDate, facilityId);
            SchedulesDataHandler schedulesDataHandler = new SchedulesDataHandler(null);
            List<SchedulesDTO> scheduleDTOList = schedulesDataHandler.GetAttractionSchedule(bookingproductId, reservationDate, facilityId);
            log.LogMethodExit(scheduleDTOList);
            return scheduleDTOList;
        }

        /// <summary>
        /// GetBookingScheduleList
        /// </summary>
        /// <param name="sqlSearchParams"></param>
        /// <returns></returns>
        public DataTable GetBookingScheduleList(List<SqlParameter> sqlSearchParams)
        {
            log.LogMethodEntry(sqlSearchParams);
            SchedulesDataHandler schedulesDataHandler = new SchedulesDataHandler(null);
            DataTable scheduleDTOList = schedulesDataHandler.GetBookingScheduleList(sqlSearchParams);
            log.LogMethodExit(scheduleDTOList);
            return scheduleDTOList;
        } 

    }

}
