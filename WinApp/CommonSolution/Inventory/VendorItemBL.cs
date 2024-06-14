/********************************************************************************************
 * Project Name - Inventory
 * Description  -Business logic -VendorItemBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        03-Jun-2019   Girish Kundar           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class VendorItemBL
    {
        private VendorItemDTO vendorItemDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="vendorItemDTO"></param>
        public VendorItemBL(ExecutionContext executionContext, VendorItemDTO vendorItemDTO)
        {
            log.LogMethodEntry(executionContext, vendorItemDTO);
            this.executionContext = executionContext;
            this.vendorItemDTO = vendorItemDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="Id">Parameter of the type Id</param>
        public VendorItemBL(ExecutionContext executionContext,int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(vendorItemDTO);
            VendorItemDataHandler vendorItemDataHandler = new VendorItemDataHandler(sqlTransaction);
            this.vendorItemDTO = vendorItemDataHandler.GetVendorItemDTO(id, sqlTransaction);
            log.LogMethodExit(vendorItemDTO);
        }

        /// <summary>
        /// Saves the VendorItemBL
        /// ads will be inserted if ads is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            VendorItemDataHandler vendorItemDataHandler = new VendorItemDataHandler(sqlTransaction);

            if (vendorItemDTO.VendorItemId < 0)
            {
                vendorItemDTO = vendorItemDataHandler.Insert(vendorItemDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                vendorItemDTO.AcceptChanges();
            }
            else
            {
                if (vendorItemDTO.IsChanged == true)
                {
                    vendorItemDTO = vendorItemDataHandler.Update(vendorItemDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    vendorItemDTO.AcceptChanges();
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// get vendorItemDTO Object
        /// </summary>
        public VendorItemDTO GetVendorItemDTO
        {
            get { return vendorItemDTO; }
        }
    }
    /// <summary>
    /// Manages the list of VendorItem
    /// </summary>
    public class VendorItemListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<VendorItemDTO> vendorItemDTOList;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public VendorItemListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            vendorItemDTOList = new List<VendorItemDTO>();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the VendorItemBL list
        /// </summary>
        public List<VendorItemDTO> GetVendorItemDTOList(List<KeyValuePair<VendorItemDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            VendorItemDataHandler vendorItemDataHandler = new VendorItemDataHandler(sqlTransaction);
            List<VendorItemDTO> vendorItemDTOList = vendorItemDataHandler.GetVendorItemDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(vendorItemDTOList);
            return vendorItemDTOList;
        }
    }
}
