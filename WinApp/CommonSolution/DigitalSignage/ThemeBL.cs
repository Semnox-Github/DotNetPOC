/********************************************************************************************
 * Project Name - Theme BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017      Lakshminarayana     Created
 *2.40        28-Sep-2018      Jagan Mohan         Added new constructor ThemeBL, ThemeListBL and 
 *                                                 methods SaveUpdateThemeList
 *2.70.2     31-Jul-2019       Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.80.0     21-May-2020       Vikas               Modified :  REST API changes for Phase -1
 *2.100       10-Aug-2020       Mushahid Faizan     Modified : Added SaveScreenTransitions() to save child DTOList, NameSpace changes and ListClass Constructor/Save method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Business logic for Theme class.
    /// </summary>
    public class ThemeBL
    {
        private ThemeDTO themeDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ThemeBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ThemeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the theme id as the parameter
        /// Would fetch the Theme object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of Theme Object</param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false.</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false.</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ThemeBL(ExecutionContext executionContext, int id, bool loadChildRecords = false,
            bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ThemeDataHandler themeDataHandler = new ThemeDataHandler(sqlTransaction);
            themeDTO = themeDataHandler.GetThemeDTO(id);
            if (themeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Theme", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
        }
      
    /// <summary>
    /// Builds the child records for Theme object.
    /// </summary>
    /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
    /// <param name="sqlTransaction"></param>
    private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)
    {
        log.LogMethodEntry(activeChildRecords, sqlTransaction);
        ScreenTransitionsListBL screenTransitionsListBL = new ScreenTransitionsListBL(executionContext);
        List<KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>> searchByScreenTransitionsParams = new List<KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>>();
        searchByScreenTransitionsParams.Add(new KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>(ScreenTransitionsDTO.SearchByParameters.THEME_ID, themeDTO.Id.ToString()));
        searchByScreenTransitionsParams.Add(new KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>(ScreenTransitionsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
        if (activeChildRecords)
        {
            searchByScreenTransitionsParams.Add(new KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>(ScreenTransitionsDTO.SearchByParameters.IS_ACTIVE, "1"));
        }
        List<ScreenTransitionsDTO> screenTransitionsDTOList = screenTransitionsListBL.GetScreenTransitionsDTOList(searchByScreenTransitionsParams, sqlTransaction);
        themeDTO.ScreenTransitionsDTOList = new SortableBindingList<ScreenTransitionsDTO>(screenTransitionsDTOList);
        log.LogMethodExit();
    }
    /// <summary>
    /// Creates ThemeBL object using the ThemeDTO
    /// </summary>
    /// <param name="executionContext">executionContext</param>
    /// <param name="themeDTO">themeDTO</param>
    public ThemeBL(ExecutionContext executionContext, ThemeDTO themeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, themeDTO);
            this.themeDTO = themeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Theme
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ThemeDataHandler themeDataHandler = new ThemeDataHandler(sqlTransaction);
            if (themeDTO.IsChangedRecursive == false
                && themeDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }

            if (themeDTO.Id < 0)
            {
                themeDTO = themeDataHandler.InsertTheme(themeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                themeDTO.AcceptChanges();
            }
            else
            {
                if(themeDTO.IsChanged)
                {
                    themeDTO = themeDataHandler.UpdateTheme(themeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    themeDTO.AcceptChanges();
                }
            }
            SaveScreenTransitions(sqlTransaction);
            themeDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : ScreenTransitionsDTOList 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveScreenTransitions(SqlTransaction sqlTransaction)
        {
            if (themeDTO.ScreenTransitionsDTOList != null &&
                themeDTO.ScreenTransitionsDTOList.Any())
            {
                List<ScreenTransitionsDTO> updatedScreenTransitionsDTOList = new List<ScreenTransitionsDTO>();
                foreach (var screenTransitionsDTO in themeDTO.ScreenTransitionsDTOList)
                {
                    if (screenTransitionsDTO.ThemeId != themeDTO.Id)
                    {
                        screenTransitionsDTO.ThemeId = themeDTO.Id;
                    }
                    if (screenTransitionsDTO.IsChanged)
                    {
                        updatedScreenTransitionsDTOList.Add(screenTransitionsDTO);
                    }
                }
                if (updatedScreenTransitionsDTOList.Any())
                {
                    ScreenTransitionsListBL screenTransitionsListBL = new ScreenTransitionsListBL(executionContext, updatedScreenTransitionsDTOList);
                    screenTransitionsListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the ThemeBL and ScreenTransitionsList- child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(themeDTO.Name))
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext,"Theme"), MessageContainerList.GetMessage(executionContext, "Name"), MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Theme Name"))));
            }
            if (!string.IsNullOrWhiteSpace(themeDTO.Name) && themeDTO.Name.Length > 50)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Theme"), MessageContainerList.GetMessage(executionContext, "Name"), MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Theme Name"), 50)));
            }
            if (themeDTO.ScreenTransitionsDTOList != null &&
                themeDTO.ScreenTransitionsDTOList.Count > 0)
            {
                foreach (ScreenTransitionsDTO screenTransitionsDTO in themeDTO.ScreenTransitionsDTOList)
                {
                    ScreenTransitionsBL screenTransitionsBL = new ScreenTransitionsBL(executionContext, screenTransitionsDTO);
                    validationErrorList.AddRange(screenTransitionsBL.Validate());
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ThemeDTO ThemeDTO
        {
            get
            {
                return themeDTO;
            }
        }       
    }

    /// <summary>
    /// Manages the list of Theme
    /// </summary>
    public class ThemeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ThemeDTO> themeDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        ///  Default constructor of ThemeListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ThemeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// create the themes list object
        /// </summary>
        /// <param name="themesToList">themesToList</param>
        /// <param name="executionContext">executionContext</param>
        public ThemeListBL(ExecutionContext executionContext, List<ThemeDTO> themeDTOList) : this(executionContext)
        {
            log.LogMethodEntry(themeDTOList, executionContext);
            this.themeDTOList = themeDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Theme list
        /// </summary>
        public List<ThemeDTO> GetThemeDTOList(List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ThemeDataHandler themeDataHandler = new ThemeDataHandler(sqlTransaction);
            List<ThemeDTO> themeDTOList = themeDataHandler.GetThemeDTOList(searchParameters);
            if (themeDTOList != null && themeDTOList.Any() && loadChildRecords)
            {
                Build(themeDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(themeDTOList);
            return themeDTOList;
        }

        /// <summary>
        /// Builds the List of theme class object based on the list of Theme id.
        /// </summary>
        /// <param name="themeDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<ThemeDTO> themeDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(themeDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ThemeDTO> themeIdReaderThemeDictionary = new Dictionary<int, ThemeDTO>();
            StringBuilder sb = new StringBuilder(string.Empty);
            string themeIdSet;
            for (int i = 0; i < themeDTOList.Count; i++)
            {
                if (themeDTOList[i].Id == -1 ||
                    themeIdReaderThemeDictionary.ContainsKey(themeDTOList[i].Id))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(themeDTOList[i].Id);
                themeIdReaderThemeDictionary.Add(themeDTOList[i].Id, themeDTOList[i]);
            }

            themeIdSet = sb.ToString();
            ScreenTransitionsListBL screenTransitionsListBL = new ScreenTransitionsListBL(executionContext);
            List<KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>> searchByScreenTransitionsParams = new List<KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>>();
            searchByScreenTransitionsParams.Add(new KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>(ScreenTransitionsDTO.SearchByParameters.THEME_ID_LIST, themeIdSet.ToString()));
            searchByScreenTransitionsParams.Add(new KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>(ScreenTransitionsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchByScreenTransitionsParams.Add(new KeyValuePair<ScreenTransitionsDTO.SearchByParameters, string>(ScreenTransitionsDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            List<ScreenTransitionsDTO> screenTransitionsDTOList = screenTransitionsListBL.GetScreenTransitionsDTOList(searchByScreenTransitionsParams, sqlTransaction);
            if (screenTransitionsDTOList != null && screenTransitionsDTOList.Any())
            {
                log.LogVariableState("ScreenTransitionsDTOList", screenTransitionsDTOList);
                foreach (var screenTransitionsDTO in screenTransitionsDTOList)
                {
                    if (themeIdReaderThemeDictionary.ContainsKey(screenTransitionsDTO.ThemeId))
                    {
                        if (themeIdReaderThemeDictionary[screenTransitionsDTO.ThemeId].ScreenTransitionsDTOList == null)
                        {
                            themeIdReaderThemeDictionary[screenTransitionsDTO.ThemeId].ScreenTransitionsDTOList = new SortableBindingList<ScreenTransitionsDTO>();
                        }
                        themeIdReaderThemeDictionary[screenTransitionsDTO.ThemeId].ScreenTransitionsDTOList.Add(screenTransitionsDTO);
                    }
                }
            }
        }

        /// Saves the ThemeBL List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (themeDTOList == null ||
                themeDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < themeDTOList.Count; i++)
            {
                var themeDTO = themeDTOList[i];
                if (themeDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    ThemeBL themeBL = new ThemeBL(executionContext, themeDTO);
                    themeBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving ThemeDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ThemeDTO", themeDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        public DateTime? GetThemeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            ThemeDataHandler themeDataHandler = new ThemeDataHandler();
            DateTime? result = themeDataHandler.GetThemeModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
