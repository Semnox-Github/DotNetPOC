/********************************************************************************************
 * Project Name - Segment Definition Source Value Data Handler
 * Description  - Data handler of the segment definition source value data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        07-Apr-2016   Raghuveera          Created 
 *2.70        25-Mar-2019   Nagesh Badiger      IsActive Changed to bool
 *2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query
 *2.110.0     15-Oct-2020   Mushahid Faizan     Modified 3 tier changes as per standards.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Segment Definition Source Value Data Handler - Handles insert, update and select of segment definition source value data objects
    /// </summary>
    public class SegmentDefinitionSourceValueDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Segment_Definition_Source_Values";
        private List<SqlParameter> parameters = new List<SqlParameter>();

        private static readonly Dictionary<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string> DBSearchParameters = new Dictionary<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>
               {
                    {SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_VALUE_ID, "SegmentDefinitionSourceValueId"},
                    {SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_ID, "SegmentDefinitionSourceId"},
                    {SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.IS_ACTIVE, "IsActive"},
                    {SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SITE_ID, "site_id"}
               };
        /// <summary>
        /// Default constructor of SegmentDefinitionSourceValueDataHandler class
        /// </summary>
        public SegmentDefinitionSourceValueDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating SegmentDefinitionSourceValueDTO parameters Record.
        /// </summary>
        /// <param name="segmentDefinitionSourceValueDTO">SegmentDefinitionSourceValueDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(segmentDefinitionSourceValueDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@segmentDefinitionSourceId", segmentDefinitionSourceValueDTO.SegmentDefinitionSourceId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@segmentDefinitionSourceValueId", segmentDefinitionSourceValueDTO.SegmentDefinitionSourceValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@applicableEntity", string.IsNullOrEmpty(segmentDefinitionSourceValueDTO.ListValue) ? DBNull.Value : (object)segmentDefinitionSourceValueDTO.ListValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dBQuery", string.IsNullOrEmpty(segmentDefinitionSourceValueDTO.DBQuery) ? DBNull.Value : (object)segmentDefinitionSourceValueDTO.DBQuery));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(segmentDefinitionSourceValueDTO.Description) ? DBNull.Value : (object)segmentDefinitionSourceValueDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (segmentDefinitionSourceValueDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", segmentDefinitionSourceValueDTO.MasterEntityId, true));

            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the segment definition source value record to the database
        /// </summary>
        /// <param name="segmentDefinitionSourceValue">SegmentDefinitionSourceValueDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public SegmentDefinitionSourceValueDTO InsertSegmentDefinitionSourceValue(SegmentDefinitionSourceValueDTO segmentDefinitionSourceValue, string userId, int siteId)
        {
            log.LogMethodEntry(segmentDefinitionSourceValue, userId, siteId);
            string insertSegmentDefinitionSourceValueQuery = @"insert into Segment_Definition_Source_Values 
                                                        (
                                                        SegmentDefinitionSourceId,
                                                        ListValue,
                                                        DBQuery,
                                                        Description,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                        @segmentDefinitionSourceId,
                                                        @applicableEntity,
                                                        @dBQuery,
                                                        @description,
                                                        @isActive,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @masterEntityId
                                                        )SELECT * FROM Segment_Definition_Source_Values WHERE SegmentDefinitionSourceValueId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertSegmentDefinitionSourceValueQuery, GetSQLParameters(segmentDefinitionSourceValue, userId, siteId).ToArray(), sqlTransaction);
                RefreshSegmentDefinitionSourceValueDTO(segmentDefinitionSourceValue, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting segmentDefinitionSourceValue", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(segmentDefinitionSourceValue);
            return segmentDefinitionSourceValue;
        }

        /// <summary>
        /// Updates the segment definition source value record
        /// </summary>
        /// <param name="segmentDefinitionSourceValue">SegmentDefinitionSourceValueDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public SegmentDefinitionSourceValueDTO UpdateSegmentDefinitionSourceValue(SegmentDefinitionSourceValueDTO segmentDefinitionSourceValue, string userId, int siteId)
        {
            log.LogMethodEntry(segmentDefinitionSourceValue, userId, siteId);
            string updateSegmentDefinitionSourceValueQuery = @"update Segment_Definition_Source_Values 
                                         set SegmentDefinitionSourceId=@segmentDefinitionSourceId,
                                             ListValue=@applicableEntity,
                                             DBQuery=@dBQuery,
                                             Description=@description,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             MasterEntityId = @masterEntityId  
                                             -- site_id=@siteid                    
                                            where SegmentDefinitionSourceValueId = @segmentDefinitionSourceValueId
                                        SELECT * FROM Segment_Definition_Source_Values WHERE SegmentDefinitionSourceValueId = @segmentDefinitionSourceValueId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateSegmentDefinitionSourceValueQuery, GetSQLParameters(segmentDefinitionSourceValue, userId, siteId).ToArray(), sqlTransaction);
                RefreshSegmentDefinitionSourceValueDTO(segmentDefinitionSourceValue, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating segmentDefinitionSourceValue", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(segmentDefinitionSourceValue);
            return segmentDefinitionSourceValue;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="segmentDefinitionSourceValueDTO">segmentDefinitionSourceValueDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshSegmentDefinitionSourceValueDTO(SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDTO, DataTable dt)
        {
            log.LogMethodEntry(segmentDefinitionSourceValueDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                segmentDefinitionSourceValueDTO.SegmentDefinitionSourceValueId = Convert.ToInt32(dt.Rows[0]["SegmentDefinitionSourceValueId"]);
                segmentDefinitionSourceValueDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                segmentDefinitionSourceValueDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                segmentDefinitionSourceValueDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                segmentDefinitionSourceValueDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                segmentDefinitionSourceValueDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                segmentDefinitionSourceValueDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of SegmentDefinitionSourceValueDTO matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of SegmentDefinitionSourceValue matching the criteria</returns>
        public int GetSegmentDefinitionSourceValueCount(List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                count = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Returns the sql query based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of query</returns>
        public string GetFilterQuery(List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_ID
                            || searchParameter.Key == SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_VALUE_ID
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Converts the Data row object to SegmentDefinitionSourceValueDTO class type
        /// </summary>
        /// <param name="segmentDefinitionSourceValueDataRow">SegmentDefinitionSourceValueDTO DataRow</param>
        /// <returns>Returns SegmentDefinitionSourceValueDTO</returns>
        private SegmentDefinitionSourceValueDTO GetSegmentDefinitionSourceValueDTO(DataRow segmentDefinitionSourceValueDataRow)
        {
            log.LogMethodEntry(segmentDefinitionSourceValueDataRow);
            SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDataObject = new SegmentDefinitionSourceValueDTO(Convert.ToInt32(segmentDefinitionSourceValueDataRow["SegmentDefinitionSourceValueId"]),
                                            segmentDefinitionSourceValueDataRow["SegmentDefinitionSourceId"] == DBNull.Value ? -1 : Convert.ToInt32(segmentDefinitionSourceValueDataRow["SegmentDefinitionSourceId"]),
                                            segmentDefinitionSourceValueDataRow["ListValue"].ToString(),
                                            segmentDefinitionSourceValueDataRow["DBQuery"].ToString(),
                                            segmentDefinitionSourceValueDataRow["Description"].ToString(),
                                            segmentDefinitionSourceValueDataRow["IsActive"] == DBNull.Value ? true : segmentDefinitionSourceValueDataRow["IsActive"].ToString() == "Y",
                                            segmentDefinitionSourceValueDataRow["CreatedBy"].ToString(),
                                            segmentDefinitionSourceValueDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentDefinitionSourceValueDataRow["CreationDate"]),
                                            segmentDefinitionSourceValueDataRow["LastUpdatedBy"].ToString(),
                                            segmentDefinitionSourceValueDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentDefinitionSourceValueDataRow["LastupdatedDate"]),
                                            segmentDefinitionSourceValueDataRow["Guid"].ToString(),
                                            segmentDefinitionSourceValueDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(segmentDefinitionSourceValueDataRow["site_id"]),
                                            segmentDefinitionSourceValueDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(segmentDefinitionSourceValueDataRow["SynchStatus"])
                                            );
            log.LogMethodExit(segmentDefinitionSourceValueDataObject);
            return segmentDefinitionSourceValueDataObject;
        }

        /// <summary>
        /// Gets the segment definition source value data of passed segment definition source value id
        /// </summary>
        /// <param name="SegmentDefinitionSourceValueId">integer type parameter</param>
        /// <returns>Returns SegmentDefinitionSourceValueDTO</returns>
        public SegmentDefinitionSourceValueDTO GetSegmentDefinitionSourceValue(int SegmentDefinitionSourceValueId)
        {
            log.LogMethodEntry(SegmentDefinitionSourceValueId);
            string selectSegmentDefinitionSourceValueQuery = @"select *
                                         from Segment_Definition_Source_Values
                                        where SegmentDefinitionSourceValueId = @segmentDefinitionSourceValueId";
            SqlParameter[] selectSegmentDefinitionSourceValueParameters = new SqlParameter[1];
            selectSegmentDefinitionSourceValueParameters[0] = new SqlParameter("@segmentDefinitionSourceValueId", SegmentDefinitionSourceValueId);
            DataTable segmentDefinitionSourceValue = dataAccessHandler.executeSelectQuery(selectSegmentDefinitionSourceValueQuery, selectSegmentDefinitionSourceValueParameters);
            if (segmentDefinitionSourceValue.Rows.Count > 0)
            {
                DataRow segmentDefinitionSourceValueRow = segmentDefinitionSourceValue.Rows[0];
                SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDataObject = GetSegmentDefinitionSourceValueDTO(segmentDefinitionSourceValueRow);
                log.LogMethodExit(segmentDefinitionSourceValueDataObject);
                return segmentDefinitionSourceValueDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the SegmentDefinitionSourceValueDTO List for segmentDefinitionSource Id List
        /// </summary>
        /// <param name="segmentDefinitionSourceIdList">integer list parameter</param>
        /// <returns>Returns List of SegmentDefinitionSourceValueDTO</returns>
        public List<SegmentDefinitionSourceValueDTO> GetSegmentDefinitionSourceValueList(List<int> segmentDefinitionSourceIdList, bool activeRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(segmentDefinitionSourceIdList);
            List<SegmentDefinitionSourceValueDTO> list = new List<SegmentDefinitionSourceValueDTO>();
            string query = @"SELECT Segment_Definition_Source_Values.*
                            FROM Segment_Definition_Source_Values, @segmentDefinitionSourceIdList List
                            WHERE SegmentDefinitionSourceId = List.Id ";
            if (activeRecords)
            {
                query += " AND Isnull(IsActive,'Y') = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@segmentDefinitionSourceIdList", segmentDefinitionSourceIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetSegmentDefinitionSourceValueDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the SegmentDefinitionSourceValueDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SegmentDefinitionSourceValueDTO matching the search criteria</returns>
        public List<SegmentDefinitionSourceValueDTO> GetSegmentDefinitionSourceValueList(List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable segmentDefinitionSourceValueData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (segmentDefinitionSourceValueData.Rows.Count > 0)
            {
                foreach (DataRow segmentDefinitionSourceValueDataRow in segmentDefinitionSourceValueData.Rows)
                {
                    SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDataObject = GetSegmentDefinitionSourceValueDTO(segmentDefinitionSourceValueDataRow);
                    segmentDefinitionSourceValueDTOList.Add(segmentDefinitionSourceValueDataObject);
                }
            }
            log.LogMethodExit(segmentDefinitionSourceValueDTOList);
            return segmentDefinitionSourceValueDTOList;

        }

        /// <summary>
        /// Gets the SegmentDefinitionSourceValueDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SegmentDefinitionSourceValueDTO matching the search criteria</returns>
        public List<SegmentDefinitionSourceValueDTO> GetSegmentDefinitionSourceValueList(List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            selectQuery += " ORDER BY Segment_Definition_Source_Values.SegmentDefinitionSourceValueId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
            selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            DataTable segmentDefinitionSourceValueData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (segmentDefinitionSourceValueData.Rows.Count > 0)
            {
                foreach (DataRow segmentDefinitionSourceValueDataRow in segmentDefinitionSourceValueData.Rows)
                {
                    SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDataObject = GetSegmentDefinitionSourceValueDTO(segmentDefinitionSourceValueDataRow);
                    segmentDefinitionSourceValueDTOList.Add(segmentDefinitionSourceValueDataObject);
                }
            }
            log.LogMethodExit(segmentDefinitionSourceValueDTOList);
            return segmentDefinitionSourceValueDTOList;

        }
    }
}
