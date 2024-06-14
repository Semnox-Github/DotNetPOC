/********************************************************************************************
 * Project Name - Site
 * Description  - Bussiness logic of CentralCompany
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.120.1     26-Apr-2021   Deeksha               Created as part of AWS Job Scheduler enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Site
{
    public class CentralCompanyBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CentralCompanyDTO centralCompanyDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CentralCompanyBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="CentralCompanyDTO"></param>
        public CentralCompanyBL(ExecutionContext executionContext, CentralCompanyDTO centralCompanyDTO)
        {
            log.LogMethodEntry(executionContext, centralCompanyDTO);
            this.executionContext = executionContext;
            this.centralCompanyDTO = centralCompanyDTO;
            log.LogMethodExit();
        }
        
        ///// <summary>
        ///// Saves the CentralCompany
        ///// Checks if the CentralCompany id is not less than or equal to 0
        /////     If it is less than or equal to 0, then inserts
        /////     else updates
        ///// </summary>
        //public void Save(SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(sqlTransaction);
        //    CentralCompanyDataHandler CentralCompanyDataHandler = new CentralCompanyDataHandler(sqlTransaction);
        //    if (CentralCompanyDTO.CentralCompanyId < 0)
        //    {
        //        int CentralCompanyId = CentralCompanyDataHandler.InsertCentralCompany(CentralCompanyDTO, executionContext.GetUserId());
        //        CentralCompanyDTO.CentralCompanyId = CentralCompanyId;
        //    }
        //    else
        //    {
        //        if (CentralCompanyDTO.IsChanged)
        //        {
        //            CentralCompanyDataHandler.UpdateCentralCompany(CentralCompanyDTO, executionContext.GetUserId());
        //            CentralCompanyDTO.AcceptChanges();
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// get CentralCompanyDTO Object
        /// </summary>
        public CentralCompanyDTO GetCentralCompanyDTO
        {
            get { return centralCompanyDTO; }
        }

    }
    /// <summary>
    /// Manages the list of CentralCompany
    /// </summary>
    public class CentralCompanyList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CentralCompanyDTO> centralCompanyDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CentralCompanyList()
        {
            log.LogMethodEntry(executionContext);
            this.centralCompanyDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="CentralCompanyDTOList"></param>
        /// <param name="executionContext"></param>
        public CentralCompanyList(ExecutionContext executionContext, List<CentralCompanyDTO> centralCompanyDTOList)
        {
            log.LogMethodEntry(executionContext, centralCompanyDTOList);
            this.executionContext = executionContext;
            this.centralCompanyDTOList = centralCompanyDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CentralCompany list with organizations and Organization Structure
        /// </summary>
        public List<CentralCompanyDTO> GetAllCompanies(List<KeyValuePair<CentralCompanyDTO.SearchByParameters, string>> searchParameters , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CentralCompanyDataHandler centralCompanyDataHandler = new CentralCompanyDataHandler(sqlTransaction);
            List<CentralCompanyDTO> centralCompanyDTOsList = centralCompanyDataHandler.GetCentralCompanyDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(centralCompanyDTOsList);
            return centralCompanyDTOsList;
        }

        /// <summary>
        /// Returns the CentralCompany list with organizations and Organization Structure
        /// </summary>
        public List<CentralCompanyDTO> GetAllCompanies(List<KeyValuePair<CentralCompanyDTO.SearchByParameters, string>> searchParameters, string connectionString = null)
        {
            log.LogMethodEntry(searchParameters);
            CentralCompanyDataHandler centralCompanyDataHandler = new CentralCompanyDataHandler(connectionString);
            List<CentralCompanyDTO> centralCompanyDTOsList = centralCompanyDataHandler.GetCentralCompanyDTOList(searchParameters);
            log.LogMethodExit(centralCompanyDTOsList);
            return centralCompanyDTOsList;
        }

    }
}