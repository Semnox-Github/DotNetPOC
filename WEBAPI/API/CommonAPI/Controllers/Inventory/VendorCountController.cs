/********************************************************************************************
 * Project Name -  VendorCount Controller
 * Description  - Created VendorCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   11-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Vendor;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class VendorCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Vendor.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/VendorCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null, string city = null, int vendorId = -1, string phone = null,
                                                    string state = null, string country = null, string postalCode = null, string email = null, string vendorName = null, string vendorCode = null,
                                                    string address = null, string contactName = null, string vendorMarkupPercent = null, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(address))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.ADDRESS, address.ToString()));
                }
                if (!string.IsNullOrEmpty(city))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.CITY, city.ToString()));
                }
                if (!string.IsNullOrEmpty(contactName))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.CONTACT_NAME, contactName.ToString()));
                }
                if (!string.IsNullOrEmpty(country))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.COUNTRY, country.ToString()));
                }
                if (!string.IsNullOrEmpty(email))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.EMAIL, email.ToString()));
                }
                if (!string.IsNullOrEmpty(vendorName))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.NAME, vendorName.ToString()));
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.PHONE, phone.ToString()));
                }
                if (!string.IsNullOrEmpty(postalCode))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.POSTAL_CODE, postalCode.ToString()));
                }
                if (!string.IsNullOrEmpty(state))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.STATE, state.ToString()));
                }
                if (!string.IsNullOrEmpty(vendorCode))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDORCODE, vendorCode.ToString()));
                }
                if (!string.IsNullOrEmpty(vendorMarkupPercent))
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDORMARKUPPERCENT, vendorMarkupPercent.ToString()));
                }
                if (vendorId > -1)
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDOR_ID, vendorId.ToString()));
                }
                VendorList vendorList = new VendorList(executionContext);
                IVendorUseCases vendorUseCases = InventoryUseCaseFactory.GetVendorUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalCount = await vendorUseCases.GetVendorCount(searchParameters);
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalCount);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalCount, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = ExceptionSerializer.Serialize(ex)
                });
            }
        }
    }
}