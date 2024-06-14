/********************************************************************************************
 * Project Name - PrinterDataHandler
 * Description  - Data Handler class for printerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        11-Sep-2017      Vinayaka V     Created 
 *2.00        18-Sep-2018      Mathew Ninan   3 tier logic added
 *2.60        04-Feb-2019      Mushahid Faizan Modified -- Remove isActive SqlParameter As it creates duplication error
 *                                                         while Inserting & Updating Record to the DB.
 *2.70        09-Jul-2019      Deeksha        Modified:Added GetSqlParameter(),SQL injection issue Fix
 *                                            changed log.debug to log.logMethodEntry and log.logMethodExit
 *            16-Jul-2019      Akshay G       Added DeletePOSPrinters() method
 *2.70.2      10-Dec-2019      Jinto Thomas   Removed siteid from update query
 *2.70.3      11-Feb-2020      Deeksha        Invariant culture-Font Issue Fix
 *2.80        10-May-2020      Girish Kundar  Modified: REST API Changes merge from WMS  
 *2.110        11-Dec-2020      Dakshakh Raj   Modified: for Peru Invoice Enhancement  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Drawing;
using Semnox.Core.Utilities;
using Semnox.Parafait.DisplayGroup;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// PrinterDataHandler class to get all the details about printers configured with system
    /// </summary>
    public class POSPrinterDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM POSPrinters AS posp ";
        /// <summary>
        /// Dictionary for searching Parameters for the AchievementClass object.
        /// </summary>
        private static readonly Dictionary<POSPrinterDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<POSPrinterDTO.SearchByParameters, string>
        {
                {POSPrinterDTO.SearchByParameters.PRINTER_ID,"posp.PrinterId"},
                {POSPrinterDTO.SearchByParameters.IS_ACTIVE, "posp.IsActive"},
                {POSPrinterDTO.SearchByParameters.POS_MACHINE_ID, "posp.POSMachineId"},
                {POSPrinterDTO.SearchByParameters.POS_MACHINE_ID_LIST, "posp.POSMachineId"},
                {POSPrinterDTO.SearchByParameters.SITE_ID, "posp.site_id"},
                {POSPrinterDTO.SearchByParameters.POS_PRINTER_ID, "posp.POSPrinterId"},
                {POSPrinterDTO.SearchByParameters.PRINT_TEMPLATE_ID, "posp.PrintTemplateId"},
                {POSPrinterDTO.SearchByParameters.SECONDARY_PRINTER_ID, "posp.SecondaryPrinterId"},
                {POSPrinterDTO.SearchByParameters.POS_TYPE_ID, "posp.POSTypeId"},
                {POSPrinterDTO.SearchByParameters.MASTER_ENTITY_ID, "posp.MasterEntityId"}
        };
        Utilities utilities;
        /// <summary>
        /// Default constructor of PrinterDataHandler class
        /// </summary>
        public POSPrinterDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating POSPrinter Record.
        /// </summary>
        /// <param name="pOSPrinterDTO">POSPrinterDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(POSPrinterDTO pOSPrinterDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrinterDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@posPrinterId", pOSPrinterDTO.POSPrinterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posMachineId", pOSPrinterDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printerId", pOSPrinterDTO.PrinterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posTypeId", pOSPrinterDTO.POSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printerTypeId", pOSPrinterDTO.PrinterTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@secondaryPrinterId", pOSPrinterDTO.SecondaryPrinterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@orderTypeGroupId", pOSPrinterDTO.OrderTypeGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printTemplateId", pOSPrinterDTO.PrintTemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", pOSPrinterDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", pOSPrinterDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the POSPrinter record to the database
        /// </summary>
        public POSPrinterDTO InsertPOSPrinter(POSPrinterDTO pOSPrinterDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrinterDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[POSPrinters]
                                                       ( POSMachineId,
                                                         PrinterId,
                                                         POSTypeId,
                                                         SecondaryPrinterId,
                                                         PrinterTypeID,
                                                         OrderTypeGroupId,
                                                         PrintTemplateId,
                                                         IsActive,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdateDate,
                                                         LastUpdatedBy,
                                                         GUID,
                                                         site_id,
                                                         MasterEntityId )
                                               values(
                                                         @posMachineId,
                                                         @printerId,
                                                         @posTypeId,
                                                         @secondaryPrinterId,
                                                         @printerTypeId,
                                                         @orderTypeGroupId,
                                                         @printTemplateId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),
                                                         Getdate(),
                                                         @lastUpdatedBy,
                                                         newId(),
                                                         @site_id,
                                                         @masterEntityId)
                             SELECT* FROM POSPrinters WHERE POSPrinterId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSPrinterDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPrinterDTO(pOSPrinterDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting POSPrinterDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSPrinterDTO);
            return pOSPrinterDTO;
        }


        /// <summary>
        /// Updates the Printer record to the database
        /// </summary>
        public POSPrinterDTO UpdatePOSPrinter(POSPrinterDTO pOSPrinterDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrinterDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[POSPrinters]
                           SET 
                                                  POSMachineId = @posMachineId,
                                                  PrinterId =  @printerId,
                                                  POSTypeId = @posTypeId,
                                                  SecondaryPrinterId = @secondaryPrinterId,
                                                  OrderTypeGroupId =  @orderTypeGroupId,
                                                  PrintTemplateId = @printTemplateId,
                                                  IsActive = @isActive,
                                                  LastUpdateDate = Getdate(),
                                                  LastUpdatedBy = @lastUpdatedBy, 
                                                  --site_id = @site_id,
                                                  MasterEntityId =  @masterEntityId,
                                                  PrinterTypeId = @printerTypeId
                                                  WHERE POSPrinterId =@posPrinterId 
                                                    SELECT * FROM POSPrinters WHERE POSPrinterId = @posPrinterId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSPrinterDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPrinterDTO(pOSPrinterDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating POSPrinterDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSPrinterDTO);
            return pOSPrinterDTO;
        }
        /// <summary>
        /// Converts datatable to DTO
        /// </summary>
        private POSPrinterDTO GetPOSPrinterDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            POSPrinterDTO POSPrinterDTO = new POSPrinterDTO(Convert.ToInt32(dataRow["POSPrinterId"]),
                                            dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                            dataRow["PrinterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrinterId"]),
                                            dataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSTypeId"]),
                                            dataRow["SecondaryPrinterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SecondaryPrinterId"]),

                                            dataRow["OrderTypeGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderTypeGroupId"]),
                                            dataRow["PrintTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrintTemplateId"]),
                                            null,//printerDTO
                                            null,//SecondaryPrinterDTO
                                            null,//receiptPrintTemplateHeaderDTO
                                            dataRow["IsActive"] == DBNull.Value ? true : (dataRow["IsActive"].ToString() == "Y" ? true : false),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["PrinterTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrinterTypeId"])

                                            );
            log.LogMethodExit(POSPrinterDTO);
            return POSPrinterDTO;
        }

        /// <summary>
        /// Gets the POS Printer data of passed POSPrinterId
        /// </summary>
        /// <param name="POSPrinterId">integer type parameter</param>
        /// <returns>Returns PrinterDTO</returns>
        public POSPrinterDTO GetPOSPrinter(int posPrinterId)
        {
            log.LogMethodEntry(posPrinterId);
            POSPrinterDTO result = null;
            string query = SELECT_QUERY + @" WHERE posp.POSPrinterId= @posPrinterId";
            SqlParameter parameter = new SqlParameter("@posPrinterId", posPrinterId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPOSPrinterDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<POSPrinterDTO> GetPOSPrinterDTOList(List<int> pOSMachineIdList, bool activeRecords)
        {
            log.LogMethodEntry(pOSMachineIdList);
            List<POSPrinterDTO> pOSPrinterDTOList = new List<POSPrinterDTO>();
            string query = @"SELECT *
                            FROM POSPrinters, @POSMachineIdList List
                            WHERE POSMachineId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@POSMachineIdList", pOSMachineIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                pOSPrinterDTOList = table.Rows.Cast<DataRow>().Select(x => GetPOSPrinterDTO(x)).ToList();
            }
            log.LogMethodExit(pOSPrinterDTOList);
            return pOSPrinterDTOList;
        }
        /// <summary>
        /// Delete the record from the POSPrinter database based on POSPrinterId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int posPrinterId)
        {
            log.LogMethodEntry(posPrinterId);
            string query = @"
                             Delete FROM POSPrinterOverrideRules
                             where POSPrinterId = @posPrinterId;
                             DELETE  
                             FROM POSPrinters
                             WHERE POSPrinters.POSPrinterId = @posPrinterId";
            SqlParameter parameter = new SqlParameter("@posPrinterId", posPrinterId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="achievementClassDTO">AchievementClassDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPOSPrinterDTO(POSPrinterDTO pOSPrinterDTO, DataTable dt)
        {
            log.LogMethodEntry(pOSPrinterDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                pOSPrinterDTO.POSPrinterId = Convert.ToInt32(dt.Rows[0]["POSPrinterId"]);
                pOSPrinterDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                pOSPrinterDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                pOSPrinterDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                pOSPrinterDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                pOSPrinterDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                pOSPrinterDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the POSPrinterDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of POSPrinterDTO matching the search criteria</returns>
        public List<POSPrinterDTO> GetPOSPrinterList(List<KeyValuePair<POSPrinterDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            string selectPOSPrinterQuery = SELECT_QUERY;
            List<POSPrinterDTO> posPrinterDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<POSPrinterDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == POSPrinterDTO.SearchByParameters.PRINTER_ID
                             || searchParameter.Key == POSPrinterDTO.SearchByParameters.POS_PRINTER_ID
                             || searchParameter.Key == POSPrinterDTO.SearchByParameters.POS_MACHINE_ID
                             || searchParameter.Key == POSPrinterDTO.SearchByParameters.PRINT_TEMPLATE_ID
                             || searchParameter.Key == POSPrinterDTO.SearchByParameters.MASTER_ENTITY_ID
                             || searchParameter.Key == POSPrinterDTO.SearchByParameters.POS_TYPE_ID
                             || searchParameter.Key == POSPrinterDTO.SearchByParameters.SECONDARY_PRINTER_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSPrinterDTO.SearchByParameters.PRINTER_NAME ||
                                 searchParameter.Key == POSPrinterDTO.SearchByParameters.PRINTER_LOCATION) //string
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSPrinterDTO.SearchByParameters.POS_MACHINE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSPrinterDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSPrinterDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                if (searchParameters.Count > 0)
                    selectPOSPrinterQuery = selectPOSPrinterQuery + query;
                selectPOSPrinterQuery = selectPOSPrinterQuery + " Order by POSPrinterId";
            }


            DataTable POSPrinterData = dataAccessHandler.executeSelectQuery(selectPOSPrinterQuery, parameters.ToArray(), sqlTransaction);
            if (POSPrinterData.Rows.Count > 0)
            {
                posPrinterDTOList = new List<POSPrinterDTO>();
                foreach (DataRow dataRow in POSPrinterData.Rows)
                {
                    POSPrinterDTO posPrinterObject = GetPOSPrinterDTO(dataRow);
                    posPrinterDTOList.Add(posPrinterObject);
                }
            }
            log.LogMethodExit(posPrinterDTOList);
            return posPrinterDTOList;
        }

        /// <summary>
        /// method to get all configured printers
        /// </summary>
        /// <param name="POSMachineId">POS machine id</param>
        /// <param name="siteId">site id</param>
        /// <returns> List of PrinterDTO </returns>
        public List<POSPrinterDTO> getPrinterList(int POSMachineId, int siteId)
        {
            log.LogMethodEntry(POSMachineId, siteId);
            List<POSPrinterDTO> POSPrinters = new List<POSPrinterDTO>();
            int RECEIPT_PRINT_TEMPLATE_ID;
            try
            {
                List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "RECEIPT_PRINT_TEMPLATE"));
                ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(null);
                List<ParafaitDefaultsDTO> parafaitDefaultsDTO = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParameters);

                if (parafaitDefaultsDTO != null && parafaitDefaultsDTO.Any())
                    RECEIPT_PRINT_TEMPLATE_ID = parafaitDefaultsDTO.ElementAt(0).DefaultValueId;
                else
                    RECEIPT_PRINT_TEMPLATE_ID = -1;
            }
            catch
            {
                RECEIPT_PRINT_TEMPLATE_ID = -1;
            }
            String fetchAllPosPrintersQuery = @"select pp.POSPrinterId, pr.PrinterName, pr.PrinterId, 
                                                        isnull(isnull(IPAddress, pr.PrinterLocation), pr.PrinterName) PrinterLocation, 
                                                        pp.PrintTemplateId, isnull(KOTPrinter, 'N') KOTPrinter,
                                                        isnull(KDSTerminal, 0) KDSTerminal, 
                                                        isnull(TicketPrinter, 0) TicketPrinter,
                                                        SecondaryPrinterId
                                                      from POSPrinters pp, Printers pr " +
                                                      "where POSMachineId = @POSMachineId and pr.PrinterId = pp.PrinterId order by pr.PrinterName";
            SqlParameter[] queryParams = new SqlParameter[1];
            queryParams[0] = new SqlParameter("@POSMachineId", POSMachineId.ToString());
            DataTable dt = dataAccessHandler.executeSelectQuery(fetchAllPosPrintersQuery, queryParams, sqlTransaction);
            if (dt.Rows.Count == 0) // no printers defined
            {
                POSPrinters = new List<POSPrinterDTO>();
                POSPrinters.Add(getReceiptTemplate(RECEIPT_PRINT_TEMPLATE_ID));
                POSPrinters.ElementAt(0).PrintOnlyTheseProducts = new DataTable();
            }
            else
            {
                POSPrinters = new List<POSPrinterDTO>();
                string CommandText = "select productId from POSPrintProducts pp where pp.POSPrinterId = @POSPrinterId";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //POSPrinters[i]
                    POSPrinterDTO printer = getReceiptTemplate(dt.Rows[i]["PrintTemplateId"] == DBNull.Value ? RECEIPT_PRINT_TEMPLATE_ID : Convert.ToInt32(dt.Rows[i]["PrintTemplateId"]));
                    printer.PrinterName = dt.Rows[i]["PrinterName"].ToString();
                    printer.PrinterLocation = dt.Rows[i]["PrinterLocation"].ToString();
                    printer.KOTPrinter = dt.Rows[i]["KOTPrinter"].ToString();
                    printer.KDSTerminal = Convert.ToBoolean(dt.Rows[i]["KDSTerminal"]);
                    printer.TicketPrinter = Convert.ToBoolean(dt.Rows[i]["TicketPrinter"]);
                    printer.PrinterId = Convert.ToInt32(dt.Rows[i]["PrinterId"]);

                    SqlParameter[] productQueryParams = new SqlParameter[1];
                    productQueryParams[0] = new SqlParameter("@POSPrinterId", dt.Rows[i]["POSPrinterId"]);

                    DataTable dtProducts = dataAccessHandler.executeSelectQuery(CommandText, productQueryParams, sqlTransaction);
                    #region Added code to add products to print based on selected displaygroup
                    // Start Modification on 9-Dec-2016 for adding the product details to printProduct Table
                    try
                    {
                        //Get Included Displaygroup to print
                        POSPrintDisplayGroupList posPrintDisplayGroup = new POSPrintDisplayGroupList();
                        List<KeyValuePair<POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters, string>> SearchParams = new List<KeyValuePair<POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters, string>>();
                        SearchParams.Add(new KeyValuePair<POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters, string>(POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.POS_PRINTER_ID, dt.Rows[i]["POSPrinterId"].ToString()));
                        List<POSPrintDisplayGroupDTO> POSPrintDisplayGroupList = posPrintDisplayGroup.GetAllPOSPrintDisplayGroup(SearchParams, sqlTransaction);

                        SqlParameter[] productsQueryParams = new SqlParameter[1];
                        productsQueryParams[0] = new SqlParameter("@POSPrinterId", dt.Rows[i]["POSPrinterId"]);

                        //Get included product's displaygroup
                        DataTable IncludeProdDisplayGroupDt = dataAccessHandler.executeSelectQuery(@"SELECT ppp.ProductId, pdg.DisplayGroupId from POSPrintProducts ppp inner join ProductsDisplayGroup pdg on pdg.ProductId = ppp.ProductId
                                                                                            WHERE ppp.POSPrinterId = @POSPrinterId ", productsQueryParams, sqlTransaction);
                        //Check the included displaygroup
                        if (POSPrintDisplayGroupList != null && POSPrintDisplayGroupList.Count > 0)
                        {
                            //Check the included product's displaygroup
                            if (IncludeProdDisplayGroupDt != null && IncludeProdDisplayGroupDt.Rows.Count > 0)
                            {
                                for (int k = 0; k < POSPrintDisplayGroupList.Count; k++)
                                {
                                    bool found = false;
                                    for (int m = 0; m < IncludeProdDisplayGroupDt.Rows.Count; m++)
                                    {
                                        if (POSPrintDisplayGroupList[k].DisplayGroupId.ToString() == IncludeProdDisplayGroupDt.Rows[m][1].ToString())
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                    {
                                        //adding the product's details for not found display group 
                                        DataTable productsDT = GetDisplayGroupProducts(POSPrintDisplayGroupList[k].DisplayGroupId, POSMachineId, siteId);
                                        foreach (DataRow rw in productsDT.Rows)
                                        {
                                            //Check product is exist in dtProducts, if not add
                                            bool productExist = dtProducts.AsEnumerable().Where(c => c.Field<int>("ProductId").Equals(rw[0])).Count() > 0;
                                            if (!productExist)
                                            {
                                                dtProducts.Rows.Add(rw[0]);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Adding the products details for included display group
                                for (int n = 0; n < POSPrintDisplayGroupList.Count; n++)
                                {
                                    DataTable productsDT = GetDisplayGroupProducts(POSPrintDisplayGroupList[n].DisplayGroupId, POSMachineId, siteId);
                                    foreach (DataRow rw in productsDT.Rows)
                                    {
                                        //Check product is exist in dtProducts, if not add
                                        bool productExist = dtProducts.AsEnumerable().Where(c => c.Field<int>("ProductId").Equals(rw[0])).Count() > 0;
                                        if (!productExist)
                                        {
                                            dtProducts.Rows.Add(rw[0]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                    //End Modification on 9-Dec-2016 for adding the product details to printProduct Table
                    #endregion
                    printer.PrintOnlyTheseProducts = dtProducts;

                    if (dt.Rows[i]["SecondaryPrinterId"] != DBNull.Value)
                    {
                        POSPrinterDTO secondaryPrinter = new POSPrinterDTO();
                        secondaryPrinter.KDSTerminal = false;
                        secondaryPrinter.KOTPrinter = "N";
                        secondaryPrinter.PrinterId = Convert.ToInt32(dt.Rows[i]["SecondaryPrinterId"]);
                        secondaryPrinter.PrintOnlyTheseProducts = POSPrinters[i].PrintOnlyTheseProducts;
                        secondaryPrinter.ReceiptTemplate = POSPrinters[i].ReceiptTemplate;
                        secondaryPrinter.PrintTemplateId = POSPrinters[i].PrintTemplateId;
                        secondaryPrinter.TicketPrinter = false;
                        SqlParameter[] secondaryPrinterQueryParams = new SqlParameter[1];
                        secondaryPrinterQueryParams[0] = new SqlParameter("@printerId", secondaryPrinter.PrinterId);
                        secondaryPrinter.PrinterLocation = dataAccessHandler.executeSelectQuery(@"select isnull(isnull(IPAddress, pr.PrinterLocation), pr.PrinterName) PrinterLocation from Printers pr where PrinterId = @printerId",
                                                                                    secondaryPrinterQueryParams).ToString();

                        SqlParameter[] secondaryPrinterQueryParams1 = new SqlParameter[1];
                        secondaryPrinterQueryParams1[0] = new SqlParameter("@printerId", secondaryPrinter.PrinterId);
                        secondaryPrinter.PrinterName = dataAccessHandler.executeSelectQuery(@"select pr.PrinterName from Printers pr where PrinterId = @printerId",
                                                                                    secondaryPrinterQueryParams1).ToString();

                        printer.SecondaryPrinter = secondaryPrinter;
                    }

                    POSPrinters.Add(printer);
                }
            }
            log.LogMethodExit(POSPrinters);

            return POSPrinters;
        }

        /// <summary>
        /// GetDisplayGroupProducts
        /// </summary>
        /// <param name="displaygroupId">display group id</param>
        /// <param name="POSMachineId">pos machine id</param>
        /// <param name="siteId">site id</param>
        /// <returns>data table of products</returns>
        public DataTable GetDisplayGroupProducts(int displaygroupId, int POSMachineId, int siteId)
        {
            log.LogMethodEntry(displaygroupId, POSMachineId, siteId);
            string getDisplayGroupProductsQuery = @"select product_id from products p
                                left outer join POSMachines pos 
                                on pos.POSTypeId = p.POSTypeId
                                inner join ProductsDisplayGroup pdg
								on pdg.ProductId = p.product_id,
                                product_type pt 
                                where p.product_type_id = pt.product_type_id 
                                and (pos.POSMachineId = @PosMachineId or p.POSTypeId is null)
                                and p.active_flag = 'Y' 
                                and (p.site_id = @site_id or @site_id = -1)   
                                and pdg.DisplayGroupId = @displayGroupId";

            List<SqlParameter> getDisplayGroupProductsQueryParameters = new List<SqlParameter>();
            getDisplayGroupProductsQueryParameters.Add(new SqlParameter("@displayGroupId", displaygroupId));
            getDisplayGroupProductsQueryParameters.Add(new SqlParameter("@PosMachineId", POSMachineId));
            getDisplayGroupProductsQueryParameters.Add(new SqlParameter("@site_id", siteId));
            DataTable productsDT = dataAccessHandler.executeSelectQuery(getDisplayGroupProductsQuery, getDisplayGroupProductsQueryParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(productsDT);
            return productsDT;
        }

        /// <summary>
        /// Based on the posPrinterId, appropriate POSPrinters record will be deleted
        /// </summary>
        /// <param name="posPrinterId">posPrinterId</param>
        /// <returns>return the int </returns>
        public int DeletePOSPrinters(int posPrinterId)
        {
            log.LogMethodEntry(posPrinterId);
            try
            {
                string deleteQuery = @"delete from POSPrinters where POSPrinterId = @posPrinterId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@posPrinterId", posPrinterId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// getReceiptTemplate
        /// </summary>
        /// <param name="TemplateId">teplate id</param>
        /// <returns>returns PrinterDTO</returns>
        public POSPrinterDTO getReceiptTemplate(int TemplateId)
        {
            log.LogMethodEntry(TemplateId);
            String fetchQuery = @"select rpt.*, " +
                                    "rpth.FontName MainFontName, " +
                                    "rpt.FontName LineFontName, " +
                                    "rpth.FontSize MainFontSize, " +
                                    "rpt.FontSize LineFontSize " +
                                "from receiptprinttemplate rpt, " +
                                      "receiptprinttemplateHeader rpth " +
                                "where (col1alignment != 'H' " +
                                        "or col2alignment != 'H' " +
                                        "or col3alignment != 'H' " +
                                        "or col4alignment != 'H' " +
                                        "or col5alignment != 'H') " +
                                "and rpth.templateId = @templateId " +
                                "and rpth.templateId = rpt.templateId " +
                                "order by case Section when 'HEADER' then 1  " +
                                                      "when 'PRODUCT' then 2 " +
                                                      "when 'CARDINFO' then 3 " +
                                                      "when 'TRANSACTIONTOTAL' then 4 " +
                                                      "when 'DISCOUNTS' then 5 " +
                                                      "when 'DISCOUNTTOTAL' then 6 " +
                                                      "when 'TAXLINE' then 7 " +
                                                      "when 'TAXTOTAL' then 8 " +
                                                      "when 'GRANDTOTAL' then 9 " +
                                                      "when 'FOOTER' then 10 " +
                                                      "when 'ITEMSLIP' then 11 " +
                                                      "else 11 end, " +
                                                      "Sequence ";

            List<SqlParameter> queryParameters = new List<SqlParameter>();
            queryParameters.Add(new SqlParameter("@templateId", TemplateId));

            DataTable dt = dataAccessHandler.executeSelectQuery(fetchQuery, queryParameters.ToArray(), sqlTransaction);

            dt.Columns.Add("Font", Type.GetType("System.Object"));
            Font ReceiptFont;
            Font defaultFont = new Font("Arial Narrow", 9);

            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    string font = dt.Rows[i]["LineFontName"].ToString();
                    if (string.IsNullOrEmpty(font))
                        font = dt.Rows[i]["MainFontName"].ToString();
                    if (string.IsNullOrEmpty(font))
                    {
                        if (dt.Rows[i]["LineFontSize"].Equals(DBNull.Value))
                        {
                            if (dt.Rows[i]["MainFontSize"].Equals(DBNull.Value))
                            {
                                ReceiptFont = defaultFont;
                            }
                            else
                            {
                                ReceiptFont = new Font(defaultFont.FontFamily, (float)Convert.ToDouble(dt.Rows[i]["MainFontSize"]));
                            }
                        }
                        else
                        {
                            ReceiptFont = new Font(defaultFont.FontFamily, (float)Convert.ToDouble(dt.Rows[i]["LineFontSize"]));
                        }
                    }
                    else
                    {
                        ReceiptFont = CustomFontConverter.ConvertStringToFont(utilities.ExecutionContext, font);
                        if (!dt.Rows[i]["LineFontSize"].Equals(DBNull.Value))
                            ReceiptFont = new Font(ReceiptFont.FontFamily, (float)Convert.ToDouble(dt.Rows[i]["LineFontSize"]));
                    }
                }
                catch
                {
                    ReceiptFont = defaultFont;
                }

                dt.Rows[i]["Font"] = ReceiptFont;
            }

            POSPrinterDTO printer = new POSPrinterDTO();
            printer.ReceiptTemplate = dt;
            printer.PrintTemplateId = TemplateId;
            printer.PrinterName = "Default";
            printer.PrinterLocation = "Default";
            printer.KOTPrinter = "N";
            log.LogMethodExit(printer);
            return (printer);
        }
    }
}
