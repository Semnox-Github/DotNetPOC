/********************************************************************************************
 * Project Name - ParafaitFunctionEventDatahandler
 * Description  - Data handler of the ParafaitFunctionEvent 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Fiona            Created for Subscription changes                                                                               
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    public class ParafaitFunctionEventDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ParafaitFunctionEvents AS pfe ";

        private static readonly Dictionary<ParafaitFunctionEventDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ParafaitFunctionEventDTO.SearchByParameters, string>
            {
                {ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_ID, "pfe.ParafaitFunctionId"},
                {ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME, "pfe.parafaitFunctionEventName"},
                {ParafaitFunctionEventDTO.SearchByParameters.IS_ACTIVE, "pfe.IsActive"},
                {ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID, "pfe.ParafaitFunctionEventId"},
                {ParafaitFunctionEventDTO.SearchByParameters.MASTER_ENTITY_ID, "pfe.MasterEntityId"},
                {ParafaitFunctionEventDTO.SearchByParameters.SITE_ID, "pfe.site_id"}

            };
        /// <summary>
        /// Parameterized Constructor for ParafaitFunctionEventDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public ParafaitFunctionEventDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ParafaitFunctionEventDTO parafaitFunctionEventDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parafaitFunctionEventDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitFunctionEventId", parafaitFunctionEventDTO.ParafaitFunctionEventId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitFunctionEventName", parafaitFunctionEventDTO.ParafaitFunctionEventName.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitFunctionEventDescription", parafaitFunctionEventDTO.ParafaitFunctionEventDescription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitFunctionId", parafaitFunctionEventDTO.ParafaitFunctionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", parafaitFunctionEventDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", parafaitFunctionEventDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", parafaitFunctionEventDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        
        //internal ParafaitFunctionEventDTO InsertParafaitFunctionEvent(ParafaitFunctionEventDTO parafaitFunctionEventDTO, string loginId, int siteId)
        //{
        //    log.LogMethodEntry(parafaitFunctionEventDTO, loginId, siteId);
        //    string query = @"INSERT INTO[dbo].ParafaitFunctionEvent
        //                       (  
        //                                ParafaitFunctionEventName,
        //                                ParafaitFunctionEventDescription,
        //                                ParafaitFunctionId,
        //                                IsActive,
        //                                SynchStatus,
        //                                site_id,
        //                                MasterEntityId,
        //                                CreatedBy,
        //                                CreationDate,
        //                                LastUpdatedBy,
        //                                LastUpdatedDate,
        //                                Guid

        //                        )
        //                 VALUES
        //                       (    @ParafaitFunctionName,
        //                            @ParafaitFunctionDescription,
        //                            @ParafaitFunctionId,
        //                            @IsActive,
        //                            @SynchStatus,
        //                            @SiteId,
        //                            @MasterEntityId
        //                            @CreatedBy,
        //                            GETDATE(),
        //                            @LastUpdatedBy,
        //                            GETDATE(),
        //                            NEWID()
        //                        )
        //                        SELECT * FROM ParafaitFunctionEvents WHERE ParafaitFunctionEventId = scope_identity()";
        //    try
        //    {
        //        DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parafaitFunctionEventDTO, loginId, siteId).ToArray(), sqlTransaction);
        //        RefreshParafaitFunctionEventDTO(parafaitFunctionEventDTO, dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        log.LogMethodExit(null, "Throwing exception - " + ex.Message);
        //        throw;
        //    }
        //    log.LogMethodExit(parafaitFunctionEventDTO);
        //    return parafaitFunctionEventDTO;
        //}
        //internal ParafaitFunctionEventDTO UpdateParafaitFunctionEvent(ParafaitFunctionEventDTO parafaitFunctionEventDTO, string loginId, int siteId)
        //{
        //    log.LogMethodEntry(parafaitFunctionEventDTO, loginId, siteId);
        //    string query = @"UPDATE [dbo].ParafaitFunctionEvent set
        //                       ParafaitFunctionEventName =  @ParafaitFunctionEventName,
        //                       ParafaitFunctionEventDescription = @ParafaitFunctionEventDescription,
        //                       ParafaitFunctionId = @ParafaitFunctionId,
        //                       MasterEntityId = @MasterEntityId,
        //                       IsActive = @IsActive, 
        //                       site_id = @SiteId,
        //                       LastUpdatedBy = @LastUpdatedBy,
        //                       LastUpdateDate = GETDATE()
        //                       where ParafaitFunctionEventId = @ParafaitFunctionEventId
        //                     SELECT * FROM ParafaitFunctionEvents WHERE ParafaitFunctionEventId = @ParafaitFunctionEventId";
        //    try
        //    {
        //        DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parafaitFunctionEventDTO, loginId, siteId).ToArray(), sqlTransaction);
        //        RefreshParafaitFunctionEventDTO(parafaitFunctionEventDTO, dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        log.LogMethodExit(null, "Throwing exception - " + ex.Message);
        //        throw;
        //    }
        //    log.LogMethodExit(parafaitFunctionEventDTO);
        //    return parafaitFunctionEventDTO;
        //}
        //private void RefreshParafaitFunctionEventDTO(ParafaitFunctionEventDTO parafaitFunctionEventDTO, DataTable dt)
        //{
        //    log.LogMethodEntry(parafaitFunctionEventDTO, dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        DataRow dataRow = dt.Rows[0];
        //        parafaitFunctionEventDTO.ParafaitFunctionEventId = Convert.ToInt32(dt.Rows[0]["ParafaitFunctionEventId"]);
        //        parafaitFunctionEventDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
        //        parafaitFunctionEventDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
        //        parafaitFunctionEventDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
        //        parafaitFunctionEventDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
        //        parafaitFunctionEventDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
        //        parafaitFunctionEventDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
        //    }
        //    log.LogMethodExit();
        //}
        internal ParafaitFunctionEventDTO GetParafaitFunctionEventDTO(int parafaitFunctionEventId)
        {
            log.LogMethodEntry(parafaitFunctionEventId);
            ParafaitFunctionEventDTO result = null;
            string query = SELECT_QUERY + @" WHERE pfe.ParafaitFunctionEventId = @ParafaitFunctionEventId";
            SqlParameter parameter = new SqlParameter("@ParafaitFunctionEventId", parafaitFunctionEventId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetParafaitFunctionEventDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        internal ParafaitFunctionEventDTO GetParafaitFunctionEventDTO(ParafaitFunctionEvents eventName)
        {
            log.LogMethodEntry(eventName);
            ParafaitFunctionEventDTO parafaitFunctionEventDTO = null;
            string query = SELECT_QUERY + @" WHERE pfe.ParafaitFunctionEventName = @ParafaitFunctionEventName";
            SqlParameter parameter = new SqlParameter("@ParafaitFunctionEventName", eventName.ToString());
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                parafaitFunctionEventDTO = GetParafaitFunctionEventDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(parafaitFunctionEventDTO);
            return parafaitFunctionEventDTO;
        }


        private ParafaitFunctionEventDTO GetParafaitFunctionEventDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ParafaitFunctionEventDTO parafaitFunctionEventDTO = new ParafaitFunctionEventDTO(
                dataRow["ParafaitFunctionEventId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParafaitFunctionEventId"]),
                dataRow["ParafaitFunctionEventName"] == DBNull.Value ? ParafaitFunctionEvents.NONE : GetParafaitFunctionEvents(Convert.ToString(dataRow["ParafaitFunctionEventName"])),
                dataRow["ParafaitFunctionEventDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParafaitFunctionEventDescription"]),
                dataRow["ParafaitFunctionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParafaitFunctionId"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"])
                );
            log.LogMethodExit(parafaitFunctionEventDTO);
            return parafaitFunctionEventDTO;
        }

        internal List<ParafaitFunctionEventDTO> GetParafaitFunctionEventDTOList(List<int> parafaitFunctionsIdList, bool loadActiveChildren)
        {
            log.LogMethodEntry(parafaitFunctionsIdList, loadActiveChildren);
            List<ParafaitFunctionEventDTO> list = new List<ParafaitFunctionEventDTO>();
            string query = @"SELECT pfe.* 
                              FROM ParafaitFunctionEvents AS pfe
                                  inner join @parafaitFunctionsIdList List on pfe.ParafaitFunctionId = List.Id
                              where isnull(pfe.isactive,0) = @LoadActiveChildren";

            DataTable table = dataAccessHandler.BatchSelect(query, "@parafaitFunctionsIdList",
                                                                  parafaitFunctionsIdList,new SqlParameter[] { new SqlParameter("LoadActiveChildren", loadActiveChildren) },
                                                                  sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetParafaitFunctionEventDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        internal List<ParafaitFunctionEventDTO> GetParafaitFunctionEventDTOList(List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ParafaitFunctionEventDTO> parafaitFunctionEventDTOList = new List<ParafaitFunctionEventDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Any()))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID || 
                            searchParameter.Key == ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_ID ||
                            searchParameter.Key == ParafaitFunctionEventDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),searchParameter.Value));
                        }
                        else if (searchParameter.Key == ParafaitFunctionEventDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        } 
                        else if (searchParameter.Key == ParafaitFunctionEventDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable != null && dataTable.Rows.Cast<DataRow>().Any())
            {
                parafaitFunctionEventDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetParafaitFunctionEventDTO(x)).ToList();
            }
            log.LogMethodExit(parafaitFunctionEventDTOList);
            return parafaitFunctionEventDTOList;
        }

        /// <summary>
        ///  Gets the ParafaitFunctionEvents by passing ParafaitFunctionEvents string
        /// </summary>
        /// <param name="parafaitFunctionEventString">parafaitFunctionEventString</param>
        /// <returns>ParafaitFunctionEvents</returns>
        private ParafaitFunctionEvents GetParafaitFunctionEvents(string parafaitFunctionEventString)
        {
            log.LogMethodEntry(parafaitFunctionEventString);
            ParafaitFunctionEvents parafaitFunctionEvents = ParafaitFunctionEvents.NONE;
            try
            {
                parafaitFunctionEvents = (ParafaitFunctionEvents)Enum.Parse(typeof(ParafaitFunctionEvents), parafaitFunctionEventString, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while parsing the ParafaitFunctionEvents", ex);
                throw ex;
            }
            log.LogMethodExit(parafaitFunctionEvents);
            return parafaitFunctionEvents;
        }
    }
}
