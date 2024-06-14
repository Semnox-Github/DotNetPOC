/********************************************************************************************
 * Project Name - Department Data Handler
 * Description  - Data handler of the Department class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018      Indhu               Created 
 *2.70.2        15-Jul-2019      Girish Kundar       Modified : Added GetSQLParameter(),SQL Injection Fix,Missed Who columns
 *2.70.2        11-Dec-2019      Jinto Thomas        Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class DepartmentDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<DepartmentDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DepartmentDTO.SearchByParameters, string>
        {
                {DepartmentDTO.SearchByParameters.DEPARTMENT_ID,"d.DepartmentId"},
                {DepartmentDTO.SearchByParameters.DEPARTMENT_NAME,"d.DepartmentName"},
                {DepartmentDTO.SearchByParameters.ISACTIVE,"d.ActiveFlag"},
                {DepartmentDTO.SearchByParameters.MASTER_ENTITY_ID,"d.MasterEntityId"},
                {DepartmentDTO.SearchByParameters.SITE_ID, "d.site_id"}
        };
        private const string SELECT_QUERY = @"SELECT * FROM Department AS d ";
        /// <summary>
        /// Default constructor of PrintersDataHandler class
        /// </summary>
        public DepartmentDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Department Record.
        /// </summary>
        /// <param name="departmentDTO">DepartmentDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(DepartmentDTO departmentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(departmentDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@departmentid", departmentDTO.DepartmentId, true);
            ParametersHelper.ParameterHelper(parameters, "@departmentNumber", string.IsNullOrEmpty(departmentDTO.DepartmentNumber) ? DBNull.Value : (object)departmentDTO.DepartmentNumber);
            ParametersHelper.ParameterHelper(parameters, "@departmentName", string.IsNullOrEmpty(departmentDTO.DepartmentName) ? DBNull.Value : (object)departmentDTO.DepartmentName);
            ParametersHelper.ParameterHelper(parameters, "@activeFlag", departmentDTO.ActiveFlag);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@siteId", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", departmentDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        ///  Inserts the record to the database table Department.
        /// </summary>
        /// <param name="departmentDTO">departmentDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>DepartmentDTO</returns>
        public DepartmentDTO InsertDepartment(DepartmentDTO departmentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(departmentDTO, loginId, siteId);
            string InsertDepartmentQuery = @"insert into Department
                                                       ( DepartmentNumber,
                                                         DepartmentName,
                                                         ActiveFlag,
                                                         LastUpdatedBy,
                                                         LastUpdatedDate,                                   
                                                         GUID,
                                                         site_id,
                                                         MasterEntityId,
                                                         CreatedBy,
                                                         CreationDate
                                                       )
                                               values(
                                                         @departmentNumber,
                                                         @departmentName,
                                                         @activeFlag,
                                                         @lastUpdatedBy,
                                                         Getdate(),
                                                         NewId(),
                                                         @siteId,
                                                         @masterEntityId,
                                                         @createdBy,
                                                         GETDATE()
                                                  )SELECT  * from Department where DepartmentId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertDepartmentQuery, BuildSQLParameters(departmentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDepartmentDTO(departmentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting departmentDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(departmentDTO);
            return departmentDTO;
        }

        /// <summary>
        ///  Updates the record to the database table Department.
        /// </summary>
        /// <param name="departmentDTO">departmentDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>DepartmentDTO</returns>
        public DepartmentDTO UpdateDepartment(DepartmentDTO departmentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(departmentDTO, loginId, siteId);
            string UpdateDepartmentQuery = @"update Department
                                                 SET
                                                  DepartmentNumber= @departmentNumber,
                                                  DepartmentName =  @departmentName,
                                                  ActiveFlag= @activeFlag,
                                                  LastUpdatedDate = Getdate(),
                                                  LastUpdatedBy = @lastUpdatedBy, 
                                                  -- site_id = @siteId,
                                                  MasterEntityId =  @masterEntityId
                                                  where  DepartmentId = @departmentid
                                         SELECT  * from Department where DepartmentId = @departmentid";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(UpdateDepartmentQuery, BuildSQLParameters(departmentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDepartmentDTO(departmentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating departmentDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(departmentDTO);
            return departmentDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="departmentDTO">DepartmentDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshDepartmentDTO(DepartmentDTO departmentDTO, DataTable dt)
        {
            log.LogMethodEntry(departmentDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                departmentDTO.DepartmentId = Convert.ToInt32(dt.Rows[0]["DepartmentId"]);
                departmentDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                departmentDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                departmentDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                departmentDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                departmentDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                departmentDTO.GUID = dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to DepartmentDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private DepartmentDTO GetDepartmentDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DepartmentDTO departmentDTO = new DepartmentDTO(Convert.ToInt32(dataRow["DepartmentId"]),
                                            dataRow["DepartmentNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DepartmentNumber"]),
                                            dataRow["DepartmentName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DepartmentName"]),
                                            dataRow["ActiveFlag"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ActiveFlag"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(departmentDTO);
            return departmentDTO;
        }

        /// <summary>
        /// Gets the Department data of passed department Id
        /// </summary>
        /// <param name="departmentId">integer type parameter</param>
        /// <returns>Returns DepartmentDTO</returns>
        public DepartmentDTO GetDepartment(int departmentId)
        {
            log.LogMethodEntry();
            DepartmentDTO departmentDataObject = null;
            string selectDepartmentQuery = SELECT_QUERY + "  WHERE d.DepartmentId = @DepartmentId";
            SqlParameter[] selectDepartmentParameters = new SqlParameter[1];
            selectDepartmentParameters[0] = new SqlParameter("@DepartmentId", departmentId);
            DataTable department = dataAccessHandler.executeSelectQuery(selectDepartmentQuery, selectDepartmentParameters, sqlTransaction);
            if (department.Rows.Count > 0)
            {
                DataRow DepartmentRow = department.Rows[0];
                departmentDataObject = GetDepartmentDTO(DepartmentRow);
            }
            log.LogMethodExit(departmentDataObject);
            return departmentDataObject;
        }

        /// <summary>
        /// Gets the DepartmentDTO list matching the UserId
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DepartmentDTO matching the search criteria</returns>
        public List<DepartmentDTO> GetDepartmentDTOList(List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<DepartmentDTO> departmentList = null;
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DepartmentDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(DepartmentDTO.SearchByParameters.DEPARTMENT_ID)
                             || searchParameter.Key.Equals(DepartmentDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(DepartmentDTO.SearchByParameters.DEPARTMENT_NAME)
                                || searchParameter.Key.Equals(DepartmentDTO.SearchByParameters.ISACTIVE))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }

                            else if (searchParameter.Key == DepartmentDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                        }
                        count++;
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

            DataTable departmentData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (departmentData.Rows.Count > 0)
            {
                departmentList = new List<DepartmentDTO>();
                foreach (DataRow departmentDataRow in departmentData.Rows)
                {
                    DepartmentDTO departmentDataObject = GetDepartmentDTO(departmentDataRow);
                    departmentList.Add(departmentDataObject);
                }
            }
            log.LogMethodExit(departmentList);
            return departmentList;
        }
        /// <summary>
        /// Delete a Department record from DB
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public int Delete(int departmentId)
        {
            log.LogMethodEntry(departmentId);
            try
            {
                string deleteQuery = @"delete from department where DepartmentId = @departmentId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@departmentId", departmentId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}
