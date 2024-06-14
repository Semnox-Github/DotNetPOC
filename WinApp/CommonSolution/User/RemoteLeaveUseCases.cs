/********************************************************************************************
 * Project Name - User
 * Description  - RemoteLeaveUseCases class to get the data  from API by doing remote call  
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
    class RemoteLeaveUseCases : RemoteUseCases, ILeaveUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string Leave_URL = "api/HR/Leaves";
        private const string Leave_COUNT_URL = "api/HR/LeaveCount";

        public RemoteLeaveUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<LeaveDTO>> GetLeave(List<KeyValuePair<LeaveDTO.SearchByParameters, string>>
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
                List<LeaveDTO> result = await Get<List<LeaveDTO>>(Leave_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<LeaveDTO>> GenerateLeave(int leaveCycleId = -1)
        {
            log.LogMethodEntry(leaveCycleId);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("leaveCycleId".ToString(), leaveCycleId.ToString()));
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<LeaveDTO> result = await Get<List<LeaveDTO>>(Leave_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<LeaveDTO>> PopulateInbox(int userId = -1)
        {
            log.LogMethodEntry(userId);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("userId".ToString(), userId.ToString()));
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<LeaveDTO> result = await Get<List<LeaveDTO>>(Leave_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<LeaveDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<LeaveDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case LeaveDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveDTO.SearchByParameters.LEAVE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("leaveId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveDTO.SearchByParameters.LEAVE_TEMPLATE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("leaveTemplateId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveDTO.SearchByParameters.LEAVE_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("leaveTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveDTO.SearchByParameters.TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("type".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveDTO.SearchByParameters.USER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("userId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveDTO.SearchByParameters.END_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("endDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveDTO.SearchByParameters.LEAVE_CYCLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("leaveCycleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveDTO.SearchByParameters.APPROVED_BY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("approvedBy".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<LeaveDTO>> SaveLeave(List<LeaveDTO> LeaveDTOList)
        {
            log.LogMethodEntry(LeaveDTOList);
            try
            {
                List<LeaveDTO> responseString = await Post<List<LeaveDTO>>(Leave_URL, LeaveDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetLeaveCount(List<KeyValuePair<LeaveDTO.SearchByParameters, string>>
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
                int result = await Get<int>(Leave_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<string> DeleteLeave(List<LeaveDTO> LeaveDTOList)
        {
            try
            {
                log.LogMethodEntry(LeaveDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(LeaveDTOList);
                string responseString = await Delete(Leave_URL, content);
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