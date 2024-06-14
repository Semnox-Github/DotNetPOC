/********************************************************************************************
 * Project Name - Product Vendor Controller
 * Description  - Created to fetch, update and insert in the Product Vendor entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60.2     13-Jun-2019   Nagesh Badiger           Created 
 *2.100.0    14-Oct-2020  Mushahid Faizan         Modified: as per API Standards, namespace changes, endPoint Changes, added searchParameters in get(),
 *                                                Renamed Controller from ProductVendorController to VendorController
 *2.110.0    23-Nov-2020   Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Vendor;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class VendorController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Vendor.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Vendors")]
        public async Task<HttpResponseMessage> Get(string isActive = null, string city = null, int vendorId = -1, string phone = null, string vendorIdList = null,
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
                if (!string.IsNullOrEmpty(vendorIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> vendorListId = new List<int>();

                    vendorListId = vendorIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String productsIdListString = String.Join(",", vendorListId.ToArray());
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDOR_ID_LIST, productsIdListString));
                }
                VendorList vendorList = new VendorList(executionContext);

                int totalNoOfPages = 0;
                int totalCount = await Task<int>.Factory.StartNew(() => { return vendorList.GetVendorCount(searchParameters, null); });
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

                IVendorUseCases vendorUseCases = InventoryUseCaseFactory.GetVendorUseCases(executionContext);
                List<VendorDTO> vendorDTOList = await vendorUseCases.GetVendors(searchParameters, currentPage, pageSize);
                log.LogMethodExit(vendorDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = vendorDTOList, currentPageNo = currentPage, TotalCount = totalCount });

            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Product Vendor
        /// </summary>
        /// <param name="vendorDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Inventory/Vendors")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<VendorDTO> vendorDTOList)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(vendorDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (vendorDTOList == null || vendorDTOList.Any(a => a.VendorId > 0))
                {
                    log.LogMethodExit(vendorDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IVendorUseCases vendorUseCases = InventoryUseCaseFactory.GetVendorUseCases(executionContext);
                await vendorUseCases.SaveVendors(vendorDTOList);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = vendorDTOList });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Product Vendor
        /// </summary>
        /// <param name="vendorDTOList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Inventory/Vendors")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<VendorDTO> vendorDTOList)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(vendorDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (vendorDTOList == null || vendorDTOList.Any(a => a.VendorId < 0))
                {
                    log.LogMethodExit(vendorDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IVendorUseCases vendorUseCases = InventoryUseCaseFactory.GetVendorUseCases(executionContext);
                await vendorUseCases.SaveVendors(vendorDTOList);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = vendorDTOList });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
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
