using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class LookupsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int lookupId;
        private string lookupName;
        private string isProtected;
        private List<LookupValuesContainerDTO> lookupValuesContainerDTOList;


        /// <summary>
        /// Default constructor
        /// </summary>
        public LookupsContainerDTO()
        {
            log.LogMethodEntry();
            lookupValuesContainerDTOList = new List<LookupValuesContainerDTO>();
            log.LogMethodExit();
        }

        public LookupsContainerDTO(int lookupId, string lookupName, string isProtected)
            : this()
        {
            log.LogMethodEntry(lookupId, lookupName, isProtected);
            this.lookupId = lookupId;
            this.lookupName = lookupName;
            this.isProtected = isProtected;
            log.LogMethodExit();

        }

        /// <summary>
        /// Get/Set method of the LookupId field
        /// </summary>
        public int LookupId { get { return lookupId; } set { lookupId = value; } }
        /// <summary>
        /// Get/Set method of the LookupName field
        /// </summary>
        public string LookupName { get { return lookupName; } set { lookupName = value; } }
        /// <summary>
        /// Get/Set method of the Protected field
        /// </summary>
        public string IsProtected { get { return isProtected; } set { isProtected = value; } }

        /// <summary>
        /// Get/Set methods for LookupValuesDTOList 
        /// </summary>
        public List<LookupValuesContainerDTO> LookupValuesContainerDTOList
        {
            get
            {
                return lookupValuesContainerDTOList;
            }

            set
            {
                lookupValuesContainerDTOList = value;
            }
        }
    }
}
