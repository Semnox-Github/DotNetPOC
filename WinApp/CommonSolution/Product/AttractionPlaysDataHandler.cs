/********************************************************************************************
 * Project Name - Products/AttractionPlaysDataHandler
 * Description  - Created to fetch, update and insert AttractionPlays in the Attraction Plays.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 *********************************************************************************************
 *2.60        31-Jan-2019   Nagesh Badiger          Created
 *2.70        27-Jun-2019   Akshay Gulaganji        Added DeleteAttractionPlays() method.
 *2.70.2        10-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Data;

namespace Semnox.Parafait.Product
{
    public class AttractionPlaysDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string> DBSearchParameters = new Dictionary<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>
        {
                {AttractionPlaysDTO.SearchByAttractionPlaysParameters.ID,"AttractionPlayId"},
                {AttractionPlaysDTO.SearchByAttractionPlaysParameters.SITE_ID, "Site_id"},
                {AttractionPlaysDTO.SearchByAttractionPlaysParameters.GUID, "Guid"},
                {AttractionPlaysDTO.SearchByAttractionPlaysParameters.MASTERENTITY_ID, "MasterEntityId"},
                {AttractionPlaysDTO.SearchByAttractionPlaysParameters.PLAYNAME, "PlayName"},
                {AttractionPlaysDTO.SearchByAttractionPlaysParameters.IS_ACTIVE, "IsActive"}
        };

        /// <summary>
        /// Default constructor of AttractionPlaysDataHandler class
        /// </summary>
        public AttractionPlaysDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating attraction plays Record.
        /// </summary>
        /// <param name="attractionPlaysDTO">AttractionPlaysDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AttractionPlaysDTO attractionPlaysDTO, string userId, int siteId)
        {
            log.LogMethodEntry(attractionPlaysDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@attractionPlayId", attractionPlaysDTO.AttractionPlayId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@playName", attractionPlaysDTO.PlayName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expiryDate", attractionPlaysDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@price", attractionPlaysDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", attractionPlaysDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", attractionPlaysDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Insert AttractionPlays
        /// </summary>
        /// <param name="attractionPlaysDTO">AttractionPlayDTO</param>        
        public int InsertAttractionPlays(AttractionPlaysDTO attractionPlaysDTO, string userId, int siteId)
        {
            log.LogMethodEntry(attractionPlaysDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"insert into AttractionPlays 
                                                        (                                                          
                                                        PlayName,
                                                        ExpiryDate,
                                                        Price,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (    
                                                      
                                                        @playName,
                                                        @expiryDate,
                                                        @price,
                                                        NewId(),
                                                        @site_id,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        Getdate(),
                                                        @lastUpdatedBy,
                                                        Getdate(),
                                                        @isActive
                                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(attractionPlaysDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, "throwing exception" + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Update the attractionPlay record to the database
        /// </summary>
        /// <param name="attractionPlaysDTO">AttractionPlaysDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns Updated record id</returns>
        public int UpdateAttractionPlays(AttractionPlaysDTO attractionPlaysDTO, string userId, int siteId)
        {
            log.LogMethodEntry(attractionPlaysDTO, userId, siteId);
            int rowsUpdated;
            string query = @"update AttractionPlays set                                                                                                             
                                                        PlayName=@playName,
                                                        ExpiryDate=@expiryDate,
                                                        Price=@price,                                                      
                                                        -- site_id=@site_id,                                                        
                                                        MasterEntityId=@masterEntityId,                                                  
                                                        LastUpdateDate=Getdate(),
                                                        IsActive=@isActive
                                                        where AttractionPlayId=@attractionPlayId";

            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(attractionPlaysDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, "throwing exception" + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Gets the GetAttractionPlaysDTO List matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic GetAttractionPlaysDTO matching the search criteria</returns>
        public List<AttractionPlaysDTO> GetAttractionPlaysDTOList(List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectGenericAttractionPlaysQuery = @"select * from AttractionPlays";

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == AttractionPlaysDTO.SearchByAttractionPlaysParameters.ID || searchParameter.Key == AttractionPlaysDTO.SearchByAttractionPlaysParameters.GUID || searchParameter.Key == AttractionPlaysDTO.SearchByAttractionPlaysParameters.MASTERENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key.Equals(AttractionPlaysDTO.SearchByAttractionPlaysParameters.PLAYNAME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " ='" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == AttractionPlaysDTO.SearchByAttractionPlaysParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == AttractionPlaysDTO.SearchByAttractionPlaysParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + "'" + searchParameter.Value + "'");
                            //query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'" + (searchParameter.Value == "true" ? (" or " + DBSearchParameters[searchParameter.Key] + " IS Null") : ""));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetAttractionPlaysList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }

                }
                selectGenericAttractionPlaysQuery = selectGenericAttractionPlaysQuery + query + " ORDER BY PlayName";
            }

            DataTable attractionPlaysData = dataAccessHandler.executeSelectQuery(selectGenericAttractionPlaysQuery, null);
            if (attractionPlaysData.Rows.Count > 0)
            {
                List<AttractionPlaysDTO> attractionPlayList = new List<AttractionPlaysDTO>();
                foreach (DataRow attractionPlaysDTODataRow in attractionPlaysData.Rows)
                {
                    AttractionPlaysDTO attractionPlaysDataObject = GetAttractionPlaysDTO(attractionPlaysDTODataRow);
                    attractionPlayList.Add(attractionPlaysDataObject);
                }
                log.LogMethodExit(attractionPlayList);
                return attractionPlayList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Converts the Data row object to AttractionPlaysDTO class type
        /// </summary>
        /// <param name="attractionPlayDataRow">GetAttractionPlays DataRow</param>
        /// <returns>Returns GetAttractionPlaysDTO</returns>
        private AttractionPlaysDTO GetAttractionPlaysDTO(DataRow attractionPlayDataRow)
        {
            log.LogMethodEntry(attractionPlayDataRow);
            AttractionPlaysDTO attractionPlaysDataObject = new AttractionPlaysDTO(Convert.ToInt32(attractionPlayDataRow["AttractionPlayId"]),
                                            attractionPlayDataRow["PlayName"].ToString(),
                                            attractionPlayDataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(attractionPlayDataRow["ExpiryDate"]),
                                            attractionPlayDataRow["Price"] == DBNull.Value ? (double?)null : Convert.ToDouble(attractionPlayDataRow["Price"]),
                                            attractionPlayDataRow["Guid"].ToString(),
                                            attractionPlayDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(attractionPlayDataRow["site_id"]),
                                            attractionPlayDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(attractionPlayDataRow["SynchStatus"]),
                                            attractionPlayDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(attractionPlayDataRow["MasterEntityId"]),
                                            attractionPlayDataRow["CreatedBy"].ToString(),
                                            attractionPlayDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionPlayDataRow["CreationDate"]),
                                            attractionPlayDataRow["LastUpdatedBy"].ToString(),
                                            attractionPlayDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionPlayDataRow["LastUpdateDate"]),
                                             attractionPlayDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(attractionPlayDataRow["IsActive"])
                                            );
            log.LogMethodExit(attractionPlaysDataObject);
            return attractionPlaysDataObject;
        }
        /// <summary>
        /// Deletes the AttractionPlays based on the attractionPlayId
        /// </summary>
        /// <param name="attractionPlayId">attractionPlayId</param>
        /// <returns>return the int</returns>
        public int DeleteAttractionPlays(int attractionPlayId)
        {
            log.LogMethodEntry(attractionPlayId);
            try
            {
                string deleteQuery = @"delete from AttractionPlays where AttractionPlayId = @attractionPlayId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@attractionPlayId", attractionPlayId);

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
    }
}
