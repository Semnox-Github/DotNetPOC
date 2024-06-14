/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API to get Scan Identifier of Account.
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.100        25-Nov-2020    Nitin Pai     Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GenCode128;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using QRCoder;

namespace Semnox.CommonAPI.Controllers.Customer.Accounts
{
    public class ScanIdentifierController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        /// <summary>
        /// Get the JSON Object Cards Collections.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/Account/Accounts/{accountId}/ScanIdentifier")]
        public async Task<HttpResponseMessage> Get(int accountId = -1, int siteId = -1, AccountQRType accountQRType = AccountQRType.TRANSACTION, string scanIdentifierType = "QR", string outputType = "STRING")
        {
            ExecutionContext executionContext = null;
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(accountId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();

                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                AccountBL accountBL = new AccountBL(executionContext, accountId);
                if(accountBL.AccountDTO == null || accountBL.AccountDTO.AccountId == -1)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Account Not Found" });
                }
                String qrCode = "";
                if (scanIdentifierType.Equals("QR"))
                {
                    qrCode = accountBL.GetAccountQRCode(siteId, accountQRType);
                    if(outputType.Equals("IMAGE"))
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCode, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCodeImage = new QRCode(qrCodeData);
                        if (qrCodeImage != null)
                        {
                            int pixelPerModule = 3;
                            Image image = qrCodeImage.GetGraphic(pixelPerModule);
                            if (image != null)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                    qrCode = System.Convert.ToBase64String(stream.ToArray());
                                }
                            }
                        }
                    }
                }
                else if (scanIdentifierType.Equals("BARCODE"))
                {
                    qrCode = accountBL.GetAccountQRCode(siteId, accountQRType);
                    if (outputType.Equals("IMAGE"))
                    {
                        Image image = Code128Rendering.MakeBarcodeImage(accountBL.AccountDTO.TagNumber, 1, true);
                        if (image != null)
                        {
                            using (var stream = new MemoryStream())
                            {
                                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                qrCode = System.Convert.ToBase64String(stream.ToArray());
                            }
                        }
                    }
                }
                log.LogMethodExit(qrCode);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = qrCode });
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
