/********************************************************************************************
 * Project Name - EntityOverrideDate
 * Description  - Bussiness logic of EntityOverrideDate
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By     Remarks          
 *********************************************************************************************
 *1.00       10-July-2017     Amaresh          Created
 *2.60       23-Jan-2019      Jagan Mohana     Created constructor EntityOverrideList and
 *                                             added new method SaveUpdateEntityOverrideDatesList
             22-Mar-2019      Nagesh Badiger   Added log method entry in SaveUpdateEntityOverrideDatesList method and EntityOverrideList constructor is modify for execution context
 *2.70       29-June-2019     Indrajeet Kumar  Created DeleteEntityOverrideDateList() method for Hard Deletion & 
 *                                             Modified Delete() method Catch Block and return.
 *2.70.2       26-Jul-2019      Dakshakh raj     Modified : Log method entries/exits, Save method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// EntityOverrideDate will creates and modifies the EntityOverrideDate
    /// </summary>
    public class EntityOverrideDate
    {
        private EntityOverrideDatesDTO entityOverrideDatesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public EntityOverrideDate(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            entityOverrideDatesDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the entityOverride DTO parameter
        /// </summary>
        /// <param name="entityOverrideDatesDTO">Parameter of the type EntityOverrideDatesDTO</param>
        /// <param name="executionContext">executionContext</param>
        public EntityOverrideDate(ExecutionContext executionContext, EntityOverrideDatesDTO entityOverrideDatesDTO)
        {
            log.LogMethodEntry(entityOverrideDatesDTO, executionContext);
            this.entityOverrideDatesDTO = entityOverrideDatesDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the entityOverride  
        /// entityOverride   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(int parentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            EntityOverrideDatesDH entityOverrideDateDH = new EntityOverrideDatesDH(sqlTransaction);
            if (entityOverrideDatesDTO.IsChanged)
            {
                if (entityOverrideDatesDTO.ID < 0)
                {
                    entityOverrideDatesDTO = entityOverrideDateDH.InsertEntityOverride(entityOverrideDatesDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                    entityOverrideDatesDTO.AcceptChanges();
                }
                else
                {
                    if (entityOverrideDatesDTO.IsChanged)
                    {
                        entityOverrideDatesDTO = entityOverrideDateDH.UpdateEntityOverride(entityOverrideDatesDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                        entityOverrideDatesDTO.AcceptChanges();
                    }
                }
                CreateRoamingData(parentSiteId, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the EntityOverrideDateDTO based on Id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public int Delete(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            try
            {
                EntityOverrideDatesDH entityOverrideDateDH = new EntityOverrideDatesDH(sqlTransaction);
                int statusId = entityOverrideDateDH.DeleteEntityOverride(id, sqlTransaction);
                log.LogMethodExit(statusId);
                return statusId;
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the customer Entity Override Date DTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        private void CreateRoamingData(int parentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            if (parentSiteId > -1 && parentSiteId != entityOverrideDatesDTO.SiteId && executionContext.GetSiteId() > -1
                    && entityOverrideDatesDTO.ID > -1)
            {
                Type type = Type.GetType("Semnox.Parafait.DBSynch.DBSynchLogBL,DBSynch");
                object dBSynchLogService = null;
                if (type != null)
                {
                    string tempString = string.Empty;
                    ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), tempString.GetType(), tempString.GetType(), parentSiteId.GetType(), tempString.GetType() });
                    dBSynchLogService = constructorN.Invoke(new object[] { executionContext, "EntityOverrideDates", entityOverrideDatesDTO.Guid, parentSiteId, "I" });
                }
                else
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "DBSynch DBSynchLogBL"));
                if (dBSynchLogService != null)
                {
                    type.GetMethod("Save").Invoke(dBSynchLogService, new object[] { sqlTransaction });
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of entityOverride List
    /// </summary>
    public class EntityOverrideList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<EntityOverrideDatesDTO> entityOverrideDatesList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityOverrideList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public EntityOverrideList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.entityOverrideDatesList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="entityOverrideDatesList">entityOverrideDatesList</param>
        /// <param name="executionContext">executionContext</param>
        public EntityOverrideList(ExecutionContext executionContext, List<EntityOverrideDatesDTO> entityOverrideDatesList)
        {
            log.LogMethodEntry(entityOverrideDatesList, executionContext);
            this.entityOverrideDatesList = entityOverrideDatesList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the EntityOverrideDatesDTO
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public EntityOverrideDatesDTO GetEntityOverride(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            EntityOverrideDatesDH entityOverrideDateDH = new EntityOverrideDatesDH(sqlTransaction);
            log.LogMethodExit();
            return entityOverrideDateDH.GetEntityOverride(id);
        }

        /// <summary>
        /// Returns the List of EntityOverrideDatesDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<EntityOverrideDatesDTO> GetAllEntityOverrideList(List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            EntityOverrideDatesDH entityOverrideDateDH = new EntityOverrideDatesDH(sqlTransaction);
            List<EntityOverrideDatesDTO> entityOverrideDatesDTOList = entityOverrideDateDH.GetEntityOverrideList(searchParameters);
            log.LogMethodExit(entityOverrideDatesDTOList);
            return entityOverrideDatesDTOList;
        }
        /// <summary>
        ///  Returns the List of EntityOverrideDatesDTO corresponding to the accountCreditplus
        /// </summary>
        /// <param name="accountIdList">accountIdList</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<EntityOverrideDatesDTO> GetEntityOverrideDatesDTOListForAccountCreditPlus(string accountIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(accountIdList, sqlTransaction);
            EntityOverrideDatesDH entityOverrideDateDH = new EntityOverrideDatesDH(sqlTransaction);
            List<EntityOverrideDatesDTO> entityOverrideDatesDTOList = entityOverrideDateDH.GetEntityOverrideDatesDTOListForAccountCreditPlus(accountIdList, sqlTransaction);
            log.LogMethodExit(entityOverrideDatesDTOList);
            return entityOverrideDatesDTOList;
        }

        /// <summary>
        ///  Returns the List of EntityOverrideDatesDTO corresponding to the accountCreditplus
        /// </summary>
        /// <param name="accountIdList">accountIdList</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<EntityOverrideDatesDTO> GetEntityOverrideDatesDTOListForAccountCreditPlusByAccountIds(List<int> accountIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(accountIdList, sqlTransaction);
            EntityOverrideDatesDH entityOverrideDateDH = new EntityOverrideDatesDH(sqlTransaction);
            List<EntityOverrideDatesDTO> entityOverrideDatesDTOList = entityOverrideDateDH.GetEntityOverrideDatesDTOListForAccountCreditPlusByAccountIds(accountIdList, sqlTransaction);
            log.LogMethodExit(entityOverrideDatesDTOList);
            return entityOverrideDatesDTOList;
        }
        /// <summary>
        /// Returns the List of EntityOverrideDatesDTO corresponding to the accountgame
        /// </summary>
        /// <param name="accountIdList">accountIdList</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<EntityOverrideDatesDTO> GetEntityOverrideDatesDTOListForAccountGame(string accountIdList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(accountIdList, sqlTransaction);
            EntityOverrideDatesDH entityOverrideDateDH = new EntityOverrideDatesDH(sqlTransaction);
            List<EntityOverrideDatesDTO> entityOverrideDatesDTOList = entityOverrideDateDH.GetEntityOverrideDatesDTOListForAccountGame(accountIdList, sqlTransaction);
            log.LogMethodExit(entityOverrideDatesDTOList);
            return entityOverrideDatesDTOList;
        }

        /// <summary>
        /// Saves or update the Entity Override Dates
        /// </summary>
        public void SaveUpdateEntityOverrideDatesList(int parentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            try
            {
                log.LogMethodEntry();
                if (entityOverrideDatesList != null)
                {
                    foreach (EntityOverrideDatesDTO entityOverrideDatesDTO in entityOverrideDatesList)
                    {
                        EntityOverrideDate entityOverrideDateObj = new EntityOverrideDate(executionContext, entityOverrideDatesDTO);
                        entityOverrideDateObj.Save(parentSiteId, sqlTransaction); 
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the EntityOverrideDateList details based on Id
        /// </summary>
        public void DeleteEntityOverrideDateList()
        {
            log.LogMethodEntry();
            if (entityOverrideDatesList != null && entityOverrideDatesList.Count > 0)
            {
                foreach (EntityOverrideDatesDTO entityOverrideDatesDTO in entityOverrideDatesList)
                {
                    if (entityOverrideDatesDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();

                                EntityOverrideDate entityOverrideDate = new EntityOverrideDate(executionContext);
                                entityOverrideDate.Delete(entityOverrideDatesDTO.ID, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
