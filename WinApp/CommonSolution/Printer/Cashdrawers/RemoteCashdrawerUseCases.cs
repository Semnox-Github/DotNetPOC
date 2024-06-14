/********************************************************************************************
 * Project Name - Device
 * Description  - RemoteCashdrawerUseCases
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Aug-2021     Girish Kundar              Created : Multi cashdrawer for POS changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    /// <summary>
    /// RemoteCashdrawerUseCases
    /// </summary>
    public class RemoteCashdrawerUseCases : RemoteUseCases, ICashdrawerUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string Cashdrawer_URL = "api/Device/Cashdrawers";
        private const string CASHDRAWER_CONTAINER_URL = "api/Device/CashdrawerContainer";

        /// <summary>
        /// RemoteCashdrawerUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteCashdrawerUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetCashdrawer
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<CashdrawerDTO>> GetCashdrawers(List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>>
                          parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
                if (parameters != null)
                {
                    searchParameterList.AddRange(BuildSearchParameter(parameters));
                }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<CashdrawerDTO> result = await Get<List<CashdrawerDTO>>(Cashdrawer_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CashdrawerDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CashdrawerDTO.SearchByParameters.CASHDRAWER_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cashdrawerName".ToString(), searchParameter.Value));
                        }
                        break;
                    case CashdrawerDTO.SearchByParameters.CAHSDRAWER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cashdrawerId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CashdrawerDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        /// <summary>
        /// SaveCashdrawer
        /// </summary>
        /// <param name="cashdrawerDTOList"></param>
        /// <returns></returns>
        public async Task<List<CashdrawerDTO>> SaveCashdrawers(List<CashdrawerDTO> cashdrawerDTOList)
        {
            log.LogMethodEntry(cashdrawerDTOList);
            try
            {
                List<CashdrawerDTO> responseString = await Post<List<CashdrawerDTO>>(Cashdrawer_URL, cashdrawerDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<CashdrawerContainerDTOCollection> GetCashdrawerContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            CashdrawerContainerDTOCollection result = await Get<CashdrawerContainerDTOCollection>(CASHDRAWER_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

    }
}
