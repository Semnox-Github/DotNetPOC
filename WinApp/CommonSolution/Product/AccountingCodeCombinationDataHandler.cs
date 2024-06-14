/********************************************************************************************
 * Project Name - AccountingCodeCombination Data Handler
 * Description  - Data handler of the AccountingCodeCombination class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Dec-2016   Amaresh          Created
 *2.70.0      20-Jun-2019   Nagesh Badiger   Added new method DeleteAccountingCode.
 *2.110.0     16-Oct-2020   Mushahid Faizan      Added GetAccountingCodeDTOList().
 ********************************************************************************************/

using Semnox.Core;
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
    ///<summary>
    ///AccountingCodeCombination Data Handler - Handles insert, update and select of AccountingCodeCombination Data objects
    ///</summary>
    public class AccountingCodeCombinationDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string> DBSearchParameters = new Dictionary<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>
            {
                {AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.ID, "Id"},
                {AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.ACCOUNTINGCODE, "AccountingCode"},
                {AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.OBJECTID, "ObjectId"},
                {AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.OBJECTTYPE, "ObjectType"},
                {AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.TRANSACTIONTYPE, "TransactionType"},
                {AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.SITEID, "site_id"},
                {AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.ISACTIVE, "IsActive"},
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of AccountingCodeCombinationDataHandler class
        /// </summary>
        public AccountingCodeCombinationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the AccountingCodeCombination record to the database
        /// </summary>
        /// <param name="accountingCodeDTO">AccountingCodeCombinationDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertAccountingCodeCombination(AccountingCodeCombinationDTO accountingCodeDTO, string userId, int siteId)
        {
            log.Debug("Starts-InsertAccountingCodeCombination(accountingCodeDTO, userId, siteId) Method.");
            string insertAccountingCodeQuery = @"insert into AccountingCodeCombination 
                                                        (                                                         
                                                         ObjectType,
                                                         TransactionType,
                                                         AccountingCode,
                                                         Description,
                                                         Tax,
                                                         ObjectId,
                                                         ObjectName,
                                                         LastUpdatedDate,
                                                         LastUpdatedUser,
                                                         Guid,
                                                         MasterEntityId,
                                                         site_id,
                                                         IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                         @objectType,
                                                         @transactionType,
                                                         @accountingCode,
                                                         @description,
                                                         @tax,
                                                         @objectId,
                                                         @objectName,
                                                         Getdate(),
                                                         @lastUpdatedUser,
                                                         NewId(),
                                                         @masterEntityId,
                                                         @siteId,
                                                         @IsActive
                                            )SELECT CAST(scope_identity() AS int)";

            List<SqlParameter> updateAccountingCodeParameters = new List<SqlParameter>();

            if (string.IsNullOrEmpty(accountingCodeDTO.ObjectType))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectType", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectType", accountingCodeDTO.ObjectType));
            }
            if (string.IsNullOrEmpty(accountingCodeDTO.TransactionType))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@transactionType", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@transactionType", accountingCodeDTO.TransactionType));
            }
            if (string.IsNullOrEmpty(accountingCodeDTO.AccountingCode))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@accountingCode", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@accountingCode", accountingCodeDTO.AccountingCode));
            }
            if (string.IsNullOrEmpty(accountingCodeDTO.Description))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@description", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@description", accountingCodeDTO.Description));
            }

            if (accountingCodeDTO.Tax == -1)
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@tax", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@tax", accountingCodeDTO.Tax));
            }
            if (accountingCodeDTO.ObjectId == -1)
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectId", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectId", accountingCodeDTO.ObjectId));
            }

            if (string.IsNullOrEmpty(accountingCodeDTO.ObjectName))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectName", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectName", accountingCodeDTO.ObjectName));
            }

            updateAccountingCodeParameters.Add(new SqlParameter("@lastUpdatedUser", userId));
                        
            if (accountingCodeDTO.MasterEntityId == -1)
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@masterEntityId", accountingCodeDTO.MasterEntityId));
            }
            if (siteId == -1)
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@siteId", siteId));
            }
            updateAccountingCodeParameters.Add(new SqlParameter("@IsActive", accountingCodeDTO.IsActive));
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertAccountingCodeQuery, updateAccountingCodeParameters.ToArray(), sqlTransaction);
            log.Debug("Ends-InsertAccountingCodeCombination(AccountingCodeDTO, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the AccountingCodeCombination record
        /// </summary>
        /// <param name="accountingCodeDTO">AccountingCodeCombinationDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateAccountingCodeCombination(AccountingCodeCombinationDTO accountingCodeDTO, string userId, int siteId)
        {
            log.Debug("Starts-UpdateAccountingCodeCombination(accountingCodeDTO, userId, siteId) Method.");
            string updateAccountingCodeQuery = @"update AccountingCodeCombination 
                                                    set  ObjectType = @ObjectType,
                                                         TransactionType =@TransactionType,
                                                         AccountingCode =@AccountingCode,
                                                         Description =@Description,
                                                         Tax =@Tax,
                                                         ObjectId =@ObjectId,
                                                         ObjectName =@ObjectName,
                                                         LastUpdatedDate = Getdate(),
                                                         LastUpdatedUser =@LastUpdatedUser,
                                                         MasterEntityId =@MasterEntityId,
                                                         IsActive = @IsActive                  
                                                   where Id = @id";
            List<SqlParameter> updateAccountingCodeParameters = new List<SqlParameter>();

            updateAccountingCodeParameters.Add(new SqlParameter("@id", accountingCodeDTO.Id));

            if (string.IsNullOrEmpty(accountingCodeDTO.ObjectType))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectType", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectType", accountingCodeDTO.ObjectType));
            }
            if (string.IsNullOrEmpty(accountingCodeDTO.TransactionType))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@transactionType", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@transactionType", accountingCodeDTO.TransactionType));
            }
            if (string.IsNullOrEmpty(accountingCodeDTO.AccountingCode))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@accountingCode", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@accountingCode", accountingCodeDTO.AccountingCode));
            }
            if (string.IsNullOrEmpty(accountingCodeDTO.Description))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@description", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@description", accountingCodeDTO.Description));
            }

            if (accountingCodeDTO.Tax == -1)
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@tax", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@tax", accountingCodeDTO.Tax));
            }
            if (accountingCodeDTO.ObjectId == -1)
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectId", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectId", accountingCodeDTO.ObjectId));
            }

            if (string.IsNullOrEmpty(accountingCodeDTO.ObjectName))
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectName", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@objectName", accountingCodeDTO.ObjectName));
            }

            updateAccountingCodeParameters.Add(new SqlParameter("@lastUpdatedUser", userId));
            if (accountingCodeDTO.MasterEntityId == -1)
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@masterEntityId", accountingCodeDTO.MasterEntityId));
            }
            if (siteId == -1)
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            }
            else
            {
                updateAccountingCodeParameters.Add(new SqlParameter("@siteId", siteId));
            }
            updateAccountingCodeParameters.Add(new SqlParameter("@IsActive", accountingCodeDTO.IsActive));
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateAccountingCodeQuery, updateAccountingCodeParameters.ToArray(), sqlTransaction);
            log.Debug("Ends-UpdateAccountingCodeCombination(AccountingCodeDTO, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to AccountingCodeCombinationDTO class type
        /// </summary>
        /// <param name="accountingCodeDataRow">AccountingCodeCombinationDTO DataRow</param>
        /// <returns>Returns AccountingCodeCombinationDTO</returns>
        private AccountingCodeCombinationDTO GetAccountingCodeCombinationDTO(DataRow accountingCodeDataRow)
        {
            log.Debug("Starts-GetAccountingCodeCombinationDTO(accountingCodeDataRow) Method.");
            AccountingCodeCombinationDTO accountingCodeDataObject = new AccountingCodeCombinationDTO(Convert.ToInt32(accountingCodeDataRow["Id"]),
                                                    accountingCodeDataRow["ObjectType"].ToString(),
                                                    accountingCodeDataRow["TransactionType"].ToString(),
                                                    accountingCodeDataRow["AccountingCode"].ToString(),
                                                    accountingCodeDataRow["Description"].ToString(),
                                                    accountingCodeDataRow["Tax"] == DBNull.Value ? -1 : Convert.ToInt32(accountingCodeDataRow["Tax"]),
                                                    accountingCodeDataRow["ObjectId"] == DBNull.Value ? -1 : Convert.ToInt32(accountingCodeDataRow["ObjectId"]),
                                                    accountingCodeDataRow["ObjectName"].ToString(),
                                                    accountingCodeDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(accountingCodeDataRow["LastUpdatedDate"]),
                                                    accountingCodeDataRow["LastUpdatedUser"].ToString(),
                                                    accountingCodeDataRow["Guid"].ToString(),
                                                    accountingCodeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(accountingCodeDataRow["SynchStatus"]),
                                                    accountingCodeDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(accountingCodeDataRow["MasterEntityId"]),
                                                    accountingCodeDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(accountingCodeDataRow["site_id"]),
                                                    accountingCodeDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(accountingCodeDataRow["IsActive"])
                                                    );
            log.Debug("Ends-GetAccountingCodeCombinationDTO(accountingCodeDataRow) Method.");
            return accountingCodeDataObject;
        }

        /// <summary>
        /// Gets the AccountingCodeCombination data of passed Id
        /// </summary>
        /// <param name="userId">integer type parameter</param>
        /// <returns>Returns AccountingCodeCombinationDTO</returns>
        public AccountingCodeCombinationDTO GetAccountingCode(int id)
        {
            log.Debug("Starts-GetAccountingCode(id) Method.");
            string selectAccountingCodeQuery = @"select *
                                                from AccountingCodeCombination
                                                where Id = @id";
            SqlParameter[] selectAccountingCodeParameters = new SqlParameter[1];
            selectAccountingCodeParameters[0] = new SqlParameter("@id", id);
            DataTable accountingCodeDt = dataAccessHandler.executeSelectQuery(selectAccountingCodeQuery, selectAccountingCodeParameters, sqlTransaction);
            if (accountingCodeDt.Rows.Count > 0)
            {
                DataRow accountingCodeRow = accountingCodeDt.Rows[0];
                AccountingCodeCombinationDTO AccountingCodeDataObject = GetAccountingCodeCombinationDTO(accountingCodeRow);
                log.Debug("Ends-GetAccountingCode(id) Method by returnting AccountingCodeDataObject.");
                return AccountingCodeDataObject;
            }
            else
            {
                log.Debug("Ends-GetAccountingCode(id) Method by returnting null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the AccountingCodeCombinationDTO List for category Id List
        /// </summary>
        /// <param name="categoryIdList">integer list parameter</param>
        /// <returns>Returns List of AccountingCodeDTO</returns>
        public List<AccountingCodeCombinationDTO> GetAccountingCodeDTOList(List<int> categoryIdList, bool activeRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(categoryIdList);
            List<AccountingCodeCombinationDTO> list = new List<AccountingCodeCombinationDTO>();
            string query = @"SELECT AccountingCodeCombination.*
                            FROM AccountingCodeCombination, @categoryIdList List
                            WHERE ObjectId = List.Id ";
            if (activeRecords)
            {
                query += " AND Isnull(IsActive,'1') = 1 ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@categoryIdList", categoryIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountingCodeCombinationDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Gets the AccountingCodeCombinationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountingCodeCombinationDTO matching the search criteria</returns>
        public List<AccountingCodeCombinationDTO> GetAccountingCodeList(List<KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectAccountingCodeQuery = @"select *
                                                 from AccountingCodeCombination";

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.ID)
                            || searchParameter.Key.Equals(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.OBJECTID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value);
                        }
                        else if (searchParameter.Key.Equals(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.SITEID))//starts:Modification  on 28-Jun-2016 added site id for filter
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value + " OR -1 =" + searchParameter.Value + ")");
                        }
                        else if (searchParameter.Key.Equals(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.ACCOUNTINGCODE) ||
                                  searchParameter.Key.Equals(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.OBJECTTYPE) ||
                                  searchParameter.Key.Equals(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.TRANSACTIONTYPE))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key.Equals(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.ISACTIVE))
                        {
                            query.Append(joinOperartor + " isnull(" + DBSearchParameters[searchParameter.Key] + ", '1') = " + searchParameter.Value);
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetAccountingCodeList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectAccountingCodeQuery = selectAccountingCodeQuery + query;
                selectAccountingCodeQuery = selectAccountingCodeQuery + "Order by Id";
            }

            DataTable accotingCodeData = dataAccessHandler.executeSelectQuery(selectAccountingCodeQuery, null, sqlTransaction);
            if (accotingCodeData.Rows.Count > 0)
            {
                List<AccountingCodeCombinationDTO> accotingCodeList = new List<AccountingCodeCombinationDTO>();
                foreach (DataRow accountingCodeDataRow in accotingCodeData.Rows)
                {
                    AccountingCodeCombinationDTO accountingCodeDataObject = GetAccountingCodeCombinationDTO(accountingCodeDataRow);
                    accotingCodeList.Add(accountingCodeDataObject);
                }
                log.LogMethodExit(accotingCodeList);
                return accotingCodeList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Based on the Id, appropriate Accounting Code details record will be deleted    
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>return the int</returns>
        public int DeleteAccountingCode(int id)
        {
            try
            {
                log.LogMethodEntry(id);
                string deleteQuery = @"delete from AccountingCodeCombination where Id = @id";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@id", id);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }
}
