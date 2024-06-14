/********************************************************************************************
 * Project Name - Inventory
 * Description  - InventoryDocumentTypeContainer class to get the List of Document Types from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      18-Aug-2022      Abhishek           Created : Web Inventory Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    public class InventoryDocumentTypeContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, InventoryDocumentTypeContainerDTO> inventoryDocumentTypeContainerDTODictionary = new Dictionary<int, InventoryDocumentTypeContainerDTO>();
        private readonly List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList;
        private readonly InventoryDocumentTypeContainerDTOCollection inventoryDocumentTypeContainerDTOCollection;
        private readonly DateTime? languageModuleLastUpdateTime;
        private readonly int siteId;

        public InventoryDocumentTypeContainer(int siteId) : this(siteId, GetInventoryDocumentTypeDTOList(siteId), GetInventoryDocumentTypeModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public InventoryDocumentTypeContainer(int siteId, List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList, DateTime? inventoryDocumentTypeModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId, inventoryDocumentTypeDTOList, inventoryDocumentTypeModuleLastUpdateTime);
            this.siteId = siteId;
            this.languageModuleLastUpdateTime = inventoryDocumentTypeModuleLastUpdateTime;
            this.inventoryDocumentTypeDTOList = inventoryDocumentTypeDTOList;

            List<InventoryDocumentTypeContainerDTO> inventoryDocumentTypeContainerDTOList = new List<InventoryDocumentTypeContainerDTO>();
            if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Any())
            {
                foreach (InventoryDocumentTypeDTO inventoryDocumentTypeDTO in inventoryDocumentTypeDTOList)
                {
                    InventoryDocumentTypeContainerDTO inventoryDocumentTypeContainerDTO = new InventoryDocumentTypeContainerDTO(inventoryDocumentTypeDTO.DocumentTypeId,
                           inventoryDocumentTypeDTO.Name, inventoryDocumentTypeDTO.Description, inventoryDocumentTypeDTO.Applicability, inventoryDocumentTypeDTO.Code);
                    inventoryDocumentTypeContainerDTOList.Add(inventoryDocumentTypeContainerDTO);
                    inventoryDocumentTypeContainerDTODictionary.Add(inventoryDocumentTypeDTO.DocumentTypeId, inventoryDocumentTypeContainerDTO);
                }
            }
            inventoryDocumentTypeContainerDTOCollection = new InventoryDocumentTypeContainerDTOCollection(inventoryDocumentTypeContainerDTOList);
            log.Info("Number of items loaded by InventoryDocumentTypeContainer " + siteId + ":" + inventoryDocumentTypeDTOList.Count);
            log.LogMethodExit();
        }

        private static List<InventoryDocumentTypeDTO> GetInventoryDocumentTypeDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = null;
            try
            {
                InventoryDocumentTypeList inventoryDocumentTypeListBL = new InventoryDocumentTypeList();
                List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> searchParameters = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
                searchParameters.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, siteId.ToString()));
                inventoryDocumentTypeDTOList = inventoryDocumentTypeListBL.GetAllInventoryDocumentTypes(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the InventoryDocumentType.", ex);
            }
            if (inventoryDocumentTypeDTOList == null)
            {
                inventoryDocumentTypeDTOList = new List<InventoryDocumentTypeDTO>();
            }
            log.LogMethodExit(inventoryDocumentTypeDTOList);
            return inventoryDocumentTypeDTOList;
        }

        private static DateTime? GetInventoryDocumentTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList();
                result = inventoryDocumentTypeList.GetInventoryDocumentTypeModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the InventoryDocumentType max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public InventoryDocumentTypeContainerDTOCollection GetInventoryDocumentTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(inventoryDocumentTypeContainerDTOCollection);
            return inventoryDocumentTypeContainerDTOCollection;
        }

        public InventoryDocumentTypeContainerDTO GetInventoryDocumentTypeContainerDTO(int documentTypeId)
        {
            log.LogMethodEntry(documentTypeId);
            if (inventoryDocumentTypeContainerDTODictionary.ContainsKey(documentTypeId) == false)
            {
                string errorMessage = "InventoryDocumentType with DocumentType Id :" + documentTypeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            InventoryDocumentTypeContainerDTO result = inventoryDocumentTypeContainerDTODictionary[documentTypeId]; ;
            log.LogMethodExit(result);
            return result;
        }

        public InventoryDocumentTypeContainer Refresh()
        {
            log.LogMethodEntry();
            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList();
            DateTime? updateTime = inventoryDocumentTypeList.GetInventoryDocumentTypeModuleLastUpdateTime(siteId);
            if (languageModuleLastUpdateTime.HasValue
                && languageModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in InventoryDocumentType since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            InventoryDocumentTypeContainer result = new InventoryDocumentTypeContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
