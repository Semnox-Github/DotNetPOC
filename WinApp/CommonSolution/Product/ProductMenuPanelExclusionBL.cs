/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel exclusion Business object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 ********************************************************************************************* 
 *2.130.0     19-Jul-2021      Lakshminarayana       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelExclusionBL
    {
        private ProductMenuPanelExclusionDTO productMenuPanelExclusionDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ProductMenuPanelExclusionBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ProductMenuPanelExclusionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productMenuPanelExclusionDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the userId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductMenuPanelExclusionBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ProductMenuPanelExclusionDataHandler productMenuPanelExclusionDataHandler = new ProductMenuPanelExclusionDataHandler(sqlTransaction);
            productMenuPanelExclusionDTO = productMenuPanelExclusionDataHandler.GetProductMenuPanelExclusion(id);
            if (productMenuPanelExclusionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductMenuPanelExclusion", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates ProductMenuPanelExclusionBL object using the ProductMenuPanelExclusionDTO
        /// </summary>
        /// <param name="productMenuPanelExclusionDTO">ProductMenuPanelExclusionDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public ProductMenuPanelExclusionBL(ExecutionContext executionContext, ProductMenuPanelExclusionDTO productMenuPanelExclusionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productMenuPanelExclusionDTO);
            if (productMenuPanelExclusionDTO.Id < 0)
            {
                ValidatePanelId(productMenuPanelExclusionDTO.PanelId);
                ValidatePOSMachineId(productMenuPanelExclusionDTO.POSMachineId);
                ValidateUserRoleId(productMenuPanelExclusionDTO.UserRoleId);
                ValidatePOSTypeId(productMenuPanelExclusionDTO.POSTypeId);
            }
            this.productMenuPanelExclusionDTO = productMenuPanelExclusionDTO;
            log.LogMethodExit();
        }

        public void Update(ProductMenuPanelExclusionDTO parameterProductMenuPanelExclusionDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterProductMenuPanelExclusionDTO, sqlTransaction);
            ChangePanelId(parameterProductMenuPanelExclusionDTO.PanelId);
            ChangePOSMachineId(parameterProductMenuPanelExclusionDTO.POSMachineId);
            ChangeUserRoleId(parameterProductMenuPanelExclusionDTO.UserRoleId);
            ChangePOSTypeId(parameterProductMenuPanelExclusionDTO.POSTypeId);
            ChangeIsActive(parameterProductMenuPanelExclusionDTO.IsActive);
            log.LogMethodExit();

        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (productMenuPanelExclusionDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelExclusion isActive");
                return;
            }
            productMenuPanelExclusionDTO.IsActive = isActive;
            log.LogMethodExit();
        }
   
        public void ChangePanelId(int panelId)
        {
            log.LogMethodEntry(panelId);
            if (productMenuPanelExclusionDTO.PanelId == panelId)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelExclusion panelId");
                return;
            }
            ValidatePanelId(panelId);
            productMenuPanelExclusionDTO.PanelId = panelId;
            log.LogMethodExit();
        }

        private void ValidatePanelId(int panelId)
        {
            log.LogMethodEntry(panelId);
            if (panelId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "menuId"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("menuId is empty.", "ProductMenuPanelExclusion", "panelId", errorMessage);
            }
            log.LogMethodExit();
        }


        public void ChangePOSMachineId(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            if (productMenuPanelExclusionDTO.POSMachineId == posMachineId)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelExclusion posMachineId");
                return;
            }
            ValidatePOSMachineId(posMachineId);
            productMenuPanelExclusionDTO.POSMachineId = posMachineId;
            log.LogMethodExit();
        }

        public void ValidatePOSMachineId(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            //if (posMachineId == -1)
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "POS Machine"));
            //    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            //    throw new ValidationException("posMachineId is empty.", "ProductMenuPanelExclusion", "POSMachineId", errorMessage);
            //}
            log.LogMethodExit();
        }

        public void ChangeUserRoleId(int userRoleId)
        {
            log.LogMethodEntry(userRoleId);
            if (productMenuPanelExclusionDTO.UserRoleId == userRoleId)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelExclusion userRoleId");
                return;
            }
            ValidateUserRoleId(userRoleId);
            productMenuPanelExclusionDTO.UserRoleId = userRoleId;
            log.LogMethodExit();
        }

        public void ValidateUserRoleId(int userRoleId)
        {
            log.LogMethodEntry(userRoleId);
            //if (userRoleId == -1)
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "POS Machine"));
            //    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            //    throw new ValidationException("posMachineId is empty.", "ProductMenuPanelExclusion", "POSMachineId", errorMessage);
            //}
            log.LogMethodExit();
        }

        public void ChangePOSTypeId(int posTypeId)
        {
            log.LogMethodEntry(posTypeId);
            if (productMenuPanelExclusionDTO.UserRoleId == posTypeId)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelExclusion userRoleId");
                return;
            }
            ValidatePOSTypeId(posTypeId);
            productMenuPanelExclusionDTO.UserRoleId = posTypeId;
            log.LogMethodExit();
        }

        public void ValidatePOSTypeId(int posTypeId)
        {
            log.LogMethodEntry(posTypeId);
            //if (posTypeId == -1)
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "POS Machine"));
            //    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            //    throw new ValidationException("posMachineId is empty.", "ProductMenuPanelExclusion", "POSMachineId", errorMessage);
            //}
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProductMenuPanelExclusion
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductMenuPanelExclusionDataHandler productMenuPanelExclusionDataHandler = new ProductMenuPanelExclusionDataHandler(sqlTransaction);
            if (productMenuPanelExclusionDTO.Id < 0)
            {
                productMenuPanelExclusionDTO = productMenuPanelExclusionDataHandler.Insert(productMenuPanelExclusionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productMenuPanelExclusionDTO.AcceptChanges();
            }
            else
            {
                if (productMenuPanelExclusionDTO.IsChanged)
                {
                    productMenuPanelExclusionDTO = productMenuPanelExclusionDataHandler.Update(productMenuPanelExclusionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productMenuPanelExclusionDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductMenuPanelExclusionDTO ProductMenuPanelExclusionDTO
        {
            get
            {
                return productMenuPanelExclusionDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ProductMenuPanelExclusion
    /// </summary>
    public class ProductMenuPanelExclusionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ProductMenuPanelExclusionListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ProductMenuPanelExclusionListBL()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="ProductMenuPanelExclusionToList">ProductMenuPanelExclusionToList</param>
        /// <param name="executionContext">executionContext</param>
        public ProductMenuPanelExclusionListBL(ExecutionContext executionContext, List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList) : this()
        {
            log.LogMethodEntry(executionContext, productMenuPanelExclusionDTOList);
            this.executionContext = executionContext;
            this.productMenuPanelExclusionDTOList = productMenuPanelExclusionDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save and Updated the ProductMenuPanelExclusions details
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productMenuPanelExclusionDTOList == null ||
                productMenuPanelExclusionDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < productMenuPanelExclusionDTOList.Count; i++)
            {
                var productMenuPanelExclusionDTO = productMenuPanelExclusionDTOList[i];
                if (productMenuPanelExclusionDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ProductMenuPanelExclusionBL productMenuPanelExclusionBL = new ProductMenuPanelExclusionBL(executionContext, productMenuPanelExclusionDTO);
                    productMenuPanelExclusionBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving ProductMenuPanelExclusionDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ProductMenuPanelExclusionDTO", productMenuPanelExclusionDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductMenuPanelExclusionDTO List for POSMachineIdList
        /// </summary>
        /// <param name="posMachineIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductMenuPanelExclusionDTO</returns>
        public List<ProductMenuPanelExclusionDTO> GetProductMenuPanelExclusionDTOListForPOSMachines(List<int> posMachineIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(posMachineIdList, activeRecords, sqlTransaction);
            ProductMenuPanelExclusionDataHandler productMenuPanelExclusionDataHandler = new ProductMenuPanelExclusionDataHandler(sqlTransaction);
            List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList = productMenuPanelExclusionDataHandler.GetProductMenuPanelExclusionDTOListForPOSMachines(posMachineIdList, activeRecords);
            log.LogMethodExit(productMenuPanelExclusionDTOList);
            return productMenuPanelExclusionDTOList;
        }

        /// <summary>
        /// Gets the ProductMenuPanelExclusionDTO List for userRoleIdList
        /// </summary>
        /// <param name="userRoleIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductMenuPanelExclusionDTO</returns>
        public List<ProductMenuPanelExclusionDTO> GetProductMenuPanelExclusionDTOListForUserRoles(List<int> userRoleIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userRoleIdList, activeRecords, sqlTransaction);
            ProductMenuPanelExclusionDataHandler productMenuPanelExclusionDataHandler = new ProductMenuPanelExclusionDataHandler(sqlTransaction);
            List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList = productMenuPanelExclusionDataHandler.GetProductMenuPanelExclusionDTOListForUserRoles(userRoleIdList, activeRecords);
            log.LogMethodExit(productMenuPanelExclusionDTOList);
            return productMenuPanelExclusionDTOList;
        }

        /// <summary>
        /// Gets the ProductMenuPanelExclusionDTO List for userRoleIdList
        /// </summary>
        /// <param name="posTypeIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductMenuPanelExclusionDTO</returns>
        public List<ProductMenuPanelExclusionDTO> GetProductMenuPanelExclusionDTOListForPosTypes(List<int> posTypeIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(posTypeIdList, activeRecords, sqlTransaction);
            ProductMenuPanelExclusionDataHandler productMenuPanelExclusionDataHandler = new ProductMenuPanelExclusionDataHandler(sqlTransaction);
            List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList = productMenuPanelExclusionDataHandler.GetProductMenuPanelExclusionDTOListForPosTypes(posTypeIdList, activeRecords);
            log.LogMethodExit(productMenuPanelExclusionDTOList);
            return productMenuPanelExclusionDTOList;
        }
    }
}