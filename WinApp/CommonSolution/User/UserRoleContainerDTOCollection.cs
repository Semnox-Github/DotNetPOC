/********************************************************************************************
 * Project Name - User 
 * Description  - Holds a list of UserRoleContainerDTO and hash value of the list
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
    public class UserRoleContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<UserRoleContainerDTO> userRoleContainerDTOList;
        private string hash;

        public UserRoleContainerDTOCollection()
        {
            log.LogMethodEntry();
            userRoleContainerDTOList = new List<UserRoleContainerDTO>();
            log.LogMethodExit();
        }

        public UserRoleContainerDTOCollection(List<UserRoleContainerDTO> userRoleContainerDTOList)
        {
            log.LogMethodEntry(userRoleContainerDTOList);
            this.userRoleContainerDTOList = userRoleContainerDTOList;
            if (this.userRoleContainerDTOList == null)
            {
                this.userRoleContainerDTOList = new List<UserRoleContainerDTO>();
            }
            hash = new DtoListHash(userRoleContainerDTOList);
            log.LogMethodExit();
        }

        public List<UserRoleContainerDTO> UserRoleContainerDTOList
        {
            get
            {
                return userRoleContainerDTOList;
            }

            set
            {
                userRoleContainerDTOList = value;
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
