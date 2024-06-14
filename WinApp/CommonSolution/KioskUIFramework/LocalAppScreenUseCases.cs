/********************************************************************************************
* Project Name - KioskUIFramework
* Description  - LocalAppScreenUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    27-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// LocalAppScreenUseCases
    /// </summary>
    public class LocalAppScreenUseCases:IAppScreenUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalAppScreenUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalAppScreenUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetAppScreens 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<AppScreenDTO>> GetAppScreens(List<KeyValuePair<AppScreenDTO.SearchByParameters, string>> searchParameters,
                                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<AppScreenDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                AppScreenListBL appScreenListBL = new AppScreenListBL(executionContext);
                List<AppScreenDTO> appScreenDTOList = appScreenListBL.GetAppScreenDTOList(searchParameters, loadChildRecords, activeChildRecords);

                log.LogMethodExit(appScreenDTOList);
                return appScreenDTOList;
            });
        }
        /// <summary>
        /// SaveAppScreens
        /// </summary>
        /// <param name="appScreenDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveAppScreens(List<AppScreenDTO> appScreenDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(appScreenDTOList);
                    if (appScreenDTOList == null)
                    {
                        throw new ValidationException("appScreenDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            AppScreenListBL appScreenListBL = new AppScreenListBL(executionContext, appScreenDTOList);
                            appScreenListBL.Save();
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
        /// <param name="appScreenDTOList"></param>
        /// <returns></returns>
        public async Task<string> Delete(List<AppScreenDTO> appScreenDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(appScreenDTOList);
                    AppScreenListBL appScreenListBL = new AppScreenListBL(executionContext, appScreenDTOList);
                    appScreenListBL.Delete();
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
