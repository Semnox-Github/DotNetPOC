/********************************************************************************************
 * Project Name - DSignageLookupValues Data Handler
 * Description  - Data handler of the DSignageLookupValues class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017   Lakshminarayana     Created 
 *2.70.2        30-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                          
 *2.100         07-Aug-2020   Mushahid Faizan     Modified : Added GetDSignageLookupValuesDTOList() and changed default isActive value to true.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    ///  DSignageLookupValues Data Handler - Handles insert, update and select of  DSignageLookupValues objects
    /// </summary>
    public class DSignageLookupValuesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM DSignageLookupValues AS dlv";

        /// <summary>
        /// Dictionary for searching Parameters for the DSignageLookupValues object.
        /// </summary>
        private static readonly Dictionary<DSignageLookupValuesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DSignageLookupValuesDTO.SearchByParameters, string>
            {
                {DSignageLookupValuesDTO.SearchByParameters.DSLOOKUP_VALUE_ID, "dlv.DSLookupValueID"},
                {DSignageLookupValuesDTO.SearchByParameters.DSLOOKUP_ID, "dlv.DSLookupID"},
                {DSignageLookupValuesDTO.SearchByParameters.IS_ACTIVE, "dlv.ValActiveFlag"},
                {DSignageLookupValuesDTO.SearchByParameters.MASTER_ENTITY_ID,"dlv.MasterEntityId"},
                {DSignageLookupValuesDTO.SearchByParameters.SITE_ID, "dlv.site_id"}
            };

        /// <summary>
        ///  Default constructor of DSignageLookupValuesDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DSignageLookupValuesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating dSignageLookupValuesDTO parameters Record.
        /// </summary>
        /// <param name="dSignageLookupValuesDTO">dSignageLookupValuesDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(DSignageLookupValuesDTO dSignageLookupValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dSignageLookupValuesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@DSLookupValueID", dSignageLookupValuesDTO.DSLookupValueID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DSLookupID", dSignageLookupValuesDTO.DSLookupID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValActiveFlag", (dSignageLookupValuesDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValDisplayOrder", dSignageLookupValuesDTO.ValDisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BeforeSpacingRows", dSignageLookupValuesDTO.BeforeSpacingRows));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value1", dSignageLookupValuesDTO.Value1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val1TextColor", dSignageLookupValuesDTO.Val1TextColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val1Font", dSignageLookupValuesDTO.Val1Font));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val1DataType", dSignageLookupValuesDTO.Val1DataType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val1Indentation", dSignageLookupValuesDTO.Val1Indentation, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val1BackColor", dSignageLookupValuesDTO.Val1BackColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val1Description", dSignageLookupValuesDTO.Val1Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value2", dSignageLookupValuesDTO.Value2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val2TextColor", dSignageLookupValuesDTO.Val2TextColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val2Font", dSignageLookupValuesDTO.Val2Font));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val2DataType", dSignageLookupValuesDTO.Val2DataType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val2Indentation", dSignageLookupValuesDTO.Val2Indentation, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val2BackColor", dSignageLookupValuesDTO.Val2BackColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val2Description", dSignageLookupValuesDTO.Val2Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value3", dSignageLookupValuesDTO.Value3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val3TextColor", dSignageLookupValuesDTO.Val3TextColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val3Font", dSignageLookupValuesDTO.Val3Font));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val3DataType", dSignageLookupValuesDTO.Val3DataType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val3Indentation", dSignageLookupValuesDTO.Val3Indentation, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val3BackColor", dSignageLookupValuesDTO.Val3BackColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val3Description", dSignageLookupValuesDTO.Val3Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value4", dSignageLookupValuesDTO.Value4));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val4TextColor", dSignageLookupValuesDTO.Val4TextColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val4Font", dSignageLookupValuesDTO.Val4Font));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val4DataType", dSignageLookupValuesDTO.Val4DataType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val4Indentation", dSignageLookupValuesDTO.Val4Indentation, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val4BackColor", dSignageLookupValuesDTO.Val4BackColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val4Description", dSignageLookupValuesDTO.Val4Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Value5", dSignageLookupValuesDTO.Value5));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val5TextColor", dSignageLookupValuesDTO.Val5TextColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val5Font", dSignageLookupValuesDTO.Val5Font));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val5DataType", dSignageLookupValuesDTO.Val5DataType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val5Indentation", dSignageLookupValuesDTO.Val5Indentation, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val5BackColor", dSignageLookupValuesDTO.Val5BackColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val5Description", dSignageLookupValuesDTO.Val5Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_updated_user", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", dSignageLookupValuesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val1Height", dSignageLookupValuesDTO.Val1Height));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val1Width", dSignageLookupValuesDTO.Val1Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val1ContentLayout", dSignageLookupValuesDTO.Val1ContentLayout, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val2Height", dSignageLookupValuesDTO.Val2Height));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val2Width", dSignageLookupValuesDTO.Val2Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val2ContentLayout", dSignageLookupValuesDTO.Val2ContentLayout, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val3Height", dSignageLookupValuesDTO.Val3Height));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val3Width", dSignageLookupValuesDTO.Val3Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val3ContentLayout", dSignageLookupValuesDTO.Val3ContentLayout, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val4Height", dSignageLookupValuesDTO.Val4Height));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val4Width", dSignageLookupValuesDTO.Val4Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val4ContentLayout", dSignageLookupValuesDTO.Val4ContentLayout, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val5Height", dSignageLookupValuesDTO.Val5Height));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val5Width", dSignageLookupValuesDTO.Val5Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Val5ContentLayout", dSignageLookupValuesDTO.Val5ContentLayout, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the DSignageLookupValues record to the database
        /// </summary>
        /// <param name="dSignageLookupValuesDTO">DSignageLookupValuesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>DSignageLookupValuesDTO</returns>
        public DSignageLookupValuesDTO InsertDSignageLookupValues(DSignageLookupValuesDTO dSignageLookupValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dSignageLookupValuesDTO, loginId, siteId);
            string query = @"INSERT INTO DSignageLookupValues 
                                        ( 
                                            DSLookupID,
                                            ValActiveFlag,
                                            ValDisplayOrder,
                                            BeforeSpacingRows,
                                            site_id,
                                            guid,
                                            last_updated_user,
                                            last_updated_date,
                                            Value1,
                                            Val1TextColor,
                                            Val1Font,
                                            Val1DataType,
                                            Val1Indentation,
                                            Val1BackColor,
                                            Val1Description,
                                            Value2,
                                            Val2TextColor,
                                            Val2Font,
                                            Val2DataType,
                                            Val2Indentation,
                                            Val2BackColor,
                                            Val2Description,
                                            Value3,
                                            Val3TextColor,
                                            Val3Font,
                                            Val3DataType,
                                            Val3Indentation,
                                            Val3BackColor,
                                            Val3Description,
                                            Value4,
                                            Val4TextColor,
                                            Val4Font,
                                            Val4DataType,
                                            Val4Indentation,
                                            Val4BackColor,
                                            Val4Description,
                                            Value5,
                                            Val5TextColor,
                                            Val5Font,
                                            Val5DataType,
                                            Val5Indentation,
                                            Val5BackColor,
                                            Val5Description,
                                            Creationdate,
                                            CreatedUser,
                                            MasterEntityId,
                                            Val1Height,
                                            Val1Width,
                                            Val1ContentLayout,
                                            Val2Height,
                                            Val2Width,
                                            Val2ContentLayout,
                                            Val3Height,
                                            Val3Width,
                                            Val3ContentLayout,
                                            Val4Height,
                                            Val4Width,
                                            Val4ContentLayout,
                                            Val5Height,
                                            Val5Width,
                                            Val5ContentLayout
                                        ) 
                                VALUES 
                                        (
                                            @DSLookupID,
                                            @ValActiveFlag,
                                            @ValDisplayOrder,
                                            @BeforeSpacingRows,
                                            @site_id,
                                            NEWID(),
                                            @last_updated_user,
                                            GETDATE(),
                                            @Value1,
                                            @Val1TextColor,
                                            @Val1Font,
                                            @Val1DataType,
                                            @Val1Indentation,
                                            @Val1BackColor,
                                            @Val1Description,
                                            @Value2,
                                            @Val2TextColor,
                                            @Val2Font,
                                            @Val2DataType,
                                            @Val2Indentation,
                                            @Val2BackColor,
                                            @Val2Description,
                                            @Value3,
                                            @Val3TextColor,
                                            @Val3Font,
                                            @Val3DataType,
                                            @Val3Indentation,
                                            @Val3BackColor,
                                            @Val3Description,
                                            @Value4,
                                            @Val4TextColor,
                                            @Val4Font,
                                            @Val4DataType,
                                            @Val4Indentation,
                                            @Val4BackColor,
                                            @Val4Description,
                                            @Value5,
                                            @Val5TextColor,
                                            @Val5Font,
                                            @Val5DataType,
                                            @Val5Indentation,
                                            @Val5BackColor,
                                            @Val5Description,
                                            GETDATE(),
                                            @CreatedUser,
                                            @MasterEntityId,
                                            @Val1Height,
                                            @Val1Width,
                                            @Val1ContentLayout,
                                            @Val2Height,
                                            @Val2Width,
                                            @Val2ContentLayout,
                                            @Val3Height,
                                            @Val3Width,
                                            @Val3ContentLayout,
                                            @Val4Height,
                                            @Val4Width,
                                            @Val4ContentLayout,
                                            @Val5Height,
                                            @Val5Width,
                                            @Val5ContentLayout

                                        )SELECT * FROM DSignageLookupValues WHERE DSLookupValueID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dSignageLookupValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDSignageLookupValuesDTO(dSignageLookupValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting dSignageLookupValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dSignageLookupValuesDTO);
            return dSignageLookupValuesDTO;
        }

        /// <summary>
        /// Updates the DSignageLookupValues record
        /// </summary>
        /// <param name="dSignageLookupValuesDTO">DSignageLookupValuesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>DSignageLookupValuesDTO</returns>
        public DSignageLookupValuesDTO UpdateDSignageLookupValues(DSignageLookupValuesDTO dSignageLookupValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dSignageLookupValuesDTO, loginId, siteId);
            string query = @"UPDATE DSignageLookupValues 
                             SET DSLookupID=@DSLookupID,
                                 ValActiveFlag=@ValActiveFlag,
                                 ValDisplayOrder=@ValDisplayOrder,
                                 BeforeSpacingRows=@BeforeSpacingRows,
                                 --site_id=@site_id,
                                 last_updated_user = @last_updated_user,
                                 last_updated_date = GETDATE(),
                                 Value1=@Value1,
                                 Val1TextColor=@Val1TextColor,
                                 Val1Font=@Val1Font,
                                 Val1DataType=@Val1DataType,
                                 Val1Indentation=@Val1Indentation,
                                 Val1BackColor=@Val1BackColor,
                                 Val1Description=@Val1Description,
                                 Value2=@Value2,
                                 Val2TextColor=@Val2TextColor,
                                 Val2Font=@Val2Font,
                                 Val2DataType=@Val2DataType,
                                 Val2Indentation=@Val2Indentation,
                                 Val2BackColor=@Val2BackColor,
                                 Val2Description=@Val2Description,
                                 Value3=@Value3,
                                 Val3TextColor=@Val3TextColor,
                                 Val3Font=@Val3Font,
                                 Val3DataType=@Val3DataType,
                                 Val3Indentation=@Val3Indentation,
                                 Val3BackColor=@Val3BackColor,
                                 Val3Description=@Val3Description,
                                 Value4=@Value4,
                                 Val4TextColor=@Val4TextColor,
                                 Val4Font=@Val4Font,
                                 Val4DataType=@Val4DataType,
                                 Val4Indentation=@Val4Indentation,
                                 Val4BackColor=@Val4BackColor,
                                 Val4Description=@Val4Description,
                                 Value5=@Value5,
                                 Val5TextColor=@Val5TextColor,
                                 Val5Font=@Val5Font,
                                 Val5DataType=@Val5DataType,
                                 Val5Indentation=@Val5Indentation,
                                 Val5BackColor=@Val5BackColor,
                                 Val5Description=@Val5Description,
                                 Val1Height=@Val1Height,
                                 Val1Width=@Val1Width,
                                 Val1ContentLayout=@Val1ContentLayout,
                                 Val2Height=@Val2Height,
                                 Val2Width=@Val2Width,
                                 Val2ContentLayout=@Val2ContentLayout,
                                 Val3Height=@Val3Height,
                                 Val3Width=@Val3Width,
                                 Val3ContentLayout=@Val3ContentLayout,
                                 Val4Height=@Val4Height,
                                 Val4Width=@Val4Width,
                                 Val4ContentLayout=@Val4ContentLayout,
                                 Val5Height=@Val5Height,
                                 Val5Width=@Val5Width,
                                 Val5ContentLayout=@Val5ContentLayout
                             WHERE DSLookupValueID = @DSLookupValueID
                             SELECT* FROM DSignageLookupValues WHERE  DSLookupValueID = @DSLookupValueID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dSignageLookupValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDSignageLookupValuesDTO(dSignageLookupValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating dSignageLookupValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dSignageLookupValuesDTO);
            return dSignageLookupValuesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="dSignageLookupValuesDTO">dSignageLookupValuesDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshDSignageLookupValuesDTO(DSignageLookupValuesDTO dSignageLookupValuesDTO, DataTable dt)
        {
            log.LogMethodEntry(dSignageLookupValuesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                dSignageLookupValuesDTO.DSLookupValueID = Convert.ToInt32(dt.Rows[0]["DSLookupValueID"]);
                dSignageLookupValuesDTO.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                dSignageLookupValuesDTO.CreationDate = dataRow["Creationdate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Creationdate"]);
                dSignageLookupValuesDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                dSignageLookupValuesDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                dSignageLookupValuesDTO.CreatedBy = dataRow["CreatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedUser"]);
                dSignageLookupValuesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to DSignageLookupValuesDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DSignageLookupValuesDTO</returns>
        private DSignageLookupValuesDTO GetDSignageLookupValuesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DSignageLookupValuesDTO dSignageLookupValuesDTO = new DSignageLookupValuesDTO(Convert.ToInt32(dataRow["DSLookupValueID"]),
                                            dataRow["DSLookupID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DSLookupID"]),
                                            dataRow["ValDisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ValDisplayOrder"]),
                                            dataRow["BeforeSpacingRows"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["BeforeSpacingRows"]),
                                            dataRow["Value1"] == DBNull.Value ? string.Empty : dataRow["Value1"].ToString(),
                                            dataRow["Val1TextColor"] == DBNull.Value ? string.Empty : dataRow["Val1TextColor"].ToString(),
                                            dataRow["Val1Font"] == DBNull.Value ? string.Empty : dataRow["Val1Font"].ToString(),
                                            dataRow["Val1DataType"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val1DataType"]),
                                            dataRow["Val1Indentation"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val1Indentation"]),
                                            dataRow["Val1BackColor"] == DBNull.Value ? string.Empty : dataRow["Val1BackColor"].ToString(),
                                            dataRow["Val1Description"] == DBNull.Value ? string.Empty : dataRow["Val1Description"].ToString(),
                                            dataRow["Val1Height"] == DBNull.Value ? string.Empty : dataRow["Val1Height"].ToString(),
                                            dataRow["Val1Width"] == DBNull.Value ? string.Empty : dataRow["Val1Width"].ToString(),
                                            dataRow["Val1ContentLayout"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val1ContentLayout"]),
                                            dataRow["Value2"] == DBNull.Value ? string.Empty : dataRow["Value2"].ToString(),
                                            dataRow["Val2TextColor"] == DBNull.Value ? string.Empty : dataRow["Val2TextColor"].ToString(),
                                            dataRow["Val2Font"] == DBNull.Value ? string.Empty : dataRow["Val2Font"].ToString(),
                                            dataRow["Val2DataType"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val2DataType"]),
                                            dataRow["Val2Indentation"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val2Indentation"]),
                                            dataRow["Val2BackColor"] == DBNull.Value ? string.Empty : dataRow["Val2BackColor"].ToString(),
                                            dataRow["Val2Description"] == DBNull.Value ? string.Empty : dataRow["Val2Description"].ToString(),
                                            dataRow["Val2Height"] == DBNull.Value ? string.Empty : dataRow["Val2Height"].ToString(),
                                            dataRow["Val2Width"] == DBNull.Value ? string.Empty : dataRow["Val2Width"].ToString(),
                                            dataRow["Val2ContentLayout"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val2ContentLayout"]),
                                            dataRow["Value3"] == DBNull.Value ? string.Empty : dataRow["Value3"].ToString(),
                                            dataRow["Val3TextColor"] == DBNull.Value ? string.Empty : dataRow["Val3TextColor"].ToString(),
                                            dataRow["Val3Font"] == DBNull.Value ? string.Empty : dataRow["Val3Font"].ToString(),
                                            dataRow["Val3DataType"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val3DataType"]),
                                            dataRow["Val3Indentation"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val3Indentation"]),
                                            dataRow["Val3BackColor"] == DBNull.Value ? string.Empty : dataRow["Val3BackColor"].ToString(),
                                            dataRow["Val3Description"] == DBNull.Value ? string.Empty : dataRow["Val3Description"].ToString(),
                                            dataRow["Val3Height"] == DBNull.Value ? string.Empty : dataRow["Val3Height"].ToString(),
                                            dataRow["Val3Width"] == DBNull.Value ? string.Empty : dataRow["Val3Width"].ToString(),
                                            dataRow["Val3ContentLayout"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val3ContentLayout"]),
                                            dataRow["Value4"] == DBNull.Value ? string.Empty : dataRow["Value4"].ToString(),
                                            dataRow["Val4TextColor"] == DBNull.Value ? string.Empty : dataRow["Val4TextColor"].ToString(),
                                            dataRow["Val4Font"] == DBNull.Value ? string.Empty : dataRow["Val4Font"].ToString(),
                                            dataRow["Val4DataType"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val4DataType"]),
                                            dataRow["Val4Indentation"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val4Indentation"]),
                                            dataRow["Val4BackColor"] == DBNull.Value ? string.Empty : dataRow["Val4BackColor"].ToString(),
                                            dataRow["Val4Description"] == DBNull.Value ? string.Empty : dataRow["Val4Description"].ToString(),
                                            dataRow["Val4Height"] == DBNull.Value ? string.Empty : dataRow["Val4Height"].ToString(),
                                            dataRow["Val4Width"] == DBNull.Value ? string.Empty : dataRow["Val4Width"].ToString(),
                                            dataRow["Val4ContentLayout"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val4ContentLayout"]),
                                            dataRow["Value5"] == DBNull.Value ? string.Empty : dataRow["Value5"].ToString(),
                                            dataRow["Val5TextColor"] == DBNull.Value ? string.Empty : dataRow["Val5TextColor"].ToString(),
                                            dataRow["Val5Font"] == DBNull.Value ? string.Empty : dataRow["Val5Font"].ToString(),
                                            dataRow["Val5DataType"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val5DataType"]),
                                            dataRow["Val5Indentation"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val5Indentation"]),
                                            dataRow["Val5BackColor"] == DBNull.Value ? string.Empty : dataRow["Val5BackColor"].ToString(),
                                            dataRow["Val5Description"] == DBNull.Value ? string.Empty : dataRow["Val5Description"].ToString(),
                                            dataRow["Val5Height"] == DBNull.Value ? string.Empty : dataRow["Val5Height"].ToString(),
                                            dataRow["Val5Width"] == DBNull.Value ? string.Empty : dataRow["Val5Width"].ToString(),
                                            dataRow["Val5ContentLayout"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Val5ContentLayout"]),
                                            dataRow["ValActiveFlag"] == DBNull.Value ? true : (dataRow["ValActiveFlag"].ToString() == "Y" ? true : false),
                                            dataRow["CreatedUser"] == DBNull.Value ? string.Empty : dataRow["CreatedUser"].ToString(),
                                            dataRow["Creationdate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Creationdate"]),
                                            dataRow["last_updated_user"] == DBNull.Value ? string.Empty : dataRow["last_updated_user"].ToString(),
                                            dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["guid"] == DBNull.Value ? string.Empty : dataRow["guid"].ToString()

                                            );
            log.LogMethodExit(dSignageLookupValuesDTO);
            return dSignageLookupValuesDTO;
        }

        /// <summary>
        /// Gets the DSignageLookupValues data of passed DSLookupValueID
        /// </summary>
        /// <param name="dSLookupValueID">integer type parameter</param>
        /// <returns>Returns DSignageLookupValuesDTO</returns>
        public DSignageLookupValuesDTO GetDSignageLookupValuesDTO(int dSLookupValueID)
        {
            log.LogMethodEntry(dSLookupValueID);
            DSignageLookupValuesDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE dlv.DSLookupValueID = @DSLookupValueID";
            SqlParameter parameter = new SqlParameter("@DSLookupValueID", dSLookupValueID);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetDSignageLookupValuesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the DSignageLookupValues List for DSLookup Id List
        /// </summary>
        /// <param name="dsLookupIdList">integer list parameter</param>
        /// <returns>Returns List of DSignageLookupValuesDTOList</returns>
        public List<DSignageLookupValuesDTO> GetDSignageLookupValuesDTOList(List<int> dsLookupIdList, bool activeRecords)
        {
            log.LogMethodEntry(dsLookupIdList);
            List<DSignageLookupValuesDTO> list = new List<DSignageLookupValuesDTO>();
            string query = @"SELECT DSignageLookupValues.*
                            FROM DSignageLookupValues, @dsLookupIdList List
                            WHERE DSLookupID = List.Id ";
            if (activeRecords)
            {
                query += " AND ValActiveFlag = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@dsLookupIdList", dsLookupIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetDSignageLookupValuesDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DSignageLookupValuesDTO matching the search criteria</returns>
        public List<DSignageLookupValuesDTO> GetDSignageLookupValuesDTOList(List<KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DSignageLookupValuesDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == DSignageLookupValuesDTO.SearchByParameters.DSLOOKUP_VALUE_ID ||
                            searchParameter.Key == DSignageLookupValuesDTO.SearchByParameters.DSLOOKUP_ID ||
                            searchParameter.Key == DSignageLookupValuesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DSignageLookupValuesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DSignageLookupValuesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                list = new List<DSignageLookupValuesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DSignageLookupValuesDTO dSignageLookupValuesDTO = GetDSignageLookupValuesDTO(dataRow);
                    list.Add(dSignageLookupValuesDTO);
                }

            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
