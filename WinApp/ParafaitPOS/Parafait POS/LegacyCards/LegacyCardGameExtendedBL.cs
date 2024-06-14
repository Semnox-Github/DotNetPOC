/*/********************************************************************************************
 * Project Name - LegacyCardGameExtendedBL
 * Description  - BL class for LegacyCardGameExtended
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     18-Feb-2022    Dakshakh                Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    public class LegacyCardGameExtendedBL
    {
            private LegacyCardGameExtendedDTO legacyCardGameExtendedDTO;
            private readonly ExecutionContext executionContext;
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            /// <summary>
            /// Parameterized constructor of LegacyCardGameExtendedBL class
            /// </summary>
            private LegacyCardGameExtendedBL(ExecutionContext executionContext)
            {
                log.LogMethodEntry(executionContext);
                this.executionContext = executionContext;
                log.LogMethodExit();
            }

            /// <summary>
            /// Constructor with the accountGameExtended id as the parameter
            /// Would fetch the accountGameExtended object from the database based on the id passed. 
            /// </summary>
            /// <param name="executionContext">execution context</param>
            /// <param name="id">Id</param>
            /// <param name="sqlTransaction">Optional sql transaction</param>
            public LegacyCardGameExtendedBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
                : this(executionContext)
            {
                log.LogMethodEntry(executionContext, id, sqlTransaction);
                LegacyCardGameExtendedDataHandler legacyCardGameExtendedDataHandler = new LegacyCardGameExtendedDataHandler(sqlTransaction);
                legacyCardGameExtendedDTO = legacyCardGameExtendedDataHandler.GetLegacyCardGameExtendedDTO(id);
                if (legacyCardGameExtendedDTO == null)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountGameExtended", id);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new EntityNotFoundException(message);
                }
                log.LogMethodExit();
            }

            /// <summary>
            /// Creates LegacyCardGameExtendedBL object using the legacyCardGameExtendedDTO
            /// </summary>
            /// <param name="executionContext">execution context</param>
            /// <param name="LegacyCardGameExtendedDTO">LegacyCardGameExtendedDTO object</param>
            public LegacyCardGameExtendedBL(ExecutionContext executionContext, LegacyCardGameExtendedDTO legacyCardGameExtendedDTO)
                : this(executionContext)
            {
                log.LogMethodEntry(executionContext, legacyCardGameExtendedDTO);
                this.legacyCardGameExtendedDTO = legacyCardGameExtendedDTO;
                log.LogMethodExit();
            }

            /// <summary>
            /// Saves the AccountGameExtended
            /// Checks if the  id is not less than or equal to 0
            /// If it is less than or equal to 0, then inserts
            /// else updates
            /// </summary>
            internal void Save(SqlTransaction sqlTransaction)
            {
                log.LogMethodEntry(sqlTransaction);
                LegacyCardGameExtendedDataHandler legacyCardGameExtendedDataHandler = new LegacyCardGameExtendedDataHandler(sqlTransaction);
                if (legacyCardGameExtendedDTO.IsChanged)
                {
                    if (legacyCardGameExtendedDTO.IsActive)
                    {
                        if (legacyCardGameExtendedDTO.LegacyCardGameExtendedId < 0)
                        {
                            legacyCardGameExtendedDTO = legacyCardGameExtendedDataHandler.InsertLegacyCardGameExtended(legacyCardGameExtendedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                            legacyCardGameExtendedDTO.AcceptChanges();
                        }
                        else
                        {
                            if (legacyCardGameExtendedDTO.IsChanged)
                            {
                                legacyCardGameExtendedDTO = legacyCardGameExtendedDataHandler.UpdateLegacyCardGameExtended(legacyCardGameExtendedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                legacyCardGameExtendedDTO.AcceptChanges();
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }

            /// <summary>
            /// Gets the DTO
            /// </summary>
            public LegacyCardGameExtendedDTO LegacyCardGameExtendedDTO
            {
                get
                {
                    return legacyCardGameExtendedDTO;
                }
            }

            /// <summary>
            /// Validates the customer relationship DTO
            /// </summary>
            public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(sqlTransaction);
                List<ValidationError> validationErrorList = new List<ValidationError>();
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
        }

        /// <summary>
        /// Manages the list of AccountGameExtended
        /// </summary>
        public class AccountGameExtendedListBL
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            private readonly ExecutionContext executionContext;

            /// <summary>
            /// Parameterized constructor
            /// </summary>
            /// <param name="executionContext">execution context</param>
            public AccountGameExtendedListBL(ExecutionContext executionContext)
            {
                log.LogMethodEntry(executionContext);
                this.executionContext = executionContext;
                log.LogMethodExit();
            }
            /// <summary>
            /// Returns the AccountGameExtended list
            /// </summary>
            public List<LegacyCardGameExtendedDTO> GetLegacyCardGameExtendedDTOList(List<KeyValuePair<LegacyCardGameExtendedDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(searchParameters);
                LegacyCardGameExtendedDataHandler legacyCardGameExtendedDataHandler = new LegacyCardGameExtendedDataHandler(sqlTransaction);
                List<LegacyCardGameExtendedDTO> returnValue = legacyCardGameExtendedDataHandler.GetLegacyCardGameExtendedDTOList(searchParameters);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
        }
    }
