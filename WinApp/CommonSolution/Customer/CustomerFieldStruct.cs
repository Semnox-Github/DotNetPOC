/********************************************************************************************
 * Project Name - Customer
 * Description  - Class for  of CustomerFieldStruct      s
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Summary description for CustomerDetailsStruct
    /// </summary>
    public class CustomerFieldStruct
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerDetailsFieldOrder;
        private string customerDetailsFieldName;
        private string entityFieldCaption;
        private string entityFieldName;
        private string customerDetailsFieldValue;
        private List<string> customerDetailsFieldValues;
        private int customAttributeFlag;
        private int customAttributeId;
        private string customerFieldType;
        private string validationType;
        private string fieldLength;
        private CustomAttributesDTO customAttributesDTO;
        private string displayFormat;

        /// <summary>
        /// 
        /// </summary>
        public int CustomerFieldOrder
        {
            get { return customerDetailsFieldOrder; }
            set { customerDetailsFieldOrder = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CustomAttributeFlag
        {
            get { return customAttributeFlag; }
            set { customAttributeFlag = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CustomAttributeId
        {
            get { return customAttributeId; }
            set { customAttributeId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerFieldName
        {
            get { return customerDetailsFieldName; }
            set { customerDetailsFieldName = value; }
        }

        /// <summary>
        /// EntityFieldCaption
        /// </summary>
        public string EntityFieldCaption
        {
            get { return entityFieldCaption; }
            set { entityFieldCaption = value; }
        }

        /// <summary>
        /// EntityFieldName
        /// </summary>
        public string EntityFieldName
        {
            get { return entityFieldName; }
            set { entityFieldName = value; }
        }
        /// <summary>
        /// CustomerFieldValue
        /// </summary>
        public string CustomerFieldValue
        {
            get { return customerDetailsFieldValue; }
            set { customerDetailsFieldValue = value; }
        }
        /// <summary>
        /// CustomerFieldValues
        /// </summary>
        public List<string> CustomerFieldValues
        {
            get { return customerDetailsFieldValues; }
            set { customerDetailsFieldValues = value; }
        }
        /// <summary>
        /// CustomerFieldType
        /// </summary>
        public string CustomerFieldType
        {
            get { return customerFieldType; }
            set { customerFieldType = value; }
        }
        /// <summary>
        /// ValidationType
        /// </summary>
        public string ValidationType
        {
            get { return validationType; }
            set { validationType = value; }
        }
        /// <summary>
        /// FieldLength
        /// </summary>
        public string FieldLength
        {
            get { return fieldLength; }
            set { fieldLength = value; }
        }
        /// <summary>
        /// CustomAttributesDTO
        /// </summary>
        public CustomAttributesDTO CustomAttributesDTO
        {
            get { return customAttributesDTO; }
            set { customAttributesDTO = value; }
        }

        /// <summary>
        /// DisplayFormat
        /// </summary>
        public string DisplayFormat
        {
            get { return displayFormat; }
            set { displayFormat = value; }
        }

        public CustomerFieldStruct()
        {
            log.LogMethodEntry("Default Constructor- Empty");
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor for CustomerFieldStruct
        /// </summary>
        /// <param name="customerDetailsFieldName"></param>
        /// <param name="customerDetailsFieldValue"></param>
        public CustomerFieldStruct(string customerDetailsFieldName, string customerDetailsFieldValue)
        {
            log.LogMethodEntry(customerDetailsFieldName , customerDetailsFieldValue);
            this.customerDetailsFieldName = customerDetailsFieldName;
            this.customerDetailsFieldValue = customerDetailsFieldValue;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor for CustomerFieldStruct
        /// </summary>
        /// <param name="customAttributeFlagPassed"></param>
        /// <param name="customerDetailsFieldOrderPassed"></param>
        /// <param name="customerDetailsFieldNamePassed"></param>
        /// <param name="customerDetailsFieldValuePassed"></param>
        /// <param name="validationType"></param>
        /// <param name="fieldLength"></param>
        public CustomerFieldStruct(int customAttributeFlagPassed, int customerDetailsFieldOrderPassed, string customerDetailsFieldNamePassed, string customerDetailsFieldValuePassed, string validationType, string fieldLength)
        {
            log.LogMethodEntry(customAttributeFlagPassed, customerDetailsFieldOrderPassed, customerDetailsFieldNamePassed, customerDetailsFieldValuePassed, validationType, fieldLength);
            customerDetailsFieldOrder = customerDetailsFieldOrderPassed;
            customAttributeFlag = customAttributeFlagPassed;
            customerDetailsFieldName = customerDetailsFieldNamePassed;
            customerDetailsFieldValue = customerDetailsFieldValuePassed;
            customerFieldType = "TEXT";
            this.validationType = validationType;
            this.fieldLength = fieldLength;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor for CustomerFieldStruct
        /// </summary>
        /// <param name="fieldOrder"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValues"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldValidation"></param>
        /// <param name="fieldLength"></param>
        /// <param name="customAttributeFlag"></param>
        /// <param name="customAttributeId"></param>
        public CustomerFieldStruct(int fieldOrder, string fieldName, List<string> fieldValues, string fieldType, String fieldValidation, string fieldLength, int customAttributeFlag, int customAttributeId)
        {
            log.LogMethodEntry(fieldOrder, fieldName, fieldValues, fieldType, fieldValidation, fieldLength, customAttributeFlag, customAttributeId);
            this.customerDetailsFieldOrder = fieldOrder;
            this.customerDetailsFieldName = fieldName;
            this.customerDetailsFieldValue = "";
            this.customerDetailsFieldValues = fieldValues;
            this.customerFieldType = fieldType;
            this.validationType = fieldValidation;
            this.fieldLength = fieldLength;
            this.customAttributeFlag = customAttributeFlag;
            this.customAttributeId = customAttributeId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor for CustomerFieldStruct
        /// </summary>
        /// <param name="fieldOrder"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValues"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldValidation"></param>
        /// <param name="fieldLength"></param>
        /// <param name="customAttributeFlag"></param>
        /// <param name="customAttributeId"></param>
        /// <param name="customAttributesDTO"></param>
        public CustomerFieldStruct(int fieldOrder, string fieldName, List<string> fieldValues, string fieldType, String fieldValidation, string fieldLength, int customAttributeFlag, int customAttributeId, CustomAttributesDTO customAttributesDTO, string entityFieldCaption)
        {
            log.LogMethodEntry(fieldOrder, fieldName, fieldValues, fieldType, fieldValidation, fieldLength, customAttributeFlag, customAttributeId, customAttributesDTO, entityFieldCaption);
            this.customerDetailsFieldOrder = fieldOrder;
            this.customerDetailsFieldName = fieldName;
            this.customerDetailsFieldValue = "";
            this.customerDetailsFieldValues = fieldValues;
            this.customerFieldType = fieldType;
            this.validationType = fieldValidation;
            this.fieldLength = fieldLength;
            this.customAttributeFlag = customAttributeFlag;
            this.customAttributeId = customAttributeId;
            this.customAttributesDTO = customAttributesDTO;
			this.EntityFieldCaption = entityFieldCaption;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor for CustomerFieldStruct
        /// </summary>
        /// <param name="customAttributeFlagPassed"></param>
        /// <param name="customerDetailsFieldOrderPassed"></param>
        /// <param name="customerDetailsFieldNamePassed"></param>
        /// <param name="customerDetailsFieldValuePassed"></param>
        /// <param name="customerDetailsFieldTypePassed"></param>
        /// <param name="validationType"></param>
        /// <param name="fieldLength"></param>
        public CustomerFieldStruct(int customAttributeFlagPassed, int customerDetailsFieldOrderPassed, string customerDetailsFieldNamePassed, string customerDetailsFieldValuePassed, string customerDetailsFieldTypePassed, string validationType, string fieldLength)
        {
            log.LogMethodEntry(customAttributeFlagPassed, customerDetailsFieldOrderPassed, customerDetailsFieldNamePassed, customerDetailsFieldValuePassed, customerDetailsFieldTypePassed, validationType, fieldLength);
            customerDetailsFieldOrder = customerDetailsFieldOrderPassed;
            customAttributeFlag = customAttributeFlagPassed;
            customerDetailsFieldName = customerDetailsFieldNamePassed;
            customerDetailsFieldValue = customerDetailsFieldValuePassed;
            customerFieldType = customerDetailsFieldTypePassed;
            this.validationType = validationType;
            this.fieldLength = fieldLength;
            log.LogMethodExit();
        }
    }
}