/********************************************************************************************
 * Project Name - EmailTemplate Datahandler Programs 
 * Description  - Data object of the EmailTemplateDatahandler
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By                         Remarks          
 *********************************************************************************************
 *1.00        14-April-2016   Rakshith                            Created 
 *2.60.0      07-Feb-2019     Flavia Jyothi dsouza                modified
 *2.70.0      17-Jul-2019     Mushahid Faizan                     Added DeleteEmailTemplate() method for Hard - deletion.
 *2.70.2        19-Jul-2019     Girish Kundar                       Modified : For Insert /Update methods returns DTO instated of Id 
 *                                                                           and SQL Injection Issue. 
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Communication
{
    public class EmailTemplateDatahandler
    {
        /// <summary>
        ///  Data Handler - Selection  of EmailTemplateDatahandler objects
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM EmailTemplate AS et ";
        private static readonly Dictionary<EmailTemplateDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<EmailTemplateDTO.SearchByParameters, string>
            {
                {EmailTemplateDTO.SearchByParameters. EMAIL_TEMPLATE_ID,"et.EmailTemplateId"},
                {EmailTemplateDTO.SearchByParameters. NAME, "et.Name"},
                {EmailTemplateDTO.SearchByParameters.MASTER_ENTITY_ID, "et.MasterEntityId"},
                {EmailTemplateDTO.SearchByParameters.SITE_ID, "et.site_id"}
             };

        /// <summary>
        /// Default constructor of  EmailDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public EmailTemplateDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating EmailTemplate Record.
        /// </summary>
        /// <param name="emailTemplateDTO">EmailTemplateDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(EmailTemplateDTO emailTemplateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(emailTemplateDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@emailTemplateId", emailTemplateDTO.EmailTemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name ", emailTemplateDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", emailTemplateDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@startDate ", (emailTemplateDTO.StartDate == DateTime.MinValue) ? DBNull.Value : (object)emailTemplateDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@endDate ", (emailTemplateDTO.EndDate == DateTime.MinValue) ? DBNull.Value : (object)emailTemplateDTO.EndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive ", emailTemplateDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@emailTemplate ", emailTemplateDTO.EmailTemplate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", emailTemplateDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the  record to the database
        /// </summary>
        /// <param name="emailTemplateDTO">emailTemplateDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> EmailTemplateDTO</returns>        
        public EmailTemplateDTO InsertEmailTemplate(EmailTemplateDTO emailTemplateDTO, String loginId, int siteId)
        {
            log.LogMethodEntry(emailTemplateDTO, loginId, siteId);
            string query = @"INSERT  INTO  EmailTemplate (Name,
                                                             Description,
                                                             StartDate,
                                                             EndDate,
                                                             EmailTemplate,
                                                             LastUpdatedUser,
                                                             LastUpdatedDate,
                                                             site_id, 
                                                             MasterEntityId,
                                                             CreatedBy,
                                                             CreationDate,
                                                             IsActive)
                                                               
                                                     VALUES
                                                         (  
                                                            @name,
                                                            @description,
                                                            @startDate,
                                                            @endDate,
                                                            @emailTemplate,
                                                            @LastUpdatedUser , 
                                                            GETDATE(),
                                                            @siteId, 
                                                            @masterEntityId,
                                                            @createdBy,
                                                            GETDATE(),
                                                            @isActive                                                             
                                                          )SELECT * from EmailTemplate where EmailTemplateId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(emailTemplateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshEmailTemplateDTO(emailTemplateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting emailTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }

        /// <summary>
        /// Updates the record in database 
        /// </summary>
        /// <param name="emailTemplateDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>EmailTemplateDTO</returns>        
        public EmailTemplateDTO UpdateEmailTemplate(EmailTemplateDTO emailTemplateDTO, String loginId, int siteId)
        {
            log.LogMethodEntry(emailTemplateDTO, loginId, siteId);
            string query = @"UPDATE EmailTemplate SET 
                                                    Name= @name,
                                                    Description=@description,
                                                    StartDate=@startDate,
                                                    EndDate=@endDate,
                                                    EmailTemplate= @emailTemplate,
                                                    LastUpdatedUser=@LastUpdatedUser,
                                                    LastUpdatedDate=GETDATE(),
                                                    --site_id=@siteId,
                                                    MasterEntityId= @masterEntityId,
                                                    IsActive=@isActive
                                             WHERE  EmailTemplateId=@emailTemplateId
                                       SELECT * from EmailTemplate where EmailTemplateId = @emailTemplateId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(emailTemplateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshEmailTemplateDTO(emailTemplateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating emailTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(emailTemplateDTO);
            return emailTemplateDTO;
        }


        /// <summary>
        ///  updates the CurrencyDTO with Id ,who columns values for further process.
        /// </summary>
        /// <param name="currencyDTO">currencyDTO object is passed</param>
        /// <param name="dt">dt an object of DataTable</param>
        private void RefreshEmailTemplateDTO(EmailTemplateDTO emailTemplateDTO, DataTable dt)
        {
            log.LogMethodEntry(emailTemplateDTO, dt);
            if (dt.Rows.Count > 0)
            {
                emailTemplateDTO.EmailTemplateId = Convert.ToInt32(dt.Rows[0]["EmailTemplateId"]);
                emailTemplateDTO.LastUpdatedDate = dt.Rows[0]["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdatedDate"]);
                emailTemplateDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                emailTemplateDTO.LastUpdatedUser = dt.Rows[0]["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedUser"]);
                emailTemplateDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
                emailTemplateDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                emailTemplateDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// return the record from the database Convert the datarow to EmailTemplateDTO object
        /// </summary>
        /// <param name="emailTemplateDataRow"></param>
        /// <returns></returns>
        public EmailTemplateDTO GetEmailTemplateDTO(DataRow emailTemplateDataRow)
        {
            log.LogMethodEntry(emailTemplateDataRow);
            EmailTemplateDTO EmailTemplateDTO = new EmailTemplateDTO(
                                        Convert.ToInt32(emailTemplateDataRow["EmailTemplateId"]),
                                        emailTemplateDataRow["Name"] == DBNull.Value ? string.Empty : emailTemplateDataRow["Name"].ToString(),
                                        emailTemplateDataRow["Description"] == DBNull.Value ? string.Empty : emailTemplateDataRow["Description"].ToString(),
                                        emailTemplateDataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(emailTemplateDataRow["StartDate"]),
                                        emailTemplateDataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(emailTemplateDataRow["EndDate"]),
                                        emailTemplateDataRow["EmailTemplate"] == DBNull.Value ? string.Empty : emailTemplateDataRow["EmailTemplate"].ToString(),
                                        emailTemplateDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : emailTemplateDataRow["LastUpdatedUser"].ToString(),
                                        emailTemplateDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(emailTemplateDataRow["LastUpdatedDate"]),
                                        emailTemplateDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(emailTemplateDataRow["site_id"]),
                                        emailTemplateDataRow["Guid"] == DBNull.Value ? string.Empty : emailTemplateDataRow["Guid"].ToString(),
                                        emailTemplateDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(emailTemplateDataRow["SynchStatus"]),
                                        emailTemplateDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(emailTemplateDataRow["MasterEntityId"]),
                                        emailTemplateDataRow["CreatedBy"] == DBNull.Value ? string.Empty : emailTemplateDataRow["CreatedBy"].ToString(),
                                        emailTemplateDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(emailTemplateDataRow["CreationDate"]),
                                        emailTemplateDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(emailTemplateDataRow["IsActive"])
                                        );
            log.LogMethodExit(EmailTemplateDTO);
            return EmailTemplateDTO;
        }
        /// <summary>
        /// Delete the record from the database based on  emailTemplateId
        /// </summary>
        /// <returns>return the int </returns>
        public int DeleteEmailTemplate(int emailTemplateId)
        {
            log.LogMethodEntry(emailTemplateId);
            try
            {
                string emailTemplateQuery = @"delete  
                                          from EmailTemplate
                                          where EmailTemplateId = @emailTemplateId";

                SqlParameter[] emailTemplateParameters = new SqlParameter[1];
                emailTemplateParameters[0] = new SqlParameter("@emailTemplateId", emailTemplateId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(emailTemplateQuery, emailTemplateParameters , sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while deleting the email template.", expn);
                log.LogMethodExit(null, "Throwing exception -" + expn.Message);
                throw;
            }
        }



        /// <summary>
        /// Gets the EmailTemplateDTO based on the Id .
        /// </summary>
        /// <param name="emailTemplateId">EmailTemplateId</param>
        /// <returns>EmailTemplateDTO</returns>
        public EmailTemplateDTO GetEmailTemplateDTO(int emailTemplateId)
        {
            log.LogMethodEntry(emailTemplateId);
            EmailTemplateDTO returnValue = null;
            string query1 = SELECT_QUERY +"  WHERE et.EmailTemplateId= @emailTemplateId ";
            SqlParameter parameter = new SqlParameter("@emailTemplateId", emailTemplateId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query1, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetEmailTemplateDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Starts-GetEmailTemplateDTOList(searchParameters) Method
        /// </summary>

        public List<EmailTemplateDTO> GetEmailTemplateDTOList(List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<EmailTemplateDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<EmailTemplateDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == EmailTemplateDTO.SearchByParameters.EMAIL_TEMPLATE_ID
                            || searchParameter.Key == EmailTemplateDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == EmailTemplateDTO.SearchByParameters.SITE_ID)

                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == EmailTemplateDTO.SearchByParameters.NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),searchParameter.Value));
                        }

                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",' ') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(),sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<EmailTemplateDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EmailTemplateDTO emailTemplateDTO = GetEmailTemplateDTO(dataRow);
                    list.Add(emailTemplateDTO);
                }
            }
            log.LogMethodEntry(list);
            return list;
        }
    }
}
