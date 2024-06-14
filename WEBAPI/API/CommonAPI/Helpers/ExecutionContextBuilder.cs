using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Helpers
{
    public class ExecutionContextBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ExecutionContext GetExecutionContext(HttpRequestMessage request)
        {
            log.LogMethodEntry();
            DateTime logTime = DateTime.UtcNow;

            string webApiToken;
            if (TryRetrieveToken(request, out webApiToken) == false)
            {
                string errorMessage = "Unable to find token in the request";
                log.LogMethodExit(null, "Throwing Exception -" + errorMessage);
                throw new UnauthorizedException(errorMessage);
            }

            log.Debug("TryRetrieveToken " + (DateTime.UtcNow - logTime).TotalMilliseconds);

            var identity = (ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal;
            string loginId = identity.FindFirst(ClaimTypes.Name).Value;
            bool isCorporate = SiteViewContainerList.IsCorporate();
            log.Debug("SiteViewContainerList " + (DateTime.UtcNow - logTime).TotalMilliseconds);
            int siteId = isCorporate ? Convert.ToInt32(identity.FindFirst(ClaimTypes.Sid).Value) : -1;
            int sitePKId = SiteViewContainerList.GetCurrentSiteContainerDTO(siteId).SiteId;
            log.Debug("SiteViewContainerList " + (DateTime.UtcNow - logTime).TotalMilliseconds);
            string machineidString = identity.FindFirst(ClaimTypes.System).Value;
            int machineId = string.IsNullOrWhiteSpace(machineidString) ? -1 : Convert.ToInt32(machineidString);
            int userPKId = UserViewContainerList.GetUserContainerDTO(siteId, loginId).UserId;
            log.Debug("UserViewContainerList " + (DateTime.UtcNow - logTime).TotalMilliseconds);
            string languageIdString = identity.FindFirst(ClaimTypes.Locality).Value;
            int languageId;
            if (int.TryParse(languageIdString, out languageId) == false)
            {
                languageId = -1;
            }
            string posMachineGuid = string.Empty;
            string posMachineName = string.Empty;
            if (machineId > -1)
            {
                try
                {
                    POSMachineContainerDTO posMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(siteId, machineId);
                    posMachineGuid = posMachineContainerDTO.Guid;
                    posMachineName = posMachineContainerDTO.POSName;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while retrieving pos machine container DTO", ex);
                }
            }
            log.Debug("GetPOSMachineContainerDTO " + siteId + ":" + machineId + ":" + (DateTime.UtcNow - logTime).TotalMilliseconds);

            string languageCode = "en-US";
            if (languageId > -1)
            {
                try
                {
                    languageCode = LanguageViewContainerList.GetLanguageContainerDTO(siteId, languageId).LanguageCode;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while retrieving language container DTO", ex);
                }
            }

            log.Debug("GetLanguageContainerDTO " + siteId + ":" + languageId + ":" + (DateTime.UtcNow - logTime).TotalMilliseconds);

            ExecutionContext result = new ExecutionContext(loginId, siteId, sitePKId, machineId, userPKId, isCorporate, languageId, webApiToken, posMachineGuid, posMachineName, languageCode);
            log.LogMethodExit(result);
            return result;
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }
    }
}