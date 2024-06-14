/********************************************************************************************
 * Project Name - Segment definition source value
 * Description  - Bussiness logic of segment definition source value
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Apr-2016   Raghuveera        Created
 *2.50        21-Jan-2019   Jagan Mohana      Created constructor SegmentDefinitionSourceValueList and 
 *                                            add new method SaveUpdateSegmentDefinationValueList
 *2.70        17-Mar-2019   Manoj Durgam      Added ExecutionContext to the constructor
 *2.110.0     15-Oct-2020   Mushahid Faizan   Modified : Constructor, Save() method, Added Validate,
 *                                                 List class changes as per 3 tier standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Segment definition source value will creates and modifies the segment definition source value
    /// </summary>
    public class SegmentDefinitionSourceValue
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDTO;
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private SegmentDefinitionSourceValue(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="segmentDefinitionSourceValueDTO">Parameter of the type SegmentDefinitionSourceValueDTO</param>
        /// <param name="executionContext">Parameter of the type executionContext</param>
        public SegmentDefinitionSourceValue(ExecutionContext executionContext, SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDTO) : this(executionContext)
        {
            log.LogMethodEntry(segmentDefinitionSourceValueDTO);
            this.segmentDefinitionSourceValueDTO = segmentDefinitionSourceValueDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the segmentDefinitionSourceValueDTO
        /// </summary>
        /// <param name="segmentDefinitionSourceValueId">segmentDefinitionSourceValueId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>segmentDefinitionSourceValueDTO DTO</returns>        
        public SegmentDefinitionSourceValue(ExecutionContext executionContext, int segmentDefinitionSourceValueId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, segmentDefinitionSourceValueId, sqlTransaction);
            SegmentDefinitionSourceValueDataHandler segmentDefinitionSourceValueDataHandler = new SegmentDefinitionSourceValueDataHandler(sqlTransaction);
            segmentDefinitionSourceValueDTO = segmentDefinitionSourceValueDataHandler.GetSegmentDefinitionSourceValue(segmentDefinitionSourceValueId);
            log.LogMethodExit(segmentDefinitionSourceValueDTO);
        }

        /// <summary>
        /// Saves the segment definition
        /// segment definition source value will be inserted if SegmentDefinitionId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry();
            SegmentDefinitionSourceValueDataHandler segmentDefinitionSourceValueDataHandler = new SegmentDefinitionSourceValueDataHandler(sqlTransaction);
            if (segmentDefinitionSourceValueDTO.IsChanged == false && segmentDefinitionSourceValueDTO.SegmentDefinitionSourceValueId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            Validate(sqlTransaction);
            if (segmentDefinitionSourceValueDTO.SegmentDefinitionSourceValueId < 0)
            {
                segmentDefinitionSourceValueDTO = segmentDefinitionSourceValueDataHandler.InsertSegmentDefinitionSourceValue(segmentDefinitionSourceValueDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                segmentDefinitionSourceValueDTO.AcceptChanges();
            }
            else
            {
                if (segmentDefinitionSourceValueDTO.IsChanged == true)
                {
                    segmentDefinitionSourceValueDTO = segmentDefinitionSourceValueDataHandler.UpdateSegmentDefinitionSourceValue(segmentDefinitionSourceValueDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    segmentDefinitionSourceValueDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the segmentDefinitionSourceValueDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            // Validation Logic here.
            log.LogMethodExit();
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SegmentDefinitionSourceValueDTO SegmentDefinitionSourceValueDTO
        {
            get { return segmentDefinitionSourceValueDTO; }
        }
    }

    /// <summary>
    /// Manages the list of segment definition source value
    /// </summary>
    public class SegmentDefinitionSourceValueList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
        private ExecutionContext executionContext;
     
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public SegmentDefinitionSourceValueList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="segmentDefinitionSourceValues"></param>
        /// <param name="executionContext"></param>
        public SegmentDefinitionSourceValueList(ExecutionContext executionContext, List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValues) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, segmentDefinitionSourceValueDTOList);
            this.segmentDefinitionSourceValueDTOList = segmentDefinitionSourceValues;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the segment definition source value list
        /// </summary>
        public List<SegmentDefinitionSourceValueDTO> GetAllSegmentDefinitionSourceValues(List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> searchParameters,
                                                                                         SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SegmentDefinitionSourceValueDataHandler segmentDefinitionSourceValueDataHandler = new SegmentDefinitionSourceValueDataHandler(sqlTransaction);
            List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList = segmentDefinitionSourceValueDataHandler.GetSegmentDefinitionSourceValueList(searchParameters);
            log.LogMethodExit(segmentDefinitionSourceValueDTOList);
            return segmentDefinitionSourceValueDTOList;
        }

        /// <summary>
        /// Gets the segmentDefinitionSourceValueDTO List for segmentDefinitionSourceIdList
        /// </summary>
        /// <param name="segmentDefinitionSourceIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of segmentDefinitionSourceValueDTO</returns>
        public List<SegmentDefinitionSourceValueDTO> GetSegmentDefinitionSourceValueList(List<int> segmentDefinitionSourceIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(segmentDefinitionSourceIdList, activeRecords);
            SegmentDefinitionSourceValueDataHandler segmentDefinitionSourceValueDataHandler = new SegmentDefinitionSourceValueDataHandler();
            this.segmentDefinitionSourceValueDTOList = segmentDefinitionSourceValueDataHandler.GetSegmentDefinitionSourceValueList(segmentDefinitionSourceIdList, activeRecords, sqlTransaction);
            log.LogMethodExit(segmentDefinitionSourceValueDTOList);
            return segmentDefinitionSourceValueDTOList;
        }

        /// <summary>
        /// Saves the segmentDefinitionSourceValueDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (segmentDefinitionSourceValueDTOList == null ||
                segmentDefinitionSourceValueDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < segmentDefinitionSourceValueDTOList.Count; i++)
            {
                var segmentDefinitionSourceValueDTO = segmentDefinitionSourceValueDTOList[i];
                if (segmentDefinitionSourceValueDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    SegmentDefinitionSourceValue segmentDefinitionSourceValueObj = new SegmentDefinitionSourceValue(executionContext, segmentDefinitionSourceValueDTO);
                    segmentDefinitionSourceValueObj.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving segmentDefinitionSourceValueDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("segmentDefinitionSourceValueDTO", segmentDefinitionSourceValueDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
