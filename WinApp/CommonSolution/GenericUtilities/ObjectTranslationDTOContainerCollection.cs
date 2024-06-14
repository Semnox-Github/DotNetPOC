/********************************************************************************************
* Project Name - GenericUtilities
* Description  - ObjectTranslationsContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.00    04-Aug-2021       Prajwal S                Created
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class ObjectTranslationContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ObjectTranslationContainerDTO> objectTranslationsContainerDTOList;
        private string hash;

        public ObjectTranslationContainerDTOCollection()
        {
            log.LogMethodEntry();
            objectTranslationsContainerDTOList = new List<ObjectTranslationContainerDTO>();
            log.LogMethodExit();
        }
        public ObjectTranslationContainerDTOCollection(List<ObjectTranslationContainerDTO> objectTranslationsContainerDTOList)
        {
            log.LogMethodEntry(objectTranslationsContainerDTOList);
            this.objectTranslationsContainerDTOList = objectTranslationsContainerDTOList;
            if (objectTranslationsContainerDTOList == null)
            {
                this.objectTranslationsContainerDTOList = new List<ObjectTranslationContainerDTO>();
            }
            hash = new DtoListHash(this.objectTranslationsContainerDTOList);
            log.LogMethodExit();
        }

        public List<ObjectTranslationContainerDTO> ObjectTranslationsContainerDTOList
        {
            get
            {
                return objectTranslationsContainerDTOList;
            }

            set
            {
                objectTranslationsContainerDTOList = value;
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
