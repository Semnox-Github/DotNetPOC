/********************************************************************************************
 * Project Name - ReservationDiscounts Data Handler
 * Description  - Data handler of the ReservationDiscounts class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        31-Aug-2017   Lakshminarayana     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Booking
{
    /// <summary>
    ///  ReservationDiscounts Data Handler - Handles insert, update and select of  ReservationDiscounts objects
    /// </summary>
    public class ReservationDiscountsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ReservationDiscountsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ReservationDiscountsDTO.SearchByParameters, string>
            {
                {ReservationDiscountsDTO.SearchByParameters.BOOKING_ID, "BookingId"},
                {ReservationDiscountsDTO.SearchByParameters.RESERVATION_DISCOUNT_ID, "ReservationDiscountId"},
                {ReservationDiscountsDTO.SearchByParameters.PRODUCT_ID, "productId"},
                {ReservationDiscountsDTO.SearchByParameters.RESERVATION_DISCOUNT_CATEGORY, "ReservationDiscountCategory"},
                {ReservationDiscountsDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {ReservationDiscountsDTO.SearchByParameters.SITE_ID, "site_id"}
            };
        DataAccessHandler dataAccessHandler;
        SqlTransaction sqlTransaction = null;

        /// <summary>
        /// Default constructor of ReservationDiscountsDataHandler class
        /// </summary>
        public ReservationDiscountsDataHandler(SqlTransaction sqlTransaction)
        {
            log.Debug("Starts-ReservationDiscountsDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.Debug("Ends-ReservationDiscountsDataHandler() default constructor.");
        }

        private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        {
            log.Debug("Starts-ParameterHelper() method.");
            if (parameters != null && !string.IsNullOrEmpty(parameterName))
            {
                if (value is int)
                {
                    if (negetiveValueNull && ((int)value) < 0)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else if (value is string)
                {
                    if (string.IsNullOrEmpty(value as string))
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else
                {
                    if (value == null)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
            }
            log.Debug("Ends-ParameterHelper() Method");
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ReservationDiscounts Record.
        /// </summary>
        /// <param name="reservationDiscountsDTO">ReservationDiscountsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(ReservationDiscountsDTO reservationDiscountsDTO, string userId, int siteId)
        {
            log.Debug("Starts-BuildSQLParameters(reservationDiscountsDTO, userId, siteId) Method.");
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParameterHelper(parameters, "@Id", reservationDiscountsDTO.Id, true);
            ParameterHelper(parameters, "@BookingId", reservationDiscountsDTO.BookingId, true);
            ParameterHelper(parameters, "@ReservationDiscountId", reservationDiscountsDTO.ReservationDiscountId, true);
            ParameterHelper(parameters, "@ReservationDiscountPecentage", reservationDiscountsDTO.ReservationDiscountPecentage);
            ParameterHelper(parameters, "@ReservationDiscountCategory", reservationDiscountsDTO.ReservationDiscountCategory);
            ParameterHelper(parameters, "@productId", reservationDiscountsDTO.ProductId);
            ParameterHelper(parameters, "@site_id", siteId, true);
            ParameterHelper(parameters, "@MasterEntityId", reservationDiscountsDTO.MasterEntityId, true);
            log.Debug("Ends-BuildSQLParameters(reservationDiscountsDTO, userId, siteId) Method.");
            return parameters;
        }

        /// <summary>
        /// Inserts the ReservationDiscounts record to the database
        /// </summary>
        /// <param name="reservationDiscountsDTO">ReservationDiscountsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertReservationDiscounts(ReservationDiscountsDTO reservationDiscountsDTO, string userId, int siteId)
        {
            log.Debug("Starts-InsertReservationDiscounts(reservationDiscountsDTO, userId, siteId) Method.");
            int idOfRowInserted;
            string query = @"INSERT INTO ReservationDiscounts 
                                        ( 
                                            BookingId,
                                            ReservationDiscountId,
                                            ReservationDiscountPecentage,
                                            ReservationDiscountCategory,
                                            productId,
                                            site_id,
                                            MasterEntityId,
                                            SynchStatus
                                        ) 
                                VALUES 
                                        (
                                            @BookingId,
                                            @ReservationDiscountId,
                                            @ReservationDiscountPecentage,
                                            @ReservationDiscountCategory,
                                            @productId,
                                            @site_id,
                                            @MasterEntityId,
                                            NULL
                                        )SELECT CAST(scope_identity() AS int)";


            List<SqlParameter> parameters = BuildSQLParameters(reservationDiscountsDTO, userId, siteId);
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, parameters.ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(reservationDiscountsDTO.ToString());
                log.Error(query);
                throw ex;
            }

            log.Debug("Ends-InsertReservationDiscounts(reservationDiscountsDTO, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the ReservationDiscounts record
        /// </summary>
        /// <param name="reservationDiscountsDTO">ReservationDiscountsDTO type parameter</param>
        /// <param name="userId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateReservationDiscounts(ReservationDiscountsDTO reservationDiscountsDTO, string userId, int siteId)
        {
            log.Debug("Starts-UpdateReservationDiscounts(reservationDiscountsDTO, userId, siteId) Method.");
            int rowsUpdated;
            string query = @"UPDATE ReservationDiscounts 
                             SET BookingId=@BookingId,
                                 ReservationDiscountId=@ReservationDiscountId,
                                 ReservationDiscountPecentage=@ReservationDiscountPecentage,
                                 ReservationDiscountCategory=@ReservationDiscountCategory,
                                 productId=@productId,
                                 MasterEntityId=@MasterEntityId,
                                 SynchStatus=NULL
                             WHERE Id = @Id";
            List<SqlParameter> parameters = BuildSQLParameters(reservationDiscountsDTO, userId, siteId);
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, parameters.ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(reservationDiscountsDTO.ToString());
                log.Error(query);
                throw ex;
            }
            log.Debug("Ends-UpdateReservationDiscounts(reservationDiscountsDTO, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to ReservationDiscountsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ReservationDiscountsDTO</returns>
        private ReservationDiscountsDTO GetReservationDiscountsDTO(DataRow dataRow)
        {
            log.Debug("Starts-GetReservationDiscountsDTO(dataRow) Method.");
            ReservationDiscountsDTO reservationDiscountsDTO = new ReservationDiscountsDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BookingId"]),
                                            dataRow["ReservationDiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReservationDiscountId"]),
                                            dataRow["productId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["productId"]),
                                            dataRow["ReservationDiscountPecentage"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["ReservationDiscountPecentage"]),
                                            dataRow["ReservationDiscountCategory"] == DBNull.Value ? null : Convert.ToString(dataRow["ReservationDiscountCategory"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString()
                                            );
            log.Debug("Ends-GetReservationDiscountsDTO(dataRow) Method.");
            return reservationDiscountsDTO;
        }

        /// <summary>
        /// Gets the ReservationDiscounts data of passed ReservationDiscounts Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ReservationDiscountsDTO</returns>
        public ReservationDiscountsDTO GetReservationDiscountsDTO(int id)
        {
            log.Debug("Starts-GetReservationDiscountsDTO(Id) Method.");
            ReservationDiscountsDTO returnValue = null;
            string query = @"SELECT *
                            FROM ReservationDiscounts
                            WHERE Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetReservationDiscountsDTO(dataTable.Rows[0]);
                log.Debug("Ends-GetReservationDiscountsDTO(id) Method by returnting ReservationDiscountsDTO.");
            }
            else
            {
                log.Debug("Ends-GetReservationDiscountsDTO(id) Method by returnting null.");
            }
            return returnValue;
        }


        /// <summary>
        /// Deletes the ReservationDiscounts data of passed ReservationDiscounts Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns no of rows deleted</returns>
        public int Delete(int id)
        {
            log.Debug("Starts-Delete(Id) Method.");
            string query = @"DELETE FROM ReservationDiscounts
                            WHERE Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            int noOfRowsDeleted = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.Debug("Ends-Delete(Id) Method.");
            return noOfRowsDeleted;
        }

        /// <summary>
        /// Gets the ReservationDiscountsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ReservationDiscountsDTO matching the search criteria</returns>
        public List<ReservationDiscountsDTO> GetReservationDiscountsDTOList(List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetReservationDiscountsDTOList(searchParameters) Method.");
            List<ReservationDiscountsDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT * FROM ReservationDiscounts";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ReservationDiscountsDTO.SearchByParameters.BOOKING_ID ||
                            searchParameter.Key == ReservationDiscountsDTO.SearchByParameters.RESERVATION_DISCOUNT_ID ||
                            searchParameter.Key == ReservationDiscountsDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == ReservationDiscountsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ReservationDiscountsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ReservationDiscountsDTO.SearchByParameters.RESERVATION_DISCOUNT_CATEGORY)
                        {
                            query.Append(joiner +DBSearchParameters[searchParameter.Key] + "='" + searchParameter.Value+"'");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetReservationDiscountsDTOList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ReservationDiscountsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReservationDiscountsDTO reservationDiscountsDTO = GetReservationDiscountsDTO(dataRow);
                    list.Add(reservationDiscountsDTO);
                }
                log.Debug("Ends-GetReservationDiscountsDTOList(searchParameters) Method by returning list.");
            }
            else
            {
                log.Debug("Ends-GetReservationDiscountsDTOList(searchParameters) Method by returning null.");
            }
            return list;
        }
    }
}
