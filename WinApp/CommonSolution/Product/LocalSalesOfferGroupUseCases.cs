/********************************************************************************************
 * Project Name - Product
 * Description  - LocalSalesOfferGroupUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    11-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    // <summary>
    /// Implementation of salesOfferGroup use-cases
    /// </summary>
    public class LocalSalesOfferGroupUseCases:ISalesOfferGroupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalSalesOfferGroupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<SalesOfferGroupDTO>> GetSalesOfferGroups(List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>>
                         searchParameters)
        {
            return await Task<List<SalesOfferGroupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                SalesOfferGroupList salesOfferGroupList = new SalesOfferGroupList(executionContext);
                List<SalesOfferGroupDTO> salesOfferGroupDTOList = salesOfferGroupList.GetAllSalesOfferGroups(searchParameters);

                log.LogMethodExit(salesOfferGroupDTOList);
                return salesOfferGroupDTOList;
            });
        }
       
        public async Task<string> SaveSalesOfferGroups(List<SalesOfferGroupDTO> salesOfferGroupsList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(salesOfferGroupsList);
                    if (salesOfferGroupsList == null)
                    {
                        throw new ValidationException("salesOfferGroupsList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            SalesOfferGroupList salesOfferGroup = new SalesOfferGroupList(executionContext, salesOfferGroupsList);
                            salesOfferGroup.SaveUpdateSalesOfferGroupsList();
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
