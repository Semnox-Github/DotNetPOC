/********************************************************************************************
 * Project Name - Device
 * Description  - Data structure of AttributeEnabledTablesContainer
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class AttributeEnabledTablesContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int attributeEnabledTableId;
        private string tableName;
        private string description;
        private string guid;
        private List<TableAttributeSetupContainerDTO> tableAttributeSetupContainerDTOList;

        

        public AttributeEnabledTablesContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Default Contructor
        /// </summary>
        /// <param name="attributeEnabledTableId"></param>
        /// <param name="tableName"></param>
        /// <param name="description"></param>
        /// <param name="guid"></param>
        public AttributeEnabledTablesContainerDTO(int attributeEnabledTableId, string tableName, string description, string guid)
        {
            log.LogMethodEntry(attributeEnabledTableId, tableName, description, guid);
            this.attributeEnabledTableId = attributeEnabledTableId;
            this.tableName = tableName;
            this.description = description;
            this.guid = guid;
            log.LogMethodExit();
        }

        public int AttributeEnabledTableId
        {
            get
            {
                return attributeEnabledTableId;
            }
            set
            {
                attributeEnabledTableId = value;
            }
        }

        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }
        public List<TableAttributeSetupContainerDTO> TableAttributeSetupContainerDTOList
        {
            get
            {
                return tableAttributeSetupContainerDTOList;
            }
            set
            {
                tableAttributeSetupContainerDTOList = value;
            }
        }
    }
}
