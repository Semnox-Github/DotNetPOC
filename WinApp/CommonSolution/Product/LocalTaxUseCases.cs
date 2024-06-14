/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalTaxUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class LocalTaxUseCases : ITaxUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalTaxUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<TaxDTO>> GetTaxes(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>
                          searchParameters, bool buildChildRecords, bool loadActiveChild, SqlTransaction sqlTransaction = null, int currentPage = 0, int pageSize = 0)
        {
            return await Task<List<TaxDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                TaxList taxListBL = new TaxList(executionContext);
                List<TaxDTO> taxDTOList = taxListBL.GetAllTaxes(searchParameters, buildChildRecords, loadActiveChild, sqlTransaction, currentPage, pageSize);
                log.LogMethodExit(taxDTOList);
                return taxDTOList;
            });
        }

        public async Task<string> SaveTaxes(List<TaxDTO> taxDTOList)
        {
            log.LogMethodEntry("taxDTOList");
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                if (taxDTOList == null)
                {
                    throw new ValidationException("taxDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (TaxDTO taxDTO in taxDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Tax taxBL = new Tax(executionContext, taxDTO);
                            taxBL.Save(parafaitDBTrx.SQLTrx);
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

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> DeleteTaxes(List<TaxDTO> taxDTOList)
        {
            log.LogMethodEntry("taxDTOList");
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                if (taxDTOList == null)
                {
                    throw new ValidationException("taxDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (TaxDTO taxDTO in taxDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Tax taxBL = new Tax(executionContext, taxDTO);
                            taxBL.Delete(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }

                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
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
                            throw ex;
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
