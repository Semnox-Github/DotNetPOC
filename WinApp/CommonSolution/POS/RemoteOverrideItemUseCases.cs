/********************************************************************************************
 * Project Name - POS 
 * Description  - Remote OverrideItemUseCases class to get the data 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      31-Dec-2020      Dakshakh Raj              Created : Peru Invoice changes
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class RemoteOverrideItemUseCases : RemoteUseCases, IOverrideItemUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string OVERRIDE_ITEM_URL = "api/POS/OverrideItems";

        public RemoteOverrideItemUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get POS Printer Override Rules
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<OverrideOptionItemDTO>> GetOverrideOptionItems(List<KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string>> parameters,
                                                                    SqlTransaction sqlTransaction = null)
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
                List<OverrideOptionItemDTO> result = await Get<List<OverrideOptionItemDTO>>(OVERRIDE_ITEM_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// BuildSearchParameter
        /// </summary>
        /// <param name="overrideOptionItemSearchParams"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string>> overrideOptionItemSearchParams)
        {
            log.LogMethodEntry(overrideOptionItemSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string> searchParameter in overrideOptionItemSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case OverrideOptionItemDTO.SearchByParameters.OPTION_ITEM_CODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("optionItemCode".ToString(), searchParameter.Value));
                        }
                        break;
                    case OverrideOptionItemDTO.SearchByParameters.OPTION_ITEM_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("optionItemName".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
    }
}
