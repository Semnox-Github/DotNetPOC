/********************************************************************************************
 * Project Name - Membership
 * Description  - MembershipEngine program
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.12.0     09-Oct-2020    Guru S A          Membership engine sql session issue
 *1.20.1      28-5-2021      Roshan Devadiga     Modified
 **********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Membership
{
    public class MembershipEngine
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private ConcurrentRequestsDTO concurrentRequestsDTO;
        private List<ConcurrentRequestDetailsDTO> concurrentRequestDetailsDTOList;
        private static int BATCHSIZE = 100;
        private static string PARAFAITOBJECT = "CUSTOMER";
        private static string PROCESSED = "PROCESSED";
        private static string ERRORED = "ERRORED";
        private static string NEWMEMBERSHIP = "NEW MEMBERSHIP";
        private static string EXISTINGMEMBERSHIP = "EXISTING MEMBERSHIP";
        private static string EXPIREMEMBERSHIP = "EXPIRE MEMBERSHIP";
        private int hoursForIncrementalRun = 24;

        public MembershipEngine(ExecutionContext executionContext, ConcurrentRequestsDTO concurrentRequestsDTO)
        {
            log.LogMethodEntry(executionContext, concurrentRequestsDTO);
            this.executionContext = executionContext;
            this.concurrentRequestsDTO = concurrentRequestsDTO;
            this.concurrentRequestDetailsDTOList = new List<ConcurrentRequestDetailsDTO>();
            SetHoursForIncrementalRun();
            log.LogMethodExit();
        }


        public bool CanRunMembershipEngine(DateTime lastRunTime)
        {
            log.LogMethodEntry();
            bool returnValue = true;
            ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(executionContext);
            List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> programSearch = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
            programSearch.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.EXECUTABLE_NAME, "DailyCardBalanceEngine.exe"));
            programSearch.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, executionContext.GetSiteId().ToString()));

            List<ConcurrentProgramsDTO> concurrentProgramsListDTO = concurrentProgramList.GetAllConcurrentPrograms(programSearch);
            List<Tuple<DateTime, string, string>> runList = new List<Tuple<DateTime, string, string>>();
            if (concurrentProgramsListDTO != null && concurrentProgramsListDTO.Any())
            {
                foreach (ConcurrentProgramsDTO concurrentProgramsDTO in concurrentProgramsListDTO)
                {
                    ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList();
                    List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID, concurrentProgramsDTO.ProgramId.ToString()));
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<ConcurrentRequestsDTO> concurrentRequestsListDTO = concurrentRequestList.GetAllConcurrentRequests(searchParameters);
                    if (concurrentRequestsListDTO != null && concurrentRequestsListDTO.Count > 0)
                    {
                        concurrentRequestsListDTO = concurrentRequestsListDTO.OrderByDescending(t => t.ActualStartTime).ToList();
                        if (!String.IsNullOrEmpty(concurrentRequestsListDTO[0].ActualStartTime) && Convert.ToDateTime(concurrentRequestsListDTO[0].ActualStartTime) > lastRunTime)
                        {
                            runList.Add(new Tuple<DateTime, string, string>(Convert.ToDateTime(concurrentRequestsListDTO[0].ActualStartTime), concurrentRequestsListDTO[0].Status, concurrentRequestsListDTO[0].Phase));
                        }
                    }
                }
            }
            if (runList != null && runList.Count > 0)
            {
                runList = runList.OrderByDescending(t => t.Item1).ToList();
                if (runList[0].Item1 > lastRunTime && (runList[0].Item2 == "Error" || runList[0].Item3 == "Running"))
                    returnValue = false;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public void ProcessMembership()
        {
            log.LogMethodEntry();
            try
            {
                //DateTime currentRunTime = ServerDateTime.Now; 
                int businessDayEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_END_TIME", 2);
                MembershipEngineDataHandler membershipEngineDataHandler = new MembershipEngineDataHandler(null);
                DateTime? lastRunTimeValue = membershipEngineDataHandler.GettMembershipEngineRunTime(executionContext.GetUserId(), executionContext.GetSiteId());
                log.LogVariableState("lastRunTimeValue", lastRunTimeValue);
                DateTime lastRunTime = ((lastRunTimeValue == null) ? ServerDateTime.Now.Date.AddHours(businessDayEndHour).AddDays(-1)
                                                                   : Convert.ToDateTime(lastRunTimeValue));
                if (!CanRunMembershipEngine(lastRunTime))
                {
                    log.Error(MessageContainerList.GetMessage(executionContext, 3030));
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3030));
                    //return;
                }
                log.LogVariableState("lastRunTime", lastRunTime);
                List<MembershipDTO> baseMembershipList = MembershipMasterList.GetBaseMembershipList(executionContext);
                DateTime cutOffForCurrentDate = GetCutOffForCurrentDate(businessDayEndHour);
                DateTime previousRunDate = lastRunTime;
                DateTime runTillDate = lastRunTime.Date.AddHours(lastRunTime.Hour).AddHours(hoursForIncrementalRun);
                log.LogVariableState("runTillDate", runTillDate);
                log.LogVariableState("previousRunDate", previousRunDate);
                if ((runTillDate <= cutOffForCurrentDate) == false)
                {
                    string msg = "Job is already run till " + cutOffForCurrentDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    AddToConcurrentRequestDetailsAndSave(msg, PROCESSED);
                }
                while (runTillDate <= cutOffForCurrentDate)
                {
                    log.LogVariableState("runTillDate", runTillDate);
                    try
                    {
                        //Refresh customers                
                        ProcessExistingMembers(previousRunDate, runTillDate);
                        //New customers 
                        ProcessNewMembers(baseMembershipList, previousRunDate, runTillDate);
                        //Expired Memberships
                        ProcessExpiredMembers(previousRunDate, runTillDate);
                        //set run time
                        try
                        {
                            membershipEngineDataHandler.SetMembershipEngineRunTime(runTillDate, executionContext.GetUserId(), executionContext.GetSiteId());
                        }
                        catch (Exception ex) { log.Error("Error while setting run time", ex); throw; }
                    }
                    finally
                    {
                        SaveConcurrentRequestDetails();
                    }
                    previousRunDate = runTillDate;
                    runTillDate = runTillDate.AddHours(hoursForIncrementalRun);
                }
                //currentRunTime = Convert.ToDateTime( "4/14/2021 6:53:00 AM"); 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                try
                {
                    string msg = ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : string.Empty);
                    AddToConcurrentRequestDetailsAndSave(msg, ERRORED);
                    SaveConcurrentRequestDetails();
                }
                catch (Exception exx)
                {
                    log.Error("Error while updating request details", exx);
                }
                throw;
            }
            log.LogMethodExit();
        }

        private DateTime GetCutOffForCurrentDate(int businessDayEndHour)
        {
            log.LogMethodEntry();
            DateTime currentTime = ServerDateTime.Now;
            DateTime cutOffForCurrentDate = ServerDateTime.Now.Date.AddHours(businessDayEndHour);
            while (cutOffForCurrentDate.AddHours(hoursForIncrementalRun) < currentTime)
            {
                cutOffForCurrentDate = cutOffForCurrentDate.AddHours(hoursForIncrementalRun);
            }
            log.LogMethodExit(cutOffForCurrentDate);
            return cutOffForCurrentDate;
        }

        private List<CustomerDTO> GetBatchData(List<int> newCustomerIdList, int processedRecords)
        {
            log.LogMethodEntry("newCustomerIdList", processedRecords);
            List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
            if (newCustomerIdList != null && newCustomerIdList.Any())
            {
                List<int> batchIdList = newCustomerIdList.GetRange(processedRecords, (newCustomerIdList.Count >= processedRecords + BATCHSIZE)
                                                                                     ? BATCHSIZE
                                                                                     : newCustomerIdList.Count - processedRecords);
                if (batchIdList != null && batchIdList.Any())
                {
                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
                    customerDTOList = customerListBL.GetCustomerDTOList(batchIdList, true);
                    LoadAccountDetails(customerDTOList, batchIdList);
                    LoadCustomerMembershipRewardLogs(customerDTOList, batchIdList);
                    LoadCustomerMembershipProgressRecords(customerDTOList, batchIdList);
                }
            }
            log.Debug("Batch fetch Count: " + (customerDTOList != null && customerDTOList.Any() ? customerDTOList.Count : -1));
            log.LogMethodExit("customerDTOList");
            return customerDTOList;
        }

        private void LoadAccountDetails(List<CustomerDTO> customerDTOList, List<int> batchIdList)
        {
            log.LogMethodEntry();
            if (customerDTOList != null && customerDTOList.Any())
            {
                AccountListBL accountListBL = new AccountListBL(executionContext);
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOListByCustomerIds(batchIdList, true, true);
                if (accountDTOList != null && accountDTOList.Any())
                {
                    for (int i = 0; i < customerDTOList.Count; i++)
                    {
                        customerDTOList[i].AccountDTOList = accountDTOList.Where(acc => acc.CustomerId == customerDTOList[i].Id).ToList();
                    }
                }
            }
            log.LogMethodExit();
        }

        private void LoadCustomerMembershipRewardLogs(List<CustomerDTO> customerDTOList, List<int> batchIdList)
        {
            log.LogMethodEntry();
            if (customerDTOList != null && customerDTOList.Any())
            {
                CustomerMembershipRewardsLogList custRewardListBL = new CustomerMembershipRewardsLogList(executionContext);
                List<CustomerMembershipRewardsLogDTO> rewardDTOList = custRewardListBL.GetCustomerMembershipRewardsLogsByCustomerIds(batchIdList);
                if (rewardDTOList != null && rewardDTOList.Any())
                {
                    for (int i = 0; i < customerDTOList.Count; i++)
                    {
                        customerDTOList[i].CustomerMembershipRewardsLogDTOList = rewardDTOList.Where(acc => acc.CustomerId == customerDTOList[i].Id).ToList();
                    }
                }
            }
            log.LogMethodExit();
        }

        private void LoadCustomerMembershipProgressRecords(List<CustomerDTO> customerDTOList, List<int> batchIdList)
        {
            log.LogMethodEntry();
            if (customerDTOList != null && customerDTOList.Any())
            {
                CustomerMembershipProgressionList custMembershipProgressionListBL = new CustomerMembershipProgressionList(executionContext);
                List<CustomerMembershipProgressionDTO> progressionDTOList = custMembershipProgressionListBL.GetCustomerMembershipProgressionByCustomerIds(batchIdList);
                if (progressionDTOList != null && progressionDTOList.Any())
                {
                    for (int i = 0; i < customerDTOList.Count; i++)
                    {
                        customerDTOList[i].CustomerMembershipProgressionDTOList = progressionDTOList.Where(acc => acc.CustomerId == customerDTOList[i].Id).ToList();
                    }
                }
            }
            log.LogMethodExit();
        }


        private void ProcessNewMembers(List<MembershipDTO> baseMembershipList, DateTime previousRunDate, DateTime tillDateTime)
        {
            log.LogMethodEntry(baseMembershipList, previousRunDate, tillDateTime);
            if (baseMembershipList != null && baseMembershipList.Any())
            {
                foreach (MembershipDTO membershipDTO in baseMembershipList)
                {
                    ProcessNewMemberForTheMembership(membershipDTO, previousRunDate, tillDateTime);
                }
            }
            log.LogMethodExit();
        }
        private void ProcessNewMemberForTheMembership(MembershipDTO membershipDTO, DateTime previousRunDate, DateTime tillDateTime)
        {
            log.LogMethodEntry(membershipDTO, previousRunDate, tillDateTime);
            List<int> newCustomerIdList = new List<int>();
            try
            {
                List<DateTime?> dateRange = MembershipMasterList.GetMembershipQualificationRange(executionContext, membershipDTO.MembershipID, tillDateTime);
                if (dateRange != null)
                {
                    CustomerListBL customerList = new CustomerListBL(executionContext);
                    newCustomerIdList = customerList.GetEligibleNewCustomerIdList(dateRange[0], dateRange[1],
                                                       membershipDTO.MembershipRuleDTORecord.QualifyingPoints, previousRunDate, tillDateTime);
                    if (newCustomerIdList != null && newCustomerIdList.Any())
                    {
                        int totalRecords = newCustomerIdList.Count;
                        int processedRecords = 0; 
                        log.Debug("totalRecords: " + totalRecords);
                        while (processedRecords < totalRecords)
                        {
                            List<CustomerDTO> newCustomerDTOList = GetBatchData(newCustomerIdList, processedRecords);
                            if (newCustomerDTOList != null && newCustomerDTOList.Any())
                            {
                                log.Debug("FetchedRecords: " + newCustomerDTOList.Count());
                                processedRecords = processedRecords + newCustomerDTOList.Count();
                                for (int i = 0; i < newCustomerDTOList.Count; i++)
                                {
                                    CustomerDTO newCustomerDTO = newCustomerDTOList[i];
                                    try
                                    {
                                        log.Info("Start new membership assignment: " + newCustomerDTO.Id.ToString());
                                        CustomerBL newCustomer = new CustomerBL(executionContext, newCustomerDTO);
                                        newCustomer.SetMembership(tillDateTime, membershipDTO.MembershipID);
                                        log.Info("End new membership assignment: " + newCustomerDTO.Id.ToString());
                                        string membershipNameUpgraded = GetMembershipName(newCustomerDTO.MembershipId);
                                        string newCustMsg = (newCustomerDTO.MembershipId == -1 ? "New customer is not upgraded"
                                                                                     : "New customer is upgraded to membership: " + membershipNameUpgraded);
                                        AddToConcurrentRequestDetails(newCustomerDTO, null, NEWMEMBERSHIP + ": " + newCustMsg);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("New memberhip assignment failed for " + newCustomerDTO.Id.ToString(), ex);
                                        log.LogVariableState("New memberhip assignment failed for " + newCustomerDTO.Id.ToString(), newCustomerDTO);
                                        AddToConcurrentRequestDetails(newCustomerDTO, ex, NEWMEMBERSHIP);
                                        //throw;
                                    }
                                }
                            }
                            else
                            {
                                log.Debug("FetchedRecords: Zero");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4519, "Customer", "batch"));
                                //Unexpected error, Unable to fetch &1 data for &2
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception(membershipDTO.MembershipName + " Membership rules setup is not proper");
                }
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error in ProcessNewMemberForTheMembership", ex);
                AddToConcurrentRequestDetails(null, ex, NEWMEMBERSHIP);
                throw;
            }
            log.LogMethodExit();
        }

        private void ProcessExistingMembers(DateTime previousRunDate, DateTime tillDateTime)
        {
            log.LogMethodEntry(previousRunDate, tillDateTime);
            try
            {
                //get list of eligbile members  
                CustomerListBL customerList = new CustomerListBL(executionContext);
                List<int> existingCustomerIdList = customerList.GetEligibleExistingCustomerIdList(previousRunDate, tillDateTime);
                if (existingCustomerIdList != null && existingCustomerIdList.Any())
                {
                    int totalRecords = existingCustomerIdList.Count;
                    log.Debug("totalRecords: " + totalRecords);
                    int processedRecords = 0;
                    while (totalRecords > processedRecords)
                    {
                        List<CustomerDTO> existingCustomerDTOList = GetBatchData(existingCustomerIdList, processedRecords);
                        if (existingCustomerDTOList != null && existingCustomerDTOList.Any())
                        {
                            log.Debug("FetchedRecords: " + existingCustomerDTOList.Count());
                            processedRecords = processedRecords + existingCustomerDTOList.Count();
                            for (int i = 0; i < existingCustomerDTOList.Count; i++)
                            {
                                CustomerDTO existingCustomerDTO = existingCustomerDTOList[i];
                                try
                                {
                                    log.Info("Start customer refresh: " + existingCustomerDTO.Id.ToString());
                                    int currentMembershipId = existingCustomerDTO.MembershipId;
                                    CustomerBL existingCustomer = new CustomerBL(executionContext, existingCustomerDTO);
                                    existingCustomer.RefreshMembership(tillDateTime);
                                    log.Info("End customer refresh: " + existingCustomerDTO.Id.ToString());
                                    string membershipNameCurrent = GetMembershipName(currentMembershipId);
                                    string membershipNameUpgraded = GetMembershipName(existingCustomerDTO.MembershipId);
                                    string existingCustMsg = (existingCustomerDTO.MembershipId != currentMembershipId
                                                        ? ": Customer is upgraded from " + membershipNameCurrent + " to membership: " + membershipNameUpgraded
                                                        : ": Refresh membership is done for membership: " + membershipNameCurrent);
                                    AddToConcurrentRequestDetails(existingCustomerDTO, null, EXISTINGMEMBERSHIP + existingCustMsg);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Refresh memberhip failed for " + existingCustomerDTO.Id.ToString(), ex);
                                    log.LogVariableState("Refresh memberhip failed for " + existingCustomerDTO.Id.ToString(), existingCustomerDTO);
                                    AddToConcurrentRequestDetails(existingCustomerDTO, ex, EXISTINGMEMBERSHIP);
                                    //throw;
                                }
                            }
                        }
                        else
                        {
                            log.Debug("FetchedRecords: Zero");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4519, "Customer", "batch"));
                            //Unexpected error, Unable to fetch &1 data for &2
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error in ProcessExistingMembers", ex);
                AddToConcurrentRequestDetails(null, ex, EXISTINGMEMBERSHIP);
            }
            log.LogMethodExit();
        }


        private void ProcessExpiredMembers(DateTime previousRunDate, DateTime tillDateTime)
        {
            log.LogMethodEntry(previousRunDate, tillDateTime);
            try
            {
                List<int> expiredCustomerIdList = new List<int>();
                CustomerListBL customerList = new CustomerListBL(executionContext);
                expiredCustomerIdList = customerList.GetExpiredMembershipCustomerIdList(tillDateTime);
                if (expiredCustomerIdList != null && expiredCustomerIdList.Any())
                {
                    int totalRecords = expiredCustomerIdList.Count;
                    int processedRecords = 0;
                    log.Debug("totalRecords: " + totalRecords);
                    while (processedRecords < totalRecords)
                    {
                        List<CustomerDTO> expiredMembershipCustomerDTOList = GetBatchData(expiredCustomerIdList, processedRecords);
                        if (expiredMembershipCustomerDTOList != null && expiredMembershipCustomerDTOList.Any())
                        {
                            log.Debug("FetchedRecords: " + expiredMembershipCustomerDTOList.Count());
                            processedRecords = processedRecords + expiredMembershipCustomerDTOList.Count();
                            for (int i = 0; i < expiredMembershipCustomerDTOList.Count; i++)
                            {
                                CustomerDTO expiredMembershipCustomerDTO = expiredMembershipCustomerDTOList[i];
                                try
                                {
                                    log.Info("Start customer membership expiry: " + expiredMembershipCustomerDTO.Id.ToString());
                                    int currentMembershipId = expiredMembershipCustomerDTO.MembershipId;
                                    CustomerBL expiredMembershipCustomer = new CustomerBL(executionContext, expiredMembershipCustomerDTO);
                                    expiredMembershipCustomer.CheckForMembershipRetention(tillDateTime);
                                    log.Info("End customer membership expiry: " + expiredMembershipCustomerDTO.Id.ToString());
                                    string membershipNameCurrent = GetMembershipName(currentMembershipId);
                                    string membershipNameRetained = GetMembershipName(expiredMembershipCustomerDTO.MembershipId);
                                    string expiredCustMsg = (expiredMembershipCustomerDTO.MembershipId == currentMembershipId
                                                        ? "Customer is retained at membership: " + membershipNameRetained
                                                        : (expiredMembershipCustomerDTO.MembershipId == -1
                                                              ? "Customer is released from membership: " + membershipNameCurrent
                                                              : "Customer is downgraded from " + membershipNameCurrent + " to membership: " + membershipNameRetained));
                                    AddToConcurrentRequestDetails(expiredMembershipCustomerDTO, null, EXPIREMEMBERSHIP + ": " + expiredCustMsg);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Retention memberhip failed for " + expiredMembershipCustomerDTO.Id.ToString(), ex);
                                    log.LogVariableState("Retention memberhip failed for " + expiredMembershipCustomerDTO.Id.ToString(), expiredMembershipCustomerDTO);
                                    AddToConcurrentRequestDetails(expiredMembershipCustomerDTO, ex, EXPIREMEMBERSHIP);
                                    //throw;
                                }
                            }
                        }
                        else
                        {
                            log.Debug("FetchedRecords: Zero");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4519, "Customer", "batch"));
                            //Unexpected error, Unable to fetch &1 data for &2
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error in ProcessExpiredMembers", ex);
                AddToConcurrentRequestDetails(null, ex, EXPIREMEMBERSHIP);
            }
            log.LogMethodExit();
        }

        private void AddToConcurrentRequestDetails(CustomerDTO customerDTO, Exception ex, string baseMsg)
        {
            log.LogMethodEntry((customerDTO != null ? customerDTO.Id : -1), ex, baseMsg);
            string errorData = (ex != null ? ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : "")
                                           : string.Empty);
            ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO = new ConcurrentRequestDetailsDTO(-1, this.concurrentRequestsDTO.RequestId, ServerDateTime.Now,
                concurrentRequestsDTO.ProgramId, PARAFAITOBJECT, (customerDTO != null ? customerDTO.Id : -1),
                (customerDTO != null ? customerDTO.Guid : string.Empty), (ex == null ? true : false),
                (ex == null ? PROCESSED : ERRORED), string.Empty, errorData, baseMsg, true);

            this.concurrentRequestDetailsDTOList.Add(concurrentRequestDetailsDTO);
            log.LogMethodExit();
        }
        private void SaveConcurrentRequestDetails()
        {
            log.LogMethodEntry();
            if (this.concurrentRequestDetailsDTOList != null && concurrentRequestDetailsDTOList.Any())
            {
                try
                {
                    ConcurrentRequestDetailsListBL concurrentRequestDetailsListBL = new ConcurrentRequestDetailsListBL(executionContext, concurrentRequestDetailsDTOList);
                    concurrentRequestDetailsListBL.Save();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.Error(concurrentRequestDetailsDTOList);
                }
                this.concurrentRequestDetailsDTOList = new List<ConcurrentRequestDetailsDTO>();
            }
            log.LogMethodExit();
        }
        private void SetHoursForIncrementalRun()
        {
            log.LogMethodEntry();
            hoursForIncrementalRun = 24;
            StringBuilder errorMsg = new StringBuilder();
            StringBuilder successMsg = new StringBuilder();
            string hoursForIncrementalRunString = ConfigurationManager.AppSettings["INCREMENTAL_RUN_BY_X_HOURS"];
            if (string.IsNullOrWhiteSpace(hoursForIncrementalRunString) == false)
            {
                int tempValue = -1;
                if (int.TryParse(hoursForIncrementalRunString, out tempValue) == false)
                {
                    string msg1 = "Please set valid value for INCREMENTAL_RUN_BY_X_HOURS in App settings. Going with default value 24. ";
                    errorMsg.Append(msg1);
                    log.Error(msg1);
                }
                else
                {
                    if (tempValue < 1)
                    {
                        string msg1 = "INCREMENTAL_RUN_BY_X_HOURS value cannot be less than 1. Going with default value 24. ";
                        errorMsg.Append(msg1);
                        log.Error(msg1);
                    }
                    else if (tempValue > 24)
                    {
                        string msg1 = "INCREMENTAL_RUN_BY_X_HOURS value cannot be more than 24. Going with default value 24. ";
                        errorMsg.Append(msg1);
                        log.Error(msg1);
                    }
                    else
                    {
                        hoursForIncrementalRun = tempValue;
                        string msg1 = "INCREMENTAL_RUN_BY_X_HOURS is set as " + hoursForIncrementalRun;
                        //successMsg.Append(msg1);
                        log.Info(msg1);
                        //AddToConcurrentRequestDetailsAndSave(successMsg.ToString(), PROCESSED);
                    }
                }
                if (errorMsg != null && errorMsg.Length > 1)
                {
                    AddToConcurrentRequestDetailsAndSave(errorMsg.ToString(), ERRORED);
                }
            }
            log.LogMethodExit();
        }

        private void AddToConcurrentRequestDetailsAndSave(string msg, string status)
        {
            log.LogMethodEntry(msg, status);
            try
            {
                ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO = new ConcurrentRequestDetailsDTO(-1, this.concurrentRequestsDTO.RequestId,
                                      ServerDateTime.Now, concurrentRequestsDTO.ProgramId, string.Empty, -1, string.Empty, true, status,
                                      string.Empty, string.Empty, msg, true);
                List<ConcurrentRequestDetailsDTO> newDTOList = new List<ConcurrentRequestDetailsDTO>();
                try
                {
                    newDTOList.Add(concurrentRequestDetailsDTO);
                    ConcurrentRequestDetailsListBL concurrentRequestDetailsListBL = new ConcurrentRequestDetailsListBL(executionContext, newDTOList);
                    concurrentRequestDetailsListBL.Save();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.Error(newDTOList);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private string GetMembershipName(int membershipId)
        {
            log.LogMethodEntry(membershipId);
            MembershipDTO membershipDTO = MembershipMasterList.GetMembershipDTO(executionContext, membershipId);
            string membershipName = (membershipDTO != null ? membershipDTO.MembershipName : string.Empty);
            log.LogMethodExit(membershipName);
            return membershipName;
        }
    }
}
