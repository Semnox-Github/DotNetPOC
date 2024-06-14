/********************************************************************************************
* Project Name - Products
* Description  - FacilityContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.00    16-Aug-2021       Prajwal S                Created
********************************************************************************************/
using System;
using System.Linq;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    public class FacilityContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<FacilityContainerDTO> facilityContainerDTOList;
        private string hash;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FacilityContainerDTOCollection()
        {
            log.LogMethodEntry();
            facilityContainerDTOList = new List<FacilityContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Contructor with facilityContainerDTOList parameter, create hash.
        /// </summary>
        /// <param name="facilityContainerDTOList"></param>
        public FacilityContainerDTOCollection(List<FacilityContainerDTO> facilityContainerDTOList)
        {
            log.LogMethodEntry(facilityContainerDTOList);
            this.facilityContainerDTOList = facilityContainerDTOList;
            if (facilityContainerDTOList == null)
            {
                this.facilityContainerDTOList = new List<FacilityContainerDTO>();
            }
            hash = new DtoListHash(this.facilityContainerDTOList);
            log.LogMethodExit();
        }

        public List<FacilityContainerDTO> FacilitysContainerDTOList
        {
            get
            {
                return facilityContainerDTOList;
            }

            set
            {
                facilityContainerDTOList = value;
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
