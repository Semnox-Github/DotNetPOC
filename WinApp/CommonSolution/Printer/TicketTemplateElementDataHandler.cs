/********************************************************************************************
 * Project Name - Printer
 * Description  - Data Handler File for TicketTemplateElement
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *            01-July-2019  Mushahid Faizan         Modified GetSqlParameters() & RefreshTicketTemplateElementDTO() method.
 *2.70.3      01-Apr-2020  Girish Kundar            Modified: Search by TemplateId list method in build child records           
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// TicketTemplateElement Data Handler - Handles insert, update and selection of TicketTemplateElement objects
    /// </summary>
    public class TicketTemplateElementDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TicketTemplateElements as tte ";

        /// <summary>
        /// Dictionary for searching Parameters for the TicketTemplateElements object.
        /// </summary>
        private static readonly Dictionary<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string> DBSearchParameters = new Dictionary<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>
        {
            { TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TICKET_TEMPLATE_ELEMENT_ID,"tte.Id"},
            { TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TICKET_TEMPLATE_ID,"tte.TicketTemplateId"},
            { TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TICKET_TEMPLATE_ID_LIST,"tte.TicketTemplateId"},
            { TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.UNIQUE_ID,"tte.UniqueId"},
            { TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.ACTIVE_FLAG,"tte.ActiveFlag"},
            { TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.LOCATION,"tte.Location"},
            { TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TYPE,"tte.Type"},
            { TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.SITE_ID,"tte.site_id"},
            { TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.MASTER_ENTITY_ID,"tte.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for TicketTemplateElement.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public TicketTemplateElementDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TicketTemplateElement Record.
        /// </summary>
        /// <param name="ticketTemplateElementDTO">ticketTemplateElementDTO object passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(TicketTemplateElementDTO ticketTemplateElementDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketTemplateElementDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", ticketTemplateElementDTO.TicketTemplateElementId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketTemplateId", ticketTemplateElementDTO.TicketTemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UniqueId", ticketTemplateElementDTO.UniqueId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", ticketTemplateElementDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value", ticketTemplateElementDTO.Value));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", ticketTemplateElementDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Font", ticketTemplateElementDTO.Font));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Location", ticketTemplateElementDTO.Location));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Width", ticketTemplateElementDTO.Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Alignment", ticketTemplateElementDTO.Alignment));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Rotate", ticketTemplateElementDTO.Rotate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FormatId", ticketTemplateElementDTO.FormatId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Color", ticketTemplateElementDTO.Color));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BarCodeHeight", ticketTemplateElementDTO.BarCodeHeight));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", ticketTemplateElementDTO.MasterEntityId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", ticketTemplateElementDTO.ActiveFlag));
            log.LogMethodExit(parameters);
            return parameters;
        }
        ///<summary>
        /// Converts the Data row object to TicketTemplateElementDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of TicketTemplateElementDTO</returns>
        private TicketTemplateElementDTO GetTicketTemplateElementDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TicketTemplateElementDTO ticketTemplateElementDTO = new TicketTemplateElementDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["TicketTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TicketTemplateId"]),
                                                dataRow["UniqueId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UniqueId"]),
                                                dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                dataRow["Value"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Value"]),
                                                dataRow["Type"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Type"]),
                                                dataRow["Font"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Font"]),
                                                dataRow["Location"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Location"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["Width"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Width"]),
                                                dataRow["Alignment"] == DBNull.Value ? 'L' : Convert.ToChar(dataRow["Alignment"]),
                                                dataRow["Rotate"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["Rotate"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["FormatId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FormatId"]),
                                                dataRow["Color"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Color"]),
                                                dataRow["BarCodeHeight"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BarCodeHeight"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["ActiveFlag"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["ActiveFlag"]));
            return ticketTemplateElementDTO;
        }

        /// <summary>
        /// Gets the TicketTemplateElement data of passed TicketTemplateElement ID
        /// </summary>
        /// <param name="ticketTemplateElementId">ticketTemplateElementId of TicketTemplateElement passed as parameter </param>
        /// <returns>Returns TicketTemplateElementDTO</returns>
        public TicketTemplateElementDTO GetTicketTemplateElementDTO(int ticketTemplateElementId)
        {
            log.LogMethodEntry(ticketTemplateElementId);
            TicketTemplateElementDTO result = null;
            string query = SELECT_QUERY + @" WHERE tte.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", ticketTemplateElementId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTicketTemplateElementDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the TicketTemplateElement record
        /// </summary>
        /// <param name="ticketTemplateElementDTO">TicketTemplateElementDTO is passed as parameter</param>
        internal void Delete(TicketTemplateElementDTO ticketTemplateElementDTO)
        {
            log.LogMethodEntry(ticketTemplateElementDTO);
            string query = @"DELETE  
                             FROM TicketTemplateElements
                             WHERE TicketTemplateElements.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", ticketTemplateElementDTO.TicketTemplateElementId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            ticketTemplateElementDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="ticketTemplateElementDTO">TicketTemplateElementDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshTicketTemplateElementDTO(TicketTemplateElementDTO ticketTemplateElementDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketTemplateElementDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                ticketTemplateElementDTO.TicketTemplateElementId = Convert.ToInt32(dt.Rows[0]["Id"]);
                ticketTemplateElementDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                ticketTemplateElementDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                ticketTemplateElementDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                ticketTemplateElementDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                ticketTemplateElementDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                ticketTemplateElementDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the TicketTemplateElement Table. 
        /// </summary>
        /// <param name="ticketTemplateElementDTO">TicketTemplateElementDTO object passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        /// <returns>Returns updated TicketTemplateElementDTO</returns>
        public TicketTemplateElementDTO Insert(TicketTemplateElementDTO ticketTemplateElementDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketTemplateElementDTO, loginId, siteId);
            if (string.IsNullOrEmpty(ticketTemplateElementDTO.UniqueId))
            {
                ticketTemplateElementDTO.UniqueId = "U" + Guid.NewGuid().ToString();
            }
            string query = @"INSERT INTO [dbo].[TicketTemplateElements]
                            (
                            TicketTemplateId,
                            UniqueId,
                            Name,
                            Value,
                            Type,
                            Font,
                            Location,
                            Guid,
                            site_id,
                            Width,
                            Alignment,
                            Rotate,
                            MasterEntityId,
                            FormatId,
                            Color,
                            BarCodeHeight,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate,
                            ActiveFlag
                            )
                            VALUES
                            (
                            @TicketTemplateId,
                            @UniqueId,
                            @Name,
                            @Value,
                            @Type,
                            @Font,
                            @Location,
                            NEWID(),
                            @site_id,
                            @Width,
                            @Alignment,
                            @Rotate,
                            @MasterEntityId,
                            @FormatId,
                            @Color,
                            @BarCodeHeight,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),
                            @ActiveFlag
                            )
                            SELECT * FROM TicketTemplateElements WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(ticketTemplateElementDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTicketTemplateElementDTO(ticketTemplateElementDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TicketTemplateElementDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(ticketTemplateElementDTO);
            return ticketTemplateElementDTO;
        }

        /// <summary>
        /// Update the record in the TicketTemplateElements Table. 
        /// </summary>
        /// <param name="ticketTemplateElementDTO">ticketTemplateElementDTO object passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        /// <returns>Returns updated TicketTemplateElementDTO</returns>
        public TicketTemplateElementDTO Update(TicketTemplateElementDTO ticketTemplateElementDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketTemplateElementDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TicketTemplateElements]
                             SET
                             TicketTemplateId = @TicketTemplateId,
                             UniqueId = @UniqueId,
                             Name = @Name,
                             Value = @Value,
                             Type = @Type,
                             Font = @Font,
                             Location = @Location,
                             site_id = @site_id,
                             Width = @Width,
                             Alignment = @Alignment,
                             Rotate = @Rotate,
                             MasterEntityId = @MasterEntityId,
                             FormatId = @FormatId,
                             Color = @Color,
                             BarCodeHeight = @BarCodeHeight,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = GETDATE(),
                             ActiveFlag = @ActiveFlag
                             WHERE Id = @Id
                            SELECT * FROM TicketTemplateElements WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(ticketTemplateElementDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTicketTemplateElementDTO(ticketTemplateElementDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating TicketTemplateElementDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(ticketTemplateElementDTO);
            return ticketTemplateElementDTO;
        }

        /// <summary>
        /// Returns the List of TicketTemplateElementDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of TicketTemplateElementDTO</returns>
        public List<TicketTemplateElementDTO> GetTicketTemplateElementDTOList(List<KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TicketTemplateElementDTO> ticketTemplateElementDTOList = new List<TicketTemplateElementDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TICKET_TEMPLATE_ELEMENT_ID ||
                            searchParameter.Key == TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TICKET_TEMPLATE_ID ||
                            searchParameter.Key == TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TYPE ||
                            searchParameter.Key == TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.UNIQUE_ID ||
                                searchParameter.Key == TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.LOCATION)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TICKET_TEMPLATE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TicketTemplateElementDTO ticketTemplateElementDTO = GetTicketTemplateElementDTO(dataRow);
                    ticketTemplateElementDTOList.Add(ticketTemplateElementDTO);
                }
            }
            log.LogMethodExit(ticketTemplateElementDTOList);
            return ticketTemplateElementDTOList;
        }
        
    }
}
