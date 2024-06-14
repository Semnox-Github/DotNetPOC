using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class LookupValuesContainerDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int lookupValueId;
        private string lookupValue;
        private string description;
        private string lookupName;


        /// <summary>
        /// Default constructor
        /// </summary>
        public LookupValuesContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all  data fields
        /// </summary>
        public LookupValuesContainerDTO(int lookupValueId, string lookupValue, string description, string lookupName)
            : this()
        {
            log.LogMethodEntry(lookupValueId, lookupValue, description);
            this.lookupValueId = lookupValueId;
            this.lookupValue = lookupValue;
            this.description = description;
            this.lookupName = lookupName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LookupValueId field
        /// </summary>
        public int LookupValueId { get { return lookupValueId; } set { lookupValueId = value; } }
        /// <summary>
        /// Get/Set method of the LookupValue field
        /// </summary>
        public string LookupValue { get { return lookupValue; } set { lookupValue = value; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { description = value;} }
        /// <summary>
        /// Get/Set method of the LookupName field
        /// </summary>
        public string LookupName { get { return lookupName; } set { lookupName = value; } }
    }
}
