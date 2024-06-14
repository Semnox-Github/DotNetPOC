/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the login details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Ashish Bhat             Created : External  REST API.
 ***************************************************************************************************/
using System;

namespace Semnox.Parafait.ThirdParty.External
{
    public class ExternalLogin
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for LoginId
        /// </summary>
        public string LoginId { get; set; }

        /// <summary>
        /// Get/Set for Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Get/Set for NewPassword
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// siteId property
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// MachineName Property
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// MachineName Property
        /// </summary>
        public string LoginToken { get; set; }

        /// <summary>
        /// IPAddress Property
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// TagNumber Property
        /// </summary>
        public string TagNumber { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExternalLogin()
        {
            log.LogMethodEntry();
            LoginId = string.Empty;
            Password = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        public ExternalLogin(String loginId, string password)
        {
            log.LogMethodEntry(loginId, password);
            this.LoginId = loginId;
            this.Password = password;
            log.LogMethodExit();
        }
    }
}



