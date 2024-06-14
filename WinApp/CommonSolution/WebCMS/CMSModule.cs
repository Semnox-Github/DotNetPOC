/********************************************************************************************
 * Project Name - CMSModule  Program
 * Description  - Data object of the CMSModule 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        11-Oct-2016   Rakshith           Created 
 *2.70       09-Jul-2019    Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.WebCMS
{
    public class CMSModule
    {
        private CMSModulesDTO cmsModulesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSModule()
        {
            log.LogMethodEntry();
            this.cmsModulesDTO = new CMSModulesDTO();
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        public CMSModule(int moduleId, SqlTransaction sqlTransaction = null) : this()
        {
            log.LogMethodEntry(moduleId, sqlTransaction);
            CMSModulesDatahandler cmsModulesDatahandler = new CMSModulesDatahandler(sqlTransaction);
            this.cmsModulesDTO = cmsModulesDatahandler.GetModule(moduleId);
            log.LogMethodExit();
        }

        //Constructor Initializes with Corresponding Object
        public CMSModule(CMSModulesDTO cmsModulesDTO)
            : this()
        {
            log.LogMethodEntry(cmsModulesDTO);
            this.cmsModulesDTO = cmsModulesDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public int Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            CMSModulesDatahandler cmsModulesDatahandler = new CMSModulesDatahandler(sqlTransaction);

            try
            {
                if (cmsModulesDTO.ModuleId < 0)
                {
                    cmsModulesDTO = cmsModulesDatahandler.InsertModule(cmsModulesDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cmsModulesDTO.AcceptChanges();
                    return cmsModulesDTO.ModuleId;
                }
                else
                {
                    if (cmsModulesDTO.IsChanged)
                    {
                        cmsModulesDTO = cmsModulesDatahandler.UpdateModule(cmsModulesDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                        cmsModulesDTO.AcceptChanges();
                    }
                    log.LogMethodExit(0);
                    return 0;
                }
            }
            catch (Exception expn)
            {
                log.Error("Error  at Save() method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the CMSModulesDTO
        /// </summary>
        public CMSModulesDTO GetCmsModulesDTO
        {
            get { return cmsModulesDTO; }
        }

        /// <summary>
        /// Delete the record from the database based on  moduleId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int moduleId, SqlTransaction sqlTransaction = null)
        {
            CMSModulesDatahandler cmsModulesDatahandler = new CMSModulesDatahandler(sqlTransaction);
            int id = cmsModulesDatahandler.cmsModuleDelete(moduleId);
            log.LogMethodExit(id);
            return id;

        }
    }
        /// <summary>
        /// List of CMSModule
        /// </summary>
        public class CMSModuleList
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            /// <summary>
            /// Returns the GetAllCMGroups based on the search parameters
            /// </summary>
            public List<CMSModulesDTO> GetAllCMSModules(List<KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>> searchParameters,SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);
                try
                {
                    CMSModulesDatahandler cmsModulesDatahandler = new CMSModulesDatahandler(sqlTransaction);
                    List<CMSModulesDTO> cMSModulesDTOList =  cmsModulesDatahandler.GetCmsModulesList(searchParameters);
                    log.LogMethodExit(cMSModulesDTOList);
                    return cMSModulesDTOList;
                }
                catch (Exception expn)
                {
                    log.Error("Error  at  GetAllCMSModules(searchParameters) method ", expn);
                    log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                    throw;
                }
            }
        }
    }
