/********************************************************************************************
 * Project Name - POS
 * Description  - Concrete implementation of product menu use cases
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class LocalProductMenuUseCases : LocalUseCases, IProductMenuUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalProductMenuUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        

        public async Task<List<ProductMenuDTO>> GetProductMenuDTOList(string isActive = null, int menuId = -1, string name = "", DateTime? endDateGreaterThanEqualTo = null, DateTime? startDateLessThanEqualTo = null, int siteId = -1, bool loadChildRecords = true, bool loadActiveChildRecords = true)
        {
            return await Task<List<ProductMenuDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(isActive, menuId, name, endDateGreaterThanEqualTo, startDateLessThanEqualTo, siteId, loadChildRecords, loadActiveChildRecords);
                ProductMenuListBL productMenuListBL = new ProductMenuListBL();
                List<KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>> searchParameters = new List<KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>>();
                if (string.IsNullOrWhiteSpace(isActive) == false && (isActive.ToString() == "1" || isActive.ToString() == "Y"))
                {
                    searchParameters.Add(new KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>(ProductMenuDTO.SearchByProductMenuParameters.IS_ACTIVE, isActive));
                }
                if (menuId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>(ProductMenuDTO.SearchByProductMenuParameters.MENU_ID, menuId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(name) == false)
                {
                    searchParameters.Add(new KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>(ProductMenuDTO.SearchByProductMenuParameters.NAME, name));
                }
                if (endDateGreaterThanEqualTo.HasValue)
                {
                    searchParameters.Add(new KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>(ProductMenuDTO.SearchByProductMenuParameters.END_DATE_GREATER_THAN_EQUAL, endDateGreaterThanEqualTo.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (startDateLessThanEqualTo.HasValue)
                {
                    searchParameters.Add(new KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>(ProductMenuDTO.SearchByProductMenuParameters.START_DATE_LESS_THAN_EQUAL, startDateLessThanEqualTo.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                searchParameters.Add(new KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>(ProductMenuDTO.SearchByProductMenuParameters.SITE_ID, executionContext.SiteId.ToString()));
                List<ProductMenuDTO> result = productMenuListBL.GetProductMenuDTOList(searchParameters, loadChildRecords, loadActiveChildRecords);
                return result;
            });
        }

        public async Task<List<ProductMenuPanelDTO>> GetProductMenuPanelDTOList(string isActive = null, int panelId = -1, string name = "", int siteId = -1, bool loadChildRecords = true, bool loadActiveChildRecords = true, string guid = "")
        {
            return await Task<List<ProductMenuPanelDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(isActive, panelId, name, siteId, loadChildRecords, loadActiveChildRecords);
                ProductMenuPanelListBL productMenuPanelListBL = new ProductMenuPanelListBL();
                List<KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>> searchParameters = new List<KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>>();
                if (string.IsNullOrWhiteSpace(isActive) == false && (isActive.ToString() == "1" || isActive.ToString() == "Y"))
                {
                    searchParameters.Add(new KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>(ProductMenuPanelDTO.SearchByProductMenuPanelParameters.IS_ACTIVE, isActive));
                }
                if (panelId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>(ProductMenuPanelDTO.SearchByProductMenuPanelParameters.PANEL_ID, panelId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(name) == false)
                {
                    searchParameters.Add(new KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>(ProductMenuPanelDTO.SearchByProductMenuPanelParameters.NAME, name));
                }
                if (string.IsNullOrWhiteSpace(guid) == false)
                {
                    searchParameters.Add(new KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>(ProductMenuPanelDTO.SearchByProductMenuPanelParameters.GUID, guid));
                }
                searchParameters.Add(new KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>(ProductMenuPanelDTO.SearchByProductMenuPanelParameters.SITE_ID, executionContext.SiteId.ToString()));
                List<ProductMenuPanelDTO> result = productMenuPanelListBL.GetProductMenuPanelDTOList(searchParameters, loadChildRecords, loadActiveChildRecords);
                return result;
            });
        }

        public async Task<List<ProductMenuDTO>> SaveProductMenuDTOList(List<ProductMenuDTO> productMenuDTOList)
        {
            return await Task<List<ProductMenuDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(productMenuDTOList);
                using(ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                {
                    parafaitDBTransaction.BeginTransaction();
                    ProductMenuListBL productMenuListBL = new ProductMenuListBL(executionContext);
                    List<ProductMenuDTO> result = productMenuListBL.Save(productMenuDTOList, parafaitDBTransaction.SQLTrx);
                    parafaitDBTransaction.EndTransaction();
                    log.LogMethodExit(result);
                    return result;
                }
            });
        }

        public async Task<List<ProductMenuPanelDTO>> SaveProductMenuPanelDTOList(List<ProductMenuPanelDTO> productMenuPanelDTOList)
        {
            return await Task<List<ProductMenuPanelDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(productMenuPanelDTOList);
                using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                {
                    parafaitDBTransaction.BeginTransaction();
                    ProductMenuPanelListBL productMenuPanelListBL = new ProductMenuPanelListBL(executionContext);
                    List<ProductMenuPanelDTO> result = productMenuPanelListBL.Save(productMenuPanelDTOList, parafaitDBTransaction.SQLTrx);
                    parafaitDBTransaction.EndTransaction();
                    log.LogMethodExit(result);
                    return result;
                }
            });
        }

        public async Task<string> AddProductMenuPanelContentDTOList(int panelId, List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(panelId, productMenuPanelContentDTOList);
                using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                {
                    parafaitDBTransaction.BeginTransaction();
                    ProductMenuPanelBL productMenuPanelBL = new ProductMenuPanelBL(executionContext, panelId, true, true, parafaitDBTransaction.SQLTrx);
                    productMenuPanelBL.AddProductMenuPanelContentDTOList(productMenuPanelContentDTOList);
                    productMenuPanelBL.Save(parafaitDBTransaction.SQLTrx);
                    parafaitDBTransaction.EndTransaction();
                    log.LogMethodExit("Success");
                    return "Success";
                }
            });
        }

        public async Task<ProductMenuContainerSnapshotDTOCollection> GetProductMenuContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache)
        {
            return await Task<ProductMenuContainerSnapshotDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, userRoleId, posMachineId, languageId, menuType, startDateTime, endDateTime, hash, rebuildCache);
                if (rebuildCache)
                {
                    ProductMenuContainerList.Rebuild(siteId);
                }
                ProductMenuContainerSnapshotDTOCollection result = ProductMenuContainerList.GetProductMenuContainerSnapshotDTOCollection(siteId, posMachineId, userRoleId, languageId, menuType, startDateTime, endDateTime);
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
