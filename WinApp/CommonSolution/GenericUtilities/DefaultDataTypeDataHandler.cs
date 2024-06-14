/********************************************************************************************
* Project Name - DefaultDataType Data Handler
* Description  - Data handler of the CustomDataSet class
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
 *2.60        03-May-2019   Mushahid Faizan     Added DBSearchParameters, GetSQLParameters(), Insert/Update Method,
                                                GetAllDefaultDataTypes() and GetAllDefaultDataTypes() methods.
 *2.70.2        26-Jul-2019   Dakshakh raj        Modified : Log method entries/exits.
 *2.70.2        06-Dec-2019   Jinto Thomas            Removed siteid from update query  
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Class will communicate with default data type table
    /// </summary>
    public class DefaultDataTypeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM defaults_datatype as dd ";

        private static readonly Dictionary<DefaultDataTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DefaultDataTypeDTO.SearchByParameters, string>
       {
                {DefaultDataTypeDTO.SearchByParameters.DATATYPE_ID, "dd.datatype_id"},
                {DefaultDataTypeDTO.SearchByParameters.DATA_TYPE, "dd.datatype"},
                {DefaultDataTypeDTO.SearchByParameters.MASTER_ENTITY_ID,"dd.MasterEntityId"},
                {DefaultDataTypeDTO.SearchByParameters.SITE_ID, "dd.site_id"}
            };
        private readonly SqlTransaction sqlTransaction = null;
        DataAccessHandler dataAccessHandler;

        /// <summary>
        ///  Default data type data handler
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DefaultDataTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///Get Default Types 
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Default Types</returns>
        public DataTable GetDefaultTypes(string name, int siteId)
        {
            log.LogMethodEntry(name, siteId);
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@site_id", siteId);
            sqlParameter[1] = new SqlParameter("@name", name);
            DataTable dTable = dataAccessHandler.executeSelectQuery("select datavalues from defaults_datatype where datatype = @name and (site_id = @site_id or @site_id = -1)", sqlParameter, sqlTransaction);
            log.LogMethodExit(dTable);
            return dTable;
        }

        /// <summary>
        /// Get Default Datatypes by DataTypeId
        /// </summary>
        /// <param name="name"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public DataTable GetDefaultTypesByDataType(string dataTypeId, int siteId)
        {
            log.LogMethodEntry(dataTypeId, siteId);
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@site_id", siteId);
            sqlParameter[1] = new SqlParameter("@dataTypeId", dataTypeId);
            DataTable dTable = dataAccessHandler.executeSelectQuery("select datatype from defaults_datatype where datatype_id = @dataTypeId and (site_id = @site_id or @site_id = -1)", sqlParameter, sqlTransaction);
            log.LogMethodExit(dTable);
            return dTable;
        }

        public DataTable GetDefaultTypesBySelectCommand(string selectCommand, int siteId)
        {
            log.LogMethodEntry(selectCommand, siteId);
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@site_id", siteId);
            DataTable dTable = dataAccessHandler.executeSelectQuery(selectCommand, sqlParameter, sqlTransaction);
            log.LogMethodExit(dTable);
            return dTable;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DefaultDataType Record.
        /// </summary>
        /// <param name="DefaultDataTypeDTO">DefaultDataTypeDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(DefaultDataTypeDTO defaultDataTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(defaultDataTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@datatype_id", defaultDataTypeDTO.DatatypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@datatype", defaultDataTypeDTO.Datatype));
            parameters.Add(dataAccessHandler.GetSQLParameter("@datavalues", defaultDataTypeDTO.Datavalues));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", defaultDataTypeDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", defaultDataTypeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the defaultDataTypeDTO record to the database
        /// </summary>
        /// <param name="defaultDataTypeDTO">defaultDataTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public DefaultDataTypeDTO InsertDefaultDataType(DefaultDataTypeDTO defaultDataTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(defaultDataTypeDTO, loginId, siteId);
            string query = @"INSERT INTO defaults_datatype 
                                        ( 
                                            datatype,
                                            datavalues,
                                            Guid,
                                            site_id,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @datatype,
                                            @datavalues,
                                            NEWID(),
                                            @site_id,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @MasterEntityId
                                        )SELECT * FROM defaults_datatype WHERE datatype_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(defaultDataTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDefaultDataTypeDTO(defaultDataTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customAttributes", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(defaultDataTypeDTO);
            return defaultDataTypeDTO;
        }

        /// <summary>
        /// Updates the defaultDataTypeDTO record
        /// </summary>
        /// <param name="defaultDataTypeDTO">defaultDataTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public DefaultDataTypeDTO UpdateDefaultDataType(DefaultDataTypeDTO defaultDataTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(defaultDataTypeDTO, loginId, siteId);
            string query = @"UPDATE [defaults_datatype] SET [datatype] = @datatype, [datavalues] = @datavalues 
                                --[site_id] = @site_id 
                                WHERE (datatype_id = @datatype_id) SELECT* FROM defaults_datatype WHERE  datatype_id = @datatype_id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(defaultDataTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDefaultDataTypeDTO(defaultDataTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(defaultDataTypeDTO);
            return defaultDataTypeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="defaultDataTypeDTO">defaultDataTypeDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshDefaultDataTypeDTO(DefaultDataTypeDTO defaultDataTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(defaultDataTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                defaultDataTypeDTO.DatatypeId = Convert.ToInt32(dt.Rows[0]["Datatype_Id"]);
                defaultDataTypeDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                defaultDataTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                defaultDataTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                defaultDataTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                defaultDataTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                defaultDataTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to DefaultDataTypeDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns defaultDataTypeDTO</returns>
        private DefaultDataTypeDTO GetDefaultDataTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DefaultDataTypeDTO defaultDataTypeDTO = new DefaultDataTypeDTO(Convert.ToInt32(dataRow["datatype_id"]),
                                            dataRow["datatype"] == DBNull.Value ? string.Empty : (dataRow["datatype"].ToString()),
                                            dataRow["datavalues"] == DBNull.Value ? string.Empty : (dataRow["datavalues"].ToString()),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(defaultDataTypeDTO);
            return defaultDataTypeDTO;
        }

        /// <summary>
        /// Gets the Default DataTypes list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DefaultDataTypeDTO matching the search criteria</returns>
        public List<DefaultDataTypeDTO> GetAllDefaultDataTypes(List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DefaultDataTypeDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == DefaultDataTypeDTO.SearchByParameters.DATATYPE_ID ||
                            searchParameter.Key == DefaultDataTypeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DefaultDataTypeDTO.SearchByParameters.DATA_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == DefaultDataTypeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<DefaultDataTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DefaultDataTypeDTO defaultDataTypeDTO = GetDefaultDataTypeDTO(dataRow);
                    list.Add(defaultDataTypeDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
