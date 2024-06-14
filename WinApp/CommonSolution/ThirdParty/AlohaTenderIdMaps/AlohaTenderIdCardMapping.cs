/********************************************************************************************
 * Project Name - AlohaTenderIDCardMapping
 * Description  - Bussiness logic of AlohaTenderIDCardMapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       15-May-2017     Amaresh          Created 
 *2.70.2       24-Jul-2019     Deeksha          Modifications as per 3 tier standard.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    ///Bussiness logic class for AlohaTenderIDCardMapping operations
    /// </summary>
    public class AlohaTenderIdCardMapping
    {
        private AlohaTenderIdCardMappingDTO alohaTenderIDCardMappingDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of AlohaTenderIDCardMapping class
        /// </summary>
        public AlohaTenderIdCardMapping()
        {
            log.LogMethodEntry();
            alohaTenderIDCardMappingDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the AlohaTenderIDCardMapping DTO based on the id passed 
        /// </summary>
        /// <param name="id">AlohaTenderIDCardMapping id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AlohaTenderIdCardMapping(int id, SqlTransaction sqlTransaction = null )
            : this()
        {
            log.LogMethodEntry(id, sqlTransaction);
            AlohaTenderIdCardMappingDataHandler alohaTenderIDCardMappingDH = new AlohaTenderIdCardMappingDataHandler(sqlTransaction);
            alohaTenderIDCardMappingDTO = alohaTenderIDCardMappingDH.GetAlohaTenderIDCardMapping(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AlohaTenderIDCardMapping object using the AlohaTenderIDCardMappingDTO
        /// </summary>
        /// <param name="alohaTenderIDCardMappingDTO">AlohaTenderIDCardMappingDTO object</param>
        public AlohaTenderIdCardMapping(AlohaTenderIdCardMappingDTO alohaTenderIDCardMappingDTO)
            : this()
        {
            log.LogMethodEntry(alohaTenderIDCardMappingDTO);
            this.alohaTenderIDCardMappingDTO = alohaTenderIDCardMappingDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Getting tender id.
        /// </summary>
        /// <param name="cardType">Card types like VISA, AMEX etc</param>
        /// <param name="posMachineId">Pos machine id</param>
        /// <returns></returns>
        public int GetTenderID(string cardType, int posMachineId)//starts :Modification on 23-May-2017 for Aloha tender id mapping
        {
            log.LogMethodEntry(cardType, posMachineId);
            try
            {
                if (string.IsNullOrEmpty(cardType))
                {
                    log.LogMethodExit(-1);
                    return -1;
                }
                ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
                List<Semnox.Parafait.ThirdParty.AlohaTenderIdCardMappingDTO> alohaTenderIdCardMappingDTOList;
                Semnox.Parafait.ThirdParty.AlohaTenderIDCardMappingsList alohaTenderIDCardMappingsList = new Semnox.Parafait.ThirdParty.AlohaTenderIDCardMappingsList();
                List<KeyValuePair<Semnox.Parafait.ThirdParty.AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string>> searchByAlohaTenderIdCardMappingParameters = new List<KeyValuePair<Semnox.Parafait.ThirdParty.AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string>>();

                List<LookupValuesDTO> lookupValuesDTOList;
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchByLookupValuesParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();

                List<Semnox.Parafait.ThirdParty.AlohaPOSTenderIdMappingDTO> alohaPOSTenderIdMappingDTOList;
                Semnox.Parafait.ThirdParty.AlohaPOSTenderIdMappingsList alohaPOSTenderIdMappingsList = new Semnox.Parafait.ThirdParty.AlohaPOSTenderIdMappingsList();
                List<KeyValuePair<Semnox.Parafait.ThirdParty.AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters, string>> searchByParameters = new List<KeyValuePair<Semnox.Parafait.ThirdParty.AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters, string>>();
                searchByParameters.Add(new KeyValuePair<Semnox.Parafait.ThirdParty.AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters, string>(Semnox.Parafait.ThirdParty.AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                searchByParameters.Add(new KeyValuePair<Semnox.Parafait.ThirdParty.AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters, string>(Semnox.Parafait.ThirdParty.AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.POS_MACHINE_ID, posMachineId.ToString()));
                alohaPOSTenderIdMappingDTOList = alohaPOSTenderIdMappingsList.GetAllAlohaPOSTenderIdMapping(searchByParameters);

                if (alohaPOSTenderIdMappingDTOList != null && alohaPOSTenderIdMappingDTOList.Count > 0)
                {
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "ALOHA_POS_CARD_TYPES"));
                    searchByLookupValuesParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, cardType));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookupValuesParameters);

                    if ((lookupValuesDTOList == null) || (lookupValuesDTOList != null && lookupValuesDTOList.Count == 0))
                    {
                        log.LogMethodExit(-1);
                        return -1;
                    }
                    searchByAlohaTenderIdCardMappingParameters.Add(new KeyValuePair<Semnox.Parafait.ThirdParty.AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string>(Semnox.Parafait.ThirdParty.AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    searchByAlohaTenderIdCardMappingParameters.Add(new KeyValuePair<Semnox.Parafait.ThirdParty.AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string>(Semnox.Parafait.ThirdParty.AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.ALOHA_MAP_ID, alohaPOSTenderIdMappingDTOList[0].AlohaPOSMapId.ToString()));
                    searchByAlohaTenderIdCardMappingParameters.Add(new KeyValuePair<Semnox.Parafait.ThirdParty.AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string>(Semnox.Parafait.ThirdParty.AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.CARD_TYPE_ID, lookupValuesDTOList[0].LookupValueId.ToString()));
                    alohaTenderIdCardMappingDTOList = alohaTenderIDCardMappingsList.GetAllAlohaTenderIDCardMapping(searchByAlohaTenderIdCardMappingParameters);

                    if (alohaTenderIdCardMappingDTOList != null && alohaTenderIdCardMappingDTOList.Count > 0)
                    {
                        int returnValue = alohaTenderIdCardMappingDTOList[0].TenderId;
                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                    else
                    {
                        log.LogMethodExit(-1);
                        return -1;
                    }
                }
                else
                {
                    log.LogMethodExit(-1);
                    return -1;
                }
            }
            catch
            {
                log.LogMethodExit(-1);
                return -1;
            }
        }//Ends :Modification on 23-May-2017 for Aloha tender id mapping

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AlohaTenderIdCardMappingDTO GetAlohaTenderIDCardMappingDTO
        {
            get
            {
                return alohaTenderIDCardMappingDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of AlohaTenderIDCardMappings
    /// </summary>
    public class AlohaTenderIDCardMappingsList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the AlohaTenderIDCardMapping list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>alohaTenderIdCardMappingList</returns>
        public List<AlohaTenderIdCardMappingDTO> GetAllAlohaTenderIDCardMapping(List<KeyValuePair<AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AlohaTenderIdCardMappingDataHandler alohaTenderIDCardMappingDH = new AlohaTenderIdCardMappingDataHandler(sqlTransaction);
            List<AlohaTenderIdCardMappingDTO> alohaTenderIdCardMappingList = new List<AlohaTenderIdCardMappingDTO>();
            alohaTenderIdCardMappingList = alohaTenderIDCardMappingDH.GetAlohaTenderIdCardMappingList(searchParameters);
            log.LogMethodExit(alohaTenderIdCardMappingList);
            return alohaTenderIdCardMappingList;
        }
    }
}
