/********************************************************************************************
 * Project Name - Generic Utilitiess 
 * Description  - LocalAccountingCalendarUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00       16-Nov-2020      Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class LocalAccountingCalendarUseCases : IAccountingCalendarUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalAccountingCalendarUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<AccountingCalendarMasterDTO>> GetAccountingCalendars(List<KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>> parameters)//, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            return await Task<List<AccountingCalendarMasterDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                AccountingCalendarMasterListBL accountingCalendarMasterListBL = new AccountingCalendarMasterListBL(executionContext);
                List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList = accountingCalendarMasterListBL.GetAccountingCalendarMasterDTOList(parameters);//, loadChildRecords, activeChildRecords);
                log.LogMethodExit(accountingCalendarMasterDTOList);
                return accountingCalendarMasterDTOList;
            });
        }

        public async Task<string> SaveAccountingCalendars(List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(accountingCalendarMasterDTOList);
                string result = string.Empty;
                if (accountingCalendarMasterDTOList == null)
                {
                    throw new ValidationException("accountingCalendarMasterDTOList is empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (AccountingCalendarMasterDTO accountingCalendarMasterDTO in accountingCalendarMasterDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            AccountingCalendarMasterBL accountingCalendarMasterBL = new AccountingCalendarMasterBL(executionContext, accountingCalendarMasterDTO);
                            accountingCalendarMasterBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ;
                        }
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
