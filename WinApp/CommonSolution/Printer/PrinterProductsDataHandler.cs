/********************************************************************************************
 * Project Name - Printer Products Datahandler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2018      Mathew Ninan   Created
 *2.60        20-May-2019      Mushahid Faizan   Added IsActive search filter in GetPrinterProductsList().
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 *2.70.2        10-Dec-2019      Jinto Thomas   Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// PrinterProducts Data Handler - Handles insert, update and select of PrinterProducts Data
    /// </summary>
    class PrinterProductsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PrinterProducts AS pp ";
        private SqlTransaction sqlTransaction;
        
        /// <summary>
        /// Dictionary for searching Parameters for the PrinterProducts object.
        /// </summary>
        private static readonly Dictionary<PrinterProductsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PrinterProductsDTO.SearchByParameters, string>
        {
                {PrinterProductsDTO.SearchByParameters.PRINTER_PRODUCT_ID, "pp.PrinterProductId"},
                {PrinterProductsDTO.SearchByParameters.ISACTIVE, "pp.IsActive"},
                {PrinterProductsDTO.SearchByParameters.SITE_ID, "pp.site_id"},
                {PrinterProductsDTO.SearchByParameters.PRINTERID, "pp.PrinterId" },
                {PrinterProductsDTO.SearchByParameters.MASTER_ENTITY_ID, "pp.MasterEntityId" },
                {PrinterProductsDTO.SearchByParameters.PRODUCTID, "pp.ProductId" }

        };
        private Utilities utilities;

        /// <summary>
        /// Parameterized constructor of PrinterProductsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public PrinterProductsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PrinterProductsDataHandler Record.
        /// </summary>
        /// <param name="PrinterProductsDTO">PrinterProductsDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PrinterProductsDTO printerProductsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(printerProductsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PrinterProductId", printerProductsDTO.PrinterProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printerId", printerProductsDTO.PrinterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", printerProductsDTO.ProductId, true));;
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", printerProductsDTO.IsActive == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", printerProductsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the PrinterDisplayGroup record to the database
        /// </summary>
        /// <param name="printerProductsDto">printerProductsDto</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns DTO</returns>
        public PrinterProductsDTO InsertPrinterProducts(PrinterProductsDTO printerProductsDto, string loginId, int siteId)
        {
            log.LogMethodEntry(printerProductsDto, loginId, siteId);
            string query = @"INSERT INTO[dbo].[PrinterProducts]
                                                       (  PrinterId,
														  ProductId,
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
                                                          @productId,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          @lastUpdatedBy,
                                                          Getdate(),
                                                          NewId(),
                                                          @siteId,
                                                          @masterEntityId)

                                SELECT * FROM PrinterProducts WHERE PrinterProductId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(printerProductsDto, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPrinterProductsDTO(printerProductsDto, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting printerProductsDto", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(printerProductsDto);
            return printerProductsDto;
        }

        /// <summary>
        /// Updates the PrinterDisplayGroup record to the database
        /// </summary>
        /// <param name="printerProductsDto">printerProductsDto</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns DTO</returns>
        public PrinterProductsDTO UpdatePrinterProducts(PrinterProductsDTO printerProductsDto, string loginId, int siteId)
        {
            log.LogMethodEntry(printerProductsDto, loginId, siteId);
            string query = @"UPDATE  [dbo].[PrinterProducts]
                                    SET                PrinterId = @printerId,
                                                       ProductId = @productId,
                                                       IsActive=@isActive,
                                                       LastUpdatedBy = @lastUpdatedBy, 
                                                       LastUpdateDate = Getdate(),
                                                       -- site_id = @siteId,
                                                       MasterEntityId =  @masterEntityId
                                                       where PrinterProductId = @printerProductId
                                            SELECT * FROM PrinterProducts WHERE PrinterProductId = @printerProductId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(printerProductsDto, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPrinterProductsDTO(printerProductsDto, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating printerProductsDto", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(printerProductsDto);
            return printerProductsDto;
        }

        /// <summary>
        /// Delete the record from the PrinterProduct database based on PrinterProductId
        /// </summary>
        /// <param name="printerProductId">printerProductId</param>
        /// <returns>return the int </returns>
        internal int Delete(int printerProductId)
        {
            log.LogMethodEntry(printerProductId);
            string query = @"DELETE  
                             FROM PrinterProducts
                             WHERE PrinterProducts.PrinterProductId = @printerProductId";
            SqlParameter parameter = new SqlParameter("@printerProductId", printerProductId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="printerProductsDTO">PrinterProductsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPrinterProductsDTO(PrinterProductsDTO printerProductsDto, DataTable dt)
        {
            log.LogMethodEntry(printerProductsDto, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                printerProductsDto.PrinterProductId = Convert.ToInt32(dt.Rows[0]["PrinterProductId"]);
                printerProductsDto.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                printerProductsDto.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                printerProductsDto.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                printerProductsDto.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                printerProductsDto.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                printerProductsDto.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PrinterProducts DTO
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DTO</returns>
        private PrinterProductsDTO GetPrinterProductsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PrinterProductsDTO printerProductsDTO = new PrinterProductsDTO(Convert.ToInt32(dataRow["PrinterProductId"]),
                                            dataRow["PrinterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrinterId"]),
                                            dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
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
            log.LogMethodExit(printerProductsDTO);
            return printerProductsDTO;
        }

        /// <summary>
        /// Gets the Printer Product data of passed PrinterProductId
        /// </summary>
        /// <param name="printerProductId">integer type parameter</param>
        /// <returns>Returns PrinterProductDTO</returns>
        public PrinterProductsDTO GetPrinterProducts(int printerProductId)
        {
            log.LogMethodEntry(printerProductId);
            PrinterProductsDTO result = null;
            string query = SELECT_QUERY + @" WHERE pp.PrinterProductId= @printerProductId";
            SqlParameter parameter = new SqlParameter("@printerProductId", printerProductId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPrinterProductsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the PrinterProductsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of PrinterProductsDTO matching the search criteria</returns>
        public List<PrinterProductsDTO> GetPrinterProductsList(List<KeyValuePair<PrinterProductsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<PrinterProductsDTO> printerProductsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectGenericPrinterProductsQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PrinterProductsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        
                        if (searchParameter.Key == PrinterProductsDTO.SearchByParameters.PRINTER_PRODUCT_ID 
                             || searchParameter.Key == PrinterProductsDTO.SearchByParameters.PRODUCTID 
                             || searchParameter.Key == PrinterProductsDTO.SearchByParameters.PRINTERID
                             || searchParameter.Key == PrinterProductsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(PrinterProductsDTO.SearchByParameters.ISACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == PrinterProductsDTO.SearchByParameters.SITE_ID)
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
                selectGenericPrinterProductsQuery = selectGenericPrinterProductsQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectGenericPrinterProductsQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                printerProductsDTOList = new List<PrinterProductsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PrinterProductsDTO printerProductsDTO = GetPrinterProductsDTO(dataRow);
                    printerProductsDTOList.Add(printerProductsDTO);
                }
            }
            log.LogMethodExit(printerProductsDTOList);
            return printerProductsDTOList;
        }
    }
}
