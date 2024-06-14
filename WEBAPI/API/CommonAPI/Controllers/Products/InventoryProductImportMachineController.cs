/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Inventory Product Import Machine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.100.0     20-Oct-2020   Vikas Dwivedi          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Products
{
    public class InventoryProductImportMachineController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("api/Products/InventoryProductImportMachines")]
        [Authorize]
        public HttpResponseMessage Get()
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                MachineList machineList = new MachineList(executionContext);
                List<MachineDTO> machinesList =  machineList.GetNonInventoryLocationMachines(executionContext.GetSiteId());
                log.LogMethodExit(machinesList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = machinesList });
            }

            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        [Route("api/Products/InventoryProductImportMachines")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MachineDTO> machinesDTOList, int locationTypeId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(machinesDTOList, locationTypeId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (machinesDTOList.Count != 0 && locationTypeId > 0)
                {
                    foreach (MachineDTO machineDTO in machinesDTOList)
                    {
                        if (machineDTO.MachineId > -1)
                        {
                            LocationDTO locationDTO = new LocationDTO(); 
                            locationDTO.Name = machineDTO.MachineName;
                            locationDTO.LocationTypeId = locationTypeId;
                            locationDTO.IsActive = true;
                            locationDTO.IsAvailableToSell = "N";
                            locationDTO.IsTurnInLocation = "N";
                            locationDTO.IsStore = "N";
                            locationDTO.MassUpdatedAllowed = "Y";
                            locationDTO.RemarksMandatory = "N";
                            locationDTO.Barcode = "";
                            locationDTO.ExternalSystemReference = "";
                            locationDTO.CustomDataSetId = -1;
                            locationDTO.SiteId = executionContext.GetSiteId();
                            LocationBL Location = new LocationBL(executionContext, locationDTO);
                            int LocationId = Location.Save();
                            machineDTO.InventoryLocationId = Location.GetLocationDTO.LocationId;
                            machineDTO.MachineCommunicationLogDTO = null;
                        }
                    }
                    MachineList machineListBL = new MachineList(executionContext, machinesDTOList);
                    machineListBL.SaveUpdateMachinesList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = machinesDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
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
