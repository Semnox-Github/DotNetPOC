/********************************************************************************************
 * Project Name - ScreenTransitions BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017      Lakshminarayana     Created 
 *2.40        28-Sep-2018      Jagan Mohan         Added new constructor ScreenTransitionsBL, ScreenTransitionsListBL                  
 *2.70.2        31-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.80       04-Apr-2020     Girish Kundar   Modified : Merge code from WMS for Cobra 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Business logic for ScreenTransitions class.
    /// </summary>
    public class ScreenTransitionsBL
    {
        private ScreenTransitionsDTO screenTransitionsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Default constructor of ScreenTransitionsBL class
        /// </summary>
        private  ScreenTransitionsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.screenTransitionsDTO = null;
            this.executionContext =executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the screenTransitions id as the parameter
        /// Would fetch the screenTransitions object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScreenTransitionsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ScreenTransitionsDataHandler screenTransitionsDataHandler = new ScreenTransitionsDataHandler(sqlTransaction);
            this.screenTransitionsDTO = screenTransitionsDataHandler.GetScreenTransitionsDTO(id);
            if (screenTransitionsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "screen Transitions", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(screenTransitionsDTO);
        }

        /// <summary>
        /// Creates ScreenTransitionsBL object using the ScreenTransitionsDTO
        /// </summary>
        /// <param name="screenTransitionsDTO">ScreenTransitionsDTO object</param>
        /// <param name="executionContext">executionContext</param>
        public ScreenTransitionsBL( ExecutionContext executionContext, ScreenTransitionsDTO screenTransitionsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(screenTransitionsDTO,executionContext);
            this.screenTransitionsDTO = screenTransitionsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ScreenTransitions
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);        
            ScreenTransitionsDataHandler screenTransitionsDataHandler = new ScreenTransitionsDataHandler(sqlTransaction);
            if(screenTransitionsDTO.Id < 0)
            {
                screenTransitionsDTO = screenTransitionsDataHandler.InsertScreenTransitions(screenTransitionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                screenTransitionsDTO.AcceptChanges();
            }
            else
            {
                if(screenTransitionsDTO.IsChanged)
                {
                    screenTransitionsDTO = screenTransitionsDataHandler.UpdateScreenTransitions(screenTransitionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    screenTransitionsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the ScreenTransitionsDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ScreenTransitionsDTO ScreenTransitionsDTO
        {
            get
            {
                return screenTransitionsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ScreenTransitions
    /// </summary>
    public class ScreenTransitionsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);       
        private ExecutionContext executionContext;
        private List<ScreenTransitionsDTO> screenTransitionsDTOList = new List<ScreenTransitionsDTO>();


        /// <summary>
        /// Default constructor of ScreenTransitionsListBL class
        /// </summary>
        public ScreenTransitionsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="screenTransitionsDTOList">screenTransitionsDTOList</param>
        public ScreenTransitionsListBL(ExecutionContext executionContext, List<ScreenTransitionsDTO> screenTransitionsDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, screenTransitionsDTOList);
            this.screenTransitionsDTOList = screenTransitionsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ScreenTransitions list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>screenTransitionTo List</returns>
        public List<ScreenTransitionsDTO> GetScreenTransitionsDTOList(List<KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ScreenTransitionsDataHandler screenTransitionsDataHandler = new ScreenTransitionsDataHandler(sqlTransaction);
            List<ScreenTransitionsDTO>  screenTransitionToList = screenTransitionsDataHandler.GetScreenTransitionsDTOList(searchParameters);
            log.LogMethodExit(screenTransitionToList);
            return screenTransitionToList;
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the ScreenTransitions List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (screenTransitionsDTOList == null ||
                screenTransitionsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < screenTransitionsDTOList.Count; i++)
            {
                var screenTransitionsDTO = screenTransitionsDTOList[i];
                if (screenTransitionsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ScreenTransitionsBL screenTransitionsBL = new ScreenTransitionsBL(executionContext, screenTransitionsDTO);
                    screenTransitionsBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving ScreenTransitionsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ScreenTransitionsDTO", screenTransitionsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
