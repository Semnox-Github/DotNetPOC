/********************************************************************************************
 * Project Name - Segment categorization
 * Description  - Bussiness logic of segment categorization
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Apr-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class SegmentCategorization
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SegmentCategorizationDTO segmentCategorizationDTO;
        private ExecutionContext excutionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private SegmentCategorization(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.excutionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the SegmentCategorization id as the parameter
        /// Would fetch the SegmentCategorization object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public SegmentCategorization(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            SegmentCategorizationDataHandler segmentCategorizationDataHandler = new SegmentCategorizationDataHandler(sqlTransaction);
            segmentCategorizationDTO = segmentCategorizationDataHandler.GetSegmentCategorization(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="segmentCategorizationDTO">Parameter of the type SegmentCategorizationDTO</param>
        public SegmentCategorization(ExecutionContext executionContext, SegmentCategorizationDTO segmentCategorizationDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(excutionContext, segmentCategorizationDTO);
            this.segmentCategorizationDTO = segmentCategorizationDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the segment categorization
        /// segment categorization will be inserted if SegmentCategoryId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (segmentCategorizationDTO.IsChanged == false
             && segmentCategorizationDTO.SegmentCategoryId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            SegmentCategorizationDataHandler segmentCategorizationDataHandler = new SegmentCategorizationDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(excutionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (segmentCategorizationDTO.SegmentCategoryId < 0)
            {
                int SegmentCategoryId = segmentCategorizationDataHandler.InsertSegmentCategorization(segmentCategorizationDTO, excutionContext.GetUserId(), excutionContext.GetSiteId());
                segmentCategorizationDTO.SegmentCategoryId = SegmentCategoryId;
                segmentCategorizationDTO.AcceptChanges();
            }
            else
            {
                if (segmentCategorizationDTO.IsChanged == true)
                {
                    segmentCategorizationDataHandler.UpdateSegmentCategorization(segmentCategorizationDTO, excutionContext.GetUserId(), excutionContext.GetSiteId());
                    segmentCategorizationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the SegmentCategorizationDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SegmentCategorizationDTO SegmentCategorizationDTO { get { return segmentCategorizationDTO; } }
    }

    /// <summary>
    /// Manages the list of segment categorization
    /// </summary>
    public class SegmentCategorizationList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public SegmentCategorizationList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the segment categorization list
        /// </summary>
        public List<SegmentCategorizationDTO> GetAllSegmentCategorizations(List<KeyValuePair<SegmentCategorizationDTO.SearchBySegmentCategorizationParameters, string>> searchParameters,
             SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SegmentCategorizationDataHandler segmentCategorizationDataHandler = new SegmentCategorizationDataHandler(sqlTransaction);
            List<SegmentCategorizationDTO> segmentCategorizationDTOList = segmentCategorizationDataHandler.GetSegmentCategorizationList(searchParameters);
            log.LogMethodExit(segmentCategorizationDTOList);
            return segmentCategorizationDTOList;
        }

        /// <summary>
        /// Returns results for the passed the segment categorization list
        /// </summary>
        public List<SegmentCategorizationDTO> GetSegmentCategorizationList(string sqlQuery)
        {
            log.LogMethodEntry(sqlQuery);
            SegmentCategorizationDataHandler segmentCategorizationDataHandler = new SegmentCategorizationDataHandler();
            List<SegmentCategorizationDTO> segmentCategorizationDTOList = segmentCategorizationDataHandler.GetSegmentCategorizationList(sqlQuery);
            log.LogMethodExit(segmentCategorizationDTOList);
            return segmentCategorizationDTOList;
        }        
    }
}
