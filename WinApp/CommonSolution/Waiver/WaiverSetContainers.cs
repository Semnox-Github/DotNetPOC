/********************************************************************************************
 * Project Name - Waiver
 * Description  - WaiverSetContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      20-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Waiver
{
    public class WaiverSetContainers 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<WaiverSetDTO> waiverSetDTOList;
        private readonly DateTime? waiverSetModuleLastUpdateTime;
        private readonly int siteId;
        private readonly Dictionary<int, WaiverSetContainerDTOCollection> languageIdWaiverSetContainerDTOCollectionDictionary = new Dictionary<int, WaiverSetContainerDTOCollection>();
        private readonly HashSet<int> languageIdHashSet = new HashSet<int>() { -1 };
        private readonly Dictionary<int, List<ObjectTranslationsDTO>> languageIdObjectTranslatedDTODictionary = new Dictionary<int, List<ObjectTranslationsDTO>>();
        private readonly Dictionary<int, WaiverSetDTO> waiverSetIdWaiverSetDTODictionary = new Dictionary<int, WaiverSetDTO>();
        //private readonly WaiverSetContainerDTOCollection waiverSetContainerDTOCollection;
        private readonly Dictionary<int, WaiverSetContainerDTO> waiverSetIdwaiverSetContainerDTODictionary = new Dictionary<int, WaiverSetContainerDTO>();

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        public WaiverSetContainers(int siteId) : this(siteId, GetWaiverSetDTOList(siteId), GetWaiverSetModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public WaiverSetContainers(int siteId, List<WaiverSetDTO> waiverSetDTOList, DateTime? waiverSetModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.waiverSetDTOList = waiverSetDTOList;
            this.waiverSetModuleLastUpdateTime = waiverSetModuleLastUpdateTime;
            foreach (var waiverSetDTO in waiverSetDTOList)
            {
                if (waiverSetIdWaiverSetDTODictionary.ContainsKey(waiverSetDTO.WaiverSetId))
                {
                    continue;
                }
                waiverSetIdWaiverSetDTODictionary.Add(waiverSetDTO.WaiverSetId, waiverSetDTO);
            }
            List<WaiverSetContainerDTO> waiverSetContainerDTOList = new List<WaiverSetContainerDTO>();

            foreach (WaiverSetDTO waiverSetDTO in waiverSetDTOList)
            {
                if (waiverSetIdwaiverSetContainerDTODictionary.ContainsKey(waiverSetDTO.WaiverSetId))
                {
                    continue;
                }
                WaiverSetContainerDTO waiverSetContainerDTO = new WaiverSetContainerDTO(waiverSetDTO.WaiverSetId, waiverSetDTO.Name);
                if (waiverSetDTO.WaiverSetSigningOptionDTOList != null && waiverSetDTO.WaiverSetSigningOptionDTOList.Any())
                {
                    waiverSetContainerDTO.WaiverSetSigningOptionsContainerDTO.AddRange(waiverSetDTO.WaiverSetSigningOptionDTOList.Select(x => new WaiverSetSigningOptionsContainerDTO
                    (x.LookupValueId, x.OptionName, x.OptionDescription)));
                }
                if (waiverSetDTO.WaiverSetDetailDTOList != null && waiverSetDTO.WaiverSetDetailDTOList.Any())
                {
                    waiverSetContainerDTO.WaiversContainerDTO.AddRange(waiverSetDTO.WaiverSetDetailDTOList.Select(x => new WaiversContainerDTO
                    (x.Name, x.WaiverFileName, x.ValidForDays, x.EffectiveDate)));
                }
                waiverSetContainerDTOList.Add(waiverSetContainerDTO);
                waiverSetIdwaiverSetContainerDTODictionary.Add(waiverSetDTO.WaiverSetId, waiverSetContainerDTO);
                foreach ( var waiverDetailDTO in waiverSetDTO.WaiverSetDetailDTOList)
                {
                    if (waiverDetailDTO.ObjectTranslationsDTOList!=null && waiverDetailDTO.ObjectTranslationsDTOList.Any())
                    {
                        foreach (var objectTranslatedDTO in waiverDetailDTO.ObjectTranslationsDTOList)
                        {
                            if (languageIdObjectTranslatedDTODictionary.ContainsKey(objectTranslatedDTO.LanguageId))
                            {
                                languageIdObjectTranslatedDTODictionary[objectTranslatedDTO.LanguageId].Add(objectTranslatedDTO);
                            }
                            else
                            {
                                languageIdObjectTranslatedDTODictionary.Add(objectTranslatedDTO.LanguageId, new List<ObjectTranslationsDTO>() { objectTranslatedDTO });
                            }
                            if (!languageIdHashSet.Contains(objectTranslatedDTO.LanguageId))
                            {
                                languageIdHashSet.Add(objectTranslatedDTO.LanguageId);
                            }
                        }
                    }
                }
            }
            languageIdWaiverSetContainerDTOCollectionDictionary.Add(-1, new WaiverSetContainerDTOCollection(waiverSetContainerDTOList));
            //waiverSetContainerDTOCollection = new WaiverSetContainerDTOCollection(waiverSetContainerDTOList);
            foreach (var languageId in languageIdHashSet)
            {
                if (languageId != -1)
                {
                    List<WaiverSetContainerDTO> langwaiverSetContainerDTOList = new List<WaiverSetContainerDTO>();
    
                    if (languageIdObjectTranslatedDTODictionary.ContainsKey(languageId))
                    {
                        foreach (WaiverSetDTO langWaiverSetDTO in waiverSetDTOList)
                        {
                            WaiverSetContainerDTO langwaiverSetContainerDTO = new WaiverSetContainerDTO(langWaiverSetDTO.WaiverSetId, langWaiverSetDTO.Name);
                            if (langWaiverSetDTO.WaiverSetSigningOptionDTOList != null && langWaiverSetDTO.WaiverSetSigningOptionDTOList.Any())
                            {
                                langwaiverSetContainerDTO.WaiverSetSigningOptionsContainerDTO.AddRange(langWaiverSetDTO.WaiverSetSigningOptionDTOList.Select(x => new WaiverSetSigningOptionsContainerDTO
                                (x.LookupValueId, x.OptionName, x.OptionDescription)));
                            }
                            if (langWaiverSetDTO.WaiverSetDetailDTOList != null && langWaiverSetDTO.WaiverSetDetailDTOList.Any())
                            {
                                foreach (WaiversDTO langwaiverDTO in langWaiverSetDTO.WaiverSetDetailDTOList)
                                {
                                    if (languageIdObjectTranslatedDTODictionary[languageId].Any(x => x.ElementGuid == langwaiverDTO.Guid))
                                    {
                                        langwaiverSetContainerDTO.WaiversContainerDTO.Add(new WaiversContainerDTO
                                            (langwaiverDTO.Name, languageIdObjectTranslatedDTODictionary[languageId].FirstOrDefault(x => x.ElementGuid == langwaiverDTO.Guid).Translation, langwaiverDTO.ValidForDays, langwaiverDTO.EffectiveDate));
                                    }
                                    else
                                    {
                                        langwaiverSetContainerDTO.WaiversContainerDTO.Add(new WaiversContainerDTO
                                            (langwaiverDTO.Name, langwaiverDTO.WaiverFileName, langwaiverDTO.ValidForDays, langwaiverDTO.EffectiveDate));
                                    }
                                }
                            }
                            langwaiverSetContainerDTOList.Add(langwaiverSetContainerDTO);
                        }                        
                    }
                    languageIdWaiverSetContainerDTOCollectionDictionary.Add(languageId, new WaiverSetContainerDTOCollection(langwaiverSetContainerDTOList));
                }
            }
            log.LogMethodExit();
        }
        private static List<WaiverSetDTO> GetWaiverSetDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<WaiverSetDTO> waiverSetDTOList = null;
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            try
            {
                WaiverSetListBL waiverSetList = new WaiverSetListBL(executionContext);

                List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> searchParameters = new List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>>();
                searchParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.SITE_ID, siteId.ToString()));
                waiverSetDTOList = waiverSetList.GetWaiverSetDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Waiver Sets.", ex);
            }

            if (waiverSetDTOList == null)
            {
                waiverSetDTOList = new List<WaiverSetDTO>();
            }
            log.LogMethodExit(waiverSetDTOList);
            return waiverSetDTOList;
        }

        private static DateTime? GetWaiverSetModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                WaiverSetListBL waiverSetList = new WaiverSetListBL();
                result = waiverSetList.GetWaiverSetLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the WaiverSet max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }


        public WaiverSetContainerDTO GetWaiverSetContainerDTO(int waiverSetId)
        {
            log.LogMethodEntry(waiverSetId);
            if (waiverSetIdwaiverSetContainerDTODictionary.ContainsKey(waiverSetId) == false)
            {
                string errorMessage = "waiverSet with waiverSet Id :" + waiverSetId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            WaiverSetContainerDTO result = waiverSetIdwaiverSetContainerDTODictionary[waiverSetId]; ;
            log.LogMethodExit(result);
            return result;
        }

        public WaiverSetContainerDTOCollection GetWaiverSetContainerDTOCollection(int languageId)
        {
            log.LogMethodEntry(languageId);
            log.LogMethodEntry(languageId);
            WaiverSetContainerDTOCollection result;
            if (languageIdWaiverSetContainerDTOCollectionDictionary.ContainsKey(languageId))
            {
                result = languageIdWaiverSetContainerDTOCollectionDictionary[languageId];
            }
            else if (languageIdWaiverSetContainerDTOCollectionDictionary.ContainsKey(-1))
            {
                result = languageIdWaiverSetContainerDTOCollectionDictionary[-1];
            }
            else
            {
                result = new WaiverSetContainerDTOCollection();
            }
            log.LogMethodExit(result);
            return result;
        }
        //internal WaiverSetContainers(int siteId)
        //{
        //    log.LogMethodEntry(siteId);
        //    this.siteId = siteId;
        //    waiverSetIdWaverSetDTODictionary = new ConcurrentDictionary<int, WaiverSetDTO>();
        //    waiverSetDTOList = new List<WaiverSetDTO>();
        //    WaiverSetListBL waiverSetList = new WaiverSetListBL(executionContext);
        //    waiverSetModuleLastUpdateTime = waiverSetList.GetWaiverSetLastUpdateTime(siteId);

        //    List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> searchParameters = new List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>>();
        //    searchParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.IS_ACTIVE, "1"));
        //    searchParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.SITE_ID, siteId.ToString()));
        //    waiverSetDTOList = waiverSetList.GetWaiverSetDTOList(searchParameters, true, true);

        //    if (waiverSetDTOList != null && waiverSetDTOList.Any())
        //    {
        //        foreach (WaiverSetDTO waiverSetDTO in waiverSetDTOList)
        //        {
        //            waiverSetIdWaverSetDTODictionary[waiverSetDTO.WaiverSetId] = waiverSetDTO;
        //        }
        //    }
        //    else
        //    {
        //        waiverSetDTOList = new List<WaiverSetDTO>();
        //        waiverSetIdWaverSetDTODictionary = new ConcurrentDictionary<int, WaiverSetDTO>();
        //    }
        //    log.LogMethodExit();
        //}
        public List<WaiverSetContainerDTO> GetWaiverSetContainerDTOList(int languageId)
        {
            log.LogMethodEntry(languageId);
            Dictionary<int, WaiverSetDTO> waiverSetDTODictionary = new Dictionary<int, WaiverSetDTO>();
            List<WaiverSetContainerDTO> waiverSetContainerDTOList = new List<WaiverSetContainerDTO>();


            foreach (WaiverSetDTO waiverSetDTO in waiverSetDTOList)
            {
                WaiverSetContainerDTO waiverSetContainerDTO = new WaiverSetContainerDTO(waiverSetDTO.WaiverSetId, waiverSetDTO.Name);

                if (waiverSetDTO.WaiverSetSigningOptionDTOList != null && waiverSetDTO.WaiverSetSigningOptionDTOList.Any())
                {
                    waiverSetContainerDTO.WaiverSetSigningOptionsContainerDTO.AddRange(waiverSetDTO.WaiverSetSigningOptionDTOList.Select(x => new WaiverSetSigningOptionsContainerDTO
                    (x.LookupValueId, x.OptionName, x.OptionDescription)));
                }
                if (waiverSetDTO.WaiverSetDetailDTOList != null && waiverSetDTO.WaiverSetDetailDTOList.Any())
                {
                    waiverSetContainerDTO.WaiversContainerDTO.AddRange(waiverSetDTO.WaiverSetDetailDTOList.Select(x => new WaiversContainerDTO
                    (x.Name, x.WaiverFileName,x.ValidForDays, x.EffectiveDate)));
                }

                waiverSetContainerDTOList.Add(waiverSetContainerDTO);
            }
            log.LogMethodExit(waiverSetContainerDTOList);
            return waiverSetContainerDTOList;
        }
        public WaiverSetContainers Refresh()
        {
            log.LogMethodEntry();
            WaiverSetListBL waiverSetList = new WaiverSetListBL();
            DateTime? updateTime = waiverSetList.GetWaiverSetLastUpdateTime(siteId);
            if (waiverSetModuleLastUpdateTime.HasValue
                && waiverSetModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in WaiverSet since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            WaiverSetContainers result = new WaiverSetContainers(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
