/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - EnabledAttributesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     20-Aug-2021   Fiona               Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class EnabledAttributesContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<EnabledAttributesDTO> enabledAttributesDTOList;
        private readonly EnabledAttributesContainerDTOCollection enabledAttributesContainerDTOCollection;
        private readonly DateTime? enabledAttributesModuleLastUpdateTime;
        private readonly int siteId;
        private readonly Dictionary<int, EnabledAttributesContainerDTO> enabledAttributesContainerDTODictionary = new Dictionary<int, EnabledAttributesContainerDTO>();
       

        public EnabledAttributesContainer(int siteId) : this(siteId, GetEnabledAttributesDTOList(siteId), GetEnabledAttributesModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static DateTime? GetEnabledAttributesModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                EnabledAttibutesListBL enabledAttributesList = new EnabledAttibutesListBL();
                result = enabledAttributesList.GetEnabledAttributesModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system option max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<EnabledAttributesDTO> GetEnabledAttributesDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<EnabledAttributesDTO> enabledAttributesDTOList = null;
            try
            {
                EnabledAttibutesListBL enabledAttributesList = new EnabledAttibutesListBL();
                List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.IS_ACTIVE, "1"));
                enabledAttributesDTOList = enabledAttributesList.GetEnabledAttibutes(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system options.", ex);
            }

            if (enabledAttributesDTOList == null)
            {
                enabledAttributesDTOList = new List<EnabledAttributesDTO>();
            }
            log.LogMethodExit(enabledAttributesDTOList);
            return enabledAttributesDTOList;
        }

        public EnabledAttributesContainer(int siteId, List<EnabledAttributesDTO> enabledAttributesDTOList, DateTime? enabledAttributesModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.enabledAttributesDTOList = enabledAttributesDTOList;
            this.enabledAttributesModuleLastUpdateTime = enabledAttributesModuleLastUpdateTime;
            List<EnabledAttributesContainerDTO> enabledAttributesContainerDTOList = new List<EnabledAttributesContainerDTO>();
            if(this.enabledAttributesDTOList != null && this.enabledAttributesDTOList.Any())
            {
                foreach (EnabledAttributesDTO enabledAttributessDTO in this.enabledAttributesDTOList)
                {
                    EnabledAttributesContainerDTO enabledAttributesContainerDTO = new EnabledAttributesContainerDTO(enabledAttributessDTO.EnabledAttibuteId, enabledAttributessDTO.TableName, enabledAttributessDTO.RecordGuid, enabledAttributessDTO.EnabledAttributeName, enabledAttributessDTO.MandatoryOrOptional, enabledAttributessDTO.Guid, enabledAttributessDTO.DefaultValue);
                    enabledAttributesContainerDTOList.Add(enabledAttributesContainerDTO);
                    if(enabledAttributesContainerDTODictionary.ContainsKey(enabledAttributesContainerDTO.EnabledAttibuteId)==false)
                    {
                        enabledAttributesContainerDTODictionary.Add(enabledAttributesContainerDTO.EnabledAttibuteId, enabledAttributesContainerDTO);
                    }
                }
            }
            enabledAttributesContainerDTOCollection = new EnabledAttributesContainerDTOCollection(enabledAttributesContainerDTOList);
            log.LogMethodExit();
        }
        public EnabledAttributesContainerDTOCollection GetEnabledAttributesContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(enabledAttributesContainerDTOCollection);
            return enabledAttributesContainerDTOCollection;
        }
        public EnabledAttributesContainerDTO GetEnabledAttributesContainerDTO(int id)
        {
            log.LogMethodEntry(id);
            if (enabledAttributesContainerDTODictionary.ContainsKey(id) == false)
            {
                string errorMessage = "EnabledAttributes with Id : " + id + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = enabledAttributesContainerDTODictionary[id];
            return result;
        }
        public EnabledAttributesContainer Refresh()
        {
            log.LogMethodEntry();
            EnabledAttibutesListBL enabledAttributessList = new EnabledAttibutesListBL();
            DateTime? updateTime = enabledAttributessList.GetEnabledAttributesModuleLastUpdateTime(siteId);
            if (enabledAttributesModuleLastUpdateTime.HasValue
                && enabledAttributesModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in system option since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            EnabledAttributesContainer result = new EnabledAttributesContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
