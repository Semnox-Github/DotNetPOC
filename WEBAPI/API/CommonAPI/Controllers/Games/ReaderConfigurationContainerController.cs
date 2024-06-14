﻿/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - ReaderConfigurationContainer controller
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *0.0        02-Sep-2020   Girish Kundar   Created : POSUI redesign using REST API 
 *2.110.0    23-Dec-2020   Prajwal S        Modified as per New standards for Container API.   
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Games
{
    public class ReaderConfigurationContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Route("api/ReaderConfiguration/ReaderConfigurationContainer")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            

            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                //ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ReaderConfigurationContainerDTOCollection readerConfigurationContainerDTOCollection = await
                           Task<ReaderConfigurationContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               return ReaderConfigurationViewContainerList.GetReaderConfigurationContainerDTOCollection(siteId, hash, rebuildCache);
                           });
                log.LogMethodExit(readerConfigurationContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = readerConfigurationContainerDTOCollection });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}
