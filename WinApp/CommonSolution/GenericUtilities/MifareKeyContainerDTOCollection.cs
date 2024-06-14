/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - view container Collection class hold a list of MifareKeyContainerDTOList and hash
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class MifareKeyContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MifareKeyContainerDTO> mifareKeyContainerDTOList;
        private string hash;

        public MifareKeyContainerDTOCollection()
        {
            log.LogMethodEntry();
            mifareKeyContainerDTOList = new List<MifareKeyContainerDTO>();
            log.LogMethodExit();
        }

        public MifareKeyContainerDTOCollection(List<MifareKeyContainerDTO> mifareKeyContainerDTOList)
        {
            log.LogMethodEntry(mifareKeyContainerDTOList);
            this.mifareKeyContainerDTOList = mifareKeyContainerDTOList;
            if (this.mifareKeyContainerDTOList == null)
            {
                this.mifareKeyContainerDTOList = new List<MifareKeyContainerDTO>();
            }
            hash = new DtoListHash(this.mifareKeyContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method for the mifareKeyContainerDTOList field
        /// </summary>
        public List<MifareKeyContainerDTO> MifareKeyContainerDTOList
        {
            get
            {
                return mifareKeyContainerDTOList;
            }

            set
            {
                mifareKeyContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method for the hash field
        /// </summary>
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
