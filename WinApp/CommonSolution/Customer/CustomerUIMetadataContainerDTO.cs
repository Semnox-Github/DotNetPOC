/********************************************************************************************
* Project Name - Customer
* Description  - CustomerUIMetadataContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    09-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
2.140.00    14-Sep-2021       Prajwal S              Modified : Added Customer Field Value field.
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public class CustomerUIMetadataContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerDetailsFieldOrder;
        private string customerDetailsFieldName;
        private string entityFieldCaption;
        private string entityFieldName;
        private string customerDetailsFieldValue;
        private List<string> customerFieldValues;
        private int customAttributeFlag;
        private int customAttributeId;
        private string customerFieldType;
        private string validationType;
        private string fieldLength;
        private CustomAttributesContainerDTO customAttributesContainerDTO;
        private string displayFormat;
        public CustomerUIMetadataContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        ///
        public CustomerUIMetadataContainerDTO(int customerDetailsFieldOrderPassed, string customerDetailsFieldNamePassed, string entityFieldCaptionPassed, string entityFieldNamePassed,
            string customerDetailsFieldValuePassed, int customAttributeFlagPassed, int customAttributeIdPassed, string customerFieldTypePassed, string validationTypePassed, string fieldLengthPassed,
            string displayFormatPassed, List<string> customerFieldValues) : this()
        {
            log.LogMethodEntry(customerDetailsFieldOrderPassed,customerDetailsFieldNamePassed,entityFieldCaptionPassed,entityFieldNamePassed,
            customerDetailsFieldValuePassed,customAttributeFlagPassed,customAttributeIdPassed,customerFieldTypePassed,validationTypePassed,fieldLengthPassed,
            displayFormatPassed);
            this.customerDetailsFieldOrder = customerDetailsFieldOrderPassed;
            this.customerDetailsFieldName = customerDetailsFieldNamePassed;
            this.entityFieldCaption = entityFieldCaptionPassed;
            this.entityFieldName = entityFieldNamePassed;
            this.customerDetailsFieldValue = customerDetailsFieldValuePassed;
            this.customAttributeFlag = customAttributeFlagPassed;
            this.customAttributeId = customAttributeIdPassed;
            this.customerFieldType = customerFieldTypePassed;
            this.validationType = validationTypePassed;
            this.fieldLength = fieldLengthPassed;
            this.displayFormat = displayFormatPassed;
            this.customerFieldValues = customerFieldValues;
            log.LogMethodExit();
        }

        /// <summary>
        /// CustomerFieldOrder
        /// </summary>
        public int CustomerFieldOrder
        {
            get { return customerDetailsFieldOrder; }
            set { customerDetailsFieldOrder = value; }
        }
        /// <summary>
        /// CustomAttributeFlag
        /// </summary>
        public int CustomAttributeFlag
        {
            get { return customAttributeFlag; }
            set { customAttributeFlag = value; }
        }
        /// <summary>
        /// CustomAttributeId
        /// </summary>
        public int CustomAttributeId
        {
            get { return customAttributeId; }
            set { customAttributeId = value; }
        }
        /// <summary>
        /// CustomerFieldName
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
        /// CustomerFieldValues
        /// </summary>
        public List<string> CustomerFieldValues
        {
            get { return customerFieldValues; }
            set { customerFieldValues = value; }
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
        /// DisplayFormat
        /// </summary>
        public string DisplayFormat
        {
            get { return displayFormat; }
            set { displayFormat = value; }
        }
        /// <summary>
        /// CustomAttributesDTO
        /// </summary>
        public CustomAttributesContainerDTO CustomAttributesContainerDTO
        {
            get { return customAttributesContainerDTO; }
            set { customAttributesContainerDTO = value; }
        }
    }
}
