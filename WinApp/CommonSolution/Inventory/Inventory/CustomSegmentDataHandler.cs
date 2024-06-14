/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - DataHandler Class for  CustomSegmentDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.0    28-Jun-2019      Mehraj        Created                          
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    class CustomSegmentDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomSegmentDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Populate segments to CustomSegmentDTO
        /// </summary>
        /// <param name="customSegmentsRow"></param>
        /// <returns></returns>
        private CustomSegmentDTO GetSegments(DataRow customSegmentsRow)
        {
            log.LogMethodEntry(customSegmentsRow);
            CustomSegmentDTO customSegmentDTO = new CustomSegmentDTO(customSegmentsRow["name"].ToString(),
                customSegmentsRow["segmentdefinitionid"].ToString(),
                customSegmentsRow["datasourcetype"].ToString(),
                customSegmentsRow["isMandatory"].ToString()
              );
            log.LogMethodExit(customSegmentDTO);
            return customSegmentDTO;
        }

        /// <summary>
        /// Gets the Segments for Product
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<CustomSegmentDTO> GetSegments(int siteId)
        {
            log.LogMethodEntry(siteId);
            //SqlParameter sqlParam;
            //sqlParam = new SqlParameter("@site_id", siteId);
            //if (utilities.ParafaitEnv.IsCorporate)
            //{
            //    sqlParam = new SqlParameter("@site_id", machineUserContext.GetSiteId());
            //}
            //else
            //{
            //    sqlParam = new SqlParameter("@site_id", -1);
            //}
            //log.LogVariableState("sqlParam", sqlParam);
            string selectSegmentDefinitionQuery = @"select segmentname name, d.segmentdefinitionid, datasourcetype, d.isMandatory
                                from segment_definition d, segment_definition_source_mapping m
                                where d.isactive = 'Y'
	                                and applicableentity = 'PRODUCT'
	                                and d.segmentdefinitionid = m.segmentdefinitionid
                                    and m.isactive = 'Y'  and (d.site_id = @site_id or @site_id = -1)";
            SqlParameter[] selectSegmentDefinitionParameters = new SqlParameter[1];
            selectSegmentDefinitionParameters[0] = new SqlParameter("@site_id", siteId);
            DataTable customSegmentDefinition = dataAccessHandler.executeSelectQuery(selectSegmentDefinitionQuery, selectSegmentDefinitionParameters);
            if (customSegmentDefinition.Rows.Count > 0)
            {
                List<CustomSegmentDTO> customSegmentList = new List<CustomSegmentDTO>();
                foreach (DataRow customSegmentsRow in customSegmentDefinition.Rows)
                {
                    CustomSegmentDTO customSegmentDTO = GetSegments(customSegmentsRow);
                    customSegmentList.Add(customSegmentDTO);
                }
                log.LogMethodEntry(customSegmentList);
                return customSegmentList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}
