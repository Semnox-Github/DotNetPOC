/********************************************************************************************
 * Project Name - Customer Admission Question UI
 * Description  - A high level structure created to classify the Customer Admission Question UI 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Dec-2016   Raghuveera          Created 
 ********************************************************************************************/
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// User interface for admission question
    /// </summary>
    public partial class CustomerFeedbackQuestionnairUI : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        string objectName;
        int objectId = -1;
        int surveyId = -1;
        int critiriaId = -1;
        int questionCount = 0;
        List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsAnswerDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
        CustomerFeedbackResponseValuesDTO customerFeedbackResponseValuesAnswerDTO;
        /// <summary>
        /// 
        /// </summary>
        public List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataAnswerDTOList = new List<CustomerFeedbackSurveyDataDTO>();
        /// <summary>
        /// 
        /// </summary>
        public CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDTO;
        /// <summary>
        /// 
        /// </summary>
        public CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDTO;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        LookupValuesList lookupValuesList = new LookupValuesList(ExecutionContext.GetExecutionContext());
        List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
        CustomerFeedbackSurveyDetailsDTO customerFbSurveyDetailsDTO = new CustomerFeedbackSurveyDetailsDTO();
        /// <summary>
        /// 
        /// </summary>
        public CustomerFeedbackQuestionnairUI(Utilities _Utilities, string objectName, int objectId, string customerName, string custPhoneNumber, string criteria)
        {
            log.Debug("Starts-CustomerFeedbackQuestionUI(Utilities,objectName,objectId,customerName,custPhoneNumber,criteria) parameterized constructor.");
            InitializeComponent();
            utilities = _Utilities;
            utilities.setLanguage(this);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            this.objectName = objectName;
            this.objectId = objectId;
            lblName.Text = customerName;

            lblPhoneNumber.Text = custPhoneNumber;
            LoadCritiria(criteria);
            if (criteria.Equals("Transaction"))
            {
                label3.Visible = lblVisitCount.Visible = label4.Visible = lblLastVisitDate.Visible = false;
            }
            if (File.Exists(Application.StartupPath + "\\Resources\\ClientLogo.jpg")) { pbLogo.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Resources\\ClientLogo.jpg"); pbLogo.Size = pbLogo.BackgroundImage.Size; }
            log.Debug("Ends-CustomerFeedbackQuestionUI(Utilities,objectName,objectId,customerName,custPhoneNumber,criteria) parameterized constructor.");
        }
        private void LoadCritiria(string criteria)
        {
            log.Debug("Starts-LoadStatus() method.");
            try
            {
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_FEEDBACK_CRITERIA"));
                //if (objectName.Equals("CUSTOMER"))
                //{
                //    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Visit Count"));
                //}
                //else if (objectName.Equals("TRX_HEADER"))
                //{
                //    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Transaction"));
                //}
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, criteria));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                {
                    critiriaId = lookupValuesDTOList[0].LookupValueId;
                }
                log.Debug("Ends-LoadStatus() Method");
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadStatus() Method with an Exception:", e);
            }
        }
        /// <summary>
        /// setting surveyId
        /// </summary>
        /// <param name="surveyId"> integer value of survey id.</param>
        public void SetSurveyId(int surveyId)
        {
            this.surveyId = surveyId;
        }

        private void LoadQuestion()
        {
            log.Debug("Starts-LoadQuestion() method.");
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList();
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
            CustomerFeedbackSurveyMappingList customerFeedbackSurveyMappingList = new CustomerFeedbackSurveyMappingList(machineUserContext);
            List<CustomerFeedbackSurveyMappingDTO> customerFeedbackSurveyMappingDTOList = new List<CustomerFeedbackSurveyMappingDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>> searchByCustomerFeedbackSurveyMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>>();
            CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails(machineUserContext);

            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_ID, objectId.ToString()));

            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_NAME, objectName.ToString()));
            customerFeedbackSurveyMappingDTOList = customerFeedbackSurveyMappingList.GetAllCustomerFeedbackSurveyMapping(searchByCustomerFeedbackSurveyMappingParameters);
            if (customerFeedbackSurveyMappingDTOList != null && customerFeedbackSurveyMappingDTOList.Any())// second or next visit
            {
                foreach (CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDTO in customerFeedbackSurveyMappingDTOList)
                {
                    lblVisitCount.Text = customerFeedbackSurveyMappingDTO.VisitCount.ToString();
                    lblLastVisitDate.Text = customerFeedbackSurveyMappingDTO.LastVisitDate.ToString(utilities.getDateTimeFormat());
                    lblVisitCount.Tag = customerFeedbackSurveyMappingDTO;
                    //int questionId 
                    customerFeedbackSurveyDetailsAnswerDTOList = customerFeedbackSurveyDetails.GetNextQuestionId(surveyId, customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId, customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId, customerFeedbackSurveyMappingDTO.VisitCount);
                    if (customerFeedbackSurveyDetailsAnswerDTOList == null)
                    {
                        //MessageBox.Show(utilities.MessageUtils.getMessage("Failed to obtain question id."));
                        this.Close();
                        return;
                    }
                    if (customerFeedbackSurveyDetailsAnswerDTOList.Count > 0)
                    {
                        if (customerFeedbackSurveyDetailsAnswerDTOList[0].NextQuestionId == -1 && customerFeedbackSurveyDetailsAnswerDTOList[0].IsRecur && customerFeedbackSurveyMappingDTO.VisitCount >= Convert.ToInt32(customerFeedbackSurveyDetailsAnswerDTOList[0].CriteriaValue))
                        {
                            LoadQuestionToUI(customerFeedbackSurveyDetailsAnswerDTOList[0].CustFbQuestionId);
                        }
                        else if (customerFeedbackSurveyDetailsAnswerDTOList[0].NextQuestionId != -1)
                        {
                            LoadQuestionToUI(customerFeedbackSurveyDetailsAnswerDTOList[0].NextQuestionId);
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    questionCount = 1;
                    if (customerFeedbackSurveyDetailsAnswerDTOList.Count > 1)
                    {
                        btnSubmit.Text = utilities.MessageUtils.getMessage("Next");
                        btnSubmit.Tag = "N";
                    }
                    else
                    {
                        btnSubmit.Text = utilities.MessageUtils.getMessage("Submit");
                        btnSubmit.Tag = "S";
                    }
                }
            }
            else//For the first visit
            {
                lblVisitCount.Text = "0";
                lblLastVisitDate.Text = "";
                searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
                //searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE, "1"));
                customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetCustomerFeedbackSurveyDetailsOfInitialLoadList(searchByCustomerFeedbackSurveyDetailsParameters);
                if (customerFeedbackSurveyDetailsDTOList != null)
                {
                    foreach (CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO in customerFeedbackSurveyDetailsDTOList)
                    {
                        if (customerFeedbackSurveyDetailsDTO.CriteriaId == critiriaId)//Question for the visitcount critiria if exists
                        {
                            LoadQuestionToUI(customerFeedbackSurveyDetailsDTO.CustFbQuestionId);
                            customerFeedbackSurveyDetailsAnswerDTOList.Add(customerFeedbackSurveyDetailsDTO);
                            questionCount = 1;
                        }
                    }
                }
            }
            if (customerFeedbackSurveyDetailsAnswerDTOList == null || customerFeedbackSurveyDetailsAnswerDTOList.Count == 0)
            {
                this.Close();
            }
            log.Debug("Ends-LoadQuestion() method.");
        }

        private void LoadQuestionToUI(int QuestionId)
        {
            log.Debug("Starts-LoadQuestion() method.");
            CustomerFeedbackQuestionsList customerFeedbackQuestionsList = new CustomerFeedbackQuestionsList(machineUserContext);
            List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList = new List<CustomerFeedbackQuestionsDTO>();
            List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchByCustomerFeedbackQuestionsParameters = new List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>>();
            CustomerFeedbackResponseList customerFeedbackResponseList = new CustomerFeedbackResponseList(machineUserContext);
            List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList = new List<CustomerFeedbackResponseDTO>();
            List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();

            CustomerFeedbackResponseValuesList customerFeedbackResponseValuesList = new CustomerFeedbackResponseValuesList(machineUserContext);
            List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList = new List<CustomerFeedbackResponseValuesDTO>();
            List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>> searchByCustomerFeedbackResponseValuesParameters = new List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>>();
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList();
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
            //CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDTO;
            Button btn;
            FLquestion.Controls.Clear();
            FLquestion.Controls.Add(lblQuestion);
            searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_QUESTION_ID, QuestionId.ToString()));
            customerFeedbackQuestionsDTOList = customerFeedbackQuestionsList.GetAllCustomerFeedbackQuestions(searchByCustomerFeedbackQuestionsParameters, utilities.ParafaitEnv.LanguageId);
            if (customerFeedbackQuestionsDTOList != null)
            {
                foreach (CustomerFeedbackQuestionsDTO customerFeedbackQuestionsDTO in customerFeedbackQuestionsDTOList)
                {
                    lblQuestion.Text = customerFeedbackQuestionsDTO.Question;//customerFeedbackQuestionsDTO.QuestionNo + ". " +
                    lblQuestion.Tag = customerFeedbackQuestionsDTO;
                    searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();
                    searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE, "1"));
                    searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID, customerFeedbackQuestionsDTO.CustFbResponseId.ToString()));
                    customerFeedbackResponseDTOList = customerFeedbackResponseList.GetAllCustomerFeedbackResponse(searchByCustomerFeedbackResponseParameters);
                    if (customerFeedbackResponseDTOList != null && customerFeedbackResponseDTOList.Count > 0)
                    {
                        foreach (CustomerFeedbackResponseDTO customerFeedbackResponseDTO in customerFeedbackResponseDTOList)
                        {
                            lookupValuesDTOList = new List<LookupValuesDTO>();
                            lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_RESPONSE_TYPE"));
                            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, customerFeedbackResponseDTO.ResponseTypeId.ToString()));
                            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                            lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                            if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                            {
                                switch (lookupValuesDTOList[0].LookupValue)
                                {
                                    case "MultiChoice":
                                        searchByCustomerFeedbackResponseValuesParameters = new List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>>();
                                        searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.IS_ACTIVE, "1"));
                                        searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                        searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID, customerFeedbackResponseDTO.CustFbResponseId.ToString()));
                                        customerFeedbackResponseValuesDTOList = customerFeedbackResponseValuesList.GetAllCustomerFeedbackResponseValues(searchByCustomerFeedbackResponseValuesParameters, utilities.ParafaitEnv.LanguageId);
                                        if (customerFeedbackResponseValuesDTOList != null && customerFeedbackResponseValuesDTOList.Count > 0)
                                        {
                                            foreach (CustomerFeedbackResponseValuesDTO customerFeedbackResponseValuesDTO in customerFeedbackResponseValuesDTOList)
                                            {
                                                btn = new Button();
                                                btn.Size = btnSample.Size;
                                                btn.Font = btnSample.Font;
                                                btn.Margin = btnSample.Margin;
                                                btn.Padding = btnSample.Padding;
                                                if (customerFeedbackResponseValuesDTO.Image == null || (customerFeedbackResponseValuesDTO.Image != null && customerFeedbackResponseValuesDTO.Image.Length == 0))
                                                {
                                                    btn.BackgroundImage = btnSample.BackgroundImage;
                                                    btn.Text = customerFeedbackResponseValuesDTO.ResponseValue;
                                                }
                                                else
                                                {
                                                    var ms = new MemoryStream(customerFeedbackResponseValuesDTO.Image);
                                                    btn.BackgroundImage = Image.FromStream(ms);
                                                    btn.Image = btnSample.BackgroundImage;
                                                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                                                }
                                                btn.Tag = customerFeedbackResponseValuesDTO;
                                                btn.FlatStyle = FlatStyle.Flat;
                                                btn.Click += new System.EventHandler(this.btnSample_Click);
                                                FLquestion.Controls.Add(btn);
                                            }
                                        }

                                        break;
                                    case "Text":
                                        TextBox txt = new TextBox();
                                        txt.Name = "Response";
                                        txt.Tag = customerFeedbackResponseDTO;
                                        txt.Size = new System.Drawing.Size(200, txt.Height);
                                        FLquestion.Controls.Add(txt);
                                        break;
                                }
                            }
                        }
                    }
                    break;
                }
            }

            log.Debug("Ends-LoadQuestion() method.");
        }



        private void btnSample_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSample_Click() method.");
            Button btn = (Button)sender;
            Button btn1;
            if (btn.Tag != null)
            {
                customerFeedbackResponseValuesAnswerDTO = (CustomerFeedbackResponseValuesDTO)btn.Tag;
                if (customerFeedbackResponseValuesAnswerDTO.Image != null && customerFeedbackResponseValuesAnswerDTO.Image.Length > 0)
                {
                    foreach (Control c in FLquestion.Controls)
                    {
                        if (c.GetType().ToString().ToLower().Contains("button"))
                        {
                            btn1 = (Button)c;
                            btn1.Image = Properties.Resources.Unchecked;
                        }
                    }
                    btn.Image = Properties.Resources.Checked;
                }
                else
                {
                    foreach (Control c in FLquestion.Controls)
                    {
                        if (c.GetType().ToString().ToLower().Contains("button"))
                        {
                            c.BackgroundImage = Properties.Resources.Unchecked;
                        }
                    }
                    btn.BackgroundImage = Properties.Resources.Checked;
                }
            }
            log.Debug("Ends-btnSample_Click() method.");
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSubmit_Click() method.");
            int trnsactionCriteriaId = -1;
            SqlTransaction SQLTrx = null;
            SqlConnection TrxCnn = null;
            CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails(machineUserContext);

            CustomerFeedbackSurveyDataDTO customerFeedbackSurveyDataDTO = new CustomerFeedbackSurveyDataDTO();
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList();
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();

            CustomerFeedbackSurveyDataSet customerFeedbackSurveyDataSet;
            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
            //CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails();
            CustomerFeedbackSurveyData customerFeedbackSurveyData;
            CustomerFeedbackSurveyMapping customerFeedbackSurveyMapping;

            CustomerFeedbackResponseList customerFeedbackResponseList = new CustomerFeedbackResponseList(machineUserContext);
            List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList = new List<CustomerFeedbackResponseDTO>();
            List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();

            lookupValuesDTOList = new List<LookupValuesDTO>();
            lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_FEEDBACK_CRITERIA"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Transaction"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
            {
                trnsactionCriteriaId = lookupValuesDTOList[0].LookupValueId;
            }

            customerFeedbackSurveyMappingDTO = (CustomerFeedbackSurveyMappingDTO)lblVisitCount.Tag;

            CustomerFeedbackQuestionsDTO customerFeedbackQuestionsDTO;

            if (btnSubmit.Tag.ToString().Equals("N"))
            {
                if ((customerFeedbackResponseValuesAnswerDTO == null) || (customerFeedbackResponseValuesAnswerDTO != null && customerFeedbackResponseValuesAnswerDTO.CustFbResponseValueId == -1))
                {
                    if (customerFeedbackSurveyDetailsAnswerDTOList[questionCount - 1].IsResponseMandatory)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("Response mandatory."));
                        customerFeedbackSurveyDataAnswerDTOList.RemoveAt(customerFeedbackSurveyDataAnswerDTOList.Count - 1);
                        return;
                    }
                }
                if (customerFeedbackSurveyDetailsAnswerDTOList.Count - 1 == questionCount)
                {
                    btnSubmit.Tag = "S";
                    btnSubmit.Text = utilities.MessageUtils.getMessage("Submit");
                }
                customerFeedbackSurveyDataDTO.CustFbResponseDate = DateTime.Now;
                customerFeedbackSurveyDataDTO.CustFbSurveyDetailId = customerFeedbackSurveyDetailsAnswerDTOList[questionCount - 1].CustFbSurveyDetailId;
                if (customerFeedbackResponseValuesAnswerDTO != null && customerFeedbackResponseValuesAnswerDTO.CustFbResponseValueId != -1)
                {
                    customerFeedbackSurveyDataDTO.CustFbResponseValueId = customerFeedbackResponseValuesAnswerDTO.CustFbResponseValueId;
                    customerFeedbackSurveyDataAnswerDTOList.Add(customerFeedbackSurveyDataDTO);
                }
                LoadQuestionToUI(customerFeedbackSurveyDetailsAnswerDTOList[questionCount].NextQuestionId);
                questionCount++;
            }
            else
            {
                if (trnsactionCriteriaId == -1 || (trnsactionCriteriaId != customerFeedbackSurveyDetailsAnswerDTOList[0].CriteriaId))
                {
                    if (SQLTrx == null)
                    {
                        TrxCnn = utilities.createConnection();
                        SQLTrx = TrxCnn.BeginTransaction();//IsolationLevel.ReadUncommitted
                    }
                }
                try
                {
                    customerFeedbackQuestionsDTO = (CustomerFeedbackQuestionsDTO)lblQuestion.Tag;
                    if (customerFeedbackQuestionsDTO != null && customerFeedbackQuestionsDTO.CustFbQuestionId > -1)
                    {
                        searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();
                        searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE, "1"));
                        searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                        searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID, customerFeedbackQuestionsDTO.CustFbResponseId.ToString()));
                        customerFeedbackResponseDTOList = customerFeedbackResponseList.GetAllCustomerFeedbackResponse(searchByCustomerFeedbackResponseParameters);
                        if (customerFeedbackResponseDTOList != null && customerFeedbackResponseDTOList.Count > 0)
                        {
                            if ((customerFeedbackResponseValuesAnswerDTO == null) || (customerFeedbackResponseValuesAnswerDTO != null && customerFeedbackResponseValuesAnswerDTO.CustFbResponseValueId == -1))
                            {
                                if (customerFeedbackSurveyDetailsAnswerDTOList[0].IsResponseMandatory)
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage("Response mandatory."));
                                    //customerFeedbackSurveyDataAnswerDTOList.RemoveAt(customerFeedbackSurveyDataAnswerDTOList.Count - 1);
                                    return;
                                }
                            }
                            foreach (CustomerFeedbackResponseDTO customerFeedbackResponseDTO in customerFeedbackResponseDTOList)
                            {
                                lookupValuesDTOList = new List<LookupValuesDTO>();
                                lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_RESPONSE_TYPE"));
                                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, customerFeedbackResponseDTO.ResponseTypeId.ToString()));
                                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                                if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                                {
                                    switch (lookupValuesDTOList[0].LookupValue)
                                    {
                                        case "MultiChoice":
                                            if (customerFeedbackQuestionsDTO.CustFbResponseId != -1)
                                            {
                                                customerFeedbackSurveyDataDTO.CustFbResponseDate = DateTime.Now;
                                                if (customerFeedbackResponseValuesAnswerDTO != null && customerFeedbackResponseValuesAnswerDTO.CustFbResponseValueId != -1)
                                                {
                                                    customerFeedbackSurveyDataDTO.CustFbResponseValueId = customerFeedbackResponseValuesAnswerDTO.CustFbResponseValueId;
                                                    customerFeedbackSurveyDataAnswerDTOList.Add(customerFeedbackSurveyDataDTO);
                                                }
                                            }
                                            break;
                                        case "Text":
                                            customerFeedbackSurveyDataDTO.CustFbResponseDate = DateTime.Now;
                                            foreach (Control cntrl in FLquestion.Controls)
                                            {
                                                if (cntrl.GetType().ToString().ToLower().Contains("textbox"))
                                                {
                                                    customerFeedbackSurveyDataDTO.CustFbResponseText = cntrl.Text;
                                                }
                                            }
                                            customerFeedbackSurveyDataAnswerDTOList.Add(customerFeedbackSurveyDataDTO);
                                            break;
                                    }

                                    if (customerFeedbackSurveyDataDTO != null && (customerFeedbackSurveyDataDTO.CustFbResponseValueId != -1 || !string.IsNullOrEmpty(customerFeedbackSurveyDataDTO.CustFbResponseText)))
                                    {
                                        customerFeedbackSurveyMappingDTO = (CustomerFeedbackSurveyMappingDTO)lblVisitCount.Tag;
                                        if (customerFeedbackSurveyMappingDTO == null)
                                        {
                                            customerFeedbackSurveyDataSetDTO = new CustomerFeedbackSurveyDataSetDTO();
                                            if (trnsactionCriteriaId == -1 || (trnsactionCriteriaId != customerFeedbackSurveyDetailsAnswerDTOList[0].CriteriaId))
                                            {
                                                customerFeedbackSurveyDataSet = new CustomerFeedbackSurveyDataSet(machineUserContext, customerFeedbackSurveyDataSetDTO);
                                                customerFeedbackSurveyDataSet.Save(SQLTrx);
                                                if (customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId == -1)
                                                {
                                                    MessageBox.Show(utilities.MessageUtils.getMessage("Not Saved"));
                                                    return;
                                                }
                                            }

                                            customerFeedbackSurveyMappingDTO = new CustomerFeedbackSurveyMappingDTO();
                                            customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId = customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId;
                                            customerFeedbackSurveyMappingDTO.ObjectId = objectId;
                                            customerFeedbackSurveyMappingDTO.ObjectName = objectName;
                                            customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId = customerFeedbackSurveyDetailsAnswerDTOList[0].CustFbSurveyDetailId;
                                            if ((customerFeedbackResponseValuesAnswerDTO == null) || (customerFeedbackResponseValuesAnswerDTO != null && customerFeedbackResponseValuesAnswerDTO.CustFbResponseValueId == -1))
                                            {
                                                if (customerFeedbackSurveyDetailsAnswerDTOList[0].IsResponseMandatory)
                                                {
                                                    MessageBox.Show(utilities.MessageUtils.getMessage("Response mandatory."));
                                                    customerFeedbackSurveyDataAnswerDTOList.RemoveAt(customerFeedbackSurveyDataAnswerDTOList.Count - 1);
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID, critiriaId.ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE, (customerFeedbackSurveyMappingDTO.VisitCount + 1).ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_QUESTION_ID, customerFeedbackSurveyDetailsAnswerDTOList[customerFeedbackSurveyDetailsAnswerDTOList.Count - 1].NextQuestionId.ToString()));
                                            customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);
                                            if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Count > 0)
                                            {
                                                customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId = customerFeedbackSurveyDetailsDTOList[0].CustFbSurveyDetailId;
                                            }
                                        }
                                        if (customerFeedbackSurveyDataAnswerDTOList != null && customerFeedbackSurveyDataAnswerDTOList.Count > 0)
                                        {
                                            if (customerFeedbackQuestionsDTO.CustFbResponseId != -1 && (customerFeedbackResponseValuesAnswerDTO != null && customerFeedbackResponseValuesAnswerDTO.CustFbResponseValueId != -1))
                                            {
                                                customerFeedbackSurveyDataDTO.CustFbSurveyDetailId = customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId;
                                                foreach (CustomerFeedbackSurveyDataDTO customerFeedbackSurveyDataAnswerDTO in customerFeedbackSurveyDataAnswerDTOList)
                                                {
                                                    customerFeedbackSurveyDataAnswerDTO.CustFbSurveyDataSetId = customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId;
                                                    if (trnsactionCriteriaId == -1 || (trnsactionCriteriaId != customerFeedbackSurveyDetailsAnswerDTOList[0].CriteriaId))
                                                    {
                                                        customerFeedbackSurveyData = new CustomerFeedbackSurveyData(machineUserContext, customerFeedbackSurveyDataAnswerDTO);
                                                        customerFeedbackSurveyData.Save(SQLTrx);
                                                    }
                                                }

                                            }
                                            else // When Text response is created response id is -1 and customerFeedbackResponseValuesAnswerDTO = null , details Id is need to updating
                                            {
                                                customerFeedbackSurveyDataDTO.CustFbSurveyDetailId = customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId;
                                            }
                                        }
                                        //if (customerFeedbackQuestionsDTO.CustFbResponseId != -1)
                                        //{
                                        //    customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId = customerFeedbackSurveyDataAnswerDTOList[customerFeedbackSurveyDataAnswerDTOList.Count - 1].CustFbSurveyDataSetId;
                                        //}
                                        customerFeedbackSurveyMappingDTO.VisitCount++;
                                        customerFeedbackSurveyMappingDTO.LastVisitDate = DateTime.Now;
                                        if (trnsactionCriteriaId == -1 || (trnsactionCriteriaId != customerFeedbackSurveyDetailsAnswerDTOList[0].CriteriaId))
                                        {
                                            customerFeedbackSurveyMapping = new CustomerFeedbackSurveyMapping(machineUserContext, customerFeedbackSurveyMappingDTO);
                                            customerFeedbackSurveyMapping.Save(SQLTrx);
                                            SQLTrx.Commit();
                                        }
                                        else
                                        {
                                            lblVisitCount.Tag = customerFeedbackSurveyMappingDTO;
                                        }
                                        //if (trnsactionCriteriaId != -1 && trnsactionCriteriaId == customerFeedbackSurveyDetailsAnswerDTOList[0].CriteriaId)
                                        //{
                                        //    customerFeedbackSurveyDetailsAnswerDTOList = customerFeedbackSurveyDetails.GetNextQuestionId(surveyId, customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId, customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId, customerFeedbackSurveyMappingDTO.VisitCount);
                                        //    if(customerFeedbackSurveyDetailsAnswerDTOList!=null&&customerFeedbackSurveyDetailsAnswerDTOList.Count>0)
                                        //    {
                                        //        LoadQuestionToUI(customerFeedbackSurveyDetailsAnswerDTOList[0].NextQuestionId);

                                        //    }
                                        //}

                                        this.Close();
                                    }
                                    else if (customerFeedbackSurveyDataDTO == null || (customerFeedbackSurveyDataDTO != null && (customerFeedbackSurveyDataDTO.CustFbResponseValueId == -1)))
                                    {
                                        customerFeedbackSurveyMappingDTO = (CustomerFeedbackSurveyMappingDTO)lblVisitCount.Tag;
                                        if (customerFeedbackSurveyMappingDTO == null)
                                        {
                                            customerFeedbackSurveyMappingDTO = new CustomerFeedbackSurveyMappingDTO();
                                        }
                                        if (customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId == -1)
                                        {
                                            customerFeedbackSurveyDataSetDTO = new CustomerFeedbackSurveyDataSetDTO();
                                            customerFeedbackSurveyDataSet = new CustomerFeedbackSurveyDataSet(machineUserContext, customerFeedbackSurveyDataSetDTO);
                                            customerFeedbackSurveyDataSet.Save(SQLTrx);
                                            customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId = customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId;
                                        }
                                        customerFeedbackSurveyMappingDTO.VisitCount++;
                                        customerFeedbackSurveyMappingDTO.LastVisitDate = DateTime.Now;
                                        customerFeedbackSurveyMappingDTO.ObjectId = objectId;
                                        customerFeedbackSurveyMappingDTO.ObjectName = objectName;
                                        if (customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId == -1)
                                        {
                                            customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId = customerFeedbackSurveyDetailsAnswerDTOList[0].CustFbSurveyDetailId;
                                        }
                                        else
                                        {
                                            searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID, critiriaId.ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE, customerFeedbackSurveyMappingDTO.VisitCount.ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_QUESTION_ID, customerFeedbackSurveyDetailsAnswerDTOList[customerFeedbackSurveyDetailsAnswerDTOList.Count - 1].NextQuestionId.ToString()));
                                            customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);
                                            if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Count > 0)
                                            {
                                                customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId = customerFeedbackSurveyDetailsDTOList[0].CustFbSurveyDetailId;
                                            }
                                        }
                                        customerFeedbackSurveyMapping = new CustomerFeedbackSurveyMapping(machineUserContext, customerFeedbackSurveyMappingDTO);
                                        customerFeedbackSurveyMapping.Save(SQLTrx);
                                        SQLTrx.Commit();
                                    }
                                }
                            }
                        }
                        else if (customerFeedbackSurveyMappingDTO != null && customerFeedbackQuestionsDTO.CustFbResponseId == -1)
                        {
                            customerFeedbackSurveyMappingDTO.VisitCount++;
                            customerFeedbackSurveyMappingDTO.LastVisitDate = DateTime.Now;
                            customerFeedbackSurveyMappingDTO.ObjectId = objectId;
                            customerFeedbackSurveyMappingDTO.ObjectName = objectName;
                            if (customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId == -1)
                            {
                                customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId = customerFeedbackSurveyDetailsAnswerDTOList[0].CustFbSurveyDetailId;
                            }
                            else
                            {
                                searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID, critiriaId.ToString()));
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE, customerFeedbackSurveyMappingDTO.VisitCount.ToString()));
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_QUESTION_ID, customerFeedbackSurveyDetailsAnswerDTOList[customerFeedbackSurveyDetailsAnswerDTOList.Count - 1].NextQuestionId.ToString()));
                                customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);
                                if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Count > 0)
                                {
                                    customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId = customerFeedbackSurveyDetailsDTOList[0].CustFbSurveyDetailId;
                                }
                            }
                            if (trnsactionCriteriaId == -1 || (trnsactionCriteriaId != customerFeedbackSurveyDetailsAnswerDTOList[0].CriteriaId))
                            {
                                customerFeedbackSurveyMapping = new CustomerFeedbackSurveyMapping(machineUserContext, customerFeedbackSurveyMappingDTO);
                                customerFeedbackSurveyMapping.Save(SQLTrx);
                                SQLTrx.Commit();
                            }
                            else
                            {
                                lblVisitCount.Tag = customerFeedbackSurveyMappingDTO;
                            }
                        }
                    }
                    this.Close();
                }
                catch (Exception ex)
                {
                    if (SQLTrx != null)
                        SQLTrx.Rollback();
                    MessageBox.Show(ex.Message, "Customer Feedback");
                }
            }
            log.Debug("Ends-btnSubmit_Click() method.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCancel_Click() method.");
            this.Close();
            log.Debug("Ends-btnCancel_Click() method.");
        }

        private void CustomerFeedbackQuestionUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-CustomerFeedbackQuestionUI_Load() method.");
            LoadQuestion();
            log.Debug("Ends-CustomerFeedbackQuestionUI_Load() method.");
        }
    }
}
