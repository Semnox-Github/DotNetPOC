/* Project Name - Semnox.Parafait.Booking.ScheduleRules 
* Description  - Business call object of the AttractionScheduleRules
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
*2.70        18-Mar-2019    Guru S A             Booking phase 2 enhancement changes
*2.70        18-Feb-2018    Nagesh Badiger       Added new method SaveUpdateProductAttractionSchedule
*2.70        30-May-2019    Akshay G             Moved from Booking project to Product
*2.70        27-Jun-2019    Akshay G             Added DeleteScheduleRules() method
*2.80.0      21-02-2020     Girish Kundar        Modified : 3 tier Changes for REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
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

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (scheduleRulesDTO.FacilityMapId == -1)
            {
                validationErrorList.Add(new ValidationError("ScheduleRules", "FacilityMapId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Facility Map Id"))));
            }
            if (scheduleRulesDTO.Units != null && scheduleRulesDTO.Units < -1)
            {
                validationErrorList.Add(new ValidationError("MasterSchedule", "Units", MessageContainerList.GetMessage(executionContext, 1)));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the AttractionScheduleRules
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            ScheduleRulesDataHandler scheduleRulesDataHandler = new ScheduleRulesDataHandler(sqlTransaction);
            if (scheduleRulesDTO.ScheduleRulesId < 0)
            {
                scheduleRulesDTO = scheduleRulesDataHandler.InsertScheduleRules(scheduleRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(scheduleRulesDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("AttractionScheduleRules", scheduleRulesDTO.Guid, sqlTransaction);
                }
                scheduleRulesDTO.AcceptChanges();
            }
            else
            {
                if (scheduleRulesDTO.IsChanged)
                {
                    scheduleRulesDTO = scheduleRulesDataHandler.UpdateScheduleRules(scheduleRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(scheduleRulesDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("AttractionScheduleRules", scheduleRulesDTO.Guid, sqlTransaction);
                    }
                    scheduleRulesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Delete the ScheduleRules based on scheduleRulesId
        /// </summary>
        /// <param name="scheduleRulesId"></param>        
        /// <param name="sqlTransaction"></param>        
        public void DeleteScheduleRules(int scheduleRulesId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scheduleRulesId, sqlTransaction);
            try
            {
                ScheduleRulesDataHandler scheduleRulesDataHandler = new ScheduleRulesDataHandler(sqlTransaction);
                scheduleRulesDataHandler.DeleteScheduleRules(scheduleRulesId);
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
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
        private List<ScheduleRulesDTO> scheduleRulesDTOList;
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
        /// Parameterized Constructor with executionContext and scheduleRuleDTOList
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="scheduleRulesDTO"></param>
        public ScheduleRulesList(ExecutionContext executionContext, List<ScheduleRulesDTO> scheduleRulesDTO)
        {
            log.LogMethodEntry(scheduleRulesDTOList, executionContext);
            this.executionContext = executionContext;
            this.scheduleRulesDTOList = scheduleRulesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetScheduleRulesDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>attractionScheduleRulesDTOList</returns>
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
