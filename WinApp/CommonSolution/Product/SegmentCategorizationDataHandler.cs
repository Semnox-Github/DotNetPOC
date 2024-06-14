/********************************************************************************************
 * Project Name - Segment Categorization Data Handler
 * Description  - Data handler of the segment categorization data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Apr-2016   Raghuveera          Created 
 *2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Segment Categorization Data Handler - Handles insert, update and select of segment categorization data objects
    /// </summary>
    public class SegmentCategorizationDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<SegmentCategorizationDTO.SearchBySegmentCategorizationParameters, string> DBSearchParameters = new Dictionary<SegmentCategorizationDTO.SearchBySegmentCategorizationParameters, string>
               {
                    {SegmentCategorizationDTO.SearchBySegmentCategorizationParameters.SEGMENT_CATEGORY_ID, "SegmentCategoryId"},
                    {SegmentCategorizationDTO.SearchBySegmentCategorizationParameters.SITE_ID, "site_id"}

               };
        DataAccessHandler dataAccessHandler;
        SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of SegmentCategorizationDataHandler class
        /// </summary>
        public SegmentCategorizationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.Debug("Starts-SegmentCategorizationDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.Debug("Ends-SegmentCategorizationDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the segment categorization record to the database
        /// </summary>
        /// <param name="segmentCategorization">SegmentCategorizationDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertSegmentCategorization(SegmentCategorizationDTO segmentCategorization, string userId, int siteId)
        {
            log.Debug("Starts-InsertSegmentCategorization(segmentCategorization, userId, siteId) Method.");
            string insertSegmentCategorizationQuery = @"insert into Segment_Categorization 
                                                        (
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        site_id,
                                                        SynchStatus
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateSegmentCategorizationParameters = new List<SqlParameter>();
            updateSegmentCategorizationParameters.Add(new SqlParameter("@createdBy", userId));
            updateSegmentCategorizationParameters.Add(new SqlParameter("@lastUpdatedBy", userId));

            if (siteId == -1)
            {
                updateSegmentCategorizationParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (segmentCategorization.SynchStatus)
            {
                updateSegmentCategorizationParameters.Add(new SqlParameter("@synchStatus", segmentCategorization.SynchStatus));
            }
            else
            {
                updateSegmentCategorizationParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertSegmentCategorizationQuery, updateSegmentCategorizationParameters.ToArray(), sqlTransaction);
            log.Debug("Ends-InsertSegmentCategorization(segmentCategorization, userId, siteId) Method.");
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the segment categorization record
        /// </summary>
        /// <param name="segmentCategorization">SegmentCategorizationDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateSegmentCategorization(SegmentCategorizationDTO segmentCategorization, string userId, int siteId)
        {
            log.Debug("Starts-UpdateSegmentCategorization(segmentCategorization, userId, siteId) Method.");
            string updateSegmentCategorizationQuery = @"update Segment_Categorization 
                                         set LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid,
                                             SynchStatus = @synchStatus                                             
                                       where SegmentCategoryId = @segmentCategoryId";
            List<SqlParameter> updateSegmentCategorizationParameters = new List<SqlParameter>();
            updateSegmentCategorizationParameters.Add(new SqlParameter("@segmentCategoryId", segmentCategorization.SegmentCategoryId));
            updateSegmentCategorizationParameters.Add(new SqlParameter("@lastUpdatedBy", userId));

            if (siteId == -1)
            {
                updateSegmentCategorizationParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (segmentCategorization.SynchStatus)
            {
                updateSegmentCategorizationParameters.Add(new SqlParameter("@synchStatus", segmentCategorization.SynchStatus));
            }
            else
            {
                updateSegmentCategorizationParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateSegmentCategorizationQuery, updateSegmentCategorizationParameters.ToArray(), sqlTransaction);
            log.Debug("Ends-UpdateSegmentCategorization(segmentCategorization, userId, siteId) Method.");
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to SegmentCategorizationDTO class type
        /// </summary>
        /// <param name="segmentCategorizationDataRow">SegmentCategorizationDTO DataRow</param>
        /// <returns>Returns SegmentCategorizationDTO</returns>
        private SegmentCategorizationDTO GetSegmentCategorizationDTO(DataRow segmentCategorizationDataRow)
        {
            log.Debug("Starts-GetSegmentCategorizationDTO(segmentCategorizationDataRow) Method.");
            SegmentCategorizationDTO segmentCategorizationDataObject = new SegmentCategorizationDTO(Convert.ToInt32(segmentCategorizationDataRow["SegmentCategoryId"]),
                                            segmentCategorizationDataRow["CreatedBy"].ToString(),
                                            segmentCategorizationDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentCategorizationDataRow["CreationDate"]),
                                            segmentCategorizationDataRow["LastUpdatedBy"].ToString(),
                                            segmentCategorizationDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentCategorizationDataRow["LastupdatedDate"]),
                                            segmentCategorizationDataRow["Guid"].ToString(),
                                            segmentCategorizationDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(segmentCategorizationDataRow["site_id"]),
                                            segmentCategorizationDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(segmentCategorizationDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetSegmentCategorizationDTO(segmentCategorizationDataRow) Method.");
            return segmentCategorizationDataObject;
        }

        /// <summary>
        /// Gets the segment categorization data of passed segment categorization id
        /// </summary>
        /// <param name="SegmentSegmentCategorizationId">integer type parameter</param>
        /// <returns>Returns SegmentCategorizationDTO</returns>
        public SegmentCategorizationDTO GetSegmentCategorization(int SegmentSegmentCategorizationId)
        {
            log.Debug("Starts-GetSegmentCategorization(SegmentSegmentCategorizationId) Method.");
            string selectSegmentCategorizationQuery = @"select *
                                         from Segment_Categorization
                                        where SegmentSegmentCategorizationId = @segmentCategoryId";
            SqlParameter[] selectSegmentCategorizationParameters = new SqlParameter[1];
            selectSegmentCategorizationParameters[0] = new SqlParameter("@segmentCategoryId", SegmentSegmentCategorizationId);
            DataTable segmentCategorization = dataAccessHandler.executeSelectQuery(selectSegmentCategorizationQuery, selectSegmentCategorizationParameters, sqlTransaction);
            if (segmentCategorization.Rows.Count > 0)
            {
                DataRow segmentCategorizationRow = segmentCategorization.Rows[0];
                SegmentCategorizationDTO segmentCategorizationDataObject = GetSegmentCategorizationDTO(segmentCategorizationRow);
                log.Debug("Ends-GetSegmentCategorization(SegmentSegmentCategorizationId) Method by returnting segmentCategorizationDataObject.");
                return segmentCategorizationDataObject;
            }
            else
            {
                log.Debug("Ends-GetSegmentCategorization(SegmentSegmentCategorizationId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// Retriving segment Categorization by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the segment Categorization</param>
        /// <returns> List of SegmentCategorizationDTO </returns>
        public List<SegmentCategorizationDTO> GetSegmentCategorizationList(string sqlQuery)
        {
            log.Debug("Starts-GetSegmentCategorization(sqlQuery) Method.");
            string Query = sqlQuery.ToUpper();
            if (Query.Contains("DROP") || Query.Contains("UPDATE") || Query.Contains("DELETE"))
            {
                log.Debug("Ends-GetSegmentCategorization(sqlQuery) Method by invalid query.");
                return null;
            }
            DataTable segmentCategorizationData = dataAccessHandler.executeSelectQuery(sqlQuery, null, sqlTransaction);
            if (segmentCategorizationData.Rows.Count > 0)
            {
                List<SegmentCategorizationDTO> segmentCategorizationList = new List<SegmentCategorizationDTO>();
                foreach (DataRow segmentCategorizationDataRow in segmentCategorizationData.Rows)
                {
                    SegmentCategorizationDTO segmentCategorizationDataObject = GetSegmentCategorizationDTO(segmentCategorizationDataRow);
                    segmentCategorizationList.Add(segmentCategorizationDataObject);
                }
                log.Debug("Ends-GetSegmentCategorizationList(sqlQuery) Method by returning segmentCategorizationList.");
                return segmentCategorizationList; ;
            }
            else
            {
                log.Debug("Ends-GetSegmentCategorization(sqlQuery) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the SegmentCategorizationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SegmentCategorizationDTO matching the search criteria</returns>
        public List<SegmentCategorizationDTO> GetSegmentCategorizationList(List<KeyValuePair<SegmentCategorizationDTO.SearchBySegmentCategorizationParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetSegmentCategorizationList(searchParameters) Method.");
            int count = 0;
            string selectSegmentCategorizationQuery = @"select *
                                         from Segment_Categorization";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SegmentCategorizationDTO.SearchBySegmentCategorizationParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(SegmentCategorizationDTO.SearchBySegmentCategorizationParameters.SEGMENT_CATEGORY_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }

                            else if (searchParameter.Key.Equals(SegmentCategorizationDTO.SearchBySegmentCategorizationParameters.SITE_ID))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(SegmentCategorizationDTO.SearchBySegmentCategorizationParameters.SEGMENT_CATEGORY_ID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(SegmentCategorizationDTO.SearchBySegmentCategorizationParameters.SITE_ID))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetSegmentCategorizationList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectSegmentCategorizationQuery = selectSegmentCategorizationQuery + query;
            }

            DataTable segmentCategorizationData = dataAccessHandler.executeSelectQuery(selectSegmentCategorizationQuery, null, sqlTransaction);
            if (segmentCategorizationData.Rows.Count > 0)
            {
                List<SegmentCategorizationDTO> segmentCategorizationList = new List<SegmentCategorizationDTO>();
                foreach (DataRow segmentCategorizationDataRow in segmentCategorizationData.Rows)
                {
                    SegmentCategorizationDTO segmentCategorizationDataObject = GetSegmentCategorizationDTO(segmentCategorizationDataRow);
                    segmentCategorizationList.Add(segmentCategorizationDataObject);
                }
                log.Debug("Ends-GetSegmentCategorizationList(searchParameters) Method by returning segmentCategorizationList.");
                return segmentCategorizationList;
            }
            else
            {
                log.Debug("Ends-GetSegmentCategorizationList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}
