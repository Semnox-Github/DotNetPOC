/********************************************************************************************
 * Project Name - CustomAttributeValueListContainer DTO
 * Description  - Data object of CustomAttributeValueListContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.120.0    12-07-2021   Prajwal          Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the CustomAttributeValueList data object class. This acts as data holder for the CustomAttributeValueList business object
    /// </summary>
    public class CustomAttributeValueListContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int valueId;
        private string value;
        private int customAttributeId;
        private string isDefault;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomAttributeValueListContainerDTO()
        {
            log.LogMethodEntry();
            valueId = -1;
            customAttributeId = -1;
            isDefault = "N";
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomAttributeValueListContainerDTO(int valueId, string value, int customAttributeId, string isDefault)
            : this()
        {
            log.LogMethodEntry(valueId, value, customAttributeId, isDefault);
            this.valueId = valueId;
            this.value = value;
            this.customAttributeId = customAttributeId;
            this.isDefault = isDefault;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ValueId field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int ValueId
        {
            get
            {
                return valueId;
            }

            set
            {
              
                valueId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Value field
        /// </summary>
        [DisplayName("Value")]
        public string Value
        {
            get
            {
                return value;
            }

            set
            {
              
                this.value = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomAttributeId field
        /// </summary>
        [DisplayName("Custom Attribute Id")]
        public int CustomAttributeId
        {
            get
            {
                return customAttributeId;
            }

            set
            {
              
                customAttributeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsDefault field
        /// </summary>
        [DisplayName("IsDefault")]
        public string IsDefault
        {
            get
            {
                return isDefault;
            }

            set
            {
              
                isDefault = value;
            }
        }

    }
}
