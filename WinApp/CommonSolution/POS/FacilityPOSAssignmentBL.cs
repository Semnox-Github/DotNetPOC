/********************************************************************************************
 * Project Name - FacilityPOSAssignmentBL
 * Description  - BL class for FacilityPOSAssignment
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.70        08-Mar-2019   Guru S A       Moved to POS namespace
              07-May-2019   Akshay G       Added Constructor with executionContext and facilityPOSAssignmentDTOList 
                                           and SaveFacilityPOSAssignmentDTOList() in FacilityPOSAssignmentList class
              31-May-2019   Jagan Mohana   Moved from Bookings to POS and Code merge from Development to WebManagementStudio
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.POS
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
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrorList.Select(x => x.Message)));
                throw new ValidationException(message, validationErrorList);
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
            log.LogMethodExit();
        }

        private List<ValidationError> Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>(FacilityPOSAssignmentDTO.SearchByParameters.FACILITY_ID, facilityPOSAssignmentDTO.FacilityId.ToString()));
            searchParameters.Add(new KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>(FacilityPOSAssignmentDTO.SearchByParameters.POS_MACHINE_ID, facilityPOSAssignmentDTO.POSMachineId.ToString()));
            FacilityPOSAssignmentDataHandler facilityPOSAssignmentDataHandler = new FacilityPOSAssignmentDataHandler(sqlTransaction);
            List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList = new List<FacilityPOSAssignmentDTO>();
            facilityPOSAssignmentDTOList = facilityPOSAssignmentDataHandler.GetFacilityPOSAssignmentDTOList(searchParameters);
            if (facilityPOSAssignmentDTOList != null && facilityPOSAssignmentDTOList.Any())
            {
                log.Debug("Duplicate entries detail");
                validationErrorList.Add(new ValidationError("FacilityPOSAssignment", "PosMachine", MessageContainerList.GetMessage(executionContext, 4982)));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Delete the FacilityPOSAssignment details based on Id
        /// </summary>
        /// <param name="Id"></param>        
        public void DeleteFacilityPOSAssignment(int Id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(Id, sqlTransaction);
            try
            {
                FacilityPOSAssignmentDataHandler facilityPOSAssignmentDataHandler = new FacilityPOSAssignmentDataHandler(sqlTransaction);
                facilityPOSAssignmentDataHandler.DeleteFacilityPOSAssignment(Id);
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
        public FacilityPOSAssignmentDTO FacilityPOSAssignmentDTO { get { return facilityPOSAssignmentDTO; } }
    }

    /// <summary>
    /// Manages the list of facilityPOSAssignment
    /// </summary>
    public class FacilityPOSAssignmentList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public FacilityPOSAssignmentList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.facilityPOSAssignmentDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor with executionContext and facilityPOSAssignmentDTOList
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="facilityPOSAssignmentDTOList"></param>
        public FacilityPOSAssignmentList(ExecutionContext executionContext, List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList)
        {
            log.LogMethodEntry(executionContext, facilityPOSAssignmentDTOList);
            this.executionContext = executionContext;
            this.facilityPOSAssignmentDTOList = facilityPOSAssignmentDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the facilityPOSAssignment list
        /// </summary>
        public List<FacilityPOSAssignmentDTO> GetFacilityPOSAssignmentDTOList(List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            FacilityPOSAssignmentDataHandler facilityPOSAssignmentDataHandler = new FacilityPOSAssignmentDataHandler(sqlTransaction);
            List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList = facilityPOSAssignmentDataHandler.GetFacilityPOSAssignmentDTOList(searchParameters);
            log.LogMethodExit(facilityPOSAssignmentDTOList);
            return facilityPOSAssignmentDTOList;
        }
        /// <summary>
        /// Saves FacilityPOSAssignment List
        /// </summary>
        public void SaveFacilityPOSAssignmentDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (facilityPOSAssignmentDTOList != null)
                {
                    foreach (FacilityPOSAssignmentDTO facilityPOSAssignmentDTO in facilityPOSAssignmentDTOList)
                    {
                        FacilityPOSAssignmentBL facilityPOSAssignmentBL = new FacilityPOSAssignmentBL(executionContext, facilityPOSAssignmentDTO);
                        facilityPOSAssignmentBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Delete the Modifier SetList
        /// </summary>
        public void DeleteFacilityPOSAssignmentDTOList()
        {
            log.LogMethodEntry();
            if (facilityPOSAssignmentDTOList != null && facilityPOSAssignmentDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (FacilityPOSAssignmentDTO facilityPOSAssignmentDTO in facilityPOSAssignmentDTOList)
                    {
                        if (facilityPOSAssignmentDTO.IsChanged && facilityPOSAssignmentDTO.Id > -1)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                FacilityPOSAssignmentBL facilityPOSAssignmentBL = new FacilityPOSAssignmentBL(executionContext, facilityPOSAssignmentDTO);
                                facilityPOSAssignmentBL.DeleteFacilityPOSAssignment(facilityPOSAssignmentDTO.Id, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}