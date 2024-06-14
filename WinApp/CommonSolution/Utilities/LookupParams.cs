using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    /// <summary>
    ///  This is the LookupParams value data object class. This acts as data holder for the LookupParams 
    /// </summary>   
    public class LookupParams
    {

        int lookupValueId;
        int lookupId;
        String lookupValue;
        String lookupName;
        String description;
        int siteId;

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Default constructor
        /// </summary>
        public LookupParams()
        {
            log.Debug("Starts-LookupParams() default constructor.");
            this.lookupValueId = -1;
            this.lookupId = -1;
            this.lookupValue = "";
            this.lookupName = "";
            this.description = "";
            this.siteId = -1;
            log.Debug("Ends-LookupParams() default constructor.");
        }
      
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LookupParams(int lookupValueId, int lookupId, String lookupValue, String lookupName, String description,int siteId = -1)
        {
            log.Debug("Starts-LookupParams(with all the data fields) Parameterized constructor.");
            this.lookupValueId = lookupValueId;
            this.lookupId = lookupId;
            this.lookupValue = lookupValue;
            this.lookupName = lookupName;
            this.description = description;
            this.siteId = siteId;
            log.Debug("Ends-LookupParams(with all the data fields) Parameterized constructor.");
        }

        /// <summary>
        /// Get/Set method of the LookupValueId field
        /// </summary>
        [DefaultValue(-1)]
        public int LookupValueId { get { return lookupValueId; } set { lookupValueId = value; } }

        /// <summary>
        /// Get/Set method of the LookupId field
        /// </summary>
        [DefaultValue(-1)]
        public int LookupId { get { return lookupId; } set { lookupId = value; } }

        /// <summary>
        /// Get/Set method of the LookupValue field
        /// </summary>
        public String LookupValue { get { return lookupValue; } set { lookupValue = value; } }

        /// <summary>
        /// Get/Set method of the LookupName field
        /// </summary>
        public String LookupName { get { return lookupName; } set { lookupName = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public String Description { get { return description; } set { description = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

    }
}
