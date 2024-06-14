/********************************************************************************************
 * Project Name - CheckOutPrices DataHandler 
 * Description  - DataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *2.60        08-Feb-2019   Indrajeet Kumar             Created 
 **********************************************************************************************
 *2.60        26-Mar-2019   Akshay Gulaganji            Modified InsertCheckOutPrices(), UpdateCheckOutPrices() methods, 
 *                                                      added GetSQLParameters(), log.MethodEntry() and log.MethodExit()
 *2.70        07-11-2019    Muhammed Mehraj Modified    Added DeleteCheckOutPrices() method
 *2.70.2      10-Dec-2019   Jinto Thomas                Removed siteid from update query
 *2.100.0     14-Aug-2020   Girish Kundar               Modified : 3 Tier changes as part of phase -3 Rest API changes
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Product
{
    public class CheckOutPricesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;

        private static readonly Dictionary<CheckOutPricesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CheckOutPricesDTO.SearchByParameters, string>
        {
             {CheckOutPricesDTO.SearchByParameters.ID, "cip.Id"},
             {CheckOutPricesDTO.SearchByParameters.PRODUCT_ID, "cip.ProductId"},
             {CheckOutPricesDTO.SearchByParameters.SITE_ID, "cip.site_id"},
             {CheckOutPricesDTO.SearchByParameters.ISACTIVE, "cip.IsActive"},
             {CheckOutPricesDTO.SearchByParameters.MASTERENTITY_ID, "cip.MasterEntityId"}
        };

        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM CheckInPrices AS cip ";
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CheckOutPricesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        private CheckOutPricesDTO GetCheckOutPricesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            try
            {
                CheckOutPricesDTO checkOutPricesDTODataObject = new CheckOutPricesDTO(
                                                    dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                    dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                    dataRow["TimeSlab"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TimeSlab"]),
                                                    dataRow["Price"] == DBNull.Value ? -1 : Convert.ToDecimal(dataRow["Price"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                    );
                log.LogMethodExit(checkOutPricesDTODataObject);
                return checkOutPricesDTODataObject;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(null);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Hard delete CheckInPrices by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteCheckOutPrices(int id)
        {
            log.LogMethodEntry(id);
            try
            {
                string deleteQuery = @"delete from CheckInPrices where Id = @Id";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@Id", id);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters);
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
        /// Builds the SQL Parameter list used for inserting and updating checkOutPricesDTO Record.
        /// </summary>
        /// <param name="checkOutPricesDTO">checkOutPricesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CheckOutPricesDTO checkOutPricesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkOutPricesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", checkOutPricesDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", checkOutPricesDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeSlab", checkOutPricesDTO.TimeSlab));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Price", checkOutPricesDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", checkOutPricesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", checkOutPricesDTO.MasterEntityId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Insert the CheckOutPrices record to the database
        /// </summary>
        /// <param name="checkOutPricesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CheckOutPricesDTO Insert(CheckOutPricesDTO checkOutPricesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkOutPricesDTO, loginId, siteId);
            string query = @"INSERT INTO CheckInPrices 
                                (
                                  ProductId,
                                  TimeSlab, 
                                  Price,
                                  site_id, 
                                  Guid, 
                                  LastUpdateDate, 
                                  LastUpdatedBy,
                                  CreatedBy,
                                  CreationDate,
                                  IsActive,
                                  MasterEntityId
                              ) 
                              VALUES
                              (
                                  @ProductId,
                                  @TimeSlab, 
                                  @Price, 
                                  @site_id, 
                                  NEWID(),
                                  GetDate(),
                                  @LastUpdatedBy,
                                  @LastUpdatedBy,
                                  GetDate(),    
                                  @IsActive,
                                  @MasterEntityId
                              )  SELECT * FROM CheckInPrices WHERE Id = scope_identity()";
            try
            {
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkOutPricesDTO, loginId, siteId).ToArray(), null);
                RefreshCheckOutPricesDTO(checkOutPricesDTO, dataTable);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting checkOutPricesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(checkOutPricesDTO);
            return checkOutPricesDTO;
        }

        private void RefreshCheckOutPricesDTO(CheckOutPricesDTO checkOutPricesDTO, DataTable dt)
        {
            log.LogMethodEntry(checkOutPricesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                checkOutPricesDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                checkOutPricesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                checkOutPricesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                checkOutPricesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                checkOutPricesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                checkOutPricesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                checkOutPricesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Update the CheckOutPrices record to the database
        /// </summary>
        /// <param name="checkOutPricesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CheckOutPricesDTO Update (CheckOutPricesDTO checkOutPricesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkOutPricesDTO, loginId, siteId);
            string query = @"UPDATE CheckInPrices 
                                 SET 
                                     ProductId = @ProductId, 
                                     TimeSlab = @TimeSlab, 
                                     Price = @Price, 
                                     LastUpdateDate = GetDate(), 
                                     LastUpdatedBy = @LastUpdatedBy,
                                     IsActive = @IsActive
                                 WHERE  Id = @Id  AND  ProductId = @ProductId
                                 SELECT* FROM  CheckInPrices WHERE Id = @Id"; ;
            try
            {
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkOutPricesDTO, loginId, siteId).ToArray(), null);
                RefreshCheckOutPricesDTO(checkOutPricesDTO, dataTable);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting checkOutPricesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(checkOutPricesDTO);
            return checkOutPricesDTO;
        }


        /// <summary>
        /// Gets the CheckOutPrices
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CheckOutPricesDTO> GetAllCheckOutPricesList(List<KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCheckOutPricesQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CheckOutPricesDTO> checkOutPricesList = new List<CheckOutPricesDTO>();
            string joiner = string.Empty;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CheckOutPricesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == CheckOutPricesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(CheckOutPricesDTO.SearchByParameters.ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(CheckOutPricesDTO.SearchByParameters.PRODUCT_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CheckOutPricesDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                    selectCheckOutPricesQuery = selectCheckOutPricesQuery + query;
                selectCheckOutPricesQuery = selectCheckOutPricesQuery + " Order by Id";
            }
            DataTable checkOutPriceDataTable = dataAccessHandler.executeSelectQuery(selectCheckOutPricesQuery, parameters.ToArray(),sqlTransaction);
            if (checkOutPriceDataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in checkOutPriceDataTable.Rows)
                {
                    CheckOutPricesDTO checkOutPricesObject = GetCheckOutPricesDTO(dataRow);
                    checkOutPricesList.Add(checkOutPricesObject);
                }
            }
            log.LogMethodExit(checkOutPricesList);
            return checkOutPricesList;
        }
    }
}
