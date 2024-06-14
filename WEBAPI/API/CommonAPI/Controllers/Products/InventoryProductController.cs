/********************************************************************************************
 * Project Name - Product                                                                          
 * Description  - Downloads all Inventory products. 
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By             Remarks          
 *********************************************************************************************
 *2.70.0     28-Jun-2019      Mehraj                  Created   
  *2.70.0    20-Aug-2019      Akshay Gulaganji        modified Post()
 *2.110.0    10-Nov-2020      Vikas Dwivedi           Modified as per the REST API Standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Products
{
    public class InventoryProductController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Downloads inventory products data
        /// Converts UploadInventoryProductDTO to Sheet object response
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Product/InventoryProductUpload")]
        [Authorize]
        public HttpResponseMessage Get(string sheetType = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if(string.IsNullOrEmpty(sheetType))
                {
                    sheetType = "DOWNLOADINVENTORYFILEFORMATE";
                }
                UploadInventory uploadInventoryList = new UploadInventory(executionContext);
                Sheet sheet = new Sheet();
                if (sheetType.ToUpper().ToString() == "DOWNLOADINVENTORYFILEFORMATE")
                {
                    sheet = uploadInventoryList.BuildTemplete();
                }
                else if(sheetType.ToUpper().ToString() == "DOWNLOADINVENTORYPRODUCT")
                {
                    List<UploadInventoryProductDTO> uploadInventoryProductsList = uploadInventoryList.DownloadData();
                    if (uploadInventoryProductsList != null && uploadInventoryProductsList.Count > 0)
                    {
                        //uploadInventoryProductsList contains the collection which will be populated to sheet 
                        //uploadInventoryProductDTODefination is the mirror of ProductDTO and Segments which does data manupulation
                        //uploadInventoryProductDTODefination inherits complexattribute defination class which has abstract methods like Serialize, DeSerialize and Configure
                        UploadInventoryProductDTODefination uploadInventoryProductDTODefination = new UploadInventoryProductDTODefination(executionContext, "");
                        foreach (UploadInventoryProductDTO uploadInventoryProductDTODef in uploadInventoryProductsList)
                        {
                            uploadInventoryProductDTODefination.Configure(uploadInventoryProductDTODef);
                        }

                        Row headerRow = new Row();
                        uploadInventoryProductDTODefination.BuildHeaderRow(headerRow);
                        sheet.AddRow(headerRow);
                        foreach (UploadInventoryProductDTO uploadInventoryProductDTODef in uploadInventoryProductsList)
                        {
                            Row row = new Row();
                            uploadInventoryProductDTODefination.Serialize(row, uploadInventoryProductDTODef);
                            sheet.AddRow(row);
                        }
                    }
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
                log.LogMethodExit(sheet);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = sheet });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Uploads inventory products
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/InventoryProductUpload")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]Sheet sheet, [FromUri]string category, [FromUri]string vendor, [FromUri]string uom, [FromUri]int tax, [FromUri]int inboundLocation, [FromUri]int outboundLocation, [FromUri]bool isPurchasable)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(sheet, category, vendor, uom, tax, inboundLocation, outboundLocation, isPurchasable);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                //Setting POSMachineId to executionContext from HttpContext.Current.Application["POSMachineId"]
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, Convert.ToInt32(HttpContext.Current.Application["POSMachineId"]), -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                              

                if (sheet.RowList != null && sheet.RowList.Count > 0)
                {
                    UploadInventory uploadInventoryList = new UploadInventory(executionContext);
                    var content = uploadInventoryList.UploadData(sheet, category, vendor, uom, tax, inboundLocation, outboundLocation, true);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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
