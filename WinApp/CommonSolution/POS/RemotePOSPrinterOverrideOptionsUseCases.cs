/********************************************************************************************
 * Project Name - POS 
 * Description  - Remote PosPrinterOverrideOptionsUseCases class to get the data 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      30-Dec-2020      Dakshakh Raj              Created : Peru Invoice changes
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
    /// <summary>
    /// RemotePOSPrinterOverrideOptionsUseCases
    /// </summary>
    public class RemotePOSPrinterOverrideOptionsUseCases : RemoteUseCases, IPOSPrinterOverrideOptionsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string POS_PRINTER_OVERRIDE_OPTIONS_URL = "api/POS/POSPrinterOverrideOptions";

        public RemotePOSPrinterOverrideOptionsUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get POS Printer Override Options
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<POSPrinterOverrideOptionsDTO>> GetPOSPrinterOverrideOptions(List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>> parameters,
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
                List<POSPrinterOverrideOptionsDTO> result = await Get<List<POSPrinterOverrideOptionsDTO>>(POS_PRINTER_OVERRIDE_OPTIONS_URL, searchParameterList);
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
        /// <param name="POSPrinterOverrideOptionsSearchParams"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>> POSPrinterOverrideOptionsSearchParams)
        {
            log.LogMethodEntry(POSPrinterOverrideOptionsSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string> searchParameter in POSPrinterOverrideOptionsSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case POSPrinterOverrideOptionsDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSPrinterOverrideOptionsDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("pOSPrinterOverrideOptionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSPrinterOverrideOptionsDTO.SearchByParameters.OPTION_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("optionName".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        /// <summary>
        /// Save POS Printer Override Options
        /// </summary>
        /// <param name="pOSPrinterOverrideOptionsDTOList"></param>
        /// <returns></returns>
        public async Task<string> SavePOSPrinterOverrideOptions(List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList)
        {
            log.LogMethodEntry(pOSPrinterOverrideOptionsDTOList);
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(pOSPrinterOverrideOptionsDTOList);
                string responseString = await Post<string>(POS_PRINTER_OVERRIDE_OPTIONS_URL, content);
                //dynamic response = JsonConvert.DeserializeObject(responseString);
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
