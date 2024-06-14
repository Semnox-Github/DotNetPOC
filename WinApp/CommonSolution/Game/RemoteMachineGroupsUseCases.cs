/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteMachineGroupUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      09-Dec-2020      Prajwal S                 Created : POS UI Redesign with REST API
 2.110.0      05-Feb-2021     Fiona                 Modified for pagination and to Get Machine Groups Count
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    public class RemoteMachineGroupsUseCases : RemoteUseCases, IMachineGroupsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MachineGroups_URL = "api/Games/MachineGroups";
        private const string MachineGroups_COUNT_URL = "api/Game/MachineGroupsCount";

        public RemoteMachineGroupsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

       

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MachineGroupsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MachineGroupsDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineGroupsDTO.SearchByParameters.MACHINE_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineGroupsId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineGroupsDTO.SearchByParameters.GROUP_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("groupsName".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveMachineGroups(List<MachineGroupsDTO> machineGroupsDTOList)
        {
            log.LogMethodEntry(machineGroupsDTOList);
            try
            {
                string responseString = await Post<string>(MachineGroups_URL, machineGroupsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetMachineGroupsCount(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                int result = await Get<int>(MachineGroups_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<MachineGroupsDTO>> GetMachineGroups(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, bool buildChildRecords = false, bool loadActiveChild = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadActiveChild, buildChildRecords, currentPage, pageSize, sqlTransaction);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), buildChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<MachineGroupsDTO> result = await Get<List<MachineGroupsDTO>>(MachineGroups_URL, searchParameterList);
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
