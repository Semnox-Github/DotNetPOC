/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - RemoteSignagePatternUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    23-Feb-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    class RemoteSignagePatternUseCases:RemoteUseCases,ISignagePatternUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SIGNAGE_PATTERN_URL = "api/DigitalSignage/SignagePatterns"; 
        public RemoteSignagePatternUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<SignagePatternDTO>> GetSignagePatterns(List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<SignagePatternDTO> result = await Get<List<SignagePatternDTO>>(SIGNAGE_PATTERN_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<SignagePatternDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case SignagePatternDTO.SearchByParameters.SIGNAGE_PATTERN_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("signagePatternId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SignagePatternDTO.SearchByParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("name".ToString(), searchParameter.Value));
                        }
                        break;
                    case SignagePatternDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;   
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveSignagePatterns(List<SignagePatternDTO> signagePatternDTOList)
        {
            log.LogMethodEntry(signagePatternDTOList);
            try
            {
                string responseString = await Post<string>(SIGNAGE_PATTERN_URL, signagePatternDTOList);
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
