/********************************************************************************************
* Project Name - Product
* Description  - LocalUpsellOfferUseCases class
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    06-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Implementation of upsellOffer use-cases
    /// </summary>
    public class LocalUpsellOfferUseCases:IUpsellOfferUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalUpsellOfferUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<UpsellOffersDTO>> GetUpsellOffers(List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>>
                         searchParameters)
        {
            return await Task<List<UpsellOffersDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                UpsellOffersList upsellOffersList = new UpsellOffersList(executionContext);
                List<UpsellOffersDTO> upsellOffersDTOList = upsellOffersList.GetAllUpsellOffers(searchParameters);

                log.LogMethodExit(upsellOffersDTOList);
                return upsellOffersDTOList;
            });
        }
        public async Task<string> SaveUpsellOffers(List<UpsellOffersDTO> upsellOffersList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(upsellOffersList);
                    if (upsellOffersList == null)
                    {
                        throw new ValidationException("upsellOffersList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            UpsellOffersList upsellOffersListBL = new UpsellOffersList(executionContext, upsellOffersList);
                            upsellOffersListBL.SaveUpdateUpsellOffersList();
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
        
        public async Task<string> Delete(List<UpsellOffersDTO> upsellOffersDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(upsellOffersDTOList);
                    UpsellOffersList upsellOffersList = new UpsellOffersList(executionContext,upsellOffersDTOList);
                    upsellOffersList.DeleteUpsellOfferList();
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
