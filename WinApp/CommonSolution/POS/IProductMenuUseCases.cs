/********************************************************************************************
 * Project Name - POS
 * Description  - Specification of product menu use cases
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public interface IProductMenuUseCases
    {
        Task<List<ProductMenuPanelDTO>> GetProductMenuPanelDTOList(string isActive = null, 
                                                             int panelId = -1, 
                                                             string name = "", 
                                                             int siteId = -1,
                                                             bool loadChildRecords = true,
                                                             bool loadActiveChildRecords = true, 
                                                             string guid = "");
        Task<List<ProductMenuDTO>> GetProductMenuDTOList(string isActive = null, 
                                                       int menuId = -1, 
                                                       string name = "", 
                                                       DateTime? endDateGreaterThanEqualTo = null, 
                                                       DateTime? startDateLessThanEqualTo = null, 
                                                       int siteId = -1,
                                                       bool loadChildRecords = true, 
                                                       bool loadActiveChildRecords = true);

        Task<List<ProductMenuPanelDTO>> SaveProductMenuPanelDTOList(List<ProductMenuPanelDTO> productMenuPanelDTOList);
        Task<List<ProductMenuDTO>> SaveProductMenuDTOList(List<ProductMenuDTO> productMenuDTOList);
        Task<string> AddProductMenuPanelContentDTOList(int panelId, List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList);
        Task<ProductMenuContainerSnapshotDTOCollection> GetProductMenuContainerSnapshotDTOCollection(int siteId, int userRoleId, int posMachineId, int languageId, string menuType, DateTime startDateTime, DateTime endDateTime, string hash, bool rebuildCache);
    }
}
