///********************************************************************************************
// * Project Name - CustomAttributes Wrapper BL
// * Description  - Business logic
// * 
// **************
// **Version Log
// **************
// *Version     Date             Modified By    Remarks          
// *********************************************************************************************
// *1.00        03-Oct-2018      Jagan     Created 
// ********************************************************************************************/
//using Semnox.Core.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;

//namespace Semnox.Core.GenericUtilities
//{
//    /// <summary>
//    /// Business logic for CustomAttributes Wrapper class.
//    /// </summary>
//    public class CustomAttributesWrapperBL
//    {
//        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
//        private readonly ExecutionContext executionContext = null;
        
//        /// <summary>
//        /// Parameterized constructor
//        /// </summary>
//        /// <param name="executionContext">execution context</param>
//        public CustomAttributesWrapperBL(ExecutionContext executioncontext)
//        {
//            executionContext = executioncontext;
//        }
//        /// <summary>
//        /// Returns the CustomAttributes Wrapper list
//        /// </summary>
//        public List<CustomAttributesWrapperDTO> GetCustomeAttributes(List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> customAttributesSearchParameters, List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> customDataSearchParameters, string customdataSetId)
//        {
//            List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>> searchParameters = null;

//            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL();
//            List<CustomAttributesDTO> CustomAttributes = customAttributesListBL.GetCustomAttributesDTOList(customAttributesSearchParameters);
//            CustomDataListBL customDataListBL = new CustomDataListBL(executionContext);
//            List<CustomDataDTO> CustomData = customDataListBL.GetCustomDataDTOList(customDataSearchParameters);
//            CustomAttributeValueListListBL customAttributeValueListBL = new CustomAttributeValueListListBL(executionContext);
//            List<CustomAttributeValueListDTO> CustomAttributeValueList = customAttributeValueListBL.GetCustomAttributeValueListDTOList(searchParameters);
//            int customDataSetId = Convert.ToInt32(customdataSetId);

//            List<CustomAttributesWrapperDTO> customAttributesDataList = new List<CustomAttributesWrapperDTO>();
//            foreach (var item in CustomAttributes)
//            {
//                CustomAttributesWrapperDTO cawDTO = new CustomAttributesWrapperDTO();
//                cawDTO.CustomAttributeId = item.CustomAttributeId;
//                cawDTO.Name = item.Name;
//                cawDTO.Type = item.Type;
//                cawDTO.Applicability = item.Applicability;
//                cawDTO.CustomDataSetId = customDataSetId;
//                cawDTO.IsChanged = false;
//                if (CustomData.Count != 0)
//                {
//                    var customDataValues = CustomData.Find(x => x.CustomDataSetId == customDataSetId && x.CustomAttributeId == item.CustomAttributeId);
//                    if (customDataValues != null)
//                    {
//                        cawDTO.CustomDataId = customDataValues.CustomDataId;
//                        cawDTO.CustomDataText = customDataValues.CustomDataText;
//                        cawDTO.CustomDataNumber = customDataValues.CustomDataNumber;
//                        cawDTO.CustomDataDate = customDataValues.CustomDataDate;
//                    }
//                }
//                if (CustomAttributeValueList.Count != 0)
//                {
//                    var valuesdata = CustomAttributeValueList.Where(x => x.CustomAttributeId == item.CustomAttributeId).ToList();
//                    if (valuesdata.Count != 0)
//                    {
//                        List<CustomAttributeValueListWrapperDTO> valueList = new List<CustomAttributeValueListWrapperDTO>();
//                        foreach (var valuedata in valuesdata)
//                        {
//                            CustomAttributeValueListWrapperDTO cavlDTO = new CustomAttributeValueListWrapperDTO();
//                            cavlDTO.CustomAttributeId = item.CustomAttributeId;
//                            cavlDTO.Value = valuedata.Value;
//                            cavlDTO.ValueId = valuedata.ValueId;
//                            valueList.Add(cavlDTO);
//                        }
//                        cawDTO.ValueList = valueList;
//                    }
//                }
//                customAttributesDataList.Add(cawDTO);
//            }
//            return customAttributesDataList;
//        }

//    }
//}
