/********************************************************************************************
 * Project Name - User
 * Description  - DepartmentUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    25-Feb-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
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
    public class RemoteDepartmentUseCases : RemoteUseCases, IDepartmentUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DEPARTMENT_URL = "api/HR/Departments";
        public RemoteDepartmentUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<DepartmentDTO>> GetDepartments(List<KeyValuePair<DepartmentDTO.SearchByParameters, string>>
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
                List<DepartmentDTO> result = await Get<List<DepartmentDTO>>(DEPARTMENT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<DepartmentDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case DepartmentDTO.SearchByParameters.DEPARTMENT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("departmentId".ToString(), searchParameter.Value));
                        }
                        break;
                    case DepartmentDTO.SearchByParameters.DEPARTMENT_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("departmentName".ToString(), searchParameter.Value));
                        }
                        break;
                    case DepartmentDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveDepartments(List<DepartmentDTO> departmentDTOList)
        {
            log.LogMethodEntry(departmentDTOList);
            try
            {
                string responseString = await Post<string>(DEPARTMENT_URL, departmentDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        //public async Task<DepartmentContainerDTOCollection> GetDepartmentContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        //{
        //    log.LogMethodEntry(hash, rebuildCache);
        //    List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
        //    parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
        //    if (string.IsNullOrWhiteSpace(hash) == false)
        //    {
        //        parameters.Add(new KeyValuePair<string, string>("hash", hash));
        //    }
        //    parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
        //    DepartmentContainerDTOCollection result = await Get<DepartmentContainerDTOCollection>(DEPARTMENT_CONTAINER_URL, parameters);
        //    log.LogMethodExit(result);
        //    return result;
        //}
        public async Task<string> Delete(List<DepartmentDTO> departmentDTOList)
        {
            try
            {
                log.LogMethodEntry(departmentDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(departmentDTOList);
                string responseString = await Delete(DEPARTMENT_URL, content);
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

