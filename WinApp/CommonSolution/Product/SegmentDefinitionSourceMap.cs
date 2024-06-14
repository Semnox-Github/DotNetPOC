/********************************************************************************************
 * Project Name - Segment definition source map
 * Description  - Bussiness logic of segment definition source map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016   Raghuveera          Created
 *2.50        22-Jan-2019   Jagan Mohana   Created constructors SegmentDefinationMappingList and 
 *                                         added new method SaveUpdateSegmentDefinationMappingList
 *2.70        08-Apr-2019   Akshay Gulaganji    Added segmentDefinitionSourceValueDTOList as a child in GetAllSegmentDefinitionSourceMaps() and Save() method
 *2.110.0        15-Oct-2020   Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, Build() to get child records and 
 *                                                 List class changes as per 3 tier standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Segment definition will creates and modifies the segment definition source map
    /// </summary>
    public class SegmentDefinitionSourceMap
    {
        private SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
       
        /// <summary>
        /// Parameterized Contructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private SegmentDefinitionSourceMap(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext">Parameter of the type executionContext</param>
        /// <param name="segmentDefinitionSourceMapDTO">Parameter of the type segmentDefinitionSourceMapDTO</param>
        public SegmentDefinitionSourceMap(ExecutionContext executionContext, SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDTO) 
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, segmentDefinitionSourceMapDTO);
            this.segmentDefinitionSourceMapDTO = segmentDefinitionSourceMapDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the segmentDefinitionSourceMapDTO
        /// </summary>
        /// <param name="segmentDefinitionSourceId">segmentDefinitionSourceId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>segmentDefinitionSourceMap DTO</returns>        
        public SegmentDefinitionSourceMap(ExecutionContext executionContext, int segmentDefinitionSourceId, bool loadChildRecords = false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(segmentDefinitionSourceId, loadChildRecords, activeChildRecords,sqlTransaction);
            SegmentDefinitionSourceMapDataHandler segmentDefinitionSourceMapDataHandler = new SegmentDefinitionSourceMapDataHandler(sqlTransaction);
            this.segmentDefinitionSourceMapDTO = segmentDefinitionSourceMapDataHandler.GetSegmentDefinitionSourceMap(segmentDefinitionSourceId);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(segmentDefinitionSourceMapDTO);
        }

        /// <summary>
        /// Generate SegmentDefinitionSourceValueDTO list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            SegmentDefinitionSourceValueList segmentDefinitionSourceValueList = new SegmentDefinitionSourceValueList(executionContext);
            List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> searchParameters = new List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>>();
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.IS_ACTIVE, "Y"));
            }
            searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_ID, segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId.ToString()));
            segmentDefinitionSourceMapDTO.SegmentDefinitionSourceValueDTOList = segmentDefinitionSourceValueList.GetAllSegmentDefinitionSourceValues(searchParameters);
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the segment definition source map
        /// segment definition source map will be inserted if SegmentDefinitionSourceMapId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry();
            SegmentDefinitionSourceMapDataHandler segmentDefinitionSourceMapDataHandler = new SegmentDefinitionSourceMapDataHandler(sqlTransaction);
            if (segmentDefinitionSourceMapDTO.IsChangedRecursive == false && segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            Validate(sqlTransaction);
            if (segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId < 0)
            {
                segmentDefinitionSourceMapDTO = segmentDefinitionSourceMapDataHandler.InsertSegmentDefinitionSourceMap(segmentDefinitionSourceMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                segmentDefinitionSourceMapDTO.AcceptChanges();
            }
            else
            {
                if (segmentDefinitionSourceMapDTO.IsChanged == true)
                {
                    segmentDefinitionSourceMapDTO= segmentDefinitionSourceMapDataHandler.UpdateSegmentDefinitionSourceMap(segmentDefinitionSourceMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    segmentDefinitionSourceMapDTO.AcceptChanges();
                }
            }
            SaveChild(sqlTransaction);
            segmentDefinitionSourceMapDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : SegmentDefinitionSourceValueDTO 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveChild(SqlTransaction sqlTransaction)
        {
            if (segmentDefinitionSourceMapDTO.SegmentDefinitionSourceValueDTOList != null &&
                segmentDefinitionSourceMapDTO.SegmentDefinitionSourceValueDTOList.Any())
            {
                List<SegmentDefinitionSourceValueDTO> updatedSegmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
                foreach (var segmentDefinitionSourceValueDTO in segmentDefinitionSourceMapDTO.SegmentDefinitionSourceValueDTOList)
                {
                    if (segmentDefinitionSourceValueDTO.SegmentDefinitionSourceId != segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId)
                    {
                        segmentDefinitionSourceValueDTO.SegmentDefinitionSourceId = segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId;
                    }
                    if (segmentDefinitionSourceMapDTO.DataSourceType == "STATIC LIST" && string.IsNullOrEmpty(segmentDefinitionSourceValueDTO.ListValue))
                    {
                        log.Error("Please enter the List values.");
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 5086);
                        throw new ValidationException(errorMessage);
                    }
                    if (segmentDefinitionSourceMapDTO.DataSourceType == "DYNAMIC LIST" && string.IsNullOrEmpty(segmentDefinitionSourceValueDTO.DBQuery))
                    {
                        log.Error("Please enter the DB query values.");
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 5087);
                        throw new ValidationException(errorMessage);
                    }
                    if (segmentDefinitionSourceValueDTO.IsChanged)
                    {
                        updatedSegmentDefinitionSourceValueDTOList.Add(segmentDefinitionSourceValueDTO);
                    }
                }
                if (updatedSegmentDefinitionSourceValueDTOList.Any())
                {
                    SegmentDefinitionSourceValueList segmentDefinitionSourceValueList = new SegmentDefinitionSourceValueList(executionContext, updatedSegmentDefinitionSourceValueDTOList);
                    segmentDefinitionSourceValueList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the segmentDefinitionSourceMapDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (segmentDefinitionSourceMapDTO.SegmentDefinitionId == -1)
            {
                log.Error("Please select the segment definition.");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5082);
                throw new ValidationException(errorMessage);
            }
            if (string.IsNullOrEmpty(segmentDefinitionSourceMapDTO.DataSourceType))
            {
                log.Error("Please select the source type.");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5083);
                throw new ValidationException(errorMessage);
            }
            if (segmentDefinitionSourceMapDTO.DataSourceType.Equals("DYNAMIC LIST") && segmentDefinitionSourceMapDTO.IsActive)
            {
                if (string.IsNullOrEmpty(segmentDefinitionSourceMapDTO.DataSourceEntity))
                {
                    log.Error("Please select the data source entity.");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 5084);
                    throw new ValidationException(errorMessage);
                }
                if (string.IsNullOrEmpty(segmentDefinitionSourceMapDTO.DataSourceColumn))
                {
                    log.Error("Please select the data source column.");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 5085);
                    throw new ValidationException(errorMessage);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SegmentDefinitionSourceMapDTO SegmentDefinitionSourceMapDTO
        {
            get { return segmentDefinitionSourceMapDTO; }
        }
    }

    /// <summary>
    /// Manages the list of segment definition source map
    /// </summary>
    public class SegmentDefinitionSourceMapList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTOList = new List<SegmentDefinitionSourceMapDTO>();
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Contructor
        /// </summary>
        public SegmentDefinitionSourceMapList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="segmentDefinitionSourceMapList"></param>
        /// <param name="executionContext"></param>
        public SegmentDefinitionSourceMapList(ExecutionContext executionContext, List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, segmentDefinitionSourceMapList);
            this.segmentDefinitionSourceMapDTOList = segmentDefinitionSourceMapList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the segmentDefinitionSourceMapDTOList List for segmentDefinitionIdList
        /// </summary>
        /// <param name="segmentDefinitionIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of segmentDefinitionSourceMapDTOList</returns>
        public List<SegmentDefinitionSourceMapDTO> GetSegmentDefinitionSourceMapDTOList(List<int> segmentDefinitionIdList, bool loadChildRecords = true, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(segmentDefinitionIdList, activeRecords);
            SegmentDefinitionSourceMapDataHandler segmentDefinitionSourceMapDataHandler = new SegmentDefinitionSourceMapDataHandler(sqlTransaction);
            this.segmentDefinitionSourceMapDTOList = segmentDefinitionSourceMapDataHandler.GetSegmentDefinitionSourceMapDTOList(segmentDefinitionIdList, activeRecords, sqlTransaction);
            if (segmentDefinitionSourceMapDTOList != null && segmentDefinitionSourceMapDTOList.Any() && loadChildRecords)
            {
                Build(segmentDefinitionSourceMapDTOList, activeRecords, sqlTransaction);
            }
            log.LogMethodExit(segmentDefinitionSourceMapDTOList);
            return segmentDefinitionSourceMapDTOList;
        }

        private void Build(List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTODictionary = new Dictionary<int, SegmentDefinitionSourceMapDTO>();
            List<int> segmentDefinitionSourceIdList = new List<int>();
            for (int i = 0; i < segmentDefinitionSourceMapDTOList.Count; i++)
            {
                if (segmentDefinitionSourceMapDTODictionary.ContainsKey(segmentDefinitionSourceMapDTOList[i].SegmentDefinitionSourceId))
                {
                    continue;
                }
                segmentDefinitionSourceMapDTODictionary.Add(segmentDefinitionSourceMapDTOList[i].SegmentDefinitionSourceId, segmentDefinitionSourceMapDTOList[i]);
                segmentDefinitionSourceIdList.Add(segmentDefinitionSourceMapDTOList[i].SegmentDefinitionSourceId);
            }
            SegmentDefinitionSourceValueList segmentDefinitionSourceValueList = new SegmentDefinitionSourceValueList(executionContext);
            List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList = segmentDefinitionSourceValueList.GetSegmentDefinitionSourceValueList(segmentDefinitionSourceIdList, activeChildRecords, sqlTransaction);

            if (segmentDefinitionSourceValueDTOList != null && segmentDefinitionSourceValueDTOList.Any())
            {
                for (int i = 0; i < segmentDefinitionSourceValueDTOList.Count; i++)
                {
                    if (segmentDefinitionSourceMapDTODictionary.ContainsKey(segmentDefinitionSourceValueDTOList[i].SegmentDefinitionSourceId) == false)
                    {
                        continue;
                    }
                    SegmentDefinitionSourceMapDTO screenZoneDefSetupDTO = segmentDefinitionSourceMapDTODictionary[segmentDefinitionSourceValueDTOList[i].SegmentDefinitionSourceId];
                    if (screenZoneDefSetupDTO.SegmentDefinitionSourceValueDTOList == null)
                    {
                        screenZoneDefSetupDTO.SegmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
                    }
                    screenZoneDefSetupDTO.SegmentDefinitionSourceValueDTOList.Add(segmentDefinitionSourceValueDTOList[i]);
                }
            }
        }

        /// <summary>
        /// Returns the segment definition source map list
        /// </summary>
        public List<SegmentDefinitionSourceMapDTO> GetAllSegmentDefinitionSourceMaps(List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>> searchParameters, bool loadChildRecord = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters);
            SegmentDefinitionSourceMapDataHandler segmentDefinitionSourceMapDataHandler = new SegmentDefinitionSourceMapDataHandler(sqlTransaction);
            this.segmentDefinitionSourceMapDTOList = segmentDefinitionSourceMapDataHandler.GetSegmentDefinitionSourceMapList(searchParameters);
            if (loadChildRecord == true && segmentDefinitionSourceMapDTOList != null)
            {
                Build(segmentDefinitionSourceMapDTOList, loadActiveChildRecords, sqlTransaction);

            }
            log.LogMethodExit(segmentDefinitionSourceMapDTOList);
            return segmentDefinitionSourceMapDTOList;
        }

        /// <summary>
        /// Saves the segmentDefinitionSourceMapDTOList 
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (segmentDefinitionSourceMapDTOList == null ||
                segmentDefinitionSourceMapDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < segmentDefinitionSourceMapDTOList.Count; i++)
            {
                var segmentDefinitionSourceMapDTO = segmentDefinitionSourceMapDTOList[i];
                if (segmentDefinitionSourceMapDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    SegmentDefinitionSourceMap segmentDefinitionSourceMapBL = new SegmentDefinitionSourceMap(executionContext, segmentDefinitionSourceMapDTO);
                    segmentDefinitionSourceMapBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving segmentDefinitionSourceMapDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("segmentDefinitionSourceMapDTO", segmentDefinitionSourceMapDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}