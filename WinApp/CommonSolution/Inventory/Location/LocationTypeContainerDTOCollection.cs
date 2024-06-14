/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationTypeContainerDTOCollection class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      15-Jan-2021      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationTypeContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LocationTypeContainerDTO> locationTypeContainerDTOList;
        private string hash;

        public LocationTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            locationTypeContainerDTOList = new List<LocationTypeContainerDTO>();
            log.LogMethodExit();
        }

        public LocationTypeContainerDTOCollection(List<LocationTypeContainerDTO> locationTypeContainerDTOList)
        {
            log.LogMethodEntry(locationTypeContainerDTOList);
            this.locationTypeContainerDTOList = locationTypeContainerDTOList;
            if (locationTypeContainerDTOList == null)
            {
                locationTypeContainerDTOList = new List<LocationTypeContainerDTO>();
            }
            hash = new DtoListHash(locationTypeContainerDTOList);
            log.LogMethodExit();
        }

        public List<LocationTypeContainerDTO> LocationTypeContainerDTOList
        {
            get
            {
                return locationTypeContainerDTOList;
            }

            set
            {
                locationTypeContainerDTOList = value;
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
