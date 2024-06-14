/********************************************************************************************
 * Project Name - Promotions
 * Description  - LocalLoyaltyRedemptionRuleUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    05-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 2.120.00    25-Mar-2021       Fiona                   Modified to add Delete UseCase
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Implementation of loyaltyRedemptionRule use-cases
    /// </summary>
    public class LocalLoyaltyRedemptionRuleUseCases :ILoyaltyRedemptionRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalLoyaltyRedemptionRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<LoyaltyRedemptionRuleDTO>> GetLoyaltyRedemptionRules(List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>>
         searchParameters)
        {
            return await Task<List<LoyaltyRedemptionRuleDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                LoyaltyRedemptionRuleListBL loyaltyRedemptionRuleListBL = new LoyaltyRedemptionRuleListBL(executionContext);
                List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList = loyaltyRedemptionRuleListBL.GetLoyaltyRedemptionRuleList(searchParameters);

                log.LogMethodExit(loyaltyRedemptionRuleDTOList);
                return loyaltyRedemptionRuleDTOList;
            });
        }
        public async Task<string> SaveLoyaltyRedemptionRules(List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(loyaltyRedemptionRuleDTOList);
                if (loyaltyRedemptionRuleDTOList == null)
                {
                    throw new ValidationException("loyaltyRedemptionRule is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO in loyaltyRedemptionRuleDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            LoyaltyRedemptionRuleBL loyaltyRedemptionRuleBL = new LoyaltyRedemptionRuleBL(executionContext, loyaltyRedemptionRuleDTO);
                            loyaltyRedemptionRuleBL.Save(parafaitDBTrx.SQLTrx);
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
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<LoyaltyRedemptionRuleContainerDTOCollection> GetLoyaltyRedemptionRuleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<LoyaltyRedemptionRuleContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    LoyaltyRedemptionRuleContainerList.Rebuild(siteId);
                }
                List<LoyaltyRedemptionRuleContainerDTO> currencyRuleContainerList = LoyaltyRedemptionRuleContainerList.GetLoyaltyRedemptionRuleContainerDTOList(siteId);
                LoyaltyRedemptionRuleContainerDTOCollection result = new LoyaltyRedemptionRuleContainerDTOCollection(currencyRuleContainerList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> Delete(List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(loyaltyRedemptionRuleDTOList);
                string result = string.Empty;

                try
                {
                    LoyaltyRedemptionRuleListBL loyaltyRedemptionRuleListBL = new LoyaltyRedemptionRuleListBL(executionContext, loyaltyRedemptionRuleDTOList);
                    loyaltyRedemptionRuleListBL.Delete();
                    result = "success";
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
