/********************************************************************************************************
 * Project Name - ParentChildCardsDataHandler
 * Description  - Data Handler Class for ParentChildCards Entity
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************************
*2.100.0      10-Oct-2020     Mathew Ninan      Modified: Support for Daily Limit Percentage for child cards
**********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CardCore
{
    /// <summary>
    ///  ParentChildCardsDataHandler   - Handles insert, update and select of  ParentChildCards objects
    /// </summary>
    public class ParentChildCardsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<ParentChildCardsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ParentChildCardsDTO.SearchByParameters, string>
            {
                {ParentChildCardsDTO.SearchByParameters.ID, "Id"},
                {ParentChildCardsDTO.SearchByParameters.PARENT_CARD_ID, "ParentCardId"},
                {ParentChildCardsDTO.SearchByParameters.CHILD_CARD_ID,"ChildCardId"},
                {ParentChildCardsDTO.SearchByParameters.ACTIVE_FLAG,"ActiveFlag"},
                {ParentChildCardsDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {ParentChildCardsDTO.SearchByParameters.SITE_ID, "site_id"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of ParentChildCardsDataHandler class
        /// </summary>
        public ParentChildCardsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating parentChildCards Record.
        /// </summary>
        /// <param name="parentChildCardsDTO">parentChildCardsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ParentChildCardsDTO parentChildCardsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(parentChildCardsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", parentChildCardsDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentCardId", parentChildCardsDTO.ParentCardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChildCardId", parentChildCardsDTO.ChildCardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", parentChildCardsDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", parentChildCardsDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", parentChildCardsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DailyLimitPercentage", parentChildCardsDTO.DailyLimitPercentage));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the parentChildCards record to the database
        /// </summary>
        /// <param name="parentChildCardsDTO">ParentChildCardsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertParentChildCards(ParentChildCardsDTO parentChildCardsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(parentChildCardsDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO ParentChildCards 
                                        ( 
                                           ParentCardId, 
                                           ChildCardId, 
                                           site_id, 
                                           Guid, 
                                          -- SynchStatus, 
                                           CreationDate, 
                                           CreatedBy, 
                                           LastUpdatedDate, 
                                           LastUpdatedBy, 
                                           ActiveFlag, 
                                           MasterEntityId,
                                           DailyLimitPercentage
                                        ) 
                                VALUES 
                                        (
                                           @ParentCardId, 
                                           @ChildCardId, 
                                           @Site_id, 
                                           NEWID(), 
                                          -- @SynchStatus, 
                                           GETDATE(), 
                                           @CreatedBy, 
                                           GETDATE(), 
                                           @LastUpdatedBy, 
                                           @ActiveFlag, 
                                           @MasterEntityId,
                                           @DailyLimitPercentage
                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(parentChildCardsDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the parentChildCards record
        /// </summary>
        /// <param name="parentChildCardsDTO">ParentChildCardsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateParentChildCards(ParentChildCardsDTO parentChildCardsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(parentChildCardsDTO, userId, siteId);
            int rowsUpdated;
            string query = @"UPDATE ParentChildCards 
                                SET ParentCardId = @ParentCardId, 
                                    ChildCardId = @ChildCardId, 
                                    --site_id  = @Site_id
                                   --SynchStatus = @SynchStatus, 
                                    LastUpdatedDate = GETDATE(),
                                    LastUpdatedBy = @LastUpdatedBy, 
                                    ActiveFlag = @ActiveFlag, 
                                    MasterEntityId = @MasterEntityId,
                                    DailyLimitPercentage = @DailyLimitPercentage
                              WHERE Id = @Id";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(parentChildCardsDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to ParentChildCardsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ParentChildCardsDTO</returns>
        private ParentChildCardsDTO GetParentChildCardsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ParentChildCardsDTO parentChildCardsDTO = new ParentChildCardsDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["ParentCardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentCardId"]),
                                            dataRow["ChildCardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ChildCardId"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(), 
                                            dataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ActiveFlag"]),  
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["DailyLimitPercentage"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["DailyLimitPercentage"])
                                            );
            log.LogMethodExit(parentChildCardsDTO);
            return parentChildCardsDTO;
        }

        /// <summary>
        /// Gets the CustomerRelationshipType data of passed parentChildCards Id
        /// </summary>
        /// <param name="parentChildCardsId">integer type parameter</param>
        /// <returns>Returns ParentChildCardsDTO</returns>
        public ParentChildCardsDTO GetParentChildCardsDTO(int id)
        {
            log.LogMethodEntry(id);
            ParentChildCardsDTO returnValue = null;
            string query = @"SELECT *
                            FROM ParentChildCards
                            WHERE Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetParentChildCardsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the ParentChildCardsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ParentChildCardsDTO matching the search criteria</returns>
        public List<ParentChildCardsDTO> GetParentChildCardsDTOList(List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ParentChildCardsDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT * FROM ParentChildCards";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ParentChildCardsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if ( (searchParameter.Key == ParentChildCardsDTO.SearchByParameters.ID) ||
                             (searchParameter.Key == ParentChildCardsDTO.SearchByParameters.PARENT_CARD_ID) ||
                             (searchParameter.Key == ParentChildCardsDTO.SearchByParameters.CHILD_CARD_ID)
                             )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if ((searchParameter.Key == ParentChildCardsDTO.SearchByParameters.SITE_ID) ||
                                 (searchParameter.Key == ParentChildCardsDTO.SearchByParameters.MASTER_ENTITY_ID) )
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ParentChildCardsDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ", 1)=" + searchParameter.Value);
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ParentChildCardsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ParentChildCardsDTO parentChildCardsDTO = GetParentChildCardsDTO(dataRow);
                    list.Add(parentChildCardsDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the ParentChildCardsDTO list matching the search key
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>Returns the list of ParentChildCardsDTO matching the search criteria</returns>
        public List<ParentChildCardsDTO> GetActiveParentCardsListByCustomer(int customerId)
        {
            log.LogMethodEntry(customerId);
            List<ParentChildCardsDTO> list = null; 
            string selectQuery = @" SELECT pcc.* 
                                      FROM parentchildcards pcc, 
                                           cards cc
                                     WHERE pcc.ParentCardId = cc.card_id
                                       AND pcc.ActiveFlag = '1'
                                       AND cc.valid_flag = 'Y'
                                       AND CC.customer_id = @CustomerId ";

            SqlParameter parameter = new SqlParameter("@CustomerId", customerId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ParentChildCardsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ParentChildCardsDTO parentChildCardsDTO = GetParentChildCardsDTO(dataRow);
                    list.Add(parentChildCardsDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

    }
}
