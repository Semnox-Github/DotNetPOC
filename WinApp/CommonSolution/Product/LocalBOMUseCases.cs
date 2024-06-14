/********************************************************************************************
 * Project Name - Product
 * Description  - LocalBOMSetUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    09-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class LocalBOMUseCases: IBOMUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalBOMUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<BOMDTO>> GetBOMs(List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>>
                         searchParameters)
        {
            return await Task<List<BOMDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                BOMList bOMList = new BOMList(executionContext);
                List<BOMDTO> bOMDTOList = bOMList.GetAllBOMs(searchParameters);

                log.LogMethodExit(bOMDTOList);
                return bOMDTOList;
            });
        }

        public async Task<string> SaveBOMs(List<BOMDTO> bOMDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(bOMDTOList);
                if (bOMDTOList == null)
                {
                    throw new ValidationException("bOMDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        BOMList bOMList = new BOMList(executionContext, bOMDTOList);
                        bOMList.SaveUpdateProductBOM();
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
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
