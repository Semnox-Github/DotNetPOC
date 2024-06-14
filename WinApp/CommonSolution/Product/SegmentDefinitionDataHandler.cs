/********************************************************************************************
 * Project Name - Segment Definition Data Handler
 * Description  - Data handler of the segment definition data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        05-Apr-2016   Raghuveera          Created 
 *2.70        29-Mar-2019   Akshay Gulaganji    isActive Changed to bool and handled in this dataHandler
 *2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query
 *2.110.0     15-Oct-2020   Mushahid Faizan   Added methods for Pagination and modified as per 3 tier standard.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Segment Definition Data Handler - Handles insert, update and select of segment definition data objects
    /// </summary>
    public class SegmentDefinitionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Segment_Definition";
        private List<SqlParameter> parameters = new List<SqlParameter>();

        private static readonly Dictionary<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string> DBSearchParameters = new Dictionary<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>
               {
                    {SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEGMENT_DEFINITION_ID, "SegmentDefinitionId"},
                    {SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEGMENT_NAME, "SegmentName"},
                    {SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, "ApplicableEntity"},
                    {SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEQUENCE_ORDER, "SequenceOrder"},
                    {SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_MANDATORY,"IsMandatory"},
                    {SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "IsActive"},
                    {SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, "site_id"}
               };

        /// <summary>
        /// Default constructor of JobTaskGroupDataHandler class
        /// </summary>
        public SegmentDefinitionDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating SegmentDefinitionDTO parameters Record.
        /// </summary>
        /// <param name="segmentDefinitionDTO">SegmentDefinitionDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(SegmentDefinitionDTO segmentDefinitionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(segmentDefinitionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@segmentDefinitionId", segmentDefinitionDTO.SegmentDefinitionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@applicableEntity", string.IsNullOrEmpty(segmentDefinitionDTO.ApplicableEntity) ? DBNull.Value : (object)segmentDefinitionDTO.ApplicableEntity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@segmentName", string.IsNullOrEmpty(segmentDefinitionDTO.SegmentName) ? DBNull.Value : (object)segmentDefinitionDTO.SegmentName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sequenceOrder", string.IsNullOrEmpty(segmentDefinitionDTO.SequenceOrder) ? DBNull.Value : (object)segmentDefinitionDTO.SequenceOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isMandatory", string.IsNullOrEmpty(segmentDefinitionDTO.IsMandatory) ? "N" : segmentDefinitionDTO.IsMandatory));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (segmentDefinitionDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", segmentDefinitionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@synchStatus", segmentDefinitionDTO.SynchStatus));

            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the segment definition record to the database
        /// </summary>
        /// <param name="segmentDefinition">SegmentDefinitionDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public SegmentDefinitionDTO InsertSegmentDefinition(SegmentDefinitionDTO segmentDefinition, string userId, int siteId)
        {
            log.LogMethodEntry(segmentDefinition, userId, siteId);
            string insertSegmentDefinitionQuery = @"insert into Segment_Definition 
                                                        (
                                                        SegmentName,
                                                        ApplicableEntity,
                                                        SequenceOrder,
                                                        IsMandatory,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        site_id,
                                                        --SynchStatus,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                        @segmentName,
                                                        @applicableEntity,
                                                        @sequenceOrder,
                                                        @isMandatory,
                                                        @isActive,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        NewId(),
                                                        @siteId,
                                                      --  @synchStatus,
                                                        @masterEntityId
                                                        )SELECT * FROM Segment_Definition WHERE SegmentDefinitionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertSegmentDefinitionQuery, GetSQLParameters(segmentDefinition, userId, siteId).ToArray(), sqlTransaction);
                RefreshSegmentDefinitionDTO(segmentDefinition, dt);
                SaveInventoryActivityLog(segmentDefinition, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting segmentDefinition", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(segmentDefinition);
            return segmentDefinition;
        }
        /// <summary>
        /// Updates the segment definition record
        /// </summary>
        /// <param name="segmentDefinition">SegmentDefinitionDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public SegmentDefinitionDTO UpdateSegmentDefinition(SegmentDefinitionDTO segmentDefinition, string userId, int siteId)
        {
            log.LogMethodEntry(segmentDefinition, userId, siteId);
            string updateSegmentDefinitionQuery = @"update Segment_Definition 
                                         set SegmentName=@segmentName,
                                             ApplicableEntity=@applicableEntity,
                                             SequenceOrder=@sequenceOrder,
                                             IsMandatory=@isMandatory,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             -- site_id=@siteid,
                                            -- SynchStatus = @synchStatus,                                             
                                             MasterEntityId = @masterEntityId                                             
                             where SegmentDefinitionId = @segmentDefinitionId
                                       SELECT * FROM Segment_Definition WHERE SegmentDefinitionId = @segmentDefinitionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateSegmentDefinitionQuery, GetSQLParameters(segmentDefinition, userId, siteId).ToArray(), sqlTransaction);
                RefreshSegmentDefinitionDTO(segmentDefinition, dt);
                SaveInventoryActivityLog(segmentDefinition, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting segmentDefinition", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(segmentDefinition);
            return segmentDefinition;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="segmentDefinitionDTO">SegmentDefinitionDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshSegmentDefinitionDTO(SegmentDefinitionDTO segmentDefinitionDTO, DataTable dt)
        {
            log.LogMethodEntry(segmentDefinitionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                segmentDefinitionDTO.SegmentDefinitionId = Convert.ToInt32(dt.Rows[0]["SegmentDefinitionId"]);
                segmentDefinitionDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                segmentDefinitionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                segmentDefinitionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                segmentDefinitionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                segmentDefinitionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                segmentDefinitionDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to SegmentDefinitionDTO class type
        /// </summary>
        /// <param name="segmentDefinitionDataRow">SegmentDefinitionDTO DataRow</param>
        /// <returns>Returns SegmentDefinitionDTO</returns>
        private SegmentDefinitionDTO GetSegmentDefinitionDTO(DataRow segmentDefinitionDataRow)
        {
            log.LogMethodEntry(segmentDefinitionDataRow);
            SegmentDefinitionDTO segmentDefinitionDataObject = new SegmentDefinitionDTO(Convert.ToInt32(segmentDefinitionDataRow["SegmentDefinitionId"]),
                                            segmentDefinitionDataRow["SegmentName"].ToString(),
                                            segmentDefinitionDataRow["ApplicableEntity"].ToString(),
                                            segmentDefinitionDataRow["SequenceOrder"].ToString(),
                                            segmentDefinitionDataRow["IsMandatory"].ToString(),
                                            segmentDefinitionDataRow["IsActive"] == DBNull.Value ? true : segmentDefinitionDataRow["IsActive"].ToString() == "Y",
                                            segmentDefinitionDataRow["CreatedBy"].ToString(),
                                            segmentDefinitionDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentDefinitionDataRow["CreationDate"]),
                                            segmentDefinitionDataRow["LastUpdatedBy"].ToString(),
                                            segmentDefinitionDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(segmentDefinitionDataRow["LastupdatedDate"]),
                                            segmentDefinitionDataRow["Guid"].ToString(),
                                            segmentDefinitionDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(segmentDefinitionDataRow["site_id"]),
                                            segmentDefinitionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(segmentDefinitionDataRow["SynchStatus"])
                                            );
            log.LogMethodExit(segmentDefinitionDataObject);
            return segmentDefinitionDataObject;
        }

        /// <summary>
        /// Gets the segment definition data of passed segment definition id
        /// </summary>
        /// <param name="SegmentDefinitionId">integer type parameter</param>
        /// <returns>Returns SegmentDefinitionDTO</returns>
        public SegmentDefinitionDTO GetSegmentDefinition(int SegmentDefinitionId)
        {
            log.Debug("Starts-GetSegmentDefinition(SegmentDefinitionId) Method.");
            string selectSegmentDefinitionQuery = @"select *
                                         from Segment_Definition
                                        where SegmentDefinitionId = @segmentDefinitionId";
            SqlParameter[] selectSegmentDefinitionParameters = new SqlParameter[1];
            selectSegmentDefinitionParameters[0] = new SqlParameter("@segmentDefinitionId", SegmentDefinitionId);
            DataTable segmentDefinition = dataAccessHandler.executeSelectQuery(selectSegmentDefinitionQuery, selectSegmentDefinitionParameters);
            if (segmentDefinition.Rows.Count > 0)
            {
                DataRow segmentDefinitionRow = segmentDefinition.Rows[0];
                SegmentDefinitionDTO segmentDefinitionDataObject = GetSegmentDefinitionDTO(segmentDefinitionRow);
                log.Debug("Ends-GetSegmentDefinition(SegmentDefinitionId) Method by returnting segmentDefinitionDataObject.");
                return segmentDefinitionDataObject;
            }
            else
            {
                log.Debug("Ends-GetSegmentDefinition(SegmentDefinitionId) Method by returnting null.");
                return null;
            }
        }

        /// <summary>
        /// Returns the no of ApprovalRuleDTO matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetSegmentDefinitionCount(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters)
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
        public string GetFilterQuery(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEGMENT_DEFINITION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEGMENT_NAME
                           || searchParameter.Key == SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY
                           || searchParameter.Key == SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_MANDATORY
                           || searchParameter.Key == SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEQUENCE_ORDER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE)
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
        /// Gets the SegmentDefinitionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SegmentDefinitionDTO matching the search criteria</returns>
        public List<SegmentDefinitionDTO> GetSegmentDefinitionList(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY Segment_Definition.SegmentDefinitionId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SegmentDefinitionDTO segmentDefinitionDTO = GetSegmentDefinitionDTO(dataRow);
                    segmentDefinitionDTOList.Add(segmentDefinitionDTO);
                }
            }
            log.LogMethodExit(segmentDefinitionDTOList);
            return segmentDefinitionDTOList;
        }


        /// <summary>
        /// Gets the SegmentDefinitionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SegmentDefinitionDTO matching the search criteria</returns>
        public List<SegmentDefinitionDTO> GetSegmentDefinitionList(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SegmentDefinitionDTO segmentDefinitionDTO = GetSegmentDefinitionDTO(dataRow);
                    segmentDefinitionDTOList.Add(segmentDefinitionDTO);
                }
            }
            log.LogMethodExit(segmentDefinitionDTOList);
            return segmentDefinitionDTOList;
        }

        /// <summary>
        ///  Inserts the record to the InventoryActivityLogDTO Table.
        /// </summary>
        /// <param name="segmentDefinitionInventoryActivityLogDTO">inventoryActivityLogDTO object passed as the Parameter</param>
        /// <param name="loginId">login id of the user </param>
        /// <param name="siteId">site id of the user</param>
        public void SaveInventoryActivityLog(SegmentDefinitionDTO segmentDefinitionInventoryActivityLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(segmentDefinitionInventoryActivityLogDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[InventoryActivityLog]
                           (TimeStamp,
                            Message,
                            Guid,
                            site_id,
                            SourceTableName,
                            InvTableKey,
                            SourceSystemId,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@TimeStamp,
                            @Message,
                            @Guid,
                            @site_id,
                            @SourceTableName,
                            @InvTableKey,
                            @SourceSystemId,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()
                            )SELECT CAST(scope_identity() AS int)";

            try
            {
                List<SqlParameter> segmentDefinitionInventoryActivityLogParameters = new List<SqlParameter>();
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@InvTableKey", DBNull.Value));
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@Message", "SegmentDefinition Inserted"));
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@SourceSystemId", segmentDefinitionInventoryActivityLogDTO.SegmentDefinitionId.ToString() + ":" + segmentDefinitionInventoryActivityLogDTO.SegmentName));
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@SourceTableName", "Segment_Definition"));
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@TimeStamp", ServerDateTime.Now));
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", segmentDefinitionInventoryActivityLogDTO.MasterEntityId, true));
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
                segmentDefinitionInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@Guid", segmentDefinitionInventoryActivityLogDTO.Guid));
                log.Debug(segmentDefinitionInventoryActivityLogParameters);

                object rowInserted = dataAccessHandler.executeScalar(query, segmentDefinitionInventoryActivityLogParameters.ToArray(), sqlTransaction);
                log.LogMethodExit(rowInserted);

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryActivityLog ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
    }
}
