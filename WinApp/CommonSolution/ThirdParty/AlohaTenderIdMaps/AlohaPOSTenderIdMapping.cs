/********************************************************************************************
 * Project Name - AlohaPOSTenderIdMapping
 * Description  - Bussiness logic of AlohaPOSTenderIdMapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
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
    ///Bussiness logic class for AlohaPOSTenderIdMapping operations
    /// </summary>
    public class AlohaPOSTenderIdMapping
    {
        private AlohaPOSTenderIdMappingDTO alohaPOSTenderIdMappingDTO;
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of AlohaPOSTenderIdMapping class
        /// </summary>
        public AlohaPOSTenderIdMapping()
        {
            log.LogMethodEntry();
            alohaPOSTenderIdMappingDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the AlohaPOSTenderIdMapping DTO based on the id passed 
        /// </summary>
        /// <param name="alohaPOSMapId">AlohaPOSTenderIdMapping alohaPOSMapId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AlohaPOSTenderIdMapping(int alohaPOSMapId, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(alohaPOSMapId, sqlTransaction);
            AlohaPOSTenderIdMappingDataHandler alohaPOSTenderIdMappingDH = new AlohaPOSTenderIdMappingDataHandler(sqlTransaction);
            alohaPOSTenderIdMappingDTO = alohaPOSTenderIdMappingDH.GetAlohaPOSTenderIdMapping(alohaPOSMapId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AlohaPOSTenderIdMapping object using the AlohaPOSTenderIdMappingDTO
        /// </summary>
        /// <param name="alohaPOSTenderIdMappingDTO">AlohaPOSTenderIdMappingDTO object</param>
        public AlohaPOSTenderIdMapping(AlohaPOSTenderIdMappingDTO alohaPOSTenderIdMappingDTO)
            : this()
        {
            log.LogMethodEntry(alohaPOSTenderIdMappingDTO);
            this.alohaPOSTenderIdMappingDTO = alohaPOSTenderIdMappingDTO;
            log.LogMethodExit();
        } 

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AlohaPOSTenderIdMappingDTO GetAlohaPOSTenderIdMappingDTO
        {
            get
            {
                return alohaPOSTenderIdMappingDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of AlohaPOSTenderIdMappings
    /// </summary>
    public class AlohaPOSTenderIdMappingsList
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the AlohaPOSTenderIdMapping list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>alohaPOSTenderIdMappingDTOs</returns>
        public List<AlohaPOSTenderIdMappingDTO> GetAllAlohaPOSTenderIdMapping(List<KeyValuePair<AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AlohaPOSTenderIdMappingDataHandler alohaPOSTenderIdMappingDH = new AlohaPOSTenderIdMappingDataHandler(sqlTransaction);
            List<AlohaPOSTenderIdMappingDTO> alohaPOSTenderIdMappingDTOs = new List<AlohaPOSTenderIdMappingDTO>();
            alohaPOSTenderIdMappingDTOs= alohaPOSTenderIdMappingDH.GetAlohaPOSTenderIdMappingList(searchParameters);
            log.LogMethodExit(alohaPOSTenderIdMappingDTOs);
            return alohaPOSTenderIdMappingDTOs;
        }
    }
}
