/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - Data structure of EnabledAttributesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     18-Aug-2021   Fiona               Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.TableAttributeSetup;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class EnabledAttributesContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int enabledAttributesId;
        private string tableName;
        private string recordGuid;
        private string enabledAttributeName;
        private EnabledAttributesDTO.IsMandatoryOrOptional mandatoryOrOptional;
        private string defaultValue;
        private string guid;

        public EnabledAttributesContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public EnabledAttributesContainerDTO(int enabledAttributesId, string tableName, string recordGuid, string enabledAttributeName, EnabledAttributesDTO.IsMandatoryOrOptional mandatoryOrOptional, string guid, string defaultValue)
        {
            log.LogMethodEntry(enabledAttributesId, tableName, recordGuid, enabledAttributeName, mandatoryOrOptional, guid, defaultValue);
            this.enabledAttributesId = enabledAttributesId;
            this.tableName = tableName;
            this.recordGuid = recordGuid;
            this.enabledAttributeName = enabledAttributeName;
            this.mandatoryOrOptional = mandatoryOrOptional;
            this.guid = guid;
            this.defaultValue = defaultValue;
            log.LogMethodExit();
        }

        public int EnabledAttibuteId
        {
            get
            {
                return enabledAttributesId;
            }
            set
            {
                enabledAttributesId = value;
                
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
        public string RecordGuid
        {
            get
            {
                return recordGuid;
            }
            set
            {
                recordGuid = value;
                
            }
        }
        public string EnabledAttributeName
        {
            get
            {
                return enabledAttributeName;
            }
            set
            {
                enabledAttributeName = value;
            }
        }
        public EnabledAttributesDTO.IsMandatoryOrOptional MandatoryOrOptional
        {
            get
            {
                return mandatoryOrOptional;
            }
            set
            {
                mandatoryOrOptional = value;
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

        public string DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;
            }
        }
    }
}
