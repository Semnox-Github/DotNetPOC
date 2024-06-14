/********************************************************************************************
 * Project Name - Device
 * Description  - CashdrawerUseCaseFactory.cs
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Aug-2021     Girish Kundar              Created : Multi cashdrawer for POS changes
 2.140.0      21-Mar-2022     Abhishek                   Modified : Added HasActivePosCashdrawers() method to fetch active poscashdrawers
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    /// <summary>
    /// CashdrawerDataHandler
    /// </summary>
    public class CashdrawerDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Cashdrawers AS cd ";
        private static readonly Dictionary<CashdrawerDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CashdrawerDTO.SearchByParameters, string>
            {
                {CashdrawerDTO.SearchByParameters.CAHSDRAWER_ID, "cd.CashdrawerId"},
                {CashdrawerDTO.SearchByParameters.CASHDRAWER_NAME, "cd.CashdrawerName"},
                {CashdrawerDTO.SearchByParameters.INTERFACE_TYPE, "cd.CashdrawerName"},
                {CashdrawerDTO.SearchByParameters.IS_ACTIVE, "cd.IsActive"},
                {CashdrawerDTO.SearchByParameters.IS_SYSTEM, "cd.IsSystem"},
                {CashdrawerDTO.SearchByParameters.MASTER_ENTITY_ID, "cd.MasterEntityId"},
                {CashdrawerDTO.SearchByParameters.SITE_ID, "cd.site_id"}
            };
        /// <summary>
        /// Default constructor of CashdrawerDataHandler class
        /// </summary>
        public CashdrawerDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CashdrawerDTO Record.
        /// </summary>
        /// <param name="cashdrawerDTO">CashdrawerDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(CashdrawerDTO cashdrawerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cashdrawerDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CashdrawerId", cashdrawerDTO.CashdrawerId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CashdrawerName", cashdrawerDTO.CashdrawerName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InterfaceType", cashdrawerDTO.InterfaceType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CommunicationString", cashdrawerDTO.CommunicationString));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SerialPort", cashdrawerDTO.SerialPort));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SerialPortBaud", cashdrawerDTO.SerialPortBaud));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsSystem", cashdrawerDTO.IsSystem));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", cashdrawerDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", cashdrawerDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cashdrawerDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CashdrawerDTO Insert(CashdrawerDTO cashdrawerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cashdrawerDTO, loginId, siteId);
            string insertQuery = @"insert into Cashdrawers 
                                                        (                                                         
                                                       [CashdrawerName] ,
                                                       [InterfaceType],
                                                       [CommunicationString],
                                                       [SerialPort],
                                                       [SerialPortBaud],
                                                       IsSystem ,
                                                       IsActive ,
                                                       CreatedBy ,
                                                       CreationDate ,
                                                       LastUpdatedBy ,
                                                       LastUpdatedDate ,
                                                       Guid ,
                                                       site_id   ,
                                                       MasterEntityId 
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @CashdrawerName ,
                                                       @InterfaceType,
                                                       @CommunicationString,
                                                       @SerialPort,
                                                       @SerialPortBaud,
                                                       @IsSystem ,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from Cashdrawers where CashdrawerId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(cashdrawerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCashdrawerDTO(cashdrawerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CashdrawerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cashdrawerDTO);
            return cashdrawerDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cashdrawerDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CashdrawerDTO Update(CashdrawerDTO cashdrawerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cashdrawerDTO, loginId, siteId);
            string updateQuery = @"update Cashdrawers 
                                         set 
                                            CashdrawerName = @CashdrawerName,
                                            InterfaceType = @InterfaceType,
                                            CommunicationString = @CommunicationString,
                                            SerialPort = @SerialPort,
                                            SerialPortBaud = @SerialPortBaud,
                                            IsActive = @IsActive,
                                            IsSystem = @IsSystem,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId 
                                               where   CashdrawerId =  @CashdrawerId  
                                        SELECT  * from Cashdrawers where CashdrawerId = @CashdrawerId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(cashdrawerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCashdrawerDTO(cashdrawerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CashdrawerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cashdrawerDTO);
            return cashdrawerDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CashdrawerDTO"></param>
        /// <returns></returns>
        public CashdrawerDTO UpdateCashdrawerDTO(CashdrawerDTO CashdrawerDTO)
        {
            log.LogMethodEntry(CashdrawerDTO);
            string updateQuery = @"update Cashdrawers 
                                         set 
                                            CashdrawerName = @CashdrawerName,
                                            InterfaceType = @InterfaceType,
                                            PrintString = @PrintString,
                                            SerialPort = @SerialPort,
                                            SerialPortBaud = @SerialPortBaud,
                                            CommunicationString = @CommunicationString,
                                            IsActive = @IsActive
                                               where   CashdrawerId =  @CashdrawerId  
                                        )SELECT  * from Cashdrawers where CashdrawerId = @CashdrawerId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(CashdrawerDTO, string.Empty, -1).ToArray(), sqlTransaction);
                RefreshCashdrawerDTO(CashdrawerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CashdrawerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(CashdrawerDTO);
            return CashdrawerDTO;
        }
        private void RefreshCashdrawerDTO(CashdrawerDTO cashdrawerDTO, DataTable dt)
        {
            log.LogMethodEntry(cashdrawerDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cashdrawerDTO.CashdrawerId = Convert.ToInt32(dt.Rows[0]["CashdrawerId"]);
                cashdrawerDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                cashdrawerDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                cashdrawerDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                cashdrawerDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cashdrawerDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                cashdrawerDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private CashdrawerDTO GetCashdrawerDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CashdrawerDTO cashdrawerDTO = new CashdrawerDTO(Convert.ToInt32(dataRow["CashdrawerId"]),
                                                    dataRow["CashdrawerName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CashdrawerName"]),
                                                    dataRow["InterfaceType"] == DBNull.Value ? CashdrawerIntefaceTypes.RECEIPTPRINTER.ToString() : GetInterfaceType(Convert.ToString(dataRow["InterfaceType"])),
                                                    dataRow["CommunicationString"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CommunicationString"]),
                                                    dataRow["SerialPort"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SerialPort"]),
                                                    dataRow["SerialPortBaud"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SerialPortBaud"]),
                                                    dataRow["IsSystem"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsSystem"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                    );
            log.LogMethodExit(cashdrawerDTO);
            return cashdrawerDTO;
        }

        
        internal DateTime? GetCashdrawerLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdateDate from Cashdrawers WHERE (site_id = @siteId or @siteId = -1) ";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
        private string GetInterfaceType(string interfacetype)
        {
            log.LogMethodEntry();
            CashdrawerIntefaceTypes type = CashdrawerIntefaceTypes.RECEIPTPRINTER;
            try
            {
                type = (CashdrawerIntefaceTypes)Enum.Parse(typeof(CashdrawerIntefaceTypes), interfacetype, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while parsing the OrderMessageStatus type", ex);
                throw ex;
            }
            log.LogMethodExit(type);
            return type.ToString();
        }

        /// <summary>
        /// Gets the CashdrawerDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns CashdrawerDTO</returns>
        internal CashdrawerDTO GetCashdrawerDTO(int id)
        {
            log.LogMethodEntry(id);
            CashdrawerDTO cashdrawerDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where cd.CashdrawerId = @CashdrawerId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@CashdrawerId", id);
            DataTable cashdrawerIdTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (cashdrawerIdTable.Rows.Count > 0)
            {
                DataRow gameMachineLevelRow = cashdrawerIdTable.Rows[0];
                cashdrawerDTO = GetCashdrawerDTO(gameMachineLevelRow);
            }
            log.LogMethodExit(cashdrawerDTO);
            return cashdrawerDTO;

        }

        /// <summary>
        /// GetCashdrawers
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CashdrawerDTO> GetCashdrawers(List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CashdrawerDTO> cashdrawerDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CashdrawerDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CashdrawerDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CashdrawerDTO.SearchByParameters.IS_ACTIVE
                            || searchParameter.Key == CashdrawerDTO.SearchByParameters.IS_SYSTEM)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(CashdrawerDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(CashdrawerDTO.SearchByParameters.CAHSDRAWER_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CashdrawerDTO.SearchByParameters.CASHDRAWER_NAME
                            || searchParameter.Key == CashdrawerDTO.SearchByParameters.INTERFACE_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        counter++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                cashdrawerDTOList = new List<CashdrawerDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    CashdrawerDTO CashdrawerDTO = GetCashdrawerDTO(dataRow);
                    cashdrawerDTOList.Add(CashdrawerDTO);
                }
            }
            log.LogMethodExit(cashdrawerDTOList);
            return cashdrawerDTOList;
        }

        /// <summary>
        /// </summary>
        /// <param name="cashdrawerId"></param>
        /// <returns>true or false</returns>
        internal bool HasActivePosCashdrawers(int cashdrawerId)
        {
            log.LogMethodEntry(cashdrawerId);
            bool result = false;
            string selectUserQuery = @"SELECT * FROM POSCashdrawers where IsActive = 1 and CashdrawerId = @CashdrawerId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@CashdrawerId", cashdrawerId);
            DataTable cashdrawerIdTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (cashdrawerIdTable.Rows.Count > 0)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
