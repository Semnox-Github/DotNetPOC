/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for retrieving technician
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.100.0    24-Sept-2020    Mushahid Faizan Created
 ********************************************************************************************/
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Parafait.Maintenance;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Controllers.Maintenance
{
    public class TechnicianLookupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string TEAM_LOOKUP_NAME = "SERVICE_REQUEST_TECHNICIAN_TEAM";

        /// <summary>
        /// Get list of Technician's 
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/AssetTechnician")]
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
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                Dictionary<string, string> technicianList = new Dictionary<string, string>();

                CommonLookupsDTO lookups = new CommonLookupsDTO();
                lookups.DropdownName = "Technician";
                List<CommonLookupDTO> commonLookupDTOList = new List<CommonLookupDTO>();
                lookups.Items = commonLookupDTOList;

                CommonLookupDTO lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                lookups.Items.Add(lookupDataObject);


                UserRolesList userRolesList = new UserRolesList(executionContext);
                List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ISACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                List<UserRolesDTO> userRolesDTOList = userRolesList.GetAllUserRoles(searchParameters,true,true);

                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                Dictionary<int, LookupValuesDTO> lookupValues = lookupValuesList.GetLookupValuesMap(TEAM_LOOKUP_NAME);

                List<int> uniqueUsers = new List<int>();
                List<UsersDTO> validUsersDTO = new List<UsersDTO>();

                if (lookupValues != null && lookupValues.Any() && userRolesDTOList != null && userRolesDTOList.Any())
                {
                    foreach (KeyValuePair<int, LookupValuesDTO> lookupValue in lookupValues)
                    {
                        String userRoleValue = lookupValue.Value.LookupValue.ToUpper();
                        log.Debug("\tTechnician Team Role : {userRoleValue}");

                        foreach (UserRolesDTO userRoleDTO in userRolesDTOList)
                        {
                            if (userRoleDTO.Role.ToUpper().Equals(userRoleValue))
                            {
                                List<UsersDTO> usersDTO = userRoleDTO.UsersDTO;
                                if (usersDTO != null & usersDTO.Any())
                                {
                                    validUsersDTO.AddRange(usersDTO);
                                }
                            }
                        }
                    }

                    //fetch unique user list
                    foreach (UsersDTO usersDTO in validUsersDTO)
                    {
                        if (!uniqueUsers.Contains(usersDTO.UserId))
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.UserName);
                            lookups.Items.Add(lookupDataObject);
                        }


                    }
                    
                }
                
                log.LogMethodExit(lookups);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = lookups });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
			
			
        }

        
    }
}
