/********************************************************************************************
 * Project Name - DefaultDataType BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-May-2017      Lakshminarayana     Created 
 *2.60        03-May-2019      Mushahid Faizan     Added Save() method and DefaultDataTypeListBL class.
 *2.70.2        26-Jul-2019      Dakshakh raj        Modified : Log method entries/exits
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Default data type business logic
    /// </summary>
    public class DefaultDataTypeBL
    {
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DefaultDataTypeDTO defaultDataTypeDTO;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// Used in the frmLockerManagementUI 
        public DefaultDataTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DefaultDataTypeBL object using the defaultDataTypeDTO
        /// </summary>
        /// <param name="defaultDataTypeDTO">defaultDataTypeDTO object</param>
        public DefaultDataTypeBL(ExecutionContext executionContext, DefaultDataTypeDTO defaultDataTypeDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, defaultDataTypeDTO);
            this.defaultDataTypeDTO = defaultDataTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Default DataType
        /// Checks if the  Datatype_id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            DefaultDataTypeDataHandler defaultDataTypeDataHandler = new DefaultDataTypeDataHandler(sqlTransaction);
            if (defaultDataTypeDTO.DatatypeId < 0)
            {
                defaultDataTypeDTO = defaultDataTypeDataHandler.InsertDefaultDataType(defaultDataTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                defaultDataTypeDTO.AcceptChanges();
            }
            else
            {
                if (defaultDataTypeDTO.IsChanged)
                {
                    defaultDataTypeDTO = defaultDataTypeDataHandler.UpdateDefaultDataType(defaultDataTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    defaultDataTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets Default CustomDataType
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Dictionary of the values</returns>
        public Dictionary<string, string> GetDefaultCustomDataType(string name, SqlTransaction sqlTransaction = null)
        {
            string[] stringArray;
            Dictionary<string, string> defaultType = new Dictionary<string, string>();
            log.LogMethodEntry(name, sqlTransaction);
            executionContext = ExecutionContext.GetExecutionContext();
            DefaultDataTypeDataHandler defaultDataTypeDataHandler = new DefaultDataTypeDataHandler(sqlTransaction);
            DataTable dTable = defaultDataTypeDataHandler.GetDefaultTypes(name, executionContext.GetSiteId());
            defaultType.Add("-1", "<SELECT>");
            if (dTable != null && dTable.Rows.Count > 0)
            {
                stringArray = dTable.Rows[0]["datavalues"].ToString().Split('|');
                for (int i = 0; i < stringArray.Length; i = i + 2)
                {
                    defaultType.Add(stringArray[i], stringArray[i + 1]);
                }
            }
            log.LogMethodExit(defaultType);
            return defaultType;
        }

        /// <summary>
        /// Get Data Type
        /// </summary>
        /// <param name="dataTypeId"></param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public DataTable GetDataType(string dataTypeId, SqlTransaction sqlTransaction = null)
        {
            DefaultDataTypeDataHandler defaultDataTypeDataHandler = new DefaultDataTypeDataHandler(sqlTransaction);
            DataTable dTable = defaultDataTypeDataHandler.GetDefaultTypesByDataType(dataTypeId, executionContext.GetSiteId());
            //dTable.Rows[0]["datatype"].ToString();
            //return dTable.Rows[0]["datatype"].ToString();
            return dTable;
        }

        /// <summary>
        /// Fetch Values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Dictionary of the values</returns>
        public Dictionary<string, string> FetchValues(string dataType, SqlTransaction sqlTransaction = null)
        {
            try
            {
                string[] stringArray;
                Dictionary<string, string> defaultType = new Dictionary<string, string>();
                log.LogMethodEntry(dataType);
                DefaultDataTypeDataHandler defaultDataTypeDataHandler = new DefaultDataTypeDataHandler(sqlTransaction);
                DataTable dTable = defaultDataTypeDataHandler.GetDefaultTypes(dataType, executionContext.GetSiteId());
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    if (dataType.StartsWith("SQL", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(dTable.Rows[0]["datavalues"].ToString()))
                    {
                        DataTable dtSqlValues = defaultDataTypeDataHandler.GetDefaultTypesBySelectCommand(dTable.Rows[0]["datavalues"].ToString(), executionContext.GetSiteId());
                        int count = 0;
                        if (dtSqlValues.Rows.Count != 0)
                        {
                            for (int j = 0; j < dtSqlValues.Rows.Count; j++)
                            {
                                if (dtSqlValues.Rows[j][0].ToString() == "-1" && count == 0)
                                {
                                    defaultType.Remove("-1");
                                    count++;
                                }
                                if (!defaultType.ContainsKey(dtSqlValues.Rows[j][0].ToString()))
                                {
                                    defaultType.Add(dtSqlValues.Rows[j][0].ToString(), dtSqlValues.Rows[j][1].ToString());
                                }
                            }
                        }
                    }
                    else if (dataType.StartsWith("Custom", StringComparison.CurrentCultureIgnoreCase))
                    {
                        stringArray = dTable.Rows[0]["datavalues"].ToString().Split('|');
                        for (int i = 0; i < stringArray.Length; i = i + 2)
                        {
                            defaultType.Add(stringArray[i], stringArray[i + 1]);
                        }
                    }
                }
                log.LogMethodExit(defaultType);
                return defaultType;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
    }
    /// <summary>
    ///  Manages the list of Defaults DataTypes
    /// </summary>
    public class DefaultDataTypeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<DefaultDataTypeDTO> defaultDataTypeDTOList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public DefaultDataTypeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.defaultDataTypeDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="defaultDataTypeDTOList"></param>
        public DefaultDataTypeListBL(ExecutionContext executionContext, List<DefaultDataTypeDTO> defaultDataTypeDTOList)
        {
            log.LogMethodEntry(executionContext, defaultDataTypeDTOList);
            this.executionContext = executionContext;
            this.defaultDataTypeDTOList = defaultDataTypeDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the DefaultDataTypeDTO list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<DefaultDataTypeDTO> GetDefaultDataTypeValues(List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DefaultDataTypeDataHandler defaultDataTypeDataHandler = new DefaultDataTypeDataHandler(sqlTransaction);
            List<DefaultDataTypeDTO> returnValue = defaultDataTypeDataHandler.GetAllDefaultDataTypes(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// This method should be used to Save and Update the Default Datatype List details for Web Management Studio. 
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();

            if (defaultDataTypeDTOList != null && defaultDataTypeDTOList.Any())
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (DefaultDataTypeDTO defaultDataTypeDTO in defaultDataTypeDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DefaultDataTypeBL defaultDataTypeBL = new DefaultDataTypeBL(executionContext, defaultDataTypeDTO);
                            defaultDataTypeBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}

