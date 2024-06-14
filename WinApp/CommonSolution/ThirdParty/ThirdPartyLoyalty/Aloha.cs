/********************************************************************************************
* Project Name - Loyalty
* Description  - Aloha - Class ALOHA Loyalty programs
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    /// <summary>
    /// Aloha
    /// </summary>
    public class AlohaBL : LoyaltyPrograms
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities Utilities;
        private string serviceUri;
        private string username;
        private string password;
        private string applicationId;
        string svcCredentials;
        private LoyaltyMemberDetails loyaltyMemberDetails;
        int AlohaTerminalId;
        int AlohaEmpNumber;
        bool continueOnError = false;
        const string COMPANY = "cec01";
        const string CONTENTTYPE = "application/json";

        /// <summary>
        /// Aloha
        /// </summary>
        /// <param name="_utilities"></param>
        public AlohaBL(Utilities _utilities) : base(_utilities)
        {
            log.LogMethodEntry(_utilities);
            Utilities = _utilities;
            LoadLoyaltyConfigs();
            log.LogMethodExit();
        }

        /// <summary>
        /// GetCustomers 
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public override async Task<LoyaltyMemberDetails> GetCustomers(string phoneNumber)
        {
            log.LogMethodEntry(phoneNumber);
            string searchUrl = "?searchby=phone&phone=";
            searchUrl = searchUrl + phoneNumber;
            searchUrl = serviceUri + searchUrl;
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Application-Id", applicationId);
                using (HttpResponseMessage response = await client.GetAsync(searchUrl))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        LoyaltyMember memberDetails = serializer.Deserialize<LoyaltyMember>(responseContent);
                        if (memberDetails != null && memberDetails.values != null)
                        {
                            foreach (String[] val in memberDetails.values)
                            {
                                loyaltyMemberDetails = new LoyaltyMemberDetails(val[0], val[2], val[3], val[4]);
                            }
                        }
                    }
                    else
                    {
                        log.Debug("GetCustomers():We’re sorry, rewards are not available at this time. Please see front counter.");
                        string message = Utilities.MessageUtils.getMessage(1458);// Message:-"We’re sorry, rewards are not available at this time. Please see front counter.");
                        log.LogMethodExit();
                        throw new ValidationException(message);
                    }
                }
            }
            catch
            {
                log.Debug("GetCustomers():We’re sorry, rewards are not available at this time. Please see front counter.");
                string message = Utilities.MessageUtils.getMessage(1458);// Message:-"We’re sorry, rewards are not available at this time. Please see front counter.");
                log.LogMethodExit();
                throw new ValidationException(message);
            }

            log.LogMethodExit(loyaltyMemberDetails);
            return loyaltyMemberDetails;
        }

        /// <summary>
        /// LoadLoyaltyConfigs
        /// </summary>
        public override void LoadLoyaltyConfigs()
        {
            try
            {
                log.LogMethodEntry();
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "ALOHA_LOYALTY_CONFIG"));
                LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                List<LookupValuesDTO> alohaConfigValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (alohaConfigValueList != null)
                {
                    foreach (LookupValuesDTO value in alohaConfigValueList)
                    {
                        if (value.LookupValue == "ALOHA_LOYALTY_INTERFACE_URL")
                            serviceUri = value.Description.ToString();
                        else if (value.LookupValue == "ALOHA_LOYALTY_INTERFACE_USERNAME")
                            username = value.Description.ToString();
                        else if (value.LookupValue == "ALOHA_LOYALTY_INTERFACE_PASSWORD")
                            password = value.Description.ToString();
                        else if (value.LookupValue == "ALOHA_LOYALTY_APPLICATION_ID")
                            applicationId = value.Description.ToString();
                    }
                    svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// PostTransaction
        /// </summary>
        /// <returns></returns>
        public override string PostTransaction()
        {
            log.LogMethodEntry();
            int concurrentRequestId = -1;
            LoyaltyPrograms loyaltyPrograms = new LoyaltyPrograms(utilities);
            AlohaDataHandler alohaDataHandler = new AlohaDataHandler(null);
            LoadLoyaltyConfigs();
            LoyaltyProgramBL loyaltyProgramBL = new LoyaltyProgramBL(utilities.ExecutionContext);
          
            string message = string.Empty;
            DateTime endUpdateTime = Utilities.getServerTime();
            DateTime lastUpdateTime = alohaDataHandler.GetLastUpdatedTime();
            DataTable dtLoyaltyTrxDetails = alohaDataHandler.GetTransactionDetails(utilities.ParafaitEnv.IsCorporate ? utilities.ParafaitEnv.SiteId : -1);
            if (dtLoyaltyTrxDetails != null && dtLoyaltyTrxDetails.Rows.Count == 0)
            {
                log.Debug("No Loyalty Transactions to synchronize with Aloha");
                return "Success";
            }
            else
            {
                concurrentRequestId = loyaltyProgramBL.CreateConcurrentRequest();
                log.Debug(dtLoyaltyTrxDetails.Rows.Count.ToString() + " Loyalty tansactions to synchronize with Aloha");
            }
            foreach (DataRow drLoyaltyTrx in dtLoyaltyTrxDetails.Rows)
            {
                try
                {
                    if (Utilities.ParafaitEnv.POSMachineId != Convert.ToInt32(drLoyaltyTrx["POSMachineId"]))
                        InitPOSDetails(Convert.ToInt32(drLoyaltyTrx["POSMachineId"]));
                    //derive check number based on check id saved in Transaction
                    int checkNumber = (Convert.ToInt32(drLoyaltyTrx["CheckId"]) >> 20) * 10000 + (Convert.ToInt32(drLoyaltyTrx["CheckId"]) & 0xFFFF);
                    AlohaLoyaltyCreateAssignmentResponseDTO createAssignmentResponseDTO = new AlohaLoyaltyCreateAssignmentResponseDTO();
                    AlohaLoyaltyCreateAssignmentUnsuccessfulResponseDTO createAssignmentUnsuccessfulResponseDTO = new AlohaLoyaltyCreateAssignmentUnsuccessfulResponseDTO();
                    AlohaLoyaltyCreateAssignmentRequestDTO assignmentRequestDTO = new AlohaLoyaltyCreateAssignmentRequestDTO(COMPANY, drLoyaltyTrx["LoyaltyCardNumber"].ToString(), checkNumber, Convert.ToInt32(drLoyaltyTrx["SiteCode"]), AlohaTerminalId.ToString(), drLoyaltyTrx["TrxDate"].ToString());
                    ExecuteAction(async () =>
                    {
                        createAssignmentResponseDTO = await CreateLoyaltyAssignment(assignmentRequestDTO, createAssignmentUnsuccessfulResponseDTO);
                        log.LogVariableState("createAssignmentResponseDTO", createAssignmentResponseDTO);
                        if (createAssignmentResponseDTO != null)
                        {
                            bool isCompleteAssignmentSuccess = await CloseLoyaltyAssignment(assignmentRequestDTO, createAssignmentResponseDTO);
                            if (!isCompleteAssignmentSuccess)
                            {
                                alohaDataHandler.UpdateConcurrentRequestDetails(drLoyaltyTrx["TrxId"], drLoyaltyTrx["LoyaltyProgramId"],
                                                                                drLoyaltyTrx["GUID"], false, "FAILED",
                                                                                assignmentRequestDTO.cardNumber + ":" + assignmentRequestDTO.checkID,
                                                                                "Complete Assignment for " + assignmentRequestDTO.cardNumber + " failed",
                                                                                Utilities.ParafaitEnv.Username);
                            }
                            else
                            {
                                alohaDataHandler.UpdateConcurrentRequestDetails(drLoyaltyTrx["TrxId"], drLoyaltyTrx["LoyaltyProgramId"],
                                                                                drLoyaltyTrx["GUID"], true, "SUCCESS",
                                                                                assignmentRequestDTO.cardNumber + ":" + assignmentRequestDTO.checkID,
                                                                                "Complete Assignment for " + assignmentRequestDTO.cardNumber + " successful",
                                                                                Utilities.ParafaitEnv.Username);
                            }
                        }
                        else
                        {
                            alohaDataHandler.UpdateConcurrentRequestDetails(drLoyaltyTrx["TrxId"], drLoyaltyTrx["LoyaltyProgramId"], drLoyaltyTrx["GUID"],
                                                                             false, "FAILED", assignmentRequestDTO.cardNumber + ":" + assignmentRequestDTO.checkID,
                                                                              createAssignmentUnsuccessfulResponseDTO != null ? createAssignmentUnsuccessfulResponseDTO.message : "Create Assignment for " + assignmentRequestDTO.cardNumber + " failed",
                                                                              Utilities.ParafaitEnv.Username);
                        }
                    });
                }
                catch (Exception ex)
                {
                    if (continueOnError) // log and continue
                    {
                        log.Fatal(ex);
                        alohaDataHandler.UpdateConcurrentRequestDetails(drLoyaltyTrx["TrxId"], drLoyaltyTrx["LoyaltyProgramId"],
                                                                          drLoyaltyTrx["GUID"], false, "FAILED",
                                                                          drLoyaltyTrx["LoyaltyCardNumber"], DBNull.Value,
                                                                          utilities.ParafaitEnv.Username);


                    }
                    else
                    {
                        log.Fatal(ex);
                        loyaltyProgramBL.UpdateConcurrentRequests(concurrentRequestId, "Error", endUpdateTime);
                        return "Failed";
                    }
                }
            }
            loyaltyProgramBL.UpdateConcurrentRequests(concurrentRequestId, "Normal", endUpdateTime);
            log.LogMethodExit("Success");
            return "Success";
        }

       

        private void InitPOSDetails(int POSMachineId)
        {
            log.LogMethodEntry();
            Utilities.ParafaitEnv.POSMachineId = POSMachineId;
            try
            {
                AlohaTerminalId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ALOHA_TERM_ID"));
            }
            catch { }
            try
            {
                AlohaEmpNumber = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ALOHA_USER_ID"));
            }
            catch { }
            try
            {
                continueOnError = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "IGNORE_THIRD_PARTY_SYNCH_ERROR").Equals("Y");
            }
            catch { continueOnError = true; }

            log.LogMethodExit();
        }

        private async Task<AlohaLoyaltyCreateAssignmentResponseDTO> CreateLoyaltyAssignment(AlohaLoyaltyCreateAssignmentRequestDTO assignmentRequestDTO, AlohaLoyaltyCreateAssignmentUnsuccessfulResponseDTO createAssignmentUnsuccessfulResponseDTO)
        {
            log.LogMethodEntry();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CONTENTTYPE));
                client.DefaultRequestHeaders.Add("Application-Id", applicationId);
                if (!serviceUri.ToString().EndsWith("/"))
                {
                    serviceUri += "/";
                }
                serviceUri += assignmentRequestDTO.cardNumber + "/assignments";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string assignmentRequestJSONString = serializer.Serialize(assignmentRequestDTO);
                HttpResponseMessage assignmentRequestResponse = await client.PostAsync(serviceUri.ToString(), new StringContent(assignmentRequestJSONString, Encoding.UTF8, CONTENTTYPE));
                string assignmentRequestResponseContent = await assignmentRequestResponse.Content.ReadAsStringAsync();
                AlohaLoyaltyCreateAssignmentResponseDTO createAssignmentResponseDTO;
                if (assignmentRequestResponse.IsSuccessStatusCode)
                {
                    createAssignmentResponseDTO = serializer.Deserialize<AlohaLoyaltyCreateAssignmentResponseDTO>(assignmentRequestResponseContent);
                }
                else
                {
                    createAssignmentResponseDTO = null;
                    AlohaLoyaltyCreateAssignmentUnsuccessfulResponseDTO unsuccessfulResponseDTO = serializer.Deserialize<AlohaLoyaltyCreateAssignmentUnsuccessfulResponseDTO>(assignmentRequestResponseContent);
                    createAssignmentUnsuccessfulResponseDTO.message = unsuccessfulResponseDTO.message;
                    createAssignmentUnsuccessfulResponseDTO.code = unsuccessfulResponseDTO.code;
                    createAssignmentUnsuccessfulResponseDTO.type = unsuccessfulResponseDTO.type;
                    createAssignmentUnsuccessfulResponseDTO.status = unsuccessfulResponseDTO.status;
                    createAssignmentUnsuccessfulResponseDTO.moreinfo = unsuccessfulResponseDTO.moreinfo;
                    createAssignmentUnsuccessfulResponseDTO.referenceId = unsuccessfulResponseDTO.referenceId;
                }
                log.LogMethodExit(createAssignmentResponseDTO);
                return createAssignmentResponseDTO;
            }
        }

        private async Task<bool> CloseLoyaltyAssignment(AlohaLoyaltyCreateAssignmentRequestDTO assignmentRequestDTO, AlohaLoyaltyCreateAssignmentResponseDTO assignmentResponseDTO)
        {
            log.LogMethodEntry();
            AlohaLoyaltyCompleteAssignmentRequestDTO completeAssignmentRequestDTO = new AlohaLoyaltyCompleteAssignmentRequestDTO();
            List<AlohaLoyaltyCreateAssignmentResponseDTO.AlohaLoyaltyReward> assignmentResponseRewards = assignmentResponseDTO.rewards;
            completeAssignmentRequestDTO.cardNumber = assignmentRequestDTO.cardNumber;
            completeAssignmentRequestDTO.companyId = assignmentRequestDTO.companyId;
            completeAssignmentRequestDTO.storeId = assignmentRequestDTO.storeId;
            completeAssignmentRequestDTO.dateOfBusiness = assignmentRequestDTO.dateOfBusiness;
            completeAssignmentRequestDTO.checkID = assignmentRequestDTO.checkID;
            foreach (AlohaLoyaltyCreateAssignmentResponseDTO.AlohaLoyaltyReward currReward in assignmentResponseRewards)
            {
                completeAssignmentRequestDTO.rewardsRejected.Add(new AlohaLoyaltyCompleteAssignmentRequestDTO.AlohaLoyaltyRewardsRejected(currReward.hstRewardProgramID, currReward.iteration, currReward.rewardProgID, currReward.tierID));
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string completeAssignmentJSONString = serializer.Serialize(completeAssignmentRequestDTO);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CONTENTTYPE));
                client.DefaultRequestHeaders.Add("Application-Id", applicationId);
                serviceUri += "/" + completeAssignmentRequestDTO.storeId + "-" + completeAssignmentRequestDTO.dateOfBusiness + "-" + completeAssignmentRequestDTO.checkID;
                HttpResponseMessage completeAssignmentResponse = await client.PostAsync(serviceUri.ToString(), new StringContent(completeAssignmentJSONString, Encoding.UTF8, CONTENTTYPE));
                string compAssignmentResponseContent = await completeAssignmentResponse.Content.ReadAsStringAsync();
                log.LogMethodExit(completeAssignmentResponse.IsSuccessStatusCode);
                if (completeAssignmentResponse.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
        }

    }
}
