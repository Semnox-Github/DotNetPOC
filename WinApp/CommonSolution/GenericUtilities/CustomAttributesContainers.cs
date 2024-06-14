/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - CustomAttributesContainers class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      27-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    public class CustomAttributesContainers : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<CustomAttributesDTO> customAttributesDTOList;
        private readonly DateTime? customAttributesLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, CustomAttributesDTO> customAttributesDTODictionary;

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        internal CustomAttributesContainers(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            customAttributesDTODictionary = new ConcurrentDictionary<int, CustomAttributesDTO>();
            customAttributesDTOList = new List<CustomAttributesDTO>();
            CustomAttributesListBL customAttributesList = new CustomAttributesListBL(executionContext);
            customAttributesLastUpdateTime = customAttributesList.GetcustomAttributesLastUpdateTime(siteId);

            List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            customAttributesDTOList = customAttributesList.GetCustomAttributesDTOList(searchParameters, true, true);

            if (customAttributesDTOList != null && customAttributesDTOList.Any())
            {
                foreach (CustomAttributesDTO customAttributesDTO in customAttributesDTOList)
                {
                    customAttributesDTODictionary[customAttributesDTO.CustomAttributeId] = customAttributesDTO;
                }
            }
            else
            {
                customAttributesDTOList = new List<CustomAttributesDTO>();
                customAttributesDTODictionary = new ConcurrentDictionary<int, CustomAttributesDTO>();
            }
            log.LogMethodExit();
        }
        public List<CustomAttributesContainerDTO> GetCustomAttributesContainerDTOList()
        {
            log.LogMethodEntry();
            Dictionary<int, CustomAttributesDTO> customAttributesDTODictionary = new Dictionary<int, CustomAttributesDTO>();
            List<CustomAttributesContainerDTO> customAttributesContainerDTOList = new List<CustomAttributesContainerDTO>();


            foreach (CustomAttributesDTO customAttributesDTO in customAttributesDTOList)
            {
                CustomAttributesContainerDTO customAttributesContainerDTO = new CustomAttributesContainerDTO(customAttributesDTO.CustomAttributeId, customAttributesDTO.Name,
                    customAttributesDTO.Sequence, customAttributesDTO.Type, customAttributesDTO.Applicability, customAttributesDTO.Access);

                if (customAttributesDTO.CustomAttributeValueListDTOList != null && customAttributesDTO.CustomAttributeValueListDTOList.Any())
                {
                    customAttributesContainerDTO.CustomAttributeValueListContainerDTOList.AddRange(customAttributesDTO.CustomAttributeValueListDTOList.Select(x => new CustomAttributeValueListContainerDTO
                    (x.ValueId, x.Value, x.CustomAttributeId, x.IsDefault)));
                }
               

                customAttributesContainerDTOList.Add(customAttributesContainerDTO);
            }
            log.LogMethodExit(customAttributesContainerDTOList);
            return customAttributesContainerDTOList;
        }
        public CustomAttributesContainers Refresh()
        {
            log.LogMethodEntry();
            CustomAttributesListBL customAttributesList = new CustomAttributesListBL(executionContext);
            DateTime? updateTime = customAttributesList.GetcustomAttributesLastUpdateTime(siteId);
            if (customAttributesLastUpdateTime.HasValue
                && customAttributesLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in CustomAttributes since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            CustomAttributesContainers result = new CustomAttributesContainers(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
