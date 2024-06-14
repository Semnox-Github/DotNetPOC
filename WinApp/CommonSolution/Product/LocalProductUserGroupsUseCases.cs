/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductUserGroupsUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
  2.110.00       17-Nov-2020       Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class LocalProductUserGroupsUseCases : IProductUserGroupsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalProductUserGroupsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<ProductUserGroupsDTO>> GetProductUserGroups(List<KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            return await Task<List<ProductUserGroupsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                ProductUserGroupsListBL productUserGroupsListBL = new ProductUserGroupsListBL(executionContext);
                List<ProductUserGroupsDTO> productUserGroupsDTOList = productUserGroupsListBL.GetAllProductUserGroupsDTOList(parameters, loadChildRecords, activeChildRecords);
                log.LogMethodExit(productUserGroupsDTOList);
                return productUserGroupsDTOList;
            });
        }

        public async Task<string> SaveProductUserGroups(List<ProductUserGroupsDTO> productUserGroupsDTOList)
        {
            log.LogMethodEntry("productUserGroupsDTOList");
            string result = string.Empty;
            return await Task<string>.Factory.StartNew(() =>
            {
                if (productUserGroupsDTOList == null)
                {
                    throw new ValidationException("productUserGroupsDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ProductUserGroupsDTO productUserGroupsDTO in productUserGroupsDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductUserGroupsBL productUserGroupsBL = new ProductUserGroupsBL(executionContext, productUserGroupsDTO);
                            productUserGroupsBL.Save(parafaitDBTrx.SQLTrx);
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
