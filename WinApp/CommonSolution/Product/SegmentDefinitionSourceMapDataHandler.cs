/********************************************************************************************
 * Project Name - Segment Definition Source Map Data Handler
 * Description  - Data handler of the segment definition source map data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Apr-2016   Raghuveera          Created 
 *2.60        25-Mar-2019   Nagesh Badiger      IsActive Changed string to bool
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
    /// Segment Definition Source Map Data Handler - Handles insert, update and select of segment definition source map data objects
    /// </summary>
    public class SegmentDefinitionSourceMapDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Segment_Definition_Source_Mapping as sm";

        private static readonly Dictionary<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string> DBSearchParameters = new Dictionary<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>
               {
                    {SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_SOURCE_ID, "sm.SegmentDefinitionSourceId"},
                    {SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_ID, "sm.SegmentDefinitionId"},
                    {SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.DATA_SOURCE_TYPE, "sm.DataSourceType"},
                    {SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.DATA_SOURCE_ENTITY, "sm.DataSourceEntity"},
                    {SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.DATA_SOURCE_COLUMN,"sm.DataSourceColumn"},
                    {SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.IS_ACTIVE, "sm.IsActive"},
                    {SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SITE_ID, "sm.site_id"},
                    {SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_APPLICABILITY, "sd.ApplicableEntity"}
               };
        /// <summary>
        /// Default constructor of SegmentDefinitionSourceMapDataHandler class
        /// </summary>
        public SegmentDefinitionSourceMapDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating screenZoneDefSetupDTO parameters Record.
        /// </summary>
        /// <param name="segmentDefinitionSourceValueDTO">screenZoneDefSetupDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(SegmentDefinitionSourceMapDTO segmentDefinitionSourceValueDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(segmentDefinitionSourceValueDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@segmentDefinitionSourceId", segmentDefinitionSourceValueDTO.SegmentDefinitionSourceId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@segmentDefinitionId", segmentDefinitionSourceValueDTO.SegmentDefinitionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataSourceType", string.IsNullOrEmpty(segmentDefinitionSourceValueDTO.DataSourceType) ? DBNull.Value : (object)segmentDefinitionSourceValueDTO.DataSourceType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataSourceEntity", string.IsNullOrEmpty(segmentDefinitionSourceValueDTO.DataSourceEntity) ? DBNull.Value : (object)segmentDefinitionSourceValueDTO.DataSourceEntity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataSourceColumn", string.IsNullOrEmpty(segmentDefinitionSourceValueDTO.DataSourceColumn) ? DBNull.Value : (object)segmentDefinitionSourceValueDTO.DataSourceColumn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (segmentDefinitionSourceValueDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", segmentDefinitionSourceValueDTO.MasterEntityId, true));

            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the segment definition source map record to the database
        /// </summary>
        /// <param name="segmentDefinitionSourceMap">SegmentDefinitionSourceMapDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public SegmentDefinitionSourceMapDTO InsertSegmentDefinitionSourceMap(SegmentDefinitionSourceMapDTO segmentDefinitionSourceMap, string userId, int siteId)
        {
            log.LogMethodEntry(segmentDefinitionSourceMap, userId, siteId);
            string insertSegmentDefinitionSourceMapQuery = @"insert into Segment_Definition_Source_Mapping 
                                                        (                                                        
                                                        SegmentDefinitionId,
                                                        DataSourceType,
                                                        DataSourceEntity,
                                                        DataSourceColumn,
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
                                                        @segmentDefinitionId,
                                                        @dataSourceType,
                                                        @dataSourceEntity,
                                                        @dataSourceColumn,
                                                        @isActive,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @masterEntityId
                                                        )SELECT * FROM Segment_Definition_Source_Mapping WHERE SegmentDefinitionSourceId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertSegmentDefinitionSourceMapQuery, GetSQLParameters(segmentDefinitionSourceMap, userId, siteId).ToArray(), sqlTransaction);
                RefreshSegmentDefinitionSourceMapDTO(segmentDefinitionSourceMap, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting segmentDefinitionSourceMap", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(segmentDefinitionSourceMap);
            return segmentDefinitionSourceMap;
        }
        /// <summary>
        /// Updates the segment definition source map record
        /// </summary>
        /// <param name="segmentDefinitionSourceMap">SegmentDefinitionSourceMapDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public SegmentDefinitionSourceMapDTO UpdateSegmentDefinitionSourceMap(SegmentDefinitionSourceMapDTO segmentDefinitionSourceMap, string userId, int siteId)
        {
            log.LogMethodEntry(segmentDefinitionSourceMap, userId, siteId);
            string updateSegmentDefinitionSourceMapQuery = @"update Segment_Definition_Source_Mapping 
                                         set SegmentDefinitionId=@segmentDefinitionId,
                                             DataSourceType=@dataSourceType,
                                             DataSourceEntity=@dataSourceEntity,
                                             DataSourceColumn=@dataSourceColumn,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             MasterEntityId = @masterEntityId
                                             -- site_id=@siteid                                             
                                         where SegmentDefinitionSourceId = @segmentDefinitionSourceId
                                       SELECT * FROM Segment_Definition_Source_Mapping WHERE SegmentDefinitionSourceId = @segmentDefinitionSourceId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateSegmentDefinitionSourceMapQuery, GetSQLParameters(segmentDefinitionSourceMap, userId, siteId).ToArray(), sqlTransaction);
                RefreshSegmentDefinitionSourceMapDTO(segmentDefinitionSourceMap, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating segmentDefinitionSourceMap", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(segmentDefinitionSourceMap);
            return segmentDefinitionSourceMap;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="segmentDefinitionSourceMapDTO">SegmentDefinitionSourceMapDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshSegmentDefinitionSourceMapDTO(SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDTO, DataTable dt)
        {
            log.LogMethodEntry(segmentDefinitionSourceMapDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId = Convert.ToInt32(dt.Rows[0]["SegmentDefinitionSourceId"]);
                segmentDefinitionSourceMapDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                segmentDefinitionSourceMapDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                segmentDefinitionSourceMapDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                segmentDefinitionSourceMapDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                segmentDefinitionSourceMapDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                segmentDefinitionSourceMapDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to SegmentDefinitionSourceMapDTO class type
        /// </summary>
        /// <param name="segmentDefinitionSourceMapDataRow">SegmentDefinitionSourceMapDTO DataRow</param>
        /// <returns>Returns SegmentDefinitionSourceMapDTO</returns>
        private SegmentDefinitionSourceMapDTO GetSegmentDefinitionSourceMapDTO(DataRow segmentDefinitionSourceMapDataRow)
        {
            log.Debug("Starts-GetSegmentDefinitionSourceMapDTO(segmentDefinitionSourceMapDataRow) Method.");
            SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDataObject = new SegmentDefinitionSourceMapDTO(Convert.ToInt32(segmentDefinitionSourceMapDataRow["SegmentDefinitionSourceId"]),
                                            segmentDefinitionSourceMapDataRow["SegmentDefinitionId"] == DBNull.Value ? -1 : Convert.ToInt32(segmentDefinitionSourceMapDataRow["SegmentDefinitionId"]),
                                            segmentDefinitionSourceMapDataRow["DataSourceType"].ToString(),
                                            segmentDefinitionSourceMapDataRow["DataSourceEntity"].ToString(),
                                            segmentDefinitionSourceMapDataRow["DataSourceColumn"].ToString(),
                                            //segmentDefinitionSourceMapDataRow["IsActive"].ToString(),
                                            segmentDefinitionSourceMapDataRow["IsActive"] == DBNull.Value ? true : segmentDefinitionSourceMapDataRow["IsActive"].ToString() == "Y",
                                            segmentDefinitionSourceMapDataRow["CreatedBy"].ToString(),
                                            segmentDefinitionSourceMapDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentDefinitionSourceMapDataRow["CreationDate"]),
                                            segmentDefinitionSourceMapDataRow["LastUpdatedBy"].ToString(),
                                            segmentDefinitionSourceMapDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentDefinitionSourceMapDataRow["LastupdatedDate"]),
                                            segmentDefinitionSourceMapDataRow["Guid"].ToString(),
                                            segmentDefinitionSourceMapDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(segmentDefinitionSourceMapDataRow["site_id"]),
                                            segmentDefinitionSourceMapDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(segmentDefinitionSourceMapDataRow["SynchStatus"])
                                            );
            log.LogMethodExit(segmentDefinitionSourceMapDataObject);
            return segmentDefinitionSourceMapDataObject;
        }

        /// <summary>
        /// Gets the segment definition source map data of passed segment definition source map id
        /// </summary>
        /// <param name="SegmentDefinitionSourceMapId">integer type parameter</param>
        /// <returns>Returns SegmentDefinitionSourceMapDTO</returns>
        public SegmentDefinitionSourceMapDTO GetSegmentDefinitionSourceMap(int SegmentDefinitionSourceMapId)
        {
            log.LogMethodEntry(SegmentDefinitionSourceMapId);
            string selectSegmentDefinitionSourceMapQuery = @"select *
                                         from Segment_Definition_Source_Mapping
                                        where SegmentDefinitionSourceMapId = @segmentDefinitionSourceMapId";
            SqlParameter[] selectSegmentDefinitionSourceMapParameters = new SqlParameter[1];
            selectSegmentDefinitionSourceMapParameters[0] = new SqlParameter("@segmentDefinitionSourceMapId", SegmentDefinitionSourceMapId);
            DataTable segmentDefinitionSourceMap = dataAccessHandler.executeSelectQuery(selectSegmentDefinitionSourceMapQuery, selectSegmentDefinitionSourceMapParameters);
            if (segmentDefinitionSourceMap.Rows.Count > 0)
            {
                DataRow segmentDefinitionSourceMapRow = segmentDefinitionSourceMap.Rows[0];
                SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDataObject = GetSegmentDefinitionSourceMapDTO(segmentDefinitionSourceMapRow);
                log.LogMethodExit(segmentDefinitionSourceMapDataObject);
                return segmentDefinitionSourceMapDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the SegmentDefinitionSourceMapDTO List for segmentDefinition Id List
        /// </summary>
        /// <param name="segmentDefinitionIdList">integer list parameter</param>
        /// <returns>Returns List of SegmentDefinitionSourceMapDTO</returns>
        public List<SegmentDefinitionSourceMapDTO> GetSegmentDefinitionSourceMapDTOList(List<int> segmentDefinitionIdList, bool activeRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(segmentDefinitionIdList);
            List<SegmentDefinitionSourceMapDTO> list = new List<SegmentDefinitionSourceMapDTO>();
            string query = @"SELECT Segment_Definition_Source_Mapping.*
                            FROM Segment_Definition_Source_Mapping, @segmentDefinitionIdList List
                            WHERE SegmentDefinitionId = List.Id ";
            if (activeRecords)
            {
                query += " AND Isnull(IsActive,'Y') = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@segmentDefinitionIdList", segmentDefinitionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetSegmentDefinitionSourceMapDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the SegmentDefinitionSourceMapDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SegmentDefinitionSourceMapDTO matching the search criteria</returns>
        public List<SegmentDefinitionSourceMapDTO> GetSegmentDefinitionSourceMapList(List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectSegmentDefinitionSourceMapQuery = @"select sm.* from Segment_Definition_Source_Mapping sm 
                                                            join Segment_Definition sd on sm.SegmentDefinitionId=sd.SegmentDefinitionId";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_ID) || searchParameter.Key.Equals(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_SOURCE_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_APPLICABILITY))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SITE_ID))
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.IS_ACTIVE)
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N") + "'");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_ID) || searchParameter.Key.Equals(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_SOURCE_ID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_APPLICABILITY))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SITE_ID))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.IS_ACTIVE)
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N") + "'");
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
                        log.Debug("Ends-GetSegmentDefinitionSourceMapList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectSegmentDefinitionSourceMapQuery = selectSegmentDefinitionSourceMapQuery + query;
            }

            DataTable segmentDefinitionSourceMapData = dataAccessHandler.executeSelectQuery(selectSegmentDefinitionSourceMapQuery, null);
            if (segmentDefinitionSourceMapData.Rows.Count > 0)
            {
                List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapList = new List<SegmentDefinitionSourceMapDTO>();
                foreach (DataRow segmentDefinitionSourceMapDataRow in segmentDefinitionSourceMapData.Rows)
                {
                    SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDataObject = GetSegmentDefinitionSourceMapDTO(segmentDefinitionSourceMapDataRow);
                    segmentDefinitionSourceMapList.Add(segmentDefinitionSourceMapDataObject);
                }
                log.LogMethodExit(segmentDefinitionSourceMapList);
                return segmentDefinitionSourceMapList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}
