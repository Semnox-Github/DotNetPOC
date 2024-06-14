/********************************************************************************************
 * Project Name - Printer Data handler
 * Description  - Data Handler for DB operations
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2018      Mathew NInan   Created
 *2.60        29-Mar-2019      Nagesh Badiger Modified:Changed IsActive data type to bool
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 *2.70.2        10-Dec-2019      Jinto Thomas   Removed siteid from update query
 *2.140.2       18-Apr-2022     Girish Kundar    Modified : Added new column Models to printer
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
    /// Printer Data Handler - Handles insert, update and select of Printer Data
    /// </summary>
    class PrinterDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Printers AS p ";
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Dictionary for searching Parameters for the Printer object.
        /// </summary>
        private static readonly Dictionary<PrinterDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PrinterDTO.SearchByParameters, string>
        {
                {PrinterDTO.SearchByParameters.PRINTERID,"p.PrinterId"},
                {PrinterDTO.SearchByParameters.ISACTIVE, "p.IsActive"},
                {PrinterDTO.SearchByParameters.SITE_ID, "p.site_id"},
                {PrinterDTO.SearchByParameters.IPADDRESS, "p.IPAddress"},
                {PrinterDTO.SearchByParameters.PRINTERNAME, "p.PrinterName"},
                {PrinterDTO.SearchByParameters.PRINTERTYPEID, "p.PrinterTypeId"},
                {PrinterDTO.SearchByParameters.PRINTERLOCATION, "p.PrinterLocation"},
                {PrinterDTO.SearchByParameters.MASTER_ENTIY_ID, "p.MasterEntityId"}
        };
        private Utilities utilities;

        /// <summary>
        /// Default constructor of PrintersDataHandler class
        /// </summary>
        public PrinterDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with sqlTransaction as a parameter 
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public PrinterDataHandler(SqlTransaction sqlTransaction)
            : this()
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PrinterDTO Record.
        /// </summary>
        /// <param name="PrinterDTO">PrinterDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PrinterDTO printerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(printerDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@printerId", printerDTO.PrinterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printerName", string.IsNullOrEmpty(printerDTO.PrinterName) ? DBNull.Value : (object)printerDTO.PrinterName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printerLocation", string.IsNullOrEmpty(printerDTO.PrinterLocation) ? DBNull.Value : (object)printerDTO.PrinterLocation));
            parameters.Add(dataAccessHandler.GetSQLParameter("@iPAddress", string.IsNullOrEmpty(printerDTO.IpAddress) ? DBNull.Value : (object)printerDTO.IpAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(printerDTO.Remarks) ? DBNull.Value : (object)printerDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@kDSTerminal", printerDTO.KDSTerminal == -1 ? DBNull.Value : (object)printerDTO.KDSTerminal));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printerTypeId", printerDTO.PrinterTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", printerDTO.IsActive == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", printerDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@model", printerDTO.WBPrinterModel, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaperSizeId", printerDTO.PaperSizeId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Printer record to the database
        /// </summary>
        /// <param name="printersDto">PrinterDTO</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns DTO</returns>
        public PrinterDTO InsertPrinters(PrinterDTO printersDto, string loginId, int siteId)
        {
            log.LogMethodEntry(printersDto, loginId, siteId);
            string query = @"INSERT INTO[dbo].[Printers]
                                                       ( PrinterName,
                                                         PrinterLocation,
                                                         IPAddress,
                                                         Remarks,
                                                         KDSTerminal,
                                                         PrinterTypeId,
                                                         IsActive,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdateDate,
                                                         LastUpdatedBy,
                                                         GUID,
                                                         site_id,
                                                         MasterEntityId,
                                                         WBPrinterModel,
                                                         PaperSizeId
                                                       )
                                               values(
                                                         @printerName,
                                                         @printerLocation,
                                                         @iPAddress,
                                                         @remarks,
                                                         @kDSTerminal,
                                                         @printerTypeId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),
                                                         Getdate(),
                                                         @lastUpdatedBy,
                                                         NewId(),
                                                         @siteId,
                                                         @masterEntityId, @model, @PaperSizeId)
                           SELECT * FROM Printers WHERE PrinterId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(printersDto, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPrintersDto(printersDto, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting printersDto", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(printersDto);
            return printersDto;
        }

        /// <summary>
        /// Updates the Printer record to the database
        /// </summary>
        /// <param name="printersDTO">PrinterDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns DTO</returns>
        public PrinterDTO UpdatePrinters(PrinterDTO printersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(printersDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[Printers]
                                    SET
                                                  PrinterName = @printerName,
                                                  PrinterLocation = @printerLocation,
                                                  IPAddress =  @iPAddress,
                                                  Remarks= @remarks,
                                                  KDSTerminal= @kDSTerminal,
                                                  PrinterTypeId=  @printerTypeId,
                                                  IsActive = @isActive,
                                                  LastUpdateDate = Getdate(),
                                                  LastUpdatedBy = @lastUpdatedBy, 
                                                  -- site_id = @siteId,
                                                  MasterEntityId =  @masterEntityId,
                                                  WBPrinterModel =  @model,
                                                  PaperSizeId = @PaperSizeId
                                                  where  PrinterId = @printerId
                                     SELECT * FROM Printers WHERE PrinterId = @printerId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(printersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPrintersDto(printersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating printersDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(printersDTO);
            return printersDTO;
        }

        /// <summary>
        /// Converts datatable to DTO
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>PrintersDTO</returns>
        private PrinterDTO GetPrinterDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PrinterDTO PrintersDTO = new PrinterDTO(Convert.ToInt32(dataRow["PrinterId"]),
                                            dataRow["PrinterName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PrinterName"]),
                                            dataRow["PrinterLocation"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PrinterLocation"]),
                                            dataRow["KDSTerminal"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["KDSTerminal"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : dataRow["IsActive"].ToString() == "Y" ? true : false,
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["IPAddress"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["IPAddress"]),
                                            dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                            dataRow["PrinterTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrinterTypeId"]),
                                            PrinterDTO.PrinterTypes.ReceiptPrinter,
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["WBPrinterModel"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["WBPrinterModel"]),
                                            dataRow["PaperSizeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaperSizeId"])
                                            );
            log.LogMethodExit(PrintersDTO);
            return PrintersDTO;
        }

        /// <summary>
        /// Gets the Printer data of passed id 
        /// </summary>
        /// <param name="printerId">Id of Printer is passed as parameter</param>
        /// <returns>Returns Printer</returns>
        public PrinterDTO GetPrinter(int printerId)
        {
            log.LogMethodEntry(printerId);
            PrinterDTO result = null;
            string query = SELECT_QUERY + @" WHERE p.PrinterId= @printerId";
            SqlParameter parameter = new SqlParameter("@printerId", printerId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPrinterDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Delete the record from the Printers database based on PrinterId
        /// </summary>
        /// <param name="printerId">printerId</param>
        /// <returns>id</returns>
        internal int Delete(int printerId)
        {
            log.LogMethodEntry(printerId);
            string query = @"DELETE  
                             FROM Printers
                             WHERE Printers.PrinterId = @printerId";
            SqlParameter parameter = new SqlParameter("@printerId", printerId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="printerDTO">PrinterDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPrintersDto(PrinterDTO printerDTO, DataTable dt)
        {
            log.LogMethodEntry(printerDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                printerDTO.PrinterId = Convert.ToInt32(dt.Rows[0]["PrinterId"]);
                printerDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                printerDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                printerDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                printerDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                printerDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                printerDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PrinterDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of PrinterDTO matching the search criteria</returns>
        public List<PrinterDTO> GetPrinterList(List<KeyValuePair<PrinterDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<PrinterDTO> printerList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectGenericPrinterQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PrinterDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {

                        if (searchParameter.Key == PrinterDTO.SearchByParameters.PRINTERID
                             || searchParameter.Key == PrinterDTO.SearchByParameters.PRINTERTYPEID
                             || searchParameter.Key == PrinterDTO.SearchByParameters.MASTER_ENTIY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PrinterDTO.SearchByParameters.PRINTERNAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == PrinterDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == PrinterDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                selectGenericPrinterQuery = selectGenericPrinterQuery + query;
            }
            DataTable printerData = dataAccessHandler.executeSelectQuery(selectGenericPrinterQuery, parameters.ToArray(), sqlTransaction);
            if (printerData.Rows.Count > 0)
            {
                printerList = new List<PrinterDTO>();
                foreach (DataRow printerDataRow in printerData.Rows)
                {
                    PrinterDTO printerDataObject = GetPrinterDTO(printerDataRow);
                    printerList.Add(printerDataObject);
                }
            }
            log.LogMethodExit(printerList);
            return printerList;
        }
    }
}

