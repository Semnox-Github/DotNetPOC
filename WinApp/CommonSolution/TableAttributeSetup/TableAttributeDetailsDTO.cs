/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - TableAttributeDetailsDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0     09-Sep-2021   Guru S A       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.TableAttributeSetup
{
    /// <summary>
    /// TableAttributeDetailsDTO
    /// </summary>
    public class TableAttributeDetailsDTO
    {
        //private bool notifyingObjectIsChanged;
        //private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private int attributeEnabledTableId;
        private string attributeEnabledtableName;
        //private int enabledAttributeId;
        private string enabledtableName;
        private string enabledRecordGuid;
        private string enabledAttributeName;
        private string attributeDisplayName;
        private EnabledAttributesDTO.IsMandatoryOrOptional mandatoryOrOptional;
        //private string dataSourceType;
        private TableAttributeSetupDTO.DataTypeEnum dataType;
        private int lookupId;
        private string sqlSource;
        private string sqlDisplayMember;
        private string sqlValueMember;
        private string attributeValue;
        private List<string> dataValidationRuleList;
        private string defaultAttributeValue;
        /// <summary>
        /// TableAttributeDetailsDTO
        /// </summary>
        public TableAttributeDetailsDTO()
        {
            log.LogMethodEntry();
            this.lookupId = -1;
            this.dataValidationRuleList = new List<string>();
            log.LogMethodExit();
        }
        /// <summary>
        /// TableAttributeDetailsDTO with parameter
        /// </summary>
        public TableAttributeDetailsDTO(string attributeEnabledtableName, string enabledtableName, string enabledRecordGuid, string enabledAttributeName,
            string attributeDisplayName, EnabledAttributesDTO.IsMandatoryOrOptional mandatoryOrOptional, TableAttributeSetupDTO.DataTypeEnum dataType, int lookupId, 
            string sqlSource, string sqlDisplayMember, string sqlValueMember, List<string> dataValidationRuleList, string attributeValue, string defaultAttributeValue) : this()
        {
            log.LogMethodEntry(attributeEnabledtableName, enabledtableName, enabledRecordGuid, enabledAttributeName, attributeDisplayName, mandatoryOrOptional, dataType,
                lookupId, sqlSource, sqlDisplayMember, sqlValueMember, dataValidationRuleList, attributeValue, defaultAttributeValue);
            this.attributeEnabledtableName = attributeEnabledtableName;
            this.enabledtableName = enabledtableName;
            this.enabledRecordGuid = enabledRecordGuid;
            this.enabledAttributeName = enabledAttributeName;
            this.attributeDisplayName = attributeDisplayName;
            this.mandatoryOrOptional = mandatoryOrOptional;
            this.dataType = dataType;
            this.lookupId = lookupId;
            this.sqlSource = sqlSource;
            this.sqlDisplayMember = sqlDisplayMember;
            this.sqlValueMember = sqlValueMember;
            this.attributeValue = attributeValue;
            this.dataValidationRuleList = dataValidationRuleList;
            this.defaultAttributeValue = defaultAttributeValue;
            log.LogMethodExit();
        }
        /// <summary>
        /// TableAttributeDetailsDTO -Copy constructor
        /// </summary>
        public TableAttributeDetailsDTO(TableAttributeDetailsDTO tableAttributeDetailsDTO) : this(tableAttributeDetailsDTO.attributeEnabledtableName,
            tableAttributeDetailsDTO.enabledtableName, tableAttributeDetailsDTO.enabledRecordGuid, tableAttributeDetailsDTO.enabledAttributeName,
            tableAttributeDetailsDTO.attributeDisplayName, tableAttributeDetailsDTO.mandatoryOrOptional, tableAttributeDetailsDTO.dataType, 
            tableAttributeDetailsDTO.lookupId, tableAttributeDetailsDTO.sqlSource, tableAttributeDetailsDTO.sqlDisplayMember, tableAttributeDetailsDTO.sqlValueMember,
            tableAttributeDetailsDTO.dataValidationRuleList, tableAttributeDetailsDTO.attributeValue, tableAttributeDetailsDTO.defaultAttributeValue)
        {
            log.LogMethodEntry(tableAttributeDetailsDTO);
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set for attributeEnabledtableName
        /// </summary>
        public string AttributeEnabledtableName
        {
            get { return attributeEnabledtableName; }
            set { attributeEnabledtableName = value; }
        }
        /// <summary>
        /// Get/Set for enabledtableName
        /// </summary>
        public string EnabledtableName
        {
            get { return enabledtableName; }
            set { enabledtableName = value; }
        }
        /// <summary>
        /// Get/Set for enabledRecordGuid
        /// </summary>
        public string EnabledRecordGuid
        {
            get { return enabledRecordGuid; }
            set { enabledRecordGuid = value; }
        }
        /// <summary>
        /// Get/Set for enabledAttributeName
        /// </summary>
        public string EnabledAttributeName
        {
            get { return enabledAttributeName; }
            set { enabledAttributeName = value; }
        }
        /// <summary>
        /// Get/Set for attributeDisplayName
        /// </summary>
        public string AttributeDisplayName
        {
            get { return attributeDisplayName; }
            set { attributeDisplayName = value; }
        }
        /// <summary>
        /// Get/Set for mandatoryOrOptional
        /// </summary>
        public EnabledAttributesDTO.IsMandatoryOrOptional MandatoryOrOptional
        {
            get { return mandatoryOrOptional; }
            set { mandatoryOrOptional = value; }
        }
        /// <summary>
        /// Get/Set for dataType
        /// </summary>
        public TableAttributeSetupDTO.DataTypeEnum DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }
        /// <summary>
        /// Get/Set for lookupId
        /// </summary>
        public int LookupId
        {
            get { return lookupId; }
            set { lookupId = value; }
        }
        /// <summary>
        /// Get/Set for sqlSource
        /// </summary>
        public string SqlSource
        {
            get { return sqlSource; }
            set { sqlSource = value; }
        }
        /// <summary>
        /// Get/Set for sqlDisplayMember
        /// </summary>
        public string SqlDisplayMember
        {
            get { return sqlDisplayMember; }
            set { sqlDisplayMember = value; }
        }
        /// <summary>
        /// Get/Set for sqlValueMember
        /// </summary>
        public string SqlValueMember
        {
            get { return sqlValueMember; }
            set { sqlValueMember = value; }
        }
        /// <summary>
        /// Get/Set for attributeValue
        /// </summary>
        public string AttributeValue
        {
            get { return attributeValue; }
            set { attributeValue = value; }
        }
        /// <summary>
        /// Get/Set for dataValidationRuleList
        /// </summary>
        public List<string> DataValidationRuleList
        {
            get { return dataValidationRuleList; }
            set { dataValidationRuleList = value; }
        }
        /// <summary>
        /// Get/Set for defaultAttributeValue
        /// </summary>
        public string DefaultAttributeValue
        {
            get { return defaultAttributeValue; }
            set { defaultAttributeValue = value; }
        }
    }
}
