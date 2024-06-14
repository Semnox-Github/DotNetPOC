/********************************************************************************************
 * Project Name - User
 * Description  - RemoteShiftConfigurationsUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version      Date              Modified By             Remarks          
 *********************************************************************************************
 2.120.0       31-Mar-2021       Prajwal S               Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    class RemoteShiftConfigurationsUseCases : RemoteUseCases, IShiftConfigurationsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ShiftConfigurations_URL = "api/HR/ShiftConfigurations";
        private const string ShiftConfigurations_COUNT_URL = "api/HR/ShiftConfigurationsCount";

        public RemoteShiftConfigurationsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ShiftConfigurationsDTO>> GetShiftConfigurations(List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>>
                          parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<ShiftConfigurationsDTO> result = await Get<List<ShiftConfigurationsDTO>>(ShiftConfigurations_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ShiftConfigurationsDTO.SearchByParameters.SHIFT_CONFIGURATION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftConfigurationId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftConfigurationsDTO.SearchByParameters.SHIFT_CONFIGURATION_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftConfigurationsName".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftConfigurationsDTO.SearchByParameters.SHIFT_TRACK_ALLOWED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftTrackAllowed".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftConfigurationsDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftConfigurationsDTO.SearchByParameters.OVERTIME_ALLOWED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("overTimeAllowed".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<ShiftConfigurationsDTO>> SaveShiftConfigurations(List<ShiftConfigurationsDTO> shiftConfigurationsDTOList)
        {
            log.LogMethodEntry(shiftConfigurationsDTOList);
            try
            {
                List<ShiftConfigurationsDTO> responseString = await Post<List<ShiftConfigurationsDTO>>(ShiftConfigurations_URL, shiftConfigurationsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetShiftConfigurationsCount(List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>>
                         parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                int result = await Get<int>(ShiftConfigurations_COUNT_URL, searchParameterList);
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