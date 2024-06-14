/********************************************************************************************
 * Project Name - Inventory                                                                    
 * Description  - InventoryDocumentTypeContainerDTOCollection class.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.150.0      08-Sep-2022   Abhishek             Created : Inventory UI redesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class InventoryDocumentTypeContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<InventoryDocumentTypeContainerDTO> inventoryDocumentTypeContainerDTOList;
        private string hash;

        public InventoryDocumentTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            inventoryDocumentTypeContainerDTOList = new List<InventoryDocumentTypeContainerDTO>();
            log.LogMethodExit();
        }

        public InventoryDocumentTypeContainerDTOCollection(List<InventoryDocumentTypeContainerDTO> inventoryDocumentTypeContainerDTOList)
        {
            log.LogMethodEntry(inventoryDocumentTypeContainerDTOList);
            this.inventoryDocumentTypeContainerDTOList = inventoryDocumentTypeContainerDTOList;
            if (inventoryDocumentTypeContainerDTOList == null)
            {
                inventoryDocumentTypeContainerDTOList = new List<InventoryDocumentTypeContainerDTO>();
            }
            hash = new DtoListHash(inventoryDocumentTypeContainerDTOList);
            log.LogMethodExit();
        }

        public List<InventoryDocumentTypeContainerDTO> InventoryDocumentTypeContainerDTOList
        {
            get
            {
                return inventoryDocumentTypeContainerDTOList;
            }

            set
            {
                inventoryDocumentTypeContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }

    }
}
