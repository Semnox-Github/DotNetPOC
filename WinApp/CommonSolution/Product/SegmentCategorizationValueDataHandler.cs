/********************************************************************************************
 * Project Name - Segment Categorization Value Data Handler
 * Description  - Data handler of the segment categorization value data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Apr-2016   Raghuveera          Created 
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
    /// Segment Categorization Value Data Handler - Handles insert, update and select of segment categorization value data objects
    /// </summary>
    public class SegmentCategorizationValueDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string> DBSearchParameters = new Dictionary<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>
               {
                   {SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_CATEGORY_VALUE_ID, "SegmentCategoryValueId"},
                   {SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_CATEGORY_ID, "SegmentCategoryId"},
                   {SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_DEFINITION_ID, "SegmentDefinitionId"},
                   {SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_VALUE_TEXT, "SegmentValueText"},
                   {SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_VALUE_DATE, "SegmentValueDate"},
                   {SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_STATIC_VALUE_ID,"SegmentStaticValueId"},
                   {SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_DYNAMIC_VALUE_ID,"SegmentDynamicValueId"},
                   {SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.IS_ACTIVE, "IsActive"},
                   {SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SITE_ID, "site_id"}
               };
        DataAccessHandler dataAccessHandler;
        SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of SegmentCategorizationValueDataHandler class
        /// </summary>
        public SegmentCategorizationValueDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.Debug("Starts-SegmentCategorizationValueDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.Debug("Ends-SegmentCategorizationValueDataHandler() default constructor.");
        }


        /// <summary>
        /// Inserts the segment definition record to the database
        /// </summary>
        /// <param name="segmentCategorizationValue">SegmentCategorizationValueDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertSegmentCategorizationValue(SegmentCategorizationValueDTO segmentCategorizationValue, string userId, int siteId)
        {
            log.Debug("Starts-InsertSegmentCategorizationValue(segmentCategorizationValue, userId, siteId) Method.");
            string insertSegmentCategorizationValueQuery = @"insert into Segment_Categorization_Values 
                                                        (                                                       
                                                        SegmentCategoryId,
                                                        SegmentDefinitionId,
                                                        SegmentValueText,
                                                        SegmentValueDate,
                                                        SegmentStaticValueId,
                                                        SegmentDynamicValueId,
                                                        IsActive,
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
                                                        @segmentCategoryId,
                                                        @segmentDefinitionId,
                                                        @segmentValueText,
                                                        @segmentValueDate,
                                                        @segmentStaticValueId,
                                                        @segmentDynamicValueId,
                                                        @isActive,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateSegmentCategorizationValueParameters = new List<SqlParameter>();
            if (segmentCategorizationValue.SegmentCategoryId == -1)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentCategoryId", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentCategoryId", segmentCategorizationValue.SegmentCategoryId));
            }
            if (segmentCategorizationValue.SegmentDefinitionId == -1)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentDefinitionId", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentDefinitionId", segmentCategorizationValue.SegmentDefinitionId));
            }
            if (string.IsNullOrEmpty(segmentCategorizationValue.SegmentValueText))
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentValueText", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentValueText", segmentCategorizationValue.SegmentValueText));
            }
            if (segmentCategorizationValue.SegmentValueDate.Equals(DateTime.MinValue))
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentValueDate", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentValueDate", segmentCategorizationValue.SegmentValueDate));
            }
            if (segmentCategorizationValue.SegmentStaticValueId == -1)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentStaticValueId", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentStaticValueId", segmentCategorizationValue.SegmentStaticValueId));
            }
            if (string.IsNullOrEmpty(segmentCategorizationValue.SegmentDynamicValueId))
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentDynamicValueId", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentDynamicValueId", segmentCategorizationValue.SegmentDynamicValueId));
            }
            updateSegmentCategorizationValueParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(segmentCategorizationValue.IsActive) ? "N" : segmentCategorizationValue.IsActive));
            updateSegmentCategorizationValueParameters.Add(new SqlParameter("@createdBy", userId));
            updateSegmentCategorizationValueParameters.Add(new SqlParameter("@lastUpdatedBy", userId));

            if (siteId == -1)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (segmentCategorizationValue.SynchStatus)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@synchStatus", segmentCategorizationValue.SynchStatus));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertSegmentCategorizationValueQuery, updateSegmentCategorizationValueParameters.ToArray(), sqlTransaction);
            log.Debug("Ends-InsertSegmentCategorizationValue(segmentCategorizationValue, userId, siteId) Method.");
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the segment definition record
        /// </summary>
        /// <param name="segmentCategorizationValue">SegmentCategorizationValueDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateSegmentCategorizationValue(SegmentCategorizationValueDTO segmentCategorizationValue, string userId, int siteId)
        {
            log.Debug("Starts-UpdateSegmentCategorizationValue(segmentCategorizationValue, userId, siteId) Method.");
            string updateSegmentCategorizationValueQuery = @"update Segment_Categorization_Values 
                                         set SegmentCategoryId=@segmentCategoryId,
                                             SegmentDefinitionId=@segmentDefinitionId,
                                             SegmentValueText=@segmentValueText,
                                             SegmentValueDate=@segmentValueDate,
                                             SegmentStaticValueId=@segmentStaticValueId,
                                             SegmentDynamicValueId=@segmentDynamicValueId,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             -- site_id=@siteid,
                                             SynchStatus = @synchStatus                                             
                                       where SegmentCategoryValueId = @segmentCategoryValueId";
            List<SqlParameter> updateSegmentCategorizationValueParameters = new List<SqlParameter>();
            updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentCategoryValueId", segmentCategorizationValue.SegmentCategoryValueId));
            if (segmentCategorizationValue.SegmentCategoryId == -1)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentCategoryId", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentCategoryId", segmentCategorizationValue.SegmentCategoryId));
            }
            if (segmentCategorizationValue.SegmentDefinitionId == -1)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentDefinitionId", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentDefinitionId", segmentCategorizationValue.SegmentDefinitionId));
            }
            if (string.IsNullOrEmpty(segmentCategorizationValue.SegmentValueText))
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentValueText", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentValueText", segmentCategorizationValue.SegmentValueText));
            }
            if (segmentCategorizationValue.SegmentValueDate.Equals(DateTime.MinValue))
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentValueDate", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentValueDate", segmentCategorizationValue.SegmentValueDate));
            }
            if (segmentCategorizationValue.SegmentStaticValueId == -1)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentStaticValueId", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentStaticValueId", segmentCategorizationValue.SegmentStaticValueId));
            }
            if (string.IsNullOrEmpty(segmentCategorizationValue.SegmentDynamicValueId))
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentDynamicValueId", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@segmentDynamicValueId", segmentCategorizationValue.SegmentDynamicValueId));
            }
            updateSegmentCategorizationValueParameters.Add(new SqlParameter("@isActive", string.IsNullOrEmpty(segmentCategorizationValue.IsActive) ? "N" : segmentCategorizationValue.IsActive));
            updateSegmentCategorizationValueParameters.Add(new SqlParameter("@lastUpdatedBy", userId));

            if (siteId == -1)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@siteid", siteId));
            }
            if (segmentCategorizationValue.SynchStatus)
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@synchStatus", segmentCategorizationValue.SynchStatus));
            }
            else
            {
                updateSegmentCategorizationValueParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateSegmentCategorizationValueQuery, updateSegmentCategorizationValueParameters.ToArray(), sqlTransaction);
            log.Debug("Ends-UpdateSegmentCategorizationValue(segmentCategorizationValue, userId, siteId) Method.");
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to SegmentCategorizationValueDTO class type
        /// </summary>
        /// <param name="segmentCategorizationValueDataRow">SegmentCategorizationValueDTO DataRow</param>
        /// <returns>Returns SegmentCategorizationValueDTO</returns>
        private SegmentCategorizationValueDTO GetSegmentCategorizationValueDTO(DataRow segmentCategorizationValueDataRow)
        {
            log.Debug("Starts-GetSegmentCategorizationValueDTO(segmentCategorizationValueDataRow) Method.");
            SegmentCategorizationValueDTO segmentCategorizationValueDataObject = new SegmentCategorizationValueDTO(Convert.ToInt32(segmentCategorizationValueDataRow["SegmentCategoryValueId"]),
                                            segmentCategorizationValueDataRow["SegmentCategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(segmentCategorizationValueDataRow["SegmentCategoryId"]),
                                            segmentCategorizationValueDataRow["SegmentDefinitionId"] == DBNull.Value ? -1 : Convert.ToInt32(segmentCategorizationValueDataRow["SegmentDefinitionId"]),
                                            segmentCategorizationValueDataRow["SegmentValueText"].ToString(),
                                            segmentCategorizationValueDataRow["SegmentValueDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentCategorizationValueDataRow["SegmentValueDate"]),
                                            segmentCategorizationValueDataRow["SegmentStaticValueId"] == DBNull.Value ? -1 : Convert.ToInt32(segmentCategorizationValueDataRow["SegmentStaticValueId"]),
                                            segmentCategorizationValueDataRow["SegmentDynamicValueId"].ToString(),
                                            segmentCategorizationValueDataRow["IsActive"].ToString(),
                                            segmentCategorizationValueDataRow["CreatedBy"].ToString(),
                                            segmentCategorizationValueDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentCategorizationValueDataRow["CreationDate"]),
                                            segmentCategorizationValueDataRow["LastUpdatedBy"].ToString(),
                                            segmentCategorizationValueDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentCategorizationValueDataRow["LastupdatedDate"]),
                                            segmentCategorizationValueDataRow["Guid"].ToString(),
                                            segmentCategorizationValueDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(segmentCategorizationValueDataRow["site_id"]),
                                            segmentCategorizationValueDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(segmentCategorizationValueDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetSegmentCategorizationValueDTO(segmentCategorizationValueDataRow) Method.");
            return segmentCategorizationValueDataObject;
        }

        /// <summary>
        /// Gets the segment definition data of passed segment definition id
        /// </summary>
        /// <param name="SegmentCategoryValueId">integer type parameter</param>
        /// <returns>Returns SegmentCategorizationValueDTO</returns>
        public SegmentCategorizationValueDTO GetSegmentCategorizationValue(int SegmentCategoryValueId)
        {
            log.Debug("Starts-GetSegmentCategorizationValue(SegmentCategoryValueId) Method.");
            string selectSegmentCategorizationValueQuery = @"select *
                                         from Segment_Categorization_Values
                                        where SegmentCategoryValueId = @segmentCategoryValueId";
            SqlParameter[] selectSegmentCategorizationValueParameters = new SqlParameter[1];
            selectSegmentCategorizationValueParameters[0] = new SqlParameter("@segmentCategoryValueId", SegmentCategoryValueId);
            DataTable segmentCategorizationValue = dataAccessHandler.executeSelectQuery(selectSegmentCategorizationValueQuery, selectSegmentCategorizationValueParameters, sqlTransaction);
            if (segmentCategorizationValue.Rows.Count > 0)
            {
                DataRow segmentCategorizationValueRow = segmentCategorizationValue.Rows[0];
                SegmentCategorizationValueDTO segmentCategorizationValueDataObject = GetSegmentCategorizationValueDTO(segmentCategorizationValueRow);
                log.Debug("Ends-GetSegmentCategorizationValue(SegmentCategoryValueId) Method by returnting segmentCategorizationValueDataObject.");
                return segmentCategorizationValueDataObject;
            }
            else
            {
                log.Debug("Ends-GetSegmentCategorizationValue(SegmentCategoryValueId) Method by returnting null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the SegmentCategorizationValueDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SegmentCategorizationValueDTO matching the search criteria</returns>
        public List<SegmentCategorizationValueDTO> GetSegmentCategorizationValueList(List<KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetSegmentCategorizationValueList(searchParameters) Method.");
            int count = 0;
            string selectSegmentCategorizationValueQuery = @"select *
                                         from Segment_Categorization_Values";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_DEFINITION_ID)
                                || searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_CATEGORY_VALUE_ID)
                                || searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_CATEGORY_ID)
                                || searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_DYNAMIC_VALUE_ID)
                                || searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_STATIC_VALUE_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_DYNAMIC_VALUE_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' ");
                            }
                            else if (searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SITE_ID))
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
                            if (searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_DEFINITION_ID)
                                || searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_CATEGORY_VALUE_ID)
                                || searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_CATEGORY_ID)
                                || searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_STATIC_VALUE_ID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_DYNAMIC_VALUE_ID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' ");
                            }
                            else if (searchParameter.Key.Equals(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SITE_ID))
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
                        log.Debug("Ends-GetSegmentCategorizationValueList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectSegmentCategorizationValueQuery = selectSegmentCategorizationValueQuery + query;
            }

            DataTable segmentCategorizationValueData = dataAccessHandler.executeSelectQuery(selectSegmentCategorizationValueQuery, null, sqlTransaction);
            if (segmentCategorizationValueData.Rows.Count > 0)
            {
                List<SegmentCategorizationValueDTO> segmentCategorizationValueList = new List<SegmentCategorizationValueDTO>();
                foreach (DataRow segmentCategorizationValueDataRow in segmentCategorizationValueData.Rows)
                {
                    SegmentCategorizationValueDTO segmentCategorizationValueDataObject = GetSegmentCategorizationValueDTO(segmentCategorizationValueDataRow);
                    segmentCategorizationValueList.Add(segmentCategorizationValueDataObject);
                }
                log.Debug("Ends-GetSegmentCategorizationValueList(searchParameters) Method by returning segmentCategorizationValueList.");
                return segmentCategorizationValueList;
            }
            else
            {
                log.Debug("Ends-GetSegmentCategorizationValueList(searchParameters) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the SegmentCategorizationValueDTO list
        /// </summary>
        /// <param name="segmentDefinitionIds">Segment Category ids as comma saparated values</param>
        /// <param name="isActive">Active or inactive indication flag</param>
        /// <param name="siteId">site id of the site</param>
        /// <returns>Returns the list of SegmentCategorizationValueDTO matching the search criteria</returns>
        public List<SegmentCategorizationValueDTO> GetSegmentCategorizationValueListIfExists(string segmentDefinitionIds, string isActive, int siteId)
        {
            log.Debug("Starts-GetSegmentCategorizationValueList(segmentDefinitionIds) Method.");

            string selectSegmentCategorizationValueQuery = @"select *
                                         from Segment_Categorization_Values";

            StringBuilder query = new StringBuilder(" where SegmentDefinitionId in (" + segmentDefinitionIds + ") ");
            query.Append(" and IsActive='" + isActive + "'");
            query.Append(" and (site_id=" + siteId + " or " + siteId + " =-1)");
            selectSegmentCategorizationValueQuery = selectSegmentCategorizationValueQuery + query;
            DataTable segmentCategorizationValueData = dataAccessHandler.executeSelectQuery(selectSegmentCategorizationValueQuery, null, sqlTransaction);
            if (segmentCategorizationValueData.Rows.Count > 0)
            {
                List<SegmentCategorizationValueDTO> segmentCategorizationValueList = new List<SegmentCategorizationValueDTO>();
                foreach (DataRow segmentCategorizationValueDataRow in segmentCategorizationValueData.Rows)
                {
                    SegmentCategorizationValueDTO segmentCategorizationValueDataObject = GetSegmentCategorizationValueDTO(segmentCategorizationValueDataRow);
                    segmentCategorizationValueList.Add(segmentCategorizationValueDataObject);
                }
                log.Debug("Ends-GetSegmentCategorizationValueList(segmentDefinitionIds) Method by returning segmentCategorizationValueList.");
                return segmentCategorizationValueList;
            }
            else
            {
                log.Debug("Ends-GetSegmentCategorizationValueList(segmentDefinitionIds) Method by returning null.");
                return null;
            }
        }
    }
}
