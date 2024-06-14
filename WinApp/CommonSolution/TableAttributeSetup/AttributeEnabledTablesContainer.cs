/********************************************************************************************
 * Project Name - Device
 * Description  - AttributeEnabledTablessContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     24-Aug-2021   Fiona               Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class AttributeEnabledTablesContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<string, AttributeEnabledTablesDTO> AttributeEnabledTablessDictionary = new Dictionary<string, AttributeEnabledTablesDTO>();
        private readonly List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList;
        private readonly AttributeEnabledTablesContainerDTOCollection attributeEnabledTablesContainerDTOCollection;
        private readonly DateTime? attributeEnabledTablesModuleLastUpdateTime;
        private readonly int siteId;
        private Dictionary<int, AttributeEnabledTablesContainerDTO> attributeEnabledTablesContainerDTODictionary=new Dictionary<int, AttributeEnabledTablesContainerDTO>();

        public AttributeEnabledTablesContainer(int siteId) : this(siteId, GetAttributeEnabledTablesDTOList(siteId), GetAttributeEnabledTablesModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private static DateTime? GetAttributeEnabledTablesModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                AttributeEnabledTablesListBL attributeEnabledTablessList = new AttributeEnabledTablesListBL();
                result = attributeEnabledTablessList.GetAttributeEnabledTablesModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system option max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<AttributeEnabledTablesDTO> GetAttributeEnabledTablesDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList = null;
            try
            {
                AttributeEnabledTablesListBL AttributeEnabledTablessList = new AttributeEnabledTablesListBL();
                List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>(AttributeEnabledTablesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>(AttributeEnabledTablesDTO.SearchByParameters.IS_ACTIVE, "1"));
                attributeEnabledTablesDTOList = AttributeEnabledTablessList.GetAttributeEnabledTables(searchParameters,true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system options.", ex);
            }

            if (attributeEnabledTablesDTOList == null)
            {
                attributeEnabledTablesDTOList = new List<AttributeEnabledTablesDTO>();
            }
            log.LogMethodExit(attributeEnabledTablesDTOList);
            return attributeEnabledTablesDTOList;
        }

        public AttributeEnabledTablesContainer(int siteId, List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList, DateTime? attributeEnabledTablessModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId, attributeEnabledTablesDTOList, attributeEnabledTablessModuleLastUpdateTime);
            this.siteId = siteId;
            this.attributeEnabledTablesDTOList = attributeEnabledTablesDTOList;
            this.attributeEnabledTablesModuleLastUpdateTime = attributeEnabledTablessModuleLastUpdateTime;
            List<AttributeEnabledTablesContainerDTO> attributeEnabledTablesContainerDTOList = new List<AttributeEnabledTablesContainerDTO>();
        
            if (attributeEnabledTablesDTOList!=null && attributeEnabledTablesDTOList.Any())
            {
                foreach(AttributeEnabledTablesDTO attributeEnabledTablesDTO in attributeEnabledTablesDTOList)
                {
                    AttributeEnabledTablesContainerDTO attributeEnabledTablesContainerDTO = new AttributeEnabledTablesContainerDTO(attributeEnabledTablesDTO.AttributeEnabledTableId, attributeEnabledTablesDTO.TableName, attributeEnabledTablesDTO.Description,
                        attributeEnabledTablesDTO.Guid);
                    if(attributeEnabledTablesDTO.TableAttributeSetupDTOList != null && attributeEnabledTablesDTO.TableAttributeSetupDTOList.Any())
                    {
                        foreach(TableAttributeSetupDTO tableAttributeSetupDTO in attributeEnabledTablesDTO.TableAttributeSetupDTOList)
                        {
                            TableAttributeSetupContainerDTO tableAttributeSetupContainerDTO = new TableAttributeSetupContainerDTO(tableAttributeSetupDTO.TableAttributeSetupId, tableAttributeSetupDTO.AttributeEnabledTableId, tableAttributeSetupDTO.ColumnName, tableAttributeSetupDTO.DisplayName,
                                tableAttributeSetupDTO.DataSourceType, tableAttributeSetupDTO.DataType, tableAttributeSetupDTO.LookupId, tableAttributeSetupDTO.SQLSource, tableAttributeSetupDTO.SQLDisplayMember,
                                tableAttributeSetupDTO.SQLValueMember, tableAttributeSetupDTO.Guid);
                            if(tableAttributeSetupDTO.TableAttributeValidationDTOList != null && tableAttributeSetupDTO.TableAttributeValidationDTOList.Any())
                            {
                                foreach(TableAttributeValidationDTO tableAttributeValidationDTO in tableAttributeSetupDTO.TableAttributeValidationDTOList)
                                {
                                    TableAttributeValidationContainerDTO tableAttributeValidationContainerDTO = new TableAttributeValidationContainerDTO(tableAttributeValidationDTO.TableAttributeValidationId, tableAttributeValidationDTO.TableAttributeSetupId, tableAttributeValidationDTO.DataValidationRule, tableAttributeValidationDTO.Guid);
                                    if(tableAttributeSetupContainerDTO.TableAttributeValidationContainerDTOList==null)
                                    {
                                        tableAttributeSetupContainerDTO.TableAttributeValidationContainerDTOList = new List<TableAttributeValidationContainerDTO>();
                                    }
                                    tableAttributeSetupContainerDTO.TableAttributeValidationContainerDTOList.Add(tableAttributeValidationContainerDTO);
                                }
                            }
                            if(attributeEnabledTablesContainerDTO.TableAttributeSetupContainerDTOList==null)
                            {
                                attributeEnabledTablesContainerDTO.TableAttributeSetupContainerDTOList = new List<TableAttributeSetupContainerDTO>();
                            }
                            attributeEnabledTablesContainerDTO.TableAttributeSetupContainerDTOList.Add(tableAttributeSetupContainerDTO);
                        }
                    }
                    attributeEnabledTablesContainerDTOList.Add(attributeEnabledTablesContainerDTO);
                    if(attributeEnabledTablesContainerDTODictionary.ContainsKey(attributeEnabledTablesContainerDTO.AttributeEnabledTableId)==false)
                    {
                        attributeEnabledTablesContainerDTODictionary.Add(attributeEnabledTablesContainerDTO.AttributeEnabledTableId, attributeEnabledTablesContainerDTO);
                    }
                }
            }
            attributeEnabledTablesContainerDTOCollection = new AttributeEnabledTablesContainerDTOCollection(attributeEnabledTablesContainerDTOList);
        }
        public AttributeEnabledTablesContainerDTOCollection GetPaymentModeContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(attributeEnabledTablesContainerDTOCollection);
            return attributeEnabledTablesContainerDTOCollection;
        }
        public AttributeEnabledTablesContainerDTO GetAttributeEnabledTablesContainerDTO(int id)
        {
            log.LogMethodEntry(id);
            if (attributeEnabledTablesContainerDTODictionary.ContainsKey(id) == false)
            {
                string errorMessage = "AttributeEnabledTables with Id : " + id + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = attributeEnabledTablesContainerDTODictionary[id];
            return result;
        }
        public AttributeEnabledTablesContainer Refresh()
        {
            log.LogMethodEntry();
            AttributeEnabledTablesListBL attributeEnabledTablesList = new AttributeEnabledTablesListBL();
            DateTime? updateTime = attributeEnabledTablesList.GetAttributeEnabledTablesModuleLastUpdateTime(siteId);
            if (attributeEnabledTablesModuleLastUpdateTime.HasValue
                && attributeEnabledTablesModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Payment modes since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            AttributeEnabledTablesContainer result = new AttributeEnabledTablesContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
