/********************************************************************************************
 * Project Name - Asset tax Data Handler
 * Description  - Data handler of the asset tax class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Jan-2016   Raghuveera     Created 
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes   
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset Tax Data Handler - Handles insert, update and select of asset tax objects
    /// </summary>
    public class AssetTaxDataHandler
    {
        private static readonly Dictionary<AssetTaxDTO.SearchByAssetTaxParameters, string> DBSearchParameters = new Dictionary<AssetTaxDTO.SearchByAssetTaxParameters, string>
            {
                {AssetTaxDTO.SearchByAssetTaxParameters.TAX_ID, "tax_id"},                
                {AssetTaxDTO.SearchByAssetTaxParameters.ACTIVE_FLAG, "active_flag"}                
            };
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         DataAccessHandler  dataAccessHandler;


        /// <summary>
        /// Default constructor of AssetTaxDataHandler class
        /// </summary>
        public AssetTaxDataHandler()
        {
            log.Debug("Starts-AssetTaxDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler ();
            log.Debug("Ends-AssetTaxDataHandler() default constructor.");
        }

        /// <summary>
        /// Converts the Data row object to AssetTaxDTO class type
        /// </summary>
        /// <param name="assetDataRow">Asset DataRow</param>
        /// <returns>Returns Asset</returns>
        private AssetTaxDTO GetAssetTaxDTO(DataRow assetDataRow)
        {
            log.Debug("Starts-GetAssetTaxDTO(assetDataRow) Method.");
            AssetTaxDTO assetDataObject = new AssetTaxDTO(Convert.ToInt32(assetDataRow["tax_id"]),
                                            assetDataRow["tax_name"].ToString(),
                                            Convert.ToDouble(assetDataRow["tax_percentage"].ToString()),
                                            assetDataRow["active_flag"].ToString() == "Y" ? true : false,
                                            assetDataRow["Guid"].ToString(),
                                            assetDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(assetDataRow["site_id"]),
                                            assetDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(assetDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetAssetTaxDTO(assetDataRow) Method.");
            return assetDataObject;
        }


        /// <summary>
        /// Gets the AssetTaxDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AssetTaxDTO matching the search criteria</returns>
        public List<AssetTaxDTO> GetAssetTaxList(List<KeyValuePair<AssetTaxDTO.SearchByAssetTaxParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAssetTaxList(searchParameters) Method.");
            int count = 0;
            string selectAssetTaxQuery = @"select *
                                         from Tax";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AssetTaxDTO.SearchByAssetTaxParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                            query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like " + "'%" + searchParameter.Value + "%'");
                        else
                            query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like " + "'%" + searchParameter.Value + "%'");
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetAssetTaxList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectAssetTaxQuery = selectAssetTaxQuery + query;
            }
            DataTable assetTaxData = dataAccessHandler.executeSelectQuery(selectAssetTaxQuery, null);
            if (assetTaxData.Rows.Count > 0)
            {
                List<AssetTaxDTO> assetList = new List<AssetTaxDTO>();
                foreach (DataRow assetTaxDataRow in assetTaxData.Rows)
                {
                    AssetTaxDTO assetTaxDataObject = GetAssetTaxDTO(assetTaxDataRow);
                    assetList.Add(assetTaxDataObject);
                }
                log.Debug("Ends-GetAssetTaxList(searchParameters) Method by returning assetList.");
                return assetList;
            }
            else
            {
                log.Debug("Ends-GetAssetTaxList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}
