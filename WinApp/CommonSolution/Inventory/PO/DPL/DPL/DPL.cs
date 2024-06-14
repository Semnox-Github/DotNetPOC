/********************************************************************************************
 * Project Name - DPL
 * Description  - DPL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.120.4     12-Nov-2021    Deeksha          Modified to accept execution context instead of utilities
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class DPL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int userId =-1;
        string userName;
        string userLoginId;

        public int UserId
        {
            get { return userId; }
        }

        public string UserName
        {
            get { return userName; }
        }

        public string UserLoginId
        {
            get { return userLoginId; }
        }
        public DPL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            UsersList usersListBL = new UsersList(executionContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters;
            searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, "Semnox"));
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<UsersDTO> usersListDTO = usersListBL.GetAllUsers(searchParameters);
            if (usersListDTO != null)
            {
                userId = usersListDTO[0].UserId;
                userName = usersListDTO[0].UserName;
                userLoginId = usersListDTO[0].LoginId;
            }
            log.LogMethodExit();
        } 
    }
}
