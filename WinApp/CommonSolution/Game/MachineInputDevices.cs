/********************************************************************************************
 * Project Name - MachineInputDevices BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 ********************************************************************************************* 
 *2.70.2        29-Jul-2019      Deeksha             Modified :Save method return DTO instead of ID. 
 *2.110.0     22-Dec-2020      Prajwal S           Added Constructor with ExecutionContext parameter, modified Constructor with Id parameter.
 *                                                  Added Get using search parameters.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    public class MachineInputDevices
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MachineInputDevicesDTO machineInputDevicesDTO;
        private SqlTransaction sqlTransaction;
        private ExecutionContext executionContext;
        int insertedMachineId = 0;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="machineInputdevicedto">machineInputdevicedto</param>
        /// <param name="executionContext">executionContext</param>
        public MachineInputDevices(MachineInputDevicesDTO machineInputdevicedto, ExecutionContext executionContext)
               : this(machineInputdevicedto)
        {
            log.LogMethodEntry(machineInputdevicedto, executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        private MachineInputDevices(ExecutionContext executionContext) //added constructor
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the MachineInputDevices parameter
        /// </summary>
        /// <param name="deviceId">deviceId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MachineInputDevices(int deviceId,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(deviceId, sqlTransaction);
            this.executionContext = ExecutionContext.GetExecutionContext();
            MachineInputDevicesDataHandler machineInputDevicesDataHandler = new MachineInputDevicesDataHandler(sqlTransaction);
            this.machineInputDevicesDTO = machineInputDevicesDataHandler.GetMachineInputDeviceDTO(deviceId);
            if (machineInputDevicesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "MachineInputDevicesDTO", deviceId);    //added condition
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the MachineInputDevicesDTO as parameter
        /// </summary>
        /// <param name="machineInputDevicesDTO">MachineInputDevices</param>
        public MachineInputDevices(MachineInputDevicesDTO machineInputDevicesDTO)
        {
            log.LogMethodEntry(machineInputDevicesDTO);
            this.executionContext = ExecutionContext.GetExecutionContext();
            this.machineInputDevicesDTO = machineInputDevicesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get methos for MachineInputDevicesDTO Object
        /// </summary>
        public MachineInputDevicesDTO GetMachineInputDevicesDTO
        {
            get { return machineInputDevicesDTO; }
        }

        /// <summary>
        /// Saves the MachineInputDevicesDTO
        /// Checks if the deviceId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public int Save(SqlTransaction sqlTransaction=null)
        {

            int deviceId = 0;
            try
            {
                log.LogMethodEntry(sqlTransaction);
                MachineInputDevicesDataHandler machineInputDevicesDataHandler = new MachineInputDevicesDataHandler(sqlTransaction);

                if (machineInputDevicesDTO.DeviceId < 0)
                {
                    machineInputDevicesDTO = machineInputDevicesDataHandler.InsertMachineInputDevice(machineInputDevicesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    insertedMachineId = machineInputDevicesDTO.DeviceId;
                    machineInputDevicesDTO.AcceptChanges();
                }
                else
                {
                    if (machineInputDevicesDTO.IsChanged )
                    {
                        machineInputDevicesDTO = machineInputDevicesDataHandler.UpdateMachineInputDevice(machineInputDevicesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        machineInputDevicesDTO.AcceptChanges();
                    }
                }
                log.LogMethodExit(insertedMachineId);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            return insertedMachineId;   // Return insert/update no of rows effected -- 08-Nov-2018 Jagan Mohana Rao
        }
    }

    /// <summary>
    /// Manages the list of MachineInputDevicesDTO
    /// </summary>
    public class MachineInputDevicesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<MachineInputDevicesDTO> machineInputDevicesDTOList;
        public MachineInputDevicesList()
        {
            log.LogMethodEntry();
            this.machineInputDevicesDTOList = null;
            this.executionContext = ExecutionContext.GetExecutionContext();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructors for the method MachineInputDevicesList()
        /// </summary>
        /// <param name="inputlistdto">inputlistdto</param>
        /// <param name="executioncontext">executioncontext</param>
        public MachineInputDevicesList(List<MachineInputDevicesDTO> machineInputDevicesDTOList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(machineInputDevicesDTOList, executionContext);
            this.executionContext = executionContext;
            this.machineInputDevicesDTOList = machineInputDevicesDTOList;
            log.LogMethodExit();
        }
        public MachineInputDevicesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves Machine Inout devices List
        /// </summary>
        /// <returns></returns>
        public void SaveMachineInputDevicesList()
        {
            int sucesscount = 0;
            try
            {
                log.LogMethodEntry();
                foreach (MachineInputDevicesDTO machineInputDevicesDTO in machineInputDevicesDTOList)
                {
                    MachineInputDevices machineInputDevices = new MachineInputDevices(machineInputDevicesDTO, executionContext);
                    if (machineInputDevicesDTO.DeviceTypeId < 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1201)); /// Please Enter Device Type.
                    }
                    else if (machineInputDevicesDTO.DeviceModelId < 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1202));  /// Please Enter Device mode.
                    }
                    sucesscount = machineInputDevices.Save();

                    log.Debug(string.Format("MachineInputDevices  {0} has been stored from SaveMachineInputDevicesList() ", machineInputDevicesDTO.DeviceId.ToString()));
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            } 
        }


        // <summary>
        ///Takes machineInputDeviceSearchParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> by converting MachineInputDevicesDTO</returns>
        public List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> BuildSearchParametersList(MachineInputDeviceSearchParams machineInputDeviceSearchParams)
        {
            log.LogMethodEntry(machineInputDeviceSearchParams);
            List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> MachineInputDeviceDTOSearchParams = new List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>>();

            if (machineInputDeviceSearchParams != null)
            {
                if (machineInputDeviceSearchParams.DeviceId > 0)
                    MachineInputDeviceDTOSearchParams.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.DEVICE_ID, machineInputDeviceSearchParams.DeviceId.ToString()));

                if (machineInputDeviceSearchParams.DeviceTypeId > 0)
                    MachineInputDeviceDTOSearchParams.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.DEVICE_TYPE_ID, machineInputDeviceSearchParams.DeviceTypeId.ToString()));

                if (machineInputDeviceSearchParams.DeviceModelId > 0)
                    MachineInputDeviceDTOSearchParams.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.DEVICE_MODEL_ID, machineInputDeviceSearchParams.DeviceModelId.ToString()));

                if (machineInputDeviceSearchParams.MachineId > 0)
                    MachineInputDeviceDTOSearchParams.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.MACHINE_ID, machineInputDeviceSearchParams.MachineId.ToString()));

                if (machineInputDeviceSearchParams.IsActive)
                    MachineInputDeviceDTOSearchParams.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.IS_ACTIVE, "1"));

                if (!string.IsNullOrEmpty(machineInputDeviceSearchParams.DeviceName))
                    MachineInputDeviceDTOSearchParams.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.DEVICE_NAME, machineInputDeviceSearchParams.DeviceName));

                if (!string.IsNullOrEmpty(machineInputDeviceSearchParams.IPAddress))
                    MachineInputDeviceDTOSearchParams.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.IP_ADDRESS, machineInputDeviceSearchParams.IPAddress));

                if (!string.IsNullOrEmpty(machineInputDeviceSearchParams.MacAddress))
                    MachineInputDeviceDTOSearchParams.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.MAC_ADDRESS, machineInputDeviceSearchParams.MacAddress));

                if (machineInputDeviceSearchParams.SiteId > 0)
                    MachineInputDeviceDTOSearchParams.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.SITE_ID, machineInputDeviceSearchParams.SiteId.ToString()));
            }
            log.LogMethodExit(MachineInputDeviceDTOSearchParams);

            return MachineInputDeviceDTOSearchParams;
        }

        /// <summary>
        /// GetAllMachineInputDevicesList method search based on machineInputDeviceSearchParams
        /// </summary>
        /// <param name="machineInputDeviceSearchParams">machineInputDeviceSearchParams</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
      //  public List<MachineInputDevicesDTO> GetAllMachineInputDevicesList(MachineInputDeviceSearchParams machineInputDeviceSearchParams,SqlTransaction sqlTransaction=null)
        public List<MachineInputDevicesDTO> GetAllMachineInputDevicesList(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);
                MachineInputDevicesDataHandler machineInputDevicesDataHandler = new MachineInputDevicesDataHandler(sqlTransaction);
                List<MachineInputDevicesDTO> machineInputDevicesDTOs = new List<MachineInputDevicesDTO>();
                machineInputDevicesDTOs = machineInputDevicesDataHandler.GetMachineInputDevicesDTOsList(searchParameters);
                log.LogMethodExit(machineInputDevicesDTOs);
                return machineInputDevicesDTOs;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        public List<MachineInputDevicesDTO> GetAllMachineInputDevicesDTOList(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);
                MachineInputDevicesDataHandler machineInputDevicesDataHandler = new MachineInputDevicesDataHandler(sqlTransaction);
                List<MachineInputDevicesDTO> machineInputDevicesDTOs = new List<MachineInputDevicesDTO>();
                machineInputDevicesDTOs = machineInputDevicesDataHandler.GetMachineInputDevicesLists(searchParameters, currentPage, pageSize);
                log.LogMethodExit(machineInputDevicesDTOs);
                return machineInputDevicesDTOs;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        public int GetMachineInputDevicesCount(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MachineInputDevicesDataHandler machineInputDevicesDataHandler = new MachineInputDevicesDataHandler(sqlTransaction);
            int machineInputDevicesCount = machineInputDevicesDataHandler.GetMachineInputDevicessCount(searchParameters);
            log.LogMethodExit(machineInputDevicesCount);
            return machineInputDevicesCount;
        }

    }
}
