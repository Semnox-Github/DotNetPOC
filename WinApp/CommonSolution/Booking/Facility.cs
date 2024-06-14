/* Project Name - Semnox.Parafait.Booking.Facility 
* Description  - Business call object of the Facility
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
    /// Facility
    /// </summary>
    public class FacilityBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FacilityDTO facilityDTO; 
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of FacilityBL class
        /// </summary>
        public FacilityBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            facilityDTO = null;
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Facility id as the parameter
        /// Would fetch the facility object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public FacilityBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction); 
            FacilityDataHandler facilityDataHandler = new FacilityDataHandler(sqlTransaction);
            facilityDTO = facilityDataHandler.GetFacilityDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates FacilityBL object using the FacilityDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="FacilityDTO">fcilityDTO object</param>
        public FacilityBL(ExecutionContext executionContext, FacilityDTO facilityDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, facilityDTO);
            this.facilityDTO = facilityDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Facility
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            FacilityDataHandler facilityDataHandler = new FacilityDataHandler(sqlTransaction);
            if (facilityDTO.FacilityId < 0)
            {
                int id = facilityDataHandler.InsertFacility(facilityDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                facilityDTO.FacilityId = id; 
                facilityDTO.AcceptChanges(); 
            }
            else
            {
                if (facilityDTO.IsChanged)
                {
                    facilityDataHandler.UpdateFacility(facilityDTO, executionContext.GetUserId(), executionContext.GetSiteId()); 
                    facilityDTO.AcceptChanges(); 
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Gets the DTO
        /// </summary>
        public FacilityDTO FacilityDTO
        {
            get
            {
                return facilityDTO;
            }
        }
        /// <summary>
        /// Check whether facity can accept additional quantity
        /// </summary>
        /// <param name="additonalQty">int</param>
        public void CanAcceptAdditionalQty(int additonalQty)
        {
            log.LogMethodEntry(additonalQty);

            log.LogMethodExit();
        }

    }


    /// <summary>
    /// Manages the list of Facilityies
    /// </summary>
    public class FacilityList
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public FacilityList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// GetFacilityDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<FacilityDTO></returns>
        public List<FacilityDTO> GetFacilityDTOList(List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            FacilityDataHandler facilityDataHandler = new FacilityDataHandler(sqlTransaction);
            List<FacilityDTO> returnValue = facilityDataHandler.GetFacilityDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        } 
    }

}
