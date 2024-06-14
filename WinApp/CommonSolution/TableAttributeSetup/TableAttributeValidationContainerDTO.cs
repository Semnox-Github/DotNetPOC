/********************************************************************************************
 * Project Name - Device
 * Description  - Data structure of TableAttributeValidationContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     24-Aug-2021   Fiona               Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class TableAttributeValidationContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int tableAttributeValidationId;
        private int tableAttributeSetupId;
        private string dataValidationRule;
        private string guid;
        public TableAttributeValidationContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public TableAttributeValidationContainerDTO(int tableAttributeValidationId, int tableAttributeSetupId, string dataValidationRule, string guid)
        {
            log.LogMethodEntry(tableAttributeValidationId, tableAttributeSetupId, dataValidationRule, guid);
            this.tableAttributeValidationId = tableAttributeValidationId;
            this.tableAttributeSetupId = tableAttributeSetupId;
            this.dataValidationRule = dataValidationRule;
            this.guid = guid;
            log.LogMethodExit();
        }

        public int TableAttributeValidationId
        {
            get
            {
                return tableAttributeValidationId;
            }
            set
            {
                tableAttributeValidationId = value;
            }
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
        public string DataValidationRule
        {
            get
            {
                return dataValidationRule;
            }
            set
            {
                dataValidationRule = value;
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
    }
}
