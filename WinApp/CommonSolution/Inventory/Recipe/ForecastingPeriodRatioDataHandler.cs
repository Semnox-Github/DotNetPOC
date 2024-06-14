/********************************************************************************************
* Project Name - Forecasting Period Ratio BL 
* Description  - DH to maintain Forecasting data based on the  historical Days value.
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*2.100.0    26-Sep-20      Deeksha              Created 
********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class ForecastingPeriodRatioDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM InventoryForecastingProportion AS invProp ";

        private static readonly Dictionary<ForecastingPeriodRatioDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ForecastingPeriodRatioDTO.SearchByParameters, string>
            {
                {ForecastingPeriodRatioDTO.SearchByParameters.PERIOD_DATA_POINTS, "invProp.PeriodDataPoints"},
                {ForecastingPeriodRatioDTO.SearchByParameters.IS_ACTIVE, "invProp.IsActive"},
                {ForecastingPeriodRatioDTO.SearchByParameters.MASTER_ENTITY_ID, "invProp.MasterEntityId"},
                {ForecastingPeriodRatioDTO.SearchByParameters.SITE_ID, "invProp.site_id"}
            };

        /// <summary>
        /// Parameterized Constructor for ForecastingPeriodRatioDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public ForecastingPeriodRatioDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        private ForecastingPeriodRatioDTO GetForecastingPeriodRatioDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ForecastingPeriodRatioDTO forecastingPeriodRatioDTO = new ForecastingPeriodRatioDTO(dataRow["PeriodDataPointsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PeriodDataPointsId"]),
                                                dataRow["PeriodDataPoints"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PeriodDataPoints"]),
                                                dataRow["Period30"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period30"]),
                                                dataRow["Period60"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period60"]),
                                                dataRow["Period90"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period90"]),
                                                dataRow["Period120"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period120"]),
                                                dataRow["Period150"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period150"]),
                                                dataRow["Period180"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period180"]),
                                                dataRow["Period210"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period210"]),
                                                dataRow["Period240"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period240"]),
                                                dataRow["Period270"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period270"]),
                                                dataRow["Period300"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period300"]),
                                                dataRow["Period330"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period330"]),
                                                dataRow["Period365"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Period365"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                 dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));
            log.LogMethodExit(forecastingPeriodRatioDTO);
            return forecastingPeriodRatioDTO;
        }

        internal List<ForecastingPeriodRatioDTO> GetForecastingPeriodRatioDTOList(List<KeyValuePair<ForecastingPeriodRatioDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ForecastingPeriodRatioDTO> forecastingPeriodRatioDTOList = new List<ForecastingPeriodRatioDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ForecastingPeriodRatioDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ForecastingPeriodRatioDTO.SearchByParameters.PERIOD_DATA_POINTS )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ForecastingPeriodRatioDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ForecastingPeriodRatioDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                forecastingPeriodRatioDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetForecastingPeriodRatioDTO(x)).ToList();
            }
            log.LogMethodExit(forecastingPeriodRatioDTOList);
            return forecastingPeriodRatioDTOList;
        }
    }
}
