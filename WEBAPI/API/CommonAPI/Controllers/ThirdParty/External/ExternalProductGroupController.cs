/********************************************************************************************
* Project Name - CommonAPI
* Description  - ExternalProductGroupController  API -  add and delete product groups data in Parafait
* 
**************
**Version Log
**************
*Version     Date                  Modified By           Remarks          
*********************************************************************************************
*0.0        28-Sept-2020           Girish Kundar          Created 
*2.130.7    07-Apr-2022            Ashish Bhat            Modified( External  REST API.)
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Environments
{
    public class ExternalProductGroupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON ProductGroups
        /// </summary>       
        /// <param name="posName">posName</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/ProductGroups")]
        public async Task<HttpResponseMessage> Get(string posName = null)
        {
            log.LogMethodEntry();
            ExecutionContext executionContext = null;
            try
            {

                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!String.IsNullOrEmpty(posName))
                {
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_NAME, posName.ToString()));
                }
                else if (executionContext.GetMachineId() != -1)
                {
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID, executionContext.GetMachineId().ToString()));
                }
                POSMachineList pOSMachineList = new POSMachineList(executionContext);
                var content = pOSMachineList.GetAllPOSMachines(searchParameters, true, true);
                List<ExternalProductGroupDTO> externalProductGroupDTOList = new List<ExternalProductGroupDTO>();
                ExternalProductGroupDTO externalProductGroupDTO = new ExternalProductGroupDTO();

                //3rd step          
                if (content != null && content.Any())
                {
                    foreach (POSMachineDTO posMachineDTO in content)
                    {
                        List<ExternalProductDTO> externalProductDTOList = new List<ExternalProductDTO>();
                        ExternalProductDTO externalProductDTO = new ExternalProductDTO();
                        if (posMachineDTO.PosProductDisplayList != null && posMachineDTO.PosProductDisplayList.Any())
                        {
                            foreach (ProductDisplayGroupFormatDTO ProductDisplayGroupFormatDTO in posMachineDTO.PosProductDisplayList)
                            {
                                List<ProductsContainerDTO> productsContainerDTOList = ProductsContainerList.GetProductContainerDTOListOfDisplayGroups(executionContext.GetSiteId(), new List<int> { ProductDisplayGroupFormatDTO.Id });
                                if (productsContainerDTOList != null && productsContainerDTOList.Any())
                                {
                                    foreach (ProductsContainerDTO productsContainerDTO in productsContainerDTOList)
                                    {
                                        List<int> displayIdList = new List<int>();
                                        List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParametersGroup = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                                        searchParametersGroup.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                        if (productsContainerDTO.ProductId > 0)
                                        {
                                            searchParametersGroup.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, productsContainerDTO.ProductId.ToString()));
                                        }
                                        IProductDisplayGroupUseCases productsDisplayGroupsUseCases = ProductsUseCaseFactory.GetProductsDisplayGroups(executionContext);
                                        List<ProductsDisplayGroupDTO> productsDisplayGroupsDTOList = await productsDisplayGroupsUseCases.GetProductsDisplayGroups(searchParametersGroup);
                                        if (productsDisplayGroupsDTOList != null && productsDisplayGroupsDTOList.Any())
                                        {
                                            foreach (ProductsDisplayGroupDTO productDisplayGroupDTO in productsDisplayGroupsDTOList)
                                            {
                                                displayIdList.Add(productDisplayGroupDTO.DisplayGroupId);
                                            }
                                        }
                                        externalProductDTO = new ExternalProductDTO(productsContainerDTO.ProductId, displayIdList, productsContainerDTO.ProductName, Convert.ToDouble(productsContainerDTO.Price),
                                            productsContainerDTO.TaxId, Convert.ToInt32(productsContainerDTO.FaceValue), productsContainerDTO.TaxInclusivePrice, productsContainerDTO.ExpiryDate.ToString(), productsContainerDTO.AvailableUnits, productsContainerDTO.Description,
                                            productsContainerDTO.WebDescription, productsContainerDTO.OnlyForVIP == "N" ? false : true, productsContainerDTO.AllowPriceOverride == "N" ? false : true, productsContainerDTO.RegisteredCustomerOnly == "N" ? false : true, productsContainerDTO.ManagerApprovalRequired == "N" ? false : true,
                                            productsContainerDTO.MinimumQuantity, productsContainerDTO.DisplayInPOS == "N" ? false : true, productsContainerDTO.ProductType, productsContainerDTO.CategoryName, Convert.ToInt32(productsContainerDTO.TaxPercentage), productsContainerDTO.MaxQtyPerDay.ToString(),
                                            productsContainerDTO.ExternalSystemReference, string.Empty, false, productsContainerDTO.SiteId);
                                        externalProductDTOList.Add(externalProductDTO);
                                    }
                                }
                                externalProductGroupDTO = new ExternalProductGroupDTO(ProductDisplayGroupFormatDTO.DisplayGroup, ProductDisplayGroupFormatDTO.Id,
                            externalProductDTOList);
                                externalProductGroupDTOList.Add(externalProductGroupDTO);
                            }
                        }
                    }
                }
                log.LogMethodExit(externalProductGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, externalProductGroupDTOList);
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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