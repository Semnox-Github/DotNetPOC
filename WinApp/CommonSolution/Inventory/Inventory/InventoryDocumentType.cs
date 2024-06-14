/********************************************************************************************
 * Project Name - Inventory Document Type
 * Description  - A high level structure created to classify the inventory document type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        08-Aug-2015   Raghuveera     Created
 *2.70.2      14-Jul-2019   Deeksha       Modifications as per three tier standard
 *2.150.0     18-Aug-2022   Abhishek      Modified : Added GetInventoryDocumentTypeModuleLastUpdateTime method to get LastUpdate DateTime
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.logging;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Business logic for the inventory document Type
    /// </summary>
    public class InventoryDocumentType
    {
        private InventoryDocumentTypeDTO inventoryDocumentTypeDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of InventoryDocumentType class
        /// </summary>
        public InventoryDocumentType()
        {
            log.LogMethodEntry();
            inventoryDocumentTypeDTO = null;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of inventoryDocumentType DTOs
    /// </summary>
    public class InventoryDocumentTypeList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public InventoryDocumentTypeList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public InventoryDocumentTypeList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the inventoryDocumentType list
        /// </summary>
        /// <param name="inventoryDocumentTypeId">inventoryDocumentTypeId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDocumentTypeDTO</returns>
        public InventoryDocumentTypeDTO GetInventoryDocumentType(int inventoryDocumentTypeId, SqlTransaction  sqlTransaction = null)
        {
            log.LogMethodEntry(inventoryDocumentTypeId, sqlTransaction);
            InventoryDocumentTypeDataHandler inventoryDocumentTypeDataHandler = new InventoryDocumentTypeDataHandler(sqlTransaction);
            InventoryDocumentTypeDTO inventoryDocumentTypeDTO = new InventoryDocumentTypeDTO();
            inventoryDocumentTypeDTO= inventoryDocumentTypeDataHandler.GetInventoryDocumentType(inventoryDocumentTypeId);
            log.LogMethodExit(inventoryDocumentTypeDTO);
            return inventoryDocumentTypeDTO;
        }

        /// <summary>
        /// Returns the GetAllInventoryDocumentTypesByApplicability list
        /// </summary>
        /// <param name="applicability">applicability</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDocumentTypeDTOs</returns>
        public List<InventoryDocumentTypeDTO> GetAllInventoryDocumentTypesByApplicability(string[] applicability,int siteId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(applicability, siteId, sqlTransaction);
            InventoryDocumentTypeDataHandler inventoryDocumentTypeDataHandler = new InventoryDocumentTypeDataHandler(sqlTransaction);
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOs = new List<InventoryDocumentTypeDTO>();
            inventoryDocumentTypeDTOs= inventoryDocumentTypeDataHandler.GetInventoryDocumentTypeList(applicability, siteId);
            log.LogMethodExit(inventoryDocumentTypeDTOs);
            return inventoryDocumentTypeDTOs;
        }

        /// <summary>
        /// Returns the inventoryDocumentType list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDocumentTypeDTOList</returns>
        public List<InventoryDocumentTypeDTO> GetAllInventoryDocumentTypes(List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryDocumentTypeDataHandler inventoryDocumentTypeDataHandler = new InventoryDocumentTypeDataHandler(sqlTransaction);
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = new List<InventoryDocumentTypeDTO>();
            inventoryDocumentTypeDTOList= inventoryDocumentTypeDataHandler.GetInventoryDocumentTypeList(searchParameters);
            log.LogMethodExit(inventoryDocumentTypeDTOList);
            return inventoryDocumentTypeDTOList;
        }

        /// <summary>
        /// Returns the inventoryDocumentType list. Guru
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDocumentTypeDTO</returns>
        public InventoryDocumentTypeDTO GetInventoryDocumentType(string name,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(name, sqlTransaction);
            InventoryDocumentTypeDataHandler inventoryDocumentTypeDataHandler = new InventoryDocumentTypeDataHandler(sqlTransaction);
            InventoryDocumentTypeDTO inventoryDocumentTypeDTO = new InventoryDocumentTypeDTO();
            inventoryDocumentTypeDTO= inventoryDocumentTypeDataHandler.GetInventoryDocumentType(name);
            log.LogMethodExit(inventoryDocumentTypeDTO);
            return inventoryDocumentTypeDTO;
        }

        /// <summary>
        /// Returns the inventoryDocumentType list.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDocumentTypeDTOs</returns>
        public List<InventoryDocumentTypeDTO> GetInventoryDocumentType(string name, int siteId)
        {
            log.LogMethodEntry(name, siteId );
            InventoryDocumentTypeDataHandler inventoryDocumentTypeDataHandler = new InventoryDocumentTypeDataHandler();
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOs = new List<InventoryDocumentTypeDTO>();
            inventoryDocumentTypeDTOs= inventoryDocumentTypeDataHandler.GetInventoryDocumentType(name,siteId);
            log.LogMethodExit(inventoryDocumentTypeDTOs);
            return inventoryDocumentTypeDTOs;
        }

        /// <summary>
        /// Returns the inventoryDocumentType
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDocumentTypeDTOList</returns>
        public InventoryDocumentTypeDTO GetInventoryDocumentType(string code,int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(code, siteId, sqlTransaction);
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList;
            InventoryDocumentTypeDataHandler inventoryDocumentTypeDataHandler = new InventoryDocumentTypeDataHandler(sqlTransaction);
            List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, siteId.ToString()));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE, code));
            inventoryDocumentTypeDTOList = inventoryDocumentTypeDataHandler.GetInventoryDocumentTypeList(inventoryDocumentTypeSearchParams);
            if(inventoryDocumentTypeDTOList!=null && inventoryDocumentTypeDTOList.Count>0)
            {
                log.LogMethodExit(inventoryDocumentTypeDTOList[0]);
                return inventoryDocumentTypeDTOList[0];
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Returns the inventoryDocumentType Last Update DateTime
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns InventoryDocumentTypeModuleLastUpdateTime</returns>
        internal DateTime? GetInventoryDocumentTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            InventoryDocumentTypeDataHandler inventoryDocumentTypeDataHandler = new InventoryDocumentTypeDataHandler();
            DateTime? result = inventoryDocumentTypeDataHandler.GetInventoryDocumentTypeModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        } 
    }
}
