/********************************************************************************************
 * Project Name - Printer                                                                        
 * Description  -CashdrawerContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    public class CashdrawerContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<CashdrawerDTO> cashdrawerDTOList;
        private readonly CashdrawerContainerDTOCollection cashdrawerContainerDTOCollection;
        private readonly DateTime? cashdrawerLastUpdateTime;
        private readonly ConcurrentDictionary<int, CashdrawerDTO> cashdrawerDTODictionary;
        private readonly ConcurrentDictionary<int, CashdrawerContainerDTO> cashdrawerContainerDTODictionary;
        private readonly int siteId;
        internal CashdrawerContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<CashdrawerContainerDTO> cashdrawerContainerDTOList = new List<CashdrawerContainerDTO>();
            cashdrawerDTODictionary = new ConcurrentDictionary<int, CashdrawerDTO>();
            cashdrawerContainerDTODictionary = new ConcurrentDictionary<int, CashdrawerContainerDTO>();
            try
            {
                CashdrawerListBL cashdrawerListBL = new CashdrawerListBL();
                cashdrawerLastUpdateTime = cashdrawerListBL.GetCashdrawerLastUpdateTime(siteId);
                List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CashdrawerDTO.SearchByParameters, string>(CashdrawerDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                cashdrawerDTOList = cashdrawerListBL.GetCashdrawers(searchParameters);
                if (cashdrawerDTOList == null)
                {
                    cashdrawerDTOList = new List<CashdrawerDTO>();
                }
                if (cashdrawerDTOList.Any())
                {
                    foreach (CashdrawerDTO cashdrawerDTO in cashdrawerDTOList)
                    {
                        cashdrawerDTODictionary[cashdrawerDTO.CashdrawerId] = cashdrawerDTO;
                        CashdrawerContainerDTO cashdrawerContainerDTO = new CashdrawerContainerDTO
                                                                                    (cashdrawerDTO.CashdrawerId,
                                                                                     cashdrawerDTO.CashdrawerName,
                                                                                     cashdrawerDTO.InterfaceType,
                                                                                     cashdrawerDTO.CommunicationString,
                                                                                     cashdrawerDTO.SerialPort,
                                                                                     cashdrawerDTO.SerialPortBaud,
                                                                                     cashdrawerDTO.IsSystem,
                                                                                     cashdrawerDTO.IsActive);
                        cashdrawerContainerDTODictionary[cashdrawerDTO.CashdrawerId] = cashdrawerContainerDTO;
                        cashdrawerContainerDTOList.Add(cashdrawerContainerDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating the Cashdrawer container.", ex);
                cashdrawerDTOList = new List<CashdrawerDTO>();
                cashdrawerDTODictionary.Clear();
                cashdrawerContainerDTOList.Clear();
                cashdrawerContainerDTODictionary.Clear();
            }
            cashdrawerContainerDTOCollection = new CashdrawerContainerDTOCollection(cashdrawerContainerDTOList);
            log.LogMethodExit();
        }

        internal List<CashdrawerContainerDTO> GetCashdrawerContainerDTOList()
        {
            log.LogMethodEntry();
            var result = cashdrawerContainerDTOCollection.CashdrawerContainerDTOList;
            log.LogMethodExit(result);
            return result;
        }


        public CashdrawerContainerDTOCollection GetCashdrawerContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(cashdrawerContainerDTOCollection);
            return cashdrawerContainerDTOCollection;
        }

        /// <summary>
        /// Returns the CashdrawerContainerDTO   for a given cashdrawerId
        /// </summary>
        /// <param name="cashdrawerId"></param>
        /// <returns></returns>
        public CashdrawerContainerDTO GetCashdrawerContainerDTO(int cashdrawerId)
        {
            log.LogMethodEntry(cashdrawerId);
            if (cashdrawerContainerDTODictionary.ContainsKey(cashdrawerId) == false)
            {
                string errorMessage = "cashdrawers with Id :" + cashdrawerId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            CashdrawerContainerDTO result = cashdrawerContainerDTODictionary[cashdrawerId];
            log.LogMethodExit(result);
            return result;
        }

        private CashdrawerDTO GetcashdrawerDTO(int attributeId)
        {
            log.LogMethodEntry(attributeId);
            if (cashdrawerDTODictionary.ContainsKey(attributeId) == false)
            {
                string errorMessage = "cashdrawers with Id :" + attributeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            CashdrawerDTO result = cashdrawerDTODictionary[attributeId];
            log.LogMethodExit(result);
            return result;
        }

        public CashdrawerContainer Refresh()
        {
            log.LogMethodEntry();
            CashdrawerListBL cashdrawerListBL = new CashdrawerListBL();
            DateTime? updateTime = cashdrawerListBL.GetCashdrawerLastUpdateTime(siteId);
            if (cashdrawerLastUpdateTime.HasValue
                && cashdrawerLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in cashdrawers since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            CashdrawerContainer result = new CashdrawerContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
