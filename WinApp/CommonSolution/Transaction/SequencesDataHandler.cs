/********************************************************************************************
 * Project Name - Sequences Data Handler
 * Description  - Data Handler for Sequences
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.60        17-Mar-2019   Jagan Mohana     Created 
 *            13-May-2019   Mushahid Faizan  Modified GetSQLParameters() method.
 *                                           Added logMethodEntry/Exit. and IsActive DBSearchParameter.
 *                                           Modified Insert/Update query i.e TableName, Column Name mismatch
2.100.0       12-oct-2020   Deeksha          Added  Sequence number generation logic 
 *2.110.0     20-Feb-2020   Dakshakh Raj     Modified: Get Sequence method changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace Semnox.Parafait.Transaction
{
    public class SequencesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<SequencesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SequencesDTO.SearchByParameters, string>
            {
                {SequencesDTO.SearchByParameters.SEQUENCE_ID, "SequenceId"},
                {SequencesDTO.SearchByParameters.SEQUENCE_NAME, "SeqName"},
                {SequencesDTO.SearchByParameters.ORDER_TYPE_GROUP_ID, "OrderTypeGroupId"},
                {SequencesDTO.SearchByParameters.SITE_ID, "site_id"},
                {SequencesDTO.SearchByParameters.PREFIX, "Prefix"},
                {SequencesDTO.SearchByParameters.SUFFIX, "Suffix"},
                {SequencesDTO.SearchByParameters.ISACTIVE, "IsActive"},
                {SequencesDTO.SearchByParameters.POS_MACHINE_ID, "POSMachineId"},
                {SequencesDTO.SearchByParameters.GUID, "Guid"}
            };
        private const string SELECT_QUERY = @"SELECT * from Sequences AS seq";
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of SequencesDataHandler class
        /// </summary>
        public SequencesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the LookupsDTO data of passed id 
        /// </summary>
        /// <param name="id">id of Lookups is passed as parameter</param>
        /// <returns>Returns LookupsDTO</returns>
        public SequencesDTO GetSequenceDTO(int id)
        {
            log.LogMethodEntry(id);
            SequencesDTO result = null;
            string query = SELECT_QUERY + @" WHERE seq.SequenceId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetSequencesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Sequneces Record.
        /// </summary>
        /// <param name="sequencesDTO">SequencesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(SequencesDTO sequencesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(sequencesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@sequenceId", sequencesDTO.SequenceId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@seqName", sequencesDTO.SequenceName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@seed", sequencesDTO.Seed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@incr", sequencesDTO.IncrementBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currval", sequencesDTO.CurrentVal));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@prefix", sequencesDTO.Prefix));
            parameters.Add(dataAccessHandler.GetSQLParameter("@suffix", sequencesDTO.Suffix));
            parameters.Add(dataAccessHandler.GetSQLParameter("@width", sequencesDTO.Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userColumnHeading", sequencesDTO.UserColumnHeading));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posMachineId", sequencesDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maximumValue", sequencesDTO.MaximumValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", sequencesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@orderTypeGroupId", sequencesDTO.OrderTypeGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", sequencesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@synchStatus", sequencesDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the InsertSequences record to the database
        /// </summary>
        /// <param name="sequencesDTO">SequencesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public SequencesDTO InsertSequences(SequencesDTO sequencesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(sequencesDTO, loginId, siteId);
            string query = @"insert into Sequences 
                                                        (
                                                          SeqName,
                                                          Seed,
                                                          Incr,
                                                          Currval,
                                                          Guid,
                                                          SynchStatus,
                                                          site_id,
                                                          Prefix,
                                                          Suffix,
                                                          Width,
                                                          UserColumnHeading,                                                          
                                                          POSMachineId,
                                                          MaximumValue,
                                                          MasterEntityId,
                                                          OrderTypeGroupId,
                                                          CreatedBy, 
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastUpdateDate,
                                                          IsActive
                                                        ) 
                                                values 
                                                        (
                                                          @seqName,
                                                          @seed,
                                                          @incr,
                                                          @currval,
                                                          NewId(),
                                                          NULL,
                                                          @siteId,
                                                          @prefix,
                                                          @suffix,
                                                          @width,
                                                          @userColumnHeading,
                                                          @posMachineId,
                                                          @maximumValue,
                                                          @masterEntityId,
                                                          @orderTypeGroupId,
                                                          @createdBy,
                                                          GETDATE(),                                                        
                                                          @lastUpdatedBy,
                                                          GetDate(),
                                                          @isActive
                                                          ) 
                                    SELECT * FROM Sequences WHERE SequenceId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(sequencesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshSequencesDTO(sequencesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting SequencesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(sequencesDTO);
            return sequencesDTO;
        }

        private void RefreshSequencesDTO(SequencesDTO sequencesDTO, DataTable dt)
        {
            log.LogMethodEntry(sequencesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                sequencesDTO.SequenceId = Convert.ToInt32(dt.Rows[0]["SequenceId"]);
                sequencesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                sequencesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                sequencesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                sequencesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                sequencesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                sequencesDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the Sequences record
        /// </summary>
        /// <param name="sequencesDTO">SequencesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public SequencesDTO UpdateSequences(SequencesDTO sequencesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(sequencesDTO, loginId, siteId);
            string query = @"update Sequences 
                                                          set SeqName = @seqName,
                                                          Seed = @seed,
                                                          Incr = @incr,
                                                          Currval = @currval,
                                                          Prefix = @prefix,
                                                          Suffix = @suffix,
                                                          Width = @width,
                                                          UserColumnHeading = @userColumnHeading,
                                                          POSMachineId = @posMachineId,
                                                          MaximumValue = @maximumValue,
                                                          MasterEntityId = @masterEntityId,
                                                          OrderTypeGroupId =@orderTypeGroupId,
                                                          LastUpdatedBy = @lastUpdatedBy, 
                                                          LastupdateDate = Getdate(),
                                                          IsActive = @isActive
                                                          where SequenceId = @sequenceId
                                   SELECT * FROM Sequences WHERE SequenceId = @sequenceId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(sequencesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshSequencesDTO(sequencesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting SequencesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(sequencesDTO);
            return sequencesDTO;
        }

        /// <summary>
        /// Converts the Data row object to SequencesDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns SequencesDTO</returns>
        private SequencesDTO GetSequencesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            SequencesDTO SequencesDTO = new SequencesDTO(dataRow["SeqName"].ToString(),
                                            dataRow["Seed"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Seed"]),
                                            dataRow["Incr"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Incr"]),
                                            dataRow["Currval"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Currval"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Prefix"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Prefix"]),
                                            dataRow["Suffix"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Suffix"]),
                                            dataRow["Width"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Width"]),
                                            dataRow["UserColumnHeading"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UserColumnHeading"]),
                                            dataRow["SequenceId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SequenceId"]),
                                            dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                            dataRow["MaximumValue"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MaximumValue"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["OrderTypeGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderTypeGroupId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                            );
            log.LogMethodExit(SequencesDTO);
            return SequencesDTO;
        }

        /// <summary>
        /// Gets the SequencesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SequencesDTO matching the search criteria</returns>
        public List<SequencesDTO> GetAllSequencesList(List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectProductQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<SequencesDTO> sequenceDTOList = new List<SequencesDTO>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SequencesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(SequencesDTO.SearchByParameters.SEQUENCE_ID)
                                || searchParameter.Key.Equals(SequencesDTO.SearchByParameters.ORDER_TYPE_GROUP_ID)
                                || searchParameter.Key.Equals(SequencesDTO.SearchByParameters.POS_MACHINE_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(SequencesDTO.SearchByParameters.SEQUENCE_NAME) ||
                                     searchParameter.Key.Equals(SequencesDTO.SearchByParameters.GUID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key == SequencesDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }

                            else if (searchParameter.Key == SequencesDTO.SearchByParameters.PREFIX ||
                                   searchParameter.Key == SequencesDTO.SearchByParameters.SUFFIX)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key == SequencesDTO.SearchByParameters.ISACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
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
                    selectProductQuery = selectProductQuery + query;
            }

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow sequenceDataRow in dataTable.Rows)
                {
                    SequencesDTO sequenceDataObject = GetSequencesDTO(sequenceDataRow);
                    sequenceDTOList.Add(sequenceDataObject);
                }
            }
            log.LogMethodExit(sequenceDTOList);
            return sequenceDTOList;
        }

        /// <summary>
        /// GetNextSeqNo
        /// </summary>
        /// <param name="sequenceName">sequenceName Parameter</param>
        /// <returns>sequence No</returns>
        public string GetNextSeqNo(SequencesDTO sequencesDTO)
        {
            log.LogMethodEntry(sequencesDTO);
            try
            {
                DataTable dTable = dataAccessHandler.executeSelectQuery(@"declare @value varchar(20)
                                exec GetNextSeqValue N'" + sequencesDTO.SequenceName + "', @value out, " + sequencesDTO.POSMachineId + ", " + sequencesDTO.OrderTypeGroupId
                                   + " select @value", null, sqlTransaction);
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    object o = dTable.Rows[0][0];
                    if (o != null)
                    {
                        log.LogMethodExit(o);
                        return (o.ToString());
                    }
                    else
                    {
                        log.LogMethodExit(-1);
                        return "-1";
                    }
                }

            }
            catch
            {
                log.LogMethodExit(-1);
                return "-1";
            }
            log.LogMethodExit("-1");
            return "-1";

        }
    }
}
