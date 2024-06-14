/********************************************************************************************
 * Project Name - RequestIdentifierHelper
 * Description  -  RequestIdentifierHelper class business logic
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.120.0      06-Jul-2021  Lakshminarayana  Created
  ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Semnox.CommonAPI.Helpers
{
    public class RequestIdentifierHelper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string GetRequestIdentifier(HttpRequestMessage request)
        {
            log.LogMethodEntry(request);
            var requestIdentifier = string.Empty;
            try
            {
                IEnumerable<string> requestIdentifierHeaders;
                if (request.Headers.TryGetValues("RequestIdentifier", out requestIdentifierHeaders) &&
                    requestIdentifierHeaders.Any())
                {
                    requestIdentifier = requestIdentifierHeaders.First();
                }
            }
            catch (Exception ex)
            {
                log.Error("error occured while retrieving request identifier from the request", ex);
            }
           
            log.LogMethodExit(requestIdentifier);
            return requestIdentifier;
        }
    }
}