/********************************************************************************************
 * Project Name - Reports
 * Description  - Data Handler of GameServerReportBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        23-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParafaitServer
{
    public class GameServerReportDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;

        public GameServerReportDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }



        public List<MachineDTO> GetMachinesDetails(ExecutionContext executionContext, SqlTransaction sqlTransaction)
        {

            log.LogMethodEntry();
            List<MachineDTO> machineDTOList = null;
            try
            {
                string query = @"select * from masters 
                              where (port_number is not null or IPAddress is not null or DirectMode = 'Y' or MacAddress is not null)
                                and ((ServerMachine is null) 
                                    or upper(ServerMachine) = upper(@ThisMachine)) 
                                and active_flag = 'Y' 
                                and exists (select 1 
                                            from Machines m 
                                            where m.master_id = masters.master_id 
                                            and m.active_flag = 'Y')
                              order by master_name";

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                machineDTOList = new List<MachineDTO>();
                sqlParameters.Add(new SqlParameter("@ThisMachine", "MLR-LT001"));// executionContext.POSMachineName));
                DataTable MasterDataTable = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);
                for (int i = 0; i < MasterDataTable.Rows.Count; i++)
                {
                    List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MASTER_ID, MasterDataTable.Rows[i]["master_id"].ToString()));
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "Y"));
                    MachineList machines = new MachineList(executionContext);
                    List<MachineDTO> machinesDTOList = machines.GetMachineList(searchParameters,false,sqlTransaction);
                    if (machinesDTOList != null && machinesDTOList.Any())
                    {
                        foreach (MachineDTO machineDTO in machinesDTOList)
                        {
                            MachineCommunicationLogList machineCommunicationLogList = new MachineCommunicationLogList(executionContext);
                            List<KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>>();
                            searchParams.Add(new KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>(MachineCommunicationLogDTO.SearchByParameters.MACHINE_ID, machineDTO.MachineId.ToString()));
                            List<MachineCommunicationLogDTO> machineCommunicationLogDTOList = machineCommunicationLogList.GetMachineCommunicationLogDTOList(searchParams);
                            if (machineCommunicationLogDTOList != null && machineCommunicationLogDTOList.Count > 0)
                            {
                                machineDTO.MachineCommunicationLogDTO = machineCommunicationLogDTOList[0];
                            }
                            machineDTOList.Add(machineDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

    }
}
