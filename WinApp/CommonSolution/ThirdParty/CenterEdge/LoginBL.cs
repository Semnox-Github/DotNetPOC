/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - LoginBL class - This is business layer class for login
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      24-Oct-2020      Girish Kundar             Created : CenterEdge  REST API
 2.140.5      06-Jul-2023      Abhishek                  Modified : Removal of UTC conversion of timestamp 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    /// This class is used to handle the login process for the Center edge 
    /// </summary>
    public class LoginBL
    {
        private LoginDTO loginDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LoginBL(LoginDTO loginDTO)
        {
            log.LogMethodEntry(loginDTO);
            this.loginDTO = loginDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method gets the User information on successfull login
        /// </summary>
        /// <returns></returns>
        public Security.User Login()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            Security security = new Security(utilities);
            Security.User user = null;
            string password = string.Empty;   //  "ceSemParafaiT!";

            Users userBL = new Users(utilities.ExecutionContext, loginDTO.username);
            if (String.IsNullOrEmpty(userBL.UserDTO.LoginId))
            {
                log.Debug("user not exists");
                throw new Security.SecurityException(loginDTO.username, Security.SecurityException.ExInvalidLogin);
            }
            try
            {
                LookupsList lookupsList = new LookupsList(utilities.ExecutionContext);
                List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, userBL.UserDTO.SiteId.ToString()));
                searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, "CENTEREDGE_CONFIGURATIONS"));
                List<LookupsDTO> lookups = lookupsList.GetAllLookups(searchParameters, true);
                log.Debug("lookups Count : " + lookups.Count);
                if (lookups != null && lookups.Any())
                {
                    if (lookups[0].LookupValuesDTOList != null && lookups[0].LookupValuesDTOList.Any())
                    {
                        LookupValuesDTO lookupValuesDTO = lookups[0].LookupValuesDTOList.Where(x => x.LookupValue == "CENTEREDGE_PASSWORD").FirstOrDefault();
                        if (lookupValuesDTO != null)
                        {
                            password = Encryption.Decrypt(lookupValuesDTO.Description);
                        }
                        else
                        {
                            log.Debug("look up values are not created : CENTEREDGE_PASSWORD,CENTEREDGE_USERNAME");
                            throw new Security.SecurityException(loginDTO.username, Security.SecurityException.ExInvalidLogin);
                        }
                    }
                }

                string requestedTimeStamp = loginDTO.requestTimestamp;
                string input = loginDTO.username + password + requestedTimeStamp;
                log.Debug("input :" + input);

                SHA1 sha1 = SHA1.Create();
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                string userCredentialHash = Convert.ToBase64String(hash);
                log.Debug("userCredentialHash : " + userCredentialHash);

                StringComparer comparer = StringComparer.InvariantCulture;
                if (comparer.Compare(userCredentialHash, loginDTO.passwordHash) != 0)
                {
                    log.Debug("Compare(userCredentialHash, loginDTO.passwordHash) falied");
                    throw new Security.SecurityException(loginDTO.username, Security.SecurityException.ExInvalidLogin);
                }
                user = security.Login(loginDTO.username, password);
            }
            catch (Security.SecurityException se)
            {
                log.Error(se.Message);
                throw new Exception(se.Message);
            }
            log.LogMethodExit(user);
            return user;
        }
    }
}
