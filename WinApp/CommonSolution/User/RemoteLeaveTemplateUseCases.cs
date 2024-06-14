/********************************************************************************************
 * Project Name - User
 * Description  - RemoteLeaveTemplateUseCases class to get the data  from API by doing remote call  
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
    class RemoteLeaveTemplateUseCases : RemoteUseCases, ILeaveTemplateUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string LeaveTemplate_URL = "api/HR/LeaveTemplates";
        private const string LeaveTemplate_COUNT_URL = "api/HR/LeaveTemplateCount";

        public RemoteLeaveTemplateUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<LeaveTemplateDTO>> GetLeaveTemplate(List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>>
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
                List<LeaveTemplateDTO> result = await Get<List<LeaveTemplateDTO>>(LeaveTemplate_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<LeaveTemplateDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case LeaveTemplateDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveTemplateDTO.SearchByParameters.LEAVE_TEMPLATE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("leaveTemplateId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveTemplateDTO.SearchByParameters.LEAVE_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("leaveTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveTemplateDTO.SearchByParameters.DEPARTMENT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("departmentId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LeaveTemplateDTO.SearchByParameters.ROLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("roleId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<LeaveTemplateDTO>> SaveLeaveTemplate(List<LeaveTemplateDTO> leaveTemplateDTOList)
        {
            log.LogMethodEntry(leaveTemplateDTOList);
            try
            {
                List<LeaveTemplateDTO> responseString = await Post<List<LeaveTemplateDTO>>(LeaveTemplate_URL, leaveTemplateDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetLeaveTemplateCount(List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>>
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
                int result = await Get<int>(LeaveTemplate_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<string> DeleteLeaveTemplate(List<LeaveTemplateDTO> leaveTemplateDTOList)
        {
            try
            {
                log.LogMethodEntry(leaveTemplateDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(leaveTemplateDTOList);
                string responseString = await Delete(LeaveTemplate_URL, content);
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