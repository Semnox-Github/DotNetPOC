/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - RemoteConnectionCheckService used to route the request to get latest ping time of the server connectivity
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Amitha Joy                Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class RemoteConnectionCheckService : RemoteUseCases
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "api/Common/RemoteConnectionCheck";
    

        public async Task<DateTime?> GetServerTime(bool checkDBConnection = false)
        {
            log.LogMethodEntry();
            DateTime? result = null;
            try
            {
                string response = await Get(GET_URL + "?checkDBConnection = " + checkDBConnection);
                JObject jObject = (JObject)JsonConvert.DeserializeObject(response);
                result = jObject["data"].ToObject<DateTime>();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while pinging the server", ex);
                result = null;
            }
            return result;
        }

    }
}
