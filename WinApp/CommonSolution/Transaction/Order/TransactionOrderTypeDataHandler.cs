/****************************************************************************************************************
 * Project Name - Transaction Order Type DataHandler
 * Description  - Transaction Order Type Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *****************************************************************************************************************
 *2.80        26-Jun-2020      Raghuveera     Created 
 *****************************************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionOrderTypeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM TransactionOrderType as tot ";

        /// <summary>
        /// Dictionary for searching Parameters for the TransactionOrderTypeDataHandler object.
        /// </summary>

        private static readonly Dictionary<TransactionOrderTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionOrderTypeDTO.SearchByParameters, string>
            {
                {TransactionOrderTypeDTO.SearchByParameters.ID, "tot.ID"},
                {TransactionOrderTypeDTO.SearchByParameters.Name, "tot.Name"},
                {TransactionOrderTypeDTO.SearchByParameters.IS_ACTIVE, "tot.IsActive"},
                {TransactionOrderTypeDTO.SearchByParameters.MASTER_ENTITY_ID,"tot.MasterEntityId"},
                {TransactionOrderTypeDTO.SearchByParameters.SITE_ID, "tot.site_id"}
            };

        /// <summary>
        /// Parameterized Constructor for TransactionOrderTypeDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>

        public TransactionOrderTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the transaction order type record to the database
        /// </summary>
        /// <param name="transactionOrderTypeDTO">TransactionOrderTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>

        public TransactionOrderTypeDTO InsertTransactionOrderType(TransactionOrderTypeDTO transactionOrderTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionOrderTypeDTO, loginId, siteId);
            string insertTransactionOrderTypeQuery = @"INSERT INTO[dbo].[TransactionOrderType]
                                                       (                                              
                                                        Name,
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
                                                        @name,
                                                        @description,
                                                        @isActive,
                                                        @createdBy,
                                                        Getdate(),
                                                        @LastUpdatedBy,
                                                        Getdate(),                                                        
                                                        Newid(),
                                                        @siteid,
                                                        @masterEntityId
                                                        )SELECT * FROM TransactionOrderType WHERE Id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertTransactionOrderTypeQuery, GetSQLParameters(transactionOrderTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionOrderTypeDTO(transactionOrderTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TransactionOrderTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionOrderTypeDTO);
            return transactionOrderTypeDTO;
        }

        /// <summary>
        /// Updates the transaction order type record
        /// </summary>
        /// <param name="transactionOrderTypeDTO">TransactionOrderTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>

        public TransactionOrderTypeDTO UpdateTransactionOrderType(TransactionOrderTypeDTO transactionOrderTypeDTO, string loginId, int siteId)
        {

            log.LogMethodEntry(transactionOrderTypeDTO, loginId, siteId);
            string updateTransactionOrderTypeQuery = @"update TransactionOrderType 
                                                     set Name = @name,
                                                         Description = @description,
                                                         IsActive = @isActive,
                                                         LastUpdatedBy = @LastUpdatedBy, 
                                                         LastupdatedDate = Getdate(),                                                        
                                                         MasterEntityId=@masterEntityId
                                                         where Id = @id
                                   SELECT* FROM TransactionOrderType WHERE Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateTransactionOrderTypeQuery, GetSQLParameters(transactionOrderTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionOrderTypeDTO(transactionOrderTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TransactionOrderTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionOrderTypeDTO);
            return transactionOrderTypeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="transactionOrderTypeDTO">TransactionOrderTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshTransactionOrderTypeDTO(TransactionOrderTypeDTO transactionOrderTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(transactionOrderTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                transactionOrderTypeDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                transactionOrderTypeDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                transactionOrderTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                transactionOrderTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                transactionOrderTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                transactionOrderTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                transactionOrderTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating transactionOrderType Record.
        /// </summary>
        /// <param name="transactionOrderTypeDTO">transactionOrderTypeDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(TransactionOrderTypeDTO transactionOrderTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionOrderTypeDTO, loginId, siteId);

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", transactionOrderTypeDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", transactionOrderTypeDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", transactionOrderTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", transactionOrderTypeDTO.IsActive ));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", transactionOrderTypeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// Converts the Data row object to TransactionOrderTypeDTO class type
        /// </summary>
        /// <param name="transactionOrderTypeDataRow">TransactionOrderTypeDTO DataRow</param>
        /// <returns>Returns TransactionOrderTypeDTO</returns>
        private TransactionOrderTypeDTO GetTransactionOrderTypeDTO(DataRow transactionOrderTypeDataRow)
        {            
            log.LogMethodEntry(transactionOrderTypeDataRow);
            TransactionOrderTypeDTO transactionOrderTypeDataObject = new TransactionOrderTypeDTO(Convert.ToInt32(transactionOrderTypeDataRow["Id"]),
                                            transactionOrderTypeDataRow["Name"].ToString(),
                                            transactionOrderTypeDataRow["Description"].ToString(),
                                            transactionOrderTypeDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(transactionOrderTypeDataRow["IsActive"]),
                                            transactionOrderTypeDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(transactionOrderTypeDataRow["CreationDate"]),
                                            transactionOrderTypeDataRow["CreatedBy"].ToString(),
                                            transactionOrderTypeDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(transactionOrderTypeDataRow["LastupdatedDate"]),
                                            transactionOrderTypeDataRow["LastUpdatedBy"].ToString(),                                            
                                            transactionOrderTypeDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(transactionOrderTypeDataRow["site_id"]),                                
                                            transactionOrderTypeDataRow["Guid"].ToString(),                                            
                                            transactionOrderTypeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(transactionOrderTypeDataRow["SynchStatus"]),
                                            transactionOrderTypeDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(transactionOrderTypeDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(transactionOrderTypeDataObject);
            return transactionOrderTypeDataObject;
        }

        /// <summary>
        /// Gets the transaction order type data of passed transaction order type Id
        /// </summary>
        /// <param name="transactionOrderTypeId">Asset group asset id</param>
        /// <returns>Returns TransactionOrderTypeDTO</returns>

        public TransactionOrderTypeDTO GetTransactionOrderType(int transactionOrderTypeId)
        {
            log.LogMethodEntry(transactionOrderTypeId);
            TransactionOrderTypeDTO result = null;
            string selectTransactionOrderTypeQuery = SELECT_QUERY + @" WHERE tot.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", transactionOrderTypeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectTransactionOrderTypeQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetTransactionOrderTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the List of TransactionOrderTypeDTO based on the search parameters.
        /// </summary>
        /// <param name="SearchByParameters">search Parameters</param>
        /// <returns>Returns the List of TransactionOrderTypeDTO</returns>
        public List<TransactionOrderTypeDTO> GetTransactionOrderTypeDTOList(List<KeyValuePair<TransactionOrderTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TransactionOrderTypeDTO> transactionOrderTypeDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionOrderTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionOrderTypeDTO.SearchByParameters.ID
                            || searchParameter.Key == TransactionOrderTypeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == TransactionOrderTypeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionOrderTypeDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value ));
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
            if (dataTable.Rows.Count > 0)
            {
                transactionOrderTypeDTOList = new List<TransactionOrderTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TransactionOrderTypeDTO transactionOrderTypeDTO = GetTransactionOrderTypeDTO(dataRow);
                    transactionOrderTypeDTOList.Add(transactionOrderTypeDTO);
                }
            }
            log.LogMethodExit(transactionOrderTypeDTOList);
            return transactionOrderTypeDTOList;
        }
    }
}
