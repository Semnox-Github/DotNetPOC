/********************************************************************************************
* Project Name -Promotions
* Description  - LocalPromotionUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    26-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Implementation of Promotion use-cases
    /// </summary>
    public class LocalPromotionUseCases:IPromotionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalPromotionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<PromotionDTO>> GetPromotions(List<KeyValuePair<PromotionDTO.SearchByParameters, string>> searchParameters,
                                                                      bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<PromotionDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                PromotionListBL promotionListBL = new PromotionListBL(executionContext);
                List<PromotionDTO> promotionDTOList = promotionListBL.GetPromotionDTOList(searchParameters, loadChildRecords, activeChildRecords, null);

                log.LogMethodExit(promotionDTOList);
                return promotionDTOList;
            });
        }
        public async Task<string> SavePromotions(List<PromotionDTO> promotionDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(promotionDTOList);
                    if (promotionDTOList == null)
                    {
                        throw new ValidationException("promotionDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            PromotionListBL promotionListBL = new PromotionListBL(executionContext, promotionDTOList);
                            promotionListBL.Save(parafaitDBTrx.SQLTrx);
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
        public async Task<string> Delete(List<PromotionDTO> promotionDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(promotionDTOList);
                    PromotionListBL promotionListBL = new PromotionListBL(executionContext, promotionDTOList);
                    promotionListBL.Delete();
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
