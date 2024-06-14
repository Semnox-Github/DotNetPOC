/********************************************************************************************
 * Project Name - POSMachine Data Handler
 * Description  - POSMachine handler of the POSMachine class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        25-Dec-2016   Vinayaka V          Created 
 *********************************************************************************************
 *1.00        02-Jan-2017   Raghuveera          Modified 
 *2.50        11-Feb-2019   Mushahid Faizan     Modified - Added isActive Parameter.
 *2.00        04-Mar-2019   Indhu               Modified for Remote Shift Open/Close changes
 *2.70        08-jul-2019   Deeksha             Modified:Added GetSqlParameter(),SQL injection issue Fix
 *                                                       createdBy and lastUpdateDate &lastUpdateBy fields
 *2.70        16-Jul-2019   Akshay G            Added DeletePOSMachines() method
 *2.70.2      10-Dec-2019   Jinto Thomas        Removed siteid from update query
 *2.70.2      04-Feb-2020   Nitin Pai           Guest App phase 2 changes
 *2.80.0      02-Apr-2020   Akshay G            Added POS_MACHINE_ID_LIST searchParameter
 *2.130.0     12-Jul-2021   Lakshminarayana     Modified : Static menu enhancement
 *2.140.0     14-Sep-2021   Deeksha             Modified : POS attendance, Provisional shift and Cash Drawer related changes
 *2.140       14-Sep-2021   Fiona               Modified: Issue fix in GetPOSMachineList
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// POSMachine Data Handler - Handles insert, update and select of POSMachine Data
    /// </summary>
    public class POSMachineDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM POSMachines AS posm ";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Dictionary for searching Parameters for the POSMachine object.
        /// </summary>
        private static readonly Dictionary<POSMachineDTO.SearchByPOSMachineParameters, string> DBSearchParameters = new Dictionary<POSMachineDTO.SearchByPOSMachineParameters, string>
            {
                {POSMachineDTO.SearchByPOSMachineParameters.COMPUTER_NAME, "posm.Computer_Name"},
                {POSMachineDTO.SearchByPOSMachineParameters.INVENTORY_LOCATION_ID, "posm.InventoryLocationId"},
                {POSMachineDTO.SearchByPOSMachineParameters.IP_ADDRESS, "posm.IPAddress"},
                {POSMachineDTO.SearchByPOSMachineParameters.LEGAL_ENTITY, "posm.Legal_Entity"},
                {POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID, "posm.POSMachineId"},
                {POSMachineDTO.SearchByPOSMachineParameters.POS_NAME, "posm.POSName"},
                {POSMachineDTO.SearchByPOSMachineParameters.POS_TYPE_ID, "posm.POSTypeId"},
                {POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, "posm.site_id"},
                {POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "posm.IsActive"},
                {POSMachineDTO.SearchByPOSMachineParameters.POS_TYPE_ID_LIST, "posm.POSTypeId"},
                {POSMachineDTO.SearchByPOSMachineParameters.POS_NAME_LIST, "posm.POSName"},
                {POSMachineDTO.SearchByPOSMachineParameters.MASTER_ENTITY_ID, "posm.MasterEntityId"},
                {POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME, "posm.POSName"},
                {POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID_LIST, "posm.POSMachineId"},
            };



        /// <summary>
        /// Default constructor of POSMachineDataHandler class
        /// </summary>
        public POSMachineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating POSMachines Record.
        /// </summary>
        /// <param name="pOSMachineDTO">POSMachineDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(POSMachineDTO pOSMachineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSMachineDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@posMachineId", pOSMachineDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posName", pOSMachineDTO.POSName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@legalEntity", pOSMachineDTO.LegalEntity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", pOSMachineDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@friendlyName", pOSMachineDTO.FriendlyName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posTypeId", pOSMachineDTO.POSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@inventoryLocationId", pOSMachineDTO.InventoryLocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ipAddress", pOSMachineDTO.IPAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@computerName", pOSMachineDTO.ComputerName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", pOSMachineDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dayBeginTime", pOSMachineDTO.DayBeginTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dayEndTime", pOSMachineDTO.DayEndTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@xReportRunTime", pOSMachineDTO.XReportRunTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdateBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", pOSMachineDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute1", pOSMachineDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute2", pOSMachineDTO.Attribute2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute3", pOSMachineDTO.Attribute3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute4", pOSMachineDTO.Attribute4));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute5", pOSMachineDTO.Attribute5));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the posMachine record to the database
        /// </summary>
        /// <param name="posMachineDTO">POSMachineDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public POSMachineDTO InsertPOSMachine(POSMachineDTO pOSMachineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSMachineDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[POSMachines] 
                                                        (    
                                                          POSName,
                                                          Legal_Entity,
                                                          Remarks,
                                                          FriendlyName,
                                                          POSTypeId,
                                                          InventoryLocationId,
                                                          IPAddress,
                                                          Computer_Name,
                                                          Guid,
                                                          site_id,
                                                          MasterEntityId,
                                                          DayBeginTime,
                                                          DayEndTime,
                                                          XReportRunTime,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastUpdateDate,
                                                          IsActive,                                       
                                                        Attribute1,                                        
                                                        Attribute2,                                        
                                                        Attribute3,                                        
                                                        Attribute4,                                        
                                                        Attribute5   
                                                        ) 
                                                values 
                                                        (    
                                                          @posName,
                                                          @legalEntity,
                                                          @remarks,
                                                          @friendlyName,
                                                          @posTypeId,
                                                          @inventoryLocationId,
                                                          @ipAddress,
                                                          @computerName,
                                                          NewId(),
                                                          @site_id,
                                                          @masterEntityId,
                                                          @dayBeginTime,
                                                          @dayEndTime,
                                                          @xReportRunTime,
                                                          GETDATE(),
                                                          @lastUpdateBy,
                                                          GETDATE(),
                                                          @isActive,
                                                        @Attribute1,
                                                        @Attribute2,
                                                        @Attribute3,
                                                        @Attribute4,
                                                        @Attribute5)
                                                       SELECT* FROM POSMachines WHERE POSMachineId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSMachineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSMachinesDTO(pOSMachineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting POSMachinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(pOSMachineDTO);
            return pOSMachineDTO;
        }

        /// <summary>
        /// Inserts the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="functionalGuid">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void AddManagementFormAccess(string formName, string functionGuid,int siteId, bool isActive)
        {
            log.LogMethodEntry(formName, functionGuid, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'POS Machine',@formName,'Data Access',@siteId,@functionGuid,@isActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", isActive));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Rename the managementFormAccess record to the database
        /// </summary>
        /// <param name="newFormName">string type object</param>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void RenameManagementFormAccess(string newFormName, string formName, int siteId, string functionGuid)
        {
            log.LogMethodEntry(newFormName,formName, siteId);
            string query = @"exec RenameManagementFormAccess @newFormName,'POS Machine',@formName,'Data Access',@siteId,@functionGuid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@newFormName", newFormName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Update the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="updatedIsActive">Site to which the record belongs</param>
        /// <param name="functionGuid">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void UpdateManagementFormAccess(string formName, int siteId, bool updatedIsActive, string functionGuid)
        {
            log.LogMethodEntry(formName, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'POS Machine',@formName,'Data Access',@siteId,@functionGuid,@updatedIsActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedIsActive", updatedIsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// Updates the posMachine record
        /// </summary>
        /// <param name="posMachineDTO">POSMachineDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public POSMachineDTO UpdatePOSMachine(POSMachineDTO pOSMachineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSMachineDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[POSMachines]
                                    SET 
                                             POSName = @posName,
                                             Legal_Entity = @legalEntity,
                                             Remarks = @remarks,
                                             FriendlyName = @friendlyName,
                                             POSTypeId = @posTypeId,
                                             InventoryLocationId = @inventoryLocationId,
                                             IPAddress = @ipAddress,
                                             Computer_Name = @computerName,
                                             -- site_id = @site_id,
                                             MasterEntityId = @masterEntityId,
                                             DayBeginTime = @dayBeginTime,
                                             DayEndTime = @dayEndTime,
                                             XReportRunTime = @xReportRunTime,
                                             LastUpdateDate = GETDATE(),
                                             LastUpdatedBy = @lastUpdateBy,
                                             Attribute1 = @Attribute1 ,
                                             Attribute2 = @Attribute2 ,
                                             Attribute3 = @Attribute3 ,
                                             Attribute4 = @Attribute4 ,
                                             Attribute5 = @Attribute5 ,
                                             IsActive = @isActive
                                       WHERE POSMachineId =@posMachineId 
                                    SELECT * FROM POSMachines WHERE POSMachineId = @posMachineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSMachineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSMachinesDTO(pOSMachineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating pOSMachineDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSMachineDTO);
            return pOSMachineDTO;
        }


        /// <summary>
        /// Converts the Data row object to POSMachineDTO class type
        /// </summary>
        /// <param name="posMachineDataRow">POSMachine DataRow</param>
        /// <returns>Returns POSMachine</returns>
        private POSMachineDTO GetPOSMachineDTO(DataRow posMachineDataRow)
        {
            log.LogMethodEntry(posMachineDataRow);
            POSMachineDTO posMachineDataObject = new POSMachineDTO(Convert.ToInt32(posMachineDataRow["POSMachineId"]),
                                             posMachineDataRow["POSName"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["POSName"]),
                                            posMachineDataRow["Legal_Entity"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["Legal_Entity"]),
                                            posMachineDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["Remarks"]),
                                            posMachineDataRow["FriendlyName"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["FriendlyName"]),
                                            posMachineDataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(posMachineDataRow["POSTypeId"]),
                                            posMachineDataRow["InventoryLocationId"] == DBNull.Value ? -1 : Convert.ToInt32(posMachineDataRow["InventoryLocationId"]),
                                            posMachineDataRow["IPAddress"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["IPAddress"]),
                                            posMachineDataRow["Computer_Name"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["Computer_Name"]),
                                            posMachineDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["Guid"]),
                                            posMachineDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(posMachineDataRow["site_id"]),
                                            posMachineDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(posMachineDataRow["SynchStatus"]),
                                            posMachineDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(posMachineDataRow["MasterEntityId"]),
                                            posMachineDataRow["DayBeginTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(posMachineDataRow["DayBeginTime"]),
                                            posMachineDataRow["DayEndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(posMachineDataRow["DayEndTime"]),
                                            posMachineDataRow["XReportRunTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(posMachineDataRow["XReportRunTime"]),
                                            posMachineDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(posMachineDataRow["CreationDate"]),
                                            posMachineDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["LastUpdatedBy"]),
                                            posMachineDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(posMachineDataRow["LastUpdateDate"]),
                                            posMachineDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(posMachineDataRow["IsActive"]),
                                             posMachineDataRow["Attribute1"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["Attribute1"]),
                                             posMachineDataRow["Attribute2"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["Attribute2"]),
                                             posMachineDataRow["Attribute3"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["Attribute3"]),
                                             posMachineDataRow["Attribute4"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["Attribute4"]),
                                             posMachineDataRow["Attribute5"] == DBNull.Value ? string.Empty : Convert.ToString(posMachineDataRow["Attribute5"])

                                            );
            log.LogMethodExit(posMachineDataObject);
            return posMachineDataObject;
        }

        public List<ShiftDTO> GetOpenShiftDTOList(List<POSMachineDTO> pOSMachineDTOList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(pOSMachineDTOList, sqlTransaction);
            List<ShiftDTO> shiftList = new List<ShiftDTO>();
            StringBuilder stringBuilder = new StringBuilder();
            if (pOSMachineDTOList != null && pOSMachineDTOList.Count > 0)
            {
                for (int i = 0; i < pOSMachineDTOList.Count; i++)
                {
                    stringBuilder.Append("'");
                    stringBuilder.Append(pOSMachineDTOList[i].POSName);
                    stringBuilder.Append("'");
                    if (i < pOSMachineDTOList.Count - 1)
                        stringBuilder.Append(",");
                }
            }

            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@shiftTime", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss")); // Added to reduce the shift day and fetch only previous 7 days shift data

            string query = string.Empty;
            if (stringBuilder.Length > 0)
            {
                query = "and a.pos_machine in (" + stringBuilder.ToString() + @")";
            }

            DataTable shiftDataTable = dataAccessHandler.executeSelectQuery(@"Select * from shift a, 
	                                                (select MAX(shift_key) shift_key, pos_machine 
	                                                        from shift where shift_time > @shiftTime
	                                                        group by pos_machine) openshift
	                                                where a.pos_machine = openshift.pos_machine 
	                                                and a.shift_key = openshift.shift_key
                                                    " + query + @"
	                                                and a.shift_action in ('Open','ROpen')
	                                                order by a.shift_time", selectParameters);

            log.LogVariableState("StringBuilder", stringBuilder);
            log.LogVariableState("shiftTime", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss"));

            if (shiftDataTable.Rows.Count > 0)
            {
                ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
                foreach (DataRow shiftDataRow in shiftDataTable.Rows)
                {
                    ShiftDTO shiftDataObject = shiftDataHandler.GetShiftDTO(shiftDataRow);
                    shiftList.Add(shiftDataObject);
                }
            }
            log.LogMethodExit(shiftList);
            return shiftList;
        }

        /// <summary>
        /// Delete the record from the POSMachine database based on POSMachineId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int pOSMachineId)
        {
            log.LogMethodEntry(pOSMachineId);
            string query = @"DELETE  
                             FROM POSMachines
                             WHERE POSMachines.POSMachineId = @posMachineId";
            SqlParameter parameter = new SqlParameter("@posMachineId", pOSMachineId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="achievementClassDTO">AchievementClassDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPOSMachinesDTO(POSMachineDTO pOSMachineDTO, DataTable dt)
        {
            log.LogMethodEntry(pOSMachineDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                pOSMachineDTO.POSMachineId = Convert.ToInt32(dt.Rows[0]["POSMachineId"]);
                pOSMachineDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                pOSMachineDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                pOSMachineDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                pOSMachineDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                pOSMachineDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the POSMachine data of passed id
        /// </summary>
        /// <param name="id">id of POSMachine is passed as parameter</param>
        /// <returns>Returns POSMachine</returns>
        public POSMachineDTO GetPOSMachineDTO(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            POSMachineDTO result = null;
            string query = SELECT_QUERY + @" WHERE posm.POSMachineId= @posMachineId";
            SqlParameter parameter = new SqlParameter("@posMachineId", posMachineId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPOSMachineDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Gets the last trx no for a pos
        /// </summary>
        /// <param name="posMachineId">integer type parameter</param>
        /// <returns>Returns Last trx No</returns>
        public int GetLastTrxNo(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            string selectPOSMachineQuery = @"select isnull(max(case isnumeric(Trx_No) when 1 then cast(trx_no as int) else 0 end), '0') TrxNo
                                            from trx_header WITH(INDEX(trx_date)) 
                                            where STATUS = 'CLOSED' 
                                              AND trxDate > 
                                                    (select case when GETDATE() < DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) 
                                                            then DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 1, GETDATE()))) 
                                                            else DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) end) 
                                                and (POSMachineId = @posMachineId or @posMachineId = -1)";
            SqlParameter[] selectPOSMachineParameters = new SqlParameter[1];
            selectPOSMachineParameters[0] = new SqlParameter("@posMachineId", posMachineId);
            DataTable posMachine = dataAccessHandler.executeSelectQuery(selectPOSMachineQuery, selectPOSMachineParameters, sqlTransaction);
            if (posMachine.Rows.Count > 0)
            {
                DataRow posMachineRow = posMachine.Rows[0];
                log.LogMethodExit(Convert.ToInt32(posMachineRow["TrxNo"].ToString()));
                return Convert.ToInt32(posMachineRow["TrxNo"].ToString());
            }
            else
            {
                log.LogMethodExit(0);
                return 0;
            }
        }
        /// <summary>
        /// Gets the POSMachineDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of POSMachineDTO matching the search criteria</returns>
        public List<POSMachineDTO> GetPOSMachineList(List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<POSMachineDTO> pOSMachineDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == POSMachineDTO.SearchByPOSMachineParameters.INVENTORY_LOCATION_ID ||
                                searchParameter.Key == POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID ||
                                searchParameter.Key == POSMachineDTO.SearchByPOSMachineParameters.POS_TYPE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(POSMachineDTO.SearchByPOSMachineParameters.POS_NAME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME))
                        {
                            query.Append(joiner + ("(POSName" + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " OR COMPUTER_NAME = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " OR IPADDRESS = " + dataAccessHandler.GetParameterName(searchParameter.Key) + ")"));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSMachineDTO.SearchByPOSMachineParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSMachineDTO.SearchByPOSMachineParameters.POS_TYPE_ID_LIST ||
                                 searchParameter.Key == POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID_LIST)  //int
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSMachineDTO.SearchByPOSMachineParameters.POS_NAME_LIST) //string
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE) //bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                pOSMachineDTOList = new List<POSMachineDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    POSMachineDTO pOSMachineDTO = GetPOSMachineDTO(dataRow);
                    pOSMachineDTOList.Add(pOSMachineDTO);
                }
            }
            log.LogMethodExit(pOSMachineDTOList);
            return pOSMachineDTOList;
        }

        internal DateTime? GetPOSModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdatedDate from POSMachines WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from POSPrinters WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from POSProductExclusions WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from Peripherals WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from ProductsDisplayGroup WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from POSPaymentModeInclusions WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from ProductMenuPOSMachineMap WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from ProductMenuPanelExclusion WHERE POSMachineId IS NOT NULL AND (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from POSCashdrawers WHERE (site_id = @siteId or @siteId = -1)
                            ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }

}
