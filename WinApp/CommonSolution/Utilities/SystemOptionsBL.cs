/********************************************************************************************
 * Project Name - SystemOptions BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera      Created 
 *2.70        04-Jul-2019      Girish kundar     Modified : Added execution object to the constructors .
 *                                                        : In Save() method return type for Insert/Update is DTO instead of Id
 *            29-Aug-2019      Mushahid Faizan  Added Save() and Constructors in SystemOptionsList class.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.Utilities
{
    public class SystemOptionsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SystemOptionsDTO systemOptionsDTO;
        private ExecutionContext executionContext;
       
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public SystemOptionsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            systemOptionsDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the optionId parameter
        /// </summary>
        /// <param name="optionId">optionId</param>
        ///<param name="executionContext">executionContext</param>
        /// <param name="sqlTransaction">sqlTransaction</param>   
        public SystemOptionsBL(ExecutionContext executionContext , int optionId , SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext,optionId, sqlTransaction);
            SystemOptionsDataHandler systemOptionsDataHandler = new SystemOptionsDataHandler(sqlTransaction);
            this.systemOptionsDTO = systemOptionsDataHandler.GetSystemOptionsDTO(optionId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="optionType">optionType</param>
        /// <param name="optionName">optionName</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public SystemOptionsBL(ExecutionContext executionContext, string optionType, string optionName, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, optionType, optionName, sqlTransaction);
            SystemOptionsDataHandler systemOptionsDataHandler = new SystemOptionsDataHandler(sqlTransaction);
            List<SystemOptionsDTO> systemOptionsDTOList = new List<SystemOptionsDTO>();
            List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.OPTION_NAME, optionName));
            searchParameters.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.OPTION_TYPE, optionType));
            systemOptionsDTOList = systemOptionsDataHandler.GetSystemOptionsDTOList(searchParameters);
            if(systemOptionsDTOList !=null && systemOptionsDTOList.Count > 0)
            {
                this.systemOptionsDTO = systemOptionsDTOList[0];
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the executionContext and SystemOptionsDTO parameter
        /// </summary>
        /// <param name="systemOptionsDTO">systemOptionsDTO</param>
        /// <param name="executionContext">executionContext</param>
        public SystemOptionsBL(ExecutionContext executionContext, SystemOptionsDTO systemOptionsDTO)
        {
            log.LogMethodEntry(systemOptionsDTO);
            this.executionContext = executionContext;
            this.systemOptionsDTO = systemOptionsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get SystemOptionsDTO Object
        /// </summary>
        public SystemOptionsDTO GetSystemOptionsDTO
        {
            get { return systemOptionsDTO; }
        }

        /// <summary>
        /// Saves the SystemOptionsDTO
        /// Checks if the optionId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            SystemOptionsDataHandler systemOptionsDataHandler = new SystemOptionsDataHandler(sqlTransaction);
            if (systemOptionsDTO.OptionId < 0)
            {
                systemOptionsDTO = systemOptionsDataHandler.InsertSystemOptionsDTO(systemOptionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                systemOptionsDTO.AcceptChanges();
            }
            else
            {
                if (systemOptionsDTO.IsChanged)
                {
                    systemOptionsDTO = systemOptionsDataHandler.UpdateSystemOptionsDTO(systemOptionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    systemOptionsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of SystemOptionsDTO
    /// </summary>
    public class SystemOptionsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<SystemOptionsDTO> systemOptionsDTOList;

        public SystemOptionsList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public SystemOptionsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            this.systemOptionsDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productKeyDTOList"></param>
        public SystemOptionsList(ExecutionContext executionContext, List<SystemOptionsDTO> systemOptionsDTOList)
        {
            log.LogMethodEntry(executionContext, systemOptionsDTOList);
            this.executionContext = executionContext;
            this.systemOptionsDTOList = systemOptionsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the SystemOptionsDTO List 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>SystemOptionsDTO</returns>
        public List<SystemOptionsDTO> GetSystemOptionsDTOList(List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>> searchParameters , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            SystemOptionsDataHandler systemOptionsDataHandler = new SystemOptionsDataHandler(sqlTransaction);
            List<SystemOptionsDTO> systemOptionsDTOList =  systemOptionsDataHandler.GetSystemOptionsDTOList(searchParameters);
            log.LogMethodExit(systemOptionsDTOList);
            return systemOptionsDTOList;
        }
        /// <summary>
        /// This method should be used to Save and Update the Facility Tables details for Web Management Studio.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save()
        {
            log.LogMethodEntry();
            if (systemOptionsDTOList != null && systemOptionsDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (SystemOptionsDTO systemOptionsDTO in systemOptionsDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            SystemOptionsBL systemOptionsBL = new SystemOptionsBL(executionContext, systemOptionsDTO);
                            systemOptionsBL.Save(parafaitDBTrx.SQLTrx);
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
                }
            }
            log.LogMethodExit();
        }

        public DateTime? GetSystemOptionModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            SystemOptionsDataHandler systemOptionsDataHandler = new SystemOptionsDataHandler();
            DateTime? result = systemOptionsDataHandler.GetSystemOptionModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
