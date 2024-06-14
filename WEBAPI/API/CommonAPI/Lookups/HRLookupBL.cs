/********************************************************************************************
* Project Name - HRLookupBL
* Description  - Created a lookup values in HR Module.
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*2.80        12-Nov-2019   Indrajeet Kumar    Created.                                            
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Lookups
{ 
    public class HRLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private readonly ExecutionContext executionContext;
        private CommonLookupsDTO lookupDTO;
        private string dependentDropdownName = string.Empty;
        private string dependentDropdownSelectedId = string.Empty;
        private string isActive = string.Empty;
        private DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private CommonLookupDTO lookupDataObject;
        public List<LookupValuesDTO> lookupValuesDTOList;

        /// <summary>
        /// Parmeterized Constructor
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public HRLookupBL(string entityName, ExecutionContext executioncontext)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }

        public enum HREntityLookup
        {
            SETUP,
            LEAVEMANAGEMENT,
            APPLYLEAVE,
            PAYCONFIGURATIONS
        }        

        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                string dropdownNames = "";
                string[] dropdowns = null;
                HREntityLookup hREntityLookup = (HREntityLookup)Enum.Parse(typeof(HREntityLookup), entityName.ToUpper().ToString());
                switch (hREntityLookup)
                {
                    case HREntityLookup.LEAVEMANAGEMENT:
                        dropdownNames = "LEAVE_TYPES,TYPE,HOUR,MINUTE,AMPM,ATTENDANCETYPE";
                        break;
                    case HREntityLookup.SETUP:
                        dropdownNames = "ROLE,DEPARTMENT,FREQUENCY,LEAVE_TYPES,ATTENDANCETYPE,YEAR";
                        break;
                    case HREntityLookup.APPLYLEAVE:
                        dropdownNames = "LEAVE_TYPES,STARTHALF,ENDHALF,MANAGER";
                        break;
                    case HREntityLookup.PAYCONFIGURATIONS:
                        dropdownNames = "PAYTYPE";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                foreach (string dropdownName in dropdowns)
                {
                    CommonLookupsDTO lookupDTO = new CommonLookupsDTO();
                    lookupDTO.Items = new List<CommonLookupDTO>();
                    lookupDTO.DropdownName = dropdownName;

                    if (dropdownName.ToUpper().ToString() == "DEPARTMENT")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");
                        lookupDTO.Items.Add(lookupDataObject);

                        DepartmentList departmentList = new DepartmentList(executionContext);
                        List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DepartmentDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<DepartmentDTO> departmentDTOList = departmentList.GetDepartmentDTOList(searchParameters);
                        if (departmentDTOList != null && departmentDTOList.Any())
                        {
                            departmentDTOList = departmentDTOList.OrderBy(x => x.DepartmentName).ToList();
                            foreach (DepartmentDTO departmentDTO in departmentDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(departmentDTO.DepartmentId), departmentDTO.DepartmentName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ROLE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");
                        lookupDTO.Items.Add(lookupDataObject);

                        List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                        searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        UserRolesList userRolesList = new UserRolesList(executionContext);

                        List<UserRolesDTO> UserRolesDTOList = userRolesList.GetAllUserRoles(searchParameters);
                        if (UserRolesDTOList != null && UserRolesDTOList.Any())
                        {
                            foreach (UserRolesDTO userRolesDTO in UserRolesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(userRolesDTO.RoleId), userRolesDTO.Role);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FREQUENCY")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");
                        lookupDTO.Items.Add(lookupDataObject);
                        Dictionary<string, String> keyValuePairs = new Dictionary<string, string>
                        {
                            { "Monthly", "Monthly" },
                            { "Quarterly","Quarterly"},
                            { "Annually","Annually"}
                        };

                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var datasourcetype in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(datasourcetype.Key, datasourcetype.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LEAVE_TYPES")
                    {
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, dropdownName));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ATTENDANCETYPE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");
                        lookupDTO.Items.Add(lookupDataObject);
                        Dictionary<string, String> keyValuePairs = new Dictionary<string, string>
                        {
                            { "IN", "IN" },
                            { "OUT","OUT"},
                            { "BOTH","BOTH"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var datasourcetype in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(datasourcetype.Key, datasourcetype.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TYPE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");
                        lookupDTO.Items.Add(lookupDataObject);

                        Dictionary<string, String> keyValuePairs = new Dictionary<string, string>
                        {
                            { "CREDIT", "CREDIT" },
                            { "DEBIT","DEBIT"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var datasourcetype in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(datasourcetype.Key, datasourcetype.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "STARTHALF")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");
                        lookupDTO.Items.Add(lookupDataObject);

                        Dictionary<string, String> keyValuePairs = new Dictionary<string, string>
                        {
                            { "FIRST_HALF", "FIRST_HALF" },
                            { "SECOND_HALF","SECOND_HALF"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var datasourcetype in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(datasourcetype.Key, datasourcetype.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ENDHALF")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");
                        lookupDTO.Items.Add(lookupDataObject);

                        Dictionary<string, String> keyValuePairs = new Dictionary<string, string>
                        {
                            { "FIRST_HALF", "FIRST_HALF" },
                            { "SECOND_HALF","SECOND_HALF"}
                        };

                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var datasourcetype in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(datasourcetype.Key, datasourcetype.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "YEAR")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "- ALL -");
                        lookupDTO.Items.Add(lookupDataObject);

                        HolidayListBL holidayListBL = new HolidayListBL(executionContext);
                        List<int> getYearList = holidayListBL.GetYearList();

                        if (getYearList != null && getYearList.Any())
                        {
                            foreach (int year in getYearList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(year), Convert.ToString(year));
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "HOUR")
                    {
                        int hour;
                        for (int i = 0; i < 12; i++)
                        {
                            hour = i;
                            lookupDataObject = new CommonLookupDTO(hour.ToString("00"), hour.ToString("00"));
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MINUTE")
                    {
                        int minute;
                        for (int i = 0; i < 60; i++)
                        {
                            minute = i;
                            lookupDataObject = new CommonLookupDTO(minute.ToString("00"), minute.ToString("00"));
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "AMPM")
                    {
                        Dictionary<string, String> keyValuePairs = new Dictionary<string, string>
                        {
                            { "AM", "AM" },
                            { "PM","PM"}
                        };

                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var datasourcetype in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(datasourcetype.Key, datasourcetype.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MANAGER")
                    {
                       // lookupDataObject = new CommonLookupDTO("-1", "- ALL -");
                       // lookupDTO.Items.Add(lookupDataObject);

                        UsersList usersList = new UsersList(executionContext);
                        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> SearchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        SearchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        SearchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                        List<UsersDTO> usersDtoList = usersList.GetAllUsers(SearchParameters);
                        if (usersDtoList != null && usersDtoList.Any())
                        {
                            foreach (UsersDTO usersDTO in usersDtoList)
                            {
                                CommonLookupDTO lookupDataObject;
                                if (usersDTO.IsActive == true)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.UserName);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.UserName + "( Inactive )");
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PAYTYPE")
                    {
                        Dictionary<int, String> payTypeIdDictionary = new Dictionary<int, string>
                            {
                                { -1,"-- None --" },
                                { 1,"Hourly" },
                                { 2,"Weekly" },
                                { 3,"Monthly" }
                            };
                        if (payTypeIdDictionary.Count != 0)
                        {
                            foreach (var payType in payTypeIdDictionary)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(payType.Key), payType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    lookups.Add(lookupDTO);
                }
                return lookups;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Loads default value to the lookupDto
        /// </summary>
        /// <param name="defaultValue">default value of Type string</param>
        private void LoadDefaultValue(string defaultValue)
        {
            List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
            selectKey.Add(new KeyValuePair<string, string>("-1", defaultValue));
            foreach (var select in selectKey)
            {
                CommonLookupDTO lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                lookupDTO.Items.Add(lookupDataObject);
            }
        }
    }
}
