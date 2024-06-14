/********************************************************************************************
 * Project Name - Game
 * Description  - LocalMachineInputUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 2.110.0      10-Dec-2020      Prajwal S                  Created : POS UI Redesign with REST API
 *2.110.0     05-Feb-2021      Fiona                      Modified to get Input Devices Count
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public class LocalMachineInputDevicesUseCases : IMachineInputDevicesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMachineInputDevicesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MachineInputDevicesDTO>> GetMachineInputDevices(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<MachineInputDevicesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                MachineInputDevicesList machineInputDevicesListBL = new MachineInputDevicesList(executionContext);
                List<MachineInputDevicesDTO> machineInputDevicesDTOList = machineInputDevicesListBL.GetAllMachineInputDevicesDTOList(searchParameters, currentPage, pageSize, sqlTransaction);

                log.LogMethodExit(machineInputDevicesDTOList);
                return machineInputDevicesDTOList;
            });
        }

        public async Task<int> GetMachineInputDevicesCount(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                MachineInputDevicesList machineInputDevicesListBL = new MachineInputDevicesList(executionContext);
                int count = machineInputDevicesListBL.GetMachineInputDevicesCount(searchParameters, sqlTransaction);
                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<string> SaveMachineInputDevices(List<MachineInputDevicesDTO> machineInputDevicesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(machineInputDevicesDTOList);
                    if (machineInputDevicesDTOList == null)
                    {
                        throw new ValidationException("MachineInputDeviceDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (MachineInputDevicesDTO machineInputDevicesDTO in machineInputDevicesDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                MachineInputDevices machineInputDevices = new MachineInputDevices(machineInputDevicesDTO, executionContext);
                                machineInputDevices.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw valEx;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw new Exception(ex.Message, ex);
                            }
                        }
                    }

                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }


    }
}
