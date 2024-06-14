/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalInventoryNotesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     11-Dec-2020       Mushahid Faizan         Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class LocalInventoryNotesUseCases : IInventoryNotesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalInventoryNotesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<InventoryNotesDTO>> GetInventoryNotes(List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>>
                          searchParameters, int currentPage = 0, int pageSize = 0)
        {

            return await Task<List<InventoryNotesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                InventoryNotesList inventoryNotesListBL = new InventoryNotesList();
                int siteId = GetSiteId();
                List<InventoryNotesDTO> inventoryNotesDTOList = inventoryNotesListBL.GetAllInventoryNotes(searchParameters);
                log.LogMethodExit(inventoryNotesDTOList);
                return inventoryNotesDTOList;
            });
        }

        public async Task<string> SaveInventoryNotes(List<InventoryNotesDTO> inventoryNotesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(inventoryNotesDTOList);
                if (inventoryNotesDTOList == null)
                {
                    throw new ValidationException("inventoryNotesDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (InventoryNotesDTO inventoryNotesDTO in inventoryNotesDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            InventoryNotes inventoryNotesBL = new InventoryNotes(executionContext, inventoryNotesDTO);
                            inventoryNotesBL.Save(parafaitDBTrx.SQLTrx);
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
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }


        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }

    }
}
