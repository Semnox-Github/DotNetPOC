/********************************************************************************************
* Project Name - DigitalSignage
* Description  - LocalDSLookupUseCases class 
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
    /// Implementation of DSLookup use-cases
    /// </summary>
    public class LocalDSLookupUseCases:IDSLookupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalDSLookupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<DSLookupDTO>> GetDSLookups(List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchParameters,
                                                                                bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<DSLookupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                DSLookupListBL lookupListBL = new DSLookupListBL(executionContext);
                List<DSLookupDTO> dSLookupDTOList = lookupListBL.GetDSLookupDTOList(searchParameters, activeChildRecords, loadChildRecords, sqlTransaction);

                log.LogMethodExit(dSLookupDTOList);
                return dSLookupDTOList;
            });
        }
        public async Task<string> SaveDSLookups(List<DSLookupDTO> lookupDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(lookupDTOList);
                    if (lookupDTOList == null)
                    {
                        throw new ValidationException("lookupDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DSLookupListBL dsLookupListBL = new DSLookupListBL(executionContext,lookupDTOList);
                            dsLookupListBL.Save();
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
