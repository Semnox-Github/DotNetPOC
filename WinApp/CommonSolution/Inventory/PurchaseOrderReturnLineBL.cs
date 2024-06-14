/********************************************************************************************
 * Project Name - Inventory
 * Description  -Business logic -PurchaseOrderReturnLineBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        03-Jun-2019   Girish Kundar           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory
{
    public class PurchaseOrderReturnLineBL
    {
        private PurchaseOrderReturnLineDTO purchaseOrderReturnLineDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="purchaseOrderReturnLineDTO"></param>
        public PurchaseOrderReturnLineBL(ExecutionContext executionContext, PurchaseOrderReturnLineDTO purchaseOrderReturnLineDTO)
        {
            log.LogMethodEntry(executionContext, purchaseOrderReturnLineDTO);
            this.executionContext = executionContext;
            this.purchaseOrderReturnLineDTO = purchaseOrderReturnLineDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="PurchaseOrderReturnLineId">Parameter of the type PurchaseOrderReturnLine id</param>
        public PurchaseOrderReturnLineBL(ExecutionContext executionContext,int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id,purchaseOrderReturnLineDTO);
            PurchaseOrderReturnLineDataHandler purchaseOrderReturnLineDataHandler = new PurchaseOrderReturnLineDataHandler(sqlTransaction);
            this.purchaseOrderReturnLineDTO = purchaseOrderReturnLineDataHandler.GetPurchaseOrderReturnLineDTO(id, sqlTransaction);
            log.LogMethodExit(purchaseOrderReturnLineDTO);
        }

        /// <summary>
        /// Saves the purchaseOrderReturnLineDTO
        /// ads will be inserted if ads is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            PurchaseOrderReturnLineDataHandler purchaseOrderReturnLineDataHandler = new PurchaseOrderReturnLineDataHandler(sqlTransaction);

            if (purchaseOrderReturnLineDTO.PurchaseOrderReturnLineId < 0)
            {
                purchaseOrderReturnLineDTO = purchaseOrderReturnLineDataHandler.Insert(purchaseOrderReturnLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                purchaseOrderReturnLineDTO.AcceptChanges();
            }
            else
            {
                if (purchaseOrderReturnLineDTO.IsChanged == true)
                {
                    purchaseOrderReturnLineDTO = purchaseOrderReturnLineDataHandler.Update(purchaseOrderReturnLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    purchaseOrderReturnLineDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get PurchaseOrderReturnLineDTO Object
        /// </summary>
        public PurchaseOrderReturnLineDTO PurchaseOrderReturnLineDTO
        {
            get { return purchaseOrderReturnLineDTO; }
        }
    }
    /// <summary>
    /// Manages the list of Ads
    /// </summary>
    public class PurchaseOrderReturnLineBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PurchaseOrderReturnLineDTO> purchaseOrderReturnLineDTO;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public PurchaseOrderReturnLineBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            purchaseOrderReturnLineDTO = new List<PurchaseOrderReturnLineDTO>();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the PurchaseOrderReturnLine DTO list
        /// </summary>
        public List<PurchaseOrderReturnLineDTO> GetPurchaseOrderReturnLineDTOList(List<KeyValuePair<PurchaseOrderReturnLineDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PurchaseOrderReturnLineDataHandler purchaseOrderReturnLineDataHandler = new PurchaseOrderReturnLineDataHandler(sqlTransaction);
            List<PurchaseOrderReturnLineDTO> purchaseOrderReturnLineDTOList = purchaseOrderReturnLineDataHandler.GetPurchaseOrderReturnLineDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(purchaseOrderReturnLineDTOList);
            return purchaseOrderReturnLineDTOList;
        }

    }
}
