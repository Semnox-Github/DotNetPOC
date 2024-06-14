/********************************************************************************************
 * Project Name - Device
 * Description  - Bussiness Logic file for Peripherals
 * 
 **************
 **Version Log
 **************
 *Version       Date            Modified By         Remarks          
 *********************************************************************************************
 *2.50        23-Jan-2019    Jagan Mohana Rao    Added new constructor PeripheralsListBL()
 *                                               added new method SaveUpdatePeripheralsList()
 *2.70         29-June-2019     Indrajeet Kumar     Created DeletePeripheralsList() & DeletePeripherals() - For Implementation of Hard Deletion
 *                                                  Modified Default constructor to Paramterized Constructor
 *2.70.2       09-Jul-2019       Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                            LogMethodEntry() and LogMethodExit(). 
 *2.80         09-Mar-2020      Vikas Dwivedi       Modified as per the standards for Phase 1 changes.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.Device.Peripherals
{
    /// <summary>
    /// Bussiness Logic of Peripherals class.
    /// </summary>
    public class PeripheralsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private PeripheralsDTO peripheralsDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor of PeripheralsBL
        /// </summary>
        private PeripheralsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Peripheral object using PeripheralDTO object
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="peripheralsDTO">PeripheralsDTO object</param>
        public PeripheralsBL(ExecutionContext executionContext, PeripheralsDTO peripheralsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(peripheralsDTO, executionContext);
            this.peripheralsDTO = peripheralsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Peripherals id as the parameter
        /// Would fetch the peripherals object from the database based on the id passed.
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="deviceId">id - Peripherals</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PeripheralsBL(ExecutionContext executionContext, int deviceId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, deviceId, sqlTransaction);
            PeripheralsDatahandler peripheralsDatahandler = new PeripheralsDatahandler(sqlTransaction);
            this.peripheralsDTO = peripheralsDatahandler.GetPeripheralsDTO(deviceId);
            if (peripheralsDTO.DeviceId < 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Peripherals", deviceId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the PeripheralsDTO object
        /// </summary>
        public PeripheralsDTO GePeripheralsDTO
        {
            get { return peripheralsDTO; }
        }
        /// <summary>
        ///    Saves the PeripheralsDTO
        /// Checks if the deviceId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (peripheralsDTO.IsChanged == false &&
                peripheralsDTO.DeviceId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            PeripheralsDatahandler peripheralsDataHandler = new PeripheralsDatahandler(sqlTransaction);

            if (peripheralsDTO.DeviceId < 0)
            {
                log.LogVariableState("PeripheralsDTO", peripheralsDTO);
                peripheralsDTO = peripheralsDataHandler.InsertPeripheralsDTO(peripheralsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                peripheralsDTO.AcceptChanges();
                if (!string.IsNullOrEmpty(peripheralsDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("Peripherals", peripheralsDTO.Guid);
                }
            }
            else if (peripheralsDTO.IsChanged)
            {
                log.LogVariableState("PeripheralsDTO", peripheralsDTO);
                peripheralsDTO = peripheralsDataHandler.UpdatePeripheralsDTO(peripheralsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                peripheralsDTO.AcceptChanges();
                if (!string.IsNullOrEmpty(peripheralsDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("Peripherals", peripheralsDTO.Guid);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the PeripheralsDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(peripheralsDTO.DeviceId.ToString()))
            {
                validationErrorList.Add(new ValidationError("Peripherals", "DeviceId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Device Id"))));
            }

            if (!string.IsNullOrWhiteSpace(peripheralsDTO.DeviceName) && peripheralsDTO.DeviceName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("Peripherals", "DeviceName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Device Name"), 50)));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of PeripheralsDTO
    /// </summary>
    public class PeripheralsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PeripheralsDTO> peripheralsList = new List<PeripheralsDTO>();
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// default Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public PeripheralsListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public PeripheralsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="peripheralsList"></param>
        /// <param name="executionContext"></param>
        public PeripheralsListBL(ExecutionContext executionContext, List<PeripheralsDTO> peripheralsList)
            : this(executionContext)
        {
            log.LogMethodEntry(peripheralsList, executionContext);
            this.peripheralsList = peripheralsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the GetPeripheralsDTOList list
        /// </summary>       
        public List<PeripheralsDTO> GetPeripheralsDTOList(List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PeripheralsDatahandler peripheralsDatahandler = new PeripheralsDatahandler(sqlTransaction);
            List<PeripheralsDTO> peripheralsDTOList = peripheralsDatahandler.GetPeripheralsDTOList(searchParameters);
            log.LogMethodExit(peripheralsDTOList);
            return peripheralsDTOList;
        }

        /// <summary>
        /// Takes LookupParams as parameter
        /// </summary>
        /// <returns>Returns KeyValuePair List of PeripheralsDTO.SearchByParameters by converting PeripheralsDTO</returns>
        public List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> BuildPeripheralsDTOSearchParametersList(PeripheralsDTO peripheralsDTO)
        {
            log.LogMethodEntry(peripheralsDTO);
            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> peripheralsDTOSearchParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
            if (peripheralsDTO != null)
            {
                if (peripheralsDTO.DeviceId >= 0)
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_ID, peripheralsDTO.DeviceId.ToString()));

                if (!(string.IsNullOrEmpty(peripheralsDTO.DeviceName)))
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_NAME, peripheralsDTO.DeviceName.ToString()));

                if (peripheralsDTO.PosMachineId >= 0)
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, peripheralsDTO.PosMachineId.ToString()));

                if (!(string.IsNullOrEmpty(peripheralsDTO.DeviceType)))
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, peripheralsDTO.DeviceType.ToString()));

                if (!(string.IsNullOrEmpty(peripheralsDTO.DeviceSubType)))
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_SUBTYPE, peripheralsDTO.DeviceSubType.ToString()));

                if (!(string.IsNullOrEmpty(peripheralsDTO.Vid)))
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.VID, peripheralsDTO.Vid.ToString()));

                if (!(string.IsNullOrEmpty(peripheralsDTO.Pid)))
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.PID, peripheralsDTO.Pid.ToString()));

                if (peripheralsDTO.SiteId >= 0)
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.SITE_ID, peripheralsDTO.SiteId.ToString()));

                if (!(string.IsNullOrEmpty(peripheralsDTO.Guid)))
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.GUID, peripheralsDTO.Guid.ToString()));

                peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, peripheralsDTO.Active.ToString()));

                if (peripheralsDTO.MasterEntityId >= 0)
                    peripheralsDTOSearchParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.MASTER_ENTITY_ID, peripheralsDTO.MasterEntityId.ToString()));

            }

            log.LogMethodExit(peripheralsDTOSearchParams);
            return peripheralsDTOSearchParams;
        }

        /// <summary>
        /// GetPeripheralsDTOsList(PeripheralsDTO peripheralsDTO) method search based on peripheralsDTO
        /// </summary>
        /// <param name="peripheralsDTO"></param>
        /// <returns>List of PeripheralsDTO object</returns>
        public List<PeripheralsDTO> GetPeripheralsDTOList(PeripheralsDTO peripheralsDTO)
        {
            try
            {
                log.LogMethodEntry(peripheralsDTO);
                List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchParameters = BuildPeripheralsDTOSearchParametersList(peripheralsDTO);
                PeripheralsDatahandler peripheralsDataHandler = new PeripheralsDatahandler();
                List<PeripheralsDTO> peripheralsDTOList = peripheralsDataHandler.GetPeripheralsDTOList(searchParameters);
                log.LogMethodExit(peripheralsDTOList);
                return peripheralsDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetPeripheralsDTOList(PeripheralsDTO peripheralsDTO) ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the PeripheralsBL List
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (peripheralsList == null ||
               peripheralsList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < peripheralsList.Count; i++)
            {
                var peripheralsDTO = peripheralsList[i];
                if (peripheralsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    PeripheralsBL peripheralsBL = new PeripheralsBL(executionContext, peripheralsDTO);
                    peripheralsBL.Save(sqlTransaction);
                }

                catch (Exception ex)
                {
                    log.Error("Error occurred while saving PeripheralsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("PeripheralsDTO", peripheralsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PeripheralsDTO List for POSMachineIdList
        /// </summary>
        /// <param name="pOSMachineIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of PeripheralsDTO</returns>
        public List<PeripheralsDTO> GetPeripheralsDTOList(List<int> pOSMachineIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSMachineIdList, activeRecords, sqlTransaction);
            PeripheralsDatahandler peripheralsDataHandler = new PeripheralsDatahandler(sqlTransaction);
            List<PeripheralsDTO> PeripheralsDTOList = peripheralsDataHandler.GetPeripheralsDTOList(pOSMachineIdList, activeRecords);
            log.LogMethodExit(PeripheralsDTOList);
            return PeripheralsDTOList;
        }



    }
}
