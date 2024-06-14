/********************************************************************************************
 * Project Name - Receipt Print Template Details Data handler
 * Description  - Data Handler for DB operations
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2018      Mathew Ninan   Created
 *2.60        07-May-2019      Mushahid Faizan Modified IsActive DBSearchParameters in GetReceiptPrintTemplateList() method.
 *2.7.0       08-Jul-2019      Archana        Redemption Receipt changes to show ticket allocation details
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 *2.70.2        10-Dec-2019      Jinto Thomas   Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    ///  ReceiptPrintTemplate Data Handler - Handles insert, update and select of ReceiptPrintTemplate Data
    /// </summary>
    public class ReceiptPrintTemplateDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ReceiptPrintTemplate AS rt ";
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Dictionary for searching Parameters for the ReceiptPrintTemplate object.
        /// </summary>
        private static readonly Dictionary<ReceiptPrintTemplateDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ReceiptPrintTemplateDTO.SearchByParameters, string>
        {
                {ReceiptPrintTemplateDTO.SearchByParameters.ID,"rt.Id"},
                {ReceiptPrintTemplateDTO.SearchByParameters.TEMPLATE_ID,"rt.TemplateId"},
                {ReceiptPrintTemplateDTO.SearchByParameters.IS_ACTIVE, "rt.IsActive"},
                {ReceiptPrintTemplateDTO.SearchByParameters.MASTER_ENTITY_ID,"rt.MasterEntityId"},
                {ReceiptPrintTemplateDTO.SearchByParameters.SITE_ID, "rt.site_id"}
        };
        private Utilities utilities;

        /// <summary>
        /// Default constructor of PrintersDataHandler class
        /// </summary>
        public ReceiptPrintTemplateDataHandler()
        {            
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Sqltranscation as a parameter.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReceiptPrintTemplateDataHandler(SqlTransaction sqlTransaction)
            : this()
        {           
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ReceiptPrintTemplateDataHandler Record.
        /// </summary>
        /// <param name="ReceiptPrintTemplateDTO">ReceiptPrintTemplateDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ReceiptPrintTemplateDTO receiptPrintTemplateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(receiptPrintTemplateDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", receiptPrintTemplateDTO.ID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@templateId", receiptPrintTemplateDTO.TemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fontName", string.IsNullOrEmpty(receiptPrintTemplateDTO.FontName) ? DBNull.Value : (object)receiptPrintTemplateDTO.FontName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fontSize", receiptPrintTemplateDTO.FontSize == null || receiptPrintTemplateDTO.FontSize == -1 ? DBNull.Value : (object)receiptPrintTemplateDTO.FontSize));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sequence", receiptPrintTemplateDTO.Sequence));
            parameters.Add(dataAccessHandler.GetSQLParameter("@section", string.IsNullOrEmpty(receiptPrintTemplateDTO.Section) ? DBNull.Value : (object)receiptPrintTemplateDTO.Section));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col1Data", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col1Data) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col1Data));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col1Alignment", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col1Alignment) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col1Alignment));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col2Data", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col2Data) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col2Data));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col2Alignment", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col2Alignment) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col2Alignment));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col3Data", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col3Data) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col3Data));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col3Alignment", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col3Alignment) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col3Alignment));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col4Data", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col4Data) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col4Data));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col4Alignment", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col4Alignment) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col4Alignment));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col5Data", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col5Data) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col5Data));
            parameters.Add(dataAccessHandler.GetSQLParameter("@col5Alignment", string.IsNullOrEmpty(receiptPrintTemplateDTO.Col5Alignment) ? DBNull.Value : (object)receiptPrintTemplateDTO.Col5Alignment));
            parameters.Add(dataAccessHandler.GetSQLParameter("@metadata", string.IsNullOrEmpty(receiptPrintTemplateDTO.MetaData) ? DBNull.Value : (object)receiptPrintTemplateDTO.MetaData));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", receiptPrintTemplateDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", receiptPrintTemplateDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///Inserts the ReceiptPrintTemplateHeader record to the database
        /// </summary>
        /// <param name="receiptPrintTemplateDTO">receiptPrintTemplateDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>returns the DTO</returns>
        public ReceiptPrintTemplateDTO InsertReceiptPrintTemplate(ReceiptPrintTemplateDTO receiptPrintTemplateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(receiptPrintTemplateDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ReceiptPrintTemplate]
                                                       ( Section,
	                                                     Sequence,
	                                                     Col1Data,
	                                                     Col1Alignment,
	                                                     Col2Data,
	                                                     Col2Alignment,
	                                                     Col3Data,
	                                                     Col3Alignment,
	                                                     Col4Data,
	                                                     Col4Alignment,
	                                                     Col5Data,
	                                                     Col5Alignment,
	                                                     TemplateId,
	                                                     Guid,
	                                                     site_id,
	                                                     FontName,
	                                                     FontSize,
	                                                     MasterEntityId,
	                                                     Metadata,
                                                         IsActive,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdateDate,
                                                         LastUpdatedBy
                                                       )
                                               values(   @section,
	                                                     @sequence,
	                                                     @col1Data,
	                                                     @col1Alignment,
	                                                     @col2Data,
	                                                     @col2Alignment,
	                                                     @col3Data,
	                                                     @col3Alignment,
	                                                     @col4Data,
	                                                     @col4Alignment,
	                                                     @col5Data,
	                                                     @col5Alignment,
	                                                     @templateId,
                                                         NewId(),
	                                                     @siteid,
	                                                     @fontName,
	                                                     @fontSize,
	                                                     @masterEntityId,
	                                                     @metadata,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),
                                                         Getdate(),
                                                         @lastUpdatedBy)
                                                       SELECT * FROM ReceiptPrintTemplate WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(receiptPrintTemplateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReceiptPrintTemplateDTO(receiptPrintTemplateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting receiptPrintTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(receiptPrintTemplateDTO);
            return receiptPrintTemplateDTO;
        }

        /// <summary>
        ///Updates the ReceiptPrintTemplate record to the database
        /// </summary>
        /// <param name="receiptPrintTemplateDTO">receiptPrintTemplateDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>returns the DTO</returns>
        public ReceiptPrintTemplateDTO UpdateReceiptPrintTemplate(ReceiptPrintTemplateDTO receiptPrintTemplateDTO, string loginId, int siteId)
        {           
            log.LogMethodEntry(receiptPrintTemplateDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[ReceiptPrintTemplate]
                                    SET 
                                                           Section=@section,
	                                                       Sequence=@sequence,
	                                                       col1Data=@col1Data,
	                                                       col1Alignment=@col1Alignment,
	                                                       col2Data=@col2Data,
	                                                       col2Alignment=@col2Alignment,
	                                                       col3Data=@col3Data,
	                                                       col3Alignment=@col3Alignment,
	                                                       col4Data=@col4Data,
	                                                       col4Alignment=@col4Alignment,
	                                                       col5Data=@col5Data,
	                                                       col5Alignment=@col5Alignment,
                                                           TemplateId = @templateId,
                                                           FontName = @fontName,
                                                           FontSize = @fontSize,
                                                           Metadata=@metadata,
                                                           IsActive = @isActive,
                                                           LastUpdateDate = Getdate(),
                                                           LastUpdatedBy = @lastUpdatedBy, 
                                                           -- site_id = @siteId,
                                                           MasterEntityId =  @masterEntityId
                                                           where  Id = @id 
                                        SELECT * FROM ReceiptPrintTemplate WHERE Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(receiptPrintTemplateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReceiptPrintTemplateDTO(receiptPrintTemplateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating receiptPrintTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(receiptPrintTemplateDTO);
            return receiptPrintTemplateDTO;
        }

        /// <summary>
        /// Delete the record from the receiptPrintTemplate database based on Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>return the int </returns>
        internal int Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM ReceiptPrintTemplate
                             WHERE ReceiptPrintTemplate.Id = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            int id1 = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id1);
            return id1;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="receiptPrintTemplateDTO">receiptPrintTemplateDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshReceiptPrintTemplateDTO(ReceiptPrintTemplateDTO receiptPrintTemplateDTO, DataTable dt)
        {
            log.LogMethodEntry(receiptPrintTemplateDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                receiptPrintTemplateDTO.ID = Convert.ToInt32(dt.Rows[0]["Id"]);
                receiptPrintTemplateDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                receiptPrintTemplateDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                receiptPrintTemplateDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                receiptPrintTemplateDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                receiptPrintTemplateDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                receiptPrintTemplateDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ReceiptPrintTemplateHeader record from the database
        /// </summary>
        /// <param name="dataRow">dataRow</param>
        /// <returns>receiptPrintTemplateDTO</returns>
        private ReceiptPrintTemplateDTO GetReceiptPrintTemplateDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ReceiptPrintTemplateDTO receiptPrintTemplateDTO = new ReceiptPrintTemplateDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["Section"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Section"]),
                                            dataRow["Sequence"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Sequence"]),
                                            dataRow["Col1Data"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Col1Data"]),
                                            dataRow["Col1Alignment"] == DBNull.Value ? "H" : Convert.ToString(dataRow["Col1Alignment"]),
                                            dataRow["Col2Data"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Col2Data"]),
                                            dataRow["Col2Alignment"] == DBNull.Value ? "H" : Convert.ToString(dataRow["Col2Alignment"]),
                                            dataRow["Col3Data"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Col3Data"]),
                                            dataRow["Col3Alignment"] == DBNull.Value ? "H" : Convert.ToString(dataRow["Col3Alignment"]),
                                            dataRow["Col4Data"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Col4Data"]),
                                            dataRow["Col4Alignment"] == DBNull.Value ? "H" : Convert.ToString(dataRow["Col4Alignment"]),
                                            dataRow["Col5Data"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Col5Data"]),
                                            dataRow["Col5Alignment"] == DBNull.Value ? "H" : Convert.ToString(dataRow["Col5Alignment"]),
                                            dataRow["TemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TemplateId"]),
                                            dataRow["FontName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FontName"]),
                                            dataRow["FontSize"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["FontSize"]),
                                            dataRow["Metadata"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Metadata"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : dataRow["IsActive"].ToString() == "Y" ? true : false,
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            null
                                            );
            log.LogMethodExit(receiptPrintTemplateDTO);
            return receiptPrintTemplateDTO;
        }

        /// <summary>
        /// Gets the ReceiptPrintTemplate data of passed id 
        /// </summary>
        /// <param name="id">id of ReceiptPrintTemplate is passed as parameter</param>
        /// <returns>Returns ReceiptPrintTemplate</returns>
        public ReceiptPrintTemplateDTO GetReceiptPrintTemplate(int id)
        {
            log.LogMethodEntry(id);
            ReceiptPrintTemplateDTO result = null;
            string query = SELECT_QUERY + @" WHERE rt.Id= @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetReceiptPrintTemplateDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ReceiptPrintTemplateDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>List of DTO</returns>
        public List<ReceiptPrintTemplateDTO> GetReceiptPrintTemplateList(List<KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ReceiptPrintTemplateDTO> receiptPrintTemplateDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            string orderBy = @" order by case Section when 'HEADER' then 1 
                                                      when 'REDEMPTION_SOURCE_HEADER' then 2 
                                                      when 'REDEMPTION_SOURCE' then 3 
                                                      when 'REDEMPTION_SOURCE_TOTAL' then 4 
                                                      when 'REDEEMED_GIFTS' then 5 
                                                      when 'PRODUCT' then 6
                                                      when 'PRODUCTSUMMARY' then 7
					                                  when 'CARDINFO' then 8 
					                                  when 'TRANSACTIONTOTAL' then 9
					                                  when 'DISCOUNTS' then 10
					                                  when 'DISCOUNTTOTAL' then 11
					                                  when 'TAXLINE' then 12
					                                  when 'TAXABLECHARGES' then 13
					                                  when 'TAXTOTAL' then 14
					                                  when 'NONTAXABLECHARGES' then 15
					                                  when 'GRANDTOTAL' then 16
					                                  when 'FOOTER' then 17
					                                  when 'ITEMSLIP' then 18
                                                      when 'REDEMPTION_BALANCE' then 19
					                                  else 20 end, 
					                                  Sequence";
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        
                        {
                            if (searchParameter.Key.Equals(ReceiptPrintTemplateDTO.SearchByParameters.TEMPLATE_ID) ||
                                searchParameter.Key.Equals(ReceiptPrintTemplateDTO.SearchByParameters.ID) ||
                                searchParameter.Key.Equals(ReceiptPrintTemplateDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == ReceiptPrintTemplateDTO.SearchByParameters.SITE_ID)
                            {

                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(ReceiptPrintTemplateDTO.SearchByParameters.IS_ACTIVE))
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
                    selectQuery = selectQuery + query + orderBy;
            }

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                receiptPrintTemplateDTOList = new List<ReceiptPrintTemplateDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReceiptPrintTemplateDTO receiptPrintTemplateDTO = GetReceiptPrintTemplateDTO(dataRow);
                    receiptPrintTemplateDTOList.Add(receiptPrintTemplateDTO);
                }
            }
            log.LogMethodExit(receiptPrintTemplateDTOList);
            return receiptPrintTemplateDTOList;
        }

    }

}
