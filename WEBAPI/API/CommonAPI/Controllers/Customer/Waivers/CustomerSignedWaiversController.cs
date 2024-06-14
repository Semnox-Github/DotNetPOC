/********************************************************************************************
* Project Name - CustomerSignedWaivers Controller
* Description  - CustomerSignedWaivers Controller
* 
**************
**Version Log
**************
*Version     Date             Modified By       Remarks          
*********************************************************************************************
*0.00        06-Nov-2019      Indrajeet Kumar   Created
*2.80        19-Mar-2020      Mushahid Faizan   Modified End Points and removed token from response body. 
*2.80        08-Apr-2020      Nitin Pai         Cobra changes for Waiver, Customer Registration and Online Sales/Added GET method
*2.100       19-Oct-2020      Guru S A          Enabling minor signature option for waiver
*2.110       20-Dec-2020      Nitin Pai         Modified for minor signatory enhancement 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using Semnox.Parafait.Customer;
using Semnox.Core.Utilities;
using System.Drawing;
using System.IO;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.POS;
using Semnox.Parafait.User;
using System.Data.SqlClient;

namespace Semnox.CommonAPI.Customer.Waiver
{
    public class CustomerSignedWaiversController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private Utilities utilities;

        /// <summary>
        /// Gets the list of customer signed waiver DTO list
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="customerId"></param>
        /// <param name="loadSignedWaiverFileContent"></param>
        /// <returns></returns>
        [Route("api/Customer/Waiver/CustomerSignedWaivers")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, int customerId = -1, bool loadSignedWaiverFileContent = false)
        {
            try
            {
                log.LogMethodEntry(isActive, customerId, loadSignedWaiverFileContent);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = null;
                List<int> customerIdList = new List<int>();
                bool activeRecordOnly = false;
                CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(executionContext);
                if (customerId > -1)
                {
                    customerIdList.Add(customerId);
                }

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        activeRecordOnly = true;
                    }
                }
                if (loadSignedWaiverFileContent)
                {
                    utilities = new Utilities();
                    utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                    utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                    utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                    utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                    utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                    utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                    utilities.ExecutionContext.SetMachineId(executionContext.GetMachineId());
                }
                customerSignedWaiverDTOList = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(customerIdList, activeRecordOnly, loadSignedWaiverFileContent, utilities);
                log.LogMethodExit(customerSignedWaiverDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerSignedWaiverDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }


        /// <summary>
        /// Embossing of Signature on the Waivers
        /// </summary>
        /// <param name="waiverCustomerAndSignatureDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/Waiver/CustomerSignedWaivers")]
        [Authorize]
        public HttpResponseMessage Post(List<WaiverCustomerAndSignatureDTO> waiverCustomerAndSignatureDTOList)
        {
            try
            {
                log.LogMethodEntry(waiverCustomerAndSignatureDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                // Utilities initialization
                Users user = new Users(executionContext, securityTokenDTO.LoginId, securityTokenDTO.SiteId);
                executionContext.SetUserId(user.UserDTO.LoginId);
                executionContext.SetUserPKId(user.UserDTO.UserId);

                POSMachines pOSMachines = null;
                if (executionContext.GetMachineId() != -1)
                    pOSMachines = new POSMachines(executionContext, executionContext.GetMachineId());

                utilities = new Utilities();
                utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                utilities.ParafaitEnv.SetPOSMachine(pOSMachines == null ? "" : pOSMachines.POSMachineDTO.IPAddress, pOSMachines == null ? Environment.MachineName : pOSMachines.POSMachineDTO.IPAddress);
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                utilities.ExecutionContext.SetMachineId(executionContext.GetMachineId());
                utilities.ParafaitEnv.Initialize();

                List<CustomerSignedWaiverHeaderDTO> customerSignedWaiverHeaderDTO = new List<CustomerSignedWaiverHeaderDTO>();
                if (waiverCustomerAndSignatureDTOList != null && waiverCustomerAndSignatureDTOList.Any())
                {
                    CustomerBL signatoryCustomerBL = new CustomerBL(executionContext, waiverCustomerAndSignatureDTOList[0].SignatoryCustomerDTO);
                    bool customerIsAdult = signatoryCustomerBL.IsAdult();
                    int guardianId = (customerIsAdult ? signatoryCustomerBL.CustomerDTO.Id : -1);
                    List<int> headerIdList = new List<int>();
                    ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    try
                    {
                        foreach (WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO in waiverCustomerAndSignatureDTOList)
                        {
                            int custSignedWaiverHeaderId = signatoryCustomerBL.CreateCustomerSignedWaiverHeader(waiverCustomerAndSignatureDTO.Channel, dBTransaction.SQLTrx);
                            if (custSignedWaiverHeaderId > -1)
                            {
                                //Image customerSignatureImage = GetDigitalSignature(waiverCustomerAndSignatureDTO.SignatureImageBase64); 
                                foreach (CustomerDTO signForCustomerDTO in waiverCustomerAndSignatureDTO.SignForCustomerDTOList)
                                {
                                    List<WaiveSignatureImageWithCustomerDetailsDTO> custIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                                    foreach (KeyValuePair<int, string> signatureItem in waiverCustomerAndSignatureDTO.CustIdSignatureImageBase64List)
                                    {
                                        if (signatureItem.Key == signatoryCustomerBL.CustomerDTO.Id)
                                        {
                                            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(signatureItem.Value)))
                                            {
                                                Image image = Image.FromStream(ms);
                                                Size size = new Size(200, 150);
                                                Image customerSignatureImage = (Image)(new Bitmap(image, size));
                                                string customerName = (string.IsNullOrWhiteSpace(signatoryCustomerBL.CustomerDTO.FirstName) ? "" : signatoryCustomerBL.CustomerDTO.FirstName)
                                                                          + " " + (string.IsNullOrWhiteSpace(signatoryCustomerBL.CustomerDTO.LastName) ? "" : signatoryCustomerBL.CustomerDTO.LastName);
                                                custIdNameSignatureImageList.Add(new WaiveSignatureImageWithCustomerDetailsDTO(signatoryCustomerBL.CustomerDTO.Id, customerName, customerSignatureImage));
                                            }
                                        }
                                        if (signatureItem.Key == signForCustomerDTO.Id && signatureItem.Key != signatoryCustomerBL.CustomerDTO.Id)
                                        {
                                            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(signatureItem.Value)))
                                            {
                                                Image image = Image.FromStream(ms);
                                                Size size = new Size(200, 150);
                                                Image customerSignatureImage = (Image)(new Bitmap(image, size));
                                                string customerName = (string.IsNullOrWhiteSpace(signForCustomerDTO.FirstName) ? "" : signForCustomerDTO.FirstName)
                                                                          + " " + (string.IsNullOrWhiteSpace(signForCustomerDTO.LastName) ? "" : signForCustomerDTO.LastName);
                                                custIdNameSignatureImageList.Add(new WaiveSignatureImageWithCustomerDetailsDTO(signForCustomerDTO.Id, customerName, customerSignatureImage));
                                            }
                                        }
                                    }
                                    CustomerBL customerBL = new CustomerBL(executionContext, signForCustomerDTO);
                                    customerBL.CreateCustomerSignedWaiver(waiverCustomerAndSignatureDTO.WaiversDTO, custSignedWaiverHeaderId, waiverCustomerAndSignatureDTO.CustomerContentDTOList, custIdNameSignatureImageList, -1, utilities, guardianId);
                                    customerBL.Save(dBTransaction.SQLTrx);
                                }
                            }
                            headerIdList.Add(custSignedWaiverHeaderId);
                        }

                        dBTransaction.EndTransaction();

                        foreach (int custSignedWaiverHeaderId in headerIdList)
                        {
                            List<KeyValuePair<CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters, string>> searchParameters = new List<KeyValuePair<CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters, string>>();
                            searchParameters.Add(new KeyValuePair<CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters, string>(CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID, custSignedWaiverHeaderId.ToString()));

                            CustomerSignedWaiverHeaderListBL customerSignedWaiverHeaderListBL = new CustomerSignedWaiverHeaderListBL(executionContext);
                            customerSignedWaiverHeaderDTO = customerSignedWaiverHeaderListBL.GetAllCustomerSignedWaiverList(searchParameters, true, true, null);
                            try
                            {
                                if (custSignedWaiverHeaderId > -1)
                                {
                                    SendWaiverEmail(signatoryCustomerBL.CustomerDTO, custSignedWaiverHeaderId, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error while sending waiver email", ex);
                            }
                        }

                        log.LogMethodExit();
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = customerSignedWaiverHeaderDTO });
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        dBTransaction.RollBack();
                        throw;
                    }
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        /// <summary>
        /// Converting Base64 to Image
        /// </summary>
        /// <param name="digitalSignature"></param>
        /// <returns></returns>
        private Image GetDigitalSignature(string digitalSignature)
        {
            Image digitalSign = null;
            byte[] bytes = Convert.FromBase64String(digitalSignature);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                digitalSign = Image.FromStream(ms);
            }
            return digitalSign;
        }
        private void SendWaiverEmail(CustomerDTO customerDTO, int custSignedWaiverHeaderId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(customerDTO, custSignedWaiverHeaderId, sqlTransaction);
            int guestCustomerId = CustomerListBL.GetGuestCustomerId(executionContext);
            if (customerDTO != null && customerDTO.Id > -1 && customerDTO.Id != guestCustomerId)
            {
                CustomerSignedWaiverHeaderListBL customerSignedWaiverHeaderList = new CustomerSignedWaiverHeaderListBL(executionContext);
                customerSignedWaiverHeaderList.SendWaiverEmail(customerDTO, custSignedWaiverHeaderId, utilities, sqlTransaction);
            }
            log.LogMethodExit();
        }
    }
}
