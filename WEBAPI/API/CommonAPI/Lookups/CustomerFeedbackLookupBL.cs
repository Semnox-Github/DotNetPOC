/********************************************************************************************
 * Project Name - CustomerFeedback Lookup BL
 * Description  - Business class of the CustomerFeedbackLookupBL class
 * 
 **************
 **Version Log
 **************
 *Version       Date           Modified By          Remarks          
 *********************************************************************************************
 * 2.70       21-Oct-2019      Rakesh Kumar         Created.
 * 2.80.0     23-Mar-2020      Mushahid Faizan      Modified all lookups.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;

namespace Semnox.CommonAPI.Lookups
{
    class CustomerFeedbackLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private string keyValuePair;
        private ExecutionContext executionContext;
        string isActive = string.Empty;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private CommonLookupDTO lookupDataObject;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executionContext"></param>
        public CustomerFeedbackLookupBL(string entityName, ExecutionContext executionContext)
        {
            log.LogMethodEntry(entityName, executionContext);
            this.entityName = entityName;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public enum CustomerFeedbackEntityNameLookup
        {
            RESPONSES,
            RESPONSEVALUES,
            QUESTIONS,
            SURVEY,
            SURVEYSETUP,
        }


        /// <summary>
        /// Gets the All lookups for all dropdowns based on the page in the Cards module.
        /// </summary>
        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                string[] dropdowns = null;
                string dropdownNames = string.Empty;

                CustomerFeedbackEntityNameLookup customerFeedbackEntityNameLookup = (CustomerFeedbackEntityNameLookup)Enum.Parse(typeof(CustomerFeedbackEntityNameLookup), entityName);
                switch (customerFeedbackEntityNameLookup)
                {
                    case CustomerFeedbackEntityNameLookup.RESPONSES:
                        dropdownNames = "TYPEID";
                        break;
                    case CustomerFeedbackEntityNameLookup.RESPONSEVALUES:
                        dropdownNames = "RESPONSE,LANGUAGE";
                        break;
                    case CustomerFeedbackEntityNameLookup.QUESTIONS:
                        dropdownNames = "RESPONSE,LANGUAGE";
                        break;
                    case CustomerFeedbackEntityNameLookup.SURVEY:
                        dropdownNames = "SURVEY,POSMACHINE";
                        break;
                    case CustomerFeedbackEntityNameLookup.SURVEYSETUP:
                        dropdownNames = "SURVEY,CRITERIA,QUESTION,RESPONSE";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                string siteId = Convert.ToString(executionContext.GetSiteId());
                foreach (string dropdownName in dropdowns)
                {
                    CommonLookupsDTO lookupDTO = new CommonLookupsDTO();
                    lookupDTO.Items = new List<CommonLookupDTO>();
                    lookupDTO.DropdownName = dropdownName;
                    if (dropdownName.ToUpper().ToString() == "TYPEID")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_RESPONSE_TYPE"));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> lookupValuesDTOList = new LookupValuesList(executionContext).GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    if (dropdownName.ToUpper().ToString() == "LANGUAGE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        Languages languages = new Languages(executionContext);
                        List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> serachLanguageParam = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                        serachLanguageParam.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LanguagesDTO> languagesDTOList = languages.GetAllLanguagesList(serachLanguageParam);
                        if (languagesDTOList != null && languagesDTOList.Any())
                        {
                            foreach (LanguagesDTO languagesDTO in languagesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(languagesDTO.LanguageId), languagesDTO.LanguageName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    if (dropdownName.ToUpper().ToString() == "RESPONSE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();
                        searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE, "1"));
                        List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList = new CustomerFeedbackResponseList(executionContext).GetAllCustomerFeedbackResponse(searchByCustomerFeedbackResponseParameters);
                        if (customerFeedbackResponseDTOList != null && customerFeedbackResponseDTOList.Any())
                        {
                            foreach (CustomerFeedbackResponseDTO customerFeedbackResponseDTO in customerFeedbackResponseDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(customerFeedbackResponseDTO.CustFbResponseId), customerFeedbackResponseDTO.ResponseName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    if (dropdownName.ToUpper().ToString() == "SURVEY")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> custFeedbackSurveySearchParams = new List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>>();
                        custFeedbackSurveySearchParams.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        custFeedbackSurveySearchParams.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.IS_ACTIVE, "1"));
                        List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList = new CustomerFeedbackSurveyList(executionContext).GetAllCustomerFeedbackSurvey(custFeedbackSurveySearchParams);
                        if (customerFeedbackSurveyDTOList != null && customerFeedbackSurveyDTOList.Any())
                        {
                            foreach (CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO in customerFeedbackSurveyDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(customerFeedbackSurveyDTO.CustFbSurveyId), customerFeedbackSurveyDTO.SurveyName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    if (dropdownName.ToUpper().ToString() == "CRITERIA")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_FEEDBACK_CRITERIA"));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> lookupValuesDTOList = new LookupValuesList(executionContext).GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    if (dropdownName.ToUpper().ToString() == "QUESTION")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchByCustomerFeedbackQuestionsParameters = new List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>>();
                        searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE, "1"));
                        List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList = new CustomerFeedbackQuestionsList(executionContext).GetAllCustomerFeedbackQuestions(searchByCustomerFeedbackQuestionsParameters, executionContext.GetLanguageId());
                        if (customerFeedbackQuestionsDTOList != null && customerFeedbackQuestionsDTOList.Any())
                        {
                            foreach (CustomerFeedbackQuestionsDTO customerFeedbackQuestionsDTO in customerFeedbackQuestionsDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(customerFeedbackQuestionsDTO.CustFbQuestionId), customerFeedbackQuestionsDTO.Question);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    if (dropdownName.ToUpper().ToString() == "POSMACHINE")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchByPOSMachineParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                        searchByPOSMachineParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<POSMachineDTO> posMachineDTOList = new POSMachineList(executionContext).GetAllPOSMachines(searchByPOSMachineParameters);
                        if (posMachineDTOList != null && posMachineDTOList.Any())
                        {
                            foreach (POSMachineDTO pOSMachineDTO in posMachineDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(pOSMachineDTO.POSMachineId), pOSMachineDTO.POSName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    lookups.Add(lookupDTO);
                }
                log.LogMethodExit(lookups);
                return lookups;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}
