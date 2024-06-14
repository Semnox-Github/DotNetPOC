/********************************************************************************************
 * Project Name - Device
 * Description  - Data structure of TableAttributeSetupContainer
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
    public class TableAttributeSetupContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int tableAttributeSetupId;
        private int attributeEnabledTableId;
        private string columnName;
        private string displayName;
        private TableAttributeSetupDTO.DataSourceTypeEnum dataSourceType;
        private TableAttributeSetupDTO.DataTypeEnum dataType;
        private int lookupId;
        private string sQLSource;
        private string sQLDisplayMember;
        private string sQLValueMember;
        private string guid;
        private List<TableAttributeValidationContainerDTO> tableAttributeValidationContainerDTOList;

        public TableAttributeSetupContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public TableAttributeSetupContainerDTO(int tableAttributeSetupId, int attributeEnabledTableId, string columnName, string displayName, 
                         TableAttributeSetupDTO.DataSourceTypeEnum dataSourceType, TableAttributeSetupDTO.DataTypeEnum dataType, int lookupId, string sQLSource, 
                         string sQLDisplayMember, string sQLValueMember, string guid)
        {
            log.LogMethodEntry(tableAttributeSetupId, attributeEnabledTableId, columnName, displayName, dataSourceType, dataType, lookupId, sQLSource, sQLDisplayMember, sQLValueMember, guid);
            this.tableAttributeSetupId = tableAttributeSetupId;
            this.attributeEnabledTableId = attributeEnabledTableId;
            this.columnName = columnName;
            this.displayName = displayName;
            this.dataSourceType = dataSourceType;
            this.dataType = dataType;
            this.lookupId = lookupId;
            this.sQLSource = sQLSource;
            this.sQLDisplayMember = sQLDisplayMember;
            this.sQLValueMember = sQLValueMember;
            this.guid = guid;
            log.LogMethodExit();
        }

        public int TableAttributeSetupId
        {
            get
            {
                return tableAttributeSetupId;
            }
            set
            {
                tableAttributeSetupId = value;
            }
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
        public string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                columnName = value;
            }
        }
        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                displayName = value;
            }
        }
        public TableAttributeSetupDTO.DataSourceTypeEnum DataSourceType
        {
            get
            {
                return dataSourceType;
            }
            set
            {
                dataSourceType = value;
            }
        }
        public TableAttributeSetupDTO.DataTypeEnum DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
            }
        }
        public int LookupId
        {
            get
            {
                return lookupId;
            }
            set
            {
                lookupId = value;
            }
        }
        public string SQLSource
        {
            get
            {
                return sQLSource;
            }
            set
            {
                sQLSource = value;
            }
        }
        public string SQLDisplayMember
        {
            get
            {
                return sQLDisplayMember;
            }
            set
            {
                sQLDisplayMember = value;
            }
        }
        public string SQLValueMember
        {
            get
            {
                return sQLValueMember;
            }
            set
            {
                sQLValueMember = value;
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
        public List<TableAttributeValidationContainerDTO> TableAttributeValidationContainerDTOList
        {
            get
            {
                return tableAttributeValidationContainerDTOList;
            }
            set
            {
                tableAttributeValidationContainerDTOList = value;
            }
        }
    }
}
