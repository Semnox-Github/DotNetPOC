using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Authentication
{
    /// <summary>
    /// Creates the system user execution context
    /// </summary>
    public class SystemUserExecutionContextBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private volatile static ExecutionContext executionContext;
        private static object singletonLockingObject = new object();
        private static DateTime? lastRefreshedTime;
        private static readonly Dictionary<string, int> loginIdLifeTimeDictionary = new Dictionary<string, int>{
            {"ParafaitPOS",  480},
            {"GameApp",  480},
            {"WaiverApp",  480},
            {"TabletPOSApp",  60},
            {"MaintApp",  60},
            {"BIZINSIGHTS",  60},
            {"InteralAppsPortal",  480},
            {"WebPartyBooking",  480},
            {"WebFandB",  480},
            {"WebInventory",  480},
            {"SmartFun",  60}
        };
        /// <summary>
        /// returns the system user execution context
        /// </summary>
        public static ExecutionContext GetSystemUserExecutionContext()
        {
            log.LogMethodEntry();
            ExecutionContext result;
            int sysLifeTime = GetSystemUserLifeTime();
            lock (singletonLockingObject)
            {
                if(executionContext == null ||
                   lastRefreshedTime.HasValue == false ||
                   lastRefreshedTime.Value.AddMinutes(sysLifeTime) < DateTime.Now ||
                   string.IsNullOrWhiteSpace(executionContext.WebApiToken))
                {
                    executionContext = CreateSystemUserExecutionContext();
                    lastRefreshedTime = DateTime.Now;
                }
                result = executionContext;
            }

            log.LogMethodExit(result);
            return result;
        }

        private static ExecutionContext CreateSystemUserExecutionContext()
        {
            log.LogMethodEntry();
            ExecutionContext result;
            LoginRequest loginRequest = new LoginRequest();
            string configLoginId = ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"];
            if (string.IsNullOrWhiteSpace(configLoginId) == false)
            {
                loginRequest.LoginId = configLoginId;
            }
            else
            {
                loginRequest.LoginId = "ParafaitPOS";
            }
            string configPassword = ConfigurationManager.AppSettings["SYSTEM_USER_PASSWORD"];
            if (string.IsNullOrWhiteSpace(configPassword) == false)
            {
                SystemUserEncryptedPassword systemUserEncryptedPassword = new SystemUserEncryptedPassword(configPassword);
                loginRequest.Password = systemUserEncryptedPassword.GetPlainTextPassword(Environment.MachineName);
            }
            else
            {
                SystemUserEncryptedPassword semnoxPassword = new SystemUserEncryptedPassword("zKYh1RgsAEsPCIO9p5de9w==");
                loginRequest.Password = semnoxPassword.GetPlainTextPassword("MLR-LT");
            }
            loginRequest.SiteId = ConfigurationManager.AppSettings["SITE_ID"];
            loginRequest.MachineName = Environment.MachineName;
            IAuthenticationUseCases authenticationUseCases = AuthenticationUseCaseFactory.GetAuthenticationUseCases();
            using (NoSynchronizationContextScope.Enter())
            {
                Task<ExecutionContext> systemUserExecutionContextTask = authenticationUseCases.LoginSystemUser(loginRequest);
                systemUserExecutionContextTask.Wait();
                result = systemUserExecutionContextTask.Result;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static int GetSystemUserLifeTime()
        {
            int lifeTime;
            string configLoginId = ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"];
            if (string.IsNullOrWhiteSpace(configLoginId))
            {
                configLoginId = "ParafaitPOS";
            }
            if(loginIdLifeTimeDictionary.ContainsKey(configLoginId))
            {
                lifeTime = loginIdLifeTimeDictionary[configLoginId];
            }
            else
            {
                lifeTime = 60;
            }
            lifeTime = lifeTime - 15;
            return lifeTime;
        }
    }
}
