/********************************************************************************************
 * Project Name - Utilities
 * Description  - Object hold the execution context data
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        11-Nov-2019   Lakshminarayana         Modified for virtual store enhancement to hold web token.
 ********************************************************************************************/

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Entity holds the current execution context identifiers like user, machine, site and userPimaryKey
    /// </summary>
    public class ExecutionContext
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string userId;
        private int siteId;
        private int sitePKId;
        private int machineId;
        private int userPKId;
        private bool isCorporate;
        private int languageId;
        private volatile static ExecutionContext executionContext;
        private static object singletonLockingObject = new object();
        private readonly object locker = new object();
        private string token;
        private string webApiToken;
        private string posMachineGuid;
        private string posMachineName;
        private string languageCode;

        private ExecutionContext()
        {
            log.LogMethodEntry();
            userId = "";
            siteId = -1;
            sitePKId = -1;
            machineId = -1;
            userPKId = -1;
            isCorporate = false;
            languageId = -9;
            webApiToken = string.Empty;
            posMachineGuid = string.Empty;
            posMachineName = string.Empty;
            languageCode = string.Empty;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Returns the default execution context.
        /// </summary>
        /// <returns></returns>
        public static ExecutionContext GetExecutionContext()
        {
            log.LogMethodEntry();
            if (executionContext == null)
            {
                lock (singletonLockingObject)
                {
                    if (executionContext == null)
                    {
                        executionContext = new ExecutionContext();
                    }
                }
            }
            log.LogMethodExit(executionContext);
            return executionContext;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="userId">application user identifier</param>
        /// <param name="siteId">site identifier</param>
        /// <param name="machineId">application machine identifier</param>
        /// <param name="userPKId">application user primary key identifier</param>
        /// <param name="isCorporate">Whether the current application execution is in a HQ site</param>
        /// <param name="languageId">language identifier</param>
        public ExecutionContext(string userId, int siteId, int machineId, int userPKId, bool isCorporate, int languageId)
        {
            log.LogMethodEntry(userId, siteId, machineId, userPKId, isCorporate, languageId);
            this.userId = userId;
            this.userPKId = userPKId;
            this.siteId = siteId;
            this.machineId = machineId;
            this.isCorporate = isCorporate;
            this.languageId = languageId;
            webApiToken = string.Empty;
            posMachineGuid = string.Empty;
            log.LogMethodExit();
        }

        
        public ExecutionContext(string userId, int siteId, int sitePKId, int machineId, int userPKId, bool isCorporate, int languageId, string webApiToken, string posMachineGuid, string posMachineName, string languageCode)
        {
            log.LogMethodEntry(userId, siteId, machineId, userPKId, isCorporate, languageId, webApiToken, posMachineGuid, posMachineName, languageCode);
            this.userId = userId;
            this.userPKId = userPKId;
            this.siteId = siteId;
            this.sitePKId = sitePKId;
            this.machineId = machineId;
            this.isCorporate = isCorporate;
            this.languageId = languageId;
            this.webApiToken = webApiToken;
            this.posMachineGuid = posMachineGuid;
            this.posMachineName = posMachineName;
            this.languageCode = languageCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// sets the user identifier of the execution context
        /// </summary>
        /// <param name="userId">application user identifier</param>

        public void SetUserId(string userId)
        {
            log.LogMethodEntry(userId);
            lock (locker)
            {
                this.userId = userId;
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// returns user identifier of the execution context
        /// </summary>
        /// <returns></returns>
        public string GetUserId()
        {
            lock (locker)
            {
                return userId;
            }
        }

        /// <summary>
        /// sets the site identifier of the execution context
        /// </summary>
        /// <param name="siteId">site identifier</param>
        public void SetSiteId(int siteId)
        {
            log.LogMethodEntry(siteId);
            lock (locker)
            {
                this.siteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// returns site identifier of the execution context
        /// </summary>
        /// <returns></returns>
        public int GetSiteId()
        {
            lock (locker)
            {
                return siteId;
            }
        }

        /// <summary>
        /// sets the primary key site identifier of the execution context
        /// </summary>
        /// <param name="sitePKId">site identifier</param>
        public void SetSitePKId(int sitePKId)
        {
            log.LogMethodEntry(sitePKId);
            lock (locker)
            {
                this.sitePKId = sitePKId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// returns primary key site identifier of the execution context
        /// </summary>
        /// <returns></returns>
        public int GetSitePKId()
        {
            lock (locker)
            {
                return sitePKId;
            }
        }

        /// <summary>
        /// sets the machine identifier of the execution context
        /// </summary>
        /// <param name="machineId">machine identifier</param>
        public void SetMachineId(int machineId)
        {
            log.LogMethodEntry(machineId);
            lock (locker)
            {
                this.machineId = machineId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// returns machine identifier of the execution context
        /// </summary>
        /// <returns></returns>
        public int GetMachineId()
        {
            lock (locker)
            {
                return machineId;
            }
        }

        /// <summary>
        /// sets the application user primary key identifier of the execution context
        /// </summary>
        /// <param name="userPKId">application user primary key identifier</param>
        public void SetUserPKId(int userPKId)
        {
            log.LogMethodEntry(userPKId);
            lock (locker)
            {
                this.userPKId = userPKId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// returns application user primary key identifier of the execution context
        /// </summary>
        /// <returns>user primary key</returns>
        public int GetUserPKId()
        {
            lock (locker)
            {
                return userPKId;
            }
        }

        /// <summary>
        /// sets the whether application execution is in a HQ Site
        /// </summary>
        /// <param name="isCorporate">application execution is in a HQ Site</param>
        public void SetIsCorporate(bool isCorporate)
        {
            log.LogMethodEntry(isCorporate);
            lock (locker)
            {
                this.isCorporate = isCorporate;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// returns application is executing in a HQ Site
        /// </summary>
        /// <returns>user primary key</returns>
        public bool GetIsCorporate()
        {
            lock (locker)
            {
                return isCorporate;
            }
        }

        /// <summary>
        /// sets current language for the execution context
        /// </summary>
        /// <param name="languageId">language identifier</param>
        public void SetLanguageId(int languageId)
        {
            log.LogMethodEntry(languageId);
            lock (locker)
            {
                this.languageId = languageId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// returns language identifier for the current execution context
        /// </summary>
        /// <returns>user primary key</returns>
        public int GetLanguageId()
        {
            lock (locker)
            {
                return languageId;
            }
        }


		/// <summary>
		/// sets the Token for execution context
		/// </summary>
		/// <param name="token">Token Guid</param>
		public void SetToken(string token)
		{
            log.LogMethodEntry(token);
            lock (locker)
			{
				this.token = token;
            }
            log.LogMethodExit();
        }

		/// <summary>
		/// returns the Token for the current execution context
		/// </summary>
		/// <returns>user primary key</returns>
		public string GetToken()
		{
            lock (locker)
            {
                return token;
            }
		}

        /// <summary>
        /// sets the Web Api Token for execution context
        /// </summary>
        /// <param name="value">Token Guid</param>
        public void SetWebApiToken(string value)
        {
            log.LogMethodEntry(value);
            lock (locker)
            {
                webApiToken = value;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// returns the Web Api Token for the current execution context
        /// </summary>
        /// <returns>user primary key</returns>
        public string GetWebApiToken()
        {
            lock(locker)
            {
                return webApiToken;
            }
        }

        /// <summary>
        /// returns the POS machine guid for the current execution context
        /// </summary>
        /// <returns>user primary key</returns>
        public string GetPosMachineGuid()
        {
            lock(locker)
            {
                return posMachineGuid;
            }
        }

        /// <summary>
        /// sets the POS machine guid for execution context
        /// </summary>
        /// <param name="value">Token Guid</param>
        public void SetPosMachineGuid(string value)
        {
            log.LogMethodEntry(value);
            lock (locker)
            {
                posMachineGuid = value;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the posMachineGuid field
        /// </summary>
        public string PosMachineGuid
        {
            get
            {
                lock(locker)
                {
                    return posMachineGuid;
                }
            }

            set
            {
                lock(locker)
                {
                    posMachineGuid = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the webApiToken field
        /// </summary>
        public string WebApiToken
        {
            get
            {
                lock(locker)
                {
                    return webApiToken;
                }
            }

            set
            {
                lock(locker)
                {
                    webApiToken = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the token field
        /// </summary>
        public string Token
        {
            get
            {
                lock(locker)
                {
                    return token;
                }
            }

            set
            {
                lock(locker)
                {
                    token = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the languageId field
        /// </summary>
        public int LanguageId
        {
            get
            {
                lock(locker)
                {
                    return languageId;
                }
            }

            set
            {
                lock(locker)
                {
                    languageId = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the isCorporate field
        /// </summary>
        public bool IsCorporate
        {
            get
            {
                lock(locker)
                {
                    return isCorporate;
                }
            }

            set
            {
                lock(locker)
                {
                    isCorporate = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the userPKId field
        /// </summary>
        public int UserPKId
        {
            get
            {
                lock(locker)
                {
                    return userPKId;
                }
            }

            set
            {
                lock(locker)
                {
                    userPKId = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the machineId field
        /// </summary>
        public int MachineId
        {
            get
            {
                lock(locker)
                {
                    return machineId;
                }
            }

            set
            {
                lock(locker)
                {
                    machineId = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                lock(locker)
                {
                    return siteId;
                }
            }

            set
            {
                lock(locker)
                {
                    log.Debug("setting SiteId: " + value);
                    siteId = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the sitePKId field
        /// </summary>
        public int SitePKId
        {
            get
            {
                lock(locker)
                {
                    return sitePKId;
                }
            }

            set
            {
                lock(locker)
                {
                    sitePKId = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the userId field
        /// </summary>
        public string UserId
        {
            get
            {
                lock(locker)
                {
                    return userId;
                }
            }

            set
            {
                lock(locker)
                {
                    userId = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the POSMachineName field
        /// </summary>
        public string POSMachineName
        {
            get
            {
                lock(locker)
                {
                    return posMachineName;
                }
            }

            set
            {
                lock(locker)
                {
                    posMachineName = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the AddressType field
        /// </summary>
        public string LanguageCode
        {
            get
            {
                lock(locker)
                {
                    return languageCode;
                }
                
            }

            set
            {
                lock(locker)
                {
                    languageCode = value;
                }
            }
        }
    }
}
