/********************************************************************************************
 * Project Name - Asset Location
 * Description  - Bussiness logic of asset location
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Jan-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset location will select the location    
    /// </summary> 
    public class AssetLocation
    {
        private AssetLocationDTO assetLocationDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public AssetLocation()
        {
            log.Debug("Starts-AssetLocation() default constructor");
            assetLocationDTO = null;
            log.Debug("Ends-AssetLocation() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="assetLocationDTO">Parameter of the type AssetLocationDTO</param>
        public AssetLocation(AssetLocationDTO assetLocationDTO)
        {
            log.Debug("Starts-AssetLocation(assetLocationDTO) parameterized constructor.");
            this.assetLocationDTO = assetLocationDTO;
            log.Debug("Ends-AssetLocation(assetLocationDTO) parameterized constructor.");
        }
        /// <summary>
        /// GetLocation method
        /// </summary>
        /// <returns></returns>
        public List<AssetLocationDTO> GetLocation()
        {
            log.Debug("Starts-GetLocation() method.");
            AssetLocationDataHandler assetLocationDataHandler=new AssetLocationDataHandler();
            log.Debug("Ends-GetLocation() method by returning the result assetLocationDataHandler.GetLocation() result.");
            return assetLocationDataHandler.GetLocation();
        }
    }
}
