/********************************************************************************************
 * Project Name - EmailTemplateBL                                                                    
 * Description  - BL for the EmailTemplate tables.
 *
 **************
 **Version Log
  *Version       Date             Modified  By                 Remarks          
 *********************************************************************************************
 *1.00         14-April-2016      Rakshith                     Created 
 *2.60.0       07-Feb-2019    Flavia Jyothi Dsouza             Modified
               21-Mar-2019       Jagan Mohana Rao              Added EmailTemplateListBL(executionContext, emailTemplateDTOList) Constructor, modified SaveUpdateEmailTemplateList()
               08-Apr-2019       Mushahid Faizan               Added SqlTransaction in SaveUpdateEmailTemplateList(), removed unused namespaces. 
 *2.70.0       17-Jul-2019       Mushahid Faizan               Added DeleteEmailTemplate() method for Hard - deletion.
 *2.70.2         16-June-2019      Girish Kundar                 Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *             29-Jul-2019       Mushahid Faizan               Added Delete in Save() method for Hard - deletion.
 *2.90.0       15-Jul-2020       Girish Kundar                 Modified : Phase -2 changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Communication
{
    public class EmailTemplate
    {
        private EmailTemplateDTO emailTemplateDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor for EmailTemplate
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public EmailTemplate(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the default_value_id as the parameter
        /// Would fetch the ParafaitDefaults object from the database based on the id passed. 
        /// </summary>
        public EmailTemplate(ExecutionContext executionContext, int id,SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            EmailTemplateDatahandler emailTemplateDatahandler = new EmailTemplateDatahandler(sqlTransaction);
            emailTemplateDTO = emailTemplateDatahandler.GetEmailTemplateDTO(id);
            log.LogMethodExit();
        }

        public EmailTemplate(ExecutionContext executionContext, string name, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, name, sqlTransaction);
            EmailTemplateDatahandler emailTemplateDatahandler = new EmailTemplateDatahandler(sqlTransaction);
            List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameter.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, name));
            List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateDatahandler.GetEmailTemplateDTOList(searchParameter);
            if (emailTemplateDTOList != null && emailTemplateDTOList.Count > 0)
            {
                emailTemplateDTO = emailTemplateDTOList[0];
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates EmailTemplateBL object using the EmailTemplateValuesDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="emailTemplateDTO">emailTemplateDTO</param>
        public EmailTemplate(ExecutionContext executionContext, EmailTemplateDTO emailTemplateDTO)
         : this(executionContext)
        {
            log.LogMethodEntry(executionContext, emailTemplateDTO);
            this.emailTemplateDTO = emailTemplateDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public EmailTemplateDTO EmailTemplateDTO
        {
            get
            {
                return emailTemplateDTO;
            }
        }


        /// <summary>
        /// Method Saves the  EmailTemplateDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (emailTemplateDTO.IsChanged == false
                    && emailTemplateDTO.EmailTemplateId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            EmailTemplateDatahandler emailTemplateDatahandler = new EmailTemplateDatahandler(sqlTransaction);
            if (emailTemplateDTO.EmailTemplateId < 0)
            {
                emailTemplateDTO = emailTemplateDatahandler.InsertEmailTemplate(emailTemplateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                emailTemplateDTO.AcceptChanges();
            }
            else
            {
                if (emailTemplateDTO.IsChanged)
                {
                    emailTemplateDTO = emailTemplateDatahandler.UpdateEmailTemplate(emailTemplateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    emailTemplateDTO.AcceptChanges();
                }
            }

            log.LogMethodExit();
        }


        /// <summary>
        ///  GetEmailTemplate(string templateName, int siteId) method
        /// </summary>
        /// <param name="templateName">string templateName</param>
        /// <param name="siteId">int siteId</param>
        /// <returns>returns EmailTemplateDTO object</returns>
        public EmailTemplateDTO GetEmailTemplate(string templateName, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(templateName, siteId);
            EmailTemplateDatahandler emailDatahandler = new EmailTemplateDatahandler(sqlTransaction);
            List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            searchParameter.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, templateName));
            List<EmailTemplateDTO> emailTemplateDTOList = emailDatahandler.GetEmailTemplateDTOList(searchParameter);
            EmailTemplateDTO emailTemplateDTO = null;
            if (emailTemplateDTOList != null && emailTemplateDTOList.Count > 0)
            {
                emailTemplateDTO = emailTemplateDTOList[0];
            }
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }

        /// <summary>
        /// Delete the EmailTemplate records from database based on emailTemplateId
        /// </summary>
        /// <param name="emailTemplateId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>int </returns>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                EmailTemplateDatahandler emailDatahandler = new EmailTemplateDatahandler(sqlTransaction);
                emailDatahandler.DeleteEmailTemplate(emailTemplateDTO.EmailTemplateId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }

    /// <summary>
    /// Manages the list of Email Template Values 
    /// </summary>
    public class EmailTemplateListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<EmailTemplateDTO> emailTemplateDTOList;
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public EmailTemplateListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            emailTemplateDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="emailTemplateDTOList">EmailTemplateDTO</param>
        public EmailTemplateListBL(ExecutionContext executionContext, List<EmailTemplateDTO> emailTemplateDTOList)
        {
            log.LogMethodEntry(executionContext, emailTemplateDTOList);
            this.executionContext = executionContext;
            this.emailTemplateDTOList = emailTemplateDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the emailTemplate list
        /// </summary>
        public List<EmailTemplateDTO> GetEmailTemplateDTOList(List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            EmailTemplateDatahandler emailTemplateDatahandler = new EmailTemplateDatahandler(sqlTransaction);
            List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateDatahandler.GetEmailTemplateDTOList(searchParameters);
            log.LogMethodExit(emailTemplateDTOList);
            return emailTemplateDTOList;
        }

        /// <summary>
        /// This method should be used to Save and Update the Email Template details for Web Management Studio.
        /// </summary>
        public void SaveUpdateEmailTemplateList()
        {
            log.LogMethodEntry();
            if (emailTemplateDTOList != null && emailTemplateDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (EmailTemplateDTO emailTemplateDTO in emailTemplateDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            EmailTemplate emailTemplateBL = new EmailTemplate(executionContext, emailTemplateDTO);
                            emailTemplateBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// This method should be used to Save and Update the Email Template details for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (emailTemplateDTOList != null && emailTemplateDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (EmailTemplateDTO emailTemplateDTO in emailTemplateDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            EmailTemplate emailTemplateBL = new EmailTemplate(executionContext, emailTemplateDTO);
                            if (emailTemplateDTO.EmailTemplateId > -1)
                            {
                                emailTemplateBL.Delete(parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                log.LogMethodExit();
            }
        }
    }
}
