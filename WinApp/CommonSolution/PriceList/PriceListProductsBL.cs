/********************************************************************************************
 * Project Name - PriceListProducts BL
 * Description  - Business Logic  
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By        Remarks          
 ********************************************************************************************* 
 *2.60        06-Feb-2019    Indrajeet Kumar    Created
 *2.60        24-Mar-2019    Nagesh Badiger     Added log method entry and method exit
 *2.70        29-Jun-2019    Akshay Gulaganji   Added DeletePriceListProducts() method
 *2.70.2        30-Jul-2019    Deeksha            Modifications as per three tier standard.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.PriceList
{
    public class PriceListProductsBL
    {
        private PriceListProductsDTO priceListProductsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public PriceListProductsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.priceListProductsDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="priceListProductsDTO"></param>
        public PriceListProductsBL(ExecutionContext executionContext, PriceListProductsDTO priceListProductsDTO)
        {
            log.LogMethodEntry(executionContext, priceListProductsDTO);
            this.executionContext = executionContext;
            this.priceListProductsDTO = priceListProductsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the rolePriceListId as the parameter
        /// Would fetch the UserRolePriceListDTO object from the database based on the RolePriceListId id passed. 
        /// </summary>
        /// <param name="rolePriceListId">PriceList id </param>
        public PriceListProductsBL(ExecutionContext executionContext, int priceListProductId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, priceListProductId, sqlTransaction);
            PriceListProductsDataHandler priceListProductsDataHandler = new PriceListProductsDataHandler(sqlTransaction);
            priceListProductsDTO = priceListProductsDataHandler.GetPriceListProductsId(priceListProductId);
            if (priceListProductsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PriceListProducts", priceListProductId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PriceListProductsDTO PriceListProductsDTO
        {
            get
            {
                return priceListProductsDTO;
            }
        }
    

    /// <summary>
    /// Save PriceList Products DataHandler
    /// </summary>
    /// <param name="sqlTransaction">sqlTransaction</param>
    public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            PriceListProductsDataHandler priceListProductsDataHandler = new PriceListProductsDataHandler(sqlTransaction);
            if (priceListProductsDTO.PriceListProductId < 0)
            {
                priceListProductsDTO = priceListProductsDataHandler.InsertPriceListProducts(priceListProductsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                priceListProductsDTO.AcceptChanges();
            }
            else
            {
                if (priceListProductsDTO.PriceListProductId > 0 && priceListProductsDTO.IsChanged )
                {
                    priceListProductsDTO = priceListProductsDataHandler.UpdatePriceListProducts(priceListProductsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    priceListProductsDTO.AcceptChanges();
                }
            }
            /// Below code is to save the Audit log details into DBAuditLog            
            if (!string.IsNullOrEmpty(priceListProductsDTO.Guid))
            {
                AuditLog auditLog = new AuditLog(executionContext);
                auditLog.AuditTable("PriceListProducts", priceListProductsDTO.Guid, sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Deletes the PriceListProducts based on PriceListProductId
        /// </summary>   
        /// <param name="priceListProductId">priceListProductId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void DeletePriceListProducts(int priceListProductId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(priceListProductId, sqlTransaction);
            try
            {
                PriceListProductsDataHandler priceListProductsDataHandler = new PriceListProductsDataHandler(sqlTransaction);
                priceListProductsDataHandler.DeletePriceListProducts(priceListProductId);
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }

    public class PriceListProductsBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PriceListProductsDTO> priceListProductsList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public PriceListProductsBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.priceListProductsList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="priceListProductsList">priceListProductsList</param>
        /// <param name="executionContext">executionContext</param>
        public PriceListProductsBLList(List<PriceListProductsDTO> priceListProductsList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(priceListProductsList, executionContext);
            this.executionContext = executionContext;
            this.priceListProductsList = priceListProductsList;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetPriceListProductsList
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>priceListProductsDTOs</returns>
        public List<PriceListProductsDTO> GetPriceListProductsList(List<KeyValuePair<PriceListProductsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PriceListProductsDataHandler priceListProductsDataHandler = new PriceListProductsDataHandler(sqlTransaction);
            List<PriceListProductsDTO> priceListProductsDTOs = new List<PriceListProductsDTO>();
            priceListProductsDTOs = priceListProductsDataHandler.GetAllPriceListProductsList(searchParameters,sqlTransaction); 
            log.LogMethodExit(priceListProductsDTOs);
            return priceListProductsDTOs;
        }

        /// <summary>
        /// Save or Update PriceListProductsList
        /// </summary>
        public void SaveUpdatePriceListProductsList()
        {
            try
            {
                log.LogMethodEntry();
                if (priceListProductsList != null)
                {
                    foreach (PriceListProductsDTO priceListProductsDTO in priceListProductsList)
                    {
                        PriceListProductsBL priceListProductsBLObj = new PriceListProductsBL(executionContext, priceListProductsDTO);
                        priceListProductsBLObj.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
