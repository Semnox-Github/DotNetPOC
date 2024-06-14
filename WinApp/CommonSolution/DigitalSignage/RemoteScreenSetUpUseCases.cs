/********************************************************************************************
* Project Name - DigitalSignage
* Description  - RemoteScreenSetupUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.140.00    22-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    class RemoteScreenSetupUseCases:RemoteUseCases,IScreenSetupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SCREENSETUP_URL = "api/DigitalSignage/Screens";
        public RemoteScreenSetupUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<ScreenSetupDTO>> GetScreenSetups(List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> searchParameters,
                                                  bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)

        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<ScreenSetupDTO> result = await Get<List<ScreenSetupDTO>>(SCREENSETUP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ScreenSetupDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ScreenSetupDTO.SearchByParameters.SCREEN_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("screenId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScreenSetupDTO.SearchByParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("name".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScreenSetupDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveScreenSetups(List<ScreenSetupDTO> screenSetupDTOList)
        {
            log.LogMethodEntry(screenSetupDTOList);
            try
            {
                string responseString = await Post<string>(SCREENSETUP_URL,screenSetupDTOList);
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
