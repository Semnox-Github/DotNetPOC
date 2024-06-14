/********************************************************************************************
* Project Name - Loyalty
* Description  - Loyalty Engine Host
* 
**************
**Version Log
**************
*Version     Date              Modified By        Remarks          
*********************************************************************************************
*2.70.2        09-Sep-2019    Jinto Thomas        Added logger for methods
*********************************************************************************************/
//using Semnox.Core.HR.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Loyalty
{
    public class LoyaltyEngineHost
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        int userId = -1;
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
        public LoyaltyEngineHost(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            this.utilities = utilities;
            UsersList usersListBL = new UsersList(utilities.ExecutionContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters;
            searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, "Semnox"));
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
