/********************************************************************************************
* Project Name -DigitalSignage
* Description  - LocalScreenSetupUseCases class 
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
    /// <summary>
    /// Implementation of ScreenSetup use-cases
    /// </summary>
    public class LocalScreenSetupUseCases:IScreenSetupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalScreenSetupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ScreenSetupDTO>> GetScreenSetups(List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> searchParameters,
                                                 bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ScreenSetupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords,activeChildRecords,sqlTransaction);

                ScreenSetupList screenSetupList = new ScreenSetupList(executionContext);
                List<ScreenSetupDTO> screenSetupDTOList = screenSetupList.GetAllScreenSetup(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                log.LogMethodExit(screenSetupDTOList);
                return screenSetupDTOList;
            });
        }
        public async Task<string> SaveScreenSetups(List<ScreenSetupDTO> screenSetupDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(screenSetupDTOList);
                    if (screenSetupDTOList == null)
                    {
                        throw new ValidationException("screenSetupDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ScreenSetupList screenSetup = new ScreenSetupList(executionContext, screenSetupDTOList);
                            screenSetup.Save();
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
    }
}
