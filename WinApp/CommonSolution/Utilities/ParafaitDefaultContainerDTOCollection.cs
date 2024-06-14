/********************************************************************************************
 * Project Name - Utilities 
 * Description  - Data object of ParafaitDefaultContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2020   Girish Kundar          Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
namespace Semnox.Core.Utilities
{
    public class ParafaitDefaultContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ParafaitDefaultContainerDTO> parafaitDefaultContainerDTOList;
        private string hash;

        public ParafaitDefaultContainerDTOCollection()
        {
            log.LogMethodEntry();
            parafaitDefaultContainerDTOList = new List<ParafaitDefaultContainerDTO>();
            log.LogMethodExit();
        }

        public ParafaitDefaultContainerDTOCollection(List<ParafaitDefaultContainerDTO> parafaitDefaultContainerDTOList)
        {
            log.LogMethodEntry(parafaitDefaultContainerDTOList);
            this.parafaitDefaultContainerDTOList = parafaitDefaultContainerDTOList;
            if (this.parafaitDefaultContainerDTOList == null)
            {
                this.parafaitDefaultContainerDTOList = new List<ParafaitDefaultContainerDTO>();
            }
            hash = new DtoListHash(parafaitDefaultContainerDTOList);
            log.LogMethodExit();
        }
        
        public List<ParafaitDefaultContainerDTO> ParafaitDefaultContainerDTOList
        {
            get
            {
                return parafaitDefaultContainerDTOList;
            }

            set
            {
                parafaitDefaultContainerDTOList = value;
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
