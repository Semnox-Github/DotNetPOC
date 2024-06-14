/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteInventoryLotUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.150.0     22-Jun-2022      Abhishek           Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class RemoteInventoryLotUseCases : RemoteUseCases, IInventoryLotUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string INVENTORY_LOT_URL = "api/Inventory/Lots";

        public RemoteInventoryLotUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<InventoryLotDTO>> GetInventoryLots(List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>>
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
                List<InventoryLotDTO> result = await Get<List<InventoryLotDTO>>(INVENTORY_LOT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>> lookupSearchParams)
        {

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case InventoryLotDTO.SearchByInventoryLotParameters.LOT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("lotId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryLotDTO.SearchByInventoryLotParameters.LOT_NUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("lotNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryLotDTO.SearchByInventoryLotParameters.PURCHASEORDER_RECEIVE_LINEID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("purchaseOrderReceiveLineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryLotDTO.SearchByInventoryLotParameters.UOM_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uomId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
    }
}
