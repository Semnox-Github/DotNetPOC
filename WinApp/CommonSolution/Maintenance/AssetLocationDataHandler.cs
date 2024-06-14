/********************************************************************************************
 * Project Name - Asset Location Data Handler
 * Description  - Data handler of the asset class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Jan-2016   Raghuveera     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset Location Data Handler selects the  
    /// </summary>
    public class AssetLocationDataHandler
    {
         DataAccessHandler  dataAccessHandler;
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of AssetLocationDataHandler class
        /// </summary>
        public AssetLocationDataHandler()
        {
            log.Debug("Starts-AssetLocationDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler ();
            log.Debug("Ends-AssetLocationDataHandler() default constructor.");
        }
        /// <summary>
        /// Converts the Data row object to AssetLocationDTO class type
        /// </summary>
        /// <param name="assetLocationDataRow">Asset Location DataRow</param>
        /// <returns>Returns Asset</returns>
        private AssetLocationDTO GetAssetLocationDTO(DataRow assetLocationDataRow)
        {
            log.Debug("Starts-GetAssetLocationDTO(assetDataRow) Method.");
            AssetLocationDTO assetLocationDataObject = new AssetLocationDTO(assetLocationDataRow["Location"].ToString());
            log.Debug("Ends-GetAssetLocationDTO(assetDataRow) Method.");
            return assetLocationDataObject;
        }
        /// <summary>
        /// This method selects the location
        /// </summary>
        /// <returns>AssetLocationDTO List</returns>
        public List<AssetLocationDTO> GetLocation()
        {
            log.Debug("Starts-GetLocation() Method.");
            string selectLocationQuery = @"Select Distinct Location
                                         from Maint_Assets where Location is NOT NULL Order by Location";

            DataTable assetLocation = dataAccessHandler.executeSelectQuery(selectLocationQuery, null);
            if (assetLocation.Rows.Count > 0)
            {
                List<AssetLocationDTO> assetLocationList = new List<AssetLocationDTO>();
                foreach (DataRow assetLocationDataRow in assetLocation.Rows)
                {
                    AssetLocationDTO assetLocationDataObject = GetAssetLocationDTO(assetLocationDataRow);
                    assetLocationList.Add(assetLocationDataObject);
                }
                log.Debug("Ends-Ends-GetLocation() Method Method by returning assetLocationList.");
                return assetLocationList;
            }
            else
            {
                log.Debug("Ends-Ends-GetLocation() Method Method by returning null.");
                return null;
            }
        }
    }
}
