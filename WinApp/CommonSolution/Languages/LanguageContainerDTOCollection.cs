/********************************************************************************************
 * Project Name - Utilities 
 * Description  - Data object of LanguageContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Languages
{
    public class LanguageContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LanguageContainerDTO> languageContainerDTOList;
        private string hash;

        public LanguageContainerDTOCollection()
        {
            log.LogMethodEntry();
            languageContainerDTOList = new List<LanguageContainerDTO>();
            log.LogMethodExit();
        }

        public LanguageContainerDTOCollection(List<LanguageContainerDTO> languageContainerDTOList)
        {
            log.LogMethodEntry(languageContainerDTOList);
            this.languageContainerDTOList = languageContainerDTOList;
            if (this.languageContainerDTOList == null)
            {
                this.languageContainerDTOList = new List<LanguageContainerDTO>();
            }
            hash = new DtoListHash(languageContainerDTOList);
            log.LogMethodExit();
        }

        public List<LanguageContainerDTO> LanguageContainerDTOList
        {
            get
            {
                return languageContainerDTOList;
            }

            set
            {
                languageContainerDTOList = value;
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
