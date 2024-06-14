/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch barcode.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0       06-Nov-2020  Mushahid Faizan         Created.
 ***************************************************************************************************/
using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class BarcodeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Generate the Location barcode.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Barcodes")]
        public HttpResponseMessage Get(int barWeight, string textToEncode, int productId = -1,
                                        bool showPrice = false, bool showDescription = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                dynamic content = null;
                Image myimg = null;
                PrinterBL printerBL = new PrinterBL(executionContext);

                if (showPrice || showDescription && productId > -1)
                {
                    content = printerBL.GenerateProductBarcode(barWeight, textToEncode, productId, showPrice, showDescription);
                }
                else
                {
                    myimg = printerBL.MakeBarcodeLibImage(barWeight, 40, BarcodeLib.TYPE.CODE128.ToString(), textToEncode);

                    ImageConverter myimgConverter = new ImageConverter();
                    content = Convert.ToBase64String((byte[])myimgConverter.ConvertTo(myimg, typeof(byte[])));
                }
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
