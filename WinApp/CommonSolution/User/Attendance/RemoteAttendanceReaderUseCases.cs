/********************************************************************************************
 * Project Name - User
 * Description  - RemoteAttendanceReaderUseCases class to get the data  from API by doing remote call  
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
    class RemoteAttendanceReaderUseCases : RemoteUseCases, IAttendanceReaderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string AttendanceReader_URL = "api/HR/AttendanceReaders";
        private const string AttendanceReader_COUNT_URL = "api/HR/AttendanceReaderCount";

        public RemoteAttendanceReaderUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<AttendanceReaderDTO>> GetAttendanceReader(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>>
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
                List<AttendanceReaderDTO> result = await Get<List<AttendanceReaderDTO>>(AttendanceReader_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AttendanceReaderDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AttendanceReaderDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttendanceReaderDTO.SearchByParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("id".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttendanceReaderDTO.SearchByParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("name".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttendanceReaderDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttendanceReaderDTO.SearchByParameters.MACHINE_NUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttendanceReaderDTO.SearchByParameters.TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("type".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttendanceReaderDTO.SearchByParameters.IP_ADDRESS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ipAddress".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<AttendanceReaderDTO>> SaveAttendanceReader(List<AttendanceReaderDTO> attendanceReaderDTOList)
        {
            log.LogMethodEntry(attendanceReaderDTOList);
            try
            {
                List < AttendanceReaderDTO > responseString = await Post<List<AttendanceReaderDTO>>(AttendanceReader_URL, attendanceReaderDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetAttendanceReaderCount(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>>
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
                int result = await Get<int>(AttendanceReader_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<string> DeleteAttendanceReader(List<AttendanceReaderDTO> AttendanceReaderDTOList)
        {
            try
            {
                log.LogMethodEntry(AttendanceReaderDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(AttendanceReaderDTOList);
                string responseString = await Delete(AttendanceReader_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }
        }
    }
}