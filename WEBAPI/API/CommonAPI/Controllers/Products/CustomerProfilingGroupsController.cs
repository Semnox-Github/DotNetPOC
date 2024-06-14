/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch, update and insert customer profiles in the product details.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.150.0    25-Apr-2022   Abhishek                 Created - As a part of Customer Profile Enhancement 
                                                    to Get,Save Customer Profile Group 
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Products
{
    public class CustomerProfilingGroupsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON products modifiers.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/CustomerProfilingGroups")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int customerProfileGroupId = -1, string groupName = null, 
                                                   bool buildChildRecords = false, bool loadActiveChildRecords = false, int currentPage = 0, int pageSize = 0)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(customerProfileGroupId, groupName, isActive, buildChildRecords, loadActiveChildRecords);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>(CustomerProfilingGroupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>(CustomerProfilingGroupDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (customerProfileGroupId > 0)
                {
                    searchParameters.Add(new KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>(CustomerProfilingGroupDTO.SearchByParameters.CUSTOMER_PROFILE_GROUP_ID, customerProfileGroupId.ToString()));
                }
                if (string.IsNullOrEmpty(groupName) == false)
                {
                    searchParameters.Add(new KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>(CustomerProfilingGroupDTO.SearchByParameters.GROUP_NAME, groupName));
                }
                IProductsUseCases customerProfilingGroupUseCases = ProductsUseCaseFactory.GetCustomerProfileGroupsUseCases(executionContext);
                List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList = await customerProfilingGroupUseCases.GetCustomerProfilingGroups(searchParameters,buildChildRecords, loadActiveChildRecords);
                log.LogMethodExit(customerProfilingGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerProfilingGroupDTOList });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Product Display Groups
        /// </summary>
        /// <param name="customerProfilingGroupDTOList">customerProfilingGroupDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/CustomerProfilingGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(customerProfilingGroupDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (customerProfilingGroupDTOList == null)
                {
                    log.LogMethodExit(customerProfilingGroupDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProductsUseCases customerProfilingGroupUseCases = ProductsUseCaseFactory.GetCustomerProfileGroupsUseCases(executionContext);
                List<CustomerProfilingGroupDTO> customerProfilingGroupList = await customerProfilingGroupUseCases.SaveCustomerProfilingGroups(customerProfilingGroupDTOList);
                log.LogMethodExit(customerProfilingGroupList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerProfilingGroupList });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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
