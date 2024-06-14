/********************************************************************************************
 * Project Name - DefaultDataType
 * Description  - RemoteDefaultDataTypeUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         10-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class RemoteDefaultDataTypeUseCases:RemoteUseCases, IDefaultDataTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DEFULTDATATYPE_URL = "api/Environment/DefaultDataTypes";
        public RemoteDefaultDataTypeUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<DefaultDataTypeDTO>> GetDefaultDataTypes(List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<DefaultDataTypeDTO> result = await Get<List<DefaultDataTypeDTO>>(DEFULTDATATYPE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case DefaultDataTypeDTO.SearchByParameters.DATATYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("DataTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case DefaultDataTypeDTO.SearchByParameters.DATA_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("DataType".ToString(), searchParameter.Value));
                        }
                        break;
                   
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveDefaultDataTypes(List<DefaultDataTypeDTO> defaultDataTypeDTO)
        {
            log.LogMethodEntry(defaultDataTypeDTO);
            try
            {
                string responseString = await Post<string>(DEFULTDATATYPE_URL, defaultDataTypeDTO);
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
