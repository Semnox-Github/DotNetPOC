/********************************************************************************************
 * Project Name - Customer Controller
 * Description  - Controller to get or save customer image
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.120.2     17-feb-2022      Nitin Pai      Initial Version
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class CustomerImage
    {
        public String ProfileImageBase64 = "";
    }
    public class CustomerImageController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>   
        /// Get the Customer JSON by Customer Id.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/Image")]
        public async Task<HttpResponseMessage> Get(int customerId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(customerId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                string customerProfileImage = "";
                CustomerBL customerBL = new CustomerBL(executionContext, customerId);
                customerProfileImage = await Task<String>.Factory.StartNew(() =>
                                            {
                                                return customerBL.GetCustomerImageBase64();
                                            });

                CustomerImage customerImage = new CustomerImage();
                customerImage.ProfileImageBase64 = customerProfileImage;
                log.LogMethodExit(customerProfileImage);

                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerImage });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Customer Collections.
        /// </summary>
        /// <param name="cutomerDTO"></param>
        [HttpPost]
        [Route("api/Customer/Image")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] CustomerImage profileImage, [FromUri] int customerId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(profileImage, customerId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (profileImage == null || String.IsNullOrEmpty(profileImage.ProfileImageBase64))
                {
                    List<ValidationError> validationErrors = new List<ValidationError>();
                    validationErrors.Add(new ValidationError("ProfileImage", "ProfileImageBase63", "Image not found"));
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = validationErrors, token = securityTokenDTO.Token });
                }

                string photoFile = Guid.NewGuid().ToString();
                photoFile += ".jpg"; //added file extension
                Byte[] imageByteArray = Convert.FromBase64String(profileImage.ProfileImageBase64);

                CustomerBL customerBL = new CustomerBL(executionContext, customerId, true);
                ProfileBL profileBL = new ProfileBL(executionContext, customerBL.CustomerDTO.ProfileId);
                await System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        profileBL.SaveProfilePhoto(photoFile, imageByteArray);
                        profileBL.Save();
                    });

                return Request.CreateResponse(HttpStatusCode.OK, new { data = photoFile });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}