/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data structure of SystemOptionViewContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Data structure of SystemOptionViewContainer
    /// </summary>
    public class SystemOptionContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string optionType;
        private string optionName;
        private string optionValue;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SystemOptionContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public SystemOptionContainerDTO(string optionType, string optionName, string optionValue)
            : this()
        {
            log.LogMethodEntry(optionType, optionName, "optionValue");
            this.optionType = optionType;
            this.optionName = optionName;
            this.optionValue = optionValue;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the optionType field
        /// </summary>
        public string OptionType
        {
            get
            {
                return optionType;
            }

            set
            {
                optionType = value;
            }
        }



        /// <summary>
        /// Get/Set method of the optionName field
        /// </summary>
        public string OptionName
        {
            get
            {
                return optionName;
            }

            set
            {
                optionName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the optionName field
        /// </summary>
        public string OptionValue
        {
            get
            {
                return optionValue;
            }

            set
            {
                optionValue = value;
            }
        }

    }
}
