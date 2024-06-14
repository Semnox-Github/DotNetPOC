/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteMachineInputDevicesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 2.110.0      10-Dec-2020      Prajwal S                  Created : POS UI Redesign with REST API
 *2.110.0     05-Feb-2021     Fiona                       Modified to get Input Devices Count
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    class RemoteMachineInputDevicesUseCases : RemoteUseCases, IMachineInputDevicesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MachineInputDevices_URL = "api/Inventory/MachineInputDevicess";
        private const string MachineInputDevices_COUNT_URL = "api/Inventory/MachineInputDevicessCount";
        public RemoteMachineInputDevicesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<MachineInputDevicesDTO>> GetMachineInputDevices(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> parameter, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameter);
            MachineInputDevicesList machineInputDevicesList = new MachineInputDevicesList(executionContext);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameter != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameter));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<MachineInputDevicesDTO> result = await Get<List<MachineInputDevicesDTO>>(MachineInputDevices_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MachineInputDevicesDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineInputDevicesDTO.SearchByParameters.DEVICE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("deviceId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineInputDevicesDTO.SearchByParameters.DEVICE_MODEL_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("deviceModelId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineInputDevicesDTO.SearchByParameters.DEVICE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("deviceName".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineInputDevicesDTO.SearchByParameters.DEVICE_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("deviceTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineInputDevicesDTO.SearchByParameters.MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineInputDevicesDTO.SearchByParameters.MAC_ADDRESS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("macAddress".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineInputDevicesDTO.SearchByParameters.IP_ADDRESS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ipAddress".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveMachineInputDevices(List<MachineInputDevicesDTO> machineInputDevicesDTOList)
        {
            log.LogMethodEntry(machineInputDevicesDTOList);
            try
            {
                string responseString = await Post<string>(MachineInputDevices_URL, machineInputDevicesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetMachineInputDevicesCount(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                int result = await Get<int>(MachineInputDevices_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
