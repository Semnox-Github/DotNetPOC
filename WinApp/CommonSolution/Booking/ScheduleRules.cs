/* Project Name - Semnox.Parafait.Booking.ScheduleRules 
* Description  - Business call object of the AttractionScheduleRules
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Booking
{

    /// <summary>
    /// Attraction Schedules
    /// </summary>
    public class ScheduleRulesBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ScheduleRulesDTO scheduleRulesDTO; 
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of ScheduleRulesBL class
        /// </summary>
        public ScheduleRulesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            scheduleRulesDTO = null;
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the scheduleRules id as the parameter
        /// Would fetch the scheduleRules object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public ScheduleRulesBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction); 
            ScheduleRulesDataHandler scheduleRulesDataHandler = new ScheduleRulesDataHandler(sqlTransaction);
            scheduleRulesDTO = scheduleRulesDataHandler.GetScheduleRulesDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates scheduleRulesBL object using the ScheduleRulesDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="scheduleRulesDTO">ScheduleRulesDTO object</param>
        public ScheduleRulesBL(ExecutionContext executionContext, ScheduleRulesDTO scheduleRulesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scheduleRulesDTO);
            this.scheduleRulesDTO = scheduleRulesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AttractionScheduleRules
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ScheduleRulesDataHandler scheduleRulesDataHandler = new ScheduleRulesDataHandler(sqlTransaction);
            if (scheduleRulesDTO.ScheduleRulesId < 0)
            {
                int id = scheduleRulesDataHandler.InsertScheduleRules(scheduleRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scheduleRulesDTO.ScheduleRulesId = id; 
                scheduleRulesDTO.AcceptChanges(); 
            }
            else
            {
                if (scheduleRulesDTO.IsChanged)
                {
                    scheduleRulesDataHandler.UpdateScheduleRules(scheduleRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId()); 
                    scheduleRulesDTO.AcceptChanges(); 
                }
            }
            log.LogMethodExit();
        }
         
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ScheduleRulesDTO ScheduleRulesDTO
        {
            get
            {
                return scheduleRulesDTO;
            }
        }

    }


    /// <summary>
    /// Manages the list of  Schedules rules
    /// </summary>
    public class ScheduleRulesList
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ScheduleRulesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// GetScheduleRulesDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<AttractionScheduleRulesDTO></returns>
        public List<ScheduleRulesDTO> GetScheduleRulesDTOList(List<KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScheduleRulesDataHandler scheduleRulesDataHandler = new ScheduleRulesDataHandler(sqlTransaction);
            List<ScheduleRulesDTO> returnValue = scheduleRulesDataHandler.GetScheduleRulesDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        } 
    }

}
