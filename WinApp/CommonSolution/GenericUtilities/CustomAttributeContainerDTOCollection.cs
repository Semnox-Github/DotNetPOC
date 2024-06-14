/********************************************************************************************
* Project Name - GenericUtilities
* Description  - CustomAttributeContainerDTOCollection
* 
**************
**Version Log
**************
*Version      Date            Modified By         Remarks          
*********************************************************************************************
*2.130.0     28-Jul-2020      Girish Kundar              Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    public  class CustomAttributeContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CustomAttributesContainerDTO> customAttributesContainerDTOList;
        private string hash;

        public CustomAttributeContainerDTOCollection()
        {
            log.LogMethodEntry();
            customAttributesContainerDTOList = new List<CustomAttributesContainerDTO>();
            log.LogMethodExit();
        }

        public CustomAttributeContainerDTOCollection(List<CustomAttributesContainerDTO> customAttributesContainerDTOList)
        {
            log.LogMethodEntry(customAttributesContainerDTOList);
            this.customAttributesContainerDTOList = customAttributesContainerDTOList;
            if (customAttributesContainerDTOList == null)
            {
                customAttributesContainerDTOList = new List<CustomAttributesContainerDTO>();
            }
            hash = new DtoListHash(customAttributesContainerDTOList);
            log.LogMethodExit();
        }

        public List<CustomAttributesContainerDTO> CustomAttributesContainerDTOList
        {
            get
            {
                return customAttributesContainerDTOList;
            }

            set
            {
                customAttributesContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}
