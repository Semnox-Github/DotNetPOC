/****************************************************************************************************
 * Project Name -  Access  Point
 * Description  - Bussiness logic of  Access Point
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ****************************************************************************************************
 *1.00        14-Jul-2017   Raghuveera         Created 
 *2.60        03-May-2019   Jagan Mohana Rao   Created SaveUpdateLockerAccessPoint() and changes log method entry and exit
 *2.70.2     18-Jul-2019   Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.90       20-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API/Validation
 ****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Bussiness logic for Locker access point
    /// </summary>
    public class LockerAccessPoint
    {
        private LockerAccessPointDTO accessPointDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of LockerAccessPoint class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private LockerAccessPoint(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor will fetch the LockerAccessPoint DTO based on the lockerAccessPoint id passed 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="accessPointId">accessPointId</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public LockerAccessPoint(ExecutionContext executionContext, int accessPointId, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accessPointId, sqltransaction);
            LockerAccessPointDataHandler lockerAccessPointDataHandler = new LockerAccessPointDataHandler(sqltransaction);
            accessPointDTO = lockerAccessPointDataHandler.GetLockerAccessPoint(accessPointId);
            log.LogMethodExit(accessPointDTO);
        }

        /// <summary>
        /// Creates lockerAccessPoint object using the LockerAccessPointDTO
        /// </summary>
        /// <param name="lockerAccessPoint">LockerAccessPointDTO object</param>
        /// <param name="executionContext">executionContext</param>
        public LockerAccessPoint(ExecutionContext executionContext, LockerAccessPointDTO lockerAccessPoint)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, lockerAccessPoint);
            this.accessPointDTO = lockerAccessPoint;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Locker id exists in the assigned range
        /// </summary>
        /// <param name="sqltransaction">sqltransaction</param>
        /// <returns>Returns boolean</returns>
        public bool IsLockerAssignedToAP(SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(sqltransaction);
            bool isLockerAssignedToAP = false;
            if (accessPointDTO != null)
            {
                LockerAccessPointDataHandler lockerAccessPointDataHandler = new LockerAccessPointDataHandler(sqltransaction);
                if (lockerAccessPointDataHandler.GetLockerIdInclussiveAPDetails(accessPointDTO.LockerIDFrom, accessPointDTO.AccessPointId, accessPointDTO.GroupCode, accessPointDTO.Channel) != null || lockerAccessPointDataHandler.GetLockerIdInclussiveAPDetails(accessPointDTO.LockerIDTo, accessPointDTO.AccessPointId, accessPointDTO.GroupCode, accessPointDTO.Channel) != null)
                {
                    isLockerAssignedToAP = true;
                }

            }
            log.LogMethodExit(isLockerAssignedToAP);
            return isLockerAssignedToAP;
        }

        /// <summary>
        /// Gets the Locker id exists in the assigned range
        /// </summary>
        /// <param name="lockerId">integer type parameter</param>
        /// <param name="accessPointId">integer type parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns LockerAccessPointDTO</returns>
        public LockerAccessPointDTO GetLockerIdInclussiveAPDetails(int lockerId, int accessPointId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lockerId, accessPointId, sqlTransaction);
            LockerAccessPointDataHandler lockerAccessPointDataHandler = new LockerAccessPointDataHandler(sqlTransaction);
            LockerAccessPointDTO lockerAccessPointDTO = lockerAccessPointDataHandler.GetLockerAccessPoint(accessPointId);
            log.LogMethodExit(lockerAccessPointDTO);
            return lockerAccessPointDTO;


        }

        /// <summary>
        /// Saves the Access Point record
        /// Checks if the schedule id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LockerAccessPointDataHandler lockerAccessPointDataHandler = new LockerAccessPointDataHandler(sqlTransaction);
            if (accessPointDTO.AccessPointId > -1 && 
                           accessPointDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (accessPointDTO.AccessPointId < 0)
            {
                accessPointDTO = lockerAccessPointDataHandler.InsertLockerAccessPoint(accessPointDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                accessPointDTO.AcceptChanges();
            }
            else if (accessPointDTO.IsChanged)
            {
                accessPointDTO = lockerAccessPointDataHandler.UpdateLockerAccessPoint(accessPointDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                accessPointDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Validate the accessPointDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            string[] ipValidate;
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (string.IsNullOrEmpty(accessPointDTO.Name))
            {
                log.Debug("Please enter AP name.");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter AP name.", MessageContainerList.GetMessage(executionContext, "Locker Access Point") , MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            if (string.IsNullOrEmpty(accessPointDTO.IPAddress))
            {
                log.Debug("Please enter the IP address.");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter the IP address.", MessageContainerList.GetMessage(executionContext, "Locker Access Point"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            ipValidate = accessPointDTO.IPAddress.Split('.');
            if (ipValidate.Length != 4)
            {
                log.Debug("Please enter the IP address.");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter the valid IP address.", MessageContainerList.GetMessage(executionContext, "Locker Access Point"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            if (accessPointDTO.LockerIDFrom == -1)
            {
                log.Debug("Please enter the locker starting range.");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter the locker starting range.", MessageContainerList.GetMessage(executionContext, "Locker Access Point"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            if (accessPointDTO.LockerIDTo == -1)
            {
                log.Debug("Please enter the locker ending range.");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter the locker ending range.", MessageContainerList.GetMessage(executionContext, "Locker Access Point"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            if (IsLockerAssignedToAP())
            {
                log.Debug("Lockers range you are entered is already exists.");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Lockers range you are entered is already exists.", MessageContainerList.GetMessage(executionContext, "Locker Access Point"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LockerAccessPointDTO getAccessPointDTO { get { return accessPointDTO; } }
    }

    /// <summary>
    /// Manages the list of lockerAccessPoints
    /// </summary>
    public class LockerAccessPointList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LockerAccessPointDTO> lockerAccessPointDTOList = new List<LockerAccessPointDTO>();
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LockerAccessPointList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="lockerAccessPointDTOList">lockerAccessPointDTOList</param>
        public LockerAccessPointList(ExecutionContext executionContext, List<LockerAccessPointDTO> lockerAccessPointDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, lockerAccessPointDTOList);
            this.lockerAccessPointDTOList = lockerAccessPointDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Returns the lockerAccessPoint list 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerAccessPointDTO> GetAllLockerAccessPoint(List<KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LockerAccessPointDataHandler lockerAccessPointDataHandler = new LockerAccessPointDataHandler(sqlTransaction);
            List<LockerAccessPointDTO> lockerAccessPointDTOList = lockerAccessPointDataHandler.GetLockerAccessPointList(searchParameters);
            log.LogMethodExit(lockerAccessPointDTOList);
            return lockerAccessPointDTOList;
        }

        /// <summary>
        /// Saves the  list of lockerAccessPointDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (lockerAccessPointDTOList == null ||
                lockerAccessPointDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < lockerAccessPointDTOList.Count; i++)
            {
                var lockerAccessPointDTO = lockerAccessPointDTOList[i];
                if (lockerAccessPointDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    LockerAccessPoint lockerAccessPoint = new LockerAccessPoint(executionContext, lockerAccessPointDTO);
                    lockerAccessPoint.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving lockerAccessPointDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("lockerAccessPointDTO", lockerAccessPointDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
