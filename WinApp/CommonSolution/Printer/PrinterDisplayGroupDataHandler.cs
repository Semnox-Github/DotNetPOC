/********************************************************************************************
 * Project Name - Printer DisplayGroup Datahandler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2018      Mathew Ninan   Created
 *2.60        20-May-2019      Mushahid Faizan Modified isActive search filter.
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 *2.70.2        10-Dec-2019      Jinto Thomas   Removed siteid from update query
 *2.140       14-Sep-2021      Fiona            Modified: Issue fix in GetPrinterDisplayGroupList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// PrinterDisplayGroup Data Handler - Handles insert, update and select of PrinterDisplayGroup Data
    /// </summary>
    class PrinterDisplayGroupDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM PrinterDisplayGroup AS pdg ";

        /// <summary>
        ///  Dictionary for searching Parameters for the PrinterDisplayGroup object.
        /// </summary>
        private static readonly Dictionary<PrinterDisplayGroupDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PrinterDisplayGroupDTO.SearchByParameters, string>
        {
                {PrinterDisplayGroupDTO.SearchByParameters.PRINTER_DISPLAY_GROUP_ID, "pdg.PrinterDisplayGroupId"},
                {PrinterDisplayGroupDTO.SearchByParameters.ISACTIVE, "pdg.IsActive"},
                {PrinterDisplayGroupDTO.SearchByParameters.MASTER_ENTITY_ID, "pdg.MasterEntityId"},
                {PrinterDisplayGroupDTO.SearchByParameters.PRINTER_ID,"pdg.PrinterId"},
                {PrinterDisplayGroupDTO.SearchByParameters.DISPLAY_GROUP_ID,"pdg.DisplayGroupId"},
                {PrinterDisplayGroupDTO.SearchByParameters.SITE_ID, "pdg.site_id"}
        };
        private Utilities utilities;

        /// <summary>
        /// Default constructor of PrinterDisplayGroupDataHandler class
        /// </summary>
        public PrinterDisplayGroupDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with sqlTransaction as a parameter.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public PrinterDisplayGroupDataHandler(SqlTransaction sqlTransaction) 
            : this()
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PrinterDisplayGroupDataHandler Record.
        /// </summary>
        /// <param name="PrinterDisplayGroupDTO">PrinterDisplayGroupDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PrinterDisplayGroupDTO printerDisplayGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(printerDisplayGroupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@printerDisplayGroupId", printerDisplayGroupDTO.PrinterDisplayGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printerId", printerDisplayGroupDTO.PrinterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayGroupId", printerDisplayGroupDTO.DisplayGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", printerDisplayGroupDTO.IsActive == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", printerDisplayGroupDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the PrinterDisplayGroup record to the database
        /// </summary>
        /// <param name="printerDisplayGroupDto">PrinterDisplayGroupDto</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns DTO</returns>
        public PrinterDisplayGroupDTO InsertPrinterDisplayGroup(PrinterDisplayGroupDTO printerDisplayGroupDto, string loginId, int siteId)
        {
            log.LogMethodEntry(printerDisplayGroupDto, loginId, siteId);
            string query = @"INSERT INTO[dbo].[PrinterDisplayGroup]
                                                       (  PrinterId,
														  DisplayGroupId,
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastUpdateDate,
                                                          GUID,
                                                          site_id, 
                                                          MasterEntityId
                                                        )
                                                       values 
                                                        ( @printerId,
                                                          @displayGroupId,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          @lastUpdatedBy,
                                                          Getdate(),
                                                          NewId(),
                                                          @siteId,
                                                          @masterEntityId) 
                                SELECT * FROM PrinterDisplayGroup WHERE PrinterDisplayGroupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(printerDisplayGroupDto, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPrinterDisplayGroupDTO(printerDisplayGroupDto, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting printerDisplayGroupDto", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(printerDisplayGroupDto);
            return printerDisplayGroupDto;
        }

        /// <summary>
        /// Updates the PrinterDisplayGroup record to the database
        /// </summary>
        /// <param name="printerDisplayGroupDto">printerDisplayGroupDto</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns DTO</returns>
        public PrinterDisplayGroupDTO UpdatePrinterDisplayGroup(PrinterDisplayGroupDTO printerDisplayGroupDto, string loginId, int siteId)
        {
            log.LogMethodEntry(printerDisplayGroupDto, loginId, siteId);
            string query = @"UPDATE  [dbo].[PrinterDisplayGroup]
                                    SET                   PrinterId = @printerId,
                                                          DisplayGroupId = @displayGroupId,
                                                          IsActive = @isActive,
                                                          LastUpdatedBy = @lastUpdatedBy, 
                                                          LastUpdateDate = Getdate(),
                                                          -- site_id = @siteId,
                                                          MasterEntityId =  @masterEntityId
                                                          where PrinterDisplayGroupId = @printerDisplayGroupId 
                                        SELECT * FROM PrinterDisplayGroup WHERE printerDisplayGroupId = @printerDisplayGroupId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(printerDisplayGroupDto, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPrinterDisplayGroupDTO(printerDisplayGroupDto, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating printerDisplayGroupDto", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(printerDisplayGroupDto);
            return printerDisplayGroupDto;
        }

        /// <summary>
        /// Delete the record from the printerDisplayGroup database based on printerDisplayGroupId
        /// </summary>
        /// <param name="printerDisplayGroupId">printerDisplayGroupId</param>
        /// <returns>Returns the int</returns>
        internal int Delete(int printerDisplayGroupId)
        {
            log.LogMethodEntry(printerDisplayGroupId);
            string query = @"DELETE  
                             FROM PrinterDisplayGroup
                             WHERE PrinterDisplayGroup.PrinterDisplayGroupId = @printerDisplayGroupId";
            SqlParameter parameter = new SqlParameter("@printerDisplayGroupId", printerDisplayGroupId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="PrinterDisplayGroupDTO">PrinterDisplayGroupDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPrinterDisplayGroupDTO(PrinterDisplayGroupDTO printerDisplayGroupDTO, DataTable dt)
        {
            log.LogMethodEntry(printerDisplayGroupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                printerDisplayGroupDTO.PrinterDisplayGroupId = Convert.ToInt32(dt.Rows[0]["PrinterDisplayGroupId"]);
                printerDisplayGroupDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                printerDisplayGroupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                printerDisplayGroupDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                printerDisplayGroupDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                printerDisplayGroupDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                printerDisplayGroupDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }       

        /// <summary>
        /// Gets PrinterDisplayGroup DTO
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DTO</returns>
        private PrinterDisplayGroupDTO GetPrinterDisplayGroupDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PrinterDisplayGroupDTO posPrintersDTO = new PrinterDisplayGroupDTO(Convert.ToInt32(dataRow["PrinterDisplayGroupId"]),
                                            dataRow["PrinterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrinterId"]),
                                            dataRow["DisplayGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayGroupId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : dataRow["IsActive"].ToString() == "Y" ? true : false,
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(posPrintersDTO);
            return posPrintersDTO;
        }

        /// <summary>
        /// Gets the Printer Display Group data of passed PrinterDisplayGroupId
        /// </summary>
        /// <param name="PrinterDisplayGroupId">integer type parameter</param>
        /// <returns>Returns PrinterDisplayGroupDTO</returns>        
        public PrinterDisplayGroupDTO GetPrinterDisplayGroup(int PrinterDisplayGroupId)
        {
            log.LogMethodEntry(PrinterDisplayGroupId);
            PrinterDisplayGroupDTO result = null;
            string query = SELECT_QUERY + @" WHERE pdg.PrinterDisplayGroupId= @printerDisplayGroupId";
            SqlParameter parameter = new SqlParameter("@printerDisplayGroupId", PrinterDisplayGroupId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPrinterDisplayGroupDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the PrinterDisplayGroup list matching the search key
        /// </summary>
        /// <param name="searchParameters">SearchParameters</param>
        ///  <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>printerDisplayGroupDTOList</returns>
        public List<PrinterDisplayGroupDTO> GetPrinterDisplayGroupList(List<KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PrinterDisplayGroupDTO> printerDisplayGroupDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        
                            if (searchParameter.Key.Equals(PrinterDisplayGroupDTO.SearchByParameters.PRINTER_DISPLAY_GROUP_ID) ||
                                searchParameter.Key.Equals(PrinterDisplayGroupDTO.SearchByParameters.PRINTER_ID) ||
                                searchParameter.Key.Equals(PrinterDisplayGroupDTO.SearchByParameters.MASTER_ENTITY_ID)||
                                searchParameter.Key.Equals(PrinterDisplayGroupDTO.SearchByParameters.DISPLAY_GROUP_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == PrinterDisplayGroupDTO.SearchByParameters.SITE_ID )
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(PrinterDisplayGroupDTO.SearchByParameters.ISACTIVE))
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
                printerDisplayGroupDTOList = new List<PrinterDisplayGroupDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PrinterDisplayGroupDTO printerDisplayGroupDTO = GetPrinterDisplayGroupDTO(dataRow);
                    printerDisplayGroupDTOList.Add(printerDisplayGroupDTO);
                }
            }
            log.LogMethodExit(printerDisplayGroupDTOList);
            return printerDisplayGroupDTOList;
        }
    }
}
