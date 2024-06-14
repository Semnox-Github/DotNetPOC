/********************************************************************************************
 * Project Name - Utilities
 * Description  - Business logic file for ParafaitExecutableVersionNumber
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2.0      23-Sep-2019   Mithesh                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;


namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Business logic for ParafaitExecutableVersionNumber class.
    /// </summary>
    public class ParafaitExecutableVersionNumberBL
    {
        private ParafaitExecutableVersionNumberDTO parafaitExecutableVersionNumberDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ParafaitExecutableVersionNumberBL class
        /// </summary>
        public ParafaitExecutableVersionNumberBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ParafaitExecutableVersionNumberBL object using the ParafaitExecutableVersionNumberDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="parafaitExecutableVersionNumberDTO">ParafaitExecutableVersionNumberDTO object</param>
        public ParafaitExecutableVersionNumberBL(ExecutionContext executionContext, ParafaitExecutableVersionNumberDTO parafaitExecutableVersionNumberDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parafaitExecutableVersionNumberDTO);
            this.parafaitExecutableVersionNumberDTO = parafaitExecutableVersionNumberDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ParafaitExecutableVersionNumberBL object using the ParafaitExecutableVersionNumberDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="parafaitExecutableVersionNumberDTO">ParafaitExecutableVersionNumberDTO object</param>
        public ParafaitExecutableVersionNumberBL(string appName, ExecutionContext executionContext, SqlTransaction sqlTransaction=null)
         : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parafaitExecutableVersionNumberDTO);
            ParafaitExecutableVersionNumberDataHandler parafaitExecutableVersionNumberDataHandler = new ParafaitExecutableVersionNumberDataHandler(sqlTransaction);
            parafaitExecutableVersionNumberDTO = parafaitExecutableVersionNumberDataHandler.GetParafaitExecutableVersionNumber(appName);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ParafaitExecutableVersionNumberDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ParafaitExecutableVersionNumberDataHandler ParafaitExecutableVersionNumberDataHandler = new ParafaitExecutableVersionNumberDataHandler(sqlTransaction);
            if (parafaitExecutableVersionNumberDTO.Id < 0)
            {
                parafaitExecutableVersionNumberDTO = ParafaitExecutableVersionNumberDataHandler.Insert(parafaitExecutableVersionNumberDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                parafaitExecutableVersionNumberDTO.AcceptChanges();
            }
            else if (parafaitExecutableVersionNumberDTO.IsChanged)
            {
                parafaitExecutableVersionNumberDTO = ParafaitExecutableVersionNumberDataHandler.Update(parafaitExecutableVersionNumberDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                parafaitExecutableVersionNumberDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ParafaitExecutableVersionNumberDTO if present
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public ParafaitExecutableVersionNumberDTO GetParafaitExecutableVersionNumberDTO(string appName,
                                                         SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appName, sqlTransaction);
            ParafaitExecutableVersionNumberDataHandler parafaitExecutableVersionNumberDataHandler = new ParafaitExecutableVersionNumberDataHandler(sqlTransaction);
            parafaitExecutableVersionNumberDTO = parafaitExecutableVersionNumberDataHandler.GetParafaitExecutableVersionNumber(appName);
            log.LogMethodExit(parafaitExecutableVersionNumberDTO);
            return parafaitExecutableVersionNumberDTO;
        }


        /// <summary>
        /// Returns the ParafaitExecutableVersionNumberDTO
        /// </summary>
        public ParafaitExecutableVersionNumberDTO getParafaitExecutableVersionNumberDTO
        {
            get
            {
                return parafaitExecutableVersionNumberDTO;
            }
        }

        /// <summary>
        /// Compares the app version against the version stored in DB
        /// </summary>
        /// <param name="evnDTO"></param>
        /// <param name="appVersion"></param>
        /// <returns></returns>
        public int CompareExecutableVersions(int[] appVersion)
        {
            int dbMajorVersion = parafaitExecutableVersionNumberDTO.MajorVersion;
            int dbMinorVersion = parafaitExecutableVersionNumberDTO.MinorVersion;
            int dbPatchVersion = parafaitExecutableVersionNumberDTO.PatchVersion;

            int appMajorVersion = appVersion[0];
            int appMinorVersion = appVersion[1];
            int appPatchVersion = appVersion[2];

            // DB Version and App version are same. Return 0
            if(dbMajorVersion == appMajorVersion && dbMinorVersion == appMinorVersion && dbPatchVersion == appPatchVersion)
            {
                return 0;
            }


            // Check if DB version is higher or lower than App version
            if ( appMajorVersion < dbMajorVersion || appMinorVersion < dbMinorVersion || appPatchVersion < dbPatchVersion)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

    }

    public class ParafaitExecutableVersionNumberListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ParafaitExecutableVersionNumberDTO> parafaitExecutableVersionNumberDTOList = new List<ParafaitExecutableVersionNumberDTO>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ParafaitExecutableVersionNumberListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="checkInDTOList"></param>
        public ParafaitExecutableVersionNumberListBL(ExecutionContext executionContext,
                                                List<ParafaitExecutableVersionNumberDTO> parafaitExecutableVersionNumberDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parafaitExecutableVersionNumberDTOList);
            this.parafaitExecutableVersionNumberDTOList = parafaitExecutableVersionNumberDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the GetParafaitExecutableVersionNumberDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>parafaitExecutableVersionNumberDTOList</returns>
        public List<ParafaitExecutableVersionNumberDTO> GetParafaitExecutableVersionNumberDTOList(List<KeyValuePair<ParafaitExecutableVersionNumberDTO.SearchByParameters, string>> searchParameters,
                                                         SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ParafaitExecutableVersionNumberDataHandler parafaitExecutableVersionNumberDataHandler = new ParafaitExecutableVersionNumberDataHandler(sqlTransaction);
            List<ParafaitExecutableVersionNumberDTO> parafaitExecutableVersionNumberDTOList = parafaitExecutableVersionNumberDataHandler.GetAllParafaitExecutableVersionNumberDTOList(searchParameters);
            log.LogMethodExit(parafaitExecutableVersionNumberDTOList);
            return parafaitExecutableVersionNumberDTOList;
        }

        

        /// <summary>
        /// Saves the  List of CheckInDTO objects
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (parafaitExecutableVersionNumberDTOList == null ||
                parafaitExecutableVersionNumberDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < parafaitExecutableVersionNumberDTOList.Count; i++)
            {
                var parafaitExecutableVersionNumberDTO   = parafaitExecutableVersionNumberDTOList[i];
                try
                {
                    ParafaitExecutableVersionNumberBL parafaitExecutableVersionNumberBL = new ParafaitExecutableVersionNumberBL(executionContext, parafaitExecutableVersionNumberDTO);
                    parafaitExecutableVersionNumberBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving ParafaitExecutableVersionNumberDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ParafaitExecutableVersionNumberDTO", parafaitExecutableVersionNumberDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
