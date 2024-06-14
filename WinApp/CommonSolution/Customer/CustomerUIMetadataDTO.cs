using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Summary description for CustomerDetailsStruct
    /// </summary>
    public class CustomerUIMetadataDTO
    {
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
        /// 
        /// </summary>
        public string CustomerFieldValue
        {
            get { return customerDetailsFieldValue; }
            set { customerDetailsFieldValue = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<string> CustomerFieldValues
        {
            get { return customerDetailsFieldValues; }
            set { customerDetailsFieldValues = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerFieldType
        {
            get { return customerFieldType; }
            set { customerFieldType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ValidationType
        {
            get { return validationType; }
            set { validationType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FieldLength
        {
            get { return fieldLength; }
            set { fieldLength = value; }
        }
        /// <summary>
        /// 
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

        public CustomerUIMetadataDTO()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerDetailsFieldName"></param>
        /// <param name="customerDetailsFieldValue"></param>
        public CustomerUIMetadataDTO(string customerDetailsFieldName, string customerDetailsFieldValue)
        {
            this.customerDetailsFieldName = customerDetailsFieldName;
            this.customerDetailsFieldValue = customerDetailsFieldValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customAttributeFlagPassed"></param>
        /// <param name="customerDetailsFieldOrderPassed"></param>
        /// <param name="customerDetailsFieldNamePassed"></param>
        /// <param name="customerDetailsFieldValuePassed"></param>
        /// <param name="validationType"></param>
        /// <param name="fieldLength"></param>
        public CustomerUIMetadataDTO(int customAttributeFlagPassed, int customerDetailsFieldOrderPassed, string customerDetailsFieldNamePassed, string customerDetailsFieldValuePassed, string validationType, string fieldLength)
        {
            customerDetailsFieldOrder = customerDetailsFieldOrderPassed;
            customAttributeFlag = customAttributeFlagPassed;
            customerDetailsFieldName = customerDetailsFieldNamePassed;
            customerDetailsFieldValue = customerDetailsFieldValuePassed;
            customerFieldType = "TEXT";
            this.validationType = validationType;
            this.fieldLength = fieldLength;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldOrder"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValues"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldValidation"></param>
        /// <param name="fieldLength"></param>
        /// <param name="customAttributeFlag"></param>
        /// <param name="customAttributeId"></param>
        public CustomerUIMetadataDTO(int fieldOrder, string fieldName, List<string> fieldValues, string fieldType, String fieldValidation, string fieldLength, int customAttributeFlag, int customAttributeId)
        {
            this.customerDetailsFieldOrder = fieldOrder;
            this.customerDetailsFieldName = fieldName;
            this.customerDetailsFieldValue = "";
            this.customerDetailsFieldValues = fieldValues;
            this.customerFieldType = fieldType;
            this.validationType = fieldValidation;
            this.fieldLength = fieldLength;
            this.customAttributeFlag = customAttributeFlag;
            this.customAttributeId = customAttributeId;
        }

        /// <summary>
        /// 
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
        public CustomerUIMetadataDTO(int fieldOrder, string fieldName, List<string> fieldValues, string fieldType, String fieldValidation, string fieldLength, int customAttributeFlag, int customAttributeId, CustomAttributesDTO customAttributesDTO, string entityFieldCaption)
        {
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customAttributeFlagPassed"></param>
        /// <param name="customerDetailsFieldOrderPassed"></param>
        /// <param name="customerDetailsFieldNamePassed"></param>
        /// <param name="customerDetailsFieldValuePassed"></param>
        /// <param name="customerDetailsFieldTypePassed"></param>
        /// <param name="validationType"></param>
        /// <param name="fieldLength"></param>
        public CustomerUIMetadataDTO(int customAttributeFlagPassed, int customerDetailsFieldOrderPassed, string customerDetailsFieldNamePassed, string customerDetailsFieldValuePassed, string customerDetailsFieldTypePassed, string validationType, string fieldLength)
        {
            customerDetailsFieldOrder = customerDetailsFieldOrderPassed;
            customAttributeFlag = customAttributeFlagPassed;
            customerDetailsFieldName = customerDetailsFieldNamePassed;
            customerDetailsFieldValue = customerDetailsFieldValuePassed;
            customerFieldType = customerDetailsFieldTypePassed;
            this.validationType = validationType;
            this.fieldLength = fieldLength;
        }
    }
}