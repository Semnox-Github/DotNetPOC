/********************************************************************************************
 * Project Name - POS 
 * Description  - RemotePOSTypeUseCases class to get the data  from local DB 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          27-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Semnox.Parafait.POS
{
    public class RemotePOSTypeUseCases : RemoteUseCases, IPOSTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string POS_MACHINE_TYPE_URL = "api/POS/POSMachineTypes";

        public RemotePOSTypeUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> posMachineTypeSearchParams)
        {
            log.LogMethodEntry(posMachineTypeSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<POSTypeDTO.SearchByParameters, string> searchParameter in posMachineTypeSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case POSTypeDTO.SearchByParameters.POS_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSTypeDTO.SearchByParameters.POS_TYPE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posTypeName".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSTypeDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }


        public async Task<List<POSTypeDTO>> GetPOSTypes(List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> parameters, SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
               if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<POSTypeDTO> result = await Get<List<POSTypeDTO>>(POS_MACHINE_TYPE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> SavePOSTypes(List<POSTypeDTO> pOSTypeDTOList)
        {
            log.LogMethodEntry(pOSTypeDTOList);
            try
            {
                string responseString = await Post<string>(POS_MACHINE_TYPE_URL, pOSTypeDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> DeletePOSTypes(List<POSTypeDTO> pOSTypeDTOList)
        {
            log.LogMethodEntry(pOSTypeDTOList);
            try
            {
                string responseString = await Delete<string>(POS_MACHINE_TYPE_URL, pOSTypeDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
