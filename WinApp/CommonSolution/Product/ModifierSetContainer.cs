/********************************************************************************************
 * Project Name - Products
 * Description  - ModifierSetContainer class to get the data    
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021        Prajwal S           Modified
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    class ModifierSetContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private string hash;
        private readonly int siteId; 
        private readonly Timer refreshTimer;
        private DateTime? modifierSetLastUpdateTime;
        private ConcurrentDictionary<int, ModifierSetDTO> modifierSetDTODictionary; 
        private readonly List<ModifierSetDTO> modifierSetDTOList;
        private ModifierSetContainerDTOCollection modifierSetContainerDTOCollection;
        private DateTime? buildTime;

        internal ModifierSetContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            modifierSetDTODictionary = new ConcurrentDictionary<int, ModifierSetDTO>();
            modifierSetDTOList = new List<ModifierSetDTO>();
            ModifierSetDTOList modifierSetList = new ModifierSetDTOList(executionContext);
            modifierSetLastUpdateTime = modifierSetList.GetModifierSetModuleLastUpdateTime(siteId);

            List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ModifierSetDTO.SearchByParameters, string>(ModifierSetDTO.SearchByParameters.ISACTIVE, "Y"));
            searchParameters.Add(new KeyValuePair<ModifierSetDTO.SearchByParameters, string>(ModifierSetDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            modifierSetDTOList = modifierSetList.GetAllModifierSetDTOList(searchParameters, true, true);
            if (modifierSetDTOList != null && modifierSetDTOList.Any())
            {
                foreach (ModifierSetDTO modifierSetDTO in modifierSetDTOList)
                {
                    modifierSetDTODictionary[modifierSetDTO.ModifierSetId] = modifierSetDTO;
                }
            }
            else
            {
                modifierSetDTOList = new List<ModifierSetDTO>();
                modifierSetDTODictionary = new ConcurrentDictionary<int, ModifierSetDTO>();
            }
            log.LogMethodExit();
        }

        public List<ModifierSetContainerDTO> GetModifierSetContainerDTOList() 
        {
            log.LogMethodEntry();
            List<ModifierSetContainerDTO> modifierSetContainerDTOList = new List<ModifierSetContainerDTO>();
            foreach (ModifierSetDTO modifierSetDTO in modifierSetDTOList)
            {

                ModifierSetContainerDTO modifierSetContainerDTO = new ModifierSetContainerDTO(modifierSetDTO.ModifierSetId, modifierSetDTO.SetName, modifierSetDTO.MinQuantity,
                                                                                          modifierSetDTO.MaxQuantity, modifierSetDTO.FreeQuantity);
                if(modifierSetDTO.ParentModifierSetId > -1)
                {
                    List<ModifierSetDTO> parentModifierSetDTO = modifierSetDTOList.Where(x => x.ModifierSetId == modifierSetDTO.ParentModifierSetId).ToList();
                    modifierSetContainerDTO.ParentModifierSetDTO = new ModifierSetContainerDTO(parentModifierSetDTO[0].ModifierSetId, parentModifierSetDTO[0].SetName, parentModifierSetDTO[0].MinQuantity, parentModifierSetDTO[0].MaxQuantity, parentModifierSetDTO[0].FreeQuantity);
                }
                modifierSetContainerDTO.ModifierSetDetailsContainerDTOList = new List<ModifierSetDetailsContainerDTO>();
                foreach (ModifierSetDetailsDTO modifierSetDetailsDTO in modifierSetDTO.ModifierSetDetailsDTO)
                {
                    modifierSetContainerDTO.ModifierSetDetailsContainerDTOList.Add(new ModifierSetDetailsContainerDTO(modifierSetDetailsDTO.ModifierSetDetailId, modifierSetDetailsDTO.ModifierSetId, modifierSetDetailsDTO.Price, modifierSetDetailsDTO.SortOrder, modifierSetDetailsDTO.ModifierProductId));
                }
                modifierSetContainerDTOList.Add(modifierSetContainerDTO);
            }
            modifierSetContainerDTOCollection = new ModifierSetContainerDTOCollection(modifierSetContainerDTOList);
            log.LogMethodExit(modifierSetContainerDTOList);
            return modifierSetContainerDTOList;
        }

        /// <summary>
        /// Returns ModifierSetContainerDTOCollection.
        /// </summary>
        /// <returns></returns>
        public ModifierSetContainerDTOCollection GetModifierSetContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(modifierSetContainerDTOCollection);
            return modifierSetContainerDTOCollection;
        }

        public ModifierSetContainer Refresh()
        {
            log.LogMethodEntry();
            ModifierSetDTOList modifierSetListBL = new ModifierSetDTOList(executionContext);
            DateTime? updateTime = modifierSetListBL.GetModifierSetModuleLastUpdateTime(siteId);
            if (modifierSetLastUpdateTime.HasValue
                && modifierSetLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in ModifierSet since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ModifierSetContainer result = new ModifierSetContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}