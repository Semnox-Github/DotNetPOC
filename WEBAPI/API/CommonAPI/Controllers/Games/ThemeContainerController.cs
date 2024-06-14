/**************************************************************************************************
 * Project Name - Games 
 * Description  - ThemeContainerController for ReaderTheme
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.80        13-Oct-2020       Girish Kundar             Created: POS UI redesign
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Games
{
    public class ThemeContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object ThemeDTO
        /// </summary>        
        /// <returns>HttpResponseMessage</returns> 
        /// <param name="moduleName">moduleName</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="isActive">activeRecordsOnly</param>
        [HttpGet]
        [Route("api/Game/ThemeContainer")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                ThemeContainerDTOCollection ThemeContainerDTOCollection = await
                           Task<ThemeContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               return ThemeContainerDTOCollection = ThemeViewContainerList.GetThemeContainerDTOCollection(siteId, hash, rebuildCache); //chnage later
                           });
                log.LogMethodExit(ThemeContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = ThemeContainerDTOCollection });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
    }
