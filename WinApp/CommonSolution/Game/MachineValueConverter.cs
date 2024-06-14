/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Machine Value Converter.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       12-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Game
{
    class MachineValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        Dictionary<int, MachineDTO> machineIdMachineDTODictionary;
        Dictionary<string, MachineDTO> machineNameMachineDTODictionary;

        public MachineValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            machineNameMachineDTODictionary = new Dictionary<string, MachineDTO>();
            machineIdMachineDTODictionary = new Dictionary<int, MachineDTO>();
            List<MachineDTO> machineList = null;
            MachineList machineDTOList = new MachineList(executionContext);
            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchCountryParams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
            searchCountryParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            machineList = machineDTOList.GetMachineList(searchCountryParams, false);
            if (machineList != null && machineList.Count > 0)
            {
                foreach (var machine in machineList)
                {
                    machineIdMachineDTODictionary.Add(machine.MachineId, machine);
                    machineNameMachineDTODictionary.Add(machine.MachineName.ToUpper(), machine);
                }
            }
            log.LogMethodExit();
        }
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int machineId = -1;
            if (machineNameMachineDTODictionary != null && machineNameMachineDTODictionary.ContainsKey(stringValue.ToUpper()))
            {
                machineId = machineNameMachineDTODictionary[stringValue.ToUpper()].MachineId;
            }
            log.LogMethodExit(machineId);
            return machineId;
        }

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string machineName = string.Empty;
            if (machineIdMachineDTODictionary != null && machineIdMachineDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                machineName = machineIdMachineDTODictionary[Convert.ToInt32(value)].MachineName;
            }
            log.LogMethodExit(machineName);
            return machineName;
        }



    }
}
