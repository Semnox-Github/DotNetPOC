/********************************************************************************************
 * Project Name - GenericCalendar Business Layer                                                                         
 * Description  - BL of the GenericCalendar class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.40        06-Oct-2018    Jagan Mohana Rao          Created new clas to fetch and update Generic Calendar.
 *2.70.2        29-Jul-2019    Deeksha                   Modified save method,Added log() methods.
 *2.110.0      08-Dec-2020    Prajwal S          Added: Get for List and Added consructor with Id, Entity Id and entity name.
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    public class GenericCalendar
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private GenericCalendarHandler genericCalendarHandler;
        private ExecutionContext executionContext;
        private GenericCalendarDTO genericCalendarDTO;
        private string activeFlag;
        
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="executioncontext">executioncontext</param>
        public GenericCalendar(ExecutionContext executioncontext)
        {
            log.LogMethodEntry(executioncontext);
            executionContext = executioncontext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="executioncontext">executioncontext</param>
        /// <param name="genericCalendarDto">genericCalendarDto</param>
        public GenericCalendar(ExecutionContext executioncontext, GenericCalendarDTO genericCalendarDto)
        {
            log.LogMethodEntry(executioncontext, genericCalendarDto);
            executionContext = executioncontext;
            genericCalendarDTO = genericCalendarDto;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public GenericCalendar(ExecutionContext executionContext, string entity, int entitId, int id, SqlTransaction sqlTransaction = null) //added
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            GenericCalendarHandler genericCalendarHandler = new GenericCalendarHandler(sqlTransaction);
            this.genericCalendarDTO = genericCalendarHandler.GetGenericCalendar(entity, entitId, id, "1");
            if (genericCalendarDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "GenericCalendarDTO", id);    //added condition
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// GetGenericCalendar method.
        /// </summary>
        /// <param name="genericId">genericId</param>
        /// <param name="moduleName">moduleName</param>
        /// <param name="isActive">isActive</param>
        /// <returns>genericCalendarDTOs</returns>
        public List<GenericCalendarDTO> GetGenericCalendar(string moduleName, int genericId,string isActive = "")
        {
            log.LogMethodEntry(moduleName, genericId, isActive);
            genericCalendarHandler = new GenericCalendarHandler(executionContext, genericCalendarDTO);
            List<GenericCalendarDTO> genericCalendarDTOs = new List<GenericCalendarDTO>();
            genericCalendarDTOs= genericCalendarHandler.GetGenericCalendarList(moduleName, genericId, isActive);
            log.LogMethodExit(genericCalendarDTOs);
            return genericCalendarDTOs;
        }

        /// <summary>
        /// SaveCalendarThemes() for the save and update of Generic Calendar.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>genericCalendarDTO</returns>
        public int SaveCalendarThemes(SqlTransaction sqlTransaction=null)
        {
            int result = 0;
            try
            {
                log.LogMethodEntry(sqlTransaction);
                GenericCalendarHandler genericCalendarhandler = new GenericCalendarHandler(executionContext, genericCalendarDTO, sqlTransaction);

                if (genericCalendarDTO.IsActive)
                {
                    genericCalendarDTO.site_id = executionContext.GetSiteId();
                    if ((genericCalendarDTO.CalendarId >= 0) && genericCalendarDTO.IsChanged == true)
                    {
                        genericCalendarDTO = genericCalendarhandler.UpdateGenericCalendar(genericCalendarDTO);
                        result = genericCalendarDTO.CalendarId;
                        genericCalendarDTO.AcceptChanges();
                    }
                    else if (genericCalendarDTO.CalendarId < 0)
                    {
                        genericCalendarDTO.IsActive = true;
                        genericCalendarDTO = genericCalendarhandler.InsertGenericCalendar(genericCalendarDTO);
                        result = genericCalendarDTO.CalendarId;
                        genericCalendarDTO.AcceptChanges();
                    }
                }
                else
                {
                    if(genericCalendarDTO.CalendarId >= 0)
                    {
                        genericCalendarhandler.Delete(genericCalendarDTO.CalendarId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// get GenericCalendarDTO Object
        /// </summary>
        public GenericCalendarDTO GetGenericDTO
        {
            get { return genericCalendarDTO; }
        }

        /// <summary>
        /// set GenericCalendarDTO Object
        /// </summary>
        public GenericCalendarDTO SetGenericDTO
        {
            set { genericCalendarDTO = value; }
        }
    }
    public class GenericCalendarList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<GenericCalendarDTO> genericCalenderList;
        private ExecutionContext executionContext;

        /// <summary>
        /// GenericCalendarList method
        /// </summary>
        /// <param name="executioncontext">executioncontext</param>
        public GenericCalendarList(ExecutionContext executioncontext)
        {
            log.LogMethodEntry(executioncontext);
            executionContext = executioncontext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GenericCalendarList method.
        /// </summary>
        /// <param name="genericCalendarDto">genericCalendarDto</param>
        /// <param name="executioncontext">executioncontext</param>
        public GenericCalendarList(List<GenericCalendarDTO> genericcalendarList, ExecutionContext executioncontext)
        {
            log.LogMethodEntry(genericcalendarList, executioncontext);
            executionContext = executioncontext;
            genericCalenderList = genericcalendarList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the GenericCalendar list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>genericCalendar List</returns>
        public List<GenericCalendarDTO> GetGenericCalendarDTOList(List<KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string>> searchParameters, string genericColIdName = "", string moduleName = "", int entityId = 0, SqlTransaction sqlTransaction = null) //added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            GenericCalendarHandler genericCalendarDataHandler = new GenericCalendarHandler(sqlTransaction);
            List<GenericCalendarDTO> genericCalendarDTOs = new List<GenericCalendarDTO>();
            genericCalendarDTOs = genericCalendarDataHandler.GetGenereicCalendar(searchParameters, genericColIdName, moduleName, entityId, sqlTransaction);
            log.LogMethodExit(genericCalendarDTOs);
            return genericCalendarDTOs;
        }

        /// <summary>
        /// SaveUpdateGenericCalendarValues method.
        /// </summary>
        /// <returns></returns>
        public int SaveUpdateGenericCalendarValues() 
        {
            int sucesscount = 0;
            try
            {
                log.LogMethodEntry();
                foreach (GenericCalendarDTO genericDto in genericCalenderList)
                {
                    /// The incoming list will have ThemeId as negative and positive IDs
                    /// -1, -2 represents fo rrecords which are added newly. 
                    /// The Positive ThemeId will be for the records which are edited and IsChanged=true
                    GenericCalendar genericCalendar = new GenericCalendar(executionContext, genericDto);
                    sucesscount = genericCalendar.SaveCalendarThemes();
                    log.Debug(string.Format("Generic calendar {0} has been stored from SaveUpdateReaderThemesList() ", genericDto.EntityId));
                }
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ;
            }
            return sucesscount;
        }
    }
}