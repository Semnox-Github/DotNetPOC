/********************************************************************************************
 * Project Name - CustomAttributeValueList BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-May-2017      Lakshminarayana     Created 
 *2.70.2        25-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *            02-Aug-2019      Mushahid Faizan     Added delete in Save() method.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Business logic for CustomAttributeValueList class.
    /// </summary>
    public class CustomAttributeValueListBL
    {
        private CustomAttributeValueListDTO customAttributeValueListDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CustomAttributeValueListBL class
        /// </summary>
        public CustomAttributeValueListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            customAttributeValueListDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customAttributeValueList id as the parameter
        /// Would fetch the customAttributeValueList object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomAttributeValueListBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CustomAttributeValueListDataHandler customAttributeValueListDataHandler = new CustomAttributeValueListDataHandler(sqlTransaction);
            customAttributeValueListDTO = customAttributeValueListDataHandler.GetCustomAttributeValueListDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomAttributeValueListBL object using the CustomAttributeValueListDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customAttributeValueListDTO">CustomAttributeValueListDTO object</param>
        public CustomAttributeValueListBL(ExecutionContext executionContext, CustomAttributeValueListDTO customAttributeValueListDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customAttributeValueListDTO);
            this.customAttributeValueListDTO = customAttributeValueListDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Saves the CustomAttributeValueList
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomAttributeValueListDataHandler customAttributeValueListDataHandler = new CustomAttributeValueListDataHandler(sqlTransaction);
            if (customAttributeValueListDTO.IsActive)
            {
                if (customAttributeValueListDTO.ValueId < 0)
                {
                    customAttributeValueListDTO = customAttributeValueListDataHandler.InsertCustomAttributeValueList(customAttributeValueListDTO, executionContext.GetUserId(), executionContext.GetSiteId());                    
                    customAttributeValueListDTO.AcceptChanges();
                }
                else
                {
                    if (customAttributeValueListDTO.IsChanged)
                    {
                        customAttributeValueListDTO = customAttributeValueListDataHandler.UpdateCustomAttributeValueList(customAttributeValueListDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        customAttributeValueListDTO.AcceptChanges();
                    }
                }
            }
            else  // Hard Delete
            {
                if (customAttributeValueListDTO.ValueId >= 0)
                {
                    customAttributeValueListDataHandler.Delete(customAttributeValueListDTO.ValueId);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomAttributeValueListDTO CustomAttributeValueListDTO
        {
            get
            {
                return customAttributeValueListDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of CustomAttributeValueList
    /// </summary>
    public class CustomAttributeValueListListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;

        public CustomAttributeValueListListBL()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomAttributeValueListListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CustomAttributeValueList list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<CustomAttributeValueListDTO> GetCustomAttributeValueListDTOList(List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CustomAttributeValueListDataHandler customAttributeValueListDataHandler = new CustomAttributeValueListDataHandler(sqlTransaction);
            List<CustomAttributeValueListDTO> returnValue = customAttributeValueListDataHandler.GetCustomAttributeValueListDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
