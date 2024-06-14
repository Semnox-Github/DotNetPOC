/********************************************************************************************
* Project Name - Discount
* Description  - DataHandler - DiscountedGames
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.170.0     05-Jul-2023      Lakshminarayana     Created
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Discounts
{
    class DiscountedGamesDataHandler
    {
        private Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT dg.*
                                              FROM DiscountedGames AS dg";

        private const string INSERT_QUERY = @"INSERT INTO DiscountedGames 
                                                    ( 
                                                        DiscountId,
                                                        GameId,
                                                        Discounted,
                                                        IsActive,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate
                                                    ) 
                                            VALUES 
                                                    (
                                                        @DiscountId,
                                                        @GameId,
                                                        @Discounted,
                                                        @IsActive,
                                                        @SiteId,
                                                        @MasterEntityId,
                                                        @CreatedBy,
                                                        GetDate(),
                                                        @LastUpdatedBy,
                                                        GetDate()
                                                    )  SELECT* from DiscountedGames where Id = scope_identity()";
        private const string UPDATE_QUERY = @"UPDATE DiscountedGames 
                                            SET DiscountId=@DiscountId,
                                                GameId=@GameId,
                                                Discounted=@Discounted,
                                                IsActive=@IsActive,
                                                MasterEntityId=@MasterEntityId,    
                                                LastUpdatedBy=@LastUpdatedBy,
                                                LastUpdateDate = getDate()
                                            WHERE Id = @Id 
                                            SELECT* from DiscountedGames where Id = @Id";
        /// <summary>
        /// Default constructor of DiscountedGamesDataHandler class
        /// </summary>
        public DiscountedGamesDataHandler(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(DiscountedGamesDTO discountedGamesDTO)
        {
            log.LogMethodEntry(discountedGamesDTO);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", discountedGamesDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountId", discountedGamesDTO.DiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameId", discountedGamesDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Discounted", discountedGamesDTO.Discounted));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", (discountedGamesDTO.IsActive == true) ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", executionContext.UserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", executionContext.UserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", executionContext.SiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", discountedGamesDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the DiscountedGames record to the database
        /// </summary>
        /// <param name="discountedGamesDTO">DiscountedGamesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted DiscountedGamesDTO</returns>
        internal DiscountedGamesDTO Save(DiscountedGamesDTO discountedGamesDTO)
        {
            log.LogMethodEntry(discountedGamesDTO);
            string query = discountedGamesDTO.Id <= -1 ? INSERT_QUERY : UPDATE_QUERY;

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(discountedGamesDTO).ToArray(), unitOfWork.SQLTrx);
                DataRow dataRow = dt.Rows[0];
                discountedGamesDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                discountedGamesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                discountedGamesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                discountedGamesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                discountedGamesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                discountedGamesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                discountedGamesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(discountedGamesDTO);
            return discountedGamesDTO;
        }

        /// <summary>
        /// Gets the GetDiscountedGames data
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns DiscountedGamesDTO</returns>
        internal DiscountedGamesDTO GetDiscountedGames(int id)
        {
            log.LogMethodEntry(id);
            DiscountedGamesDTO result = null;
            string query = SELECT_QUERY + @" WHERE dg.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                result = GetDiscountedGamesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts the Data row object to DiscountedGamesDTO class type
        /// </summary>
        /// <param name="dataRow">DiscountedGames DataRow</param>
        /// <returns>Returns DiscountedGamesDTO</returns>
        private DiscountedGamesDTO GetDiscountedGamesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DiscountedGamesDTO discountedGamesDTO = new DiscountedGamesDTO(Convert.ToInt32(dataRow["Id"]),
                                                                            dataRow["DiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DiscountId"]),
                                                                            dataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameId"]),
                                                                            dataRow["Discounted"] == DBNull.Value ? "N" : Convert.ToString(dataRow["Discounted"]),
                                                                            dataRow["IsActive"] == DBNull.Value ? true : dataRow["IsActive"].ToString() == "Y" ? true : false,
                                                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                                            );
            log.LogMethodExit(discountedGamesDTO);
            return discountedGamesDTO;
        }

        /// <summary>
        /// Gets the DiscountedGamesDTO List for Discount Id List
        /// </summary>
        public List<DiscountedGamesDTO> GetDiscountedGamesDTOListOfDiscounts(List<int> discountIdList, bool activeRecords, bool onlyDiscountedChildRecord)
        {
            log.LogMethodEntry(discountIdList, activeRecords, onlyDiscountedChildRecord);
            List<DiscountedGamesDTO> list = new List<DiscountedGamesDTO>();
            string query = SELECT_QUERY + @", @DiscountIdList List WHERE dg.DiscountId = List.Id ";
            if (activeRecords)
            {
                query += " AND dg.IsActive = 'Y' ";
            }
            if (onlyDiscountedChildRecord)
            {
                query += " AND dg.Discounted = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@DiscountIdList", discountIdList, null, unitOfWork.SQLTrx);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetDiscountedGamesDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

    }

}