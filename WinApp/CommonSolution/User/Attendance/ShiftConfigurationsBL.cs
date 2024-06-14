/********************************************************************************************
 * Project Name - ShiftConfigurations
 * Description  - Business logic file for  ShiftConfigurations
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90.0      06-Jul-2020   Akshay Gulaganji        Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business logic for ShiftConfigurations class
    /// </summary>
    public class ShiftConfigurationsBL
    {
        private ShiftConfigurationsDTO shiftConfigurationsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ShiftConfigurationsBL class
        /// </summary>
        private ShiftConfigurationsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ShiftConfigurationsBL object using the ShiftConfigurationsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="shiftConfigurationsDTO">ShiftConfigurationsDTO object</param>
        public ShiftConfigurationsBL(ExecutionContext executionContext, ShiftConfigurationsDTO shiftConfigurationsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, shiftConfigurationsDTO);
            this.shiftConfigurationsDTO = shiftConfigurationsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ShiftConfigurationsId as the parameter
        /// Would fetch the ShiftConfigurations object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="shiftConfigurationId">id - shiftConfigurationsId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ShiftConfigurationsBL(ExecutionContext executionContext, int shiftConfigurationId, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, shiftConfigurationId, sqlTransaction);
            ShiftConfigurationsDataHandler shiftConfigurationsDataHandler = new ShiftConfigurationsDataHandler(sqlTransaction);
            shiftConfigurationsDTO = shiftConfigurationsDataHandler.GetShiftConfigurationsDTO(shiftConfigurationId);
            if (shiftConfigurationsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ShiftConfigurationsDTO", shiftConfigurationId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ShiftConfigurationsDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (shiftConfigurationsDTO.IsChanged == false && shiftConfigurationsDTO.ShiftConfigurationId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ShiftConfigurationsDataHandler shiftConfigurationsDataHandler = new ShiftConfigurationsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (shiftConfigurationsDTO.ShiftConfigurationId < 0)
            {
                log.LogVariableState("ShiftConfigurationsDTO", shiftConfigurationsDTO);
                shiftConfigurationsDTO = shiftConfigurationsDataHandler.Insert(shiftConfigurationsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                shiftConfigurationsDTO.AcceptChanges();
            }
            else if (shiftConfigurationsDTO.IsChanged)
            {
                log.LogVariableState("ShiftConfigurationsDTO", shiftConfigurationsDTO);
                shiftConfigurationsDTO = shiftConfigurationsDataHandler.Update(shiftConfigurationsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                shiftConfigurationsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the ShiftConfigurationsDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            // Required validations to be added here
            if (string.IsNullOrEmpty(shiftConfigurationsDTO.ShiftConfigurationName))
            {
                validationError = new ValidationError("ShiftConfigurations", "ShiftConfigurationName", MessageContainerList.GetMessage(executionContext, 1144, "Shift Configuration Name"));
                validationErrorList.Add(validationError);
            }
            if (shiftConfigurationsDTO.ShiftMinutes <= 0 && (!shiftConfigurationsDTO.WeeklyShiftMinutes.HasValue || shiftConfigurationsDTO.WeeklyShiftMinutes.Value <= 0))
            {
                validationError = new ValidationError("ShiftConfigurations", "ShiftMinutes", MessageContainerList.GetMessage(executionContext, 1144, "Shift Minutes"));
                validationErrorList.Add(validationError);
            }
            else if (!shiftConfigurationsDTO.WeeklyShiftMinutes.HasValue && shiftConfigurationsDTO.WeeklyShiftMinutes.Value <= 0)
            {
                validationError = new ValidationError("ShiftConfigurations", "WeeklyShiftMinutes", MessageContainerList.GetMessage(executionContext, 1144, "Weekly Shift Minutes"));
                validationErrorList.Add(validationError);
            }
            if (shiftConfigurationsDTO.OvertimeAllowed)
            {
                if (!shiftConfigurationsDTO.MaximumOvertimeMinutes.HasValue || shiftConfigurationsDTO.MaximumOvertimeMinutes.Value <= 0)
                {
                    validationError = new ValidationError("ShiftConfigurations", "MaximumOvertimeMinutes", MessageContainerList.GetMessage(executionContext, 1144, "Maximum Overtime Minutes"));
                    validationErrorList.Add(validationError);
                }
                else if (!shiftConfigurationsDTO.MaximumWeeklyOvertimeMinutes.HasValue || shiftConfigurationsDTO.MaximumWeeklyOvertimeMinutes.Value <= 0)
                {
                    validationError = new ValidationError("ShiftConfigurations", "MaximumWeeklyOvertimeMinutes", MessageContainerList.GetMessage(executionContext, 1144, "Maximum Weekly Overtime Minutes"));
                    validationErrorList.Add(validationError);
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ShiftConfigurationsDTO ShiftConfigurationsDTO
        {
            get
            {
                return shiftConfigurationsDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of ShiftConfigurations
    /// </summary>
    public class ShiftConfigurationsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<ShiftConfigurationsDTO> shiftConfigurationsDTOList = new List<ShiftConfigurationsDTO>();

        /// <summary>
        /// Parameterized constructor for ShiftConfigurationsListBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ShiftConfigurationsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for ShiftConfigurationsListBL
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="shiftConfigurationsDTOList">ShiftConfigurationsDTOList passed as a parameter</param>
        public ShiftConfigurationsListBL(ExecutionContext executionContext, List<ShiftConfigurationsDTO> shiftConfigurationsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, shiftConfigurationsDTOList);
            this.shiftConfigurationsDTOList = shiftConfigurationsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ShiftConfigurationsDTOList based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the ShiftConfigurationsDTO List</returns>
        public List<ShiftConfigurationsDTO> GetShiftConfigurationsDTOList(List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ShiftConfigurationsDataHandler shiftConfigurationsDTODataHandler = new ShiftConfigurationsDataHandler(sqlTransaction);
            List<ShiftConfigurationsDTO> shiftConfigurationsDTODTOList = shiftConfigurationsDTODataHandler.GetShiftConfigurationsDTOList(searchParameters);
            log.LogMethodExit(shiftConfigurationsDTODTOList);
            return shiftConfigurationsDTODTOList;
        }

        /// <summary>
        /// Saves the ShiftConfigurationsDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ShiftConfigurationsDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ShiftConfigurationsDTO> shiftConfigurationsDTOLists = new List<ShiftConfigurationsDTO>();
            if (shiftConfigurationsDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ShiftConfigurationsDTO shiftConfigurationsDTO in shiftConfigurationsDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ShiftConfigurationsBL shiftConfigurationsBL = new ShiftConfigurationsBL(executionContext, shiftConfigurationsDTO);
                            shiftConfigurationsBL.Save(sqlTransaction);
                            shiftConfigurationsDTOLists.Add(shiftConfigurationsBL.ShiftConfigurationsDTO);
                            parafaitDBTrx.EndTransaction();

                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();
                }
            }
            return shiftConfigurationsDTOLists;
        }
    }
}
