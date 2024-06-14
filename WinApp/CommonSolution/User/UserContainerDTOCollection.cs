/********************************************************************************************
 * Project Name - User 
 * Description  - Holds a list of UserContainerDTO and hash value of the list
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

namespace Semnox.Parafait.User
{
    public class UserContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<UserContainerDTO> userContainerDTOList;
        private string hash;

        public UserContainerDTOCollection()
        {
            log.LogMethodEntry();
            userContainerDTOList = new List<UserContainerDTO>();
            log.LogMethodExit();
        }

        public UserContainerDTOCollection(List<UserContainerDTO> userContainerDTOList)
        {
            log.LogMethodEntry(userContainerDTOList);
            this.userContainerDTOList = userContainerDTOList;
            if (this.userContainerDTOList == null)
            {
                this.userContainerDTOList = new List<UserContainerDTO>();
            }
            hash = new DtoListHash(userContainerDTOList);
            log.LogMethodExit();
        }

        public List<UserContainerDTO> UserContainerDTOList
        {
            get
            {
                return userContainerDTOList;
            }

            set
            {
                userContainerDTOList = value;
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
