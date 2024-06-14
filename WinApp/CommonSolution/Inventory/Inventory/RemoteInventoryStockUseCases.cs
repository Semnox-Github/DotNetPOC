/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteInventoryStockUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         30-Nov-2020       Girish                    Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Inventory
{
    public class RemoteInventoryStockUseCases : RemoteUseCases, IInventoryStockUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string STOCK_URL = "api/Inventory/Stocks";

        public RemoteInventoryStockUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async  Task<List<InventoryDTO>> GetInventoryDTOList(List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams)
        {
            log.LogMethodEntry(inventorySearchParams);
            List<InventoryDTO> result = null;
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.AddRange(BuildSearchParameter(inventorySearchParams));
            try
            {
                result = await Get<List<InventoryDTO>>(STOCK_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchByInventoryParameters)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryDTO.SearchByInventoryParameters, string> searchParameter in searchByInventoryParameters)
            {
                switch (searchParameter.Key)
                {

                    case InventoryDTO.SearchByInventoryParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryDTO.SearchByInventoryParameters.PRODUCT_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryDTO.SearchByInventoryParameters.POS_MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posMachineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryDTO.SearchByInventoryParameters.IS_REDEEMABLE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isRedeemable".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryDTO.SearchByInventoryParameters.IS_SELLABLE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isSellable".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryDTO.SearchByInventoryParameters.UPDATED_AFTER_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("updatedAfterDate".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
    }
}
