/********************************************************************************************
 * Project Name - Reports
 * Description  - Bussiness logic of lookups for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        03-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.POS;
using Semnox.Parafait.Reports;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.CommonAPI.Lookups
{
    public class ReportsLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        string dependentDropdownName = string.Empty;
        string dependentDropdownSelectedId = string.Empty;
        string isActive = string.Empty;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private List<LookupValuesDTO> lookupValuesDTOList;
        private CommonLookupDTO lookupDataObject;
        private CommonLookupsDTO lookupDTO;

        /// <summary>
        /// Constructor for the method PromotionsLookupBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public ReportsLookupBL(string entityName, ExecutionContext executioncontext)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }

        public enum ReportsEntityLookup
        {
            POSSHIFTS,
            VIEWTRANSACTION,
            GAMEPERFORMANCE,
            GAMESERVERVIEW
        }

        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                string dropdownNames = string.Empty;
                string[] dropdowns = null;
                ReportsEntityLookup reportsEntityLookup = (ReportsEntityLookup)Enum.Parse(typeof(ReportsEntityLookup), entityName.ToUpper().ToString());
                switch (reportsEntityLookup)
                {
                    case ReportsEntityLookup.POSSHIFTS:
                        dropdownNames = "POSMACHINE,POSUSER";
                        break;
                    case ReportsEntityLookup.VIEWTRANSACTION:
                        dropdownNames = "USER,POS,STATUS,PAYMENTMODE";
                        break;
                    case ReportsEntityLookup.GAMEPERFORMANCE:
                        dropdownNames = "YEAR,PERIOD";
                        break;
                    case ReportsEntityLookup.GAMESERVERVIEW:
                        dropdownNames = "READERFILTER";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                foreach (string dropdownName in dropdowns)
                {
                    lookupDTO = new CommonLookupsDTO
                    {
                        Items = new List<CommonLookupDTO>(),
                        DropdownName = dropdownName
                    };
                    if (dropdownName.ToUpper().ToString() == "POSMACHINE")
                    {
                        LoadDefaultValue("ALL");
                        
                        List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchByShiftParameters = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                        searchByShiftParameters.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ShiftDTO> shiftDTOList = new ShiftListBL(executionContext).GetShiftDTOList(searchByShiftParameters, false, false, null);
                        if (shiftDTOList != null && shiftDTOList.Any())
                        {
                            List<string> posNameList = new List<string>();
                            posNameList = shiftDTOList.Select(x => x.POSMachine).Distinct().ToList();
                            foreach (string posMachine in posNameList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(posMachine), posMachine);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }

                    }
                    else if (dropdownName.ToUpper().ToString() == "POSUSER")
                    {
                        LoadDefaultValue("ALL");

                        List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchByShiftParameters = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                        searchByShiftParameters.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ShiftDTO> shiftDTOList = new ShiftListBL(executionContext).GetShiftDTOList(searchByShiftParameters, false, false, null);
                        if (shiftDTOList != null && shiftDTOList.Any())
                        {
                            List<string> shiftUserNameList = new List<string>();
                            shiftUserNameList = shiftDTOList.Select(x => x.ShiftUserName).Distinct().ToList();
                            foreach (string shiftUserName in shiftUserNameList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(shiftUserName), shiftUserName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "USER")
                    {
                        LoadDefaultValue("-ALL-");
                        
                        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByUserParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        searchByUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<UsersDTO> userDTOList = new UsersList(executionContext).GetAllUsers(searchByUserParameters,false, null);
                        if (userDTOList != null && userDTOList.Any())
                        {
                            foreach (UsersDTO usersDTO in userDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.UserName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }

                    }
                    else if (dropdownName.ToUpper().ToString() == "POS")
                    {
                        LoadDefaultValue("-ALL-");
                        
                        List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchByPOSMachineParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                        searchByPOSMachineParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<POSMachineDTO> posMachineDTOList = new POSMachineList(executionContext).GetAllPOSMachines(searchByPOSMachineParameters, false, false, null);
                        if (posMachineDTOList != null && posMachineDTOList.Any())
                        {
                            foreach (POSMachineDTO pOSMachineDTO in posMachineDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(pOSMachineDTO.POSMachineId), pOSMachineDTO.POSName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }

                    }
                    else if (dropdownName.ToUpper().ToString() == "PAYMENTMODE")
                    {
                        LoadDefaultValue("-ALL-");

                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchByPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchByPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchByPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "N"));
                        List<PaymentModeDTO> paymentModeDTOList = new PaymentModeList(executionContext).GetPaymentModeList(searchByPaymentModeParameters);
                        if (paymentModeDTOList != null && paymentModeDTOList.Any())
                        {
                            foreach (PaymentModeDTO paymentMode in paymentModeDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(paymentMode.PaymentModeId), paymentMode.PaymentMode);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }

                    }
                    else if (dropdownName.ToUpper().ToString() == "STATUS")
                    {
                        LoadDefaultValue("-ALL-");

                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "CLOSED", "CLOSED" },
                            { "OPEN", "OPEN" },
                            { "CANCELLED", "CANCELLED" },
                            { "PENDING", "PENDING" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var status in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(status.Key, status.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "YEAR")
                    {
                        string year;
                        for (int i = 2011; i < DateTime.Now.Year + 5; i++)
                        {
                            if (i == 2011)
                                year = "ALL";
                            else
                                year = i.ToString();
                            lookupDataObject = new CommonLookupDTO(year, year);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PERIOD")
                    {
                        LoadDefaultValue("ALL");
                        
                        List<KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>> searchByUserPeriodParameters = new List<KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>>();
                        searchByUserPeriodParameters.Add(new KeyValuePair<UserPeriodDTO.SearchByUserPeriodSearchParameters, string>(UserPeriodDTO.SearchByUserPeriodSearchParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<UserPeriodDTO> userPeriodDTOList = new UserPeriodListBL(executionContext).GetUserPeriodDTOList(searchByUserPeriodParameters, null);
                        if (userPeriodDTOList != null && userPeriodDTOList.Any())
                        {
                            foreach (UserPeriodDTO userPeriodDTO in userPeriodDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(userPeriodDTO.PeriodId), userPeriodDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "READERFILTER")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "Any", "Any Field" },
                            { "AccessPoint", "Access Point" },
                            { "MachineName", "Machine Name" },
                            { "ID", "ID" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var status in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(status.Key, status.Value);
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

        /// <summary>
        /// Loads the lookupValues
        /// </summary>
        /// <param name="lookupName"></param>
        private List<LookupValuesDTO> LoadLookupValues(string lookupName)
        {
            log.LogMethodEntry(lookupName);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
        }
    }
}