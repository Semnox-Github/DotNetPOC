/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Get and Insert or update methods for kiosk setup details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        18-Mar-2019   Jagan Mohana          Created 
              23-Apr-2019   Mushahid Faizan       Added GetSQLParameters(), modified Insert/Update Method & DBSearchParameters,GetAllKioskSetup().
 *2.70.2        25-Jul-2019   Dakshakh Raj          Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix.
 *            29-Jul-2019   Mushahid Faizan       Added Delete Method.
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query             
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class KioskSetupDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM KioskMoneyAcceptorInfo as kma ";

        /// <summary>
        /// Dictionary for searching Parameters for the KioskSetupDTO object.
        /// </summary>
        private static readonly Dictionary<KioskSetupDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<KioskSetupDTO.SearchByParameters, string>
        {
            { KioskSetupDTO.SearchByParameters.ID,"kma.Id"},
            { KioskSetupDTO.SearchByParameters.NOTE_COIN_FLAG, "kma.NoteCoinFlag"},
            { KioskSetupDTO.SearchByParameters.SITE_ID, "kma.site_id"},
            { KioskSetupDTO.SearchByParameters.ISACTIVE, "kma.Active"},
            { KioskSetupDTO.SearchByParameters.MASTER_ENTITY_ID, "kma.MasterEntityId"}
        };

        /// <summary>
        /// Default constructor of KioskSetupDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public KioskSetupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to kioskSetupDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private KioskSetupDTO GetKioskSetupDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            KioskSetupDTO KioskSetupDTO = new KioskSetupDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["NoteCoinFlag"] == DBNull.Value ? string.Empty : dataRow["NoteCoinFlag"].ToString(),
                                            dataRow["DenominationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DenominationId"]),
                                            dataRow["Name"] == DBNull.Value ? string.Empty : dataRow["Name"].ToString(),
                                            dataRow["Image"] == DBNull.Value ? new Byte[0] : (byte[])(dataRow["Image"]),
                                            Convert.ToDouble(dataRow["Value"]),
                                            dataRow["AcceptorHexCode"] == DBNull.Value ? string.Empty : dataRow["AcceptorHexCode"].ToString(),
                                            dataRow["Active"] == DBNull.Value ? true : (dataRow["Active"].ToString() == "Y" ? true : false),
                                            dataRow["ModelCode"] == DBNull.Value ? string.Empty : dataRow["ModelCode"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(KioskSetupDTO);
            return KioskSetupDTO;
        }

        /// <summary>
        /// Gets the KioskSetupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of KioskSetupDTO matching the search criteria</returns>
        public List<KioskSetupDTO> GetAllKioskSetup(List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<KioskSetupDTO> kioskSetupDTOList = null;
            string selectQuery = SELECT_QUERY;
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<KioskSetupDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key == KioskSetupDTO.SearchByParameters.ID
                                || searchParameter.Key == KioskSetupDTO.SearchByParameters.MASTER_ENTITY_ID)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == KioskSetupDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == KioskSetupDTO.SearchByParameters.ISACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
                    selectQuery = selectQuery + query;
            }

            DataTable kioskSetupData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (kioskSetupData.Rows.Count > 0)
            {
                kioskSetupDTOList = new List<KioskSetupDTO>();
                foreach (DataRow kioskSetupDataRow in kioskSetupData.Rows)
                {
                    KioskSetupDTO lookupDataObject = GetKioskSetupDTO(kioskSetupDataRow);
                    kioskSetupDTOList.Add(lookupDataObject);
                }
               
            }
            log.LogMethodExit(kioskSetupDTOList);
            return kioskSetupDTOList;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating KioskSetup Record.
        /// </summary>
        /// <param name="kioskSetupDTO">kioskSetupDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(KioskSetupDTO kioskSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(kioskSetupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", kioskSetupDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@noteCoinFlag", kioskSetupDTO.NoteCoinFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@denominationId", kioskSetupDTO.DenominationId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", kioskSetupDTO.Name));
            /// The database field is Image type so if image is null then need add the db type and send the db null value
            if(!(kioskSetupDTO.Image.Length > 0) || kioskSetupDTO.Image == (byte[])null)
            {
                SqlParameter imageParameter = new SqlParameter("@image", SqlDbType.Image)
                {
                    Value = DBNull.Value
                };
                parameters.Add(imageParameter);
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@image", kioskSetupDTO.Image));
            }
            //parameters.Add(dataAccessHandler.GetSQLParameter("@image", kioskSetupDTO.Image == (byte[])null ? (byte[])null : kioskSetupDTO.Image));            
            parameters.Add(dataAccessHandler.GetSQLParameter("@value", kioskSetupDTO.Value));
            parameters.Add(dataAccessHandler.GetSQLParameter("@acceptorHexCode", kioskSetupDTO.AcceptorHexCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", (kioskSetupDTO.Active == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@modelCode", kioskSetupDTO.ModelCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@synchStatus", kioskSetupDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", kioskSetupDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the kioskSetup record to the database
        /// </summary>
        /// <param name="kioskSetupDTO">KioskSetupDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public KioskSetupDTO InsertKioskSetup(KioskSetupDTO kioskSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(kioskSetupDTO, loginId, siteId);
            string insertKioskSetupQuery = @"insert into KioskMoneyAcceptorInfo 
                                                        (                                                         
                                                        NoteCoinFlag,
                                                        DenominationId,
                                                        Name,
                                                        Image,
                                                        Value,
                                                        AcceptorHexCode,
                                                        Active,
                                                        ModelCode,
                                                        site_id,
                                                        Guid,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate                                                        
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @noteCoinFlag,
                                                        @denominationId,
                                                        @name,
                                                        @image,
                                                        @value,
                                                        @acceptorHexCode,
                                                        @active,
                                                        @modelCode,
                                                        @siteId,
                                                        NewId(),
                                                        @masterEntityId,
                                                        @createdBy,
                                                        GETDATE(),                                                        
                                                        @lastUpdatedBy,                                                        
                                                        GetDate()
                                            )SELECT * FROM KioskMoneyAcceptorInfo WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertKioskSetupQuery, GetSQLParameters(kioskSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshKioskSetupDTO(kioskSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting kioskSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(kioskSetupDTO);
            return kioskSetupDTO;
        }

        /// <summary>
        /// Updates the kioskSetup record
        /// </summary>
        /// <param name="kioskSetupDTO">KioskSetupDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public KioskSetupDTO UpdateKioskSetup(KioskSetupDTO kioskSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(kioskSetupDTO, loginId, siteId);
            string updateKioskSetupQuery = @"update KioskMoneyAcceptorInfo 
                                         set NoteCoinFlag=@noteCoinFlag,
                                             DenominationId= @denominationId,
                                             Name= @name,
                                             Image= @image,
                                             Value= @value,
                                             AcceptorHexCode= @acceptorHexCode,
                                             Active= @active,
                                             ModelCode= @modelCode,
                                             --site_id = @siteId,
                                             MasterEntityId = @masterEntityId,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdateDate = GETDATE()
                                       where Id = @id
                                       SELECT * FROM KioskMoneyAcceptorInfo WHERE Id = @id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateKioskSetupQuery, GetSQLParameters(kioskSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshKioskSetupDTO(kioskSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating kioskSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(kioskSetupDTO);
            return kioskSetupDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="scheduleCalendarDTO">scheduleCalendarDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshKioskSetupDTO(KioskSetupDTO kioskSetupDTO, DataTable dt)
        {
            log.LogMethodEntry(kioskSetupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                kioskSetupDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                kioskSetupDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                kioskSetupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                kioskSetupDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                kioskSetupDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                kioskSetupDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                kioskSetupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        ///  Deletes the KioskMoneyAcceptorInfo record based on Id 
        /// </summary>
        /// <param name="id">id is passed as parameter</param>
        internal void Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM KioskMoneyAcceptorInfo
                             WHERE KioskMoneyAcceptorInfo.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
    }
}