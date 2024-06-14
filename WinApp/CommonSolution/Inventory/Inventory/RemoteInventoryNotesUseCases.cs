/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteInventoryNotesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     31-Dec-2020       Mushahid Faizan         Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class RemoteInventoryNotesUseCases : RemoteUseCases, IInventoryNotesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string INVENTORY_NOTES_URL = "api/Inventory/InventoryNotes";

        public RemoteInventoryNotesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<InventoryNotesDTO>> GetInventoryNotes(List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>>
                          parameters, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<InventoryNotesDTO> result = await Get<List<InventoryNotesDTO>>(INVENTORY_NOTES_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>> lookupSearchParams)
        {

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    //case InventoryNotesDTO.SearchByInventoryNotesParameters.ISACTIVE:
                    //    {
                    //        searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                    //    }
                    //    break;
                    case InventoryNotesDTO.SearchByInventoryNotesParameters.INVENTORY_NOTE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("inventoryNotesId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryNotesDTO.SearchByInventoryNotesParameters.NOTE_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("notetypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("parafaitObjectName".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("parafaitObjectId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveInventoryNotes(List<InventoryNotesDTO> inventoryNotesDTOList)
        {
            log.LogMethodEntry(inventoryNotesDTOList);
            try
            {
                string responseString = await Post<string>(INVENTORY_NOTES_URL, inventoryNotesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
