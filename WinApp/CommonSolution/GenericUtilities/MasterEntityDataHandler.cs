/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - MasterEntityDataHandler 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// class of MasterEntityDataHandler
    /// </summary>
    public class MasterEntityDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private Semnox.Core.Utilities.Utilities Utilities;
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// MasterEntityDataHandler constructor
        /// </summary>
        /// <param name="_utilities"></param>
        public MasterEntityDataHandler(Semnox.Core.Utilities.Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            Utilities = _utilities;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// return the organization details
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrganizationDetails()
        {
            log.LogMethodEntry();
            
            string query = @"SELECT * 
                              FROM Organization 
                              WHERE ParentOrgId is null ORDER BY OrgName";

            DataTable dt = dataAccessHandler.executeSelectQuery(query, new SqlParameter[0]);

            if (dt != null && dt.Rows.Count > 0 )
            {
                log.LogMethodExit(dt);
                return dt;
            }
            log.LogMethodExit();
            return new DataTable();
        }

        /// <summary>
        /// Check the organization
        /// </summary>
        /// <param name="orgId"></param>
        public void CheckOrganization(object orgId)
        {
            log.LogMethodEntry(orgId);
            string query = @"SELECT 1 
                                FROM getOrgstructure(@orgId)";

            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@orgId", orgId);

            dataAccessHandler.executeSelectQuery(query, selectParameters);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Organization child details
        /// </summary>
        /// <param name="parentOrgId"></param>
        /// <returns></returns>
        public DataTable GetOrganizationChildren(int parentOrgId)
        {
            log.LogMethodEntry(parentOrgId);

            string query = @"SELECT * 
                                FROM Organization 
                                WHERE ParentOrgId = @orgId 
                                        ORDER BY OrgName";

            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@orgId", parentOrgId);

            DataTable dt = dataAccessHandler.executeSelectQuery(query, selectParameters);

            if(dt != null && dt.Rows.Count > 0)
            {
                log.LogMethodExit(dt);
                return dt;
            }

            log.LogMethodExit();
            return new DataTable();
        }

        /// <summary>
        /// Returns the child sites
        /// </summary>
        /// <param name="parentOrgId"></param>
        /// <returns></returns>
        public DataTable GetChildSites(int parentOrgId)
        {
            log.LogMethodEntry(parentOrgId);
            string query = @"SELECT * 
                                FROM site 
                                WHERE OrgId = @orgId 
                                     ORDER BY site_name";

            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@orgId", parentOrgId);

            DataTable dt = dataAccessHandler.executeSelectQuery(query, selectParameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                log.LogMethodExit(dt);
                return dt;
            }

            log.LogMethodExit();
            return new DataTable();
        }

        /// <summary>
        /// Returns the Site Detail based on the input parameter
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="siteId"></param>
        /// <param name="serachCriteria"></param>
        /// <returns></returns>
        public DataTable GetSiteDetails(string tableName, string columns, int siteId, string serachCriteria)
        {
            log.LogMethodEntry(tableName, columns, siteId, serachCriteria);

            object pkColumn = GetPKColName(tableName);

            string query = "SELECT  '' as [Master Site " + pkColumn + "], "+ columns + " FROM " + tableName + " WHERE site_id = @siteId and masterEntityId is null";

            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@siteId", siteId);

            if(!string.IsNullOrEmpty(serachCriteria))
            {
                query = query + " AND (" + serachCriteria + ")";
            }

            query = query + " ORDER BY " + pkColumn;

            DataTable siteDetails;
            if (query.ToLower().Contains("update") || query.ToLower().Contains("delete"))
            {
                throw new Exception("Query should not contains SQL KeyWords");
            } 
            else
            {
                siteDetails = dataAccessHandler.executeSelectQuery(query, selectParameters);
            }

            log.LogMethodExit(siteDetails);
            return siteDetails;
        }

        /// <summary>
        /// Returns the PK Column from table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public object GetPKColName(string tableName)
        {
            log.LogMethodEntry(tableName);
            string Query = @"select c.name column_name
		                                            from sys.tables t, sys.columns c 
		                                            where c.object_id = t.object_id
		                                            and c.is_identity = 1
		                                            and t.name = @TableName";

            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@tableName", tableName);

            object pkColName = ExecuteScalar(Query, selectParameters);

            if (pkColName == null)
            {
                log.Debug("Ends-getPKColName() method");
                throw new ApplicationException("Primary Key column not found for table: " + tableName);
            }

            log.LogMethodExit(pkColName);
            return pkColName;
        }

        object ExecuteScalar(string query, SqlParameter[] sqlParameters, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(query, sqlParameters, SQLTrx);
            DataTable dt = dataAccessHandler.executeSelectQuery(query, sqlParameters, SQLTrx);

            if (dt != null && dt.Rows.Count > 0)
            {
                log.LogMethodExit(dt.Rows[0][0]);
                return dt.Rows[0][0];
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Returns the Master Detail based on the input parameter
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="siteId"></param>
        /// <param name="serachCriteria"></param>
        /// <returns></returns>
        public DataTable GetMasterDetails(string tableName, string columns, int siteId, string serachCriteria)
        {
            log.LogMethodEntry(tableName, columns, siteId, serachCriteria);

            object pkColumn = GetPKColName(tableName);

            string query = "SELECT " + columns + " FROM " + tableName + " p1" +
                            " WHERE (p1.site_id is null or p1.site_id= @masterSiteId) " +
                            " AND NOT EXISTS(SELECT 'X' FROM " + tableName + " p2 WHERE p2.site_id= @siteId AND p1." + pkColumn + " = p2.MasterEntityId)";

            SqlParameter[] selectParameters = new SqlParameter[2];
            selectParameters[0] = new SqlParameter("@siteId", siteId);
            selectParameters[1] = new SqlParameter("@masterSiteId", Utilities.ParafaitEnv.SiteId);

            if (!string.IsNullOrEmpty(serachCriteria))
            {
                query = query + " AND (" + serachCriteria + ")";
            }

            query = query + " ORDER BY " + pkColumn;

            DataTable masterDetails;
            if (query.ToLower().Contains("update") || query.ToLower().Contains("delete"))
            {
                throw new Exception("Query should not contains SQL KeyWords");
            }
            else
            {
                masterDetails = dataAccessHandler.executeSelectQuery(query, selectParameters);
            }

            log.LogMethodExit(masterDetails);
            return masterDetails;
        }

        /// <summary>
        /// Check the active flag column is valid or not
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="activeFlagColumnName"></param>
        /// <returns></returns>
        public bool IsActiveFlagValid(string entity, string activeFlagColumnName)
        {
            log.LogMethodEntry(entity, activeFlagColumnName);
            string query = @"SELECT NAME as COLUMN_NAMES FROM sys.columns WHERE object_id = OBJECT_ID(@entity)";

            SqlParameter[] selectParams = new SqlParameter[1];
            selectParams[0] = new SqlParameter("@entity", entity);

            DataTable dt = dataAccessHandler.executeSelectQuery(query, selectParams);

            if (dt != null && dt.Rows.Count > 0)
            {

                bool roleExist = dt.AsEnumerable().Where(c => c.Field<string>("COLUMN_NAMES").Equals(activeFlagColumnName, StringComparison.CurrentCultureIgnoreCase)).Count() > 0;
                if (roleExist)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }

            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Retuns datatype of the passed Column
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        object GetDataType(string entity, string columnName)
        {
            log.LogMethodEntry(entity, columnName);
            string query = @"SELECT DATA_TYPE 
                                FROM INFORMATION_SCHEMA.COLUMNS 
                                WHERE  TABLE_NAME = @entity
                                AND COLUMN_NAME = @columnName" ;

            SqlParameter[] selectParams = new SqlParameter[2];
            selectParams[0] = new SqlParameter("@entity", entity);
            selectParams[1] = new SqlParameter("@columnName", columnName);

            DataTable dataDT = dataAccessHandler.executeSelectQuery(query, selectParams);

            if (dataDT != null && dataDT.Rows.Count > 0)
            {
                log.LogMethodExit(dataDT.Rows[0][0]);
                return dataDT.Rows[0][0];
            }

            log.LogMethodExit();
            return null;
        }
    }
}
