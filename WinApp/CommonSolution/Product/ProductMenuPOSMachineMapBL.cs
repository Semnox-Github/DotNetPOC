/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu POS mapping Business object
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
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPOSMachineMapBL
    {
        private ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ProductMenuPOSMachineMapBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ProductMenuPOSMachineMapBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productMenuPOSMachineMapDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the userId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductMenuPOSMachineMapBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ProductMenuPOSMachineMapDataHandler productMenuPOSMachineMapDataHandler = new ProductMenuPOSMachineMapDataHandler(sqlTransaction);
            productMenuPOSMachineMapDTO = productMenuPOSMachineMapDataHandler.GetProductMenuPOSMachineMap(id);
            if (productMenuPOSMachineMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductMenuPOSMachineMap", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates ProductMenuPOSMachineMapBL object using the ProductMenuPOSMachineMapDTO
        /// </summary>
        /// <param name="productMenuPOSMachineMapDTO">ProductMenuPOSMachineMapDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public ProductMenuPOSMachineMapBL(ExecutionContext executionContext, ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productMenuPOSMachineMapDTO);
            if (productMenuPOSMachineMapDTO.Id < 0)
            {
                ValidateMenuId(productMenuPOSMachineMapDTO.MenuId);
                ValidatePOSMachineId(productMenuPOSMachineMapDTO.POSMachineId);
            }
            this.productMenuPOSMachineMapDTO = productMenuPOSMachineMapDTO;
            log.LogMethodExit();
        }

        public void Update(ProductMenuPOSMachineMapDTO parameterProductMenuPOSMachineMapDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterProductMenuPOSMachineMapDTO, sqlTransaction);
            ChangeMenuId(parameterProductMenuPOSMachineMapDTO.MenuId);
            ChangePOSMachineId(parameterProductMenuPOSMachineMapDTO.POSMachineId);
            ChangeIsActive(parameterProductMenuPOSMachineMapDTO.IsActive);
            log.LogMethodExit();

        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (productMenuPOSMachineMapDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPOSMachineMap isActive");
                return;
            }
            productMenuPOSMachineMapDTO.IsActive = isActive;
            log.LogMethodExit();
        }
   
        public void ChangeMenuId(int menuId)
        {
            log.LogMethodEntry(menuId);
            if (productMenuPOSMachineMapDTO.MenuId == menuId)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPOSMachineMap menuId");
                return;
            }
            ValidateMenuId(menuId);
            productMenuPOSMachineMapDTO.MenuId = menuId;
            log.LogMethodExit();
        }


        public void ChangePOSMachineId(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            if (productMenuPOSMachineMapDTO.POSMachineId == posMachineId)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPOSMachineMap posMachineId");
                return;
            }
            productMenuPOSMachineMapDTO.POSMachineId = posMachineId;
            log.LogMethodExit();
        }

        public void ValidatePOSMachineId(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            if (posMachineId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "POS Machine"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("posMachineId is empty.", "ProductMenuPOSMachineMap", "POSMachineId", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateMenuId(int menuId)
        {
            log.LogMethodEntry(menuId);
            if (menuId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "menuId"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("menuId is empty.", "ProductMenuPOSMachineMap", "menuId", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProductMenuPOSMachineMap
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductMenuPOSMachineMapDataHandler productMenuPOSMachineMapDataHandler = new ProductMenuPOSMachineMapDataHandler(sqlTransaction);
            if (productMenuPOSMachineMapDTO.Id < 0)
            {
                productMenuPOSMachineMapDTO = productMenuPOSMachineMapDataHandler.Insert(productMenuPOSMachineMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productMenuPOSMachineMapDTO.AcceptChanges();
            }
            else
            {
                if (productMenuPOSMachineMapDTO.IsChanged)
                {
                    productMenuPOSMachineMapDTO = productMenuPOSMachineMapDataHandler.Update(productMenuPOSMachineMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productMenuPOSMachineMapDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductMenuPOSMachineMapDTO ProductMenuPOSMachineMapDTO
        {
            get
            {
                return productMenuPOSMachineMapDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ProductMenuPOSMachineMap
    /// </summary>
    public class ProductMenuPOSMachineMapListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductMenuPOSMachineMapDTO> productMenuPOSMachineMapDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ProductMenuPOSMachineMapListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ProductMenuPOSMachineMapListBL()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="ProductMenuPOSMachineMapToList">ProductMenuPOSMachineMapToList</param>
        /// <param name="executionContext">executionContext</param>
        public ProductMenuPOSMachineMapListBL(ExecutionContext executionContext, List<ProductMenuPOSMachineMapDTO> productMenuPOSMachineMapDTOList) : this()
        {
            log.LogMethodEntry(executionContext, productMenuPOSMachineMapDTOList);
            this.executionContext = executionContext;
            this.productMenuPOSMachineMapDTOList = productMenuPOSMachineMapDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save and Updated the ProductMenuPOSMachineMaps details
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productMenuPOSMachineMapDTOList == null ||
                productMenuPOSMachineMapDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < productMenuPOSMachineMapDTOList.Count; i++)
            {
                var productMenuPOSMachineMapDTO = productMenuPOSMachineMapDTOList[i];
                if (productMenuPOSMachineMapDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ProductMenuPOSMachineMapBL productMenuPOSMachineMapBL = new ProductMenuPOSMachineMapBL(executionContext, productMenuPOSMachineMapDTO);
                    productMenuPOSMachineMapBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving ProductMenuPOSMachineMapDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ProductMenuPOSMachineMapDTO", productMenuPOSMachineMapDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductMenuPOSMachineMapDTO List for POSMachineIdList
        /// </summary>
        /// <param name="posMachineIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductMenuPOSMachineMapDTO</returns>
        public List<ProductMenuPOSMachineMapDTO> GetProductMenuPanelMappingDTOList(List<int> posMachineIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(posMachineIdList, activeRecords, sqlTransaction);
            ProductMenuPOSMachineMapDataHandler productMenuPOSMachineMapDataHandler = new ProductMenuPOSMachineMapDataHandler(sqlTransaction);
            List<ProductMenuPOSMachineMapDTO> productMenuPOSMachineMapDTOList = productMenuPOSMachineMapDataHandler.GetProductMenuPOSMachineMapDTOList(posMachineIdList, activeRecords);
            log.LogMethodExit(productMenuPOSMachineMapDTOList);
            return productMenuPOSMachineMapDTOList;
        }
    }
}