/********************************************************************************************
 * Project Name - User Role
 * Description  -  Controller of the User Roles Refresh Functions.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.80.0      14-Oct-2019   Jagan Mohana  Created
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 *2.150.3     29-MAY-2023   Sweedol             Modified: Removed generating management form access records for Ticket Station.
  ********************************************************************************************/
using Semnox.Parafait.User;
using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Site;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.POS;
using Semnox.Parafait.Reports;
using Semnox.Parafait.Redemption;
using System.Data.SqlClient;

namespace Semnox.CommonAPI.Helpers
{
    public class ManagementFormAccessService
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<UserRolesDTO> userRolesDTOList;
        private List<ManagementFormAccessDTO> allLoadedManagementFormAccessDTOList;
        #endregion
        public ManagementFormAccessService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            allLoadedManagementFormAccessDTOList = LoadAllLists();
            log.LogMethodExit();
        }
        public List<ManagementFormAccessDTO> GetManagementFormAccessList(int roleId = -1)
        {
            List<ManagementFormAccessDTO> existingManagementFormAccessDTOList = new List<ManagementFormAccessDTO>();
            List<ManagementFormAccessDTO> result = new List<ManagementFormAccessDTO>();
            ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext);

            List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, roleId.ToString()));
            existingManagementFormAccessDTOList = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParams);

            if (existingManagementFormAccessDTOList != null && existingManagementFormAccessDTOList.Any())
            {
                foreach (ManagementFormAccessDTO formAccessDTO in allLoadedManagementFormAccessDTOList)
                {
                    List<ManagementFormAccessDTO> existing = existingManagementFormAccessDTOList.FindAll(m => m.FormName.ToLower() == formAccessDTO.FormName.ToLower()
                                                                && m.MainMenu.ToLower() == formAccessDTO.MainMenu.ToLower() &&
                                                                    m.FunctionGroup.ToLower() == formAccessDTO.FunctionGroup.ToLower());
                    if (existing == null || existing.Count == 0)
                    {//inserting new valid record
                        result.Add(new ManagementFormAccessDTO(formAccessDTO.ManagementFormAccessId, roleId, formAccessDTO.MainMenu, formAccessDTO.FormName,
                            formAccessDTO.AccessAllowed, formAccessDTO.FunctionId, formAccessDTO.FunctionGroup, formAccessDTO.FunctionGUID, formAccessDTO.IsActive));
                    }
                    else if (existing.Count > 1 && (result.Any(m => m.FormName.ToLower() == formAccessDTO.FormName.ToLower()
                                                    && m.MainMenu.ToLower() == formAccessDTO.MainMenu.ToLower() &&
                                                    m.FunctionGroup.ToLower() == formAccessDTO.FunctionGroup.ToLower() &&
                                                    m.RoleId == formAccessDTO.RoleId)) == false)
                    {//inactivating duplicate records
                        if (existing.Any(x => x.FunctionGUID == formAccessDTO.FunctionGUID))
                        {
                            ManagementFormAccessDTO accessDTO = existing.Where(x => x.FunctionGUID == formAccessDTO.FunctionGUID).FirstOrDefault();
                            existing.Remove(accessDTO);
                            existingManagementFormAccessDTOList.Remove(accessDTO);

                            if (accessDTO.IsActive == false)
                            {
                                result.Add(new ManagementFormAccessDTO(accessDTO.ManagementFormAccessId, accessDTO.RoleId, accessDTO.MainMenu,
                                                        accessDTO.FormName, accessDTO.AccessAllowed, accessDTO.FunctionId,
                                                        accessDTO.FunctionGroup, accessDTO.FunctionGUID, true));
                            }

                            foreach (ManagementFormAccessDTO formDTO in existing)
                            {
                                result.Add(new ManagementFormAccessDTO(formDTO.ManagementFormAccessId, formDTO.RoleId, formDTO.MainMenu,
                                                        formDTO.FormName, formDTO.AccessAllowed, formDTO.FunctionId,
                                                        formDTO.FunctionGroup, formDTO.FunctionGUID, false));
                                existingManagementFormAccessDTOList.Remove(formDTO);
                            }
                        }
                        else
                        {
                            ManagementFormAccessDTO accessDTO = existing.FirstOrDefault();
                            existing.Remove(accessDTO);
                            existingManagementFormAccessDTOList.Remove(accessDTO);

                            if (accessDTO.IsActive == false)
                            {
                                result.Add(new ManagementFormAccessDTO(accessDTO.ManagementFormAccessId, roleId, accessDTO.MainMenu,
                                                        accessDTO.FormName, accessDTO.AccessAllowed, accessDTO.FunctionId,
                                                        accessDTO.FunctionGroup, formAccessDTO.FunctionGUID, true));
                            }
                            else
                            {
                                result.Add(new ManagementFormAccessDTO(accessDTO.ManagementFormAccessId, roleId, accessDTO.MainMenu,
                                                        accessDTO.FormName, accessDTO.AccessAllowed, accessDTO.FunctionId,
                                                        accessDTO.FunctionGroup, formAccessDTO.FunctionGUID, accessDTO.IsActive));
                            }
                            foreach (ManagementFormAccessDTO formDTO in existing)
                            {
                                result.Add(new ManagementFormAccessDTO(formDTO.ManagementFormAccessId, formDTO.RoleId, formDTO.MainMenu,
                                                        formDTO.FormName, formDTO.AccessAllowed, formDTO.FunctionId,
                                                        formDTO.FunctionGroup, formDTO.FunctionGUID, false));
                                existingManagementFormAccessDTOList.Remove(formDTO);
                            }
                        }
                    }
                }

                foreach (ManagementFormAccessDTO formAccessDTO in existingManagementFormAccessDTOList)
                {
                    var existing = allLoadedManagementFormAccessDTOList.Find(m => m.FormName.ToLower() == formAccessDTO.FormName.ToLower() && m.MainMenu.ToLower() == formAccessDTO.MainMenu.ToLower() &&
                                    m.FunctionGroup.ToLower() == formAccessDTO.FunctionGroup.ToLower());
                    if (existing == null)
                    {//inactivate if valid record doesnt exist
                        result.Add(new ManagementFormAccessDTO(formAccessDTO.ManagementFormAccessId, formAccessDTO.RoleId, formAccessDTO.MainMenu, formAccessDTO.FormName,
                            formAccessDTO.AccessAllowed, formAccessDTO.FunctionId, formAccessDTO.FunctionGroup, formAccessDTO.FunctionGUID, false));
                    }
                    else
                    {//update functionGuid if doesnt match with entity GUID and activate if inactive
                        if ((result.Any(m => m.ManagementFormAccessId == formAccessDTO.ManagementFormAccessId)) == false)
                        {
                            if (string.IsNullOrWhiteSpace(formAccessDTO.FunctionGUID) || formAccessDTO.FunctionGUID != existing.FunctionGUID)
                            {
                                if (formAccessDTO.IsActive == false)
                                {
                                    result.Add(new ManagementFormAccessDTO(formAccessDTO.ManagementFormAccessId, formAccessDTO.RoleId, formAccessDTO.MainMenu, formAccessDTO.FormName,
                                    formAccessDTO.AccessAllowed, formAccessDTO.FunctionId, formAccessDTO.FunctionGroup, existing.FunctionGUID, true));
                                }
                                else
                                {
                                    result.Add(new ManagementFormAccessDTO(formAccessDTO.ManagementFormAccessId, formAccessDTO.RoleId, formAccessDTO.MainMenu, formAccessDTO.FormName,
                                        formAccessDTO.AccessAllowed, formAccessDTO.FunctionId, formAccessDTO.FunctionGroup, existing.FunctionGUID, formAccessDTO.IsActive));
                                }
                            }
                            else if (formAccessDTO.IsActive == false)
                            {
                                result.Add(new ManagementFormAccessDTO(formAccessDTO.ManagementFormAccessId, formAccessDTO.RoleId, formAccessDTO.MainMenu, formAccessDTO.FormName,
                                    formAccessDTO.AccessAllowed, formAccessDTO.FunctionId, formAccessDTO.FunctionGroup, formAccessDTO.FunctionGUID, true));
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (ManagementFormAccessDTO formAccessDTO in allLoadedManagementFormAccessDTOList)
                {
                    result.Add(new ManagementFormAccessDTO(formAccessDTO.ManagementFormAccessId, roleId, formAccessDTO.MainMenu, formAccessDTO.FormName,
                            formAccessDTO.AccessAllowed, formAccessDTO.FunctionId, formAccessDTO.FunctionGroup, formAccessDTO.FunctionGUID, formAccessDTO.IsActive));
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<ManagementFormAccessDTO> RefreshUserRoles()
        {
            log.LogMethodEntry();
            List<ManagementFormAccessDTO> managementFormAccessDTOList = new List<ManagementFormAccessDTO>();
            if (userRolesDTOList != null && userRolesDTOList.Any())
            {
                for (int i = 0; i < userRolesDTOList.Count; i++)
                {
                    List<ManagementFormAccessDTO> result = GetManagementFormAccessList(userRolesDTOList[i].RoleId);
                    if(result != null && result.Any())
                    {
                        managementFormAccessDTOList.AddRange(result);
                    }
                }
            }
            log.LogMethodExit(managementFormAccessDTOList);
            return managementFormAccessDTOList;
        }

        private List<ManagementFormAccessDTO> LoadAllLists()
        {
            log.LogMethodEntry();
            List<ManagementFormAccessDTO> managementFormAccessDTOList = new List<ManagementFormAccessDTO>();

            //Load all reports for given site
            List<ReportsDTO> reportsDTOList = new List<ReportsDTO>();
            ReportsList reportsList = new ReportsList();
            List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> searchReportsParameters = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
            searchReportsParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchReportsParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.IS_ACTIVE, "1"));
            reportsDTOList = reportsList.GetAllReports(searchReportsParameters);
            if (reportsDTOList != null && reportsDTOList.Any())
            {
                for (int i = 0; i < reportsDTOList.Count; i++)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(-1,-1, "Run Reports", reportsDTOList[i].ReportName, false,-1, "Reports", reportsDTOList[i].Guid,true);
                    managementFormAccessDTOList.Add(managementFormAccessDTO);
                }
            }

            //Game Prfile List 
            List<GameProfileDTO> gameProfileDTOList = new List<GameProfileDTO>();
            GameProfileList gameProfileList = new GameProfileList(executionContext);
            List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.IS_ACTIVE, "1"));
            gameProfileDTOList = gameProfileList.GetGameProfileDTOList(searchParameters);
            if (gameProfileDTOList != null && gameProfileDTOList.Any())
            {
                for (int i = 0; i < gameProfileDTOList.Count; i++)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(-1, -1, "Game Profile", gameProfileDTOList[i].ProfileName, false, -1, "Data Access", gameProfileDTOList[i].Guid, true);
                    managementFormAccessDTOList.Add(managementFormAccessDTO);
                }
            }

            //Pos type list
            List<POSTypeDTO> posTypeDTOList = new List<POSTypeDTO>();
            POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
            List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParam.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            posTypeDTOList = pOSTypeListBL.GetPOSTypeDTOList(searchParam);
            if (posTypeDTOList != null && posTypeDTOList.Any())
            {
                for (int i = 0; i < posTypeDTOList.Count; i++)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(-1, -1, "POS Counter", posTypeDTOList[i].POSTypeName, false, -1, "Data Access", posTypeDTOList[i].Guid, true);
                    managementFormAccessDTOList.Add(managementFormAccessDTO);
                }
            }

            // POSMachineList
            List<POSMachineDTO> pOSMachineDTOList = new List<POSMachineDTO>();
            POSMachineList pOSMachineList = new POSMachineList(executionContext);
            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameter = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParameter.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameter.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
            pOSMachineDTOList = pOSMachineList.GetAllPOSMachines(searchParameter, false, false);
            if (pOSMachineDTOList != null && pOSMachineDTOList.Any())
            {
                for (int i = 0; i < pOSMachineDTOList.Count; i++)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(-1, -1, "POS Machine", pOSMachineDTOList[i].POSName, false, -1, "Data Access", pOSMachineDTOList[i].Guid, true);
                    managementFormAccessDTOList.Add(managementFormAccessDTO);
                }
            }

            //RedemptionCurrencyList
            List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = new List<RedemptionCurrencyDTO>();
            RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(executionContext);
            List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchRedemptionCurrencyParameter = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
            searchRedemptionCurrencyParameter.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchRedemptionCurrencyParameter.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "1"));
            redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(searchRedemptionCurrencyParameter);
            if (redemptionCurrencyDTOList != null && redemptionCurrencyDTOList.Any())
            {
                for (int i = 0; i < redemptionCurrencyDTOList.Count; i++)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(-1, -1, "Redemption Currency", redemptionCurrencyDTOList[i].CurrencyName, false, -1, "Data Access", redemptionCurrencyDTOList[i].Guid, true);
                    managementFormAccessDTOList.Add(managementFormAccessDTO);
                }
            }

            //UserRolesList
            userRolesDTOList = new List<UserRolesDTO>();
            UserRolesList userRolesList = new UserRolesList();
            List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> userRolesSearchParams = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
            userRolesSearchParams.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            userRolesSearchParams.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ISACTIVE, "1"));
            userRolesDTOList = userRolesList.GetAllUserRoles(userRolesSearchParams);
            if (userRolesDTOList != null && userRolesDTOList.Any())
            {
                for (int i = 0; i < userRolesDTOList.Count; i++)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(-1, -1, "User Roles", userRolesDTOList[i].Role, false, -1, "Data Access", userRolesDTOList[i].Guid, true);
                    managementFormAccessDTOList.Add(managementFormAccessDTO);
                }
            }

            //SiteList
            List<SiteDTO> siteDTOList = new List<SiteDTO>();
            SiteList siteList = new SiteList(executionContext);
            List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> siteSearchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
            siteSearchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "Y"));
            siteDTOList = siteList.GetAllSites(siteSearchParameters);
            if (siteDTOList != null && siteDTOList.Any())
            {
                for (int i = 0; i < siteDTOList.Count; i++)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(-1, -1, "Sites", siteDTOList[i].SiteName, false, -1, "Data Access", siteDTOList[i].Guid, true);
                    managementFormAccessDTOList.Add(managementFormAccessDTO);
                }
            }

            //ManagementFormsListBL
            List<ManagementFormsDTO> managementFormsDTOList = new List<ManagementFormsDTO>();
            ManagementFormsListBL managementFormsListBL = new ManagementFormsListBL(executionContext);
            List<KeyValuePair<ManagementFormsDTO.SearchByParameters, string>> searchManagementFormsParameter = new List<KeyValuePair<ManagementFormsDTO.SearchByParameters, string>>();
            searchManagementFormsParameter.Add(new KeyValuePair<ManagementFormsDTO.SearchByParameters, string>(ManagementFormsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchManagementFormsParameter.Add(new KeyValuePair<ManagementFormsDTO.SearchByParameters, string>(ManagementFormsDTO.SearchByParameters.ISACTIVE, "1"));
            managementFormsDTOList = managementFormsListBL.GetManagementFormsDTOList(searchManagementFormsParameter);
            if (managementFormsDTOList != null && managementFormsDTOList.Any())
            {
                for (int i = 0; i < managementFormsDTOList.Count; i++)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(-1, -1, managementFormsDTOList[i].GroupName, managementFormsDTOList[i].FormName, false, -1, managementFormsDTOList[i].FunctionGroup, managementFormsDTOList[i].Guid, true);
                    managementFormAccessDTOList.Add(managementFormAccessDTO);
                }
            }

            //SystemOptionsList
            List<SystemOptionsDTO> systemOptionsDTOList = new List<SystemOptionsDTO>();
            SystemOptionsList systemOptionsList = new SystemOptionsList(executionContext);
            List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>> searchSystemOptionsParameter = new List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>>();
            searchSystemOptionsParameter.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.OPTION_TYPE, "POS Task Access"));
            searchSystemOptionsParameter.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchSystemOptionsParameter.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.IS_ACTIVE, "1"));
            systemOptionsDTOList = systemOptionsList.GetSystemOptionsDTOList(searchSystemOptionsParameter);
            if (systemOptionsDTOList != null && systemOptionsDTOList.Any())
            {
                for (int i = 0; i < systemOptionsDTOList.Count; i++)
                {
                    ManagementFormAccessDTO managementFormAccessDTO = new ManagementFormAccessDTO(-1, -1, "Parafait POS", systemOptionsDTOList[i].OptionName, false, -1, systemOptionsDTOList[i].OptionType, systemOptionsDTOList[i].Guid, true);
                    managementFormAccessDTOList.Add(managementFormAccessDTO);
                }
            }

            log.LogMethodExit();
            return managementFormAccessDTOList;
        }
    }
}