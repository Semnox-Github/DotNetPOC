/********************************************************************************************
 * Project Name - Waiver 
 * Description  - LocalWaiverSetUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************
 *2.130.0     20-Jul-2021   Mushahid Faizan    Created
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Waiver
{
    class LocalWaiverSetUseCases : IWaiverSetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalWaiverSetUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<WaiverSetDTO>> GetWaiverSets(List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> waiversSearchList,
             bool loadActiveChild = false, bool removeIncompleteRecords = false, bool getLanguageSpecificContent = false, string waiverSetSigningOptions = null)
        {
            return await Task<List<WaiverSetDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(waiversSearchList);
                List<WaiverSetDTO> waiverSetDTOListTemp = new List<WaiverSetDTO>();
                List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> signingOptionsSearchParam = null;
                if (!String.IsNullOrEmpty(waiverSetSigningOptions))
                {
                    signingOptionsSearchParam = new List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>>();
                    signingOptionsSearchParam.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.WAIVERSET_SIGNING_OPTIONS_LIST, waiverSetSigningOptions));
                }

                WaiverSetListBL waiverSetListBL = new WaiverSetListBL(executionContext);
                waiverSetDTOListTemp = waiverSetListBL.GetWaiverSetDTOList(waiversSearchList, true, loadActiveChild, removeIncompleteRecords, true, null, signingOptionsSearchParam);

                if (waiverSetDTOListTemp != null && getLanguageSpecificContent)
                {
                    foreach (WaiverSetDTO waiverSetDTO in waiverSetDTOListTemp)
                    {
                        foreach (WaiversDTO waiversDTO in waiverSetDTO.WaiverSetDetailDTOList)
                        {
                            if (waiversDTO.ObjectTranslationsDTOList != null && waiversDTO.ObjectTranslationsDTOList.Any())
                            {
                                ObjectTranslationsDTO objectTranslationsDTO = waiversDTO.ObjectTranslationsDTOList.Find(otl => otl.LanguageId == executionContext.GetLanguageId() && otl.ElementGuid == waiversDTO.Guid);
                                if (objectTranslationsDTO != null)
                                {
                                    if (waiversDTO.WaiverFileName != objectTranslationsDTO.Translation)
                                    {
                                        waiversDTO.WaiverFileName = objectTranslationsDTO.Translation;
                                        WaiversBL waiversBL = new WaiversBL(executionContext, waiversDTO);
                                        waiversBL.GetWaiverFileContentInBase64Format();
                                    }
                                }
                            }
                        }
                    }
                }
                log.LogMethodExit(waiverSetDTOListTemp);
                return waiverSetDTOListTemp;
            });
        }

        public async Task<string> SaveWaiverSets(List<WaiverSetDTO> waiverSetDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    WaiverSetListBL waiverSetList = new WaiverSetListBL(executionContext, waiverSetDTOList);
                    waiverSetList.SaveUpdateWaivers();
                    transaction.EndTransaction();
                    string result = "Success";
                    log.LogMethodExit(result);
                    return result;
                }
            });
        }


        public async Task<WaiverSetContainerDTOCollection> GetWaiverSetContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache)
        {
            return await Task<WaiverSetContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    WaiverSetContainerList.Rebuild(siteId);
                }
                WaiverSetContainerDTOCollection result = WaiverSetContainerList.GetWaiverSetContainerDTOCollection(siteId, languageId);
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
