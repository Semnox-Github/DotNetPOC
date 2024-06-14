/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteAccountingCalendarUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00      16-Nov-2020        Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class RemoteAccountingCalendarUseCases : RemoteUseCases, IAccountingCalendarUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ACCOUNTING_CALENDAR_URL = "api/Inventory/Recipe/AccountingCalendars";


        public RemoteAccountingCalendarUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<AccountingCalendarMasterDTO>> GetAccountingCalendars(List<KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>> parameters)//, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();       
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList = await Get<List<AccountingCalendarMasterDTO>>(ACCOUNTING_CALENDAR_URL, searchParameterList);
                log.LogMethodExit(accountingCalendarMasterDTOList);
                return accountingCalendarMasterDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case AccountingCalendarMasterDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case AccountingCalendarMasterDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AccountingCalendarMasterDTO.SearchByParameters.MASTER_ENTITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterEntityId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveAccountingCalendars(List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList)
        {
            log.LogMethodEntry(accountingCalendarMasterDTOList);
            try
            {
                string responseString = await Post<string>(ACCOUNTING_CALENDAR_URL, accountingCalendarMasterDTOList);
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
