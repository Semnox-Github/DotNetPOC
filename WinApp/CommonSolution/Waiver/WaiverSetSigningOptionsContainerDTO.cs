/********************************************************************************************
* Project Name - Waiver
* Description  - WaiverSetSigningOptionsContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/

using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Waiver
{
    public class WaiverSetSigningOptionsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int lookupValueId;
        private string optionName;
        private string optionDescription;

        public WaiverSetSigningOptionsContainerDTO()
        {
            log.LogMethodEntry();
            this.lookupValueId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with required data fields.
        /// </summary>
        public WaiverSetSigningOptionsContainerDTO(int lookupValueId, string optionName, string optionDescription)
            : this()
        {
            log.LogMethodEntry(optionDescription, optionName, lookupValueId);
            this.optionDescription = optionDescription;
            this.optionName = optionName;
            this.lookupValueId = lookupValueId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LookupValueId field
        /// </summary>
        [DisplayName("LookupValueId")]
        [DefaultValue(-1)]
        public int LookupValueId { get { return lookupValueId; } set { lookupValueId = value;  } }

        /// <summary>
        /// Get/Set method of the OptionName field
        /// </summary>
        [DisplayName("OptionName")]
        [DefaultValue("")]
        public string OptionName { get { return optionName; } set { optionName = value; } }

        /// <summary>
        /// Get/Set method of the optionDescription field
        /// </summary>
        [DisplayName("optionDescription")]
        [DefaultValue("")]
        public string OptionDescription { get { return optionDescription; } set { optionDescription = value; } }
    }
}
