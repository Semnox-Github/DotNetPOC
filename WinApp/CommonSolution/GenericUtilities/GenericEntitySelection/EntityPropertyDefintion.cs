/********************************************************************************************
 * Project Name - GenericUtilities                                                                          
 * Description  - EntityPropertyDefinition
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By          Remarks          
 ********************************************************************************************* 
 *2.70.2        13-Aug-2019    Deeksha              Added logger methods.
 ********************************************************************************************/
using System;
using System.Windows.Forms;
namespace Semnox.Core.GenericUtilities
{
    public class EntityPropertyDefintion
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string propertyName;
        private readonly string displayName;
        private readonly DataGridViewCellStyle dataGridViewCellStyle;
        private readonly bool isFlag = false;
        private readonly bool isFilter = false;
        public EntityPropertyDefintion(string propertyName, 
                                       string displayName = null,
                                       bool searchable = false,
                                       DataGridViewCellStyle dataGridViewCellStyle = null,
                                       bool isFlag = false)
        {
            log.LogMethodEntry(propertyName, displayName, searchable, dataGridViewCellStyle, isFlag);
            this.propertyName = propertyName;
            if(string.IsNullOrWhiteSpace(propertyName))
            {
                throw new Exception("propertyName should not be null");
            }
            this.displayName = displayName;
            if(string.IsNullOrWhiteSpace(propertyName))
            {
                this.displayName = propertyName;
            }
            this.isFilter = searchable;
            this.dataGridViewCellStyle = dataGridViewCellStyle;
            this.isFlag = isFlag;
            log.LogMethodExit();
        }

        public EntityPropertyDefintion(string displayName)
        {
            log.LogMethodEntry(displayName);
            propertyName = "";
            this.displayName = displayName;
            log.LogMethodExit();
        }

        public string PropertyName { get { return propertyName; } }
        public string DisplayName { get { return displayName; } }
        public DataGridViewCellStyle DataGridViewCellStyle { get { return dataGridViewCellStyle; } }
        public bool IsFlag { get { return isFlag; } }
        public bool IsFilter { get { return isFilter; } }
    }
}
