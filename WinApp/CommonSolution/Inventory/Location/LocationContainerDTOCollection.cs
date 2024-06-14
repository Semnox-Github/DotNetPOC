/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationContainerDTOCollection class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0        09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LocationContainerDTO> locationContainerDTOList;
        private string hash;

        public LocationContainerDTOCollection()
        {
            log.LogMethodEntry();
            locationContainerDTOList = new List<LocationContainerDTO>();
            log.LogMethodExit();
        }

        public LocationContainerDTOCollection(List<LocationContainerDTO> locationContainerDTOList)
        {
            log.LogMethodEntry(locationContainerDTOList);
            this.locationContainerDTOList = locationContainerDTOList;
            if (locationContainerDTOList == null)
            {
                locationContainerDTOList = new List<LocationContainerDTO>();
            }
            hash = new DtoListHash(locationContainerDTOList);
            log.LogMethodExit();
        }

        public List<LocationContainerDTO> LocationContainerDTOList
        {
            get
            {
                return locationContainerDTOList;
            }

            set
            {
                locationContainerDTOList = value;
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
