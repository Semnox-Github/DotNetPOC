/********************************************************************************************
 * Project Name - Printer                                                                        
 * Description  -CashdrawerContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    public class CashdrawerContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CashdrawerContainerDTO> cashdrawerContainerDTOList;
        private string hash;

        public CashdrawerContainerDTOCollection()
        {
            log.LogMethodEntry();
            cashdrawerContainerDTOList = new List<CashdrawerContainerDTO>();
            log.LogMethodExit();
        }

        public CashdrawerContainerDTOCollection(List<CashdrawerContainerDTO> cashdrawerContainerDTOList)
        {
            log.LogMethodEntry(cashdrawerContainerDTOList);
            this.cashdrawerContainerDTOList = cashdrawerContainerDTOList;
            if (cashdrawerContainerDTOList == null)
            {
                cashdrawerContainerDTOList = new List<CashdrawerContainerDTO>();
            }
            hash = new DtoListHash(cashdrawerContainerDTOList);
            log.LogMethodExit();
        }

        public List<CashdrawerContainerDTO> CashdrawerContainerDTOList
        {
            get
            {
                return cashdrawerContainerDTOList;
            }

            set
            {
                cashdrawerContainerDTOList = value;
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
