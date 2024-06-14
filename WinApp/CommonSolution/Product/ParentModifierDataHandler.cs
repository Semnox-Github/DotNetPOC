/********************************************************************************************
 * Project Name - ParentModifierDataHandler
 * Description  - ParentModifie Datahandler class for DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 *********************************************************************************************
 *2.60       18-Feb-2019      Mehraj/Guru S A        3 tier class creation
 *2.70.2       10-Dec-2019     Jinto Thomas            Removed siteid from update query
 *2.110.00   30-Nov-2020      Abhishek                Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ParentModifierDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;

        private static readonly Dictionary<ParentModifierDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ParentModifierDTO.SearchByParameters, string>
            {
                {ParentModifierDTO.SearchByParameters.ID, "pm.Id "},
                {ParentModifierDTO.SearchByParameters.MODIFIERID, "md.id "},
                {ParentModifierDTO.SearchByParameters.PARENTMODIFIERID,"parentMD.id "},
                {ParentModifierDTO.SearchByParameters.IS_ACTIVE,"parentMD.IsActive"},
                {ParentModifierDTO.SearchByParameters.SITE_ID, "ISNULL(pm.site_id,parentMD.site_Id) "},
                 {ParentModifierDTO.SearchByParameters.MASTER_ENTITY_ID, "pm.MasterEntity)Id "}
            };

        /// <summary>
        /// Default constructor of ParentModifierDataHandler class
        /// </summary>
        public ParentModifierDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Parent Modifier Record.
        /// </summary>
        /// <param name="parentModifierDTO">ParentModifierDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ParentModifierDTO parentModifierDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parentModifierDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", parentModifierDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ModifierId", parentModifierDTO.ModifierId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentModifierId", parentModifierDTO.ParentModifierId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Price", parentModifierDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", parentModifierDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", parentModifierDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Parent Modifier record to the database
        /// </summary>
        /// <param name="parentModifierDTO">ParentModifierDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ParentModifierDTO Insert(ParentModifierDTO parentModifierDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parentModifierDTO, loginId, siteId);
            string query = @"INSERT INTO dbo.ParentModifiers
                               (
                                  [ModifierId],
                                  [ParentModifierId],
                                  [Price],
                                  [LastUpdatedBy],
                                  [LastUpdatedDate],
                                  [Guid],
                                  [site_id],
                                  [MasterEntityId],
                                  [CreatedBy],
                                  [CreationDate],
                                  [IsActive]
                               )
                         VALUES
                               (@ModifierId
                               ,@ParentModifierId
                               ,@Price
                               ,@LastUpdatedBy
                               ,getdate()
                               ,Newid() 
                               ,@site_id
                               ,@MasterEntityId
                               ,@CreatedBy
                               ,getdate()
                                ,@IsActive
                               )SELECT * FROM ParentModifiers WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parentModifierDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshParentModifierDTO(parentModifierDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(parentModifierDTO);
            return parentModifierDTO;
        }

        /// <summary>
        /// Updates the Parent Modifier record
        /// </summary>
        /// <param name="parentModifierDTO">ParentModifierDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ParentModifierDTO Update(ParentModifierDTO parentModifierDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parentModifierDTO, loginId, siteId);
            string query = @"UPDATE dbo.ParentModifiers
                               SET [ModifierId] = @ModifierId,
                                   [ParentModifierId] = @ParentModifierId,
                                   [Price] = @Price,
                                   [LastUpdatedBy] = @LastUpdatedBy,
                                   [LastUpdatedDate] = getdate(), 
                                   [IsActive] = @IsActive,
                                  [MasterEntityId] = @MasterEntityId 
                             WHERE  Id = @Id
                             SELECT * FROM ParentModifiers WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(parentModifierDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshParentModifierDTO(parentModifierDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(parentModifierDTO);
            return parentModifierDTO;
        }

        /*
         int id, int modifierid, int parentmodifiersid,  double price, string lastUpdatedBy, DateTime lastUpdateDate,string guid, int site_id, 
        int masterEntityId, string createdBy, DateTime creationDate*/
        /// <summary>
        /// Converts the Data row object to ParentModifierDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ParentModifierDTO</returns>
        private ParentModifierDTO GetParentModifierDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ParentModifierDTO parentModifierDTO = new ParentModifierDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["ModifierId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ModifierId"]),
                                            dataRow["ParentModifierId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentModifierId"]),
                                            dataRow["ParentModifierName"].ToString(),
                                            dataRow["price"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["price"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(parentModifierDTO);
            return parentModifierDTO;
        }

        private void RefreshParentModifierDTO(ParentModifierDTO parentModifierDTO, DataTable dt)
        {
            log.LogMethodEntry(parentModifierDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                parentModifierDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                parentModifierDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                parentModifierDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                parentModifierDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                parentModifierDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                parentModifierDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                parentModifierDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the ParentModifier data of passed ParentModifier Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ParentModifierDTO</returns>
        public ParentModifierDTO GetParentModifierDTO(int id)
        {
            log.LogMethodEntry(id);
            ParentModifierDTO parentModifierDTO = null;
            string query = @"select ISNULL(pm.Id, -1) as Id,
                                     md.id as modifierId,
                                     ms.SetName,
                                     parentMD.id as ParentModifierId ,
                                     p.product_name as ParentModifierName, 
                                     pm.Price,
                                     pm.IsActive,
                                     pm.CreatedBy,
                                     pm.CreationDate,
                                     pm.LastUpdatedBy,
                                     pm.LastUpdatedDate,
                                     pm.Guid,
                                     ISNULL(pm.site_id,parentMD.site_Id) as site_id,
                                     pm.MasterEntityId,
                                     pm.SynchStatus
                                from ModifierSetDetails md join ModifierSet ms on  md.ModifierSetId = ms.ModifierSetId
                                     join  ModifierSetDetails parentMD on  parentMD.ModifierSetId = ms.ParentModifierSetId
                                     left outer join ParentModifiers pm on pm.ParentModifierId = parentMd.Id and md.id = pm.ModifierId,
                                     products p
                               where p.product_id = parentMD.ModifierProductId 
                                 and pm.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                parentModifierDTO = GetParentModifierDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(parentModifierDTO);
            return parentModifierDTO;
        }


        /// <summary>
        /// Gets the ParentModifierDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ParentModifierDTO matching the search criteria</returns>
        public List<ParentModifierDTO> GetParentModifierDTOList(List<KeyValuePair<ParentModifierDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ParentModifierDTO> parentModifierDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = @"select ISNULL(pm.Id, -1) as Id,
                                          md.id as modifierId,
                                          ms.SetName,
                                          parentMD.id as ParentModifierId ,
                                          p.product_name as ParentModifierName, 
                                          pm.Price,
                                          pm.IsActive,
                                          pm.CreatedBy,
                                          pm.CreationDate,
                                          pm.LastUpdatedBy,
                                          pm.LastUpdatedDate,
                                          pm.Guid,
                                          ISNULL(pm.site_id,parentMD.site_Id) as site_id,
                                          pm.MasterEntityId,
                                          pm.SynchStatus
                                    from ModifierSetDetails md join ModifierSet ms on  md.ModifierSetId = ms.ModifierSetId
                                         join  ModifierSetDetails parentMD on  parentMD.ModifierSetId = ms.ParentModifierSetId
                                         left outer join ParentModifiers pm on pm.ParentModifierId = parentMd.Id and md.id = pm.ModifierId,
                                         products p
                                   where p.product_id = parentMD.ModifierProductId ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" ");
                foreach (KeyValuePair<ParentModifierDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = " and ";
                        if (searchParameter.Key == ParentModifierDTO.SearchByParameters.ID ||
                            searchParameter.Key == ParentModifierDTO.SearchByParameters.MODIFIERID ||
                            searchParameter.Key == ParentModifierDTO.SearchByParameters.PARENTMODIFIERID ||
                            searchParameter.Key == ParentModifierDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParentModifierDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParentModifierDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                parentModifierDTOList = new List<ParentModifierDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ParentModifierDTO parentModifierDTO = GetParentModifierDTO(dataRow);
                    parentModifierDTOList.Add(parentModifierDTO);
                }
            }
            log.LogMethodExit(parentModifierDTOList);
            return parentModifierDTOList;
        }

        /// <summary>
        /// Based on the id, appropriate ParentModifiers record will be deleted
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>return the int</returns>
        public int Delete(int id)
        {
            log.LogMethodEntry(id);
            try
            {
                string deleteQuery = @"delete from ParentModifiers where Id = @id";
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
