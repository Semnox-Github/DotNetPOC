using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Semnox.Core.Lookups;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.MasterEntity
{
    /// <summary>
    /// class of MasterEntity
    /// </summary>
    public class MasterEntity
    {
        Utilities Utilities;
        MasterEntityDataHandler masterEntityDataHandler;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_utilities"></param>
        public MasterEntity(Semnox.Core.Utilities.Utilities _utilities)
        {
            log.Debug("Starts-MasterEntity() default constructor");
            Utilities = _utilities;
            masterEntityDataHandler = new MasterEntityDataHandler(Utilities);
            log.Debug("Ends-MasterEntity() default constructor");
        }

        /// <summary>
        /// GetOrganizationDetails() method
        /// </summary>
        /// <returns></returns>
        #region TreeNodes Methods
        public DataTable GetOrganizationDetails()
        {
            log.Debug("Starts-GetOrganizationDetails() BL method");
            log.Debug("Ends-GetOrganizationDetails() BL method");
           return masterEntityDataHandler.GetOrganizationDetails();
        }

        /// <summary>
        /// CheckOrganization(orgId) method
        /// </summary>
        /// <param name="orgId"></param>
        public void CheckOrganization(object orgId)
        {
            log.Debug("Starts-CheckOrganization() BL method");
            masterEntityDataHandler.CheckOrganization(orgId);
            log.Debug("ends-CheckOrganization() BL method");
        }

        /// <summary>
        /// GetOrganizationChildren(parentOrgId) method
        /// </summary>
        /// <param name="parentOrgId"></param>
        /// <returns></returns>
        public DataTable GetOrganizationChildren(int parentOrgId)
        {
            log.Debug("Starts-GetOrganizationChildren() BL method");
            log.Debug("ends-GetOrganizationChildren() BL method");
            return masterEntityDataHandler.GetOrganizationChildren(parentOrgId);
        }

        /// <summary>
        /// GetChildSites(parentOrgId) method
        /// </summary>
        /// <param name="parentOrgId"></param>
        /// <returns></returns>
        public DataTable GetChildSites(int parentOrgId)
        {
            log.Debug("Starts-GetChildSites() BL method");
            log.Debug("ends-GetChildSites() BL method");
            return masterEntityDataHandler.GetChildSites(parentOrgId);
        }
        #endregion

        #region UI Grid details related methods
        /// <summary>
        /// returns the site details based on in the input parameter
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columns"></param>
        /// <param name="siteId"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public DataTable GetSiteDetails(string entityName, string columns, int siteId, string searchCriteria)
        {
            log.Debug("Starts-GetSiteDetails() BL method");
            DataTable siteDetails = masterEntityDataHandler.GetSiteDetails(entityName, columns, siteId, searchCriteria);
            log.Debug("ends-GetSiteDetails() BL method");
            return siteDetails;
        }

        /// <summary>
        /// returns the Master details based on in the input parameter
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columns"></param>
        /// <param name="siteId"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public DataTable GetMasterDetails(string entityName, string columns, int siteId, string searchCriteria)
        {
            log.Debug("Starts-GetMasterDetails() BL method");
            DataTable masterDetails = masterEntityDataHandler.GetMasterDetails(entityName, columns, siteId, searchCriteria);
            log.Debug("Starts-GetMasterDetails() BL method");
            return masterDetails;
        }

        /// <summary>
        /// Check the active flag column name is valid or not
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="activeFlagColumnName"></param>
        /// <returns></returns>
        public bool IsActiveFlagValid(string entity, string activeFlagColumnName)
        {
            return masterEntityDataHandler.IsActiveFlagValid(entity, activeFlagColumnName);
        }

        /// <summary>
        /// Returns true if passed key ir primary key of the table
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool IsPrimaryKey(string entityName, string columnName)
        {
            try
            {
               object pkName = masterEntityDataHandler.GetPKColName(entityName);

                if(columnName.ToLower() == pkName.ToString().ToLower())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }


        /// <summary>
        /// Publish all entity in the list 
        /// </summary>
        /// <param name="lstEntityDetails"></param>
        /// <returns></returns>
        public bool PublishAllEntities(List<EntityDetails> lstEntityDetails)
        {
            log.Debug("Starts-PublishAllEntity(lstEntityDetails) BL method");
            bool published = false;

            SqlConnection TrxCnn = Utilities.createConnection();
            SqlTransaction SQLTrx = TrxCnn.BeginTransaction();

            try
            {
                Semnox.Parafait.Publish.Publish publish = new Semnox.Parafait.Publish.Publish(lstEntityDetails[0].entityName, Utilities);
                published = publish.PublishAllEntity(lstEntityDetails, SQLTrx);
            }
            catch(Exception ex)
            {
                log.Debug("Ends-PublishAllEntity(lstEntityDetails) BL method");
                published = false;

                if (SQLTrx != null)
                    SQLTrx.Rollback();
                    
                if (TrxCnn != null)
                    TrxCnn.Close();

                throw new Exception(ex.Message);
            }

            if(published)
            {
                if (SQLTrx != null)
                    SQLTrx.Commit();

                if (TrxCnn != null)
                    TrxCnn.Close();
                log.Debug("Ends-PublishAllEntity(lstEntityDetails) BL method");
                return true;
            }
            else
            {
                if (SQLTrx != null)
                    SQLTrx.Rollback();

                if (TrxCnn != null)
                    TrxCnn.Close();
                log.Debug("Ends-PublishAllEntity(lstEntityDetails) BL method");
            }
            return false;
        }
        #endregion
    }
}
