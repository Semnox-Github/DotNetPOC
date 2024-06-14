/********************************************************************************************
 * Project Name - Tags
 * Description  - NotificationTagPatternUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    12-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    public class RemoteNotificationTagPatternUseCases:RemoteUseCases,INotificationTagPatternUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string NOTIFICATIONTAGPATTERN_URL = "api/Tag/NotificationTagPatterns";

        public RemoteNotificationTagPatternUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<NotificationTagPatternDTO>> GetNotificationTagPatterns(List<KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>>
                          searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<NotificationTagPatternDTO> result = await Get<List<NotificationTagPatternDTO>>(NOTIFICATIONTAGPATTERN_URL,searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case NotificationTagPatternDTO.SearchByParameters.NOTIFICATIONTAGPATTERNID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("notificationTagPatternId".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagPatternDTO.SearchByParameters.NOTIFICATIONTAGPATTERNNAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>(" notificationTagPatternName".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagPatternDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagPatternDTO.SearchByParameters.LEDPATTERNNUMBER:
                       
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ledPatternNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagPatternDTO.SearchByParameters.BUZZERPATTERNNUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("buzzerPatternNumber".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveNotificationTagPatterns(List<NotificationTagPatternDTO> notificationTagPatternDTOList)
        {
            log.LogMethodEntry(notificationTagPatternDTOList);
            try
            {
                string responseString = await Post<string>(NOTIFICATIONTAGPATTERN_URL, notificationTagPatternDTOList);
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
