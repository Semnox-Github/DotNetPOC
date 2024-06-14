/********************************************************************************************
 * Project Name - Receipt Print Template Header Datahandler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2018      Mathew Ninan   Created 
 *2.60        07-May-2019      Mushahid Faizan Modified IsActive DBSearchParameters in GetReceiptPrintTemplateHeaderList() method.
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 *2.70.2        10-Dec-2019      Jinto Thomas   Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Data;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// ReceiptPrintTemplateHeader Data Handler - Handles insert, update and select of  ReceiptPrintTemplateHeader objects
    /// </summary>
    class ReceiptPrintTemplateHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ReceiptPrintTemplateHeader AS pth ";

        /// <summary>
        /// Dictionary for searching Parameters for the ReceiptPrintTemplateHeader object.
        /// </summary>
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>
        {
                {ReceiptPrintTemplateHeaderDTO.SearchByParameters.TEMPLATE_ID,"pth.TemplateId"},
                {ReceiptPrintTemplateHeaderDTO.SearchByParameters.TEMPLATE_NAME,"pth.TemplateName"},
                {ReceiptPrintTemplateHeaderDTO.SearchByParameters.IS_ACTIVE, "pth.IsActive"},
                {ReceiptPrintTemplateHeaderDTO.SearchByParameters.MASTER_ENTITY_ID,"pth.MasterEntityId"},
                {ReceiptPrintTemplateHeaderDTO.SearchByParameters.SITE_ID, "pth.site_id"},
                {ReceiptPrintTemplateHeaderDTO.SearchByParameters.GUID, "pth.Guid"}
        };
        private Utilities utilities;

        /// <summary>
        /// Default constructor of ReceiptPrintTemplateHeaderDataHandler class
        /// </summary>
        public ReceiptPrintTemplateHeaderDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with sqlTranscation as a parameter of ReceiptPrintTemplateHeaderDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public ReceiptPrintTemplateHeaderDataHandler(SqlTransaction sqlTransaction) : this()
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ReceiptPrintTemplateHeaderDataHandler Record.
        /// </summary>
        /// <param name="ReceiptPrintTemplateHeaderDTO">ReceiptPrintTemplateHeaderDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(receiptPrintTemplateHeaderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@templateId", receiptPrintTemplateHeaderDTO.TemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fontName", string.IsNullOrEmpty(receiptPrintTemplateHeaderDTO.FontName) ? DBNull.Value : (object)receiptPrintTemplateHeaderDTO.FontName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fontSize", (receiptPrintTemplateHeaderDTO.FontSize == null || receiptPrintTemplateHeaderDTO.FontSize == -1) ? DBNull.Value : (object)receiptPrintTemplateHeaderDTO.FontSize));
            parameters.Add(dataAccessHandler.GetSQLParameter("@templateName", receiptPrintTemplateHeaderDTO.TemplateName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", receiptPrintTemplateHeaderDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", receiptPrintTemplateHeaderDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ReceiptPrintTemplateHeader record to the database
        /// </summary>
        /// <param name="receiptPrintTemplateHeaderDTO">receiptPrintTemplateHeaderDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>receiptPrintTemplateHeaderDTO</returns>
        public ReceiptPrintTemplateHeaderDTO InsertReceiptPrintTemplateHeader(ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO, string loginId, int siteId)
        {           
            log.LogMethodEntry(receiptPrintTemplateHeaderDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ReceiptPrintTemplateHeader]
                                                       ( 
                                                         TemplateName,
                                                         FontName,
                                                         FontSize,
                                                         IsActive,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdateDate,
                                                         LastUpdatedBy,
                                                         Guid,
                                                         site_id, 
                                                         MasterEntityId
                                                       )
                                               values(  
                                                         @templateName,
                                                         @fontName,
                                                         @fontSize,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),
                                                         Getdate(),
                                                         @lastUpdatedBy,
                                                         NewId(),
                                                         @siteId,
                                                         @masterEntityId)
                                                SELECT * FROM ReceiptPrintTemplateHeader WHERE TemplateId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(receiptPrintTemplateHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReceiptPrintTemplateHeaderDTO(receiptPrintTemplateHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting receiptPrintTemplateHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(receiptPrintTemplateHeaderDTO);
            return receiptPrintTemplateHeaderDTO;
        }

        /// <summary>
        /// Updates the ReceiptPrintTemplateHeader record to the database
        /// </summary>
        /// <param name="receiptPrintTemplateHeaderDTO">receiptPrintTemplateHeaderDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>receiptPrintTemplateHeaderDTO</returns>
        public ReceiptPrintTemplateHeaderDTO UpdateReceiptPrintTemplateHeader(ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO, string loginId, int siteId)
        {         
            log.LogMethodEntry(receiptPrintTemplateHeaderDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[ReceiptPrintTemplateHeader]
                                    SET 
                                                           TemplateName =  @templateName,
                                                           FontName = @fontName,
                                                           FontSize = @fontSize,
                                                           IsActive = @isActive,
                                                           -- site_id = @siteId,
                                                           MasterEntityId =  @masterEntityId,
                                                           LastUpdatedBy = @lastUpdatedBy,
                                                           LastUpdateDate = getdate()
                                                           WHERE  TemplateId = @templateId
                                                     SELECT * FROM ReceiptPrintTemplateHeader WHERE TemplateId = @templateId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(receiptPrintTemplateHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReceiptPrintTemplateHeaderDTO(receiptPrintTemplateHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating receiptPrintTemplateHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(receiptPrintTemplateHeaderDTO);
            return receiptPrintTemplateHeaderDTO;
        }

        /// <summary>
        ///  Delete the record from the ReceiptPrintTemplateHeader database based on templateId
        /// </summary>
        /// <param name="templateId">templateId</param>
        /// <returns>Returns the int</returns>
        internal int Delete(int templateId)
        {
            log.LogMethodEntry(templateId);
            string query = @"DELETE  
                             FROM ReceiptPrintTemplateHeader
                             WHERE ReceiptPrintTemplateHeader.TemplateId = @templateId";
            SqlParameter parameter = new SqlParameter("@templateId", templateId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="receiptPrintTemplateHeaderDTO">receiptPrintTemplateHeaderDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshReceiptPrintTemplateHeaderDTO(ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO, DataTable dt)
        {
            log.LogMethodEntry(receiptPrintTemplateHeaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                receiptPrintTemplateHeaderDTO.TemplateId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                receiptPrintTemplateHeaderDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                receiptPrintTemplateHeaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                receiptPrintTemplateHeaderDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                receiptPrintTemplateHeaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                receiptPrintTemplateHeaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                receiptPrintTemplateHeaderDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ReceiptPrintTemplateHeadre data of passed id 
        /// </summary>
        /// <param name="id">id of ReceiptPrintTemplateHeadre is passed as parameter</param>
        /// <returns>Returns ReceiptPrintTemplateHeader</returns>
        public ReceiptPrintTemplateHeaderDTO GetReceiptPrintTemplateHeaderDTO(int id)
        {
            log.LogMethodEntry(id);
            ReceiptPrintTemplateHeaderDTO result = null;
            string query = SELECT_QUERY + @" WHERE pth.TemplateId= @templateId";
            SqlParameter parameter = new SqlParameter("@templateId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetReceiptPrintTemplateHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ReceiptPrintTemplateHeader data of Query 
        /// </summary>
        /// <param name="templateId">templateId</param>
        /// <param name="templateName">templateName</param>
        /// <param name="duplicate">duplicate</param>
        /// <param name="loginId">loginId</param>
        /// <returns>Returns Query</returns>
        internal string GetExportQueries(int templateId, string templateName, bool duplicate, string loginId)
        {
            log.LogMethodEntry(templateId, templateName, duplicate, loginId);
            DataTable result;
            string queryHeader = @"
            declare @TemplateId int
            INSERT INTO [ReceiptPrintTemplateHeader]
                                                   ([TemplateName]
                                                   ,[FontName]
                                                   ,[FontSize]
                                                   ,[site_id]
                                                   ,[IsActive] )
                                             VALUES
                                                   (";

            result = dataAccessHandler.executeSelectQuery(@"select '''' + replace(TemplateName, '''', '''''') + '' " + (duplicate ? " + ' - Copy'" : " + ''") + @"',' +
                                                                   case when FontName is null then 'NULL' else '''' + FontName + '''' end + ',' + 
                                                                   case when FontSize is null then 'NULL' else cast(FontSize as varchar) end + ',' + 
                                                                   case when site_id is null then 'NULL' else cast(site_id as varchar) end +',''' + ISNULL(IsActive,'Y')+''''
                                                                    from ReceiptPrintTemplateHeader 
                                                                    where TemplateId = @templateId;", GetExportQueryParameters(templateId, loginId), sqlTransaction);

            if (result!=null && result.Rows.Count>0)
            {
                queryHeader += result.Rows[0][0].ToString()+")";
            }
            queryHeader += "select @TemplateId = @@identity;" + Environment.NewLine;
            string queryTemplateInsert = @"INSERT INTO [ReceiptPrintTemplate]
                                                    ([Section]
                                                    ,[Sequence]
                                                    ,[Col1Data]
                                                    ,[Col1Alignment]
                                                    ,[Col2Data]
                                                    ,[Col2Alignment]
                                                    ,[Col3Data]
                                                    ,[Col3Alignment]
                                                    ,[Col4Data]
                                                    ,[Col4Alignment]
                                                    ,[Col5Data]
                                                    ,[Col5Alignment]
                                                    ,[TemplateId]
                                                    ,[site_id]
                                                    ,[FontName]
                                                    ,[FontSize]
                                                    ,[IsActive])
                                                VALUES
                                                    (";

            DataTable dtTemplate = dataAccessHandler.executeSelectQuery(@"select ''''+ Section + ''',' +
                                                           cast([Sequence] as varchar) + ',' + 
                                                           case when [Col1Data] is null then 'NULL,' else '''' + replace([Col1Data], '''', '''''') + ''',' end +
                                                           case when [Col1Alignment] is null then 'NULL,' else '''' + [Col1Alignment] + ''',' end +
                                                           case when [Col2Data] is null then 'NULL,' else '''' + replace([Col2Data], '''', '''''') + ''',' end +
                                                           case when [Col2Alignment] is null then 'NULL,' else '''' + [Col2Alignment] + ''',' end +
                                                           case when [Col3Data] is null then 'NULL,' else '''' + replace([Col3Data], '''', '''''') + ''',' end +
                                                           case when [Col3Alignment] is null then 'NULL,' else '''' + [Col3Alignment] + ''',' end +
                                                           case when [Col4Data] is null then 'NULL,' else '''' + replace([Col4Data], '''', '''''') + ''',' end +
                                                           case when [Col4Alignment] is null then 'NULL,' else '''' + [Col4Alignment] + ''',' end +
                                                           case when [Col5Data] is null then 'NULL,' else '''' + replace([Col5Data], '''', '''''') + ''',' end +
                                                           case when [Col5Alignment] is null then 'NULL,' else '''' + [Col5Alignment] + ''',' end +
                                                           '@TemplateId' + ',' + 
                                                           case when site_id is null then 'NULL' else cast(site_id as varchar) end + ',' +
		                                                   case when FontName is null then 'NULL' else '''' + FontName + '''' end + ',' +
                                                           case when FontSize is null then 'NULL' else cast(FontSize as varchar) end +',''' + ISNULL(IsActive,'Y')+''''
                                                            from ReceiptPrintTemplate
                                                            where TemplateId= @templateId",
                                                             GetExportQueryParameters(templateId, loginId));

            List<string> templateQuery = new List<string>();
            foreach (DataRow dr in dtTemplate.Rows)
                templateQuery.Add(queryTemplateInsert + dr[0].ToString() + ")");

            string ticketHeaderInsert = "";
            List<string> ticketElementsQuery = new List<string>();

            result = dataAccessHandler.executeSelectQuery(@"select '@TemplateId,' + 
                                                                   cast(Width as varchar) + ',' + 
                                                                   cast(Height as varchar) + ',' + 
                                                                   cast(LeftMargin as varchar) + ',' + 
                                                                   cast(RightMargin as varchar) + ',' + 
                                                                   cast(TopMargin as varchar) + ',' + 
                                                                   cast(BottomMargin as varchar) + ',' + 
                                                                   'cast(' + cast(cast(getdate() as decimal(20, 10)) as nvarchar) + ' as datetime)' + ',' +
                                                                   '''' + @user + ''',' +
                                                                   cast(BorderWidth as varchar) + ',' + 
                                                                   case when site_id is null then 'NULL' else cast(site_id as varchar) end+','+ cast(isNull(Isactive,1) as varchar)
                                                                    from TicketTemplateHeader 
                                                                    where TemplateId = @templateId",
                                                                  GetExportQueryParameters(templateId, loginId));
            if (result != null && result.Rows.Count > 0)
            {
                ticketHeaderInsert = @"
            declare @TicketTemplateId int
            INSERT INTO [TicketTemplateHeader]
                                                    ([TemplateId]
                                                    ,[Width]
                                                    ,[Height]
                                                    ,[LeftMargin]
                                                    ,[RightMargin]
                                                    ,[TopMargin]
                                                    ,[BottomMargin]
                                                    ,[LastUpdateDate]
                                                    ,[LastUpdatedBy]
                                                    ,[BorderWidth]
                                                    ,[site_id]
                                                    ,[Isactive])
                                                VALUES
                                                    (" + result.Rows[0][0].ToString() + @"); 
                select @TicketTemplateId = @@identity" + Environment.NewLine;

                string elementInsert = @"INSERT INTO [TicketTemplateElements]
                                                   ([TicketTemplateId]
                                                   ,[UniqueId]
                                                   ,[Name]
                                                   ,[Value]
                                                   ,[Type]
                                                   ,[Font]
                                                   ,[Location]
                                                   ,[site_id]
                                                   ,[Width]
                                                   ,[Alignment]
                                                   ,[Rotate]
                                                   ,[Color])
                                             VALUES
                                                   (";

                DataTable dtTicketElements = dataAccessHandler.executeSelectQuery(@"select '@TicketTemplateId, ' +
                                                                                '''' + te.UniqueId + ''',' + 
                                                                                '''' + replace(te.Name, '''', '''''') + ''',' + 
                                                                                '''' + replace(te.Value, '''', '''''') + ''',' + 
                                                                                cast(te.Type as varchar) + ',' +
                                                                                '''' + te.Font + ''',' + 
                                                                                '''' + te.Location + ''',' + 
                                                                                case when te.site_id is null then 'NULL' else cast(te.site_id as varchar) end + ',' +
                                                                                cast(te.Width as varchar) + ',' +
                                                                                '''' + te.Alignment + ''',' + 
                                                                                case when te.Rotate is null then 'NULL' else '''' + te.Rotate + '''' end + ',' +
                                                                                case when te.Color is null then 'NULL' else '''' + te.Color + '''' end + ')'
                                                                                from TicketTemplateElements te, TicketTemplateHeader h
                                                                                where te.TicketTemplateId = h.TicketTemplateId
                                                                                and h.TemplateId = @TemplateId",
                                                                               GetExportQueryParameters(templateId, loginId));

                foreach (DataRow dr in dtTicketElements.Rows)
                    ticketElementsQuery.Add(elementInsert + dr[0].ToString());
            }

            string finalQuery = @"BEGIN
                BEGIN TRY
            " +
                                @queryHeader;
            foreach (string q in templateQuery)
                finalQuery += q + Environment.NewLine;

            if (string.IsNullOrEmpty(ticketHeaderInsert) == false)
            {
                finalQuery += ticketHeaderInsert;
                foreach (string q in ticketElementsQuery)
                    finalQuery += q + Environment.NewLine;
            }

            finalQuery += @"END TRY
            BEGIN CATCH
                THROW
            END CATCH
            END; select 1";

            if (duplicate)
            {

                using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                {
                    parafaitDBTransaction.BeginTransaction();
                    try
                    {
                        log.LogVariableState("finalQuery", finalQuery);
                        dataAccessHandler.executeSelectQuery(finalQuery, null, parafaitDBTransaction.SQLTrx);
                        parafaitDBTransaction.EndTransaction();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        parafaitDBTransaction.RollBack();
                        throw ;
                    }
                }
            }
            log.LogMethodExit("finalQuery", finalQuery);
            return finalQuery;
        }

        internal void ExecuteImportQuery(string query, string loginId)
        {            
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@LastUpdatedBy", loginId));
            sqlParameters.Add(new SqlParameter("@LastUpdateDate", DateTime.Now));
            dataAccessHandler.executeUpdateQuery(query, sqlParameters.ToArray(), sqlTransaction);
        }
        /// <summary>
        /// Gets the GetExportQueryParameters
        /// </summary>
        /// <param name="templateId">templateId</param>
        /// <param name="loginId">loginId</param>
        /// <returns>returns Array</returns>
        private static SqlParameter[] GetExportQueryParameters(int templateId, string loginId)
        {
            log.LogMethodEntry(templateId, loginId);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@templateId", templateId));
            sqlParameters.Add(new SqlParameter("@user", loginId));
            log.LogMethodExit(sqlParameters);
            return sqlParameters.ToArray();
        }

        /// <summary>
        ///Gets the ReceiptPrintTemplateHeader record from the database
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>receiptPrintTemplateHeaderDTO</returns>
        private ReceiptPrintTemplateHeaderDTO GetReceiptPrintTemplateHeaderDTO(DataRow dataRow)
        {           
            log.LogMethodEntry(dataRow);
            ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO = new ReceiptPrintTemplateHeaderDTO(Convert.ToInt32(dataRow["TemplateId"]),
                                            dataRow["TemplateName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TemplateName"]),
                                            dataRow["FontName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FontName"]),
                                            dataRow["FontSize"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["FontSize"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : (dataRow["IsActive"].ToString() == "Y" ? true : false) ,
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(receiptPrintTemplateHeaderDTO);
            return receiptPrintTemplateHeaderDTO;
        }

        /// <summary>
        /// Gets the Receipt Template header data of passed TemplateId
        /// </summary>
        /// <param name="PrintTemplateId">integer type parameter</param>
        /// <returns>Returns PrinterProductDTO</returns>
        public ReceiptPrintTemplateHeaderDTO GetReceiptPrintTemplateHeader(int PrintTemplateId)
        {
            log.LogMethodEntry(PrintTemplateId);
            ReceiptPrintTemplateHeaderDTO result = null;
            string query = SELECT_QUERY + @" WHERE pth.TemplateId= @templateId";
            SqlParameter parameter = new SqlParameter("@templateId", PrintTemplateId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetReceiptPrintTemplateHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ReceiptPrintTemplateHeaderDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>Returns List DTO</returns>
        public List<ReceiptPrintTemplateHeaderDTO> GetReceiptPrintTemplateHeaderList(List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(ReceiptPrintTemplateHeaderDTO.SearchByParameters.TEMPLATE_ID) ||
                            searchParameter.Key.Equals(ReceiptPrintTemplateHeaderDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReceiptPrintTemplateHeaderDTO.SearchByParameters.SITE_ID)

                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(ReceiptPrintTemplateHeaderDTO.SearchByParameters.TEMPLATE_NAME)||
                                 searchParameter.Key.Equals(ReceiptPrintTemplateHeaderDTO.SearchByParameters.GUID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(ReceiptPrintTemplateHeaderDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {

                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    count++;
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                receiptPrintTemplateHeaderDTOList = new List<ReceiptPrintTemplateHeaderDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO = GetReceiptPrintTemplateHeaderDTO(dataRow);
                    receiptPrintTemplateHeaderDTOList.Add(receiptPrintTemplateHeaderDTO);
                }
            }

            log.LogMethodExit(receiptPrintTemplateHeaderDTOList);
            return receiptPrintTemplateHeaderDTOList;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ReceiptPrintTemplateHeaderDTO> PopulateTemplate()
        {
            string selectQuery = @"select *  from ReceiptPrintTemplateHeader rh ,TicketTemplateHeader th
                                                            where rh.TemplateId = th.TemplateId ";

            DataTable ReceiptPrintTemplateHeaderData = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (ReceiptPrintTemplateHeaderData.Rows.Count > 0)
            {
                List<ReceiptPrintTemplateHeaderDTO> ReceiptPrintTemplateHeaderList = new List<ReceiptPrintTemplateHeaderDTO>();
                foreach (DataRow ReceiptPrintTemplateHeaderRow in ReceiptPrintTemplateHeaderData.Rows)
                {
                    ReceiptPrintTemplateHeaderDTO ReceiptPrintTemplateHeaderObject = GetReceiptPrintTemplateHeaderDTO(ReceiptPrintTemplateHeaderRow);
                    ReceiptPrintTemplateHeaderList.Add(ReceiptPrintTemplateHeaderObject);
                }
                log.LogMethodExit(ReceiptPrintTemplateHeaderList);
                return ReceiptPrintTemplateHeaderList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
    }
}
