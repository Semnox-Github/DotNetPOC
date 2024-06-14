/* Project Name - Semnox.Parafait.Booking.MasterSchedule 
* Description  - Business call object of the MasterSchedule
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
    public class MasterScheduleBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MasterScheduleDTO masterScheduleDTO; 
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of MasterScheduleBL class
        /// </summary>
        public MasterScheduleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            masterScheduleDTO = null;
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the masterSchedule id as the parameter
        /// Would fetch the masterSchedule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public MasterScheduleBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction); 
            MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTransaction);
            masterScheduleDTO = masterScheduleDataHandler.GetMasterScheduleDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates masterScheduleBL object using the masterScheduleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="masterScheduleDTO">masterScheduleDTO object</param>
        public MasterScheduleBL(ExecutionContext executionContext, MasterScheduleDTO masterScheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, masterScheduleDTO);
            this.masterScheduleDTO = masterScheduleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the masterSchedule
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTransaction);
            if (masterScheduleDTO.MasterScheduleId < 0)
            {
                int id = masterScheduleDataHandler.InsertMasterSchedule(masterScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                masterScheduleDTO.MasterScheduleId = id; 
                masterScheduleDTO.AcceptChanges(); 
            }
            else
            {
                if (masterScheduleDTO.IsChanged)
                {
                    masterScheduleDataHandler.UpdateMasterSchedule(masterScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId()); 
                    masterScheduleDTO.AcceptChanges(); 
                }
            }
            log.LogMethodExit();
        }
         
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MasterScheduleDTO MasterScheduleDTO
        {
            get
            {
                return masterScheduleDTO;
            }
        }

    }


    /// <summary>
    /// Manages the list of Attraction Schedules
    /// </summary>
    public class MasterScheduleList
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public MasterScheduleList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// GetMasterScheduleDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<MasterScheduleDTO></returns>
        public List<MasterScheduleDTO> GetMasterScheduleDTOList(List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTransaction);
            List<MasterScheduleDTO> returnValue = masterScheduleDataHandler.GetMasterScheduleDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        } 
    }

}
