/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Bulk Upload Mapper ThemeDTO Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       16-Mar-2019   Muhammed Mehraj  Created   
 *2.70.2       12-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    class ThemeValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        Dictionary<int, ThemeDTO> themeIdThemeDTODictionary;
        Dictionary<string, ThemeDTO> themeNameThemeDTODictionary;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ThemeValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            themeNameThemeDTODictionary = new Dictionary<string, ThemeDTO>();
            themeIdThemeDTODictionary = new Dictionary<int, ThemeDTO>();
            string themeTypeList = "Audio,Display,Visualization";
            List<ThemeDTO> readerList = null;
            ThemeListBL themeListBL = new ThemeListBL(executionContext);
            List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchCountryParams = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
            searchCountryParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchCountryParams.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_LIST, themeTypeList.ToString()));
            readerList = themeListBL.GetThemeDTOList(searchCountryParams, true, true);
            if (readerList != null && readerList.Count > 0)
            {
                foreach (var reader in readerList)
                {
                    try
                    {
                        if (themeIdThemeDTODictionary.ContainsKey(reader.Id) == false)
                        {
                            themeIdThemeDTODictionary.Add(reader.Id, reader);
                        }
                        if (themeNameThemeDTODictionary.ContainsKey(reader.Name.ToUpper()) == false)
                        {
                            themeNameThemeDTODictionary.Add(reader.Name.ToUpper(), reader);
                        }
                    }
                    catch
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 1857);
                        log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                        throw new ValidationException(reader.Name + " " + message);
                    }
                }

            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts themename to themeid
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int themeId = -1;
            if (themeNameThemeDTODictionary != null && themeNameThemeDTODictionary.ContainsKey(stringValue.ToUpper()))
            {
                themeId = themeNameThemeDTODictionary[stringValue.ToUpper()].Id;
            }
            log.LogMethodExit(themeId);
            return themeId;
        }

        /// <summary>
        /// Converts themeid to themename
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string themeName = string.Empty;
            if (themeIdThemeDTODictionary != null && themeIdThemeDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                themeName = themeIdThemeDTODictionary[Convert.ToInt32(value)].Name;
            }
            log.LogMethodExit(themeName);
            return themeName;
        }
    }
}
