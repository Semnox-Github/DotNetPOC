/********************************************************************************************
 * Project Name - Machine Attribute Data Handler                                                                          
 * Description  - Data handler of the machine attribute class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created 
 *1.10        11-Jan-2016   Mathew         Updated GetMachineAttribute queries to look at 
 *                                         hierarchy. All attributes will be fetched even if 
 *                                         not defined for a machine
 *2.40        10-Oct-2018   Jagan          Added method for GetEntityIDs checking AttributeId and EntityID existed or not
 *2.50.0      12-dec-2018   Guru S A       Who column changes
 *            19-dec-2018   Jagan          DeleteMachineAttribute for Reset the configuration for particular attribute against EntityId.
 *2.60        05-Mar-2019   Jagan          Update statement changed for the GUID in UpdateMachineAttribute method and created new GetMachineAttributeList()
 *            22-Mar-2019   Divya          Attribute change in InsertMachineAttribute method.
 *2.70.2        19-Jun-2019   Girish Kundar  Modified :  Fix for the SQL Injection Issue 
 *2.70.2        31-Jul-2019   Deeksha        Modified:Added getSqlParameter(),Insert/Update method returns DTO.
 *2.70.3        23-Mar-2020   Archana        Added attributeContext parameter to the RefreshMachineAttributeDTO() method
 *2.130.8     14-Apr-2022     Abhishek      Modified : GetMachineAttributes methods as a part of Promotion Game Attribute Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Machine Attribute data handler - Handles insert, update and select of machine attribute data objects
    /// At a game machine level, there are number of attributes, which could be of 
    /// a. Hardware type - In which case it's transferred to the game reader
    /// b. Software type - For the server logic to use and accordingly send instructions etc
    /// </summary>
    public class MachineAttributeDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<MachineAttributeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MachineAttributeDTO.SearchByParameters, string>
            {
                {MachineAttributeDTO.SearchByParameters.GAME_ID, "gpav.game_id"},
                {MachineAttributeDTO.SearchByParameters.GAME_PROFILE_ID, "gpav.game_profile_id"},
                {MachineAttributeDTO.SearchByParameters.MACHINE_ID, "gpav.machine_id"},
                {MachineAttributeDTO.SearchByParameters.SITE_ID, "gpav.site_id"},
                {MachineAttributeDTO.SearchByParameters.MASTER_ENTITY_ID, "gpav.MasterEntityId"},
                {MachineAttributeDTO.SearchByParameters.IS_ACTIVE, "gpav.ISActive"},
                {MachineAttributeDTO.SearchByParameters.ATTRIBUTE_ID, "gpav.AttributeId"},
                {MachineAttributeDTO.SearchByParameters.PROMOTION_ID, "gpav.PromotionId"},
                {MachineAttributeDTO.SearchByParameters.PROMOTION_DETAIL_ID, "gpav.PromotionDetailId"}
            };

        private SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of MachineAttributeDataHandler class
        /// </summary>
        public MachineAttributeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets attribute id of the passed attribute name
        /// </summary>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="siteId">Site Id</param>
        /// <returns>Attribute Id</returns>
        public int GetAttributeId(string attributeName, int siteId)
        {
            log.LogMethodEntry(attributeName, siteId);
            try
            {
                string selectAttributeQuery = @"select AttributeId attributeId
                                              from GameProfileAttributes
                                             where attribute = @attributeName and (site_id = @siteId or @siteId = -1)";
                List<SqlParameter> getMachineAttributeParameters = new List<SqlParameter>();
                getMachineAttributeParameters.Add(new SqlParameter("@siteId", siteId));
                getMachineAttributeParameters.Add(new SqlParameter("@attributeName", attributeName));
                DataTable attributeData = dataAccessHandler.executeSelectQuery(selectAttributeQuery, getMachineAttributeParameters.ToArray(), sqlTransaction);
                DataRow attributeDataRow = attributeData.Rows[0];
                int attributeId = Convert.ToInt32(attributeDataRow["attributeId"]);
                log.LogMethodExit(attributeId);
                return attributeId;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }

        public MachineAttributeDTO GetMachineAttributeDTO(int id, int siteId)
        {
            log.LogMethodEntry(id, siteId);
            try
            {
                MachineAttributeDTO machineAttributeDTO = null; 
                string selectAttributeQuery = @"select * 
                                              from GameProfileAttributes gpa
                                               join GameProfileAttributeValues gpav on gpa.AttributeId = gpav.AttributeId
                                             where gpa.AttributeId = @id and (gpa.site_id = @siteId or @siteId = -1)";
                List<SqlParameter> getMachineAttributeParameters = new List<SqlParameter>();
                getMachineAttributeParameters.Add(new SqlParameter("@siteId", siteId));
                getMachineAttributeParameters.Add(new SqlParameter("@id", id));
                DataTable attributeData = dataAccessHandler.executeSelectQuery(selectAttributeQuery, getMachineAttributeParameters.ToArray(), sqlTransaction);
                if(attributeData.Rows.Count > 0)
                {
                     machineAttributeDTO = GetMachineAttributeDTO(attributeData.Rows[0]);
                }
                log.LogMethodExit(machineAttributeDTO);
                return machineAttributeDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }
        /// <summary>
        /// Gets the MachineAttributeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        public int GetEntityIDs(List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            try
            {
                int count = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                string joiner;
                string selectMachineAttributeQuery = @"select gpav.Id from GameProfileAttributeValues as gpav";
                StringBuilder query = new StringBuilder(" where ");
                if (searchParameters != null)
                {
                    foreach (KeyValuePair<MachineAttributeDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            joiner = (count == 0) ? string.Empty : "  and ";

                            if (searchParameter.Key == MachineAttributeDTO.SearchByParameters.GAME_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.MACHINE_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.GAME_PROFILE_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.ATTRIBUTE_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.PROMOTION_DETAIL_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.PROMOTION_ID)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == MachineAttributeDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                            log.LogMethodExit(null, "throwing exception");
                            log.LogVariableState("searchParameter.Key", searchParameter.Key);
                            throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        }
                    }
                }

                if ((searchParameters != null) && (searchParameters.Count > 0))
                    selectMachineAttributeQuery = selectMachineAttributeQuery + query;

                DataTable machineAttributeData = dataAccessHandler.executeSelectQuery(selectMachineAttributeQuery, parameters.ToArray(), sqlTransaction);
                return machineAttributeData.Rows.Count;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Game Record.
        /// </summary>
        /// <param name="MachineAttributeDTO">MachineAttributeDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(MachineAttributeDTO machineAttributeDTO, MachineAttributeDTO.AttributeContext attributeContext, int id, string loginId, int siteId)
        {
            log.LogMethodEntry(machineAttributeDTO, attributeContext, loginId, siteId);
            int attributeId = GetAttributeId(machineAttributeDTO.AttributeName.ToString(), siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", machineAttributeDTO.AttributeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attributeId", attributeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attributeValue", machineAttributeDTO.AttributeValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameProfileId", attributeContext == MachineAttributeDTO.AttributeContext.GAME_PROFILE ? id : (object)DBNull.Value, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameId", attributeContext == MachineAttributeDTO.AttributeContext.GAME ? id : (object)DBNull.Value, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineId", attributeContext == MachineAttributeDTO.AttributeContext.MACHINE ? id : (object)DBNull.Value, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@promotionId", attributeContext == MachineAttributeDTO.AttributeContext.PROMOTION ? id : (object)DBNull.Value, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@promotionDetailId", attributeContext == MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL ? id : (object)DBNull.Value, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", machineAttributeDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@guid", machineAttributeDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the machine attributes
        /// </summary>
        /// <param name="attribute">MachineAttributeDTO</param>
        /// <param name="attributeContext">Attribute context - It could be system, game profile, game or machine</param>
        /// <param name="id">Id of the object to which the attribute is to be linked</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MachineAttributeDTO InsertMachineAttribute(MachineAttributeDTO attribute, MachineAttributeDTO.AttributeContext attributeContext, int id, string loginId, int siteId)
        {
            log.LogMethodEntry(attribute, attributeContext, loginId, siteId);
            int attributeId = GetAttributeId(attribute.AttributeName.ToString(), siteId);

            string insertMachineAttributeQuery = @"insert into GameProfileattributevalues 
                                                        (
                                                          game_profile_id,
                                                          game_id,
                                                          machine_id, 
                                                          AttributeId, 
                                                          AttributeValue,
                                                          Guid,
                                                          site_id, 
                                                          LastUpdatedDate, 
                                                          LastUpdatedBy,
                                                          IsActive,
                                                          CreationDate,
                                                          CreatedBy,
                                                          PromotionId,
                                                          PromotionDetailId
                                                        ) 
                                                values 
                                                        (
                                                          @gameProfileId,
                                                          @gameId,
                                                          @machineId,
                                                          @attributeId,
                                                          @attributeValue,
                                                          NEWID(),
                                                          @siteId,
                                                          GETDATE(),
                                                          @userId,
                                                          @isActive,
                                                          GETDATE(),
                                                          @userId,
                                                          @promotionId,
                                                          @promotionDetailId) 
                                                    SELECT * FROM GameProfileattributevalues WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMachineAttributeQuery, GetSQLParameters(attribute, attributeContext, id, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineAttributeDTO(attribute, dt, attributeContext);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting GameProfileattributevalues", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attribute);
            return attribute;
        }


        /// <summary>
        /// Updates machine attribute record
        /// </summary>
        /// <param name="attribute">MachineAttributeDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <returns>Returns the count of updated rows</returns>
        public MachineAttributeDTO UpdateMachineAttribute(MachineAttributeDTO attribute, MachineAttributeDTO.AttributeContext attributeContext, int id, string loginId, int siteId)
        {
            log.LogMethodEntry(attribute, attributeContext, loginId, siteId);

            int attributeId = GetAttributeId(attribute.AttributeName.ToString(), siteId);
            string updateMachineAttributeQuery = @"update GameProfileattributevalues 
                                                      set AttributeValue = @attributeValue,
                                                          AttributeId = @attributeId,
                                                          IsActive = @isActive,
                                                          LastUpdatedDate = GETDATE(),
                                                          LastUpdatedBy = @userId,
                                                          --site_id = @siteId,
                                                          Guid = case when @guid IS NOT NULL then @guid else guid end
                                                    where Id = @id
                            SELECT * FROM GameProfileattributevalues WHERE Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMachineAttributeQuery, GetSQLParameters(attribute, attributeContext, id, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineAttributeDTO(attribute, dt, attributeContext);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting attribute", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attribute);
            return attribute;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="MachineAttributeDTO">MachineAttributeDTO+ object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshMachineAttributeDTO(MachineAttributeDTO attribute, DataTable dt, MachineAttributeDTO.AttributeContext attributeContext)
        {
            log.LogMethodEntry(attribute, dt, attributeContext);//Added attributeContext parameter to update the MachineAttributeDTO.AttributeContext each time during the refresh
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                attribute.AttributeId = Convert.ToInt32(dt.Rows[0]["Id"]);
                attribute.AttributeValue = dataRow["AttributeValue"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["AttributeValue"]);
                attribute.ContextOfAttribute = attributeContext;//Since there will be possibility of updated the values in different places for the different attributeContext like MACHINE, SYSTEM, GAME,GAME_PROFILE
                attribute.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                attribute.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                attribute.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                attribute.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                attribute.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                attribute.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        private MachineAttributeDTO GetMachineAttributeDTO(DataRow machineAttributeDataRow)
        {
            log.LogMethodEntry(machineAttributeDataRow);
            MachineAttributeDTO.AttributeContext lAttributeContext = (MachineAttributeDTO.AttributeContext)Enum.Parse(typeof(MachineAttributeDTO.AttributeContext), machineAttributeDataRow["AttributeContext"].ToString(), true);
            MachineAttributeDTO.MachineAttribute attributeNameText = (MachineAttributeDTO.MachineAttribute)Enum.Parse(typeof(MachineAttributeDTO.MachineAttribute), machineAttributeDataRow["AttributeName"].ToString(), true);
            MachineAttributeDTO machineAttributeDTO = new MachineAttributeDTO(
                                                                machineAttributeDataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(machineAttributeDataRow["Id"]),
                                                                attributeNameText,
                                                                machineAttributeDataRow["AttributeValue"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeDataRow["AttributeValue"]),
                                                                machineAttributeDataRow["IsFlag"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeDataRow["IsFlag"]),
                                                                machineAttributeDataRow["isSoftwareAttribute"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeDataRow["isSoftwareAttribute"]),
                                                                lAttributeContext,
                                                                machineAttributeDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeDataRow["Guid"]),
                                                                machineAttributeDataRow["EnableForPromotion"] == DBNull.Value ? false : Convert.ToBoolean(machineAttributeDataRow["EnableForPromotion"])
                                                                );

            log.LogMethodExit(machineAttributeDTO);
            return machineAttributeDTO;

        }



        /// <summary>
        /// Gets the game machine attribute list
        /// </summary>
        /// <param name="attributeContext">Attribute context - Whether its system, game profile, game or machine</param>
        /// <param name="id">Id of the object looking up the attribute</param>
        /// <returns>Returns the machine attribute dto list set at the level defined by attribute context for the object defined by id</returns>
        public List<MachineAttributeDTO> GetMachineAttributes(MachineAttributeDTO.AttributeContext attributeContext, int id, int siteId = -1)
        {
            log.LogMethodEntry(attributeContext, id);
            try
            {
                MachineAttributeDTO.AttributeContext lAttributeContext; //added local attributeContext variable for query result assignment 11-Jan-2016
                string selectMachineAttributeQuery = @"SELECT ISNULL(gpa.id,defaults.id) id,
                                                          gp.AttributeId AttributeId, isnull(gpa.Guid, defaults.Guid) Guid,                                                          
                                                          isnull(gpa.attributeValue, defaults.attributeValue) attributeValue,
                                                          CASE 
	                                                      WHEN gpa.attributeValue is null and defaults.gattributeValue IS NULL
                                                               and defaults.gpattributeValue IS NULL
	                                                      THEN 'SYSTEM'
	                                                      WHEN gpa.attributeValue is null and defaults.gattributeValue IS NULL and defaults.gpattributeValue is not null
	                                                      THEN 'GAME_PROFILE'
	                                                      WHEN gpa.attributeValue is null and defaults.gattributeValue is not null 
	                                                      THEN 'GAME'
	                                                      WHEN gpa.attributeValue is not null
	                                                      THEN 'MACHINE'
	                                                      END AttributeContext,
	                                                      isnull(gp.IsFlag, 'N') isFlag,
                                                          isnull(gp.SoftwareAttribute, 'N') isSoftwareAttribute,
                                                          gp.Attribute attributeName, gp.EnableForPromotion
                                                     FROM GameProfileAttributes gp,
                                                          (
                                                             SELECT attribute, isnull(g1.attributeValue, isnull(g2.attributeValue, g3.attributeValue)) attributeValue, 
		                                                            isnull(g1.id,isnull(g2.id,g3.id)) id,
                                                                    isnull(g1.Guid, isnull(g2.Guid, g3.Guid)) Guid,
			                                                        a.attributeId,
		                                                            g1.attributeValue gAttributeValue,g2.attributeValue gpAttributeValue,g3.attributeValue sAttributeValue
                                                               FROM GameProfileAttributes a 
                                                                     LEFT OUTER JOIN GameProfileAttributeValues g1
                                                                            ON g1.game_id = (SELECT TOP 1 game_id 
				                                                                               FROM machines 
				                                                                              WHERE ((@MachineId is not null 
                                                                                                      AND machine_id = @MachineId)
								                                                                    OR (@MachineId is null 
                                                                                                        AND game_id = @gameId)
								                                                                    )
								                                                             )
                                                                            AND a.attributeId = g1.attributeId
                                                                     LEFT OUTER JOIN GameProfileAttributeValues g2
                                                                             ON (g2.game_profile_id = (SELECT TOP 1 game_profile_id 
                                                                                                         FROM games g
                                                                                                        WHERE (g.game_id = (SELECT TOP 1 m.game_id FROM MACHINES m WHERE machine_id=@machineId))
										                                                                    OR (@MachineId is null and g.game_id=@gameId)
										                                                                    OR (@MachineId is null and @gameId is null and game_profile_id=@gameProfileId)
										                                                                    )
                                                                                )
                                                                             AND a.attributeId = g2.attributeId
                                                                     LEFT OUTER JOIN GameProfileAttributeValues g3
                                                                                ON a.attributeId = g3.attributeId 
                                                                                AND g3.game_profile_id is null 
                                                                                AND g3.game_id is null 
                                                                                AND g3.machine_id is null
                                                                                AND g3.PromotionId is null
                                                                                AND g3.PromotionDetailId is null
                                                           where (a.site_id = @site_id or @site_id = -1)) defaults
                                                          LEFT OUTER JOIN GameProfileAttributeValues gpa
                                                                          ON defaults.attributeId = gpa.attributeId
                                                                          AND machine_id = @MachineId
                                                    WHERE GP.attributeId = isnull(gpa.attributeId,defaults.attributeId)";
                List<SqlParameter> getMachineAttributeParameters = new List<SqlParameter>();
                getMachineAttributeParameters.Add(new SqlParameter("@site_id", siteId));
                if (attributeContext == MachineAttributeDTO.AttributeContext.GAME_PROFILE)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@gameProfileId", id));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@gameProfileId", DBNull.Value));
                }
                if (attributeContext == MachineAttributeDTO.AttributeContext.GAME)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@gameId", id));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@gameId", DBNull.Value));
                }
                if (attributeContext == MachineAttributeDTO.AttributeContext.MACHINE)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@machineId", id));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@machineId", DBNull.Value));
                }
                if (attributeContext == MachineAttributeDTO.AttributeContext.PROMOTION)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionId", id));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionId", DBNull.Value));
                }
                if (attributeContext == MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionDetailId", id));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionDetailId", DBNull.Value));
                }
                DataTable machineAttributeData = dataAccessHandler.executeSelectQuery(selectMachineAttributeQuery, getMachineAttributeParameters.ToArray(), sqlTransaction);
                if (machineAttributeData != null && machineAttributeData.Rows.Cast<DataRow>().Any())
                {
                    List<MachineAttributeDTO> machineAttributeList = new List<MachineAttributeDTO>();
                    try
                    {
                        machineAttributeList = machineAttributeData.Rows.Cast<DataRow>().Select(x => GetMachineAttributeDTO(x)).ToList();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                        throw;
                    }
                    log.LogMethodExit(machineAttributeList);
                    return machineAttributeList;
                }
                else
                {
                    log.LogMethodExit("Returning null.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the game machine attribute list
        /// </summary>
        /// <param name="attributeContext">Attribute context - Whether its system, game profile, game or machine</param>
        /// <param name="id">Id of the object looking up the attribute</param>
        /// <returns>Returns the machine attribute dto list set at the level defined by attribute context for the object defined by id</returns>
        public List<MachineAttributeDTO> GetMachineAttributes(MachineAttributeDTO.AttributeContext attributeContext, int gameProfileId = -1, int gameId = -1, int machineId = -1, int promotionId = -1, int promotionDetailId = -1, int siteId = -1)
        {
            log.LogMethodEntry(attributeContext, gameProfileId);
            try
            {
                MachineAttributeDTO.AttributeContext lAttributeContext; //added local attributeContext variable for query result assignment 11-Jan-2016
                string selectMachineAttributeQuery = @"select ISNULL(gpavpd.Id,ISNULL(gpavp.Id,ISNULL(gpavm.Id,ISNULL(gpavg.Id,ISNULL(gpavgf.Id,gpav.Id)))))Id,
                                  ISNULL(gpavpd.AttributeId,ISNULL(gpavp.AttributeId,ISNULL(gpavm.AttributeId,ISNULL(gpavg.AttributeId,ISNULL(gpavgf.AttributeId,gpav.AttributeId)))))AttributeId,
                                  ISNULL(gpavpd.Guid,ISNULL(gpavp.Guid,ISNULL(gpavm.Guid,ISNULL(gpavg.Guid,ISNULL(gpavgf.Guid,gpav.Guid)))))Guid,
                                  ISNULL(gpavpd.AttributeValue,ISNULL(gpavp.AttributeValue,ISNULL(gpavm.AttributeValue,ISNULL(gpavg.AttributeValue,ISNULL(gpavgf.AttributeValue,gpav.AttributeValue)))))AttributeValue,
                                  CASE 
                                  WHEN gpavpd.AttributeId IS NOT NULL THEN 'PROMOTION_DETAIL' 
                                  WHEN gpavp.AttributeId IS NOT NULL THEN 'PROMOTION' 
			                      WHEN gpavm.AttributeId IS NOT NULL THEN 'MACHINE'  
			                      WHEN gpavg.AttributeId IS NOT NULL THEN 'GAME'  
			                      WHEN gpavgf.AttributeId IS NOT NULL THEN 'GAME_PROFILE'      
		                          ELSE 'SYSTEM' END AttributeContext, 
			                      ISNULL(gpa.IsFlag, 'N') IsFlag,
                                  ISNULL(gpa.SoftwareAttribute, 'N') isSoftwareAttribute,
                                  gpa.Attribute AttributeName, gpa.EnableForPromotion
                                  from GameProfileAttributes gpa
                                        inner join GameProfileAttributeValues gpav on gpav.AttributeId = gpa.AttributeId and machine_id is NULL 
                                        and game_id is NULL and game_profile_id IS NULL and PromotionId IS NULL and PromotionDetailId IS NULL

                                        left outer join (SELECT gpavp.* 
				                        FROM GameProfileAttributeValues gpavp, promotions p
				                        where  gpavp.PromotionId = p.promotion_id
				                        AND p.promotion_id = @promotionId) AS gpavp ON gpavp.AttributeId = gpa.AttributeId

                                        left outer join (SELECT gpavpd.* 
				                                FROM GameProfileAttributeValues gpavpd, promotion_detail pd
				                                where  gpavpd.PromotionDetailId = pd.promotion_detail_id
				                                AND pd.promotion_detail_id = @promotionDetailId) AS gpavpd ON gpavpd.AttributeId = gpa.AttributeId

                                        left outer join (SELECT gpavm.* FROM GameProfileAttributeValues gpavm, machines m
                                                where  gpavm.machine_id = m.machine_id
				                                AND m.machine_id = @MachineId) AS gpavm ON gpavm.AttributeId = gpa.AttributeId

                                        left outer join (SELECT gpavg.* FROM GameProfileAttributeValues gpavg, games g
                                                where  gpavg.game_id = g.game_id
				                                AND g.game_id = @gameId) AS gpavg ON gpavg.AttributeId = gpa.AttributeId

                                        left outer join (SELECT gpavgf.* FROM GameProfileAttributeValues gpavgf, game_profile gf
                                            where  gpavgf.game_profile_id = gf.game_profile_id
				                            AND gf.game_profile_id = @gameProfileId) AS gpavgf ON gpavgf.AttributeId = gpa.AttributeId
                                        WHERE (gpa.site_id = @site_id or @site_id = -1)";
                List<SqlParameter> getMachineAttributeParameters = new List<SqlParameter>();
                getMachineAttributeParameters.Add(new SqlParameter("@site_id", siteId));
                if (gameProfileId != -1)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@gameProfileId", gameProfileId));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@gameProfileId", DBNull.Value));
                }
                if (gameId != -1)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@gameId", gameId));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@gameId", DBNull.Value));
                }
                if (machineId != -1)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@machineId", machineId));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@machineId", DBNull.Value));
                }
                if (promotionId != -1)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionId", promotionId));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionId", DBNull.Value));
                }
                if (promotionDetailId != -1)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionDetailId", promotionDetailId));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionDetailId", DBNull.Value));
                }
                DataTable machineAttributeData = dataAccessHandler.executeSelectQuery(selectMachineAttributeQuery, getMachineAttributeParameters.ToArray(), sqlTransaction);
                if (machineAttributeData != null && machineAttributeData.Rows.Cast<DataRow>().Any())
                {
                    List<MachineAttributeDTO> machineAttributeList = new List<MachineAttributeDTO>();
                    try
                    {
                        machineAttributeList = machineAttributeData.Rows.Cast<DataRow>().Select(x => GetMachineAttributeDTO(x)).ToList();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                        throw;
                    }
                    log.LogMethodExit(machineAttributeList);
                    return machineAttributeList;
                }
                else
                {
                    log.LogMethodExit("Returning null.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        public List<MachineAttributeDTO> GetMachinePromotionAttributes(int machineId = -1, int promotionDetailId = -1)
        {
            log.LogMethodEntry(machineId, promotionDetailId);
            try
            {
                string selectMachineAttributeQuery = @"select defaults.attribute attribute, 
                                         isnull(gpapd.attributeValue, 
                                                isnull(gpap.attributeValue, 
                                                       isnull(gpa.attributeValue, defaults.attributeValue))) value,
		                                 isnull(defaults.EnableForPromotion, 0) enableForPromotion
                                    from (
                                        select a.attribute, a.EnableForPromotion, isnull(g1.attributeValue, isnull(g2.attributeValue, g3.attributeValue)) attributeValue, a.attributeId
                                        from GameProfileAttributes a 
                                                left outer join GameProfileAttributeValues g1
                                                on g1.game_id = (select game_id from machines where machine_id = @machine_id)
                                                and a.attributeId = g1.attributeId
                                                left outer join GameProfileAttributeValues g2
                                                on (g2.game_profile_id = (select game_profile_id from games g, machines m
                                                                        where m.machine_id = @machine_id and g.game_id = m.game_id))
                                                and a.attributeId = g2.attributeId
                                                left outer join GameProfileAttributeValues g3
                                                on a.attributeId = g3.attributeId 
                                                    and g3.game_profile_id is null 
                                                    and g3.game_id is null 
                                                    and g3.machine_id is null
													AND g3.PromotionId is null
													AND g3.PromotionDetailId is null) defaults
                                left outer join GameProfileAttributeValues gpa
                                    on defaults.attributeId = gpa.attributeId
                                    and machine_id = @machine_id
                                left outer join GameProfileAttributeValues gpap
                                    on defaults.attributeId = gpap.attributeId
                                    and gpap.PromotionId = (select promotion_id from promotion_detail 
                                                             where promotion_detail_id = @promotionDetailId)
                                left outer join GameProfileAttributeValues gpapd
                                    on defaults.attributeId = gpapd.attributeId
                                    and gpapd.PromotionDetailId = @promotionDetailId";
                List<SqlParameter> getMachineAttributeParameters = new List<SqlParameter>();

                if (machineId != -1)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@machine_id", machineId));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@machine_id", DBNull.Value));
                }
                if (promotionDetailId != -1)
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionDetailId", promotionDetailId));
                }
                else
                {
                    getMachineAttributeParameters.Add(new SqlParameter("@promotionDetailId", DBNull.Value));
                }
                DataTable machineAttributeData = dataAccessHandler.executeSelectQuery(selectMachineAttributeQuery, getMachineAttributeParameters.ToArray(), sqlTransaction);

                if (machineAttributeData.Rows.Count > 0)
                {
                    List<MachineAttributeDTO> machineAttributeList = new List<MachineAttributeDTO>();
                    try
                    {
                        foreach (DataRow dataRow in machineAttributeData.Rows)
                        {
                            MachineAttributeDTO machineAttributeDTO = new MachineAttributeDTO();
                            MachineAttributeDTO.MachineAttribute attributeNameText = (MachineAttributeDTO.MachineAttribute)Enum.Parse(typeof(MachineAttributeDTO.MachineAttribute), dataRow["attribute"].ToString(), true);
                            machineAttributeDTO.AttributeName = attributeNameText;
                            machineAttributeDTO.AttributeValue = dataRow["value"] == DBNull.Value ? string.Empty : dataRow["value"].ToString();
                            machineAttributeDTO.EnableForPromotion = dataRow["EnableForPromotion"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["EnableForPromotion"]);
                            machineAttributeList.Add(machineAttributeDTO);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                        throw;
                    }
                    log.LogMethodExit(machineAttributeList);
                    return machineAttributeList;
                }
                else
                {
                    log.LogMethodExit("Returning null.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Based on the attributeId, appropriate machine attribute value record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="attributeId">primary key of profile attribute value id</param>
        /// <param name="entityId">Either GameId/GameProfileId/MachineId</param>
        /// <param name="siteId"></param>
        /// <param name="attributeContext">GAME/GAME_PROFILE/MACHINE</param>
        /// <returns>rowsDeleted</returns>

        public int DeleteMachineAttribute(int attributeId, int entityId, int siteId, MachineAttributeDTO.AttributeContext attributeContext)
        {
            log.LogMethodEntry(attributeId);
            int rowsDeleted = 0;
            try
            {
                string selectAttributeQuery = @"delete from GameProfileAttributeValues where id = @attributeId 
                                                and (game_profile_id = @game_profile_id or game_id = @game_id or machine_id = @machine_id
                                                or PromotionId = @promotionId or PromotionDetailId = @promotionDetailId)
                                                and (site_id = @siteId or @siteId = -1)";

                List<SqlParameter> attributeParameters = new List<SqlParameter>();
                attributeParameters.Add(new SqlParameter("@attributeId", attributeId));
                attributeParameters.Add(new SqlParameter("@siteId", siteId));
                if (attributeContext == MachineAttributeDTO.AttributeContext.GAME_PROFILE)
                {
                    attributeParameters.Add(new SqlParameter("@game_profile_id", entityId));
                }
                else
                {
                    attributeParameters.Add(new SqlParameter("@game_profile_id", DBNull.Value));
                }
                if (attributeContext == MachineAttributeDTO.AttributeContext.GAME)
                {
                    attributeParameters.Add(new SqlParameter("@game_id", entityId));
                }
                else
                {
                    attributeParameters.Add(new SqlParameter("@game_id", DBNull.Value));
                }
                if (attributeContext == MachineAttributeDTO.AttributeContext.MACHINE)
                {
                    attributeParameters.Add(new SqlParameter("@machine_id", entityId));
                }
                else
                {
                    attributeParameters.Add(new SqlParameter("@machine_id", DBNull.Value));
                }
                if (attributeContext == MachineAttributeDTO.AttributeContext.PROMOTION_DETAIL)
                {
                    attributeParameters.Add(new SqlParameter("@promotionDetailId", entityId));
                }
                else
                {
                    attributeParameters.Add(new SqlParameter("@promotionDetailId", DBNull.Value));
                }
                if (attributeContext == MachineAttributeDTO.AttributeContext.PROMOTION)
                {
                    attributeParameters.Add(new SqlParameter("@promotionId", entityId));
                }
                else
                {
                    attributeParameters.Add(new SqlParameter("@promotionId", DBNull.Value));
                }
                rowsDeleted = dataAccessHandler.executeUpdateQuery(selectAttributeQuery, attributeParameters.ToArray(), sqlTransaction);
                log.LogMethodExit(rowsDeleted);
                return rowsDeleted;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get the machine attributes list based on the searchParameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="attributeContext">Attribute context - Accepts the input parameter as system/game profile/game/machine</param>
        /// <returns>Returns the machine attribute dto list</returns>
        public List<MachineAttributeDTO> GetMachineAttributeList(List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchParameters, MachineAttributeDTO.AttributeContext attributeContext)
        {
            log.LogMethodEntry(searchParameters, attributeContext);
            try
            {
                int count = 0;
                string selectMachineAttributeQuery = @"select gpav.AttributeValue,gpav.Id,gpa.Attribute,gpa.IsFlag,gpa.SoftwareAttribute,gpav.Guid,gpa.EnableForPromotion from GameProfileAttributes gpa 
                                                       join GameProfileAttributeValues gpav on gpa.AttributeId = gpav.AttributeId";
                StringBuilder query = new StringBuilder(" where ");
                List<SqlParameter> parameters = new List<SqlParameter>();
                string joiner;
                if (searchParameters != null)
                {
                    foreach (KeyValuePair<MachineAttributeDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            joiner = (count == 0) ? string.Empty : "  and ";

                            if (searchParameter.Key == MachineAttributeDTO.SearchByParameters.GAME_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.MACHINE_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.GAME_PROFILE_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.MASTER_ENTITY_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.PROMOTION_DETAIL_ID ||
                                searchParameter.Key == MachineAttributeDTO.SearchByParameters.PROMOTION_ID)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == MachineAttributeDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == MachineAttributeDTO.SearchByParameters.IS_ACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1) =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" )));
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
                            log.Error("Ends-GetAllFacilityPOSAssignmentList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                            log.LogMethodExit("Throwing exception- The query parameter does not exist ");
                            throw new Exception("The query parameter does not exist");
                        }
                    }
                }

                if ((searchParameters != null) && (searchParameters.Count > 0))
                    selectMachineAttributeQuery = selectMachineAttributeQuery + query;

                DataTable machineAttributeData = dataAccessHandler.executeSelectQuery(selectMachineAttributeQuery, parameters.ToArray(), sqlTransaction);
                if (machineAttributeData.Rows.Count > 0)
                {
                    List<MachineAttributeDTO> machineAttributeList = new List<MachineAttributeDTO>();
                    foreach (DataRow machineAttributeDataRow in machineAttributeData.Rows)
                    {
                        try
                        {
                            MachineAttributeDTO.MachineAttribute attributeNameText = (MachineAttributeDTO.MachineAttribute)Enum.Parse(typeof(MachineAttributeDTO.MachineAttribute), machineAttributeDataRow["attribute"].ToString(), true);
                            machineAttributeList.Add(new MachineAttributeDTO(
                                                                                machineAttributeDataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(machineAttributeDataRow["Id"]),
                                                                                attributeNameText,
                                                                                machineAttributeDataRow["AttributeValue"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeDataRow["AttributeValue"]),
                                                                                machineAttributeDataRow["IsFlag"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeDataRow["IsFlag"]),
                                                                                machineAttributeDataRow["SoftwareAttribute"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeDataRow["SoftwareAttribute"]),
                                                                                attributeContext,
                                                                                machineAttributeDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeDataRow["Guid"]),
                                                                                machineAttributeDataRow["EnableForPromotion"] == DBNull.Value ? true : Convert.ToBoolean(machineAttributeDataRow["EnableForPromotion"])
                                                                                ));
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                        }
                    }
                    log.LogMethodExit(machineAttributeList);
                    return machineAttributeList;
                }
                else
                {
                    log.LogMethodExit("Retuning null.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        public DateTime? GetAttributeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from GameProfileAttributeValues WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from GameProfileAttributes WHERE (site_id = @siteId or @siteId = -1)
                            )a";
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
