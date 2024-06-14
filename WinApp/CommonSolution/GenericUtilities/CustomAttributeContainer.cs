/********************************************************************************************
* Project Name - GenericUtilities
* Description  - CustomAttributeContainer
* 
**************
**Version Log
**************
*Version      Date            Modified By         Remarks          
*********************************************************************************************
*2.130.0     28-Jul-2020      Girish Kundar              Created 
********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    public class CustomAttributeContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<CustomAttributesDTO> customAttributesDTOList;
        private readonly CustomAttributeContainerDTOCollection CustomAttributesContainerDTOCollection;
        private readonly DateTime? customAttributeLastUpdateTime;
        private readonly ConcurrentDictionary<int, CustomAttributesDTO> customAttributesDTODictionary;
        private readonly ConcurrentDictionary<int, CustomAttributesContainerDTO> CustomAttributesContainerDTODictionary;
        private readonly int siteId;
        internal CustomAttributeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<CustomAttributesContainerDTO> CustomAttributesContainerDTOList = new List<CustomAttributesContainerDTO>();
            customAttributesDTODictionary = new ConcurrentDictionary<int, CustomAttributesDTO>();
            CustomAttributesContainerDTODictionary = new ConcurrentDictionary<int, CustomAttributesContainerDTO>();
            try
            {
                CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL();
                customAttributeLastUpdateTime = customAttributesListBL.GetcustomAttributesLastUpdateTime(siteId);
                List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters,true,true);
                if (customAttributesDTOList == null)
                {
                    customAttributesDTOList = new List<CustomAttributesDTO>();
                }
                if (customAttributesDTOList.Any())
                {
                    foreach (CustomAttributesDTO customAttributesDTO in customAttributesDTOList)
                    {
                        customAttributesDTODictionary[customAttributesDTO.CustomAttributeId] = customAttributesDTO;
                        CustomAttributesContainerDTO customAttributesContainerDTO = new CustomAttributesContainerDTO
                                                                                    (customAttributesDTO.CustomAttributeId,
                                                                                     customAttributesDTO.Name, 
                                                                                     customAttributesDTO.Sequence,
                                                                                     customAttributesDTO.Type, 
                                                                                     customAttributesDTO.Applicability,
                                                                                     customAttributesDTO.Access);
                        customAttributesContainerDTO.CustomAttributeValueListContainerDTOList = new List<CustomAttributeValueListContainerDTO>();
                        if (customAttributesDTO.CustomAttributeValueListDTOList != null
                                         && customAttributesDTO.CustomAttributeValueListDTOList.Any())
                        {
                            foreach (CustomAttributeValueListDTO customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
                            {
                                CustomAttributeValueListContainerDTO customAttributeValueListContainerDTO = new CustomAttributeValueListContainerDTO(
                                                                                      customAttributeValueListDTO.ValueId,
                                                                                      customAttributeValueListDTO.Value,
                                                                                      customAttributeValueListDTO.CustomAttributeId,
                                                                                      customAttributeValueListDTO.IsDefault);
                                customAttributesContainerDTO.CustomAttributeValueListContainerDTOList.Add(customAttributeValueListContainerDTO);
                            }
                        }
                        CustomAttributesContainerDTODictionary[customAttributesDTO.CustomAttributeId] = customAttributesContainerDTO;
                        CustomAttributesContainerDTOList.Add(customAttributesContainerDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating the CustomAttributes container.", ex);
                customAttributesDTOList = new List<CustomAttributesDTO>();
                customAttributesDTODictionary.Clear();
                CustomAttributesContainerDTOList.Clear();
                CustomAttributesContainerDTODictionary.Clear();
            }
            CustomAttributesContainerDTOCollection = new CustomAttributeContainerDTOCollection(CustomAttributesContainerDTOList);
            log.LogMethodExit();
        }

        internal List<CustomAttributesContainerDTO> GetCustomAttributesContainerDTOList()
        {
            log.LogMethodEntry();
            var result = CustomAttributesContainerDTOCollection.CustomAttributesContainerDTOList;
            log.LogMethodExit(result);
            return result;
        }


        public CustomAttributeContainerDTOCollection GetCustomAttributesContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(CustomAttributesContainerDTOCollection);
            return CustomAttributesContainerDTOCollection;
        }

        /// <summary>
        /// Returns the CustomAttributesContainerDTO   for a given attributeId
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public CustomAttributesContainerDTO GetCustomAttributesContainerDTO(int attributeId)
        {
            log.LogMethodEntry(attributeId);
            if (CustomAttributesContainerDTODictionary.ContainsKey(attributeId) == false)
            {
                string errorMessage = "CustomAttributes with attributeId :" + attributeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            CustomAttributesContainerDTO result = CustomAttributesContainerDTODictionary[attributeId];
            log.LogMethodExit(result);
            return result;
        }

        private CustomAttributesDTO GetCustomAttributesDTO(int attributeId)
        {
            log.LogMethodEntry(attributeId);
            if (customAttributesDTODictionary.ContainsKey(attributeId) == false)
            {
                string errorMessage = "CustomAttributes with attributeId :" + attributeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            CustomAttributesDTO result = customAttributesDTODictionary[attributeId];
            log.LogMethodExit(result);
            return result;
        }

        public CustomAttributeContainer Refresh()
        {
            log.LogMethodEntry();
            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL();
            DateTime? updateTime = customAttributesListBL.GetcustomAttributesLastUpdateTime(siteId);
            if (customAttributeLastUpdateTime.HasValue
                && customAttributeLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in customAttributes since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            CustomAttributeContainer result = new CustomAttributeContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
