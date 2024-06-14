/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - RemoteConnectionCheckController used to check the server connectivity before the remote call
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Amitha Joy                Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Games.Controllers.Common
{
    public class RemoteConnectionCheckController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        [HttpGet]
        [Route("api/Common/RemoteConnectionCheck")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get(bool checkDBConnection = false)
        {
            try
            {
                log.LogMethodEntry();
                DateTime? result = null;
                if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
                {
                    RemoteConnectionCheckService remoteConnectionCheckService = new RemoteConnectionCheckService();
                    result = await remoteConnectionCheckService.GetServerTime(checkDBConnection);
                }
                else
                {
                    if(checkDBConnection)
                    {
                        LocalRemoteConnectionCheckDataService localRemoteConnectionCheckDataService = new LocalRemoteConnectionCheckDataService();
                        DateTime? currentDBDate = localRemoteConnectionCheckDataService.Get();
                        if (currentDBDate.HasValue == false || currentDBDate.Value == DateTime.MinValue)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = "Unable to connect to Database."});
                        }
                    }
                    result = ServerDateTime.Now;
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }

    }
}