/********************************************************************************************
 * Project Name - Communication
 * Description  - BL of PushNotificationDevice Entity
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0   15-Sep-2020   Nitin Pai               Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Communication
{
    public class PushNotificationDeviceBL
    {
        private PushNotificationDeviceDTO pushNotificationDeviceDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PushNotificationDeviceBL class
        /// </summary>
        private PushNotificationDeviceBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the PushNotificationDevice id as the parameter
        /// Would fetch the PushNotificationDevice object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public PushNotificationDeviceBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            PushNotificationDeviceDataHandler PushNotificationDeviceDataHandler = new PushNotificationDeviceDataHandler(sqlTransaction);
            pushNotificationDeviceDTO = PushNotificationDeviceDataHandler.GetPushNotificationDeviceDTO(id);
            if (pushNotificationDeviceDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PushNotificationDevice", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the PushNotificationDevice id as the parameter
        /// Would fetch the PushNotificationDevice object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public PushNotificationDeviceBL(ExecutionContext executionContext, string token, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, token, sqlTransaction);
            PushNotificationDeviceDataHandler PushNotificationDeviceDataHandler = new PushNotificationDeviceDataHandler(sqlTransaction);
            pushNotificationDeviceDTO = PushNotificationDeviceDataHandler.GetPushNotificationDeviceDTO(token);
            if (pushNotificationDeviceDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PushNotificationDevice", token);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates PushNotificationDeviceBL object using the PushNotificationDeviceDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="PushNotificationDeviceDTO">PushNotificationDeviceDTO object</param>
        public PushNotificationDeviceBL(ExecutionContext executionContext, PushNotificationDeviceDTO pushNotificationDeviceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, pushNotificationDeviceDTO);
            this.pushNotificationDeviceDTO = pushNotificationDeviceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PushNotificationDevice
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            PushNotificationDeviceDataHandler PushNotificationDeviceDataHandler = new PushNotificationDeviceDataHandler(sqlTransaction);
            if (pushNotificationDeviceDTO.Id < 0)
            {
                pushNotificationDeviceDTO = PushNotificationDeviceDataHandler.InsertPushNotificationDevice(pushNotificationDeviceDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                pushNotificationDeviceDTO.LastUpdateDate = DateTime.Now;
                pushNotificationDeviceDTO.AcceptChanges();
            }
            else
            {
                if (pushNotificationDeviceDTO.IsChanged)
                {
                    pushNotificationDeviceDTO = PushNotificationDeviceDataHandler.UpdatePushNotificationDevice(pushNotificationDeviceDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    pushNotificationDeviceDTO.LastUpdateDate = DateTime.Now;
                    pushNotificationDeviceDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PushNotificationDeviceDTO PushNotificationDeviceDTO
        {
            get
            {
                return pushNotificationDeviceDTO;
            }
        }

        /// <summary>
        /// Validates the PushNotificationDevice, throws ValidationException if any fields are not valid
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    public class PushNotificationDeviceListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PushNotificationDeviceDTO> pushNotificationDeviceDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public PushNotificationDeviceListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public PushNotificationDeviceListBL(ExecutionContext executionContext, List<PushNotificationDeviceDTO> pushNotificationDeviceDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.pushNotificationDeviceDTOList = pushNotificationDeviceDTOList;
            log.LogMethodExit();
        }

        public void SavePushNotificationDeviceList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (this.pushNotificationDeviceDTOList != null)
            {
                foreach(PushNotificationDeviceDTO pushNotificationDeviceDTO in pushNotificationDeviceDTOList)
                {
                    PushNotificationDeviceBL pushNotificationDeviceBL = new PushNotificationDeviceBL(executionContext, pushNotificationDeviceDTO);
                    pushNotificationDeviceBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PushNotificationDevice list
        /// </summary>
        public List<PushNotificationDeviceDTO> GetPushNotificationDeviceDTOList(List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PushNotificationDeviceDataHandler pushNotificationDeviceDataHandler = new PushNotificationDeviceDataHandler(sqlTransaction);
            List<PushNotificationDeviceDTO> returnValue = pushNotificationDeviceDataHandler.GetPushNotificationDeviceDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

    }
}
