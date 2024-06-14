/********************************************************************************************
* Project Name - KioskUIFramework
* Description  - LocalAppUIPanelUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    27-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// Implementation of AppUIPanel use-cases
    /// </summary>
    public class LocalAppUIPanelUseCases:IAppUIPanelUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalAppUIPanelUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalAppUIPanelUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetAppUIPanels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async  Task<List<AppUIPanelDTO>> GetAppUIPanels(List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>> searchParameters,
                                                       bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<AppUIPanelDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                AppUIPanelListBL appUIPanelListBL = new AppUIPanelListBL(executionContext);
                List<AppUIPanelDTO> appUIPanelsDTOList = appUIPanelListBL.GetAppUIPanelDTOList(searchParameters, loadChildRecords, activeChildRecords);

                log.LogMethodExit(appUIPanelsDTOList);
                return appUIPanelsDTOList;
            });
        }
        /// <summary>
        /// SaveAppUIPanels
        /// </summary>
        /// <param name="appUIPanelsDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveAppUIPanels(List<AppUIPanelDTO> appUIPanelsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(appUIPanelsDTOList);
                    if (appUIPanelsDTOList == null)
                    {
                        throw new ValidationException("appUIPanelsDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            AppUIPanelListBL appUIPanelListBL = new AppUIPanelListBL(executionContext, appUIPanelsDTOList);
                            appUIPanelListBL.Save();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="appUIPanelsDTOList"></param>
        /// <returns></returns>
        public async Task<string> Delete(List<AppUIPanelDTO> appUIPanelsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(appUIPanelsDTOList);
                    AppUIPanelListBL appUIPanelListBL = new AppUIPanelListBL(executionContext, appUIPanelsDTOList);
                    appUIPanelListBL.Delete();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }

    }
}
