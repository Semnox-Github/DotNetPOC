/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel mapping Business object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.130.0        27-May-2021      Prajwal S       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelMappingBL
    {
        ProductMenuPanelMappingDTO productMenuPanelMappingDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of ProductMenuPanelMappingBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ProductMenuPanelMappingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productMenuPanelMappingDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductMenuPanelMappingId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductMenuPanelMappingBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ProductMenuPanelMappingDataHandler productMenuPanelMappingDataHandler = new ProductMenuPanelMappingDataHandler(sqlTransaction);
            productMenuPanelMappingDTO = productMenuPanelMappingDataHandler.GetProductMenuPanelMapping(id);
            if (productMenuPanelMappingDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductMenuPanelMapping", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates ProductMenuPanelMappingBL object using the ProductMenuPanelMappingDTO
        /// </summary>
        /// <param name="productMenuPanelMappingDTO">ProductMenuPanelMappingDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public ProductMenuPanelMappingBL(ExecutionContext executionContext, ProductMenuPanelMappingDTO productMenuPanelMappingDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productMenuPanelMappingDTO);
            this.productMenuPanelMappingDTO = productMenuPanelMappingDTO;
            log.LogMethodExit();
        }


        public void Update(ProductMenuPanelMappingDTO parameterproductMenuPanelMappingDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterproductMenuPanelMappingDTO, sqlTransaction);
            ChangePanelId(parameterproductMenuPanelMappingDTO.PanelId);
            ChangeMenuId(parameterproductMenuPanelMappingDTO.MenuId);
            ChangeIsActive(parameterproductMenuPanelMappingDTO.IsActive);
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (productMenuPanelMappingDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to productMenuPanelMapping IsActive");
                return;
            }
            productMenuPanelMappingDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangePanelId(int panelId)
        {
            log.LogMethodEntry(panelId);
            if (productMenuPanelMappingDTO.PanelId == panelId)
            {
                log.LogMethodExit(null, "No changes to productMenuPanelMapping PanelId");
                return;
            }
            productMenuPanelMappingDTO.PanelId = panelId;
            log.LogMethodExit();
        }

        public void ChangeMenuId(int menuId)
        {
            log.LogMethodEntry(menuId);
            if (productMenuPanelMappingDTO.MenuId == menuId)
            {
                log.LogMethodExit(null, "No changes to productMenuPanelMapping MenuId");
                return;
            }
            productMenuPanelMappingDTO.MenuId = menuId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the productMenuPanelMapping
        /// Checks if the User id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductMenuPanelMappingDataHandler productMenuPanelMappingDataHandler = new ProductMenuPanelMappingDataHandler(sqlTransaction);
            if (productMenuPanelMappingDTO.Id < 0)
            {
                productMenuPanelMappingDTO = productMenuPanelMappingDataHandler.Insert(productMenuPanelMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productMenuPanelMappingDTO.AcceptChanges();
            }
            else
            {
                if (productMenuPanelMappingDTO.IsChanged)
                {
                    productMenuPanelMappingDTO = productMenuPanelMappingDataHandler.Update(productMenuPanelMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productMenuPanelMappingDTO.AcceptChanges();
                }
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductMenuPanelMappingDTO ProductMenuPanelMappingDTO
        {
            get
            {
                ProductMenuPanelMappingDTO result = new ProductMenuPanelMappingDTO(productMenuPanelMappingDTO);
                return result;
            }
        }
    }

    /// <summary>
    /// Manages the list of ProductMenuPanelMapping
    /// </summary>
    /// 
    public class ProductMenuPanelMappingListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductMenuPanelMappingListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public ProductMenuPanelMappingListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductMenuPanelMapping list
        /// </summary>
        public List<ProductMenuPanelMappingDTO> GetProductMenuPanelMappingDTOList(List<KeyValuePair<ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            ProductMenuPanelMappingDataHandler productMenuPanelMappingDataHandler = new ProductMenuPanelMappingDataHandler(sqlTransaction);
            List<ProductMenuPanelMappingDTO> productMenuPanelMappingDTOsList = productMenuPanelMappingDataHandler.GetProductMenuPanelMappingDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(productMenuPanelMappingDTOsList);
            return productMenuPanelMappingDTOsList;
        }

        /// This method should be used to Save and Update the ProductMenuPanelMapping.
        /// </summary>
        public List<ProductMenuPanelMappingDTO> Save(List<ProductMenuPanelMappingDTO> productMenuPanelMappingDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ProductMenuPanelMappingDTO> savedProductMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
            if (productMenuPanelMappingDTOList == null || productMenuPanelMappingDTOList.Any() == false)
            {
                log.LogMethodExit(savedProductMenuPanelMappingDTOList);
                return savedProductMenuPanelMappingDTOList;
            }
            foreach (ProductMenuPanelMappingDTO productMenuPanelMappingDTO in productMenuPanelMappingDTOList)
            {
                ProductMenuPanelMappingBL productMenuPanelMappingBL = new ProductMenuPanelMappingBL(executionContext, productMenuPanelMappingDTO);
                productMenuPanelMappingBL.Save(sqlTransaction);
                savedProductMenuPanelMappingDTOList.Add(productMenuPanelMappingBL.ProductMenuPanelMappingDTO);
            }
            log.LogMethodExit(savedProductMenuPanelMappingDTOList);
            return savedProductMenuPanelMappingDTOList;
        }

        /// <summary>
        /// Gets the ProductMenuPanelMappingDTO List for ProductMenuPanelMappingList
        /// </summary>
        /// <param name="menuIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductMenuPanelMappingDTO</returns>
        public List<ProductMenuPanelMappingDTO> GetProductMenuPanelMappingDTOList(List<int> menuIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(menuIdList, activeRecords, sqlTransaction);
            ProductMenuPanelMappingDataHandler productMenuPanelMappingDataHandler = new ProductMenuPanelMappingDataHandler(sqlTransaction);
            List<ProductMenuPanelMappingDTO> productMenuPanelMappingDTOList = productMenuPanelMappingDataHandler.GetProductMenuPanelMappingDTOList(menuIdList, activeRecords);
            log.LogMethodExit(productMenuPanelMappingDTOList);
            return productMenuPanelMappingDTOList;
        }

    }
}