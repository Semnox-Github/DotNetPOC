/********************************************************************************************
 * Project Name - Segment definition
 * Description  - Bussiness logic of segment definition
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016   Raghuveera          Created 
 *2.70        22-Jan-2019   Jagan Mohana        Created constructors SegmentDefinitionList and 
 *                                              added new method SaveUpdatesegmentDefinitionList
 *2.70        17-Mar-2019   Manoj Durgam        Added ExecutionContext to the constructor
 *2.70        25-Mar-2019   Nagesh Badiger      Added log method entry and method exit
 *2.110.0     15-Oct-2020   Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, Build() to get child records and 
 *                                                 List class changes as per 3 tier standards.
 *2.120.0     05-Mar-2021   Girish kundar       Modified : Issue Fix - validation error message
 *2.150.0     13-Dec-2022   Abhishek            Modified:Validate() as a part of Web Inventory Redesign.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Segment definition will creates and modifies the segment definition
    /// </summary>
    public class SegmentDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SegmentDefinitionDTO segmentDefinitionDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private SegmentDefinition(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="segmentDefinitionDTO">Parameter of the type SegmentDefinitionDTO</param>
        /// <param name="executionContext">Parameter of the type executionContext</param>
        public SegmentDefinition(ExecutionContext executionContext, SegmentDefinitionDTO segmentDefinitionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(segmentDefinitionDTO, executionContext);
            this.segmentDefinitionDTO = segmentDefinitionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the segmentDefinitionDTO
        /// </summary>
        /// <param name="segmentDefinitionId">segmentDefinitionId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>segmentDefinitionSourceMap DTO</returns>        
        public SegmentDefinition(ExecutionContext executionContext, int segmentDefinitionId, bool loadChildRecords = false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, segmentDefinitionId, loadChildRecords, activeChildRecords, sqlTransaction);
            SegmentDefinitionDataHandler segmentDefinitionDataHandler = new SegmentDefinitionDataHandler(sqlTransaction);
            segmentDefinitionDTO = segmentDefinitionDataHandler.GetSegmentDefinition(segmentDefinitionId);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(segmentDefinitionDTO);
        }

        /// <summary>
        /// Generate SegmentDefinitionSourceMapDTO list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            SegmentDefinitionSourceMapList segmentDefinitionSourceMapList = new SegmentDefinitionSourceMapList(executionContext);
            List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>> searchParameters = new List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>>();
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.IS_ACTIVE, "Y"));
            }
            searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            searchParameters.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_ID, Convert.ToString(segmentDefinitionDTO.SegmentDefinitionId)));
            segmentDefinitionDTO.SegmentDefinitionSourceMapDTOList = segmentDefinitionSourceMapList.GetAllSegmentDefinitionSourceMaps(searchParameters, true, activeChildRecords);
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the segment definition
        /// segment definition will be inserted if SegmentDefinitionId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            SegmentDefinitionDataHandler segmentDefinitionDataHandler = new SegmentDefinitionDataHandler(sqlTransaction);
            if (segmentDefinitionDTO.IsChangedRecursive == false && segmentDefinitionDTO.SegmentDefinitionId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            Validate(sqlTransaction);
            if (segmentDefinitionDTO.SegmentDefinitionId < 0)
            {
                segmentDefinitionDTO = segmentDefinitionDataHandler.InsertSegmentDefinition(segmentDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                segmentDefinitionDTO.AcceptChanges();
            }
            else
            {
                if (segmentDefinitionDTO.IsChanged == true)
                {
                    segmentDefinitionDTO = segmentDefinitionDataHandler.UpdateSegmentDefinition(segmentDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    segmentDefinitionDTO.AcceptChanges();
                }
            }
            SaveChild(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : ScreenZoneDefSetupDTO 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveChild(SqlTransaction sqlTransaction)
        {
            if (segmentDefinitionDTO.SegmentDefinitionSourceMapDTOList != null &&
                segmentDefinitionDTO.SegmentDefinitionSourceMapDTOList.Any())
            {
                List<SegmentDefinitionSourceMapDTO> updatedSegmentDefinitionSourceMapDTOList = new List<SegmentDefinitionSourceMapDTO>();
                foreach (var segmentDefinitionSourceMapDTO in segmentDefinitionDTO.SegmentDefinitionSourceMapDTOList)
                {
                    if (segmentDefinitionSourceMapDTO.SegmentDefinitionId != segmentDefinitionDTO.SegmentDefinitionId)
                    {
                        segmentDefinitionSourceMapDTO.SegmentDefinitionId = segmentDefinitionDTO.SegmentDefinitionId;
                    }
                    updatedSegmentDefinitionSourceMapDTOList.Add(segmentDefinitionSourceMapDTO);
                }
                if (updatedSegmentDefinitionSourceMapDTOList.Any())
                {
                    SegmentDefinitionSourceMapList segmentDefinitionSourceMapList = new SegmentDefinitionSourceMapList(executionContext, updatedSegmentDefinitionSourceMapDTOList);
                    segmentDefinitionSourceMapList.Save(sqlTransaction);
                }
                segmentDefinitionDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Validates the segmentDefinitionDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (string.IsNullOrWhiteSpace(segmentDefinitionDTO.SegmentName))
            {
                log.Error("Please enter segment name");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2607, MessageContainerList.GetMessage(executionContext, "Segment"));
                throw new ValidationException(errorMessage);
            }
            if (string.IsNullOrWhiteSpace(segmentDefinitionDTO.SequenceOrder))
            {
                log.Debug("Please enter the Sequence Order.");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1255, MessageContainerList.GetMessage(executionContext, "SequenceOrder"));
                throw new ValidationException(errorMessage);
            }
            SegmentDefinitionDataHandler segmentDefinitionDataHandler = new SegmentDefinitionDataHandler(sqlTransaction);
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            searchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = segmentDefinitionDataHandler.GetSegmentDefinitionList(searchParams);
            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any())
            {
                if (segmentDefinitionDTOList.Exists(x => x.SegmentName.ToLower() == segmentDefinitionDTO.SegmentName.ToLower()) && segmentDefinitionDTO.SegmentDefinitionId == -1)
                {
                    log.Error("Duplicate entries detail");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Segment"));
                    throw new ValidationException(errorMessage);
                }
                if (segmentDefinitionDTOList.Exists(x => x.SegmentName.ToLower() == segmentDefinitionDTO.SegmentName.ToLower() && x.SegmentDefinitionId != segmentDefinitionDTO.SegmentDefinitionId))
                {
                    log.Debug("Duplicate entries detail");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Segment"));
                    throw new ValidationException(errorMessage);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SegmentDefinitionDTO SegmentDefinitionDTO//added
        {
            get { return segmentDefinitionDTO; }
        }
    }
    /// <summary>
    /// Manages the list of segment definition
    /// </summary>
    public class SegmentDefinitionList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
        private ExecutionContext executionContext;
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public SegmentDefinitionList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor to initialize segmentDefinitionList and executionContext
        /// </summary>
        public SegmentDefinitionList(ExecutionContext executionContext, List<SegmentDefinitionDTO> segmentDefinitionList) : this(executionContext)
        {
            log.LogMethodEntry(segmentDefinitionList, executionContext);
            this.segmentDefinitionDTOList = segmentDefinitionList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the segment definition list
        /// </summary>
        public List<SegmentDefinitionDTO> GetAllSegmentDefinitions(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SegmentDefinitionDataHandler segmentDefinitionDataHandler = new SegmentDefinitionDataHandler(sqlTransaction);
            this.segmentDefinitionDTOList = segmentDefinitionDataHandler.GetSegmentDefinitionList(searchParameters);
            log.LogMethodExit(segmentDefinitionDTOList);
            return segmentDefinitionDTOList;
        }

        /// <summary>
        /// Returns the no of Segment Definition matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetSegmentDefinitionCount(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SegmentDefinitionDataHandler segmentDefinitionDataHandler = new SegmentDefinitionDataHandler(sqlTransaction);
            int count = segmentDefinitionDataHandler.GetSegmentDefinitionCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }


        /// <summary>
        /// Returns the segment definition list
        /// </summary>
        public List<SegmentDefinitionDTO> GetAllSegmentDefinitionsDTOList(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters,
                                                    bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            SegmentDefinitionDataHandler segmentDefinitionDataHandler = new SegmentDefinitionDataHandler(sqlTransaction);
            this.segmentDefinitionDTOList = segmentDefinitionDataHandler.GetSegmentDefinitionList(searchParameters, currentPage, pageSize);
            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any() && loadChildRecords)
            {
                Build(segmentDefinitionDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(segmentDefinitionDTOList);
            return segmentDefinitionDTOList;
        }

        private void Build(List<SegmentDefinitionDTO> segmentDefinitionDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, SegmentDefinitionDTO> segmentDefinitionDTODictionary = new Dictionary<int, SegmentDefinitionDTO>();
            List<int> segmentDefinitionIdList = new List<int>();
            for (int i = 0; i < segmentDefinitionDTOList.Count; i++)
            {
                if (segmentDefinitionDTODictionary.ContainsKey(segmentDefinitionDTOList[i].SegmentDefinitionId))
                {
                    continue;
                }
                segmentDefinitionDTODictionary.Add(segmentDefinitionDTOList[i].SegmentDefinitionId, segmentDefinitionDTOList[i]);
                segmentDefinitionIdList.Add(segmentDefinitionDTOList[i].SegmentDefinitionId);
            }
            SegmentDefinitionSourceMapList segmentDefinitionSourceMapList = new SegmentDefinitionSourceMapList(executionContext);
            List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTOList = segmentDefinitionSourceMapList.GetSegmentDefinitionSourceMapDTOList(segmentDefinitionIdList, true, activeChildRecords, sqlTransaction);

            if (segmentDefinitionSourceMapDTOList != null && segmentDefinitionSourceMapDTOList.Any())
            {
                for (int i = 0; i < segmentDefinitionSourceMapDTOList.Count; i++)
                {
                    if (segmentDefinitionDTODictionary.ContainsKey(segmentDefinitionSourceMapDTOList[i].SegmentDefinitionId) == false)
                    {
                        continue;
                    }
                    SegmentDefinitionDTO segmentDefinitionDTO = segmentDefinitionDTODictionary[segmentDefinitionSourceMapDTOList[i].SegmentDefinitionId];
                    if (segmentDefinitionDTO.SegmentDefinitionSourceMapDTOList == null)
                    {
                        segmentDefinitionDTO.SegmentDefinitionSourceMapDTOList = new List<SegmentDefinitionSourceMapDTO>();
                    }
                    segmentDefinitionDTO.SegmentDefinitionSourceMapDTOList.Add(segmentDefinitionSourceMapDTOList[i]);
                }
            }
        }

        /// <summary>
        /// This method is will return Sheet object for segmentDefinitionDTO.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            SegmentDefinitionDataHandler segmentDefinitionDataHandler = new SegmentDefinitionDataHandler(sqlTransaction);
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> searchParameters = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            searchParameters.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            segmentDefinitionDTOList = segmentDefinitionDataHandler.GetSegmentDefinitionList(searchParameters);

            SegmentExcelDTODefinition segmentExcelDTODefinition = new SegmentExcelDTODefinition(executionContext, "");
            ///Building headers from SegmentDefinitionExcelDTODefinition
            segmentExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any())
            {
                foreach (SegmentDefinitionDTO segmentDefinitionDTO in segmentDefinitionDTOList)
                {
                    segmentExcelDTODefinition.Configure(segmentDefinitionDTO);

                    Row row = new Row();
                    segmentExcelDTODefinition.Serialize(row, segmentDefinitionDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }


        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            SegmentExcelDTODefinition segmentExcelDTODefinition = new SegmentExcelDTODefinition(executionContext, "");
            List<SegmentDefinitionDTO> rowSegmentDefinitionDTOList = new List<SegmentDefinitionDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    SegmentDefinitionDTO rowSegmentDefinitionDTO = (SegmentDefinitionDTO)segmentExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowSegmentDefinitionDTOList.Add(rowSegmentDefinitionDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowSegmentDefinitionDTOList != null && rowSegmentDefinitionDTOList.Any())
                    {
                        SegmentDefinitionList segmentDefinitionsListBL = new SegmentDefinitionList(executionContext, rowSegmentDefinitionDTOList);
                        segmentDefinitionsListBL.Save(sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            log.LogMethodExit(keyValuePairs);
            return keyValuePairs;

        }

        /// <summary>
        /// Save and Updated the segmentDefinitionDTOList
        /// </summary>        
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (segmentDefinitionDTOList == null ||
                segmentDefinitionDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < segmentDefinitionDTOList.Count; i++)
            {
                var segmentDefinitionDTO = segmentDefinitionDTOList[i];
                if (segmentDefinitionDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    SegmentDefinition segmentDefinitionObj = new SegmentDefinition(executionContext, segmentDefinitionDTO);
                    segmentDefinitionObj.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving segmentDefinitionDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("segmentDefinitionDTO", segmentDefinitionDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
