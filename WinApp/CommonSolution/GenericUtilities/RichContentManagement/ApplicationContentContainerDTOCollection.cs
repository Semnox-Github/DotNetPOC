/********************************************************************************************
* Project Name - ApplicationContent
* Description  - ApplicationContentContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    public class ApplicationContentContainerDTOCollection
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ApplicationContentContainerDTO> applicationContentContainerDTOList;
        private string hash;

        public ApplicationContentContainerDTOCollection()
        {
            log.LogMethodEntry();
            applicationContentContainerDTOList = new List<ApplicationContentContainerDTO>();
            log.LogMethodExit();
        }
        public ApplicationContentContainerDTOCollection(List<ApplicationContentContainerDTO> applicationContentContainerDTOList)
        {
            log.LogMethodEntry(applicationContentContainerDTOList);
            this.applicationContentContainerDTOList = applicationContentContainerDTOList;
            if (applicationContentContainerDTOList == null)
            {
                applicationContentContainerDTOList = new List<ApplicationContentContainerDTO>();
            }
            hash = new DtoListHash(applicationContentContainerDTOList);
            log.LogMethodExit();
        }

        public List<ApplicationContentContainerDTO> ApplicationContentContainerDTOList
        {
            get
            {
                return applicationContentContainerDTOList;
            }

            set
            {
                applicationContentContainerDTOList = value;
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
