/********************************************************************************************
* Project Name - Products
* Description  - ModifierSetContainerDTOCollection Class.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.140.00   14-Sep-2021        Prajwal S          Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ModifierSetContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ModifierSetContainerDTO> modifierSetContainerDTOList;
        private string hash;

        public ModifierSetContainerDTOCollection()
        {
            log.LogMethodEntry();
            modifierSetContainerDTOList = new List<ModifierSetContainerDTO>();
            log.LogMethodExit();
        }

        public ModifierSetContainerDTOCollection(List<ModifierSetContainerDTO> modifierSetContainerDTOList)
        {
            log.LogMethodEntry(ModifierSetContainerDTOList);
            this.modifierSetContainerDTOList = modifierSetContainerDTOList;
            if (modifierSetContainerDTOList == null)
            {
                modifierSetContainerDTOList = new List<ModifierSetContainerDTO>();
            }
            hash = new DtoListHash(modifierSetContainerDTOList);
            log.LogMethodExit();
        }

        public List<ModifierSetContainerDTO> ModifierSetContainerDTOList
        {
            get
            {
                return modifierSetContainerDTOList;
            }

            set
            {
                modifierSetContainerDTOList = value;
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




