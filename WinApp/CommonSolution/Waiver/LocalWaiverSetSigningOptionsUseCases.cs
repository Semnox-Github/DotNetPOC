/********************************************************************************************
 * Project Name - Waiver
 * Description  - LocalWaiverSetSigningOptionsUseCases class 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Waiver
{
    public class LocalWaiverSetSigningOptionsUseCases:IWaiverSetSigningOptionsUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalWaiverSetSigningOptionsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
       public async Task<List<WaiverSetSigningOptionsDTO>> GetWaiverSetSigningOptions(List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<WaiverSetSigningOptionsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                WaiverSetSigningOptionsListBL signingOptionList = new WaiverSetSigningOptionsListBL(executionContext);
                List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTOList = signingOptionList.GetWaiverSetSigningOptionsList(searchParameters);

                log.LogMethodExit(waiverSetSigningOptionsDTOList);
                return waiverSetSigningOptionsDTOList;
            });
        }
        public async Task<string> SaveWaiverSetSigningOptions(List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTOs)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(waiverSetSigningOptionsDTOs);
                    if (waiverSetSigningOptionsDTOs == null)
                    {
                        throw new ValidationException("waiverSetSigningOptionsDTOs is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            WaiverSetSigningOptionsListBL waiverSetSigningOptionsListBL = new WaiverSetSigningOptionsListBL(executionContext, waiverSetSigningOptionsDTOs);
                            waiverSetSigningOptionsListBL.SaveUpdateWaiverSetSigningOptionList();
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
