/********************************************************************************************
 * Project Name - FacilityMapDetailsBL
 * Description  - Business logic file for  FacilityMapDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        14-jun-2019   Guru S A                Created 
 *2.70.2      18-Oct-2019   Akshay G                ClubSpeed enhancement changes
 *2.80.0      26-Feb-2020   Girish Kundar           Modified : 3 Tier Changes for API
 *********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for FacilityMapDetails class.
    /// </summary>
    public class FacilityMapDetailsBL
    {
        private FacilityMapDetailsDTO facilityMapDetailsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of FacilityMapDetailsBL class
        /// </summary>
        private FacilityMapDetailsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates FacilityMapDetailsBL object using the FacilityMapDetailsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="facilityMapDetailsDTO">FacilityMapDetailsDTO object</param>
        public FacilityMapDetailsBL(ExecutionContext executionContext, FacilityMapDetailsDTO facilityMapDetailsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, facilityMapDetailsDTO);
            this.facilityMapDetailsDTO = facilityMapDetailsDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the facilityMap id as the parameter
        /// Would fetch the facilityMap object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param> 
        /// <param name="sqlTransaction"></param>
        public FacilityMapDetailsBL(ExecutionContext executionContext, int id,
              bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            FacilityMapDetailsDataHandler facilityMapDetailsDataHandler = new FacilityMapDetailsDataHandler(sqlTransaction);
            facilityMapDetailsDTO = facilityMapDetailsDataHandler.GetFacilityMapDetails(id);
            if (facilityMapDetailsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "FacilityMapDetails", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            FacilityList facilityListBL = new FacilityList(executionContext);
            List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.FACILITY_ID, facilityMapDetailsDTO.FacilityId.ToString()));
            facilityMapDetailsDTO.FacilityDTOList = facilityListBL.GetFacilityDTOList(searchParameters, true, true, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the FacilityMap
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (facilityMapDetailsDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            FacilityMapDetailsDataHandler facilityMapDetailsDataHandler = new FacilityMapDetailsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (facilityMapDetailsDTO.FacilityMapDetailId < 0)
            {
                log.LogVariableState("facilityMapDetailsDTO", facilityMapDetailsDTO);
                facilityMapDetailsDTO = facilityMapDetailsDataHandler.Insert(facilityMapDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(facilityMapDetailsDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("FacilityMapDetails", facilityMapDetailsDTO.Guid, sqlTransaction);
                }
                facilityMapDetailsDTO.AcceptChanges();
            }
            else if (facilityMapDetailsDTO.IsChanged)
            {
                facilityMapDetailsDTO = facilityMapDetailsDataHandler.Update(facilityMapDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(facilityMapDetailsDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("FacilityMapDetails", facilityMapDetailsDTO.Guid, sqlTransaction);
                }
                facilityMapDetailsDTO.AcceptChanges();
            }

            log.LogMethodExit();
        }


        /// <summary>
        /// Validates the FacilityMapDetailsDTO  ,FacilityMapDetailsDTOList - child 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public FacilityMapDetailsDTO FacilityMapDetailsDTO
        {
            get
            {
                return facilityMapDetailsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of FacilityMapDetails
    /// </summary>
    public class FacilityMapDetailsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<FacilityMapDetailsDTO> facilityMapDetailsDTOList = new List<FacilityMapDetailsDTO>(); // To be initialized
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public FacilityMapDetailsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="facilityMapDetailsDTOList"></param>
        public FacilityMapDetailsListBL(ExecutionContext executionContext,
                                               List<FacilityMapDetailsDTO> facilityMapDetailsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, facilityMapDetailsDTOList);
            this.facilityMapDetailsDTOList = facilityMapDetailsDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        ///  Returns the Get the FacilityMapDetailsDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>facilityMapDetailsDTOList</returns>
        public List<FacilityMapDetailsDTO> GetFacilityMapDetailsDTOList(List<KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>> searchParameters,
                                   bool buildChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction, buildChildRecords, activeChildRecords);
            FacilityMapDetailsDataHandler facilityMapDetailsDataHandler = new FacilityMapDetailsDataHandler(sqlTransaction);
            List<FacilityMapDetailsDTO> facilityMapDetailsDTOList = facilityMapDetailsDataHandler.GetAllFacilityMapDetails(searchParameters);
            if (facilityMapDetailsDTOList != null && facilityMapDetailsDTOList.Any() && buildChildRecords)
            {
                Build(facilityMapDetailsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(facilityMapDetailsDTOList);
            return facilityMapDetailsDTOList;
        }

        private void Build(List<FacilityMapDetailsDTO> facilityMapDetailsDTOList, bool activeChildRecords = false,
                          SqlTransaction sqlTransaction = null)
        {
            Dictionary<int, FacilityMapDetailsDTO> facilityMapDetailldictionary = new Dictionary<int, FacilityMapDetailsDTO>();
            StringBuilder sb = new StringBuilder("");
            foreach (FacilityMapDetailsDTO facilityMapDetailsDTO in facilityMapDetailsDTOList)
            {
                FacilityList facilityListBL = new FacilityList(executionContext);
                List<KeyValuePair<FacilityDTO.SearchByParameters, string>> facilitySearchParameters = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
                facilitySearchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.FACILITY_ID, facilityMapDetailsDTO.FacilityId.ToString()));
                facilitySearchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (activeChildRecords)
                {
                    facilitySearchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                }
                facilityMapDetailsDTO.FacilityDTOList = facilityListBL.GetFacilityDTOList(facilitySearchParameters, false,false, sqlTransaction);
            }
        }

        /// <summary>
        /// Saves the FacilityMap List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (facilityMapDetailsDTOList == null ||
                facilityMapDetailsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < facilityMapDetailsDTOList.Count; i++)
            {
                var facilityMapDetailsDTO = facilityMapDetailsDTOList[i];
                if (facilityMapDetailsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    FacilityMapDetailsBL facilityMapDetailsBL = new FacilityMapDetailsBL(executionContext, facilityMapDetailsDTO);
                    facilityMapDetailsBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving FacilityMapDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("FacilityMapDetailsDTO", facilityMapDetailsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
