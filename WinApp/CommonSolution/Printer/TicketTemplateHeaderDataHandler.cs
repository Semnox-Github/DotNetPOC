/********************************************************************************************
 * Project Name - Printer
 * Description  - Data Handler File for TicketTemplateHeader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *            01-July-2019   Mushahid Faizan         Modified GetSqlParameters() & RefreshTicketTemplateHeaderDTO() method.
 *2.70.3      02-Apr-2020    Girish Kundar           Modified : Issue fix for Image data  type
 *2.120.3     28-Dec-2021    Girish Kundar           Modified : Issue fix for Wristband duplicate 
 *2.140       14-Sep-2021      Fiona                Modified: Issue fixes in Insert
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// TicketTemplateHeader Data Handler - Handles insert, update and selection of TicketTemplateHeader objects
    /// </summary>
    public class TicketTemplateHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TicketTemplateHeader as tth ";

        /// <summary>
        /// Dictionary for searching Parameters for the TicketTemplateHeader object.
        /// </summary>
        private static readonly Dictionary<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string> DBSearchParameters = new Dictionary<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>
        {
            { TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.TEMPLATE_ID,"tth.TemplateId"},
            { TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.TICKET_TEMPLATE_ID,"tth.TicketTemplateId"},
            { TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.SITE_ID,"tth.site_id"},
            { TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.IS_ACTIVE,"tth.Isactive"},
            { TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.MASTER_ENTITY_ID,"tth.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for TicketTemplateHeader.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public TicketTemplateHeaderDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TicketTemplateHeader Record.
        /// </summary>
        /// <param name="ticketTemplateHeaderDTO">TicketTemplateHeaderDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(TicketTemplateHeaderDTO ticketTemplateHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketTemplateHeaderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketTemplateId", ticketTemplateHeaderDTO.TicketTemplateId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TemplateId", ticketTemplateHeaderDTO.TemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Width", ticketTemplateHeaderDTO.Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Height", ticketTemplateHeaderDTO.Height));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeftMargin", ticketTemplateHeaderDTO.LeftMargin));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RightMargin", ticketTemplateHeaderDTO.RightMargin));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TopMargin", ticketTemplateHeaderDTO.TopMargin));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BottomMargin", ticketTemplateHeaderDTO.BottomMargin));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BorderWidth", ticketTemplateHeaderDTO.BorderWidth));
            SqlParameter bgparameter = new SqlParameter("@BackgroundImage", SqlDbType.VarBinary);
            if (ticketTemplateHeaderDTO.BackgroundImage == null)
            {
                bgparameter.Value = DBNull.Value;
            }
            else
            {
                bgparameter.Value = ticketTemplateHeaderDTO.BackgroundImage;
            }
            parameters.Add(bgparameter);
            //parameters.Add(dataAccessHandler.GetSQLParameter("@BackgroundImage", ticketTemplateHeaderDTO.BackgroundImage == (byte[])null ? (byte[])null : ticketTemplateHeaderDTO.BackgroundImage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BacksideTemplateId", ticketTemplateHeaderDTO.BacksideTemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Isactive", ticketTemplateHeaderDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", ticketTemplateHeaderDTO.MasterEntityId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NotchDistance", ticketTemplateHeaderDTO.NotchDistance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NotchWidth", ticketTemplateHeaderDTO.NotchWidth));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PrintReverse", ticketTemplateHeaderDTO.PrintReverse));
            log.LogMethodExit(parameters);
            return parameters;
        }
        ///<summary>
        /// Converts the Data row object to TicketTemplateHeaderDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of TicketTemplateHeaderDTO</returns>
        private TicketTemplateHeaderDTO GetTicketTemplateHeaderDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TicketTemplateHeaderDTO ticketTemplateHeaderDTO = new TicketTemplateHeaderDTO(dataRow["TicketTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TicketTemplateId"]),
                                                dataRow["TemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TemplateId"]),
                                                dataRow["Width"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Width"]),
                                                dataRow["Height"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Height"]),
                                                dataRow["LeftMargin"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["LeftMargin"]),
                                                dataRow["RightMargin"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["RightMargin"]),
                                                dataRow["TopMargin"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TopMargin"]),
                                                dataRow["BottomMargin"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["BottomMargin"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["BorderWidth"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["BorderWidth"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["BackgroundImage"] == DBNull.Value ? null : (byte[])(dataRow["BackgroundImage"]),
                                                //dataRow["BackgroundImage"] == DBNull.Value ? null : (byte?[])(dataRow["BackgroundImage"]),
                                                dataRow["BacksideTemplateId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["BacksideTemplateId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["Isactive"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Isactive"]),
                                                dataRow["NotchDistance"] == DBNull.Value ? 1 : Convert.ToDecimal(dataRow["NotchDistance"]),
                                                dataRow["NotchWidth"] == DBNull.Value ? 0.25M : Convert.ToDecimal(dataRow["NotchWidth"]),
                                                dataRow["PrintReverse"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["PrintReverse"]));
            return ticketTemplateHeaderDTO;
        }

        /// <summary>
        /// Gets the TicketTemplateHeader data of passed TicketTemplateHeader ID
        /// </summary>
        /// <param name="ticketTemplateHeaderId">ticketTemplateHeaderId is passed as parameter</param>
        /// <returns>Returns TicketTemplateHeaderDTO</returns>
        public TicketTemplateHeaderDTO GetTicketTemplateHeaderDTO(int ticketTemplateHeaderId)
        {
            log.LogMethodEntry(ticketTemplateHeaderId);
            TicketTemplateHeaderDTO result = null;
            string query = SELECT_QUERY + @" WHERE tth.TicketTemplateId = @TicketTemplateId";
            SqlParameter parameter = new SqlParameter("@TicketTemplateId", ticketTemplateHeaderId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTicketTemplateHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the TicketTemplateHeader record
        /// </summary>
        /// <param name="ticketTemplateHeaderDTO">TicketTemplateHeaderDTO is passed as parameter</param>
        internal void Delete(TicketTemplateHeaderDTO ticketTemplateHeaderDTO)
        {
            log.LogMethodEntry(ticketTemplateHeaderDTO);
            string query = @"DELETE  
                             FROM TicketTemplateHeader
                             WHERE TicketTemplateHeader.TicketTemplateId = @TicketTemplateId";
            SqlParameter parameter = new SqlParameter("@TicketTemplateId", ticketTemplateHeaderDTO.TicketTemplateId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            ticketTemplateHeaderDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="ticketTemplateHeaderDTO">TicketTemplateHeaderDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshTicketTemplateHeaderDTO(TicketTemplateHeaderDTO ticketTemplateHeaderDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketTemplateHeaderDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                ticketTemplateHeaderDTO.TicketTemplateId = Convert.ToInt32(dt.Rows[0]["TicketTemplateId"]);
                ticketTemplateHeaderDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                ticketTemplateHeaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                ticketTemplateHeaderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                ticketTemplateHeaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                ticketTemplateHeaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                ticketTemplateHeaderDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the TicketTemplateHeader Table. 
        /// </summary>
        /// <param name="ticketTemplateHeaderDTO">TicketTemplateHeaderDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        /// <returns>Returns updated TicketTemplateHeaderDTO</returns>
        public TicketTemplateHeaderDTO Insert(TicketTemplateHeaderDTO ticketTemplateHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketTemplateHeaderDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[TicketTemplateHeader]
                            (
                            TemplateId,
                            Width,
                            Height,
                            LeftMargin,
                            RightMargin,
                            TopMargin,
                            BottomMargin,
                            LastUpdateDate,
                            LastUpdatedBy,
                            BorderWidth,
                            Guid,
                            site_id,
                            MasterEntityId,
                            BackgroundImage,
                            BacksideTemplateId,
                            CreatedBy,
                            CreationDate,
                            Isactive,
                            NotchDistance,
                            NotchWidth,
                            PrintReverse
                            )
                            VALUES
                            (
                            @TemplateId,
                            @Width,
                            @Height,
                            @LeftMargin,
                            @RightMargin,
                            @TopMargin,
                            @BottomMargin,
                            GETDATE(),
                            @LastUpdatedBy,
                            @BorderWidth,
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @BackgroundImage,
                            @BacksideTemplateId,
                            @CreatedBy,
                            GETDATE(),
                            @Isactive,
                            @NotchDistance,
                            @NotchWidth,
                            @PrintReverse
                            )
                            SELECT * FROM TicketTemplateHeader WHERE TicketTemplateId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(ticketTemplateHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTicketTemplateHeaderDTO(ticketTemplateHeaderDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TicketTemplateHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(ticketTemplateHeaderDTO);
            return ticketTemplateHeaderDTO;
        }

        /// <summary>
        /// Update the record in the TicketTemplateHeader Table. 
        /// </summary>
        /// <param name="ticketTemplateHeaderDTO">TicketTemplateHeaderDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        /// <returns>Returns updated TicketTemplateHeaderDTO</returns>
        public TicketTemplateHeaderDTO Update(TicketTemplateHeaderDTO ticketTemplateHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketTemplateHeaderDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TicketTemplateHeader]
                             SET
                             TemplateId = @TemplateId,
                             Width = @Width,
                             Height = @Height,
                             LeftMargin = @LeftMargin,
                             RightMargin = @RightMargin,
                             TopMargin = @TopMargin,
                             BottomMargin = @BottomMargin,
                             LastUpdateDate = GETDATE(),
                             LastUpdatedBy = @LastUpdatedBy,
                             BorderWidth = @BorderWidth,
                             site_id = @site_id,
                             MasterEntityId = @MasterEntityId,
                             BackgroundImage = @BackgroundImage,
                             BacksideTemplateId = @BacksideTemplateId,
                             NotchDistance = @NotchDistance,
                             NotchWidth = @NotchWidth,
                             PrintReverse= @PrintReverse,
                             Isactive = @Isactive
                             WHERE TicketTemplateId = @TicketTemplateId
                            SELECT * FROM TicketTemplateHeader WHERE TicketTemplateId = @TicketTemplateId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(ticketTemplateHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTicketTemplateHeaderDTO(ticketTemplateHeaderDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating TicketTemplateHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(ticketTemplateHeaderDTO);
            return ticketTemplateHeaderDTO;
        }

        /// <summary>
        /// Returns the List of TicketTemplateHeaderDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of TicketTemplateHeaderDTO </returns>
        public List<TicketTemplateHeaderDTO> GetTicketTemplateHeaderDTOList(List<KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TicketTemplateHeaderDTO> ticketTemplateHeaderDTOList = new List<TicketTemplateHeaderDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.TICKET_TEMPLATE_ID ||
                            searchParameter.Key == TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.TEMPLATE_ID ||
                            searchParameter.Key == TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.IS_ACTIVE)  // bit
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
                    TicketTemplateHeaderDTO ticketTemplateHeaderDTO = GetTicketTemplateHeaderDTO(dataRow);
                    ticketTemplateHeaderDTOList.Add(ticketTemplateHeaderDTO);
                }
            }
            log.LogMethodExit(ticketTemplateHeaderDTOList);
            return ticketTemplateHeaderDTOList;
        }
    }
}
