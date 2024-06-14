/********************************************************************************************
* Project Name - RelatedCustomer Controller
* Description  - The Below Controller is used for add a CustomerRelationship to the Primary Customer
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*0.00        06-Nov-2019      Indrajeet Kumar    Created
* 2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
*2.130.8     10-Jun-2022      Nitin Pai          Removed the site id from search params as relations created in site were not visible
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Core.GenericUtilities;
using System.Linq;

namespace Semnox.CommonAPI.Customer
{
    public class CustomerRelationshipController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// Gets the All the Related Customer collection
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Customer/CustomerRelationships")]
        [Authorize]
        public HttpResponseMessage Get(int customerId, bool buildChildRecords = false, string isActive = null)
        {
            try
            {
                log.LogMethodEntry(customerId, buildChildRecords, isActive);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));

                // If isactive = 0 then fetch all the records
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                }
                else
                {
                    // Fetch active records by default.
                    searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(executionContext);
                List<CustomerRelationshipDTO> customerRelationshipDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchParameters, buildChildRecords);
                log.LogMethodExit(customerRelationshipDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { CustomerRelationshipDTO = customerRelationshipDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        /// <summary>
        /// Posting Customer Relationship to the primary customer
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/CustomerRelationships")]
        [Authorize]
        public HttpResponseMessage Post(List<CustomerRelationshipDTO> customerRelationshipDTOList)
        {
            try
            {
                log.LogMethodEntry(customerRelationshipDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (customerRelationshipDTOList != null && customerRelationshipDTOList.Any())
                {
                    foreach (CustomerRelationshipDTO customerRelationshipDTO in customerRelationshipDTOList)
                    {
                        CustomerRelationshipBL customerRelationshipBL = new CustomerRelationshipBL(executionContext, customerRelationshipDTO);
                        customerRelationshipBL.Save();
                    }
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customerRelationshipDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}
