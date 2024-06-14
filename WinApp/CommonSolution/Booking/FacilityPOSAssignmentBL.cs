using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Booking
{
    public class FacilityPOSAssignmentBL
    {
        FacilityPOSAssignmentDTO facilityPOSAssignmentDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of FacilityPOSAssignment class
        /// </summary>
        private FacilityPOSAssignmentBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            facilityPOSAssignmentDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the FacilityPOSAssignment DTO based on the facilityPOSAssignment id passed 
        /// </summary>
        /// <param name="facilityPOSAssignmentId">FacilityPOSAssignment id</param>
        public FacilityPOSAssignmentBL(ExecutionContext executionContext, int facilityPOSAssignmentId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, facilityPOSAssignmentId, sqlTransaction);
            FacilityPOSAssignmentDataHandler facilityPOSAssignmentDataHandler = new FacilityPOSAssignmentDataHandler(sqlTransaction);
            facilityPOSAssignmentDTO = facilityPOSAssignmentDataHandler.GetFacilityPOSAssignmentDTO(facilityPOSAssignmentId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates facilityPOSAssignment object using the FacilityPOSAssignment
        /// </summary>
        /// <param name="facilityPOSAssignment">FacilityPOSAssignment object</param>
        public FacilityPOSAssignmentBL(ExecutionContext executionContext, FacilityPOSAssignmentDTO facilityPOSAssignment)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.facilityPOSAssignmentDTO = facilityPOSAssignment;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the facilityPOSAssignment record
        /// Checks if the FacilityPOSAssignmentId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (facilityPOSAssignmentDTO.IsChanged)
            {
                List<ValidationError> validationErrorList = Validate(sqlTransaction);
                if (validationErrorList.Count > 0)
                {
                    throw new ValidationException("Validation Failed", validationErrorList);
                }
                FacilityPOSAssignmentDataHandler facilityPOSAssignmentDataHandler = new FacilityPOSAssignmentDataHandler(sqlTransaction);
                if (facilityPOSAssignmentDTO.Id < 0)
                {
                    facilityPOSAssignmentDTO = facilityPOSAssignmentDataHandler.InsertFacilityPOSAssignment(facilityPOSAssignmentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    facilityPOSAssignmentDTO.AcceptChanges();
                }
                else
                {
                    if (facilityPOSAssignmentDTO.IsChanged)
                    {
                        facilityPOSAssignmentDTO = facilityPOSAssignmentDataHandler.UpdateFacilityPOSAssignment(facilityPOSAssignmentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        facilityPOSAssignmentDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodExit();
        }

        private List<ValidationError> Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public FacilityPOSAssignmentDTO FacilityPOSAssignmentDTO { get { return facilityPOSAssignmentDTO; } }
    }

    /// <summary>
    /// Manages the list of facilityPOSAssignment
    /// </summary>
    public class FacilityPOSAssignmentList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public FacilityPOSAssignmentList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the facilityPOSAssignment list
        /// </summary>
        public List<FacilityPOSAssignmentDTO> GetFacilityPOSAssignmentDTOList(List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            FacilityPOSAssignmentDataHandler facilityPOSAssignmentDataHandler = new FacilityPOSAssignmentDataHandler(sqlTransaction);
            List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList = facilityPOSAssignmentDataHandler.GetFacilityPOSAssignmentDTOList(searchParameters);
            log.LogMethodExit(facilityPOSAssignmentDTOList);
            return facilityPOSAssignmentDTOList;
        }
    }
}
