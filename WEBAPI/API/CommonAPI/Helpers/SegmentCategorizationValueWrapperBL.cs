/********************************************************************************************
 * Project Name - Product Controller
 * Description  - Created to fetch, update and insert Segments values.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        24-Jan-2019   Muhammed Mehraj          Created 
 *2.60        17-May-2019   Akshay Gulaganji         modified GetSegmentValues() for getting a Vendor and Category and modified SaveUpdateSegmentValueList() for saving DATE
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;
using System;
using System.Collections.Generic;
using System.Data;

namespace Semnox.CommonAPI.Helpers
{
    public class SegmentCategorizationValueWrapperBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public SegmentCategorizationValueWrapperBL(ExecutionContext executioncontext)
        {
            log.LogMethodEntry(executioncontext);
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the Segment Values
        /// </summary>
        /// <param name="segmentDefinitionDTOSearchParams"></param>
        /// <param name="applicability"></param>
        /// <param name="segmentCategoryId"></param>
        /// <returns>segmentCategorizationValueWrapperDTOList</returns>
        public List<SegmentCategorizationValueWrapperDTO> GetSegmentValues(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionDTOSearchParams, string applicability, string segmentCategoryId)
        {
            log.LogMethodEntry(segmentDefinitionDTOSearchParams, applicability, segmentCategoryId);
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTOList;
            SegmentDefinitionSourceMapList segmentDefinitionSourceMapList = new SegmentDefinitionSourceMapList(executionContext);
            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(executionContext);
            List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>> segmentDefinitionSourceMapDTOSearchParams;
            segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionDTOSearchParams);
            List<SegmentCategorizationValueWrapperDTO> segmentsList = new List<SegmentCategorizationValueWrapperDTO>();
            if (segmentDefinitionDTOList != null)
            {
                foreach (SegmentDefinitionDTO segmentDefinitionDTO in segmentDefinitionDTOList)
                {
                    //Retriving source maping data for the perticular segment definiton
                    segmentDefinitionSourceMapDTOList = new List<SegmentDefinitionSourceMapDTO>();
                    segmentDefinitionSourceMapDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>>();
                    segmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    segmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.IS_ACTIVE, "Y"));
                    segmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_ID, segmentDefinitionDTO.SegmentDefinitionId.ToString()));
                    segmentDefinitionSourceMapDTOList = segmentDefinitionSourceMapList.GetAllSegmentDefinitionSourceMaps(segmentDefinitionSourceMapDTOSearchParams);

                    List<KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>> segmentCategorizationValueDTOSearchParams;
                    List<SegmentCategorizationValueDTO> segmentCategorizationValueDTOList = new List<SegmentCategorizationValueDTO>();
                    SegmentCategorizationValueList segmentCategorizationValueList = new SegmentCategorizationValueList(executionContext);
                    segmentCategorizationValueDTOSearchParams = new List<KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>>();
                    segmentCategorizationValueDTOSearchParams.Add(new KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    segmentCategorizationValueDTOSearchParams.Add(new KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.IS_ACTIVE, "Y"));
                    segmentCategorizationValueDTOSearchParams.Add(new KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_CATEGORY_ID, segmentCategoryId));
                    segmentCategorizationValueDTOList = segmentCategorizationValueList.GetAllSegmentCategorizationValues(segmentCategorizationValueDTOSearchParams);
                    if (segmentDefinitionSourceMapDTOList != null)
                    {
                        foreach (SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDTO in segmentDefinitionSourceMapDTOList)
                        {
                            SegmentCategorizationValueWrapperDTO segment = new SegmentCategorizationValueWrapperDTO();
                            segment.SegmentDefinationId = segmentDefinitionDTO.SegmentDefinitionId;
                            segment.SegmentName = segmentDefinitionDTO.SegmentName;
                            segment.Applicability = applicability.ToUpper();
                            segment.IsMandatory = segmentDefinitionDTO.IsMandatory == "Y" ? true : false;
                            switch (segmentDefinitionSourceMapDTO.DataSourceType)
                            {
                                case "TEXT":
                                    {
                                        segment.Type = "TEXT";
                                        if (segmentCategorizationValueDTOList != null)
                                        {
                                            foreach (SegmentCategorizationValueDTO segmentCategorizationValueDTO in segmentCategorizationValueDTOList)
                                            {
                                                if (segmentDefinitionSourceMapDTO.SegmentDefinitionId == segmentCategorizationValueDTO.SegmentDefinitionId)
                                                {
                                                    segment.SegmentValueText = segmentCategorizationValueDTO.SegmentValueText;
                                                    segment.SegmentCategoryId = segmentCategorizationValueDTO.SegmentCategoryId;
                                                    segment.SegmentCategoryValueId = segmentCategorizationValueDTO.SegmentCategoryValueId;
                                                }
                                            }
                                        }
                                        segmentsList.Add(segment);
                                    }
                                    break;
                                case "DATE":
                                    {
                                        segment.Type = "DATE";
                                        if (segmentCategorizationValueDTOList != null)
                                        {
                                            foreach (SegmentCategorizationValueDTO segmentCategorizationValueDTO in segmentCategorizationValueDTOList)
                                            {
                                                if (segmentDefinitionSourceMapDTO.SegmentDefinitionId == segmentCategorizationValueDTO.SegmentDefinitionId)
                                                {
                                                    segment.SegmentValueDate = segmentCategorizationValueDTO.SegmentValueDate;
                                                    segment.SegmentCategoryId = segmentCategorizationValueDTO.SegmentCategoryId;
                                                    segment.SegmentCategoryValueId = segmentCategorizationValueDTO.SegmentCategoryValueId;
                                                }
                                            }
                                        }
                                        segmentsList.Add(segment);

                                    }
                                    break;
                                case "STATIC LIST":
                                    {
                                        segment.Type = "LIST";
                                        List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
                                        segmentDefinitionSourceValueDTOList.Insert(0, new SegmentDefinitionSourceValueDTO());
                                        segmentDefinitionSourceValueDTOList[0].ListValue = "";
                                        SegmentDefinitionSourceValueList segmentDefinitionSourceValueList = new SegmentDefinitionSourceValueList(executionContext);
                                        List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> segmentDefinitionSourceValueDTOSearchParams;
                                        segmentDefinitionSourceValueDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>>();
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.IS_ACTIVE, "Y"));
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_ID, segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId.ToString()));

                                        List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceList = segmentDefinitionSourceValueList.GetAllSegmentDefinitionSourceValues(segmentDefinitionSourceValueDTOSearchParams);
                                        if (segmentDefinitionSourceList != null)
                                        {
                                            segmentDefinitionSourceValueDTOList.AddRange(segmentDefinitionSourceList);
                                        }
                                        if (segmentCategorizationValueDTOList != null)
                                        {
                                            foreach (SegmentCategorizationValueDTO segmentCategorizationValueDTO in segmentCategorizationValueDTOList)
                                            {
                                                if (segmentDefinitionSourceMapDTO.SegmentDefinitionId == segmentCategorizationValueDTO.SegmentDefinitionId)
                                                {
                                                    segment.ValueId = segmentCategorizationValueDTO.SegmentStaticValueId;
                                                    segment.SegmentCategoryId = segmentCategorizationValueDTO.SegmentCategoryId;
                                                    segment.SegmentDefinationId = segmentCategorizationValueDTO.SegmentDefinitionId;
                                                    segment.SegmentCategoryValueId = segmentCategorizationValueDTO.SegmentCategoryValueId;
                                                }
                                            }
                                        }
                                        List<SegmentCategorizationValueListWrapperDTO> valueList = new List<SegmentCategorizationValueListWrapperDTO>();
                                        if (segmentDefinitionSourceValueDTOList.Count > 0)
                                        {
                                            foreach (SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDTO in segmentDefinitionSourceValueDTOList)
                                            {
                                                SegmentCategorizationValueListWrapperDTO customAttributeValue = new SegmentCategorizationValueListWrapperDTO();
                                                customAttributeValue.Value = segmentDefinitionSourceValueDTO.ListValue;
                                                customAttributeValue.ValueId = segmentDefinitionSourceValueDTO.SegmentDefinitionSourceValueId;
                                                valueList.Add(customAttributeValue);
                                            }
                                            segment.SegmentValueList = valueList;
                                        }
                                        segmentsList.Add(segment);
                                    }
                                    break;
                                case "DYNAMIC LIST":
                                    {
                                        segment.Type = "DYNAMIC LIST";
                                        string sql = string.Empty;
                                        List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList;
                                        SegmentDefinitionSourceValueList segmentDefinitionSourceValueList = new SegmentDefinitionSourceValueList(executionContext);
                                        List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> segmentDefinitionSourceValueDTOSearchParams;
                                        segmentDefinitionSourceValueDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>>();
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.IS_ACTIVE, "Y"));
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_ID, segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId.ToString()));
                                        segmentDefinitionSourceValueDTOList = segmentDefinitionSourceValueList.GetAllSegmentDefinitionSourceValues(segmentDefinitionSourceValueDTOSearchParams);
                                        sql = "Select * from " + segmentDefinitionSourceMapDTO.DataSourceEntity + " " + segmentDefinitionSourceMapDTO.DataSourceEntity.Substring(0, 1);
                                        if (segmentDefinitionSourceValueDTOList != null && segmentDefinitionSourceValueDTOList.Count > 0)
                                        {
                                            sql += " Where " + segmentDefinitionSourceValueDTOList[0].DBQuery;
                                        }
                                        switch (segmentDefinitionSourceMapDTO.DataSourceEntity)
                                        {
                                            case "VENDOR":
                                                {
                                                    List<VendorDTO> vendorDTOList;// = new List<Core.Vendor.VendorDTO>();
                                                    VendorList vendorList = new VendorList(executionContext);
                                                    vendorDTOList = vendorList.GetVendorList(sql);
                                                    if (vendorDTOList == null)
                                                    {
                                                        vendorDTOList = new List<VendorDTO>();
                                                    }
                                                    if (segmentCategorizationValueDTOList != null)
                                                    {
                                                        foreach (SegmentCategorizationValueDTO segmentCategorizationValueDTO in segmentCategorizationValueDTOList)
                                                        {
                                                            if (segmentDefinitionSourceMapDTO.SegmentDefinitionId == segmentCategorizationValueDTO.SegmentDefinitionId)
                                                            {

                                                                segment.SegmentCategoryId = segmentCategorizationValueDTO.SegmentCategoryId;
                                                                //customAttributes.CustomDataId = segmentCategorizationValueDTO.SegmentDefinitionId;
                                                                segment.DynamicValueID = segmentCategorizationValueDTO.SegmentDynamicValueId;
                                                                segment.SegmentCategoryValueId = segmentCategorizationValueDTO.SegmentCategoryValueId;
                                                            }
                                                        }
                                                    }
                                                    List<SegmentCategorizationValueListWrapperDTO> dynamicCustomAttributeValueList = new List<SegmentCategorizationValueListWrapperDTO>();
                                                    if (vendorDTOList != null)
                                                    {
                                                        DataTable dataTable = vendorList.GetVendorColumnsName();
                                                        foreach (var item in vendorDTOList)
                                                        {
                                                            foreach (DataRow dataRow in dataTable.Rows)
                                                            {
                                                                if (segmentDefinitionSourceMapDTO.DataSourceColumn == dataRow.ItemArray[0].ToString())
                                                                {
                                                                    SegmentCategorizationValueListWrapperDTO customAttribute = new SegmentCategorizationValueListWrapperDTO();
                                                                    customAttribute.Value = item.Name;
                                                                    customAttribute.ValueId = item.VendorId;//Added on 17 May 2019
                                                                    customAttribute.AcceptChanges();//Added on 17 May 2019
                                                                    dynamicCustomAttributeValueList.Add(customAttribute);
                                                                }
                                                            }
                                                            segment.SegmentValueList = dynamicCustomAttributeValueList;
                                                        }
                                                    }
                                                    segmentsList.Add(segment);
                                                    break;
                                                }
                                            case "CATEGORY":
                                                {
                                                    List<CategoryDTO> categoryDTOList = new List<CategoryDTO>();
                                                    CategoryList categoryList = new CategoryList(executionContext);
                                                    categoryDTOList = categoryList.GetCategoryList(sql);
                                                    if (categoryDTOList == null)
                                                    {
                                                        categoryDTOList = new List<CategoryDTO>();
                                                    }
                                                    if (segmentCategorizationValueDTOList != null)
                                                    {

                                                        foreach (SegmentCategorizationValueDTO segmentCategorizationValueDTO in segmentCategorizationValueDTOList)
                                                        {
                                                            if (segmentDefinitionSourceMapDTO.SegmentDefinitionId == segmentCategorizationValueDTO.SegmentDefinitionId)
                                                            {
                                                                segment.SegmentCategoryId = segmentCategorizationValueDTO.SegmentCategoryId;
                                                                //customAttributes.CustomDataId = segmentCategorizationValueDTO.SegmentDefinitionId;
                                                                segment.DynamicValueID = segmentCategorizationValueDTO.SegmentDynamicValueId;
                                                                segment.SegmentCategoryValueId = segmentCategorizationValueDTO.SegmentCategoryValueId;
                                                            }
                                                        }
                                                    }
                                                    List<SegmentCategorizationValueListWrapperDTO> dynamicList = new List<SegmentCategorizationValueListWrapperDTO>();
                                                    if (categoryDTOList != null)
                                                    {
                                                        DataTable dataTable = categoryList.GetCategoryColumnsName();
                                                        foreach (var item in categoryDTOList)
                                                        {
                                                            foreach (DataRow dataRow in dataTable.Rows)
                                                            {
                                                                if (segmentDefinitionSourceMapDTO.DataSourceColumn == dataRow.ItemArray[0].ToString())
                                                                {
                                                                    SegmentCategorizationValueListWrapperDTO customAttributeValue = new SegmentCategorizationValueListWrapperDTO();
                                                                    customAttributeValue.Value = item.Name;
                                                                    customAttributeValue.ValueId = item.CategoryId;//Added on 17-May-2019
                                                                    customAttributeValue.AcceptChanges();//Added on 17-May-2019
                                                                    dynamicList.Add(customAttributeValue);
                                                                }
                                                            }
                                                            segment.SegmentValueList = dynamicList;
                                                        }
                                                    }
                                                    segmentsList.Add(segment);
                                                    break;
                                                }
                                        }
                                    }
                                    break;
                            }
                            break;//Because one segment definition can have only one source maping for one applicability
                        }
                    }
                }
            }
            log.LogMethodExit(segmentsList);
            return segmentsList;
        }

        public void SaveUpdateSegmentValueList(List<SegmentCategorizationValueWrapperDTO> segmentCategorizationsList, int productId, string applicability)
        {
            try
            {
                log.LogMethodEntry(segmentCategorizationsList, productId, applicability);
                List<SegmentCategorizationValueDTO> segmentCategorizationValueList = new List<SegmentCategorizationValueDTO>();

                foreach (SegmentCategorizationValueWrapperDTO segmentCategorizationValueWrapperDTO in segmentCategorizationsList)
                {
                    if ((string.IsNullOrEmpty(segmentCategorizationValueWrapperDTO.SegmentValueText) || string.IsNullOrWhiteSpace(segmentCategorizationValueWrapperDTO.SegmentValueText)) && segmentCategorizationValueWrapperDTO.IsMandatory)
                    {
                        throw new ValidationException(segmentCategorizationValueWrapperDTO.SegmentName + " is mandatory ");                        
                    }
                    else
                    {
                        if (segmentCategorizationValueWrapperDTO.SegmentValueList != null)
                        {
                            foreach (SegmentCategorizationValueListWrapperDTO segmentCategorizationValueListWrapperDTO in segmentCategorizationValueWrapperDTO.SegmentValueList)
                            {
                                if (segmentCategorizationValueWrapperDTO.Type == "LIST")
                                {
                                    SegmentCategorizationValueDTO segmentCategorizationDTO = new SegmentCategorizationValueDTO();
                                    segmentCategorizationDTO.SegmentStaticValueId = segmentCategorizationValueListWrapperDTO.ValueId;
                                    if (segmentCategorizationValueWrapperDTO.SegmentCategoryId == 0)
                                    {
                                        segmentCategorizationDTO.SegmentCategoryId = -1;
                                    }
                                    else
                                    {
                                        segmentCategorizationDTO.SegmentCategoryId = segmentCategorizationValueWrapperDTO.SegmentCategoryId;
                                    }
                                    //segmentCategorizationDTO.SegmentCategoryId = segmentCategorizationValueWrapperDTO.SegmentCategoryId;
                                    segmentCategorizationDTO.SegmentDefinitionId = segmentCategorizationValueWrapperDTO.SegmentDefinationId;
                                    segmentCategorizationDTO.SegmentCategoryValueId = segmentCategorizationValueWrapperDTO.SegmentCategoryValueId;
                                    segmentCategorizationDTO.Siteid = executionContext.GetSiteId();
                                    segmentCategorizationDTO.CreatedBy = executionContext.GetUserId();
                                    segmentCategorizationDTO.CreationDate = DateTime.Now;
                                    segmentCategorizationDTO.LastUpdatedBy = executionContext.GetUserId();
                                    segmentCategorizationDTO.IsChanged = segmentCategorizationValueListWrapperDTO.IsChanged;
                                    segmentCategorizationValueList.Add(segmentCategorizationDTO);
                                }
                                else if (segmentCategorizationValueWrapperDTO.Type == "DYNAMIC LIST")
                                {
                                    SegmentCategorizationValueDTO segmentCategorizationDTO = new SegmentCategorizationValueDTO();
                                    segmentCategorizationDTO.SegmentDynamicValueId = segmentCategorizationValueListWrapperDTO.Value;
                                    if (segmentCategorizationValueWrapperDTO.SegmentCategoryId == 0)
                                    {
                                        segmentCategorizationDTO.SegmentCategoryId = -1;
                                    }
                                    else
                                    {
                                        segmentCategorizationDTO.SegmentCategoryId = segmentCategorizationValueWrapperDTO.SegmentCategoryId;
                                    }
                                    //segmentCategorizationDTO.SegmentCategoryId = segmentCategorizationValueWrapperDTO.SegmentCategoryId;
                                    segmentCategorizationDTO.SegmentDefinitionId = segmentCategorizationValueWrapperDTO.SegmentDefinationId;
                                    segmentCategorizationDTO.SegmentCategoryValueId = segmentCategorizationValueWrapperDTO.SegmentCategoryValueId;
                                    segmentCategorizationDTO.Siteid = executionContext.GetSiteId();
                                    segmentCategorizationDTO.CreatedBy = executionContext.GetUserId();
                                    segmentCategorizationDTO.CreationDate = DateTime.Now;
                                    segmentCategorizationDTO.LastUpdatedBy = executionContext.GetUserId();
                                    segmentCategorizationDTO.IsChanged = segmentCategorizationValueListWrapperDTO.IsChanged;
                                    segmentCategorizationValueList.Add(segmentCategorizationDTO);
                                }
                            }
                        }
                        if (segmentCategorizationValueWrapperDTO.Type == "DATE")
                        {
                            SegmentCategorizationValueDTO segmentCategorizationDTO = new SegmentCategorizationValueDTO();
                            segmentCategorizationDTO.SegmentValueDate = segmentCategorizationValueWrapperDTO.SegmentValueText == "" ? DateTime.MinValue : Convert.ToDateTime(segmentCategorizationValueWrapperDTO.SegmentValueText);
                            if (segmentCategorizationValueWrapperDTO.SegmentCategoryId == 0)
                            {
                                segmentCategorizationDTO.SegmentCategoryId = -1;
                            }
                            else
                            {
                                segmentCategorizationDTO.SegmentCategoryId = segmentCategorizationValueWrapperDTO.SegmentCategoryId;
                            }
                            segmentCategorizationDTO.SegmentDefinitionId = segmentCategorizationValueWrapperDTO.SegmentDefinationId;
                            segmentCategorizationDTO.SegmentCategoryValueId = segmentCategorizationValueWrapperDTO.SegmentCategoryValueId;
                            segmentCategorizationDTO.Siteid = executionContext.GetSiteId();
                            segmentCategorizationDTO.CreatedBy = executionContext.GetUserId();
                            segmentCategorizationDTO.CreationDate = DateTime.Now;
                            segmentCategorizationDTO.LastUpdatedBy = executionContext.GetUserId();
                            segmentCategorizationDTO.IsChanged = segmentCategorizationValueWrapperDTO.IsChanged;
                            segmentCategorizationValueList.Add(segmentCategorizationDTO);
                        }
                        else if (segmentCategorizationValueWrapperDTO.Type == "TEXT")
                        {
                            SegmentCategorizationValueDTO segmentCategorizationDTO = new SegmentCategorizationValueDTO();
                            segmentCategorizationDTO.SegmentValueText = segmentCategorizationValueWrapperDTO.SegmentValueText;
                            if (segmentCategorizationValueWrapperDTO.SegmentCategoryId == 0)
                            {
                                segmentCategorizationDTO.SegmentCategoryId = -1;
                            }
                            else
                            {
                                segmentCategorizationDTO.SegmentCategoryId = segmentCategorizationValueWrapperDTO.SegmentCategoryId;
                            }
                            //segmentCategorizationDTO.SegmentCategoryId = segmentCategorizationValueWrapperDTO.SegmentCategoryId;
                            segmentCategorizationDTO.SegmentDefinitionId = segmentCategorizationValueWrapperDTO.SegmentDefinationId;
                            segmentCategorizationDTO.SegmentCategoryValueId = segmentCategorizationValueWrapperDTO.SegmentCategoryValueId;
                            segmentCategorizationDTO.Siteid = executionContext.GetSiteId();
                            segmentCategorizationDTO.CreatedBy = executionContext.GetUserId();
                            segmentCategorizationDTO.CreationDate = DateTime.Now;
                            segmentCategorizationDTO.LastUpdatedBy = executionContext.GetUserId();
                            segmentCategorizationDTO.IsChanged = segmentCategorizationValueWrapperDTO.IsChanged;
                            segmentCategorizationValueList.Add(segmentCategorizationDTO);
                        }
                    }
                }
                if (segmentCategorizationValueList.Count != 0)
                {
                    SegmentCategorizationValue segmentCategorization = new SegmentCategorizationValue(executionContext);
                    segmentCategorization.SaveSegmentCategorizationValues(segmentCategorizationValueList, productId, applicability);
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
