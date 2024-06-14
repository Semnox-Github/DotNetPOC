/********************************************************************************************
 * Project Name - Inventory
 * Description  - IUOMUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         30-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    public class LocalUOMUseCases : IUOMUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalUOMUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<UOMDTO>> GetUOMs(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>
                                                         searchParameters, bool loadChildRecords, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0)
        {
            return await Task<List<UOMDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                UOMList uomListBL = new UOMList(executionContext);
                List<UOMDTO> uomDTOList = uomListBL.GetAllUOMDTOList(searchParameters, loadChildRecords, activeChildRecords, null, currentPage, pageSize,true);
                log.LogMethodExit(uomDTOList);
                return uomDTOList;
            });
        }

        public async Task<int> GetUOMCounts(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>
                                                         searchParameters)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                UOMList uomListBL = new UOMList(executionContext);
                int result = uomListBL.GetUOMCount(searchParameters);
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> SaveUOMs(List<UOMDTO> uomDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry("uomDTOList");
                string result = string.Empty;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (UOMDTO uomDTO in uomDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            UOM uomBL = new UOM(executionContext, uomDTO);
                            uomBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }

                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
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
                result = "success";
                log.LogMethodExit(result);
                return result;
            });

        }
    }
}
