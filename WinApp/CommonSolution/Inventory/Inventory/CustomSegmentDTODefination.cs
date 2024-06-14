/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - Bulk Upload Mapper Class for getting segments
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.0       25-Jul-2019   Muhammed Mehraj  Created      
 *2.70.2         13-Aug-2019   Deeksha          Added logger methods.
 *2.130.2      11-Aug-2021   Mushahid Faizan   Modified : WMS Issue fixes for duplicate item in dictionary.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    class CustomSegmentDTODefination : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Dictionary<string, string> segmentList;
        List<KeyValuePair<string, string>> segmentList;
        protected ExecutionContext executionContext;
        public CustomSegmentDTODefination(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(Dictionary<string, string>))
        {
            log.LogMethodEntry(executionContext, fieldName);
            this.executionContext = executionContext;


            CustomSegmentList customSegmentListBL = new CustomSegmentList(executionContext);
            List<CustomSegmentDTO> customSegmentList = customSegmentListBL.GetSegments(executionContext.GetSiteId());
            //KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();

            List<KeyValuePair<string, string>> segments = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> myItem = new KeyValuePair<string, string>();

            //  Dictionary<string, string> segments = new Dictionary<string, string>();
            if (customSegmentList != null && customSegmentList.Count > 0)
            {
                int i = 0;
                foreach (var item in customSegmentList)
                {
                    myItem = new KeyValuePair<string, string>(item.Name, string.Empty);
                    segments.Add(myItem);
                    // segments.Add(item.Name, string.Empty);
                    i++;
                }
                segmentList = segments;

            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Display names in Sheet rows
        /// </summary>
        public override string DisplayName
        {
            get
            {
                string result = string.Empty;
                if (segmentList != null && segmentList.Count > 0)
                {
                    result = segmentList.ElementAt(0).Key;
                }
                log.LogMethodExit(result);
                return result;
            }
        }

        /// <summary>
        /// Configure the templete object
        /// </summary>
        /// <param name="templateObject"></param>
        public override void Configure(object templateObject)
        {
            log.LogMethodEntry(templateObject);
            if (templateObject == null)
            {
                SetDisplayHeaderRows(false);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds headers rows for Segments
        /// </summary>
        /// <param name="headerRow"></param>
        public override void BuildHeaderRow(Row headerRow)
        {
            log.LogMethodEntry(headerRow);
            if (displayHeaderRows && segmentList != null && segmentList.Count > 0)
            {
                foreach (var segmentDefination in segmentList)
                {
                    headerRow.AddCell(new Cell(segmentDefination.Key));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Serialize the Segments Dictionary and add to Cell
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        public override void Serialize(Row row, object value)
        {
            log.LogMethodEntry(row, value);
            if (value != null && value is Dictionary<string, string>)
            {
                Dictionary<string, string> segmentItemList = value as Dictionary<string, string>;
                foreach (var segment in segmentItemList)
                {
                    if (!string.IsNullOrEmpty(segment.Value))
                    {
                        row.AddCell(new Cell(segment.Value));
                    }
                    else
                    {
                        row.AddCell(new Cell(string.Empty));
                    }

                }
            }
            else if (displayHeaderRows && segmentList != null && segmentList.Count > 0)
            {
                foreach (var segmentDefination in segmentList)
                {
                    row.AddCell(new Cell(string.Empty));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Desearlize segments to Dictionary
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="row"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public override object Deserialize(Row headerRow, Row row, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow, row, currentIndex);
            Dictionary<string, string> segmentKeyList = null;
            if (segmentList != null &&
                segmentList.Count > 0 &&
                row.Cells.Count > currentIndex)
            {
                segmentKeyList = new Dictionary<string, string>();
                foreach (var item in segmentList)
                {
                    segmentKeyList.Add(headerRow[currentIndex].Value, row[currentIndex].Value);
                    currentIndex++;
                }

            }
            log.LogMethodExit(segmentKeyList);
            return segmentKeyList;
        }

    }
}
