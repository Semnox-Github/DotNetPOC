/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data object of ParafaitDefaults for view 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        3- Jul- 2019  Girish Kundar       Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// This is the ParafaitDefaults data object class. This acts as data holder for the ParafaitDefaults business object
    /// </summary>
    public class ParafaitDefaultContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string defaultValueName;
        private string defaultValue;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ParafaitDefaultContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public ParafaitDefaultContainerDTO(string defaultValueName, string defaultValue)
            : this()
        {
            log.LogMethodEntry(defaultValueName, defaultValue);
            this.defaultValueName = defaultValueName;
            this.defaultValue = defaultValue;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the DefaultValueName field
        /// </summary>
        public string DefaultValueName
        {
            get
            {
                return defaultValueName;
            }

            set
            {
                defaultValueName = value;
            }
        }



        /// <summary>
        /// Get/Set method of the DefaultValue field
        /// </summary>
        [DisplayName("Default Value")]
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
