/********************************************************************************************
 * Project Name - SpecialPricingOptions DataHandler 
 * Description  - DataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date                Modified By            Remarks          
 ******************************************************************************************
 *2.50        13-Feb-2019        Indrajeet Kumar         Created 
 *2.60        22-Mar-2019        Nagesh Badiger          Added GetSQLParameters() and log method entry and method exit
 *2.70        29-Jun-2019        Akshay Gulaganji        Added DeleteSpecialPricing() method
 *2.70.2        10-Dec-2019        Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Data;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// SpecialPricingOptions DataHandler
    /// </summary>
    public class SpecialPricingOptionsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;

        private static readonly Dictionary<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string> DBSearchParameters = new Dictionary<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>
        {
             {SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.PRICING_ID, "PricingId"},
             {SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.SITE_ID, "site_id"},
             {SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.ACTIVE_FLAG, "ActiveFlag"},
             {SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.MASTERENTITY_ID, "MasterEntityId"},
             {SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.PERCENTAGE, "Percentage"}
        };

        /// <summary>
        /// Default constructor of SpecialPricingOptionsDataHandler class
        /// </summary>
        public SpecialPricingOptionsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to SpecialPricingOptionsDTO class type
        /// </summary>
        /// <param name="priceListDataRow">PriceList DataRow</param>
        /// <returns>Returns PriceList</returns>
        private SpecialPricingOptionsDTO GetSpecialPricingOptionsDTO(DataRow specialPricingOptionsDataRow)
        {
            log.LogMethodEntry(specialPricingOptionsDataRow);
            try
            {
                SpecialPricingOptionsDTO SpecialPricingOptionsDataObject = new SpecialPricingOptionsDTO(
                                                        Convert.ToInt32(specialPricingOptionsDataRow["PricingId"]),
                                                        specialPricingOptionsDataRow["PricingName"].ToString(),
                                                        specialPricingOptionsDataRow["Percentage"] == DBNull.Value ? -1 : Convert.ToDecimal(specialPricingOptionsDataRow["Percentage"]),
                                                        specialPricingOptionsDataRow["ActiveFlag"] == DBNull.Value ? true : (specialPricingOptionsDataRow["ActiveFlag"].ToString() == "Y" ? true : false),
                                                        specialPricingOptionsDataRow["RequiresManagerApproval"].ToString(),
                                                        specialPricingOptionsDataRow["Guid"].ToString(),
                                                        specialPricingOptionsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(specialPricingOptionsDataRow["site_id"]),
                                                        specialPricingOptionsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(specialPricingOptionsDataRow["SynchStatus"]),
                                                        specialPricingOptionsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(specialPricingOptionsDataRow["MasterEntityId"]),
                                                        specialPricingOptionsDataRow["CreatedBy"].ToString(),
                                                        specialPricingOptionsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(specialPricingOptionsDataRow["CreationDate"]),
                                                        specialPricingOptionsDataRow["LastUpdatedBy"].ToString(),
                                                        specialPricingOptionsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(specialPricingOptionsDataRow["LastUpdateDate"])
                                                       );
                log.LogMethodExit(SpecialPricingOptionsDataObject);
                return SpecialPricingOptionsDataObject;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while executing Converting the Data row object to SpecialPricingOptionsDTO class type", ex);
                log.LogMethodExit(null);
                throw;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating product special pricing Record.
        /// </summary>
        /// <param name="specialPricingOptionsDTO">LookupsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(SpecialPricingOptionsDTO specialPricingOptionsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(specialPricingOptionsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@pricingId", specialPricingOptionsDTO.PricingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pricingName", specialPricingOptionsDTO.PricingName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@percentage", specialPricingOptionsDTO.Percentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", specialPricingOptionsDTO.ActiveFlag ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requiresManagerApproval", specialPricingOptionsDTO.RequiresManagerApproval));            
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the SpecialPricingOptions record to the database
        /// </summary>
        /// <param name="specialPricingOptionsDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int InsertSpecialPricingOptions(SpecialPricingOptionsDTO specialPricingOptionsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(specialPricingOptionsDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO SpecialPricing 
					                                                (	
						                                                PricingName, 
						                                                Percentage, 
						                                                ActiveFlag, 
						                                                RequiresManagerApproval, 
						                                                Guid, 
                                                                        site_id,
                                                                        CreatedBy,
                                                                        CreationDate,
                                                                        LastUpdatedBy,
                                                                        LastUpdateDate
					                                                ) 
					                                                VALUES	
					                                                (
						                                                @pricingName, 
						                                                @percentage, 
						                                                @activeFlag, 
						                                                @requiresManagerApproval, 
						                                                NEWID(), 
						                                                @site_id,
                                                                        @createdBy,
                                                                        GetDate(),
                                                                        @lastUpdatedBy,
                                                                        GetDate()
					                                                )SELECT CAST(scope_identity() AS int)";



            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(specialPricingOptionsDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;

        }

        /// <summary>
        /// Updates the SpecialPricingOptions record to the database
        /// </summary>
        /// <param name="specialPricingOptionsDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int UpdateSpecialPricingOptions(SpecialPricingOptionsDTO specialPricingOptionsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(specialPricingOptionsDTO, userId, siteId);
            int idOfRowUpdated = 0;
            string query = @"UPDATE SpecialPricing 
                                                                    SET 
                                                                        PricingName     = @pricingName, 
                                                                        Percentage      = @percentage, 
                                                                        ActiveFlag      = @activeFlag, 
                                                                        RequiresManagerApproval = @requiresManagerApproval, 
                                                                        -- site_id         = @site_id, 
                                                                        LastUpdatedBy   = @lastUpdatedBy,
                                                                        LastUpdateDate  = GetDate()
                                                                    WHERE 
                                                                        PricingId = @pricingId";


            try
            {
                idOfRowUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(specialPricingOptionsDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(idOfRowUpdated);
            return idOfRowUpdated;
        }

        /// <summary>
        /// Gets the SpecialPricingOptionsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>Returns the list of SpecialPricingOptionsDTO matching the search criteria</returns>
        public List<SpecialPricingOptionsDTO> GetAllSpecialPricingOptionsList(List<KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectSpecialPricingOptionsQuery = @"select * from  SpecialPricing";

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.PRICING_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N") + "'");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetAllSpecialPricingOptionsList(searchParameters) method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectSpecialPricingOptionsQuery = selectSpecialPricingOptionsQuery + query;
                selectSpecialPricingOptionsQuery = selectSpecialPricingOptionsQuery + "Order by PricingId";
            }
            DataTable SpecialPricingOptionsData = dataAccessHandler.executeSelectQuery(selectSpecialPricingOptionsQuery, null);
            if (SpecialPricingOptionsData.Rows.Count > 0)
            {
                List<SpecialPricingOptionsDTO> specialPricingOptionsList = new List<SpecialPricingOptionsDTO>();
                foreach (DataRow specialPricingOptionsDataRow in SpecialPricingOptionsData.Rows)
                {
                    SpecialPricingOptionsDTO SpecialPricingOptionsObject = GetSpecialPricingOptionsDTO(specialPricingOptionsDataRow);
                    specialPricingOptionsList.Add(SpecialPricingOptionsObject);
                }
                log.LogMethodExit(specialPricingOptionsList);
                return specialPricingOptionsList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        /// <summary>
        /// Deletes the ProductSpecialPricing based on the pricingId
        /// </summary>
        /// <param name="pricingId">pricingId</param>
        /// <returns>return the int</returns>
        public int DeleteSpecialPricing(int pricingId)
        {
            log.LogMethodEntry(pricingId);
            try
            {
                string deleteQuery = @"delete from SpecialPricing where PricingId = @pricingId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@pricingId", pricingId);

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
