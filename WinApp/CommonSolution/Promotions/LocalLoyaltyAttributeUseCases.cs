/********************************************************************************************
 * Project Name - Achievements
 * Description  - LocalLoyaltyAttributeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    04-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
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
    /// Implementation of loyaltyAttribute use-cases
    /// </summary>
    class LocalLoyaltyAttributeUseCases : ILoyaltyAttributeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalLoyaltyAttributeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<LoyaltyAttributesDTO>> GetLoyaltyAttributes(List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<LoyaltyAttributesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                LoyaltyAttributeListBL loyaltyAttributeListBL = new LoyaltyAttributeListBL(executionContext);
                List<LoyaltyAttributesDTO> loyaltyAttributeDTOList = loyaltyAttributeListBL.GetAllLoyaltyAttributesList(searchParameters);

                log.LogMethodExit(loyaltyAttributeDTOList);
                return loyaltyAttributeDTOList;
            });
        }
        public async Task<string> SaveLoyaltyAttributes(List<LoyaltyAttributesDTO> loyaltyAttributeDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(loyaltyAttributeDTOList);
                if (loyaltyAttributeDTOList == null)
                {
                    throw new ValidationException("loyaltyAttributeDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (LoyaltyAttributesDTO loyaltyAttributeDTO in loyaltyAttributeDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            LoyaltyAttributeBL loyaltyAttributeBL = new LoyaltyAttributeBL(executionContext, loyaltyAttributeDTO);
                            loyaltyAttributeBL.Save(parafaitDBTrx.SQLTrx);
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
        public async Task<LoyaltyAttributeContainerDTOCollection> GetLoyaltyAttributeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<LoyaltyAttributeContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    LoyaltyAttributeContainerList.Rebuild(siteId);
                }
                List<LoyaltyAttributeContainerDTO> currencyRuleContainerList = LoyaltyAttributeContainerList.GetLoyaltyAttributeContainerDTOList(siteId);
                LoyaltyAttributeContainerDTOCollection result = new LoyaltyAttributeContainerDTOCollection(currencyRuleContainerList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
