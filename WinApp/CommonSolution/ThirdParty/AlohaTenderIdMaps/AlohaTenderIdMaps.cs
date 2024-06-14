/********************************************************************************************
 * Project Name - AlohaTenderIdMaps
 * Description  - Bussiness logic of AlohaTenderIdMaps
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       15-May-2017     Amaresh          Created 
 *2.70.2       24-Jul-2019     Deeksha          Modifications as per 3 tier standard.
 ********************************************************************************************/
using Semnox.Parafait.logging;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    ///Bussiness logic class for AlohaTenderIdMaps operations
    /// </summary>
    public class AlohaTenderIdMaps
    {
        private AlohaTenderIdMapsDTO alohaTenderIdMapsDTO;
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of AlohaTenderIdMaps class
        /// </summary>
        public AlohaTenderIdMaps()
        {
            log.LogMethodEntry();
            alohaTenderIdMapsDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the AlohaTenderIdMaps DTO based on the AlohaTenderIdMaps id passed 
        /// </summary>
        /// <param name="alohaMapId">AlohaTenderIdMaps id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AlohaTenderIdMaps(int alohaMapId, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(alohaMapId, sqlTransaction);
            AlohaTenderIdMapsDataHandler alohaTenderIdMapsDataHandler = new AlohaTenderIdMapsDataHandler(sqlTransaction);
            alohaTenderIdMapsDTO = alohaTenderIdMapsDataHandler.GetAlohaTenderIdMaps(alohaMapId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AlohaTenderIdMaps object using the AlohaTenderIdMapsDTO
        /// </summary>
        /// <param name="alohaTenderIdMapsDTO">AlohaTenderIdMapsDTO object</param>
        public AlohaTenderIdMaps(AlohaTenderIdMapsDTO alohaTenderIdMapsDTO)
            : this()
        {
            log.LogMethodEntry(alohaTenderIdMapsDTO);
            this.alohaTenderIdMapsDTO = alohaTenderIdMapsDTO;
            log.LogMethodExit();
        }
       
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AlohaTenderIdMapsDTO GetAlohaTenderIdMapsDTO
        {
            get
            {
                return alohaTenderIdMapsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of AlohaTenderIdMapss
    /// </summary>
    public class AlohaTenderIdMapssList
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the AlohaTenderIdMaps list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>alohaTenderIdMapsList</returns>
        public List<AlohaTenderIdMapsDTO> GetAllAlohaTenderIdMaps(List<KeyValuePair<AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AlohaTenderIdMapsDataHandler alohaTenderIdMapsDataHandler = new AlohaTenderIdMapsDataHandler(sqlTransaction);
            List<AlohaTenderIdMapsDTO> alohaTenderIdMapsList = new List<AlohaTenderIdMapsDTO>();
            alohaTenderIdMapsList= alohaTenderIdMapsDataHandler.GetAlohaTenderIdMapsList(searchParameters);
            log.LogMethodExit(alohaTenderIdMapsList);
            return alohaTenderIdMapsList;
        }
    }
}
